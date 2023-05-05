
var loader = $('<img class="submit-loader" src="/Content/ajax-loaders/horizont/89.gif" >');
var content = $('.modalp');
var submit = content.find('input[type="submit"]');
var target = $('#m-target');
var fail = $('#md-fail');
var i = $('<i class="icon-warning-sign" />&nbsp;');

function onSuccess(data) {
    if (data.type == "error") addError(data.message);
    else if (data.type == "success") { content.html('<p>Здравствуйте ' + data.username + ', вы успешно авторизовались</p>'); closeModal(); }
    else alert('Что-то пошло не так!');
    closeLoader();
    submit.unbind('click');
}
function onBegin() {

    submit.prop('disabled', true);
    addLoader();
    target.css('opacity', '0.5');

}
function onComplete() {
    submit.unbind('click');
    submit.prop('disabled', false);
    closeLoader();
    target.css('opacity', '1');
    //$("#sendfeed").empty().append('Отправить');
}


function onFailure() {
    // $("#sendfeed").empty().append('Отправить');
    alert("Ошибка ! Повторите снова.");
    submit.prop('disabled', false);
    closeLoader();
}
function addLoader() {
    //alert(submit.attr('class'));
    submit.closest('div').prepend(loader);
    submit.val('');

}

function closeModal() {
    setTimeout(function () {
        if ($('.md-show').length) {
            $('.md-show').remove();
            document.location.reload(true);
        }
    }, '3000');
}
function closeLoader() {
    $('.submit-loader').remove();
    submit.val('войти');
}
function addError(mess) {

    if (mess != '') fail.html(mess).prepend(i);
}
