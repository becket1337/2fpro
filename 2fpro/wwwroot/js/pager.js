window.onpopstate = function (event) {

    
    if (document.location.pathname.indexOf("/Admin/UploadFiles/") == 0) {
        $.post("/Admin/UploadFiles/GetFiles", event.state, function (data) {
            $("#FilesList").effect('highlight');
            $("#FilesList").html(data);
            $(".stripedMe tbody tr:even").addClass("alt");
            imagePreview();
            removeLoader();
        });
    }
};

//для фалов менеджера лоадер
function addLoader() { $('.manager-loader').show(); }
function removeLoader() { $('.manager-loader').hide(); }
// для каталога лоадер
function addProdLoader() { $('#listview').append(' <img src="/Content/ajax-loaders/svg-loaders/puff.svg" class="carousel-main__loader" />'); $('#listview-target').css('opacity', '0.5');}
function removeProdLoader() { $('.carousel-main__loader').remove(); $('#listview-target').css('opacity', '1'); }

function getData(target) {
 var modelParam, partsParam = [];

    if (target.data('service') == "files") {
        return { page: target.data('page'), folder: target.data('folder') };
    }
    if (target.data('service') == "prod") {

        if (getUrlParameter("models") !== undefined) {
            var getArray = getUrlParameter("models");
            var modelParam = getArray.toString().split(',').map(function (item) { return parseInt(item, 10); });

        }

        if (getUrlParameter("parts") !== undefined) {
            var getArray1 = getUrlParameter("parts");
            var partsParam = getArray1.toString().split(',').map(function (item) { return parseInt(item, 10); });
        }
       
        var refresh = window.location.protocol + "//" + window.location.host + window.location.pathname;

        var url = new URL(refresh);
        var search_params = new URLSearchParams(url.search);
        search_params.set('page', target.data('page'));

        return { models: modelParam, parts: partsParam, page: target.data('page') }; 
    }
}
function processStart(target) { //активация пагинации
    var jsonData = getData(target);
   

    if (target.data('service') == 'files') { // для файлов менеджера
        $.post("/Admin/UploadFiles/GetFiles", jsonData, function (data) {
            $("#FilesList").effect('highlight');
            $("#FilesList").html(data);
            $(".stripedMe tbody tr:even").addClass("alt");
            imagePreview();
            removeLoader();

            var dataJson = jsonData;
            var str = $.param(jsonData);
            history.pushState(JSON.stringify(dataJson), 'files', '/Admin/UploadFiles/Index?' + str);
        });
    }
    if (target.data('service') == 'prod') {// для каталога
        
        $.get("/Product/ProdListPartial", { jsonData:JSON.stringify(jsonData) }, function (data) {
            setTimeout(function () {
                $("#listview-target").html(data);

                removeProdLoader();
              
                if (XN.IsMobile) {
                    $('html').css('overflow', 'hidden');
                    $('html, body').scrollTop($("html").offset().top);
                    $('html').css('overflow', 'auto');
                }
                else { $('html, body').scrollTop($("body").offset().top); }
            }, 200);
            var dataJson = jsonData;
            var str = $.param(jsonData);
            
            var catid = dataJson.catid;
            var name = jsonData.catname;
            var page = jsonData.page;
            //history.pushState(JSON.stringify(dataJson), 'files', '/ProdList/' + catid + "/" + name+"/"+page);
            });
       
    }
}

var clickPager = function () {//обработчик клика
    if ($(this).data('service') == 'files') addLoader();
    if ($(this).data('service') == "prod") addProdLoader();
    processStart($(this));
    }


    $(document).on('click', '#section-pager a', clickPager);
//hisotry ajax start page 1

