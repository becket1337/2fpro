/**
 * X-NOVA Widget Factory

 */
// Encoding: utf-8


var XN = XN || {}; // initialize global namespace
var mod, modc;

var ins_loader = '<img class="ov-spin" src="/Content/ajax-loaders/horizont/tail-spin-gal.svg" />';
var ins_loader_black = '<img class="ov-spin" src="/Content/ajax-loaders/horizont/tail-spin.svg" />';
var istablet = $('#screenWidth').val() < 768 ? "tablet" : "desktop";


$(function () {
    //history back state
    window.onpopstate = function (event) {
        //console.log(event.state.models);
        //updateInputs(JSON.stringify(event.state).models, '');
      if ($('#listview-target').length) { XN.AjaxRequest.MakeRequest('/Product/ProdListPartial', { jsonData: event.state }, '#listview-target'); }
    };


    XN.Auth.startModal(0, null, null);
    // lazy loading - main-slider, prodlist
    var listview = $('#listview');
    var data = { type: istablet };
    var pageParam = listview.data('page');
    var catidParam = listview.data('catid');//$('.content-wrap').data('catid') != undefined ? 0 : $('#listview').data('catid');

    // Catalog: parts models ajax load / url params
    var modelParam, partsParam = [];
   
    if (getUrlParameter("models") !== undefined) {
       
        var getArray = getUrlParameter("models");
        modelParam = getArray.toString().split(',').map(function (item) { return parseInt(item, 10); });

        updateInputs(modelParam, '');
       

    } else modelParam = [];
    if (getUrlParameter("parts") !== undefined) {

        var getArray = getUrlParameter("parts");        
        partsParam = getArray.toString().split(',').map(function (item) { return parseInt(item, 10); });

        updateInputs('', partsParam);

    } else partsParam = [];

    console.log(modelParam + "   ,   " + partsParam);
    var data1 = { models: modelParam, parts: partsParam,page:1 };//XN.getUrlParameter('page') ==null ? 1 : XN.getUrlParameter('page') };
    var isFullCat;
  
    //if ($('.s-slider').length) XN.AjaxRequest.MakeRequest('/Product/ProdsToSlider', { jsonData: JSON.stringify(data) }, '.s-slider');
    if ($('.s-slider').length) XN.AjaxRequest.MakeRequest('/Slider/GetSlider', { jsonData: JSON.stringify(data) }, '.s-slider');
    if ($('#listview-target').length) {
        
        XN.AjaxRequest.MakeRequest('/Product/ProdListPartial', { jsonData: JSON.stringify(data1) }, '#listview-target');

        //history ajax start page 1

        var dataJson = data1;
        var str = $.param({ catid: getUrlParameter("catid"), page: getUrlParameter("page") });

        //history.pushState(JSON.stringify(dataJson), 'prod+' + 1, '/Product/ProdList?' + str);

    }
    if ($('#lastprods-target').length) { XN.AjaxRequest.MakeRequest('/Product/LastProds', {}, '#lastprods-target'); } // каталог 
    if ($('#catlist-target').length) { XN.AjaxRequest.MakeRequest('/Category/CatTags', {}, '#catlist-target'); } // категории на главную
    if ($('#slidermain-target').length) { XN.AjaxRequest.MakeRequest('/Category/CatSlider', {}, '#slidermain-target'); } // слайдер на главной
    // content image resize
    //$('.content-wrap img').on('click', function () {
    //    var arr = [];
    //    var index;
    //    var curr = $(this);
    //    var data = {};
    //    $('.content-wrap img').each(function (el) {
    //        if ($.grep(arr, function (n) { n.src != $(this).attr('src') })) arr.push({ link: $(this).attr('src'), title: $(this).attr('alt') });
    //    });

    //    index = $.map(arr, function (obj, index) {
    //        if (obj.link == curr.attr('src')) return index;
    //    })
    //    data =
    //        {
    //            type: 4,
    //            el: index[0],
    //            pack: arr
    //        };
    //    //alert('index - ' + index + ', length - ' + arr.length + ' curr src - ' + curr.attr('src'));
     
    //    XN.Inscreaser.BuildModal('/Widget/Inscreaser', data);
    //});
    // folder image gallery
    $('*[data-ins-type="3"]').on('click', function () {
        XN.Inscreaser.BuildModal('/Widget/Inscreaser', { type: 3, folder: $(this).data('folder') });
    });
    // photogallery
    $('#photogallery-target .xn-listview-item').on('click', function () {

        XN.Inscreaser.BuildModal('/Widget/Inscreaser', { type: 2, id: $(this).data('id') });
    });

    // update checkbox inputs
    function updateInputs(modArr, partsArr) {
        if (modArr != '') {

            
            $('.lv-modblock').hide();
            $(modArr).each(function (s) {
                $('input[data-aside-catid="' + this + '"]').prop('checked', true);
             
                    $('.lv-modblock[data-modrowid="' + this + '"]').show();
                
                
               
            });
        }
        if (partsArr != '') {
            $(partsArr).each(function (s) {

                $('input[data-aside-catid="' + this + '"]').prop('checked', true);
                //$('.lv-modblock[data-modrowid="' + this + '"]').show();

            });
        }
    }

    // url params load
    function reloadCatalog(type) {

        var modelsArr= [];
        var partsArr = [];

        //filter models
        $('.form-check input[data-type="models"]').each(function () {
            var modid = $(this).data('asideCatid');
            if ($(this).is(":checked")) {
                modelsArr.push($(this).data('asideCatid'));
                $('input[data-modid="' + modid + '"]').closest('.lv-modblock').show();
            }
            else {
                if (type == 'models') {
                    if($('input[data-type="models"]:checked').length == 0){
                        $('.lv-modblock').show();
                    }else {
                        $('.form-check-input[data-modid="' + modid + '"]').prop('checked', false);
                        $('input[data-modid="' + modid + '"]').closest('.lv-modblock').hide();
                    }
                }
            }
        });
        
        //var newUrl = encodeURIComponent(JSON.stringify(modelsArr));

        
        // filter Parts
        $('.form-check input[data-type="parts"]').each(function () {

            if ($(this).is(":checked")) partsArr.push($(this).data('asideCatid'));
        });

        var refresh = window.location.protocol + "//" + window.location.host + window.location.pathname;

      
        if (partsArr.length > 0) {
            var url = new URL(refresh);
            var search_params = new URLSearchParams(url.search);
            search_params.set('parts', partsArr.join(','));
            url.search = search_params.toString();

            refresh = url.toString();
           
        
        }
     
        if (modelsArr.length > 0) {

            // add "topic" parameter
            var url = new URL(refresh);
            var search_params = url.searchParams;
            search_params.set('models', modelsArr.join(','));
            url.search = search_params.toString();

            refresh = url.toString();
        }

        var filteredData = JSON.stringify({ models: modelsArr, parts: partsArr,cattype:type });
        window.history.pushState(JSON.stringify({ models: modelsArr, parts: partsArr,cattype:type }), 'prodlist', refresh);

        // Result DATA

        console.log(filteredData);
        XN.AjaxRequest.MakeRequest('/Product/ProdListPartial',{ jsonData: filteredData}, '#listview-target');
    }
  
    $('.form-check input').on('change', function () {
        var type = $(this).data('type');
        reloadCatalog(type);
    });
    // prodlist preview

    $('.xn__popup').on('click', function () {

        XN.Inscreaser.BuildModal('/Widget/Inscreaser', { type: 1, id: $(this).closest('.xn-listview-item').data('id') });
    });

    $()

    $('.xn__popup').on('click', function () {

        XN.Inscreaser.BuildModal('/Widget/Inscreaser', { type: 1, id: $(this).closest('.xn-listview-item').data('id') });
    });
});
/*==========================================================================================*/
/*=====================================  Configuration ===============================*/
/*==========================================================================================*/
var isMobile = false; //initiate as false
// device detection
if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
    || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
    isMobile = true;
}
XN.IsMobile = isMobile; // -- проверка на мобильное устройство
XN.WindowWidth = $(window).width();
XN.WindowHeight = $(window).height();
XN.AbsoluteUrl = document.URL;
XN.JavaEnabled = navigator.javaEnabled();
XN.Lang = navigator.language;
XN.IsTablet = $(window).width() < 768 ? true : false;
XN.getUrlParameter = function (sParam) {
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


/*==========================================================================================*/
/*=====================================  Modal Auth/Register widget ===============================*/
/*==========================================================================================*/

XN.Auth = { // Widget - Modal Account Auth/Reg

    startModal: function (code, processType, username) {

        mod = $('#modal-7');
        modc = $('#modal-7').find('.md-content');
        if (processType == 1) {
            // Подверждение о успешной регистрации
            XN.BuildModal('#modal-7', '/Account/Mconf?username=' + username);
            return false;
        }
        else if (processType == 2) {
            // Сброс пароля
            XN.BuildModal('#modal-7', '/Account/Mset?userid=' + username + '&code=' + code);
            return false;
        }
        else { }

        XN.Auth.OpenEvents();

        // открытия мод окна и загрузка контента
        //   

        $('.feed-close').hover(function () {
            $(this).toggleClass("entypo-cancel");
            $(this).toggleClass("entypo-cancel-circled");
        }, function () {
            $(this).toggleClass("entypo-cancel");
            $(this).toggleClass("entypo-cancel-circled");
        });
    },
    CloseModal: function () {
        $('.md-modal').removeClass('md-show');
        //$('.md-modal').hide();
    },
    RemoveModal: function () {

        mod.removeClass('md-show');
        $('.md-auth .md-loader').show();
    },
    CloseEvent: function () {
        $('.md-close').on('click', function () {

            mod.removeClass('md-show');
        });
    },
    OpenEvents: function (form) {
        if (form == 1) {
            $('.md-content *[data-content-type="loginForm"]').on('click', function () {
                XN.Auth.BuildModal('#modal-7', '/Account/Mauth');
            });
            $('.md-content *[data-content-type="registerForm"]').on('click', function () {
                XN.Auth.BuildModal('#modal-7', '/Account/Mreg');
            });
            $('.md-content *[data-content-type="recoveryForm"]').on('click', function () {
                XN.Auth.BuildModal('#modal-7', '/Account/Mrec');
            });
        }
        else {
            $('*[data-content-type="feedForm"]').on('click', function () {

                XN.Auth.BuildModal('#modal-7', '/Feedback/GetIndex');
            });
            $('*[data-content-type="orderForm"]').on('click', function () {

                var id = $(this).data('pid');
                var quan = 1;//parseInt($('*[data-qid="' + id + '"]').val());

                XN.Auth.BuildModal('#modal-7', '/Order/Makeorder?id=' + id + '&quan=' + quan);
            });
            $('*[data-content-type="loginForm"]').on('click', function () {
                XN.Auth.BuildModal('#modal-7', '/Account/Mauth');
            });
            $('*[data-content-type="registerForm"]').on('click', function () {
                XN.Auth.BuildModal('#modal-7', '/Account/Mreg');
            });
            $('*[data-content-type="recoveryForm"]').on('click', function () {
                XN.Auth.BuildModal('#modal-7', '/Account/Mrec');
            });
        }
    },
    BuildModal: function (modalId, dataToLoad) {

        var modal = $(modalId);
        var loader = $('.md-auth .md-loader');
        var modalc = modal.find('.md-content');

        modalc.empty();
        XN.Auth.RemoveModal();
        $(".md-overlay").css({ background: 'rgba(255, 255, 255, 0.6)' });
        modal.addClass('md-show');
        modal.show();

        $.get(dataToLoad, function (data) {

            modal.find('.md-content').html(data);
            var form = modal.find('form');

            $.validator.unobtrusive.parse(form);
            XN.Auth.CloseEvent();
            XN.Auth.OpenEvents(1);
            loader.hide();

        }).fail(function () { XN.Auth.RemoveModal(); loader.hide(); alert('Ошибка приложения  !'); });


    }

}
/*==========================================================================================*/
/*=====================================  Modal PhotoGallery/Preview widget ===============================*/
/*==========================================================================================*/
XN.Inscreaser = { // Widget - Catalog/Gallery
    Init: function (type, el, index) {
        var input;

        var ins_modal = $('#modal-6');
        var ins_content = $('#modal-6 .md-content');
        var ins_scont = $('.xn-i-sphoto');
        var targetImage = $('#xn-i-main');

        var screenWidth = document.getElementById('screenWidth').getAttribute('value');
        var isMobile = screenWidth < 500;
        isMobile ? $('.md-modal').addClass('md-mobile') : "";
        var isGal = false;
        var prodMode = false;
        type == 2 ? isGal = true : "";
        type == 1 ? prodMode = true : "";

        /* конечная настройка виджета */
        // $('.ov-spin').remove();
        /* ---------------------- */

        settings = {
            current: 0,
            list: $([]),
            wrapper: $([])
            //$('.xn-i-sphoto img')
        }


        if (type == 3) {

            /* Папка */
            for (var i = 0; i < el.length; i++) {
                var guiImg = $('<img alt="" src="' + (this.Config().smallPhotoUrl + this.Utilities.FormatedUrl(el[i], this.Config().isCat)) + '" />');
                //alert(guiImg.attr('alt'));
                settings.wrapper = settings.wrapper.add(guiImg);

            }
        }
        else if (type == 2) {
            /* Фотогаллерея */
            for (i = 0; i < el.length; i++) {
                var guiImg = $('<img alt="title of image" data-id="' + el[i] + '" src="/ImageData/GetPhotoGalleryImage?id=' + el[i] + '&width=100&height=100&isGallery=false" />');
                settings.wrapper = settings.wrapper.add(guiImg);

            }
        }
        else if (type == 1) {
            /* Каталог */
            ins_modal.removeClass('xn-modal');
            ins_modal.addClass('md-cat');
            ins_modal.find('.xn-i-sphoto').removeAttr('style');
            ins_modal.find('.xn-i-sphoto').addClass('style');
            //var rouble = $('<i class="fa fa-rouble"></i>');
            el.added ? $('.xn-cart-option').attr({ dataEventType: 'cart__view' }).attr('href', '/Checkout/Index?step=1').html('В корзину') : '';
            //$('.xn-h-w a').attr('href', '/Product/Details/' + el.id);
            //$('.d-title td').html(el.name);
            $('.d-title').html(el.name);
            $('.d-cat').html(el.cat);

            // СКИДКА - disc

            el.manufacturer != null ? $('.md-modal .d-manufacturer td').html(el.manufacturer) : $('.md-modal .d-manufacturer').hide();
            el.weight != null ? $('.md-modal .d-weight td').html(el.weight) : $('.md-modal .d-weight').hide();
            el.packsize != null ? $('.md-modal .d-packsize td').html(el.packsize) : $('.md-modal .d-packsize').hide();
            el.pack != null ? $('.md-modal .d-pack td').html(el.pack) : $('.md-modal .d-pack').hide();
            el.mat != null ? $('.md-modal .d-mat td').html(el.mat) : $('.md-modal .d-mat').hide();
            el.fill != null ? $('.md-modal .d-fill td').html(el.fill) : $('.md-modal .d-fill').hide();
            el.size != null ? $('.md-modal .d-size td').html(el.size) : $('.md-modal .d-size').hide();
            //el.price != null ? $('.md-modal .d-price td').html(el.price) : $('.md-modal .d-price').hide();
            el.price != null ? $('.md-modal .d-price').html(el.price) : $('.md-modal .d-price').hide();
            el.desc != null ? $('.md-modal .d-desc td').html(el.desc) : $('.md-modal .d-desc').hide();
            //$('.d-price td').html(el.price + ' <i class="fa fa-rouble"></i>');
            $('*[data-event-type="cart__add"]').attr('data-pid', el.id);
            //if(XN.IsMobile)$('.prod-link-details').attr('href', '/Product/Details/'+el.id);
            //$('.prod-link-details').attr('href', '/Product/Details/' + el.id);
            for (i = 0; i < el.si.length; i++) {

                var guiImg = this.Utilities.GetDataImg(el.si[i].id, false, 100, 100, el.id, el.si[i].src);
                //alert(sm.attr('src'));
                settings.wrapper = settings.wrapper.add(guiImg);

            }
            //$('body').append(simgs);

            $('.xn-h-w,.xn-d-w').fadeIn();

        }
        else {
            /* Контент  */
            var list = $('.content-wrap img').clone();
            var currList = ('.content-wrap img');
            var arr = [];
            settings.current = index;

            for (var i = 0; i < el.length; i++) {
                var contAlt = el[i].title;
                if (typeof contAlt == 'undefined') contAlt = '';
                var guiImg = $('<img alt="' + contAlt + '" src="' + this.Config().smallPhotoUrl + "?path=~" + el[i].link + '" />');
                settings.wrapper = settings.wrapper.add(guiImg);
            }
        }



        //setTimeout(function () { $('.xn-i-sphoto').css({ visibility: 'visible' }); }, '100');


        ins_scont.html(settings.wrapper);
        settings.list = $('.xn-i-sphoto img');
        ins_scont.append('<div class="curr-border" />');

        settings.list.each(function (n) {

            $(this).removeClass('xn-i-current');
            $(this).attr('data-i-index', n);
            $(this).attr('data-pos', $(this).offset().left);
            if (n == settings.current) {
                $('.curr-border').css({ left: (n * $(this).outerWidth()) + 7 + "px" });
                //$('.curr-border').animate({ left: (n * $(this).outerWidth()) + 7 + "px" }, 300);
                $('.i-info').html($(this).attr('alt'));
                $(this).addClass("xn-i-current").css('opacity', '1');
                $('.i-counter').html(n + 1 + ' из ' + settings.list.length);
            }


        })
            .click(function () {
                ShowPhoto($(this).data('i-index'), true);
            });

        if (isGal) this.Utilities.LoadImg($('img[data-i-index="0"]').data('id'), 'gal');
        else if (prodMode) this.Utilities.LoadImg($('img[data-i-index="0"]').data('id'), 'prod', $('img[data-i-index="0"]').attr('src').replace('/200x150', ''));
        else this.Utilities.LoadImg(settings.current);

        $(this.Config().imgEl).css('cursor', 'pointer');

        // current image

        var currPos = this.Utilities.GetCurrentImg(settings.current);
        var elPos = $(ins_scont).find('img[data-i-index="' + settings.current + '"]').data('pos');

        settings.scontLeft = ins_scont.offset().left;
        // animate to current image
        var idx = currPos.index() + 1;
        var outer = currPos.outerWidth();
        ins_scont.animate({ left: (-outer * idx) + outer + "px" }, 300);

        // show large image
        function ShowPhoto(index, isPrev) {

            isPrev == null ? isPrev = false : "";
            settings.current = index;
            // load image event

            if (isGal) XN.Inscreaser.Utilities.LoadImg($('img[data-i-index="' + settings.current + '"]').data('id'), 'gal');
            else if (prodMode) XN.Inscreaser.Utilities.LoadImg($('img[data-i-index="' + settings.current + '"]').data('id'), 'prod', $('img[data-i-index="' + settings.current + '"]').attr('src').replace('/200x150', ''));
            else XN.Inscreaser.Utilities.LoadImg(settings.current);

            // counter
            $('.i-counter').html(index + 1 + ' из ' + settings.list.length);

            // add current image and description
            settings.list.each(function (n) {

                $(this).removeClass('xn-i-current');
                $('.i-info').html(settings.list[index].alt);
                n == settings.current ? $(this).addClass("xn-i-current").css('opacity', '1') : $(this).css('opacity', '0.5');
            });

            isPrev == true ? "" : ins_scont.animate({ left: -outer * index + "px" }, 300);

            $('.curr-border').animate({ left: (index * outer) + 7 + "px" }, 300);
            $('.curr-border').data('offset', $('.curr-border').offset().left - $(window).scrollLeft());

        }

        $(this.Config().target).off();
        $(this.Config().ctlBprev).off();
        $(this.Config().ctlBnext).off();
        $('.xn-i-details__spoiler').off();
        $(this.Config().target).on('click', function (e) { ShowPhoto((settings.current + 1) % settings.list.length); });
        $(this.Config().ctlBprev).on('click', function () { settings.current == 0 ? ShowPhoto(0) : ShowPhoto((settings.current - 1) % settings.list.length); });
        $(this.Config().ctlBnext).on('click', function () { ShowPhoto((settings.current + 1) % settings.list.length); });
        $('.xn-i-details__spoiler').on('click', function () {

            $(this).siblings().slideToggle();

        });

        //XN.Inscreaser.Config({ isGal: true });
        //XN.Inscreaser.BuildModal('/Widget/Inscreaser', { type: 1, folder: '/Content/qm/design' });
        //alert(settings.list.length);
    },
    Config: function (opts) {
        return $.extend(this.Default, opts || {});
    },

    Inscrease: function (type) {

    },
    BuildModal: function (dataUrl, opts) {

        this.Open(1);

        //console.log(opts.pack.length, opts.el);
        if (opts.type == 4) {
            $('.xn-modal .md-content').load('/Widget/InscreaserView');

            setTimeout(function () { XN.Inscreaser.Init(0, opts.pack, opts.el); XN.Inscreaser.reEvent(); }, '500');
        }
        else {
            $.ajax({

                type: 'GET',
                url: dataUrl,


                data: opts,
                success: function (data) {

                    $(XN.Inscreaser.Config().content).html(data);
                    XN.Inscreaser.reEvent();


                    // <- reevent
                }
            });
        }
    },
    reEvent: function () {
        $('.md-close').on('click', function () {
            $(XN.Inscreaser.Config().content).html('');
            $(XN.Inscreaser.Config().modal).removeClass('md-show');
            $(XN.Inscreaser.Config().container).css('visibility', 'hidden');
        });


    },
    Open: function (type) {
        this.Close();
        $(this.Config().container).css('visibility', 'visible');
        //$('body').attr('style', 'overflow:hidden');
        var preload = type == 0 ? ins_loader_black : ins_loader;
        $('.md-wrapper').append(preload);
        $(this.Config().modal).addClass('md-show');
    },
    Close: function () {
        $('body').removeAttr('style');
        // $(this.Config().modal).find("*").off();
        $(this.Config().content).removeClass('md-finished').html('');
        $(this.Config().modal).removeClass('md-show');
        $(this.Config().container).css('visibility', 'hidden');
        $('.xn-modal .md-content').removeAttr('style');

    },

    Utilities: {
        LoadImg: function (index, type, src) {

            var timer;
            var timer1;
            function clearTimer1() {
                if (timer1) {
                    clearTimeout(timer1);
                    timer1 = null;


                }
            }


            function clearTimer() {
                if (timer) {
                    clearTimeout(timer);
                    timer = null;
                    $('.md-gif').remove();

                }
            }
            function paddingBottomProcents(width, height) {
                return Math.round((height * 100) / width);
            }

            $(XN.Inscreaser.Config().imgEl).hide();
            $('<img id="xn-i-beforesend" class="md-gif"  src="/Content/ajax-loaders/feed-load2.gif" width="31" height="31" />')
                .appendTo($(XN.Inscreaser.Config().imgEl)
                    .closest('div'));

            var screenWidth = document.getElementById('screenWidth').getAttribute('value');
            var isMobile = screenWidth < 400;

            $('#xn-i-beforesend').hide();
            var img = new Image();

            $(XN.Inscreaser.Config().imgEl).removeClass('img-responsive');

            img.onload = function () {

                var loaded = document.getElementById(XN.Inscreaser.Config().imgEl.substring(1));
                var imgWrap = loaded.parentElement.parentElement;
                var modal = findParentBySelector(imgWrap, '.md-modal');
                var mdCont = document.getElementsByClassName('md-content')[1];
                var spin = document.getElementsByClassName('ov-spin');

                // отображение картинки
                loaded.style.display = "";
                loaded.setAttribute('src', this.src);

                var mdContent = findParentBySelector(imgWrap, '.md-content');
                var insc = mdContent.children[0];
                mdContent.setAttribute('style', '');
                insc.setAttribute('style', '');

                //if (img.width < 300) mdContent.setAttribute('style', 'max-width:300px');

                if (type != 'prod') {
                    //if (img.width > 300 && procent > 120) mdContent.setAttribute('style', 'max-width:500px');
                    if (img.height > 500) mdContent.setAttribute('style', 'max-width:500px');
                    mdContent.setAttribute('style', 'max-width:' + img.width + 'px');
                }
                else {
                    var catalWindow = img.width + 350;
                    if (img.width < 585) mdContent.setAttribute('style', 'max-width:' + catalWindow + 'px');
                }
                if (img.height < 251) insc.setAttribute('style', 'min-height:inherit');
                // ширина дефолтная для окна -- desktop,tablet
                if (!isMobile) {
                    //findParentBySelector(imgWrap, '.md-content').style.maxWidth = loaded.clientWidth + 326 + 'px';
                    //findParentBySelector(imgWrap, '.md-content').style.top = wh + 'px';
                }
                //loaded.setAttribute('class', 'img-responsive');

                /*    ПОСЛЕ РЕСПОНСИВА КАРТИНКИ    */


                //if ($('.xn-d-w') >= 374) { $('.md-cat .xn-i-wrap1').css('overflowY', 'scroll'); $('.md-cat .xn-i-wrap1 .xn-i-details').css('width', '330px'); }
                if (type == 'prod') {
                    var mainImg = document.getElementById('xn-i-main');
                    var catalog_leftBlock = findParentBySelector(mainImg, '.xn-i-wrap1');
                    var differHeight = catalog_leftBlock.children[1].children[1].scrollHeight > img.height - 30;

                    //catalog_leftBlock.removeAttribute('style');
                    catalog_leftBlock.children[1].removeAttribute('style');
                    if (img.height < 300 || catalog_leftBlock.children[1].children[1].clientHeight >= 410 || differHeight) {
                        catalog_leftBlock.children[1].setAttribute('style', 'overflow-y:scroll');
                        //catalog_leftBlock.children[1].setAttribute('style', 'width:330px');

                    }

                }
                // процент отступа для картинки
                var procent = paddingBottomProcents(img.width, img.height);
                loaded.parentElement.setAttribute('style', 'padding-bottom:' + procent + '%');

                var whMob = Math.max(modal.clientHeight - (loaded.height + 168));

                clearTimer();

                timer1 = setTimeout(function () { clearTimer1(); spin.remove(); mdCont.className = 'md-content'; mdCont.className += ' md-finished' }, 300);
            };

            if (type == 'post') { img.src = this.GetCurrentImg(index).attr('src'); }
            else if (type == 'gal') {
                img.src = '/ImageData/GetPhotoGalleryImage?id=' + index + '&width=750&height=500&isGallery=false';
            }
            else if (type == 'prod') {

                if (!isMobile) img.src = src;
                else img.src = src;

            }
            else {

                img.src = XN.Inscreaser.Config().largePhotoUrl + this.FormatedUrl(this.GetCurrentImg(index).attr('src'), true);

            }

            timer = setTimeout(function (theImg) {

                return function () {
                    $('#xn-i-beforesend').show();
                };
            }(img), 100);


        },
        GetDataImg: function (id, ispreview, width, height, pid, src) {

            return ispreview ?
                $('<img data-id="' + id + '" data-pid="' + pid + '" src="/Content/Files/Product/' + pid + '/' + src + '" />') :
                $('<img data-id="' + id + '" data-pid="' + pid + '" src="/Content/Files/Product/' + pid + '/200x150/' + src + '" />');
        },
        GetDataGalImg: function (id, isGal, width, height) {
            return ispreview ?
                $('<img data-id="' + id + '" data-pid="' + pid + '" src="/ImageData/GetProdImage?pid=' + id + '&width=' + width + '&height=' + height + '" />') :
                $('<img data-id="' + id + '" data-pid="' + pid + '" src="/ImageData/GetProdImage?pimgid=' + id + '&width=' + width + '&height=' + height + '" />');
        },
        FormatedUrl: function (url, isCat) {
            //alert(url);
            if (typeof url != 'undefined') {
                return isCat ? url.match(/\?path=\S+[a-zA-Z0-9_&%\.-]+/) : '?path=~' + url;
            }
        },
        GetCurrentImg: function (index) {

            return $('.xn-i-sphoto').find('img[data-i-index="' + index + '"]');
        },
        updateCartEvent: function (id, a) {

            if (a) {
                $('.prod_id-' + id).html('товар добавлен');
                $('.prod_id-' + id).removeClass('cart__add').addClass('cart__view');
            } else {
                $('.prod_id-' + id).html('Купить');
                $('.prod_id-' + id).removeClass('cart__view').addClass('cart__add');
            }
        }
    },
    Default: {
        modal: '#modal-6',
        overlay: '.md-overlay',
        content: '#modal-6 .md-content',
        container: '.md-container',
        scont: '.xn-i-sphoto',
        isCat: false,
        prodMode: false,
        isGal: false,
        isPost: false,
        imgEl: '#xn-i-main',
        ImagesDataUrl: '',
        largePhotoUrl: '/Widget/LoadMainPic',
        smallPhotoUrl: '/Widget/LoadSmallPic',
        ctlBnext: '.xn-i-mright',
        ctlBprev: '.xn-i-mleft',
        modalHeight: 600,
        modalWidth: 935,
        target: '#xn-i-main'


    }

}
/*==========================================================================================*/
/*=====================================  Lazy-Loading widget ===============================*/
/*==========================================================================================*/
XN.AjaxRequest = {
    Success: function (response) {
        //setTimeout(function () {
        //    $(target).html(response);

        //}, '500');
    },
    MakeRequest: function (url, data, target) {



        $.ajax({
            beforeSend: function () {
                if (target == '#listview-target') $(target).html('<img style="margin: 0 auto;display: block;" src="/Content/main/icons-svg/micon.svg" />');
            },
            
            url: url,
            data: data,
          
            success: function (response) {

               // setTimeout(function () {
                    $(target).html(response);
                    $('.xn__popup').on('click', function () {

                        XN.Inscreaser.BuildModal('/Widget/Inscreaser', { type: 1, id: $(this).closest('.xn-listview-item').data('id') });
                    });

                    if (target == '#listview-target') {
                        $('.page-link').on('click', function () {
                            //ajax history state
                            var data = { catId: $(this).data('catid'), page: $(this).data('page') };

                            var str = $.param(data);
                            //var str = JSON.stringify(data);
                            history.pushState(JSON.stringify(data), 'prod+' + XN.getUrlParameter('page'), '/Product/ProdList?' + str);


                            //ajax page request

                            XN.AjaxRequest.MakeRequest('/Product/ProdListPartial', { jsonData: JSON.stringify(data) }, '#listview-target');
                        });
                    }

               // }, '500');
            },
            error: function () {
                //alert('Ошибка приложения!');
                $(target).empty();
            }
        });

    }

}

XN.Formatter = new Intl.NumberFormat('re-RU', {
    style: 'currency',
    currency: 'RUB',
    minimumFractionDigits: 0,
    // the default value for minimumFractionDigits depends on the currency
    // and is usually already 2
});



function collectionHas(a, b) { //helper function (see below)
    for (var i = 0, len = a.length; i < len; i++) {
        if (a[i] == b) return true;
    }

    return false;
}
function findParentBySelector(elm, selector) {
    var all = document.querySelectorAll(selector);
    var cur = elm.parentNode;
    while (cur && !collectionHas(all, cur)) { //keep going up until you find a match
        cur = cur.parentNode; //go up
    }
    return cur; //will return null if not found
}
function findClass(element, className) {
    var foundElement = null, found;
    function recurse(element, className, found) {
        for (var i = 0; i < element.childNodes.length && !found; i++) {
            var el = element.childNodes[i];
            var classes = el.className != undefined ? el.className.split(" ") : [];
            for (var j = 0, jl = classes.length; j < jl; j++) {
                if (classes[j] == className) {
                    found = true;
                    foundElement = element.childNodes[i];
                    break;
                }
            }
            if (found)
                break;
            recurse(element.childNodes[i], className, found);
        }
    }
    recurse(element, className, false);
    return foundElement;
}
Element.prototype.remove = function () {
    this.parentElement.removeChild(this);
}
NodeList.prototype.remove = HTMLCollection.prototype.remove = function () {
    for (var i = this.length - 1; i >= 0; i--) {
        if (this[i] && this[i].parentElement) {
            this[i].parentElement.removeChild(this[i]);
        }
    }
}


