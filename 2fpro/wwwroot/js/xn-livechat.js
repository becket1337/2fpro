


var observe;
if (window.attachEvent) {
    observe = function (element, event, handler) {
        element.attachEvent('on' + event, handler);
    };
}
else {
    observe = function (element, event, handler) {
        element.addEventListener(event, handler, false);
    };
}

function resize1() {
    var text = document.getElementById('ch-edit');
    text.style.height = 'auto';
    text.style.height = text.scrollHeight + 'px';
    $('#chat-input').height($('.ch-field-input').height());

} function emptyInput1() {

    $('#ch-edit').val('');
    resize1();
    $('#ch-edit').focus();
}
function init() {

    var text = document.getElementById('ch-edit');
    var text1 = $("#chat-input");
    //text.style.height = 'auto';
    $('.ch-field-holder').css({ position: "relative" });
    function resize() {
        //text.style.height = 'auto';
        text.style.height = 'auto';
        text.style.height = text.scrollHeight + 'px';
        $('#chat-input').height($('.ch-field-input').height());

    } function emptyInput() {

        $('#ch-edit').val('');
        resize();
    }
    /* 0-timeout to get the already changed text */
    function delayedResize() {
        window.setTimeout(resize, 0);
    }
    observe(text, 'change', resize);
    observe(text, 'cut', delayedResize);
    observe(text, 'paste', delayedResize);
    observe(text, 'drop', delayedResize);
    observe(text, 'keydown', delayedResize);
    $(document).on('click', '.ch-clear', emptyInput);
    text1.focus();
    text1.select();
    // resize();

}
var textResize = init();

function LivechatInit() {

    $livechat = $('#livechat');
    $chatwrap = $('#livechat-wrap');
    $isAdmin = $livechat.hasClass('adm-lch-isauth');

    var chat = $.connection.xnovaHub; // hub

    var timeoutId; // таймер 
    //$conn = chat.connection.id;

    $.connection.hub.qs = { 'X-NOVA-consultant': '2.0' };
    $.connection.hub.disconnected(function () { setTimeout(function () { $.connection.hub.start(); }, 5000) });

    // Шлюз готов

    $.connection.hub.error(function (error) {
        console.log('SignalR error: ' + error)
    });
    chat.client.showInfo = function (mess) {
        alert(mess);

    }
    // Инитиализация виджета

    $(document).on('click', '#popup-livechat', popupLivechat);

    // Подключение к пользовательской группе

    $(document).on('click', '.ch-user', function () {

        // Загрузка пользовательского окна
        $this = $(this);
        $.getJSON('/Consultant/GetUserRoom', { conn: $(this).data('connid') }, function (result) {

            // вход консультанта в группу пользователя
            chat.server.joinGroup(result.groupId, 1);

            // обновление счетчика новых вопросов
            result.totalQ == 0 ? $('.ch-all-inbox').text("") : $('.ch-all-inbox').html("+" + result.totalQ);

            // удаление вопроса из очереди
            $this.remove();
            $('.ch-text').empty();

            // загрузка пользовательских вопросов в окно диалога
            for (var i = 0; i < result.totalM.length; i++) {

                $('.ch-text').append(
                '<div class="ch-comm-w">' +
                                   '<div class="ch-author">' + result.name + '</div>' +
                                   '<div class="arrow-top"></div>' +
                                   '<div class="ch-comm"><div style="padding:5px;">' + result.totalM[i].mess +
                                   '</div><div class="ch-date">' + result.totalM[i].date + '</div></div> </div>'

                )
            }
        });

        //chat.server.sendUserRoom($(this).data('connid'));
    });

    // Дисконект консультанта

    $(document).on('click', '.ch-close .fa-power-off', function () {

        $.connection.hub.stop();
        //chat.server.collapseChatroom();
    });

    // Сворачиваем окно

    $(document).on('click', '.ch-close .fa-angle-double-down', function () {
        $chatwrap.hide();
        //chat.server.collapseChatroom();
    });

    // Событие на авторизацию

    $(document).on("submit", "#livechat-form", function (e) {

        e.preventDefault();
        if ($isAdmin) {
            $.post('/Consultant/AllowUser', { connId: chat.connection.id, adm_auth: true, __RequestVerificationToken: $(this).closest('form').find('input[name="__RequestVerificationToken"]').val(), }, function (data) {

                $('#chat-input').show();
                $('.live-form').hide();
                $('.ch-loader').remove();
                $('#ch-edit').focus();
            });
            return false;
        }

        var jdata = {
            Username: $('#l-name').val(),
            Email: $('#l-email').val(),
            FeedMessage: $('#l-feed').val(),
            IsAdmin: false,
            IsOnline: true,
            __RequestVerificationToken: $(this).closest('form').find('input[name="__RequestVerificationToken"]').val(),
            connId: chat.connection.id
        };
        var option = {
            beforeSend: function () { $('.live-form').hide(); $('.ch-text').append('<div class="ch-loader">Подключение</div>'); setTimer(); },
            url: "/Consultant/AllowUser",
            data: jdata,
            type: 'post',
            error: function () { alert('error'); },
            complete: function () { $('.live-form').show(); $('.ch-loader').remove(); },
            success: function (data) {

                chat.server.joinGroup(chat.connection.id, 0);
                chat.server.notifyAboutConnect($('#l-name').val());

                $('#chat-input').show();
                $('.live-form').hide();
                $('.ch-loader').remove();
                $('.ch-text').html(
                         '<div class="ch-comm-w">' +
                                    '<div class="ch-author">Консультант</div>' +
                                    '<div class="arrow-top-adm"></div>' +
                                    '<div class="ch-admreply">Здравствуйте! Чем мы вам можем помочь ?</div> </div>'

                    );
                $('#ch-edit').focus();
            }
        };

        $.ajax(option);

    });

    // Событие на отправку сообщения на почту

    $(document).on("submit", "#livechat-feed", function (e) {
        e.preventDefault();

        var jdata = {
            Username: $('#l-name').val(),
            Email: $('#l-email').val(),
            FeedMessage: $('#l-mess').val(),
            IsAdmin: false,
            IsOnline: true,
            __RequestVerificationToken: $(this).closest('form').find('input[name="__RequestVerificationToken"]').val()

        };
        var option = {
            beforeSend: function () { $('#l-feedsend').prop("disabled", true); $('.lch-loader').show(); },
            url: "/Consultant/SendFeed",
            data: jdata,
            type: 'post',
            error: function () { alert('error'); },
            complete: function () { $('#l-feedsend').prop("disabled", false); $('.lch-loader').hide(); },
            success: function () {
                $('.ch-text').empty();
                $('.ch-active').html(
                         'Сообщение отправлено!'

                    );
            }
        };

        $.ajax(option);

    });
    chat.client.test = function (a, b, c) {
        $('.ch-active').html("<p>GroupId: " + a + "</p><p>AdmId: " + b + "</p><p>connId: " + c + "</p>");
    }
    // Create a function that the hub can call back to display messages.
    chat.client.getUserRoom = function (n, m, conn) {
        $('.ch-text').html('');
    };

    // Публикация сообщения

    chat.client.addNewMessageToPage = function (date, name, message, isadm) {

        console.log("дата +" + date + " | имя " + name + " | сообщение " + message + " | права " + isadm);

        if (message != "") {
            if (isadm) {
                $('.ch-text').append(
                    '<div class="ch-comm-w">' +
                    '<div class="ch-author">' + name + '</div>' +
                    '<div class="arrow-top-adm"></div>' +
                    '<div class="ch-admreply"><div style="padding:5px;">' + message + '</div><div class="ch-date">' + date + '</div></div> </div>'
                    );
            }
            else {
                $('.ch-text').append(
                    '<div class="ch-comm-w">' +
                    '<div class="ch-author">' + name + '</div>' +
                    '<div class="arrow-top"></div>' +
                    '<div class="ch-comm"><div style="padding:5px;">' + message + '</div><div class="ch-date">' + date + '</div></div> </div>'
                   );
            }
            $(".ch-body-3_1").mCustomScrollbar("update");
            $(".ch-body-3_1").mCustomScrollbar("scrollTo", ".last-line", {
                scrollInertia: 0

            });
        }
        return false;
    };

    // Новое сообщение от пользователя

    chat.client.notifyNewQuestion = function (n, c, conn, a) {
        $('audio').remove();
        var pushAudio = function () {
            $("<audio class='ch-aud'></audio>").attr({
                'src': '/Content/Audio/livechat-message.mp3',
                'volume': 0.4,
                'autoplay': 'autoplay'
            }).appendTo("body");
        };

        if (!$('.ch-user[data-connid="' + conn + '"]').length) {
            pushAudio();
            $('.ch-all-inbox').text("+" + a);
            $('.ch-inbox').append(
                '<div class="ch-user" style="display:none" data-connid="' + conn + '">' +
               ' <div class="ch-username">' + n + '</div>' +
                   ' <div class="ch-inbox-cnt">' + c + '</div>' +
                  '</div>'
                );
            $('.ch-user[data-connid="' + conn + '"]').show('highlight', { duration: 1000 });
        }
        else {

            $('.ch-user[data-connid="' + conn + '"] .ch-inbox-cnt').html(c);
        }
    }

    // Счет подключившихся

    chat.client.notifyAboutConnection = function (count, name, time) {
        $('.in-chat').html(count);
        if (name != null) {
            $('.ch-notifybox-user').html(name + ' зашел в ' + time);
        }
    }

    // Новое сообщение от консультанта

    chat.client.notifyAdmAnswear = function () {
        var pushAudio = function () {
            $("<audio class='ch-aud'></audio>").attr({
                'src': '/Content/Audio/livechat-message.mp3',
                'volume': 0.4,
                'autoplay': 'autoplay'
            }).appendTo("body");
        };
        $('audio').remove();
        pushAudio();
    }

    //chat.client.afterGroup = function (mess) {
    //    $('.ch-active').html(mess);
    //}
    chat.client.showErrorMessage = function (mess) {

        notifyInfo(mess);
    }
    chat.client.showInfo = function (mess) {
        notifyInfo(mess);

    }
    // Консультант отключился

    chat.client.adminOut = function (mess) {

        $('.ch-user').remove();
        $('#chat-input').hide();
        //$('.live-form').load('/Consultant/FormFeed').show();
        $('.ch-active').html(mess);

    }

    // Пользователь отключился

    chat.client.userOut = function (conn, mess, total, isInGroup) {
        $("audio").remove();
        total == 0 || total == -1 ? $('.ch-all-inbox').text("") : $('.ch-all-inbox').text("+" + total);
        isInGroup ? notifyInfo(mess) : "";
        $('.ch-user[data-connid="' + conn + '"]').remove();
    }

    // Дисконект пользователя

    chat.client.notifyAboutDisc = function (conn) {
        $('.ch-user[data-connid="' + conn + '"]').remove();
    }
    $('#message').focus();


    $.connection.hub.start().done(function () {

        console.log("conId:" + $.connection.hub.id);
        console.log("transport:" + $.connection.hub.transport.name);

        $('#livechat').attr("data-uid", $.connection.hub.id);
        $('#ch-edit').on("keyup", function (e) {

            if (e.keyCode == 13) {
                // Отправка сообщения

                chat.server.send($(this).val(), $.connection.hub.id)
                    .fail(function (e) {

                        // обработка ошибок
                        if (e.source === 'HubException') {
                            console.log(e.message + ' : ' + e.data.user);
                            notifyError(e.message);
                        }
                        else {
                            notifyError('Не удалось отправить сообщение!' + e.message);
                        }
                    });

                // Обнуление textarea
                emptyInput1();
                $('#ch-edit').focus();

                //$(".ch-body-3_1").mCustomScrollbar("scrollTo", ".ch-active");
            }
            else { return false; }
        });
    }).fail(function (error) { $('.ch-error').html("Ошибка подключения!"); });;
    // Напоминание - info

    function notifyInfo(m, hide) {

        clearTimeout(timeoutId);
        hide == 0 ? timeoutId = setTimeout(function () { $('.ch-active').fadeOut('fast', function () { $('.ch-active').html('').show(); }); }, 5000) : '';
        var icon = $('<i class="fa fa-warning-sign"></i><span> </span>');
        $('.ch-error').html('');
        $('.ch-active').html(m).prepend(icon);
    }

    // Напоминание - error

    function notifyError(m) {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(function () { $('.ch-error').fadeOut('fast', function () { $('.ch-error').html('').show(); }); }, 5000);
        var icon = $('<i class="icon-warning-sign"></i><span> </span>');
        $('.ch-active').html('');
        $('.ch-error').html(m).prepend(icon);
    }
    // This optional function html-encodes messages for display in the page.
    function htmlEncode(value) {
        var encodedValue = $('<div />').text(value).html();
        return encodedValue;
    }
    function setTimer() {
        setTimeout(function () {
            var str = $('.ch-loader').text();
            if (str.match(/\.{3}/) != null) {
                str = str.substr(0, str.indexOf("."));
            }
            str += ".";
            $('.ch-loader').text(str);
            setTimer();
        }, 1000);
    }
}