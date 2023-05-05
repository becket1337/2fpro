"use strict";
var XN_signalr_Module = XN_signalr_Module || {};
// кол во попыток переподключения
var failCount = 0;
// сердцебиение
var keepAlive = 15;

var reloadTimer = 0;
// интервал смерти
var terminatedTimer = 35;
// подключение окончательно закрыто
var connDeterminated = false;
// интервал обновления таймера жизни пользователя
var resetTimer;
var connection;

XN_signalr_Module = {

    signalRConnection: "",
    visitorQuit: function (visitor, conn) {
       
        conn.invoke("heyho", visitor).catch(function (err) {
            //return console.error(err.toString());
        }).then(function () {
            //console.log('клиент ушел');
        });
    },
    //Интервал жизни
    resetTimer: function(visitor,conn) {
        
        visitor.find('.diag-user-timer').removeClass("diag-user-timer-off").html('Online');

        function myFn() {
         
            visitor.find('.diag-user-timer').addClass("diag-user-timer-off").html('Offline');
            clearInterval(visitor.attr('data-timerid'));
           
            conn.invoke("heyho", visitor.data('username')).catch(function (err) {
                //console.log('str');
               //return console.error(err.toString());
            }).then(function () {  });
        }
        // главный интервал - не подает признаки жизни, закрываем и обновляем статус онлайн у пользователя аа
        function mainTimer() {
            resetTimer = setInterval(myFn, terminatedTimer * 1000);
            visitor.attr('data-timerid', resetTimer);
        }
   
        //обнуляем главный интервал 
        clearInterval(visitor.attr('data-timerid'));
        mainTimer();
        

    },
    //таймеры время жизни пользователей
    setVisitorsKeepAliveTimer: function(conn) {
        for (var i = 0; i < $('.diag-user').length; i++) {

            XN_signalr_Module.resetTimer($('.diag-user:eq(' + i + ')'),conn);
            mainWidgetUserInfoEvent($('.diag-user:eq(' + i + ')').data('username'));
        }
    },
    anihilateVisitor: function (visitor) {
        XN_signalr_Module.signalRConnection.invoke("UserDeterminated", visitor).catch(function (err) {
            //return console.error(err.toString());
        });
        visitor.find('.diag-user-timer').html('dead');
        clearInterval(visitor.attr('data-timerid'));
    },
   // нет применений!!!
    updateUserList: function (username) {
        var visitor = $('.diag-user[data-username="' + username + '"]');
        if (!visitor.length) {
            var formUsername = username.length > 15 ? username.substr(0, 15) + ".." : username;
            var user =
                '<div class="diag-user diag_identity" data-username="' + username + '" data-timerid="" data-secount>' +
                    '<span class="diag-user-name" title="'+username+'">' + formUsername + '</span>' +

                    '<span class="diag-user-time">' + new Date().toLocaleTimeString() + '</span>&nbsp;' +
                    '<span class="diag-user-timer"> Online</span>' +
                    '<span class="diag-details"><a class="diag-trigger-moreinfo"><span class=""> </span><i class="fa fa-globe"></i></a></span>'+
                    ' </div>';
            $('.diag-users-list').prepend(user);
            mainWidgetUserInfoEvent();
        }
        else visitor.find('.diag-user-timer').removeClass("diag-user-timer-off").html('Online');
    }

};




function loadHub() {

   
     connection = new signalR.HubConnectionBuilder().withUrl("/myHub")
        .build();

   

    connection.start().catch(function (err) {
        //return console.error(err.toString());
    }).then(function () {

       

    });
    //init realtime xn module connection
    
    //после подключения
    connection.on("ReceiveMessage", function (message, usersCount, loc, locCount, isadm, geoloc, username, role) {
        //console.log(message + ", " + isadm + ", " +geoloc);
        updateUsersList(username,true,geoloc.ipAddress,role);//если новый пользователь, обновляем список

        receiveMess(message, isadm, false, geoloc);//новое сообщение на клиенте

        $('.diag-online-value').html(usersCount);

        updateLocation(loc, locCount);//обновляем геолокацию
        var visitor = $('.diag-user[data-username="' + username + '"]');
        XN_signalr_Module.resetTimer(visitor, connection); //создать новый интервал 
    });
    // статус онлайн на сайте, обновляем жизненный цикл 
    connection.on("AliveTimerUpdater", function (username, currTimeVisit) {
        //console.log(username + ", " + currTimeVisit);
        updateAliveInterval(username);
      
    });
    //отключение
    connection.on("ReceiveMessageOut", function (message, usersCount, loc, locCount, isadm,geoloc, username) {
       
        receiveMess(message, isadm, true, geoloc);

        $('.diag-online-value').html(usersCount);

        updateLocation(loc, locCount);
        if (!isadm) updateUsersList(username,false);
    });
    connection.on("badrequest", (data) => {
        //console.error(data);
    });
   
   // проверка  - жив или  нет
    connection.on("hbeat", (data, timeVisit) => {
        
        var visitor = $('.diag-user[data-username="' + data + '"]');
        clearInterval(visitor.attr('data-timerid'));
        //$('.diag-user[data-username="' + data + '"]').effect("highlight", {}, 1000);
        XN_signalr_Module.resetTimer(visitor,connection);

        //XN_signalr_Module.visitorQuit(data,connection);
       
    });
    addHandlers(connection);
 // Переподключение 3 попытки, если пропала связь
    connection.onclose(async () => {
        await start(connection);
        
    });

  
    // интервал проверка на жизнеспособность клиента 15сек
    setInterval(() =>
        connection.invoke("HeartBeatTock")
            //.then(() =>) console.log("Client heatbeat.")
            .catch(err => {
               

            }), keepAlive * 1000);

    XN_signalr_Module.setVisitorsKeepAliveTimer(connection);
}
 // Создание хаба

loadHub();

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
//Обновить регионы
function updateLocation(loc, locCount) {
    //регион существует
    
    var geoloc = $('.geoloc_item[data-loc="' + loc + '"]');
  
    if (geoloc.length) {

        if (locCount ==0) geoloc.remove();
        else geoloc.find('.diag-receiver__val').html(locCount);
    }
    else {

        $('.diag-location').append('<div class="geoloc_item" data-loc="' + loc + '" id="geoloc_' + loc + '">' +
            '<span class="diag-receiver__text">' + loc + '</span> - ' +
            '<span class="diag-receiver__val">' + locCount + '</span>' +
            '</div>');
    }
}
//добавление нового в список пользователей онлайн
function updateUsersList(username,isenter,ip,role) {
    var visitor = $('.diag-user[data-username="' + username + '"]');
    if (!isenter) {//если пользователь ушел
        visitor.remove();
    }
    else { //если пришел
        if (!visitor.length) {
            var formUsername = username.length > 15 ? username.substr(0, 15) + ".." : username;
            var resultName = role == "User" ? formUsername : role;
            var user = 
                '<div class="diag-user diag_identity" data-username="' + username + '" data-timerid="" data-secount>' +
                '<span class="diag-user-name">' + resultName + '</span>' +

                '<span class="diag-user-time"> ' + new Date().toLocaleTimeString() + '</span>&nbsp;' +
                '<span class="diag-user-timer">Online</span>' +
                '  <span class="diag-details"><a class="diag-trigger-moreinfo"><span class=""> </span><i class="fa fa-globe"></i></a></span>' +
                ' </div>';
            $('.diag-users-list').prepend(user);
            mainWidgetUserInfoEvent(username);
        }
        else visitor.find('.diag-user-timer').html('alive');
    }
}
//Update KeepAlive
function updateAliveInterval(username) {
    clearInterval($('.diag-user[data-username="' + username + '"]').attr('data-timerid'));
    //$('.diag-user[data-username="' + username + '"]').effect("highlight", {}, 1000);
}
//продлить жизнь подключения еще на 30сек
//

async function start(conn) {
    if (connDeterminated) {
        //console.log('сеанс закончен!');
    }
    else {
        try {
            await conn.start();
            //console.log('связь восстановлена');
        } catch (err) {
            
            //console.log('переподключение..');
            failCount++;
           
            if (failCount > 3) { conn.stop(); connDeterminated = true; }

            setTimeout(() => start(conn), 5000);

        }
    }
};



//var timerId = setInterval(function () { console.log('iterval'); count = true; }, keepAlive * 1000);
//
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
            //return console.error(err.toString());
        });
        $('.diag-location').html('история удалена');

        e.preventDefault();
    });
  

    
}
function closeEvents() {

    $('#diag-info-back').bind('click', function () {
      
        $('.userinfo_wrapper').remove();
        closeEvents();
      
    });


    $('#diag-userslist-back').bind('click', function () {
      
        $('.diag-userslist_wrapper').remove();
        closeEvents();
      
    });
  


  
}
function GetInfoEvent() {//новые оброботчик для юзер лист
    $('.diag-userslist_wrapper .diag-trigger-moreinfo').bind('click', function () {
        var dataJson = {
            uname: $(this).closest('.diag_identity').data('username')
        };
        var opts = {
            url: "/Diagnostic/GetUserInfo",
            method: 'post',
            data: { data: JSON.stringify(dataJson) }
        }
        $.ajax(opts).done(function (data) {
            $('.diag-wrapper').append(data);
            closeEvents();
        });
    });


}
// юзер лист
$('#getusers_list').bind('click', function () {

    var opts = {
        url: "/Diagnostic/GetUsersList",
        method: 'post'

    }
    $.ajax(opts).done(function (data) {
        $('.diag-wrapper').append(data);
        closeEvents();
        GetInfoEvent();
    });
});
function mainWidgetUserInfoEvent(username) {
    $('.diag-user[data-username="'+username+'"] .diag-trigger-moreinfo').bind('click', function () {
        var dataJson = {
            uname: $(this).closest('.diag_identity').data('username')
        };
        var opts = {
            url: "/Diagnostic/GetUserInfo",
            method: 'post',
            data: { data: JSON.stringify(dataJson) }
        }
        $.ajax(opts).done(function (data) {
            $('.diag-wrapper').append(data);
            closeEvents();
        });
    });
}
// юзер инфо обработчик для главного окна

// открыть закрыть главный виджет
   $('.diag-toggled').bind('click', function () {
        $(this).closest('.diag-wrapper').find('.diag-wrapper1').fadeToggle();
        $(this).hide();

    });
$('.diag-close').bind('click', function () {
        $(this).closest('.diag-wrapper').find('.diag-wrapper1').fadeToggle();
        $('.diag-toggled').show();
    });



GetInfoEvent();
closeEvents();

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