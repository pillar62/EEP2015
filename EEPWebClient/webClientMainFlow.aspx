<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webClientMainFlow.aspx.cs"
    Inherits="webClientMainFlow" Theme="ClientGlobal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>EEP 2012</title>
    <link href="css/ClientMain.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="methods.js"></script>

    <script language="javascript" type="text/javascript" src="prototype-1.4.0.js"></script>

    <script language="javascript" type="text/javascript">
        function hidImg(imgs, tds) {
            var images = tds[0].getElementsByTagName('img');
            for (var x = 0; x < images.length; x++) {
                if (imgs.indexOf(images[x].id) != -1) {
                    images[x].style.display = 'none';
                    images[x].style.visibility = 'hidden';
                }
            }
        }

        function btnMouseOver(buttonname) {
            var btn = document.getElementById(buttonname + 'Container');
            btn.style.backgroundImage = "url('Image/main/" + buttonname + "_over.gif')";
            btn.style.backgroundRepeat = "no-repeat";

        }

        function btnMouseOut(buttonname) {
            var btn = document.getElementById(buttonname + 'Container');
            btn.style.backgroundImage = "url('Image/main/" + buttonname + ".gif')";
            btn.style.backgroundRepeat = "no-repeat";
        }

        function isHTMLEmpty(val) {
            return val == '&nbsp;' || val == '';
        }

        function todolist_onload() {
            if (document.getElementById('dgvToDoList')) {
                var trs = document.getElementById('dgvToDoList').getElementsByTagName('tr');
                for (var i = 0; i < trs.length; i++) {
                    if (i == 0) {
                        var ths = trs[i].getElementsByTagName("th");
                        setListDisplay(ths);
                    }
                    else {
                        var tds = trs[i].getElementsByTagName("td");
                        if (tds.length >= 40) {
                            setListDisplay(tds);
                            var x = tds.length;
                            var LISTID = isHTMLEmpty(tds[x - 42].innerHTML) ? '' : tds[x - 42].childNodes[0].nodeValue;
                            var FLOWPATH = isHTMLEmpty(tds[x - 5].innerHTML) ? '' : tds[x - 5].childNodes[0].nodeValue;
                            var openA = tds[0].getElementsByTagName('a')[0];
                            if (openA) {
                                var NAVIGATOR_MODE = isHTMLEmpty(tds[x - 28].innerHTML) ? '' : tds[x - 28].childNodes[0].nodeValue;
                                var FLNAVIGATOR_MODE = isHTMLEmpty(tds[x - 27].innerHTML) ? '' : tds[x - 27].childNodes[0].nodeValue;
                                var PARAMETERS = isHTMLEmpty(tds[x - 26].innerHTML) ? '' : ConvertUserParameters(tds[x - 26].childNodes[0].nodeValue);
                                var SENDTOID = isHTMLEmpty(tds[x - 24].innerHTML) ? '' : tds[x - 24].childNodes[0].nodeValue;
                                var FLOWIMPORTANT = isHTMLEmpty(tds[x - 22].innerHTML) ? '' : tds[x - 22].childNodes[0].nodeValue;
                                var FLOWURGENT = isHTMLEmpty(tds[x - 21].innerHTML) ? '' : tds[x - 21].childNodes[0].nodeValue;
                                var STATUS = isHTMLEmpty(tds[x - 20].innerHTML) ? '' : tds[x - 20].childNodes[0].nodeValue;
                                if (STATUS.indexOf(':') != -1) {
                                    STATUS = STATUS.split(':')[0];
                                }
                                var FORM_PRESENTATION = isHTMLEmpty(tds[x - 17].innerHTML) ? '' : tds[x - 17].childNodes[0].nodeValue;
                                var FORM_NAME = isHTMLEmpty(tds[x - 7].innerHTML) ? '' : tds[x - 7].childNodes[0].nodeValue;
                                var PLUSAPPROVE = isHTMLEmpty(tds[x - 4].innerHTML) ? '' : tds[x - 4].childNodes[0].nodeValue;
                                var PLUSROLES = isHTMLEmpty(tds[x - 3].innerHTML) ? '' : tds[x - 3].childNodes[0].nodeValue;
                                var MULTISTEPRETURN = isHTMLEmpty(tds[x - 2].innerHTML) ? '' : tds[x - 2].childNodes[0].nodeValue;
                                var ATTACHMENTS = isHTMLEmpty(tds[x - 1].innerHTML) ? '' : tds[x - 1].childNodes[0].nodeValue;
                                var VDSNAME = isHTMLEmpty(tds[x - 10].innerHTML) ? '' : tds[x - 10].childNodes[0].nodeValue;
                                if (FORM_NAME.toUpperCase().indexOf('WEB.') == 0 && FORM_NAME.length > 4) {
                                    FORM_NAME = FORM_NAME.substring(4);
                                }
                                var packageName = FORM_NAME.substring(0, FORM_NAME.indexOf('.'));
                                var formName = FORM_NAME.substring(FORM_NAME.indexOf('.') + 1);

                                if (STATUS) {
                                    if (STATUS == "A") {
                                        FLNAVIGATOR_MODE = '7';
                                        hidImg(new Array("returnImg"), tds);
                                    }
                                    if (STATUS == "F") {
                                        hidImg(new Array("approveImg", "returnImg"), tds);
                                    }
                                    if (FLNAVIGATOR_MODE == "0") {
                                        hidImg(new Array("returnImg"), tds);
                                    }
                                }
                                if (FLNAVIGATOR_MODE == "5") {
                                    hidImg(new Array("returnImg"), tds);
                                }
                                if (PLUSROLES && PLUSROLES != "") {
                                    FLNAVIGATOR_MODE = '8';
                                    hidImg(new Array("approveImg", "returnImg"), tds);
                                }
                                openA.href = packageName + "/" + formName + ".aspx?LISTID=" + LISTID + "&FLOWPATH=" + encodeURIComponent(FLOWPATH) + "&WHERESTRING=" + encodeURIComponent(FORM_PRESENTATION) + "&NAVMODE=" + NAVIGATOR_MODE + "&FLNAVMODE=" + FLNAVIGATOR_MODE + "&ISIMPORTANT=" + FLOWIMPORTANT + "&ISURGENT=" + FLOWURGENT + "&STATUS=" + STATUS + "&PLUSAPPROVE=" + PLUSAPPROVE + "&MULTISTEPRETURN=" + MULTISTEPRETURN + "&ATTACHMENTS=" + encodeURIComponent(ATTACHMENTS) + "&SENDTOID=" + encodeURIComponent(SENDTOID) + PARAMETERS + "&VDSNAME=" + VDSNAME;
                                //openA.target = "main";
                            }
                        }
                    }
                }
            }
        }

        function notify_onload() {
            if (document.getElementById('dgvNotify')) {
                var trs = document.getElementById('dgvNotify').getElementsByTagName("tr");
                for (var i = 0; i < trs.length; i++) {
                    if (i == 0) {
                        var ths = trs[i].getElementsByTagName("th");
                        setListDisplay(ths);
                    }
                    else {
                        var tds = trs[i].getElementsByTagName("td");
                        if (tds.length >= 40) {
                            setListDisplay(tds);
                            var x = tds.length;
                            var LISTID = isHTMLEmpty(tds[x - 42].innerHTML) ? '' : tds[x - 42].childNodes[0].nodeValue;
                            var FLOWPATH = isHTMLEmpty(tds[x - 5].innerHTML) ? '' : tds[x - 5].childNodes[0].nodeValue;
                            var openA = tds[0].getElementsByTagName('a')[0];
                            if (openA) {
                                var NAVIGATOR_MODE = isHTMLEmpty(tds[x - 28].innerHTML) ? '' : tds[x - 28].childNodes[0].nodeValue;
                                var FLNAVIGATOR_MODE = isHTMLEmpty(tds[x - 27].innerHTML) ? '' : tds[x - 27].childNodes[0].nodeValue;
                                var PARAMETERS = isHTMLEmpty(tds[x - 26].innerHTML) ? '' : ConvertUserParameters(tds[x - 26].childNodes[0].nodeValue);
                                var SENDTOID = isHTMLEmpty(tds[x - 24].innerHTML) ? '' : tds[x - 24].childNodes[0].nodeValue;
                                var FLOWIMPORTANT = isHTMLEmpty(tds[x - 22].innerHTML) ? '' : tds[x - 22].childNodes[0].nodeValue;
                                var FLOWURGENT = isHTMLEmpty(tds[x - 21].innerHTML) ? '' : tds[x - 21].childNodes[0].nodeValue;
                                var STATUS = isHTMLEmpty(tds[x - 20].innerHTML) ? '' : tds[x - 20].childNodes[0].nodeValue;
                                if (STATUS.indexOf(':') != -1) {
                                    STATUS = STATUS.split(':')[0];
                                }
                                var FORM_PRESENTATION = isHTMLEmpty(tds[x - 17].innerHTML) ? '' : tds[x - 17].childNodes[0].nodeValue;
                                var FORM_NAME = isHTMLEmpty(tds[x - 7].innerHTML) ? '' : tds[x - 7].childNodes[0].nodeValue;
                                var PLUSAPPROVE = isHTMLEmpty(tds[x - 4].innerHTML) ? '' : tds[x - 4].childNodes[0].nodeValue;
                                var MULTISTEPRETURN = isHTMLEmpty(tds[x - 2].innerHTML) ? '' : tds[x - 2].childNodes[0].nodeValue;
                                var ATTACHMENTS = isHTMLEmpty(tds[x - 1].innerHTML) ? '' : tds[x - 1].childNodes[0].nodeValue;
                                var VDSNAME = isHTMLEmpty(tds[x - 10].innerHTML) ? '' : tds[x - 10].childNodes[0].nodeValue;

                                if (FORM_NAME.toUpperCase().indexOf('WEB.') == 0 && FORM_NAME.length > 4) {
                                    FORM_NAME = FORM_NAME.substring(4);
                                }

                                var packageName = FORM_NAME.substring(0, FORM_NAME.indexOf('.'));
                                var formName = FORM_NAME.substring(FORM_NAME.indexOf('.') + 1);


                                openA.href = packageName + "/" + formName + ".aspx?LISTID=" + LISTID + "&FLOWPATH=" + encodeURIComponent(FLOWPATH) + "&WHERESTRING=" + encodeURIComponent(FORM_PRESENTATION) + "&NAVMODE=" + NAVIGATOR_MODE + "&FLNAVMODE=" + FLNAVIGATOR_MODE + "&ISIMPORTANT=" + FLOWIMPORTANT + "&ISURGENT=" + FLOWURGENT + "&STATUS=" + STATUS + "&PLUSAPPROVE=" + PLUSAPPROVE + "&MULTISTEPRETURN=" + MULTISTEPRETURN + "&ATTACHMENTS=" + encodeURIComponent(ATTACHMENTS) + "&SENDTOID=" + encodeURIComponent(SENDTOID) + PARAMETERS + "&VDSNAME=" + VDSNAME;
                            }
                        }
                    }
                }
            }
        }

        function todohis_onload() {
            if (document.getElementById('dgvToDoHis')) {
                var trs = document.getElementById('dgvToDoHis').getElementsByTagName("tr");
                for (var i = 0; i < trs.length; i++) {
                    if (i == 0) {
                        var ths = trs[i].getElementsByTagName("th");
                        setHisDisplay(ths);
                    }
                    else {
                        var tds = trs[i].getElementsByTagName("td");
                        if (tds.length >= 40) {
                            setHisDisplay(tds);
                            var x = tds.length;
                            var LISTID = isHTMLEmpty(tds[x - 42].innerHTML) ? '' : tds[x - 42].childNodes[0].nodeValue;
                            var FLOWPATH = isHTMLEmpty(tds[x - 5].innerHTML) ? '' : tds[x - 5].childNodes[0].nodeValue;
                            var openA = tds[0].getElementsByTagName('a')[0];
                            if (openA) {
                                var FORM_NAME = isHTMLEmpty(tds[x - 7].innerHTML) ? '' : tds[x - 7].childNodes[0].nodeValue;
                                var FORM_PRESENTATION = isHTMLEmpty(tds[x - 17].innerHTML) ? '' : tds[x - 17].childNodes[0].nodeValue;
                                if (FORM_NAME.toUpperCase().indexOf('WEB.') == 0 && FORM_NAME.length > 4) {
                                    FORM_NAME = FORM_NAME.substring(4);
                                }
                                var packageName = FORM_NAME.substring(0, FORM_NAME.indexOf('.'));
                                var formName = FORM_NAME.substring(FORM_NAME.indexOf('.') + 1);
                                var PARAMETERS = isHTMLEmpty(tds[x - 26].innerHTML) ? '' : ConvertUserParameters(tds[x - 26].childNodes[0].nodeValue);
                                var ATTACHMENTS = isHTMLEmpty(tds[x - 1].innerHTML) ? '' : tds[x - 1].childNodes[0].nodeValue;
                                var VDSNAME = isHTMLEmpty(tds[x - 10].innerHTML) ? '' : tds[x - 10].childNodes[0].nodeValue;
                                openA.href = packageName + "/" + formName + ".aspx?LISTID=" + LISTID + "&FLOWPATH=" + encodeURIComponent(FLOWPATH) + "&WHERESTRING="
                        + encodeURIComponent(FORM_PRESENTATION) + "&NAVMODE=0&FLNAVMODE=6" + PARAMETERS + "&ATTACHMENTS=" + encodeURIComponent(ATTACHMENTS) + "&VDSNAME=" + VDSNAME;
                                //openA.target = "main";
                            }
                        }
                        else if (tds.length >= 11) {
                            setHisDisplay(tds);
                            var x = tds.length;
                            var openA = tds[0].getElementsByTagName('a')[0];
                            if (openA) {
                                var LISTID = isHTMLEmpty(tds[x - 11].innerHTML) ? '' : tds[x - 11].childNodes[0].nodeValue;
                                var FORM_NAME = isHTMLEmpty(tds[x - 7].innerHTML) ? '' : tds[x - 7].childNodes[0].nodeValue;
                                var FORM_PRESENTATION = isHTMLEmpty(tds[x - 6].innerHTML) ? '' : tds[x - 6].childNodes[0].nodeValue;
                                if (FORM_NAME.toUpperCase().indexOf('WEB.') == 0 && FORM_NAME.length > 4) {
                                    FORM_NAME = FORM_NAME.substring(4);
                                }
                                var packageName = FORM_NAME.substring(0, FORM_NAME.indexOf('.'));
                                var formName = FORM_NAME.substring(FORM_NAME.indexOf('.') + 1);
                                var ATTACHMENTS = isHTMLEmpty(tds[x - 2].innerHTML) ? '' : tds[x - 2].childNodes[0].nodeValue;
                                var VDSNAME = isHTMLEmpty(tds[x - 1].innerHTML) ? '' : tds[x - 1].childNodes[0].nodeValue;
                                openA.href = packageName + "/" + formName + ".aspx?LISTID=" + LISTID + "&WHERESTRING=" + FORM_PRESENTATION + "&NAVMODE=0&FLNAVMODE=6&ATTACHMENTS=" + encodeURIComponent(ATTACHMENTS) + "&VDSNAME=" + encodeURIComponent(VDSNAME);
                                openA.target = "main";
                            }
                        }
                    }
                }
            }

            if (document.getElementById('dgvFlowRunOver')) {
                var trs = document.getElementById('dgvFlowRunOver').getElementsByTagName("tr");
                for (var i = 0; i < trs.length; i++) {
                    if (i == 0) {
                        var ths = trs[i].getElementsByTagName("th");
                        setHisDisplay(ths);
                    }
                    else {
                        var tds = trs[i].getElementsByTagName("td");
                        if (tds.length >= 40) {
                            setHisDisplay(tds);
                            var x = tds.length;
                            var LISTID = isHTMLEmpty(tds[x - 42].innerHTML) ? '' : tds[x - 42].childNodes[0].nodeValue;
                            var FLOWPATH = isHTMLEmpty(tds[x - 5].innerHTML) ? '' : tds[x - 5].childNodes[0].nodeValue;
                            var openA = tds[0].getElementsByTagName('a')[0];
                            if (openA) {
                                var FORM_NAME = isHTMLEmpty(tds[x - 7].innerHTML) ? '' : tds[x - 7].childNodes[0].nodeValue;
                                var FORM_PRESENTATION = isHTMLEmpty(tds[x - 17].innerHTML) ? '' : tds[x - 17].childNodes[0].nodeValue;
                                if (FORM_NAME.toUpperCase().indexOf('WEB.') == 0 && FORM_NAME.length > 4) {
                                    FORM_NAME = FORM_NAME.substring(4);
                                }
                                var packageName = FORM_NAME.substring(0, FORM_NAME.indexOf('.'));
                                var formName = FORM_NAME.substring(FORM_NAME.indexOf('.') + 1);
                                var PARAMETERS = isHTMLEmpty(tds[x - 26].innerHTML) ? '' : ConvertUserParameters(tds[x - 26].childNodes[0].nodeValue);
                                var ATTACHMENTS = isHTMLEmpty(tds[x - 1].innerHTML) ? '' : tds[x - 1].childNodes[0].nodeValue;
                                var VDSNAME = isHTMLEmpty(tds[x - 10].innerHTML) ? '' : tds[x - 10].childNodes[0].nodeValue;
                                openA.href = packageName + "/" + formName + ".aspx?LISTID=" + LISTID + "&FLOWPATH=" + encodeURIComponent(FLOWPATH) + "&WHERESTRING="
                        + encodeURIComponent(FORM_PRESENTATION) + "&NAVMODE=0&FLNAVMODE=6" + PARAMETERS + "&ATTACHMENTS=" + encodeURIComponent(ATTACHMENTS) + "&VDSNAME=" + VDSNAME;
                                //openA.target = "main";
                            }
                        }
                        else if (tds.length >= 11) {
                            setHisDisplay(tds);
                            var x = tds.length;
                            var openA = tds[0].getElementsByTagName('a')[0];
                            if (openA) {
                                var LISTID = isHTMLEmpty(tds[x - 11].innerHTML) ? '' : tds[x - 11].childNodes[0].nodeValue;
                                var FORM_NAME = isHTMLEmpty(tds[x - 7].innerHTML) ? '' : tds[x - 7].childNodes[0].nodeValue;
                                var FORM_PRESENTATION = isHTMLEmpty(tds[x - 6].innerHTML) ? '' : tds[x - 6].childNodes[0].nodeValue;
                                if (FORM_NAME.toUpperCase().indexOf('WEB.') == 0 && FORM_NAME.length > 4) {
                                    FORM_NAME = FORM_NAME.substring(4);
                                }
                                var packageName = FORM_NAME.substring(0, FORM_NAME.indexOf('.'));
                                var formName = FORM_NAME.substring(FORM_NAME.indexOf('.') + 1);
                                var ATTACHMENTS = isHTMLEmpty(tds[x - 2].innerHTML) ? '' : tds[x - 2].childNodes[0].nodeValue;
                                var VDSNAME = isHTMLEmpty(tds[x - 1].innerHTML) ? '' : tds[x - 1].childNodes[0].nodeValue;
                                openA.href = packageName + "/" + formName + ".aspx?LISTID=" + LISTID + "&WHERESTRING=" + FORM_PRESENTATION + "&NAVMODE=0&FLNAVMODE=6&ATTACHMENTS=" + encodeURIComponent(ATTACHMENTS) + "&VDSNAME=" + encodeURIComponent(VDSNAME);
                                openA.target = "main";
                            }
                        }
                    }
                }
            }
        }

        function overtimeActive_onload() {
            if (document.getElementById('dgvOvertime')) {
                
                var trs = document.getElementById('dgvOvertime').getElementsByTagName("tr");
                for (var i = 0; i < trs.length; i++) {
                    if (i == 0) {
                        var ths = trs[i].getElementsByTagName("th");
                        setOvertimeDisplay(ths);
                    }
                    else {
                        var tds = trs[i].getElementsByTagName("td");
                        if (tds.length >= 40) {
                            setOvertimeDisplay(tds);
                            
                            var x = tds.length;
                            var openA = tds[0].getElementsByTagName('a')[0];
                            if (openA) {
                                var LISTID = isHTMLEmpty(tds[x - 46].innerHTML) ? '' : tds[x - 46].childNodes[0].nodeValue;
                                var FORM_NAME = isHTMLEmpty(tds[x - 12].innerHTML) ? '' : tds[x - 12].childNodes[0].nodeValue;
                                var FORM_PRESENTATION = isHTMLEmpty(tds[x - 22].innerHTML) ? '' : tds[x - 22].childNodes[0].nodeValue;
                                var packageName = FORM_NAME.substring(0, FORM_NAME.indexOf('.'));
                                var formName = FORM_NAME.substring(FORM_NAME.indexOf('.') + 1);
                                var VDSNAME = isHTMLEmpty(tds[x - 15].innerHTML) ? '' : tds[x - 15].childNodes[0].nodeValue;
                                openA.href = packageName + "/" + formName + ".aspx?LISTID=" + LISTID + "&WHERESTRING=" + encodeURIComponent(FORM_PRESENTATION) + "&NAVMODE=0&FLNAVMODE=6" + "&VDSNAME=" + VDSNAME;
                                //openA.target = "main";
                            }
                        }
                    }
                }
            }
        }

        function setListDisplay(tds) {
            var i = tds.length;
            if (i >= 39) {
                tds[i - 42].style.display = 'none'; // LISTID,1
                tds[i - 41].style.display = 'none'; // FLOW_ID,2
                //    tds[i-40].style.display = 'none'; // FLOW_DESC,3
                tds[i - 39].style.display = 'none'; // APPLICANT,4
                tds[i - 38].style.display = 'none'; // S_USER_ID,5
                tds[i - 37].style.display = 'none'; // S_STEP_ID,6
                tds[i - 36].style.display = 'none'; // S_STEP_DESC,7
                //    tds[i-35].style.display = 'none'; // D_STEP_ID,8
                tds[i - 34].style.display = 'none'; // D_STEP_DESC,9
                tds[i - 33].style.display = 'none'; // EXP_TIME,10
                tds[i - 32].style.display = 'none'; // URGENT_TIME,11
                tds[i - 31].style.display = 'none'; // TIME_UNIT,12
                //    tds[i-30].style.display = 'none'; // USERNAME,13
                tds[i - 29].style.display = 'none'; // FORM_NAME,14
                tds[i - 28].style.display = 'none'; // NAVIGATOR_MODE,15
                tds[i - 27].style.display = 'none'; // FLNAVIGATOR_MODE,16
                tds[i - 26].style.display = 'none'; // PARAMETERS,17
                tds[i - 25].style.display = 'none'; // SENDTO_KIND,18
                tds[i - 24].style.display = 'none'; // SENDTO_ID,19
                tds[i - 23].style.display = 'none'; // SENDTO_NAME,20
                tds[i - 22].style.display = 'none'; // FLOWIMPORTANT,21
                tds[i - 21].style.display = 'none'; // FLOWURGENT,22
                //    tds[i-20].style.display = 'none'; // STATUS,23
                tds[i - 19].style.display = 'none'; // FORM_TABLE,24
                tds[i - 18].style.display = 'none'; // FORM_KEYS,25
                tds[i - 17].style.display = 'none'; // FORM_PRESENTATION,26
                //    tds[i-16].style.display = 'none'; // FORM_PRESENT_CT,27
                //    tds[i-15].style.display = 'none'; // REMARK,28
                tds[i - 14].style.display = 'none'; // PROVIDER_NAME,29
                tds[i - 13].style.display = 'none'; // VERSION,30
                tds[i - 12].style.display = 'none'; // EMAIL_ADD,31
                tds[i - 11].style.display = 'none'; // EMAIL_STATUS,32
                tds[i - 10].style.display = 'none'; // VDSNAME,33
                tds[i - 9].style.display = 'none'; // SENDBACKSTEP,34
                tds[i - 8].style.display = 'none'; // LEVEL_NO,35
                tds[i - 7].style.display = 'none'; // WEBFORMNAME,36
                //    tds[i-6].style.display = 'none'; // UPDATE_WHOLE_TIME,37
                tds[i - 5].style.display = 'none'; // FLOWPATH,38
                tds[i - 4].style.display = 'none'; // PLUSAPPROVE 39
                tds[i - 3].style.display = 'none'; // PLUSROLES 40
                tds[i - 2].style.display = 'none'; // MULTISTEPRETURN 41
                tds[i - 1].style.display = 'none'; // ATTACHMENTS 42
            }
        }

        function isOverTime(TIME_UNIT, FLOWURGENT, UPDATE_DATE, UPDATE_TIME, URGENT_TIME, EXP_TIME) {

        }

        function setHisDisplay(tds) {
            var i = tds.length;
            if (i >= 39) {
                tds[i - 42].style.display = 'none'; // LISTID,1
                tds[i - 41].style.display = 'none'; // FLOW_ID,2
                //    tds[i-40].style.display = 'none'; // FLOW_DESC,3
                tds[i - 39].style.display = 'none'; // APPLICANT,4
                tds[i - 38].style.display = 'none'; // S_USER_ID,5
                tds[i - 37].style.display = 'none'; // S_STEP_ID,6
                tds[i - 36].style.display = 'none'; // S_STEP_DESC,7
                //    tds[i-35].style.display = 'none'; // D_STEP_ID,8
                tds[i - 34].style.display = 'none'; // D_STEP_DESC,9
                tds[i - 33].style.display = 'none'; // EXP_TIME,10
                tds[i - 32].style.display = 'none'; // URGENT_TIME,11
                tds[i - 31].style.display = 'none'; // TIME_UNIT,12
                tds[i - 30].style.display = 'none'; // USERNAME,13
                tds[i - 29].style.display = 'none'; // FORM_NAME,14
                tds[i - 28].style.display = 'none'; // NAVIGATOR_MODE,15
                tds[i - 27].style.display = 'none'; // FLNAVIGATOR_MODE,16
                tds[i - 26].style.display = 'none'; // PARAMETERS,17
                tds[i - 25].style.display = 'none'; // SENDTO_KIND,18
                tds[i - 24].style.display = 'none'; // SENDTO_ID,19
                //    tds[i-23].style.display = 'none'; // SENDTO_NAME,20
                tds[i - 22].style.display = 'none'; // FLOWIMPORTANT,21
                tds[i - 21].style.display = 'none'; // FLOWURGENT,22
                //    tds[i-20].style.display = 'none'; // STATUS,23
                tds[i - 19].style.display = 'none'; // FORM_TABLE,24
                tds[i - 18].style.display = 'none'; // FORM_KEYS,25
                tds[i - 17].style.display = 'none'; // FORM_PRESENTATION,26
                //    tds[i-16].style.display = 'none'; // FORM_PRESENT_CT,27
                //    tds[i-15].style.display = 'none'; // REMARK,28
                tds[i - 14].style.display = 'none'; // PROVIDER_NAME,29
                tds[i - 13].style.display = 'none'; // VERSION,30
                tds[i - 12].style.display = 'none'; // EMAIL_ADD,31
                tds[i - 11].style.display = 'none'; // EMAIL_STATUS,32
                tds[i - 10].style.display = 'none'; // VDSNAME,33
                tds[i - 9].style.display = 'none'; // SENDBACKSTEP,34
                tds[i - 8].style.display = 'none'; // LEVEL_NO,35
                tds[i - 7].style.display = 'none'; // WebFormName,36
                //    tds[i-6].style.display = 'none'; // UPDATE_WHOLE_TIME,37
                tds[i - 5].style.display = 'none'; // FLOWPATH,38
                tds[i - 4].style.display = 'none'; // PLUSAPPROVE 39
                tds[i - 3].style.display = 'none'; // PLUSROLES 40
                tds[i - 2].style.display = 'none'; // MULTISTEPRETURN 41
                tds[i - 1].style.display = 'none'; // ATTACHMENTS 42
            }
            else if (i >= 11) {
                tds[i - 11].style.display = 'none'; //LISTID
                //tds[i-10].style.display = 'none'; //FLOW_DESC 流程
                //tds[i-9].style.display = 'none'; //D_STEP_ID 作業名稱
                tds[i - 8].style.display = 'none'; //FORM_NAME
                tds[i - 7].style.display = 'none'; //WEBFORM_NAME
                tds[i - 6].style.display = 'none'; //FORM_PRESENTATION
                //tds[i-5].style.display = 'none'; //FORM_PRESENT_CT 單據?碼
                //tds[i-4].style.display = 'none'; //REMARK ?息
                //tds[i-3].style.display = 'none'; //UPDATE_WHOLE_TIME 日期
                tds[i - 2].style.display = 'none'; //ATTACHMENTS
                tds[i - 1].style.display = 'none'; //VDSNAME
            }
        }
        function setOvertimeDisplay(tds) {
            var i = tds.length;
            if (i >= 40) {
                tds[i - 46].style.display = 'none'; // LISTID,1
                tds[i - 45].style.display = 'none'; // LISTID,1
                tds[i - 43].style.display = 'none'; // LISTID,1
                tds[i - 42].style.display = 'none'; // FLOW_ID,2
                   tds[i-41].style.display = 'none'; // FLOW_DESC,3
                tds[i - 40].style.display = 'none'; // APPLICANT,4
                //tds[i - 39].style.display = 'none'; // S_USER_ID,5
                tds[i - 38].style.display = 'none'; // S_STEP_ID,6
                tds[i - 37].style.display = 'none'; // S_STEP_DESC,7
                tds[i-36].style.display = 'none'; // D_STEP_ID,8
                tds[i - 35].style.display = 'none'; // D_STEP_DESC,9
                tds[i - 34].style.display = 'none'; // EXP_TIME,10
                tds[i - 33].style.display = 'none'; // URGENT_TIME,11
                tds[i - 32].style.display = 'none'; // TIME_UNIT,12
                tds[i - 31].style.display = 'none'; // USERNAME,13
                tds[i - 30].style.display = 'none'; // FORM_NAME,14
                tds[i - 29].style.display = 'none'; // NAVIGATOR_MODE,15
                tds[i - 28].style.display = 'none'; // FLNAVIGATOR_MODE,16
                tds[i - 27].style.display = 'none'; // PARAMETERS,17
                tds[i - 26].style.display = 'none'; // SENDTO_KIND,18
                tds[i - 25].style.display = 'none'; // SENDTO_ID,19
                tds[i - 24].style.display = 'none'; // FLOWIMPORTANT,20
                tds[i - 23].style.display = 'none'; // FLOWURGENT,21
                tds[i - 22].style.display = 'none'; // STATUS,22
               // tds[i - 21].style.display = 'none'; // FORM_TABLE,23
               //tds[i - 20].style.display = 'none'; // FORM_KEYS,24
                tds[i - 19].style.display = 'none'; // FORM_PRESENTATION,25
                 tds[i-18].style.display = 'none'; // FORM_PRESENT_CT,26
                tds[i - 17].style.display = 'none'; // REMARK,27
                tds[i - 16].style.display = 'none'; // PROVIDER_NAME,28
                tds[i - 15].style.display = 'none'; // VERSION,29
                tds[i - 14].style.display = 'none'; // EMAIL_ADD,30
                tds[i - 13].style.display = 'none'; // EMAIL_STATUS,31
                tds[i - 12].style.display = 'none'; // VDSNAME,32
                tds[i - 11].style.display = 'none'; // UPDATE_DATE,33
                tds[i - 10].style.display = 'none'; // UPDATE_TIME,34
                tds[i - 9].style.display = 'none'; // WEBFORMNAME,35
                tds[i - 8].style.display = 'none'; // UPDATE_DATE,36
                tds[i - 7].style.display = 'none'; // UPDATE_TIME,37
                tds[i - 6].style.display = 'none'; // FLOWPATH,38
                tds[i - 5].style.display = 'none'; // PLUSAPPROVE,39
                tds[i - 4].style.display = 'none'; // PLUSROLES 40

                 //   tds[i-3].style.display = 'none'; // SENDTO_DETAIL,41
                //    tds[i-2].style.display = 'none'; // UPDATE_WHOLE_TIME,42
                //    tds[i-1].style.display = 'none'; // OVERTIME,43
            }
        }

        function ConvertUserParameters(userParameters) {
            var retValue = '';
            var lstUserParameters = userParameters.split(';');
            for (i = 0; i < lstUserParameters.length; i++) {
                var userParameter = lstUserParameters[i];
                if (userParameter != "" && userParameter.indexOf('=') != -1) {
                    var key = userParameter.substring(0, userParameter.indexOf('=') + 1);
                    var value = encodeURIComponent(userParameter.substring(userParameter.indexOf('=') + 1));
                    retValue += "&" + key + value;
                }
            }
            return retValue;
        }

        function hisQuery() {
            var chk = $get('chkSubmitted');
            if (chk) {
                if (!chk.checked) {
                    $find('popHisQueryExtenderBehavior').show();
                }
                else {
                    $find('popHisQuery2ExtenderBehavior').show();
                }
            }

            return false;
        }
        
        function openFaverMenu()
        {
            window.open("WebFavorMenu.aspx","_blank","height=330,width=530,top=200,left=200,toolbar=no,status=no,resizable=yes,scrollable=yes,location=no");
        }

        function fApproveOrReturnAll(OperateType) {
            var ievar = 0;
            var Sys = EstimateBrowserVersion();
            if ((Sys.ie && Sys.ie == "9.0") || Sys.chrome || Sys.firefox)
                ievar = 1;

            if (document.getElementById('dgvToDoList')) {
                var trs = document.getElementById('dgvToDoList').getElementsByTagName('tr');
                var count = -1;
                var param = "";
                for (var i = 0; i < trs.length; i++) {
                    if (i == 0) {
                        var ths = trs[i].getElementsByTagName("th");
                        //setListDisplay(ths);
                    }
                    else {
                        var tds = trs[i].getElementsByTagName("td");
                        var x = tds.length
                        if (x >= 40) {
                            if (tds[x - 43].childNodes[ievar].checked) {
                            
                                var LISTID = tds[x - 42].childNodes[0].nodeValue;
                                var FLOWPATH = tds[x - 5].childNodes[0].nodeValue;
                                var KEYS = tds[x - 18].childNodes[0].nodeValue;
                                var VALUES = tds[x - 17].childNodes[0].nodeValue;
                                var PROVIDER = tds[x - 14].childNodes[0].nodeValue;
                                var replaceText = '$$$$$';
                                VALUES = VALUES.replace(/'/g, replaceText);
                                var FLOWIMPORTANT = tds[x - 22].childNodes[0].nodeValue;
                                var FLOWURGENT = tds[x - 21].childNodes[0].nodeValue;
                                var STATUS = tds[x - 20].childNodes[0].nodeValue;
                                if (STATUS.indexOf(':') != -1) {
                                    STATUS = STATUS.split(':')[0];
                                }
                                var MULTISTEPRETURN = tds[x - 2].childNodes[0].nodeValue;
                                var ATTACHMENTS = isHTMLEmpty(tds[x - 1].innerHTML) ? '' : tds[x - 1].childNodes[0].nodeValue;
                                var SENDTOID = isHTMLEmpty(tds[x - 24].innerHTML) ? '' : tds[x - 24].childNodes[0].nodeValue;
                                var VDSNAME = isHTMLEmpty(tds[x - 10].innerHTML) ? '' : tds[x - 10].childNodes[0].nodeValue;
                                var OPERATETYPE = OperateType;
                                var url = 'LISTID=' + LISTID + '&OPERATETYPE=' + OPERATETYPE + '&KEYS=' + encodeURIComponent(KEYS) + '&VALUES='
                                 + encodeURIComponent(VALUES) + '&FLOWPATH=' + encodeURIComponent(FLOWPATH) + '&PROVIDER=' + encodeURIComponent(PROVIDER) + '&ISIMPORTANT=' + FLOWIMPORTANT + '&ISURGENT=' + FLOWURGENT + '&STATUS=' + STATUS + '&MULTISTEPRETURN=' + MULTISTEPRETURN + '&ATTACHMENTS=' + encodeURIComponent(ATTACHMENTS) + "&SENDTOID=" + encodeURIComponent(SENDTOID) + "&VDSNAME=" + VDSNAME;
                                 
                                var NAVIGATOR_MODE = isHTMLEmpty(tds[x - 28].innerHTML) ? '' : tds[x - 28].childNodes[0].nodeValue;
                                var FLNAVIGATOR_MODE = isHTMLEmpty(tds[x - 27].innerHTML) ? '' : tds[x - 27].childNodes[0].nodeValue;
                                var PLUSAPPROVE = isHTMLEmpty(tds[x - 4].innerHTML) ? '' : tds[x - 4].childNodes[0].nodeValue;
                                var PLUSROLES = isHTMLEmpty(tds[x - 3].innerHTML) ? '' : tds[x - 3].childNodes[0].nodeValue;

                                count++;
                                document.cookie = "FlowApproveAll" + count + "=" + url;
                                param = "InnerPages/FlowSubmitConfirm.aspx?" + url;
                            }
                        }
                    }
                }
                
                count++;
                var exDate = new Date();       
                var exDateString = exDate.toGMTString(); //expires  
                document.cookie = "FlowApproveAll" + count + "=" + "" + exDateString;
                
                if (STATUS) {
                    if (STATUS == "A" && OperateType == "Return") {
                        FLNAVIGATOR_MODE = '7';
                        alert("Some rows you've selected can't " + OperateType + ".");
                        return;
                    }
                    if (STATUS == "F") {
                        alert("Some rows you've selected can't " + OperateType + ".");
                        return;
                    }
                    if (FLNAVIGATOR_MODE == "0" && OperateType == "Return") {
                        alert("Some rows you've selected can't " + OperateType + ".");
                        return;
                    }
                }
                if (OperateType == "Return" && FLNAVIGATOR_MODE == "5") {
                        alert("Some rows you've selected can't " + OperateType + ".");
                        return;
                }
                if (PLUSROLES && PLUSROLES != "") {
                    FLNAVIGATOR_MODE = '8';
                        alert("Some rows you've selected can't " + OperateType + ".");
                        return;
                }
                
                if(count == 0)
                    alert("You have to select one.");
                else if(count == 1)
                {
                    window.open(param, '', 'resizable=yes,scrollbars=yes,width=500,height=410,top=200,left=200');
                }
                else
                {
                    window.open("InnerPages/frmApproveAll.aspx", '', 'resizable=yes,scrollbars=yes,width=500,height=410,top=200,left=200');
                }
            }
        }
        
        function fOnCheckedChanged()
        {

            var isChecked;
            if (document.getElementById('dgvToDoList')) {
                var trs = document.getElementById('dgvToDoList').getElementsByTagName('tr');
                var count = -1;
                var param = "";
                for (var i = 0; i < trs.length; i++) {
                    if (i == 0) {
                        var ths = trs[i].getElementsByTagName("th");
                        var count = ths.length;
                        var ievar = 0;

                        var Sys = EstimateBrowserVersion();
                        if ((Sys.ie && Sys.ie == "9.0") || Sys.chrome || Sys.firefox)
                            ievar = 1;
                        isChecked = ths[count - 43].childNodes[ievar].checked;
                    }
                    else {
                        var tds = trs[i].getElementsByTagName("td");
                        var x = tds.length;
                        if (x >= 40) {
                            var ievar = 0;
                            var Sys = EstimateBrowserVersion();
                            if ((Sys.ie && Sys.ie == "9.0") || Sys.chrome || Sys.firefox)
                                ievar = 1;
                            tds[x - 43].childNodes[ievar].checked = isChecked;
                        }
                    }
                }
            }
        }

        function EstimateBrowserVersion() {
            var Sys = {};
            var ua = navigator.userAgent.toLowerCase();
            var s;
            (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
            (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
            (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
            (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
            (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;

            return Sys;
        }
    </script>

</head>
<body onload="body_onload()" onbeforeunload="RunOnBeforeUnload()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <table border="0" cellpadding="0" cellspacing="0" class="fullWidthTable">
            <tr>
                <td id="headerLogo_bg">
                    <div id="headerLogo">
                    </div>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" class="fullWidthTable">
            <tr>
                <td id="leftframeContainer">
                    <div id="leftframe" class="leftPart">
                        <table id="refTab" border="0" cellpadding="0" cellspacing="0" class="leftPart">
                            <tr>
                                <td colspan="3" id="topContent">
                                </td>
                            </tr>
                            <tr>
                                <td id="middleContent">
                                    <table border="0" cellpadding="0" cellspacing="0" class="leftPart">
                                        <tr>
                                            <td class="marginContent">
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ibMyFavor" runat="server" ImageUrl="~/Image/main/favorite_normal.png"
                                                    CssClass="buttonImg"></asp:ImageButton>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCaption" runat="server"></asp:TextBox>
                                            </td>
                                            <td id="goContainer">
                                                <asp:LinkButton ID="ibGO" runat="server" onmouseover="btnMouseOver('go')" onmouseout="btnMouseOut('go')"
                                                    Width="100%" Height="27px" onclick="ibGO_Click" />
                                            </td>
                                            <td class="marginContent">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="marginContent">
                                            </td>
                                            <td>
                                                <img id="imgSolution" src="Image/main/solution.png" alt="solution" class="buttonImg" />
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlSolution" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSolution_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="marginContent">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="marginContent">
                                            </td>
                                            <td colspan="3">
                                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="204px">
                                                    <asp:TreeView ID="tView" runat="server" ImageSet="Simple" NodeIndent="10" ShowLines="True"
                                                        Width="178px" AutoGenerateDataBindings="False" ExpandDepth="1" OnTreeNodePopulate="tView_TreeNodePopulate">
                                                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="White" HorizontalPadding="0px"
                                                            NodeSpacing="0px" VerticalPadding="0px" />
                                                        <SelectedNodeStyle Font-Underline="True" ForeColor="#DD5555" HorizontalPadding="0px"
                                                            VerticalPadding="0px" />
                                                        <HoverNodeStyle Font-Underline="True" ForeColor="#DD5555" />
                                                        <ParentNodeStyle Font-Bold="False" />
                                                    </asp:TreeView>
                                                </asp:Panel>
                                            </td>
                                            <td class="marginContent">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" id="footContent">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="marginContent1">
                                            </td>
                                            <td id="changePasswordContainer">
                                                <asp:Button ID="btnChangepassword" runat="server" Text="change password" OnClick="btnChangepassword_Click"
                                                    CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';" />
                                            </td>
                                            <td id="logOutContainer">
                                                <asp:Button ID="btnLogOut" runat="server" Text="log out" OnClick="btnLogOut_Click"
                                                    CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';" />
                                            </td>
                                            <td id="marginContent2">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <%--左右缩进--%>
                <td id="split">
                    <img src="Image/main/close.gif" alt="hide tree" id="IMG1" onclick="IMG1_onclick(this)" />
                </td>
                <td id="rightframeContainer">
                    <div id="rightframe">
                        <asp:Panel ID="Panel2" runat="server" onclick="topPartSizeChanged()">
                            <div id="wfTitleContainer">
                                <div id="wftitle" runat="server">
                                </div>
                                <div id="wfECDirector">
                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                </div>
                                <div id="wfECImgDirector">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="Image/Ajax/expand.jpg" />
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="Panel3" runat="server">
                            <div id="progressContainer" style="text-align:right;width:100%">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel4" DisplayAfter="200">
                                <ProgressTemplate>
                                    <img src="Image/Ajax/updateProgress.gif" alt="waiting..."/>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <infolight:webmultiviewcaptions id="WebMultiViewCaptions1" runat="server" multiviewid="MultiView1" isinrootpath="True" font-names="Arial" font-size="12pt" font-bold="True" onload="WebMultiViewCaptions1_Load">
                                <captions>
                                    <InfoLight:WebMultiViewCaption Caption="" />
                                    <InfoLight:WebMultiViewCaption Caption="" />
                                    <InfoLight:WebMultiViewCaption Caption="" />
                                    <InfoLight:WebMultiViewCaption Caption="" />
                                </captions>
                            </infolight:webmultiviewcaptions>
                                    <asp:MultiView ID="MultiView1" runat="server" OnActiveViewChanged="MultiView1_ActiveViewChanged" >
                                        <asp:View ID="View1" runat="server">
                                            <div id="fs1" class="fs">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" OnLoad="UpdatePanel1_Load" OnPreRender="UpdatePanel1_PreRender"
                                                    UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlToDoListFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlToDoListFilter_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:LinkButton ID="lnkRefresh" runat="server" OnClick="lnkRefresh_Click">Refresh</asp:LinkButton>
                                                        <asp:LinkButton ID="lnkToDoListQuery" runat="server">Query</asp:LinkButton>
                                                        <asp:HyperLink ID="lbApproveAll" runat="server" Text="Approve All" onclick="fApproveOrReturnAll('Approve')" Font-Overline="False" Font-Underline="True" CssClass="link" />
                                                        <asp:HyperLink ID="lbReturnAll" runat="server" Text="Return All" onclick="fApproveOrReturnAll('Return')" Font-Underline="True" CssClass="link" />
                                                        <%--<input id="Button1" type="button" value="Approve All" onclick="fApproveAll()" />--%>
                                                        <asp:GridView ID="dgvToDoList" SkinID="FlowClientMainGrid" runat="server" 
                                                            EmptyDataText="there's no to do list!" AllowSorting="True" 
                                                            onsorting="dgvToDoList_Sorting" onrowdatabound="dgvToDoList_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <table cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:HyperLink ID="openA" runat="server" onclick="autoHide()" ImageUrl="Image/UIPics/Select.gif" Target="main"></asp:HyperLink>
                                                                                </td>
                                                                                <td>
                                                                                    <img id="approveImg" alt="<%= this.GetToolTip(6) %>" src="Image/UIPics/Approve.gif" onclick="approveClick()" />
                                                                                </td>
                                                                                <td>
                                                                                    <img id="returnImg" alt="<%= this.GetToolTip(8) %>" src="Image/UIPics/Return.gif" onclick="returnClick()" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgImportant" runat="server" ImageUrl="~/Image/UIPics/important.gif"
                                                                            Visible='<%# ConvertStringToBoolean(Eval("FLOWIMPORTANT")) %>' /><asp:Image ID="imgUrgent"
                                                                                runat="server" ImageUrl="Image/UIPics/urgent.gif" Visible='<%# ConvertStringToBoolean(Eval("FLOWURGENT")) %>' /></ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <input id="Checkbox2" type="checkbox" onclick="fOnCheckedChanged()" />
                                                                        <%--<asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="" />--%>
                                                                    </HeaderTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle Height="20px" />
                                                        </asp:GridView>
                                                        <asp:Panel ID="lstQueryContainer" runat="server" CssClass="popup" Style="display: none;">
                                                            <asp:Panel ID="lstQueryTitle" runat="server" CssClass="div_title">
                                                                <asp:ImageButton ID="lstQueryClose" runat="server" ImageUrl="~/Image/Ajax/close.gif" /></asp:Panel>
                                                            <asp:Panel ID="lstQueryContent" runat="server" CssClass="div_content">
                                                                <table class="query_condition_container">
                                                                    <tr>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(0) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:DropDownList ID="ddlLstQueryFlow" runat="server" Width="155px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(1) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:TextBox ID="txtLstQueryDStep" runat="server" Width="150px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(2) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:DropDownList ID="ddlLstQueryUser" runat="server" Width="155px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(3) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:TextBox ID="txtLstQueryFormPresent" runat="server" Width="150px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(5) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:TextBox ID="txtLstQueryDateFrom" runat="server" Width="130px" /><asp:ImageButton
                                                                                runat="Server" ID="imgLstQueryDateFrom" ImageUrl="~/Image/main/calender.png"
                                                                                AlternateText="Click to show calendar" /><ajaxToolkit:CalendarExtender ID="lstQueryDateFromCalenderExtender"
                                                                                    runat="server" TargetControlID="txtLstQueryDateFrom" PopupButtonID="imgLstQueryDateFrom"
                                                                                    Format="yyyy-MM-dd" />
                                                                        </td>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(6) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:TextBox ID="txtLstQueryDateTo" runat="server" Width="130px" /><asp:ImageButton
                                                                                runat="Server" ID="imgLstQueryDateTo" ImageUrl="~/Image/main/calender.png" AlternateText="Click to show calendar" /><ajaxToolkit:CalendarExtender
                                                                                    ID="lstQueryDateToCalenderExtender" runat="server" TargetControlID="txtLstQueryDateTo"
                                                                                    PopupButtonID="imgLstQueryDateTo" Format="yyyy-MM-dd" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(4) %>
                                                                        </td>
                                                                        <td colspan="3" class="query_caption">
                                                                            <asp:TextBox ID="txtLstQueryRemark" runat="server" Width="410px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Button ID="lstQueryOK" runat="server" Width="80px" Text="OK" OnClick="lstQueryOK_Click"
                                                                                        CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';" />
                                                                                    </td>
                                                                                    <td>
                                                                                    <asp:Button ID="lstQueryCancel" runat="server" Width="80px" Text="Cancel" OnClientClick="$find('popLstQueryExtenderBehavior').hide();return false;" CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';"/>
                                                                                        <%--<img id="lstQueryCancel" alt="cancel" onclick="$find('popLstQueryExtenderBehavior').hide();"
                                                                                            src="Image/innerpage/cancel.gif" style="cursor: pointer;" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </asp:Panel>
                                                        <ajaxToolkit:ModalPopupExtender ID="popLstQueryExtender" BehaviorID="popLstQueryExtenderBehavior"
                                                            runat="server" TargetControlID="lnkToDoListQuery" PopupControlID="lstQueryContainer"
                                                            CancelControlID="lstQueryClose" Y="150" BackgroundCssClass="modalBackground"
                                                            PopupDragHandleControlID="lstQueryContainer" Drag="true"></ajaxToolkit:ModalPopupExtender>
                                                        <flTools:FLWebWizard ID="wizToDoList" runat="server" Active="True" SqlMode="ToDoList"
                                                            BindingObjectID="dgvToDoList" onrefreshed="wizToDoList_Refreshed"></flTools:FLWebWizard>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="tmFlow" EventName="Tick" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </asp:View>
                                        <asp:View ID="View2" runat="server">
                                            <div id="fs2" class="fs">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" OnLoad="UpdatePanel2_Load" OnPreRender="UpdatePanel2_PreRender"
                                                    UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlToDoHisFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlToDoHisFilter_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:CheckBox ID="chkSubmitted" runat="server" AutoPostBack="True" OnCheckedChanged="chkSubmitted_CheckedChanged" />
                                                        <asp:LinkButton ID="lnkHisRefresh" runat="server" OnClick="lnkHisRefresh_Click">Refresh</asp:LinkButton>
                                                        <asp:LinkButton ID="lnkToDoHisQueryHiden" runat="server"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkToDoHisQuery" runat="server" OnClientClick="return hisQuery();">Query</asp:LinkButton>
                                                        <asp:GridView ID="dgvToDoHis" SkinID="FlowClientMainGrid" runat="server" OnSelectedIndexChanged="dgvToDoHis_SelectedIndexChanged"
                                                            OnRowCommand="dgvToDoHis_RowCommand" 
                                                            EmptyDataText="there's no to do history!" AllowSorting="True" 
                                                            onsorting="dgvToDoHis_Sorting" onrowdatabound="dgvToDoHis_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <table cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:HyperLink ID="openA" onclick="autoHide()" ImageUrl="Image/UIPics/Select.gif" runat="server" Target="main"></asp:HyperLink>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton runat="server" ID="returnImg" AlternateText="return" ImageUrl="Image/UIPics/Return.gif"
                                                                                        CommandName="Return" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'
                                                                                        Visible='<%# RetakeVisible(Container.DataItem) %>' />
                                                                                </td>
                                                                                <td>
                                                                                <asp:ImageButton runat="server" ID="notifyImg" AlternateText="Notify" ImageUrl="Image/UIPics/Press.gif"
                                                                                        CommandName="Notify" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'
                                                                                        Visible='<%# ReturnVisible() %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField SortExpression="FLOWIMPORTANT">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgImportant" runat="server" ImageUrl="~/Image/UIPics/important.gif"
                                                                            Visible='<%# ConvertStringToBoolean(Eval("FLOWIMPORTANT")) %>' /><asp:Image ID="imgUrgent"
                                                                                runat="server" ImageUrl="Image/UIPics/urgent.gif" Visible='<%# ConvertStringToBoolean(Eval("FLOWURGENT")) %>' /></ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle Height="20px" />
                                                        </asp:GridView>
                                                         <asp:GridView ID="dgvFlowRunOver" Visible="false" SkinID="FlowClientMainGrid" runat="server"
                                                            EmptyDataText="there's no to do history!" AllowSorting="True" 
                                                            onsorting="dgvFlowRunOver_Sorting" >
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <table cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:HyperLink ID="openA" onclick="autoHide()" ImageUrl="Image/UIPics/Select.gif" runat="server" Target="main"></asp:HyperLink>
                                                                                </td>
                                                                              <%--  <td>
                                                                                    <asp:ImageButton runat="server" ID="returnImg" AlternateText="return" ImageUrl="Image/UIPics/Return.gif"
                                                                                        CommandName="Return" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'
                                                                                        Visible='<%# RetakeVisible(Container.DataItem) %>' />
                                                                                </td>
                                                                                <td>
                                                                                <asp:ImageButton runat="server" ID="notifyImg" AlternateText="Notify" ImageUrl="Image/UIPics/Press.gif"
                                                                                        CommandName="Notify" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'
                                                                                        Visible='<%# ReturnVisible() %>' />
                                                                                </td>--%>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                               <%-- <asp:TemplateField SortExpression="FLOWIMPORTANT">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgImportant" runat="server" ImageUrl="~/Image/UIPics/important.gif"
                                                                            Visible='<%# ConvertStringToBoolean(Eval("FLOWIMPORTANT")) %>' /><asp:Image ID="imgUrgent"
                                                                                runat="server" ImageUrl="Image/UIPics/urgent.gif" Visible='<%# ConvertStringToBoolean(Eval("FLOWURGENT")) %>' /></ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                            </Columns>
                                                            <HeaderStyle Height="20px" />
                                                        </asp:GridView>
                                                        <asp:Panel ID="hisQueryContainer" runat="server" CssClass="popup" Style="display: none;">
                                                            <asp:Panel ID="hisQueryTitle" runat="server" CssClass="div_title">
                                                                <asp:ImageButton ID="hisQueryClose" runat="server" ImageUrl="~/Image/Ajax/close.gif" /></asp:Panel>
                                                            <asp:Panel ID="hisQueryContent" runat="server" CssClass="div_content">
                                                                <table class="query_condition_container">
                                                                    <tr>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(0) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:DropDownList ID="ddlHisQueryFlow" runat="server" Width="155px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(1) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:TextBox ID="txtHisQueryDStep" runat="server" Width="150px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(7) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:DropDownList ID="ddlHisQuerySendTo" runat="server" Width="155px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(3)%>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                        <asp:TextBox ID="txtHisQueryFormPresent" runat="server" Width="150px">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(5) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:TextBox ID="txtHisQueryDateFrom" runat="server" Width="130px" /><asp:ImageButton
                                                                                runat="Server" ID="imgHisQueryDateFrom" ImageUrl="~/Image/main/calender.png"
                                                                                AlternateText="Click to show calendar" /><ajaxToolkit:CalendarExtender ID="hisQueryDateFromCalenderExtender"
                                                                                    runat="server" TargetControlID="txtHisQueryDateFrom" PopupButtonID="imgHisQueryDateFrom"
                                                                                    Format="yyyy-MM-dd" />
                                                                        </td>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(6) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:TextBox ID="txtHisQueryDateTo" runat="server" Width="130px" /><asp:ImageButton
                                                                                runat="Server" ID="imgHisQueryDateTo" ImageUrl="~/Image/main/calender.png" AlternateText="Click to show calendar" />
                                                                            <ajaxToolkit:CalendarExtender ID="hisQueryDateToCalenderExtender" runat="server"
                                                                                TargetControlID="txtHisQueryDateTo" PopupButtonID="imgHisQueryDateTo" Format="yyyy-MM-dd" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(4) %>
                                                                        </td>
                                                                        <td colspan="3" class="query_caption">
                                                                            <asp:TextBox ID="txtHisQueryRemark" runat="server" Width="410px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Button ID="hisQueryOK" runat="server" Width="80px" Text="OK" OnClick="hisQueryOK_Click" CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="hisQueryCancel" runat="server" Width="80px" Text="Cancel" OnClientClick="$find('popHisQueryExtenderBehavior').hide();return false;" CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';" />
                                                                                        <%--<img id="hisQueryCancel" alt="cancel" onclick="$find('popHisQueryExtenderBehavior').hide();"
                                                                                            src="Image/innerpage/cancel.gif" style="cursor: pointer;" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </asp:Panel>
                                                        <ajaxToolkit:ModalPopupExtender ID="popHisQueryExtender" BehaviorID="popHisQueryExtenderBehavior"
                                                            runat="server" TargetControlID="lnkToDoHisQueryHiden" PopupControlID="hisQueryContainer"
                                                            CancelControlID="hisQueryClose" Y="150" BackgroundCssClass="modalBackground"
                                                            PopupDragHandleControlID="hisQueryContainer" Drag="true"></ajaxToolkit:ModalPopupExtender>
                                                        <asp:Panel ID="hisQuery2Container" runat="server" CssClass="popup" Style="display: none;">
                                                            <asp:Panel ID="hisQuery2Title" runat="server" CssClass="div_title">
                                                                <asp:ImageButton ID="hisQuery2Close" runat="server" ImageUrl="~/Image/Ajax/close.gif" /></asp:Panel>
                                                            <asp:Panel ID="hisQuery2Content" runat="server" CssClass="div_content">
                                                                <table class="query_condition_container">
                                                                    <tr>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(0) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:DropDownList ID="ddlHisQuery2Flow" runat="server" Width="155px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(1) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:TextBox ID="txtHisQuery2DStep" runat="server" Width="150px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(3) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:TextBox ID="txtHisQuery2FormPresent" runat="server" Width="150px">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(5) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:TextBox ID="txtHisQuery2DateFrom" runat="server" Width="130px" /><asp:ImageButton
                                                                                runat="Server" ID="imgHisQuery2DateFrom" ImageUrl="~/Image/main/calender.png"
                                                                                AlternateText="Click to show calendar" /><ajaxToolkit:CalendarExtender ID="hisQuery2DateFromCalenderExtender"
                                                                                    runat="server" TargetControlID="txtHisQuery2DateFrom" PopupButtonID="imgHisQuery2DateFrom"
                                                                                    Format="yyyy-MM-dd" />
                                                                        </td>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(6) %>
                                                                        </td>
                                                                        <td class="query_caption">
                                                                            <asp:TextBox ID="txtHisQuery2DateTo" runat="server" Width="130px" /><asp:ImageButton
                                                                                runat="Server" ID="imgHisQuery2DateTo" ImageUrl="~/Image/main/calender.png" AlternateText="Click to show calendar" />
                                                                            <ajaxToolkit:CalendarExtender ID="hisQuery2DateToCalenderExtender" runat="server"
                                                                                TargetControlID="txtHisQuery2DateTo" PopupButtonID="imgHisQuery2DateTo" Format="yyyy-MM-dd" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <%= this.GetQueryCaption(4) %>
                                                                        </td>
                                                                        <td colspan="3" class="query_caption">
                                                                            <asp:TextBox ID="txtHisQuery2Remark" runat="server" Width="410px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <table style="position:relative;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Button ID="hisQuery2OK" runat="server" Width="80px" Text="OK" OnClick="hisQuery2OK_Click" CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';" />
                                                                                    </td>
                                                                                    <td>
                                                                                    <asp:Button ID="hisQuery2Cancel" runat="server" Width="80px" Text="Cancel" OnClientClick="$find('popHisQuery2ExtenderBehavior').hide();return false;" CssClass="btn_mouseout" onmouseover="this.className='btn_mouseover';" onmouseout="this.className='btn_mouseout';"/>
                                                                                        <%--<img id="hisQuery2Cancel" alt="cancel" onclick="$find('popHisQuery2ExtenderBehavior').hide();"
                                                                                            src="Image/innerpage/cancel.gif" style="cursor: pointer;" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </asp:Panel>
                                                        <ajaxToolkit:ModalPopupExtender ID="popHisQuery2Extender" BehaviorID="popHisQuery2ExtenderBehavior"
                                                            runat="server" TargetControlID="lnkToDoHisQueryHiden" PopupControlID="hisQuery2Container"
                                                            CancelControlID="hisQuery2Close" Y="150" BackgroundCssClass="modalBackground"
                                                            PopupDragHandleControlID="hisQuery2Container" Drag="true"></ajaxToolkit:ModalPopupExtender>
                                                        <flTools:FLWebWizard ID="wizToDoHis" runat="server" Active="True" SqlMode="ToDoHis"
                                                            BindingObjectID="dgvToDoHis" onrefreshed="wizToDoHis_Refreshed"></flTools:FLWebWizard>
                                                        <flTools:FLWebWizard ID="wizFlowRunOver" runat="server" Active="True" SqlMode="FlowRunOver"
                                                            BindingObjectID="dgvFlowRunOver" onrefreshed="wizFlowRunOver_Refreshed"></flTools:FLWebWizard>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="tmFlow" EventName="Tick" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </asp:View>
                                        <asp:View ID="View4" runat="server">
                                            <div id="fs4" class="fs">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" OnLoad="UpdatePanel5_Load" OnPreRender="UpdatePanel5_PreRender"
                                                    UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlNotifyFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNotifyFilter_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:LinkButton ID="lnkNotifyRefresh" runat="server" OnClick="lnkNotifyRefresh_Click">Refresh</asp:LinkButton>
                                                        <asp:GridView
                                                            ID="dgvNotify" SkinID="FlowClientMainGrid" runat="server" EmptyDataText="there's no notify!"
                                                            OnRowCommand="dgvNotify_RowCommand" AllowSorting="True" 
                                                            onsorting="dgvNotify_Sorting" onrowdatabound="dgvNotify_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <table cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:HyperLink ID="openA" onclick="autoHide()" ImageUrl="Image/UIPics/Select.gif" runat="server" Target="main"></asp:HyperLink>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton runat="server" ID="deleteImg" AlternateText="delete" ImageUrl="Image/UIPics/FlowDelete.gif"
                                                                                        CommandName="FlowDelete" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgImportant" runat="server" ImageUrl="~/Image/UIPics/important.gif"
                                                                            Visible='<%# ConvertStringToBoolean(Eval("FLOWIMPORTANT")) %>' /><asp:Image ID="imgUrgent"
                                                                                runat="server" ImageUrl="Image/UIPics/urgent.gif" Visible='<%# ConvertStringToBoolean(Eval("FLOWURGENT")) %>' /></ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle Height="20px" />
                                                        </asp:GridView>
                                                        <flTools:FLWebWizard ID="wizNotify" runat="server" Active="True" SqlMode="Notify"
                                                            BindingObjectID="dgvNotify" onrefreshed="wizNotify_Refreshed"></flTools:FLWebWizard>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </asp:View>
                                        <asp:View ID="View3" runat="server">
                                            <div id="fs3" class="fs">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" OnLoad="UpdatePanel3_Load" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblLevel" runat="server" Text="level"></asp:Label><asp:DropDownList
                                                            ID="ddlLevel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLevel_SelectedIndexChanged">
                                                            <asp:ListItem Value="0"></asp:ListItem>
                                                            <asp:ListItem Value="1"></asp:ListItem>
                                                            <asp:ListItem Value="2"></asp:ListItem>
                                                            <asp:ListItem Value="3"></asp:ListItem>
                                                            <asp:ListItem Value="4"></asp:ListItem>
                                                            <asp:ListItem Value="5"></asp:ListItem>
                                                            <asp:ListItem Value="6"></asp:ListItem>
                                                            <asp:ListItem Value="7"></asp:ListItem>
                                                            <asp:ListItem Value="8"></asp:ListItem>
                                                            <asp:ListItem Value="9"></asp:ListItem>
                                                            <asp:ListItem Value="10"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:LinkButton ID="lnkOvertimeRefresh" runat="server" OnClick="lnkOvertimeRefresh_Click">Refresh</asp:LinkButton>
                                                        <asp:GridView ID="dgvOvertime" SkinID="FlowClientMainGrid" runat="server" 
                                                            EmptyDataText="there's no overtime!" AllowSorting="True" 
                                                            onsorting="dgvOvertime_Sorting" onrowdatabound="dgvOvertime_RowDataBound">
                                                            <RowStyle ForeColor="Red" />
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="openA" onclick="autoHide()" ImageUrl="Image/UIPics/Select.gif" runat="server" Target="main"></asp:HyperLink></ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle Height="20px" />
                                                            <AlternatingRowStyle ForeColor="Red" />
                                                        </asp:GridView>
                                                        <flTools:FLWebWizard ID="wizOvertime" runat="server" Active="True" SqlMode="Delay" BindingObjectID="dgvOvertime" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </asp:View>
                                    </asp:MultiView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                        <ajaxToolkit:CollapsiblePanelExtender ID="cpeDemo" runat="Server" TargetControlID="Panel3"
                            ExpandControlID="Panel2" CollapseControlID="Panel2" Collapsed="False" TextLabelID="Label1"
                            ExpandedText="" CollapsedText="" ImageControlID="Image1" ExpandedImage="Image/Ajax/collapse.jpg"
                            CollapsedImage="Image/Ajax/expand.jpg" SuppressPostBack="true" BehaviorID="flowCollaspibleBehavior" />
                        <div id="updown" style="width: 100%; text-align: right;">
                            <img id="imgMax" src="Image/main/down.gif" alt="normal" onclick="imgMax_click()" />
                        </div>
                        <hr id="flowhr" />
                        <iframe id="main" name="main" frameborder="0" marginheight="0" marginwidth="2" scrolling="yes"
                            width="100%" src="DefaultPage.aspx" runat="server"></iframe>
                    </div>
                </td>
            </tr>
        </table>
        <asp:Timer ID="tmFlow" runat="server" Interval="180000" OnTick="tmFlow_Tick">
        </asp:Timer>
    </div>
    </form>

    <script language="javascript" type="text/javascript">
function approveClick() {

    var theEvent = window.event || arguments.callee.caller.arguments[0];
    var srcElement = theEvent.srcElement;
    if (!srcElement) {
        srcElement = theEvent.target;
    }
    var approveImg = srcElement;
    var rowElement = approveImg.parentNode;
    var tds = rowElement.getElementsByTagName("td");
    while (tds.length < 40 && rowElement) {
        rowElement = rowElement.parentNode;
        tds = rowElement.getElementsByTagName("td");
    }
    if (tds.length >= 40) {
        var x = tds.length;
        var approveImg = tds[0].getElementsByTagName('img')[1];
        var LISTID = tds[x - 42].childNodes[0].nodeValue;
        var FLOWPATH = tds[x - 5].childNodes[0].nodeValue;
        var KEYS = tds[x - 18].childNodes[0].nodeValue;
        var VALUES = tds[x - 17].childNodes[0].nodeValue;
        var PROVIDER = tds[x - 14].childNodes[0].nodeValue;
        var replaceText = '$$$$$';
        VALUES = VALUES.replace(/'/g, replaceText);
        var FLOWIMPORTANT = tds[x - 22].childNodes[0].nodeValue;
        var FLOWURGENT = tds[x - 21].childNodes[0].nodeValue;
        var STATUS = tds[x - 20].childNodes[0].nodeValue;
        if (STATUS.indexOf(':') != -1) {
            STATUS = STATUS.split(':')[0];
        }
        var MULTISTEPRETURN = tds[x - 2].childNodes[0].nodeValue;
        var ATTACHMENTS = isHTMLEmpty(tds[x - 1].innerHTML) ? '' : tds[x - 1].childNodes[0].nodeValue;
        var SENDTOID = isHTMLEmpty(tds[x - 24].innerHTML) ? '' : tds[x - 24].childNodes[0].nodeValue;
        var VDSNAME = isHTMLEmpty(tds[x - 10].innerHTML) ? '' : tds[x - 10].childNodes[0].nodeValue;
        var PARAMETERS = isHTMLEmpty(tds[x - 26].innerHTML) ? '' : ConvertUserParameters(tds[x - 26].childNodes[0].nodeValue);
        if (approveImg && approveImg.id == "approveImg") {
            var OPERATETYPE = "Approve";
            var url = 'InnerPages/FlowSubmitConfirm.aspx?LISTID=' + LISTID + '&OPERATETYPE=' + OPERATETYPE + '&KEYS=' + encodeURIComponent(KEYS) + '&VALUES='
             + encodeURIComponent(VALUES) + '&FLOWPATH=' + encodeURIComponent(FLOWPATH) + '&PROVIDER=' + encodeURIComponent(PROVIDER) + '&ISIMPORTANT=' + FLOWIMPORTANT + '&ISURGENT=' + FLOWURGENT + '&STATUS=' + STATUS + '&MULTISTEPRETURN=' + MULTISTEPRETURN + '&ATTACHMENTS=' + encodeURIComponent(ATTACHMENTS) + "&SENDTOID=" + encodeURIComponent(SENDTOID) + PARAMETERS + "&VDSNAME=" + VDSNAME;
            window.open(url, '', 'resizable=yes,scrollbars=yes,width=500,height=410,top=200,left=200');

        }
    }
}

function returnClick() {
    var theEvent = window.event || arguments.callee.caller.arguments[0];
    var srcElement = theEvent.srcElement;
    if (!srcElement) {
        srcElement = theEvent.target;
    }
    var returnImg = srcElement;
    var rowElement = returnImg.parentNode;
    var tds = rowElement.getElementsByTagName("td");
    while (tds.length < 40 && rowElement) {
        rowElement = rowElement.parentNode;
        tds = rowElement.getElementsByTagName("td");
    }
    if (tds.length >= 40) {
        var x = tds.length;
        var returnImg = tds[0].getElementsByTagName('img')[2];
        var LISTID = tds[x - 42].childNodes[0].nodeValue;
        var FLOWPATH = tds[x - 5].childNodes[0].nodeValue;
        var KEYS = tds[x - 18].childNodes[0].nodeValue;
        var VALUES = tds[x - 17].childNodes[0].nodeValue;
        var PROVIDER = tds[x - 14].childNodes[0].nodeValue;
        var replaceText = '$$$$$';
        VALUES = VALUES.replace(/'/g, replaceText);
        var FLOWIMPORTANT = tds[x - 22].childNodes[0].nodeValue;
        var FLOWURGENT = tds[x - 21].childNodes[0].nodeValue;
        var STATUS = tds[x - 20].childNodes[0].nodeValue;
        if (STATUS.indexOf(':') != -1) {
            STATUS = STATUS.split(':')[0];
        }
        var MULTISTEPRETURN = tds[x - 2].childNodes[0].nodeValue;
        var ATTACHMENTS = isHTMLEmpty(tds[x - 1].innerHTML) ? '' : tds[x - 1].childNodes[0].nodeValue;
        var SENDTOID = isHTMLEmpty(tds[x - 24].innerHTML) ? '' : tds[x - 24].childNodes[0].nodeValue;
        var VDSNAME = isHTMLEmpty(tds[x - 10].innerHTML) ? '' : tds[x - 10].childNodes[0].nodeValue;
        if (returnImg && returnImg.id == "returnImg") {
            var OPERATETYPE = "Return";
            var url = 'InnerPages/FlowSubmitConfirm.aspx?LISTID=' + LISTID + '&OPERATETYPE=' + OPERATETYPE + '&KEYS=' + encodeURIComponent(KEYS) + '&VALUES='
             + encodeURIComponent(VALUES) + '&FLOWPATH=' + encodeURIComponent(FLOWPATH) + '&PROVIDER=' + encodeURIComponent(PROVIDER) + '&ISIMPORTANT=' + FLOWIMPORTANT + '&ISURGENT=' + FLOWURGENT + '&STATUS=' + STATUS + '&MULTISTEPRETURN=' + MULTISTEPRETURN + '&ATTACHMENTS=' + encodeURIComponent(ATTACHMENTS) + "&SENDTOID=" + encodeURIComponent(SENDTOID) + "&VDSNAME=" + VDSNAME;
            window.open(url, '', 'resizable=yes,scrollbars=yes,width=500,height=410,top=200,left=200');

        }
    }
}

function queryStringValue(pname) {
    return document.location.search.match(new RegExp("[\?\&]" + pname + "=([^\&]*)(\&?)", "i"));
}

function body_onload() {
    var folder = queryStringValue('FolderName');
    var formname = queryStringValue('FormName');
    var lid = queryStringValue('LISTID');
    var fPath = queryStringValue('FLOWPATH');
    var wString = queryStringValue('WHERESTRING');
    var nMod = queryStringValue('NAVMODE');
    var fMod = queryStringValue('FLNAVMODE');
    var plusApprove = queryStringValue('PLUSAPPROVE');
    var status = queryStringValue('STATUS');
    var sendtoid = queryStringValue('SENDTOID');
    var multi = queryStringValue('MULTISTEPRETURN');
    var attachment = queryStringValue('ATTACHMENTS');
    var isUserInList = '<%= this.IsUserInList() %>';
    if (isUserInList == 'false') {
        alert('user id does not fit!');
        return;
    }
    
   
      // alert(sendtoid[1]);  
    if (folder && formname && lid && fPath && wString && nMod && fMod && plusApprove) {
     
      var isCurrent = '<%= this.IsMailCurrent() %>';
        if(isCurrent == 'false'){
            nMod[1] = '0';
            fMod[1] = '6';
        }
    
        document.getElementById('main').src = folder[1] + '/' + formname[1] + '?LISTID=' + lid[1] + '&FLOWPATH=' + fPath[1] + '&WHERESTRING=' + wString[1] + '&NAVMODE=' + nMod[1] + '&FLNAVMODE=' + fMod[1] + '&PLUSAPPROVE=' + plusApprove[1] + '&STATUS=' + status[1] + '&SENDTOID=' + sendtoid[1] + '&MULTISTEPRETURN=' + multi[1] + '&ATTACHMENTS=' + attachment[1];
    }

    mainPageload();
}
    </script>
</body>
</html>
