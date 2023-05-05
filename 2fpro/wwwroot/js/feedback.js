var cont = $('.md-content').filter('#authTarget');

var sendFeed = function (e) {
    $('.md-content').hide(); $('.md-loader').show();
    e.preventDefault();
    disSubmit($(this).find('[type="submit"]'),true);
    $.ajax({
        url: '/Feedback/GetIndex',
        type: 'post',
        dataType: 'json',
        data: $(this).closest('form').serialize()
    })
    .done(function (data) {
        $('#authTarget').html(data);
        disSubmit($(this).find('[type="submit"]'), false);
        $('.md-content').show(); $('.md-loader').hide();
    });
} 
var sendOrder = function (e) {
    $('.md-content').hide(); $('.md-loader').show();
    e.preventDefault();
    disSubmit($(this).find('[type="submit"]'), true);
    $.ajax({
        url: '/Order/Makeorder',
        type: 'post',
        dataType: 'json',
        data: $(this).closest('form').serialize()
    })
    .done(function (data) {
        $('#authTarget').html(data);
        disSubmit($(this).find('[type="submit"]'), false);
        $('.md-content').show(); $('.md-loader').hide();
        $('*[data-prod-count="1"]').text("1");
        $('*[data-prod-count="1"]').data("prodNumb",1);
    });
} 
function disSubmit(elem, disable) {
    if (disable) elem.prop('disabled', true);
    else elem.prop('disabled', false);
}
$(function () {

    $(document).on('submit', 'form#feed-form', sendFeed);
    $(document).on('submit', 'form#order-form', sendOrder);
});