/** @description: 去除前後(左右)空白 */
String.prototype.trim = function () {
    var s = this;
    s = s.replace(/(^[\s]*)|([\s]*$)/g, "");

    var trimed = ""; var i = "";

    for (i = 0 ; i < s.length ; i++)
        if (!s.charAt(i).match(' '))
            trimed = trimed.concat(s.charAt(i));

    return trimed;
}


/** @description: 在init中添加OnBlur事件，當grid的Text失去焦點時可以觸發事件，例jqOPOM01的dataGridDetail中QUANTITY、PRICE欄位 */
if (typeof ($) != "undefined") {
    $.extend($.fn.datagrid.defaults.editors, {
        text: {
            init: function (container, options) {
                var maxLength = '';
                var disabled = '';
                if (options != undefined) {
                    if (options.maxLength != undefined && options.maxLength > 0)
                        maxLength = 'maxlength="' + options.maxLength + '"';
                    if (options.disabled != undefined && options.disabled == true) {
                        disabled = 'disabled="disabled"';
                    }
                }
                var input = $('<input type="text" ' + maxLength + disabled + ' >').appendTo(container);
                if (options && options.placeholder) {
                    input.attr('placeholder', options.placeholder);
                }
                if (options.capsLock) {
                    $.changeCapsLock(input, options.capsLock);
                }
                //添加OnBlur事件
                if (options.onblur) {
                    input.blur(function () {
                        eval(options.onblur).call(input);
                    });
                }
                return input;
            },
            getValue: function (target) {
                return $(target).val();
            },
            setValue: function (target, value) {
                $(target).val(value);
            },
            resize: function (target, width) {
                var input = $(target);
                if ($.boxModel == true) {
                    input.width(width - (input.outerWidth() - input.width()));
                } else {
                    input.width(width);
                }
            }
        }
    });
}


//重寫jquery.infolight.js中的initInfoFileUpload()方法
function initInfoFileUpload(infofileUpload) {
    var field = getInfolightOption(infofileUpload).field;
    var form = getInfolightOption(infofileUpload).form;
    var isAutoNum = getInfolightOption(infofileUpload).isAutoNum;
    var filter = getInfolightOption(infofileUpload).filter;
    if (filter != null)
        filter = filter.replace(/;/g, '|');
    var UpLoadFolder = getInfolightOption(infofileUpload).upLoadFolder;
    var ShowButton = getInfolightOption(infofileUpload).showButton;
    var ShowLocalFile = getInfolightOption(infofileUpload).showLocalFile;
    var onSuccess = getInfolightOption(infofileUpload).onSuccess;
    var onError = getInfolightOption(infofileUpload).onError;
    var sizeFieldName = getInfolightOption(infofileUpload).sizeFieldName;
    var accept = getInfolightOption(infofileUpload).accept;

    //$Eidt 20160629 by alex：上傳路徑帶上DB，比如Files\genieERP\INVM01
    UpLoadFolder = uAddDBToUpLoadFolder(UpLoadFolder);

    var id = infofileUpload.attr('id');
    if (id == undefined) {
        id = form + field;
    }
    var name = field;
    if (name == "") {
        name = "name" + form + field;
    }
    id = "infoFileUpload" + id;
    var indexadd = 1;
    while ($('#' + id).length > 0) {
        id = form + field + eval(indexadd);
        indexadd++;
    }
    $(infofileUpload).addClass("info-fileUpload-f").hide();
    $(infofileUpload).removeAttr("name").attr("FileUploadName", name);
    var span = $("<span class=\"info-fileUpload-span\"></span>").insertAfter(infofileUpload);
    span.width($(infofileUpload).width());
    var disabled = getInfolightOption(infofileUpload).disabled;
    if ((disabled != undefined && disabled == true) || infofileUpload.attr('disabled') == 'disabled') {
        disabled = 'disabled="disabled"';
    }
    else disabled = "";
    var infofileUploadvalue = $("<input type=\"text\" class=\"info-fileUpload-value\" name=\"" + field + "\" " + disabled + ">").appendTo(span);
    infofileUploadvalue.attr(infolightOption_attr, $(infofileUpload).attr(infolightOption_attr));
    if ($(infofileUpload).attr("data-options") != "" && $(infofileUpload).attr("data-options") != undefined) {
        if ($(infofileUpload).attr("data-options").indexOf("required") != -1) {
            infofileUploadvalue.attr("data-options", $(infofileUpload).attr("data-options"));
            infofileUploadvalue.validatebox();
        }
    }
    if ($(infofileUpload).attr(infolightOption_attr) && $(infofileUpload).attr(infolightOption_attr).indexOf("required") != -1) {
        infofileUpload.attr(infolightOption_attr, $(infofileUpload).attr(infolightOption_attr).replace(/,required:'required'/, ''));
    }
    //name不能和上面相同，form的load时要给name赋值的。
    //添加一个accept属性，IE10以上，FF,Chrome都支持，设定拓展名后打开的窗口只能选到拓展名的文件。
    var infofileUploadfile = $("<input style=\"width:68px; vertical-align:middle\" type=\"file\" accept=\"" + accept + "\" class=\"info-fileUpload-file\" name=\"" + name + "file" + "\" id=\"" + id + "\" " + disabled + " />").appendTo(span);
    //infofileUploadfile.width(58);
    $.data(infofileUpload, "infofileupload", { value: infofileUploadvalue, file: infofileUploadfile });

    $(infofileUploadfile).unbind().bind('change', function () {
        var filter = getInfolightOption(infofileUpload).filter;
        if (filter != undefined && filter != "") {
            var re_text = filter.split("|");
            var ext = $(this).val().split(".").pop();
            //alert(ext.toLowerCase());
            var isext = false;
            for (var i = 0; i < re_text.length; i++) {
                if (re_text[i].toLowerCase() == ext.toLowerCase()) {
                    isext = true;
                    break;
                }
            }
            if (!isext) {
                alert("File does not have text(" + re_text + ") extension");
                return false;
            }
        }
    });

    if (ShowButton != false) {
        if ($(infofileUpload).width() > 127) {
            infofileUploadvalue.width($(infofileUpload).width() - 127);
        }
    }
    else {
        if ($(infofileUpload).width() > 59) {
            infofileUploadvalue.width($(infofileUpload).width() - 59);
        }
    }
    if (ShowLocalFile == true) {
        infofileUploadvalue.hide();
        infofileUploadfile.width($(infofileUpload).width() - 59);
    }
    if (ShowButton != false) {
        var infofileUploadsubmit = $("<a class='info-fileUpload-button href='#' " + disabled + " >" + uploadButtonText + "</a> ").appendTo(span);
        infofileUploadsubmit.linkbutton({
            iconCls: 'icon-upload',
            plain: true
        });
        $(infofileUploadsubmit).unbind().bind('click', function () {
            if ($(this).attr('class').indexOf('l-btn-disabled') == -1)
                infoFileUploadMethod(infofileUpload);
        });
    }
};

//重寫jquery.infolight.js中的infoFileUploadMethod()方法
function uInfoFileUploadMethod(infofileUpload, onAfterUploadSuccess, onAfterUploadError) {
    var fileexist = true;

    var options = getInfolightOption(infofileUpload);
    var onBeforeUpload = options.onBeforeUpload;
    if (onBeforeUpload != null && onBeforeUpload != undefined) {
        var flag = onBeforeUpload.call(infofileUpload, options);
        if (flag != undefined && flag.toString() == 'false') {
            return;
        }
    }
    var field = options.field;
    var form = options.form;
    var isAutoNum1 = options.isAutoNum;
    var filter1 = options.filter.replace(/;/g, '|');
    var upLoadFolder1 = options.upLoadFolder;
    var showButton = options.showButton;
    var showLocalFile = options.showLocalFile;
    var onSuccess = options.onSuccess;
    var onError = options.onError;
    var sizeFieldName = options.sizeFieldName;
    var fileSizeLimited = options.fileSizeLimited;

    var infofileUploadvalue = $('.info-fileUpload-value', infofileUpload.next())
    var infofileUploadfile = $('.info-fileUpload-file', infofileUpload.next())
    var fileId = infofileUploadfile.attr('id');
    var fileexist = true;

    //$Eidt 20160629 by alex：上傳路徑帶上DB，比如Files\genieERP\INVM01
    upLoadFolder1 = uAddDBToUpLoadFolder(upLoadFolder1);

    if ($('#' + fileId) == undefined || $('#' + fileId).val() == "") {
        alertMessage("nofile");
        fileexist = false;
    }
    else if ($('#' + fileId).val().indexOf("&") != -1) {
        alertMessage("lawname");
        fileexist = false;
    }
    if (fileexist) {
        var postUrl = "";
        if (isSubPath == undefined || isSubPath == true) {
            postUrl = "../handler/UploadHandler.ashx";
        }
        else {
            postUrl = "handler/UploadHandler.ashx";
        }
        $.fn.Error.errorCode = 1400;
        $.ajaxFileUpload({
            url: postUrl, //需要链接到服务器地址   
            secureuri: false,
            data: {
                filter: filter1,
                isAutoNum: isAutoNum1,
                UpLoadFolder: upLoadFolder1,
                fileSizeLimited: fileSizeLimited == undefined ? "" : fileSizeLimited
            },
            fileElementId: fileId, //文件选择框的id属性   
            dataType: 'json', //服务器返回的格式，可以是json   
            success: function (data) {
                if (data['result'] == "success") {
                    if (infofileUploadvalue != undefined && infofileUploadvalue != "")
                        infofileUploadvalue.val(data["message"]);
                    if (showLocalFile) {
                        $('#' + fileId).attr('disabled', 'disabled');
                        $('#' + fileId).attr('isalsoreadonly', 'false');
                    }
                    if (sizeFieldName != undefined) {
                        var size = data["size"];
                        var sizefield = new Object();
                        sizefield[sizeFieldName] = size;
                        $('#' + form).form('updateRow', sizefield);
                    }
                    if (onSuccess != undefined) {
                        onSuccess.call(this, data["message"]);
                    }
                }
                else if (data["result"] == "error") {
                    if (onError != undefined) {
                        onError.call(this, data["message"]);
                    }
                    else {
                        alert(data["message"]);
                    }
                }

                if (onAfterUploadSuccess != undefined) {
                    onAfterUploadSuccess(infofileUploadvalue.val());
                }
                if (onAfterUploadError != undefined) {
                    onAfterUploadError(infofileUploadvalue.val());
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    }
};

/** @description: 檢查瀏覽器及版本 */
function uBrowserCheck() {
    var userAgent = navigator.userAgent, rMsie = /.*(msie) ([\w.]+).*/, rFirefox = /.*(firefox)\/([\w.]+).*/, rOpera = /(opera).+version\/([\w.]+)/, rChrome = /.*(chrome)\/([\w.]+).*/, rSafari = /.*version\/([\w.]+).*(safari).*/;
    var ua = userAgent.toLowerCase();
    function uaMatch(ua) {
        if (!!window.ActiveXObject || "ActiveXObject" in window)
            return { browser: "IE", version: "11" };
        else {
            var match = rMsie.exec(ua);
            if (match != null) {
                return { browser: match[1] || "", version: match[2] || "0" };
            }
            var match = rFirefox.exec(ua);
            if (match != null) {
                return { browser: match[1] || "", version: match[2] || "0" };
            }
            var match = rOpera.exec(ua);
            if (match != null) {
                return { browser: match[1] || "", version: match[2] || "0" };
            }
            var match = rChrome.exec(ua);
            if (match != null) {
                return { browser: match[1] || "", version: match[2] || "0" };
            }
            var match = rSafari.exec(ua);
            if (match != null) {
                return { browser: match[2] || "", version: match[1] || "0" };
            }
            if (match != null) {
                return { browser: "", version: "0" };
            }
        }
    }
    var browserMatch = uaMatch(userAgent.toLowerCase());

    return browserMatch.browser;// browserMatch.version;
}

/** @description: 設置Theme，于uInitTable中使用*/
function uSetTheme() {
    var theme = uGetCookie("uThemeName");
    if (theme == "") theme = "Default";
    var easyuiTheme = $("#easyuiTheme");
    var url = easyuiTheme.attr("href");
    if (typeof (url) != "undefined") {
        var href = url.substring(0, url.indexOf("themes")) + "themes/" + theme + "/easyui.css";
        easyuiTheme.attr("href", href);
    }
}

/** @description: 執行SQL 語句
 * @param {string} sql : 需要執行的SQL語句
 * @param {string} returnData : 是否要傳回數據, Y表示傳回數據(可以在uAfterExecSQL方法中對數據操作)
 * @return {string}
 * @Sample : uExecSQL("Update YearFinal Set AMT = 1000","N");
 */
function uExecSQL(sql, returnData) {
    //parameters參數：sql-要執行的SQL，returnData-是否要取回資料。
    var parameters = sql + "|" + returnData;
    $.ajax({
        type: "POST",
        url: "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp",
        data: "mode=method&method=commExcuteSql&parameters=" + parameters,
        cache: false,
        async: false,
        success: function (data) {
            if (returnData == "N")
                alert("success");
            else if (returnData == "Y") {
                //呼叫前端的uAfterExecSQL方法，並將data傳入
                uAfterExecSQL(data);
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}

/** @description:初始化Table中的tr、td
 * @param {string} markName : 基準元件
 * @param {number} tWidth : table寬度
 * @param {string} tColumns :參數分別：元件名稱，寬度，元件類型，colspan，同一個TD，align，rowspan
 * @return {string}
 * @Sample : var tColumns = [["btMultiQuery,100,button,2,Y"]];
                    uInitTable("queryPanel", 1100, tColumns);
 */
function uInitTable(markName, tWidth, tColumns) {
    uSetTheme();

    var developer = $('#_DEVELOPERID').val();
    if (developer) {//cloud 
        $("#cbPDFPrint").hide();
        $("#lbPDFPrint").hide();
    }

    var table = $('<table ID="table1" style="width: ' + tWidth + 'px;"></table>').insertBefore($('#' + markName));
    table.appendTo($('#queryPanel'));
    //var tr = $('<tr></tr>').appendTo(table);
    for (var i = 0; i < tColumns.length; i++) {
        var tr = $('<tr></tr>').appendTo(table);
        for (var j = 0; j < tColumns[i].length; j++) {
            var columnsArr = tColumns[i][j].split(',');
            var columnName = columnsArr[0];
            var tdWidth = columnsArr[1];
            var cType = columnsArr[2];
            var colspan = typeof (columnsArr[3]) == "undefined" ? 1 : columnsArr[3];
            var sameTd = typeof (columnsArr[4]) == "undefined" ? "N" : columnsArr[4];
            var align = typeof (columnsArr[5]) == "undefined" ? "" : columnsArr[5];
            var rowspan = typeof (columnsArr[6]) == "undefined" ? 1 : columnsArr[6];

            var tdStr = '<td width="' + tdWidth + 'px" valign="top" colspan="' + colspan + '" rowspan="' + rowspan + '"></td>';
            if (align.length != "") tdStr = '<td width="' + tdWidth + 'px" valign="top" align="' + align + '" colspan="' + colspan + ' rowspan="' + rowspan + '"></td>';
            if (cType == "label" || cType == "button" || cType == "buttonToLabel" || cType == "text" || cType == "tree" || cType == "JQTab" || cType == "chart") {
                if (sameTd == "N")
                    var td = $(tdStr).appendTo(tr);
                if (cType == "buttonToLabel") {
                    var temp = $('#' + columnName);
                    temp[0].className = "";
                    temp[0].style.color = "blue";
                    temp[0].style.cursor = "pointer";
                }
                $('#' + columnName).appendTo(td);
            }
            else if (cType == "combo" || cType == "datebox" || cType == "refval" || cType == "options" || cType == "combobox") {
                if (sameTd == "N")
                    var td = $(tdStr).appendTo(tr);
                var panel = $('#' + columnName).next();
                var op = $('#' + columnName).appendTo(td);
                panel.insertAfter(op);
            }
            else if (cType == "dataGrid") {
                if (sameTd == "N")
                    var td = $(tdStr).appendTo(tr);
                var gridDiv = $('#' + columnName).parent().parent().parent().appendTo(td);
            }
            else if (cType == "queryDataGrid") {
                if (sameTd == "N")
                    var td = $(tdStr).appendTo(tr);
                var panel = $('#' + columnName).parent();
                panel.insertBefore(gridDiv);
            }
        }
    }

    //多國語言
    var lbMultiLanguage = $('#lbMultiLanguage');
    if (typeof (lbMultiLanguage[0]) != "undefined") {
        lbMultiLanguage[0].style.color = "white";
        lbMultiLanguage[0].style.fontSize = "1px";
    }
}

/** @description:給數字格式化
 * @param {string} value : 具體數值
 * @Sample : uParseNum(uGetQueryValue("dataFormMaster2PAYBILLMONEY1"))
 */
function uParseNum(value) {
    var num = 0;
    num = isNaN(parseInt(value)) ? 0 : value;
    num = parseInt(String(num).replace(',', ''));
    return num;
}
/** @description: 以下是分頁方法，SP的報表用
 * @param {string} data : 數據
 * @return {string}
 * @Sample : dataGrid.datagrid({ loadFilter: uPagerFilter, url: "", loadMsg: "loadMsg" }).datagrid('loadData', rows);
 */
function uPagerFilter(data) {
    if (typeof data.length == 'number' && typeof data.splice == 'function') {    // is array
        data = {
            total: data.length,
            rows: data
        }
    }
    var dg = $(this);
    var opts = dg.datagrid('options');
    var pager = dg.datagrid('getPager');
    pager.pagination({
        onSelectPage: function (pageNum, pageSize) {
            opts.pageNumber = pageNum;
            opts.pageSize = pageSize;
            pager.pagination('refresh', {
                pageNumber: pageNum,
                pageSize: pageSize
            });
            dg.datagrid('loadData', data);
        }
    });
    if (!data.originalRows) {
        data.originalRows = (data.rows);
    }
    var start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
    var end = start + parseInt(opts.pageSize);
    data.rows = (data.originalRows.slice(start, end));
    return data;
}

/** @description: Grid中顯示超鏈接
 * @param {string} obj : 對象
 * @param {string} title : 開窗標題
 * @param {string} solution : 對象
 * @param {string} pageID : 頁面
 * @param {string} keyName : 主鍵
 * @return {string}
 * @Sample : return "<a href='javascript: void(0)' onclick='uLinkReply(this, \"OPOM01 報價單\", \"gOPO\", \"jqOPOM01.aspx\", \"BILLNO\");'>" + value + "</a>"; 
 */
function uLinkReply(obj, title, solutionID, pageID, keyName) {
    var value = obj.text;
    keyValue = keyName + "=" + value;
    var developer = $('#_DEVELOPERID').val();
    if (developer) {//cloud
        parent.addTab(title, "preview" + developer + "/SD_" + developer + "_" + pageID + "?" + keyValue);
    }
    else {//local
        parent.addTab(title, solutionID + "/" + pageID + "?" + keyValue);
    }
}

/** @description: 獲取元件的值
 * @param {string} columnName : 元件的ID
 * @return {string}
 * @Sample : uGetQueryValue("dataFormMasterBILLNO")，uGetQueryValue("BILLDATE1Query")
 */
function uGetQueryValue(columnName) {
    var queryColumn = $('#' + columnName);
    var value = queryColumn.val();
    var controlClass = queryColumn.attr('class');
    if (controlClass != undefined) {
        if (controlClass.indexOf('easyui-datebox') == 0) {
            value = queryColumn.datebox('getBindingValue');
            //民國日期轉換為公曆
            if (typeof (minGuoDate) != "undefined") {
                if (minGuoDate) {
                    var year = value.split('-')[0] * 1;
                    var month = value.split('-')[1] * 1;
                    var date = value.split('-')[2] * 1;
                    year += 1911;
                    value = year + "-" + month + "-" + date;
                }
            }
        }
        else if (controlClass.indexOf('easyui-datetimebox') == 0) {
            value = queryColumn.datetimebox('getBindingValue');
        }
        else if (controlClass.indexOf('info-combobox') == 0) {
            value = queryColumn.combobox('getValue');
        }
        else if (controlClass.indexOf('info-combogrid') == 0) {
            value = queryColumn.combogrid('getValue');
            value = value == "" ? "*" : value;
        }
        else if (controlClass.indexOf('combo-text') == 0) {
            value = '';
        }
        else if (controlClass.indexOf('info-refval') == 0) {
            value = queryColumn.refval('getValue');
        }
        else if (controlClass.indexOf('info-autocomplete') == 0) {
            value = queryColumn.combobox('getValue');
        }
        else if (controlClass.indexOf('info-options') == 0) {
            //value = $(this).options('getValue');
            value = queryColumn.options('getValue');
        }
    }
    else {
        if ($(this).attr('type') == "checkbox") {
            value = $(this).checkbox('getValue');
        }
        else if (typeof (queryColumn[0]) != "undefined" && queryColumn[0].type == "checkbox") {
            value = queryColumn.checkbox('getValue');
        }
    }
    return value;
}

/** @description: 設置元件的值
 * @param {string} columnName : 元件的ID
 * @param {string} columnValue : 元件的值
 * @return {string}
 * @Sample : uSetQueryValue('YEARQuery', date.getFullYear())
 */
function uSetQueryValue(columnName, columnValue) {
    var queryColumn = $('#' + columnName);
    var value = queryColumn.val();
    var controlClass = queryColumn.attr('class');
    if (controlClass != undefined) {
        if (controlClass.indexOf('easyui-datebox') == 0) {
            queryColumn.datebox('setValue', columnValue);
        }
        else if (controlClass.indexOf('easyui-datetimebox') == 0) {
            queryColumn.datetimebox('getBindingValue', columnValue);
        }
        else if (controlClass.indexOf('info-combobox') == 0) {
            queryColumn.combobox('setValue', columnValue);
        }
        else if (controlClass.indexOf('info-combogrid') == 0) {
            queryColumn.combogrid('setValue', columnValue);
        }
        else if (controlClass.indexOf('combo-text') == 0) {
            queryColumn.val(columnValue);
        }
        else if (controlClass.indexOf('info-refval') == 0) {
            queryColumn.refval('setValue', columnValue);
        }
        else if (controlClass.indexOf('info-autocomplete') == 0) {
            queryColumn.combobox('setValue', columnValue);
        }
        else if (controlClass.indexOf('info-options') == 0) {
            //$(this).options('setValue', columnValue);
            queryColumn.options('setValue', columnValue);
        }
        else if (controlClass.indexOf('easyui-validatebox') == 0) {
            queryColumn.val(columnValue);
        }
    }
    else {
        if ($(this).attr('type') == "checkbox") {
            $(this).checkbox('setValue', columnValue);
        }
        else if (typeof (queryColumn[0]) != "undefined" && queryColumn[0].type == "checkbox") {
            queryColumn.checkbox('setValue', columnValue);
        }
        else {
            queryColumn.val(columnValue);
        }
    }
}

/** @description: View報表查詢，支持多選Table
 * @param {string} dataGridID : 需要查詢的Grid
 * @param {string} queryStr1 : 範圍查詢
 * @param {string} queryStr2 : Like查詢
 * @param {string} queryStr3 : 單個查詢
 * @param {string} multiTableNameAndKey : 多選的Table及Key
 * @return {string}
 * @Sample : uReportMultiQuery("dataGridMaster", "BILLDATE;BILLNO;CUSTID;PRODUCTID;PERSONID", "PRODDESC", "", "BASCUSTOMER,CUSTID;BASPRODUCT,PRODUCTID;BASPERSON,PERSONID");
 */
function uReportMultiQuery(dataGridID, queryStr1, queryStr2, queryStr3, multiTableNameAndKey) {
    var setstr = uGetReportMultiQuerySetWhere(queryStr1, queryStr2, queryStr3, multiTableNameAndKey);
    $('#' + dataGridID).datagrid('setWhere', setstr);
    $('#' + dataGridID).datagrid('reload');

    $.ajax({
        type: 'POST',
        beforeSend: uAjaxLoading,
        success: function (data) {
            setTimeout('uAjaxLoadEnd();', uAjaxLoadingTime);
        }
    });
    //HTML5 Web Storage
    window.sessionStorage.setItem("uReportData", "");
}

var paraFilter = "";
/** @description: 整合查詢的Where語句，在uReportMultiQuery、uOpenPivotDialogView中使用
 * @param {string} queryStr1 : 範圍查詢
 * @param {string} queryStr2 : Like查詢
 * @param {string} queryStr3 : 單個查詢
 * @param {string} multiTableNameAndKey : 多選的Table及Key
 * @return {string}
 */
function uGetReportMultiQuerySetWhere(queryStr1, queryStr2, queryStr3, multiTableNameAndKey) {
    var setstr = "";
    var queryName1 = "";
    var queryName2 = "";
    var tempValue1 = "";
    var tempValue2 = "";

    var queryColumn1 = queryStr1.split(';');
    var queryColumn2 = queryStr2.split(';');
    var queryColumn3 = queryStr3.split(';');

    //Between Compare 
    for (var i = 0; i < queryColumn1.length; i++) {
        queryName1 = queryColumn1[i] + '1Query';
        queryName2 = queryColumn1[i] + '2Query';
        tempValue1 = uGetQueryValue(queryName1);
        tempValue2 = uGetQueryValue(queryName2);
        if (tempValue1 == "*")
            tempValue1 = "";
        else
            paraFilter += getInfolightOption($('#' + queryName1)).caption + tempValue1 + " ";

        if (tempValue2 == "*")
            tempValue2 = "zzzz";
        else
            paraFilter += getInfolightOption($('#' + queryName2)).caption + tempValue2 + " ";

        setstr += queryColumn1[i] + " >= '" + tempValue1 + "' AND " + queryColumn1[i] + " <= '" + tempValue2 + "' AND ";
    }

    //Fill Text Search
    for (var i1 = 0; i1 < queryColumn2.length; i1++) {
        queryName1 = queryColumn2[i1] + '1Query';
        tempValue1 = uGetQueryValue(queryName1);

        if (tempValue1 != "*" && tempValue1 != "" && typeof (tempValue1) != "undefined")
            setstr += queryColumn2[i1] + " Like '%" + tempValue1 + "%' AND ";
    }
    //Single Equal
    for (var i = 0; i < queryColumn3.length; i++) {
        queryName1 = queryColumn3[i] + '1Query';
        tempValue1 = uGetQueryValue(queryName1);

        if (tempValue1 != "*" && tempValue1 != "" && typeof (tempValue1) != "undefined")
            setstr += queryColumn3[i] + " = '" + tempValue1 + "' AND ";
    }

    //多選
    var multiArr = multiTableNameAndKey.split(';');
    for (var j = 0; j < multiArr.length; j++) {
        if (multiArr[j].length > 0) {
            var keyValue = "";
            var multiTableName = multiArr[j].split(',')[0];
            var multiTableKey = multiArr[j].split(',')[1];
            if (typeof (eval('ms' + multiTableName)) != "undefined")
                var multiSelectData = eval('ms' + multiTableName);
            if (multiSelectData.length > 0) {
                for (var i = 0; i < multiSelectData.length; i++) {
                    keyValue += "'" + multiSelectData[i].IDNO + "',";
                }
                keyValue = keyValue.substr(0, keyValue.length - 1);
                setstr += multiTableKey + " IN (" + keyValue + ") AND ";
            }
        }
    }

    setstr = setstr.substr(0, setstr.length - 4);
    return setstr;
}

var uReportData;
/** @description: SP報表查詢，支持多選Table。現在不用該方法
 * @param {string} dataGridID : 需要查詢的Grid
 * @param {string} queryStr : 查詢條件 
 * @param {string} multiTableNameAndKey : 多選的Table及Key
 * @param {string} FUNCTAG : GEXBAS_TMP.FUNCTIONTAG 
 * @return {string}
 * @Sample : uReportSPQuery("dataGridMaster", "sBILLDATE1,sBILLDATE2,sCUSTID1,sCUSTID2,sPRODUCTID1,sPRODUCTID2,sPERSONID1,sPERSONID2,sISSIGN,sLOGINID", "BASCUSTOMER,CUSTID;BASPRODUCT,PRODUCTID;BASPERSON,PERSONID");
 */
function uReportSPQuery(dataGridID, queryStr, multiTableNameAndKey, FUNCTAG) {
    var setstr = uGetSPParam(queryStr, multiTableNameAndKey, FUNCTAG);
    $('#' + dataGridID).datagrid('setWhere', setstr);
    $('#' + dataGridID).datagrid('reload');
}

/** @description: SP報表查詢，支持多選Table，翻頁不用再次下SQL
 * @param {string} dataGridID : 需要查詢的Grid
 * @param {string} assemblyName : 後端程式 
 * @param {string} methodName : 後端方法 
 * @param {string} multiTableNameAndKey : 多選的Table及Key
 * @param {string} FUNCTAG : GEXBAS_TMP.FUNCTIONTAG 
 * @return {string}
 * @Sample : uReportSPQuery2("dataGridMaster", "srINV", "GetData_GEXRPT_INVR07_JS", "sBILLDATE1,sBILLDATE2,sPRODCATEID1,sPRODCATEID2,sPRODUCTID1,sPRODUCTID2,sWAREID1,sWAREID2,sLoginID", "BASPRODCATEGORY,PRODCATEID;BASPRODUCT,PRODUCTID;BASWAREHOUSE,WAREID");
 */
function uReportSPQuery2(dataGridID, assemblyName, methodName, queryStr, multiTableNameAndKey, FUNCTAG) {
    var setstr = uGetSPParam(queryStr, multiTableNameAndKey, FUNCTAG);
    uExecSPFunction(dataGridID, assemblyName, methodName);

    //HTML5 Web Storage
    var sessionValue = uJsonToString(uReportData);
    window.sessionStorage.setItem("uReportData", sessionValue);
    SPWhereText = setstr;
}

/**jquery easyui loading的關閉時間長度，毫秒級*/
var uAjaxLoadingTime = 1000;

/**開啟jquery easyui loading css效果，用於報表查詢：uExecSPFunction，uReportMultiQuery */
function uAjaxLoading(dataGridID) {
    //$.fn.datagrid.defaults.loadMsg來自于js\locale\easyui-lang-zh_TW.js 
    $("<div class=\"datagrid-mask\"></div>").css({ display: "block", width: "100%", height: $(window).height() }).appendTo("body");
    $("<div class=\"datagrid-mask-msg\"></div>").html($.fn.datagrid.defaults.loadMsg).appendTo("body").css({ display: "block", left: ($(document.body).outerWidth(true) - 190) / 2, top: ($(window).height() - 20) / 2 });
}

/**關閉jquery easyui loading css效果 */
function uAjaxLoadEnd() {
    //js\jquery.easyui.min.js 8120行
    $(".datagrid-mask").remove();
    $(".datagrid-mask-msg").remove();
}

/** @description: 設置Cookie，Cookie大小限制4K*/
function uSetCookie(cookieName, value, expiredays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + expiredays);
    document.cookie = cookieName + "=" + escape(value) + ((expiredays == null) ? "" : ";path=/;expires=" + exdate.toGMTString());
}

/** @description: 讀取Cookie */
function uGetCookie(cookieName) {
    //if (document.cookie.length > 0) {
    //    cStart = document.cookie.indexOf(cookieName + "=")
    //    if (cStart != -1) {
    //        cStart = cStart + cookieName.length + 1
    //        cEnd = document.cookie.indexOf(";", cStart)
    //        if (cEnd == -1) cEnd = document.cookie.length;
    //        return unescape(document.cookie.substring(cStart, cEnd))
    //    }
    //}
    var cookieValue = "";

    var cookies = document.cookie.split(';');
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        var temp = cookie.split('=');
        if (temp[0].trim() == cookieName) {
            cookieValue = temp[1];
        }
    }
    return cookieValue;
}

/** @用於呼叫SP的後端方法，然後取值塞入JQDataGrid。 */
function uExecSPFunction(dataGridID, assemblyName, methodName) {
    try {
        $.ajax({
            type: 'POST',
            url: '../handler/jqDataHandle.ashx?RemoteName=' + assemblyName,
            data: 'mode=method&method=' + methodName + '&parameters=' + SPParam,
            cache: false,
            async: false,
            beforeSend: uAjaxLoading(dataGridID),
            success: function (data) {
                setTimeout('uAjaxLoadEnd();', uAjaxLoadingTime);
                uReportData = data;
                var rows = $.parseJSON(data);
                // Clear dataGrid
                var rowsLength = $('#' + dataGridID).datagrid('getRows').length;
                for (var i = rowsLength - 1; i >= 0; i--) {
                    $('#' + dataGridID).datagrid('deleteRow', i);
                }
                // Set Value
                if (rows.length == 0)
                    alert('No Data !!');
                else {
                    $('#' + dataGridID).datagrid({ loadFilter: uPagerFilter, url: "", loadMsg: "loadMsg" }).datagrid('loadData', rows);
                }
            }
        });
    }
    catch (ex) {
        alert(ex);
    }
}

/** @description: SP報表查詢，SPParam記錄參數順序以及值，SPWhereString記錄where語句顯示與報表中
 * @param {string} queryStr : 查詢條件
 * @param {string} multiTableNameAndKey : 多選的Table及Key
 * @param {string} FUNCTAG : GEXBAS_TMP.FUNCTIONTAG
 * @return {string}
 * @Sample : uSetQueryValue('YEARQuery', date.getFullYear())
 */
function uGetSPParam(queryStr, multiTableNameAndKey, FUNCTAG) {
    //頁面中的變量：SPParam，SPWhereString
    var setstr = "";
    SPParam = ""; SPWhereString = "";
    var queryName1 = "", queryName2 = "", tempValue1 = "", tempValue2 = "";
    queryStr = queryStr.replace(/s/g, "");
    var queryColumn = queryStr.split(',');

    for (var i = 0; i < queryColumn.length; i++) {
        if (queryColumn[i].indexOf("1") > 0 || queryColumn[i].indexOf("2") > 0) {
            //範圍查詢
            queryName1 = queryColumn[i] + 'Query';
            tempValue1 = uGetQueryValue(queryName1);
            //if (queryColumn[i].indexOf("1") > 0) operSymbol = " >= '";
            //else if (queryColumn[i].indexOf("2") > 0) operSymbol = " <= '";
            operSymbol = " = '";
            if (typeof (tempValue1) != "undefined") {
                SPWhereString += tempValue1 != "*" ? queryColumn[i] + operSymbol + tempValue1 + "' AND " : "";
                SPParam += tempValue1 + '|';
                setstr += queryColumn[i] + operSymbol + tempValue1 + "' AND ";
            }
            else {
                queryName1 = queryColumn[i] + '1Query';
                queryName2 = queryColumn[i] + '2Query';
                tempValue1 = uGetQueryValue(queryName1);
                tempValue2 = uGetQueryValue(queryName2);
                if (typeof (tempValue2) != "undefined") {
                    //合併(YEARMONTH)，比如前後兩個下拉列表，前一個2015，後一個06
                    SPWhereString += queryColumn[i] + " = '" + tempValue1 + tempValue2 + "' AND ";
                    SPParam += tempValue1 + tempValue2 + '|';
                    setstr += queryColumn[i] + " = '" + tempValue1 + tempValue2 + "' AND ";
                }
            }
        }
            //Modify By Michael
        else if (queryColumn[i].toUpperCase() == "FUNCTAG" && typeof (FUNCTAG) != "undefined") {
            //FUNCTAG
            SPParam += FUNCTAG + '|';
            setstr += "FUNCTAG = '" + FUNCTAG + "' AND ";
        }
            //Modify By Michael
        else if (queryColumn[i].toUpperCase() == "LOGINID") {
            //LOGINID
            var loginID = uGetUserID();
            SPParam += loginID + '|';
            setstr += "LOGINID = '" + loginID + "' AND ";
        }
        else if (queryColumn[i].toUpperCase() == "CLASS") {
            //$2015-10-14 by alex：ACTR報表，4開頭都是1 , 5開頭是2, 6開頭是 3
            var actClass = 1;
            if (typeof (FUNCTAG) != "undefined") {
                if (FUNCTAG.indexOf("ACTR4") >= 0) actClass = 1;
                if (FUNCTAG.indexOf("ACTR5") >= 0) actClass = 2;
                if (FUNCTAG.indexOf("ACTR6") >= 0) actClass = 3;
            }
            SPParam += actClass + '|';
        }
        else if (queryColumn[i].toUpperCase() == "ALL") {
            //jqACTR1X使用
            SPParam += SPParam.replace(/\|/g, "") + '|';
        }
        else {
            queryName1 = queryColumn[i] + '1Query';
            queryName2 = queryColumn[i] + '2Query';
            tempValue1 = uGetQueryValue(queryName1);
            tempValue2 = uGetQueryValue(queryName2);
            if (typeof (tempValue2) != "undefined") {
                //合併(YEARMONTH)，比如前後兩個下拉列表，前一個2015，後一個06
                SPWhereString += queryColumn[i] + " = '" + tempValue1 + tempValue2 + "' AND ";
                SPParam += tempValue1 + tempValue2 + '|';
                setstr += queryColumn[i] + " = '" + tempValue1 + tempValue2 + "' AND ";
            }
            else {
                //Modify By Michael
                tempValue1 = uGetQueryValue(queryColumn[i] + 'Query');
                //單個查詢 
                if (tempValue1 != "*" && tempValue1 != "" && typeof (tempValue1) != "undefined")
                    SPWhereString += queryColumn[i] + " = '" + tempValue1 + "' AND ";
                SPParam += tempValue1 + '|';
                setstr += queryColumn[i] + " = '" + tempValue1 + "' AND ";
            }
        }
    }

    SPParam = SPParam.substr(0, SPParam.length - 1);
    SPWhereString = SPWhereString.substr(0, SPWhereString.length - 4);
    setstr = setstr.substr(0, setstr.length - 4);

    //多選
    var multiArr = multiTableNameAndKey.split(';');
    for (var j = 0; j < multiArr.length; j++) {
        var keyValue = "";
        var multiTableName = multiArr[j].split(',')[0];
        var multiTableKey = multiArr[j].split(',')[1];
        if (multiTableName.length == 0) continue;
        if (typeof (eval('ms' + multiTableName)) != "undefined")
            var multiSelectData = eval('ms' + multiTableName);
        if (multiSelectData.length > 0) {
            for (var i = 0; i < multiSelectData.length; i++) {
                keyValue += "'" + multiSelectData[i].IDNO + "',";
            }
            keyValue = keyValue.substr(0, keyValue.length - 1);
            SPWhereString += multiTableKey + " IN (" + keyValue + ") AND ";
        }
    }
    return setstr;
    //queryStr3-合併(YEARMONTH)，比如前後兩個下拉列表，前一個2015，後一個06
    //if (typeof (queryStr3) != "undefined") {
    //    var queryColumn3 = queryStr3.split(';');
    //    for (var i = 0; i < queryColumn3.length; i++) {
    //        queryName1 = queryColumn3[i] + '1Query';
    //        queryName2 = queryColumn3[i] + '2Query';
    //        tempValue1 = uGetQueryValue(queryName1);
    //        tempValue2 = uGetQueryValue(queryName2);

    //        SPWhereString += queryColumn3[i] + " = '" + tempValue1 + tempValue2 + "' AND ";
    //        SPParam += tempValue1 + tempValue2 + '|';
    //    }
    //}
}

/** @description: 設置查詢默認值
 * @param {string} queryStr1 : 日期查詢
 * @param {string} queryStr2 : 範圍查詢
 * @param {string} queryStr3 : 單個查詢
 * @return {string}
 * @Sample : uSetQueryDefault("BILLDATE", "BILLDATE;BILLNO;CUSTID;PRODUCTID;PERSONID", "PRODDESC");
 */
function uSetQueryDefault(queryStr1, queryStr2, queryStr3) {
    var queryColumn1 = queryStr1.split(';');
    var queryColumn2 = queryStr2.split(';');
    var queryColumn3 = queryStr3.split(';');
    if (typeof (jqDateChar) == "undefined") jqDateChar = "-";
    for (var i = 0; i < queryColumn2.length; i++) {
        if (queryColumn1.indexOf(queryColumn2[i]) >= 0) {
            //jqDateChar為日期分隔符，可自定義
            var tempValue = uDashToOther(uGetTodayDash(), jqDateChar);
            uSetQueryValue(queryColumn2[i] + '1Query', tempValue);
            uSetQueryValue(queryColumn2[i] + '2Query', tempValue);
        }
        else {
            uSetQueryValue(queryColumn2[i] + '1Query', '*');
            uSetQueryValue(queryColumn2[i] + '2Query', '*');
        }
    }
    for (var i = 0; i < queryColumn3.length; i++) {
        if (queryColumn1.indexOf(queryColumn3[i]) >= 0) {
            var defaultDate = uGetTodayDash();
            if (typeof (minGuoDate) != "undefined") {
                if (minGuoDate) {
                    var tempValue = uDashToOther(uGetMinGuoTodayDash(), jqDateChar);
                    defaultDate = tempValue;
                }
                else {
                    var tempValue = uDashToOther(uGetTodayDash(), jqDateChar);
                    defaultDate = tempValue;
                }
            }
            uSetQueryValue(queryColumn3[i] + '1Query', defaultDate);
            uSetQueryValue(queryColumn3[i] + 'Query', defaultDate);
        }
        else {
            uSetQueryValue(queryColumn3[i] + '1Query', '*');
            uSetQueryValue(queryColumn3[i] + 'Query', '*');
        }
    }
}

/** @description: 獲取QueryString
 * @param {string} name : 需要獲取的name
 * @return {string}
 * @Sample : uGetQueryString("BILLNO");
 */
function uGetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = decodeURIComponent(window.location.search.substr(1)).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

//--------------------------------對日期操作------------------------
/** @description: 獲取今天日期，格式yyyyMMdd */
function uGetToday() {
    var date = uGetTodayDash().split("-");
    return date[0] + date[1] + date[2];
}

/** @description: 獲取今天日期，格式yyyy-MM-dd */
function uGetTodayDash() {
    var date = new Date();
    var year = date.getFullYear().toString();
    var month = (date.getMonth() + 1).toString();
    if (month.length == 1) {
        month = "0" + month;
    }
    var day = date.getDate().toString();
    if (day.length == 1) {
        day = "0" + day;
    }
    return year + '-' + month + '-' + day;
}

/** @description: 獲取今天日期轉換為民國，格式yyyMMdd */
function uGetMinGuoToday() {
    var date = uGetMinGuoTodayDash().split("-");
    return date[0] + date[1] + date[2];
}

/** @description: 獲取今天日期轉換為民國，格式yyy-MM-dd */
function uGetMinGuoTodayDash() {
    var date = new Date();
    var year = (date.getFullYear() - 1911).toString();
    var month = (date.getMonth() + 1).toString();
    if (month.length == 1) {
        month = "0" + month;
    }
    var day = date.getDate().toString();
    if (day.length == 1) {
        day = "0" + day;
    }
    return year + '-' + month + '-' + day;
}

/** @description: 獲取這月第一天，格式yyyyMMdd */
function uGetMonthFirstDate() {
    var date = uGetMonthFirstDateDash().split("-");
    return date[0] + date[1] + date[2];
}

/** @description: 獲取這月第一天，格式yyyy-MM-dd */
function uGetMonthFirstDateDash() {
    var date = new Date();
    var year = (date.getFullYear()).toString();
    var month = (date.getMonth() + 1).toString();
    if (month.length == 1) {
        month = "0" + month;
    }
    return year + '-' + month + '-01';
}

/** @description: 獲取這月第一天轉換為民國，格式yyyMMdd */
function uGetMinGuoMonthFirstDate() {
    var date = uGetMinGuoTodayDash().split("-");
    return date[0] + date[1] + date[2];
}

/** @description: 獲取這月第一天轉換為民國，格式yyy-MM-dd */
function uGetMinGuoMonthFirstDateDash() {
    var date = new Date();
    var year = (date.getFullYear() - 1911).toString();
    var month = (date.getMonth() + 1).toString();
    if (month.length == 1) {
        month = "0" + month;
    }
    return year + '-' + month + '-01';
}

/** @description: 獲取這月最後一天，格式yyyyMMdd */
function uGetMonthLastDate() {
    var date = uGetMonthLastDateDash().split("-");
    return date[0] + date[1] + date[2];
}

/** @description: 獲取這月最後一天，格式yyyy-MM-dd */
function uGetMonthLastDateDash() {
    var date = new Date();
    date.setDate(1);
    date.setMonth(date.getMonth() + 1);
    var date2 = new Date(date.getTime() - 1000 * 60 * 60 * 24);
    var year = (date2.getFullYear()).toString();
    var month = (date2.getMonth() + 1).toString();
    if (month.length == 1) {
        month = "0" + month;
    }
    var day = date2.getDate().toString();
    if (day.length == 1) {
        day = "0" + day;
    }
    return year + "-" + month + "-" + day;
}

/** @description: 獲取這月最後一天換為民國，格式yyyMMdd */
function uGetMinGuoMonthLastDate() {
    var date = uGetMinGuoMonthLastDateDash().split("-");
    return date[0] + date[1] + date[2];
}

/** @description: 獲取這月最後一天換為民國，格式yyy-MM-dd */
function uGetMinGuoMonthLastDateDash() {
    var date = new Date();
    date.setDate(1);
    date.setMonth(date.getMonth() + 1);
    var date2 = new Date(date.getTime() - 1000 * 60 * 60 * 24);
    var year = (date2.getFullYear() - 1911).toString();
    var month = (date2.getMonth() + 1).toString();
    if (month.length == 1) {
        month = "0" + month;
    }
    var day = date2.getDate().toString();
    if (day.length == 1) {
        day = "0" + day;
    }
    return year + "-" + month + "-" + day;
}

/** @description: 將字符串轉換為日期 
 * @param {string} str : 日期字符串
 * @Sample : uStrToDate(billdateStr)
 */
function uStrToDate(str) {
    var date = new Date();
    var strArr = [];
    if (str.indexOf("T") > 0)
        str = str.split("T")[0];

    if (str.indexOf("-") > 0) strArr = str.split("-");
    if (str.indexOf("/") > 0) strArr = str.split("/");
    if (str.length == 8) {
        strArr.push(str.substr(0, 4));
        strArr.push(str.substr(4, 2));
        strArr.push(str.substr(6, 2));
    }

    if (typeof (strArr) != "undefined") {
        date.setYear(strArr[0]);
        date.setMonth(strArr[1] * 1 - 1);
        date.setDate(strArr[2]);
    }
    return date;
}

/** @description: 將日期轉換為字符串 
 * @param {date} date : 日期
 * @Sample :uDateToStr(closeDate)
 */
function uDateToStr(date) {
    var str;
    var year = date.getFullYear().toString();
    var month = (date.getMonth() + 1).toString();
    if (month.length == 1) {
        month = "0" + month;
    }
    var day = date.getDate().toString();
    if (day.length == 1) {
        day = "0" + day;
    }
    str = year + '-' + month + '-' + day;
    return str;
}

/** @description: 轉換分隔符 
 * @param {string} str : 字符串
 * @param {string} char : 分隔符
* @Sample : uDashToOther(uGetTodayDash(), jqDateChar); */
function uDashToOther(str, char) {
    if (typeof (char) == "undefined") char = "-";
    return str.replace(/-/g, char);
}
//--------------------------------對日期操作------------------------

/** @description: 獲取登錄數據庫  
* @Sample : var database = uGetDataBase(); */
function uGetDataBase() {
    var database = "";
    var developer = $('#_DEVELOPERID').val();
    var cookieName = "";
    if (developer)
        cookieName = "database";
    else
        cookieName = "database";

    database = uGetCookie(cookieName);
    return database;
}

/** @description: 獲取登錄人員編號  
* @Sample : var solution = uGetSolution(); */
function uGetSolution() {
    var solution = "";
    var developer = $('#_DEVELOPERID').val();
    var cookieName = "";
    if (developer)
        cookieName = "solution";
    else
        cookieName = "solution";

    solution = uGetCookie(cookieName);
    return solution;
}

/** @description: 獲取登錄人員編號  
* @Sample : var loginID = uGetUserID(); */
function uGetUserID() {
    var userID = "";
    var developer = $('#_DEVELOPERID').val();
    var cookieName = "";
    if (developer)
        cookieName = "ruserID";
    else
        cookieName = "username";

    userID = uGetCookie(cookieName);
    return userID;
}

/** @description: 獲取登錄人員名稱  
* @Sample : var loginID = uGetUserName(); */
function uGetUserName() {
    var userID = uGetUserID();
    var userName = "";

    $.ajax({
        type: 'POST',
        url: '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp',
        data: 'mode=method&method=getUserName&parameters=' + userID,
        cache: false,
        async: false,
        success: function (data) {
            userName = data;
        }
    });

    return userName;
}

/** @description: 獲取倉庫編號
* @Sample : var wareID = uGetWareID(); */
function uGetWareID() {
    var wareID = "";

    $.ajax({
        type: 'POST',
        url: '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp',
        data: 'mode=method&method=getWareID&parameters=1',
        cache: false,
        async: false,
        success: function (data) {
            wareID = data;
        }
    });

    return wareID;
}

/** @description: 獲取幣別編號
* @Sample : var currencyID = uGetCURRENCYID(); */
function uGetCURRENCYID(url) {
    if (typeof (url) == "undefined")
        url = '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp';
    var currencyID = "";
    $.ajax({
        type: 'POST',
        url: url,
        data: 'mode=method&method=getParam&parameters=USEDCURRENCY|SYS',
        cache: false,
        async: false,
        success: function (data) {
            currencyID = data;
        }
    });
    return currencyID;
}

/** @description: 計算浮點數
 * @param {number} arg1 : 第一個數字
 * @param {number} arg2 : 第二個數字
 * @param {string} operators : 運算符號(加減乘)
 * @param {number} decimal : 小數位
* @Sample : uAccCalc(sumQuantity, dQty, "-", uQtyDecimal); */
function uAccCalc(arg1, arg2, operators, decimal) {
    if (typeof (decimal) == "undefined") decimal = 0;
    if (typeof (arg1) == "undefined") arg1 = "0";
    if (typeof (arg2) == "undefined") arg2 = "0";
    var r1 = 0, r2 = 0, m = 0, s1 = arg1.toString(), s2 = arg2.toString();
    try { r1 += arg1.toString().split(".")[1].length; } catch (e) { }
    try { r2 += arg2.toString().split(".")[1].length; } catch (e) { }
    if (operators == "+" || operators == "-") {
        m = Math.pow(10, Math.max(r1, r2));
        if (r1 > r2) {
            for (var i = 0; i < r1 - r2; i++) s2 = s2 + "0";
        }
        else {
            for (var i = 0; i < r2 - r1; i++) s1 = s1 + "0";
        }
    }
    else
        m = Math.pow(10, r1 + r2);

    var result = 0;
    var s1Num = Number(s1.replace(".", "").replace(",", ""));
    var s2Num = Number(s2.replace(".", "").replace(",", ""));
    switch (operators) {
        case "+": result = (s1Num + s2Num) / m; break;
        case "-": result = (s1Num - s2Num) / m; break;
        case "*": result = (s1Num * s2Num) / m; break;
        case "/": result = Number(s1) / Number(s2); break;
    }
    return Number(result).toFixed(decimal);
}

/** @description: 給JQDataGrid非編輯狀態的單元格賦值
 * @param {string} detailGrid : 明細對應的dataGrid
 * @param {number} rowIndex : 行數
 * @param {string} columnName : 欄位名稱
 * @param {string} columnValue : 欄位值
* @Sample : uSetDetailValue($("#dataGridDetail"), 1, "WAREID", row.WAREID); */
function uSetDetailValue(detailGrid, rowIndex, columnName, columnValue) {
    var temp = detailGrid.datagrid('getEditor', { index: rowIndex, field: columnName });
    if (temp != null)
        temp.actions.setValue(temp.target, columnValue);
    else {
        var rows = detailGrid.datagrid("getRows");
        var newRow = new Object();
        var dColumns = detailGrid.datagrid("getColumnFields");
        for (var i = 0; i < dColumns.length; i++) {
            if (dColumns[i] == columnName)
                newRow[dColumns[i]] = columnValue;
            else
                newRow[dColumns[i]] = eval("rows[rowIndex]." + dColumns[i]);
        }
        detailGrid.datagrid("updateRow", { index: rowIndex, row: newRow });
    }
}

/** @description: 獲取JQDataGrid非編輯狀態的單元格的值
 * @param {string} detailGrid : 明細對應的dataGrid
 * @param {number} rowIndex : 行數
 * @param {string} columnName : 欄位名稱 
* @Sample : uGetDetailValue(dataGrid, i, "ENABLED"); */
function uGetDetailValue(detailGrid, rowIndex, columnName) {
    var temp = detailGrid.datagrid('getEditor', { index: rowIndex, field: columnName });
    var retStr = "noEditor";
    if (temp != null)
        retStr = temp.actions.getValue(temp.target);
    return retStr;
}

/** @description: 多選開窗并Reload對應JQDataGrid
 * @param {string} JQDialogID : 明細對應的dataGrid
 * @param {string} reloadGridID1 : 需要reload的dataGrid，可為空
 * @param {string} reloadGridID2 : 需要reload的dataGrid，可為空
 * @param {string} reloadGridID3 : 需要reload的dataGrid，可為空
* @Sample : uOpenMulitDialog("JQDialogMultiSelect1", "dataGridMultiSelect1", "dataGridMultiSelect2"); */
function uOpenMulitDialog(JQDialogID, reloadGridID1, reloadGridID2, reloadGridID3) {
    if (typeof (reloadGridID1) != "undefined") $('#' + reloadGridID1).datagrid('reload');
    if (typeof (reloadGridID2) != "undefined") $('#' + reloadGridID2).datagrid('reload');
    if (typeof (reloadGridID3) != "undefined") $('#' + reloadGridID3).datagrid('reload');
    var dialogID = $('#' + JQDialogID);
    var top = getInfolightOption(dialogID).dialogTop;
    var left = getInfolightOption(dialogID).dialogLeft;
    dialogID.dialog('open');
    dialogID.dialog('move', { top: top, left: left });
}

/** @description: 刪除明細檔後計算主檔總計
 * @param {string} masterForm : 主檔的DataFormID
 * @param {string} mQtyName : 主檔數量合計欄位
 * @param {string} mTotalName : 主檔未稅合計欄位
 * @param {string} mSumName : 主檔總計欄位
 * @param {string} mTaxName : 主檔營業稅欄位
 * @param {number} dQty : 明細檔數量
 * @param {number} dSub : 明細檔金額
 * @param {number} dTax : 明細檔稅額
* @Sample : uDeleteDetail("dataFormMaster1", "SUMQUALITY", "TOTALAMOUNT", "SUMAMOUNT", "TAXAMOUNT", rowData.QUANTITY, rowData.SUBAMOUNT, rowData.TAXAMOUNT); */
function uDeleteDetail(masterForm, mQtyName, mTotalName, mSumName, mTaxName, dQty, dSub, dTax) {
    var sumQuantity = $('#' + masterForm + mQtyName).val();
    var totalAmount = $('#' + masterForm + mTotalName).val();
    var sumAmount = $('#' + masterForm + mSumName).val();
    sumQuantity = uAccCalc(sumQuantity, dQty, "-", uQtyDecimal);
    totalAmount = uAccCalc(totalAmount, dSub, "-", uSumDecimal);
    sumAmount = uAccCalc(sumAmount, dTax, "-", uSumDecimal);
    $('#' + masterForm + mQtyName).val(sumQuantity);//數量合計
    $('#' + masterForm + mTotalName).val(totalAmount);//未稅合計
    $('#' + masterForm + mSumName).val(sumAmount);//總計
    $('#' + masterForm + mTaxName).val(uAccCalc(sumAmount, totalAmount, "-", uTaxDecimal));//營業稅
}

/** @description: 單據存檔前，若表身無資料，不得存檔(檢查表身產品編號不為空，筆數大於0)
 * @Sample : uCheckDetail("dataGridDetail") */
function uCheckDetail(detailGridID) {
    var detailGrid = $("#" + detailGridID);
    var rows = detailGrid.datagrid("getRows");
    var ret = true;
    var msg = "";
    if (rows.length == 0) {
        msg = "明細檔不能為空。";
        ret = false;
    }
    else {
        for (var i = 0; i < rows.length; i++) {
            var editorP = uGetDetailValue(detailGrid, i, "PRODUCTID");
            var editorQ = uGetDetailValue(detailGrid, i, "QUANTITY");
            var PRODUCTID = rows[i].PRODUCTID;
            var QUANTITY = rows[i].QUANTITY;
            if (editorP == "")
                msg = "明細檔產品變化不能為空。";
            else if (editorP == "noEditor" && PRODUCTID == "")
                msg = "明細檔產品變化不能為空。";
            if (editorQ == 0)
                msg = "明細檔數量不能為0。";
            else if (editorQ == "noEditor" && QUANTITY == 0)
                msg = "明細檔數量不能為0。";
        }
    }
    if (msg.length > 0) {
        alert(msg); ret = false;
    }
    return ret;
}

/** @description: 獲取PARAMETER中設定的稅率和小數位參數 */
var uTaxRate = 1.05, uPriceDecimal = 0, uQtyDecimal = 0, uSubDecimal = 0, uSumDecimal = 0, uTaxDecimal = 0;
function uGetDecimalParam(url) {
    if (typeof (url) == "undefined")
        url = '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp';
    //獲取稅率
    $.ajax({
        type: 'POST',
        url: url,
        data: 'mode=method&method=getParam&parameters=SALETAXRATE|OPO',
        cache: false,
        async: false,
        success: function (data) {
            uTaxRate = data;
            window.sessionStorage.setItem("paraTaxRate", data);
        }
    });

    //獲取小數位參數
    $.ajax({
        type: 'POST',
        url: url,
        data: 'mode=method&method=getReportParam&parameters=',
        cache: false,
        async: false,
        success: function (data) {
            var rows = $.parseJSON(data);
            for (var i = 0; i < rows.length; i++) {
                switch (rows[i].PARAFIELD) {
                    case "PRICEDECIMAL": uPriceDecimal = rows[i].NAME_CN; window.sessionStorage.setItem("paraPriceDecimal", rows[i].NAME_CN); break;
                    case "QTYDECIMAL": uQtyDecimal = rows[i].NAME_CN; window.sessionStorage.setItem("paraQtyDecimal", rows[i].NAME_CN); break;
                    case "SUBDECIMAL": uSubDecimal = rows[i].NAME_CN; window.sessionStorage.setItem("paraSubDecimal", rows[i].NAME_CN); break;
                    case "SUMDECIMAL": uSumDecimal = rows[i].NAME_CN; window.sessionStorage.setItem("paraSumDecimal", rows[i].NAME_CN); break;
                    case "TAXDECIMAL": uTaxDecimal = rows[i].NAME_CN; window.sessionStorage.setItem("paraTaxDecimal", rows[i].NAME_CN); break;
                }
            }
        }
    });
}

/** @description: 讀取Storage中的Decimal設置 */
function uGetDecimalFromStorage() {
    uTaxRate = window.sessionStorage.getItem("paraTaxRate");
    uPriceDecimal = window.sessionStorage.getItem("paraPriceDecimal");
    uQtyDecimal = window.sessionStorage.getItem("paraQtyDecimal");
    uSubDecimal = window.sessionStorage.getItem("paraSubDecimal");
    uSumDecimal = window.sessionStorage.getItem("paraSumDecimal");
    uTaxDecimal = window.sessionStorage.getItem("paraTaxDecimal");
}

/** @description: 設置小數位，在JQDataGrid中對應的欄位的屬性FormatScript添加以下方法 */
function uSetQtyDecimal(value) {
    return Number(value).toFixed(uQtyDecimal);//數量
}
function uSetPriceDecimal(value) {
    return Number(value).toFixed(uPriceDecimal);//單價
}
function uSetTaxDecimal(value) {
    return Number(value).toFixed(uTaxDecimal);//含稅單價，含稅金額
}
function uSetSubDecimal(value) {
    return Number(value).toFixed(uSubDecimal);//金額
}

/** @description: 修改明細檔數量、單價時，計算明細檔金額，含稅單價，含稅金額；計算主檔數量合計，未稅合計，營業稅，總計。 
 * @param {string} detailGridID : 明細檔對應dataGridID
 * @param {string} dQtyName : 明細檔數量欄位
 * @param {string} dPriceName : 明細檔單價欄位
 * @param {string} dSubName : 明細檔金額欄位
 * @param {string} dTaxPName : 明細檔含稅單價欄位
 * @param {string} dTaxAName : 明細檔含稅金額欄位
 * @param {string} masterForm : 主檔DataFormID 
 * @param {string} mQtyName : 主檔數量合計欄位
 * @param {string} mTotalName : 主檔未稅合計欄位
 * @param {string} mSumName : 主檔總計欄位
 * @param {string} mTaxName : 主檔營業稅欄位
* @Sample : 
    uCalculateAmount("dataGridDetail", "QUANTITY", "PRICE", "SUBAMOUNT", "TAXPRICE", "TAXAMOUNT", "dataFormMaster1", "SUMQUALITY", "TOTALAMOUNT", "SUMAMOUNT", "TAXAMOUNT");
    uCalculateAmount("dataGridDetail", "QUANTITY", "PRICE", "SUBAMOUNT", "", "", "dataFormMaster1", "SUMQUALITY", "TOTALAMOUNT", "SUMAMOUNT", ""); */
function uCalculateAmount(detailGridID, dQtyName, dPriceName, dSubName, dTaxPName, dTaxAName, masterForm, mQtyName, mTotalName, mSumName, mTaxName) {
    var detailGrid = $('#' + detailGridID);
    var selectedRow = detailGrid.datagrid("getSelected");
    if (selectedRow) {
        var rowIndex = detailGrid.datagrid("getRowIndex", selectedRow);
        var quantity = uGetDetailValue(detailGrid, rowIndex, dQtyName);
        var price = uGetDetailValue(detailGrid, rowIndex, dPriceName);
        var subAmount = uAccCalc(quantity, price, "*", uSubDecimal);
        var taxPrice = uAccCalc(price, uTaxRate, "*", uTaxDecimal);
        var taxAmount = uAccCalc(taxPrice, quantity, "*", uTaxDecimal);
        uSetDetailValue(detailGrid, rowIndex, dSubName, subAmount);
        if (dTaxPName.length > 0)
            uSetDetailValue(detailGrid, rowIndex, dTaxPName, taxPrice);
        if (dTaxAName.length > 0)
            uSetDetailValue(detailGrid, rowIndex, dTaxAName, taxAmount);

        var rows = detailGrid.datagrid('getRows');
        var sumQuantity = 0, totalAmount = 0, sumAmount = 0;
        for (var i = 0; i < rows.length; i++) {
            if (i == rowIndex) {
                sumQuantity = uAccCalc(sumQuantity, quantity, "+", uQtyDecimal);
                totalAmount = uAccCalc(totalAmount, subAmount, "+", uSumDecimal);
                sumAmount = uAccCalc(sumAmount, taxAmount, "+", uSumDecimal);
            }
            else {
                //sumQuantity = uAccCalc(sumQuantity, rows[i].QUANTITY, "+", uQtyDecimal);
                //totalAmount = uAccCalc(totalAmount, rows[i].SUBAMOUNT, "+", uSumDecimal);
                //sumAmount = uAccCalc(sumAmount, rows[i].TAXAMOUNT, "+", uSumDecimal);
                if (dQtyName.length > 0)
                    sumQuantity = uAccCalc(sumQuantity, eval("rows[i]." + dQtyName), "+", uQtyDecimal);
                if (dSubName.length > 0)
                    totalAmount = uAccCalc(totalAmount, eval("rows[i]." + dSubName), "+", uSumDecimal);
                if (dTaxAName.length > 0)
                    sumAmount = uAccCalc(sumAmount, eval("rows[i]." + dTaxAName), "+", uSumDecimal);
            }
        }

        $('#' + masterForm + mQtyName).val(sumQuantity);//數量合計
        $('#' + masterForm + mTotalName).val(totalAmount);//未稅合計
        $('#' + masterForm + mSumName).val(sumAmount);//總計 
        $('#' + masterForm + mTaxName).val(uAccCalc(sumAmount, totalAmount, "-", uTaxDecimal));//營業稅 
    }
}


/** @description: 修改明細檔數量、單價、金額時，計算明細檔相關欄位；計算主檔數量合計，未稅合計，營業稅，總計。 
 * @param {string} detailGridID : 明細檔對應dataGridID
 * @param {string} masterForm : 主檔DataFormID
 * @param {string} masterTotalColumn : 主檔需要合計的欄位
 * @param {string} editColumn : 觸發編輯的欄位
 * @param {string} formula1 : 計算公式1
 * @param {string} formula2 : 計算公式2
 * @Sample :
            uCalculateDetail("dataGridDetail", "dataFormMaster1", "SUMQUALITY;TOTALAMOUNT", "QUANTITY", "QUANTITY*PRICE=SUBAMOUNT", "");
            uCalculateDetail("dataGridDetail", "dataFormMaster1", "SUMQUALITY;TOTALAMOUNT;SUMAMOUNT;TAXAMOUNT", "PRICE", "QUANTITY*PRICE=SUBAMOUNT", "QUANTITY*TAXPRICE=TAXAMOUNT");*/
function uCalculateDetail(detailGridID, masterForm, masterTotalColumn, editColumn, formula1, formula2) {
    var detailGrid = $('#' + detailGridID);
    var selectedRow = detailGrid.datagrid("getSelected");
    //formula1：數量*單價=金額
    //"QUANTITY*PRICE=SUBAMOUNT"
    var formula1Arr = formula1.split("=");
    var formula1Arr2 = formula1Arr[0].split("*");
    var dQuantity = formula1Arr2[0].trim();
    var dPrice = formula1Arr2[1].trim();
    var dSubAmount = formula1Arr[1].trim();

    if (selectedRow) {
        var rowIndex = detailGrid.datagrid("getRowIndex", selectedRow);
        var quantity = 0, price = 0, subAmount = 0, taxPrice = 0, taxAmount = 0;
        if (typeof (formula2) == "undefined" || formula2.length == 0) {
            if (editColumn == dQuantity || editColumn == dPrice) {
                quantity = uGetDetailValue(detailGrid, rowIndex, dQuantity);
                price = uGetDetailValue(detailGrid, rowIndex, dPrice);
                subAmount = uAccCalc(quantity, price, "*", uSubDecimal);
                uSetDetailValue(detailGrid, rowIndex, dSubAmount, subAmount);
            }
            else if (editColumn == dSubAmount) {
                quantity = uGetDetailValue(detailGrid, rowIndex, dQuantity);
                subAmount = uGetDetailValue(detailGrid, rowIndex, dSubAmount);
                price = uAccCalc(subAmount, quantity, "/", uSubDecimal);
                uSetDetailValue(detailGrid, rowIndex, dPrice, price);
            }
        }
        else {
            //formula2：數量*含稅單價=含稅金額
            //"QUANTITY*TAXPRICE=TAXAMOUNT"
            var formula2Arr = formula2.split("=");
            var formula2Arr2 = formula2Arr[0].split("*");
            var dTaxQuantity = formula2Arr2[0].trim();
            var dTaxPrice = formula2Arr2[1].trim();
            var dTaxAmount = formula2Arr[1].trim();

            if (editColumn == dQuantity || editColumn == dPrice) {
                quantity = uGetDetailValue(detailGrid, rowIndex, dQuantity);
                price = uGetDetailValue(detailGrid, rowIndex, dPrice);
                subAmount = uAccCalc(quantity, price, "*", uSubDecimal);
                taxPrice = uAccCalc(price, uTaxRate, "*", uTaxDecimal);
                taxAmount = uAccCalc(taxPrice, quantity, "*", uTaxDecimal);
                uSetDetailValue(detailGrid, rowIndex, dSubAmount, subAmount);
                uSetDetailValue(detailGrid, rowIndex, dTaxPrice, taxPrice);
                uSetDetailValue(detailGrid, rowIndex, dTaxAmount, taxAmount);
            }
            else if (editColumn == dSubAmount) {
                quantity = uGetDetailValue(detailGrid, rowIndex, dQuantity);
                subAmount = uGetDetailValue(detailGrid, rowIndex, dSubAmount);
                price = uAccCalc(subAmount, quantity, "/", uSubDecimal);
                taxPrice = uAccCalc(price, uTaxRate, "*", uTaxDecimal);
                taxAmount = uAccCalc(taxPrice, quantity, "*", uTaxDecimal);
                uSetDetailValue(detailGrid, rowIndex, dPrice, price);
                uSetDetailValue(detailGrid, rowIndex, dTaxPrice, taxPrice);
                uSetDetailValue(detailGrid, rowIndex, dTaxAmount, taxAmount);
            }
            else if (editColumn == dTaxPrice) {
                quantity = uGetDetailValue(detailGrid, rowIndex, dQuantity);
                taxPrice = uGetDetailValue(detailGrid, rowIndex, dTaxPrice);
                taxAmount = uAccCalc(taxPrice, quantity, "*", uTaxDecimal);
                price = uAccCalc(taxPrice, uTaxRate, "/", uSubDecimal);
                subAmount = uAccCalc(quantity, price, "*", uSubDecimal);
                uSetDetailValue(detailGrid, rowIndex, dTaxAmount, taxAmount);
                uSetDetailValue(detailGrid, rowIndex, dPrice, price);
                uSetDetailValue(detailGrid, rowIndex, dSubAmount, subAmount);
            }
            else if (editColumn == dTaxAmount) {
                quantity = uGetDetailValue(detailGrid, rowIndex, dQuantity);
                taxAmount = uGetDetailValue(detailGrid, rowIndex, dTaxAmount);
                taxPrice = uAccCalc(taxAmount, quantity, "/", uTaxDecimal);
                price = uAccCalc(taxPrice, uTaxRate, "/", uSubDecimal);
                subAmount = uAccCalc(quantity, price, "*", uSubDecimal);
                uSetDetailValue(detailGrid, rowIndex, dTaxPrice, taxPrice);
                uSetDetailValue(detailGrid, rowIndex, dPrice, price);
                uSetDetailValue(detailGrid, rowIndex, dSubAmount, subAmount);
            }
        }


        //Master加總
        var rows = detailGrid.datagrid('getRows');
        var sumQuantity = 0, totalAmount = 0, sumAmount = 0;
        for (var i = 0; i < rows.length; i++) {
            if (i == rowIndex) {
                sumQuantity = uAccCalc(sumQuantity, quantity, "+", uQtyDecimal);
                totalAmount = uAccCalc(totalAmount, subAmount, "+", uSumDecimal);
                sumAmount = uAccCalc(sumAmount, taxAmount, "+", uSumDecimal);
            }
            else {
                sumQuantity = uAccCalc(sumQuantity, rows[i].QUANTITY, "+", uQtyDecimal);
                totalAmount = uAccCalc(totalAmount, rows[i].SUBAMOUNT, "+", uSumDecimal);
                sumAmount = uAccCalc(sumAmount, rows[i].TAXAMOUNT, "+", uSumDecimal);
            }
        }
        //masterTotalColumn："SUMQUALITY;TOTALAMOUNT;SUMAMOUNT;TAXAMOUNT" 
        var masterTotalColumnArr = masterTotalColumn.split(";");
        var mQtyName = masterTotalColumnArr[0].trim();
        var mTotalName = masterTotalColumnArr.length > 1 ? masterTotalColumnArr[1].trim() : "";
        var mSumName = masterTotalColumnArr.length > 2 ? masterTotalColumnArr[2].trim() : "";
        var mTaxName = masterTotalColumnArr.length > 3 ? masterTotalColumnArr[3].trim() : "";
        $('#' + masterForm + mQtyName).val(sumQuantity);//數量合計
        $('#' + masterForm + mTotalName).val(totalAmount);//未稅合計
        $('#' + masterForm + mSumName).val(sumAmount);//總計
        $('#' + masterForm + mTaxName).val(uAccCalc(sumAmount, totalAmount, "-", uTaxDecimal));//營業稅
    }
}

/** @description: 多選帶回時，執行SP（GEX_GETPRICE）帶出單價
 * @param {string} masterForm : 主檔的DataFormID
 * @param {string} custName : 客戶編號欄位
 * @param {string} cnyName : 幣別編號欄位
 * @param {string} invoiceName : 發票聯式欄位
 * @param {string} productID : 產品編號
 * @param {string} flag : 旗號
 * @param {string} detailGridID : detail的dataGridID
 * @param {string} mQtyName : 主檔數量合計欄位
 * @param {string} mTotalName : 主檔未稅合計欄位
 * @param {string} mSumName : 主檔總計欄位
 * @param {string} mTaxName : 主檔營業稅欄位
* @Sample : uBatchGetPrice("dataFormMaster1", "CUSTID", "CURRENCYID", "INVOICETYPE", dataRow.PRODUCTID, "21", "dataGridDetail", "SUMQUALITY", "TOTALAMOUNT", "SUMAMOUNT", "TAXAMOUNT"); */
function uBatchGetPrice(masterForm, custName, cnyName, invoiceName, productID, flag, detailGridID, mQtyName, mTotalName, mSumName, mTaxName) {
    //parameters 
    var custID = $('#' + masterForm + custName).length > 0 ? $('#' + masterForm + custName).refval('getValue') : "";
    var currencyID = $('#' + masterForm + cnyName).length > 0 ? $('#' + masterForm + cnyName).refval('getValue') : "";
    var invoiceType = $('#' + masterForm + invoiceName).length > 0 ? $('#' + masterForm + invoiceName).combobox('getValue') : "";
    var date = uGetToday();

    $.ajax({
        type: 'POST',
        url: '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp',
        data: 'mode=method&method=getPrice&parameters=' + flag + '|' + custID + '|' + currencyID + '|' + productID + '|||' + date + '|' + invoiceType + '|',
        cache: false,
        async: false,
        success: function (price) {
            var rows = $('#' + detailGridID).datagrid('getRows');
            if (rows.length > 0) {
                var rowIndex = rows.length - 1;
                var row = rows[rowIndex];
                row.PRICE = uAccCalc(price, 1, '*', uPriceDecimal);
                row.TAXPRICE = uAccCalc(price, uTaxRate, '*', uTaxDecimal);
                row.SUBAMOUNT = uAccCalc(price, row.QUANTITY, '*', uPriceDecimal);
                row.TAXAMOUNT = uAccCalc(row.SUBAMOUNT, uTaxRate, '*', uTaxDecimal);
                var sumQuantity = 0, totalAmount = 0, sumAmount = 0;
                if (typeof ($('#' + masterForm + mQtyName).val()) != "undefined")
                    sumQuantity = $('#' + masterForm + mQtyName).val().replace(/\,/g, "");
                if (typeof ($('#' + masterForm + mTotalName).val()) != "undefined")
                    totalAmount = $('#' + masterForm + mTotalName).val().replace(/\,/g, "");
                if (typeof ($('#' + masterForm + mSumName).val()) != "undefined")
                    sumAmount = $('#' + masterForm + mSumName).val().replace(/\,/g, "");
                sumQuantity = uAccCalc(sumQuantity, row.QUANTITY, "+", uQtyDecimal);
                totalAmount = uAccCalc(totalAmount, row.SUBAMOUNT, "+", uSumDecimal);
                sumAmount = uAccCalc(sumAmount, uAccCalc(row.SUBAMOUNT, uTaxRate, '*', uTaxDecimal), "+", uSumDecimal);

                if (typeof ($('#' + masterForm + mQtyName).val()) != "undefined")
                    $('#' + masterForm + mQtyName).val(sumQuantity);//數量合計
                $('#' + masterForm + mTotalName).val(totalAmount);//未稅合計
                $('#' + masterForm + mSumName).val(sumAmount);//總計
                $('#' + masterForm + mTaxName).val(uAccCalc(sumAmount, totalAmount, "-", uTaxDecimal));//營業稅 
                $('#' + detailGridID).datagrid('updateRow', { index: rowIndex, row: row });
            }
        }
    });
}


/** @description: 轉單帶回時，重新整理表頭的的值
 * @param {string} masterForm : 主檔的DataFormID
 * @param {string} detailGridID : detail的dataGridID
 * @param {string} mQtyName : 主檔數量合計欄位
 * @param {string} mTotalName : 主檔未稅合計欄位
 * @param {string} mSumName : 主檔總計欄位
 * @param {string} mTaxName : 主檔營業稅欄位
* @Sample : uTransferGetPrice("dataFormMaster1", "dataGridDetail", "SUMQUALITY", "TOTALAMOUNT", "SUMAMOUNT", "TAXAMOUNT"); */
function uTransferGetPrice(masterForm, detailGridID, mQtyName, mTotalName, mSumName, mTaxName) {
            var rows = $('#' + detailGridID).datagrid('getRows');
            if (rows.length > 0) {
                var rowIndex = rows.length - 1;
                var row = rows[rowIndex];
                row.TAXPRICE = uAccCalc(row.PRICE, uTaxRate, '*', uTaxDecimal);
                row.SUBAMOUNT = uAccCalc(row.PRICE, row.QUANTITY, '*', uPriceDecimal);
                row.TAXAMOUNT = uAccCalc(row.SUBAMOUNT, uTaxRate, '*', uTaxDecimal);
                var sumQuantity = 0, totalAmount = 0, sumAmount = 0;
                if (typeof ($('#' + masterForm + mQtyName).val()) != "undefined")
                    sumQuantity = $('#' + masterForm + mQtyName).val().replace(/\,/g, "");
                if (typeof ($('#' + masterForm + mTotalName).val()) != "undefined")
                    totalAmount = $('#' + masterForm + mTotalName).val().replace(/\,/g, "");
                if (typeof ($('#' + masterForm + mSumName).val()) != "undefined")
                    sumAmount = $('#' + masterForm + mSumName).val().replace(/\,/g, "");
                sumQuantity = uAccCalc(sumQuantity, row.QUANTITY, "+", uQtyDecimal);
                totalAmount = uAccCalc(totalAmount, row.SUBAMOUNT, "+", uSumDecimal);
                sumAmount = uAccCalc(sumAmount, uAccCalc(row.SUBAMOUNT, uTaxRate, '*', uTaxDecimal), "+", uSumDecimal);

                if (typeof ($('#' + masterForm + mQtyName).val()) != "undefined")
                    $('#' + masterForm + mQtyName).val(sumQuantity);//數量合計
                $('#' + masterForm + mTotalName).val(totalAmount);//未稅合計
                $('#' + masterForm + mSumName).val(sumAmount);//總計
                $('#' + masterForm + mTaxName).val(uAccCalc(sumAmount, totalAmount, "-", uTaxDecimal));//營業稅 
                $('#' + detailGridID).datagrid('updateRow', { index: rowIndex, row: row });
            }

}


/** @description: 明細檔選擇產品時，帶回單價，計算明細檔金額，含稅單價，含稅金額；計算主檔數量合計，未稅合計，營業稅，總計。  需要執行SP（GEX_GETPRICE）
 * @param {string} refRow : dataGrid中RefVal選擇的行
 * @param {string} masterForm : 主檔的DataFormID
 * @param {string} custName : 客戶編號欄位
 * @param {string} cnyName : 幣別編號欄位
 * @param {string} invoiceName : 發票聯式欄位 
 * @param {string} flag : 旗號
 * @param {string} detailGridID : detail的dataGridID
 * @param {string} dProdName : 產品名稱欄位
 * @param {string} dStruName : 產品規格欄位
 * @param {string} dQtyName : 明細檔數量欄位
 * @param {string} dUnitName : 明細檔單位欄位
 * @param {string} dPriceName : 明細檔單價欄位
 * @param {string} dSubName : 明細檔金額欄位
 * @param {string} dTaxPName : 明細檔含稅金額欄位
 * @param {string} mQtyName : 主檔數量合計欄位
 * @param {string} mTotalName : 主檔未稅合計欄位
 * @param {string} mSumName : 主檔總計欄位
 * @param {string} mTaxName : 主檔營業稅欄位
* @Sample : uGetPriceAfterSelectProd(refRow, "dataFormMaster1", "CUSTID", "CURRENCYID", "INVOICETYPE", "21", "dataGridDetail", "PRODCNAME", "PRODSTRUCTURE", "QUANTITY", "UNIT", "PRICE", "SUBAMOUNT", "TAXPRICE", "TAXAMOUNT", "SUMQUALITY", "TOTALAMOUNT", "SUMAMOUNT", "TAXAMOUNT"); */
function uGetPriceAfterSelectProd(refRow, masterForm, custName, cnyName, invoiceName, flag, detailGridID, dProdName, dStruName, dQtyName, dUnitName, dPriceName, dSubName, dTaxPName, dTaxAName, mQtyName, mTotalName, mSumName, mTaxName) {
    var detailGrid = $('#' + detailGridID);
    var row = detailGrid.datagrid('getSelected');
    //抓到被編輯的grid的rowindex
    var rowIndex = detailGrid.datagrid('getRowIndex', row);

    if (dProdName.length > 0)
        uSetDetailValue(detailGrid, rowIndex, dProdName, refRow.PRODCNAME);
    if (dStruName.length > 0)
        uSetDetailValue(detailGrid, rowIndex, dStruName, refRow.PRODSTRUCTURE);
    uSetDetailValue(detailGrid, rowIndex, dQtyName, 1);
    uSetDetailValue(detailGrid, rowIndex, dUnitName, refRow.CALCUNIT);

    //parameters
    var custID = $('#' + masterForm + custName).refval('getValue');
    var currencyID = $('#' + masterForm + cnyName).refval('getValue');
    var invoiceType = 0;
    if (typeof ($('#' + masterForm + invoiceName)[0]) != "undefined")
        invoiceType = $('#' + masterForm + invoiceName).combobox('getValue');
    var date = uGetToday();
    var productID = refRow.PRODUCTID;
    $.ajax({
        type: 'POST',
        url: '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp',
        data: 'mode=method&method=getPrice&parameters=' + flag + '|' + custID + '|' + currencyID + '|' + productID + '|||' + date + '|' + invoiceType + '|',
        cache: false,
        async: false,
        success: function (price) {
            if (dPriceName.length > 0)
                uSetDetailValue(detailGrid, rowIndex, dPriceName, price);
            if (dSubName.length > 0)
                uSetDetailValue(detailGrid, rowIndex, dSubName, price);
            if (dTaxPName.length > 0)
                uSetDetailValue(detailGrid, rowIndex, dTaxPName, uAccCalc(price, uTaxRate, "*", uTaxDecimal));
            if (dTaxAName.length > 0)
                uSetDetailValue(detailGrid, rowIndex, dTaxAName, uAccCalc(price, uTaxRate, "*", uTaxDecimal));
            if (mQtyName.length > 0)
                uCalculateAmount(detailGridID, dQtyName, dPriceName, dSubName, "", "", masterForm, mQtyName, mTotalName, mSumName, "");
            if (dTaxPName.length > 0 && dTaxAName.length > 0)
                uCalculateAmount(detailGridID, dQtyName, dPriceName, dSubName, dTaxPName, dTaxAName, masterForm, mQtyName, mTotalName, mSumName, mTaxName);
        }
    });
}

var showConfirm = true;
var closeWin = true;
var submitFlag = false;
/** @description: 離開前詢問
 * @param {string} dialogID : 主檔對應的JQDialogID
 * @param {string} masterForm : 主檔的DataFormID
 * @param {string} dataViewID : view對應的Grid
* @Sample : uCloseMasterForm(dialogID, "dataFormMaster"); */
function uCloseMasterForm(dialogID, masterForm, dataViewID) {
    if (dialogID != '#JQDialog1') $(dialogID).dialog('close');
    else {
        if (showConfirm) {
            showConfirm = false;
            var mode = getEditMode($('#' + masterForm));
            if (mode == "inserted" || mode == "updated") {
                closeWin = confirm("您輸入的資料還沒保存，是否離開？");
                if (closeWin) {
                    if (typeof (dataViewID) != "undefined")
                        $("#" + dataViewID).datagrid("reload");
                    jqDetailRows = [];
                    jqCopyStatus = "";
                    $(dialogID).dialog('close');
                }
            }
            else {
                $(dialogID).dialog('close');
            }
        }
        showConfirm = true;
    }
}

/** @description: Detail加載完后，將數量合計、總計加總到Master
 * @param {string} detailGridID : 明細檔對應的dataGridID
 * @param {string} masterForm : 主檔的DataFormID
 * @param {string} mQtyName : 主檔數量合計欄位
 * @param {string} mSumName : 主檔總計欄位 
* @Sample : uSumDetailAfterLoad("dataGridDetail", "dataFormMaster1", "SUMQUALITY", "SUMAMOUNT"); */
function uSumDetailAfterLoad(detailGridID, masterForm, mQtyName, mSumName) {
    var rows = $('#' + detailGridID).datagrid('getRows');
    var sumQuantity = 0, sumAmount = 0;
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].QUANTITY != null)
            sumQuantity = uAccCalc(sumQuantity, rows[i].QUANTITY, "+", uQtyDecimal);
        if (rows[i].TAXAMOUNT != null)
            sumAmount = uAccCalc(sumAmount, rows[i].TAXAMOUNT, "+", uSumDecimal);
    }
    $('#' + masterForm + mQtyName).val(sumQuantity);//數量合計 
    $('#' + masterForm + mSumName).val(sumAmount);//總計
}

/** @description: 開窗查詢
 * @param {string} dialogID : 需要打開的dialogID
 * @param {string} dataGridID : View對應的dataGridID(為空則直接打開)
 * @param {string} keyName : 主鍵欄位
 * @param {string} colName : view欄位名稱
 * @param {string} extraGridID : 多個dataGridID 
* @Sample : uOpenDialog("JQDialogFivinvfinal", "dataGridDetail", "PRODUCTID", "PRODUCTID");
                   uOpenDialog("JQDialogTab", "dgView", "CUSTID", "CUSTID", "dataGridVisitGrid;dataGridConnGrid;dataGridPetitorGrid"); */
function uOpenDialog(dialogID, dataGridID, keyName, colName, extraGridID) {
    var dataGrid = "#" + dialogID.replace("JQDialog", "dataGrid");
    var JQDialog = "#" + dialogID;
    var setstr = "";
    if (typeof (dataGridID) == "undefined") {
        openForm(JQDialog, null, "updated", "dialog");
        $(JQDialog).find(".infosysbutton-s").hide();
        $(JQDialog).find(".infosysbutton-c").hide();
        //$("#JQDialogView").dialog("open");
    }
    else {
        var rows = $('#' + dataGridID).datagrid('getSelected');
        var sRows = $('#' + dataGridID).datagrid('getSelections');
        if (rows == null) {
            if (dataGridID == "dataGridDetail")
                alert("請選擇一筆明細檔資料。");
            else
                alert("請選擇一筆資料。");
        }
        else {
            if (sRows.length > 1)
                alert("只能選擇一筆資料。");
            else {
                var keyNameArrs = keyName.split(";");
                var ColNameArrs = colName.split(";");
                for (var i = 0; i < keyNameArrs.length; i++) {
                    var value = eval("rows." + ColNameArrs[i]);
                    setstr += i > 0 ? " AND " : "";
                    setstr += keyNameArrs[i] + "='" + value + "'";
                }
            }
        }
        if (setstr != "") {
            $(dataGrid).datagrid('setWhere', setstr);
            $(dataGrid).datagrid('reload');
            if (typeof (extraGridID) != "undefined" && extraGridID.length > 0) {
                var extraGridArr = extraGridID.split(';');
                for (var i = 0; i < extraGridArr.length; i++) {
                    $("#" + extraGridArr[i]).datagrid('setWhere', setstr);
                    $("#" + extraGridArr[i]).datagrid('reload');
                }
            }
            $(JQDialog).find(".infosysbutton-s").hide();
            $(JQDialog).find(".infosysbutton-c").hide();
            //參數依次是dialog的名字，dataform開啟的row，dialog的狀態(viewed,inserted,updated)，dialog方式(dialog,switch,continue,expand)。
            openForm(JQDialog, rows, "viewed", "dialog");
        }
    }
}

/** @description: 打開複製界面
 * @param {string} dialogID : 需要打開的dialogID
 * @param {string} dataGridID : View對應的dataGridID
 * @param {string} detailGridID : 明細檔對應的dataGridID
 * @param {string} keyName : 主鍵欄位
 * @param {string} colName : view欄位名稱
 * @param {string} autoNumColName : autoNum的欄位
 * @param {string} identityColName : 自增值的欄位
* @Sample : uOpenCopyDialog("JQDialog1", "dgView", "dataGridDetail", "BILLNO", "BILLNO", "BILLNO", "UNIQUENO"); */
function uOpenCopyDialog(dialogID, dataGridID, detailGridID, keyName, colName, autoNumColName, identityColName) {
    jqCopyStatus = "copyDetail";
    var JQDialog = "#" + dialogID;
    var setstr = "";
    var rows = $('#' + dataGridID).datagrid('getSelected');

    var keyNameArrs = keyName.split(";");
    var ColNameArrs = colName.split(";");
    for (var i = 0; i < keyNameArrs.length; i++) {
        var value = eval("rows." + ColNameArrs[i]);
        setstr += i > 0 ? " AND " : "";
        setstr += keyNameArrs[i] + "='" + value + "'";
    }

    if (setstr != "") {
        dRows = $("#" + detailGridID).datagrid("getData");
        var dColumns = $("#" + detailGridID).datagrid("getColumnFields");
        for (var i = 0; i < dRows.rows.length; i++) {
            var newRow = new Object();
            for (var j = 0; j < dColumns.length; j++) {
                if (dColumns[j] == autoNumColName) newRow[dColumns[j]] = "autoNum";
                else if (dColumns[j] == identityColName) newRow[dColumns[j]] = -1 * (i + 1);
                else newRow[dColumns[j]] = eval("dRows.rows[i]." + dColumns[j]);
            }
            jqDetailRows.push(newRow);
        }

        detailLoadOnce = 0;
        //參數依次是dialog的名字，dataform開啟的row，dialog的狀態(viewed,inserted,updated)，dialog方式(dialog,switch,continue,expand)。
        openForm(JQDialog, rows, "inserted", "dialog");
    }
}

/** @description: 轉單，譬如【報價單轉出貨單】
 * @param {string} dialogID : 需要打開的dialogID
 * @param {string} dataFormID : 主檔的DataFormID
 * @param {string} keyName : 主鍵欄位
 * @param {string} colName : view欄位名稱
 * @param {string} dateRange : 日期範圍查詢
 * @param {string} msg : 提示信息
* @Sample : uOpenTransferForm("JQDialogTransferForm1", "dataFormMaster1", "B.CUSTID;B.CURRENCYID", "IDNO;CURRENCYID", "BILLDATE", "請先輸入客戶及幣別代號！"); */
function uOpenTransferForm(dialogID, dataFormID, keyName, colName, dateRange, msg) {
    var dataGrid = "#" + dialogID.replace("JQDialog", "dataGrid");
    var JQDialog = "#" + dialogID;
    var setstr = "";
    var noReload = false;
    var keyNameArrs = keyName.split(";");
    var colNameArrs = colName.split(";");
    for (var i = 0; i < keyNameArrs.length; i++) {
        var value = uGetQueryValue(dataFormID + colNameArrs[i]);
        if (value == "") {
            alert(msg);
            noReload = true;
            break;
        }
        else {
            setstr += i > 0 ? " AND " : "";
            setstr += keyNameArrs[i] + "='" + value + "'";

            if (dateRange != "") {
                setstr += " AND " + dateRange + ">='" + uGetMonthFirstDate() + "' AND " + dateRange + "<='" + uGetMonthLastDate() + "'";
            }
        }
    }

    if (!noReload) {
        $(dataGrid).datagrid('setWhere', setstr);
        $(dataGrid).datagrid('reload');
        //參數依次是dialog的名字，dataform開啟的row，dialog的狀態(viewed,inserted,updated)，dialog方式(dialog,switch,continue,expand)。
        openForm(JQDialog, null, "viewed", "dialog");
    }
}


/** @description: 複製時，將資料載入Detail
 * @param {string} detailGridID : 明細檔對應的dataGridID
* @Sample : uCopyDetail("dataGridDetail"); */
function uCopyDetail(detailGridID) {
    if (jqCopyStatus == "copyDetail") {
        jqCopyStatus = "copyApply";
        var grid = $("#" + detailGridID);
        for (var i = 0; i < jqDetailRows.length; i++) {
            grid.datagrid("insertRow", { index: i, row: jqDetailRows[i] });
        }
    }
};

/** @description: 複製之後，Apply時再將Detail資料載入
 * @param {string} detailGridID : 明細檔對應的dataGridID
* @Sample : uCopyApply("dataGridDetail"); */
function uCopyApply(detailGridID) {
    if (jqCopyStatus == "copyApply") {
        var grid = $("#" + detailGridID);
        while (grid.datagrid("getRows").length > 0) {
            grid.datagrid("getRows").pop();
        }
        var changeRows = grid.datagrid('getChanges');
        var keys = grid.attr("keyColumns").split(',');

        for (var i = 0; i < jqDetailRows.length; i++) {
            var haveRow = false;
            for (var j = 0; j < changeRows.length; j++) {
                var eq = true;
                for (var k = 0; k < keys.length; k++) {
                    if (changeRows[j][keys[k]] != jqDetailRows[i][keys[k]]) {
                        eq = false;
                        break;
                    }
                }
                if (eq) {
                    haveRow = true;
                    break;
                }
            }

            //grid.datagrid("insertRow", { index: i, row: jqDetailRows[i] }); 
            //grid.datagrid("updateRow", { index: i, row: jqDetailRows[i] });
            if (!haveRow) {
                row = jqDetailRows[i];
            }
            else {
                //修改的Detail，需要加密encodeURIComponent
                var row = {};
                for (var prop in jqDetailRows[i]) {
                    var value = jqDetailRows[i][prop];
                    if (value != undefined && typeof value == 'string') {
                        row[prop] = encodeURIComponent(value);
                    }
                    else {
                        row[prop] = value;
                    }
                }

            }
            grid.datagrid("appendRow", row);
        }
        jqCopyStatus = "";
        jqDetailRows = [];
    }
}

/** @description: 開新頁簽
 * @param {string} detailGridID : view對應的dataGridID
 * @param {string} keyName : 主鍵欄位
 * @param {string} title : 新開的Tab的標題
 * @param {string} solutionID : 對應的SolutionID
 * @param {string} pageID : 新開頁的ID
 * @param {string} dataFormID : 主檔的DataFormID
 * @param {string} mkeyName : 主檔主鍵
* @Sample : uOpenNewTab("dataGridDetail", "PRODUCTID", "交易歷史查詢", "gINV", "jqINVM01_1.aspx", "dataFormMaster1", "IDNO"); */
function uOpenNewTab(dataGridID, keyName, title, solutionID, pageID, dataFormID, mkeyName) {
    var keyValue = "";
    var keyNameArrs = keyName.split(";");
    var rows = $('#' + dataGridID).datagrid('getSelected');
    if (rows == null)
        alert("請選擇一筆資料。");
    else {
        if (keyName.indexOf(";") > 0) {
            for (var i = 0; i < keyNameArrs.length; i++) {
                var value = eval("rows." + keyNameArrs[i]);
                keyValue += i > 0 ? "&" : "";
                keyValue += keyNameArrs[i] + "=" + value;
            }
        }
        else {
            var value = eval("rows." + keyName);
            keyValue = keyName + "=" + value;
        }
        if (typeof (dataFormID) != "undefined") {
            var value = uGetQueryValue(dataFormID + mkeyName);
            keyValue += keyValue.length > 0 ? "&" : "";
            keyValue += mkeyName + "=" + value;
        }
        //parent.$('#tabsMain').tabs('close', title);
        var developer = $('#_DEVELOPERID').val();
        if (developer) {//cloud
            parent.addTab(title, "preview" + developer + "/SD_" + developer + "_" + pageID + "?" + encodeURIComponent(keyValue));
        }
        else {//local 
            parent.addTab(title, solutionID + "/" + pageID + "?" + encodeURIComponent(keyValue));
        }
    }
}


/** @description: 開新頁簽
 * @param {string} title : 新開的Tab的標題
 * @param {string} FUNCTIONNAME : 對應的SolutionID+新開頁的ID+主檔主鍵，例../gOPO/jqOPOM01.aspx?BILLNO=
 * @param {string} keyValue : 主檔值
* @Sample : uOpenNewTabByKeyValue("交易歷史查詢", "../gOPO/jqOPOM01.aspx?BILLNO=","Q2016120601"); */
function uOpenNewTabByKeyValue(title, FUNCTIONNAME, keyValue) {
    var solutionID = FUNCTIONNAME.split('/')[1];
    var pageID = FUNCTIONNAME.split('/')[2].split('?')[0];
    var keyName = FUNCTIONNAME.split('/')[2].split('?')[1];
    pageID = "jq" + pageID.substr(1);

    var developer = $('#_DEVELOPERID').val();
    if (developer) {//cloud
        parent.addTab(title, "preview" + developer + "/SD_" + developer + "_" + pageID + "?" + encodeURIComponent(keyName + keyValue));
    }
    else {//local 
        parent.addTab(title, solutionID + "/" + pageID + "?" + encodeURIComponent(keyName + keyValue));
    }
}

/** @description: 獲取RDLC
 * @param {string} parameters : 對應SYS_RPTDEFINE.FUNCTAG
 * @param {string} TemplateList : 報表列表
* @Sample : uGetRDLC("INVM01", "TemplateList"); */
function uGetRDLC(parameters, TemplateList) {
    $.ajax({
        type: 'POST',
        url: '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp',
        data: 'mode=method&method=getReportParam&parameters=' + parameters,
        cache: false,
        async: false,
        success: function (data) {
            var rows = $.parseJSON(data);
            $('#' + TemplateList).combobox('clear');
            var comboData = [];
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].PARAFIELD == "REPORTTYPE" || rows[i].PARAFIELD == "REPORTTYPE2") {
                    //$Edit 20160919 by alex：REPORTTYPE 對應 RDLC ，REPORTTYPE2 對應 grf
                    var reportType = rows[i].PARAFIELD == "REPORTTYPE" ? ".rdlc" : ".grf";
                    if (comboData.length == 0)
                        $('#' + TemplateList).combobox('setValue', rows[i].RDLCID + reportType);
                    var rdlcText = rows[i].RDLCNAME_TW;
                    var rdlcValue = rows[i].RDLCID + reportType;
                    comboData.push({ text: rdlcText, value: rdlcValue });
                }

                //直接列印PDF
                if (rows[i].PARAFIELD == "AUTOPRINT" && typeof ($('#cbPDFPrint')[0]) != "undefined") {
                    if (rows[i].NAME_TW == "Y")
                        $('#cbPDFPrint')[0].checked = true;
                    else
                        $('#cbPDFPrint')[0].checked = false;
                }
            }
            $('#' + TemplateList).combobox('loadData', comboData);

        }
    });
}

/** @description: 打開報表選擇界面，通過button呼叫 */
function uShowTempSelectDiv() {
    var JQDialogTempSelect = $("#JQDialogTempSelect");
    JQDialogTempSelect.find(".infosysbutton-s").hide();
    JQDialogTempSelect.find(".infosysbutton-c").hide();
    JQDialogTempSelect.dialog({
        buttons: [{
            text: '確認',
            handler: function () {
                openReport();
            }
        }, {
            text: '取消',
            handler: function () { JQDialogTempSelect.dialog('close'); }
        }]
    });
    openForm('#JQDialogTempSelect', null, "viewed", 'dialog');
}

/** @description: 打開報表
 * @param {string} dgViewID : view對應的dataGridID
 * @param {string} keyName : 主鍵欄位
 * @param {string} cbSingleChoice : 是否單獨列印
 * @param {string} TemplateList : 報表列表
 * @param {string} parameters : 對應SYS_RPTDEFINE.FUNCTAG
 * @param {string} remoteName : 後端InfoCommand
 * @param {string} tableName : 表名
 * @param {string} solution : 對應的SolutionID
 * @param {bool} useQueryParams : 是否用查詢條件，可為空
 * @param {bool} useSP : 使用SP，可為空
 * @param {string} assemblyName : 後端程式 
 * @param {string} methodName : 後端方法 
* @Sample : 表單範例jqOPOM01：uOpenRDLC("dgView", "OPOORDER1_M.BILLNO", "cbSingleChoice", "TemplateList", "OPOM01", "srOPO2.cmdOPOM01", "cmdOPOM01"); 
                  報表View範例jqOPOR01：uOpenRDLC("dataGridMaster", "", "", "TemplateList", "OPOR01", "srOPO.VIEW_RPT_OPOR01", "VIEW_RPT_OPOR01", "gOPO", true);
                  報表SP範例jqOPOR10：uOpenRDLC("dataGridMaster", "", "", "TemplateList", "OPOR10", "", "", "gOPO", false, true); */
function uOpenRDLC(dgViewID, keyName, cbSingleChoice, TemplateList, parameters, remoteName, tableName, solution, useQueryParams, useSP, assemblyName, methodName) {
    var rows = $("#" + dgViewID).datagrid('getSelections');
    var keyNameArr = keyName.split(';');
    var whereString = "";
    if (keyName.length > 0) {
        for (var i = 0; i < rows.length ; i++) {
            for (var j = 0; j < keyNameArr.length; j++) {
                var keyColumn = keyNameArr[j].indexOf('.') > 0 ? keyNameArr[j].split('.')[1] : keyNameArr[j];
                var keyValue = eval("rows[i]." + keyColumn);
                if (j == 0) whereString += "(";
                whereString += " " + keyNameArr[j] + " = '" + keyValue + "'";
                whereString += j == keyNameArr.length - 1 ? ")  OR " : " AND ";
            }
        }
        whereString = whereString.substr(0, whereString.length - 4);

        var singleChoice = false;
        if (typeof ($('#' + cbSingleChoice)[0]) != "undefined") singleChoice = $('#' + cbSingleChoice)[0].checked;
        if (singleChoice && $("#" + dgViewID).datagrid('getSelected') != null)
            whereString = " " + keyName + " = '" + eval("$('#' + dgViewID).datagrid('getSelected')." + keyColumn) + "'";
    }

    if (useQueryParams) {
        var queryParams = $("#" + dgViewID).datagrid('options').queryParams;
        if (queryParams.queryWord != "") {
            var queryWord = eval('(' + queryParams.queryWord + ')');
            if (queryWord != undefined && queryWord != null)
                whereString = queryWord.whereString;
        }
    }

    //直接列印PDF
    var PDFPrint = false;
    if (typeof ($('#cbPDFPrint')[0]) != "undefined")
        PDFPrint = $('#cbPDFPrint')[0].checked;

    //多国语言
    var lang = "zh-TW";
    var developer = $('#_DEVELOPERID').val();
    if (!developer) {
        if (typeof ($('#lbMultiLanguage')[0]) != "undefined")
            lang = $('#lbMultiLanguage')[0].innerHTML;
    }

    if (whereString.length == 0 && !useSP)
        alert('請選擇一筆資料');
    if (rows.length == 0) {
        alert('該報表無資料');
        $('#JQDialogTempSelect').dialog('close');
    }
    else {
        var selectRdlcName = $('#' + TemplateList).combo('getValue');

        //Edit 20160919 by alex：grf報表部分
        if (selectRdlcName.toLowerCase().indexOf(".grf") > 0) {
            var developer = $('#_DEVELOPERID').val();
            if (typeof (solution) == "undefined")
                solution = "gOPO";
            var ReportURL = "../" + solution + "/grf_report/" + selectRdlcName;
            var url = "../grf_report/DisplayReport.aspx?RemoteName=" + remoteName + "&TableName=" + tableName + "&WhereString=" + encodeURIComponent(whereString) + "&ReportURL=" + ReportURL;
            var height = $(window).height() - 20;
            var width = $(window).width() - 20;
            var dialog = $('<div/>')
                .dialog({
                    draggable: false,
                    modal: true,
                    height: height,
                    width: width,
                    title: "Report"
                });
            $('<iframe style="border: 0px;" src="' + url + '" width="100%" height="95%"></iframe>').appendTo(dialog.find('.panel-body'));
            dialog.dialog('open');
        }
        else if (selectRdlcName.toLowerCase().indexOf(".rdlc") > 0) {
            //Edit 20160919 by alex：rdlc報表部分
            $.ajax({
                type: 'POST',
                url: '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp',
                data: 'mode=method&method=getReportParam&parameters=' + parameters,
                cache: false,
                async: false,
                success: function (data) {
                    var rows = $.parseJSON(data);
                    var options = {};
                    options["DataSetName"] = "ReportDS";
                    var developer = $('#_DEVELOPERID').val();
                    if (typeof (solution) == "undefined")
                        solution = "gOPO";
                    var rdlcName = "~/" + solution + "/RS_REPORT/" + selectRdlcName;
                    var xmlPath = "../" + solution + "/RS_REPORT/" + selectRdlcName;
                    options["Parameters"] = uGetParameter(rdlcName, xmlPath, developer, selectRdlcName, PDFPrint, rows, lang, whereString);

                    if (useSP) {
                        //使用SP時的參數
                        if (SPWhereText.length > 0) {
                            parameters["PARA_FILTER"] = uTurnWhereText(SPWhereText);
                        }
                        else
                            parameters["PARA_FILTER"] = SPWhereString;
                        options["SP"] = "Y";
                        options["SPParam"] = SPParam;
                        options["AssemblyName"] = assemblyName;
                        options["MethodName"] = methodName;
                        dgViewID = "#" + dgViewID;
                    }
                    exportReport(dgViewID, remoteName, tableName, rdlcName, whereString, options);
                }
            });
        }
    }
}


function uGetParameter(rdlcName, xmlPath, developer, selectRdlcName, PDFPrint, rows, lang, whereString) {
    var parameters = {};
    var notParamName = [];

    //cloud
    if (developer) {
        rdlcName = "~/RDLC/" + selectRdlcName;
        xmlPath = "../preview" + developer + "/RDLC/" + selectRdlcName;
    }
    else {
        parameters["PDFPrint"] = PDFPrint;
    }

    //讀取RDLC中參數  
    //var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
    //xmlDoc.async = "false";
    //xmlDoc.load(xmlPath);
    //root = xmlDoc.documentElement;
    //if (root != null) {
    //    for (var i = 0; i < root.childNodes.length; i++) {
    //        if (root.childNodes[i].baseName == "ReportParameters") {
    //            var paraNodes = root.childNodes[i].childNodes;
    //            for (var j = 0; j < paraNodes.length; j++) {
    //                parameters.push(paraNodes[j].childNodes[3].text);
    //                paramName.push(nodeParam);
    //            }
    //        }
    //    }
    //}
    //else 
    var solutionArr = ["ACTM", "BASM", "CRMM", "EIPM", "ESSM", "OPOM", "INVM"];
    var inSolution = false;
    for (var i = 0; i < solutionArr.length; i++) {
        if (selectRdlcName.indexOf(solutionArr[i]) >= 0) {
            inSolution = true;
            //INVM01等，需要清空uReportData
            window.sessionStorage.setItem("uReportData", "");
            break;
        }
    }

    var mList1 = ['INVM01', 'INVM02', 'INVM03', 'INVM04', 'INVM11', 'INVM12', 'OPOM01', 'OPOM02', 'OPOM12', 'OPOM12'];
    var mList2 = ['ACTM01'];
    var inList1 = false, inList2 = false;
    //RDLC參數，多了報錯，少了不顯示 
    if (inSolution) {
        notParamName = ["FILTER", "FILTER_CAP", "REPORTTYPE", "AUTOPRINT", "REPORTTYPE2"];
        for (var i = 0; i < mList1.length; i++) {
            if (selectRdlcName.indexOf(mList1[i]) >= 0) {
                inList1 = true; break;
            }
        }
        for (var i = 0; i < mList2.length; i++) {
            if (selectRdlcName.indexOf(mList2[i]) >= 0) {
                inList2 = true; break;
            }
        }
        if (inList1)
            notParamName = ["FILTER", "FILTER_CAP", "REPORTTYPE", "ECORP", "AUTOPRINT", "RATEDECIMAL", "REPORTTYPE2"];
        else if (inList2)
            notParamName = ["FILTER", "FILTER_CAP", "REPORTTYPE", "EADDR", "ECORP", "AUTOPRINT", "REPORTTYPE2"];
    }
    else {
        notParamName = ["ADDR", "EADDR", "FAX", "ECORP", "TEL", "ROWCOUNT", "AUTOPRINT", "REPORTTYPE2"];
    }

    for (var i = 0; i < rows.length; i++) {
        var notParam = false;
        for (var j = 0; j < notParamName.length; j++) {
            if (rows[i].PARAFIELD == notParamName[j]) {
                notParam = true; break;
            }
        }
        if (!notParam) {
            switch (lang) {
                case "zh-TW": parameters["PARA_" + rows[i].PARAFIELD] = rows[i].NAME_TW; break;
                case "zh-CN": parameters["PARA_" + rows[i].PARAFIELD] = rows[i].NAME_CN; break;
                case "en-us": parameters["PARA_" + rows[i].PARAFIELD] = rows[i].NAME_EN; break;
            }
        }
    }

    if (notParamName.indexOf("FILTER") < 0) {
        var whereArr = whereString.split('AND');
        var paraFilter = "";
        for (var i = 0; i < whereArr.length; i++) {
            if (whereArr[i].indexOf("''") < 0 && whereArr[i].indexOf("zzzz") < 0)
                paraFilter += whereArr[i] + " AND ";
        }
        parameters["PARA_FILTER"] = paraFilter.substr(0, paraFilter.length - 4);
        if (whereString.trim().length > 0) {
            parameters["PARA_FILTER"] = uTurnWhereText(whereString);
        }
    }

    //CRT用參數
    if (typeof (jqGexBasTmp) != "undefined") {
        var areaCount = 3, companyCount = 4;
        if (selectRdlcName.indexOf("_01") > 0) {
            var companyID = [1, 2, 3, 9];
            areaCount = 3, companyCount = 4;
        }
        if (selectRdlcName.indexOf("_02") > 0) {
            var companyID = [1, 2, 3, 4, 5, 6, 7, 8, 9];
            areaCount = 8, companyCount = 9;
        }

        var tempIndex = 1;
        //動態設置地區
        for (var i = 0; i < jqGexBasTmp.length; i++) {
            if (jqGexBasTmp[i].BASTABLE == "BASAREA") {
                if (tempIndex > areaCount) break;
                parameters["PARA_AREA" + tempIndex] = jqGexBasTmp[i].CNAME;
                tempIndex++;
            }
        }
        parameters["PARA_AREA" + (areaCount + 1)] = "合計";

        tempIndex = 1;
        //動態設置公司
        for (var i = 0; i < jqGexBasTmp.length; i++) {
            if (jqGexBasTmp[i].BASTABLE == "CRTCOMPANY") {
                for (var j = 0; j < companyID.length; j++) {
                    parameters["PARA_AMT" + companyID[j] + tempIndex] = jqGexBasTmp[i].CNAME;
                }
                tempIndex++;
                if (tempIndex > companyCount) break;
            }
        }

        for (var i = 1; i < companyCount; i++) {
            parameters["PARA_AMTS" + i] = "小計";
        }
        parameters["PARA_AMTS9"] = "總計"
    }

    return parameters;
}


/** @description: 將查詢條件轉成文字 */
function uTurnWhereText(SPWhereText) {
    var retText = "";
    var whereArr = SPWhereText.split(' AND ');

    for (var i = 0; i < whereArr.length; i++) {
        var column = whereArr[i].split(' ')[0];
        var value = whereArr[i].split(' ')[2].replace(/\'/g, "");
        var tempText = uGetLabelText(column);
        retText += tempText + value + " ";
    }

    return retText;
}

var uLastColumn = "";
/** @description: 獲取欄位Label的名稱 */
function uGetLabelText(column) {
    var columnName = "";
    if (column == "LOGINID") {
        columnName = "登錄人員：";
    }
    else {
        try {
            //SP用的普通Label
            if (typeof (eval("lb" + column)) != "undefined") {
                columnName = eval("lb" + column).innerText;
            }
        }
        catch (ex) { }

        if (columnName.length == 0) {
            var num = uLastColumn == column ? 2 : 1;

            try {
                //View用的普通Label
                if (typeof (eval("lb" + column + num)) != "undefined") {
                    columnName = eval("lb" + column + num).innerText;
                    uLastColumn = column;
                }
            }
            catch (ex) { }
        }

        if (columnName.length == 0) {
            try {
                //View用的開窗按鈕
                if (typeof (eval("btOpen" + column)) != "undefined") {
                    columnName = eval("btOpen" + column).text;
                    uLastColumn = column;
                }
            }
            catch (ex) { }
        }

        if (columnName.length == 0) {
            try {
                //SP用的開窗按鈕
                column = column.substr(0, column.length - 1);
                if (typeof (eval("btOpen" + column)) != "undefined") {
                    columnName = eval("btOpen" + column).text;
                }
            }
            catch (ex) { }
        }
    }

    return columnName;
}


var uRDLCParam;
var uParamInRDLC;
function uGetRDLCParam(parameters, solution, selectRdlcName) {
    $.ajax({
        type: 'POST',
        url: '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp',
        data: 'mode=method&method=getReportParam&parameters=' + parameters,
        cache: false,
        async: false,
        success: function (data) {
            var rows = $.parseJSON(data);
            //直接列印PDF
            var PDFPrint = false;
            var lang = "zh-TW";
            var developer = $('#_DEVELOPERID').val();
            var rdlcName = "~/" + solution + "/RS_REPORT/" + selectRdlcName;
            var xmlPath = "../" + solution + "/RS_REPORT/" + selectRdlcName;
            var retObj = uGetParameter(rdlcName, xmlPath, developer, selectRdlcName, PDFPrint, rows, lang, whereString);

            var parameters = [];
            for (i in retObj) parameters.push(i);

            uRDLCParam = parameters.sort();
        }
    });


    var developer = $('#_DEVELOPERID').val();
    if (typeof (solution) == "undefined")
        solution = "gOPO";
    var xmlPath = "../" + solution + "/RS_REPORT/" + selectRdlcName;

    //讀取RDLC中參數
    var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
    xmlDoc.async = "false";
    xmlDoc.load(xmlPath);
    root = xmlDoc.documentElement;
    var parameters = [];
    if (root != null) {
        for (var i = 0; i < root.childNodes.length; i++) {
            if (root.childNodes[i].baseName == "ReportParameters") {
                var paraNodes = root.childNodes[i].childNodes;
                for (var j = 0; j < paraNodes.length; j++) {
                    var paraValue = paraNodes[j].childNodes[3].text;
                    if (paraValue == "true") paraValue = paraNodes[j].childNodes[4].text;
                    parameters.push(paraValue);
                }
            }
        }

        uParamInRDLC = parameters.sort();
    }
}

/** @description: 打開審核界面 */
function uOpenSignDiv() {
    $('#taSignMemo').val('');
    $("#JQDialogSign").find(".infosysbutton-s").hide();
    $("#JQDialogSign").find(".infosysbutton-c").hide();
    $('#JQDialogSign').dialog({
        buttons: [{ text: '確認', handler: function () { sign(); } },
            { text: '取消', handler: function () { $('#JQDialogSign').dialog('close'); } }]
    });
    openForm('#JQDialogSign', null, "viewed", 'dialog');
}

/** @description: 審核
 * @param {string} dgViewID : view對應的dataGridID
 * @param {string} flag : 旗號 
 * @param {string} tableName : 表名
 * @param {string} keyColumn : 主鍵欄位 
* @Sample : 範例jqOPOM02：uDoSign("dgView", "22", "OPOORDER2_M"); 
                   範例jqCRMM12：uDoSign("dgView", "", "CRMCUSTOMER", "CUSTID"); */
function uDoSign(dgViewID, flag, tableName, keyColumn) {
    var rows = $("#" + dgViewID).datagrid('getSelections');
    var keyValue = "";
    for (var i = 0; i < rows.length ; i++) {
        if (typeof (keyColumn) == "undefined")
            keyColumn = "BILLNO";
        keyValue += eval("rows[i]." + keyColumn) + ",";
    }
    keyValue = keyValue.substr(0, keyValue.length - 1);

    var userID = uGetUserID();

    var signStatus = $('#optionSignStatus').options('getValue')
    var signMemo = $('#taSignMemo').val();
    if (signMemo.length == 0)
        alert("請輸入審核意見！");
    else {
        $.ajax({
            type: 'POST',
            url: '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp',
            data: 'mode=method&method=DoneSign&parameters=' + flag + '|' + keyValue + '|' + userID + '|' + signStatus + '|' + signMemo + '|' + tableName + '|' + keyColumn,
            cache: false,
            async: false,
            success: function (data) {
                alert(data);
                $('#' + dgViewID).datagrid('reload');
            }
        });

        $('#JQDialogSign').dialog('close');
    }
}

/** @description: 客戶產品多選時，檢查未轉數量 */
var tempUniqueNo = [], tempQty = [];
function uCheckQty(dataGridID1, dataGridID2, qtyName) {
    var sourceGrid = $('#' + dataGridID1);
    var destGrid = $('#' + dataGridID2);
    var rows = sourceGrid.datagrid('getRows');
    var selectedRow = destGrid.datagrid("getSelected");
    if (selectedRow) {
        var rowIndex = destGrid.datagrid("getRowIndex", selectedRow);
        var quantity = uGetDetailValue(destGrid, rowIndex, qtyName);
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].UNIQUENO == selectedRow.UNIQUENO) {
                var qty = eval("rows[i]." + qtyName);
                var index = tempUniqueNo.indexOf(selectedRow.UNIQUENO);
                if (index < 0) {
                    tempUniqueNo.push(rows[i].UNIQUENO);
                    tempQty.push(rows[i].QUANTITY);
                }
                if (index >= 0) {
                    if (Number(tempQty[index]) < Number(quantity)) {
                        alert("未轉數量不得超過" + tempQty[index]);
                    }
                }
                else if (Number(qty) < Number(quantity)) {
                    alert("未轉數量不得超過" + qty);
                }
                break;
            }
        }
    }
}

/** @description: 當新增重複時，提示並放棄保存
 * @param {string} dataFormID : 主檔的DataFormID
 * @param {string} dataGridID : view對應的dataGridID 
 * @param {string} colName : view欄位
 * @param {string} colCaption : view欄位名稱
 * @param {string} editor : 元件名稱 
* @Sample : 範例jqBASM607：uApplyCheckExist("dataFormAgentPerson", "dataGridAgentPerson", "AGENTID", "代理人", "refval");
                   範例jqESSM11，多鍵值：uApplyCheckExist("dataFormMaster1", "dataGridMaster1", "ESS_SUBJECTID;SALTERM", "薪資科目;多期數", "refval;combobox"); */
function uApplyCheckExist(dataFormID, dataGridID, colName, colCaption, editor) {
    var mode = getEditMode($('#' + dataFormID));
    if (mode == "inserted") {
        var colValue = "";
        var rows = $('#' + dataGridID).datagrid('getRows');
        var colNameArr = colName.split(";");
        var colCaptionArr = colCaption.split(";");
        var editorArr = editor.split(";");
        var dataExist = false;
        var msg = "";
        for (var i = 0; i < rows.length; i++) {
            for (var j = 0; j < colNameArr.length; j++) {
                var element = $('#' + dataFormID + colNameArr[j]);
                switch (editorArr[j]) {
                    case "combobox": colValue = element.combobox('getValue'); break;
                    case "combogrid": colValue = element.combogrid('getValue'); break;
                    case "refval": colValue = element.refval('getValue'); break;
                    case "datebox": colValue = element.datebox('getValue'); break;
                    case "datetimebox": colValue = element.datetimebox('getValue'); break;
                    default: colValue = element.val(); break;
                }
                var value = eval("rows[i]." + colNameArr[j]);
                if (editorArr[j] == "datebox") value = value.replace("T00:00:00", "");
                if (value == colValue) {
                    dataExist = true;
                    msg += colCaptionArr[j] + ":" + colValue + ";";
                }
                else {
                    dataExist = false;
                    msg = "";
                    break;
                }
            }
            if (dataExist) break;
        }


        if (dataExist) {
            msg = msg.substr(0, msg.length - 1) + ' 的資料已存在。'
            alert(msg);
            return false;
        }
    }
}


/** @description: 修改、刪除前先判斷是否有權限
 * @param {string} parameters : 主檔的DataFormID
* @Sample : 範例jqINVM01：uSysEditCheck(parameters); */
function uSysEditCheck(parameters) {
    var cancelEvent = true;
    $.ajax({
        type: 'POST',
        url: '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp',
        data: 'mode=method&method=SysEditCheck_JS&parameters=' + parameters,
        cache: false,
        async: false,
        success: function (data) {
            var ret = data.split("|");
            var flag = String(ret[0]).toLowerCase();
            var retFields = ret[1];
            var retMsg = ret[2];
            if (flag == "false") {
                alert(retMsg);
                cancelEvent = false;
            }
        }
    });
    return cancelEvent;
}



//--------------------------------上傳附檔------------------------
/** @description: 上傳文檔數量變量 */
var fileCount = 1;

/** @description: 初始化上傳文檔，分號分開顯示，並且開窗，用於Master的OnLoadSuccess事件中
 * @param {string} dataFormID : 對應的DataFormID
 * @param {string} colName : 上傳欄位
 * @param {string} JQDialogID : 開窗DialogID
 * @param {string} dataGridID : 顯示文檔的Grid
 * @param {string} someFiles : 記錄多個文檔名的變量
 * @param {bool} isopen : 已開
 * @param {string} tableName : 表名
 * @param {string} keyColumn : 更新表對應的鍵值
 * @param {string} dataViewID : view對應的Grid
 * @param {string} uploadFormID : 上傳對應的FormID(固定為dfFileUpload)
 * @param {string} uploadColumn : 上傳時用的虛擬欄位(固定為FILEUPLOAD)
 * @param {string} keyFormID : key對應的FormID
* @Sample : 範例jqPMSM12：uFileUploadOpenDialog("dfMaster", "ISSUE_EXTDOC", "JQDialogFileUpload", "dgFileUpload", "someFiles", isopen, "PMS_ISSUELOG", "ISSUEID", "dgView", "dfFileUpload", "FILEUPLOAD", "dfMaster");
                   範例jqBASM607：uFileUploadOpenDialog("dataFormMaster1", "DATAPATH", "JQDialogFileUpload", "dgFileUpload", "someFiles", isopen2, "BASPERSON", "PERSONID", "dgView", "dfFileUpload", "FILEUPLOAD", "dfMaster"); */
function uFileUploadOpenDialog(dataFormID, colName, JQDialogID, dataGridID, someFiles, isopen, tableName, keyColumn, dataViewID, uploadFormID, uploadColumn, keyFormID) {
    var element = $('#' + dataFormID + colName);
    var fileUpload = $("#infoFileUpload" + dataFormID + colName);
    fileUpload[0].type = "button";
    fileUpload[0].value = "上傳";

    fileUpload.css("width", "65");
    var fileName = $('[name="' + colName + '"]').val();

    fileUpload.click(function () {
        var JQDialog = $('#' + JQDialogID);
        JQDialog.find(".infosysbutton-s").hide();
        JQDialog.find(".infosysbutton-c").hide();

        JQDialog.dialog({
            buttons: [{
                text: '確認', handler: function () {
                    uFileUploadToDB(keyFormID, dataFormID, colName, JQDialog, dataGridID, someFiles, isopen, tableName, keyColumn, dataViewID);
                    ////重置上傳控件
                    $('#' + uploadFormID + uploadColumn).next().remove();
                    initInfoFileUpload($('#' + uploadFormID + uploadColumn));
                    uFileUploadInitForGrid(uploadFormID, uploadColumn);
                }
            },
                {
                    text: '取消', handler: function () {
                        //重置上傳控件
                        $('#' + uploadFormID + uploadColumn).next().remove();
                        initInfoFileUpload($('#' + uploadFormID + uploadColumn));
                        uFileUploadInitForGrid(uploadFormID, uploadColumn);
                        JQDialog.dialog('close');
                    }
                }]
        });

        var dgFileUpload = $('#' + dataGridID);
        var newData = dgFileUpload.datagrid('getData');
        newData.rows = newData.rows.slice(0, 0);

        var temp = 0;
        try {
            while (eval('someFiles' + temp)) {
                var tempSomeFiles = eval('someFiles' + temp);
                var tempRow = new Object();
                if (tempSomeFiles.text) {
                    tempRow[uploadColumn] = tempSomeFiles.text;
                    newData.rows.push(tempRow);
                }
                temp++;
            }
        }
        catch (ex) { }
        dgFileUpload.datagrid('loadData', newData);

        uOpenDialog(JQDialogID);
    });

    isopen = uSetSomeHrefs(dataFormID, colName, fileName, isopen, someFiles);
    return isopen;
}

/** @description: 上傳文檔，設置多個超鏈接，在uFileUploadOpenDialog，uFileUploadToDB中使用 */
function uSetSomeHrefs(dataFormID, colName, fileName, isopen, someFiles) {
    var element = $('#' + dataFormID + colName);
    var fileUpload = $("#infoFileUpload" + dataFormID + colName);
    if (fileName.indexOf(';') > 0) {
        var fileNameArr = fileName.split(';');
        fileCount = fileCount > fileNameArr.length ? fileCount : fileNameArr.length;
        //fileCount = fileNameArr.length;
        for (var i = 0; i < fileCount; i++) {
            var someFileID = someFiles + i;
            if (fileNameArr.length > i)
                uSetHref(fileUpload, fileNameArr[i], element, isopen, someFileID);
            else
                uSetHref(fileUpload, "", element, isopen, someFileID);
        }
        isopen = "Y";
    }
    else {
        //fileCount = fileName.length > 0 ? 1 : 0;
        for (var i = 0; i < fileCount; i++) {
            var someFileID = someFiles + i;
            if (i == 0)
                isopen = uSetHref(fileUpload, fileName, element, isopen, someFileID);
            else
                isopen = uSetHref(fileUpload, "", element, isopen, someFileID);
        }
    }
    return isopen;
}

/** @description: 上傳文檔，設置單個超鏈接，在uSetSomeHrefs中使用 */
function uSetHref(fileUpload, fileName, element, isopen, someFileID) {
    var developer = $('#_DEVELOPERID').val();
    var src = "";

    //$Edit 20160629 by alex：給上傳路徑添加DB
    var upLoadFolderDB = uAddDBToUpLoadFolder(getInfolightOption(element).upLoadFolder);

    if (developer) {//cloud
        src = "../preview" + developer + "/" + upLoadFolderDB + "/" + fileName;
    }
    else {//local
        src = "../" + upLoadFolderDB + "/" + fileName;
    }

    var fileType = src.substr(src.lastIndexOf('.') + 1);
    if (isopen == "N") {
        var href = "<a id='" + someFileID + "' target='_blank' width='200' href='" + src + "' >" + fileName + "<a> ";
        fileUpload.before(href);
        isopen = "Y";
    }

    if ($("#" + someFileID).length > 0) {
        $("#" + someFileID)[0].text = fileName;
    }
    else if (fileName.length > 0) {
        var href = "<a id='" + someFileID + "' target='_blank' width='200' href='" + src + "' >" + fileName + "<a> ";
        fileUpload.before(href);
        $("#" + someFileID)[0].text = fileName;
        isopen = "Y";
    }

    if (typeof ($("#" + someFileID)) != "undefined") {
        $("#" + someFileID).attr("href", src);
    }
    return isopen;
}

/** @description: 初始化上傳文檔控件，上傳后顯示在Grid里，用於Master的OnLoadSuccess事件中，跟在uFileUploadOpenDialog之後
 * @param {string} uploadFormID : 上傳對應的FormID(固定為dfFileUpload)
 * @param {string} uploadColumn : 上傳時用的虛擬欄位(固定為FILEUPLOAD)
* @Sample : uFileUploadInitForGrid("dfFileUpload", "FILEUPLOAD"); */
function uFileUploadInitForGrid(uploadFormID, uploadColumn) {
    var element = $('#' + uploadFormID + uploadColumn);
    var fileUpload = $("#infoFileUpload" + uploadFormID + uploadColumn);
    //對應可能反復觸發的事件，需要unbind()
    fileUpload.unbind().change(function () {
        //infoFileUploadMethod(element);
        uInfoFileUploadMethod(element);
    });
}

/** @description: Eidt 20160629 by alex：上傳路徑帶上DB，比如Files\genieERP\INVM01
 * @param {string} upLoadFolder1 : 上傳路徑
* @Sample : upLoadFolder1 = uAddDBToUpLoadFolder(upLoadFolder1); */
function uAddDBToUpLoadFolder(upLoadFolder1) {
    var database = uGetDataBase();
    var upLoadFolder1Arr = upLoadFolder1.split('/');
    var upLoadFolderTemp = "";
    for (var i = 0; i < upLoadFolder1Arr.length; i++) {
        if (i == 0)
            upLoadFolderTemp += upLoadFolder1Arr[i] + "/" + database;
        else
            upLoadFolderTemp += "/" + upLoadFolder1Arr[i];
    }
    return upLoadFolderTemp;
}

/** @description: 上傳完顯示在Grid中，用於infofileupload元件的OnSuccess事件中
 * @param {string} fileName : 上傳文檔名
 * @param {string} dataGridID : 上傳對應的GridID(固定為dgFileUpload)
 * @param {string} dataFormID : 上傳對應的FormID(固定為dfFileUpload)
 * @param {string} uploadColumn : 上傳時用的虛擬欄位(固定為FILEUPLOAD)
* @Sample : uFileUploadOnSuccessForGrid(fileName, "dgFileUpload", "dfFileUpload", "FILEUPLOAD"); */
function uFileUploadOnSuccessForGrid(fileName, dataGridID, dataFormID, uploadColumn) {
    var dgFileUpload = $('#' + dataGridID);
    var newRow = new Object();
    newRow[uploadColumn] = fileName;
    dgFileUpload.datagrid('appendRow', newRow);
    var fileUpload = $("#infoFileUpload" + dataFormID + uploadColumn);
    fileUpload.attr('disabled', false);
    fileUpload.attr('isalsoreadonly', true);
    uFileUploadInitForGrid(dataFormID, uploadColumn);
}

/** @description: 按確認后，上傳文檔Update到數據庫
 * @param {string} keyFormID : key對應的FormID
 * @param {string} dataFormID : 對應的FormID
 * @param {string} colName : 上傳欄位
 * @param {string} JQDialog : 開窗DialogID
 * @param {string} dataGridID : 顯示文檔的Grid
 * @param {string} someFiles : 記錄多個文檔名的變量
 * @param {bool} isopen : 已開
 * @param {string} tableName : 表名
 * @param {string} keyColumn : 更新表對應的鍵值
 * @param {string} dataViewID : -view對應的Grid 
 */
function uFileUploadToDB(keyFormID, dataFormID, colName, JQDialog, dataGridID, someFiles, isopen, tableName, keyColumn, dataViewID) {
    var rows = $('#' + dataGridID).datagrid('getRows');
    var fileUploadName = "";
    for (var i = 0; i < rows.length; i++) {
        fileUploadName += rows[i].FILEUPLOAD + ";";
    }
    if (fileUploadName.length > 0) {
        fileUploadName = fileUploadName.substr(0, fileUploadName.length - 1);
    }
    //var keyValue = eval("$('#" + dataViewID + "').datagrid('getSelected')." + keyColumn); 
    //var keyValue = $('#' + keyFormID + keyColumn).val();
    //keyValue使用表名+登錄人員ID
    var keyValue = tableName + uGetUserID();
    var url = "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp";

    $.ajax({
        type: "POST",
        url: url,
        data: "mode=method&method=fileUploadUpdate&parameters=" + tableName + "|" + colName + "|" + keyColumn + "|" + keyValue + "|" + fileUploadName,
        cache: false,
        async: false,
        success: function (data) {
            $('#' + dataViewID).datagrid('reload');
            //jqUploadNames前端程式記錄上傳文檔名稱
            if (typeof (jqUploadNames) != "undefined")
                jqUploadNames = fileUploadName;
            uSetSomeHrefs(dataFormID, colName, fileUploadName, isopen, someFiles);
        },
        error: function (data) {

        }
    });

    JQDialog.dialog('close');
}

/** @description: 刪除上傳文件，button觸發 */
function uFileUploadDelete() {
    var dgFileUpload = $('#dgFileUpload');
    var selectedIndex = getSelectedIndex(dgFileUpload);
    if (selectedIndex >= 0)
        dgFileUpload.datagrid('deleteRow', selectedIndex);
}
//--------------------------------上傳附檔------------------------



//--------------------------------上傳圖檔------------------------
/** @description: 初始化上傳元件，例如圖片等，用於Master的OnLoadSuccess事件中
 * @param {string} dataFormID : 對應dataFormID
 * @param {string} colName : 上傳欄位
 * @param {string} samplephotoID : 記錄文檔名的變量
 * @param {bool} isopen : 已開
 * @param {bool} showPicture : 是否顯示圖片
* @Sample : ufileUploadInit("dataFormMaster3", "PICPATH", "samplephoto", isopen1, true); */
function ufileUploadInit(dataFormID, colName, samplephotoID, isopen, showPicture) {
    var element = $('#' + dataFormID + colName);
    var fileUpload = $("#infoFileUpload" + dataFormID + colName);
    //改變上傳控制項寬度
    fileUpload.css("width", "65");

    //選擇圖片後就上傳
    fileUpload.change(function () {
        //infoFileUploadMethod(element);
        uInfoFileUploadMethod(element);
    });

    var fileName = $('[name="' + colName + '"]').val();
    var developer = $('#_DEVELOPERID').val();
    var src = "";

    //$Edit 20160629 by alex：給上傳路徑添加DB
    var upLoadFolderDB = uAddDBToUpLoadFolder(getInfolightOption(element).upLoadFolder);

    if (developer) {//cloud
        if (fileName == "") {
            if (showPicture) fileName = "sample.jpg";
        }
        else src = "../preview" + developer + "/" + upLoadFolderDB + "/" + fileName;
    }
    else {//local
        if (fileName == "") {
            if (showPicture) src = '../Image/erp/sample.jpg';
        }
        else src = "../" + upLoadFolderDB + "/" + fileName;
    }

    var fileType = src.substr(src.lastIndexOf('.') + 1);
    if (isopen == "N") {
        if (fileType == "bmp" || fileType == "png" || fileType == "jpg")
            fileUpload.before("<img id='" + samplephotoID + "' width='300' src='" + src + "' />");
        if (fileType == "txt" || fileType == "doc")
            fileUpload.before("<a id='" + samplephotoID + "'  width='200' href='" + src + "' >" + fileName + "<a>");

        isopen = "Y";
    }
    if ($("#" + samplephotoID).length > 0) {
        if (fileType == "") $("#" + samplephotoID)[0].text = "";
        else if (fileType == "txt" || fileType == "doc") $("#" + samplephotoID)[0].text = fileName;
    }

    //加載圖片
    if (typeof ($("#" + samplephotoID)) != "undefined") {
        switch (fileType) {
            case "bmp":
            case "png":
            case "jpg": $("#" + samplephotoID).attr("src", src); break;
            case "txt":
            case "doc":
            default: $("#" + samplephotoID).attr("href", src); break;
        }
    }
    return isopen;
}

/** @description: 上傳完顯示圖片，用於infofileupload元件的OnSuccess事件中
 * @param {string} fileName : 上傳文件名
 * @param {string} dataFormID : 對應dataFormID
 * @param {string} colName : 上傳欄位
 * @param {string} samplephotoID : 記錄文檔名的變量
 * @param {string} fileType : 上傳文檔類型
* @Sample : uFileUpload(fileName, "dataFormMaster3", "PICPATH", "samplephoto"); */
function uFileUpload(fileName, dataFormID, colName, samplephotoID, fileType) {
    var developer = $('#_DEVELOPERID').val();
    var src = "";

    //$Edit 20160629 by alex：給上傳路徑添加DB
    var upLoadFolderDB = uAddDBToUpLoadFolder(getInfolightOption($("#" + dataFormID + colName)).upLoadFolder);

    if (developer) {//cloud
        src = "../preview" + developer + "/" + upLoadFolderDB + "/" + fileName;
    }
    else {//local
        src = "../" + upLoadFolderDB + "/" + fileName;
    }
    var fileType = src.substr(src.lastIndexOf('.') + 1);
    if (fileType == "txt" || fileType == "doc" || fileType == "docx") {
        if ($("#" + samplephotoID).length > 0)
            $("#" + samplephotoID)[0].text = fileName;
        else {
            var fileUpload = $("#infoFileUpload" + dataFormID + colName);
            fileUpload.before("<a id='" + samplephotoID + "'  width='200' href='" + src + "' >" + fileName + "<a>");
        }
    }
    switch (fileType) {
        case "bmp":
        case "png":
        case "jpg": $("#" + samplephotoID).attr("src", src); break;
        case "txt":
        case "doc":
        default: $("#" + samplephotoID).attr("href", src); break;
    }
}
//--------------------------------上傳圖檔------------------------


//--------------------------------EIPM11，EIPM13等開窗選擇列表，并存檔------------------------
var tableSeries = [["BASDEPARTMENT", "DEPARTMENTID", "部門編號", "部門名稱", "部門列表", "已選部門"],
   ["GROUPS", "GROUPID", "群組編號", "群組名稱", "群組列表", "已選群組"],
   ["PMSPROJECT", "PROJECTID", "專案編號", "專案名稱", "專案列表", "已選專案"],
   ["BASPERSON", "PERSONID", "人員編號", "人員名稱", "人員列表", "已選人員"]];


/** @description: 開窗
* @Sample : uBtnOpenWin(1, "EIPBBS_D"); */
function uBtnOpenWin(recType, detailTableName) {
    var uniqueNo = $("#dataFormMasterUNIQUENO").val();

    var buttons = [{
        text: "存檔",
        handler: function () { uSaveDetail(uniqueNo, recType, detailTableName); }
    }, {
        text: "取消",
        handler: uCloseWindow
    }];
    var title = "", tableName = "";
    switch (recType) {
        case 1: title = "部門"; tableName = "BASDEPARTMENT"; break;
        case 2: title = "群組"; tableName = "GROUPS"; break;
        case 3: title = "專案"; tableName = "PMSPROJECT"; break;
        case 4: title = "人員"; tableName = "BASPERSON"; break;
        default: break;
    }

    var formUrl = "../InnerPages/AccessUser.aspx";
    uCreateAndOpenWizardDialog("winAccessUser", title, "450", "350", formUrl, function () { uLoadHandler(uniqueNo, recType, tableName, detailTableName); }, buttons);
}

/** @description: 保存，在uBtnOpenWin中使用該方法 */
function uSaveDetail(uniqueNo, recType, detailTableName) {
    var IDNO = "", IDNAME = "";
    var rows = $("#gridUsersTo_AccessUser").datagrid("getData").rows;
    for (var i = 0; i < rows.length; i++) {
        IDNO += rows[i].IDNO + ";";
        IDNAME += rows[i].IDNAME + ";";
    }
    var url = "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp";

    $.ajax({
        type: "POST",
        url: url,
        data: "mode=method&method=insertDetailData&parameters=" + uniqueNo + "|" + recType + "|" + IDNO + "|" + IDNAME + "|" + detailTableName,
        cache: false,
        async: false,
        success: function (data) {
            var uniqueNo = $("#dataFormMasterUNIQUENO").val();
            $('#winAccessUser').dialog().dialog('close');
            if (uniqueNo > 0) {
                $('#dataGridDetail').datagrid('reload');
            } else {
                var retUnique = data;
                newDetailUnique += retUnique + ",";
                var detailUniqueNo = newDetailUnique.substr(0, newDetailUnique.length - 1);
                var setstr = " UNIQUENO IN (" + detailUniqueNo + ")";
                $("#dataGridDetail").datagrid('setWhere', setstr);
                $("#dataGridDetail").datagrid('reload');
            }
        },
        error: function (data) {

        }
    });
}

/** @description: 新增Master時，由於Master還沒有存檔，所以先新增臨時UNIQUENO，之後Master保存時再UPDATE
* @Sample : uAfterSaveDetail("EIPBBS_M", "EIPBBS_D"); */
function uAfterSaveDetail(masterTableName, detailTableName) {
    var detailUniqueNo = newDetailUnique.substr(0, newDetailUnique.length - 1);
    var url = "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp";

    $.ajax({
        type: "POST",
        url: url,
        data: "mode=method&method=afterInsertDetailData&parameters=" + masterTableName + "|" + detailTableName + "|" + detailUniqueNo,
        cache: false,
        async: false,
        success: function (data) {
            var ret = data;
        },
        error: function (data) {
            var ret = data;
        }
    });
}

/** @description: 關閉窗口，在uBtnOpenWin中使用該方法 */
function uCloseWindow() {
    $('#winAccessUser').dialog({
        onClose: function () {
        }
    }).dialog('close');
}

/** @description: 加載數據，在uBtnOpenWin中使用該方法 */
function uLoadHandler(uniqueNo, recType, tableName, detailTableName) {
    var title1 = "", title2 = "";
    for (var i = 0; i < tableSeries.length; i++) {
        if (tableName == tableSeries[i][0]) {
            IDNOCaption = tableSeries[i][2];
            IDNAMECaption = tableSeries[i][3];
            title1 = tableSeries[i][4];
            title2 = tableSeries[i][5];
            break;
        }
    }

    $('#gridUsersFrom_AccessUser').datagrid({
        columns: [[
                    { field: 'IDNO', title: IDNOCaption, sortable: true, width: 80 },
                    { field: 'IDNAME', title: IDNAMECaption, sortable: true, width: 87 }
        ]],
        fitColumns: false
    });
    $('#gridUsersTo_AccessUser').datagrid({
        columns: [[
                    { field: 'IDNO', title: IDNOCaption, sortable: true, width: 80 },
                    { field: 'IDNAME', title: IDNAMECaption, sortable: true, width: 87 }
        ]],
        fitColumns: false
    });


    var selectedData = [];
    var url = "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp";

    uLoadDataForDataGrid("gridUsersTo_AccessUser", url, function (data) {
        selectedData = [];
        for (var i = 0 ; i < data.length; i++) {
            selectedData.push(data[i]);
        }
        return selectedData;
    }, title2, "mode=method&method=getMultiData&parameters=" + detailTableName + "|" + uniqueNo + "|" + recType);

    uLoadDataForDataGrid("gridUsersFrom_AccessUser", url, undefined, title1,
        "mode=method&method=getMultiData&parameters=" + tableName + "||",
        function (data) {
            var newData = [];
            for (var i = 0 ; i < data.length; i++) {
                var isExisted = -1;
                for (var j = 0 ; j < selectedData.length; j++) {
                    if (data[i].IDNO == selectedData[j].IDNO) {
                        isExisted = j;
                        break;
                    }
                }
                if (isExisted != -1) {
                    selectedData.splice(isExisted, 1);
                }
                else {
                    newData.push(data[i]);
                }
            }
            return newData;
        }
    );
}

/** @description: 左側的選中的選項移動到右側，button觸發，頁面為InnerPages/AccessUser.aspx */
function btnUsersTo_AccessUserClick() {
    uFromTo("gridUsersFrom_AccessUser", "gridUsersTo_AccessUser");
}

/** @description: 右側的選中的選項返回到左側，button觸發，頁面為InnerPages/AccessUser.aspx */
function btnUsersBack_AccessUser() {
    uFromTo("gridUsersTo_AccessUser", "gridUsersFrom_AccessUser");
}

/** @description: 移動選中的選項，在btnUsersTo_AccessUserClick，btnUsersBack_AccessUser，uLoadMultiHandler中使用 */
function uFromTo(fromDataGrid, toDataGrid) {
    var to_Data = $('#' + toDataGrid).datagrid('getData');
    var from_Data = $('#' + fromDataGrid).datagrid('getData');
    var from_Selected = $('#' + fromDataGrid).datagrid('getSelected');
    var from_SelectedIndex = $('#' + fromDataGrid).datagrid('getRowIndex', from_Selected);
    if (from_SelectedIndex > -1) {
        to_Data.rows.push(from_Data.rows[from_SelectedIndex]);
        from_Data.rows.splice(from_SelectedIndex, 1);
    }
    //排序
    to_Data.rows.sort(uSort);
    from_Data.rows.sort(uSort);

    $('#' + toDataGrid).datagrid('loadData', to_Data.rows);
    $('#' + fromDataGrid).datagrid('loadData', from_Data.rows);
}

/** @description: 排序的規則，在uFromTo中使用 */
function uSort(a, b) {
    var aID = a.IDNO, bID = b.IDNO;
    return aID > bID ? 1 : (aID == bID ? 0 : -1);
}

/** @description: 開窗操作，在uBtnOpenWin，uOpenMultiWin中使用 */
function uCreateAndOpenWizardDialog(winId, title, width, height, formUrl, onLoadHandler, buttons) {
    if ($('#' + winId).length == 0) {
        //若沒有DIV，新增一個
        $('<div id="' + winId + '">').appendTo(document.body);
    }
    $('#' + winId).dialog({
        title: title,
        width: width,
        height: height,
        modal: true,
        draggable: true,
        closed: false,
        maximized: false,
        minimizable: false,
        collapsible: false,
        inline: false,
        zIndex: 9001,
        onLoad: function () {
            $('#' + winId).dialog('open');
            if (onLoadHandler != undefined) {
                onLoadHandler();
            }
        },
        onLoadError: function (err) {
            var v = 0;
        },
        buttons: buttons
    });
    $('#' + winId).dialog('refresh', formUrl);
}

/** @description: 加載數據，在uLoadMultiHandler，uLoadHandler中使用*/
function uLoadDataForDataGrid(dg, u, onAfterLoadSuccess, title, params, onBeforeLoadData) {
    $.ajax({
        type: 'POST',
        url: u,
        data: params,
        cache: false,
        async: false,
        success: function (data) {
            data = eval('(' + data + ')');
            if (onBeforeLoadData != undefined) {
                data = onBeforeLoadData(data);
            }
            $('#' + dg).datagrid('getPanel').panel('setTitle', title)
            $('#' + dg).datagrid("loadData", data);
            if (onAfterLoadSuccess != undefined) {
                onAfterLoadSuccess(data);
            }
        }
    });
}
//--------------------------------開窗選擇列表，并存檔------------------------


//--------------------------------多選開窗選擇列表，并存檔------------------------
/** @description: 多選開窗
 * @param {string} multiTableName : 對應多選的Table
 * @param {string} funcTag : GEXBAS_TMP.FUNCTIONTAG
* @Sample : uOpenMultiWin('BASCUSTOMER', 'OPOR10'); */
function uOpenMultiWin(multiTableName, funcTag) {
    var buttons = [{
        text: "確認",
        handler: function () {
            uSelectData(multiTableName, funcTag);
        }
    }, {
        text: "關閉",
        handler: uCloseWindow
    }];
    var title = "";

    var formUrl = "../InnerPages/AccessUser.aspx";
    uCreateAndOpenWizardDialog("winAccessUser", title, "450", "350", formUrl, function () { uLoadMultiHandler(multiTableName); }, buttons);
}

/** @description: 多選選擇，在uOpenMultiWin中使用
 * @param {string} multiTableName : 對應多選的Table
 * @param {string} funcTag : GEXBAS_TMP.FUNCTIONTAG */
function uSelectData(multiTableName, funcTag) {
    var IDNO = "", IDNOs = "";
    var rows = $("#gridUsersTo_AccessUser").datagrid("getData").rows;
    if (typeof (eval('ms' + multiTableName)) != "undefined") {
        var multiSelectData = eval('ms' + multiTableName);
        //splice(index,howmany)删除项目，index-起始位置，howmany-刪除數量
        multiSelectData.splice(0, multiSelectData.length);
        for (var i = 0; i < rows.length; i++) {
            IDNO += rows[i].IDNO + ";";
            IDNOs += rows[i].IDNO + ";";
            if (multiSelectData.indexOf(rows[i]) < 0) {
                multiSelectData.push(rows[i]);
            }
        }
    }
    var loginID = uGetUserID();
    var url = "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp";

    $.ajax({
        type: "POST",
        url: url,
        data: "mode=method&method=insertGEXBASTMP&parameters=" + loginID + "|" + funcTag + "|" + multiTableName + "|" + IDNO,
        cache: false,
        async: false,
        success: function (data) {
            $('#winAccessUser').dialog().dialog('close');
        },
        error: function (data) {

        }
    });
}

/** @description: 刪除暫存當GEXBAS_TMP，用於Master的OnLoadSuccess事件中 
 * @param {string} funcTag : GEXBAS_TMP.FUNCTIONTAG
* @Sample : uDeleteGEXBASTM('OPOR10'); */
function uDeleteGEXBASTM(funcTag) {
    var loginID = uGetUserID();
    var url = "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp";
    $.ajax({
        type: "POST",
        url: url,
        data: "mode=method&method=insertGEXBASTMP&parameters=" + loginID + "|" + funcTag,
        cache: false,
        async: false,
        success: function (data) {
            $('#winAccessUser').dialog().dialog('close');
        },
        error: function (data) {

        }
    });
}

//view
if (typeof ($) != "undefined") {
    var uDefaultview = $.extend({}, $.fn.datagrid.defaults.view, {
        onAfterRender: function (target) {
            uRenderQueryAutoColumn(target);
        }
    });

    $.extend($.fn.datagrid.methods, {
        /** @description: 報表多選開窗，對欄位進行查詢 */
        uSetWhere: function (jq, setWhereParam) {
            jq.each(function () {
                var selectedData = [];
                var param = setWhereParam.split('|');
                if (param.length > 0) {
                    var where = param[0];
                    var title = param[1];
                    var multiTableName = param[2];
                    var multiColumnName = param[3];
                }
                var url = "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp";
                uLoadDataForDataGrid("gridUsersFrom_AccessUser", url, undefined, title, "mode=method&method=getMultiData&parameters=" + multiTableName + "|||" + encodeURIComponent(where) + "|" + multiColumnName, function (data) {
                    var newData = [];
                    for (var i = 0 ; i < data.length; i++) {
                        var isExisted = -1;
                        for (var j = 0 ; j < selectedData.length; j++) {
                            if (data[i].IDNO == selectedData[j].IDNO) {
                                isExisted = j;
                                break;
                            }
                        }
                        if (isExisted != -1) {
                            selectedData.splice(isExisted, 1);
                        }
                        else {
                            newData.push(data[i]);
                        }
                    }
                    return newData;
                });

            });
        },
        /** @description: 報表多選開窗，獲取查詢條件 */
        uGetWhereAutoQuery: function (jq) {
            var where = '';
            var queryTr = $('#queryTr_' + $(jq[0]).attr('id'));
            $(":text", queryTr).each(function () {
                var text = $(this);
                var value = text.val();
                if (value != '') {
                    var fieldName = getInfolightOption(text).field;
                    if (fieldName != undefined) {
                        var isNvarChar = false;
                        var nvarchar = '';
                        if (isNvarChar) {
                            nvarchar = 'N';
                        }

                        var conditionId = text[0].id.replace("_TextBox", "_btn");
                        var condition = $('#' + conditionId).text();
                        var dataType = getInfolightOption(text).dataType;
                        switch (condition) {
                            case '= ':
                            case '!=':
                            case '>=':
                            case '<=': where += fieldName + condition + formatQueryValue(value, dataType, isNvarChar); break;
                            case '% ': where += fieldName + " like " + nvarchar + "'" + value.toString().replace(/\'/g, "''") + "%'"; break;
                            case '%%': where += fieldName + " like " + nvarchar + "'%" + value.toString().replace(/\'/g, "''") + "%'"; break;
                            case 'in':
                                var vs = value.toString().split(",");
                                var values = "";
                                for (var i = 0; i < vs.length; i++) {
                                    if (vs[i] != "")
                                        values += "'" + vs[i] + "',";
                                }
                                values = values.substring(0, values.length - 1);
                                where += fieldName + " in (" + values + ")";
                                break;
                            default:
                        }
                    }
                }
            });

            if (where != "")
                where = "(" + where + ")";

            return where;
        }
    });
}


/** @description: 報表多選開窗，添加查詢框 */
function uRenderQueryAutoColumn(target) {
    var infolightOptions = getInfolightOption($(target));
    if ($(target).attr('id') == undefined) {
        var index = 1;
        var id = 'querygrid' + index;
        while ($('#queryTr_' + id).length > 0) {
            index++;
            id = 'querygrid' + index;
        }
        $(target).attr('id', id);
    }
    var panel = $(target).datagrid('getPanel');

    var queryTr = $('#queryTr_' + target.id);
    if (queryTr.length == 0) {
        var hTable = panel.children('div.datagrid-view').children('div.datagrid-view2').children('div.datagrid-header').children('div.datagrid-header-inner').children('table.datagrid-htable');
        var bTable = panel.children('div.datagrid-view').children('div.datagrid-view2').children('div.datagrid-body').children('table.datagrid-btable');
        var tds = "";
        var columns = hTable.children('tbody').children('tr.datagrid-header-row').children('td');
        for (var i = 0; i < columns.length; i++) {
            var field = columns[i].attributes['field'];
            if (field != undefined) {
                var fieldName = columns[i].attributes['field'].value;
                if ($(target).datagrid("getColumnOption", fieldName).hidden != "true" && $(target).datagrid("getColumnOption", fieldName).hidden != true) {
                    var condition = "%";
                    var queryCondition = $(target).datagrid("getColumnOption", fieldName).queryCondition;
                    if (queryCondition) {
                        condition = queryCondition;
                    }
                    var dataType = "string";
                    var pnid = getInfolightOption($(target)).queryDialog;
                    if (pnid) {
                        var queryField = $('#' + fieldName + '_Query', pnid);
                        if (queryField.length != 0) {
                            condition = getInfolightOption($(queryField)).condition;
                            dataType = getInfolightOption($(queryField)).dataType;
                        }
                    }
                    if (condition.length == 1)
                        condition += " ";
                    var td = '<td>';
                    var conditionId = target.id + "_" + fieldName + '_btn';
                    td += '<a class="easyui-linkbutton infosysbutton-s" id="' + conditionId + '" onclick="changeCondition(this)" style="cursor:pointer;padding:0px 5px 0px 7px" >' + condition + '</a>';
                    td += '<input onblur="uQueryAutoColumn(\'#' + target.id + '\')" id ="' + target.id + "_" + fieldName + "_TextBox" + '" type=\"text\" infolight-options="field:\'' + fieldName + '\',condition:\'' + condition + '\',dataType:\'' + dataType + '\'" style="width:50px" />';
                    //if (i == columns.length - 1) {
                    //    td += '<span class="icon-search" style="display:inline-block;width:16px;height:16px;cursor:pointer;" onclick="query(' + target.id + ', \'QueryAutoColumn\', true)" />';
                    //}
                    tds = tds + td + '</td>';
                }
            }
            else {
                var td = '<td/>';
                tds = tds + td;
            }
        }
        var tr = '<tr class="datagrid-header-row" id="queryTr_' + target.id + '">' + tds + '</tr>'; //$('<tr class="datagrid-header-row"><td><input id="Text333" type="text" /></td></tr>');
        tr = $(tr);
        tr.appendTo($('tbody', hTable));

    }
};

/** @description: 報表多選開窗，進行查詢 */
function uQueryAutoColumn(dgid) {
    var where = $(dgid).datagrid('uGetWhereAutoQuery');
    var keyName = "", multiColumnName = "";
    try {
        for (var i = 0; i < multiTableSeries.length; i++) {
            if (IDNOCaption == multiTableSeries[i][3]) {
                multiTableName = multiTableSeries[i][0];
                IDNOName = multiTableSeries[i][1];
                IDNAMEName = multiTableSeries[i][2];
                title = multiTableSeries[i][5];
                multiColumnName = IDNOName + ";" + IDNAMEName;
                break;
            }
        }
        if (where.indexOf('IDNO') >= 0 && where.indexOf('IDNAME') > 0) {
            where = where.substr(0, where.indexOf('IDNAME')) + " AND " + where.substr(where.indexOf('IDNAME'));
        }

        where = where.replace('IDNO', IDNOName);
        where = where.replace('IDNAME', IDNAMEName);
        $(dgid).datagrid('uSetWhere', where + '|' + title + '|' + multiTableName + '|' + multiColumnName);
        $(dgid).datagrid('reload');
    }
    catch (ex) {

    }
}

/** @description: 加載數據，在uOpenMultiWin中使用 */
function uLoadMultiHandler(multiTableName) {
    //隱藏2個button
    $('#btnUsersTo_AccessUser')[0].style.display = "none";
    $('#btnUsersBack_AccessUser')[0].style.display = "none";
    var title1 = "", title2 = "";

    for (var i = 0; i < multiTableSeries.length; i++) {
        if (multiTableName == multiTableSeries[i][0]) {
            IDNOName = multiTableSeries[i][1];
            IDNAMEName = multiTableSeries[i][2];
            IDNOCaption = multiTableSeries[i][3];
            IDNAMECaption = multiTableSeries[i][4];
            title1 = multiTableSeries[i][5];
            title2 = multiTableSeries[i][6];
            break;
        }
    }
    multiColumnName = IDNOName + ";" + IDNAMEName;

    $('#gridUsersFrom_AccessUser').datagrid({
        columns: [[
                    { field: 'IDNO', title: IDNOCaption, sortable: true, width: 80 },
                    { field: 'IDNAME', title: IDNAMECaption, sortable: true, width: 87 }
        ]],
        fitColumns: false,
        view: uDefaultview,
        onClickRow: function (rowindex, rowdata) { uFromTo("gridUsersFrom_AccessUser", "gridUsersTo_AccessUser"); }
    });
    $('#gridUsersTo_AccessUser').datagrid({
        columns: [[
                    { field: 'IDNO', title: IDNOCaption, sortable: true, width: 80 },
                    { field: 'IDNAME', title: IDNAMECaption, sortable: true, width: 87 }
        ]],
        fitColumns: false,
        onClickRow: function (rowindex, rowdata) { uFromTo("gridUsersTo_AccessUser", "gridUsersFrom_AccessUser"); }
    });


    var selectedData = [];
    var url = "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp";

    uLoadDataForDataGrid("gridUsersTo_AccessUser", url, function (data) {
        selectedData = [];
        for (var i = 0 ; i < data.length; i++) {
            selectedData.push(data[i]);
        }
        return selectedData;
    }, title2, "mode=method&method=getMultiData&parameters=" + multiTableName + "||||" + multiColumnName,
    function (data) {
        var newData = [];
        if (typeof (eval('ms' + multiTableName)) != "undefined")
            var multiSelectData = eval('ms' + multiTableName);
        for (var i = 0 ; i < multiSelectData.length; i++) {
            var isExisted = -1;
            for (var j = 0 ; j < selectedData.length; j++) {
                if (multiSelectData[i].IDNO == selectedData[j].IDNO) {
                    isExisted = j;
                    break;
                }
            }
            if (isExisted != -1) {
                selectedData.splice(isExisted, 1);
            }
            else {
                newData.push(multiSelectData[i]);
            }
        }
        return newData;
    });

    uLoadDataForDataGrid("gridUsersFrom_AccessUser", url, undefined, title1, "mode=method&method=getMultiData&parameters=" + multiTableName + "||||" + multiColumnName, function (data) {
        var newData = [];
        for (var i = 0 ; i < data.length; i++) {
            var isExisted = -1;
            for (var j = 0 ; j < selectedData.length; j++) {
                if (data[i].IDNO == selectedData[j].IDNO) {
                    isExisted = j;
                    break;
                }
            }
            if (isExisted != -1) {
                selectedData.splice(isExisted, 1);
            }
            else {
                newData.push(data[i]);
            }
        }
        return newData;
    });
}
//--------------------------------開窗選擇列表，并存檔------------------------


//--------------------------------Excel導入界面------------------------
/** @description: 打開Excel導入Dialog
* @Sample : jqBASM501：uOpenImportGrid("BASPRODUCT"); */
var uExcelImportTable = "";
function uOpenImportGrid(tableName) {
    uExcelImportTable = tableName;
    var JQDialog = "JQDialogReadExcel";
    $("#" + JQDialog).find(".infosysbutton-s").hide();
    $("#" + JQDialog).find(".infosysbutton-c").hide();
    $('[name="undefined"]').val("")
    $("#dgExcel").datagrid("reload");

    var sql = "SELECT FIELD_NAME,CAPTION FROM COLDEF WHERE TABLE_NAME = '" + tableName + "'";
    var parameters = sql + "|Y";

    $.ajax({
        type: "POST",
        url: "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp",
        data: "mode=method&method=commExcuteSql&parameters=" + parameters,
        cache: false,
        async: false,
        success: function (data) {
            var width = 100;
            var rows = $.parseJSON(data);
            var columns = [];
            for (var j = 0; j < excelFields.length; j++) {
                var field = excelFields[j];
                var column = new Object();
                column.field = field;
                column.width = width;
                for (var i = 0; i < rows.length; i++) {
                    var fieldName = rows[i].FIELD_NAME;
                    if (field.toUpperCase() == fieldName.toUpperCase()) {
                        column.title = rows[i].CAPTION;
                        break;
                    }
                }
                if (typeof (column.title) == "undefined")
                    column.title = field;
                columns.push(column);
            }

            $('#dgExcel').datagrid({ columns: [columns] });
            $("#dgExcel").datagrid("reload");
            uOpenDialog(JQDialog);
        },
        error: function (data) {
            alert(data);
        }
    });
};

/** @description: 讀取Excel，button觸發 */
function uReadExcel() {
    //var fileName = $('[name="undefined"]').val();
    var fileName = $("#importFileUpload").next().find('.info-fileUpload-value').val();
    var folder = getInfolightOption($("#importFileUpload")).upLoadFolder;

    if (fileName == "") {
        alert("請選擇文件。");
    }
    else {
        var parameters = folder + "/" + fileName + "|" + uExcelImportTable;
        $.ajax({
            type: "POST",
            url: "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp",
            data: "mode=method&method=readExcel&parameters=" + parameters,
            cache: false,
            async: false,
            success: function (data) {
                try {
                    var rows = $.parseJSON(data);
                    $("#dgExcel").datagrid({ loadFilter: uPagerFilter, url: "", loadMsg: "loadMsg" }).datagrid('loadData', rows);
                }
                catch (ex) {
                    alert(data);
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    }
}

/** @description: 導入Excel
 * @param {string} dgViewID : view對應的dataGridID
 * @param {string} tableName : 表名
 * @param {string} keyColumns : 主鍵欄位
* @Sample : jqBASM501：uExcelImport("dgView", "BASPRODUCT"); */
function uExcelImport(dgViewID, tableName, keyColumns) {
    //var fileName = $('[name="undefined"]').val();
    var fileName = $("#importFileUpload").next().find('.info-fileUpload-value').val();
    var folder = getInfolightOption($("#importFileUpload")).upLoadFolder;
    var cover = $('#cbCover')[0].checked;
    if (typeof (keyColumns) == "undefined")
        keyColumns = $("#" + dgViewID).attr('keyColumns');
    var parameters = folder + "/" + fileName + "|" + cover + "|" + keyColumns + "|" + tableName;

    $.ajax({
        type: "POST",
        url: "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp",
        data: "mode=method&method=excelImport&parameters=" + parameters,
        cache: false,
        async: false,
        success: function (data) {
            try {
                var ret = data.split('|');
                var rows = $.parseJSON(ret[0]);
                var successNum = ret[1];
                var coverNum = ret[2];
                var failNum = rows.length;
                if (failNum > 0) {
                    alert(successNum + "筆資料成功轉入。\n" + failNum + "筆資料轉入失敗。\n" + coverNum + "筆資料成功覆蓋。");
                    $("#dgExcel").datagrid({ loadFilter: uPagerFilter, url: "", loadMsg: "loadMsg" }).datagrid('loadData', rows);
                }
                else {
                    alert(successNum + "筆資料成功轉入。\n" + coverNum + "筆資料成功覆蓋。");
                    $('#' + dgViewID).datagrid('reload');
                    $('#JQDialogReadExcel').dialog('close');
                }
            }
            catch (ex) {
                alert(data);
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}

/** @description: 導出Excel，dgid=對應的dataGrid，fields-需要導出的欄位 */
function uExportExcelOld(dgid, fields) {
    //var title = $(dgid).datagrid('options').title;
    //var columns = [];
    //for (var i = 0; i < fields.length; i++) {
    //    var field = fields[i]
    //    var column = new Object();
    //    column.field = field;
    //    column.title = field;
    //    columns.push(column);
    //}

    ////../handler/jqDataHandle.ashx?RemoteName=ssESSM01.ESSPERSONNELCODE&TableName=ESSPERSONNELCODE&IncludeRows=true
    //var url = $(dgid).datagrid('options').url;
    //var queryWord = $(dgid).datagrid('options').queryParams.queryWord;

    //$.ajax({
    //    type: "POST",
    //    url: url,
    //    data: "mode=export&title=" + title + "&columns=" + $.toJSONString(columns) + "&queryWord=" + encodeURIComponent(queryWord),
    //    cache: false,
    //    async: false,
    //    success: function (data) {
    //        window.open('../handler/JqFileHandler.ashx?File=' + data, 'download');
    //    }
    //});
}

/** @description: 導出Excel
 * @param {string} dgid : 對應的dataGrid
 * @param {string} title : 標題
 * @param {string} sheetName : 頁簽名
 * @param {string} fileName : 文件名稱
 * @param {string} table : 對應的table
 * @param {string} fields : 顯示欄位
 * @param {string} execSQL : 執行的SP
* @Sample : 範例jqESSM01：uExportExcel(dgid, "職務代號設定", "ESSPERSONNELCODE", "gESS_jqESSM01", "ESSPERSONNELCODE", excelFields);
                   範例jqOPOR01：uExportExcel(dgid, "報價單日報表", "VIEW_RPT_OPOR01", "gOPO_jqOPOR01", "VIEW_RPT_OPOR01", excelFields);
                   範例jqOPOR10：uExportExcel(dgid, "已報價未訂購明細表", "GEXRPT_OPOR10", "gOPO_jqOPOR10", "StoreProcedure", excelFields, sql);   */
function uExportExcel(dgid, title, sheetName, fileName, table, fields, execSQL) {
    var sql = "", reportData = "";
    if (table == "StoreProcedure") {
        //HTML5 Web Storage
        var reportData = window.sessionStorage.getItem("uReportData");
        if (reportData.length == 0) {
            if (typeof (execSQL) == "undefined") {
                alert("No sql");
                return false;
            }
            else
                sql = execSQL;
        }
    }
    else if (table == "execSQL") {
        sql = execSQL;
    }
    else {
        var queryWord = $(dgid).datagrid('options').queryParams.queryWord;
        var whereString = " 1=1";
        if (queryWord.length > 0)
            whereString = $.parseJSON(queryWord).whereString;
        sql = "SELECT * FROM " + table + " WHERE " + whereString;
    }

    if (typeof (fields) == "undefined") fields = "";
    parameters = title + "|" + sheetName + "|" + fileName + "|" + sql + "|" + fields + "|" + reportData;

    $.ajax({
        type: "POST",
        url: '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp',
        data: 'mode=method&method=exportExcel&parameters=' + parameters,
        cache: false,
        async: false,
        success: function (data) {
            window.open('../handler/JqFileHandler.ashx?File=' + data, 'download');
        }
    });
}

/** @description: SP導出Excel時使用，整理SQL
 * @param {string} SPName : SP的名稱
 * @param {string} queryStr : 查詢條件
 * @param {string} multiTableNameAndKey : 多選的Table及Key
* @Sample : 範例jqOPOR10：var sql = uGetSPSql("GEXRPT_OPOR10", "sBILLDATE1,sBILLDATE2,sCUSTID1,sCUSTID2,sPRODUCTID1,sPRODUCTID2,sPERSONID1,sPERSONID2,sISSIGN,sLOGINID", "BASCUSTOMER,CUSTID;BASPRODUCT,PRODUCTID;BASPERSON,PERSONID"); */
function uGetSPSql(SPName, queryStr, multiTableNameAndKey) {
    uGetSPParam(queryStr, multiTableNameAndKey);
    var spParam = SPParam.split("|");

    var sql = "exec " + SPName + " ";
    queryStr = queryStr.replace(/s/g, "");
    var queryColumn = queryStr.split(',');
    for (var i = 0; i < queryColumn.length; i++) {
        sql += "@" + queryColumn[i] + "=N'" + spParam[i] + "',";
    }
    sql = sql.substr(0, sql.length - 1);
    return sql;
}

var uColumnPara1 = "", uColumnPara2 = "";

/** @description: 讀取特殊Excel，CRTM17 */
//var uColumnPara1 = "CYEAR,2,0,,,4;AREAID,3,0,,-,0;CORPID,7,j,,,2;CURRENCYID,3,1,,,0;DIVID,0,0,),,0;PLINEID,i,0,,,2;PRODNAME,i,0,,,0;FORECAST,i,j,,,0";
//var uColumnPara2 = "TITLEID:3,9,00110;11,17,00210;19,25,00430";
function uReadSpecialExcel() {
    //var fileName = $('[name="undefined"]').val();
    var fileName = $("#importFileUpload").next().find('.info-fileUpload-value').val();
    var folder = getInfolightOption($("#importFileUpload")).upLoadFolder;
    if (fileName == "") {
        alert("請選擇文件。");
    }
    else {
        var parameters = folder + "/" + fileName + "|" + uColumnPara1 + "|" + uColumnPara2;
        $.ajax({
            type: "POST",
            url: "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp",
            data: "mode=method&method=readSpecialExcel&parameters=" + parameters,
            cache: false,
            async: false,
            success: function (data) {
                try {
                    var rows = $.parseJSON(data);
                    $("#dgExcel").datagrid({ loadFilter: uPagerFilter, url: "", loadMsg: "loadMsg" }).datagrid('loadData', rows);
                }
                catch (ex) {
                    alert(data);
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    }
}

/** @description: 特殊導入Excel
 * @param {string} dgViewID : view對應的dataGridID
 * @param {string} tableName : 查詢表名條件
 * @param {string} keyColumns : 鍵值
* @Sample : 範例jqCRTM17：uSpecialExcelImport("dgView", "ACT_YEARBUDGET7", "CYEAR,AREAID,CORPID,CURRENCYID,DIVID,PLINEID,TITLEID,PRODNAME"); */
function uSpecialExcelImport(dgViewID, tableName, keyColumns) {
    //var fileName = $('[name="undefined"]').val();
    var fileName = $("#importFileUpload").next().find('.info-fileUpload-value').val();
    var folder = getInfolightOption($("#importFileUpload")).upLoadFolder;
    var cover = $('#cbCover')[0].checked;
    if (typeof (keyColumns) == "undefined")
        keyColumns = $("#" + dgViewID).attr('keyColumns');
    var parameters = folder + "/" + fileName + "|" + cover + "|" + keyColumns + "|" + tableName + "|" + uColumnPara1 + "|" + uColumnPara2;

    $.ajax({
        type: "POST",
        url: "../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp",
        data: "mode=method&method=specialExcelImport&parameters=" + parameters,
        cache: false,
        async: false,
        success: function (data) {
            try {
                var ret = data.split('|');
                var rows = $.parseJSON(ret[0]);
                var successNum = ret[1];
                var coverNum = ret[2];
                var failNum = rows.length;
                if (failNum > 0) {
                    alert(successNum + "筆資料成功轉入。\n" + failNum + "筆資料轉入失敗。\n" + coverNum + "筆資料成功覆蓋。");
                    $("#dgExcel").datagrid({ loadFilter: uPagerFilter, url: "", loadMsg: "loadMsg" }).datagrid('loadData', rows);
                }
                else {
                    alert(successNum + "筆資料成功轉入。\n" + coverNum + "筆資料成功覆蓋。");
                    $('#' + dgViewID).datagrid('reload');
                    $('#JQDialogReadExcel').dialog('close');
                }
            }
            catch (ex) {
                alert(data);
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}

/** @description: 開窗時自動調整寬度至全屏/置中，在uOpenForm方法中使用 */
function uSetDialogs(fid, modal) {
    var setDailog = false;
    if (typeof (fullWindowDialogs) != "undefined" && fullWindowDialogs.indexOf(fid) >= 0) {
        var fDialogs = fullWindowDialogs.split(";");
        for (var i = 0; i < fDialogs.length; i++) {
            if (fDialogs[i] == fid) {
                var height = $(window).height() - 20;
                var width = $(window).width() - 20;
                if (modal) {
                    $(fid).dialog({
                        height: height,
                        width: width,
                        modal: true
                    });
                }

                $(fid).dialog('open');
                var dialogLeft = 10;
                var dialogTop = 10;
                $(fid).window('move', {
                    left: dialogLeft,
                    top: $(document).scrollTop() + dialogTop
                });
                setDailog = true;

                break;
            }
        }
    }
    if (typeof (centerWindowDialogs) != "undefined" && centerWindowDialogs.indexOf(fid) >= 0) {
        //$Edit 2015-11-13 by alex：開窗時自動調整寬度置中
        var cDialogs = centerWindowDialogs.split(";");
        for (var i = 0; i < cDialogs.length; i++) {
            if (cDialogs[i] == fid) {
                var height = $(fid).dialog('options').height;
                var width = $(fid).dialog('options').width;
                if (modal) {
                    $(fid).dialog({
                        height: height,
                        width: width,
                        modal: true
                    });
                }
                $(fid).dialog('open');
                setDailog = true;
                break;
            }
        }
    }

    return setDailog;
}

/** @description: 將jquery.infolight.js中openForm方法提出來，呼叫uSetDialogs開窗時自動調整寬度至全屏/置中
* @Sample : 範例jqINVM01：uOpenForm(fid, rowData, mode, editmode, keys); */
function uOpenForm(fid, rowData, mode, editmode, keys) {
    $.fn.Error.errorCode = 1100;
    var formname = getInfolightOption($(fid)).containForm;
    var form = $(formname);
    var dialoggrid = form.attr('dialogGrid');
    if (dialoggrid == undefined) dialoggrid = form.attr('switchGrid');
    if (dialoggrid == undefined) dialoggrid = form.attr('continueGrid');
    if (dialoggrid) {
        var autoApply = getInfolightOption($(dialoggrid)).autoApply;
        if (!autoApply) {
            var dialogbuttontext = $.sysmsg('getValue', 'JQWebClient/dialogbuttontext');
            var dialogbuttontexts = dialogbuttontext.split(',');
            var oklocal = dialogbuttontexts[2];
            $(fid).find('.infosysbutton-s').find('.l-btn-text').text(oklocal);
        }
    }

    var tableName = getInfolightOption(form).tableName;
    if (editmode.toLowerCase() == "dialog") {
        if ($(fid).attr('class') != undefined && $(fid).hasClass('easyui-dialog')) {
            var modal = $(fid).dialog('options').modal;
            //$Edit 2015-11-12 by alex：開窗時自動調整寬度至全屏/置中
            if (!uSetDialogs(fid, modal)) {
                if (modal) {
                    $(fid).dialog({
                        modal: true
                    });
                }
                $(fid).dialog('open');
                var dialogLeft = getInfolightOption($(fid)).dialogLeft;
                var dialogTop = getInfolightOption($(fid)).dialogTop;
                //            if (dialogLeft != undefined && dialogTop != undefined) {
                $(fid).window('move', {
                    left: dialogLeft,
                    top: $(document).scrollTop() + dialogTop
                });
                //            }
            }
        }
        else {
            alertMessage('dialogerrorsetting');
            return;
        }
    }
    else if (editmode.toLowerCase() == "switch") {
        if ($(fid).hasClass('easyui-dialog')) {
            alertMessage('dialogerrorsetting');
            return;
        } else {
            var styles = ['width', 'padding'];
            var style = {};
            for (var i = 0; i < styles.length; i++) {
                style[styles[i]] = $(fid).css(styles[i]);
            }
            $(fid).css('display', 'block');
            $(fid).css(style);
        }
        $('.info-flcomment', fid).each(function () {
            initInfoFLComment($(this));
        });
    }
    if (tableName != undefined) {
        var parentRow = new Object();
        for (var property in rowData) {
            parentRow[property] = rowData[property];
        }
        loadFormRow(form, rowData, mode);
        changeFormState(fid, mode, keys, editmode);
        $(dataGrid_class, fid).each(function () {
            var datagrid = $(this);
            if ($(this).closest('.no-preload').length > 0) {
                return true;
            }
            var parent = getInfolightOption(datagrid).parent;
            if (parent != undefined && parent == form.attr('id')) {
                if (mode != "inserted" && rowData != null) {
                    initInfoDataGrid(datagrid, tableName, parentRow);
                }
                else {
                    initInfoDataGrid(datagrid, tableName);
                }
            }
        });
        var onLoadSuccess = getInfolightOption(form).onLoadSuccess
        if (onLoadSuccess) {
            onLoadSuccess.call(form);
        }
    }
};

/** @description: json轉字符串，在uSetHtmlEditor中使用 */
function uJsonToString(obj) {
    var THIS = this;
    switch (typeof (obj)) {
        case 'string':
            return '"' + obj.replace(/(["\\])/g, '\\$1') + '"';
        case 'array':
            return '[' + obj.map(THIS.uJsonToString).join(',') + ']';
        case 'object':
            if (obj instanceof Array) {
                var strArr = [];
                var len = obj.length;
                for (var i = 0; i < len; i++) {
                    strArr.push(THIS.uJsonToString(obj[i]));
                }
                return '[' + strArr.join(',') + ']';
            } else if (obj == null) {
                return 'null';

            } else {
                var string = [];
                for (var property in obj) string.push(THIS.uJsonToString(property) + ':' + THIS.uJsonToString(obj[property]));
                return '{' + string.join(',') + '}';
            }
        case 'number':
            return obj;
        case false:
            return obj;
    }
}

/** @description: 設置HtmlEditor
 * @param {string} jqhtmlId : HtmlEditor的ID
 * @param {string} keyID : 鍵值
* @Sample :範例jqEIPM41_1：uSetHtmlEditor("JQHtml1", "WB_no");*/
function uSetHtmlEditor(jqhtmlId, keyID) {
    var ue = UE.getEditor(jqhtmlId);
    ue.addListener('ready', function () {
        var keyValue = uGetQueryString(keyID);
        var key = { key: keyID, value: keyValue };
        var kvs = [];
        kvs.push(key);
        var sK = uJsonToString(kvs);
        $('#' + jqhtmlId).attr('keyValues', sK);
        loadHtmlFromDB(jqhtmlId, kvs);
    });
}

function uLoadHtmlFromDB(jqhtmlId, keyID, mode) {
    var ue = UE.getEditor(jqhtmlId);
    var keyValue = uGetQueryString(keyID);
    var key = { key: keyID, value: keyValue };
    var kvs = [];
    kvs.push(key);
    var sK = uJsonToString(kvs);
    $('#' + jqhtmlId).attr('keyValues', sK);

    var infolightOptions = getInfolightOption($('#' + jqhtmlId));
    if (mode == "Edit") {
        UE.getEditor(jqhtmlId).setEnabled();
        $("#" + jqhtmlId + "_view").css("display", "none");
    }
    else if (mode == "Show") {
        UE.getEditor(jqhtmlId).setHide();
    }
    var keyValues = kvs;
    if (keyValues.length == 0) {
        alert("Please set keys and keyValues first.");
        return;
    }

    var queryWord = new Object();
    queryWord.whereString = "";
    for (var i = 0; i < keyValues.length; i++) {
        queryWord.whereString += keyValues[i].key + " = " + formatQueryValue(keyValues[i].value, "string", false) + " and ";
    }
    queryWord.whereString = queryWord.whereString.substr(0, queryWord.whereString.lastIndexOf(' and '));

    $.ajax({
        type: "POST",
        dataType: 'json',
        url: getRemoteUrl(infolightOptions.remoteName, infolightOptions.tableName),
        data: 'queryWord=' + $.toJSONString(queryWord),
        cache: false,
        async: false,
        success: function (data) {
            if (data.length > 0) {
                var content = data[0][infolightOptions.columnName];
                content = content.replace(/qnMk/g, "\"");
                content = content.replace(/bhMk/g, "\\");
                if (mode == "Edit")
                    UE.getEditor(jqhtmlId).setContent(content, false);
                else if (mode == "Show") {
                    $("#" + jqhtmlId + "_view")[0].innerHTML = content;
                    //UE.getEditor(jqhtmlId).setContent(content, false);
                }

            }
        },
        error: function (ex) {

        },
        complete: function () {

        }
    });
}

/** @description: 将鼠标移到标签条上，打开标签面板。
* @Sample : 範例jqSYSM06：uHoverTabs("JQTab1"); */
function uHoverTabs(tabID) {
    var tabs = $('#' + tabID).tabs().tabs('tabs');
    for (var i = 0; i < tabs.length; i++) {
        tabs[i].panel('options').tab.unbind().bind('mouseenter', { index: i }, function (e) {
            $('#' + tabID).tabs('select', e.data.index);
        });
    }
}

/** @description: 開啟樞紐分析，數據源為SP 
 * @param {string} title : 標題
 * @param {string} pageID : 新打開的頁面
 * @param {string} remoteName : 後端InfoComman
 * @param {string} tableName : 表名
 * @param {string} queryStr : 查詢條件
 * @param {string} multiTableNameAndKey : 多選的Table及Key
 * @param {string} FUNCTAG : GEXBAS_TMP.FUNCTIONTAG 
 * @param {string} showColumns : 顯示的欄位
 * @param {string} showColumnNames : 欄位名稱
 * @param {string} rows : 加載時的行欄位
 * @param {string} cols : 加載時的列欄位
* @Sample : 範例jqOPOR10：uOpenPivotDialogSP("OPOR10樞紐分析", "gSYS/jqPivotTable.aspx", "srOPO.GEXRPT_OPOR10", "GEXRPT_OPOR10", "sBILLDATE1,sBILLDATE2,sCUSTID1,sCUSTID2,sPRODUCTID1,sPRODUCTID2,sPERSONID1,sPERSONID2,sISSIGN,sLOGINID", "BASCUSTOMER,CUSTID;BASPRODUCT,PRODUCTID;BASPERSON,PERSONID", "OPOR10", "PRODUCTID,PERSONID,PRODCNAME,PRICE", "PRODUCTID", "PRICE"); */
function uOpenPivotDialogSP(title, pageID, remoteName, tableName, queryStr, multiTableNameAndKey, FUNCTAG, showColumns, showColumnNames, rows, cols) {
    var setstr = uGetSPParam(queryStr, multiTableNameAndKey, FUNCTAG);
    var url = encodeURI(encodeURI(pageID + "?remoteName=" + remoteName + "&tableName=" + tableName + "&setstr=" + setstr + "&showColumns=" + showColumns + "&showColumnNames=" + showColumnNames + "&rows=" + rows + "&cols=" + cols));
    parent.addTab(title, url);
}

/** @description: 開啟樞紐分析，數據源為View
 * @param {string} title : 標題
 * @param {string} pageID : 新打開的頁面
 * @param {string} remoteName : 後端InfoComman
 * @param {string} tableName : 表名
 * @param {string} queryStr1 : 範圍查詢
 * @param {string} queryStr2 : Like查詢
 * @param {string} queryStr3 : 單個查詢
 * @param {string} multiTableNameAndKey : 多選的Table及Key
 * @param {string} FUNCTAG : GEXBAS_TMP.FUNCTIONTAG 
 * @param {string} showColumns : 顯示的欄位
 * @param {string} showColumnNames : 欄位名稱
 * @param {string} rows : 加載時的行欄位
 * @param {string} cols : 加載時的列欄位
* @Sample : 範例jqOPOR01：uOpenPivotDialogView("OPOR01樞紐分析", "gSYS/jqPivotTable.aspx", "srOPO.VIEW_RPT_OPOR01", "VIEW_RPT_OPOR01", "BILLDATE;BILLNO;CUSTID;PRODUCTID;PERSONID", "PRODDESC", "", "BASCUSTOMER,CUSTID;BASPRODUCT,PRODUCTID;BASPERSON,PERSONID", "DATEYEAR,DATEMONTH,CUSTID,PRODUCTID,WAREID,PRODCNAME,PRICE", "PRODUCTID", "DATEYEAR,DATEMONTH"); */
function uOpenPivotDialogView(title, pageID, remoteName, tableName, queryStr1, queryStr2, queryStr3, multiTableNameAndKey, showColumns, showColumnNames, rows, cols) {
    var setstr = uGetReportMultiQuerySetWhere(queryStr1, queryStr2, queryStr3, multiTableNameAndKey);
    var url = encodeURI(encodeURI(pageID + "?remoteName=" + remoteName + "&tableName=" + tableName + "&setstr=" + setstr + "&showColumns=" + showColumns + "&showColumnNames=" + showColumnNames + "&rows=" + rows + "&cols=" + cols));
    parent.addTab(title, url);
}

/** @description: 初始化ComboGrid */
function uInitInfoComboGrid(combogrid, width) {
    //var field = getInfolightOption(combogrid).field;
    var form = getInfolightOption(combogrid).form;
    var valueField = getInfolightOption(combogrid).valueField;
    var textField = getInfolightOption(combogrid).textField;
    var valueFieldCaption = getInfolightOption(combogrid).valueFieldCaption;
    var textFieldCaption = getInfolightOption(combogrid).textFieldCaption;
    var PageSize = getInfolightOption(combogrid).PageSize;
    var panelWidth = getInfolightOption(combogrid).panelWidth;
    var remoteName = getInfolightOption(combogrid).remoteName;
    var tableName = getInfolightOption(combogrid).tableName;
    var columns = getInfolightOption(combogrid).columns;
    var selectOnly = getInfolightOption(combogrid).selectOnly;
    var onBeforeLoad = getInfolightOption(combogrid).onBeforeLoad;
    var multiple = getInfolightOption(combogrid).multiple;
    if (columns == null || columns.length == 0) {
        columns = [{
            field: valueField, title: valueFieldCaption, align: 'left', width: 150
        }];
        if (valueField != textField) {
            var textColumn = {
                field: textField, title: textFieldCaption, align: 'left', width: 150
            }
            columns.push(textColumn);
        }
    }
    if (multiple) {
        columns.unshift({ field: 'ck', checkbox: true });
    }
    var onSelect = getInfolightOption(combogrid).onSelect;
    var queryParams = new Object();
    $(combogrid).css("width", width);
    queryParams.queryWord = '';
    $(combogrid).combogrid({
        url: getRemoteUrl(remoteName, tableName, true),
        panelWidth: panelWidth,
        pagination: true,
        editable: !selectOnly,
        keyHandler: {
            up: function () { },
            down: function () { },
            enter: function () { },
            query: combogridQuery
        },
        queryParams: queryParams,
        fitColumns: false,
        idField: valueField,
        textField: textField,
        columns: [columns],
        onBeforeLoad: onBeforeLoad,
        multiple: multiple,
        onClickRow: function (rowindex, rowdata) {
            if (onSelect != undefined) {
                onSelect.call(combogrid, rowdata);
            }
        }
    });   //初始化
};

function uInitInfoComboGrids(combogrids, width) {
    var combogridsArr = combogrids.split(";");
    for (var i = 0; i < combogridsArr.length; i++) {
        var combogrid = $("#" + combogridsArr[i] + "1Query");
        uInitInfoComboGrid(combogrid, width);
        var combogrid = $("#" + combogridsArr[i] + "2Query");
        uInitInfoComboGrid(combogrid, width);
    }
}

/** @description: 執行SQL，存入HTML5的Storage
 * @param {string} sql : 需要執行的sql 
 * @param {string} returnData : 是否要傳回數據, Y表示傳回數據(可以在uAfterExecSQL方法中對數據操作)
 * @param {string} storageName : HTML5的storage對應的名稱
 * @param {string} columnName : 取回資料的欄位名稱，ALL則表示全部存入Storage
* @Sample : 範例MainPage.js：uExecSqlToStorage(sql, "Y", "paraCloseDate", "PARAVALUE");*/
function uExecSqlToStorage(sql, returnData, storageName, columnName) {
    var parameters = sql + "|" + returnData;
    $.ajax({
        type: "POST",
        url: "handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp",
        data: "mode=method&method=commExcuteSql&parameters=" + parameters,
        cache: false,
        async: false,
        success: function (data) {
            var rows = $.parseJSON(data);
            if (rows.length > 0) {
                if (columnName == "ALL")
                    window.sessionStorage.setItem(storageName, data);
                else if (typeof (eval("rows[0]." + columnName)) != "undefined")
                    window.sessionStorage.setItem(storageName, eval("rows[0]." + columnName));
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}


/** @description: 讀取Storage中的KEYAVLUE，賦值給Combo 
 * @param {string} comboID : Combo的ID
 * @param {string} storageName : HTML5的storage對應的名稱
 * @param {string} keyTag : 需要顯示的KEYTAG
* @Sample : 範例jqINVM01：uGetStorageToCombo("dataFormMaster1TAXCLASS", "paraKeyValue", "TAXCLASS");*/
function uGetStorageToCombo(comboID, storageName, keyTag) {
    var paraKeyValue = window.sessionStorage.getItem(storageName);
    var rows = $.parseJSON(paraKeyValue);
    var comboData = [];
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].KEYTAG == keyTag)
            comboData.push({ text: rows[i].KEYVALUE, value: rows[i].KEYID });
    }
    $('#' + comboID).combobox('clear');
    $('#' + comboID).combobox('loadData', comboData);
}

/** @description: 新增時確認KEY值是否重複 建議用在JQValidate
 * @param {string} Key : 新增資料的Key值
 * @param {string} table : 屬於哪張資料表
* @Sample : 範例jqBASM603：uCheckKey(val, "BASCUSTCATEGORY");*/
function uCheckKey(key, table, column) {
    var SPParam = key + "|" + table + "|" + column;
    var check;
    try {
        $.ajax({
            type: 'POST',
            url: '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp',
            data: 'mode=method&method=CheckKey&parameters=' + SPParam,
            cache: false,
            async: false,
            success: function (data) {
                check = data;
            }
        });
        return check;
    }
    catch (ex) {
    }
}

/*@description: 查看該員工屬於哪個部門*/
function uGETDEPARTMENTID() {
    try {
        $.ajax({
            type: 'POST',
            url: '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp',
            data: 'mode=method&method=GETDEPARTMENTID&parameters=' + getClientInfo('_usercode'),
            cache: false,
            async: false,
            success: function (data) {
                check = data;
            }
        });
        return check;
    }
    catch (ex) {
    }
}

/** @description: 新增時確認KEY值是否重複 建議用在JQValidate
 * @param {string} Key : 新增資料的Key值
 * @param {string} table : 屬於哪張資料表
* @Sample : 範例jqINVM01：openBOXDiv();*/
function uopenBOXDiv() {
    var JQDialog = $("#JQDialogBOX");
    JQDialog.find(".infosysbutton-s").hide();
    JQDialog.find(".infosysbutton-c").hide();
    var detailGrid = $("#dataGridDetail");
    var rows = detailGrid.datagrid("getRows");
    var selectedRow = detailGrid.datagrid("getSelected");
    if (selectedRow) {
        var rowIndex = detailGrid.datagrid("getRowIndex", selectedRow);
        var sPRODUCTID = uGetDetailValue(detailGrid, rowIndex, "PRODUCTID");
    }
    else {
        alert("未選取資料");
        return;
    }
    if (sPRODUCTID == "noEditor") {
        alert("請點選要修改的資料列");
        return;
        //$("#tbBOXQTY").attr('value', detailGrid.datagrid('getSelected').BOXQTY);
        //$("#tbBOXPRICE").attr('value', detailGrid.datagrid('getSelected').BOXPRICE);
    }
    else {
        var sBOXQTY = uGetDetailValue(detailGrid, rowIndex, "BOXQTY");
        var sBOXPRICE = uGetDetailValue(detailGrid, rowIndex, "BOXPRICE");
        $("#tbBOXQTY").attr('value', uGetDetailValue(detailGrid, rowIndex, "BOXQTY"));
        $("#tbBOXPRICE").attr('value', uGetDetailValue(detailGrid, rowIndex, "BOXPRICE"));
        var NEWdata;
        var check;
        $.ajax({
            type: 'POST',
            url: '../handler/jqDataHandle.ashx?RemoteName=gServerDataModuleComm.cmdTemp',
            data: 'mode=method&method=CheckBOXUNIT&parameters=' + sPRODUCTID,
            cache: false,
            async: false,
            success: function (data) {
                check = data;
                if (data != "N") {
                    NEWdata = data.split("|");
                    $("#lbPACKINGNUMBER1").text(NEWdata[0]);
                    $("#lbSTANDARDCOST").text(NEWdata[1]);
                }
            }
        });
    }

    if (check == "N") {
        alert("該產品的包裝數量與計量單位可能有誤");
        return;
    }
    JQDialog.dialog({
        title: "多單位輸入",
        buttons: [{
            text: "確認", handler: function () {
                sBOXQTY = $("#tbBOXQTY").val();
                sBOXPRICE = $("#tbBOXPRICE").val();

                var QUANTITY = uAccCalc(sBOXQTY, NEWdata[0], "*", uSetQtyDecimal);
                var PRICE = uAccCalc(sBOXPRICE, NEWdata[0], "/", uPriceDecimal);
                var SUBAMOUNT = uAccCalc(sBOXQTY, sBOXPRICE, "*", uSetSubDecimal);
                var TAXPRICE = uAccCalc(PRICE, uTaxRate, "*", uTaxDecimal);
                var TAXAMOUNT = uAccCalc(SUBAMOUNT, uTaxRate, "*", uSetTaxDecimal);
                uSetDetailValue(detailGrid, rowIndex, "BOXQTY", sBOXQTY);
                uSetDetailValue(detailGrid, rowIndex, "BOXPRICE", sBOXPRICE);
                uSetDetailValue(detailGrid, rowIndex, "QUANTITY", QUANTITY);
                uSetDetailValue(detailGrid, rowIndex, "UNIT", NEWdata[1]);
                uSetDetailValue(detailGrid, rowIndex, "PRICE", PRICE);
                uSetDetailValue(detailGrid, rowIndex, "SUBAMOUNT", SUBAMOUNT);
                uSetDetailValue(detailGrid, rowIndex, "TAXPRICE", TAXPRICE);
                uSetDetailValue(detailGrid, rowIndex, "TAXAMOUNT", TAXAMOUNT);
                JQDialog.dialog('close');

            }
        },
{ text: "取消", handler: function () { JQDialog.dialog('close'); } }]
    });

    uOpenDialog("JQDialogBOX");
}
