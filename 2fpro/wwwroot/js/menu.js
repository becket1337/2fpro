
$(function () {
    /*fmenu*/

    var fcont = $('.foreign-cont');
    $('.forg-btn').hover(function (e) {


        fcont.css({ marginTop: '20px', opacity: 1, visibility: '' });

    }, function () { $('.foreign-cont').css({ marginTop: '50px', opacity: 0, visibility: 'hidden' }); });

    $(document).on('mouseover', '.foreign-cont,.forg-tnl', function () {
        $('.foreign-cont').css({ marginTop: '20px', opacity: 1, visibility: '' });
    });
    $(document).on('mouseout', '.foreign-cont,.forg-tnl', function () {
        fcont.css({ marginTop: '50px', opacity: 0, visibility: 'hidden' });
    });
    /* tmenu */
    var tlink = $('.tmenu li a');
    tlink.hover(function () {

        $(this).addClass('no-pseudo');
    }, function () { $(this).removeClass('no-pseudo'); });

    //  Mobile NAVI
    var menuLeft = $('#cbp-spmenu-s1'),
        showLeft = $('#showLeft'),
        body = $('body');
    body.on('click', function (e) {

        if (menuLeft.hasClass('cbp-spmenu-open') && !$(e.target).closest('.cbp-spmenu').length) { menuLeft.toggleClass('cbp-spmenu-open'); showLeft.toggleClass('active'); }
    });
    showLeft.on('click', function () {

        $(this).toggleClass('active');
        setTimeout(function () { menuLeft.toggleClass('cbp-spmenu-open'); }, '100');
        //disableOther('showLeft');
    });

    function disableOther(button) {
        if (button !== 'showLeft') {
            classie.toggle(showLeft, 'disabled');
        }

    }


});

