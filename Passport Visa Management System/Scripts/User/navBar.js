$(function () {
    $(".navigation-tab-item").click(function () {
        $(".navigation-tab-item").removeClass("active");
        $(this).addClass("active");
        $(".navigation-tab-overlay").css({
            left: $(this).prevAll().length * 195 + "px"
        });
    });
});
$('#nav_container').mouseenter(function () {
    window.addEventListener('wheel', scrollevent);
});
$('#nav_container').mouseleave(function () {
    window.removeEventListener('wheel', scrollevent);
});
function scrollevent(e) {
    var navContainer = $('#nav_container')[0];
    if (e.deltaY > 0) navContainer.scrollLeft += 100;
    else navContainer.scrollLeft -= 100;
}