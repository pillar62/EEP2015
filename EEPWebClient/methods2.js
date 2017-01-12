
/*-----------------------------------------params------------------------------------------------*/

var logoImageHeight = 58; //logo的高度
var logoImageWidth = 1000; //logo图片宽度

var clientHeight = 0;

var leftWidth = 224;//左边(treeview等控件)宽度
var splitWidth = 10; //中间分割宽度

var currentBrowser = currentBrowserType();
function currentBrowserType() {
    if (window.navigator.userAgent.indexOf("MSIE") >= 1) {
        return 1; // ie
    }
    else if (window.navigator.userAgent.indexOf("Firefox") >= 1) {
        return 2; // firefox
    }
    else {
        return 0;
    }
}

function mainPageload() {
    //if (document.body.clientHeight <= logoImageHeight + 177) {
    //屏幕太小,最大化浏览器窗体
    window.moveTo(0, 0);
    window.resizeTo(window.screen.availWidth, window.screen.availHeight);
    //}
    //if (!getCookie('mt_height')) {
    //    setCookie('mt_height', document.documentElement.clientHeight);
    //}
    //clientHeight = getCookie('mt_height');

    clientHeight = Math.max(document.body.clientHeight, document.documentElement.clientHeight); //切换solution会有问题
    document.getElementById('Panel1').style.height = (clientHeight - logoImageHeight - 178) + "px";
    document.getElementById('main').style.height = (clientHeight - logoImageHeight - 38) + "px";

    if (document.getElementById('headerLogo_bg').clientWidth == logoImageWidth) {
        document.getElementById('main').style.width = (logoImageWidth - leftWidth - splitWidth) + "px";
    }
}

function IMG1_onclick(value) {
    if (value.alt == "hide tree") {
        document.getElementById('leftframe').style.display = "none";
        value.alt = "display tree";
        value.src = "Image/Main2012/open.png";
        if (document.getElementById('headerLogo_bg').clientWidth == logoImageWidth) {
            document.getElementById('main').style.width = logoImageWidth - splitWidth;
        }
    }
    else {
        document.getElementById('leftframe').style.display = "block";
        document.getElementById('leftframe').style.width = "224px";
        value.alt = "hide tree";
        value.src = "Image/Main2012/close.png";
        if (document.getElementById('headerLogo_bg').clientWidth == logoImageWidth) {
            document.getElementById('main').style.width = logoImageWidth - leftWidth - splitWidth;
        }
    }
}

/*-----------------------------------------------------------------------------------------------*/

this._createDelegate = function(instance, method) {
    return function() {
        method.apply(instance, arguments);
    }
}

this.RunOnBeforeUnload = function() {
    if (event.clientX > document.getElementById('refTab').clientWidth && event.clientY < 0) {
        this._ajaxRequest("", this._createDelegate(this, this._updateCallback));
    }
}
this._updateCallback = function() {
}

this._ajaxRequest = function(params, callback) {
    new Ajax.Request(
			"ClearSessionHandler.ashx",
			{
			    method: 'post',
			    evalScript: true,
			    parameters: params,
			    onFailure: callback,
			    onException: callback,
			    onComplete: callback
			});
}

function checkdate(inpar) {
    var flag = true;
    var getdate = inpar;
    if (getdate != "") {
        var datepart = getdate.split('');
        var syear = '', smonth = '', sday = '';
        var f = 0;
        for (var i = 0; i < datepart.length; i++) {
            var j = datepart[i];
            if (parseInt(j) <= 9 && parseInt(j) >= 0) {
                if (f == 0) {
                    syear += j;
                }
                else if (f == 1) {
                    smonth += j;
                }
                else if (f == 2) {
                    sday += j;
                }
            }
            else {
                f++;
            }
        }
        var year = parseInt(syear);
        var month = parseInt(smonth);
        var day = parseInt(sday);
        //判断年份是否格式正确
        if (year > 9999 || year < 1) { flag = false; }
        // 判断月份是否格式正确
        if (month > 12 || month < 1) { flag = false; }
        // 判断4,6,9,11月份
        if (month == 4 || month == 6 || month == 9 || month == 11) {
            if (day > 30 || day < 1) {
                flag = false;
            }
        }
        // 判断2月份
        else if (month == 2) {
            if (LeapYear(year)) {
                if (day > 29 || day < 1) { flag = false; }
            }
            else {
                if (day > 28 || day < 1) { flag = false; }
            }
        }
        // 判断1,3,5,7,8,10,12月份
        else {
            if (day > 31 || day < 1) {
                flag = false;
            }
        }
    }
    if (flag == false) {
        alert('message');
    }
    return flag;
}


// 判断当前年是否为闰年
function LeapYear(intYear) {
    if (intYear % 100 == 0) {
        if (intYear % 400 == 0) { return true; }
    }
    else {
        if ((intYear % 4) == 0) { return true; }
    }
    return false;
}


var fontSize = 12;
function OnClientResizeText(sender, eventArgs) {
    var e = sender.get_element();
    while ((e.scrollWidth <= e.clientWidth) || (e.scrollHeight <= e.clientHeight)) {
        e.style.fontSize = (fontSize++) + 'pt';
    }
    var lastScrollWidth = -1;
    var lastScrollHeight = -1;
    while (((e.clientWidth < e.scrollWidth) || (e.clientHeight < e.scrollHeight)) && ((e.scrollWidth != lastScrollWidth) || (e.scrollHeight != lastScrollHeight)) && fontSize > 0) {
        lastScrollWidth = e.scrollWidth;
        lastScrollHeight = e.scrollHeight;
        e.style.fontSize = (fontSize--) + 'pt';
    }
}

function getCookie(name) {
    var start = document.cookie.indexOf(name + "=");
    var len = start + name.length + 1;
    if ((!start) && (name != document.cookie.substring(0, name.length))) return null;
    if (start == -1) return null;
    var end = document.cookie.indexOf(';', len);
    if (end == -1) end = document.cookie.length;
    return unescape(document.cookie.substring(len, end));
}
function setCookie(name, value, expires, path, domain, secure) {
    var today = new Date();
    today.setTime(today.getTime());
    if (expires)
        expires = expires * 1000 * 60 * 60 * 24;
    var expires_date = new Date(today.getTime() + (expires));
    document.cookie = name + '=' + escape(value) + ((expires) ? ';expires=' + expires_date.toGMTString() : '') + ((path) ? ';path=' + path : '') + ((domain) ? ';domain=' + domain : '') + ((secure) ? ';secure' : '');
}
function deleteCookie(name, path, domain) {
    if (getCookie(name))
        document.cookie = name + '=' + ((path) ? ';path=' + path : '') + ((domain) ? ';domain=' + domain : '') + ';expires=Thu, 01-Jan-1970 00:00:01 GMT';
}