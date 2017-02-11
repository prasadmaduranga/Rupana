
$(".read-more").click(function (e) {
    e.preventDefault();
    $(this).closest('.post-content').find('.detail-less').css("visibility", "hidden");
    $(this).closest('.post-content').find('.detail-more').css("visibility", "visible");
    $(this).css("visibility", "hidden");
    $(this).closest('.post-content').find(".read-less").css("visibility", "visible");

});

$(".read-less").click(function (e) {
    e.preventDefault();
    $(this).closest('.post-content').find('.detail-less').css("visibility", "visible");
    $(this).closest('.post-content').find('.detail-more').css("visibility", "hidden");
    $(this).css("visibility", "hidden");
    $(this).closest('.post-content').find(".read-more").css("visibility", "visible");

})
