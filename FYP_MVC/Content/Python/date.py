import cgitb
cgitb.enable()
import sys
from datetime import*
path=[]
path = sys.argv[1].split(',')
from dateutil import parser
dateString =""


def intTryParse(value):
    try:
        temp = int(value)
        return True 
    except ValueError:
        return False

def parseDay(value):
    try:
        dateString = parser.parse(path[i])
        return dateString;
    except:
        return "error"


result = [None] * len(path)
for i in range(len(path)):
    if(intTryParse(path[i])):
        temp = int(path[i])
        if(temp>1800 and temp < 2100):
            result[i] = parseDay(temp)
        else:
            result[i] = "error"
    else:
        result[i] = parseDay(path[i])

for res in result:
    print(res)






