




/*=============================  Image Preview  ============================================== */





//var initPartialViews = function () {

//    $.get('/Config/GetPartials', function (html) {
//        $('body').prepend(html);
//    });
//}

var expandPartialBlock = function () {

    var fcont = $('.foreign-cont');
    $('.forg-btn').hover(function (e) {

        $('.foreign-cont').css({ marginTop: '20px', opacity: 1, visibility: '' });
    });
    $(document).on('mouseover', '.foreign-cont,.forg-tnl', function () {

        $('.foreign-cont').css({ marginTop: '20px', opacity: 1, visibility: '' });
    });
    $(document).on('mouseout', '.foreign-cont,.forg-tnl', function () {
        fcont.css({ marginTop: '50px', opacity: 0, visibility: 'hidden' });
    });
    $(document).on('mouseout', 'body', function () {

        if (fcont.css('marginTop') == '20px') fcont.css({ marginTop: '50px', opacity: 0, visibility: 'hidden' });
    });
}

var footNav = function () {
    $mainNav = $('.xn-pc').length ? $('.toplvl-nav') : $('.n-bmenu .toplvl-nav');
    $serviceNav = $('.toplvl-nav-cat');
    $footWrap = $('.f-menu');
    $mobnavWrap = $('.mob-nav-target');
    $cat = $('#catsList');


    $serviceNav.clone().appendTo($mobnavWrap);
    $mainNav.clone().appendTo($footWrap);
    // $serviceNav.clone().appendTo($footWrap);
    $cat.clone().appendTo($footWrap);
    
    $('<li>Разделы</li>').insertBefore($('.f-menu ul.toplvl-nav li:eq(0)'));
    $('<li>Продукция</li>').insertBefore($('.f-menu ul#catsList li:eq(0)'));
    $('.f-menu ul').attr('class', 'foot-nav');
    $('.foot-nav ul').remove();
    $('.f-menu ul').removeAttr('id');
    $('.f-menu ul i').remove();
    $('.foot-nav li span').html(function (i, h) {
        return h.replace(/&nbsp;/g, '');
    });
  

}


var cartIsHidden = false;
var loader = $('<img style="width:25px" class="submit-loader" src="/Content/ajax-loaders/horizont/puff.svg" >');
var loader1 = $('<img class="submit-loader" src="/Content/ajax-loaders/horizont/main-loader1.GIF" >');
var pcartsubmit = $('.loader-state');
var cartSummary = $('#cart__summary');
var cartIndex = $('.cart-cont').length;
var partialTimeout = null;

var addToCart = function () {

    $this = $(this);
    //var cloth = $('input[name="Cloth"]').val();
    //var color = $('input[name="Color"]').val();
    //var molding = $('input[name="Molding"]:checked').val();
    //var screed = $('input[name="Screed"]').is(":checked") ? 1 : 0;
    //var ty = $(this).data('type');

    
    if ($this.data('catIndex') && $this.attr('data-isadded')=='False') {//из каталога кнопка купить
        $this.attr('data-isadded','True').find('span').html('В корзине');        
    }
    else if ($this.data('pageIndex') && !$this.hasClass('prod-view-btn-added')) {//из карточки товара кнопка купить
        $this.removeClass('prod-toadd').addClass('prod-view-btn-added').find('span').html('Товар в козрине');
    }
    else {
        toTheCart();
        return false;
    }
    
    addLoader();
    XN.Inscreaser.Close();
    var id = $this.data('pid');
    //var lastPrice = $('.item-features__price-value').data('price');
   
    $.post('/Cart/AddToCart', { prodId: id,quan:$('*[data-qid="'+id+'"]').data('prod-numb'),  t:$(this).data('type') }, function (data) {

    updateSummary(data.count);
    //if(!XN.IsMobile)ExpandPartialCart(id, data.title, data.price, data.count, data.total);
    //initCartEvents();
        //updateCartEvent(id,true);
    setTimeout(function () { closeLoader(); }, '500');
        //partialOff();
});
}
var cartCheckout = function (step, isAuth) {
    $.ajax({
        beforeSend: function () { addLoader(); },
        url: '/Checkout/Processing',
        type: 'post',
        success: function (data) {
            //    if (data.type == 0){
            //        XN.Auth.BuildModal('#modal-7', '/Account/Mauth');
            //        closeLoader();
            //    }
            //   else       
            //        location.replace('/Checkout/Index?step=2');

            //
            
            location.replace('/Checkout/Index?step=2');
        }
    })
}
var initCartEvents = function () {
    var fcont = $('.foreign-cont');
    var hasItems = $('.forg-menu .item-row').length;
    $('.forg-btn').click(function (e) {
        if (XN.IsMobile) {
            toTheCart();
        }
        else {
            if (!hasItems) return false;
            $('.foreign-cont').css({ marginTop: '20px', opacity: 1, visibility: '' });
        }
    });
    //$('.forg-btn').click(function (e) { $('.foreign-cont').css({ marginTop: '50px', opacity: 0, visibility: 'hidden' }); });

    $(document).on('mouseover', '.foreign-cont,.forg-tnl', function () {
        if (hasItems)
            $('.foreign-cont').css({ marginTop: '20px', opacity: 1, visibility: '' });
    });
    $(document).on('mouseout', '.foreign-cont,.forg-tnl', function () {
        if (!cartIsHidden)
            fcont.css({ marginTop: '50px', opacity: 0, visibility: 'hidden' });
    });
    // $(document).on('click', '*[data-event-type="cart__remove"]', removeFromCart);
}
var authAttach = function () {
    XN.BuildModal('#modal-7', '/Account/Mauth');
};

var toTheCart = function () {

    document.location.replace('/Checkout/Index?step=1');
}
var removeFromCart = function () {// УДАЛИТЬ ТОВАР ИЗ КОРЗИНЫ

    addLoader();
    var pid = $(this).closest('.item-row').data('pid');

    var row = $(this).closest('.item-row');
    $.post("/Cart/RemoveFromCart", { prodId: pid }, function (data) {
        $('.forg-menu tr[data-pid="' + pid + '"]').remove();
        updateSummary(data.count);
        updateTotal(data.total, data.count);
        //updateCartEvent(pid, false);
        //$('.cart__view[data-pid="' + pid + '"]').addClass('cart__add').removeClass('cart__view').html('Купить');
        setTimeout(function () { closeLoader(data.total, data.count); row.remove(); }, '200');
    });
}
function partialOff() {
    clearTimeout(partialTimeout);
    partialTimeout = setTimeout(function () { $('.foreign-cont').css({ marginTop: '50px', opacity: 0, visibility: 'hidden' }); }, '4500');
};
function partialOffNow() {
    $('.foreign-cont').css({ marginTop: '50px', opacity: 0, visibility: 'hidden' });
}
function updateSummary(val) { // ИТОГО СТОИМОСТЬ ТОВАРОВ

    $('.top-cart-total').hide();
    $('.top-cart-total i').css('display', '');
    //var res = isPlus ? cartSummary.text() + val : cartSummary.text() - val;
    $('#cart__summary').html(val);
    $('.cartbtn-topblock').on('click', function () {
        location.replace('http://_2fpro.ru/Checkout?step=1');
    });

}
function updateTotal(total, quant) { // ОБНОВЛЕНИЕ КОЛ-ВА И СТОИМОСТИ ТОВАРА
    if ($('.cart-cont').length) {

    }
    else if (quant > 0) {
        $('.partial-cart__summary').html(quant);
        $('.partial-cart__total').html(PriceFormatted(total) + ' ₽');
    }
    else {
        //$('.top-cart-total i').hide();

        $('.partial-cart__total').html('');
        $('.partial-cart__summary').html('корзина пуста');
        partialOffNow();
    }
}

function ExpandPartialCart(id, title, p, c, t) {// МИНИ КОЗРИНА В ШАПКЕ
    var subTitle = title.length > 25 ? title.substring(0, 25) + '...' : title;
    var src = $('#mpic_' + id).attr('src');
   
    var img = $('<img src="' + src + '" />');
    $('.forg-menu').append(
        '<tr class="item-row" data-pid="' + id + '">' +
        '<td><img class="forg-img" src="' + src + '" /></td>' +
        '<td><span>' + subTitle + '</span><span class="top-cart-remove" data-event-type="cart__remove">удалить</span></td>' +
        '<td>' + PriceFormatted(p) + ' ₽</td>' +
        '</tr>'
        );

    updateTotal(t, c);
    $('.foreign-cont').css({ marginTop: '20px', opacity: 1, visibility: '' });
    //$(document).on('click', '.forg-menu tr[data-pid="'+id+'"] span[data-event-type="cartRemove"]', removeFromCart);
    //closeLoader();
}
function updateCartEvent(id, a) {

    if (a) {
        $('.prod_id-' + id).html('товар добавлен');
        $('.prod_id-' + id).removeClass('cart__add').addClass('cart__view');
    } else {
        $('.prod_id-' + id).html('Купить');
        $('.prod_id-' + id).removeClass('cart__view').addClass('cart__add');
    }
}
function summaryCartInit() {
    $.get('/Cart/CartSummary', function (data) {
        $('#cart__summary').html(data==0?"":data);
        //initCartEvents();
    });
}
function partialCartInit() {
    if (!$('#ch-content').length) {
        $.get('/Cart/CartPartial', function (data) {

            $('.pbasket').append(data);
            initCartEvents();
        });
    }
    summaryCartInit();
}
function addLoader() {

    if ($('.cart-cont').length) {
        $('.cart-sum-td').html(loader1);
    } else {
        //alert(submit.attr('class'));
        //$('#cart__partial .loader-state').text('');
        $('.top-cart-checkout .loader-state').html(loader);
        //alert(pcartsubmit.text());
       
    }
}
function PriceFormatted(price) {
    return price.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1 ");
}

function closeLoader(s, t) {

    if ($('.cart-cont').length) {
        //<td class="cart-sum-td" colspan="2">Общая цена: <span class="pbasket-total">@Model.Cart.TotalValue().ToString("0 000")</span> <i class="fa fa-rouble"></i></td>

        $('.submit-loader').remove();
        //$('.pbasket-total').html(data.s);
        // $('.item-total').html(data.t);

        $('.cart-sum-td').html('Общая цена: <span class="pbasket-total">' + PriceFormatted(s) + ' ₽</span>');
    }
    else {
        $('.top-cart-total').fadeIn();
        $('.submit-loader').remove();
        $('#cart__partial .loader-state').text('Оформить заказ');
    }
}
var cartItemDecrease = function () {
    $this = $(this);
    if ($(this).hasClass('item-disabled')) { return false; }
    $(this).addClass('item-disabled');
    $('.item-inc').removeClass('item-disabled');
    addLoader();
    row = $(this).closest('.item-row');
    id = $(this).closest('.item-row').data('pid');
    quant = $(this).find('.cart-quanity-val').text();
    $.post('/Cart/UpdateItem', { pid: id, type: 0 }, function (data) {
        row.find('.cart-quantity-val').html(data.quant);
        row.find('.item-total').html(PriceFormatted(data.quant * data.price) + ' р₽');
        setTimeout(function () { closeLoader(data.s, data.t); $this.removeClass('item-disabled'); }, '200');
        if (data.t == 1) $this.addClass('item-disabled');
    });
}
var cartItemIncrease = function () {
    $this = $(this);
    if ($(this).hasClass('item-disabled')) { return false; }
    $(this).addClass('item-disabled');
    $('.item-dec').removeClass('item-disabled');
    addLoader();
    row = $(this).closest('.item-row');
    id = $(this).closest('.item-row').data('pid');
    quant = $(this).find('.cart-quanity-val').text();

    $.post('/Cart/UpdateItem', { pid: id, type: 1 }, function (data) {
        row.find('.cart-quantity-val').html(data.quant);
        row.find('.item-total').html(PriceFormatted(data.quant * data.price) + ' ₽');
        setTimeout(function () { closeLoader(data.s, data.t); $this.removeClass('item-disabled'); }, '200');

        if (data.t == 19) $this.addClass('item-disabled');
    });
}




/*=========================   Main   =============================*/

var mainConfiguration = function () {
    var width = $(window).width();
    var height = $(window).height();
    var input = $('<input type="hidden" id="screenWidth"  value="' + width + '">');
    var input1 = $('<input type="hidden" id="screenHeight"  value="' + height + '">');
    input.appendTo('body');
    input1.appendTo('body');

    var swidth = $('input[name="screenWidth"]').val();
    var mod = $('#modal-6');
    var mod1 = $('#modal-7');
    if (swidth < 768) {
        //mod.addClass('disabled-state');
        //mod1.addClass('disabled-state');

    }
    //$('img').filter(function () { return !$(this).hasClass("img-responsive")&&!$(this).closest('#listview').length; }).addClass('img-responsive');
}

function headerInit() {
    window.addEventListener('scroll', function (e) {
        var distanceY = window.pageYOffset || document.documentElement.scrollTop,
            shrinkOn = 300,
            header = document.querySelector("header");
        if (distanceY > shrinkOn) {
            classie.add(header, "smaller");
        } else {
            if (classie.has(header, "smaller")) {
                classie.remove(header, "smaller");
            }
        }
    });
}

var popupLivechat = function () {
    if ($('#livechat').length) {
        //alert('s');
        $('#livechat-wrap').show();

    }
    else {

        var wrapper = $("<div id='livechat-wrap'><img style='position:absolute;top:150px;left:140px' src='/Content/ajax-loaders/3011.gif' /></div>");
        $('body').append(wrapper);
        setTimeout(function () {
            $.get('/Consultant/GetChatRoom', function (html) {
                var $form = $("#livechat-feed");

                $form.removeData("validator");
                $form.removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse($form);
                $('#livechat-wrap').css({ height: "auto" }).html(html);
            });

        }, 1000);
    }
}



var initSections = function () {
   
    $.getJSON("/StaticSection/GetSections", {}, function (data) {
        for (var i = 0; i < data.sections.length; i++) {
            $('#section_' + data.sections[i].ID).html(data.sections[i].Content);
        }
        //XN.Auth.OpenEvents();
    });
   
}

var initDiagWindow = function () {
   // setTimeout(function () {
        $.get("/Diagnostic/GetDiagWindow", function (data) {
            $('body').prepend(data);

        });
   
    //}, 500);
}


var zoomStyle = 'overflow: hidden; background-position: 0px 0px; text-align: center; background-color: rgb(255, 255, 255); width: 400px; height: 400px; float: left; background-size: 640px 480px; display: none; z-index: 100; border: 4px solid rgb(136, 136, 136); background-repeat: no-repeat; position: absolute; background-image:';


/*=============================  POPUP  ============================================== */




/*== JCarousel ==*/


var disabled = function () { return false; };



var checkboxUpdate = function () {
    var total = $('.item-features__total');
    var price = $('.item-features__price-value').data('price');

    //var priceInt = PriceFormatted(priceModified)
    if (this.checked) {
        total.html('Итого: ' + PriceFormatted(price + 500) + ' руб');
        $('.item-features__text-screed').html('+ двухсторонняя стяжка');
        $('.item-features__price-value').data('price', price + 500);
    }
    else {
        total.html('Итого: ' + PriceFormatted(price - 500) + ' руб');
        $('.item-features__text-screed').empty();
        $('.item-features__price-value').data('price', price - 500);


    }
}
var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};
var removeURLParameter = function(url, parameter) {
    //prefer to use l.search if you have a location/link object
    var urlparts = url.split('?');
    if (urlparts.length >= 2) {

        var prefix = encodeURIComponent(parameter) + '=';
        var pars = urlparts[1].split(/[&;]/g);

        //reverse iteration as may be destructive
        for (var i = pars.length; i-- > 0;) {
            //idiom for string.startsWith
            if (pars[i].lastIndexOf(prefix, 0) !== -1) {
                pars.splice(i, 1);
            }
        }

        return urlparts[0] + (pars.length > 0 ? '?' + pars.join('&') : '');
    }
    return url;
}
var addParams = function (url, data) {
    if (!$.isEmptyObject(data)) {
        url += (url.indexOf('?') >= 0 ? '&' : '?') + $.param(data);
    }

    return url;
}
var toggleMenu = function () {
    var subm = $(this).children('ul').first();
    if (subm.length) {

        //subm.toggleClass('hide-state');
        subm.toggle();
    }
};
// акордион в каталоге
var togleCatBar = function () {
    $(this).find('i').toggleClass('fa-minus');
    $(this).find('i').toggleClass('fa-plus');
    $(this).closest('.lv-block').find('.lv-aside-list').slideToggle('fast');
}
var togleCatBarMainPage = function () {
    $(this).closest('.tl-det-list').slideToggle('fast');
    
}
/*=============================  Init Scripts  ============================================== */

//window.onload = headerInit();
$(function () {
    $('*[data-quan="1"]').on('click', function () {
        var id = $(this).data('pid');
        var targ = $('*[data-qid="' + id + '"]');

        var quan = parseInt(targ.val());
        quan >= 1 ? quan-- : "";
        targ.val(quan);
        targ.data("prod-numb", quan);
    });
    $('*[data-quan="2"]').on('click', function () {
        var id = $(this).data('pid');
        var targ = $('*[data-qid="' + id + '"]');
        var quan = parseInt(targ.val());
        quan <= 10001 ? quan++ : "";
        targ.val(quan);
        targ.data("prod-numb", quan);
    });
    $('.cart-quantity-val').on('change', function () {
        $(this).data('prod-numb', $(this).val());


    });


    //footNav();
    mainConfiguration();
    //zoomedImg();
    //initSections();
    //initDiagWindow();
    //partialCartInit();   //Мини корзина в ШАПКЕ
    //summaryCartInit(); // Кол во покупок в ШАПКЕ

    //$(document).on('mouseover', '.tmenu-item', function () { if ($(this).find('i').length) $('.main-overlay').show(); });
    //$(document).on('mouseout', '.tmenu-item', function () { if ($(this).find('i').length) $('.main-overlay').hide(); });
    $(document).on('click', '.tmenu li', toggleMenu);
    $(document).on('click', '.up-float-button', function () { window.scrollTo(0, 0); });
    $(document).on('click', '.navbar-brand, .pmenu-logo img', function () { document.location.href = "/"; });
    $(document).on('mouseover', '.xn-listview-item', function () { $(this).find('.caption-more-details').show(); });
    $(document).on('mouseout', '.xn-listview-item', function () { $(this).find('.caption-more-details').hide(); });
    $(document).on('click', '*[data-event-type="cart__decrease"]', cartItemDecrease);
    $(document).on('click', '*[data-event-type="cart__increase"]', cartItemIncrease);
    $(document).on('click', '*[data-event-type="cart__remove"]', removeFromCart);
    $(document).on('click', '*[data-event-type="cart__add"]', addToCart);
    //$(document).on('click', '*[data-event-type="cart__view"]', toTheCart);
    $(document).on('click', '*[data-event-type="cart__checkout"]', cartCheckout);
    //$(document).on('click', '.checkout-auth', authAttach);

  
    //$(document).on('click', '.culture-info .lg-i', dropdownList2);


    $(document).on('click', '.disabled-state', disabled);
    $(document).on('click', '.title-bar', togleCatBar);
    $(document).on('click', '.tl-det-btn', togleCatBarMainPage);
    $(document).on('change', '#Screed', checkboxUpdate);


    var shrinkHeader =$('#tag-list').length? $('#tag-list').offset().top:"";
    var shrinkHeader1 = $('#services').length ? $('#services').offset().top : "";
    if ($('body').hasClass('page-layout')) {
        $('.lbmenu').addClass('lb-light');
    }
    $(window).scroll(function () {
    
            var scroll = $(window).scrollTop();
        var shrink = $('#tag-list');
        //console.log("catalog " + shrinkHeader1 + " serv " + shrinkHeader + " scroll " + scroll);
        if ($('body').hasClass('page-layout')) {
          
        } else {
            if (scroll + 100 > shrinkHeader1) {
                $('.lbmenu').removeClass('lb-light');
            }
            else if (scroll + 100 > shrinkHeader) {
                $('.lbmenu').addClass('lb-light');

            }
            else {
                $('.lbmenu').removeClass('lb-light');

            }
        }

    });
    function getCurrentScroll() {
        return window.pageYOffset || document.documentElement.scrollTop;
    }

    $(document).on("click", ".md-close", function (e) {
        XN.Inscreaser.Close();
        XN.Auth.CloseModal();
    });
    /* $(document).on("click", ".md-overlay", function (e) {
         if ($('.md-modal').hasClass('md-show')) $('.md-modal').removeClass('md-show');
     });*/
    $(document).on("click", "body", function (e) {
        if (!$(e.target).closest(".md-content,.caption-more-details,.md-mainarrs,.xn__popup").length) {

            XN.Inscreaser.Close();
            XN.Auth.CloseModal();

        }
        if ($('.dropdown-list').is(':visible')) {

            $(".dropdown-list").hide();

        }


    });
    $(document).on("click", ".dropdown-list", function (e) {
        if ($('.xn-selectlist__target').is(':visible')) {
            $('.xn-selectlist__target').hide();

        }
    });
    if ($('#ch-content').length) { $('.forg-btn').addClass('disabled-state'); $('.forg-btn').unbind('click'); return false; }
    if (XN.IsMobile) $('#cart__summary').css('right', 'initial');


    //$('.cat-list li').hover(function () {
    //    $this = $(this);
    //    $this.find("i,a").css({ color: "#004CA3" });
    //    $this.find("a").css({ textDecoration: "none" });
    //}, function () {
    //    $this = $(this);
    //    $this.find("i,a").css({ color: "#4e89bf" });
    //    $this.find("a").css({ textDecoration: "none" });
    //});
    $('.prod-image').hover(function () {
        // $(this).find('.prod-details').show();
    }, function () { $(this).find('.prod-details').hide(); })


    $('#catsList li').removeClass('selected-cat');
    $('#catsList li').filter(function () {
        return $(this).data('catId') == getUrlParameter('catId');
    }).addClass('selected-cat');


    $('#bmenu li').removeClass('active');
    $('#bmenu li a').removeClass('current');
    $('#bmenu li').filter(function () {
        return $(this).find('a').attr('href') == window.location.pathname;
    }).find('a').addClass('current');

    $('.tmenu li').removeClass('active');
    $('.tmenu li a').removeClass('current');
    $('.tmenu li').filter(function () {
        return $(this).find('a').attr('href') == window.location.pathname;
    }).addClass('active').find('a').addClass('current');
    /* Pager
    ---------------------------------------------------------------------------------*/


});

