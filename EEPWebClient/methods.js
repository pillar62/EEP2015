var logoImageHeight = 58; //logo的高度
var logoImageWidth = 1000; //logo图片宽度

var leftWidth = 224; //左边(treeview等控件)宽度
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
    //if (document.documentElement.clientHeight <= logoImageHeight + 278) {
    //屏幕太小,最大化浏览器窗体
    window.moveTo(0, 0);
    window.resizeTo(window.screen.availWidth, window.screen.availHeight);
    //}
    var clientHeight = document.documentElement.clientHeight;
    var behavior = $find('flowCollaspibleBehavior');
    //behavior.add_collapseComplete(function() {  });
    //behavior.add_expandComplete(function() {  });
    if (behavior && behavior.get_Collapsed()) {
//        document.getElementById('Panel1').style.height = (clientHeight - logoImageHeight - 160) + "px";
//        document.getElementById('rightframe').style.height = (clientHeight - logoImageHeight- 24) + "px";
//        document.getElementById('main').style.height = (clientHeight - logoImageHeight - 78) + "px";

//        if (document.getElementById('headerLogo_bg').clientWidth == logoImageWidth) {
//            document.getElementById('updown').style.width = (logoImageWidth - leftWidth - splitWidth) + "px";
//        }
        //        oriMainHeight = document.getElementById("main").style.height;
        behavior._doOpen($get('Panel2'));
    }
    //else {
        document.getElementById('Panel1').style.height = (clientHeight - logoImageHeight - 178) + "px";
        document.getElementById('rightframe').style.height = (clientHeight - logoImageHeight - 40) + "px";
        document.getElementById('main').style.height = (clientHeight - logoImageHeight - 278) + "px";

        if (document.getElementById('headerLogo_bg').clientWidth == logoImageWidth) {
            document.getElementById('updown').style.width = (logoImageWidth - leftWidth - splitWidth) + "px";
        }
        oriMainHeight = document.getElementById("main").style.height;
    //}
}

var isFlowMax = false;
var oriMainHeight;
var oriFlowHeight;
function topPartSizeChanged() {
    var behavior = $find('flowCollaspibleBehavior');
    if (behavior.get_Collapsed()) {
        if (isFlowMax) {
            $get('main').style.display = 'none';
            $get('flowhr').style.display = 'none';
        }
        else {
            $get('main').style.height = oriMainHeight;
        }
    }
    else {
        if (isFlowMax) {
            $get('main').style.display = 'block';
            $get('flowhr').style.display = 'block';
        }
        $get('main').style.height = (document.documentElement.clientHeight - logoImageHeight - 104) + "px";
    }
}

function setMaxSize() {
    if ($get('main').style.display == 'none') {
        var fs1 = $get('fs1');
        var fs2 = $get('fs2');
        var fs3 = $get('fs3');
        var fs4 = $get('fs4');

        var maxHeight = (document.documentElement.clientHeight - logoImageHeight - 112) + "px";
        if (fs1) fs1.style.height = maxHeight;
        if (fs2) fs2.style.height = maxHeight;
        if (fs3) fs3.style.height = maxHeight;
        if (fs4) fs4.style.height = maxHeight;
    }
}

function imgMax_click() {
    var behavior = $find('flowCollaspibleBehavior');
    var fs1 = $get('fs1');
    var fs2 = $get('fs2');
    var fs3 = $get('fs3');
    var fs4 = $get('fs4');
    if (!isFlowMax) {
        event.srcElement.src = "Image/main/up.gif";
        oriFlowHeight = behavior.get_ExpandedSize();
        $get('main').style.display = 'none';
        $get('flowhr').style.display = 'none';
        setMaxSize();
        behavior.set_ExpandedSize(document.documentElement.clientHeight - logoImageHeight - 88);
        if (behavior.expandPanel) {
            behavior.expandPanel($get('Panel2'));
        }
        else {
            behavior._doOpen($get('Panel2'));
        }
        isFlowMax = true;
    }
    else {
        event.srcElement.src = "Image/main/down.gif";
        behavior.set_ExpandedSize(oriFlowHeight);
        if (behavior.expandPanel) {
            behavior.expandPanel($get('Panel2'));
        }
        else {
            behavior._doOpen($get('Panel2'));
        }
        isFlowMax = false;
        if (fs1) fs1.style.height = "150px";
        if (fs2) fs2.style.height = "150px";
        if (fs3) fs3.style.height = "150px";
        if (fs4) fs4.style.height = "150px";
        //setTimeout("resume_completed()", 400);
        resume_completed();
    }
}

function resume_completed() {
    $get('main').style.height = oriMainHeight;
    $get('main').style.display = 'block';
    $get('flowhr').style.display = 'block';
}

function autoHide() {
    if (!autoHidePanel) return;
    var behavior = $find('flowCollaspibleBehavior');
    if (behavior) {
        topPartSizeChanged();
        
        if (behavior.collapsePanel) {
            behavior.collapsePanel($get('Panel2'));
        }
        else {
            behavior._doClose($get('Panel2'));
        }
    }
    var frame = $get('main');
    if (frame) {
        frame.src = event.srcElement.parentElement.href;
    }
}

function IMG1_onclick(value) {
    if (value.alt == "hide tree") {
        document.getElementById('leftframe').style.display = "none";
        value.alt = "display tree";
        value.src = "Image/main/open.gif";
        if (document.getElementById('headerLogo_bg').clientWidth == logoImageWidth) {
            document.getElementById('updown').style.width = (logoImageWidth - splitWidth) + "px";
        }
    }
    else {
        document.getElementById('leftframe').style.display = "block";
        document.getElementById('leftframe').style.width = "224px";
        value.alt = "hide tree";
        value.src = "Image/main/close.gif";
        if (document.getElementById('headerLogo_bg').clientWidth == logoImageWidth) {
            document.getElementById('updown').style.width = (logoImageWidth - leftWidth - splitWidth) + "px";
        }
    }
}





this._createDelegate = function(instance, method) {
    return function() {
        method.apply(instance, arguments);
    }
}

this.RunOnBeforeUnload = function() {
    //if(event.clientX>document.body.clientWidth)
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