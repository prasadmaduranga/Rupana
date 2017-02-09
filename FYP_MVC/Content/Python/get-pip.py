#!/usr/bin/env python
#
# Hi There!
# You may be wondering what this giant blob of binary data here is, you might
# even be worried that we're up to something nefarious (good for you for being
# paranoid!). This is a base85 encoding of a zip file, this zip file contains
# an entire copy of pip.
#
# Pip is a thing that installs packages, pip itself is a package that someone
# might want to install, especially if they're looking to run this get-pip.py
# script. Pip has a lot of code to deal with the security of installing
# packages, various edge cases on various platforms, and other such sort of
# "tribal knowledge" that has been encoded in its code base. Because of this
# we basically include an entire copy of pip inside this blob. We do this
# because the alternatives are attempt to implement a "minipip" that probably
# doesn't do things correctly and has weird edge cases, or compress pip itself
# down into a single file.
#
# If you're wondering how this is created, it is using an invoke task located
# in tasks/generate.py called "installer". It can be invoked by using
# ``invoke generate.installer``.

import os.path
import pkgutil
import shutil
import sys
import struct
import tempfile

# Useful for very coarse version differentiation.
PY2 = sys.version_info[0] == 2
PY3 = sys.version_info[0] == 3

if PY3:
    iterbytes = iter
else:
    def iterbytes(buf):
        return (ord(byte) for byte in buf)

try:
    from base64 import b85decode
except ImportError:
    _b85alphabet = (b"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                    b"abcdefghijklmnopqrstuvwxyz!#$%&()*+-;<=>?@^_`{|}~")

    def b85decode(b):
        _b85dec = [None] * 256
        for i, c in enumerate(iterbytes(_b85alphabet)):
            _b85dec[c] = i

        padding = (-len(b)) % 5
        b = b + b'~' * padding
        out = []
        packI = struct.Struct('!I').pack
        for i in range(0, len(b), 5):
            chunk = b[i:i + 5]
            acc = 0
            try:
                for c in iterbytes(chunk):
                    acc = acc * 85 + _b85dec[c]
            except TypeError:
                for j, c in enumerate(iterbytes(chunk)):
                    if _b85dec[c] is None:
                        raise ValueError(
                            'bad base85 character at position %d' % (i + j)
                        )
                raise
            try:
                out.append(packI(acc))
            except struct.error:
                raise ValueError('base85 overflow in hunk starting at byte %d'
                                 % i)

        result = b''.join(out)
        if padding:
            result = result[:-padding]
        return result


def bootstrap(tmpdir=None):
    # Import pip so we can use it to install pip and maybe setuptools too
    import pip
    from pip.commands.install import InstallCommand
    from pip.req import InstallRequirement

    # Wrapper to provide default certificate with the lowest priority
    class CertInstallCommand(InstallCommand):
        def parse_args(self, args):
            # If cert isn't specified in config or environment, we provide our
            # own certificate through defaults.
            # This allows user to specify custom cert anywhere one likes:
            # config, environment variable or argv.
            if not self.parser.get_default_values().cert:
                self.parser.defaults["cert"] = cert_path  # calculated below
            return super(CertInstallCommand, self).parse_args(args)

    pip.commands_dict["install"] = CertInstallCommand

    implicit_pip = True
    implicit_setuptools = True
    implicit_wheel = True

    # Check if the user has requested us not to install setuptools
    if "--no-setuptools" in sys.argv or os.environ.get("PIP_NO_SETUPTOOLS"):
        args = [x for x in sys.argv[1:] if x != "--no-setuptools"]
        implicit_setuptools = False
    else:
        args = sys.argv[1:]

    # Check if the user has requested us not to install wheel
    if "--no-wheel" in args or os.environ.get("PIP_NO_WHEEL"):
        args = [x for x in args if x != "--no-wheel"]
        implicit_wheel = False

    # We only want to implicitly install setuptools and wheel if they don't
    # already exist on the target platform.
    if implicit_setuptools:
        try:
            import setuptools  # noqa
            implicit_setuptools = False
        except ImportError:
            pass
    if implicit_wheel:
        try:
            import wheel  # noqa
            implicit_wheel = False
        except ImportError:
            pass

    # We want to support people passing things like 'pip<8' to get-pip.py which
    # will let them install a specific version. However because of the dreaded
    # DoubleRequirement error if any of the args look like they might be a
    # specific for one of our packages, then we'll turn off the implicit
    # install of them.
    for arg in args:
        try:
            req = InstallRequirement.from_line(arg)
        except:
            continue

        if implicit_pip and req.name == "pip":
            implicit_pip = False
        elif implicit_setuptools and req.name == "setuptools":
            implicit_setuptools = False
        elif implicit_wheel and req.name == "wheel":
            implicit_wheel = False

    # Add any implicit installations to the end of our args
    if implicit_pip:
        args += ["pip"]
    if implicit_setuptools:
        args += ["setuptools"]
    if implicit_wheel:
        args += ["wheel"]

    delete_tmpdir = False
    try:
        # Create a temporary directory to act as a working directory if we were
        # not given one.
        if tmpdir is None:
            tmpdir = tempfile.mkdtemp()
            delete_tmpdir = True

        # We need to extract the SSL certificates from requests so that they
        # can be passed to --cert
        cert_path = os.path.join(tmpdir, "cacert.pem")
        with open(cert_path, "wb") as cert:
            cert.write(pkgutil.get_data("pip._vendor.requests", "cacert.pem"))

        # Execute the included pip and use it to install the latest pip and
        # setuptools from PyPI
        sys.exit(pip.main(["install", "--upgrade"] + args))
    finally:
        # Remove our temporary directory
        if delete_tmpdir and tmpdir:
            shutil.rmtree(tmpdir, ignore_errors=True)


def main():
    tmpdir = None
    try:
        # Create a temporary working directory
        tmpdir = tempfile.mkdtemp()

        # Unpack the zipfile into the temporary directory
        pip_zip = os.path.join(tmpdir, "pip.zip")
        with open(pip_zip, "wb") as fp:
            fp.write(b85decode(DATA.replace(b"\n", b"")))

        # Add the zipfile to sys.path so that we can import it
        sys.path.insert(0, pip_zip)

        # Run the bootstrap
        bootstrap(tmpdir=tmpdir)
    finally:
        # Clean up our temporary working directory
        if tmpdir:
            shutil.rmtree(tmpdir, ignore_errors=True)


DATA = b"""
P)h>@6aWAK2mm&gW=RK-r8gW8002}h000jF003}la4%n9X>MtBUtcb8d8JxybK5o&{;pqv$n}tHC^l~
I+Mavrq?0()%%qLSNv=2JXgHJzNr)+u1xVRS+y8#M3xEJc+D&`xG$ILLcd^)g_Juxq^hK-W7fVro!OK
0X56!kJCu>>lSemZerj<NRnb_5pY*@BbRnay))z6cOd0$kktl;ixvk~RSK31x`tD8ELs+)M5$r2{2j*
dEXb0wclPS}@E&c2>K`FeKt4O?bX9-iiWDY7!D<mQ~UvM9vzD|VKg{exwB&U54-sxm8>YHK31t|U{{>
PE#tZP_+VteGhH)eTGzMZy!aHJ(Q?6Cjc(3MQ0lIm@hktf`o4axNvVCTc)Ts4@VJ>@!hh%K`|2$iKE+
HHx+6sw#7#MJW!3g|Y$%N)ur)tC3;}#CBEQ7CdI~xY=+?Ot(T=34r+9E$`%6N}j>;=NFf=Z&^bu!zEv
3t>Ua&1Gxq!8;Ps7soN%ES($^#>_e*>Ru`El;Z0c`kR05XmE3{WT9s{ZCofrE;qGp;vO#hcs@DjeDR$
tn@v;IglI6VSWzNghfplGqI!0<h0I1-4T3r;??TjQs&6OmeCw>gHwP<*5k}E|s-0tgq2{VgAu^rc%*@
7HRg@?*fSPs9ypVK;PZfg`L*{@Wh4H}=)J&0S$#2!{sXR907wMxwCB>Zm0$&8dG^t{{SFIu9BwcKPai
iS)37*53oHqWOqTV)O3RPrz%ERGmE0Tun4O(ssPA=8(oYCvxpzPymKk}-Q$?RIdE=IK(@bmxe)jVQYH
8{VWs)8KiU3x%fE5{sAyYgujXSqq0M!Jcq(%y4NcRLa4k(bC--&}_#|G%=iwT(weU1)OKQ+;gdjz%u)
oWwP6Kw|to?PIw?Km1kAC7Ms_kh)WuY*}FOiLCVc@zRudBQ9tsceu3uNfZ`pomDWvf`>KU^QgE|jC3f
JeGPP6hUu>U2ZL8-0vz>6l;DW>Cpc;OqR~k!*C(#5^E>jBZX2(m7SL-6X;oqX)fNgKHx<25fw`lciam
T?0Sq;<*0efo>>~_mb!wtQ8FET)G{S3%GLx;TuGy_0I*8^0_3ZXQ@aNJf0K2y8VP7S+U1FD)Lc1YZU5
_=?sZ~~HXI3ze#a`M|s-Y_t#noGnybn*-wS~M*gQeu&v6y8yuxLY<q9;0n@W-JMJ0uYy508zYY>!d!A
F!&;`Rs^bRcsWT^vka6lXVZTrPm;4Ks2igbSlrx(sRT^p6}=17w9Ix8?jmITqsTRyjGrANWtnsTD|j$
Y4h<paYnHW51=d#=yy0PVPR28xPL1c&PPKBFnT5A#G$`o~VciTH#|qsF6%jQG1Q0R6Lp!tY%}ORT@1j
I!XUhX%b1PT4WrSG(Rb=IHS6cvPrdCqa6o@jljoC-FWoXJmZGoWK1^u3|=M-D)E-|LIC@LU2z9(*Q$V
eykHz^><5(QWgT)w<ae|Y!yb^7e}PnWMQ-d+S`hPZ!~Kq4b#Rch_wCBaf;NslWq(;Q9B&ASeeNczj`t
LJZmMWX6LG+}gocD`^cV1X!`aIokZt_l`fwT(PDo^ZwzJ$i0fUTZotcBaW{r~vEA`5oc-*wP@-hv6UA
oLz&9(4oUGLM@^kd0Y?l!bmf6-gUh&C*a5p<#uD_4Y=%<nB5`=qdqtSdi3O4TtE5KjSXr43^t{=Xbcw
A1=$Uxm}tzYei=psx$Um3K^#$bEMXCVC14(SpyKB&0BfxSpAu#gWz{1%PL$2(X1OCzyE=eX+=0!UMfb
7=$Tev)VWSDl6k8Q(w=K<E=AX<1f^-W4a%r@FV>b!BhII2*G}|zk1yNsG$GkHLdlf5Gzaat{Tc>$@p`
a+TwY7Wli;y;&R%LORzm+XNlE7>Vmn1j*;EP+Vbf#*@tWz5o0+$?;>TN2)pj76eCD51u1o>jxO7Rd+T
^~TVJZ5V=f+fUt3~2+X?Q3%F77oSob@jkBylRQqf|H}cxNlq|euM|+Co9)Srn2x(&;r3@IQS4AF!H7F
o8r-xn-D4>d|PI6qlSXmJ;9W|=O@}p6HPuvOHX05<L9&{7U)Fm(Yz}NlQ-`!FRw1%yh(q&cy+m$cy6Q
vDwZ*zCcYO{tH6WExz?hq_>>OET{SlFW?YMVB^bOj7$3}o2vCc*b=Na9ht-RL`cQj!G22J9&fIo^m$3
29+HJ>nF|s8yA0n&;d{IKJHp=j(V~BT0>~4G)GPEMc(VQAuvRl`;M6?1>952}1Ot5I~#MYk0Kxxi3Ah
q0z)s{+ML0+{{$39}{osGDzV+%G3gnJXTS9DXfMe;)R!F^lZ>b%Fq4=Wdfk4}wCzJh`hBBYO~_dq4)E
TcmM7`3(}e7h%A3)V?v$2PKRYqb~<uxK^(plFO)SZM~0IY%8iDngjXg9p6)jNviKdF<^@SR^&-uAaig
r#r1axPS%8hf0*;^__DtUn=yIQNxY&=6lFT$?;fbaPB1!>CG)@>9<ahfchB$1pW8rDVDqJ--i45?AjR
0B8c7mEYDNiW~v8a<%<jq&YQ8el_!inSeXKvx>bn8D8{C!mRaF*M5$oJ*5h{7A4fUSurLlk|Ge9D<V{
W>j35F+Yz8R+C*ftDqF;u_LZHS<>zfUP3#rrKI%~GDOrn(Geb3oa;V;xkn21A-6!o~;5)IrK3&>Lg$n
YELmLl9n0XsGIFkW7P7W+cQbn<5G`uwYfk^6+2P*{9yc*!NCRzAqXJB#nGfJ}B!NvFOOhTfndqX%NMl
ise-9(t=Sm&iY#gz#t1Fx4SUsz^%mm(E_;O<CP4yAy56Hgr>VNHrzq3|yB|Hrue#yvyr>(@~yJ^SpJ4
OF^(;kKyNZ_T@JUlux=BG5cWr9_}dO9aCTQY^g^RyvVq;_ugniS6F79aaVdkZH1IkocB$7G|e~a`MGK
!XEswYXIAV1vyQFCC0F2wx<nPqsyd^NE;Vs6>gx`n?t@Ug!osc^vn#KqIIKVLe-1_p#*HV3aVasg00W
>1$}nd<H?N4%IUL7q)`%U4Y-aw?AZCG0;o){R!zvi>UjF>@ZB-R2u;rQ+EVZ$_M+hh_d^Rb?NSN{|#E
&S)jskXLv=z{g*0oLz4Y%3MIH@hdj)++w_Ub=yY}Mo-b#g03!^1v$ME6ew7%D``6|eh~C_r=)A@uzIJ
N=ON&A!*ch(O)=3CM}bncF8OaorQ9gI$?Nhg|T|4M#Y5=A{BwMaNvm<)f~Zvmpdn?c=+yAoeAhSb@87
TMqdtzY}KDV&{B5+UyK14KGjFsSQCzTOv4hWZCpoO%X5b5|_B(AtRH1E(COJCKK$k!;-T@)v_JO?!To
)%RJsP6QFy)qYW9u%;pS0F><gE4vd;3XT?+ji-Eo>J1x>2t;K8GzcH^9$#>PBA1lHjmwg*|^KH_x<*S
=isHy<C%6%xa?|>hr3Ego`XEQrC#p5F9cRF;-Fk<wiuw#Zdf+KO9W1qybT^ra^)ID*8&EC=M;C4?9ET
cl5KePa5RV)4We)kQ|w3`){A<Y(o-Db;lt5lir(yd7hu%u>fs^?iV@3$}~Bb~8<sx8*IVBvR??1v8Q|
H7*QoNy@(N=z@Vu3lfAL%5rQ$-&$KqPV#aBFdQyMV#Y@MU0vHD<|gBpo%q6;`mvo$}yWJ1Bh%J+^oeq
z<ydu?9>GHlja<r_)s^7hvJRC3(bpH&(a@Wy#o9Wda5y_PCdQa$P$4VSYr8>VSWu|(Mo1&i*{s&u@{2
vB>ipRBhNi?@MIwmShkyR`VyPj6zz!LsnP`&@S*HQQ=7(&M}FoqXi;>q5?XVgA32$|3zK777d8C`@``
Q>eL*?-`xmZezko^TratE%IrW;s|5rr@c=|$CA9;DDD_s0Y6IRO)eAR$E8qZkc2N%#@nudxO>zHZlhN
2jBVZNHhBtEQG^Dy!P2rftr_IL518vqjU9{%mWwnSm9`zqI)V0jtc<E<7p#fF6BL=<P$u+vZmGa0_mA
4i`V>q>J>%|@n$Up{%CyZ>kbt$0eh+HnBqyweJn0Mr=_SB26a5@YX!F%-Jxjf(olZ*omrcHoC;?4S<n
5Nhz*1(9=MZ|7cjbL^8P+?wx#a+OMVyne7d{`RSxbd(q1XJuTCy+UrfZDA)+KR|ltMUd~0_1xcH`rJo
^3$+qEK7BT}^M3T@cmSM7?rm^99E{^N)g;K%L00qk5Fi@!#3FqB&$Bm(pbf{mFJ{wma@b&{KVmRF*0$
`lql+a3kh|4j@vtMQl|)|<{MT@7I5G&2e`V9bv#Kq0Q$2?;CU+1ifNEVS(Nyx_EEQ@EsIA<Ae1h24LT
$=4F2KnNd-RBXq8Py^Ym3|_Q$3R!&h_k7XExnHul@Gm)W5<L+qp^uT|)Q0Q2-W>e^vyEI1TC~oScxJA
ydY*9ir{^bUp|3fq&=IMa<q0HWpmm!3s>i&S)*AlSmHC7Z!cjNpdQ`)7^W##r$<hOADi6t-l@D4C&-M
TO7}T%C}i<5uhPCFt7}9Ka;C%IH-s4%5}NyEix$m;41J2#|+yG9hISHsC{YS3|JfiTo}M`Ftio?JmuD
nf8f9g9=Ln+!-#m;!EtAx-6QVZKYA2Y#%GQSkIv=GH@<^U0S&wY^F99@b1o#k7HFpX(m}?WQm26OgYn
NSpM(&^4N&66%m4m#0qi=Y=s3Q+dWBALtQ$5&i;kZDO9FsS^Or5><8wz4V*m_)V>32#VR*ohWuXOIH=
sMUKJ;SFsX7PGyqBJzH9agmUcR4<Z$#7Fqi5KOiS7!Xjg!1zCyrF`+o}2k@x}S&pAdZ@m2jjHc7s#(^
i-Yj&1P=efA`Ab+yDJe1`^*th=2sFbQ(1NDHAXE)+Y6Z(z#qME6l3X2XkkeZGxFJVs(^m_Srklo9vpn
baR{_7E&FMLZVwAAiquC=br^Sn|IU2nvEEVm%(43>tm!(8=?0d&g_`7e6Mm)jWmUWC$m06TLbvadj&v
W2y^Z;Zu-6cO2gdsaIxnc_KJlF8^$0_h`_YKC!7uSl|V7|pGHx0EY)4x)chSpS2a^%2D$kE08mQ<1QY
-O00;m!mS#yyNu*^n0RR9<0ssIH0001RX>c!JUu|J&ZeL$6aCu#kPfx=z48`yL6qa^qhepR4X$Ov65%
(yx$r_O+A$C>v?Xk0z4RXq#_nz%vY>qQ1WfxkqQ3~9gVkXcZ82v&<UC&KZ?;~zIykOJp;MKxvKxYGa3
BiRkSV`2dPR95H=y3#^%=HKq#n&fI6MNq$hoHTWD;CXy`fMOwXo>-nOOFrzI{72-zy%~$-fkObx$UHf
Pxf%%rxUd8a|66~GLQ3R8vL7cRBF~PDAlJ+)moR4V01a?*}x!0kg`h%(L#G~Xb*s9h+(`5M8UCb&3ZG
qcoGOQp;VW#N-&4rFgQZvZ8g0VLYnU307k(&=&*eVS1J1Pdg6a5y1w?^{XcI6_WR=6a(m`zGIdXf614
yQS7FS(g!rYKD_V)ETsH=luY{RzM;)7bdFi;y4^T@31QY-O00;m!mS#zU7>o5o4FCX!E&u=$0001RX>
c!MVRL0;Z*6U1Ze%WSdCeMYkJ~o#`~C_-MPNDSC{2p%hXssYvX9niy1Up%+rwf(&=PH{ktLOsyz49S-
*1KwiJ~NLdg$W}Br8!f!{NM#WDo@JndIc8*lt;#kT_#f&ImpVp0SF<-=eP4oXa2xj#i@B5=vKfRSQlj
Nw;MoD#Dhs$m)ty{eE<0#<OC*PV=>WEu?*t`{uDItC9)H?fWAWIpD}6Jz1HSc9wXX0B~C5viTIHdBUG
8z!i%>vNb=)LD9lwMa&eMg%fp-Q_vdW=q?pi%`%?vT9l-C%(H?e4dt}F;Zg#T7KT5?yzI~o-?PLBaz+
-ptXP(*na_kM#Ejg*tp4B;Iq);Y4EmMeyR@j~`#Q~%(^RP8X)C8FF197BNLTnYN#p9I$XDsQg<OKpmD
GiW))1F!L09Sv@LMLpX}&(?D^_Qf{Elbkc_Fr}s$BUB{;Q>87Jbcsty96bJg;U%%|k^y<fspzt6I{yN
O&tnC6b%FlasTXn;AK~zP`K$UM{}BxcupYn%5r}*SB}?KAc_rNG~pL>G|c|#i^F%)%Dqri_5zk`u=Y5
;gp^(t_{x7w4E0$I%_6OcqzCxkr`R@ik6~S&q$6d&C>sH3PRm@xRH@=yYK{71_J}~(Fov0iSj3d0bl5
j3$!U3Z+QIi=;(-25FWVIoZL^0?k5j0j+23^=2oW>aQQ)vg_P!O3$6%uaHO2q8ckR%f8lX8JyuddAi%
#Ua<1NM36A0pY|;c)03+utlX?gyqp}j5Z6%C{0e`BFU%v*|1+^uxoM1+}V_b*;_(0r*uOLpOd0J5#N}
jD|B!w7(0+_2A3}5)uhDbj?!Ysda{9&TloE#IR5UH20!%R?B@O|<^k{5D