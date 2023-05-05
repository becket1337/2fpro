"use strict";

// fail reconnected count 
var failCount = 0;
// interval for heartbeat
var keepAlive = 15;
var attempsCount = 0;
var reloadTimer = 0;

// given up with connection
var connDeterminated = false;


function loadHub() {

    console.log('hub loaded success !');
    var connection = new signalR.HubConnectionBuilder().withUrl("/myHub")
        .build();

    connection.start().catch(function (err) {
        return console.error(err.toString());
    });
    connection.on("ReceiveMessage", function (message, usersCount, loc, locCount, isadm, geoloc) {

        receiveMess(message, isadm, false, geoloc);

        $('.diag-online-value').html(usersCount);

        if (locCount > 1 || $('.geoloc_item[data-loc="' + loc + '"]').length) {


            $('.geoloc_item[data-loc="' + loc + '"] .diag-receiver__val').html(locCount);
        }
        else {

            $('.diag-location').append('<div class="geoloc_item" data-loc="' + loc + '" id="geoloc_' + loc + '">' +
                '<span class="diag-receiver__text">' + loc + '</span> - ' +
                '<span class="diag-receiver__val">' + locCount + '</span>' +
                '</div>');
        }

    });
    connection.on("ReceiveMessageOut", function (message, usersCount, loc, locCount, isadm, geoloc, test) {
        console.log(test);
        receiveMess(message, isadm, true, geoloc);

        $('.diag-online-value').html(usersCount);

        if (locCount >= 1) {

            $('.geoloc_item[data-loc="' + loc + '"] .diag-receiver__val').html(locCount);
        }
        else {
            $('.geoloc_item[data-loc="' + loc + '"]').remove();
        }
    });


    addHandlers(connection);

    connection.onclose(async () => {
        //await start(connection);
        console.log('closed');
    });
    // Переподключение 3 попытки, если пропала связь

    // проверка на жизнеспособность клиента 10сек
    //setInterval(() =>
    //    connection.invoke("HeartBeatTock")
    //        .then(() => console.log("Client heatbeat."))
    //        .catch(err => {
    //            console.log("reconnected!")

    //        }), keepAlive * 1000);
}
loadHub(); // Создание хаба

//сообщение
function receiveMess(mess, isadm, isout, geoloc) {
    //console.log(key);
    var encodedMsg;
    var msg = mess.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var timeNow = new Date().getHours() + ":" + (new Date().getMinutes() < 10 ? '0' : '') + new Date().getMinutes();
    var ismob = (geoloc.isMobileConn ? " (с моб.)" : "");
    var admMsg = "<span class='diag-row-orange'>msg: Здравствуйте админ - " + timeNow + ismob + "  </span> ";
    if (isout) encodedMsg = "<span class='diag-row-red'>msg: " + msg + "</span> ";//кто вышел
    else encodedMsg = "<span class='diag-row-green'>msg: " + msg + ismob + "</span> ";// кто вошел
    if (isadm) $('.diag-receiver').append(admMsg);//сооб админа
    else {// сооб пользователя
        if ($('.diag-receiver span').length > 5) {
            $('.diag-receiver span:eq(0)').remove();
            $('.diag-receiver').append(encodedMsg);
        }
        else $('.diag-receiver').append(encodedMsg);
    }
}


async function start(conn) {

    try {
        await conn.start();
        console.log('reconnected');
    } catch (err) {
        console.log(err);
        console.log('connecting..');
        failCount++;
        console.log(failCount + ' deter - ' + connDeterminated + ", ");
        if (failCount > 3) { conn.stop(); connDeterminated = true; }
        if (connDeterminated) { console.log('connection determinated!'); }
        else {
            setTimeout(() => start(), 5000);
        }
    }

};



//var timerId = setInterval(function () { console.log('iterval'); count = true; }, keepAlive * 1000);

//connection.on("Heartbeat", serverTime =>
//    console.log("Server heartbeat: " + serverTime));



function addHandlers(conn) {

    $('#conn_stop').bind('click', function (e) {
        conn.invoke("SignOut");
        connDeterminated = true;
        conn.stop();
    });
    $('#stream_btn').bind('click', function (e) {


        conn.invoke("DelHistory").catch(function (err) {
            return console.error(err.toString());
        });
        $('.diag-location').html('история удалена');

        e.preventDefault();
    });
    $('.diag-toggled').bind('click', function () {
        $(this).closest('.diag-wrapper').find('.diag-wrapper1').fadeToggle();
    });



}

//$(document).click(function (e) {
//    // Check for left button

//    if (e.button == 0) {
//        console.log('alive');
//    }

//});

//var count = false;
//$(window).scroll(function (event) {

//    //var timeoutId = this.setTimeout(function () { count <= 1 ? console.log('scroll') : "";count++; }, interval * 1000);
//    //console.log(count);
//    if (count)
//        console.log('scroll');
//    count = false;
//});