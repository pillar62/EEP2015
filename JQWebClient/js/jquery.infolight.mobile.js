var databaseType = '';
$(document).live('pagecreate', function (e) {
    if (databaseType == '') {
        $.ajax({
            type: "POST",
            url: window.currentUrl,
            data: "mode=databasetype",
            cache: false,
            async: false,
            success: function (data) {
                databaseType = data;
            }, error: function (data) {

            }
        });
    }

    $.ajax({
        type: "POST",
        url: 'handler/SystemHandle.ashx?type=IsWorkflow',
        cache: false,
        async: true,
        success: function (data) {
            if (data != "true") {
                $("#fieldsetMenu").css("display", "none");
            }
        },
        error: function (data) {
            //data.responseText = '';
            //obj = "[{\"" + textField + "\":\"\"}]";
        }
    });

    //for logon and main form
    $(window).bind("orientationchange resize pageshow", fixgeometry);
    fixgeometry();
    if (!$.languageInitialized) {
        $.initLanguage(e.target);
    }
    $("." + $.fn.logon.class, e.target).logon({});
    $("." + $.fn.main.class, e.target).main({});

    if ($.fn.flow) {
        $("." + $.fn.flow.class, e.target).each(function () {
            if (e.target.id == "pInbox") {
                $(this).flow("initializeInbox");
            }
            else if (e.target.id == "pOutbox") {
                $(this).flow("initializeOutbox");
            }
            else if (e.target.id == "pNotify") {
                $(this).flow("initializeNotify");
            }
            else if (e.target.id == "pDelay") {
                $(this).flow("initializeDelay");
            }
            else {
                $(this).flow({});
            }
        });
    }

    //for user form
    $("." + $.fn.datagrid.class, e.target).each(function () {
        var parentObjectID = $.parseOption($(this).attr('infolight-options')).parentObjectID;
        if (parentObjectID == undefined) {
            $(this).datagrid({});
        }
    });
    //    $("." + $.fn.form.class).each(function () {
    //        $(this).form({});
    //    });
    //    $("." + $.fn.query.class, e.target).each(function () {
    //        $(this).query({});
    //    });
    $('.info-rotator').each(function () {
        $(this).rotator('slide');
    });

});


//--------------------------------------------------------------------------------------------------------------------------------
var fixgeometry = function () {
    /* Some orientation changes leave the scroll position at something
    * that isn't 0,0. This is annoying for user experience. */
    scroll(0, 0);
    /* Calculate the geometry that our content area should take */
    var header = $("[data-role='header']:visible");
    var footer = $("[data-role='footer']:visible");
    var content = $("[data-role='content']:visible");
    var logo = $(".logo:visible");

    if (content.length > 0) {
        if (content.children('.ui-selectmenu-list').length > 0) {
            return;
        }
        var viewport_height = $(window).height();

        var content_height = viewport_height;
        if (header.length > 0) {
            content_height -= header.outerHeight();
        }
        if (footer.length > 0) {
            content_height -= footer.outerHeight();
        }
        if (logo.length > 0) {
            content_height -= logo.outerHeight();
        }
        /* Trim margin/border/padding height */
        content_height -= (content.outerHeight() - content.height());
        content_height -= 10;
        content.height(content_height);

        var compensateToolbars = function (page, desiredHeight) {
            var pageParent = page.parent(),
			toolbarsAffectingHeight = [],
			externalHeaders = pageParent.children(":jqmData(role='header')"),
			internalHeaders = page.children(":jqmData(role='header')"),
			externalFooters = pageParent.children(":jqmData(role='footer')"),
			internalFooters = page.children(":jqmData(role='footer')");

            // If we have no internal headers, but we do have external headers, then their height
            // reduces the page height
            if (internalHeaders.length === 0 && externalHeaders.length > 0) {
                toolbarsAffectingHeight = toolbarsAffectingHeight.concat(externalHeaders.toArray());
            }

            // If we have no internal footers, but we do have external footers, then their height
            // reduces the page height
            if (internalFooters.length === 0 && externalFooters.length > 0) {
                toolbarsAffectingHeight = toolbarsAffectingHeight.concat(externalFooters.toArray());
            }

            $.each(toolbarsAffectingHeight, function (index, value) {
                desiredHeight -= $(value).outerHeight();
            });

            // Height must be at least zero
            return Math.max(0, desiredHeight);
        };

        var page = $("." + $.mobile.activePageClass),
				pageHeight = page.height(),
				pageOuterHeight = page.outerHeight(true);

        height = compensateToolbars(page,
				(typeof height === "number") ? height : $.mobile.getScreenHeight());

        page.css("min-height", height - (pageOuterHeight - pageHeight) - 10);
    }

    $("." + $.fn.datagrid.class).datagrid('resize');
    $("." + $.fn.datalist.class).datalist('resize');

};            /* fixgeometry */

jQuery.cookie = function (name, value, options) {
    if (typeof value != 'undefined') { // name and value given, set cookie
        options = options || {};
        if (value === null) {
            value = '';
            options.expires = -1;
        }
        var expires = '';
        if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
            var date;
            if (typeof options.expires == 'number') {
                date = new Date();
                date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
            } else {
                date = options.expires;
            }
            expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
        }
        var path = options.path ? '; path=' + options.path : '';
        var domain = options.domain ? '; domain=' + options.domain : '';
        var secure = options.secure ? '; secure' : '';
        document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
    } else { // only name given, get cookie
        var cookieValue = null;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = jQuery.trim(cookies[i]);
                // Does this cookie string begin with the name we want?
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }
};

$.fn.setText = function (text) {
    if ($(this).find('.ui-btn-text').length > 0) {      //1.3.2
        $(this).find('.ui-btn-text').html(text);
    }
    else {                                              //1.4.2
        $(this).html(text);
    }
};



//--------------------------------------------------------------------------------------------------------------------------------
$.fn.logon = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.logon.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            $(this).logon('initialize');
            if (!$(this).hasClass($.fn.logon.class)) {
                $(this).addClass($.fn.logon.class)
            }
        });
    }
};

$.fn.logon.class = 'info-logon';

$.fn.logon.defaults = {
    userIDText: "UserID",
    passwordText: "Password",
    databaseText: "Database",
    solutionText: "Solution",
    rememberText: "Remember",
    logonText: "Logon",
    cancelText: "Cancel",
    forgetText: "Forget password",
    returnText: "Return",
    resetText: "Reset",
    userIDValidateMessage: "UserID can not be empty."
};

$.fn.logon.methods = {
    initialize: function (jq) {
        if (jq.length > 0) {
            //load database
            if ($("#database", jq[0]).children().length == 0) {
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: $.getSystemUrl(),
                    data: "mode=getDatabases",
                    cache: false,
                    async: true,
                    success: function (data) {
                        $("#database", jq[0]).empty();
                        for (var i = 0; i < data.length; i++) {
                            $("#database", jq[0]).append($.createOption(data[i], data[i]));
                        }
                        var lastDatabase = $.cookie("database");
                        if (lastDatabase != null) {
                            $("#database", jq[0]).val(lastDatabase);
                        }
                        $("#database", jq[0]).selectmenu("refresh");
                    }
                });
            }
            else {
                var lastDatabase = $.cookie("database");
                if (lastDatabase != null) {
                    $("#database", jq[0]).val(lastDatabase);
                }
            }
            //load solution
            if ($("#solution", jq[0]).children().length == 0) {
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: $.getSystemUrl(),
                    data: "mode=getSolutions",
                    cache: false,
                    async: true,
                    success: function (data) {
                        $("#solution", jq[0]).empty();
                        for (var i = 0; i < data.length; i++) {
                            $("#solution", jq[0]).append($.createOption(data[i].Name, data[i].ID));
                        }
                        var lastSolution = $.cookie("solution");
                        if (lastSolution != null) {
                            $("#solution", jq[0]).val(lastSolution);
                        }
                        $("#solution", jq[0]).selectmenu("refresh");
                    }
                });
            }
            else {
                var lastSolution = $.cookie("solution");
                if (lastSolution != null) {
                    $("#solution", jq[0]).val(lastSolution);
                }
            }
            var lastUserID = $.cookie("userID");
            if (lastUserID != null) {
                $("#userID", jq[0]).val(lastUserID);
            }
            var password = $.cookie("password");
            if (password != null) {
                $("#password", jq[0]).val(password);
                $("#remember", jq[0]).attr('checked', true);
                $("#remember", jq[0]).checkboxradio("refresh")
            }

            //language
            $("label[for='userID']", jq[0]).html($.fn.logon.defaults.userIDText);
            $("label[for='password']", jq[0]).html($.fn.logon.defaults.passwordText);
            $("label[for='database']", jq[0]).html($.fn.logon.defaults.databaseText);
            $("label[for='solution']", jq[0]).html($.fn.logon.defaults.solutionText);
            $("label[for='remember']", jq[0]).setText($.fn.logon.defaults.rememberText);
            $("label[for='user']", jq[0]).html($.fn.logon.defaults.userIDText);
            $("a.logon").setText($.fn.logon.defaults.logonText);
            //$("a.cancel").setText($.fn.logon.defaults.cancelText);
            $("a.cancel").setText($.fn.logon.defaults.forgetText);
            $("a.return").setText($.fn.logon.defaults.returnText);
            $("a.reset").setText($.fn.logon.defaults.resetText);
        }
    },
    logon: function (jq) {
        if (jq.length > 0) {
            var obj = new Object();
            obj.userID = $("#userID", jq[0]).val();
            if (obj.userID.length == 0) {
                $("p", "#error").html($.fn.logon.defaults.userIDValidateMessage);
                $("#error").popup("open");
                return;
            }

            obj.password = $("#password", jq[0]).val();
            obj.database = $("#database", jq[0]).val();
            obj.solution = $("#solution", jq[0]).val();
            var remember = $("#remember", jq[0]).attr("checked");

            $.mobile.loading('show', { theme: 'b', text: $.fn.logon.defaults.logonText, textVisible: true });

            $.ajax({
                type: "POST",
                dataType: 'json',
                url: $.getSystemUrl(),
                data: "mode=logon&data=" + $.toJSONString(obj),
                cache: false,
                async: true,
                success: function (data) {
                    $.cookie("userID", obj.userID, { expires: 14 });
                    $.cookie("database", obj.database, { expires: 14 });
                    if (remember && obj.password) {
                        $.cookie("password", obj.password, { expires: 14 });
                    }
                    else {
                        $.cookie("password", null);
                    }
                    $.cookie("solution", obj.solution, { expires: 14 });
                    $.mobile.loading('hide');
                    $.mobile.changePage("MobileMainPage.aspx" + location.search, { transition: "slideup", role: "page" })
                },
                error: function (data) {
                    $.mobile.loading('hide');
                    $("p", "#error").html($.getErrorMessage(data.responseText));
                    $("#error").popup("open");
                }
            });
        }
    },
    forget: function (jq) {
        //window.location.href = "InnerPages/MobileResetPWD.aspx?db=" + $("#database", jq[0]).val();
        $.mobile.changePage("InnerPages/MobileResetPWD.aspx?db=" + $("#database", jq[0]).val(), { transition: "slideup", role: "page" });
    },
    resetP: function (jq) {
        $("p", "#reseterror").html('');
        var db = Request.getQueryStringByName('db');
        if ($('#user').val() == '') {
            $("p", "#reseterror").html('用戶名不能為空');
            $("#reseterror").popup("open");
            return;
        }
        if ($('#email').val() == '') {
            $("p", "#reseterror").html('Email不能為空');
            $("#reseterror").popup("open");
            return;
        }

        var match = $('#user').val().match(/[\w\d]+/g);
        if (match && match.length > 0 && match[0].length == $('#user').val().length) {

        }
        else {
            $("p", "#reseterror").html('用戶名只能允許英文字母或數字');
            $("#reseterror").popup("open");
            return;
        }
        var data = { user: $('#user').val(), email: $('#email').val(), database: db };
        $('.logon').hide();
        $.ajax({
            type: 'post',
            dataType: 'text',
            url: '../handler/SystemHandle.ashx?type=resetUserP',
            data: data,
            async: true,
            cache: false,
            success: function () {
                //$("p", "#reseterror").html('帳號密碼已經重置，請登入Email取得新的密碼');
                //$("#reseterror").popup("open");
                //$.mobile.changePage("../MobileLogOn.aspx", { transition: "slideup", role: "page" });
                alert('帳號密碼已經重置，請登入Email取得新的密碼');
                window.location.href = '../MobileLogOn.aspx';
            },
            error: function (data) {
                //var excection = $.parseJSON(data.responseText);
                var exception = {};
                exception.message = $.getErrorMessage(data.responseText);
                if (exception.message == 'EmailError') {
                    $("p", "#reseterror").html('輸入的Email地址不正確');
                    $("#reseterror").popup("open");
                }
                else if (exception.message == 'UserNotFound') {
                    $("p", "#reseterror").html('該用戶不存在');
                    $("#reseterror").popup("open");
                }
                else {
                    $("p", "#reseterror").html(exception.message);
                    $("#reseterror").popup("open");
                }
                data.responseText = '';
            },
            complete: function () {
                $('.logon').show();
            }
        });
    }
}

$.fn.main = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.main.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            $(this).main('initialize');
            if (!$(this).hasClass($.fn.main.class)) {
                $(this).addClass($.fn.main.class)
            }
        });
    }
};
//--------------------------------------------------------------------------------------------------------------------------------
$.fn.main.class = 'info-main';

$.fn.main.defaults = {
    refreshText: "Refresh",
    logoutText: "Logout",
    menuText: "Menu",
    flowText: "Flow",
    placeholderText: "Search menu"
};

$.fn.main.methods = {
    initialize: function (jq) {
        if (jq.length > 0) {
            //load menu
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: $.getSystemUrl(),
                data: "mode=getMenus",
                cache: false,
                async: false,
                success: function (data) {
                    var parentMenus = [];
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].PARENT == '' || data[i].PARENT == null) {
                            parentMenus.push(data[i]);
                        }
                    }
                    for (var i = 0; i < parentMenus.length; i++) {
                        var childMenus = [];
                        for (var j = 0; j < data.length; j++) {
                            if (data[j].PARENT == parentMenus[i].MENUID) {
                                childMenus.push(data[j]);
                            }
                        }
                        if (childMenus.length > 0) {
                            $("#menu", jq[0]).append($.createListDivide(parentMenus[i].CAPTION));
                            for (var j = 0; j < childMenus.length; j++) {
                                var url = childMenus[j].PACKAGE + '/' + childMenus[j].FORM + '.aspx';
                                if (childMenus[j].ITEMPARAM != null && childMenus[j].ITEMPARAM != "") {
                                    url += "?" + encodeURI(childMenus[j].ITEMPARAM);
                                }
                                var ccMenus = [];
                                for (var k = 0; k < data.length; k++) {
                                    if (data[k].PARENT == childMenus[j].MENUID) {
                                        ccMenus.push(data[k]);
                                    }
                                }
                                if (ccMenus.length == 0) {
                                    $("#menu", jq[0]).append($.createListItem(childMenus[j].CAPTION, url, childMenus[j].IMAGEURL));
                                }
                                else {
                                    var html = '<li data-role="collapsible" data-iconpos="right" data-inset="false">';
                                    html += '<h2>' + childMenus[j].CAPTION + '</h2>';
                                    html += '<ul data-role="listview" data-theme="b">';
                                    for (var k = 0; k < ccMenus.length; k++) {
                                        url = ccMenus[k].PACKAGE + '/' + ccMenus[k].FORM + '.aspx';
                                        if (ccMenus[k].ITEMPARAM != null && ccMenus[k].ITEMPARAM != "") {
                                            url += "?" + encodeURI(ccMenus[k].ITEMPARAM);
                                        }
                                        html += $.createListItem(ccMenus[k].CAPTION, url, ccMenus[k].IMAGEURL)
                                    }
                                    html += '</ul></li>';
                                    $(html).appendTo($("#menu", jq[0])).collapsible({}).trigger('create');
                                }
                            }
                        }
                        else {
                            if (parentMenus[i].PACKAGE != '' && parentMenus[i].PACKAGE != null) {
                                var url = parentMenus[i].PACKAGE + '/' + parentMenus[i].FORM + '.aspx';
                                if (parentMenus[i].ITEMPARAM != null && parentMenus[i].ITEMPARAM != "")
                                    url += "?" + encodeURI(parentMenus[i].ITEMPARAM);
                                $("#menu", jq[0]).append($.createListItem(parentMenus[i].CAPTION, url, parentMenus[j].IMAGEURL));
                            }
                        }
                    }
                    $("#menu", jq[0]).listview('refresh');
                },
                error: function (data) {
                    window.location.href = "MobileLogOn.aspx";
                    //                    var obj = $.mobile.path.parseUrl(window.location.href);
                    //                    //alert(obj.domain);
                    //                    window.location.href = obj.domain + obj.directory + "MobileLogOn.aspx";
                }
            });
            //language
            $("a.refresh").attr("title", $.fn.main.defaults.refreshText);
            $("a.logout").attr("title", $.fn.main.defaults.logoutText);

            $("a.menu .ui-btn-text").html($.fn.main.defaults.menuText);
            $("a.flow .ui-btn-text").html($.fn.main.defaults.flowText);
            if ($("a.menu .ui-btn-text").length == 0) {
                $("a.menu").html($.fn.main.defaults.menuText);
            }
            if ($("a.flow .ui-btn-text").length == 0) {
                $("a.flow").html($.fn.main.defaults.flowText);
            }
            $("#menu", jq[0]).prev().find(".ui-input-search>input").attr("placeholder", $.fn.main.defaults.placeholderText);
        }
    },
    logout: function (jq) {
        $.ajax({
            type: "POST",
            url: $.getSystemUrl(),
            data: "mode=logout",
            cache: false,
            async: true,
            success: function (data) {

            }
        });
        $.mobile.changePage("MobileLogOn.aspx" + location.search, { transition: "slidedown", role: "page" });
    }
}

function showLoading() {
    $.mobile.loading('show', { theme: 'b', text: '', textVisible: false });
}

//--------------------------------------------------------------------------------------------------------------------------------
//$(document).ready(function () {
//    $("." + $.fn.datagrid.class).each(function () {
//        $(this).datagrid({});
//    });
//    $("." + $.fn.form.class).each(function () {
//        $(this).form({});
//    });
//    $("." + $.fn.query.class).each(function () {
//        $(this).query({});
//    });
//});
var xmllanguage = "ENG";
$.extend({
    languageInitialized: false,
    EEPTimeOutMessage: 'Timeout, relogon please',
    EEPNotRegisterMessage: 'EEP Mobile is not registered.',
    initLanguage: function (target) {
        var currentLang = navigator.browserLanguage ? navigator.browserLanguage : navigator.language;
        var xmlFile = "../sysmsg.mobile.xml";
        var logonFile = "../MobileLogOn.aspx";
        if ($("." + $.fn.logon.class, target).length > 0 || $("." + $.fn.main.class, target).length > 0) {
            xmlFile = "sysmsg.mobile.xml";
            logonFile = "MobileLogOn.aspx";
        }
        else if ($.fn.flow && $("." + $.fn.flow.class, target).length > 0) {
            xmlFile = "sysmsg.mobile.xml";
            logonFile = "MobileLogOn.aspx";
        }

        var language = {};
        language.current = "GetCurrentLanguage";
        $.ajax({
            type: "POST",
            dataType: 'json',
            url: logonFile,
            data: "mode=default&method=" + $.toJSONString(language),
            cache: false,
            async: false,
            success: function (data) {
                currentLang = data.current;
            }, error: function (data) {
            }
        });
        switch (currentLang.toLowerCase()) {
            case 'zh-hk': xmllanguage = 'HKG'; break;
            case 'zh-hans-cn': xmllanguage = 'SIM'; break;
            case 'zh-cn': xmllanguage = 'SIM'; break;
            case 'zh-hant-tw': xmllanguage = 'TRA'; break;
            case 'zh-tw': xmllanguage = 'TRA'; break;
            default: xmllanguage = 'ENG'; break;
        }
        $.ajax({
            url: xmlFile,
            dataType: 'xml',
            type: 'GET',
            timeout: 3000,
            async: false,
            success: function (xml) {
                $.languageInitialized = true;
                var elements = [];
                elements.push($.fn.logon);
                elements.push($.fn.main);
                elements.push($.fn.datagrid);
                elements.push($.fn.form);
                elements.push($.fn.query);
                elements.push($.fn.validate);
                elements.push($.fn.file);
                if ($.fn.flow) {
                    elements.push($.fn.flow);
                }
                for (var i = 0; i < elements.length; i++) {
                    var element = elements[i];
                    var elementNode = $(xml).find(element.class);
                    if (elementNode.length > 0) {
                        for (var property in element.defaults) {
                            var textNode = elementNode.find(property).find(xmllanguage);
                            if (textNode.length > 0) {
                                element.defaults[property] = textNode.text();
                            }
                        }
                    }
                }
            },
            error: function (xml) {
            }
        });
    },
    parseOption: function (optionString) {   //option is not JSON format
        var option = new Object();
        if (optionString != undefined) {
            var op = '';
            var options = optionString.split(',');
            for (var i = 0; i < options.length; i++) {
                if (op.length > 0) {
                    op += ',';
                }
                op += options[i];
                if (op.split('{').length != op.split('}').length) {
                    continue;
                }
                if (op.split('[').length != op.split(']').length) {
                    continue;
                }
                if (op.split("'").length % 2 == 0) {
                    continue;
                }

                var index = op.indexOf(':');
                if (index > 0) {
                    var pname = op.substr(0, index).replace(/(^\s*)|(\s*$)/g, "");
                    var pvalue = op.substr(index + 1);
                    if (pvalue.length > 0 && pvalue[0] == '{' && pvalue[pvalue.length - 1] == '}') {
                        if (pvalue.length == 2) {
                            option[pname] = new Object();
                        }
                        else {
                            option[pname] = $.parseOption(pvalue.substr(1, pvalue.length - 2));
                        }
                    }
                    else {
                        option[pname] = eval(pvalue);
                    }
                }
                op = '';
            }
        }
        return option;
    },
    getSystemUrl: function () {
        return "handler/JQMobileHandler.ashx";
    },
    getUrl: function (remoteName, tableName, noCount) {
        var param = '';
        if (Request.getQueryStringByName('p')) {
            param += '&p=' + Request.getQueryStringByName('p');
        }
        if (Request.getQueryStringByName('publicKey')) {
            param += '&publicKey=' + Request.getQueryStringByName('publicKey');
        }
        if (noCount) {
            return "../handler/jqDataHandle.ashx?RemoteName=" + remoteName + "&TableName=" + tableName + "&IncludeRows=false" + param;
        }
        else {
            return "../handler/jqDataHandle.ashx?RemoteName=" + remoteName + "&TableName=" + tableName + "&IncludeRows=true" + param;
        }
    },
    getUploadUrl: function () {
        return "../handler/UploadHandler.ashx";
    },
    getErrorMessage: function (html) {
        if (html == '' || html == null) {
            return '';
        }
        var startIndex = html.indexOf('<title>');
        var endIndex = html.indexOf('</title>');
        if (startIndex > 0 && endIndex > startIndex + 7) {
            return html.substring(startIndex + 7, endIndex);
        }
    },
    createTextButton: function (text, theme, type) {
        return "<a class='" + type + "' data-mini='true' data-role='button' data-theme='" + theme + "'>" + text + "</a>"
    },
    createButton: function (text, icon, theme, type) {
        return "<a class='" + type + "' data-mini='true' data-inline='true' data-role='button' data-icon='" + icon + "' data-iconpos='notext' data-theme='" + theme + "'>" + text + "</a>"
    },
    createOption: function (text, value) {
        return "<option value='" + value + "'>" + text + "</option>"
    },
    createRadioButton: function (text, value, id, name) {
        return "<input id='" + id + "' name='" + name + "' type='radio' value='" + value + "' /><label for='" + id + "'>" + text + "</label>";
    },
    createCheckbox: function (text, value, id, name) {
        return "<input id='" + id + "' name='" + name + "' type='checkbox' value='" + value + "' /><label for='" + id + "'>" + text + "</label>";
    },
    createListDivide: function (text) {
        return "<li data-role='list-divider'><a>" + text + "</a></li>"
    },
    createListItem: function (text, url, imageUrl) {
        if (imageUrl) {
            imageUrl = 'Image/MenuTree/' + imageUrl;
            return "<li><a href='" + url + "' data-transition='slide' rel='external' onclick='showLoading()'><img src='" + imageUrl + "' alt=" + text + " class='ui-li-icon'>" + text + "</a></li>";
        }
        else {
            return "<li><a href='" + url + "' data-transition='slide' rel='external' onclick='showLoading()'>" + text + "</a></li>"; //include multipage
        }
    },
    utf16to8: function (str) {
        var out, i, len, c;
        out = "";
        len = str.length;
        for (i = 0; i < len; i++) {
            c = str.charCodeAt(i);
            if ((c >= 0x0001) && (c <= 0x007F)) {
                out += str.charAt(i);
            } else if (c > 0x07FF) {
                out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));
                out += String.fromCharCode(0x80 | ((c >> 6) & 0x3F));
                out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
            } else {
                out += String.fromCharCode(0xC0 | ((c >> 6) & 0x1F));
                out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
            }
        }
        return out;
    },
    getFormatedValue: function (val, format, param) {
        var index = format.indexOf('-');
        if (index == 1 && format.length > 2) {
            var formatType = format.substr(0, 1).toLowerCase();
            var formatString = format.substr(2);
            //string
            if (formatType == 's') {
                var newStr = '';
                for (var i = 0; i < val.length && i < formatString.length; i++) {
                    if (formatString[i] == '?') {
                        newStr += val[i];
                    }
                    else {
                        newStr += formatString[i];
                    }
                }
                return newStr;
            }
            //logical
            else if (formatType == 'l') {
                if (formatString == 'checkbox') {
                    if (val.toString().toLowerCase() == 'true' || val.toString().toLowerCase() == 'y' || val.toString() == '1') {
                        return "<input  data-role='none' type='checkbox' checked='true' disabled='true' />";
                    }
                    else {
                        return "<input  data-role='none' type='checkbox'  disabled='true' />";
                    }
                }
                else {
                    var formats = formatString.split('-');
                    if (val.toString().toLowerCase() == 'true' || val.toString().toLowerCase() == 'y' || val.toString() == '1') {
                        return formats[0];
                    }
                    else {
                        return formats[1];
                    }
                }
            }
            //number
            else if (formatType == 'n') {
                var numString = val.toString().split('.');
                var newNum = '';
                for (var i = numString[0].length - 1; i >= 0; i--) {
                    newNum = numString[0][i] + newNum;
                    if (i > 0 && ((numString[0].length - i) % 3 == 0)) {
                        newNum = ',' + newNum;
                    }
                }
                if (numString.length == 2) {
                    if (formatString.length >= 2) {
                        var len = parseInt(formatString.substring(1));
                        if (len > 0) {
                            newNum += '.';
                        }
                        for (var i = 0; i < len && i < numString[1].length; i++) {
                            newNum += numString[1][i];
                        }
                    }
                    else {
                        newNum += '.' + numString[1];
                    }
                }
                else {
                    if (formatString.length >= 2) {
                        var len = parseInt(formatString.substring(1));
                        if (len > 0) {
                            newNum += '.';
                        }
                        for (var i = 0; i < len; i++) {
                            newNum += '0';
                        }
                    }
                }
                if (formatString[0] == 'C' && formatString.toLowerCase() != "call") {
                    newNum = '$' + newNum;
                }
                return newNum
            }
            //datetime
            else if (formatType == 'd') {
                var date = new Date(val);
                if (date == "Invalid Date" && val.length == 8) {
                    var s = val.toString().substr(0, 4) + '-' + val.toString().substr(4, 2) + '-' + val.toString().substr(6, 2);
                    date = new Date(s);
                }
                if (date != "Invalid Date") {
                    var year = date.getFullYear();
                    var month = (date.getMonth() + 1).toString();
                    if (month.length == 1) {
                        month = '0' + month;
                    }
                    var date = date.getDate().toString();
                    if (date.length == 1) {
                        date = '0' + date;
                    }
                    var YYY = (year - 1911).toString();
                    if (YYY.length == 1) {
                        YYY = '00' + YYY;
                    }
                    else if (YYY.length == 2) {
                        YYY = '0' + YYY;
                    }

                    var YY = ((year - 1911) % 100).toString();
                    if (YY.length == 1) {
                        YY = '0' + YY;
                    }
                    var yy = (year % 100).toString();
                    if (yy.length == 1) {
                        yy = '0' + yy;
                    }
                    return formatString.replace(/yyyy/g, year).replace(/yy/g, yy).replace(/mm/g, month).replace(/dd/g, date).replace(/YYY/g, YYY).replace(/YY/g, YY);
                }
                else {
                    return val;
                }
            }
            //refval
            else if (formatType == 'r') {
                if (val.toString() == '') {
                    return '';
                }
                var formats = formatString.split('-');
                if (formats.length == 1) {
                    var items = param;
                    for (var i = 0; i < items.length; i++) {
                        if (items[i].value.toString() == val.toString()) {
                            val = items[i].text.toString();
                            break;
                        }
                    }
                }
                else {
                    var remoteName = formats[0];
                    var tableName = formats[1];
                    var displayMember = formats[2];
                    var valueMember = formats[3];
                    if (displayMember != valueMember) {
                        var queryWord = new Object();
                        queryWord.whereString = valueMember + " = '" + val.toString().replace(/\'/g, "''") + "'";
                        if (param != undefined && param != '') { //whereItem
                            queryWord.whereString += " and " + param;
                        }
                        $.ajax({
                            type: "POST",
                            dataType: 'json',
                            url: $.getUrl(remoteName, tableName, true),
                            data: 'queryWord=' + $.toJSONString(queryWord),
                            cache: false,
                            async: false,
                            success: function (data) {
                                if (data.length > 0) {
                                    val = data[0][displayMember];
                                }
                                else {
                                    val = '';
                                }
                            }, error: function (data) {
                                val = '';
                            }
                        });
                    }
                }
                return val;
            }
            else if (formatType == 'i') {
                if (val != undefined && val != "") {
                    var formats = formatString.split('-');
                    var directory = formats[0];
                    var height = formats[1];

                    var path = "../";
                    var developer = $('#_DEVELOPERID').val();
                    if (developer) {
                        path += 'preview' + developer + '/';
                    }
                    if (directory != undefined && directory != '') {
                        path += directory + "/";
                    }
                    path += val;
                    var popupID = val.split('.')[0] + "_image_popup";
                    return '<a href="#' + popupID + '" data-rel="popup" data-position-to="window" data-transition="fade">'
                    + '<img height="' + height + '" src="' + path + '" /></a>'
                    + '<div data-role="popup" id="' + popupID + '" data-overlay-theme="a" data-theme="d" data-corners="false">'
                    + '<a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete" data-iconpos="notext" class="ui-btn-right">Close</a>'
                    + '<img class="popphoto" src="' + path + '"></div>';
                }
                else return val;
            }
            else if (formatType == 'a') {
                var formats = formatString.split('-');
                var directory = formats[0];
                var path = "../";
                var developer = $('#_DEVELOPERID').val();
                if (developer) {
                    path += 'preview' + developer + '/';
                }
                if (directory != undefined && directory != '') {
                    path += directory + "/";
                }
                path += val;
                return "<a href='" + path + "' target='_blank' rel='external'>" + val + "</a>";
            }
            else if (formatType == 'q') {
                var formats = formatString.split('-');
                var size = formats[0];
                return '<div class="grid-qrcode" style="display:inline" value="' + val + '" size="' + size + '"></div>';
            }
            else if (formatType == "m") {
                var formats = formatString.split('-');
                var height = formats[0];
                //return '<div data-mini="true" infolight-options="enableHighAcuracy:false,geolocation:false,address:true" class="info-map" data-role="none">';
                return '<div infolight-options="height:' + height + ',width:' + height + ',enableHighAcuracy:false,geolocation:false,address:true" class="grid-geomap" value="' + val + '">';

            }
            else if (formatType == "w") {
                var formats = formatString.split('-');
                var objectID = formats[0];
                var fields = formats[1];
                var fieldstring = fields.split(";");
                var drillfields = "";
                for (var i = 0; i < fieldstring.length; i++) {
                    if (drillfields != "") drillfields += ",";
                    drillfields += "{field:'" + fieldstring[i] + "',value:'" + param[fieldstring[i]] + "'}";
                }

                var aval = '<a href="#" infolight-options="drillobjectid:\'' + objectID + '\',drillfields:[' + drillfields + ']" onclick="$(this).drilldown(\'load\');">' + val + '</a>';
                return aval;
            }
        }

        //兼容jquery web格式
        if (format.toLowerCase().indexOf('yy') >= 0) {
            return $.getFormatedValue(val, "D-" + format);
        }
        else if (format.indexOf('?') >= 0) {
            return $.getFormatedValue(val, "S-" + format);
        }
        else if ((format.toLowerCase().indexOf('n') == 0 || format.toLowerCase().indexOf('c') == 0) && format.toLowerCase() != "call") {
            return $.getFormatedValue(val, "N-" + format);
        }
        else if (format.toLowerCase().indexOf('image,') == 0) {
            if (val != undefined && val != "") {
                var formats = format.toLowerCase().split(',');
                var folder = '';
                var height = '80';
                for (var i = 0; i < formats.length; i++) {
                    if (formats[i].indexOf('folder:') == 0) {
                        folder = formats[i].replace('folder:', '');
                    }
                    else if (formats[i].indexOf('height:') == 0) {
                        height = formats[i].replace('height:', '');
                    }
                }
                return $.getFormatedValue(val, "I-" + folder + "-" + height);
            }
            else return val;
        }
        else if (format.toLowerCase().indexOf('download,') == 0) {
            var formats = format.toLowerCase().split(',');
            var folder = '';
            for (var i = 0; i < formats.length; i++) {
                if (formats[i].indexOf('folder:') == 0) {
                    folder = formats[i].replace('folder:', '');
                }
            }
            return $.getFormatedValue(val, "A-" + folder);
        }
        else if (format.toLowerCase().indexOf('qrcode,') == 0) {
            var formats = format.toLowerCase().split(',');
            var size = '120';
            for (var i = 0; i < formats.length; i++) {
                if (formats[i].indexOf('size:') == 0) {
                    size = formats[i].replace('size:', '');
                }
            }
            return $.getFormatedValue(val, "Q-" + size);
        }
        else if (format.toLowerCase() == 'l,checkbox') {
            return $.getFormatedValue(val, "L-checkbox");
        }
        else if (format.toLowerCase().indexOf('map,') == 0) {
            var formats = format.toLowerCase().split(',');
            var height = 80;
            var address = true;
            for (var i = 0; i < formats.length; i++) {
                if (formats[i].indexOf('height:') == 0) {
                    height = formats[i].replace('size:', '');
                }
                if (formats[i].indexOf('geo:') == 0) {
                    address = formats[i].replace('geo:', '') == "true" ? false : true;
                }
            }
            return '<div infolight-options="height:' + height + ',enableHighAcuracy:false,geolocation:false,address:' + address + '" class="grid-geomap" value="' + val + '">';
        }
        else if (format.toLowerCase().indexOf('drilldown,') == 0) {
            var objectID = "";
            var fields = "";
            var optionss = format.split(',');
            for (var i = 0; i < optionss.length; i++) {
                if (optionss[i].split(":").length == 2) {
                    var pname = optionss[i].split(":")[0];
                    var pvalue = optionss[i].split(":")[1];
                    if (pname.toLowerCase() == "drillobjectid") {
                        objectID = pvalue;
                    }
                    if (pname.toLowerCase() == "drillfields") {
                        fields = pvalue;
                    }
                }
            }
            var fieldstring = fields.split(";");
            var drillfields = "";
            for (var i = 0; i < fieldstring.length; i++) {
                if (drillfields != "") drillfields += ",";
                drillfields += "{field:'" + fieldstring[i] + "',value:'" + param[fieldstring[i]] + "'}";
            }

            var aval = '<a href="#" infolight-options="drillobjectid:\'' + objectID + '\',drillfields:[' + drillfields + ']" onclick="$(this).drilldown(\'load\');">' + val + '</a>';
            return aval;
        }
        else if (format.toLowerCase().indexOf("signature") == 0) {
            var sheight = "";
            var sformat = "";
            var optionss = format.split(',');
            for (var i = 0; i < optionss.length; i++) {
                if (optionss[i].split(":").length == 2) {
                    var pname = optionss[i].split(":")[0];
                    var pvalue = optionss[i].split(":")[1];
                    if (pname.toLowerCase() == "height") {
                        sheight = pvalue;
                    }
                    if (pname.toLowerCase() == "format") {
                        sformat = pvalue;
                    }
                }
            }

            var aval = '<div infolight-options="height:' + sheight + ',format:\'' + sformat + '\'" class="grid-signature" value="' + val + '" style="height:' + sheight + 'px" />';
            return aval;
        }

        return val;
    },
    createUploadIframe: function (id, uri) {
        //create frame
        var frameId = 'jUploadFrame' + id;
        var iframeHtml = '<iframe id="' + frameId + '" name="' + frameId + '" style="position:absolute; top:-9999px; left:-9999px"';
        if (window.ActiveXObject) {
            if (typeof uri == 'boolean') {
                iframeHtml += ' src="' + 'javascript:false' + '"';

            }
            else if (typeof uri == 'string') {
                iframeHtml += ' src="' + uri + '"';

            }
        }
        iframeHtml += ' />';
        jQuery(iframeHtml).appendTo(document.body);

        return jQuery('#' + frameId).get(0);
    },

    createUploadForm: function (id, fileElementId, data) {
        //create form	
        var formId = 'jUploadForm' + id;
        var fileId = 'jUploadFile' + id;
        var form = jQuery('<form action="" method="POST" name="' + formId + '" id="' + formId + '" enctype="multipart/form-data"></form>');
        if (data) {
            for (var i in data) {
                jQuery('<input type="hidden" name="' + i + '" value="' + data[i] + '" />').appendTo(form);
            }
        }
        var oldElement = jQuery('#' + fileElementId);
        var newElement = jQuery(oldElement).clone();
        jQuery(oldElement).attr('id', fileId);
        newElement.data("mobile-textinput", oldElement.data("mobile-textinput"));
        newElement.data("options", oldElement.data("options"));
        jQuery(oldElement).before(newElement);
        jQuery(oldElement).appendTo(form);



        //set attributes
        jQuery(form).css('position', 'absolute');
        jQuery(form).css('top', '-1200px');
        jQuery(form).css('left', '-1200px');
        jQuery(form).appendTo('body');
        return form;
    },

    ajaxFileUpload: function (s) {
        // TODO introduce global settings, allowing the client to modify them for all requests, not only timeout		
        s = jQuery.extend({}, jQuery.ajaxSettings, s);
        var id = new Date().getTime();
        var form = jQuery.createUploadForm(id, s.fileElementId, (typeof (s.data) == 'undefined' ? false : s.data));
        var io = jQuery.createUploadIframe(id, s.secureuri);
        var frameId = 'jUploadFrame' + id;
        var formId = 'jUploadForm' + id;
        // Watch for a new set of requests
        if (s.global && !jQuery.active++) {
            jQuery.event.trigger("ajaxStart");
        }
        var requestDone = false;
        // Create the request object
        var xml = {};
        if (s.global)
            jQuery.event.trigger("ajaxSend", [xml, s]);
        // Wait for a response to come back
        var uploadCallback = function (isTimeout) {
            var io = document.getElementById(frameId);
            try {
                if (io.contentWindow) {
                    xml.responseText = io.contentWindow.document.body ? io.contentWindow.document.body.innerHTML : null;
                    xml.responseXML = io.contentWindow.document.XMLDocument ? io.contentWindow.document.XMLDocument : io.contentWindow.document;

                } else if (io.contentDocument) {
                    xml.responseText = io.contentDocument.document.body ? io.contentDocument.document.body.innerHTML : null;
                    xml.responseXML = io.contentDocument.document.XMLDocument ? io.contentDocument.document.XMLDocument : io.contentDocument.document;
                }
            } catch (e) {
                jQuery.handleError(s, xml, null, e);
            }
            if (xml || isTimeout == "timeout") {
                requestDone = true;
                var status;
                try {
                    status = isTimeout != "timeout" ? "success" : "error";
                    // Make sure that the request was successful or notmodified
                    if (status != "error") {
                        // process the data (runs the xml through httpData regardless of callback)
                        var data = jQuery.uploadHttpData(xml, s.dataType);
                        // If a local callback was specified, fire it and pass it the data
                        if (s.success)
                            s.success(data, status);

                        // Fire the global callback
                        if (s.global)
                            jQuery.event.trigger("ajaxSuccess", [xml, s]);
                    } else
                        s.error(status);
                } catch (e) {
                    status = "error";
                    s.error(e, status);
                }

                // The request was completed
                if (s.global)
                    jQuery.event.trigger("ajaxComplete", [xml, s]);

                // Handle the global AJAX counter
                if (s.global && ! --jQuery.active)
                    jQuery.event.trigger("ajaxStop");

                // Process result
                if (s.complete)
                    s.complete(xml, status);

                jQuery(io).unbind();

                setTimeout(function () {
                    try {
                        jQuery(io).remove();
                        jQuery(form).remove();

                    } catch (e) {
                        jQuery.handleError(s, xml, null, e);
                    }

                }, 100);

                xml = null;

            }
        };
        // Timeout checker
        if (s.timeout > 0) {
            setTimeout(function () {
                // Check to see if the request is still happening
                if (!requestDone) uploadCallback("timeout");
            }, s.timeout);
        }
        try {

            var form = jQuery('#' + formId);
            jQuery(form).attr('action', s.url);
            jQuery(form).attr('method', 'POST');
            jQuery(form).attr('target', frameId);
            if (form.encoding) {
                jQuery(form).attr('encoding', 'multipart/form-data');
            }
            else {
                jQuery(form).attr('enctype', 'multipart/form-data');
            }
            jQuery(form).submit();

        } catch (e) {
            jQuery.handleError(s, xml, null, e);
        }

        jQuery('#' + frameId).load(uploadCallback);
        return { abort: function () { } };

    },
    uploadHttpData: function (r, type) {
        var data = !type;
        data = type == "xml" || data ? r.responseXML : r.responseText;
        // If the type is "script", eval it in global context
        if (type == "script")
            jQuery.globalEval(data);
        // Get the JavaScript object, if JSON is used.
        if (type == "json") {
            data2 = data.toLowerCase();
            if (data2.indexOf('<pre') != -1) {
                //此段代码时为了兼容火狐和chrome浏览器
                var newDiv = jQuery(document.createElement("div"));
                newDiv.html(data2);
                data = $("pre:first", newDiv).html();
            }
            eval("data = " + data);
        }
        // evaluate scripts within html
        if (type == "html")
            jQuery("<div>").html(data).evalScripts();
        return data;
    }
});
//--------------------------------------------------------------------------------------------------------------------------------
$.fn.datagrid = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.datagrid.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).datagrid('initialize', options);
            if (!$(this).hasClass($.fn.datagrid.class)) {
                $(this).addClass($.fn.datagrid.class)
            }
        });
    }
};

$.fn.datagrid.class = 'info-datagrid';

$.fn.datagrid.defaults = {
    pageNumber: 1,
    pageSize: 10,
    insertText: "Insert",
    updateText: "Update",
    deleteText: "Delete",
    detailText: "Detail",
    viewText: "View",
    previousPageText: "Previous page",
    nextPageText: "Next page",
    queryText: "Query",
    refreshText: "Refresh",
    backText: "Back",
    confirmDeleteText: "Sure to delete data?",
    loadingMessage: "Loading data",
    updatingMessage: "Updating data"
};

$.fn.datagrid.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var gridOptions = new Object();
            gridOptions.pageNumber = $.fn.datagrid.defaults.pageNumber;      //default option
            gridOptions.pageSize = $.fn.datagrid.defaults.pageSize;

            var htmlOptions = $.parseOption($(this).attr('infolight-options'));   //html option
            for (var property in htmlOptions) {
                gridOptions[property] = htmlOptions[property];
            }
            var columns = [];
            $("th", this).each(function () {
                var columnOptions = $(this).attr('infolight-options');
                if (columnOptions != undefined) {
                    var column = $.parseOption(columnOptions);
                    column.caption = $(this).html();
                    columns.push(column);

                    //$(this).removeAttr('infolight-options')
                }
            });

            gridOptions.columns = columns;
            if (options != undefined) {                                     //load option
                for (var property in options) {
                    gridOptions[property] = options[property];
                }
            }
            $(this).data('options', gridOptions);

            var title = gridOptions.title;
            if (title != undefined) {
                $(this).datagrid('pageObject').find("h1").html(title);
            }

            if (gridOptions.alwaysClose) {
                $(this).datagrid('options').whereString = '1=0';
            }

            if (gridOptions.queryMode == 'panel' || gridOptions.queryMode == 'fuzzy') {
                $(this).datagrid('openQuery');
            }

            var parentObjectID = gridOptions.parentObjectID;
            if (parentObjectID == undefined) {
                $(this).datagrid('load');
            }
        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('options');
        }
        return new Object();
    },
    getColumnFields: function (jq) {
        if (jq.length > 0) {
            var columnFields = [];
            var options = $(jq[0]).datagrid('options');
            for (var i = 0; i < options.columns.length; i++) {
                columnFields.push(options.columns[i].field);
            }
            return columnFields;
        }
        return [];
    },
    getColumnOption: function (jq, field) {
        if (jq.length > 0) {
            var options = $(jq[0]).datagrid('options');
            for (var i = 0; i < options.columns.length; i++) {
                if (field == options.columns[i].field) {
                    return options.columns[i];
                }
            }
            return null;
        }
        return new Object();
    },
    pageObject: function (jq) {
        if (jq.length > 0) {
            var page = $(jq[0]).closest("div[data-role='page']");
            if (page.length > 0) {
                return page;
            }
            else {
                return $(jq[0]).closest("div[data-role='dialog']");
            }
        }
        return Object();
    },
    resize: function (jq) {
        jq.each(function () {
            var datagrid = $(this);
            $("tbody tr td", this).each(function () {
                var field = $(this).attr('field');
                if (field != undefined) {
                    if ($(window).width() > 540) {
                        var columnOption = datagrid.datagrid('getColumnOption', field);
                        //$(this).attr('style', 'width:' + columnOption.width + 'px');
                        $(this).width(columnOption.width);
                        var align = columnOption.align;
                        if (align) {
                            $(this).css('text-align', align);
                        }
                    }
                    else {
                        //$(this).attr('style', 'width:auto');
                        $(this).width('auto');
                        $(this).css('text-align', 'left');
                    }
                }
            });
        });
    },
    createToolItem: function (jq, data) {
        jq.each(function () {
            $(this).siblings(".grid-header").remove();
            $(this).siblings(".grid-footer").remove();
            var columnFields = $(this).datagrid('getColumnFields');
            var toolItems = '';
            var toolItemObjectID = $(this).datagrid('options').toolItemObjectID;
            if (toolItemObjectID != undefined) {
                toolItems = $(toolItemObjectID).html();
            }

            var toolItemsPosition = $(this).datagrid('options').toolItemsPosition;
            if (toolItemsPosition == "top" || toolItemsPosition == "both") {
                //add header
                $(this).before("<div class='grid-header'><div>");
                $(this).prev(".grid-header").append(toolItems);
                $(this).prev(".grid-header").find("a").attr('data-role', 'button');
                $(this).prev(".grid-header").trigger("create");
            }
            if (toolItemsPosition == undefined || toolItemsPosition == "bottom" || (toolItemsPosition == "both" && data.length > 0)) {
                //add footer
                $(this).after("<div class='grid-footer'><div>");
                $(this).next(".grid-footer").append(toolItems);
                $(this).next(".grid-footer").find("a").attr('data-role', 'button');
                $(this).next(".grid-footer").trigger("create");
            }
            $("a.grid-insert[data-role='button']", $(this).parent()).each(function () {
                $(this).attr('title', $.fn.datagrid.defaults.insertText);
                var grid = $(this).parent().siblings('.info-datagrid');
                var allowAdd = grid.datagrid('options').allowAdd;
                if (allowAdd == false) {
                    $(this).hide();
                }
                else {
                    $(this).bind('click', function () {
                        var grid = $(this).parent().siblings('.info-datagrid');
                        grid.datagrid('insertRow');
                    });
                }
            });

            $("a.grid-previous[data-role='button']", $(this).parent()).each(function () {
                $(this).attr('title', $.fn.datagrid.defaults.previousPageText);
                $(this).bind('click', function () {
                    var grid = $(this).parent().siblings('.info-datagrid');
                    grid.datagrid('previousPage');
                });
            });
            $("a.grid-next[data-role='button']", $(this).parent()).each(function () {
                $(this).attr('title', $.fn.datagrid.defaults.nextPageText);
                $(this).bind('click', function () {
                    var grid = $(this).parent().siblings('.info-datagrid');
                    grid.datagrid('nextPage');
                });
            });
            $("a.grid-query[data-role='button']", $(this).parent()).each(function () {
                $(this).attr('title', $.fn.datagrid.defaults.queryText);
                var grid = $(this).parent().siblings('.info-datagrid');
                var queryObjectID = grid.datagrid('options').queryObjectID;
                var queryMode = grid.datagrid('options').queryMode;
                if (queryObjectID == undefined || queryMode == 'panel') {
                    $(this).hide();
                }
                else {
                    $(this).bind('click', function () {
                        var grid = $(this).parent().siblings('.info-datagrid');
                        grid.datagrid('openQuery');
                    });
                }
            });
            $("a.grid-refresh[data-role='button']", $(this).parent()).each(function () {
                $(this).attr('title', $.fn.datagrid.defaults.refreshText);
                $(this).bind('click', function () {
                    var grid = $(this).parent().siblings('.info-datagrid');
                    grid.datagrid('load');
                });

            });
            $("a.grid-export[data-role='button']", $(this).parent()).each(function () {
                //$(this).attr('title', $.fn.datagrid.defaults.refreshText);
                $(this).bind('click', function () {
                    var grid = $(this).parent().siblings('.info-datagrid');
                    grid.datagrid('export');
                });
            });
            $("a.grid-exportreport[data-role='button']", $(this).parent()).each(function () {
                //$(this).attr('title', $.fn.datagrid.defaults.refreshText);
                $(this).bind('click', function () {
                    var grid = $(this).parent().siblings('.info-datagrid');
                    grid.datagrid('exportReport', grid.datagrid('options').reportFileName);
                });
            });
            $("a.grid-return[data-role='button']", $(this).parent()).each(function () {
                $(this).attr('title', $.fn.datagrid.defaults.backText);
                var grid = $(this).parent().siblings('.info-datagrid');
                var parentObjectID = grid.datagrid('options').parentObjectID;
                if (parentObjectID == undefined) {
                    $(this).hide();
                }
                else {
                    $(this).bind('click', function () {
                        var grid = $(this).parent().siblings('.info-datagrid');
                        var parentPage = grid.datagrid('options').parentObjectID;
                        $.mobile.changePage($(parentPage), { transition: 'pop', role: 'page', transition: 'slide', reverse: true });
                    });
                }
            });
        });
    },
    loadData: function (jq, data) {
        jq.each(function () {
            $("tbody", this).remove();
            $("th.commandheader", this).remove();
            $("thead", this).after("<tbody></tbody>");

            $(this).datagrid('createToolItem', data);

            var columnFields = $(this).datagrid('getColumnFields');
            for (var i = 0; i < data.length; i++) {
                $("tbody", this).append("<tr index='" + i + "'></tr>");
                $("tr[index='" + i + "']", this).data("rowData", data[i]);
                for (var j = 0; j < columnFields.length; j++) {
                    var value = "";
                    if (data[i][columnFields[j]] != undefined) {
                        value = data[i][columnFields[j]];
                    }
                    var format = $(this).datagrid('getColumnOption', columnFields[j]).format;
                    if (format != undefined) {
                        var whereItems = $(this).datagrid('getColumnOption', columnFields[j]).whereItems;
                        if (whereItems && whereItems.length > 0) {
                            var whereValue = $(this).datagrid('getWhereValue', { whereItems: whereItems, rowData: data[i] });
                            value = $.getFormatedValue(value, format, whereValue);
                        }
                        else {
                            var formatParameters = $(this).datagrid('getColumnOption', columnFields[j]).formatParameters;
                            if (formatParameters && formatParameters == "fullRow") formatParameters = data[i];
                            value = $.getFormatedValue(value, format, formatParameters);
                        }
                    }
                    var formatter = $(this).datagrid('getColumnOption', columnFields[j]).formatter;
                    if (formatter != undefined) {
                        var formatedValue = formatter.call(this, value, data[i]);
                        if (formatedValue != undefined) {
                            value = formatedValue;
                        }
                    }
                    var nowrap = $(this).datagrid('getColumnOption', columnFields[j]).nowrap ? "nowrap='nowrap'" : '';
                    var textAlign = $(this).datagrid('getColumnOption', columnFields[j]).align ? 'text-align:' + $(this).datagrid('getColumnOption', columnFields[j]).align : '';
                    $("tr[index='" + i + "']", this).append("<td " + nowrap + " field='" + columnFields[j] + "' style='" + textAlign + "'>" + value + "</td>");

                    //initialize qrcode
                }

            }
            if ($.fn.qrcode) {
                $(this).find(".grid-qrcode").each(function () {
                    var size = eval($(this).attr("size"));
                    var text = $(this).attr("value");
                    $(this).qrcode({
                        render: "canvas",
                        width: size,
                        height: size,
                        text: $.utf16to8(text)
                    });

                });
            }
            if ($.fn.geomap) {
                $(this).find(".grid-geomap").each(function () {
                    var htmlOptions = $.parseOption($(this).attr('infolight-options'));   //html option
                    if ($(this).attr('id') == undefined)
                        $(this).attr('id', "a1");
                    $(this).geomap(htmlOptions);
                    var text = $(this).attr("value");
                    $(this).geomap('setValue', text);
                });
            }
            if ($.fn.signature) {
                $(this).find(".grid-signature").each(function () {
                    var data = $(this).attr('value');
                    $(this).signature('viewStatus', data);
                });
            }
            $(this).datagrid('resize');

            var gridTheme = $(this).attr('data-theme');
            var editPage = $(this).datagrid('options').editPage;
            var detailGrid = $("." + $.fn.datagrid.class, editPage);
            //add command field
            $("thead>tr", this).prepend("<th class='commandheader'></th>");
            for (var i = 0; i < data.length; i++) {
                var command = "<td align='center' nowrap='nowrap'>"
                + $.createButton($.fn.datagrid.defaults.viewText, "check", gridTheme, "grid-view")
                + $.createButton($.fn.datagrid.defaults.updateText, "edit", gridTheme, "grid-edit")
                + $.createButton($.fn.datagrid.defaults.deleteText, "delete", gridTheme, "grid-delete");
                var detailObjectID = $(this).datagrid('options').detailObjectID;
                if (detailObjectID != undefined && detailGrid.length == 0) {
                    command += $.createButton($.fn.datagrid.defaults.detailText, "grid", gridTheme, "grid-detail");
                }
                command += "</td>";
                $("tr[index='" + i + "']", this).prepend(command).trigger("create");
            }
            $("a.grid-view", this).each(function () {
                var grid = $(this).closest('.info-datagrid');
                var allowView = grid.datagrid('options').allowView;
                if (allowView == false) {
                    $(this).hide();
                }
                else {
                    $(this).bind('click', function () {
                        var rowIndex = $(this).closest('tr').attr('index');
                        var grid = $(this).closest('.info-datagrid');
                        grid.datagrid('viewRow', rowIndex);
                    });
                }
            });
            $("a.grid-edit", this).each(function () {
                var grid = $(this).closest('.info-datagrid');
                var allowUpdate = grid.datagrid('options').allowUpdate;
                if (allowUpdate == false) {
                    $(this).hide();
                }
                else {
                    $(this).bind('click', function () {
                        var rowIndex = $(this).closest('tr').attr('index');
                        var grid = $(this).closest('.info-datagrid');
                        grid.datagrid('editRow', rowIndex);
                    });
                }
            });
            $("a.grid-delete", this).each(function () {
                var grid = $(this).closest('.info-datagrid');

                var allowDelete = grid.datagrid('options').allowDelete;
                if (allowDelete == false) {
                    $(this).hide();
                }
                else {
                    $(this).bind('click', function () {
                        var rowIndex = $(this).closest('tr').attr('index');
                        var grid = $(this).closest('.info-datagrid');
                        grid.datagrid('deleteRow', rowIndex);
                    });
                }
            });
            $("a.grid-detail", this).each(function () {
                $(this).bind('click', function () {
                    var rowIndex = $(this).closest('tr').attr('index');
                    var grid = $(this).closest('.info-datagrid');
                    grid.datagrid('openDetail', rowIndex);
                });
            });


            $(this).table('refresh').trigger("create");
        });
    },
    getWhereValue: function (jq, options) {
        if (jq.length > 0) {
            var where = '';
            if (options.whereItems != undefined) {
                var whereRow = new Object();
                var whereMethods = new Object();
                var hasRemote = false;
                for (var i = 0; i < options.whereItems.length; i++) {
                    var whereItem = options.whereItems[i];
                    if (whereItem.whereValue.type == 'remote') //more effect
                    {
                        whereMethods[whereItem.field] = whereItem.whereValue.value[0];
                        hasRemote = true;
                    }
                    else if (whereItem.whereValue.type == 'row') {
                        whereRow[whereItem.field] = options.rowData[whereItem.whereValue.value[0]];
                    }
                    else {
                        whereRow[whereItem.field] = $(jq[0]).defaultValue(whereItem.whereValue.type, whereItem.whereValue.value);
                    }
                }
                if (hasRemote) {
                    var whereObjs = refval.defaultValue('remote', whereMethods);
                    var whereObj = $.parseJSON(whereObjs);
                    for (var property in whereObj) {
                        whereRow[property] = whereObj[property];
                    }
                }
                for (var property in whereRow) {
                    if (whereRow[property] != undefined && whereRow[property] != '') {
                        if (where.length > 0) {
                            where += " and ";
                        }
                        where += property + " = '" + whereRow[property].toString().replace(/\'/g, "''") + "'";
                    }
                }
            }
            return where;
        }
    },
    load: function (jq) {
        jq.each(function () {
            var options = $(this).datagrid('options');
            if (options.remoteName != undefined && options.remoteName != "" && options.tableName != undefined) {
                if (options.onBeforeLoad != undefined) {
                    options.onBeforeLoad.call(this);
                }
                var queryWord = new Object();
                if (options.parentObjectID != undefined) {
                    queryWord.parentRow = options.parentRow;
                    queryWord.parentTableName = options.parentTableName;
                }
                var gridOpt = window.sessionStorage.getItem('gridOpt');
                if (gridOpt) {
                    var opt = JSON.parse(gridOpt);
                    if (opt.id == $(this).datagrid('pageObject').attr('id')) {
                        options.pageNumber = opt.pageNumber;
                        options.whereString = opt.whereString;
                        window.sessionStorage.removeItem('gridOpt');
                    }
                }

                if (options.whereString != undefined) {
                    queryWord.whereString = encodeURI(options.whereString);
                }
                var drilldown = Request.getQueryStringByName("DRILLDOWN");
                if (drilldown != undefined && drilldown == 'true') {
                    var REMOTENAME = decodeURIComponent(Request.getQueryStringByName("REMOTENAME"));
                    var TABLENAME = decodeURIComponent(Request.getQueryStringByName("TABLENAME"));
                    var DRILLDOWN_KEYFIELD = Request.getQueryStringByName("DRILLDOWN_KEYFIELD");
                    if (REMOTENAME == options.remoteName && TABLENAME == options.tableName) {
                        queryWord.whereString = decodeURIComponent(DRILLDOWN_KEYFIELD).replace(/''/g, "'").replace(/;/g, " and ");
                    }
                }

                var columnFields = $(this).datagrid('getColumnFields');
                var totalWord = {};
                var hasTotal = false;
                for (var i = 0; i < columnFields.length; i++) {
                    var total = $(this).datagrid('getColumnOption', columnFields[i]).total;
                    if (total) {
                        hasTotal = true;
                        totalWord[columnFields[i]] = total;
                    }
                }



                //flow
                self.opened = false;
                var flowKey = Request.getQueryStringByName("FORM_PRESENTATION");
                var navMODE = Request.getFlowStringByName(flowKey, "NAVMODE");
                if (navMODE == "") {
                    navMODE = Request.getQueryStringByName("NAVMODE");
                }
                var keyValues = Request.getFlowStringByName(flowKey, "FORM_PRESENTATION");
                if (navMODE == "") {
                    navMODE = Request.getFlowStringByName(flowKey, "NAVIGATOR_MODE");
                }
                if (navMODE != "" && !options.parentObjectID) {
                    /*if (navMODE.toLowerCase() == "insert") {
                    queryWord.whereString = "1=0";
                    $(this).datagrid('insertRow');
                    }
                    else*/
                    if (navMODE.toLowerCase() == "inquery") {
                        queryWord.whereString = "(FlowFlag is Null OR FlowFlag = '')";
                        $(this).datagrid('options').allowAdd = false;
                        $(this).datagrid('options').allowUpdate = false;
                        $(this).datagrid('options').allowDelete = false;
                    }
                    else if (navMODE.toLowerCase() == "prepare") {
                        $(this).datagrid('options').allowAdd = false;
                        $(this).datagrid('options').allowDelete = false;
                    }
                    else {
                        $(this).datagrid('options').allowAdd = false;
                        $(this).datagrid('options').allowUpdate = false;
                        $(this).datagrid('options').allowDelete = false;
                    }
                    if (keyValues != "") {
                        queryWord.whereString = decodeURIComponent(keyValues).replace(/''/g, "'").replace(/;/g, " and ");
                    }
                }

                //flow
                if (!options.parentObjectID) {
                    if (navMODE.toLowerCase() == "insert") {
                        queryWord.whereString = "1=0";
                        setTimeout(function () {
                            datagrid.datagrid('insertRow');
                        }, 50);
                    }
                }
                var gridTheme = $(this).attr('data-theme');
                $.mobile.loading('show', { theme: gridTheme, text: $.fn.datagrid.defaults.loadingMessage, textVisible: true });
                var datagrid = $(this);
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: $.getUrl(options.remoteName, options.tableName),
                    data: 'remoteName=' + options.remoteName + '&page=' + options.pageNumber + '&rows=' + options.pageSize + '&queryWord=' + $.toJSONString(queryWord) + (hasTotal ? '&totalColumn=' + $.toJSONString(totalWord) : ''),
                    cache: false,
                    async: true,
                    success: function (data) {
                        if (data.rows != undefined) {
                            if (datagrid.hasClass($.fn.datalist.class)) {
                                datagrid.datalist('loadData', data.rows);
                            }
                            else {
                                datagrid.datagrid('loadData', data.rows);
                            }
                            options.keys = data.keys.split(',');
                            var total = eval(data.total);
                            options.pageCount = Math.ceil(total / options.pageSize);
                        }
                        $.mobile.loading('hide');
                        if (options.onLoadSuccess != undefined) {
                            options.onLoadSuccess.call(datagrid, data);
                        }
                        //flow
                        if (self.opened != true && !options.parentObjectID) {
                            self.opened = true;
                            var keyValues = Request.getFlowStringByName(flowKey, "FORM_PRESENTATION");
                            if (keyValues != undefined && keyValues != null && keyValues != "") {
                                if (Request.getFlowStringByName(flowKey, "NAVIGATOR_MODE") == "2") {
                                    var detailObjectID = datagrid.datagrid('options').detailObjectID;
                                    if (detailObjectID) {
                                        $("." + $.fn.datagrid.class, detailObjectID).data('isDetail', true);
                                    }
                                    if (datagrid.data('isDetail') != true) {
                                        setTimeout(function () {
                                            datagrid.datagrid('editRow', 0);
                                        }, 1000);
                                    }
                                }
                                else {
                                    setTimeout(function () {
                                        datagrid.datagrid('viewRow', 0);
                                    }, 700);
                                }
                            }
                            else {

                            }
                        }
                        //flow
                    },
                    error: function (data) {
                        $.mobile.loading('hide');
                        var message = $.getErrorMessage(data.responseText);
                        if (message == $.EEPTimeOutMessage) {
                            window.parent.location.href = "../MobileTimeout.aspx";
                        }
                        else if (message == $.EEPNotRegisterMessage) {
                            alert(message);
                        }
                        else {
                            datagrid.datagrid('warning', message);
                        }

                    }
                });
            }
        });
    },
    reload: function (jq) {
        jq.each(function () {
            $(this).datagrid('options').pageNumber = 1;
            $(this).datagrid('load');
        });
    },
    getData: function (jq) {
        if (jq.length > 0) {
            var data = [];
            $("tbody>tr", jq[0]).each(function () {
                data.push($(this).data("rowData"));
            });
            return data;
        }
        return [];
    },
    getRow: function (jq, index) {
        if (jq.length > 0) {
            if ($(jq[0]).hasClass($.fn.datalist.class)) {   //为了不重写editrow viewrow opendetail
                return $("li[index='" + index + "']", $(jq[0]).siblings('ul')).data("rowData");
            }
            else {
                return $("tr[index='" + index + "']", jq[0]).data("rowData");
            }
        }
        return new Object();
    },
    deleteRow: function (jq, index) {
        jq.each(function () {
            var options = $(this).datagrid('options');
            var row = $(this).datagrid('getRow', index);
            if (options.onDelete != undefined) {
                if (options.onDelete.call(this, row) == false) {
                    return true;
                }
            }

            var datagrid = $(this);
            var gridTheme = $(this).attr('data-theme');
            var gridViewType = options.gridViewType;

            var deleteRowF = function () {
                var applyRemote = options.remoteName;
                var applyTable = options.tableName;

                var changedData = [];
                var parentObjectID = datagrid.datagrid('options').parentObjectID;
                if (parentObjectID) {
                    var parentOptions = $("." + $.fn.datagrid.class, parentObjectID).datagrid('options');
                    var parentRow = datagrid.datagrid('options').parentRow;
                    var parentTableName = datagrid.datagrid('options').parentTableName;
                    var parentChangedData = new Object();
                    parentChangedData.tableName = parentTableName;
                    parentChangedData.inserted = [];
                    parentChangedData.deleted = [];
                    parentChangedData.updated = [];
                    parentChangedData.updated.push(parentRow);
                    changedData.push(parentChangedData);

                    applyRemote = parentOptions.remoteName;
                    applyTable = parentOptions.tableName;
                }

                var changedRows = new Object();
                changedRows.tableName = options.tableName;
                changedRows.inserted = [];
                changedRows.deleted = [];
                changedRows.updated = [];
                changedRows.deleted.push(row);
                changedData.push(changedRows);

                $.mobile.loading('show', { theme: gridTheme, text: $.fn.form.defaults.updatingMessage, textVisible: true });
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: $.getUrl(applyRemote, applyTable),
                    data: "data=" + $.toJSONString(changedData) + "&mode=update",
                    cache: false,
                    async: true,
                    success: function (data) {
                        if (datagrid.hasClass($.fn.datalist.class)) {
                            var list = datagrid.siblings("ul");
                            $("li[index='" + index + "']", list).remove();
                            $("li", list).each(function () {
                                var rowIndex = eval($(this).attr("index"));
                                if (rowIndex > eval(index)) {
                                    $(this).attr("index", rowIndex - 1);
                                }
                            });
                        }
                        else {
                            $("tr[index='" + index + "']", datagrid).remove();
                            $("tbody>tr", datagrid).each(function () {
                                var rowIndex = eval($(this).attr("index"));
                                if (rowIndex > eval(index)) {
                                    $(this).attr("index", rowIndex - 1);
                                }
                            });
                        }
                        $.mobile.loading('hide');
                    },
                    error: function (data) {
                        $.mobile.loading('hide');
                    }
                });
            };
            if (gridViewType == 'listitem') {
                deleteRowF.call(this);
            }
            $.messager.confirm('', $.fn.datagrid.defaults.confirmDeleteText, function (r) {
                if (r) {
                    deleteRowF.call(this);
                }
            });
        });
    },
    editRow: function (jq, index) {
        if (jq.length > 0) {
            var options = $(jq[0]).datagrid('options');
            var editPage = options.editPage;
            if (editPage != undefined) {
                var keys = options.keys;
                var row = $(jq[0]).datagrid('getRow', index);
                if (options.onEdit != undefined) {
                    if (options.onEdit.call(jq[0], row) == false) {
                        return;
                    }
                }
                $("." + $.fn.form.class, editPage).form({ viewPage: "#" + $(jq[0]).datagrid('pageObject').attr("id"), rowIndex: index, keys: keys });
                $("." + $.fn.form.class, editPage).form('editRow', row);
                $("." + $.fn.datagrid.class, editPage).datagrid({ parentRow: row, parentTableName: options.tableName, allowAdd: true, allowUpdate: true, allowDelete: true });
                $("." + $.fn.datagrid.class, editPage).datagrid('load');
                var role = "page";
                if (options.editMode == "dialog" && options.parentObjectID == undefined) {
                    role = "dialog";
                }
                if ($(editPage).length > 0) {
                    $.mobile.changePage($(editPage), { transition: "pop", role: role });
                }
                if ($(editPage).length > 0) {
                    $.mobile.changePage($(editPage), { transition: "pop", role: role });
                    $("." + $.fn.form.class, editPage).form('opened');
                }
            }
        }
    },
    insertRow: function (jq) {
        if (jq.length > 0) {
            var options = $(jq[0]).datagrid('options');
            var editPage = options.editPage;
            if (editPage != undefined) {
                if (options.onInsert != undefined) {
                    if (options.onInsert.call(jq[0]) == false) {
                        return;
                    }
                }
                var keys = options.keys;
                $("." + $.fn.form.class, editPage).form({ viewPage: "#" + $(jq[0]).datagrid('pageObject').attr("id"), keys: keys });
                $("." + $.fn.form.class, editPage).form('insertRow');
                $("." + $.fn.datagrid.class, editPage).datagrid({ parentRow: {}, parentTableName: options.tableName, allowAdd: false, allowUpdate: false, allowDelete: false });
                $("." + $.fn.datagrid.class, editPage).find('tbody').remove();
                $("." + $.fn.datagrid.class, editPage).siblings(".grid-header").remove();
                $("." + $.fn.datagrid.class, editPage).siblings(".grid-footer").remove();
                var role = "page";
                if (options.editMode == "dialog" && options.parentObjectID == undefined) {
                    role = "dialog";
                }
                if ($(editPage).length > 0) {
                    $.mobile.changePage($(editPage), { transition: "pop", role: role });
                }
                if ($(editPage).length > 0) {
                    $.mobile.changePage($(editPage), { transition: "pop", role: role });
                    $("." + $.fn.form.class, editPage).form('opened');
                }
            }
        }
    },
    viewRow: function (jq, index) {
        if (jq.length > 0) {
            var options = $(jq[0]).datagrid('options');
            var editPage = options.editPage;
            if (editPage != undefined) {
                var row = $(jq[0]).datagrid('getRow', index);
                if (options.onSelect != undefined) {
                    if (options.onSelect.call(jq[0], row) == false) {
                        return;
                    }
                }

                $("." + $.fn.form.class, editPage).form({ viewPage: "#" + $(jq[0]).datagrid('pageObject').attr("id") });
                $("." + $.fn.form.class, editPage).form('viewRow', row);
                $("." + $.fn.datagrid.class, editPage).datagrid({ parentRow: row, parentTableName: options.tableName, allowAdd: false, allowUpdate: false, allowDelete: false });
                $("." + $.fn.datagrid.class, editPage).datagrid('load');
                var role = "page";
                if ($(jq[0]).datagrid('options').editMode == "dialog" && $(jq[0]).datagrid('options').parentObjectID == undefined) {
                    role = "dialog";
                }
                if ($(editPage).length > 0) {
                    $.mobile.changePage($(editPage), { transition: "pop", role: role });
                }
                if ($(editPage).length > 0) {
                    $.mobile.changePage($(editPage), { transition: "pop", role: role });
                    $("." + $.fn.form.class, editPage).form('opened');
                }
            }
        }
    },
    updateRow: function (jq, data) {
        if (jq.length > 0) {
            var index = data.index;
            if (index != undefined) {
                var gridData = $(jq[0]).datagrid('getData');
                for (var property in data.row) {
                    gridData[index][property] = data.row[property];
                }
                $(jq[0]).datagrid('loadData', gridData);
            }
        }
    },
    setWhere: function (jq, where) {
        jq.each(function () {
            $(this).datagrid('options').whereString = where;
            $(this).datagrid('reload');
        });
    },
    getWhere: function (jq, where) {
        if (jq.length > 0) {
            return $(jq[0]).datagrid('options').whereString;
        }
    },
    openDetail: function (jq, index) {
        if (jq.length > 0) {
            var options = $(jq[0]).datagrid('options');
            var row = $(jq[0]).datagrid('getRow', index);
            var detailObjectID = options.detailObjectID;
            $.mobile.changePage($(detailObjectID), { transition: "pop", role: "page", transition: 'slide' })
            $("." + $.fn.datagrid.class, detailObjectID).datagrid({ parentRow: row, parentTableName: options.tableName });
            $("." + $.fn.datagrid.class, detailObjectID).datagrid('load');
        }
    },
    openQuery: function (jq) {
        if (jq.length > 0) {
            var options = $(jq[0]).datagrid('options');
            var queryObjectID = options.queryObjectID;
            $("." + $.fn.query.class, queryObjectID).query({ viewPage: "#" + $(jq[0]).datagrid('pageObject').attr("id"), queryMode: $(jq[0]).datagrid('options').queryMode });
            if (options.queryMode == 'panel' || options.queryMode == 'fuzzy') {
                //$("h4", queryObjectID).html($.fn.datagrid.defaults.queryText);
                //$("." + $.fn.query.class, queryObjectID).insertBefore($(jq[0]));
            }
            else {

                $.mobile.changePage($(queryObjectID), { transition: "pop", role: "dialog" });
                $("." + $.fn.query.class, queryObjectID).query('opened');
            }
        }
    },
    nextPage: function (jq) {
        jq.each(function () {
            var pageCount = $(this).datagrid('options').pageCount;
            if ($(this).datagrid('options').pageNumber < pageCount) {
                $(this).datagrid('options').pageNumber++;
                $(this).datagrid('load');
            }
        });
    },
    previousPage: function (jq) {
        jq.each(function () {
            if ($(this).datagrid('options').pageNumber > 1) {
                $(this).datagrid('options').pageNumber--;
                $(this).datagrid('load');
            }
        });
    },
    export: function (jq) {
        if (jq.length > 0) {
            var columns = [];
            var fields = $(jq[0]).datagrid('getColumnFields');
            for (var i = 0; i < fields.length; i++) {
                var field = fields[i]
                var option = $(jq[0]).datagrid('getColumnOption', field);
                var column = new Object();
                column.field = field;
                column.title = option.caption;

                var format = option.format;
                if (format != undefined) {
                    var index = format.indexOf('-');
                    if (index == 1 && format.length > 2) {
                        var formatType = format.substr(0, 1).toLowerCase();
                        var formatString = format.substr(2);
                        if (formatType == 'r') {
                            var formats = formatString.split('-');
                            if (formats.length > 1) {
                                column.options = {};
                                column.options.remoteName = formats[0];
                                column.options.tableName = formats[1];
                                column.options.textField = formats[2];
                                column.options.valueField = formats[3];
                            }
                        }
                    }
                }

                columns.push(column);
            }
            var options = $(jq[0]).datagrid('options');
            var title = options.title;
            var queryWord = {};
            if (options.whereString != undefined) {
                queryWord.whereString = encodeURI(options.whereString);
            }
            $.ajax({
                type: "POST",
                url: $.getUrl(options.remoteName, options.tableName),
                data: "mode=export&title=" + title + "&columns=" + $.toJSONString(columns) + "&queryWord=" + $.toJSONString(queryWord),
                cache: false,
                async: false,
                success: function (data) {
                    window.open('../handler/JqFileHandler.ashx?File=' + data, 'download');
                }
            });
        }
    },
    exportReport: function (jq, reportFile) {
        if (jq.length > 0) {
            var options = $(jq[0]).datagrid('options');
            var remoteName = options.remoteName;
            var tableName = options.tableName;
            var whereString = encodeURI(options.whereString);

            if (remoteName && tableName && reportFile) {
                var url = "../ReportViewerTemplate.aspx?RemoteName=" + remoteName + "&TableName=" + tableName + "&ReportPath=" + reportFile
                    + "&WhereString=" + whereString + "&WhereTextString=" + '';
                window.open(url);
            }
        }
    },
    warning: function (jq, message) {
        if (jq.length > 0) {
            var popupid = $(jq[0]).datagrid('pageObject').attr('id') + "_popup";

            $("p", "#" + popupid).html(message);
            $("#" + popupid).popup("open");
        }
    }
};
//--------------------------------------------------------------------------------------------------------------------------------
$.fn.datalist = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.datalist.methods[methodName];
        if (method) {
            return method(this, value);
        }
        else {
            method = $.fn.datagrid.methods[methodName];
            if (method) {
                return method(this, value);
            }
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).datalist('initialize', options);
            if (!$(this).hasClass($.fn.datalist.class)) {
                $(this).addClass($.fn.datalist.class)
            }
        });
    }
};

$.fn.datalist.class = 'info-datalist';

$.fn.datalist.methods = {
    loadData: function (jq, data) {
        jq.each(function () {
            $(this).hide();
            $(this).siblings("ul").remove();
            $(this).datagrid('createToolItem', data);
            $(this).after("<ul data-role='listview' data-inset='true'></ul>");
            var datalist = $(this).next("ul");
            var datagrid = $(this);
            var gridViewType = $(this).datagrid('options').gridViewType;
            var columnFields = $(this).datagrid('getColumnFields');
            var columnsCount = $(this).datagrid('options').listColumnCount;

            var captionWidth = $(this).datagrid('options').listCaptionWidth;
            if (gridViewType == 'listitem') {
                columnsCount = 4;
                captionWidth = 0;
            }
            if ($(window).width() <= 540) {
                columnsCount = 1;
                if (gridViewType == 'listitem') {
                    columnsCount = 2;
                }
            }
            $(this).datagrid('options').actualColumnCount = columnsCount;
            for (var i = 0; i < data.length; i++) {
                datalist.append("<li index='" + i + "'></li>");
                var li = $("li[index='" + i + "']", datalist).data("rowData", data[i]).data('rowIndex', i);
                if ($(this).datagrid('options').allowPopMenu) {
                    li.click(function () {
                        var rowIndex = $(this).data('rowIndex');
                        var popup = $('#' + datagrid.datagrid('pageObject').attr('id') + '_listpopup');
                        popup.find('a.view').html($.fn.datagrid.defaults.viewText);
                        popup.find('a.edit').html($.fn.datagrid.defaults.updateText);
                        popup.find('a.delete').html($.fn.datagrid.defaults.deleteText);
                        popup.popup('open', { positionTo: $(this) });
                        popup.find('a.view').unbind().click(function () {
                            datagrid.datagrid('viewRow', rowIndex);
                        });
                        popup.find('a.edit').unbind().click(function () {
                            datagrid.datagrid('editRow', rowIndex);
                        });
                        popup.find('a.delete').unbind().click(function () {
                            datagrid.datagrid('deleteRow', rowIndex);
                        });
                    });
                }
                if ($(this).datagrid('options').allowPopMenu) {
                    $("li[index='" + i + "']", datalist).prepend('<a class="popupSource ui-btn ui-btn-icon-right ui-icon-carat-r" />');
                }
                var p = "<p><table style='width: 100%;table-layout:fixed'><tr>";
                for (var j = 0; j < columnFields.length; j++) {
                    var value = "";
                    if (data[i][columnFields[j]] != undefined) {
                        value = data[i][columnFields[j]];
                    }
                    var format = $(this).datagrid('getColumnOption', columnFields[j]).format;
                    if (format != undefined) {
                        var whereItems = $(this).datagrid('getColumnOption', columnFields[j]).whereItems;
                        if (whereItems && whereItems.length > 0) {
                            var whereValue = $(this).datagrid('getWhereValue', { whereItems: whereItems, rowData: data[i] });
                            value = $.getFormatedValue(value, format, whereValue);
                        }
                        else {
                            var formatParameters = $(this).datagrid('getColumnOption', columnFields[j]).formatParameters;
                            value = $.getFormatedValue(value, format, formatParameters);
                        }
                    }
                    var formatter = $(this).datagrid('getColumnOption', columnFields[j]).formatter;
                    if (formatter != undefined) {
                        var formatedValue = formatter.call(this, value, data[i]);
                        if (formatedValue != undefined) {
                            value = formatedValue;
                        }
                    }
                    var fontSize = 14;
                    if (gridViewType == 'listitem' && j == 0) {
                        fontSize = 20;
                    }
                    p += "<td style='width: " + captionWidth + "px'><div class='ui-li-desc' style='font-weight:bolder;font-size:14px'>"
                    + (gridViewType == 'listitem' ? '' : $(this).datagrid('getColumnOption', columnFields[j]).caption)
                    + "</div></td>"
                    + "<td><div class='ui-li-desc' style='font-size:" + fontSize + "px'>"
                    + value
                    + "</div></td>";
                    if (columnsCount == 1 || j == 0 || (j - 1) % columnsCount == columnsCount - 1 || j == columnFields.length - 1) {
                        p += "</tr></table>";
                        if (!$(this).datagrid('options').allowPopMenu) {
                            $("li[index='" + i + "']", datalist).append(p);
                        }
                        else {
                            $("li[index='" + i + "']", datalist).find('a.popupSource').append(p);
                        }
                        p = "<table style='width: 100%;table-layout:fixed'><tr>";
                    }
                    //                    var p = "<p><table style='width: 100%;table-layout:fixed'><tr>"
                    //                    + "<td style='width: 120px'><div class='ui-li-desc' style='font-weight:bolder;font-size:14px'>"
                    //                    + $(this).datagrid('getColumnOption', columnFields[j]).caption
                    //                    + "</div></td>"
                    //                    + "<td><div class='ui-li-desc' style='font-size:14px'>"
                    //                    + value
                    //                    + "</div></td>"
                    //                    + "</tr></table><p>";
                    //                    $("li[index='" + i + "']", datalist).append(p);
                }
            }
            if ($.fn.qrcode) {
                datalist.find(".grid-qrcode").each(function () {
                    var size = eval($(this).attr("size"));
                    var text = $(this).attr("value");
                    $(this).qrcode({
                        render: "canvas",
                        width: size,
                        height: size,
                        text: text
                    });

                });
            }
            if ($.fn.geomap) {
                datalist.find(".grid-geomap").each(function () {
                    var htmlOptions = $.parseOption($(this).attr('infolight-options'));   //html option
                    if ($(this).attr('id') == undefined)
                        $(this).attr('id', "a1");
                    $(this).geomap(htmlOptions);
                    var text = $(this).attr("value");
                    $(this).geomap('setValue', text);
                });
            }

            if ($.fn.signature) {
                datalist.find(".grid-signature").each(function () {
                    var data = $(this).attr('value');
                    $(this).signature('viewStatus', data);
                });
            }
            var gridTheme = $(this).attr('data-theme');

            for (var i = 0; i < data.length; i++) {
                if (!$(this).datagrid('options').allowPopMenu) {
                    var command = "<p class='ui-li-aside' style='right:0.2em'>"
                  + $.createButton($.fn.datagrid.defaults.viewText, "check", gridTheme, "grid-view")
                  + $.createButton($.fn.datagrid.defaults.updateText, "edit", gridTheme, "grid-edit")
                  + $.createButton($.fn.datagrid.defaults.deleteText, "delete", gridTheme, "grid-delete");
                    var detailObjectID = $(this).datagrid('options').detailObjectID;
                    if (detailObjectID != undefined) {
                        command += $.createButton($.fn.datagrid.defaults.detailText, "grid", gridTheme, "grid-detail");
                    }
                    command += "</p>";
                    $("li[index='" + i + "']", datalist).prepend(command).trigger("create");
                }
                else {
                    $("li[index='" + i + "']", datalist).find('a').trigger("create");
                }

            }

            $("a.grid-view", datalist).each(function () {
                var grid = $(this).closest("ul").siblings('.info-datagrid');
                var allowView = grid.datagrid('options').allowView;
                if (allowView == false) {
                    $(this).hide();
                }
                else {
                    $(this).bind('click', function () {
                        var rowIndex = $(this).closest('li').attr('index');
                        var grid = $(this).closest("ul").siblings('.info-datagrid');
                        grid.datagrid('viewRow', rowIndex);
                    });
                }
            });
            $("a.grid-edit", datalist).each(function () {
                var grid = $(this).closest("ul").siblings('.info-datagrid');
                var allowUpdate = grid.datagrid('options').allowUpdate;
                if (allowUpdate == false) {
                    $(this).hide();
                }
                else {
                    $(this).bind('click', function () {
                        var rowIndex = $(this).closest('li').attr('index');
                        var grid = $(this).closest("ul").siblings('.info-datagrid');
                        grid.datagrid('editRow', rowIndex);
                    });
                }
            });
            $("a.grid-delete", datalist).each(function () {
                var grid = $(this).closest("ul").siblings('.info-datagrid');
                var allowDelete = grid.datagrid('options').allowDelete;
                if (allowDelete == false) {
                    $(this).hide();
                }
                else {
                    $(this).bind('click', function () {
                        var rowIndex = $(this).closest('li').attr('index');
                        var grid = $(this).closest("ul").siblings('.info-datagrid');
                        grid.datagrid('deleteRow', rowIndex);
                    });
                }
            });
            $("a.grid-detail", datalist).each(function () {
                $(this).bind('click', function () {
                    var rowIndex = $(this).closest('li').attr('index');
                    var grid = $(this).closest("ul").siblings('.info-datagrid');
                    grid.datagrid('openDetail', rowIndex);
                });
            });


            datalist.listview();
            //datalist.listview('refresh');
        });
    },
    getData: function (jq) {
        if (jq.length > 0) {
            var data = [];
            $("li", $(jq[0]).siblings('ul')).each(function () {
                data.push($(this).data("rowData"));
            });
            return data;
        }
        return [];
    },
    updateRow: function (jq, data) {
        if (jq.length > 0) {
            var index = data.index;
            if (index != undefined) {
                var gridData = $(jq[0]).datalist('getData');
                for (var property in data.row) {
                    gridData[index][property] = data.row[property];
                }
                $(jq[0]).datalist('loadData', gridData);
            }
        }
    },
    resize: function (jq) {
        jq.each(function () {
            var actualColumnCount = $(this).datagrid('options').actualColumnCount;
            var gridViewType = $(this).datagrid('options').gridViewType;
            if (gridViewType == 'listitem') {
                if ((actualColumnCount == 2 && $(window).width() > 540) || (actualColumnCount > 2 && $(window).width() <= 540)) {
                    var gridData = $(this).datalist('getData');
                    $(this).datalist('loadData', gridData);
                }
            }
            else {
                if ((actualColumnCount == 1 && $(window).width() > 540) || (actualColumnCount > 1 && $(window).width() <= 540)) {
                    var gridData = $(this).datalist('getData');
                    $(this).datalist('loadData', gridData);
                }
            }
        });
    }
};
//--------------------------------------------------------------------------------------------------------------------------------
$.fn.form = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.form.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).form('initialize', options);
            if (!$(this).hasClass($.fn.form.class)) {
                $(this).addClass($.fn.form.class)
            }
        });
    }
};

$.fn.form.class = 'info-form';

$.fn.form.defaults = {
    okText: "OK",
    cancelText: "Cancel",
    updatingMessage: "Updating data",
    updateFailMessage: "Update data faild.",
    updateSuccessMessage: "Update data successed."
};

$.fn.form.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var formOptions = new Object();

            var htmlOptions = $.parseOption($(this).attr('infolight-options'));   //html option
            for (var property in htmlOptions) {
                formOptions[property] = htmlOptions[property];
            }

            if (options != undefined) {                                     //load option
                for (var property in options) {
                    formOptions[property] = options[property];
                }
            }
            $(this).data('options', formOptions);
        });
    },
    initializeChildren: function (jq) {
        jq.each(function () {
            var selectClass = [];
            selectClass.push("." + $.fn.selects.class);
            selectClass.push("." + $.fn.radiobuttons.class);
            selectClass.push("." + $.fn.checkboxes.class);
            $(selectClass.join(), this).each(function () {
                $(this).selects({});
            });
            $("." + $.fn.refval.class, this).each(function () {
                $(this).refval({});
            });
            $("." + $.fn.mdatebox.class, this).each(function () {
                $(this).mdatebox({});
            });
            $("." + $.fn.mtimebox.class, this).each(function () {
                $(this).mtimebox({});
            });
            $("." + $.fn.file.class, this).each(function () {
                $(this).file({});
            });
            $("." + $.fn.scan.class, this).each(function () {
                $(this).scan({});
            });
            $("." + $.fn.capture.class, this).each(function () {
                $(this).capture({});
            });
            $("." + $.fn.place.class, this).each(function () {
                $(this).place({});
            });
            $("." + $.fn.signature.class, this).each(function () {
                $(this).signature({});
            });
            var geolocationClass = [];
            geolocationClass.push("." + $.fn.geolocation.class);
            geolocationClass.push("." + $.fn.geomap.class);
            $(geolocationClass.join(), this).each(function () {
                $(this).geolocation({});
            });

            var formTheme = $(this).attr('data-theme');
            $(":radio,:checkbox", this).each(function () {
                $(this).checkboxradio({ theme: formTheme });
            });
        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('options');
        }
        return new Object();
    },
    pageObject: function (jq) {
        if (jq.length > 0) {
            var page = $(jq[0]).closest("div[data-role='page']");
            if (page.length > 0) {
                return page;
            }
            else {
                return $(jq[0]).closest("div[data-role='dialog']");
            }
        }
        return Object();
    },
    loadDefault: function (jq) {
        if (jq.length > 0) {
            var defaultRow = new Object();
            var defaultMethods = new Object();
            var hasRemote = false;
            var options = $(jq[0]).form('options');
            //load default value
            $("input,select,textarea,fieldset", jq[0]).each(function () {
                var option = $.parseOption($(this).attr('infolight-options'));
                if (option != undefined) {
                    if (option.field != undefined && option.defaultValue != undefined) {
                        if (option.defaultValue.type == 'remote') //more effect
                        {
                            defaultMethods[option.field] = option.defaultValue.value[0];
                            hasRemote = true;
                        }
                        else if (option.defaultValue.type == 'field') //more effect
                        {
                            var viewPage = options.viewPage;
                            var row = $("." + $.fn.datagrid.class, viewPage).datagrid('options').parentRow;
                            option.defaultValue.value.push(row);
                            defaultRow[option.field] = $(this).defaultValue(option.defaultValue.type, option.defaultValue.value);
                        }
                        else {
                            defaultRow[option.field] = $(this).defaultValue(option.defaultValue.type, option.defaultValue.value);
                        }
                    }
                }
            });
            if (hasRemote) {
                var defaultObjs = $(this).defaultValue('remote', defaultMethods);
                var defaultObj = $.parseJSON(defaultObjs);
                for (var property in defaultObj) {
                    defaultRow[property] = defaultObj[property];
                }
            }
            getSeq(null, $(jq), defaultRow);
            return defaultRow;
        }
    },
    loadRow: function (jq, row) {
        jq.form('updateRow', row);
        if (jq.length > 0) {
            var options = $(jq[0]).form('options');
            $("div.commandfooter", jq[0]).remove();
            var formTheme = $(jq[0]).attr('data-theme');
            var command = "<div class='commandfooter'>"
            + "<fieldset class='ui-grid-a'>"
            + "<div class='ui-block-a'>" + $.createTextButton($.fn.form.defaults.okText, formTheme, "form-ok") + "</div>"
            + "<div class='ui-block-b'>" + $.createTextButton($.fn.form.defaults.cancelText, formTheme, "form-cancel") + "</div>"
            + "</fieldset>"
            + "</div>";
            $(jq[0]).append(command);
            $("div.commandfooter", jq[0]).trigger("create");
            $("a.form-ok", jq[0]).bind('click', function () {
                var masterForm = $(jq[0]).form('options').masterForm;
                if (masterForm) {
                    $(masterForm).form('apply');
                }
                else {
                    $(jq[0]).form('apply');
                }
            });
            $("a.form-cancel", jq[0]).bind('click', function () {
                if (options.onCancel != undefined) {
                    if (options.onCancel.call(jq[0]) == false) {
                        return;
                    }
                }
                $(jq[0]).form('close');
            });

            //            if (options.onLoadSuccess != undefined) {
            //                options.onLoadSuccess.call(jq[0]);
            //            }
        }

        //$("a.form-FlowSubmit", jq[0]).each(function () {
        //    $(this).bind('click', function () {
        //        doSubmit();
        //    });
        //});
    },
    updateRow: function (jq, row) {
        if (jq.length > 0) {
            if (row != undefined) {
                $("input,select,textarea,fieldset,div", jq[0]).each(function () {
                    var option = $.parseOption($(this).attr('infolight-options'));
                    if (option != undefined) {
                        var field = option.field;
                        if (field != undefined && row[field] != undefined) {
                            if ($(this).hasClass($.fn.mdatebox.class)) {
                                $(this).mdatebox('setValue', row[field]);
                            }
                            else if ($(this).hasClass($.fn.mtimebox.class)) {
                                $(this).mtimebox('setValue', row[field]);
                            }
                            else if ($(this).hasClass($.fn.radiobuttons.class)) {
                                $(this).radiobuttons('setValue', row[field]);
                            }
                            else if ($(this).hasClass($.fn.checkboxes.class)) {
                                $(this).checkboxes('setValue', row[field]);
                            }
                            else if ($(this).hasClass($.fn.refval.class)) {
                                $(this).refval('setValue', row[field]);
                            }
                            else if ($(this).hasClass($.fn.flipswitch.class)) {
                                $(this).flipswitch('setValue', row[field]);
                            }
                            else if ($(this).hasClass($.fn.file.class)) {
                                $(this).file('setValue', row[field]);
                            }
                            else if ($(this).hasClass($.fn.geomap.class)) {
                                $(this).geomap('setValue', row[field]);
                            }
                            else if ($(this).hasClass($.fn.geolocation.class)) {
                                $(this).geolocation('setValue', row[field]);
                            }
                            else if ($(this).hasClass($.fn.signature.class)) {
                                var status = $(jq[0]).form('status');
                                if (status == "view") {
                                    $(this).signature('viewStatus', row[field]);
                                }
                                else if (row[field] == undefined || row[field] == "") {
                                    $(this).signature('load', row[field]);
                                }
                                else {
                                    $(this).signature('noEmptyStatus', row[field]);
                                }
                            }

                            else if ($(this).attr('type') == 'range') {
                                $(this).attr('value', row[field]);
                            }
                            else {
                                $(this).val(row[field]);
                                if ($(this).hasClass($.fn.selects.class)) {
                                    if ($(this).data("mobile-selectmenu") != undefined) {
                                        $(this).selectmenu("refresh");
                                    }
                                }
                                else if ($(this).data('mobile-slider') != undefined) {
                                    $(this).slider("refresh");
                                }
                            }
                        }
                    }
                });
                $(".info-qrcode", jq[0]).each(function () {
                    var option = $.parseOption($(this).attr('infolight-options'));
                    if (option != undefined) {
                        var field = option.field;
                        if (field != undefined && row[field] != undefined) {
                            if ($(this).hasClass($.fn.infoqrcode.class)) {
                                $(this).infoqrcode('setValue', row[field]);
                            }
                        }
                    }
                });
            }
        }
    },
    clear: function (jq) {
        jq.each(function () {
            $("input,select,textarea", this).each(function () {
                if ($(this).is(":radio") || $(this).is(":checkbox")) {
                    $(this).prop("checked", false);
                    if ($(this).data("mobile-checkboxradio") != undefined) {
                        $(this).checkboxradio("refresh");
                    }
                }
                else if ($(this).hasClass($.fn.file.class)) {
                    $(this).file('setValue', '');
                }
                else if ($(this).hasClass($.fn.refval.class)) {
                    $(this).refval('setValue', '');
                }
                else {
                    $(this).val('');
                    if ($(this).hasClass($.fn.selects.class)) {
                        if ($(this).data("mobile-selectmenu") != undefined) {
                            $(this).selectmenu("refresh");
                        }
                    }
                }
            });
        });
    },
    editRow: function (jq, row) {
        $(jq).data("rowData", row);
        $(jq).form('initializeChildren'); //为了refval元件的WhereItem能取到form上的值
        $(jq).form('clear');
        $(jq).data("status", 'edit'); //为了file元件能判断出当前的状态
        $(jq).form('loadRow', row);
        $(jq).removeData("rowData");
        $(jq).form('status', 'edit');
        var options = $(jq[0]).form('options');
        if (options.onLoadSuccess != undefined) {
            options.onLoadSuccess.call(jq[0]);
        }
        var chainDataForm = $(jq).form('options').chainDataForm;
        if (chainDataForm) {
            var masterForm = $(jq).form('options').masterForm;
            if (!masterForm) {
                masterForm = jq[0];
            }
            $(chainDataForm).form({ viewPage: $(jq).form('options').viewPage, rowIndex: $(jq).form('options').rowIndex, keys: $(jq).form('options').keys, masterForm: masterForm });
            $(chainDataForm).form('editRow', row);
        }
    },
    insertRow: function (jq) {
        $(jq).data("rowData", {});
        $(jq).form('initializeChildren');
        $(jq).form('clear');
        $(jq).data("status", 'insert');
        $(jq).form('loadRow', $(jq).form('loadDefault'));
        $(jq).removeData("rowData");
        $(jq).form('status', 'insert');
        var options = $(jq[0]).form('options');
        if (options.onLoadSuccess != undefined) {
            options.onLoadSuccess.call(jq[0]);
        }
        var chainDataForm = $(jq).form('options').chainDataForm;
        if (chainDataForm) {
            var masterForm = $(jq).form('options').masterForm;
            if (!masterForm) {
                masterForm = jq[0];
            }
            $(chainDataForm).form({ viewPage: $(jq).form('options').viewPage, masterForm: masterForm });
            $(chainDataForm).form('insertRow');
        }
    },
    viewRow: function (jq, row) {
        $(jq).data("rowData", row);
        $(jq).form('initializeChildren');
        $(jq).form('clear');
        $(jq).data('status', 'view');
        $(jq).form('loadRow', row);
        $(jq).removeData("rowData");
        $(jq).form('status', 'view');
        $(jq).find('.form-ok').hide();
        $(jq).find('.form-cancel').hide();
        var options = $(jq[0]).form('options');
        if (options.onLoadSuccess != undefined) {
            options.onLoadSuccess.call(jq[0]);
        }
        var chainDataForm = $(jq).form('options').chainDataForm;
        if (chainDataForm) {
            $(chainDataForm).form({ viewPage: $(jq).form('options').viewPage });
            $(chainDataForm).form('viewRow', row);
        }
    },
    status: function (jq, status) {
        if (status != undefined) {
            jq.each(function () {
                if (status == 'view') {
                    $("input,select,textarea,fieldset", this).each(function () {
                        $(this).disable();
                        $(this).closest('div[data-role="fieldcontain"]').find('label').removeClass('validate-label');
                    });
                }
                else if (status == 'edit') {
                    var keys = $(this).form('options').keys;
                    $("input,select,textarea,fieldset", this).each(function () {
                        var option = $.parseOption($(this).attr('infolight-options'));
                        if (option != undefined) {
                            var isKey = false;
                            for (var i = 0; i < keys.length; i++) {
                                if (keys[i] == option.field) {
                                    isKey = true;
                                    break;
                                }
                            }
                            if (isKey) {
                                $(this).disable();
                            }
                            else {
                                if (option.readOnly) {
                                    $(this).disable();
                                }
                                else {
                                    $(this).enable();
                                }
                            }
                            if (option.validate) {
                                $(this).closest('div[data-role="fieldcontain"]').find('label').addClass('validate-label');
                            }
                        }
                        else {
                            $(this).enable();
                        }
                    });
                }
                else {
                    $("input,select,textarea,fieldset", this).each(function () {
                        var option = $.parseOption($(this).attr('infolight-options'));
                        if (option != undefined) {
                            if (option.validate) {
                                $(this).closest('div[data-role="fieldcontain"]').find('label').addClass('validate-label');
                            }
                            if (option.readOnly) {
                                $(this).disable();
                            }
                            else {
                                $(this).enable();
                            }
                        }
                        else {
                            $(this).enable();
                        }
                    });
                }
                //$(this).data("status", status);
            });
        }
        else {
            if (jq.length > 0) {
                return $(jq[0]).data("status");
            }
        }
    },
    opened: function (jq) {
        jq.each(function () {
            $("." + $.fn.scan.class, this).each(function () {
                $(this).scan('createButton');
            });
            $("." + $.fn.capture.class, this).each(function () {
                $(this).capture('createButton');
            });
            $("." + $.fn.refval.class, this).each(function () {
                $(this).refval('createButton');
            });
            $("." + $.fn.place.class, this).each(function () {
                $(this).place('createButton');
            });
        });
    },
    getRow: function (jq, validate) {
        if (jq.length > 0) {
            //打开时取传进来的初值
            if ($(jq[0]).data("rowData") != undefined) {
                return $(jq[0]).data("rowData");
            }
            var validateSuccess = true;
            var messages = [];
            var row = new Object();
            $("input,select,textarea,fieldset,div", jq[0]).each(function () {
                var option = $.parseOption($(this).attr('infolight-options'));
                if (option != undefined) {
                    var value = '';
                    if ($(this).hasClass($.fn.mdatebox.class)) {
                        value = $(this).mdatebox('getValue');
                    }
                    else if ($(this).hasClass($.fn.mtimebox.class)) {
                        value = $(this).mtimebox('getValue');
                    }
                    else if ($(this).hasClass($.fn.radiobuttons.class)) {
                        value = $(this).radiobuttons('getValue');
                    }
                    else if ($(this).hasClass($.fn.checkboxes.class)) {
                        value = $(this).checkboxes('getValue');
                    }
                    else if ($(this).hasClass($.fn.refval.class)) {
                        value = $(this).refval('getValue');
                    }
                    else if ($(this).hasClass($.fn.file.class)) {
                        value = $(this).file('getValue');
                    }
                    else if ($(this).hasClass($.fn.geomap.class)) {
                        value = $(this).geomap('getValue');
                    }
                    else if ($(this).hasClass($.fn.geolocation.class)) {
                        value = $(this).geolocation('getValue');
                    }
                    else if ($(this).hasClass($.fn.signature.class)) {
                        value = $(this).signature('getValue');
                    }
                    else {
                        value = $(this).val();
                    }
                    row[option.field] = value;
                    if (validate == true && option.validate) {
                        var caption = option.field;
                        if ($(this).hasClass($.fn.radiobuttons.class) || $(this).hasClass($.fn.checkboxes.class)) {
                            var legend = $("legend", this);
                            if (legend.length > 0) {
                                caption = legend.text();
                            }
                        }
                        else {
                            var label = $("label[for='" + $(this).attr('id') + "']", jq[0]);
                            if (label.length > 0) {
                                caption = label.text();
                            }
                        }
                        option.caption = caption;
                        if (option.validate.required == true && value == '') {
                            messages.push(caption + $.fn.validate.defaults.valueNotNullMessage);
                            validateSuccess = false;
                            return true;
                        }
                        else if (option.validate.type && value != '') {
                            var valid = $(option).validate(option.validate.type, value, option.validate.value);
                            if (valid == false) {
                                if (option.validate.message) {
                                    messages.push(option.validate.message);
                                }
                                else {
                                    messages.push(caption + $.fn.validate.defaults.valueNotInRangeMessage);
                                }
                                validateSuccess = false;
                                return true;
                            }
                        }
                    }
                }
            });
            if (!validateSuccess) {
                $(jq[0]).form('warning', messages.join("<br/>"));
                return false;
            }
            else {
                //chain
                var chainDataForm = $(jq[0]).form('options').chainDataForm;
                if (chainDataForm) {
                    var chainRow = $(chainDataForm).form('getRow');
                    if (chainRow == false) {
                        return false;
                    }
                    else {
                        for (var p in chainRow) {
                            row[p] = chainRow[p];
                        }
                    }
                }
                return row;
            }
        }
    },
    validate: function (jq) {
        if (jq.length > 0) {
            return $(jq[0]).form('getRow', true);
        }
    },
    validateDuplicate: function (jq, changedData) {
        var validate = true;
        if (jq.length > 0) {
            var form = $(jq[0]);
            var options = form.form('options');
            $.ajax({
                type: "POST",
                url: $.getUrl(options.remoteName, options.tableName),
                data: "data=" + $.toJSONString(changedData) + "&mode=duplicate",
                cache: false,
                async: false,
                success: function (data) {
                    if (data == "false") {
                        form.form('warning', $.fn.validate.defaults.valueDuplicateMessage);
                        validate = false;
                    }
                },
                error: function (data) {
                    validate = false;
                }
            });
        }
        return validate;
    },
    warning: function (jq, message) {
        if (jq.length > 0) {
            var popupid = $(jq[0]).form('pageObject').attr('id') + "_popup";
            $("p", "#" + popupid).css('color', '#FF0000');
            $("p", "#" + popupid).html(message);
            $("#" + popupid).popup("open");
        }
    },
    close: function (jq) {
        if (jq.length > 0) {
            var viewPage = $(jq[0]).form('options').viewPage;
            var editMode = $("." + $.fn.datagrid.class, viewPage).datagrid('options').editMode;
            var parentObjectID = $("." + $.fn.datagrid.class, viewPage).datagrid('options').parentObjectID;
            //            if (editMode == "dialog" && parentObjectID == undefined) {
            //                $(jq[0]).form('pageObject').dialog('close');
            //            }
            //            else {
            $.mobile.changePage($(viewPage), { transition: "pop", role: "page" })
            //            }
        }
    },
    apply: function (jq) {
        jq.each(function () {
            var row = $(this).form('validate');
            if (row == false) {
                return true; // continue
            }
            var changedData = [];
            var changedRows = new Object();
            var options = $(this).form('options');
            if (options.onApply != undefined) {
                if (options.onApply.call(this, row) == false) {
                    return true;
                }
            }

            //Detail apply
            var applyRemote = options.remoteName;
            var applyTable = options.tableName;
            var viewPage = options.viewPage;
            var parentObjectID = $("." + $.fn.datagrid.class, viewPage).datagrid('options').parentObjectID;
            while (parentObjectID != undefined) {
                var parentOptions = $("." + $.fn.datagrid.class, parentObjectID).datagrid('options');
                var parentRow = $("." + $.fn.datagrid.class, viewPage).datagrid('options').parentRow;
                var parentTableName = $("." + $.fn.datagrid.class, viewPage).datagrid('options').parentTableName;
                var parentChangedData = new Object();
                parentChangedData.tableName = parentTableName;
                parentChangedData.inserted = [];
                parentChangedData.deleted = [];
                parentChangedData.updated = [];
                parentChangedData.updated.push(parentRow);
                changedData.push(parentChangedData);

                applyRemote = parentOptions.remoteName;
                applyTable = parentOptions.tableName;
                //viewPage = parentOptions.viewPage;
                parentObjectID = $("." + $.fn.datagrid.class, parentObjectID).datagrid('options').parentObjectID;
            }

            changedRows.tableName = options.tableName;
            changedRows.inserted = [];
            changedRows.deleted = [];
            changedRows.updated = [];

            var encodeRow = new Object();
            for (var p in row) {
                var value = row[p];
                if (value) {
                    encodeRow[p] = encodeURIComponent(value);
                    encodeRow[p] = encodeRow[p].toString().replace(/(%22)/g, "\"").replace(/(%5C)/g, "\\");
                }
                else {
                    encodeRow[p] = value;
                }
            }

            var status = $(this).form('status');
            if (status == "edit") {
                changedRows.updated.push(encodeRow);
            }
            else if (status == "insert") {
                changedRows.inserted.push(encodeRow);
            }
            changedData.push(changedRows);

            if (status == "insert" && options.duplicateCheck == true) {
                if ($(this).form('validateDuplicate', changedData) == false) {
                    return false;
                }
            }

            var form = $(this);
            var formTheme = $(this).attr('data-theme');
            $.mobile.loading('show', { theme: formTheme, text: $.fn.form.defaults.updatingMessage, textVisible: true });
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: $.getUrl(applyRemote, applyTable),
                data: "data=" + $.toJSONString(changedData) + "&mode=update",
                cache: false,
                async: true,
                success: function (data) {
                    var viewPage = options.viewPage;
                    var parentObjectID = $("." + $.fn.datagrid.class, viewPage).datagrid('options').parentObjectID;
                    if (parentObjectID) {
                        $(form).form('close');
                        $("." + $.fn.datagrid.class, viewPage).datagrid('reload');
                    }
                    else if (Request.getQueryStringByName("FLOWFILENAME") != "" || Request.getQueryStringByName("LISTID") != ""
                        || Request.getQueryStringByName("NAVMODE") == "Insert") {
                        alert($.fn.form.defaults.updateSuccessMessage);
                        if (data != null) {
                            form.form('updateRow', data[0]);
                        }
                        form.data('status', 'view');
                        form.form('status', 'view');

                        if (status == "insert") {
                            var editPage = form.closest('div[data-role="page"]');
                            if (editPage.length == 0) {
                                editPage = form.closest('div[data-role="dialog"]');
                            }
                            $("." + $.fn.datagrid.class, editPage).each(function () {
                                $(this).datagrid({ parentRow: data[0], parentTableName: options.tableName, allowAdd: true, allowUpdate: true, allowDelete: true });
                                $(this).datagrid('load');
                            });
                        }

                        if (options.autoPause && Request.getQueryStringByName("FLOWFILENAME") != "") {
                            doPause(options.id, 'Pause', function () {
                                if (options.autoSubmit) {
                                    doSubmit(options.id, 'Approve');
                                }
                            });
                        }
                        else if (options.autoSubmit) {
                            if (Request.getQueryStringByName("FLNAVIGATOR_MODE") == "1")
                                doApprove(options.id, 'Approve');
                            else if (Request.getQueryStringByName("LISTID") != "" && Request.getQueryStringByName("FLOWFILENAME") == "")
                                doApprove(options.id, 'Approve');
                            else
                                doSubmit(options.id, 'Approve');
                        }
                    }
                    else {
                        if (options.appliedClose == false) {
                            if (status == "insert") {
                                if (data != null) {
                                    form.form('updateRow', data[0]);
                                }
                                var editPage = form.closest('div[data-role="page"]');
                                if (editPage.length == 0) {
                                    editPage = form.closest('div[data-role="dialog"]');
                                }
                                $("." + $.fn.datagrid.class, editPage).each(function () {
                                    $(this).datagrid({ parentRow: data[0], parentTableName: options.tableName, allowAdd: true, allowUpdate: true, allowDelete: true });
                                    $(this).datagrid('load');
                                });
                            }
                        }
                        else {
                            $(form).form('close');
                        }
                        if (status == "insert") {
                            var viewPage = form.form('options').viewPage;
                            $("." + $.fn.datagrid.class, viewPage).datagrid('reload');
                        }
                    }
                    if (options.onApplied != undefined) {
                        options.onApplied.call(this);
                    }
                    if (status == "edit") {
                        var viewPage = form.form('options').viewPage;
                        var rowIndex = form.form('options').rowIndex;
                        $("." + $.fn.datagrid.class, viewPage).each(function () {
                            if ($(this).hasClass($.fn.datalist.class)) {
                                $(this).datalist('updateRow', { index: rowIndex, row: row });
                            }
                            else {
                                $(this).datagrid('updateRow', { index: rowIndex, row: row });
                            }
                        })
                    }
                    $.mobile.loading('hide');
                },
                error: function (data) {
                    $.mobile.loading('hide');
                    var message = $.getErrorMessage(data.responseText);
                    $(form).form('warning', message);
                }
            });
        });
    }
};
//--------------------------------------------------------------------------------------------------------------------------------
$.fn.mdatebox = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.mdatebox.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).mdatebox('initialize', options);
            if (!$(this).hasClass($.fn.mdatebox.class)) {
                $(this).addClass($.fn.mdatebox.class)
            }
        });
    }
};

$.fn.mdatebox.class = 'info-datebox';

$.fn.mdatebox.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var dateboxOptions = new Object();
            var htmlOptions = $.parseOption($(this).attr('infolight-options'));   //html option
            for (var property in htmlOptions) {
                dateboxOptions[property] = htmlOptions[property];
            }

            if (options != undefined) {                                     //load option
                for (var property in options) {
                    dateboxOptions[property] = options[property];
                }
            }
            $(this).data('moptions', dateboxOptions);
            //            if ($(this).data("mobile-datebox") == undefined) {
            //                $(this).datebox();
            //            }
            $(this).bind('datebox', function (e, passed) {
                //alert(passed.method);
                if (passed.method == "set") {
                    var date = $(this).datebox('getTheDate');
                    var format = $(this).mdatebox('options').format;
                    var formatedValue = $.getFormatedValue(date, "D-" + format);
                    passed.value = formatedValue;
                }
            });
        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('moptions') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('moptions');
        }
        return new Object();
    },
    getValue: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data("mobile-datebox") != undefined) {
                if ($(jq[0]).val()) {
                    var format = $(jq[0]).mdatebox('options').format;
                    var specialvalue = $(jq[0]).val();
                    specialvalue = specialvalue.replace(/\./g, '-');
                    specialvalue = specialvalue.replace(/\//g, '-');
                    format = format.replace(/\./g, '-');
                    format = format.replace(/\//g, '-');
                    var reg = new RegExp("YYY-");
                    var result = reg.test(format);
                    if (result) {
                        var vdate = specialvalue.split("-");
                        var realyear = eval(vdate[0]) + 1911;
                        if (vdate.length == 2) {
                            var datatype = $(jq[0]).mdatebox('options').dataType;
                            if (datatype == "varchar8")
                                return realyear + vdate[1];
                            else
                                return realyear + "-" + vdate[1];
                        }
                        else if (vdate.length == 3) {
                            var datatype = $(jq[0]).mdatebox('options').dataType;
                            if (datatype == "varchar8")
                                return realyear + vdate[1] + vdate[2];
                            else
                                return realyear + "-" + vdate[1] + "-" + vdate[2];
                        }
                    }
                    else {
                        reg = new RegExp("YY-");
                        result = reg.test(format);
                        if (result) {
                            var vdate = specialvalue.split("-");
                            var realyear = eval(vdate[0]) + 1911;
                            if (vdate.length == 2) {
                                var datatype = $(jq[0]).mdatebox('options').dataType;
                                if (datatype == "varchar8")
                                    return realyear + vdate[1];
                                else
                                    return realyear + "-" + vdate[1];
                            }
                            else if (vdate.length == 3) {
                                var datatype = $(jq[0]).mdatebox('options').dataType;
                                if (datatype == "varchar8")
                                    return realyear + vdate[1] + vdate[2];
                                else
                                    return realyear + "-" + vdate[1] + "-" + vdate[2];
                            }
                        }
                    }
                    var datatype = $(jq[0]).mdatebox('options').dataType;
                    if (datatype == "varchar8")
                        return $.getFormatedValue(specialvalue, "D-yyyymmdd");
                    else
                        return $.getFormatedValue(specialvalue, "D-yyyy-mm-dd");
                }
                else {
                    return '';
                }
            }
            else {
                return $(jq[0]).val();
            }
        }
    },
    setValue: function (jq, value) {
        if (jq.length > 0) {
            var format = $(jq[0]).mdatebox('options').format;
            var formatedValue = $.getFormatedValue(value, "D-" + format);
            if (format.lockInput)
                $(jq[0]).removeAttr("readonly");
            if (format.lockInput)
                $(jq[0]).attr("readonly", "readonly");
            setTimeout(function () { $(jq[0]).val(formatedValue); }, 100);
        }
    }
};

//--------------------------------------------------------------------------------------------------------------------------------
$.fn.mtimebox = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.mtimebox.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).mtimebox('initialize', options);
            if (!$(this).hasClass($.fn.mtimebox.class)) {
                $(this).addClass($.fn.mtimebox.class)
            }
        });
    }
};

$.fn.mtimebox.class = 'info-time';

$.fn.mtimebox.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var dateboxOptions = new Object();
            var htmlOptions = $.parseOption($(this).attr('infolight-options'));   //html option
            for (var property in htmlOptions) {
                dateboxOptions[property] = htmlOptions[property];
            }

            if (options != undefined) {                                     //load option
                for (var property in options) {
                    dateboxOptions[property] = options[property];
                }
            }
            $(this).data('moptions', dateboxOptions);
        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('moptions') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('moptions');
        }
        return new Object();
    },
    getValue: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data("mobile-datebox") != undefined) {
                if ($(jq[0]).val()) {
                    var date = $(jq[0]).datebox('getTheDate');
                    var hours = date.getHours();
                    if (hours < 10)
                        hours = "0" + hours;
                    var minutes = date.getMinutes();
                    if (minutes < 10)
                        minutes = "0" + minutes;
                    return hours + ":" + minutes;
                    //return date.getHours() + ":" + date.getMinutes();
                    //return $(jq[0]).val();
                }
                else {
                    return '';
                }
            }
            else {
                return $(jq[0]).val();
            }
        }
    },
    setValue: function (jq, value) {
        jq.each(function () {
            var time = $(this);
            var datetime = new Date(value);
            if (datetime.toString() == "Invalid Date") {
                datetime = new Date("01-01-2000 " + value); //chrome
                if (datetime.toString() == "Invalid Date") {
                    datetime = new Date("2000/01/01 " + value); //safari
                }
            }
            setTimeout(function () {//chrome
                time.val(value);
                if (datetime.toString() != "Invalid Date") {
                    time.datebox('setTheDate', datetime);
                }
            }, 100);
            if (value.length == 5) {
                var hours = value.substring(0, 2);
                var minutes = value.substring(3);
                if (hours > 12) {
                    hours = hours - 12;
                    if (hours < 10)
                        hours = "0" + hours;

                    setTimeout(function () { time.val(hours + ":" + minutes + " PM"); }, 200);
                }
                else {
                    setTimeout(function () { time.val(hours + ":" + minutes + " AM"); }, 200);
                }
            }
        });
    }

};

//--------------------------------------------------------------------------------------------------------------------------------
$.fn.selects = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.selects.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).selects('initialize', options);
            if (!$(this).hasClass($.fn.selects.class)) {
                $(this).addClass($.fn.selects.class)
            }
        });
    }
};

$.fn.selects.class = 'info-selects';

$.fn.selects.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var selectsOptions = new Object();
            var htmlOptions = $.parseOption($(this).attr('infolight-options'));   //html option
            for (var property in htmlOptions) {
                selectsOptions[property] = htmlOptions[property];
            }

            if (options != undefined) {                                     //load option
                for (var property in options) {
                    selectsOptions[property] = options[property];
                }
            }
            $(this).data('options', selectsOptions);
            $(this).selects('load');

        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('options');
        }
        return new Object();
    },
    loadData: function (jq, data) {
        jq.each(function () {
            $("option", this).remove();
            var options = $(this).selects('options');
            $(this).append("<option selected='selected'></option>");
            for (var i = 0; i < data.length; i++) {
                var item = $.createOption(data[i][options.displayMember], data[i][options.valueMember]);
                $(this).append(item);
            }
            if ($(this).data("mobile-selectmenu") != undefined) {
                $(this).selectmenu("refresh");
            }
            if (options.onSelect) {
                $(this).bind('change', function () {
                    options.onSelect.call(this, $(this).val());
                });
            }
        });
    },
    load: function (jq) {
        jq.each(function () {
            var options = $(this).selects('options');
            var selects = $(this);
            if (options.remoteName != undefined && options.tableName != undefined) {
                var data = '';
                if (options.pageNumber != undefined && options.pageSize != undefined) {
                    data += 'page=' + options.pageNumber + '&rows=' + options.pageSize;
                }
                var queryWord = new Object();
                queryWord.whereString = '';
                if (options.whereString != undefined) {
                    queryWord.whereString = encodeURI(options.whereString);
                }
                if (selects.hasClass($.fn.refval.class)) {
                    var whereValue = selects.refval('getWhereValue');
                    if (whereValue != undefined && whereValue != '') {
                        if (queryWord.whereString.length > 0) {
                            queryWord.whereString += " and ";
                        }
                        queryWord.whereString += whereValue;
                    }
                }
                data += '&queryWord=' + $.toJSONString(queryWord);
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: $.getUrl(options.remoteName, options.tableName),
                    data: data,
                    cache: false,
                    async: false,
                    success: function (data) {
                        if (data.rows != undefined) {
                            if (selects.hasClass($.fn.radiobuttons.class)) {
                                selects.radiobuttons('loadData', data.rows);
                            }
                            else if (selects.hasClass($.fn.checkboxes.class)) {
                                selects.checkboxes('loadData', data.rows);
                            }
                            else if (selects.hasClass($.fn.refval.class)) {
                                var total = eval(data.total);
                                options.pageCount = Math.ceil(total / options.pageSize);
                                selects.refval('loadData', data.rows);
                            }
                            else {
                                selects.selects('loadData', data.rows);
                            }
                        }
                    },
                    error: function (data) {
                    }
                });
            }
            else {
                if (options.onSelect) {
                    if (selects.hasClass($.fn.radiobuttons.class)) {
                        var radiobuttons = $(this);
                        $(this).find(':radio').click(function () {
                            options.onSelect.call(radiobuttons, radiobuttons.radiobuttons('getValue'));
                        });
                    }
                    else if (selects.hasClass($.fn.checkboxes.class)) {
                        var checkboxes = $(this);
                        if (options.onSelect) {
                            $(this).find(':checkbox').click(function () {
                                options.onSelect.call(checkboxes, checkboxes.checkboxes('getValue'));
                            });
                        }
                    }
                    else {
                        $(this).unbind().bind('change', function () {
                            options.onSelect.call(this, $(this).val());
                        });
                    }
                }
            }
        });
    },
    setWhere: function (jq, where) {
        jq.each(function () {
            $(this).selects('options').whereString = where;
            $(this).selects('load');
        });
    }
};
//--------------------------------------------------------------------------------------------------------------------------------
$.fn.radiobuttons = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.radiobuttons.methods[methodName];
        if (method) {
            return method(this, value);
        }
        else {
            method = $.fn.selects.methods[methodName];
            if (method) {
                return method(this, value);
            }
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).radiobuttons('initialize', options);
            if (!$(this).hasClass($.fn.radiobuttons.class)) {
                $(this).addClass($.fn.radiobuttons.class)
            }
        });
    }
};

$.fn.radiobuttons.class = 'info-radiobuttons';
$.fn.radiobuttons.methods = {
    loadData: function (jq, data) {
        jq.each(function () {
            var radiobuttons = $(this);
            $("input, label", this).remove();
            $("div.ui-radio", this).remove();
            var options = $(this).radiobuttons('options');
            var id = $(this).attr('id');
            id = new Date().getMilliseconds();
            for (var i = 0; i < data.length; i++) {
                var item = $.createRadioButton(data[i][options.displayMember], data[i][options.valueMember], id + "_" + i, id + "_choice");
                if ($(".ui-controlgroup-controls", this).length == 0) {
                    $(this).append(item);
                }
                else {
                    var radio = $('<div class="ui-radio ui-mini"><div class="ui-radio ui-mini">' + item + '</div></div>').appendTo($(".ui-controlgroup-controls", this));
                    radio.find('label').addClass('ui-btn ui-corner-all ui-btn-b ui-btn-icon-left ui-radio-off');
                    if (i == 0) {
                        radio.find('label').addClass('ui-first-child');
                    }
                    if (i == data.length - 1) {
                        radio.find('label').addClass('ui-last-child');
                    }
                    radio.find(':radio').checkboxradio({});
                }
            }
            if (options.onSelect) {
                $(this).find(':radio').click(function () {
                    options.onSelect.call(radiobuttons, radiobuttons.radiobuttons('getValue'));
                });
            }
        });
    },
    getValue: function (jq) {
        if (jq.length > 0) {
            var selectedValues = [];
            $(":checked", jq[0]).each(function () {
                selectedValues.push($(this).val());
            });
            return selectedValues.join();
        }
    },
    setValue: function (jq, value) {
        jq.each(function () {
            if (value != undefined) {
                var selectedValues = value.toString().split(",");
                for (var i = 0; i < selectedValues.length; i++) {
                    $(":radio,:checkbox", this).each(function () {
                        if ($(this).val() == selectedValues[i]) {
                            $(this).prop("checked", true);
                        }
                        else {
                            $(this).prop("checked", false);
                        }
                        if ($(this).data("mobile-checkboxradio") != undefined) {
                            $(this).checkboxradio("refresh");
                        }
                    });
                }
            }
        });
    }
};
//--------------------------------------------------------------------------------------------------------------------------------
$.fn.checkboxes = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.checkboxes.methods[methodName];
        if (method) {
            return method(this, value);
        }
        else {
            method = $.fn.selects.methods[methodName];
            if (method) {
                return method(this, value);
            }
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).checkboxes('initialize', options);
            if (!$(this).hasClass($.fn.checkboxes.class)) {
                $(this).addClass($.fn.checkboxes.class)
            }
        });
    }
};

$.fn.checkboxes.class = 'info-checkboxes';
$.fn.checkboxes.methods = {
    loadData: function (jq, data) {
        jq.each(function () {
            $("input, label", this).remove();
            $("div.ui-checkbox", this).remove();
            var options = $(this).checkboxes('options');
            var id = $(this).attr('id');
            for (var i = 0; i < data.length; i++) {
                var item = $.createCheckbox(data[i][options.displayMember], data[i][options.valueMember], id + "_" + i, id + "_" + i);
                if ($(".ui-controlgroup-controls", this).length == 0) {
                    $(this).append(item);
                }
                else {

                    var checkbox = $('<div class="ui-checkbox ui-mini">' + item + '</div>').appendTo($(".ui-controlgroup-controls", this));
                    checkbox.find('label').addClass('ui-btn ui-corner-all ui-btn-b ui-btn-icon-left ui-checkbox-off');
                    if (i == 0) {
                        checkbox.find('label').addClass('ui-first-child');
                    }
                    if (i == data.length - 1) {
                        checkbox.find('label').addClass('ui-last-child');
                    }

                    //$(".ui-controlgroup-controls", this).append('<div class="ui-checkbox ui-mini">' + item + '</div>');
                }
            }
            var checkboxes = $(this);

            if (options.onSelect) {
                $(this).find(':checkbox').click(function () {
                    options.onSelect.call(checkboxes, checkboxes.checkboxes('getValue'));
                });
            }
        });
    },
    getValue: function (jq) {
        if (jq.length > 0) {
            var selectedValues = [];
            $(":checked", jq[0]).each(function () {
                selectedValues.push($(this).val());
            });
            return selectedValues.join();
        }
    },
    setValue: function (jq, value) {
        jq.each(function () {
            if (value != undefined) {
                var selectedValues = value.toString().split(",");
                $(":radio,:checkbox", this).each(function () {
                    $(this).prop("checked", false);
                    for (var i = 0; i < selectedValues.length; i++) {
                        if ($(this).val() == selectedValues[i]) {
                            $(this).prop("checked", true);
                        }
                    }
                    if ($(this).data("mobile-checkboxradio") != undefined) {
                        $(this).checkboxradio("refresh");
                    }
                });
            }
        });
    }
};

//--------------------------------------------------------------------------------------------------------------------------------
$.fn.refval = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.refval.methods[methodName];
        if (method) {
            return method(this, value);
        }
        else {
            method = $.fn.selects.methods[methodName];
            if (method) {
                return method(this, value);
            }
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).refval('initialize', options);
            if (!$(this).hasClass($.fn.refval.class)) {
                $(this).addClass($.fn.refval.class)
            }
        });
    }
};

$.fn.refval.class = 'info-refval';
$.fn.refval.methods = {
    initialize: function (jq, options) {
        if (options == undefined) {
            options = new Object();
        }
        options.pageNumber = 1;
        $(jq).selects('initialize', options);
    },
    loadData: function (jq, data) {
        jq.each(function () {
            var refval = $(this);
            var popupid = refval.attr('id') + "_popup";
            if ($("li", "#" + popupid).length > 0) {
                $("li", "#" + popupid).remove();
            }

            //$("ul", "#" + popupid).append("<li data-role='list-divider'>Select Item</li>");
            //            var button = "<div><a data-mini='true' data-inline='true' data-role='button' data-icon='arrow-l' data-iconpos='notext' data-theme='e'>test</a>"
            //                + "<a data-mini='true' data-inline='true' data-role='button' data-icon='arrow-r' data-iconpos='notext' data-theme='e'>test</a>"
            //                + "<a data-mini='true' data-inline='true' data-role='button' data-icon='search' data-iconpos='notext' data-theme='e'>test</a></div>";
            //            $("ul", "#" + popupid).before(button);

            var options = refval.refval('options');
            for (var i = 0; i < data.length; i++) {
                var li = "<li in data-icon='check' index='" + i + "'><a class='list-select'><h2>" + data[i][options.displayMember] + "</h2>";
                li += "<p><strong>" + data[i][options.valueMember] + "</strong></p>"
                if (options.columns != undefined) {
                    for (var j = 0; j < options.columns.length; j++) {
                        li += "<p><strong>" + data[i][options.columns[j].field] + "</strong></p>";
                    }
                }
                li += "</a></li>";
                $("ul", "#" + popupid).append(li);
                $("li[index='" + i + "']", "#" + popupid).data("rowData", data[i]);
            }
            if ($("ul", "#" + popupid).hasClass('ui-listview')) {
                $("ul", "#" + popupid).listview("refresh");
            }
            if (options.pageNumber == 1) {
                //                $("a.grid-previous[data-role='button']", "#" + popupid).attr('data-icon', ' ');
                //                $("a.grid-previous[data-role='button']", "#" + popupid).removeClass('ui-icon-arrow-l');


                $("a.grid-previous[data-role='button']", "#" + popupid).hide();
            }
            else {
                $("a.grid-previous[data-role='button']", "#" + popupid).css('display', 'inline-block');
                $("a.grid-previous[data-role='button']", "#" + popupid).show();
                //$("a.grid-previous[data-role='button']", "#" + popupid).addClass('ui-icon-arrow-l');
            }
            if (options.pageNumber == options.pageCount) {
                $("a.grid-next[data-role='button']", "#" + popupid).hide();
                //                $("a.grid-next[data-role='button']", "#" + popupid).attr('data-icon', ' ');
                //                $("a.grid-next[data-role='button']", "#" + popupid).removeClass('ui-icon-arrow-r');
            }
            else {
                $("a.grid-next[data-role='button']", "#" + popupid).show();
                //$("a.grid-next[data-role='button']", "#" + popupid).addClass('ui-icon-arrow-r');
            }
            $("a.grid-previous[data-role='button']", "#" + popupid).each(function () {
                $(this).attr('title', $.fn.datagrid.defaults.previousPageText);
                $(this).unbind().bind('click', function () {
                    refval.refval('previousPage');
                });
            });
            $("a.grid-next[data-role='button']", "#" + popupid).each(function () {
                $(this).attr('title', $.fn.datagrid.defaults.nextPageText);
                $(this).unbind().bind('click', function () {
                    refval.refval('nextPage');
                });
            });
            $("a.grid-query[data-role='button']", "#" + popupid).each(function () {
                $(this).attr('title', $.fn.datagrid.defaults.queryText);
                var queryObjectID = refval.refval('options').queryObjectID;
                if (queryObjectID == undefined) {
                    $(this).hide();
                }
                else {
                    $(this).unbind().bind('click', function () {
                        //refval.refval('openQuery');
                        var value = $(this).parent().parent().find('input.fuzzy-query').val();
                        refval.refval('fuzzyQuery', value);
                    });
                }
            });
            $("a.grid-clear[data-role='button']", "#" + popupid).each(function () {
                $(this).attr('title', $.fn.query.defaults.clearText);
                $(this).unbind().bind('click', function () {
                    refval.refval('setValue', '');
                    $("#" + popupid).popup("close");
                });
            });
            $("a.grid-query[data-role='button']", $(this).parent()).each(function () {
                $(this).attr('title', $.fn.datagrid.defaults.queryText);
                var queryObjectID = refval.refval('options').queryObjectID;
                if (queryObjectID == undefined) {
                    $(this).hide();
                }
                else {
                    $(this).unbind().bind('click', function () {
                        //refval.refval('openQuery');
                        var value = $(this).parent().parent().find('input.fuzzy-query').val();
                        refval.refval('fuzzyQuery', value);
                    });
                }
            });
            $("a.grid-clear[data-role='button']", $(this).parent()).each(function () {
                $(this).attr('title', $.fn.query.defaults.clearText);
                $(this).unbind().bind('click', function () {
                    refval.refval('setValue', '');
                    $("#" + popupid).popup("close");
                });
            });

            $("a.list-select", "#" + popupid).bind('click', function () {
                var rowData = $(this).closest('li').data("rowData");
                refval.refval('setValue', rowData[options.valueMember]);
                //column match
                if (options.columnMatches != undefined) {
                    var matchRow = new Object();
                    var matchMethods = new Object();
                    var hasRemote = false;
                    for (var i = 0; i < options.columnMatches.length; i++) {
                        var columnMatch = options.columnMatches[i];
                        if (columnMatch.matchValue.type == 'remote') //more effect
                        {
                            matchMethods[columnMatch.field] = columnMatch.matchValue.value[0];
                            hasRemote = true;
                        }
                        else if (columnMatch.matchValue.type == 'row') {
                            matchRow[columnMatch.field] = rowData[columnMatch.matchValue.value[0]];
                        }
                        else {
                            matchRow[columnMatch.field] = refval.defaultValue(columnMatch.matchValue.type, columnMatch.matchValue.value);
                        }
                    }
                    if (hasRemote) {
                        var matchObjs = refval.defaultValue('remote', matchMethods);
                        var matchObj = $.parseJSON(matchObjs);
                        for (var property in matchObj) {
                            matchRow[property] = matchObj[property];
                        }
                    }
                    refval.closest("." + $.fn.form.class).form('updateRow', matchRow);
                }
                var onSelect = options.onSelect;
                if (onSelect) {
                    onSelect.call(refval, rowData[options.valueMember], rowData);
                }

                $("#" + popupid).popup("close");

            });
        });
    },
    getValue: function (jq) {
        if (jq.length > 0) {

            return $(jq[0]).data("value");
            //return $(jq[0]).val();
        }
    },
    setValue: function (jq, value) {
        jq.each(function () {
            var refval = $(this);
            refval.data("value", value);

            var options = refval.refval('options');
            if (options.valueMember == options.displayMember) {
                $(this).val(value);
            }
            else {
                var options = $(this).refval('options');
                var format = "R-" + options.remoteName + "-" + options.tableName + "-" + options.displayMember + "-" + options.valueMember;
                var whereValue = $(this).refval('getWhereValue');
                var displayValue = $.getFormatedValue(value, format, whereValue);
                $(this).val(displayValue);
            }

        });
    },
    getWhereValue: function (jq) {
        if (jq.length > 0) {
            var where = '';
            var options = $(jq[0]).refval('options');
            if (options.whereItems != undefined) {
                var whereRow = new Object();
                var whereMethods = new Object();
                var hasRemote = false;
                for (var i = 0; i < options.whereItems.length; i++) {
                    var whereItem = options.whereItems[i];
                    if (whereItem.whereValue.type == 'remote') //more effect
                    {
                        whereMethods[whereItem.field] = whereItem.whereValue.value[0];
                        hasRemote = true;
                    }
                    else if (whereItem.whereValue.type == 'row') {
                        var rowData = $(jq[0]).closest("." + $.fn.form.class).form('getRow');
                        whereRow[whereItem.field] = rowData[whereItem.whereValue.value[0]];
                    }
                    else {
                        whereRow[whereItem.field] = $(jq[0]).defaultValue(whereItem.whereValue.type, whereItem.whereValue.value);
                    }
                }
                if (hasRemote) {
                    var whereObjs = refval.defaultValue('remote', whereMethods);
                    var whereObj = $.parseJSON(whereObjs);
                    for (var property in whereObj) {
                        whereRow[property] = whereObj[property];
                    }
                }
                for (var property in whereRow) {
                    if (whereRow[property] != undefined && whereRow[property] != '') {
                        if (where.length > 0) {
                            where += " and ";
                        }
                        where += property + " = '" + whereRow[property].toString().replace(/\'/g, "''") + "'";
                    }
                }
            }
            return where;
        }
    },
    createButton: function (jq) {
        jq.each(function () {
            if ($(this).next('a').length == 0) {
                $(this).css('display', 'inline-block');
                $(this).css('margin-right', '-40px');
                var input = $(this);
                input.removeAttr('onclick');
                input.unbind('click').prop('readonly', true);
                $('<a href="#" class="ui-input-clear ui-btn ui-icon-grid ui-btn-icon-notext ui-shadow ui-corner-all" role="button" style="vertical-align: middle; display: inline-block;"></a>')
                .insertAfter(this)
                .click(function () {
                    input.refval('open');

                });
            }
        });
    },
    open: function (jq) {
        if (jq.length > 0) {
            //如果有whereItem,重新过滤资料
            var options = $(jq[0]).refval('options');
            if (options.whereItems != undefined && options.whereItems.length > 0) {
                var whereString = $(jq[0]).refval('options').whereString;
                $(jq[0]).refval('setWhere', whereString);
            }
            var popupid = $(jq[0]).attr('id') + "_popup";
            $("#" + popupid).popup("open");
        }
    },
    setWhere: function (jq, where) {
        jq.each(function () {
            $(this).refval('options').whereString = where;
            $(this).refval('options').pageNumber = 1;
            $(this).refval('load');
        });
    },
    openQuery: function (jq) {
        if (jq.length > 0) {
            var options = $(jq[0]).refval('options');
            var queryObjectID = options.queryObjectID
            $(queryObjectID).query({ refval: "#" + $(jq[0]).attr("id") });
            $(queryObjectID).show();
        }
    },
    fuzzyQuery: function (jq, value) {
        if (jq.length > 0) {
            var refval = $(jq[0]);
            var options = refval.refval('options');
            var queryObjectID = refval.refval('options').queryObjectID;
            var where = "";
            $('input,select,textarea,fieldset', queryObjectID).each(function () {
                var option = $.parseOption($(this).attr('infolight-options'));
                if (option != undefined) {
                    var queryField = option.queryField;
                    if (option.queryTable) {
                        queryField = option.queryTable + '.' + queryField;
                    }
                    if (queryField != undefined) {
                        var condition = option.condition;
                        var dataType = option.dataType;
                        var isNvarChar = option.isNvarChar;
                        if (where.length > 0) {
                            where += " or ";
                        }
                        var nvarchar = '';
                        if (isNvarChar) {
                            nvarchar = 'N';
                        }
                        var formatValue = '';
                        if (dataType == 'string') {
                            formatValue = nvarchar + "'" + value.toString().replace(/\'/g, "''") + "'"
                        }
                        else if (dataType == 'datetime') {
                            if (databaseType == 'oracle') {
                                if (value.length > 10)
                                    formatValue = "to_date('" + value + "','yyyy-mm-dd HH24:Mi:SS')";
                                formatValue = "to_date('" + value + "','yyyy-mm-dd')";
                            }
                            else {
                                formatValue = "'" + value.toString().replace(/\'/g, "''") + "'";
                            }
                        }
                        else {
                            formatValue = value;
                        }
                        switch (condition) {
                            case '=':
                            case '!=':
                            case '>=':
                            case '<=':
                            case '>':
                            case '<':
                                where += queryField + " " + condition + " " + formatValue; break;
                            case '%': where += queryField + " like " + nvarchar + "'" + value.toString().replace(/\'/g, "''") + "%'"; break;
                            case '%%': where += queryField + " like " + nvarchar + "'%" + value.toString().replace(/\'/g, "''") + "%'"; break;
                            default: break;
                        }
                    }
                }
            });
            refval.refval('setWhere', where);
        }
    },
    closeQuery: function (jq) {
        if (jq.length > 0) {
            var options = $(jq[0]).refval('options');
            var queryObjectID = options.queryObjectID
            $(queryObjectID).hide();
        }
    },
    nextPage: function (jq) {
        jq.each(function () {
            var pageCount = $(this).refval('options').pageCount;
            if ($(this).refval('options').pageNumber < pageCount) {
                $(this).refval('options').pageNumber++;
                $(this).refval('load');
            }
        });
    },
    previousPage: function (jq) {
        jq.each(function () {
            if ($(this).refval('options').pageNumber > 1) {
                $(this).refval('options').pageNumber--;
                $(this).refval('load');
            }
        });
    }
};

$.fn.flipswitch = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.flipswitch.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
};

$.fn.flipswitch.class = 'info-flipswitch';
$.fn.flipswitch.methods = {
    setValue: function (jq, value) {
        jq.each(function () {
            var onValue = $($(this).children()[1]).val();
            if (onValue == value.toString()) {
                this.selectedIndex = 1;
            }
            else {
                this.selectedIndex = 0;
            }
            if ($(this).hasClass("ui-slider-switch")) {
                $(this).slider("refresh");
            }
        });
    }
}


$.fn.file = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.file.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).file('initialize', options);
            if (!$(this).hasClass($.fn.file.class)) {
                $(this).addClass($.fn.file.class)
            }
        });
    }
};

$.fn.file.class = 'info-file';

$.fn.file.defaults = {
    uploadingMessage: "Uploading file",
    ExtensionNotMatchMessage: "File does not have extension in"
};

$.fn.file.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var fileOptions = new Object();
            var htmlOptions = $.parseOption($(this).attr('infolight-options'));   //html option
            for (var property in htmlOptions) {
                fileOptions[property] = htmlOptions[property];
            }

            if (options != undefined) {                                     //load option
                for (var property in options) {
                    fileOptions[property] = options[property];
                }
            }
            $(this).data('options', fileOptions);
            if (!$(this).hasClass("ui-input-text")) {
                $(this).textinput();
            }
            $(this).file('load');
        });
    },
    load: function (jq) {
        jq.each(function () {
            $(this).unbind().bind("change", function () {
                var filter = $(this).file('options').filter;
                if (filter != undefined) {
                    var extensions = filter.split("|");
                    var ext = $(this).val().split(".").pop();
                    var isext = false;
                    for (var i = 0; i < extensions.length; i++) {
                        if (extensions[i].toLowerCase() == ext.toLowerCase()) {
                            isext = true;
                            break;
                        }
                    }
                    if (!isext) {
                        extensions = filter.split(';');
                        for (var i = 0; i < extensions.length; i++) {
                            if (extensions[i].toLowerCase() == ext.toLowerCase()) {
                                isext = true;
                                break;
                            }
                        }
                    }
                    if (!isext) {
                        $(this).closest("." + $.fn.form.class).form('warning', $.fn.file.defaults.ExtensionNotMatchMessage + " (" + extensions.join(',') + ")");
                        return false;
                    }
                }
                var onSuccess = $(this).file('options').onSuccess;
                $.mobile.loading('show', { theme: 'd', text: 'uploading file', textVisible: true });
                var fileId = $(this).attr('id');
                var file = $(this);
                var directory = $(this).file('options').directory;
                var fileSizeLimited = $(this).file('options').fileSizeLimited;
                $.ajaxFileUpload({
                    url: $.getUploadUrl(), //需要链接到服务器地址   
                    secureuri: false,
                    data: {
                        filter: '',
                        isAutoNum: false,
                        UpLoadFolder: directory,
                        fileSizeLimited: fileSizeLimited == undefined ? "" : fileSizeLimited
                    },
                    fileElementId: fileId, //文件选择框的id属性   
                    dataType: 'json', //服务器返回的格式，可以是json   
                    success: function (data) {
                        $.mobile.loading('hide');
                        if (data['result'] == "success") {
                            $("#" + fileId).file('setValue', data.message);
                            $("#" + fileId).file('options').size = data.size;
                            if (onSuccess) {
                                onSuccess.call(file, data);
                            }
                        }
                        else if (data["result"] == "error") {
                            alert(data["message"]);
                        }
                    },
                    error: function (data) {
                        $.mobile.loading('hide');
                        alert(data);
                    }
                });
            });
        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('options');
        }
        return new Object();
    },
    getValue: function (jq) {
        if (jq.length > 0) {
            var value = $(jq[0]).data("value");
            if (value != undefined) {
                return value
            }
            else {
                return '';
            }
        }
    },
    setValue: function (jq, value) {
        jq.each(function () {
            $(this).data("value", value);
            $(".file-container", $(this).parent()).remove();
            if (value != undefined && value != '') {
                var path = "../"
                var directory = $(this).file('options').directory;
                if (directory != undefined && directory != '') {
                    path += directory + "/";
                }
                path += value;

                $(this).parent().prepend("<div class='file-container'><a href='" + path + "' target='_blank' rel='external'>" + value + "</a><div>");
                $(".file-container", $(this).parent()).trigger('create');

                var status = $(this).closest("." + $.fn.form.class).form('status');
                if (status == 'insert' || status == 'edit') {
                    $(".file-container", $(this).parent()).bind("click", function () {
                        $("." + $.fn.file.class, $(this).parent()).file('setValue', '');
                        return false;
                    });
                }
                $(this).parent().removeClass("ui-shadow-inset ui-corner-all ui-body-c");
                $(this).hide();
            }
            else {
                $(this).parent().addClass("ui-shadow-inset ui-corner-all ui-body-c");
                $(this).val('');
                $(this).file('load');
                $(this).show();
            }
        });
    },
    enable: function (jq) {
        jq.each(function () {
            $(this).textinput("enable");
        });
    },
    disable: function (jq) {
        jq.each(function () {
            if ($(this).file('getValue') == '') {
                $(this).textinput("disable");
            }
            else {
                $(this).textinput("enable");
            }
        });
    }
}

$.fn.geolocation = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.geolocation.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).geolocation('initialize', options);
            if (!$(this).hasClass($.fn.geolocation.class)) {
                $(this).addClass($.fn.geolocation.class)
            }
        });
    }
};

$.fn.geolocation.class = 'info-geolocation';

$.fn.geolocation.defaults = {
    notSupportMessage: 'Geolocation is not supported by this browser.'
};

$.fn.geolocation.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var geolocationOptions = new Object();
            var htmlOptions = $.parseOption($(this).attr('infolight-options'));   //html option
            for (var property in htmlOptions) {
                geolocationOptions[property] = htmlOptions[property];
            }

            if (options != undefined) {                                     //load option
                for (var property in options) {
                    geolocationOptions[property] = options[property];
                }
            }
            $(this).data('options', geolocationOptions);
            $(this).removeAttr('style');
            $(this).removeAttr('readonly');
            $(this).geolocation('load');
        });
    },
    loadData: function (jq, position) {
        $(jq).val(position.coords.latitude + "," + position.coords.longitude);
        $(jq).attr('readonly', true);
    },
    load: function (jq) {
        jq.each(function () {
            var geolocation = $(this);
            if (geolocation.geolocation('options').geolocation) {
                if (navigator.geolocation) {
                    navigator.geolocation.getCurrentPosition(
                function (position) {
                    geolocation.data("value", position.coords.latitude + "," + position.coords.longitude);
                    if (geolocation.hasClass($.fn.geomap.class)) {
                        geolocation.geomap('loadData', position);
                    }
                    else {
                        geolocation.geolocation('loadData', position);
                    }
                },
                function (error) {
                    var message = '';
                    switch (error.code) {
                        case error.PERMISSION_DENIED:
                            message = "User denied the request for Geolocation.";
                            break;
                        case error.POSITION_UNAVAILABLE:
                            message = "Location information is unavailable.";
                            break;
                        case error.TIMEOUT:
                            message = "The request to get user location timed out.";
                            break;
                        case error.UNKNOWN_ERROR:
                            message = "An unknown error occurred.";
                            break;
                    }
                    if (geolocation.hasClass($.fn.geomap.class)) {
                        geolocation.geomap('error', message);
                    }
                    else {
                        geolocation.geolocation('error', message);
                    }
                },
                {
                    enableHighAccuracy: true,
                    maximumAge: 0,
                    timeout: 15000
                });
                }
                else {
                    if (geolocation.hasClass($.fn.geomap.class)) {
                        geolocation.geomap('error', $.fn.geolocation.defaults.notSupportMessage);
                    }
                    else {
                        geolocation.geolocation('error', $.fn.geolocation.defaults.notSupportMessage);
                    }
                }
            }
        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('options');
        }
        return new Object();
    },
    getValue: function (jq) {
        if (jq.length > 0) {
            var value = $(jq[0]).data("value");
            if (value != undefined) {
                return value
            }
            else {
                return '';
            }
        }
    },
    error: function (jq, message) {
        $(jq).val(message);
        $(jq).attr('readonly', true);
    }
}

$.fn.geomap = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.geomap.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).geomap('initialize', options);
            if (!$(this).hasClass($.fn.geomap.class)) {
                $(this).addClass($.fn.geomap.class)
            }
        });
    }
};

$.fn.geomap.class = 'info-map';

$.fn.geomap.defaults = {

};

$.fn.geomap.methods = {
    initialize: function (jq, options) {
        $(jq).geolocation('initialize', options);
    },
    displayMap: function (jq, coords) {
        jq.each(function () {
            if (typeof google != "undefined") {
                var latlon = new google.maps.LatLng(coords.latitude, coords.longitude)
                var myOptions = {
                    center: latlon, zoom: 14,
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    mapTypeControl: false,
                    navigationControlOptions: { style: google.maps.NavigationControlStyle.SMALL }
                };
                $(this).height(160);
                $(this).width(240);
                $(this).css("display", "inline-block");
                var map = new google.maps.Map(this, myOptions);
                var marker = new google.maps.Marker({ position: latlon, map: map, title: "You are here!" });
            }
            else if (typeof BMap != "undefined") {
                $(this).height(160);
                $(this).width(240);
                $(this).css("display", "inline-block");
                var id = $(this).attr('id');
                var map = new BMap.Map(id);
                map.centerAndZoom(new BMap.Point(coords.longitude, coords.latitude), 14);
            }
        });
    },
    loadData: function (jq, position) {
        setTimeout(function () {
            $(jq).geomap('displayMap', position.coords);
        }, 1000);
    },
    load: function (jq) {
        $(jq).geolocation('load');
    },
    getValue: function (jq) {
        if (jq.length > 0) {
            var value = $(jq[0]).data("value");
            if (value != undefined) {
                return value
            }
            else {
                return '';
            }
        }
    },
    setValue: function (jq, value) {
        jq.each(function () {
            if ($(this).geolocation('options').geolocation)
            { }
            else {
                if (value) {
                    if ($(this).geolocation('options').address) {
                        var geomap = $(this);
                        if (typeof google != "undefined") {
                            $.ajax({
                                type: "POST",
                                url: "http://maps.google.com/maps/api/geocode/json?address=" + encodeURIComponent(value) + "&sensor=false",
                                cache: false,
                                async: false,
                                success: function (data) {
                                    if (data.status == 'OK') {
                                        if (data.results.length > 0) {
                                            var location = data.results[0].geometry.location;
                                            var latitude = location.lat;
                                            var longitude = location.lng;
                                            setTimeout(function () {
                                                geomap.geomap('displayMap', { latitude: latitude, longitude: longitude });
                                            }, 1000);
                                        }
                                    }

                                }, error: function (data) {

                                }
                            });
                        }
                        else if (typeof BMap != "undefined") {
                            var ak = $('#_BAIDUAK').val();
                            $.ajax({
                                type: "GET",
                                url: "http://api.map.baidu.com/geocoder/v2/?address=" + encodeURIComponent(value) + "&output=json&ak=" + ak,
                                cache: false,
                                async: false,
                                dataType: 'jsonp',
                                success: function (data) {
                                    if (data.status == 0) {
                                        var location = data.result.location;
                                        var latitude = location.lat;
                                        var longitude = location.lng;
                                        geomap.geomap('displayMap', { latitude: latitude, longitude: longitude });
                                    }

                                }, error: function (data) {

                                }
                            });
                        }
                    }
                    else {
                        var position = value.toString().split(',');
                        if (position.length == 2) {
                            var latitude = eval(position[0]);
                            var longitude = eval(position[1]);
                            $(this).geomap('displayMap', { latitude: latitude, longitude: longitude });
                        }
                    }
                    $(this).data("value", value);
                }
            }
        });
    }
}

$.fn.scan = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.scan.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).scan('initialize', options);
            if (!$(this).hasClass($.fn.scan.class)) {
                $(this).addClass($.fn.scan.class)
            }
        });
    }
};


$.fn.scan.class = 'info-scan';

$.fn.scan.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var scanOptions = new Object();
            var htmlOptions = $.parseOption($(this).attr('infolight-options'));   //html option
            for (var property in htmlOptions) {
                scanOptions[property] = htmlOptions[property];
            }

            if (options != undefined) {                                     //load option
                for (var property in options) {
                    scanOptions[property] = options[property];
                }
            }
            $(this).data('options', scanOptions);
        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('options');
        }
        return new Object();
    },
    createButton: function (jq) {
        jq.each(function () {
            if ($(this).next('a').length == 0) {
                $(this).css('display', 'inline-block');
                $(this).css('margin-right', '-40px');
                var input = $(this);
                var onScan = $(this).scan('options').onScan;
                if ($(this).scan('options').autoOpen) {
                    $(this).focus(function () {
                        if (cordova && cordova.plugins) {
                            cordova.plugins.barcodeScanner.scan(
                              function (result) {
                                  if (!result.cancelled) {
                                      if (onScan) onScan.call(input, result.text);
                                      input.val(result.text);
                                  }
                              },
                              function (error) {
                                  alert("Failed: " + error);
                              }
                            );
                        }
                        else {
                            window.alert('Your device does not support.');
                        }
                    });
                }


                $('<a href="#" class="ui-input-clear ui-btn ui-icon-bars ui-btn-icon-notext ui-shadow ui-corner-all" role="button" style="vertical-align: middle; display: inline-block;"></a>')
                .insertAfter(this)
                .click(function () {
                    try {
                        if (cordova && cordova.plugins) {
                            cordova.plugins.barcodeScanner.scan(
                              function (result) {
                                  if (!result.cancelled) {
                                      if (onScan) onScan.call(input, result.text);
                                      input.val(result.text);
                                  }
                              },
                              function (error) {
                                  alert("Failed: " + error);
                              }
                            );
                        }
                        else {
                            window.alert('Your device does not support.');
                        }
                    }
                    catch (ex) {
                        var smg = $.sysmsg('getValue', 'Web/JQMobile/MobileOnly');
                        window.alert(smg);
                    }
                });
            }
        });
    }
};

$.fn.signature = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.signature.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).signature('initialize', options);
            if (!$(this).hasClass($.fn.signature.class)) {
                $(this).addClass($.fn.signature.class)
            }
        });
    }
};

$.fn.signature.class = "info-signature";

$.fn.signature.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var signatureOptions = new Object();
            var htmlOptions = $.parseOption($(this).attr('infolight-options'));   //html option
            for (var property in htmlOptions) {
                signatureOptions[property] = htmlOptions[property];
            }

            if (options != undefined) {                                     //load option
                for (var property in options) {
                    signatureOptions[property] = options[property];
                }
            }
            $(this).data('options', signatureOptions);
            $(this).signature('load');
        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('options');
        }
        return new Object();
    },
    load: function (jq) {
        jq.each(function () {
            $(this).signature('clear');
            var signature = $(this);
            var height = signature.height();
            if ($.mobile.version == '1.3.2') {
                if (!signature.prev().hasClass('ui-input-text')) {
                    signature.prev().addClass('ui-input-text');
                }
            }
            var htmlOptions = $.parseOption(signature.attr('infolight-options'));   //html option
            if (signature.children().length == 0) {
                signature.jSignature({ height: htmlOptions.height });
            }
            else
                signature.jSignature('reset');
        });
    },
    clear: function (jq) {
        jq.each(function () {
            $(this).children().remove();
            $(this).next().remove();
        });
    },
    getValue: function (jq) {
        if (jq.length > 0) {
            var signature = $(jq[0]);
            if (signature.has('canvas').length > 0) {
                var htmlOptions = $.parseOption(signature.attr('infolight-options'));   //html option
                if (htmlOptions.format == "svgbase64") {
                    var data = signature.jSignature("getData", "svgbase64");
                    return encodeURIComponent(data[1]);
                }
                else if (htmlOptions.format == "base30") {
                    var data = signature.jSignature("getData", "base30");
                    return encodeURIComponent(data[1]);
                }
                else if (htmlOptions.format == "image") {
                    var data = signature.jSignature("getData");
                    return encodeURIComponent(data);
                }
                else if (htmlOptions.format == "svg") {
                    var data = signature.jSignature("getData", "svg");
                    var retvalue = data[1].replace(/</g, "ASCII3C").replace(/>/g, "ASCII3E").replace(/=/g, "ASCII3D").replace(/\"/g, "ASCII22");
                    //return encodeURIComponent(retvalue);
                    return retvalue;
                }
            }
            else if (signature.data('data') != undefined) {
                return signature.data('data');
            }
            else return "";
        }
    },
    viewStatus: function (jq, data) {
        jq.each(function () {
            $(this).signature('clear');
            $(this).data('data', data);
            $(this).css("display", "inline-block");
            var htmlOptions = $.parseOption($(this).attr('infolight-options'));
            //html option
            if (htmlOptions.format == "svgbase64") {
                if (data != undefined && data != "") {
                    var i = new Image();
                    i.src = "data:image/svg+xml;base64," + decodeURIComponent(data);
                    $(i).appendTo($(this)); // append the image (SVG) to DOM. 
                }
            }
            else if (htmlOptions.format == "base30") {
                if (data != undefined && data != "") {
                    $(this).jSignature({ height: htmlOptions.height });
                    $(this).jSignature("setData", "data:image/jsignature;base30," + decodeURIComponent(data));
                }
            }
            else if (htmlOptions.format == "image") {
                if (data != undefined && data != "") {
                    var i = new Image();
                    //i.src = data;//data:image/png;base64,
                    i.src = decodeURIComponent(data);
                    $(i).appendTo($(this)); // append the image (SVG) to DOM. 
                }
            }
            else if (htmlOptions.format == "svg") {
                if (data != undefined && data != "") {
                    data = data.replace(/ASCII3C/g, "<").replace(/ASCII3E/g, ">").replace(/ASCII3D/g, "=").replace(/ASCII22/g, "\"");
                    //data = data.replace(/&lt;/g,"<").replace(/&gt;/g,">");
                    //$(this).html(decodeURIComponent(data));
                    $(this).html(data);
                }
            }

        });
    },
    noEmptyStatus: function (jq, data) {
        jq.each(function () {
            var cancelText = $.fn.query.defaults.clearText;
            var signature = $(this);
            signature.data('data', data);
            signature.signature('clear');
            var htmlOptions = $.parseOption(signature.attr('infolight-options'));
            //html option
            if (htmlOptions.format == "svgbase64") {
                if ($.mobile.version == '1.3.2') {
                    var a = $('<a href="javascript:void(0)" style="position:absolute" data-role="button" data-icon="refresh" data-inline="true" data-theme="b" data-corners="true" data-shadow="true" data-iconshadow="true" data-wrapperels="span" class="ui-btn ui-shadow ui-btn-corner-all ui-btn-inline ui-btn-icon-left ui-btn-up-b"><span class="ui-btn-inner"><span class="ui-btn-text">' + cancelText + '</span><span class="ui-icon ui-icon-refresh ui-icon-shadow">&nbsp;</span></span></a>');
                    var clearbutton = $(a).appendTo(signature);
                    clearbutton.unbind().bind('click', function () {
                        signature.signature('clear');
                        signature.signature('load');
                    });
                }
                else {
                    var a = $('<a href="javascript:void(0)" style="position:absolute" data-role="button" data-icon="refresh" data-inline="true" data-theme="b" data-corners="true" data-shadow="true" data-iconshadow="true" data-wrapperels="span" class="ui-link ui-btn ui-btn-b ui-icon-refresh ui-btn-icon-left ui-shadow-icon ui-btn-inline ui-shadow ui-corner-all" role="button">' + cancelText + '</a>');
                    var clearbutton = $(a).appendTo(signature);
                    clearbutton.unbind().bind('click', function () {
                        signature.signature('clear');
                        signature.signature('load');
                    });
                }

                if (data != undefined && data != "") {
                    var i = new Image();
                    i.src = "data:image/svg+xml;base64," + decodeURIComponent(data);
                    $(i).appendTo(signature); // append the image (SVG) to DOM. 
                }
            }
            else if (htmlOptions.format == "base30") {
                if (data != undefined && data != "") {
                    signature.jSignature({ height: htmlOptions.height });
                    signature.jSignature("setData", "data:image/jsignature;base30," + decodeURIComponent(data));
                }
            }
            else if (htmlOptions.format == "image") {
                if ($.mobile.version == '1.3.2') {
                    var a = $('<a href="javascript:void(0)" style="position:absolute" data-role="button" data-icon="refresh" data-inline="true" data-theme="b" data-corners="true" data-shadow="true" data-iconshadow="true" data-wrapperels="span" class="ui-btn ui-shadow ui-btn-corner-all ui-btn-inline ui-btn-icon-left ui-btn-up-b"><span class="ui-btn-inner"><span class="ui-btn-text">' + cancelText + '</span><span class="ui-icon ui-icon-refresh ui-icon-shadow">&nbsp;</span></span></a>');
                    var clearbutton = $(a).appendTo(signature);
                    clearbutton.unbind().bind('click', function () {
                        signature.signature('clear');
                        signature.signature('load');
                    });
                }
                else {
                    var a = $('<a href="javascript:void(0)" style="position:absolute" data-role="button" data-icon="refresh" data-inline="true" data-theme="b" data-corners="true" data-shadow="true" data-iconshadow="true" data-wrapperels="span" class="ui-link ui-btn ui-btn-b ui-icon-refresh ui-btn-icon-left ui-shadow-icon ui-btn-inline ui-shadow ui-corner-all" role="button">' + cancelText + '</a>');
                    var clearbutton = $(a).appendTo(signature);
                    clearbutton.unbind().bind('click', function () {
                        signature.signature('clear');
                        signature.signature('load');
                    });
                }
                if (data != undefined && data != "") {
                    var i = new Image();
                    //i.src = data;//data:image/png;base64,
                    i.src = decodeURIComponent(data);
                    $(i).appendTo(signature); // append the image (SVG) to DOM. 
                }
            }
            else if (htmlOptions.format == "svg") {
                if (data != undefined && data != "") {
                    data = data.replace(/ASCII3C/g, "<").replace(/ASCII3E/g, ">").replace(/ASCII3D/g, "=").replace(/ASCII22/g, "\"");
                    //data = data.replace(/&lt;/g,"<").replace(/&gt;/g,">");
                    //$(this).html(decodeURIComponent(data));
                    $(this).html(data);
                    if ($.mobile.version == '1.3.2') {
                        var a = $('<a href="javascript:void(0)" style="position:absolute" data-role="button" data-icon="refresh" data-inline="true" data-theme="b" data-corners="true" data-shadow="true" data-iconshadow="true" data-wrapperels="span" class="ui-btn ui-shadow ui-btn-corner-all ui-btn-inline ui-btn-icon-left ui-btn-up-b"><span class="ui-btn-inner"><span class="ui-btn-text">' + cancelText + '</span><span class="ui-icon ui-icon-refresh ui-icon-shadow">&nbsp;</span></span></a>');
                        var clearbutton = $(a).insertBefore(signature.children()[0]);
                        clearbutton.unbind().bind('click', function () {
                            signature.signature('clear');
                            signature.signature('load');
                        });
                    }
                    else {
                        var a = $('<a href="javascript:void(0)" style="position:absolute" data-role="button" data-icon="refresh" data-inline="true" data-theme="b" data-corners="true" data-shadow="true" data-iconshadow="true" data-wrapperels="span" class="ui-link ui-btn ui-btn-b ui-icon-refresh ui-btn-icon-left ui-shadow-icon ui-btn-inline ui-shadow ui-corner-all" role="button">' + cancelText + '</a>');
                        var clearbutton = $(a).insertBefore(signature.children()[0]);
                        clearbutton.unbind().bind('click', function () {
                            signature.signature('clear');
                            signature.signature('load');
                        });
                    }

                }
            }
        });
    }
};
//--------------------------------------------------------------------------------------------------------------------------------
$.fn.query = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.query.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).query('initialize', options);
            if (!$(this).hasClass($.fn.query.class)) {
                $(this).addClass($.fn.query.class)
            }
        });
    }
};

//query
$.fn.query.class = 'info-query';

$.fn.query.defaults = {
    clearText: "Clear"
};

$.fn.query.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var queryOptions = new Object();

            var htmlOptions = $.parseOption($(this).attr('infolight-options'));   //html option
            for (var property in htmlOptions) {
                queryOptions[property] = htmlOptions[property];
            }

            if (options != undefined) {                                     //load option
                for (var property in options) {
                    queryOptions[property] = options[property];
                }
            }
            $(this).data('options', queryOptions);

            var selectClass = [];
            selectClass.push("." + $.fn.selects.class);
            selectClass.push("." + $.fn.radiobuttons.class);
            selectClass.push("." + $.fn.checkboxes.class);
            $(selectClass.join(), this).each(function () {
                $(this).selects({});
            });
            $('.' + $.fn.refval.class, this).each(function () {
                $(this).refval({});
                $(this).refval('setValue', "");
            });
            $("." + $.fn.mdatebox.class, this).each(function () {
                $(this).mdatebox({});
            });
            $("." + $.fn.mtimebox.class, this).each(function () {
                $(this).mtimebox({});
            });
            $("." + $.fn.scan.class, this).each(function () {
                $(this).scan({});
                var queryMode = queryOptions.queryMode;
                if (queryMode == 'window')
                { }
                else {
                    $(this).scan('createButton');
                }
            });
            var viewPage = $(this).query('options').viewPage;
            var gridTheme = $("." + $.fn.datagrid.class, viewPage).attr('data-theme');
            $(":radio,:checkbox", this).each(function () {
                $(this).checkboxradio({ theme: formTheme });
            });

            $(this).query('load');
            $(this).form('clear');
            $(this).form('updateRow', $(this).form('loadDefault'));

        });
    },
    opened: function (jq) {
        jq.each(function () {
            $("." + $.fn.scan.class, this).each(function () {
                $(this).scan('createButton');
            });
            $("." + $.fn.capture.class, this).each(function () {
                $(this).capture('createButton');
            });
            $("." + $.fn.refval.class, this).each(function () {
                $(this).refval('createButton');
            });
            $("." + $.fn.place.class, this).each(function () {
                $(this).place('createButton');
            });
        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('options');
        }
        return new Object();
    },
    pageObject: function (jq) {
        if (jq.length > 0) {
            var page = $(jq[0]).closest("div[data-role='page']");
            if (page.length > 0) {
                return page;
            }
            else {
                return $(jq[0]).closest("div[data-role='dialog']");
            }
        }
        return Object();
    },
    load: function (jq) {
        if (jq.length > 0) {
            $("div.commandfooter", jq[0]).remove();
            var formTheme = $(jq[0]).attr('data-theme');
            var okText = $.fn.form.defaults.okText;
            var cancelText = $.fn.form.defaults.cancelText;
            var queryMode = $(jq[0]).query('options').queryMode;
            if (queryMode == 'panel') {
                okText = $.fn.datagrid.defaults.queryText;
                cancelText = $.fn.query.defaults.clearText;
            }


            var command = "<div class='commandfooter'>"
            + "<fieldset class='ui-grid-a'>"
            + "<div class='ui-block-a'>" + $.createTextButton(okText, formTheme, "form-ok") + "</div>"
            + "<div class='ui-block-b'>" + $.createTextButton(cancelText, formTheme, "form-cancel") + "</div>"
            + "</fieldset>"
            + "</div>";
            $(jq[0]).append(command);
            $("div.commandfooter", jq[0]).trigger("create");
            $("a.form-ok", jq[0]).bind('click', function () {
                $(jq[0]).query('query');
            });
            $("a.form-cancel", jq[0]).bind('click', function () {
                var queryMode = $(jq[0]).query('options').queryMode;
                if (queryMode != 'panel') {
                    $(jq[0]).query('close');
                }
                else {
                    $(jq[0]).form('clear');
                    $(jq[0]).form('updateRow', $(jq[0]).form('loadDefault'));
                }
            });
        }
    },
    query: function (jq) {
        if (jq.length > 0) {
            var where = '';
            var queryMode = $(jq[0]).query('options').queryMode;
            $("input,select,textarea,fieldset", jq[0]).each(function () {
                var option = $.parseOption($(this).attr('infolight-options'));
                if (option != undefined) {
                    var queryField = option.queryField;
                    if (queryField != undefined) {
                        if (option.queryTable) {
                            queryField = option.queryTable + '.' + queryField;
                        }
                        var condition = option.condition;
                        var dataType = option.dataType;
                        var isNvarChar = option.isNvarChar;
                        var value = '';
                        if ($(this).hasClass($.fn.mdatebox.class)) {
                            value = $(this).mdatebox('getValue');
                        }
                        else if ($(this).hasClass($.fn.mtimebox.class)) {
                            value = $(this).mtimebox('getValue');
                        }
                        else if ($(this).hasClass($.fn.radiobuttons.class)) {
                            value = $(this).radiobuttons('getValue');
                        }
                        else if ($(this).hasClass($.fn.checkboxes.class)) {
                            value = $(this).checkboxes('getValue');
                        }
                        else if ($(this).hasClass($.fn.refval.class)) {
                            value = $(this).refval('getValue');
                        }
                        else {
                            value = $(this).val();
                        }
                        if (queryMode == 'fuzzy') {
                            if ($(jq[0]).closest('div[data-role=\'page\']').find('input.header-query').length > 0) {
                                value = $(jq[0]).closest('div[data-role=\'page\']').find('input.header-query').val();
                            }
                            else {
                                $("input", jq[0]).each(function () {
                                    $(this).val(value);
                                });
                            }
                        }
                        if (value != '' && value != undefined) {
                            if (where.length > 0) {

                                if (queryMode == 'fuzzy') {
                                    where += " or ";
                                }
                                else {
                                    where += " and ";
                                }

                            }
                            var nvarchar = '';
                            if (isNvarChar) {
                                nvarchar = 'N';
                            }
                            var formatValue = '';
                            if (dataType == 'string') {
                                formatValue = nvarchar + "'" + value.toString().replace(/\'/g, "''") + "'"
                            }
                            else if (dataType == 'datetime') {
                                if (databaseType == 'oracle') {
                                    if (value.length > 10)
                                        formatValue = "to_date('" + value + "','yyyy-mm-dd HH24:Mi:SS')";
                                    formatValue = "to_date('" + value + "','yyyy-mm-dd')";
                                }
                                else {
                                    formatValue = "'" + value.toString().replace(/\'/g, "''") + "'";
                                }
                            }
                            else {
                                formatValue = value;
                            }

                            switch (condition) {
                                case '=':
                                case '!=':
                                case '>=':
                                case '<=':
                                case '>':
                                case '<':
                                    where += queryField + " " + condition + " " + formatValue; break;
                                case '%': where += queryField + " like " + nvarchar + "'" + value.toString().replace(/\'/g, "''") + "%'"; break;
                                case '%%': where += queryField + " like " + nvarchar + "'%" + value.toString().replace(/\'/g, "''") + "%'"; break;
                                default: break;
                            }
                        }
                    }
                }
            });

            var viewPage = $(jq[0]).query('options').viewPage;
            if (viewPage != undefined) {
                $("." + $.fn.datagrid.class, viewPage).datagrid('setWhere', where);
            }
            else {
                var refval = $(jq[0]).query('options').refval;
                $(refval).refval('setWhere', where);
            }
            $(jq[0]).query('close');
        }
    },
    close: function (jq) {
        if (jq.length > 0) {
            var viewPage = $(jq[0]).query('options').viewPage;
            if (viewPage != undefined) {
                var queryMode = $(jq[0]).query('options').queryMode;
                if (queryMode == 'window') {
                    $(jq[0]).form('pageObject').dialog('close');
                }
            }
            else {
                var refval = $(jq[0]).query('options').refval;
                $(refval).refval('closeQuery');
            }

        }
    }
};

//--------------------------------------------------------------------------------------------------------------------------------
$.fn.infoqrcode = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.infoqrcode.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).infoqrcode('initialize', options);
            if (!$(this).hasClass($.fn.infoqrcode.class)) {
                $(this).addClass($.fn.infoqrcode.class)
            }
        });
    }
};

$.fn.infoqrcode.class = 'info-qrcode';

$.fn.infoqrcode.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var qrcodeOptions = new Object();
            var htmlOptions = $.parseOption($(this).attr('infolight-options'));   //html option
            for (var property in htmlOptions) {
                qrcodeOptions[property] = htmlOptions[property];
            }

            if (options != undefined) {                                     //load option
                for (var property in options) {
                    qrcodeOptions[property] = options[property];
                }
            }
            $(this).data('options', qrcodeOptions);
        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('options');
        }
        return new Object();
    },
    setValue: function (jq, value) {
        var qrcodeOptions = new Object;
        var htmlOptions = $.parseOption(jq.attr('infolight-options')); //html option
        for (var property in htmlOptions) {
            qrcodeOptions[property] = htmlOptions[property];
        }
        var out, i, len, c;
        out = "";
        len = value.length;
        for (i = 0; i < len; i++) {
            c = value.charCodeAt(i);
            if ((c >= 0x0001) && (c <= 0x007F)) {
                out += value.charAt(i);
            } else if (c > 0x07FF) {
                out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));
                out += String.fromCharCode(0x80 | ((c >> 6) & 0x3F));
                out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
            } else {
                out += String.fromCharCode(0xC0 | ((c >> 6) & 0x1F));
                out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
            }
        }
        $(jq).css("display", "inline-block");
        //$(jq).prev("label").addClass("ui-input-text");
        qrcodeOptions["text"] = out;
        $('canvas', jq).remove();
        $('table', jq).remove();
        $(jq).qrcode(qrcodeOptions);
    }
};

//--------------------------------------------------------------------------------------------------------------------------------
//disable and enable
$.fn.enable = function () {
    if ($(this).hasClass($.fn.selects.class) && $(this).data("mobile-selectmenu") != undefined) {
        $(this).selectmenu("enable");
    }
    else if ($(this).hasClass($.fn.refval.class)) {
        $(this).attr("disabled", false);
        $(this).parent().removeClass("ui-disabled"); //for ie
        $(this).parent().parent().removeClass("ui-disabled"); //for ie
    }
    else if ($(this).hasClass($.fn.mdatebox.class) && $(this).data("mobile-datebox") != undefined) {
        $(this).datebox("enable");
        $(this).parent().parent().removeClass("ui-disabled"); //for ie
        $(this).parent().parent().removeClass("ui-state-disabled");
    }
    else if ($(this).hasClass($.fn.mtimebox.class) && $(this).data("mobile-datebox") != undefined) {
        $(this).mtimebox("enable");
        $(this).parent().parent().removeClass("ui-disabled"); //for ie
        $(this).parent().parent().removeClass("ui-state-disabled");
    }
    else if ($(this).hasClass("ui-slider-input")) {
        $(this).slider("enable");
        $(this).removeClass("ui-disabled"); //for ie
    }
    else if ($(this).hasClass("ui-slider-switch")) {
        $(this).slider("enable");
    }
    else if ($(this).hasClass($.fn.file.class) && $(this).data("mobile-textinput") != undefined) {
        $(this).file("enable");
    }
    else if ($(this).hasClass('ui-input-text') && $(this).data("mobile-textinput") != undefined) {
        $(this).textinput("enable");
    }
    else {
        $(this).attr("disabled", false);
    }
    $(this).parent().removeClass("ui-state-disabled");
};

$.fn.disable = function () {
    if ($(this).hasClass($.fn.selects.class) && $(this).data("mobile-selectmenu") != undefined) {
        $(this).selectmenu("disable");
    }
    else if ($(this).hasClass($.fn.refval.class)) {
        $(this).attr("disabled", true);
        $(this).parent().addClass("ui-disabled"); //for ie
    }
    else if ($(this).hasClass($.fn.mdatebox.class) && $(this).data("mobile-datebox") != undefined) {
        $(this).datebox("disable");
        $(this).parent().parent().addClass("ui-disabled"); //for ie
    }
    else if ($(this).hasClass($.fn.mtimebox.class) && $(this).data("mobile-datebox") != undefined) {
        $(this).datebox("disable");
        $(this).parent().parent().addClass("ui-disabled"); //for ie
    }
    else if ($(this).hasClass("ui-slider-input")) {
        $(this).slider("disable");
        $(this).addClass("ui-disabled"); //for ie
    }
    else if ($(this).hasClass("ui-slider-switch")) {
        $(this).slider("disable");
    }
    else if ($(this).hasClass($.fn.file.class) && $(this).data("mobile-textinput") != undefined) {
        $(this).file("disable");
    }
    else if ($(this).hasClass('ui-input-text') && $(this).data("mobile-textinput") != undefined) {
        $(this).textinput("disable");
    }
    else {
        $(this).attr("disabled", true);
    }
};
//--------------------------------------------------------------------------------------------------------------------------------
//default
$.fn.defaultValue = function (methodName, param) {
    if (typeof methodName == "string") {
        var method = $.fn.defaultValue.methods[methodName];
        if (method) {
            return method(param);
        }
    }
};


$.fn.defaultValue.methods = {
    constant: function (param) {
        return param[0];
    },
    field: function (param) {
        var field = param[0];
        var row = param[1];
        return row[field];
    },
    client: function (param) {
        return eval(param[0]).call(this);
    },
    remote: function (param) {
        var obj = "";
        $.ajax({
            type: "POST",
            url: window.currentUrl,
            data: "mode=default&method=" + $.toJSONString(param),
            cache: false,
            async: false,
            success: function (data) {
                obj = data;
            }, error: function (data) {
                obj = "";
            }
        });
        return obj;
    }
};
//--------------------------------------------------------------------------------------------------------------------------------
//validate
$.fn.validate = function (methodName, value, param) {
    if (typeof methodName == "string") {
        var method = $.fn.validate.methods[methodName];
        if (method) {
            return method(this, value, param);
        }
    }
};

$.fn.validate.class = "info-validate";

$.fn.validate.defaults = {
    valueNotNullMessage: " can not be empty",
    valueNotLessThanMessage: " should be less than ",
    valueNotGreaterThanMessage: " should be greater than ",
    valueNotInRangeMessage: " is not in range",
    valueDuplicateMessage: "record already exist in database",
    valueNotidCardMessage: "input idCard is not valid.",
    valueNotUrlMessage: "input url is not valid.",
    valueNotemailMessage: "input email is not valid."
};

$.fn.validate.methods = {
    client: function (jq, value, param) {
        var val = eval(param[0]).call(this, value);
        return val == true;
    },
    remote: function (jq, value, param) {
        var val = false;
        $.ajax({
            type: "POST",
            url: window.currentUrl,
            data: "mode=validate&method=" + param[0] + "&value=" + value,
            cache: false,
            async: false,
            success: function (data) {
                if (data.toLowerCase() == 'true') {
                    val = true;
                }
            }, error: function (data) {
                val = "";
            }
        });
        return val == true;
    },
    greater: function (jq, value, param) {
        if (jq.length > 0) {
            jq[0].validate.message = jq[0].caption + $.fn.validate.defaults.valueNotGreaterThanMessage + param[0];
        }
        if (!isNaN(parseFloat(value)) && !isNaN(parseFloat(param[0]))) {
            return parseFloat(value) >= parseFloat(param[0]);
        }
        if (value < param[0]) {
            return false;
        }
        return true;
    },
    less: function (jq, value, param) {
        if (jq.length > 0) {
            jq[0].validate.message = jq[0].caption + $.fn.validate.defaults.valueNotLessThanMessage + param[0];
        }
        if (!isNaN(parseFloat(value)) && !isNaN(parseFloat(param[0]))) {
            return parseFloat(value) <= parseFloat(param[0]);
        }
        if (value > param[0]) {
            return false;
        }
        return true;
    },
    range: function (jq, value, param) {
        if (jq.length > 0) {
            jq[0].validate.message = jq[0].caption + $.fn.validate.defaults.valueNotInRangeMessage + "(" + param[0] + "-" + param[1] + ")";
        }
        if (!isNaN(parseFloat(value)) && !isNaN(parseFloat(param[0])) && !isNaN(parseFloat(param[1]))) {
            return parseFloat(value) >= parseFloat(param[0]) && parseFloat(value) <= parseFloat(param[1]);
        }
        if (param[0] != '' && value < param[0]) {
            return false;
        }
        if (param[1] != '' && value > param[1]) {
            return false;
        }
        return true;
    },
    idCard: function (jq, value, param) {
        if (jq.length > 0) {
            jq[0].validate.message = jq[0].caption + $.fn.validate.defaults.valueNotidCardMessage;
        }
        if (value == '' || value == undefined) {
            return true;
        }
        if (IDCheck(value) == true)
            return true;
        else
            return false;
    },
    url: function (jq, value, param) {
        if (jq.length > 0) {
            jq[0].validate.message = jq[0].caption + $.fn.validate.defaults.valueNotUrlMessage;
        }
        if (value == '' || value == undefined) {
            return true;
        }

        if (CheckURLFormat(value) == true)
            return true;
        else
            return false;
    },
    email: function (jq, value, param) {
        if (jq.length > 0) {
            jq[0].validate.message = jq[0].caption + $.fn.validate.defaults.valueNotemailMessage;
        }
        if (value == '' || value == undefined) {
            return true;
        }

        if (CheckEmailFormat(value) == true)
            return true;
        else
            return false;
    }
};


//身分證號 檢核函數
function IDCheck(sIDNO) {
    if (sIDNO.length != 10)
        return false;
    var local_table = [10, 11, 12, 13, 14, 15, 16, 17, 34, 18, 19, 20, 21,
                       22, 35, 23, 24, 25, 26, 27, 28, 29, 32, 30, 31, 33];
    /* A, B, C, D, E, F, G, H, I, J, K, L, M,
    N, O, P, Q, R, S, T, U, V, W, X, Y, Z */
    var local_digit = local_table[sIDNO.charCodeAt(0) - 'A'.charCodeAt(0)];
    var checksum = 0;
    checksum += Math.floor(local_digit / 10);
    checksum += (local_digit % 10) * 9;
    for (var i = 1, p = 8; i <= 8; i++, p--) {
        checksum += parseInt(sIDNO.charAt(i)) * p;
    }
    checksum += parseInt(sIDNO.charAt(9));
    return !(checksum % 10);
}

//Email 檢核函數
function CheckEmailFormat(sEmail) {
    var re = /^([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
    return re.test(sEmail);
}

//URL 檢核函數
function CheckURLFormat(sURL) {
    var strRegex = "^((https|http|ftp|rtsp|mms)?://)"
                 + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" // ftp的user@
                 + "(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP形式的URL- 199.194.52.184
                 + "|" // 允許IP和DOMAIN（域名）
                 + "([0-9a-z_!~*'()-]+\.)*" // 域名- www.
                 + "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // 二級域名
                 + "[a-z]{2,6})" // first level domain- .com or .museum
                 + "(:[0-9]{1,4})?" // 端口- :80
                 + "((/?)|" // a slash isn't required if there is no file name
                 + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
    var re = new RegExp(strRegex);
    return re.test(sURL);
}

//--------------------------------------------------------------------------------------------------------------------------------

$.fn.warning = function (methodName, value, param) {
    if (typeof methodName == "string") {
        var method = $.fn.warning.methods[methodName];
        if (method) {
            return method(value, param);
        }
    }
};

$.fn.warning.methods = {
    show: function (message) {
        var errorWindow = $("#errorPopup", "body");
        if (errorWindow.length == 0) {
            var popup = "<div id='errorPopup' data-role='popup' class='ui-content'><p></p></div>";
            $("body").append(popup);
            errorWindow = $("#errorPopup", "body");
        }
        $("p", errorWindow).html(message);
        errorWindow.popup({});
        errorWindow.popup("option", "overlayTheme", "a");
        errorWindow.popup("option", "theme", "d");
        errorWindow.popup("open");
    }
};

$.messager = {};


$.messager.confirm = function (title, msg, fn) {
    var dialog = $('<div data-role="popup" data-theme="b" data-dismissible="false" class="ui-content" style="max-width:200px"><div data-role="content"><p>' + msg + '</p><a href="#" class="popup-ok" data-role="button" data-inline="true" data-rel="back" data-mini="true" data-theme="b">' + $.fn.form.defaults.okText + '</a><a  href="#" class="popup-cancel" data-role="button" data-inline="true" data-rel="back" data-mini="true" data-theme="c">' + $.fn.form.defaults.cancelText + '</a></div></div>');
    $.mobile.activePage.append(dialog);
    dialog.trigger("create");
    dialog.popup();
    dialog.find('a.popup-ok').bind('click', function () {
        if (fn) {
            fn(true);
        }
    });
    dialog.find('a.popup-cancel').bind('click', function () {
        if (fn) {
            fn();
        }
    });
    dialog.popup('open');
}


//set multilanguage
$.sysmsg = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.sysmsg.methods[methodName];
        if (method) {
            return method(value);
        }
    }
};

$.sysmsg.messages = {};

$.sysmsg.methods = {
    load: function (keys) {
        $.ajax({
            type: "POST",
            url: window.currentUrl,
            dataType: 'json',
            data: "mode=message&keys=" + $.toJSONString(keys),
            cache: false,
            async: false,
            success: function (data) {
                for (var key in data) {
                    $.sysmsg.messages[key] = data[key];
                }
            }, error: function (data) {


            }
        });
    },
    getValue: function (key) {
        if ($.sysmsg.messages[key]) { }
        else {
            var keys = [];
            keys.push(key);
            $.sysmsg('load', keys);
        }
        return $.sysmsg.messages[key];
    }
}


//exception method
$(document).ajaxError(function (event, jqXHR, ajaxSettings, thrownError) {

});

Request = {
    QueryString: function (item) {
        var svalue = location.search.match(new RegExp("[\?\&]" + item + "=([^\&]*)(\&?)", "i"));
        return svalue ? decodeURI(svalue[1]) : decodeURI(svalue);
    },
    //获取QueryString的数组 
    getQueryString: function () {
        var result = location.search.match(new RegExp("[\?\&][^\?\&]+=[^\?\&]+", "g"));
        for (var i = 0; i < result.length; i++) {
            result[i] = result[i].substring(1);
        }
        return decodeURI(result);
    },
    //根据QueryString参数名称获取值 
    getQueryStringByName: function (name) {
        var queryString = $('#_PARAMETERS').val();
        if (queryString) {
            queryString = "?" + queryString;
            var result = queryString.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
            if (result == null || result.length < 1) {
                return "";
            }
            return decodeURI(result[1]).replace(/markand/g, '&');
        }
        else if (location.search != "") {
            var queryString = location.search;
            var result = queryString.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
            if (result == null || result.length < 1) {
                return "";
            }
            return decodeURI(result[1]).replace(/markand/g, '&');
        }
        else {
            return '';
        }
    },
    //根据QueryString参数索引获取值 
    getQueryStringByIndex: function (index) {
        if (index == null) {
            return "";
        }
        var queryStringList = getQueryString();
        if (index >= queryStringList.length) {
            return "";
        }
        var result = queryStringList[index];
        var startIndex = result.indexOf("=") + 1;
        result = result.substring(startIndex);
        return decodeURI(result);
    },
    getFlowStringByName: function (key, name) {
        var queryString = localStorage.getItem(key);
        if (queryString) {
            queryString = "?" + queryString;
            var result = queryString.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
            if (result == null || result.length < 1) {
                return "";
            }
            return decodeURI(result[1]);
        }
        else {
            return '';
        }
    }
}

//seq method
function getSeq(grid, form, rowData) {
    if (rowData == undefined)
        rowData = new Object();
    var element = grid;
    var eacht = "th,input,select,textarea";
    if (form != undefined) {
        element = form;
        eacht = "input,select,textarea";
    }
    $(eacht, element).each(function () {
        var column = $(this);
        var options = $.parseOption(column.attr('infolight-options'));
        var autoSeq = options.autoSeq;
        if (autoSeq != undefined) {
            var field = options.field;
            if (autoSeq.length > 0) {
                var numDig = autoSeq[0].numDig;
                var startValue = autoSeq[0].startValue;
                var step = autoSeq[0].step;
                var viewPage = form.form('options').viewPage;
                var grid = $("." + $.fn.datagrid.class, viewPage);
                var url = $.getUrl(options.remoteName, options.tableName); //$(grid).datagrid('options').url;
                var queryWord = ""; //$(grid).datagrid('options').queryParams.queryWord;
                var parent = $.parseOption(grid.attr('infolight-options')).parentObjectID;
                if (parent != undefined) {
                    var Rows = $(grid).datagrid("getData");
                    //var insertdRows = $(grid).datagrid("getChanges", "inserted");
                    var maxvalue = eval(startValue - parseInt(step));
                    for (var i = 0; i < Rows.length; i++) {
                        var oldvalue = Rows[i][field];
                        if (oldvalue != undefined) {
                            var oldvaluelength = oldvalue.length;
                            for (var j = 0; j < oldvaluelength; j++) {
                                if (oldvalue.indexOf("0") == 0) {
                                    oldvalue = oldvalue.substring(1);
                                }
                            }
                            var valuei = parseInt(oldvalue);
                            if (isNaN(valuei)) {
                                maxvalue = eval(startValue - parseInt(step));
                            }
                            else if (maxvalue == undefined) {
                                maxvalue = valuei
                            }
                            else {
                                if (maxvalue < valuei) {
                                    maxvalue = valuei;
                                }
                            }
                        }
                    }
                    maxvalue = eval(maxvalue + parseInt(step));
                    var length = maxvalue.toString().length;
                    if (numDig > maxvalue.toString().length) {
                        for (var j = 0; j < numDig - length; j++) {
                            maxvalue = "0" + maxvalue;
                        }
                    }
                    rowData[field] = maxvalue;
                }
                else {
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: "mode=seq&numDig=" + numDig + "&startValue=" + startValue + "&step=" + step + "&field=" + field + "&queryWord=" + queryWord,
                        cache: false,
                        async: false,
                        success: function (data) {
                            rowData[field] = data;
                        }
                    });
                }
            }
        }
    });
    return rowData;
};
//----------------------------------------------------------------------------------------------------------------

$.convertRemark = function (remark, flag) {
    if (remark != undefined && remark != null) {
        if (flag)
            remark = remark.replace(/\?/g, "gimonnhu");
        else
            remark = remark.replace(/gimonnhu/g, "?");
        remark = encodeURIComponent(remark);
    }
    return remark;
};
$.fn.drilldown = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.drilldown.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
};
$.fn.drilldown.methods = {
    load: function (jq) {
        jq.each(function () {
            var drilldown = $.parseOption($(this).attr('infolight-options')).drillobjectid;
            if ($('#' + drilldown)) {
                drilldown = $('#' + drilldown);
                var drillfieldsstring = $.parseOption($(this).attr('infolight-options')).drillfields;
                var drillfields = drillfieldsstring;
                var datagrid = $(this).closest('.info-datagrid');
                var opt = { id: datagrid.datagrid('pageObject').attr('id') };
                opt.pageNumber = datagrid.datagrid('options').pageNumber;
                opt.whereString = datagrid.datagrid('options').whereString;
                window.sessionStorage.setItem('gridOpt', JSON.stringify(opt));
                drilldown.drilldown('showDrillDown', drillfields);
            }
        });
    },
    showDrillDown: function (jq, values) {
        jq.each(function () {
            var drilldown = $(this);
            var beforeDrillDown = $.parseOption(drilldown.attr('infolight-options')).beforeDrillDown;
            if (beforeDrillDown != undefined) {
                var flag = beforeDrillDown.call(drilldown, values);
                if (flag != undefined && flag.toString() == 'false') {
                    return;
                }
            }
            var drillStyle = $.parseOption(drilldown.attr('infolight-options')).drillStyle;
            var tableName = $.parseOption(drilldown.attr('infolight-options')).tableName;
            var remoteName = $.parseOption(drilldown.attr('infolight-options')).remoteName;
            var keyFields = $.parseOption(drilldown.attr('infolight-options')).keyFields;
            var where = "";
            if (values != undefined) {
                for (var i = 0; i < values.length; i++) {
                    if (keyFields[i] != undefined) {
                        var value = values[i].value;
                        var field = keyFields[i].field;
                        if (where != "") where += ";";
                        where += field + "='" + value + "'";
                    }
                }
            }

            if (drillStyle == 'webform') {
                var formname = $.parseOption(drilldown.attr('infolight-options')).formName;
                if (formname.indexOf("~/") == 0) {
                    formname = ".." + formname.slice(1);
                }
                if (formname.indexOf(".aspx") == -1) {
                    formname = formname + ".aspx";
                }

                var developer = $('#_DEVELOPERID').val();
                if (developer) {
                    formname = 'SD_' + developer + '_' + formname;
                }

                if (formname != "") {
                    //var openMode = $.parseOption(drilldown.attr('infolight-options')).openMode;
                    var urlParam = "?DRILLDOWN=true&REMOTENAME=" + encodeURIComponent(remoteName) + "&TABLENAME=" + encodeURIComponent(tableName) + "&DRILLDOWN_KEYFIELD=" + encodeURIComponent(where);
                    //var url = "../handler/SystemHandle.ashx?Type=Encrypt";
                    //if (developer) {
                    //    url = "../handler/SystemHandler.ashx?type=Encrypt";
                    //}
                    //$.ajax({
                    //    url: url,
                    //    data: { param: urlParam },
                    //    type: 'post',
                    //    async: true,
                    //    success: function (param) {
                    var newurl = formname + urlParam;
                    window.location.href = newurl;
                    //}
                    //});
                }
            }
            else if (drillStyle == 'mobileform') {
                var formname = $.parseOption(drilldown.attr('infolight-options')).formName;
                if (formname.indexOf("~/") == 0) {
                    formname = ".." + formname.slice(1);
                }
                if (formname.indexOf(".aspx") == -1) {
                    formname = formname + ".aspx";
                }

                var developer = $('#_DEVELOPERID').val();
                if (developer) {
                    formname = 'SD_' + developer + '_' + formname;
                }
                if (formname != "") {
                    var urlParam = "?DRILLDOWN=true&REMOTENAME=" + encodeURIComponent(remoteName) + "&TABLENAME=" + encodeURIComponent(tableName) + "&DRILLDOWN_KEYFIELD=" + encodeURIComponent(where);
                    //var url = "../handler/SystemHandle.ashx?Type=Encrypt";
                    //if (developer) {
                    //    url = "../handler/SystemHandler.ashx?type=Encrypt";
                    //}
                    var newurl = formname + urlParam;
                    //window.open(newurl);
                    window.location.href = newurl;
                    //$.mobile.changePage(newurl, { transition: "slideup", role: "page" });
                }
            }
            else if (drillStyle.toLowerCase() == "rdlc") {
                var reportName = $.parseOption(drilldown.attr('infolight-options')).reportName;
                if (reportName != undefined) {
                    var developer = $('#_DEVELOPERID').val();
                    if (developer) {
                        reportName = 'SD_' + developer + '_' + reportName;
                        reportName = 'preview' + developer + '/' + reportName;
                    }
                    if (reportName.indexOf(".rdlc") == -1) {
                        reportName = reportName + ".rdlc";
                    }
                    var url = "../ReportViewerTemplate.aspx?RemoteName=" + remoteName + "&TableName=" + tableName + "&ReportPath=" + reportName + "&WhereString=" + encodeURIComponent(where);
                    window.location.href = url;
                }
            }
            else if (drillStyle == "command") {
                var DisplayFields = $.parseOption(drilldown.attr('infolight-options')).displayFields;
                var FormCaption = $.parseOption(drilldown.attr('infolight-options')).formCaption;
                if (FormCaption == undefined || FormCaption == "") {//空白的title会导致dialog上方的标题行消失，会没有关闭的大叉
                    FormCaption = 'Drill Down Dialog';
                }
                if (DisplayFields != undefined && DisplayFields.length > 0) {
                    var div = $('<div data-overlay-theme="b" data-role="page"/>').appendTo('body');
                    $('<div data-role="header" data-theme="b"><h1>' + FormCaption + '</h1></div>').appendTo(div);
                    var div2 = $('<div data-theme="b" data-role="content"></div>').appendTo(div);
                    $('<div class="ui-content" id="dataFormMaster_popup" data-role="popup" data-theme="d" data-overlay-theme="a"><a data-rel="back" data-role="button" data-icon="delete" data-iconpos="notext" class="ui-btn-right">Close</a><p></p></div>').appendTo(div2);
                    var datagrid = $('<table class="info-datagrid table-stripe infolight-breakpoint" data-role="table" data-mode="reflow" data-theme="b" infolight-options="remoteName:\'' + remoteName + '\',tableName:\'' + tableName + '\',title:\'' + FormCaption + '\'"/>').appendTo(div2);
                    var thead = $('<thead></thead>').appendTo(datagrid);
                    var tr = $('<tr></tr>').appendTo(thead);
                    for (var i = 0; i < DisplayFields.length; i++) {
                        var formater = "";
                        if (DisplayFields[i].drillObjectID != undefined && DisplayFields[i].drillObjectID != "") {
                            var drillfieldstring = "";
                            for (var j = 0; j < DisplayFields[i].drillFields.length; j++) {
                                if (drillfieldstring != "") drillfieldstring += ";";
                                drillfieldstring += DisplayFields[i].drillFields[j].field;
                            }
                            if (drillfieldstring == "") drillfieldstring = DisplayFields[i].field;
                            drillfieldstring = "formatParameters:'fullRow',format:'drilldown,drillObjectID:'" + DisplayFields[i].drillObjectID + "',drillFields:'" + drillfieldstring + "'";
                        }
                        $('<th nowrap="nowrap" infolight-options="field:\'' + DisplayFields[i].field + '\',width:' + DisplayFields[i].width + ',align:\'left\',nowrap:false">' + DisplayFields[i].caption + '</th>').appendTo(tr);
                    }

                    datagrid.datagrid({});
                    datagrid.datagrid('setWhere', where);
                    $.mobile.changePage(div, { transition: "pop", role: "dialog" });
                }
            }
        });
    }
}

//----------------------------------------------------------------------------------------------------------------
$.fn.rotator = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.rotator.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
}
$.fn.rotator.methods = {
    slide: function (jq) {
        var rotator = $(jq[0]);
        var options = $.parseOption(rotator.attr('infolight-options'));

        options.whereString = "";
        var beforeload = options.onBeforeLoad;
        if (beforeload) {
            beforeload.call(rotator, options);
        }

        var menu = options.menuID;
        if (menu) {
            var rotatorType = options.rotatorType;
            var remoteName = options.remoteName;
            var tableName = options.tableName;
            var fieldName = options.fieldName;
            var queryWord = new Object();
            queryWord.whereString = options.whereString;

            var obj;
            $.ajax({
                type: "POST",
                url: $.getUrl(remoteName, tableName),
                data: 'remoteName=' + remoteName + '&queryWord=' + $.toJSONString(queryWord),
                cache: false,
                async: false,
                success: function (data) {
                    obj = $.parseJSON(data);
                    obj = obj.rows;
                    var items = [];
                    for (var i = 0; i < obj.length; i++) {
                        if (rotatorType == "text")
                            items.push({ text: obj[i][fieldName] });
                        else if (rotatorType == "image") {
                            var folder = options.imageFolder;
                            if (folder.toLowerCase().indexOf("\\") == 0 || folder.toLowerCase().indexOf("/") == 0) {
                                folder = folder.substring(1);
                            }
                            else if (folder.substring(folder.length - 1).toLowerCase() == "\\" || folder.substring(folder.length - 1).toLowerCase() == "/") {
                                folder = folder.substring(0, folder.length - 1);
                            }
                            var developer = $('#_DEVELOPERID').val();

                            folder = "../" + (developer ? ('preview' + developer + '/') : '') + folder + "/";
                            var vpath = folder + obj[i][fieldName];

                            items.push({ imgUrl: vpath });
                        }
                    }
                    $.metro.changeItemsImageText("Menu" + menu, items);
                }, error: function (data) {
                }
            });
        }
    }
}

$.fn.capture = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.capture.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).capture('initialize', options);
            if (!$(this).hasClass($.fn.capture.class)) {
                $(this).addClass($.fn.capture.class)
            }
        });
    }
};

$.fn.capture.class = 'info-capture';

$.fn.capture.methods = {
    initialize: function (jq, options) {

    },
    createButton: function (jq) {
        jq.each(function () {
            if ($(this).next('a').length == 0) {
                $(this).css('display', 'inline-block');
                $(this).css('margin-right', '-40px');
                var input = $(this);
                $('<a href="#" class="ui-input-clear ui-btn ui-icon-camera ui-btn-icon-notext ui-shadow ui-corner-all" role="button" style="vertical-align: middle; display: inline-block;"></a>')
                .insertAfter(this)
                .click(function () {
                    try {
                        if (navigator.device == null) {
                            var smg = $.sysmsg('getValue', 'Web/JQMobile/MobileOnly');
                            window.alert(smg);
                        }
                        else {
                            navigator.device.capture.captureImage(captureSuccess, captureError, { limit: 1 });

                            function captureSuccess(mediaFiles) {
                                for (var i = 0; i < mediaFiles.length; i += 1) {
                                    var mediaFile = mediaFiles[i];
                                    var serverUri = encodeURI(webSiteUrl + '/handler/file_upload.ashx');

                                    var ft = new FileTransfer(),
                                        path = mediaFile.fullPath,
                                        name = mediaFile.name;

                                    ft.upload(path,
                                        serverUri,
                                        function (result) {
                                            input.val(mediaFile.name);
                                        },
                                        function (error) {
                                            alert("fail");
                                            alert("An error has occurred: Code = " + error.code + "upload error source " + error.source
                                                + "upload error target " + error.target);
                                            alert(error.body);
                                            //console.log('Error uploading file ' + path + ': ' + error.code);
                                        },
                                        { fileName: name });
                                }
                            }
                            function captureError(error) {
                                var msg = 'An error occurred during capture: ' + error.code;
                                navigator.notification.alert(msg, null, 'Uh oh!');
                            }
                        }
                    }
                    catch (ex) {
                        alert(ex.message);
                    }
                });
            }
        });
    }
};

$.fn.place = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.place.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).place('initialize', options);
            if (!$(this).hasClass($.fn.place.class)) {
                $(this).addClass($.fn.place.class)
            }
        });
    }
};

$.fn.place.class = "info-place";

$.fn.place.methods = {
    initialize: function (jq, options) {
        if (options == undefined) {
            options = new Object();
            $(jq).place('initialize', options);
        }
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('options');
        }
        return new Object();
    },
    getValue: function (jq) {
        if (jq.length > 0) {
            return $(jq[0]).data("value");
            //return $(jq[0]).val();
        }
    },
    setValue: function (jq, value) {
        jq.each(function () {
            var place = $(this);
            place.data("value", value);

            var options = place.place('options');
            $(this).val(value);
        });
    },
    open: function (jq) {
        var place = $(jq[0]);
        var htmlOptions = $.parseOption(place.attr('infolight-options'));
        var geoFormat = htmlOptions.geoFormat;
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
        function (position) {
            var currentLat = position.coords.latitude;
            var currentLon = position.coords.longitude;
            var popupid = place.attr('id') + "_popup";
            var geomap = $('#' + place.attr("ID") + "_map", "#" + popupid);
            var mapholder = document.getElementById(place.attr("ID") + "_map");
            var searchbox = $('#' + place.attr("ID") + "_search", "#" + popupid);
            if ($("li", "#" + popupid).length > 0) {
                $("li", "#" + popupid).remove();
            }

            if (typeof google != "undefined") {
                var latlon = new google.maps.LatLng(currentLat, currentLon)
                var myOptions = {
                    center: latlon, zoom: 15,
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    mapTypeControl: false,
                    navigationControlOptions: { style: google.maps.NavigationControlStyle.SMALL }
                };
                geomap.height(200);
                geomap.width(300);
                geomap.css("display", "inline-block");
                var map = new google.maps.Map(mapholder, myOptions);
                var marker = new google.maps.Marker({ position: latlon, map: map, title: "You are here!" });
                $("div.placefooter", "#" + popupid).remove();
                var formTheme = $("#" + popupid).attr('data-overlay-theme');
                var command = "<div class='placefooter'>"
                + "<fieldset class='ui-grid-a'>"
                + "<div class='ui-block-a'>" + $.createTextButton($.fn.form.defaults.okText, formTheme, "form-ok") + "</div>"
                + "<div class='ui-block-b'>" + $.createTextButton($.fn.form.defaults.cancelText, formTheme, "form-cancel") + "</div>"
                + "</fieldset>"
                + "</div>";
                $(command).insertBefore($("ul", "#" + popupid));
                $("div.placefooter", '#' + popupid).trigger("create");
                $("a.form-ok", '#' + popupid).bind('click', function () {
                    //warning
                    //place.val($("input", '#' + popupid).val());
                    var value = place.data('MarkerValue');
                    if (value != undefined && value != "") {
                        place.place('setValue', value);
                        place.data('MarkerValue', "");
                        $("#" + popupid).popup("close");
                    }
                    else {
                        var value2 = place.data('MapClickValue');
                        if (value2 != undefined && value2 != "") {
                            place.place('setValue', value2);
                            place.data('MapClickValue', "");
                            $("#" + popupid).popup("close");
                        }
                    }
                });
                $("a.form-cancel", '#' + popupid).bind('click', function () {
                    $("#" + popupid).popup("close");
                });
                $("#" + popupid).popup("open");

                var service = new google.maps.places.PlacesService(map);
                service.nearbySearch({
                    location: { lat: currentLat, lng: currentLon },
                    radius: 100
                }, function (results, status) {
                    if (status === google.maps.places.PlacesServiceStatus.OK) {
                        var markers = [];
                        for (var i = 0; i < results.length; i++) {
                            var placeeach = results[i];
                            var placeLoc = placeeach.geometry.location;
                            var name = placeeach.name;
                            var li = $('<li in index="' + i + '"></li>').appendTo($("ul", "#" + popupid));
                            var a = $('<a class="list-select">' + name + '</a>').appendTo(li);
                            var p1 = $('<p>' + placeeach.vicinity + '</p>').appendTo(a);
                            //$('<p>' + direction + '    ' + distance + '</p>').appendTo(a);
                            $("li[index='" + i + "']", "#" + popupid).data("rowData", placeeach);
                            var marker2 = new google.maps.Marker({
                                map: map,
                                position: placeeach.geometry.location
                            });
                            place.place('markerclick', { marker: marker2, map: map, content: placeeach.name, position: placeeach.geometry.location });
                            markers[i] = marker2;
                        }
                        place.data('markers', markers);
                        if ($("ul", "#" + popupid).hasClass('ui-listview')) {
                            $("ul", "#" + popupid).listview("refresh");
                        }
                        $("ul", "#" + popupid).on('click', 'li', function () {
                            var placeeach = $(this).closest('li').data("rowData");
                            var placeLoc = placeeach.geometry.location;
                            if (geoFormat == "Geo")
                                place.place('setValue', placeLoc.lng() + "," + placeLoc.lat());
                            else
                                place.place('setValue', placeeach.name);
                            $("#" + popupid).popup("close");
                            return false;
                        });
                    }
                });
                searchbox.unbind().bind('blur', function () {
                    var search = searchbox.val();
                    if ($("li", "#" + popupid).length > 0) {
                        $("li", "#" + popupid).remove();
                    }
                    var service2 = new google.maps.places.PlacesService(map);
                    service2.nearbySearch({
                        location: { lat: currentLat, lng: currentLon },
                        radius: 1000,
                        keyword: search
                    }, function (results, status) {
                        if (status === google.maps.places.PlacesServiceStatus.OK) {
                            var oldmarkers = place.data('markers');
                            for (var i = 0; i < oldmarkers.length; i++) {
                                oldmarkers[i].setMap(null);
                            }
                            var markers = [];
                            for (var i = 0; i < results.length; i++) {
                                var placeeach = results[i];
                                var placeLoc = placeeach.geometry.location;
                                var name = placeeach.name;
                                var li = $('<li in index="' + i + '"></li>').appendTo($("ul", "#" + popupid));
                                var a = $('<a class="list-select">' + name + '</a>').appendTo(li);
                                var p1 = $('<p>' + placeeach.vicinity + '</p>').appendTo(a);
                                //$('<p>' + direction + '    ' + distance + '</p>').appendTo(a);
                                $("li[index='" + i + "']", "#" + popupid).data("rowData", placeeach);
                                var marker2 = new google.maps.Marker({
                                    map: map,
                                    position: placeeach.geometry.location
                                });
                                place.place('markerclick', { marker: marker2, map: map, content: placeeach.name, position: placeeach.geometry.location });
                                markers[i] = marker2;
                            }
                            place.data('markers', markers);
                            if ($("ul", "#" + popupid).hasClass('ui-listview')) {
                                $("ul", "#" + popupid).listview("refresh");
                            }
                            $("ul", "#" + popupid).on('click', 'li', function () {
                                var placeeach = $(this).closest('li').data("rowData");
                                var placeLoc = placeeach.geometry.location;
                                if (geoFormat == "Geo")
                                    place.place('setValue', placeLoc.lng() + "," + placeLoc.lat());
                                else
                                    place.place('setValue', placeeach.name);
                                $("#" + popupid).popup("close");
                                return false;
                            });
                        }
                    });
                });

            }
            else if (typeof BMap != "undefined") {
                var ak = $('#_BAIDUAK').val();
                //var url = "http://api.map.baidu.com/location/ip?ak=" + ak + "&coor=bd09ll";
                //$.ajax({
                //    type: "GET",
                //    url: url,
                //    cache: false,
                //    async: false,
                //    success: function (data) {
                //        if (data.status == 0) {
                //            alert('b');
                //            alert(data.address);
                //            alert(data.content.point.x);
                //            alert(data.content.point.y);
                //        }
                //    }
                //});
                //var url = "http://api.map.baidu.com/geoconv/v1/?coords=" + currentLon + "," + currentLat + "&from=3&ak=" + ak;
                //$.ajax({
                //    type: "GET",
                //    url: url,
                //    cache: false,
                //    async: false,
                //    success: function (data) {
                //        if (data.status == 0) {
                //currentLon = data.result.x;
                //currentLat = data.result.y;
                var url2 = "http://api.map.baidu.com/geocoder/v2/?ak=" + ak + "&location=" + currentLat + "," + currentLon + "&output=json&pois=1&coordtype=wgs84ll";
                $.ajax({
                    type: "GET",
                    url: url2,
                    cache: false,
                    async: false,
                    dataType: 'jsonp',
                    success: function (data) {
                        if (data.status == 0) {
                            var address = data.result.formatted_address;
                            $("div.placefooter", "#" + popupid).remove();
                            var formTheme = $("#" + popupid).attr('data-overlay-theme');
                            var command = "<div class='placefooter'>"
                            + "<fieldset class='ui-grid-a'>"
                            + "<div class='ui-block-a'>" + $.createTextButton($.fn.form.defaults.okText, formTheme, "form-ok") + "</div>"
                            + "<div class='ui-block-b'>" + $.createTextButton($.fn.form.defaults.cancelText, formTheme, "form-cancel") + "</div>"
                            + "</fieldset>"
                            + "</div>";
                            $(command).insertBefore($("ul", "#" + popupid));
                            $("div.placefooter", '#' + popupid).trigger("create");
                            $("a.form-ok", '#' + popupid).bind('click', function () {
                                //warning
                                //place.val($("input", '#' + popupid).val());
                                var value = place.data('MarkerValue');
                                if (value != undefined && value != "") {
                                    place.place('setValue', value);
                                    place.data('MarkerValue', "");
                                    $("#" + popupid).popup("close");
                                }
                                else {
                                    var value2 = place.data('MapClickValue');
                                    if (value2 != undefined && value2 != "") {
                                        place.place('setValue', value2);
                                        place.data('MapClickValue', "");
                                        $("#" + popupid).popup("close");
                                    }
                                }
                            });
                            $("a.form-cancel", '#' + popupid).bind('click', function () {
                                $("#" + popupid).popup("close");
                            });

                            $("#" + popupid).popup("open");

                            //map
                            geomap.height(200);
                            geomap.width(300);
                            geomap.css("display", "inline-block");
                            var id = geomap.attr('id');
                            var map = new BMap.Map(id);
                            var resultPoint = new BMap.Point(data.result.location.lng, data.result.location.lat);
                            map.centerAndZoom(resultPoint, 15);
                            //map.centerAndZoom(point2, 15);
                            map.enableDragging();
                            var markerhere = new BMap.Marker(resultPoint);
                            map.addOverlay(markerhere);
                            var labelhere = new BMap.Label(address, { offset: new BMap.Size(20, -10) });
                            markerhere.setLabel(labelhere);
                            var gc = new BMap.Geocoder();
                            map.addEventListener("click", function (e) {
                                var value = place.data('MarkerValue');
                                if (value != undefined && value != "") { }
                                else {
                                    gc.getLocation(e.point, function (rs) {
                                        var addComp = rs.addressComponents;
                                        nidizhi = rs.address; //addComp.province + ", " + addComp.city + ", " + addComp.district + ", " + addComp.street + ", " + addComp.streetNumber;
                                        if (geoFormat == "Geo")
                                            place.data('MapClickValue', e.point.lng + "," + e.point.lat);
                                        else
                                            place.data('MapClickValue', nidizhi);
                                    });
                                }
                            });
                            //map.setCenter(new BMap.Point(data.result.location.lng, data.result.location.lat));
                            //endmap
                            //setmarker
                            var markers = [];
                            for (var i = 0; i < data.result.pois.length; i++) {
                                var s = data.result.pois[i];
                                var name = s.name;
                                var addr = s.addr;
                                var direction = s.direction;
                                var distance = s.distance;
                                var poiType = s.poiType;
                                var cp = s.cp;
                                var point = s.point;
                                //listview
                                var li = $('<li in index="' + i + '"></li>').appendTo($("ul", "#" + popupid));
                                var a = $('<a class="list-select">' + name + '</a>').appendTo(li);
                                var p1 = $('<p>' + addr + '</p>').appendTo(a);
                                $("li[index='" + i + "']", "#" + popupid).data("rowData", s);

                                //$('<p>坐标:' + point + '</p>').appendTo(li);
                                //$('<p>数据来源:' + cp + '</p>').appendTo(li);

                                //marker
                                var marker2 = new BMap.Marker(new BMap.Point(point.x, point.y));
                                map.addOverlay(marker2);
                                place.place('markerclick', { marker: marker2, map: map, content: name });
                                markers[i] = marker2;
                            }
                            place.data('markers', markers);
                            if ($("ul", "#" + popupid).hasClass('ui-listview')) {
                                $("ul", "#" + popupid).listview("refresh");
                            }
                            $("ul", "#" + popupid).on('click', 'li', function () {
                                var rowData = $(this).closest('li').data("rowData");
                                if (geoFormat == "Geo")
                                    place.place('setValue', rowData.point.x + "," + rowData.point.y);
                                else
                                    place.place('setValue', rowData.name);
                                $("#" + popupid).popup("close");
                                return false;
                            });


                            searchbox.unbind().bind('blur', function () {
                                var searchword = searchbox.val();
                                if (searchword != "") {
                                    if ($("li", "#" + popupid).length > 0) {
                                        $("li", "#" + popupid).remove();
                                    }
                                    map.clearOverlays();
                                    var local = new BMap.LocalSearch(map, {
                                        onSearchComplete: function (results) {
                                            // 判断状态是否正确
                                            if (local.getStatus() == BMAP_STATUS_SUCCESS) {
                                                var markers = [];
                                                for (var i = 0; i < results.getCurrentNumPois(); i++) {
                                                    var s = results.getPoi(i);
                                                    var name = s.title;
                                                    var addr = s.address;
                                                    var point = s.point;
                                                    var li = $('<li in index="' + i + '"></li>').appendTo($("ul", "#" + popupid));
                                                    var a = $('<a class="list-select">' + name + '</a>').appendTo(li);
                                                    var p1 = $('<p>' + addr + '</p>').appendTo(a);
                                                    $("li[index='" + i + "']", "#" + popupid).data("rowData", s);

                                                    var marker2 = new BMap.Marker(new BMap.Point(point.lng, point.lat));
                                                    map.addOverlay(marker2);
                                                    place.place('markerclick', { marker: marker2, map: map, content: name });
                                                    markers[i] = marker2;
                                                }
                                                place.data('markers', markers);
                                                if ($("ul", "#" + popupid).hasClass('ui-listview')) {
                                                    $("ul", "#" + popupid).listview("refresh");
                                                }
                                                $("ul", "#" + popupid).on('click', 'li', function () {
                                                    var rowData = $(this).closest('li').data("rowData");
                                                    if (geoFormat == "Geo")
                                                        place.place('setValue', rowData.point.lng + "," + rowData.point.lat);
                                                    else
                                                        place.place('setValue', rowData.title);
                                                    $("#" + popupid).popup("close");
                                                    return false;
                                                });


                                            }
                                        },
                                        pageCapacity: 10

                                    });
                                    var circle = new BMap.Circle(resultPoint, 1000, { fillColor: "blue", strokeWeight: 1, fillOpacity: 0.3, strokeOpacity: 0.3 });
                                    map.addOverlay(circle);
                                    local.searchNearby(searchword, resultPoint, 1000);
                                }
                            });
                        }
                        else {
                            alert(data.status);
                            alert(data.result);
                        }
                    },
                    error: function (data) {
                        alert(data);
                    }
                });
                //}
                //else {
                //    alert(data.result);
                //}
                //    },
                //    error: function (data) {
                //        alert(data.result);
                //    }
                //});
            }
        },
        function (error) {
            var message = '';
            switch (error.code) {
                case error.PERMISSION_DENIED:
                    message = "User denied the request for Geolocation.";
                    break;
                case error.POSITION_UNAVAILABLE:
                    message = "Location information is unavailable.";
                    break;
                case error.TIMEOUT:
                    message = "The request to get user location timed out.";
                    break;
                case error.UNKNOWN_ERROR:
                    message = "An unknown error occurred.";
                    break;
            }
            alert(message);
        },
        {
            enableHighAccuracy: true,
            maximumAge: 0,
            timeout: 15000
        });
        }
    },
    markerclick: function (jq, options) {
        var place = $(jq[0]);
        var marker = options.marker;
        var map = options.map;
        var content = options.content;

        if (typeof google != "undefined") {
            var position = options.position;
            google.maps.event.addListener(marker, 'click', function () {
                var htmlOptions = $.parseOption(place.attr('infolight-options'));
                var geoFormat = htmlOptions.geoFormat;
                if (geoFormat == "Geo")
                //warning google .maps.event  lng?lat?
                    place.data('MarkerValue', position.lng() + "," + position.lat());
                else {
                    place.data('MarkerValue', content);
                }

                infowindow.setContent(content);
                infowindow.open(map, this);
            });
        }
        else if (typeof BMap != "undefined") {
            marker.addEventListener("click", function (e) {
                var htmlOptions = $.parseOption(place.attr('infolight-options'));
                var geoFormat = htmlOptions.geoFormat;
                if (geoFormat == "Geo")
                    place.data('MarkerValue', e.target.getPosition().lng + "," + e.target.getPosition().lat);
                else {
                    place.data('MarkerValue', content);
                }

                place.place('openInfoWindow', { map: map, content: content, p: e.target });
                //place.place('setValue', content);
                //var popupid = place.attr('id') + "_popup";
                //$("#" + popupid).popup("close");
            });
        }
    },
    openInfoWindow: function (jq, options) {
        var p = options.p;
        var map = options.map;
        var content = options.content;
        var point = new BMap.Point(p.getPosition().lng, p.getPosition().lat);
        var opts = {
            width: 100,     // 信息窗口宽度
            height: 40,     // 信息窗口高度
            title: content
        };
        var infoWindow = new BMap.InfoWindow("", opts);  // 创建信息窗口对象 
        map.openInfoWindow(infoWindow, point); //开启信息窗口
    },
    createButton: function (jq) {
        jq.each(function () {
            if ($(this).next('a').length == 0) {
                $(this).css('display', 'inline-block');
                $(this).css('margin-right', '-40px');
                var input = $(this);
                input.removeAttr('onclick');
                input.unbind('click').prop('readonly', true);
                $('<a href="#" class="ui-input-clear ui-btn ui-icon-grid ui-btn-icon-notext ui-shadow ui-corner-all" title="Open Code Scaner" role="button" style="vertical-align: middle; display: inline-block;"></a>')
                .insertAfter(this)
                .click(function () {
                    input.place('open');

                });
            }
        });
    }
};

function getClientInfo(variableName) {
    var value = '';
    $.ajax({
        type: "POST",
        url: window.currentUrl,
        dataType: 'text',
        data: "mode=clientinfo&key=" + variableName,
        cache: false,
        async: false,
        success: function (data) {
            value = data;
        }, error: function (data) {


        }
    });
    return value;
};