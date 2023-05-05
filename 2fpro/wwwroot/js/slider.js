var onHover = false, startSlide = false, holder = $('.slider-img-holder'), timer;
var isTablet = false;
var onInit = true;

var getImages = function () {
    $.getJSON('/Product/All_Sl_Images', {}, function (data) {
        var t = 0;
        for (var i = 0; i < data.pids.length; i++) {
            if (t < 4) $('.slider-img-holder').append('<span class="slide-illu sl-active" data-pid="' + data.pids[i].pid + '" data-title="' + data.pids[i].title + '" data-sl-counter="' + i + '"  />');
            else $('.slider-img-holder').append('<span class="slide-illu" data-pid="' + data.pids[i].pid + '" data-title="' + data.pids[i].title + '" data-sl-counter="' + i + '"  />');
            t++;
        }

    });
}

$(document).ready(function () {

    //getImages();

    if ($('#screenWidth').val() < 992) isTablet = true;


    $('.sl-arrs i:first-child').on('click', function () {
        onHover = false;
        animSlides(false, isTablet);
        onHover = true;
    });
    $('.sl-arrs i:last-child').on('click', function () {
        onHover = false;
        animSlides(true, isTablet);
        onHover = true;
    });

    $('.sl-arrs').hover(function () {
        onHover = true;

    }, function () { onHover = false; });
    $('.s-slider').hover(function () {
        onHover = true;

    }, function () { onHover = false; });

    function StartSliding() {
        timer = setInterval(function () { animSlides(true, isTablet, onInit); }, 4000);
    }
    /*$('.sl-arrs').hover(function () {

        clearInterval(timer);
    }, function () {
        setInterval(function () { animSlides(true, isTablet); }, 4000);
    });*/
    animSlides(true, isTablet, onInit);


    onInit = false;
    StartSliding();

});


function animSlides(direction, isTablet) {

    var slidesVisible = isTablet ? 1 : 1;
    var translate = 980;
    var wrapper = $('.slider-wrap');
    // $('.sl-slide').css({ opacity: 1, display: 'none' });



    var settings = {
        list: $('.sl-slide').length - 1,
        current: $('.sl-active'),
        elem: 0
    }

    if (!onHover) {
        //if (settings.onInit) {
        //    $('.sl-slide').slice(0, slidesVisible).addClass('sl-active');
        //    settings.current = $('.sl-active');
        //}
        ShowSlides(direction);





        function ShowSlides(direction) {
            if (direction) {
                settings.elem = settings.current.last().index();
                var difference = settings.list - settings.elem;
            } else {
                settings.elem = settings.current.first().index();
                var difference = settings.elem;
            }
            Activate(settings.elem, direction);



            function Activate(index, dir) {

                var currSlide;
                var slideTitle;
                $('.sl-slide').removeClass('sl-active');

                if (dir) {
                    //wrapper.css({ left: settings.current.last().outerWidth()+"px" });
                    if (settings.elem == settings.list) {

                        currSlide = $('.sl-slide').slice(0, slidesVisible);
                        currSlide.addClass('sl-active');

                    }
                    else if (difference < slidesVisible) {

                        currSlide = $('.sl-slide').slice(-slidesVisible)
                        currSlide.addClass('sl-active');
                    }
                    else {
                        currSlide = $('.sl-slide').slice(settings.elem + 1, settings.elem + 1 + slidesVisible);
                        currSlide.addClass('sl-active');
                    }

                } else {
                    // wrapper.css({ left: settings.current.last().outerWidth() + "px" });
                    if (settings.elem == 0) {

                        currSlide = $('.sl-slide').slice(-slidesVisible);
                        currSlide.addClass('sl-active');
                    }
                    else if (difference < slidesVisible) {
                        currSlide = $('.sl-slide').slice(0, slidesVisible);
                        currSlide.addClass('sl-active');
                    }
                    else {
                        currSlide = $('.sl-slide').slice(settings.elem - slidesVisible, settings.elem);
                        currSlide.addClass('sl-active');
                    }
                }
                slideTitle = currSlide.data('title');
                slidePrice = PriceFormatted(currSlide.data('price')) + ' руб';
                slideLink = currSlide.data('link');
                $('.slider-title').text(slideTitle);
                $('.slider-price').text(slidePrice);
                $('.slider-leftside a').attr('href', currSlide.data('link'));

                settings.current = $('.sl-active');
                var idx = settings.current.first().index();
                var outer = 190;
                $('.slider-counter').html(settings.current.last().index() + 1 + " из " + (settings.list + 1));
                $('.sl-arrs').show();
                //onInit ? wrapper.css({ left: -outer * idx+ "px" }) : wrapper.css({ left: (-outer * idx+1) + "px" });
                //wrapper.css({ left: (-outer * idx) + "px" }); 
                //console.log(idx + ", ");
                //$('.sl-circle[data-slide-index="' + settings.elem + '"]').addClass('sl-active');


                //settings.current = $('.sl-active');



            }
            //$('#slide_' + index).css({ background: "url(/ImageData/GetProdImage?pid=" + $(this).data('pid') + "&width=400&height=300) no-repeat 50%", opacity: "0.5" }).animate({ opacity: "1" }, 1000);
            //$('#slide_' + index).attr('title', $(this).data('title'));
            //$('#slide_' + index).closest('a').attr('href', '/Product/Details/' + $(this).data('pid'));
        }

    }
}
function PriceFormatted(price) {
    return price.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1 ");
}
