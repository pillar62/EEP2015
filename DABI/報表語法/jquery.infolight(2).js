//infolight option method
function getInfolightOption(element) {
    var option = {};
    var value = element.attr(infolightOption_attr);
    if (value != undefined) {
        var op = '';
        var options = value.split(',');
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

            var index = op.indexOf(':');
            if (index > 0) {
                var pname = op.substr(0, index).replace(/(^\s*)|(\s*$)/g, "");
                var pvalue = op.substr(index + 1);
                option[pname] = eval(pvalue);
            }

            //            var parts = op.split(':');
            //            if (parts.length == 2) {
            //                var pname = parts[0].replace(/(^\s*)|(\s*$)/g, "");
            //                var pvalue = parts[1];
            //                option[pname] = eval(pvalue);
            //            }
            op = '';
        }
    }
    return option;
}

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
}

function getParentFolder() {
    var parentFolder = $('#_PARENTFOLDER');
    return parentFolder.length ? parentFolder.val() : '';
}

function getRemoteParam(param, remoteName, tableName, includeRows, rows) {
    var remoteParam = param || {};
    remoteParam.RemoteName = remoteName;
    remoteParam.TableName = tableName;
    if (includeRows != undefined && includeRows == true) {
        remoteParam.includeRows = true;
    }
    if (rows != undefined && rows != "") {
        remoteParam.rows = rows;
    }
    return remoteParam;
}

function getDataUrl() {
    var param = [];
    if (Request.getQueryStringByName2('p')) {
        param.push('p=' + Request.getQueryStringByName2('p'));
    }
    if (Request.getQueryStringByName2('publicKey')) {
        param.push('publicKey=' + Request.getQueryStringByName2('publicKey'));
    }
    return getParentFolder() + "../handler/jqDataHandle.ashx?" + param.join('&');
}

function getRemoteUrl(remoteName, tableName, includeRows, rows) {
    var param = '';
    if (Request.getQueryStringByName2('p')) {
        param += '&p=' + Request.getQueryStringByName2('p');
    }
    if (Request.getQueryStringByName2('publicKey')) {
        param += '&publicKey=' + Request.getQueryStringByName2('publicKey');
    }

    if (includeRows != undefined && includeRows == true) {
        if (rows != undefined && rows != "")
            return getParentFolder() + "../handler/jqDataHandle.ashx?RemoteName=" + remoteName + "&TableName=" + tableName + "&IncludeRows=true&rows=" + rows + param;
        else
            return getParentFolder() + "../handler/jqDataHandle.ashx?RemoteName=" + remoteName + "&TableName=" + tableName + "&IncludeRows=true" + param;
    }
    else {
        if (rows != undefined && rows != "")
            return getParentFolder() + "../handler/jqDataHandle.ashx?RemoteName=" + remoteName + "&TableName=" + tableName + "&rows=" + rows + param;
        else
            return getParentFolder() + "../handler/jqDataHandle.ashx?RemoteName=" + remoteName + "&TableName=" + tableName + param;
    }
}

function getFormatValue(val, format) {
    //if (val.constructor == Boolean) {
    if (val == undefined || val.toString() == '') {
        return '';
    }
    var formats = format.split(/[-,]/);
    if (formats.length == 3 && formats[0] == 'L') {
        if (val.toString().toLowerCase() == 'true' || val.toString().toLowerCase() == 'y' || val.toString() == '1') {
            return formats[1];
        }
        else {
            return formats[2];
        }
    }
    else if (formats.length == 2 && formats[0] == 'L' && formats[1].toLowerCase() == "checkbox") {
        if (val.toString().toLowerCase() == 'true' || val.toString().toLowerCase() == 'y' || val.toString() == '1') {
            return "<input type='checkbox' checked='true' disabled='true' />";
        }
        else {
            return "<input type='checkbox'  disabled='true' />";
        }
    }
    else if (format[0] == 'C' || format[0] == 'N') {
        val = val.toString().replace(/,/g, '');
        var numString = val.toString().split('.');
        var newNum = '';
        var flag = numString[0][0] == '-' ? 1 : 0;
        for (var i = numString[0].length - 1; i >= flag; i--) {
            newNum = numString[0][i] + newNum;
            if (i > 0 && ((numString[0].length - i) % 3 == 0)) {
                if (i == 1 && flag == 1) {
                }
                else {
                    newNum = ',' + newNum;
                }
            }
        }
        if (flag == 1) {
            newNum = '-' + newNum;
        }

        if (numString.length == 2) {
            if (format.length >= 2) {
                var len = parseInt(format.substring(1));
                if (len > 0) {
                    newNum += '.';
                }
                for (var i = 0; i < len && i < numString[1].length; i++) {
                    newNum += numString[1][i];
                }
                for (var i = 0; i < len - numString[1].length; i++) {
                    newNum += '0';
                }
            }
            else {
                newNum += '.' + numString[1];
            }
        }
        else {
            if (format.length >= 2) {
                var len = parseInt(format.substring(1));
                if (len > 0) {
                    newNum += '.';
                }
                for (var i = 0; i < len; i++) {
                    newNum += '0';
                }
            }
        }

        if (format[0] == 'C') {
            newNum = '$' + newNum;
        }
        return newNum
    }
    // }
    else if (val.constructor == String) {
        if (format.toString().indexOf('?') >= 0) {
            var newStr = '';
            for (var i = 0; i < val.length && i < format.length; i++) {
                if (format[i] == '?') {
                    newStr += val[i];
                }
                else {
                    newStr += format[i];
                }
            }
            return newStr;
        }
        else {
            var date = $.fn.datebox('parseDate', val);
            if (date == "Invalid Date") {
                date = $.fn.datebox('parseDate', val.toString().replace(/\./g, '-').replace(/\//g, '-'));
                if (date == "Invalid Date" && val.toString().length == 8) { //8位varchar
                    var s = val.toString().substr(0, 4) + '-' + val.toString().substr(4, 2) + '-' + val.toString().substr(6, 2);
                    date = $.fn.datebox('parseDate', s);
                }
            }
            if (date == "Invalid Date") {
                return '';
            }
            else {
                var year = date.getFullYear();
                var month = (date.getMonth() + 1).toString();
                if (month.length == 1) {
                    month = '0' + month;
                }
                var day = date.getDate().toString();
                if (day.length == 1) {
                    day = '0' + day;
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

                var hour = date.getHours().toString();
                if (hour.length == 1) {
                    hour = '0' + hour;
                }
                var minute = date.getMinutes().toString();
                if (minute.length == 1) {
                    minute = '0' + minute;
                }
                var second = date.getSeconds().toString();
                if (second.length == 1) {
                    second = '0' + second;
                }

                return format.replace(/yyyy/g, year).replace(/yy/g, yy).replace(/mm/g, month).replace(/dd/g, day).replace(/YYY/g, YYY).replace(/YY/g, YY).replace(/HH/g, hour).replace(/MM/g, minute).replace(/SS/g, second);
            }
        }
    }
    //    else if (val.constructor == Number) {
    //       
    //    }
}

function getDisplayRow(val, options, whereItem) {
    if (val == '' || val == undefined) {
        return null;
    }
    var valueField = options.valueField;
    var textField = options.textField;
    var remoteName = options.remoteName;
    var tableName = options.tableName;
    var isNvarChar = false;
    if (options.columns) {
        for (var i = 0; i < options.columns.length; i++) {
            if (options.columns[i].field == valueField) {
                if (options.columns[i].table) {
                    valueField = options.columns[i].table + '.' + valueField;
                }
                isNvarChar = options.columns[i].isNvarChar;
            }
        }
    }
    var queryWord = {whereString: valueField + " = " + formatQueryValue(val.toString(), "string", isNvarChar) + ""};
    if (whereItem != undefined && whereItem != '') {
        queryWord.whereString += " and " + whereItem;
    }
    var obj = '';
    $.ajax({
        type: "POST",
        url: getDataUrl(),
        data: getRemoteParam({queryWord: $.toJSONString(queryWord)}, remoteName, tableName, false),
        cache: false,
        async: false,
        success: function (data) {
            obj = data;
        }, error: function (data) {
            data.responseText = '';
            obj = "[{\"" + textField + "\":\"\"}]";
        }
    });
    if (obj != null) {
        var rows = $.parseJSON(obj);
        if (rows.length > 0) {
            return rows[0];
        }
    }
    return null;
}

function getDisplayValue(val, options, whereItem) {
    if (val == '' || val == undefined) {
        return '';
    }
    var valueField = options.valueField;
    var textField = options.textField;
    var remoteName = options.remoteName;
    var tableName = options.tableName;

    var isNvarChar = false;
    if (options.columns) {
        for (var i = 0; i < options.columns.length; i++) {
            if (options.columns[i].field == valueField) {
                if (options.columns[i].table) {
                    valueField = options.columns[i].table + '.' + valueField;
                }
                isNvarChar = options.columns[i].isNvarChar;
            }
        }
    }

    var queryWord = {whereString: valueField + " = " + formatQueryValue(val.toString(), "string", isNvarChar) + ""};

    if (whereItem != undefined && whereItem != '') {
        queryWord.whereString += " and " + whereItem;
    }
    if (valueField != textField && val != undefined) {
        var obj = '';
        $.ajax({
            type: "POST",
            url: getDataUrl(),
            data: getRemoteParam({queryWord: $.toJSONString(queryWord)}, remoteName, tableName, false),
            cache: false,
            async: false,
            success: function (data) {
                obj = data;
            }, error: function (data) {
                data.responseText = '';
                obj = "[{\"" + textField + "\":\"\"}]";
            }
        });
        if (obj != null) {
            var rows = $.parseJSON(obj);
            if (rows.length > 0) {
                if (rows.length > 1) {
                    for (var i = 0; i < rows.length; i++) {
                        var entry = rows[i];
                        if (entry[valueField] == val) {
                            return rows[i][textField];
                        }
                    }
                }
                return rows[0][textField];
            }
        }
    }
    return val;
}

function getDisplayValues(val, options, whereItem) {
    if (val == '' || val == undefined) {
        return '';
    }
    var valueField = options.valueField;
    var textField = options.textField;
    var remoteName = options.remoteName;
    var tableName = options.tableName;

    var isNvarChar = false;
    if (options.columns) {
        for (var i = 0; i < options.columns.length; i++) {
            if (options.columns[i].field == valueField) {
                if (options.columns[i].table) {
                    valueField = options.columns[i].table + '.' + valueField;
                }
                isNvarChar = options.columns[i].isNvarChar;
            }
        }
    }


    var vals = val.toString().split(',');
    var values = [];
    for (var i = 0; i < vals.length; i++) {
        values.push(formatQueryValue(vals[i].toString(), "string", isNvarChar));
    }
    var queryWord = {whereString: valueField + " in (" + values.join(',') + ")"};
    if (whereItem != undefined && whereItem != '') {
        queryWord.whereString += " and " + whereItem;
    }
    if (valueField != textField && val != undefined) {
        var obj = '';
        $.ajax({
            type: "POST",
            url: getDataUrl(),
            data: getRemoteParam({queryWord: $.toJSONString(queryWord)}, remoteName, tableName, false),
            cache: false,
            async: false,
            success: function (data) {
                obj = data;
            }, error: function (data) {
                data.responseText = '';
                obj = "[{\"" + textField + "\":\"\"}]";
            }
        });
        if (obj != null) {
            var rows = $.parseJSON(obj);
            if (rows.length > 0) {
                var texts = [];
                for (var i = 0; i < rows.length; i++) {

                    texts.push(rows[i][textField]);

                }
                return texts.join(',');
            }
        }
    }
    return val;
}


function infogridimageformat(val, options) {
    if (val != "") {
        //return '<img SRC="data:image/jpeg;base64,' + val + '">';
        //var infolightoptions = 'imganimate:[{width:"130px",height:"130px",animatewidth:"220px",animateheight:"220px"}]';
        //var path = location.pathname;

        var height = undefined;
        if (options.height != undefined) {
            height = options.height;
        }
        var value = val;
        if (val.toLowerCase().indexOf("\\") == 0 || val.toLowerCase().indexOf("/") == 0)
            value = val.substring(1);

        //add width .use stretch.this width is column's width
        var width = options.width;
        var usewidth = false;

        var folder = "";
        var optionss = options.format.split(',');
        for (var i = 0; i < optionss.length; i++) {
            if (optionss[i].split(":").length == 2) {
                var pname = optionss[i].split(":")[0];
                var pvalue = optionss[i].split(":")[1];
                if (pname.toLowerCase() == "folder") {
                    folder = pvalue;
                    if (folder.toLowerCase().indexOf("\\") == 0 || folder.toLowerCase().indexOf("/") == 0) {
                        folder = folder.substring(1);
                    }
                    else if (folder.substring(folder.length - 1).toLowerCase() == "\\" || folder.substring(folder.length - 1).toLowerCase() == "/") {
                        folder = folder.substring(0, folder.length - 1);
                    }
                }
                if (pname.toLowerCase() == "height") {
                    height = pvalue.replace("px", "");
                }
            }
            else if (optionss[i].toLowerCase() == "stretch") {
                usewidth = true;
            }
        }
        var developer = $('#_DEVELOPERID').val();

        folder = "../" + (developer ? ('preview' + developer + '/') : '') + folder + "/";
        if (folder == "..//")
            folder = "../";
        var vpath = folder + value;
        var onmouseover = 'infogridimageformatterset(this,"' + vpath + '");';
        if ($('.info-imagecontainer').length == 0) {
            //var divHeight = ($(window).height() - 20) / 2;
            var divWidth = (document.body.clientWidth - 20) / 2;
            $('<div class="info-imagecontainer" style="display:none;width:' + divWidth + 'px;"/>').appendTo('body');
        }

        //add width string 
        var widths = "";
        if (usewidth && width != undefined) {
            widths = "width=" + width + "px";
        }
        var heights = "";
        if (height != undefined)
            heights = "height=" + height + "px ";

        //var onmouseout = 'setTimeout(function(){if($(".info-imagecontainer:first").attr("canclose") != "false") {$(".info-imagecontainer:first").fadeOut(1200);}},1000);';
        return "<input type='image' onclick='return false;' class='info-image' " + heights + widths + " src='" + vpath + "' onmouseover='" + onmouseover + "' onmouseout='infoimageonmouseover();' />";
    }
}

(function ($) {
    $.fn.extend({
        resizeImg: function () {//由于使用了parent().parent()这样很特例的方式，这个方法只适用于在datagrid中使用image的format的图片，调用方法为datagrid的onloadsuccess事件中调用$(".info-image").resizeImg();
            this.each(function () {
                $(this).load(function () {
                    $(this).parent().parent().css({
                        "width": $(this).parent().parent().width() + 1,
                        "height": $(this).parent().parent().height() + 1
                    });
                    $(this).parent().css({
                        "padding": "0px",
                        "width": $(this).parent().parent().width(),
                        "height": $(this).parent().parent().height()
                    });
                    $(this).css({"width": "0px", "height": "0px"});
                    var MaxWidth = $(this).parent().width(); //設置圖片寬度界限
                    var MaxHeight = $(this).parent().height(); //設置圖片高度界限 

                    var NaturalImage = new Image();
                    NaturalImage.src = $(this).attr("src");
                    var imgWidth = NaturalImage.width; //取得原始圖片寬度
                    var imgHeight = NaturalImage.height; //取得原始圖片高度 

                    var SetWidth = 0;
                    var SetHeight = 0;
                    if (imgWidth > MaxWidth) {
                        SetWidth = MaxWidth;
                        SetHeight = imgHeight * (MaxWidth / imgWidth);
                        imgWidth = SetWidth;
                        imgHeight = SetHeight;

                        $(this).css({"width": SetWidth, "height": SetHeight});
                        $(this).css({"margin-top": MaxHeight / 2 - SetHeight / 2});
                    }

                    if (imgHeight > MaxHeight) {
                        SetHeight = MaxHeight;
                        SetWidth = imgWidth * (MaxHeight / imgHeight);
                        /* imgHeight = SetHeight;
                         imgWidth = SetWidth;*/

                        $(this).css({"width": SetWidth, "height": SetHeight});
                        $(this).css({"display": "block", "margin": "auto"});
                    }
                });
            });
        }
    });
})(jQuery);

function infoimageonmouseover() {
    setTimeout(function () {
        var date = new Date();
        var imageContainer = $(".info-imagecontainer:first");
        if (imageContainer.attr("canclose") != "false" || date - imagesettime > 1500) {
            imageContainer.fadeOut(1200);
        }
    }, 500);
}
var imagesettime = 0;
function infogridimageformatterset(target, val) {
    if (val != "") {
        var container = $(".info-imagecontainer:first");
        if (container.length) {
            if ($(".info-image", container).length > 0) {
                $(".info-image", container).each(function () {
                    $(this).remove();
                });
            }
            //        var img = new Image();
            //        img.src = val;
            //        $(img).appendTo(window);
            //        var clientWidth = document.body.clientWidth;
            //        var imgWidth = $(img).width();
            //        if (window.width() != 0) {
            //            imgWidth = window.width();
            //            $(img).width(imgWidth);
            //        }
            //        var left = (clientWidth - imgWidth) / 2;
            //        window.css('left', left);
            //        var clientHeight = document.body.clientHeight;
            //        var imgHeight = $(img).height();
            //        if (window.height() != 0) {
            //            imgHeight = window.height();
            //            //$(img).height(imgHeight);
            //        }
            //        var top = (clientHeight - imgHeight) / 2;

            var img = $('<img class="info-image" src="' + val + '" />').appendTo(container);
            img.width(container.width());

            var left = (document.body.clientWidth - img.width()) / 2;
            var top = ($(window).height() - container.height()) / 2;
            if (top < 0) {
                top = 0;
            }
            container.css('left', left);
            container.css('top', top);
            container.css('position', 'absolute');
            container.css('z-index', '9010');
            container.css('background-color', '#FFFFFF');

            var onMouseOver = '$(".info-imagecontainer:first").attr("canclose",false);var date= new Date();imagesettime = date;';
            var onMouseOut = '$(".info-imagecontainer:first").attr("canclose",true);$(".info-imagecontainer:first").fadeOut(1200);';
            $(img).attr('onMouseOut', onMouseOut);
            $(img).attr('onMouseOver', onMouseOver);
            $(img).addClass("info-image");
            var date = new Date();
            var canshow = false;
            if (imagesettime == 0) {
                canshow = true;
            }
            else if (date - imagesettime > 1500) {
                canshow = true;
            }
            imagesettime = date;
            if (canshow) {
                container.fadeIn(1200);
            }
            else {
                container.attr("canclose", false);
            }
            container.attr("canshow", true);
        }
    }
}
//----------------------------------------------------------------------------------------------------------------
//grid method

var deleteMessage = 'Sure to Delete?';
var uploadButtonText = "Upload";

var defaultview = $.extend({}, $.fn.datagrid.defaults.view, {
    onAfterRender: function (target) {
        renderQueryAutoColumn(target);
    }
});

var commandview = $.extend({}, $.fn.datagrid.defaults.view, {
    addCommandColumn: function (target, index) {
        var opts = $.data(target, 'datagrid').options;
        if (index >= 0) {
            _add(index);
        } else {
            var length = $(target).datagrid('getRows').length;
            for (var i = 0; i < length; i++) {
                _add(i);
            }
            var commandButtons = getInfolightOption($(target)).commandButtons;
            var commandCount = 0;
            if (commandButtons == undefined || commandButtons.indexOf('v') != -1) {
                commandCount++;
            }
            if (commandButtons == undefined || commandButtons.indexOf('u') != -1) {
                commandCount++;
            }
            if (commandButtons == undefined || commandButtons.indexOf('d') != -1) {
                commandCount++;
            }
            if (commandCount != 0) {
                if (commandCount < 2)
                    commandCount = 2;
                opts.finder.getTr(target, 0, 'allfooter', 1).each(function () {

                    var totalCaption = getInfolightOption($(target)).totalCaption;
                    var s = '<td><div style="width:' + commandCount * 20 + 'px;text-align:right;">' + totalCaption + '</div></td>';
                    var tr = $(this);
                    if (tr.is(':empty')) {
                        tr.html(s);
                    } else if (tr.children('td.datagrid-td-rownumber').length) {
                        $(s).insertAfter(tr.children('td.datagrid-td-rownumber'));
                    } else {
                        $(s).insertBefore(tr.children('td:first'));
                    }
                });
            }
        }

        function _add(rowIndex) {
            var tr = opts.finder.getTr(target, rowIndex, 'body', 1);
            if (tr.find('div.datagrid-row-command').length) {
                return;
            } // the expander is already exists

            var commandButtons = getInfolightOption($(target)).commandButtons;
            var viewCommandVisible = false;
            var updateCommandVisible = false;
            var deleteCommandVisible = false;
            var commandCount = 0;
            if (commandButtons == undefined || commandButtons.indexOf('v') != -1) {
                viewCommandVisible = true;
                commandCount++;
            }
            if (commandButtons == undefined || commandButtons.indexOf('u') != -1) {
                updateCommandVisible = true;
                commandCount++;
            }
            if (commandButtons == undefined || commandButtons.indexOf('d') != -1) {
                deleteCommandVisible = true;
                commandCount++;
            }
            if (commandCount != 0) {
                if (commandCount < 2)
                    commandCount = 2;
                var cc = [];
                cc.push('<td>');
                cc.push('<div class="datagrid-row-command" style="text-align:center;width:' + commandCount * 20 + 'px;height:16px;">');
                if (viewCommandVisible) {
                    cc.push('<span class="icon-view" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
                }
                if (updateCommandVisible && !getReadOnly($(target))) {
                    cc.push('<span class="icon-edit" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
                }
                if (deleteCommandVisible && !getReadOnly($(target))) {
                    cc.push('<span class="icon-remove" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
                }
                cc.push('</div>');
                cc.push('</td>');
                if (tr.is(':empty')) {
                    tr.html(cc.join(''));
                } else if (tr.children('td.datagrid-td-rownumber').length) {
                    $(cc.join('')).insertAfter(tr.children('td.datagrid-td-rownumber'));
                } else {
                    $(cc.join('')).insertBefore(tr.children('td:first'));
                }
            }
            tr.find('span.icon-view').unbind('.datagrid').bind('click.datagrid', function () {
                var rowIndex = parseInt($(this).closest('tr').attr('datagrid-row-index'));
                var editDialog = getInfolightOption($(target)).editDialog;
                var editMode = getInfolightOption($(target)).editMode;
                var lastSelectIndex = getSelectedIndex($(target));

                if (editDialog != undefined) {
                    $(target).datagrid('selectRow', rowIndex);
                    var rowData = $(target).datagrid('getRows')[rowIndex];  //$(target).datagrid('getSelected');

                    var onView = getInfolightOption($(target)).onView;
                    if (onView) {
                        var flag = onView.call($(target), rowData);
                        if (flag != undefined && flag.toString() == 'false') {
                            return false;
                        }
                    }

                    $(target).datagrid('setCurrentRow', rowData);
                    var formname = getInfolightOption($(editDialog)).containForm;
                    var form = $(formname);
                    if (editMode.toLowerCase() == "switch") {
                        $(target).datagrid('getPanel').panel('collapse');

                        form.attr("switchGrid", "#" + $(target).attr('id'));
                        //hide submit div
                        if (getInfolightOption($(target)).CollapseDiv != undefined) {
                            $(getInfolightOption($(target)).CollapseDiv).each(function () {
                                $(this).css('display', 'none');
                            });
                        }
                        else if ($("#" + $(target).attr('id') + "-SubmitDiv") != undefined) {
                            $("#" + $(target).attr('id') + "-SubmitDiv").css('display', 'none');
                        }
                        //end
                    }
                    else if (editMode.toLowerCase() == "dialog") {
                        form.attr('dialogGrid', "#" + $(target).attr('id'));
                    }
                    else if (editMode.toLowerCase() == 'continue') {
                        form.attr('continueGrid', "#" + $(target).attr('id'));
                    }
                    openForm(editDialog, rowData, "viewed", editMode);
                }
                else if (lastSelectIndex != rowIndex) {
                    var editIndex = getEditIndex($(target));
                    if (editIndex == -1) {
                        $(target).datagrid('selectRow', rowIndex);
                        var rowData = $(target).datagrid('getRows')[rowIndex];  //$(target).datagrid('getSelected');
                        $(target).datagrid('setCurrentRow', rowData);
                    }
                }
                return false;
            });

            tr.find('span.icon-edit').unbind('.datagrid').bind('click.datagrid', function () {
                if (getReadOnly($(target))) {
                    return false;
                }

                var rowIndex = parseInt($(this).closest('tr').attr('datagrid-row-index'));
                var editDialog = getInfolightOption($(target)).editDialog;
                var editMode = getInfolightOption($(target)).editMode;
                var lastSelectIndex = getSelectedIndex($(target));
                $(target).datagrid('selectRow', rowIndex);
                var rowData = $(target).datagrid('getRows')[rowIndex];  //$(target).datagrid('getSelected');
                var onUpdate = getInfolightOption($(target)).onUpdate;
                if (onUpdate != undefined) {
                    //var rowData = $(target).datagrid('getSelected');
                    var flag = onUpdate.call($(target), rowData);
                    if (flag != undefined && flag.toString() == 'false') {
                        return false;
                    }
                }

                if (editDialog == undefined) {
                    var editIndex = getEditIndex($(target));
                    if (editIndex == -1) {
                        //record lock
                        var lock = $(target).datagrid('addLock', {type: "updating"});
                        if (!lock) {
                            return false;
                        }
                        //
                        beginEdit($(target), rowIndex);
                        if (lastSelectIndex != rowIndex) {
                            //var rowData = $(target).datagrid('getSelected');
                            $(target).datagrid('setCurrentRow', rowData);
                        }
                    }
                    else {
                        if (editIndex != rowIndex) {
                            if (endEdit($(target))) {
                                //record lock
                                var lock = $(target).datagrid('addLock', "updating");
                                if (!lock) {
                                    return false;
                                }
                                //
                                beginEdit($(target), rowIndex);
                                if (lastSelectIndex != rowIndex) {
                                    // var rowData = $(target).datagrid('getSelected');
                                    $(target).datagrid('setCurrentRow', rowData);
                                }
                            }
                            else {
                                $(target).datagrid('selectRow', editIndex);
                            }
                        }
                    }
                }
                else {
                    $(target).attr('gridUpdateIndex', rowIndex);
                    //record lock
                    //var rowData = $(target).datagrid('getSelected');
                    var lock = $(target).datagrid('addLock', {type: "updating", rowIndex: rowIndex});
                    if (!lock) {
                        return false;
                    }
                    else if (typeof lock == 'object') {
                        rowData = lock;
                    }
                    //
                    $(target).datagrid('setCurrentRow', rowData);
                    var formname = getInfolightOption($(editDialog)).containForm;
                    var form = $(formname);
                    if (editMode.toLowerCase() == "switch") {
                        $(target).datagrid('getPanel').panel('collapse');

                        form.attr("switchGrid", "#" + $(target).attr('id'));
                        //hide submit div
                        if (getInfolightOption($(target)).CollapseDiv != undefined) {
                            $(getInfolightOption($(target)).CollapseDiv).each(function () {
                                $(this).css('display', 'none');
                            });
                        }
                        else if ($("#" + $(target).attr('id') + "-SubmitDiv") != undefined) {
                            $("#" + $(target).attr('id') + "-SubmitDiv").css('display', 'none');
                        }
                        //end
                    }
                    else if (editMode.toLowerCase() == "dialog") {
                        form.attr('dialogGrid', "#" + $(target).attr('id'));
                    }
                    else if (editMode.toLowerCase() == 'continue') {
                        var dataFormTabID = getInfolightOption($(editDialog)).dataFormTabID;
                        if (dataFormTabID != undefined) {
                            var tabs = $(editDialog).closest('.easyui-tabs');
                            if (tabs != undefined) {
                                var tabslist = $(tabs).tabs('tabs');
                                for (var i = 0; i < tabslist.length; i++) {
                                    if ($(tabslist[i]).attr('id') == dataFormTabID) {
                                        $(tabs).tabs('select', i);
                                    }
                                }
                            }
                        }
                        form.attr('continueGrid', "#" + $(target).attr('id'));
                    }
                    openForm(editDialog, rowData, "updated", editMode, $(target).attr('keyColumns'));
                }
                return false;
            });

            tr.find('span.icon-remove').unbind('.datagrid').bind('click.datagrid', function () {
                if (getReadOnly($(target))) {
                    return false;
                }

                var rowIndex = parseInt($(this).closest('tr').attr('datagrid-row-index'));
                var editDialog = getInfolightOption($(target)).editDialog;
                if (editDialog == undefined) {
                    var editIndex = getEditIndex($(target));
                    if (editIndex == -1) {
                    }
                    else {
                        if (editIndex != rowIndex) {
                            if (endEdit($(target))) {
                            }
                            else {
                                $(target).datagrid('selectRow', editIndex);
                                return false;
                            }
                        }
                    }
                }

                $(target).datagrid('selectRow', rowIndex);
                var rowData = $(target).datagrid('getRows')[rowIndex];  //$(target).datagrid('getSelected');
                var onDelete = getInfolightOption($(target)).onDelete;
                if (onDelete != undefined) {
                    //var rowData = $(target).datagrid('getSelected');
                    var flag = onDelete.call($(target), rowData);
                    if (flag != undefined && flag.toString() == 'false') {
                        return false;
                    }
                }

                if (confirm(deleteMessage)) {
                    //var rowData = $(target).datagrid('getSelected');
                    //record lock
                    var lock = $(target).datagrid('addLock', {type: "deleting", rowIndex: rowIndex});
                    if (!lock) {
                        return false;
                    }
                    //

                    $(target).datagrid('deleteRow', rowIndex);
                    var onDeleting = getInfolightOption($(target)).onDeleting;
                    if (onDeleting) {
                        onDeleting.call($(target), rowData);
                    }

                    var autoApply = getInfolightOption($(target)).autoApply;
                    if (autoApply) {
                        $.fn.Error.errorCode = 1006;

                        applyUpdates($(target));
                    }
                    else {
                        $(target).datagrid('changeState', 'editing');
                    }
                    if ($(target).datagrid('options').showFooter == true && $(target).datagrid('options').pagination == false) {
                        setFooter($(target));
                    }
                    $(target).datagrid('setCurrentRow', null);
                    $(dataGrid_class).each(function () {
                        var multiSelectGrid = getInfolightOption($(this)).multiSelectGrid;
                        if (multiSelectGrid != undefined && multiSelectGrid == '#' + $(target).attr('id')) {
                            $(this).datagrid('reload');
                        }
                    });
                }
                return false;
            });
        }
    },
    render: function (target, container, frozen) {
        var state = $.data(target, 'datagrid');
        var opts = state.options;
        //debugger;
        var rows = state.data.rows;
        var fields = $(target).datagrid('getColumnFields', frozen);
        var table = [];
        for (var i = 0; i < rows.length; i++) {
            table.push('<table class="datagrid-btable" cellspacing="0" cellpadding="0" border="0"><tbody>');

            // get the class and style attributes for this row
            var cls = (i % 2 && opts.striped) ? 'class="datagrid-row datagrid-row-alt"' : 'class="datagrid-row"';
            var styleValue = opts.rowStyler ? opts.rowStyler.call(target, i, rows[i]) : '';
            var style = styleValue ? 'style="' + styleValue + '"' : '';
            var rowId = state.rowIdPrefix + '-' + (frozen ? 1 : 2) + '-' + i;
            table.push('<tr id="' + rowId + '" datagrid-row-index="' + i + '" ' + cls + ' ' + style + '>');
            table.push(this.renderRow.call(this, target, fields, frozen, i, rows[i]));
            table.push('</tr>');

            table.push('<tr style="display:none;">');
            if (frozen) {
                table.push('<td colspan=' + (fields.length + 2) + ' style="border-right:0">');
            } else {
                table.push('<td colspan=' + (fields.length) + '>');
            }

            table.push('</td>');
            table.push('</tr>');

            table.push('</tbody></table>');
        }

        $(container).html(table.join(''));
    },
    deleteRow: function (target, index) {
        var opts = $.data(target, "datagrid").options;
        //var data = $.data(target, "datagrid").data;
        opts.finder.getTr(target, index).remove();
        $.fn.datagrid.defaults.view.deleteRow.call(this, target, index);
    },
    updateRow: function (target, rowIndex, row) {
        //var dc = $.data(target, 'datagrid').dc;
        //var opts = $.data(target, 'datagrid').options;
        $.fn.datagrid.defaults.view.updateRow.call(this, target, rowIndex, row);
        this.addCommandColumn.call(this, target, rowIndex);
    },
    onBeforeRender: function (target) {
        var opts = $.data(target, 'datagrid').options;
        var dc = $.data(target, 'datagrid').dc;
        //var panel = $(target).datagrid('getPanel');

        var t = dc.view1.children('div.datagrid-header').find('table');
        if (t.find('div.datagrid-header-expander').length) {
            return;
        }

        var commandButtons = getInfolightOption($(target)).commandButtons;
        var commandCount = 0;
        if (commandButtons == undefined || commandButtons.indexOf('v') != -1) {
            commandCount++;
        }
        if (commandButtons == undefined || commandButtons.indexOf('u') != -1) {
            commandCount++;
        }
        if (commandButtons == undefined || commandButtons.indexOf('d') != -1) {
            commandCount++;
        }
        if (commandCount < 2 && commandCount != 0)
            commandCount = 2;
        var tdstring = '<td rowspan="' + opts.frozenColumns.length + '">';
        if (commandCount != 0) {
            tdstring += '<div class="datagrid-header-expander" style="width:' + commandCount * 20 + 'px;"></div></td>';
            var td = $(tdstring);
            if ($('tr', t).length == 0) {
                td.wrap('<tr></tr>').parent().appendTo($('tbody', t));
            } else if (opts.rownumbers) {
                td.insertAfter(t.find('td:has(div.datagrid-header-rownumber)'));
            } else {
                td.prependTo(t.find('tr:first'));
            }
        }

    },
    onAfterRender: function (target) {
        var state = $.data(target, 'datagrid');
        var dc = state.dc;
        var opts = state.options;
        var panel = $(target).datagrid('getPanel');

        $.fn.datagrid.defaults.view.onAfterRender.call(this, target);

        if (!state.onResizeColumn) {
            state.onResizeColumn = opts.onResizeColumn;
        }
        if (!state.onResize) {
            state.onResize = opts.onResize;
        }
        function setBodyTableWidth() {
            var columnWidths = dc.view2.children('div.datagrid-header').find('table').width();
            dc.body2.children('table').width(columnWidths);
        }

        opts.onResizeColumn = function (field, width) {
            setBodyTableWidth();
            // call the old event code
            state.onResizeColumn.call(target, field, width);
        };
        opts.onResize = function (width, height) {
            setBodyTableWidth();
            state.onResize.call(panel, width, height);
        };

        this.addCommandColumn(target);
        $(target).datagrid('resize');

        renderQueryAutoColumn(target);
        //dc.footer1.find('span.datagrid-row-expander').css('visibility', 'hidden');
    }
});


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
    },
    fixtext: {
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
            input.keyup(function (sender) {
                if (options.maxLength != undefined && options.maxLength > 0) {
                    var target = $(sender.target);
                    if (target.val().length == options.maxLength) {
                        var inputs = $('input,textarea', '.datagrid-row-editing');
                        var idx = inputs.index(target);
                        if (idx == inputs.length - 1) {
                            //inputs[0].focus();
                            //inputs[0].select();
                        }
                        else {
                            if (inputs[idx + 1].className == "info-refval refval-f") idx++;
                            inputs[idx + 1].focus();
                            //inputs[idx + 1].select();
                        }
                    }
                }
            });
            if (options && options.placeholder) {
                input.attr('placeholder', options.placeholder);
            }
            if (options.capsLock) {
                $.changeCapsLock(input, options.capsLock);

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
    },
    validatebox: {
        init: function (container, options) {
            var input = $("<input type=\"text\" class=\"datagrid-editable-input\">").appendTo(container);
            input.validatebox(options);
            if (options && options.maxLength) {
                input.attr('maxlength', options.maxLength);
            }
            if (options && options.placeholder) {
                input.attr('placeholder', options.placeholder);
            }
            if (options.capsLock) {
                $.changeCapsLock(input, options.capsLock);

            }
            return input;
        },
        destroy: function (target) {
            $(target).validatebox("destroy");
        },
        getValue: function (target) {
            return $(target).val();
        },
        setValue: function (target, value) {
            $(target).val(value);
        },
        resize: function (target, width) {
            $(target)._outerWidth(width);
        }
    },
    checkbox: {
        init: function (container, options) {
            var input = $("<input type=\"checkbox\">").appendTo(container);
            if (options.disabled != undefined && options.disabled == true) {
                input.attr('disabled', true);
            }
            input.val(options.on);
            input.attr("offval", options.off);
            return input;
        },
        getValue: function (target) {
            if ($(target).is(":checked")) {
                return $(target).val();
            } else {
                return $(target).attr("offval");
            }
        },
        setValue: function (target, value) {
            var checked = false;
            if ($(target).val() == value) {
                checked = true;
            }
            $(target)._propAttr("checked", checked);
        }
    },
    datebox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            if (options.showTimeSpinner == true) {
                $(input).datetimebox({
                    showSeconds: options.showSeconds,
                    required: options.required,
                    disabled: options.disabled,
                    editable: options.editable
                });
            }
            else {
                $(input).datebox({
                    required: options.required,
                    disabled: options.disabled,
                    validType: options.validType,
                    editable: options.editable
                });
                $(input).datebox('dateFormat', options.dateFormat);
            }
            return input;
        },
        getValue: function (target) {
            //datetime
            if ($(target).hasClass('datetimebox-f') == true) {
                return $(target).datetimebox('getBindingValue');
            }
            //nvarchar
            return $(target).datebox('getBindingValue');
        },
        setValue: function (target, value) {
            if ($(target).hasClass('datetimebox-f') == true) {
                $(target).datetimebox('setValue', value);
            }
            else {
                //nvarchar
                var dateFormat = $(target).datebox('dateFormat');
                if (dateFormat == 'nvarchar') {
                    if (value != undefined && value.toString().length == 8) {
                        var s = value.toString().substr(0, 4) + '-' + value.toString().substr(4, 2) + '-' + value.toString().substr(6, 2);
                        var date = $.fn.datebox('parseDate', s);
                        var text = $.fn.datebox.defaults.formatter.call($(target), date);
                        $(target).datebox('setValue', text);
                    }
                }
                else {
                    if (value) {
                        var date = $.fn.datebox('parseDate', value.toString().replace(/\./g, '-').replace(/\//g, '-'));
                        if (date != "Invalid Date") {
                            value = $.fn.datebox.defaults.formatter.call($(target), date);
                        }
                    }
                    $(target).datebox('setValue', value);
                }
            }
        },
        resize: function (target, width) {
            $(target).combo('resize', width)
        }
    },
    timespinner: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            $(input).timespinner({
                showSeconds: options.showSeconds,
                required: options.required
            });
            return input;
        },
        getValue: function (target) {
            return $(target).timespinner('getValue');
        },
        setValue: function (target, value) {
            $(target).timespinner('setValue', value);
        },
        resize: function (target, width) {
            $(target).timespinner('resize', width)
        }
    },
    infocombobox: {
        init: function (container, options) {
            var combobox = $('<input class="info-combobox">').appendTo(container);

            var valueField = options.valueField;
            var textField = options.textField;

            var remoteName = options.remoteName;
            var tableName = options.tableName;
            var onSelect = options.onSelect;
            var disabled = options.disabled;
            var panelHeight = options.panelHeight;
            var rows = options.pageSize;
            var selectOnly = options.selectOnly;
            var onBeforeLoad = options.onBeforeLoad;
            if (onBeforeLoad) {
                $(combobox).attr(infolightOption_attr, "onBeforeLoad:" + getClassName(onBeforeLoad));
            }

            if (rows == undefined)
                rows = -1;
            if (remoteName != undefined && remoteName != "") {
                $(combobox).combobox({
                    url: getDataUrl(),
                    //queryParams: getRemoteParam({}, remoteName, tableName, false, rows),
                    valueField: valueField,
                    textField: textField,
                    mode: 'remote',
                    required: options.required,
                    validType: options.validType,
                    disabled: disabled,
                    editable: !selectOnly,
                    panelHeight: panelHeight,
                    onBeforeLoad: function (param) {
                        param.RemoteName = remoteName;
                        param.TableName = tableName;
                        param.IncludeRows = false;
                        param.rows = rows;
                        if (comboBeforeLoad) {
                            comboBeforeLoad.call(this, param);
                        }
                    },
                    onSelect: function (rowdata) {
                        if (onSelect != undefined) {
                            onSelect.call(this, rowdata);
                        }
                    },
                    onShowPanel: function () {
                        if (selectOnly == true) {
                            //                            var panel = $(this).combobox('panel');
                            //                            if ($(".combobox-empty-item", panel).length == 0) {
                            //                                var item = $("<div class=\"combobox-item combobox-empty-item\"></div>").prependTo(panel);
                            //                                item.attr("value", "");
                            //                                item.html($.fn.combobox.defaults.emptyText);
                            //                                item.hover(function () {
                            //                                    $(this).addClass("combobox-item-hover");
                            //                                }, function () {
                            //                                    $(this).removeClass("combobox-item-hover");
                            //                                });
                            //                                item.click(function () {
                            //                                    $(combobox).combobox('setValue', '');
                            //                                    $(combobox).combobox('hidePanel');
                            //                                });
                            //                            }
                        }
                    }
                }); //初始化
            }
            else {
                var items = options.items;
                combobox.remove();
                combobox = $('<select class="info-combobox">').appendTo(container);
                for (var i = 0; i < items.length; i++) {
                    $('<option value=' + items[i].value + ' >' + items[i].text + '</option>').appendTo(combobox);
                }
                $(combobox).combobox({
                    onBeforeLoad: comboBeforeLoad,
                    required: options.required,
                    disabled: disabled,
                    validType: options.validType,
                    editable: !selectOnly,
                    panelHeight: panelHeight,
                    onSelect: function (rowdata) {
                        if (onSelect != undefined) {
                            onSelect.call(this, rowdata);
                        }
                    },
                    onShowPanel: function () {
                        if (selectOnly == true) {
                            //                            var panel = $(this).combobox('panel');
                            //                            if ($(".combobox-empty-item", panel).length == 0) {
                            //                                var item = $("<div class=\"combobox-item combobox-empty-item\"></div>").prependTo(panel);
                            //                                item.attr("value", "");
                            //                                item.html($.fn.combobox.defaults.emptyText);
                            //                                item.hover(function () {
                            //                                    $(this).addClass("combobox-item-hover");
                            //                                }, function () {
                            //                                    $(this).removeClass("combobox-item-hover");
                            //                                });
                            //                                item.click(function () {
                            //                                    $(combobox).combobox('setValue', '');
                            //                                    $(combobox).combobox('hidePanel');
                            //                                });
                            //                            }
                        }
                    }
                });      //初始化
            }
            return combobox;
        },
        getValue: function (target) {
            return $(target).combobox('getValue');
        },
        setValue: function (target, value) {
            $(target).combobox('setValue', value);
        },
        resize: function (target, width) {
            $(target).combo('resize', width)
        }
    },
    infocombogrid: {
        init: function (container, options) {
            var combogrid = $('<input class="info-combogrid">').appendTo(container);
            //var form = options.form;
            var valueField = options.valueField;
            var textField = options.textField;
            var valueFieldCaption = options.valueFieldCaption;
            var textFieldCaption = options.textFieldCaption;
            //var PageSize = options.PageSize;
            var panelWidth = options.panelWidth;
            var panelHeight = options.panelHeight;
            var remoteName = options.remoteName;
            var tableName = options.tableName;
            var disabled = options.disabled;
            var columns = options.columns;
            var selectOnly = options.selectOnly;
            var onBeforeLoad = options.onBeforeLoad;
            var multiple = options.multiple;

            if (columns == null || columns.length == 0) {
                columns = [{
                    field: valueField, title: valueFieldCaption, align: 'left', width: 150
                }];
                if (valueField != textField) {
                    var textColumn = {
                        field: textField, title: textFieldCaption, align: 'left', width: 150
                    };
                    columns.push(textColumn);
                }
            }
            if (multiple) {
                columns.unshift({field: 'ck', checkbox: true});
            }

            for (var i = 0; i < columns.length; i++) {
                if (columns[i].width == undefined) {
                    columns[i].width = panelWidth / columns.length;
                }
            }
            var onSelect = options.onSelect;
            $(combogrid).combogrid({
                url: getDataUrl(),
                onBeforeLoad: function () {
                    param.RemoteName = remoteName;
                    param.TableName = tableName;
                    param.IncludeRows = true;
                    if (onBeforeLoad) {
                        onBeforeLoad.call(this, param);
                    }
                },
                //queryParams: getRemoteParam({}, remoteName, tableName, true),
                panelWidth: panelWidth,
                panelHeight: panelHeight,
                pagination: true,
                editable: !selectOnly,
                required: options.required,
                validType: options.validType,
                keyHandler: {
                    up: function () {
                    },
                    down: function () {
                    },
                    enter: function () {
                    },
                    query: combogridQuery
                },
                fitColumns: false,
                idField: valueField,
                textField: textField,
                disabled: disabled,
                columns: [columns],
                multiple: multiple,
                onClickRow: function (rowindex, rowdata) {
                    if (onSelect != undefined) {
                        onSelect.call(combogrid, rowdata);
                    }
                }
            }); //初始化
            return combogrid;
        },
        getValue: function (target) {
            return $(target).combogrid('getValue');
        },
        setValue: function (target, value) {
            $(target).combogrid('setValue', value);
        },
        resize: function (target, width) {
            $(target).combo('resize', width)
        }
    },
    inforefval: {
        init: function (container, options) {
            var refval = $('<input class="info-refval">').appendTo(container);
            //var columnstring = "";
            var columns = options.columns;
            if (columns == null || columns.length == 0) {
                var valueField = options.valueField;
                var textField = options.textField;
                var valueFieldCaption = options.valueFieldCaption;
                var textFieldCaption = options.textFieldCaption;
                columns = [{
                    field: valueField, title: valueFieldCaption, align: 'left', width: 150
                }];
                if (valueField != textField) {
                    var textColumn = {
                        field: textField, title: textFieldCaption, align: 'left', width: 150
                    };
                    columns.push(textColumn);
                }
            }
            //for (var i = 0; i < columns.length; i++) {
            //    if (columns[i].width == undefined) {
            //        columns[i].width = PanelWidth / columns.length;
            //    }
            //    if (columnstring != "")
            //        columnstring += ",";
            //    columnstring += "{field:'" + columns[i].field + "',title:'" + columns[i].title + "',width:" + columns[i].width + "}";
            //}
            var optionsstring = "field:'" + options.field + "',title:'" + options.title + "',panelWidth:" + options.panelWidth + ",tableName:'" + options.tableName + "',remoteName:'" + options.remoteName + "',columns:" + $.toJSONString(columns) + ",valueField:'" + options.valueField + "',textField:'" + options.textField + "'";
            if (options.whereItems != undefined) {
                var whereItems = [];
                for (var i = 0; i < options.whereItems.length; i++) {
                    whereItems.push("{field:'" + options.whereItems[i].field + "',value:'" + options.whereItems[i].value + "'}");
                }
                optionsstring += ",whereItems:[" + whereItems.join(',') + "]";
            }
            if (options.onSelect != undefined) {
                optionsstring += ",onSelect:" + getClassName(options.onSelect);
            }
            if (options.disabled != undefined) {
                optionsstring += ",disabled:" + options.disabled;
            }
            if (options.selectOnly != undefined) {
                optionsstring += ",selectOnly:" + options.selectOnly;
            }
            if (options.required != undefined) {
                optionsstring += ",required:'required'";
            }
            optionsstring += ",columnMatches:['']";
            if (options.checkData) {
                optionsstring += ",checkData:true";
            }
            if (options.maxLength) {
                $(refval).attr('maxLength', options.maxLength);
            }
            if (options.placeholder) {
                optionsstring += ",placeholder:'" + options.placeholder + "'";
            }
            if (options.capsLock) {
                optionsstring += ",capsLock:'" + options.capsLock + "'";
            }
            if (options.fixTextbox) {
                optionsstring += ",fixTextbox:'" + options.fixTextbox + "'";
            }
            $(refval).attr(infolightOption_attr, optionsstring);
            initInfoRefVal(refval[0]);
            return refval;
        },
        getValue: function (target) {
            return $(target).refval('getValue');
            //return $.data(target[0], "inforefval").refval.find("input.refval-text").val();
        },
        setValue: function (target, value) {
            $(target).refval('setValue', value);
            //$.data(target[0], "inforefval").refval.find("input.refval-text").val(value);
        },
        resize: function (target, width) {
            $.data(target[0], "inforefval").refval.find("input.refval-text").width(width - 30);
        }
    },
    yearMonth: {
        init: function (container, options) {
            var yearmonth = $('<input class="info-yearmonth">').appendTo(container);
            var type = options.type;
            var durationMinus = options.durationMinus;
            var durationPlus = options.durationPlus;
            var format = options.format;
            var datatype = options.datatype;
            var onSelect = options.onSelect;
            var selectOnly = options.selectOnly;
            var panelHeight = options.panelHeight;
            var optionsstring = "type:'" + type + "'";
            if (durationMinus != undefined) {
                optionsstring += ",durationMinus:" + durationMinus;
            }
            if (durationPlus != undefined) {
                optionsstring += ",durationPlus:" + durationPlus;
            }
            if (format != undefined) {
                optionsstring += ",format:'" + format + "'";
            }
            if (datatype != undefined) {
                optionsstring += ",datatype:'" + datatype + "'";
            }
            if (onSelect != undefined) {
                optionsstring += ",onSelect:" + getClassName(onSelect);
            }
            if (selectOnly != undefined) {
                optionsstring += ",selectOnly:" + selectOnly;
            }
            if (panelHeight != undefined) {
                if (panelHeight == "auto") {
                    optionsstring += ",panelHeight:'" + panelHeight + "'";
                } else
                    optionsstring += ",panelHeight:" + panelHeight;
            }
            $(yearmonth).attr(infolightOption_attr, optionsstring);
            initInfoYearMonth(yearmonth[0]);
            return yearmonth;
        },
        getValue: function (target) {
            return $(target).yearmonth('getValue');
        },
        setValue: function (target, value) {
            $(target).yearmonth('setValue', value);
        },
        resize: function (target, width) {
            $(target).combo('resize', width)
        }
    },
    infoautocomplete: {
        init: function (container, options) {
            var combobox = $('<input >').appendTo(container);
            var valueField = options.valueField;
            var remoteName = options.remoteName;
            var tableName = options.tableName;
            var url = getRemoteUrl(remoteName, tableName);
            var target = combobox;
            $(combobox).combobox({
                valueField: valueField,
                textField: valueField,
                hasDownArrow: false,
                mode: 'remote',
                keyHandler: {
                    up: function () {
                        var panel = $(this).combo('panel');
                        var values = $(this).combo('getValues');
                        var item = panel.find('div.combobox-item[value="' + values.pop() + '"]');
                        if (item.length) {
                            var prev = item.prev(':visible');
                            //var prev = item.prev();
                            if (prev.length) {
                                item = prev;
                            }
                        } else {
                            item = panel.find('div.combobox-item:visible:last');
                        }
                        var value = item.attr('value');

                        var opts = $.data(this, 'combobox').options;
                        var data = $.data(this, 'combobox').data;

                        panel.find('div.combobox-item-selected').removeClass('combobox-item-selected');
                        var vv = [], ss = [];
                        for (var i = 0; i < [value].length; i++) {
                            var v = [value][i];
                            var s = v;
                            for (var j = 0; j < data.length; j++) {
                                if (data[j][opts.valueField] == v) {
                                    s = data[j][opts.textField];
                                    break;
                                }
                            }
                            vv.push(v);
                            ss.push(s);
                            panel.find('div.combobox-item[value="' + v + '"]').addClass('combobox-item-selected');
                        }

                        $(this).combo('setValues', vv);

                        for (var i = 0; i < data.length; i++) {
                            if (data[i][opts.valueField] == value) {
                                opts.onSelect.call(target, data[i]);
                                return;
                            }
                        }
                        scrollTo(this, value);
                    },
                    down: function () {
                        var panel = $(this).combo('panel');
                        var values = $(this).combo('getValues');
                        var item = panel.find('div.combobox-item[value="' + values.pop() + '"]');
                        if (item.length) {
                            var next = item.next(':visible');
                            //var next = item.next();
                            if (next.length) {
                                item = next;
                            }
                        } else {
                            item = panel.find('div.combobox-item:visible:first');
                        }
                        var value = item.attr('value');

                        var opts = $.data(this, 'combobox').options;
                        var data = $.data(this, 'combobox').data;

                        panel.find('div.combobox-item-selected').removeClass('combobox-item-selected');
                        var vv = [], ss = [];
                        for (var i = 0; i < [value].length; i++) {
                            var v = [value][i];
                            var s = v;
                            for (var j = 0; j < data.length; j++) {
                                if (data[j][opts.valueField] == v) {
                                    s = data[j][opts.textField];
                                    break;
                                }
                            }
                            vv.push(v);
                            ss.push(s);
                            panel.find('div.combobox-item[value="' + v + '"]').addClass('combobox-item-selected');
                        }

                        $(this).combo('setValues', vv);

                        for (var i = 0; i < data.length; i++) {
                            if (data[i][opts.valueField] == value) {
                                opts.onSelect.call(target, data[i]);
                                return;
                            }
                        }

                        scrollTo(this, value);
                    },
                    enter: function () {
                        var values = $(this).combobox('getValues');
                        $(this).combobox('setValues', values);
                        $(this).combobox('hidePanel');
                    },
                    query: function (q) {
                        if (q != null && q != "") {
                            var nurl = url + "&whereString=" + valueField + " like '" + encodeURIComponent(q.toString().replace(/\'/g, "''")) + "%'";
                            $(target).combobox("reload", nurl);
                        }
                        $(target).combobox("setValue", q);
                    }
                }
            });
            return combobox;
        },
        getValue: function (target) {
            return $(target).combobox('getValue');
        },
        setValue: function (target, value) {
            $(target).combobox('setValue', value);
        },
        resize: function (target, width) {
            $(target).combo('resize', width)
        }
    },
    textarea: {
        init: function (container, options) {
            var maxLength = "";
            var disabled = "";
            var height = "";
            if (options != undefined) {
                if (options.maxLength != undefined && options.maxLength > 0)
                    maxLength = 'maxlength="' + options.maxLength + '"';
                if (options.disabled != undefined && options.disabled == true) {
                    disabled = 'disabled="disabled"';
                }
                if (options.height != undefined && options.height > 0) {
                    height = 'style="height:' + options.height + 'px"';
                }
            }
            return $('<textarea ' + maxLength + disabled + height + ' >').appendTo(container);
        },
        getValue: function (target) {
            return $(target).val();
        },
        setValue: function (target, value) {
            $(target).val(value);
        },
        resize: function (target, width) {
            $(target)._outerWidth(width);
        }
    }
});

$.extend($.fn.datagrid.defaults, {
    lockMsg: ' is modifying data, please wait ...',
    lockDeletedMsg: 'Data is deleted, please refresh ...'
});

$.extend($.fn.datagrid.methods, {
    getWhere: function (jq) {
        var where = '';
        var pnid = getInfolightOption($(jq[0])).queryDialog;
        if (pnid != undefined) {
            $("input,select", pnid).each(function () {
                var text = $(this);
                var value = text.val();
                if ($(this).closest('tr.fuzzy').length == 0) {
                    $("input", pnid).each(function () {
                        if ($(this).closest('tr.fuzzy').length > 0) {
                            $(this).val(value);
                        }
                    });
                }
                var controlClass = $(this).attr('class');
                if (controlClass != undefined) {
                    if (controlClass.indexOf('easyui-datebox') == 0) {
                        value = text.datebox('getBindingValue');
                        if (text.datebox('dateFormat') != 'nvarchar' && (getInfolightOption(text).condition == "<=" || getInfolightOption(text).condition == "<")) {
                            if (value) {
                                value = value + " 23:59:59";
                            }
                        }
                    }
                    else if (controlClass.indexOf('easyui-datetimebox') == 0) {
                        value = text.datetimebox('getBindingValue');
                    }
                    else if (controlClass.indexOf('info-combobox') == 0
                        || controlClass.indexOf('info-combogrid') == 0) {
                        value = text.combobox('getValue');
                    }
                    else if (controlClass.indexOf('combo-text') == 0) {
                        value = '';
                    }
                    else if (controlClass.indexOf('info-refval') == 0) {
                        value = text.refval('getValue');
                    }
                    else if (controlClass.indexOf('info-autocomplete') == 0) {
                        value = text.combobox('getValue');
                    }
                    else if (controlClass.indexOf('info-options') == 0) {
                        value = $(this).options('getValue');
                    }
                    else if (controlClass.indexOf('info-yearmonth') == 0) {
                        value = $(this).yearmonth('getValue');
                    }
                }
                else {
                    if ($(this).attr('type') == "checkbox") {
                        value = $(this).checkbox('getValue');
                    }
                }
                if (value != undefined && value != '') {
                    var fieldName = getInfolightOption(text).field;
                    if (fieldName != undefined) {
                        var tableName = getInfolightOption(text).table;
                        if (tableName != undefined) {
                            fieldName = tableName + "." + fieldName;
                        }
                        var condition = getInfolightOption(text).condition;
                        var dataType = getInfolightOption(text).dataType;
                        var isNvarChar = getInfolightOption(text).isNvarChar;
                        var nvarchar = '';
                        if (isNvarChar) {
                            nvarchar = 'N';
                        }
                        if (where != "") {
                            var andOr = getInfolightOption(text).andOr;
                            if (andOr == undefined || andOr == "")
                                andOr = "and";
                            where += " " + andOr + " ";
                        }
                        if (controlClass != undefined && controlClass.indexOf('info-options') == 0 && getInfolightOption($(this)).multiSelect) {
                            var values = value.split(',');
                            where += "(";
                            var optionsstring = "";
                            for (i = 0; i < values.length; i++) {
                                if (optionsstring != "") optionsstring += " or ";
                                switch (condition) {
                                    case '=':
                                    case '!=':
                                    case '>=':
                                    case '<=':
                                    case '>':
                                    case '<':
                                        optionsstring += fieldName + condition + formatQueryValue(values[i], dataType, isNvarChar);
                                        break;
                                    case '%':
                                        optionsstring += fieldName + " like " + nvarchar + "'" + values[i].toString().replace(/\'/g, "''") + "%'";
                                        break;
                                    case '%%':
                                        optionsstring += fieldName + " like " + nvarchar + "'%" + values[i].toString().replace(/\'/g, "''") + "%'";
                                        break;
                                    case 'in':
                                        var vs = values[i].toString().split(",");
                                        var value = "";
                                        for (var j = 0; j < vs.length; j++) {
                                            if (vs[j] != "")
                                                value += "'" + vs[j] + "',";
                                        }
                                        value = value.substring(0, value.length - 1);
                                        optionsstring += fieldName + " in (" + value + ")";
                                        break;
                                    default:
                                }
                            }
                            where += optionsstring;
                            where += ")";
                        }
                        else if (controlClass != undefined && controlClass.indexOf('info-yearmonth') == 0) {
                            switch (condition) {
                                case '=':
                                case '!=':
                                    var type = getInfolightOption(text).type;
                                    var datatype = getInfolightOption(text).datatype;
                                    if (datatype == 'varchar') {
                                        where += fieldName + condition + formatQueryValue(value, dataType, isNvarChar);
                                    }
                                    else if (datatype == 'datetime') {
                                        var year = new Date(value).getFullYear();
                                        var month = new Date(value).getMonth() + 1;
                                        if (type == 'year') {
                                            where += "Year(" + fieldName + ")" + condition + value;
                                        }
                                        else if (type == "month") {
                                            where += "Year(" + fieldName + ")" + condition + year + " AND Month(" + fieldName + ")" + condition + month;
                                        }
                                    }
                                    break;
                                case '%':
                                case '%%':
                                    var type = getInfolightOption(text).type;
                                    var datatype = getInfolightOption(text).datatype;
                                    if (datatype == 'varchar') {
                                        where += fieldName + "=" + formatQueryValue(value, dataType, isNvarChar);
                                    }
                                    else if (datatype == 'datetime') {
                                        var year = new Date(value).getFullYear();
                                        var month = new Date(value).getMonth() + 1;
                                        if (type == 'year') {
                                            where += "Year(" + fieldName + ")=" + year;
                                        }
                                        else if (type == "month") {
                                            where += "Year(" + fieldName + ")=" + +year + " AND Month(" + fieldName + ")=" + month;
                                        }
                                    }
                                    break;
                                case 'in':
                                    //如果以后yearmonth要做多选，这里就要处理了，参照上面区分类型的方法处理，同时要看具体规格
                                    //var vs = value.toString().split(",");
                                    //var value = "";
                                    //for (var j = 0; j < vs.length; j++) {
                                    //    if (vs[j] != "")
                                    //        value += "'" + vs[j] + "',";
                                    //}
                                    //value = value.substring(0, value.length - 1);
                                    //where += fieldName + " in (" + value + ")";
                                    break;
                                case '>=':
                                case '<=':
                                case '>':
                                case '<':
                                    where += fieldName + condition + formatQueryValue(value, dataType, isNvarChar);
                                    break;
                                default:
                            }

                        }
                        else {
                            switch (condition) {
                                case '=':
                                case '!=':
                                case '>=':
                                case '<=':
                                case '>':
                                case '<':
                                    where += fieldName + condition + formatQueryValue(value, dataType, isNvarChar);
                                    break;
                                case '%':
                                    where += fieldName + " like " + nvarchar + "'" + value.toString().replace(/\'/g, "''") + "%'";
                                    break;
                                case '%%':
                                    where += fieldName + " like " + nvarchar + "'%" + value.toString().replace(/\'/g, "''") + "%'";
                                    break;
                                case 'in':
                                    var vs = value.toString().split(",");
                                    var value = "";
                                    for (var j = 0; j < vs.length; j++) {
                                        if (vs[j] != "")
                                            value += "'" + vs[j] + "',";
                                    }
                                    value = value.substring(0, value.length - 1);
                                    where += fieldName + " in (" + value + ")";
                                    break;
                                default:
                            }
                        }
                    }
                }
            });
            if ($(jq[0]).attr('class') == 'refval-grid') {

            }
            else {
                var flowKey = Request.getQueryStringByName("FORM_PRESENTATION");
                var FLNAVMODE = Request.getQueryStringByName("FLNAVMODE");
                if (FLNAVMODE == "") FLNAVMODE = Request.getFlowStringByName(flowKey, "FLNAVMODE");
                if (FLNAVMODE != undefined && FLNAVMODE != null && FLNAVMODE == "Submit") {
                    if (where != "")
                        where += " and ";
                    var tableName = Request.getFlowStringByName(flowKey, "FORM_TABLE");
                    var field = 'FlowFlag';
                    if (tableName) {
                        field = tableName + '.FlowFlag';
                    }
                    where += "(" + field + " is Null OR " + field + " = '')";
                }
            }
        }
        return where;
    },
    getWhereText: function (jq) {
        var where = '';
        var pnid = getInfolightOption($(jq[0])).queryDialog;
        if (pnid != undefined) {

            var conditionCaptions = $.sysmsg('getValue', 'Srvtools/AnyQuery/OperatorHelp1').split(";");
            var conditionCaptions1 = $.sysmsg('getValue', 'Srvtools/QueryTranslate/Condition').split(";");
            $(":text", pnid).each(function () {
                var text = $(this);
                var value = text.val();
                var controlClass = $(this).attr('class');
                if (controlClass != undefined) {
                    if (controlClass.indexOf('easyui-datebox') == 0) {
                        value = text.datebox('getBindingValue');
                    }
                    else if (controlClass.indexOf('easyui-datetimebox') == 0) {
                        value = text.datetimebox('getBindingValue');
                    }
                    else if (controlClass.indexOf('info-combobox') == 0
                        || controlClass.indexOf('info-combogrid') == 0) {
                        value = text.combobox('getValue');
                    }
                    else if (controlClass.indexOf('combo-text') == 0) {
                        value = '';
                    }
                    else if (controlClass.indexOf('info-refval') == 0) {
                        value = text.refval('getValue');
                    }
                    else if (controlClass.indexOf('info-autocomplete') == 0) {
                        value = text.combobox('getValue');
                    }
                }
                if (value != undefined && value != '') {
                    var fieldName = getInfolightOption(text).field;
                    var caption = getInfolightOption(text).caption;
                    if (caption == undefined || caption == "")
                        caption = fieldName;
                    if (fieldName != undefined) {
                        var condition = getInfolightOption(text).condition;
                        var dataType = getInfolightOption(text).dataType;
                        var conditionCaption = "";
                        var x;
                        for (x in queryConditions) {
                            if ($.trim(queryConditions[x].value) == condition) {
                                var index = queryConditions[x].key;
                                conditionCaption = conditionCaptions[index];
                            }
                        }
                        where += caption + String.format(conditionCaption, formatQueryValue(value, dataType));
                        //where += " <br/> ";
                        where += " " + conditionCaptions1[0] + " ";
                    }
                }
            });
            where = where.substring(0, where.lastIndexOf(" " + conditionCaptions1[0] + " "));
        }
        where = where.replace('**', '');
        return where;
    },
    getWhereAutoQuery: function (jq) {
        var where = '';
        var isQueryAutoColumn = getInfolightOption($(jq[0])).queryAutoColumn;
        if (isQueryAutoColumn == true) {
            var queryAutoColumns = {};
            var queryTr = $('#queryTr_' + $(jq[0]).attr('id'));
            $(":text", queryTr).each(function () {
                var text = $(this);
                var value = text.val();
                if (value != '') {
                    var fieldName = getInfolightOption(text).field;
                    if (fieldName != undefined) {
                        queryAutoColumns[fieldName] = value;
                        var tableName = undefined;
                        var isNvarChar = false;
                        if ($(jq[0]).attr('class') == 'refval-grid') {    //为refval的查询添加表名
                            var refval = $(jq[0]).data('inforefval').refval;
                            var columns = getInfolightOption($(refval)).columns;
                            for (var i = 0; i < columns.length; i++) {
                                if (fieldName == columns[i].field) {
                                    tableName = columns[i].table;
                                    isNvarChar = columns[i].isNvarChar;
                                    break;
                                }
                            }
                        }
                        else {
                            var columnOption = $(jq[0]).datagrid('getColumnOption', fieldName);
                            if (columnOption) {
                                tableName = columnOption.tableName;
                            }
                        }
                        var nvarchar = '';
                        if (isNvarChar) {
                            nvarchar = 'N';
                        }
                        if (tableName != undefined && tableName != '') {
                            fieldName = tableName + "." + fieldName;
                        }
                        if (where != "") {
                            var andOr = getInfolightOption(text).andOr;
                            if (andOr == undefined || andOr == "")
                                andOr = "and";
                            where += " " + andOr + " ";
                        }

                        var conditionId = text[0].id.replace("_TextBox", "_btn");
                        var condition = $('#' + conditionId).text();
                        //var condition = getInfolightOption(text).condition;
                        var dataType = getInfolightOption(text).dataType;
                        switch (condition) {
                            case '= ':
                            case '!=':
                            case '>=':
                            case '<=':
                                where += fieldName + condition + formatQueryValue(value, dataType, isNvarChar);
                                break;
                            case '% ':
                                where += fieldName + " like " + nvarchar + "'" + value.toString().replace(/\'/g, "''") + "%'";
                                break;
                            case '%%':
                                where += fieldName + " like " + nvarchar + "'%" + value.toString().replace(/\'/g, "''") + "%'";
                                break;
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
            var originalWhere = $(jq).datagrid('getWhere');
            if (originalWhere != undefined && originalWhere != "")
                if (where != "")
                    where = where + " and (" + originalWhere + ")";
                else
                    where = "(" + originalWhere + ")";

            //refval 内置的查询还要加上whereItem
            if ($(jq[0]).attr('class') == 'refval-grid') {
                var refval = $(jq[0]).data('inforefval').refval;
                var whereItem = $(refval).refval('getWhereItem');
                if (whereItem != undefined && whereItem != '') {
                    if (where != '') {
                        where += " and (" + whereItem + ")";
                    }
                    else {
                        where += whereItem;
                    }
                }
                else {
                    $(jq[0]).data('queryAutoColumns', queryAutoColumns);
                }
            }
            else {
                var flowKey = Request.getQueryStringByName("FORM_PRESENTATION");
                var keyValues = Request.getFlowStringByName(flowKey, "FORM_PRESENTATION");
                if (keyValues != undefined && keyValues != null && keyValues != "") {
                    if (where != "")
                        where += " and ";
                    var tableName = Request.getFlowStringByName(flowKey, "FORM_TABLE");
                    var field = 'FlowFlag';
                    if (tableName) {
                        field = tableName + '.FlowFlag';
                    }
                    where += "(" + field + " is Null OR " + field + " = '')";
                }
            }

            return where;
        }
    },
    setWhere: function (jq, where) {
        jq.each(function () {
            var queryParams = $(this).datagrid('options').queryParams;
            var queryWord = {whereString: where};
            queryParams.queryWord = $.toJSONString(queryWord);
            if ($(this).datagrid('options').url == '') {
                var remoteName = getInfolightOption($(this)).remoteName;
                var tableName = getInfolightOption($(this)).tableName;
                $(this).datagrid({
                    loadFilter: defaultFilter,
                    url: getDataUrl(),
                    queryParams: getRemoteParam(queryParams, remoteName, tableName, true)
                });
            }
            else {
                $(this).datagrid('load');
            }

        });
    },
    changeState: function (jq, state) {
        jq.each(function () {
            var toolbar = $(this).datagrid('options').toolbar;
            $(".easyui-linkbutton", toolbar).each(function () {
                var onclick = $(this).attr('name');
                if (onclick != undefined) {
                    if (onclick.indexOf('apply') == 0 || onclick.indexOf('cancel') == 0 || onclick.indexOf('ok') == 0) {
                        if (state == "normal" || state == "initial") {
                            $(this).linkbutton("disable");
                        }
                        else if (state == "editing") {
                            $(this).linkbutton("enable");
                        }
                    }
                }
            });
            if (state == "normal") {
                //find child
                var grid = $(this);
                $(dataGrid_class).each(function () {
                    var parent = getInfolightOption($(this)).parent;
                    if (parent != undefined && parent == grid.attr('id')) {
                        $(this).datagrid('changeState', 'normal');
                    }
                });
            }
            else if (state == "editing") {
                //find parent
                var parent = getInfolightOption($(this)).parent;
                if (parent != undefined && parent != '') {
                    $('#' + parent + dataGrid_class).datagrid('changeState', 'editing');
                }
            }
        });
    },
    getChangedDatas: function (jq, includeDetail) {
        var grid = $(jq[0]);
        var changedDatas = [];
        var changes = grid.datagrid("getChanges");
        if (changes.length != 0) {
            var changedRows = {tableName: getInfolightOption(grid).tableName, inserted: [], deleted: [], updated: []};
            var insertdRows = grid.datagrid("getChanges", "inserted");
            if (insertdRows != null) {
                for (var i = 0; i < insertdRows.length; i++) {
                    var row = {};

                    for (var prop in insertdRows[i]) {
                        var value = insertdRows[i][prop];
                        //if (value != undefined && typeof value == 'string') {
                        //    row[prop] = encodeURIComponent(value);
                        //    row[prop] = row[prop].toString().replace(/(%22)/g, "\"").replace(/(%5C)/g, "\\");
                        //}
                        //else {
                            row[prop] = value;
                        //}
                    }
                    changedRows.inserted.push(row);
                }
            }
            var deletedRows = grid.datagrid("getChanges", "deleted");
            if (deletedRows != null) {
                for (var i = 0; i < deletedRows.length; i++) {
                    var row = {};
                    for (var prop in deletedRows[i]) {
                        var value = deletedRows[i][prop];
                        //if (value != undefined && typeof value == 'string') {
                        //    row[prop] = encodeURIComponent(value);
                        //    row[prop] = row[prop].toString().replace(/(%22)/g, "\"").replace(/(%5C)/g, "\\");
                        //}
                        //else {
                            row[prop] = value;
                        //}
                    }
                    changedRows.deleted.push(row);
                }

            }
            var updatedRows = grid.datagrid("getChanges", "updated");
            if (updatedRows != null) {
                for (var i = 0; i < updatedRows.length; i++) {
                    var row = {};
                    for (var prop in updatedRows[i]) {
                        var value = updatedRows[i][prop];
                        //if (value != undefined && typeof value == 'string') {
                        //    row[prop] = encodeURIComponent(value);
                        //    row[prop] = row[prop].toString().replace(/(%22)/g, "\"").replace(/(%5C)/g, "\\");
                        //}
                        //else {
                            row[prop] = value;
                        //}
                    }
                    changedRows.updated.push(row);
                }
            }
            changedDatas.push(changedRows);
        }
        if (includeDetail != undefined && includeDetail == true) {
            $(dataGrid_class).each(function () {
                var parent = getInfolightOption($(this)).parent;
                if (parent != undefined && parent == grid.attr('id')) {
                    var detailChangedDatas = $(this).datagrid('getChangedDatas', true);
                    for (var i = 0; i < detailChangedDatas.length; i++) {
                        changedDatas.push(detailChangedDatas[i]);
                    }
                }
            });
        }
        return changedDatas;
    },
    setCurrentRow: function (jq, rowData) {
        jq.each(function () {
            var grid = $(this);

            var tableName = getInfolightOption($(this)).tableName;
            $(dataGrid_class).each(function () {
                var parent = getInfolightOption($(this)).parent;
                if (parent != undefined && parent == grid.attr('id')) {
                    initInfoDataGrid($(this), tableName, rowData);
                    setEditIndex($(this), -1); //如果原来在编辑状态，取消这
                }
            });
        });
    },
    addLock: function (jq, options) {
        var flag = true;
        if (jq.length > 0) {
            var type = options.type;
            var grid = $(jq[0]);
            var recordLock = getInfolightOption(grid).recordLock;
            var mode = getInfolightOption(grid).recordLockMode;
            if (recordLock) {
                var remoteName = getInfolightOption(grid).remoteName;
                var tableName = getInfolightOption(grid).tableName;
                var keys = grid.attr("keyColumns").split(',');
                var row = grid.datagrid('getSelected');
                var selectedIndex = getSelectedIndex(grid);
                if (options.rowIndex != undefined) {
                    row = grid.datagrid('getRows')[options.rowIndex];
                    selectedIndex = options.rowIndex;
                }
                var rows = [];
                var lockRow = {};
                for (var i = 0; i < keys.length; i++) {
                    lockRow[keys[i]] = row[keys[i]];
                }
                rows.push(lockRow);
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: getDataUrl(),
                    data: getRemoteParam({
                        keys: $.toJSONString(keys),
                        rows: $.toJSONString(rows),
                        mode: 'recordlock',
                        locktype: type,
                        lockmode: mode
                    }, remoteName, tableName),
                    cache: false,
                    async: false,
                    success: function (data) {
                        if (data.result == "idle") {
                            if (grid.data("lockRows") == undefined) {
                                grid.data("lockRows", []);
                            }
                            grid.data("lockRows").push(row);
                            //readload
                            if (mode == 'reload') {
                                if (data.rows.length == 0) {
                                    alert($.fn.datagrid.defaults.lockDeletedMsg);
                                    flag = false;
                                }
                                else {
                                    flag = data.rows[0];
                                    grid.datagrid('updateRow', {index: selectedIndex, row: data.rows[0]});
                                }
                            }
                        }
                        else {
                            alert(data.user + $.fn.datagrid.defaults.lockMsg);
                            flag = false;
                        }
                    }
                });
            }
        }
        return flag;
    },
    removeLock: function (jq) {
        jq.each(function () {
            //var grid = $(this);
            var recordLock = getInfolightOption($(this)).recordLock;
            if (recordLock) {
                var remoteName = getInfolightOption($(this)).remoteName;
                var tableName = getInfolightOption($(this)).tableName;
                var keys = $(this).attr("keyColumns").split(',');
                var rows = $(this).data("lockRows");
                if (rows && rows.length > 0) {
                    var lockRows = [];
                    for (var i = 0; i < rows.length; i++) {
                        var lockRow = {};
                        for (var j = 0; j < keys.length; j++) {
                            lockRow[keys[j]] = rows[i][keys[j]];
                        }
                        lockRows.push(lockRow);
                    }
                    $.ajax({
                        type: "POST",
                        dataType: 'text',
                        url: getDataUrl(),
                        data: getRemoteParam({
                            keys: $.toJSONString(keys),
                            rows: $.toJSONString(rows),
                            mode: 'recordlock',
                            locktype: 'idle'
                        }, remoteName, tableName),
                        cache: false,
                        async: false,
                        success: function (data) {

                        }
                    });
                }
                $(this).data("lockRows", []);
            }
        });
    },
    validateAll: function (jq) {
        var grid = $(jq[0]);
        var validate = true;
        var editIndex = getEditIndex(grid);
        if (editIndex != -1) {
            validate = grid.datagrid('validateRow', editIndex)
        }
        if (validate) {
            $(dataGrid_class).each(function () {
                var parent = getInfolightOption($(this)).parent;
                if (parent != undefined && parent == grid.attr('id')) {
                    var detailValidate = $(this).datagrid('validateAll');
                    if (!detailValidate) {
                        validate = false;
                    }
                }
            });
        }
        return validate;
    },
    endEditAll: function (jq) {
        jq.each(function () {
            var grid = $(this);
            var editIndex = getEditIndex(grid);
            if (editIndex != -1) {
                grid.datagrid('endEdit', editIndex);
            }
            $(dataGrid_class).each(function () {
                var parent = getInfolightOption($(this)).parent;
                if (parent != undefined && parent == grid.attr('id')) {
                    $(this).datagrid('endEditAll');
                }
            });
        });
    },
    acceptChangesAll: function (jq) {
        jq.each(function () {
            var grid = $(this);
            grid.datagrid("acceptChanges");
            $(dataGrid_class).each(function () {
                var parent = getInfolightOption($(this)).parent;
                if (parent != undefined && parent == grid.attr('id')) {
                    $(this).datagrid('acceptChangesAll');
                }
            });
        });
    },
    hideToolItems: function (jq) {
        jq.each(function () {
            var panel = $(this).datagrid('getPanel');
            $("a[name='insertItem'],a[name='updateItem'],a[name='deleteItem']", panel).hide();
        });
    },
    showToolItems: function (jq) {
        jq.each(function () {
            var panel = $(this).datagrid('getPanel');
            $("a[name='insertItem'],a[name='updateItem'],a[name='deleteItem']", panel).show();
        });
    },
    pasteDatasFromExcel: function (jq) {
        var grid = $(jq[0]);
        var cname = $.sysmsg('getValue', 'JQWebClient/pasteCaption').split(";");
        var div = $('<div></div>').appendTo('body');
        div.dialog({
            title: 'paste ',
            width: 250,
            height: 'auto',
            closed: false,
            cache: false,
            modal: true,
            onclose: function () {
                div.remove();
            }
        });
        var div1 = $('<div class="designer-print"></div>').appendTo(div);
        $('<label>' + cname[0] + '</label >').appendTo(div1);
        var columnIndext = $("<input type=\"text\" value ='1' />").appendTo(div1).css('width', '30px');
        $('<br/>').appendTo(div1);
        $('<br/>').appendTo(div1);
        $('<label>' + cname[1] + '</label >').appendTo(div1);
        var insertornott = $("<input checked=\"checked\" type=\"checkbox\" />").appendTo(div1);
        $('<br/>').appendTo(div1);
        $('<br/>').appendTo(div);

        var okbutton = $('<a href="#" value="OK" ></a>').appendTo(div);
        var canclebutton = $('<a href="#" value="Close" ></a>').appendTo(div);

        okbutton.linkbutton({
            iconCls: 'icon-ok'
        });
        canclebutton.linkbutton({
            iconCls: 'icon-cancel'
        });

        okbutton.unbind().bind('click', function () {
            var columnindex = parseInt($(columnIndext).val()) - 1;
            var insertornot = insertornott.attr('checked') == "checked" ? "insert" : "update";
            div.dialog('close');
            grid.datagrid('pasteDatasFromExcelExcute', {columnIndex: columnindex, mode: insertornot});
        });
        canclebutton.unbind().bind('click', function () {
            div.dialog('close');
        });

        div.dialog('open');
    },
    pasteDatasFromExcelExcute: function (jq, op) {
        var mode, columnIndex, grid;
        mode = op.mode;
        columnIndex = op.columnIndex;
        grid = $(jq[0]);
        //window.clipboardData.getData("Text") ie下获取黏贴的内容 e.clipboardData.getData("text/plain")火狐谷歌下获取黏贴的内容
        if (window.clipboardData) {
            var textdata = window.clipboardData.getData("Text");
            if (textdata != undefined && textdata != "") {
                var rowdatas = textdata.split('\r\n');
                if (mode.toLowerCase() == "insert" || mode.toLowerCase() == "update") {
                    var columnoptions = grid.datagrid('getColumnFields');
                    var rowCount = grid.datagrid('getRows').length;
                    var selectedRow = grid.datagrid('getSelected');
                    var lng = columnoptions.length;
                    if (columnIndex > lng) {
                        //alert('error1');
                        return;
                    }
                    for (var i = 0; i < rowdatas.length; i++) {
                        if (!$.trim(rowdatas[i]) == 0) {
                            var newdata = {};
                            if (mode.toLowerCase() == "insert") {
                                newdata = getDefaultValues(grid, newdata);
                                newdata = getSeq(grid, null, newdata);

                            }
                            //for (var j = 0; j < columnIndex; j++) {
                            //    newdata[columnoptions[j]] = null;
                            //}
                            var celldatas = rowdatas[i].split('\t');
                            var l = 0;
                            for (var k = 0; k < celldatas.length; k++) {
                                if ($.trim(celldatas[k]) != "") {
                                    if (l + columnIndex > lng) {
                                        //alert('error2');
                                        break;
                                    }
                                    if (mode.toLowerCase() == "update") {
                                        var keys = grid.attr("keyColumns");
                                        if (keys != undefined && keys != "") {
                                            var keysp = keys.split(',');
                                            for (var kp = 0; kp < keysp.length; kp++) {
                                                if (columnoptions[l + columnIndex] == keysp[kp]) {
                                                    var error2 = $.sysmsg('getValue', 'JQWebClient/pasteError');
                                                    alert(error2);
                                                    //alert("Key Fields can't update.");
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    newdata[columnoptions[l + columnIndex]] = celldatas[k];
                                    l++;
                                }
                            }
                            if (mode.toLowerCase() == "insert") {
                                grid.datagrid('appendRow', newdata);
                                var autoApply = getInfolightOption(grid).autoApply;
                                if (autoApply) {
                                    $.fn.Error.errorCode = 1102;
                                    applyUpdates(grid);
                                }
                                else {
                                    grid.datagrid('changeState', 'editing');
                                }
                                //grid.datagrid('acceptChanges');
                            }
                            else if (mode.toLowerCase() == "update") {
                                if (selectedRow != undefined) {
                                    var selectedIndex = grid.datagrid('getRowIndex', selectedRow);
                                    if (selectedIndex + i >= rowCount) {
                                        grid.datagrid('appendRow', newdata);
                                    }
                                    else {
                                        grid.datagrid('beginEdit', selectedIndex + i);
                                        //grid.datagrid('updateRow', {
                                        //    index: selectedIndex + i,
                                        //    row: newdata
                                        //});
                                        for (var field in newdata) {
                                            var editor = grid.datagrid('getEditor', {
                                                index: selectedIndex + i,
                                                field: field
                                            });
                                            if (editor) {
                                                editor.actions.setValue(editor.target, newdata[field]);
                                            }
                                        }
                                        grid.datagrid('endEdit', selectedIndex + i);
                                    }
                                    var autoApply = getInfolightOption(grid).autoApply;
                                    if (autoApply) {
                                        $.fn.Error.errorCode = 1102;
                                        applyUpdates(grid);
                                    }
                                    else {
                                        grid.datagrid('changeState', 'editing');
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else {
            alert('您的瀏覽器不支持這個功能，請使用IE');
        }

    }
});

$.extend($.fn.datagrid.defaults, {
    parentEmptyMessage: "Can not insert item since parent key is empty"
});

$.extend($.fn.tree.methods, {
    setWhere: function (jq, where) {
        jq.each(function () {
            var whereString = where;
            var remoteName = getInfolightOption($(this)).remoteName;
            var tableName = getInfolightOption($(this)).tableName;
            $(this).tree({
                url: getDataUrl(),
                queryParams: getRemoteParam({whereString: whereString}, remoteName, tableName, false, -1)
            });
            $(this).tree('reload');
        });
    }
});

function getClassName(obj) {
    var funcNameRegex = /function (.{1,})\(/;
    var results = (funcNameRegex).exec(obj.prototype.constructor.toString());
    return (results && results.length > 1) ? results[1] : "";
}

function renderQueryAutoColumn(target) {
    var infolightOptions = getInfolightOption($(target));
    if (infolightOptions.queryAutoColumn) {
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
            //var bTable = panel.children('div.datagrid-view').children('div.datagrid-view2').children('div.datagrid-body').children('table.datagrid-btable');
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
                        var value = '';
                        if ($(target).data('queryAutoColumns') && $(target).data('queryAutoColumns')[fieldName] != undefined) {
                            value = $(target).data('queryAutoColumns')[fieldName];
                        }
                        var wdth = $(target).datagrid("getColumnOption", fieldName).width - 35;
                        if (wdth < 50) {
                            wdth = 50;
                        }
                        td += '<input onblur="queryAutoColumn(\'#' + target.id + '\')" value="' + value + '" id ="' + target.id + "_" + fieldName + "_TextBox" + '" type=\"text\" infolight-options="field:\'' + fieldName + '\',condition:\'' + condition + '\',dataType:\'' + dataType + '\'" style="width:' + wdth + 'px" />';
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
    }
}

//等於;不等於;大於;小於;大於等於;小於等於;以**結尾;以**開頭;包含字符**;不包含字符**;區間之內;區間之外;選擇的條件內;選擇的條件外
var queryConditions = [
    {key: 0, value: "% "},
    {key: 1, value: "%%"},
    {key: 2, value: "= "},
    {key: 3, value: ">="},
    {key: 4, value: "<="},
    {key: 5, value: "!="},
    {key: 6, value: "in"}
];

function changeCondition(sender) {
    var conditionId = sender.id;
    var target = $("#" + conditionId);
    var currentCondition = target.text();
    var x;
    var nextCondition;
    for (x in queryConditions) {
        if (queryConditions[x].value == currentCondition) {
            if (parseInt(x) < 5) {
                nextCondition = queryConditions[parseInt(x) + 1].value;
            }
            else {
                nextCondition = queryConditions[0].value;
            }
        }
    }
    target[0].innerHTML = nextCondition;
}

function initInfoDataGrid(datagrid, parentTableName, parentRow) {
    var remoteName = getInfolightOption(datagrid).remoteName;
    var tableName = getInfolightOption(datagrid).tableName;
    var flowKey = Request.getQueryStringByName("FORM_PRESENTATION");
    var realTableName = Request.getFlowStringByName(flowKey, "FORM_TABLE");
    //增加了ontotal之后原来传给grid的参数有变化，为了在app_code里的代码不变动，传上去做setwhere条件的不变
    var totalWord = {};
    var totalParamters = {};
    var hasTotal = false;
    $("th,input,select,textarea", datagrid).each(function () {
        var column = $(this);
        var total = getInfolightOption(column).total;
        var ontotal = getInfolightOption(column).onTotal;
        if (total != undefined) {
            var field = getInfolightOption(column).field;
            var totalsingle = {};
            totalsingle["type"] = total;
            totalsingle["method"] = ontotal;
            totalWord[field] = total;
            totalParamters[field] = totalsingle;
            hasTotal = true
        }
    });
    var totalCaption = "";
    //        if (getInfolightOption(datagrid).editDialog == undefined) {
    //            totalCaption = getInfolightOption(datagrid).totalCaption; ;
    //            if (totalCaption == undefined) {
    //                totalCaption = "";
    //            }
    //        }

    var whereString = '';
    var alwaysClose = getInfolightOption(datagrid).alwaysClose;
    if (alwaysClose != undefined && alwaysClose == true) {
        var queryWord = {whereString: '1=0'};
        whereString = $.toJSONString(queryWord);
    }
    var editDialog = getInfolightOption(datagrid).editDialog;
    var formView = $(getInfolightOption($(editDialog)).containForm);
    var isShowFlowIcon = getInfolightOption(formView).isshowflowicon;
    if (isShowFlowIcon == "true" || isShowFlowIcon == true) {
        var flowKey = Request.getQueryStringByName("FORM_PRESENTATION");
        var navMODE = Request.getQueryStringByName("NAVMODE");
        if (navMODE == "") navMODE = Request.getQueryStringByName("NAVIGATOR_MODE");
        if (navMODE == "") navMODE = Request.getFlowStringByName(flowKey, "NAVMODE");
        if (navMODE == "") navMODE = Request.getFlowStringByName(flowKey, "NAVIGATOR_MODE");
        if (navMODE != undefined && navMODE != null && (navMODE == "Insert" || navMODE == "Prepare" || navMODE == "Inquery")) {
            var queryWord = {};
            if (navMODE == "Insert")
                queryWord.whereString = '1=0';
            else if (navMODE == "Prepare") {
                //var tableName = Request.getQueryStringByName("FORM_TABLE");
                var field = 'FlowFlag';
                if (realTableName) {
                    field = realTableName + '.FlowFlag';
                }
                queryWord.whereString = "(" + field + " is Null OR " + field + " = '')";
            }
            whereString = $.toJSONString(queryWord);

            var infolightOptions = datagrid.attr("infolight-options");
            if (navMODE == "Inquery" || navMODE == "3") {
                //infolightOptions = infolightOptions.replace("allowInsert:true,allowUpdate:true,allowDelete:true", "allowInsert:false,allowUpdate:false,allowDelete:false");
                infolightOptions = infolightOptions.replace("allowInsert:true", "allowInsert:false");
                infolightOptions = infolightOptions.replace("allowUpdate:true", "allowUpdate:false");
                infolightOptions = infolightOptions.replace("allowDelete:true", "allowDelete:false");
                infolightOptions = infolightOptions.replace("commandButtons:'vud'", "commandButtons:'v'");
                $("[onclick='insertItem('#" + datagrid[0].id + "')']").hide();
                $("[onclick='apply('#" + datagrid[0].id + "')']").hide();
                $("[onclick='cancel('#" + datagrid[0].id + "')']").hide();
            }
            else if (navMODE == "Prepare" || navMODE == "4") {
                //infolightOptions = infolightOptions.replace("allowInsert:true,allowUpdate:true,allowDelete:true", "allowInsert:false,allowUpdate:true,allowDelete:false");
                infolightOptions = infolightOptions.replace("allowInsert:true", "allowInsert:false");
                infolightOptions = infolightOptions.replace("allowUpdate:false", "allowUpdate:true");
                infolightOptions = infolightOptions.replace("allowDelete:true", "allowDelete:false");
                infolightOptions = infolightOptions.replace("commandButtons:'vud'", "commandButtons:'vu'");
                $("[onclick='insertItem('#" + datagrid[0].id + "')']").hide();
                //$("#toolItem" + datagrid[0].id + "apply").hide();
                //$("#toolItem" + datagrid[0].id + "cancel").hide();
            }
            else {
                //infolightOptions = infolightOptions.replace("allowInsert:true,allowUpdate:true,allowDelete:true", "allowInsert:false,allowUpdate:false,allowDelete:false");
                infolightOptions = infolightOptions.replace("allowInsert:true", "allowInsert:false");
                infolightOptions = infolightOptions.replace("allowUpdate:true", "allowUpdate:false");
                infolightOptions = infolightOptions.replace("allowDelete:true", "allowDelete:false");
                infolightOptions = infolightOptions.replace("commandButtons:'vud'", "commandButtons:'v'");
                $("[onclick='insertItem('#" + datagrid[0].id + "')']").hide();
                $("[onclick='apply('#" + datagrid[0].id + "')']").hide();
                $("[onclick='cancel('#" + datagrid[0].id + "')']").hide();
            }
            datagrid.attr("infolight-options", infolightOptions);
        }
        var flowKey = Request.getQueryStringByName("FORM_PRESENTATION");
        var keyValues = Request.getFlowStringByName(flowKey, "FORM_PRESENTATION");
        if (keyValues != undefined && keyValues != null && keyValues != "") {
            var queryWord = {whereString: decodeURIComponent(keyValues).replace(/''/g, "'").replace(/;/g, " and ")};
            //var tableName = Request.getQueryStringByName("FORM_TABLE");
            if (realTableName) {
                var keys = queryWord.whereString.split(";");
                queryWord.whereString = "";
                for (var i = 0; i < keys.length; i++) {
                    if (keys[i] != "") {
                        queryWord.whereString += realTableName + "." + keys[i] + ";";
                    }
                }
                queryWord.whereString = queryWord.whereString.substr(0, queryWord.whereString.lastIndexOf(";"));
            }
            whereString = $.toJSONString(queryWord);
        }
        else {

        }
    }
    var drilldown = Request.getQueryStringByName("DRILLDOWN");
    if (drilldown != undefined && drilldown == 'true') {
        var REMOTENAME = decodeURIComponent(Request.getQueryStringByName("REMOTENAME"));
        var TABLENAME = decodeURIComponent(Request.getQueryStringByName("TABLENAME"));
        var DRILLDOWN_KEYFIELD = Request.getQueryStringByName("DRILLDOWN_KEYFIELD");
        if (REMOTENAME == remoteName && TABLENAME == tableName) {
            var queryWord = {whereString: decodeURIComponent(DRILLDOWN_KEYFIELD).replace(/''/g, "'").replace(/;/g, " and ")};
            whereString = $.toJSONString(queryWord);
        }
    }
    if (parentTableName != undefined) {
        var queryWord = {parentTableName: parentTableName};
        if (parentRow != undefined && parentRow != null) {
            queryWord.parentRow = parentRow;
        }
        whereString = $.toJSONString(queryWord);
    }

    var showFooter = false;
    var queryParams = {queryWord: whereString};
    if (hasTotal) {
        showFooter = true;
        //if (datagrid.datagrid('options').pagination == false) {
        datagrid.attr('totalCaption', totalCaption);
        datagrid.attr('totalColumn', $.toJSONString(totalParamters));
        //}
        queryParams.totalColumn = $.toJSONString(totalWord);
        queryParams.totalCaption = totalCaption;

    }
    var view = commandview;
    var detailFormatter = undefined;
    var onExpandRow = undefined;
    var editMode = getInfolightOption(datagrid).editMode;
    var multiSelect = getInfolightOption(datagrid).multiSelect;
    if (multiSelect == undefined) {
        multiSelect = false;
    }

    if (getInfolightOption(datagrid).bufferView) {
        view = bufferview;
    }
    else if (editDialog != undefined && editMode != undefined && editMode.toLowerCase() == "expand") {
        view = detailview;
        detailFormatter = gridDetailFormatter;
        onExpandRow = gridExpandRow;
    }
    var onHeaderContextMenu = undefined;
    var columnsHibeable = getInfolightOption(datagrid).columnsHibeable;
    var fitColumns = false;
    if (columnsHibeable != undefined && columnsHibeable == true) {
        fitColumns = true;
        var hideColumnMenuId = datagrid.attr('id') + "HideColumnMenu";
        var hideColumnMenu = $('#' + hideColumnMenuId);
        if (hideColumnMenu.length) {
        }
        else {
            hideColumnMenu = $('<div id="' + hideColumnMenuId + '" />').appendTo('body');
        }
        onHeaderContextMenu = function (e, field) {
            e.preventDefault();
            if (hideColumnMenu[0].childNodes == undefined || hideColumnMenu[0].childNodes.length == 0) {
                hideColumnMenu.menu({
                    onClick: function (item) {
                        if (item.iconCls == 'icon-ok') {
                            $(datagrid).datagrid('hideColumn', item.name);
                            hideColumnMenu.menu('setIcon', {
                                target: item.target,
                                iconCls: 'icon-empty'
                            });
                        } else {
                            $(datagrid).datagrid('showColumn', item.name);
                            hideColumnMenu.menu('setIcon', {
                                target: item.target,
                                iconCls: 'icon-ok'
                            });
                        }
                    }
                });
                var fields = $(datagrid).datagrid('getColumnFields');
                for (var i = 0; i < fields.length; i++) {
                    var field = fields[i];
                    var col = $(datagrid).datagrid('getColumnOption', field);
                    if (col.hidden == 'false') {
                    }
                    else {
                        hideColumnMenu.menu('appendItem', {
                            text: col.title,
                            name: field,
                            iconCls: 'icon-ok'
                        });
                    }
                }
            }
            hideColumnMenu.menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        }
    }

    var flowKey = Request.getQueryStringByName("FORM_PRESENTATION");
    var flNavigatorMode = Request.getQueryStringByName("FLNAVMODE");
    if (flNavigatorMode == "") flNavigatorMode = Request.getFlowStringByName(flowKey, "FLNAVMODE");
    if (flNavigatorMode == undefined || flNavigatorMode == "" || flNavigatorMode == null) {
        flNavigatorMode = Request.getFlowStringByName(flowKey, "FLNAVIGATOR_MODE");
    }
    if (navMODE == "Normal" || navMODE == "0" || flNavigatorMode == "Notify" || flNavigatorMode == "3") {
        var infolightOptions = datagrid.attr("infolight-options");
        //infolightOptions = infolightOptions.replace("allowInsert:true,allowUpdate:true,allowDelete:true", "allowInsert:false,allowUpdate:false,allowDelete:false");
        infolightOptions = infolightOptions.replace("allowInsert:true", "allowInsert:false");
        infolightOptions = infolightOptions.replace("allowUpdate:true", "allowUpdate:false");
        infolightOptions = infolightOptions.replace("allowDelete:true", "allowDelete:false");
        infolightOptions = infolightOptions.replace("commandButtons:'vud'", "commandButtons:'v'");
        datagrid.attr("infolight-options", infolightOptions);
    }
    if (flNavigatorMode == "1") {
        //$("#FlowEdit").hide();
        $('#FlowEdit').linkbutton('disable');
    }
    var totalWidth = datagrid.context.style ? datagrid.context.style.width.replace('px', '') : '';
    var rowNumbers = getInfolightOption(datagrid).rowNumbers;
    if (rowNumbers == undefined) {
        rowNumbers = true;
    }
    datagrid.removeData('lastInsertedRow');
    $.fn.Error.errorCode = 1000;
    datagrid.datagrid({
        url: getDataUrl(),
        queryParams: getRemoteParam(queryParams, remoteName, tableName, true),
        collapsible: true,
        pageNumber: 1,
        rownumbers: rowNumbers,
        selectOnCheck: true,
        singleSelect: !multiSelect,
        showFooter: showFooter,
        onClickRow: onClickRow,
        view: view,
        detailFormatter: detailFormatter,
        onExpandRow: onExpandRow,
        fitColumns: fitColumns,
        onHeaderContextMenu: onHeaderContextMenu,
        onLoadSuccess: function (data) {
            $.fn.Error.errorCode = 0;
            if (data.keys != undefined && data.keys.length > 0) {
                $(this).attr('keyColumns', data.keys);
            }
            if (data.tableName != undefined) {
                $(this).attr('tableName', data.tableName);
            }
            setEditIndex($(this), -1); //如果原来在编辑状态，取消这
            var editDialog = getInfolightOption($(this)).editDialog;
            var editMode = getInfolightOption($(this)).editMode;
            if (data.rows.length > 0) {
                var selectIndex = datagrid.data("selectIndex");
                if (selectIndex != undefined) {
                    if (selectIndex >= 0 && selectIndex < data.rows.length) {
                        $(this).datagrid('selectRow', selectIndex);
                        $(this).datagrid('setCurrentRow', data.rows[selectIndex]);

                        if (editMode != undefined && editMode.toLowerCase() == "continue") {
                            openForm(editDialog, data.rows[selectIndex], "viewed", editMode);
                        }
                    }
                    datagrid.removeData("selectIndex");
                }
                else {
                    if (multiSelect && multiSelectGrid) {
                    }
                    else {
                        $(this).datagrid('selectRow', 0);
                        $(this).datagrid('setCurrentRow', data.rows[0]);
                        if (editMode != undefined && editMode.toLowerCase() == "continue") {
                            openForm(editDialog, data.rows[0], "viewed", editMode);
                        }
                    }
                }
                if (multiSelect && multiSelectGrid) {
                    var rows = $(multiSelectGrid).datagrid('getRows');
                    //$(this).datagrid('uncheckAll');
                    $(this).datagrid('getPanel').find('.datagrid-header-check').find('input').prop('checked', false);
                    //$(multiSelectGrid).datagrid('loadData', []);
                    var keys = $(this).attr("keyColumns").split(',');
                    for (var i = 0; i < data.rows.length; i++) {
                        var find = false;
                        for (var j = 0; j < rows.length; j++) {
                            var eq = true;
                            for (var k = 0; k < keys.length; k++) {
                                if (data.rows[i][keys[k]] != rows[j][keys[k]]) {
                                    eq = false;
                                    break;
                                }
                            }
                            if (eq) {
                                find = true;
                                break;
                            }
                        }
                        if (find) {
                            $(this).datagrid('checkRow', i);
                            //$(multiSelectGrid).datagrid('appendRow', checkedRows[i]);
                        }
                    }

                }
            }
            else {
                $(this).datagrid('setCurrentRow', null);
                if (editMode != undefined && editMode.toLowerCase() == "continue") {
                    openForm(editDialog, null, "viewed", editMode);
                }
            }

            //var navMODE = Request.getQueryStringByName("NAVMODE");
            if (isShowFlowIcon == "true" || isShowFlowIcon == true) {
                if (self.opened != true) {
                    self.opened = true;

                    var flowKey = Request.getQueryStringByName("FORM_PRESENTATION");
                    var navMODE = Request.getQueryStringByName("NAVMODE");
                    if (navMODE == "") navMODE = Request.getQueryStringByName("NAVIGATOR_MODE");
                    if (navMODE == "") navMODE = Request.getFlowStringByName(flowKey, "NAVMODE");
                    if (navMODE == "") navMODE = Request.getFlowStringByName(flowKey, "NAVIGATOR_MODE");

                    if (keyValues != undefined && keyValues != null && keyValues != "") {
                        if (editMode != undefined && (editMode.toLowerCase() == "switch" || editMode.toLowerCase() == 'dialog')) {
                            if (getInfolightOption($(this)).parent == undefined || getInfolightOption($(this)).parent == "") {
                                if (navMODE == "2") {
                                    //var editIcons = $('span.icon-edit');
                                    //if (editIcons.length > 0)
                                    //    editIcons[0].click();
                                    var target = this;
                                    var rowIndex = 0;
                                    var editDialog = getInfolightOption($(target)).editDialog;
                                    var editMode = getInfolightOption($(target)).editMode;
                                    var lastSelectIndex = getSelectedIndex($(target));
                                    $(target).datagrid('selectRow', rowIndex);
                                    var onUpdate = getInfolightOption($(target)).onUpdate;
                                    if (onUpdate != undefined) {
                                        var rowData = $(target).datagrid('getSelected');
                                        var flag = onUpdate.call($(target), rowData);
                                        if (flag != undefined && flag.toString() == 'false') {
                                            return false;
                                        }
                                    }
                                    if (editDialog == undefined) {
                                        var editIndex = getEditIndex($(target));
                                        if (editIndex == -1) {
                                            beginEdit($(target), rowIndex);
                                            if (lastSelectIndex != rowIndex) {
                                                var rowData = $(target).datagrid('getSelected');
                                                $(target).datagrid('setCurrentRow', rowData);
                                            }
                                        }
                                        else {
                                            if (editIndex != rowIndex) {
                                                if (endEdit($(target))) {
                                                    beginEdit($(target), index);
                                                    if (lastSelectIndex != rowIndex) {
                                                        var rowData = $(target).datagrid('getSelected');
                                                        $(target).datagrid('setCurrentRow', rowData);
                                                    }
                                                }
                                                else {
                                                    $(target).datagrid('selectRow', editIndex);
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        var rowData = $(target).datagrid('getSelected');
                                        $(target).datagrid('setCurrentRow', rowData);
                                        if (editMode.toLowerCase() == "switch") {
                                            $(target).datagrid('getPanel').panel('collapse');
                                            var formname = getInfolightOption($(editDialog)).containForm;
                                            var form = $(formname);
                                            form.attr("switchGrid", "#" + $(target).attr('id'));
                                            //hide submit div
                                            if (getInfolightOption($(target)).CollapseDiv != undefined) {
                                                $(getInfolightOption($(target)).CollapseDiv).each(function () {
                                                    $(this).css('display', 'none');
                                                });
                                            }
                                            else if ($("#" + $(target).attr('id') + "-SubmitDiv") != undefined) {
                                                $("#" + $(target).attr('id') + "-SubmitDiv").css('display', 'none');
                                            }
                                            //end
                                        }
                                        else if (editMode.toLowerCase() == "dialog") {
                                            var formname = getInfolightOption($(editDialog)).containForm;
                                            var form = $(formname);
                                            form.attr('dialogGrid', "#" + $(target).attr('id'));
                                        }
                                        else if (editMode.toLowerCase() == 'continue') {
                                            var dataFormTabID = getInfolightOption($(editDialog)).dataFormTabID;
                                            if (dataFormTabID != undefined) {
                                                var tabs = $(editDialog).closest('.easyui-tabs');
                                                if (tabs != undefined) {
                                                    var tabslist = $(tabs).tabs('tabs');
                                                    for (var i = 0; i < tabslist.length; i++) {
                                                        if ($(tabslist[i]).attr('id') == dataFormTabID) {
                                                            $(tabs).tabs('select', i);
                                                        }
                                                    }
                                                }
                                            }
                                            var formname = getInfolightOption($(editDialog)).containForm;
                                            var form = $(formname);
                                            form.attr('continueGrid', "#" + $(target).attr('id'));
                                        }
                                        var openMode = "updated";
                                        if (Request.getFlowStringByName(flowKey, "PLUSROLES") != "")
                                            openMode = "viewed";
                                        openForm(editDialog, rowData, openMode, editMode, $(target).attr('keyColumns'));
                                    }
                                }
                                else {
                                    var viewIcons = $('span.icon-view', '.datagrid-row-command');
                                    if (viewIcons.length > 0)
                                        viewIcons[0].click();
                                }
                                datagrid.datagrid('getPanel').panel('collapse');
                            }
                        }
                        //var infolightOptions = datagrid.attr("infolight-options");
                        //infolightOptions = infolightOptions.replace("allowInsert:true,allowUpdate:true,allowDelete:true", "allowInsert:false,allowUpdate:false,allowDelete:false");
                        //infolightOptions = infolightOptions.replace("commandButtons:'vud'", "commandButtons:'v'");
                        //datagrid.attr("infolight-options", infolightOptions);
                        //$("#toolItem" + datagrid[0].id + "insertItem").hide();
                        //$("#toolItem" + datagrid[0].id + "apply").hide();
                        //$("#toolItem" + datagrid[0].id + "cancel").hide();
                    }
                    else if (navMODE != undefined && navMODE != null) {
                        if (navMODE == "Insert") {
                            if (editMode != undefined && (editMode.toLowerCase() == "switch" || editMode.toLowerCase() == 'dialog')) {
                                if (getInfolightOption($(this)).parent == undefined || getInfolightOption($(this)).parent == "") {
                                    insertItem("#" + $(this).attr('id'));
                                    datagrid.datagrid('getPanel').panel('collapse');
                                }
                            }
                            else {
                                var localstring = $.sysmsg('getValue', 'JQWebClient/CanNotUseFlow');
                                alert(localstring)
                            }
                        }
                        //var infolightOptions = datagrid.attr("infolight-options");
                        //infolightOptions = infolightOptions.replace("allowInsert:true,allowUpdate:true,allowDelete:true", "allowInsert:false,allowUpdate:false,allowDelete:false");
                        //infolightOptions = infolightOptions.replace("commandButtons:'vud'", "commandButtons:'v'");
                        //datagrid.attr("infolight-options", infolightOptions);
                        //$("#toolItem" + datagrid[0].id + "insertItem").hide();
                        //$("#toolItem" + datagrid[0].id + "apply").hide();
                        //$("#toolItem" + datagrid[0].id + "cancel").hide();
                    }
                }
            }
            $(".info-qrcode", datagrid.parent()).each(function () {
                initinfoqrcode($(this));
            });

            var onLoadSuccess = getInfolightOption($(this)).onLoadSuccess;
            if (onLoadSuccess != undefined) {
                onLoadSuccess.call($(this), data);
            }
            if (datagrid.datagrid('options').showFooter == true && datagrid.datagrid('options').pagination == false) {
                setFooter(datagrid);
            }
        }
    });
    if (columnsHibeable != undefined && columnsHibeable == true) {
        datagrid.datagrid('resize', {width: (totalWidth == "" ? "auto" : totalWidth)});
    }
    datagrid.datagrid('changeState', 'initial');
    var multiSelectGrid = getInfolightOption(datagrid).multiSelectGrid;
    if (multiSelect && multiSelectGrid) {
        var checkGrid = function () {
            var checkedRows = $(this).datagrid('getChecked');
            //$(multiSelectGrid).datagrid('loadData', []);
            var keys = $(this).attr("keyColumns").split(',');
            for (var i = 0; i < checkedRows.length; i++) {

                var rows = $(multiSelectGrid).datagrid('getRows');

                var find = false;
                for (var j = 0; j < rows.length; j++) {
                    var eq = true;
                    for (var k = 0; k < keys.length; k++) {
                        if (checkedRows[i][keys[k]] != rows[j][keys[k]]) {
                            eq = false;
                            break;
                        }
                    }
                    if (eq) {
                        find = true;
                        break;
                    }
                }
                if (!find) {
                    $(multiSelectGrid).datagrid('appendRow', checkedRows[i]);
                }
            }

            //$(multiSelectGrid)
        };

        var unCheckGrid = function (rows, row) {
            var uncheckRows = [];
            if (row) {
                uncheckRows.push(row);
            }
            else {
                for (var i = 0; i < rows.length; i++) {
                    uncheckRows.push(rows[i]);
                }
            }
            var keys = $(this).attr("keyColumns").split(',');
            for (var i = 0; i < uncheckRows.length; i++) {
                var rows = $(multiSelectGrid).datagrid('getRows');
                for (var j = 0; j < rows.length; j++) {
                    var eq = true;
                    for (var k = 0; k < keys.length; k++) {
                        if (uncheckRows[i][keys[k]] != rows[j][keys[k]]) {
                            eq = false;
                            break;
                        }
                    }
                    if (eq) {
                        $(multiSelectGrid).datagrid('deleteRow', j);
                        break;
                    }
                }
            }
        };


        datagrid.datagrid('options').onCheck = checkGrid;
        datagrid.datagrid('options').onUncheck = unCheckGrid;
        datagrid.datagrid('options').onCheckAll = checkGrid;
        datagrid.datagrid('options').onUncheckAll = unCheckGrid;
    }

    var pager = datagrid.datagrid('getPager');

    if (pager) {
        $(pager).pagination({
            onBeforeRefresh: function () {
                var selectIndex = getSelectedIndex(datagrid);
                datagrid.data("selectIndex", selectIndex);
            },
            onBeforeSelectPage: function () {
                if (datagrid.datagrid('getChanges').length > 0) {
                    var message = '資料尚未存檔, 請先保存';
                    alert(message);
                    return false;
                }
                return true;
            }
        });

    }
}

function initInfoComboBox(combobox) {
    var valueField = getInfolightOption(combobox).valueField;
    var textField = getInfolightOption(combobox).textField;
    var remoteName = getInfolightOption(combobox).remoteName;
    var tableName = getInfolightOption(combobox).tableName;
    var onSelect = getInfolightOption(combobox).onSelect;
    var rows = getInfolightOption(combobox).pageSize;
    var selectOnly = getInfolightOption(combobox).selectOnly;
    var panelHeight = getInfolightOption(combobox).panelHeight;
    var checkData = getInfolightOption(combobox).checkData;

    if (panelHeight == undefined) {
        //panelHeight = "auto";
    }
    if (rows == undefined)
        rows = -1;
    if (remoteName != undefined && remoteName != "") {
        var value = $(combobox).hasClass('combobox-f') ? $(combobox).combobox('getValue') : '';
        $(combobox).combobox({
            url: getDataUrl(),
            queryParams: getRemoteParam({}, remoteName, tableName, false, rows),
            valueField: valueField,
            textField: textField,
            mode: 'remote',
            onBeforeLoad: function (param) {
                param.RemoteName = remoteName;
                param.TableName = tableName;
                param.IncludeRows = false;
                param.rows = rows;
                if (comboBeforeLoad) {
                    comboBeforeLoad.call(this, param);
                }
            },
            editable: !selectOnly,
            panelHeight: panelHeight,
            value: value,
            onSelect: function (rowdata) {
                if (onSelect != undefined) {
                    onSelect.call(this, rowdata);
                }
            },
            onShowPanel: function () {
                if (selectOnly == true) {
                    //                    var panel = $(this).combobox('panel');
                    //                    if ($(".combobox-empty-item", panel).length == 0) {
                    //                        var item = $("<div class=\"combobox-item combobox-empty-item\"></div>").prependTo(panel);
                    //                        item.attr("value", "");
                    //                        item.html($.fn.combobox.defaults.emptyText);
                    //                        item.hover(function () {
                    //                            $(this).addClass("combobox-item-hover");
                    //                        }, function () {
                    //                            $(this).removeClass("combobox-item-hover");
                    //                        });
                    //                        item.click(function () {
                    //                            $(combobox).combobox('setValue', '');
                    //                            $(combobox).combobox('hidePanel');
                    //                        });
                    //                    }
                }
            }
        });                 //初始化

        combobox.combobox("textbox").blur(function () {
            var value = combobox.combobox("getValue");
            // var whereItem = "";
            if (checkData == true) {
                var param = [];
                param.push(remoteName);
                param.push(tableName);
                param.push(valueField);
                //whereItem = $(combobox).combobox('getWhereItem');
                //param.push(whereItem);

                var valid = $.fn.validatebox.defaults.rules.checkData.validator.call(this, value, param);
                if (valid == false) {
                    alert($.fn.validatebox.defaults.rules.checkData.message);
                    combobox.combobox('setValue', '');
                }
            }
        });
    }
    else {
        $(combobox).combobox({
            onBeforeLoad: comboBeforeLoad,
            editable: !selectOnly,
            panelHeight: panelHeight,
            onSelect: function (rowdata) {
                if (onSelect != undefined) {
                    onSelect.call(this, rowdata);
                }
            },
            onShowPanel: function () {
                if (selectOnly == true) {
                    //                    var panel = $(this).combobox('panel');
                    //                    if ($(".combobox-empty-item", panel).length == 0) {
                    //                        var item = $("<div class=\"combobox-item combobox-empty-item\"></div>").prependTo(panel);
                    //                        item.attr("value", "");
                    //                        item.html($.fn.combobox.defaults.emptyText);
                    //                        item.hover(function () {
                    //                            $(this).addClass("combobox-item-hover");
                    //                        }, function () {
                    //                            $(this).removeClass("combobox-item-hover");
                    //                        });
                    //                        item.click(function () {
                    //                            $(combobox).combobox('setValue', '');
                    //                            $(combobox).combobox('hidePanel');
                    //                        });
                    //                    }
                }
            }
        });       //初始化
    }
}

function initInfoComboGrid(combogrid) {
    //var field = getInfolightOption(combogrid).field;
    //var form = getInfolightOption(combogrid).form;
    var valueField = getInfolightOption(combogrid).valueField;
    var textField = getInfolightOption(combogrid).textField;
    var valueFieldCaption = getInfolightOption(combogrid).valueFieldCaption;
    var textFieldCaption = getInfolightOption(combogrid).textFieldCaption;
    //var PageSize = getInfolightOption(combogrid).PageSize;
    var panelWidth = getInfolightOption(combogrid).panelWidth;
    var panelHeight = getInfolightOption(combogrid).panelHeight;
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
            };
            columns.push(textColumn);
        }
    }
    if (multiple) {
        columns.unshift({field: 'ck', checkbox: true});
    }
    var onSelect = getInfolightOption(combogrid).onSelect;
    var queryParams = {queryWord: ''};
    var value = $(combogrid).hasClass('combogrid-f') ? $(combogrid).combogrid('getValue') : '';
    $(combogrid).combogrid({
        url: getDataUrl(),
        //queryParams: getRemoteParam({}, remoteName, tableName, true),
        onBeforeLoad: function (param) {
            param.RemoteName = remoteName;
            param.TableName = tableName;
            param.IncludeRows = true;
            if (onBeforeLoad) {
                onBeforeLoad.call(this, param);
            }
        },
        panelWidth: panelWidth,
        panelHeight: panelHeight,
        pagination: true,
        editable: !selectOnly,
        value: value,
        keyHandler: {
            up: function () {
            },
            down: function () {
            },
            enter: function () {
            },
            query: combogridQuery
        },
        queryParams: queryParams,
        fitColumns: false,
        idField: valueField,
        textField: textField,
        columns: [columns],
        //onBeforeLoad: onBeforeLoad,
        multiple: multiple,
        onClickRow: function (rowindex, rowdata) {
            if (onSelect != undefined) {
                onSelect.call(combogrid, rowdata);
            }
        }
    });     //初始化
}

function initInfoRefVal(info_refval) {
    var infoRefval = $(info_refval);
    //var field = getInfolightOption(infoRefval).field;
    var form = getInfolightOption(infoRefval).form;
    var valueField = getInfolightOption(infoRefval).valueField;
    //var textField = getInfolightOption(infoRefval).textField;
    //var PageSize = getInfolightOption(infoRefval).PageSize;
    var panelWidth = getInfolightOption(infoRefval).panelWidth;
    var title = getInfolightOption(infoRefval).title;
    var remoteName = getInfolightOption(infoRefval).remoteName;
    var tableName = getInfolightOption(infoRefval).tableName;
    var columns = getInfolightOption(infoRefval).columns;
    var checkData = getInfolightOption(infoRefval).checkData;
    var selectOnly = getInfolightOption(infoRefval).selectOnly;
    //var defaultSelect = getInfolightOption(infoRefval).defaultSelect;
    var dialogCenter = getInfolightOption(infoRefval).dialogCenter;
    var capsLock = getInfolightOption(infoRefval).capsLock;
    var fixTextbox = getInfolightOption(infoRefval).fixTextbox;

    if (columns == null || columns.length == 0) {
        var textField = getInfolightOption(infoRefval).textField;
        var valueFieldCaption = getInfolightOption(infoRefval).valueFieldCaption;
        var textFieldCaption = getInfolightOption(infoRefval).textFieldCaption;
        columns = [{
            field: valueField, title: valueFieldCaption, align: 'left', width: 150
        }];
        if (valueField != textField) {
            var textColumn = {
                field: textField, title: textFieldCaption, align: 'left', width: 150
            };
            columns.push(textColumn);
        }
    }
    var onSelect = getInfolightOption(infoRefval).onSelect;
    var onFilter = getInfolightOption(infoRefval).onFilter;

    $(infoRefval).addClass("refval-f").hide();
    var span = $("<span class=\"refval\"></span>").insertAfter(infoRefval);
    var disabled = getInfolightOption(infoRefval).disabled;

    if ((disabled != undefined && disabled == true) || infoRefval.attr('disabled') == 'disabled') {
        disabled = 'disabled="disabled"';
    }


    var infoRefvaltext = $("<input type=\"text\" class=\"refval-text\" " + disabled + ">").appendTo(span);
    if ($(infoRefval).attr('data-options') != undefined && $(infoRefval).attr('data-options').toString().indexOf("required:'required'") >= 0) {
        $(infoRefvaltext).validatebox({
            required: true
        });
    }
    else if (getInfolightOption(infoRefval).required != undefined) {
        if (getInfolightOption(infoRefval).form) {
        }
        else {
            $(infoRefvaltext).validatebox({
                required: true
            });
        }
    }
    var maxlength = $(infoRefval).attr('maxlength');
    if (maxlength != undefined) {
        $(infoRefvaltext).attr('maxlength', maxlength)
    }

    var validateOption = $(infoRefval).attr('data-options');
    if (validateOption != undefined) {
        infoRefvaltext.attr('data-options', validateOption);
    }
    var width = 129;
    if ($(info_refval).width() > 21) {
        if ((getInfolightOption(infoRefval).disabled != null && getInfolightOption(infoRefval).disabled == true) || infoRefval.attr('disabled') == 'disabled')
            width = $(info_refval).width();
        else
            width = $(info_refval).width() - 21;
    }
    if (selectOnly)
        infoRefvaltext.attr('disabled', 'disabled');

    $(infoRefvaltext).width(width);
    if (capsLock) {
        $.changeCapsLock($(infoRefvaltext), capsLock);
    }
    $(infoRefvaltext).focus(function () {
        $(info_refval).refval('showValue');
    });
    if (fixTextbox == "true") {
        $(infoRefvaltext).keyup(function () {
            if (form != undefined) {
                $.gotoNextControl($(infoRefvaltext), "#" + form);
            }
            else {
                $.gotoNextControl($(infoRefvaltext), '.datagrid-row-editing');
            }
        });
    }
    $(infoRefvaltext).blur(function () {
        var value = $(infoRefvaltext).val();
        if (infoRefvaltext.attr('disabled')) {
            return;
        }
        var whereItem = "";
        if (checkData == true) {
            var param = [];
            param.push(remoteName);
            param.push(tableName);
            param.push(valueField);
            whereItem = $(info_refval).refval('getWhereItem');
            param.push(whereItem);

            var valid = $.fn.validatebox.defaults.rules.checkData.validator.call(this, value, param);
            if (valid == false) {
                alert($.fn.validatebox.defaults.rules.checkData.message);
                $(info_refval).refval('setValue', '');
                $(info_refval).refval('showText');
                var textClass = $(info_refval).data('inforefval').refval.find('input').attr('class');
                $(info_refval).data('inforefval').refval.find('input').attr('class', 'refval-text'); //去掉validate防止columnmatch后焦点仍然回到refval
                $(info_refval).refval('doColumnMatch', null);
                $(info_refval).data('inforefval').refval.find('input').attr('class', textClass);
                return;
            }
        }

        $(info_refval).refval('setValue', value);
        $(info_refval).refval('showText');

        var options = getInfolightOption($(info_refval));
        if (options.columnMatches.length > 0) {
            if (whereItem == "") {
                whereItem = $(info_refval).refval('getWhereItem');
            }
            $.fn.Error.errorCode = 1800;

            var row = getDisplayRow(value, options, whereItem);
            var textClass = $(info_refval).data('inforefval').refval.find('input').attr('class');
            $(info_refval).data('inforefval').refval.find('input').attr('class', 'refval-text');  //去掉validate防止columnmatch后焦点仍然回到refval
            $(info_refval).refval('doColumnMatch', row);
            $(info_refval).data('inforefval').refval.find('input').attr('class', textClass);
        }

    });
    //    $(infoRefvaltext).validatebox({
    //        validType: 'checkData["' + remoteName + '","' + tableName + '","' + valueField + '","' + textField + '"]'
    //    });

    //    var infoRefvalquerybutton = $("<input type='button' class='icon-view' plain=\"true\"></input>").appendTo(span);
    //    $(infoRefvalquerybutton).attr('style', "height: 21px; width: 21px; text-align: center;");
    var infoRefvalquerybutton = $("<span type='button' class='icon-view'></span>").appendTo(span);
    $(infoRefvalquerybutton).attr('style', " vertical-align:sub; display:inline-block;width:16px;height:16px;cursor:pointer;");
    disabled = getInfolightOption(infoRefval).disabled;
    if ((disabled != undefined && disabled == true) || infoRefval.attr('disabled') == 'disabled') {
        $(infoRefvalquerybutton).hide();
        //$(infoRefvalquerybutton).attr('disabled', true);
    }

    //$("<input type=\"hidden\" class=\"refval-value\">").appendTo(span);
    var infoRefvalpanel = $("<div class=\"refval-panel\"></div>").appendTo("body");
    infoRefvalpanel.dialog({
        title: title,
        width: panelWidth,
        closed: true,
        modal: true
    });
    var name = $(infoRefval).attr("name");
    if (name) {
        //span.find("input.refval-value").attr("name", name);
        span.find("input.refval-text").attr("name", name);
        $(infoRefval).removeAttr("name").attr("refvalName", name);
    }
    infoRefvaltext.attr("autocomplete", "off");

    var placeholder = getInfolightOption(infoRefval).placeholder;
    if (placeholder != undefined && placeholder != null)
        infoRefvaltext.attr("placeholder", placeholder);

    //    if (columns != undefined && columns.length > 0) {
    //        for (var i = 0; i < columns.length; i++) {
    //            var column = columns[i];
    //            var width = column.width;
    //            width = width - 6;
    //            var columnscript = '<input type="text" name="' + column.field + '" infolight-options="field:\'' + column.field + '\',condition:\'%\',dataType:\'string\'" style="width: ' + width + 'px"></input>';
    //            $(infoRefvalpanel).append(columnscript);
    //        }
    //        $(infoRefvalpanel).append('<input type="button" value="query" class="refval-button" />');
    //        var button = infoRefvalpanel.find("input.refval-button");
    //        $(button).unbind().bind('click', function () {
    //            var queryParams = $(infoRefvalpanel).find('table.refval-grid').datagrid('options').queryParams;
    //            var queryWord = new Object();
    //            var where = '';
    //            $(":text", infoRefvalpanel).each(function () {
    //                var text = $(this);
    //                var value = text.val();
    //                if (value != '') {
    //                    var fieldName = getInfolightOption(text).field;
    //                    if (fieldName != undefined) {
    //                        if (where != '')
    //                            where += ' and ';
    //                        var condition = getInfolightOption(text).condition;
    //                        var dataType = getInfolightOption(text).dataType;
    //                        switch (condition) {
    //                            case '=': where += fieldName + " = " + formatQueryValue(value, dataType); break;
    //                            case '%': where += fieldName + " like '" + value + "%'"; break;
    //                            case '%%': where += fieldName + " like '%" + value + "%'"; break;
    //                            default:
    //                        }
    //                    }
    //                }
    //            });

    //            queryWord.whereString = where;
    //            queryParams.queryWord = $.toJSONString(queryWord);
    //            $(infoRefvalpanel).find('table.refval-grid').datagrid('load');
    //        });
    //    }
    var panelHeight = getInfolightOption(infoRefval).panelHeight;
    if (panelHeight) {
        $(infoRefvalpanel).append('<table class="refval-grid" infolight-options="queryAutoColumn:true" style="height:' + panelHeight + 'px"></table>');
    }
    else {
        $(infoRefvalpanel).append('<table class="refval-grid" infolight-options="queryAutoColumn:true" ></table>');
    }
    var grid = $(infoRefvalpanel).find('table.refval-grid');
    grid.datagrid({}); //防止在onclick之前调用setWhere会报错的问题

    $(infoRefvalquerybutton).unbind().bind('click', function (event) {
        if ($(this).attr("disabled") != undefined && $(this).attr("disabled") == "disabled") {  //兼容firefox
            return;
        }
        var refval = $('.refval-f', this.parentElement.parentElement);
        if (refval.length > 0) {
            var queryParams = grid.datagrid('options').queryParams;
            var whereItem = $(info_refval).refval('getWhereItem');
            if (whereItem != undefined && whereItem != '') {
                var queryWord = {whereString: whereItem};
                queryParams.queryWord = $.toJSONString(queryWord);
                //$(refval).attr('whereString', whereItem)
            }
            grid.datagrid({
                url: getDataUrl(),
                queryParams: getRemoteParam(queryParams, remoteName, tableName, true),
                panelWidth: panelWidth,
                view: defaultview,
                singleSelect: true,
                pagination: true,
                columns: [columns],
                onClickRow: function (rowindex, rowdata) {
                    var value = rowdata[valueField];
                    $(info_refval).refval('setValue', value);
                    if ($(infoRefvaltext).hasClass("validatebox-text")) {  //修复有时选回值时不会刷新validate不过的消息
                        $(infoRefvaltext).validatebox('validate');
                    }
                    $(info_refval).refval('doColumnMatch', rowdata);

                    if (onSelect != undefined) {
                        onSelect.call(info_refval, rowdata);
                    }
                    //eval(onSelect).call(this, rowdata);
                    $(infoRefvalpanel).dialog('close');
                },
                onFilter: onFilter
            });
            var panel = $.data(info_refval, 'inforefval').panel;
            if (dialogCenter != true) {
                var top = 0;
                if ($(this).offset().top > 450 && event.clientY > 450) {
                    top = $(this).offset().top - 450;
                }
                else {
                    top = $(this).offset().top + $(this).height() + 5;
                }

                $(panel).dialog({top: top, left: $(this).offset().left - width});
            }
            else {
                var top = $(document).scrollTop() + ($(window).height() - 250) * 0.5;
                $(panel).dialog({top: top});
            }
            $(panel).dialog('open');
        }
        //$(infoRefvalpanel).dialog('open');
    });


    grid.data("inforefval", {refval: info_refval});
    $.data(info_refval, "inforefval", {refval: span, panel: infoRefvalpanel});
}

function initInfoFileUpload(infofileUpload) {
    var field = getInfolightOption(infofileUpload).field;
    var form = getInfolightOption(infofileUpload).form;
    //var isAutoNum = getInfolightOption(infofileUpload).isAutoNum;
    /* var filter = getInfolightOption(infofileUpload).filter;
     if (filter != null)
     filter = filter.replace(/;/g, '|');*/
    //var UpLoadFolder = getInfolightOption(infofileUpload).upLoadFolder;
    var ShowButton = getInfolightOption(infofileUpload).showButton;
    var ShowLocalFile = getInfolightOption(infofileUpload).showLocalFile;
    //var onSuccess = getInfolightOption(infofileUpload).onSuccess;
    //var onError = getInfolightOption(infofileUpload).onError;
    //var sizeFieldName = getInfolightOption(infofileUpload).sizeFieldName;
    var accept = getInfolightOption(infofileUpload).accept;

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
    $.data(infofileUpload, "infofileupload", {value: infofileUploadvalue, file: infofileUploadfile});

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
    //    var infofileUploadbutton = $("<input type=\"button\" width=\"20px\" value=\"...\">").appendTo(span);
    //    infofileUploadbutton.unbind().bind('click',function(){
    //        $(".info-fileUpload-file",span).each(function(){
    //            $(this).remove(); 
    //        });

    //        var infofileUploadfile = $("<input name=" + field + " id=" + id + " type=\"file\" class=\"info-fileUpload-file\" width='30px' />").appendTo(span);
    //        infofileUploadfile.unbind().bind('change', function () {
    //            var filter = getInfolightOption(infofileUpload).filter;
    //            if(filter != undefined){
    //                var re_text = filter.split("|");
    //                var ext = $(this).val().split(".").pop();
    //                var isext = false;
    //                for (var i = 0; i < re_text.length; i++) {
    //                    if (re_text[i].toLowerCase() == ext) {
    //                        isext = true;
    //                        break;
    //                    }
    //                }
    //                if (!isext) {
    //                    alert("File does not have text(" + re_text + ") extension");
    //                    return false;
    //                }
    //            }
    //        });
    //        infofileUploadfile.click();
    //    });
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
}

function infoFileUploadMethod(infofileUpload, onAfterUploadSuccess, onAfterUploadError) {
    //var fileexist = true;

    var options = getInfolightOption(infofileUpload);
    var onBeforeUpload = options.onBeforeUpload;
    if (onBeforeUpload != null && onBeforeUpload != undefined) {
        var flag = onBeforeUpload.call(infofileUpload, options);
        if (flag != undefined && flag.toString() == 'false') {
            return;
        }
    }
    //var field = options.field;
    var form = options.form;
    var isAutoNum1 = options.isAutoNum;
    var filter1 = options.filter.replace(/;/g, '|');
    var upLoadFolder1 = options.upLoadFolder;
    //var showButton = options.showButton;
    var showLocalFile = options.showLocalFile;
    var onSuccess = options.onSuccess;
    var onError = options.onError;
    var sizeFieldName = options.sizeFieldName;
    var fileSizeLimited = options.fileSizeLimited;

    var infofileUploadvalue = $('.info-fileUpload-value', infofileUpload.next());
    var infofileUploadfile = $('.info-fileUpload-file', infofileUpload.next());
    var fileId = infofileUploadfile.attr('id');
    var fileexist = true;
    var target = $('#' + fileId);
    if (target.length == 0 || target.val() == "") {
        alertMessage("nofile");
        fileexist = false;
    }
    else if (target.val().indexOf("&") != -1) {
        alertMessage("lawname");
        fileexist = false;
    }
    if (fileexist) {
        var postUrl = "";
        if (isSubPath == undefined || isSubPath == true) {
            postUrl = getParentFolder() + "../handler/UploadHandler.ashx";
        }
        else {
            postUrl = getParentFolder() + "handler/UploadHandler.ashx";
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
                        $('#' + fileId).attr('disabled', 'disabled').attr('isalsoreadonly', 'false');
                    }
                    if (sizeFieldName != undefined) {
                        var size = data["size"];
                        var sizefield = {};
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
}

function infoTreeViewDataLoop(data, parentValue, sloopData, id, pid, text) {
    var lng = data.length;
    var i = 0;
    var children = "children";
    for (; i < lng; i++) {
        var pvalue = data[i][pid];
        if (pvalue == parentValue) {
            if (sloopData[children] == undefined) sloopData[children] = [];
            sloopData[children].push({id: data[i][id], pid: data[i][pid], text: data[i][text]});
            infoTreeViewDataLoop(data, data[i][id], loopData, id, pid, text);
        }
    }
}

function initInfoTreeView(treeview) {
    var remoteName = getInfolightOption(treeview).remoteName;
    var tableName = getInfolightOption(treeview).tableName;
    var idField = getInfolightOption(treeview).idField;
    var textField = getInfolightOption(treeview).textField;
    var parentField = getInfolightOption(treeview).parentField;
    var menuID = getInfolightOption(treeview).menuID;
    var onClick = getInfolightOption(treeview).onClick;
    var rootValue = getInfolightOption(treeview).rootValue;
    var checkbox = getInfolightOption(treeview).checkbox;
    $(treeview).tree({
        url: getDataUrl(),
        queryParams: getRemoteParam({}, remoteName, tableName, true),
        checkbox: checkbox,
        loadFilter: function (data) {
            var r = [], /*hash = {},d = {},*/  id = idField, pid = parentField, text = textField, children = "children", i = 0, lng = data.length;
            if ($(this).attr('editMode') == undefined) {
                for (; i < lng; i++) {
                    var pvalue = data[i][pid];
                    if (pvalue == rootValue || ((pvalue == null || pvalue == "") && (rootValue == null || rootValue == ""))) {
                        var topData = {id: data[i][id], pid: data[i][pid], text: data[i][text]};
                        infoTreeViewDataLoop(data, data[i][id], topData, id, pid, text);
                        r.push(topData);

                    }
                }
            }
            else {
                for (; i < lng; i++) {
                    var topData = {id: data[i][id], pid: data[i][pid], text: data[i][text], attributes: data[i]};
                    r.push(topData);
                }
            }
            return r;
        },
        animate: true,
        onClick: eval(onClick),
        //dnd:true,
        onContextMenu: function (e, node) {
            e.preventDefault();
            $(this).tree('select', node.target);
            $(menuID).menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        }
    });
}
function initinfoautocomplete(target) {
    var remoteName = getInfolightOption(target).remoteName;
    var tableName = getInfolightOption(target).tableName;
    var valueField = getInfolightOption(target).valueField;
    var url = getRemoteUrl(remoteName, tableName);
    $(target).combobox({
        valueField: valueField,
        textField: valueField,
        hasDownArrow: false,
        mode: 'remote',
        keyHandler: {
            up: function () {
                var panel = $(this).combo('panel');
                var values = $(this).combo('getValues');
                var item = panel.find('div.combobox-item[value=' + values.pop() + ']');
                if (item.length) {
                    var prev = item.prev(':visible');
                    //var prev = item.prev();
                    if (prev.length) {
                        item = prev;
                    }
                } else {
                    item = panel.find('div.combobox-item:visible:last');
                }
                var value = item.attr('value');

                var opts = $.data(this, 'combobox').options;
                var data = $.data(this, 'combobox').data;

                panel.find('div.combobox-item-selected').removeClass('combobox-item-selected');
                var vv = [], ss = [];
                for (var i = 0; i < [value].length; i++) {
                    var v = [value][i];
                    var s = v;
                    for (var j = 0; j < data.length; j++) {
                        if (data[j][opts.valueField] == v) {
                            s = data[j][opts.textField];
                            break;
                        }
                    }
                    vv.push(v);
                    ss.push(s);
                    panel.find('div.combobox-item[value="' + v + '"]').addClass('combobox-item-selected');
                }

                $(target).combo('setValues', vv);

                for (var i = 0; i < data.length; i++) {
                    if (data[i][opts.valueField] == value) {
                        opts.onSelect.call(target, data[i]);
                        return;
                    }
                }
                scrollTo(target, value);

            },
            down: function () {
                var panel = $(this).combo('panel');
                var values = $(this).combo('getValues');
                var item = panel.find('div.combobox-item[value=' + values.pop() + ']');
                if (item.length) {
                    var next = item.next(':visible');
                    //var next = item.next();
                    if (next.length) {
                        item = next;
                    }
                } else {
                    item = panel.find('div.combobox-item:visible:first');
                }
                var value = item.attr('value');

                var opts = $.data(this, 'combobox').options;
                var data = $.data(this, 'combobox').data;

                panel.find('div.combobox-item-selected').removeClass('combobox-item-selected');
                var vv = [], ss = [];
                for (var i = 0; i < [value].length; i++) {
                    var v = [value][i];
                    var s = v;
                    for (var j = 0; j < data.length; j++) {
                        if (data[j][opts.valueField] == v) {
                            s = data[j][opts.textField];
                            break;
                        }
                    }
                    vv.push(v);
                    ss.push(s);
                    panel.find('div.combobox-item[value="' + v + '"]').addClass('combobox-item-selected');
                }

                $(target).combo('setValues', vv);

                for (var i = 0; i < data.length; i++) {
                    if (data[i][opts.valueField] == value) {
                        opts.onSelect.call(target, data[i]);
                        return;
                    }
                }

                scrollTo(target, value);
            },
            enter: function () {
                var values = $(this).combobox('getValues');
                $(this).combobox('setValues', values);
                $(this).combobox('hidePanel');
            },
            query: function (q) {
                if (q != null && q != "") {
                    var nurl = url + "&whereString=" + valueField + " like '" + encodeURIComponent(q.toString().replace(/\'/g, "''")) + "%'";
                    $(target).combobox("reload", nurl);
                }
                $(target).combobox("setValue", q);
            }
        }
    });
}

function initInfoOptions(target) {
    //var valueField = getInfolightOption(target).valueField;
    //var textField = getInfolightOption(target).textField;
    //var remoteName = getInfolightOption(target).remoteName;
    //var tableName = getInfolightOption(target).tableName;
    var panelWidth = getInfolightOption(target).panelWidth;
    var onSelect = getInfolightOption(target).onSelect;
    var columnCount = getInfolightOption(target).columnCount;
    //var multiSelect = getInfolightOption(target).multiSelect;
    var title = getInfolightOption(target).title;
    //var items = getInfolightOption(target).items;
    var openDialog = getInfolightOption(target).openDialog;
    var selectOnly = getInfolightOption(target).selectOnly;
    var selectAll = getInfolightOption(target).selectAll;

    $(target).addClass("option-f").hide();
    var span = $("<span class='options'></span>").insertAfter(target);
    var disabled = getInfolightOption(target).disabled;
    if ((disabled != undefined && disabled == true) || target.attr('disabled') == 'disabled') {
        disabled = 'disabled="disabled"';
    }
    var optionstext = $("<input type=\"text\" class=\"options-text\" " + disabled + ">").appendTo(span);
    if (getInfolightOption(target).required != undefined && openDialog) {
        if (getInfolightOption(target).validType != undefined && getInfolightOption(target).validType != "") {
            $(optionstext).validatebox({
                validType: getInfolightOption(target).validType,
                required: true
            });
        }
        else {
            $(optionstext).validatebox({
                required: true
            });
        }
    }
    var optionsbutton = $("<span type='button' class='icon-view'></span>").appendTo(span);
    $(optionsbutton).attr('style', " vertical-align:sub; display:inline-block;width:16px;height:16px;cursor:pointer;");
    disabled = getInfolightOption(target).disabled;
    if ((disabled != undefined && disabled == true) || target.attr('disabled') == 'disabled') {
        $(optionsbutton).attr('disabled', "disabled");
    }
    var name = $(target).attr("name");
    if (name) {
        optionstext.attr("name", name);
        $(target).removeAttr("name").attr("refvalName", name);
    }
    var width = 129;
    if ($(target).width() > 21) {
        width = $(target).width() - 21;
    }

    $(optionstext).width(width);
    if (selectOnly)
        $(optionstext).attr('readonly', true);


    var optionspanel = $("<div class=\"options-panel\"></div>").appendTo(span);
    var panelHeight = getInfolightOption(target).panelHeight;
    if (panelHeight) {
        optionspanel.height(panelHeight);
    }
    if (openDialog) {
        optionspanel.dialog({
            title: title,
            width: panelWidth,
            closed: true,
            modal: true
        });
        $(target).data("infooptions", {
            text: optionstext,
            button: optionsbutton,
            panel: optionspanel.find('.panel-body')
        });
        $(optionsbutton).unbind().bind('click', function () {
            if ($(this).attr("disabled") != undefined && $(this).attr("disabled") == "disabled") {  //兼容firefox
                return;
            }
            $(target).options('initializePanel');
            var table = $("table", optionspanel);
            var tr = $("<tr/>").appendTo(table);
            var td = $("<td colspan='" + columnCount + "' align='right' />").appendTo(tr);

            var okText = "OK";
            var selectText = "Select All";
            var unSelectText = "UnSelect All";
            var message = $.sysmsg('getValue', 'Srvtools/WebListBoxList/ButtonCaption');
            if (message) {
                var texts = message.split(';');
                selectText = texts[0];
                unSelectText = texts[1];
                okText = texts[2];
            }
            if (selectAll) {
                $('<a/>').appendTo(td).linkbutton({text: selectText})
                    .click(function () {
                        $(target).options('selectAll');
                    });
                $('<a/>').appendTo(td).linkbutton({text: unSelectText})
                    .click(function () {
                        $(target).options('unSelectAll');
                    });
            }
            $('<a/>').appendTo(td).linkbutton({text: okText})
                .click(function () {
                    $(target).options('setValue', $(target).options('getCheckedValue'));
                    if (onSelect != undefined) {
                        onSelect.call(target, $(target).options('getCheckedValue'));
                    }
                    $(optionspanel).dialog('close');
                });
            $(optionspanel).dialog('open');
        });
    }
    else {
        if (panelHeight) {
            optionspanel.css('overflow', 'scroll');
        }
        $(target).data("infooptions", {text: optionstext, button: optionsbutton, panel: optionspanel});
        optionstext.hide();
        optionsbutton.hide();
        $(target).options('initializePanel');
        if ((disabled != undefined && disabled == true) || target.attr('disabled') == 'disabled') {
            optionspanel.attr('disabled', true);
            optionspanel.find('input').attr('disabled', true);
        }
    }
}

function initInfoFLComment(target) {
    var flowKey = Request.getQueryStringByName("FORM_PRESENTATION");
    target.datagrid({
        //url: '../handler/SystemHandle_Flow.ashx',
        //data: { Type: "gdvHis", LISTID: Request.getFlowStringByName(flowKey, "LISTID") },
        nowrap: false,
        columns: [[
            {field: 'S_STEP_ID', title: $.fn.flcomment.defaults.stepidText, sortable: true, width: 80}, //'作业名称'
            {field: 'USER_ID', title: $.fn.flcomment.defaults.useridText, sortable: true, width: 80}, //'用户编号'
            {field: 'USERNAME', title: $.fn.flcomment.defaults.usernameText, sortable: true, width: 80}, //'寄件者'
            {field: 'STATUS', title: $.fn.flcomment.defaults.statusText, sortable: true, width: 50}, //'情况'
            {
                field: 'REMARK',
                title: $.fn.flcomment.defaults.remarkText,
                sortable: true,
                width: 120,
                formatter: function (value) {
                    return $.convertRemark(value);
                }
            }, //'讯息'
            {field: 'UPDATE_DATE', title: $.fn.flcomment.defaults.updatedateText, sortable: true, width: 80}, //'日期'
            {field: 'UPDATE_TIME', title: $.fn.flcomment.defaults.updatetimeText, sortable: true, width: 60}//'时间'
        ]]
    });
    $.ajax({
        type: "POST",
        url: getParentFolder() + '../handler/SystemHandle_Flow.ashx',
        data: {Type: "gdvHis", LISTID: Request.getFlowStringByName(flowKey, "LISTID")},
        cache: false,
        async: true,
        success: function (data) {
            data = eval(data);
            target.datagrid("loadData", data);
        },
        error: function () {
            return false;
        }
    });
}

function initInfoYearMonth(target) {
    var yearmonth = $(target);
    var options = getInfolightOption(yearmonth);
    var type = options.type;
    var durationMinus = options.durationMinus;
    var durationPlus = options.durationPlus;
    var format = options.format;
    //var datatype = options.datatype;
    var onSelect = options.onSelect;
    var selectOnly = options.selectOnly;
    var panelHeight = options.panelHeight;

    var data = [];
    var date = new Date();
    var yearcur = date.getFullYear();
    var monthcur = date.getMonth() + 1;
    if (type == 'year') {
        for (var i = yearcur - durationMinus; i <= yearcur + durationPlus; i++) {
            data.push({lable: i, value: i});
        }
    }
    else if (type == 'month') {
        var yearMinus = yearcur;
        //var monthMinus = monthcur;
        var overrideMinus = 0;

        var yearPlus = yearcur;
        //var monthPlus = monthcur;
        var overridePlus = 0;

        var monthwhile = monthcur;
        //当前月小月要减少的月份，递归递减12月，yearMinus是最小的年份，overrideMinus是年份减少次数
        while (monthwhile <= durationMinus) {
            yearMinus = yearMinus - 1;
            overrideMinus = overrideMinus + 1;
            monthwhile = monthwhile + 12;
        }
        monthwhile = durationPlus;
        //当前月和要增加的月份和大于12时，增加的月份递归减少12月，yearPlus是最大的年份，overridePlus是年份增加次数
        while (monthcur + monthwhile > 12) {
            yearPlus = yearPlus + 1;
            overridePlus = overridePlus + 1;
            monthwhile = monthwhile - 12;
        }
        //最小年份的起始月份
        var monthMinus = monthcur + 12 * overrideMinus - durationMinus;
        //最大年份的最终月份
        var monthPlus = monthcur + monthwhile;
        //当没有减少年份也没有增加年份的时候，是当年内的递归，需要单独处理
        if (overrideMinus == 0 && overridePlus == 0) {
            var YYY = (yearcur - 1911).toString();
            if (YYY.length == 1) {
                YYY = '00' + YYY;
            }
            else if (YYY.length == 2) {
                YYY = '0' + YYY;
            }

            var YY = ((yearcur - 1911) % 100).toString();
            if (YY.length == 1) {
                YY = '0' + YY;
            }
            var yy = (yearcur % 100).toString();
            if (yy.length == 1) {
                yy = '0' + yy;
            }

            for (var i = monthMinus; i <= monthcur + durationPlus; i++) {
                var month = i;
                if (month.toString().length == 1) {
                    month = '0' + month;
                }
                data.push({
                    label: format.replace(/yyyy/g, yearcur).replace(/yy/g, yy).replace(/mm/g, month).replace(/m/g, i).replace(/YYY/g, YYY).replace(/YY/g, YY),
                    value: yearcur.toString() + i.toString()
                });
            }
        }
        else {
            //先从最早的年开始
            if (overrideMinus > 0) {
                var YYY = (yearMinus - 1911).toString();
                if (YYY.length == 1) {
                    YYY = '00' + YYY;
                }
                else if (YYY.length == 2) {
                    YYY = '0' + YYY;
                }

                var YY = ((yearMinus - 1911) % 100).toString();
                if (YY.length == 1) {
                    YY = '0' + YY;
                }
                var yy = (yearMinus % 100).toString();
                if (yy.length == 1) {
                    yy = '0' + yy;
                }
                //先添加最小的那年的月份，必定是到12月的。
                for (var i = monthMinus; i < 13; i++) {
                    var month = i;
                    if (month.toString().length == 1) {
                        month = '0' + month;
                    }
                    data.push({
                        label: format.replace(/yyyy/g, yearMinus).replace(/yy/g, yy).replace(/mm/g, month).replace(/m/g, i).replace(/YYY/g, YYY).replace(/YY/g, YY),
                        value: yearMinus.toString() + i.toString()
                    });
                }
                //如果减少了2年以上，那么添加最小那年到当年之间所有年份的月份。
                for (var i = 1; i < overrideMinus; i++) {
                    for (var j = 1; j < 13; j++) {
                        var month = j;
                        if (month.toString().length == 1) {
                            month = '0' + month;
                        }
                        data.push({
                            label: format.replace(/yyyy/g, yearMinus + i).replace(/yy/g, yy).replace(/mm/g, month).replace(/m/g, j).replace(/YYY/g, YYY).replace(/YY/g, YY),
                            value: yearMinus.toString() + j.toString()
                        });
                    }
                }
            }
            //处理增加的年份的部分，先处理增加的情况，要添加除了最后那年之外当年和最后那年之间的所有年份。
            if (overridePlus > 0) {
                var YYY = (yearcur - 1911).toString();
                if (YYY.length == 1) {
                    YYY = '00' + YYY;
                }
                else if (YYY.length == 2) {
                    YYY = '0' + YYY;
                }

                var YY = ((yearcur - 1911) % 100).toString();
                if (YY.length == 1) {
                    YY = '0' + YY;
                }
                var yy = (yearcur % 100).toString();
                if (yy.length == 1) {
                    yy = '0' + yy;
                }
                for (var i = 0; i < overridePlus; i++) {
                    for (var j = 1; j < 13; j++) {
                        var month = j;
                        if (month.toString().length == 1) {
                            month = '0' + month;
                        }
                        data.push({
                            label: format.replace(/yyyy/g, yearcur + i).replace(/yy/g, yy).replace(/mm/g, month).replace(/m/g, j).replace(/YYY/g, YYY).replace(/YY/g, YY),
                            value: yearcur.toString() + j.toString()
                        });
                    }
                }
            }
            var YYY = (yearPlus - 1911).toString();
            if (YYY.length == 1) {
                YYY = '00' + YYY;
            }
            else if (YYY.length == 2) {
                YYY = '0' + YYY;
            }

            var YY = ((yearPlus - 1911) % 100).toString();
            if (YY.length == 1) {
                YY = '0' + YY;
            }
            var yy = (yearPlus % 100).toString();
            if (yy.length == 1) {
                yy = '0' + yy;
            }
            //添加最后那年的前月到最终月份的月份。
            for (var i = 1; i <= monthPlus; i++) {
                var month = i;
                if (month.toString().length == 1) {
                    month = '0' + month;
                }
                data.push({
                    label: format.replace(/yyyy/g, yearPlus).replace(/yy/g, yy).replace(/mm/g, month).replace(/m/g, i).replace(/YYY/g, YYY).replace(/YY/g, YY),
                    value: yearPlus.toString() + i.toString()
                });
            }

        }
    }
    yearmonth.combobox({
        valueField: 'value',
        textField: 'label',
        editable: !selectOnly,
        panelHeight: panelHeight,
        onSelect: function (rowdata) {
            if (onSelect != undefined) {
                onSelect.call(this, rowdata);
            }
        },
        data: data
    });

}

function initinfoqrcode(target) {
    /*var render = getInfolightOption(target).render;
     if (render == undefined) render = "table";*/
    //var size = getInfolightOption(target).size;
    //var width = size;
    //var height = size;
    var text = getInfolightOption(target).text;

    if (text != undefined) {
        $(target).infoqrcode('setValue', text);
    }
}
function utf16to8(str) {
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
}

function initinfoschedule(target) {
    var options = getInfolightOption(target);
    var defaultStyle = options.defaultStyle;
    var remoteName = options.remoteName;
    var tableName = options.tableName;
    var dateField = options.dateField;
    var dateToField = options.dateToField;
    var timeFromField = options.timeFromField;
    var timeToField = options.timeToField;
    var titleField = options.titleField;
    var tipField = options.tipField;
    var onBeforeLoad = options.onBeforeLoad;
    var onItemChanged = options.onItemChanged;
    var onItemRemoved = options.onItemRemoved;
    var onItemFormating = options.onItemFormating;
    var dayHourFrom = options.dayHourFrom;
    var dayHourTo = options.dayHourTo;
    var interval = options.interval;
    var allowUpdate = options.allowUpdate;
    var weekSummary = options.weekSummary;
    var dateFormat = options.dateFormat;
    var timeFormat = options.timeFormat;
    $.fn.Error.errorCode = 1500;
    $(target).schedule({
        url: getRemoteUrl(remoteName, tableName, false),
        mode: defaultStyle,
        showStyle: options.showStyle,
        dateField: dateField,
        dateToField: dateToField,
        timeFromField: timeFromField,
        timeToField: timeToField,
        titleField: titleField,
        tipField: tipField,
        dayHourFrom: dayHourFrom,
        dayHourTo: dayHourTo,
        interval: interval,
        editable: allowUpdate,
        weekSummary: weekSummary,
        weekHeight: options.weekHeight,
        monthHeight: options.monthHeight,
        onPrevious: options.onPrevious,
        onNext: options.onNext,
        onSelected: options.onSelected,
        onTimeFormat: options.onTimeFormat,
        onBeforeLoad: function () {
            var dateFrom = $(this).schedule('options').dateFrom;
            var dateTo = $(this).schedule('options').dateTo;

            dateFrom = new Date(dateFrom.getTime());
            var yearFrom = dateFrom.getFullYear().toString();
            var monthFrom = (dateFrom.getMonth() + 1).toString();
            if (monthFrom.length == 1) {
                monthFrom = "0" + monthFrom;
            }
            var dayFrom = dateFrom.getDate().toString();
            if (dayFrom.length == 1) {
                dayFrom = "0" + dayFrom;
            }

            dateTo = new Date(dateTo.getTime() + 1000 * 3600 * 24);
            var yearTo = dateTo.getFullYear().toString();
            var monthTo = (dateTo.getMonth() + 1).toString();
            if (monthTo.length == 1) {
                monthTo = "0" + monthTo;
            }
            var dayTo = dateTo.getDate().toString();
            if (dayTo.length == 1) {
                dayTo = "0" + dayTo;
            }

            $(this).schedule('options').queryWord = {};

            if (dateFormat == 'nvarchar') {
                var whereString = "(";
                whereString += dateField + " >= '" + yearFrom + monthFrom + dayFrom
                    + "' and " + dateField + " < '" + yearTo + monthTo + dayTo + "'";
                if (dateToField) {
                    whereString += " or " + dateToField + " >= '" + yearFrom + monthFrom + dayFrom
                        + "' and " + dateField + " < '" + yearTo + monthTo + dayTo + "'";
                }
                whereString += ")";
                $(this).schedule('options').queryWord.whereString = whereString;
            }
            else {
                var whereString = "(";
                whereString += dateField + " >= " + formatQueryValue(yearFrom + "-" + monthFrom + "-" + dayFrom, 'datetime')
                    + " and " + dateField + " < " + formatQueryValue(yearTo + "-" + monthTo + "-" + dayTo, 'datetime');
                if (dateToField) {
                    whereString += " or " + dateToField + " >= " + formatQueryValue(yearFrom + "-" + monthFrom + "-" + dayFrom, 'datetime')
                        + " and " + dateField + " < " + formatQueryValue(yearTo + "-" + monthTo + "-" + dayTo, 'datetime');
                }
                whereString += ")";
                $(this).schedule('options').queryWord.whereString = whereString;
            }

            if (onBeforeLoad) {
                onBeforeLoad.call(this);
            }
        },
        onItemFormating: onItemFormating,
        onItemChanged: function (e) {
            if (e.date) {
                var year = e.date.getFullYear().toString();
                var month = (e.date.getMonth() + 1).toString();
                if (month.length == 1) {
                    month = "0" + month;
                }
                var day = e.date.getDate().toString();
                if (day.length == 1) {
                    day = "0" + day;
                }
                if (dateFormat == 'nvarchar') {
                    e.item[dateField] = year + month + day;
                }
                else {
                    e.item[dateField] = year + "-" + month + "-" + day;
                }
            }
            if (e.dateTo) {
                var year = e.dateTo.getFullYear().toString();
                var month = (e.dateTo.getMonth() + 1).toString();
                if (month.length == 1) {
                    month = "0" + month;
                }
                var day = e.dateTo.getDate().toString();
                if (day.length == 1) {
                    day = "0" + day;
                }
                if (dateFormat == 'nvarchar') {
                    e.item[dateToField] = year + month + day;
                }
                else {
                    e.item[dateToField] = year + "-" + month + "-" + day;
                }
            }
            if (e.timeFrom) {
                if (timeFormat == 'hhmm') {
                    e.item[timeFromField] = e.timeFrom.replace(':', '');
                }
                else {
                    e.item[timeFromField] = e.timeFrom;
                }
            }
            if (e.timeTo) {
                if (timeFormat == 'hhmm') {
                    e.item[timeToField] = e.timeTo.replace(':', '');
                }
                else {
                    e.item[timeToField] = e.timeTo;
                }
            }


            var changedData = [];
            var changedRows = {tableName: options.tableName, inserted: [], deleted: [], updated: []};
            changedRows.updated.push(e.item);
            changedData.push(changedRows);
            $(target).schedule('disableMenu');
            $.fn.Error.errorCode = 1501;

            $.ajax({
                type: "POST",
                dataType: 'json',
                url: getDataUrl(),
                data: getRemoteParam({data: $.toJSONString(changedData), mode: 'update'}, remoteName, tableName),
                cache: false,
                async: true,
                success: function () {
                    if (onItemChanged) {
                        onItemChanged.call(this, e);
                    }
                },
                complete: function () {
                    $(target).schedule('enableMenu');
                }
            });
        },
        onItemRemoved: function (e) {
            var changedData = [];
            var changedRows = {tableName: options.tableName, inserted: [], deleted: [], updated: []};
            changedRows.deleted.push(e.item);
            changedData.push(changedRows);
            $(target).schedule('disableMenu');
            $.fn.Error.errorCode = 1502;

            $.ajax({
                type: "POST",
                dataType: 'json',
                url: getDataUrl(),
                data: getRemoteParam({data: $.toJSONString(changedData), mode: 'update'}, remoteName, tableName),
                cache: false,
                async: true,
                success: function () {
                    if (onItemRemoved) {
                        onItemRemoved.call(this, e);
                    }
                },
                complete: function () {
                    $(target).schedule('enableMenu');
                }
            });
        }
    });
}
$.fn.BatchMove = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.BatchMove.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            $(this).BatchMove('initialize', methodName);
            if (!$(this).hasClass($.fn.BatchMove.foo)) {
                $(this).addClass($.fn.BatchMove.foo)
            }
        });
    }
};
$.fn.BatchMove.foo = 'info-batchMove';
$.fn.BatchMove.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var cOptions = {};
            var htmlOptions = getInfolightOption($(this)); //html option
            for (var property in htmlOptions) {
                cOptions[property] = htmlOptions[property];
            }
            $(this).data('options', cOptions);
        });
    },

    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return {};
            }
            return $(jq[0]).data('options');
        }
        return {};
    },
    batchMove: function (target) {
        var options = $(target).BatchMove('options');
        var desDataGrid = options.desDataGrid;
        var srcDataGrid = options.srcDataGrid;
        var alwaysInsert = options.alwaysInsert;
        //var alwaysReplace = options.alwaysReplace;
        //var keyMatchColumns = options.keyMatchColumns;
        var matchColumns = options.matchColumns;
        var srcSelectAll = options.srcSelectAll;
        var onEachMove = options.onEachMove;
        var dg = $('#' + desDataGrid);
        var srcDg = $('#' + srcDataGrid);
        if (srcDg.length && dg.length) {
            var srcrows = srcDg.datagrid('getChecked');
            if (srcSelectAll) {
                srcrows = srcDg.datagrid('getRows');
            }
            for (var k = 0; k < srcrows.length; k++) {
                var srcrow = srcrows[k];
                var newRow = {};
                newRow = getDefaultValues(dg, newRow);
                newRow = getSeq(dg, null, newRow);

                for (var i = 0; i < matchColumns.length; i++) {
                    var columns = matchColumns[i];
                    var desColumn = columns.desColumn;
                    var srcColumn = columns.srcColumn;
                    newRow[desColumn] = srcrow[srcColumn];
                }

                if (!alwaysInsert) {
                    var duplicateCheckS = true;
                    var getRows = dg.datagrid("getRows");
                    var keycolumns = dg.attr("keyColumns");
                    var keycolumnssplit = keycolumns.split(',');
                    //var isnotcomplated = true;
                    for (var i = 0; i < getRows.length; i++) {
                        var a = true;
                        for (var j = 0; j < keycolumnssplit.length; j++) {
                            var oldvalue = getRows[i][keycolumnssplit[j]];
                            var newvalue = newRow[keycolumnssplit[j]];
                            if (oldvalue != newvalue) {
                                a = false;
                                break;
                            }
                        }
                        if (a) {
                            duplicateCheckS = false;
                            alertMessage("duplicatecheckmsg");
                            break;
                        }
                    }
                    if (duplicateCheckS) {
                        dg.datagrid('changeState', 'editing');
                        dg.datagrid('appendRow', newRow);
                        if (onEachMove) {
                            onEachMove.call(dg, srcrow);
                        }
                    }

                }
                else {
                    dg.datagrid('changeState', 'editing');
                    dg.datagrid('appendRow', newRow);
                    if (onEachMove) {
                        onEachMove.call(dg, srcrow);
                    }
                }
            }
        }
        else {
        }
    },
    batchMoveOne: function (target) {
        var options = $(target).BatchMove('options');
        var desDataGrid = options.desDataGrid;
        var srcDataGrid = options.srcDataGrid;
        var alwaysInsert = options.alwaysInsert;
        //var alwaysReplace = options.alwaysReplace;
        //var keyMatchColumns = options.keyMatchColumns;
        var matchColumns = options.matchColumns;
        var onEachMove = options.onEachMove;
        var dg = $('#' + desDataGrid);
        var srcDg = $('#' + srcDataGrid);
        if (srcDg.length && dg.length) {
            var srcrow = srcDg.datagrid('getSelected');
            var newRow = {};
            for (var i = 0; i < matchColumns.length; i++) {
                var columns = matchColumns[i];
                var desColumn = columns.desColumn;
                var srcColumn = columns.srcColumn;
                newRow[desColumn] = srcrow[srcColumn];
            }
            if (!alwaysInsert) {
                var duplicateCheckS = true;
                var getRows = dg.datagrid("getRows");
                var keycolumns = dg.attr("keyColumns");
                var keycolumnssplit = keycolumns.split(',');
                //var isold = false;
                for (var i = 0; i < getRows.length; i++) {
                    var a = true;
                    for (var j = 0; j < keycolumnssplit.length; j++) {
                        var oldvalue = getRows[i][keycolumnssplit[j]];
                        var newvalue = newRow[keycolumnssplit[j]];
                        if (oldvalue != newvalue) {
                            a = false;
                            break;
                        }
                    }
                    if (a) {
                        duplicateCheckS = false;
                        alertMessage("duplicatecheckmsg");
                        break;
                    }
                }
                if (duplicateCheckS) {
                    dg.datagrid('changeState', 'editing');
                }
                else {
                    return;
                }
            }

            dg.datagrid('appendRow', newRow);
            if (onEachMove) {
                onEachMove.call(dg, srcrow);
            }
        }
        else {
        }
    },
    Move: function (target) {
        var options = $(target).BatchMove('options');
        var desDataGrid = options.desDataGrid;
        var srcDataGrid = options.srcDataGrid;
        var srcSelectAll = options.srcSelectAll;
        var dg = $('#' + desDataGrid);
        var srcDg = $('#' + srcDataGrid);
        if (srcDg.length && dg.length) {
            if (getInfolightOption(srcDg).multiSelect) {
                $(target).BatchMove('batchMove');
            }
            else {
                if (srcSelectAll) {
                    $(target).BatchMove('batchMove');
                }
                else {
                    $(target).BatchMove('batchMoveOne');
                }
            }
        }
        else {
        }
    },

    getValue: function (target) {
        var options = $(target).BatchMove('options');
    }
};
$.fn.Mail = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.Mail.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            $(this).Mail('initialize', methodName);
            if (!$(this).hasClass($.fn.Mail.foo)) {
                $(this).addClass($.fn.Mail.foo)
            }
        });
    }
};
$.fn.Mail.foo = 'info-mail';
$.fn.Mail.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var cOptions = {};
            var htmlOptions = getInfolightOption($(this)); //html option
            for (var property in htmlOptions) {
                cOptions[property] = htmlOptions[property];
            }
            $(this).data('options', cOptions);
        });
    },

    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return {};
            }
            return $(jq[0]).data('options');
        }
        return {};
    },
    sendMail: function (target, op) {
        $.fn.Error.errorCode = 2200;

        var options = $(target).Mail('options');
        var success = options.sendSuccess;
        var error = options.sendError;
        if (op != undefined) {
            for (var i in op) {
                if (i != "body" && i != "getBodyFunction") {
                    options[i] = op[i];
                }
            }
            if (op.body != undefined) options.body = op.body;
            else if (op.getBodyFunction != undefined) options.body = eval(op.getBodyFunction).call(target);
        }
        options.body = $('<div />').text(options.body).html();
        $.ajax({
            type: "POST",
            url: window.currentUrl,
            dataType: 'text',
            //data: "mode=sendMail&options=" + $.toJSONString(options),
            data: {mode: "sendMail", options: $.toJSONString(options)},
            cache: false,
            async: true,
            success: function (data) {
                if (data == "o") {
                    if (success != "" && eval(success) != undefined) eval(success).call();
                }
                else if (data.indexOf("e:") == 0) {
                    if (error != "" && eval(error) != undefined) eval(error).call(this, data.slice(2));
                }
                data.responseText = "";
            }, error: function (data) {
                alert(data);
                data.responseText = "";
            }
        });
    },
    syncSendMail: function (target, op) {
        $.fn.Error.errorCode = 2201;

        var options = $(target).Mail('options');
        var success = options.sendSuccess;
        var error = options.sendError;
        if (op != undefined) {
            for (var i in op) {
                if (i != "body" && i != "getBodyFunction") {
                    options[i] = op[i];
                }
            }
            if (op.body != undefined) options.body = op.body;
            else if (op.getBodyFunction != undefined) options.body = eval(op.getBodyFunction).call(target);
        }
        options.body = $('<div />').text(options.body).html();
        $.ajax({
            type: "POST",
            url: window.currentUrl,
            dataType: 'text',
            data: {mode: "sendMail", options: $.toJSONString(options)},
            cache: false,
            async: false,
            success: function (data) {
                if (data == "o") {
                    if (success != "" && eval(success) != undefined) eval(success).call(this, options);
                }
                else if (data.indexOf("e:") == 0) {
                    if (error != "" && eval(error) != undefined) eval(error).call(this, data.slice(2), options);
                }
                data.responseText = "";
            }, error: function (data) {
                alert(data);
                data.responseText = "";
            }
        });
    }
};
var isSubPath = true;
var dataGrid_class = ".info-datagrid";
var editIndex_attr = "editIndex";
var readOnly_attr = "readOnly";
var infolightOption_attr = "infolight-options";
var expandFormSysButtonDiv = '<div style="padding: 5px 30px 5px 0; text-align: center"><a href="#" class="easyui-linkbutton l-btn l-btn-plain" iconcls="icon-ok" plain="true" infolight-options="sysButton:\'save\'" id=""><span class="l-btn-left"><span class="l-btn-text icon-ok l-btn-icon-left">Save</span></span></a><a href="#" class="easyui-linkbutton l-btn l-btn-plain" iconcls="icon-cancel" plain="true" infolight-options="sysButton:\'cancel\'" id=""><span class="l-btn-left"><span class="l-btn-text icon-cancel l-btn-icon-left">Cancel</span></span></a></div>';
var databaseType = '';
$(document).ready(function () {
    if (databaseType == '') {
        $.fn.Error.errorCode = 3100;

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
    //set currentLanguage
    var currentLang = navigator.language;   //判断除IE外其他浏览器使用语言
    if (!currentLang) {//判断IE浏览器使用语言
        currentLang = navigator.browserLanguage;
    }
    setlanguage(currentLang);

    //initialize all datagrid
    $(dataGrid_class).each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        var editDialog = getInfolightOption($(this)).editDialog;
        var gridid = this.id;
        var formView = $(getInfolightOption($(editDialog)).containForm);
        var alwaysReadOnly = getInfolightOption(formView).alwaysReadOnly;
        if (alwaysReadOnly && alwaysReadOnly == true) {
            //修改datagrid的pagesize为1，为了前一笔后一笔使用，因为没有前一笔后一笔的算法，只能通过前一页后一页来算
            var value = $(this).attr('data-options');
            var option = {};
            if (value != undefined) {
                var op = "";
                var options = value.split(',');
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

                    var index = op.indexOf(':');
                    if (index > 0) {
                        var pname = op.substr(0, index).replace(/(^\s*)|(\s*$)/g, "");
                        var pvalue = op.substr(index + 1);
                        option[pname] = eval(pvalue);
                    }
                    op = '';
                }
                $(this).attr('data-options', $(this).attr('data-options').replace('pageSize:' + option.pageSize, 'pageSize:1'));
                $(this).attr('data-options', $(this).attr('data-options').replace('pageList:[' + option.pageList.toString() + ']', 'pageList:[1]'));
            }
        }
        var parent = getInfolightOption($(this)).parent;
        var notInitGrid = getInfolightOption($(this)).notInitGrid;
        if (parent == undefined && notInitGrid == undefined) {
            initInfoDataGrid($(this));
        }
        else {
            $(this).datagrid({});
        }
        if (alwaysReadOnly && alwaysReadOnly == true) {
            $(this).datagrid('getPanel').parent().hide();
            $(formView).attr('formOnlyRelationGrid', "#" + gridid);
        }
    });
    $(".info-schedule").each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        initinfoschedule($(this));
    });
    $(".info-combobox").each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            $(this).combobox({});
        }
        else {
            initInfoComboBox($(this));
        }
    });
    $(".info-combogrid").each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            $(this).combogrid({});
        }
        else {
            initInfoComboGrid($(this));
        }
    });
    $(".info-refval").each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        initInfoRefVal(this);
    });
    $(".easyui-dialog").each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        $(this)[0].style.display = "table";
        // var style = $(this).attr('style');
    });
    $(".easyui-window").each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        $(this)[0].style.display = "table";
        $(this).window({});
    });
    $(".info-fileUpload").each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        initInfoFileUpload($(this));
    });
    $('.info-treeview').each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        initInfoTreeView($(this));
    });
    $('.info-autocomplete').each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        initinfoautocomplete($(this));
    });
    $('.info-options').each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        initInfoOptions($(this));
    });
    $('.info-flcomment').each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        initInfoFLComment($(this));
    });
    $('.info-qrcode').each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        initinfoqrcode($(this));
    });
    $(".info-batchMove").each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        $(this).BatchMove({});
    });
    $('.info-mail').each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        $(this).Mail({});
    });
    $('.info-yearmonth').each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        initInfoYearMonth(this);
    });
    $('.info-rotator').each(function () {
        $(this).rotator('slide');
    });
    $(".info-pivottable").each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        $(this).pivotTable({});
    });
    $(dataGrid_class).each(function () {
        if ($(this).closest('.no-preload').length > 0) {
            return true;
        }
        var queryDialog = getInfolightOption($(this)).queryDialog;
        if (queryDialog) {
            if (!$(queryDialog).hasClass('easyui-window')) {
                setQueryDefault(queryDialog);
            }
        }
    });
});

function initControls(container) {
    //initialize all datagrid
    $(dataGrid_class, container).each(function () {
        var parent = getInfolightOption($(this)).parent;
        var notInitGrid = getInfolightOption($(this)).notInitGrid;
        if (parent == undefined && notInitGrid == undefined) {
            initInfoDataGrid($(this));
        }
        else {
            $(this).datagrid({});
        }
    });
    $(".info-schedule", container).each(function () {
        initinfoschedule($(this));
    });
    $(".info-combobox", container).each(function () {
        initInfoComboBox($(this));
    });
    $(".info-combogrid", container).each(function () {
        initInfoComboGrid($(this));
    });
    $(".info-refval", container).each(function () {
        initInfoRefVal(this);
    });
    $(".easyui-dialog", container).each(function () {
        $(this)[0].style.display = "table";
        // var style = $(this).attr('style');
    });
    $(".easyui-window", container).each(function () {
        $(this)[0].style.display = "table";
        $(this).window({});
    });
    $(".info-fileUpload", container).each(function () {
        initInfoFileUpload($(this));
    });
    $('.info-treeview', container).each(function () {
        initInfoTreeView($(this));
    });
    $('.info-autocomplete', container).each(function () {
        initinfoautocomplete($(this));
    });
    $('.info-options', container).each(function () {
        initInfoOptions($(this));
    });
    $('.info-flcomment', container).each(function () {
        initInfoFLComment($(this));
    });
    $('.info-qrcode', container).each(function () {
        initinfoqrcode($(this));
    });
    $(".info-batchMove", container).each(function () {
        $(this).BatchMove({});
    });
    $('.info-mail', container).each(function () {
        $(this).Mail({});
    });
    $(dataGrid_class, container).each(function () {
        var queryDialog = getInfolightOption($(this)).queryDialog;
        if (queryDialog) {
            if (!$(queryDialog).hasClass('easyui-window')) {
                setQueryDefault(queryDialog);
            }
        }
    });
}

function gridDetailFormatter(index) {
    var datagrid = $(this);
    return '<div id="ddv' + datagrid.attr('id') + index + '" class="detailContainer" style="padding:5px 0"></div>';
}

function gridExpandRow(index, rowData) {
    var datagrid = $(this);
    var editDialog = getInfolightOption(datagrid).editDialog;
    var container = $(this).datagrid('getRowDetail', index);
    var detailContainer = $('div.detailContainer', container);
    //var ddvname = '#ddv' + datagrid.attr('id') + index;


    var ddvv;
    //if ($(expand + index, ddvname).length == 0) {
    if ($('.expandContent', detailContainer).length == 0) {
        var expandclone = $(editDialog).clone(true);
        expandclone.attr('id', expandclone.attr('id') + index);
        expandclone.attr('class', 'expandContent');
        expandclone.css('display', '');
        //ddvv = $(ddvname).append(expandclone);
        ddvv = detailContainer.append(expandclone);
        //$(ddvv).append(expandFormSysButtonDiv);
        //$(expand + index, ddvname).show();
        $(ddvv).show();
        //test
        var formData = {};
        for (var p in rowData) {
            formData[p] = rowData[p]
        }
        $('select', ddvv).each(function () {
            if ($.data(this, "combo") != undefined) {
                $(this.parentElement).find('.combo').remove();
                var option = $.data(this, "combo").options;
                //$(this.parentElement).append('<select id="combo' + datagrid.attr('id') + index + '">');
                var newcombo = $('<select id="combo' + datagrid.attr('id') + index + '">').appendTo(this.parentElement);
                newcombo.attr('name', $(this).attr('comboname'));
                newcombo.attr('class', $(this).attr('class'));
                newcombo.attr(infolightOption_attr, "field:\'" + $(this).attr('comboname') + "\',form:\'ddv" + datagrid.attr('id') + index + "\'");
                newcombo.attr('data-options', $(this).attr('data-options'));
                newcombo.attr('style', $(this).attr('style'));
                for (var i = 0; i < this.children.length; i++) {
                    $(this.children[i]).clone(true).appendTo(newcombo);
                }
                if ($.data(this, "combobox") != undefined) {
                    newcombo.combobox(option);
                }
                $(this).remove();
            }

        });
        //qrcode
        $('div', ddvv).each(function () {
            if ($(this).hasClass('info-qrcode')) {
                var qrcode = $(this);
                var options = getInfolightOption(qrcode);
                var field = getInfolightOption(qrcode).field;
                var newqrcode = $('<div id="qrcode' + datagrid.attr('id') + index + '">').appendTo(this.parentElement);
                newqrcode.attr('class', $(this).attr('class'));
                var infolightoptionss = "field:\'" + field + "\',form:\'ddv" + datagrid.attr('id') + index + "\'";
                if (options.size != undefined)
                    infolightoptionss += ",size:" + options.size;
                if (options.render != undefined)
                    infolightoptionss += ",render:" + options.render;
                newqrcode.attr(infolightOption_attr, infolightoptionss);
                newqrcode.attr('data-options', $(this).attr('data-options'));
                newqrcode.attr('style', $(this).attr('style'));
                $(this).remove();
                if (field != undefined) {
                    newqrcode.infoqrcode('setValue', rowData[field]);
                }
            }
        });
        $('input', ddvv).each(function () {
            if ($.data(this, "combo") != undefined) {
                $(this.parentElement).find('.combo').remove();
                var option = $.data(this, "combo").options;
                //$(this.parentElement).append('<select id="combo' + datagrid.attr('id') + index + '">');
                var newcombo = $('<input id="combo' + datagrid.attr('id') + index + '">').appendTo(this.parentElement);
                newcombo.attr('name', $(this).attr('comboname'));
                newcombo.attr('class', $(this).attr('class'));
                newcombo.attr(infolightOption_attr, "field:\'" + $(this).attr('comboname') + "\',form:\'ddv" + datagrid.attr('id') + index + "\'");
                newcombo.attr('data-options', $(this).attr('data-options'));
                newcombo.attr('style', $(this).attr('style'));
                if ($.data(this, "combobox") != undefined)
                    newcombo.combobox(option);
                else if ($.data(this, "datebox") != undefined)
                    newcombo.datebox(option);
                else if ($.data(this, "combogrid") != undefined)
                    newcombo.combogrid(option);
                $(this).remove();
                //置换combo属性的方法不能用，combo有对象可以置换，但是options和panel没有对象置换，赋值是OK了，但是保存出错，并且当第二个展开时下拉框的位置就不正确了
                //$.data(this, 'combo').combo = $(this.parentElement).find('.combo');
                if ($(this).hasClass('easyui-datebox')) {
                    var field = getInfolightOption($(this)).field;
                    var format = getInfolightOption($(this)).format;
                    if (format && format.indexOf('YY') == 0) {
                        $(this).datebox('rocYear', true);
                    }
                    if (format) {
                        formData[field] = getFormatValue(rowData[field], format);
                    }
                }
            }
            else if ($.data(this, "inforefval") != undefined) {
                $(this.parentElement).find('.refval').remove();
                $(this).attr('name', $(this).attr('refvalName'));
                initInfoRefVal(this);
            }


            else if ($(this).hasClass('easyui-numberbox')) {
                var parent = $(this.parentElement);
                var newnumberbox = $('<input id="numberbox' + datagrid.attr('id') + index + '">').appendTo($(this.parentElement));
                newnumberbox.attr('name', $(this).attr('numberboxName'));
                newnumberbox.attr('style', $(this).attr('style'));
                newnumberbox.numberbox($.data(this, 'numberbox').options);
                parent[0].removeChild(parent[0].childNodes[1]);
                $(this).remove();
            }
            else if ($(this).hasClass('easyui-validatebox')) {
                $(this).validatebox($.data(this, 'validatebox').options);
            }
        });
        //end
    }
    else {
        //ddvv = $(expand + index, ddvname);
        ddvv = $('.expandContent', detailContainer);
    }

    $(ddvv).form('load', formData);
    $('input,select,textarea', ddvv).each(function () {
        //if (getInfolightOption($(this)).field != undefined) {
        if ($(this).attr('name') != undefined) {
            var name = $(this).attr('name');
            //var name = getInfolightOption($(this)).field;
            var id = $(this).attr('id');
            if (id != undefined) {
                id = id + datagrid.attr('id') + index;
                $(this).attr('id', id);
            }
            $(this).attr(infolightOption_attr, "field:\'" + name + "\',form:\'ddv" + datagrid.attr('id') + index + "\'");
        }
    });
    $('a', ddvv).each(function () {
        //                        var sysButton = getInfolightOption($(this)).sysButton;
        //                        if (sysButton != undefined) {
        //                            if (sysButton == 'save') {
        //                                var clickString = 'saveItem(' + datagrid.attr('id') + ',' + index + ')';
        //                                $(this).attr('onClick', clickString);
        //                            }
        //                            else if (sysButton == 'cancel') {
        //                                var clickString = 'cancelItem(' + datagrid.attr('id') + ',' + index + ')';
        //                                $(this).attr('onClick', clickString);
        //                            }
        //                        }
        if ($(this).attr('onclick') != undefined && $(this).attr('onclick') != "") {
            var clickString = $(this).attr('onclick');
            if (clickString.indexOf('submitForm') != -1) {
                clickString = 'saveItem(' + datagrid.attr('id') + ',' + index + ')';
                //$(this).attr('onclick', "");
                $(this).unbind('click').removeAttr('onclick'); //好像只有這樣是IE和Crone以及FireFox都可行的修改onclick的方法
                //                                $(this).unbind('click');
                $(this).attr('onclick', clickString);
            }
            else if (clickString.indexOf('closeForm') != -1) {
                clickString = 'cancelItem(' + datagrid.attr('id') + ',' + index + ')';
                //$(this).attr('onclick', "");
                $(this).unbind('click').removeAttr('onclick'); //好像只有這樣是IE和Crone以及FireFox都可行的修改onclick的方法
                //                                $(this).unbind('click');
                $(this).attr('onclick', clickString);
            }
        }
    });
    datagrid.datagrid('fixDetailRowHeight', index);
}

function getEditIndex(dg) {
    if (dg.attr(editIndex_attr) != undefined) {
        return eval(dg.attr(editIndex_attr));
    }
    else {
        return -1;
    }
}

function setEditIndex(dg, index) {
    dg.attr(editIndex_attr, index);
}

function getReadOnly(dg) {
    if (dg.attr(readOnly_attr) != undefined) {
        return dg.attr(readOnly_attr) == "readonly";
    }
    else {
        return false;
    }
}

function setReadOnly(dg, readOnly) {
    dg.attr(readOnly_attr, readOnly);
}

function getSelectedIndex(dg) {
    var selectedRow = dg.datagrid('getSelected');
    if (selectedRow == null) {
        return -1;
    }
    else {
        return dg.datagrid('getRowIndex', selectedRow);
    }
}

function formatValue(val, row) {
    var format = $(this)[0].format;
    if (format != undefined && val != undefined) {
        if (format.toLowerCase().indexOf('image') == 0) {
            return infogridimageformat(val, $(this)[0]);
        }
        else if (format.toLowerCase().indexOf('download') == 0) {
            var folder = "";
            var onclick = "";
            var optionss = format.split(',');
            for (var i = 0; i < optionss.length; i++) {
                if (optionss[i].split(":").length == 2) {
                    var pname = optionss[i].split(":")[0];
                    var pvalue = optionss[i].split(":")[1];
                    if (pname.toLowerCase() == "folder") {
                        folder = pvalue;
                        if (folder.toLowerCase().indexOf("\\") == 0 || folder.toLowerCase().indexOf("/") == 0) {
                            folder = folder.substring(1);
                        }
                        else if (folder.substring(folder.length - 1).toLowerCase() == "\\" || folder.substring(folder.length - 1).toLowerCase() == "/") {
                            folder = folder.substring(0, folder.length - 1);
                        }
                    }
                    if (pname.toLowerCase() == "onclick") {
                        onclick = pvalue;
                    }
                }
            }
            var developer = $('#_DEVELOPERID').val();

            folder = "../" + (developer ? ('preview' + developer + '/') : '') + folder + "/";
            var vpath = folder + val;
            var onclickstring = "";
            if (onclick != "") {
                onclickstring += onclick + "('" + vpath + "');";
            }
            onclickstring += "window.open('" + getParentFolder() + "../handler/JqFileHandler.ashx?File=" + encodeURI(vpath) + "', 'download');";
            return '<a href="#" onclick="' + onclickstring + '">' + val + '</a>';
        }
        else if (format.toLowerCase().indexOf('qrcode') == 0) {
            var size = "60";
            var render = "table";
            var optionss = format.split(',');
            for (var i = 0; i < optionss.length; i++) {
                if (optionss[i].split(":").length == 2) {
                    var pname = optionss[i].split(":")[0];
                    var pvalue = optionss[i].split(":")[1];
                    if (pname.toLowerCase() == "size") {
                        size = pvalue;
                    }
                    if (pname.toLowerCase() == "render") {
                        render = pvalue;
                    }
                }
            }
            return '<div class="info-qrcode" infolight-options="size:' + size + ',render:\'' + render + '\',text:\'' + val + '\'" />';
            //newqrcode.infoqrcode('setValue', val);
        }
        else if (format.toLowerCase().indexOf('drilldown') == 0) {
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
                drillfields += "{field:'" + fieldstring[i] + "',value:'" + row[fieldstring[i]] + "'}";
            }

            return '<a href="#" infolight-options="drillobjectid:\'' + objectID + '\',drillfields:[' + drillfields + ']" onclick="$(this).drilldown(\'load\');">' + val + '</a>';
        }
        else {
            return getFormatValue(val, format);
        }
    }
    return val;
}

function onClickRow(index) {
    if (getReadOnly($(this))) {
        $(this).datagrid('selectRow', index);
        var rowData = $(this).datagrid('getSelected');
        $(this).datagrid('setCurrentRow', rowData);
        return;
    }
    var editDialog = getInfolightOption($(this)).editDialog;
    //var expandForm = getInfolightOption($(this)).expandForm;
    if (editDialog == undefined) {
        var editIndex = getEditIndex($(this));
        if (editIndex == -1) {
            var editOnEnter = getInfolightOption($(this)).editOnEnter;
            if (editOnEnter == undefined || editOnEnter == true) {
                var allowUpdate = getInfolightOption($(this)).allowUpdate;
                if (allowUpdate) {
                    //var lastSelectIndex = getSelectedIndex($(this)); //
                    $(this).datagrid('selectRow', index);
                    var onUpdate = getInfolightOption($(this)).onUpdate;
                    if (onUpdate != undefined) {
                        var rowData = $(this).datagrid('getSelected');
                        var flag = onUpdate.call($(this), rowData);
                        if (flag != undefined && flag.toString() == 'false') {
                            return;
                        }
                    }
                    //record lock
                    var lock = $(this).datagrid('addLock', {type: "updating"});
                    if (!lock) {
                        return false;
                    }
                    //
                    beginEdit($(this), index);
                }
            }
            var rowData = $(this).datagrid('getSelected');
            $(this).datagrid('setCurrentRow', rowData);
        }
        else {
            if (editIndex != index) {
                if (endEdit($(this))) {
                    var autoApply = getInfolightOption($(this)).autoApply; //beginEdit($(this), index);  combobox在autoapply时会出js错误，和applyupdates中的success貌似冲突
                    if (!autoApply) {
                        var editOnEnter = getInfolightOption($(this)).editOnEnter;
                        if (editOnEnter == undefined || editOnEnter == true) {
                            $(this).datagrid('selectRow', index);
                            var onUpdate = getInfolightOption($(this)).onUpdate;
                            if (onUpdate != undefined) {
                                var rowData = $(this).datagrid('getSelected');
                                var flag = onUpdate.call($(this), rowData);
                                if (flag != undefined && flag.toString() == 'false') {
                                    return;
                                }
                            }
                            //record lock
                            var lock = $(this).datagrid('addLock', {type: "updating"});
                            if (!lock) {
                                return;
                            }
                            //
                            beginEdit($(this), index);
                        }
                    }
                    var rowData = $(this).datagrid('getSelected');
                    $(this).datagrid('setCurrentRow', rowData);
                }
                else {
                    $(this).datagrid('selectRow', editIndex);
                }
            }
        }
    }
    else {
        $(this).datagrid('selectRow', index);
        var rowData = $(this).datagrid('getSelected');
        $(this).datagrid('setCurrentRow', rowData);
        //continue模式时同步form的资料
        var editMode = getInfolightOption($(this)).editMode;
        if (editMode.toLowerCase() == 'continue') {
            var formname = getInfolightOption($(editDialog)).containForm;
            var form = $(formname);
            form.attr('continueGrid', "#" + $(this).attr('id'));

            openForm(editDialog, rowData, "viewed", editMode);
        }
    }
}

function beginEdit(dg, index) {
    dg.datagrid('selectRow', index).datagrid('beginEdit', index);
    setEditIndex(dg, index);
    dg.datagrid('changeState', 'editing');

}

function endEdit(dg) {
    var editIndex = getEditIndex(dg);
    //    if (editIndex == -1) { return true }
    //    if (dg.datagrid('validateRow', editIndex)) {
    //        dg.datagrid('endEdit', editIndex);
    var returnvalue = true;
    if (dg.datagrid('validateAll')) {
        dg.datagrid('endEditAll');
        var changedDatas = [];
        var changedRows = {tableName: getInfolightOption(dg).tableName};
        var insertdRows = dg.datagrid("getChanges", "inserted");
        if (insertdRows.length > 0) {
            dg.data('lastInsertedRow', insertdRows[insertdRows.length - 1]);
        }
        if (getInfolightOption(dg).duplicateCheck && insertdRows != null && insertdRows.length > 0) {
            changedRows.inserted = insertdRows;
            changedDatas.push(changedRows);
            var autoApply = getInfolightOption(dg).autoApply;
            var parent = getInfolightOption(dg).parent;
            var duplicateCheckS = true;
            if (parent != undefined && parent != "" && autoApply != undefined && !autoApply) {
                var getRows = dg.datagrid("getRows");
                var keycolumns = dg.attr("keyColumns");
                var keycolumnssplit = keycolumns.split(',');
                //var isold = false;
                var newrow = insertdRows[insertdRows.length - 1];
                for (var i = 0; i < getRows.length - 1; i++) {
                    var a = true;
                    for (var j = 0; j < keycolumnssplit.length; j++) {
                        var oldvalue = getRows[i][keycolumnssplit[j]];
                        var newvalue = newrow[keycolumnssplit[j]];
                        if (oldvalue != newvalue) {
                            a = false;
                            break;
                        }
                    }
                    if (a) {
                        duplicateCheckS = false;
                        break;
                    }
                }
            }
            else {
                //var url = dg.datagrid('options').url;
                $.fn.Error.errorCode = 1001;

                var remoteName = getInfolightOption(dg).remoteName;
                var tableName = getInfolightOption(dg).tableName;
                $.ajax({
                    type: "POST",
                    url: getDataUrl(),
                    data: getRemoteParam({
                        data: $.toJSONString(changedDatas),
                        mode: 'duplicate'
                    }, remoteName, tableName, true),
                    cache: false,
                    async: false,
                    success: function (a) {
                        if (a == "false") {
                            duplicateCheckS = false;
                        }
                    },
                    error: function (data) {
                        duplicateCheckS = false;
                        alert(data);
                    }
                });
            }
            if (duplicateCheckS) {
                setEditIndex(dg, -1);
                if (autoApply) {
                    $.fn.Error.errorCode = 1007;
                    applyUpdates(dg);
                }
                if (dg.datagrid('options').showFooter == true && dg.datagrid('options').pagination == false) {
                    setFooter(dg);
                }
                returnvalue = true;
            }
            else {
                dg.datagrid('beginEdit', editIndex);
                alertMessage("duplicatecheckmsg");
                returnvalue = false;
            }
        }
        else {
            var autoApply = getInfolightOption(dg).autoApply;
            if (autoApply) {
                $.fn.Error.errorCode = 1002;
                applyUpdates(dg);
            }
            setEditIndex(dg, -1); //为了能在保存中取到当前笔的值
            returnvalue = true;
            if (dg.datagrid('options').showFooter == true && dg.datagrid('options').pagination == false) {
                setFooter(dg);
            }
        }
    } else {
        returnvalue = false;
    }
    return returnvalue;
}

$.extend({
    cloneObj: function (obj) {
        // Handle the 3 simple types, and null or undefined
        if (null == obj || "object" != typeof obj) return obj;

        // Handle Date
        if (obj instanceof Date) {
            var copy = new Date();
            copy.setTime(obj.getTime());
            return copy;
        }

        // Handle Array
        if (obj instanceof Array) {
            var copy = [];
            for (var i = 0; i < obj.length; i++) {
                copy[i] = $.cloneObj(obj[i]);
            }
            return copy;
        }

        // Handle Object
        if (obj instanceof Object) {
            var copy = {};
            for (var attr in obj) {
                if (obj.hasOwnProperty(attr)) copy[attr] = $.cloneObj(obj[attr]);
            }
            return copy;
        }

        throw new Error("Unable to copy obj! Its type isn't supported.");
    }
});


function copyItem(dgid) {
    if (getReadOnly($(dgid))) {
        return;
    }
    var rowData = $(dgid).datagrid('getSelected');
    if (rowData == null) {
        return;
    }
    rowData = $.cloneObj(rowData);
    var oldData = $.cloneObj(rowData);
    oldData = $.toJSONString(oldData);
    var keys = $(dgid).attr('keyColumns');
    if (keys != undefined) {
        var keyColumns = keys.split(',');
        for (var i = 0; i < keyColumns.length; i++) {
            rowData[keyColumns[i]] = '';
        }
    }

    var editDialog = getInfolightOption($(dgid)).editDialog;
    var editMode = getInfolightOption($(dgid)).editMode;
    if (editDialog == undefined) {
        if (endEdit($(dgid))) {
            rowData = getSeq($(dgid), null, rowData);
            $(dgid).datagrid('appendRow', rowData);
            var editIndex = $(dgid).datagrid('getRows').length - 1;
            beginEdit($(dgid), editIndex);
            $(dgid).datagrid('setCurrentRow', null);
        }
    }
    else {
        if (editMode.toLowerCase() == 'dialog') {
            var formname = getInfolightOption($(editDialog)).containForm;
            var form = $(formname);
            form.attr('dialogGrid', dgid);
            form.attr('copyItem', oldData);
            rowData = getSeq($(dgid), form, rowData);
            openForm(editDialog, rowData, "inserted", editMode);
        }
        else if (editMode.toLowerCase() == "switch") {
            $(dgid).datagrid('getPanel').panel('collapse');
            var formname = getInfolightOption($(editDialog)).containForm;
            var form = $(formname);
            //hide submit div
            if (getInfolightOption($(dgid)).CollapseDiv != undefined) {
                $(getInfolightOption($(dgid)).CollapseDiv).each(function () {
                    $(this).css('display', 'none');
                });
            }
            else if ($("#" + $(dgid).attr('id') + "-SubmitDiv") != undefined) {
                $("#" + $(dgid).attr('id') + "-SubmitDiv").css('display', 'none');
            }
            //end
            form.attr("switchGrid", dgid);
            form.attr('copyItem', oldData);
            rowData = getSeq($(dgid), form, rowData);
            openForm(editDialog, rowData, "inserted", editMode);
        }
        else if (editMode.toLowerCase() == 'continue') {
            var formname = getInfolightOption($(editDialog)).containForm;
            var form = $(formname);
            form.attr('continueGrid', dgid);
            rowData = getSeq($(dgid), form, rowData);
            openForm(editDialog, rowData, "inserted", editMode);
        }
        else if (editMode.toLowerCase() == 'expand') {
            if (endEdit($(dgid))) {
                rowData = getSeq($(dgid), $(editDialog), rowData);
                $(dgid).datagrid('appendRow', rowData);
                var editIndex = $(dgid).datagrid('getRows').length - 1;
                beginEdit($(dgid), editIndex);
                $(dgid).datagrid('expandRow', editIndex);
                $(dgid).datagrid('fixRowHeight');
            }
        }
    }
}

function copyDetailItem(dgid) {
    if (getReadOnly($(dgid))) {
        return;
    }
    var remoteName = getInfolightOption($(dgid)).remoteName;
    var tableName = getInfolightOption($(dgid)).tableName;
    var parentRelations = getInfolightOption($(dgid)).parentRelations;

    var formid = $('#' + getInfolightOption($(dgid)).parent);
    if (formid != undefined && formid.attr('copyItem') != undefined) {
        var newObject = [];
        var newValue = [];
        for (var i = 0; i < parentRelations.length; i++) {
            newObject.push(parentRelations[i].field);
        }
        $('input,select,textarea', $(formid)).each(function () {
            var field = getInfolightOption($(this)).field;
            if (newObject.indexOf(field) != -1) {
                var formid = getInfolightOption($(this)).form;
                if (formid != undefined && field != undefined) {
                    var value = "";
                    var controlClass = $(this).attr('class');
                    if (controlClass != undefined) {
                        if (controlClass.indexOf('easyui-datebox') == 0) {
                            value = $(this).datebox('getBindingValue');
                        }
                        else if (controlClass.indexOf('easyui-combobox') == 0) {
                            value = $(this).combobox('getValue');
                        }
                        else if (controlClass.indexOf('easyui-datetimebox') == 0) {
                            value = $(this).datetimebox('getBindingValue');
                        }
                        else if (controlClass.indexOf('easyui-combogrid') == 0) {
                            value = $(this).combogrid('getValue');
                        }
                        else if (controlClass.indexOf('info-combobox') == 0) {
                            value = $(this).combobox('getValue');
                        }
                        else if (controlClass.indexOf('info-combogrid') == 0) {
                            value = $(this).combogrid('getValue');
                        }
                        else if (controlClass.indexOf('info-refval') == 0) {
                            value = $(this).refval('getValue');
                        }
                        else if (controlClass.indexOf('info-options') == 0) {
                            value = $(this).options('getValue');
                        }
                        else if (controlClass.indexOf('info-autocomplete') == 0) {
                            value = $(this).combobox('getValue');
                        }
                        else {
                            value = $(this).val();
                        }
                    }
                    else {
                        if ($(this).attr('type') == "checkbox") {
                            value = $(this).checkbox('getValue');
                        }
                        else {
                            value = $(this).val();
                        }
                    }
                    newValue.push(value);
                }
            }
        });
        var options = getInfolightOption($(formid));
        var parentTableName = options.tableName;
        $.fn.Error.errorCode = 3200;
        if (parentTableName != undefined) {
            var queryWord = {parentTableName: parentTableName, parentRow: $.parseJSON(formid.attr('copyItem'))};
            $.ajax({
                type: "POST",
                url: getDataUrl(),
                data: getRemoteParam({queryWord: $.toJSONString(queryWord)}, remoteName, tableName, true),
                cache: false,
                async: false,
                success: function (data) {
                    if (data != null) {
                        var rows = $.parseJSON(data);
                        if (rows.total > 0) {
                            var rows = rows.rows;
                            for (var i = 0; i < rows.length; i++) {
                                var rowData = $.cloneObj(rows[i]);

                                for (var j = 0; j < newObject.length; j++) {
                                    rowData[newObject[j]] = newValue[j];
                                }
                                rowData = getSeq($(dgid), null, rowData);
                                $(dgid).datagrid('appendRow', rowData);
                                $(dgid).datagrid('changeState', 'editing');
                            }
                        }
                    }
                }
            });
        }
    }
}
function insertItem(dgid) {
    if (getReadOnly($(dgid))) {
        return;
    }

    var onInsert = getInfolightOption($(dgid)).onInsert;
    if (onInsert != undefined) {
        var flag = onInsert.call($(dgid), null);
        if (flag != undefined && flag.toString() == 'false') {
            return;
        }
    }

    var editDialog = getInfolightOption($(dgid)).editDialog;
    var editMode = getInfolightOption($(dgid)).editMode;
    if (editDialog == undefined) {
        if (endEdit($(dgid))) {
            setTimeout(function () {
                $.fn.Error.errorCode = 1005;
                var rowData = getDefaultValues($(dgid));
                if (rowData == null) {
                    return;
                }
                rowData = getSeq($(dgid), null, rowData);
                $(dgid).datagrid('appendRow', rowData);
                var editIndex = $(dgid).datagrid('getRows').length - 1;
                beginEdit($(dgid), editIndex);
                $(dgid).datagrid('setCurrentRow', null);
            }, 200);
        }
    }
    else {
        var formname = getInfolightOption($(editDialog)).containForm;
        var form = $(formname);
        if (editMode.toLowerCase() == 'dialog') {
            //var rowData = getDefaultValues($(dgid));
            form.attr('dialogGrid', dgid);
            var rowData = getSeq($(dgid), form, {});
            openForm(editDialog, rowData, "inserted", editMode);
        }
        else if (editMode.toLowerCase() == "switch") {
            $(dgid).datagrid('getPanel').panel('collapse');
            //hide submit div
            if (getInfolightOption($(dgid)).CollapseDiv != undefined) {
                $(getInfolightOption($(dgid)).CollapseDiv).each(function () {
                    $(this).css('display', 'none');
                });
            }
            else if ($("#" + $(dgid).attr('id') + "-SubmitDiv") != undefined) {
                $("#" + $(dgid).attr('id') + "-SubmitDiv").css('display', 'none');
            }
            //end
            form.attr("switchGrid", dgid);
            var rowData = getSeq($(dgid), form, {});
            openForm(editDialog, rowData, "inserted", editMode);
        }
        else if (editMode.toLowerCase() == 'continue') {
            form.attr('continueGrid', dgid);
            var rowData = getSeq($(dgid), form, {});
            openForm(editDialog, rowData, "inserted", editMode);
        }
        else if (editMode.toLowerCase() == 'expand') {
            if (endEdit($(dgid))) {
                var rowData = getSeq($(dgid), $(editDialog), {});
                rowData = getDefaultValues($(editDialog), rowData);
                //var rowData = getDefaultValues($(dgid));
                //rowData = getSeq($(dgid), null, rowData);
                $(dgid).datagrid('appendRow', rowData);
                var editIndex = $(dgid).datagrid('getRows').length - 1;
                beginEdit($(dgid), editIndex);
                var container = $(dgid).datagrid('getRowDetail', editIndex);
                var detailContainer = $('div.detailContainer', container);
                setEditMode(detailContainer, "inserted");

                $(dgid).datagrid('expandRow', editIndex);
                $(dgid).datagrid('fixRowHeight');

            }
        }
    }
}


function deleteItem(dgid) {
    if (getReadOnly($(dgid))) {
        return;
    }

    var selectedIndex = getSelectedIndex($(dgid));
    if (selectedIndex == -1) {
        return;
    }

    var onDelete = getInfolightOption($(dgid)).onDelete;
    if (onDelete != undefined) {
        var rowData = $(dgid).datagrid('getSelected');
        var flag = onDelete.call($(dgid), rowData);
        if (flag != undefined && flag.toString() == 'false') {
            return;
        }
    }

    if (confirm(deleteMessage)) {
        var rowData = $(dgid).datagrid('getSelected');

        //record lock
        var lock = $(dgid).datagrid('addLock', {type: "deleting"});
        if (!lock) {
            return false;
        }
        //
        $(dgid).datagrid('deleteRow', selectedIndex);
        var onDeleting = getInfolightOption($(dgid)).onDeleting;
        if (onDeleting) {
            onDeleting.call($(dgid), rowData);
        }

        var autoApply = getInfolightOption($(dgid)).autoApply;
        if (autoApply) {
            $.fn.Error.errorCode = 1003;
            applyUpdates($(dgid));
        }
        else {
            $(dgid).datagrid('changeState', 'editing');
        }
        if ($(dgid).datagrid('options').showFooter == true && $(dgid).datagrid('options').pagination == false) {
            setFooter($(dgid));
        }
        $(dataGrid_class).each(function () {
            var multiSelectGrid = getInfolightOption($(this)).multiSelectGrid;
            if (multiSelectGrid != undefined && multiSelectGrid == '#' + $(dgid).attr('id')) {
                $(this).datagrid('reload');
            }
        });
    }
}

function updateItem(dgid) {
    if (getReadOnly($(dgid))) {
        return;
    }

    var selectedIndex = getSelectedIndex($(dgid));
    if (selectedIndex == -1) {
        return;
    }

    var onUpdate = getInfolightOption($(dgid)).onUpdate;
    if (onUpdate != undefined) {
        var flag = onUpdate.call($(dgid), $(dgid).datagrid('getSelected'));
        if (flag != undefined && flag.toString() == 'false') {
            return;
        }
    }
    var rowData = $(dgid).datagrid('getSelected');
    //record lock
    var lock = $(dgid).datagrid('addLock', {type: "updating"});
    if (!lock) {
        return false;
    }
    else if (typeof lock == 'object') {
        rowData = lock;
    }
    //

    var editDialog = getInfolightOption($(dgid)).editDialog;
    var editMode = getInfolightOption($(dgid)).editMode;

    if (editDialog == undefined) {
        beginEdit($(dgid), selectedIndex);
    }
    else {
        var formname = getInfolightOption($(editDialog)).containForm;
        var form = $(formname);
        if (editMode.toLowerCase() == "switch") {
            $(dgid).datagrid('getPanel').panel('collapse');

            form.attr("switchGrid", dgid);
            //hide submit div
            if (getInfolightOption($(dgid)).CollapseDiv != undefined) {
                $(getInfolightOption($(dgid)).CollapseDiv).each(function () {
                    $(this).css('display', 'none');
                });
            }
            else if ($("#" + $(dgid).attr('id') + "-SubmitDiv") != undefined) {
                $("#" + $(dgid).attr('id') + "-SubmitDiv").css('display', 'none');
            }
            //end
        }
        else if (editMode.toLowerCase() == "dialog") {
            form.attr('dialogGrid', dgid);
        }
        else if (editMode.toLowerCase() == 'continue') {
            var dataFormTabID = getInfolightOption($(editDialog)).dataFormTabID;
            if (dataFormTabID != undefined) {
                var tabs = $(editDialog).closest('.easyui-tabs');
                if (tabs != undefined) {
                    var tabslist = $(tabs).tabs('tabs');
                    for (var i = 0; i < tabslist.length; i++) {
                        if ($(tabslist[i]).attr('id') == dataFormTabID) {
                            $(tabs).tabs('select', i);
                        }
                    }
                }
            }
            form.attr('continueGrid', dgid);
        }
        openForm(editDialog, rowData, "updated", editMode, $(dgid).attr('keyColumns'));
    }
}

function viewItem(dgid) {
    var selectedIndex = getSelectedIndex($(dgid));
    if (selectedIndex == -1) {
        return;
    }
    var editDialog = getInfolightOption($(dgid)).editDialog;
    var editMode = getInfolightOption($(dgid)).editMode;
    if (editDialog != undefined) {
        var rowData = $(dgid).datagrid('getSelected');
        var formname = getInfolightOption($(editDialog)).containForm;
        var form = $(formname);
        if (editMode.toLowerCase() == "switch") {
            $(dgid).datagrid('getPanel').panel('collapse');

            form.attr("switchGrid", dgid);
            //hide submit div
            if (getInfolightOption($(dgid)).CollapseDiv != undefined) {
                $(getInfolightOption($(dgid)).CollapseDiv).each(function () {
                    $(this).css('display', 'none');
                });
            }
            else if ($("#" + $(dgid).attr('id') + "-SubmitDiv") != undefined) {
                $("#" + $(dgid).attr('id') + "-SubmitDiv").css('display', 'none');
            }
            //end
        }
        else if (editMode.toLowerCase() == "dialog") {
            form.attr('dialogGrid', dgid);
        }
        else if (editMode.toLowerCase() == 'continue') {
            form.attr('continueGrid', dgid);
        }
        openForm(editDialog, rowData, "viewed", editMode);
    }
}

function ok(dgid) {
    endEdit($(dgid));
}

function openImport(dgid, row, cell) {
    //create 
    var importWindow = $("#importWindow", "body");
    if (importWindow.length == 0) {

        $("body").append('<div id="importWindow"/>');
        importWindow = $("#importWindow", "body");
    }
    if (row == undefined) {
        row = 0;
    }
    if (cell == undefined) {
        cell = 0;
    }
    importWindow.window({
        title: 'Import',
        iconCls: 'icon-view',
        collapsible: false,
        minimizable: false,
        maximizable: false,
        resizable: false,
        href: '../InnerPages/Upload.aspx',
        width: 400,
        height: 80,
        modal: true,
        onLoad: function () {
            $(".easyui-linkbutton", this).data("datagrid", dgid);
            $(".easyui-linkbutton", this).data("row", row);
            $(".easyui-linkbutton", this).data("cell", cell);
        }
    });
    importWindow.window('open');
}

function pagerFilter(data) {
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

function defaultFilter(data) {
    return data;

}
function importGrid() {
    var dgid = $(".easyui-linkbutton", "#importWindow").data("datagrid");
    var row = $(".easyui-linkbutton", "#importWindow").data("row");
    var cell = $(".easyui-linkbutton", "#importWindow").data("cell");
    var remoteName = getInfolightOption($(dgid)).remoteName;
    var tableName = getInfolightOption($(dgid)).tableName;
    $.fn.Error.errorCode = 1060;

    $.ajaxFileUpload({
        url: getDataUrl(), //需要链接到服务器地址   
        secureuri: false,
        data: getRemoteParam({mode: "import", beginrow: row, begincell: cell}, remoteName, tableName, true),
        fileElementId: "importFileUpload", //文件选择框的id属性   
        dataType: 'json', //服务器返回的格式，可以是json   
        success: function (data) {
            if (typeof data == "string") {
                $.messager.alert('Error', data, 'error');
            }
            else {
                var pagination = $(dgid).datagrid('options').pagination;
                $(dgid).data('pagination', pagination).data('imported', true).datagrid({
                    url: '',
                    pagination: false
                }).datagrid('loadData', []);
                for (var i = 0; i < data.rows.length; i++) {
                    $(dgid).datagrid('appendRow', data.rows[i]);
                }
                //                $(dgid).datagrid({ loadFilter: pagerFilter, url: '' }).datagrid('loadData', data);
                //                $(dgid).data("datagrid").insertedRows = data.rows;
                $(dgid).datagrid('changeState', 'editing');
                $("#importWindow", "body").window('close');
            }
        },
        error: function (data) {
            //            if (typeof data == "string") {
            //                var message = data;
            //                $.messager.alert('Error', message, 'error');
            //            }

        }
    });

}

function pasteDatasFromExcel(dgid) {
    var grid = $(dgid);
    var cname = $.sysmsg('getValue', 'JQWebClient/pasteCaption').split(";");
    var div = $('<div></div>').appendTo('body');
    div.dialog({
        title: 'paste ',
        width: 250,
        height: 'auto',
        closed: false,
        cache: false,
        modal: true,
        onclose: function () {
            div.remove();
        }
    });
    var div1 = $('<div class="designer-print"></div>').appendTo(div);
    $('<label>' + cname[0] + '</label >').appendTo(div1);
    var columnIndext = $("<input type=\"text\" value ='1' />").appendTo(div1).css('width', '30px');
    $('<br/>').appendTo(div1);
    $('<br/>').appendTo(div1);
    $('<label>' + cname[1] + '</label >').appendTo(div1);
    var insertornott = $("<input checked=\"checked\" type=\"checkbox\" />").appendTo(div1);
    $('<br/>').appendTo(div1);
    $('<br/>').appendTo(div);

    var okbutton = $('<a href="#" value="OK" ></a>').appendTo(div);
    var canclebutton = $('<a href="#" value="Close" ></a>').appendTo(div);

    okbutton.linkbutton({
        iconCls: 'icon-ok'
    });
    canclebutton.linkbutton({
        iconCls: 'icon-cancel'
    });

    okbutton.unbind().bind('click', function () {
        var columnindex = parseInt($(columnIndext).val()) - 1;
        var insertornot = insertornott.attr('checked') == "checked" ? "insert" : "update";
        div.dialog('close');
        grid.datagrid('pasteDatasFromExcelExcute', {columnIndex: columnindex, mode: insertornot});
    });
    canclebutton.unbind().bind('click', function () {
        div.dialog('close');
    });

    div.dialog('open');
}
function exportGrid(dgid) {
    $.fn.Error.errorCode = 1070;
    var title = $(dgid).datagrid('options').title;
    var columns = [];
    var fields = $(dgid).datagrid('getColumnFields', true);
    for (var i = 0; i < fields.length; i++) {
        var field = fields[i];
        var option = $(dgid).datagrid('getColumnOption', field);
        if (!option.hidden) {
            var column = {field: field, title: option.title};

            if (option.editor != undefined && (option.editor.type == 'infocombobox' || option.editor.type == 'infocombogrid' || option.editor.type == 'inforefval')) {
                column.options = {};
                column.options.remoteName = option.editor.options.remoteName;
                column.options.tableName = option.editor.options.tableName;
                column.options.valueField = option.editor.options.valueField;
                column.options.textField = option.editor.options.textField;
                column.options.items = option.editor.options.items;

            }
            columns.push(column);
        }
    }
    fields = $(dgid).datagrid('getColumnFields');
    for (var i = 0; i < fields.length; i++) {
        var field = fields[i];
        var option = $(dgid).datagrid('getColumnOption', field);
        if (!option.hidden) {
            var column = {field: field, title: option.title};
            if (option.editor != undefined && (option.editor.type == 'infocombobox' || option.editor.type == 'infocombogrid' || option.editor.type == 'inforefval')) {
                column.options = {};
                column.options.remoteName = option.editor.options.remoteName;
                column.options.tableName = option.editor.options.tableName;
                column.options.valueField = option.editor.options.valueField;
                column.options.textField = option.editor.options.textField;
                column.options.items = option.editor.options.items;
            }
            columns.push(column);
        }
    }

    for (var i = 0; i < columns.length; i++) {
        if (columns[i].field) {
            $("th", $(dgid)).each(function () {
                var field = getInfolightOption($(this)).field;
                if (field == columns[i].field) {
                    var total = getInfolightOption($(this)).total;
                    if (total) {
                        columns[i].total = total;
                    }
                    return false;
                }
            });
        }
    }


    var remoteName = getInfolightOption($(dgid)).remoteName;
    var tableName = getInfolightOption($(dgid)).tableName;
    var queryWord = $(dgid).datagrid('options').queryParams.queryWord;
    $.fn.Error.errorCode = 1071;
    $.ajax({
        type: "POST",
        url: getDataUrl(),
        data: getRemoteParam({ mode: 'export', title: title, columns: $.toJSONString(columns), queryWord: queryWord }, remoteName, tableName, true),
        cache: false,
        async: false,
        success: function (data) {
            window.open(getParentFolder() + '../handler/JqFileHandler.ashx?File=' + data, 'download');
        }
    });
}
function exportPDF(dgid, remoteName, tableName, reportFileName, whereString, options) {
    if (options == undefined) {
        options = {};
    }
    options.pdf = true;
    exportReport(dgid, remoteName, tableName, reportFileName, whereString, options);
}
function exportWord(dgid, remoteName, tableName, reportFileName, whereString, options) {
    if (options == undefined) {
        options = {};
    }
    options.word = true;
    exportReport(dgid, remoteName, tableName, reportFileName, whereString, options);
}
function exportExcel(dgid, remoteName, tableName, reportFileName, whereString, options) {
    if (options == undefined) {
        options = {};
    }
    options.excel = true;
    exportReport(dgid, remoteName, tableName, reportFileName, whereString, options);
}
function exportReport(dgid, remoteName, tableName, reportFileName, whereString, options) {
    $.fn.Error.errorCode = 1080;
    var RemoteName = "";
    var TableName = "";
    var ReportFileName = "";
    var WhereString = "";
    var WhereTextString = "";
    var DataSetName = "";
    if ($(dgid) != undefined && $(dgid).length != 0) {
        RemoteName = getInfolightOption($(dgid)).remoteName;
        TableName = getInfolightOption($(dgid)).tableName;
        DataSetName = TableName;
        ReportFileName = getInfolightOption($(dgid)).reportFileName;
        var queryParams = $(dgid).datagrid('options').queryParams;
        if (queryParams.queryWord != "") {
            var queryWord = eval('(' + queryParams.queryWord + ')');
            if (queryWord != undefined && queryWord != null)
                WhereString = queryWord.whereString;
        }
        //WhereString = $(dgid).datagrid('getWhere');
        WhereTextString = $(dgid).datagrid('getWhereText');
    }
    if (remoteName != undefined && remoteName != "undefined" && remoteName != "")
        RemoteName = remoteName;
    if (tableName != undefined && tableName != "undefined" && tableName != "")
        TableName = tableName;
    if (reportFileName != undefined && reportFileName != "undefined" && reportFileName != "")
        ReportFileName = reportFileName;
    if (whereString != undefined && whereString != "undefined" && whereString != "")
        WhereString = whereString;
    if (options && options.DataSetName) {
        DataSetName = options.DataSetName;
    }
    if (RemoteName != "" && TableName != "" && ReportFileName != "") {
        if (ReportFileName.toLowerCase().indexOf('.rdlc') > 0) { //jquery 或者 sd 使用reportName
            var developer = $('#_DEVELOPERID').val();
            if (developer) {
                ReportFileName = 'preview' + developer + '/' + ReportFileName;
            }
            var url = "../ReportViewerTemplate.aspx?RemoteName=" + RemoteName + "&TableName=" + TableName + "&ReportPath=" + ReportFileName + "&WhereString=" + encodeURIComponent(WhereString) + "&WhereTextString=" + WhereTextString;
            url += "&DataSetName=" + DataSetName;
            if (options) {
                if (options.Parameters) {
                    for (var p in options.Parameters) {
                        url += "&RP" + p + "=" + options.Parameters[p];
                    }
                }
                if (options.SP) {
                    url += "&SP=" + options.SP;
                }
                if (options.SPParam) {
                    url += "&SPParam=" + options.SPParam;
                }
                if (options.AssemblyName) {
                    url += "&AssemblyName=" + options.AssemblyName;
                }
                if (options.MethodName) {
                    url += "&MethodName=" + options.MethodName;
                }
                if (options.pdf) {
                    url += "&pdf=true";
                }
                if (options.word) {
                    url += "&word=true";
                }
                if (options.excel) {
                    url += "&excel=true";
                }
                if (options.reportname) {
                    url += "&reportname=" + options.reportname;
                }
            }
            var height = $(window).height() - 20;
            var width = $(window).width() - 20;
            if ($.browser.msie) {
                window.open(url, '_blank', 'scrollbars=yes, resizable=yes, location=no, width=' + width + ', height=' + height);
            }
            else {
                var dialog = $('<div/>')
                    .dialog({
                        draggable: false,
                        modal: true,
                        height: height,
                        width: width,
                        title: "Report"//,
                        //maximizable: true
                    });
                $('<iframe style="border: 0px;" src="' + url + '" width="100%" height="95%"></iframe>').appendTo(dialog.find('.panel-body'));
                dialog.dialog('open');
            }
        }
        else { //sd 不使用reportName
            $.fn.Error.errorCode = 1081;
            var DataSetName = options && options.DataSetName ? options.DataSetName : '';
            var HeaderDataSetName = options && options.HeaderDataSetName ? options.HeaderDataSetName : '';
            var HeaderTableName = options && options.HeaderTableName ? options.HeaderTableName : '';
            $.ajax({
                type: 'post',
                dataType: 'text',
                url: getParentFolder() + '../handler/SystemHandler.ashx?type=Menu',
                data: {
                    mode: 'Run',
                    id: ReportFileName,
                    type: 'report',
                    remoteName: RemoteName,
                    whereString: WhereString,
                    whereTextString: WhereTextString,
                    DataSetName: DataSetName,
                    HeaderDataSetName: HeaderDataSetName,
                    HeaderTableName: HeaderTableName
                },
                async: true,
                success: function (url) {
                    url = '../' + url;
                    var height = $(window).height() - 20;
                    var width = $(window).width() - 20;
                    if ($.browser.msie) {
                        window.open(url, '_blank', 'scrollbars=yes, resizable=yes, location=no, width=' + width + ', height=' + height);
                        //window.showModalDialog(url, '', 'dialogHeight=' + height + 'px;dialogWidth=' + width + 'px;location=no;');
                    }
                    else {
                        var dialog = $('<div/>')
                            .dialog({
                                draggable: false,
                                modal: true,
                                height: height,
                                width: width,
                                title: "Report"//,
                                //maximizable: true
                            });
                        $('<iframe style="border: 0px;" src="' + url + '" width="100%" height="100%"></iframe>').appendTo(dialog.find('.panel-body'));
                        //dialog.dialog('maximize');
                        dialog.dialog('open');
                    }
                },
                complete: function () {
                    $.messager.progress('close');
                }
            });
        }
    }
    //    var url = "../handler/jqDataHandle.ashx?RemoteName=S001.Master&TableName=Master";
    //    $.ajax({
    //        type: "POST",
    //        url: url,
    //        data: "mode=reportview",
    //        cache: false,
    //        async: false,
    //        success: function (data) {
    //            window.open(data);
    //        }
    //    });
}

function exportDevExcel(dgid, remoteName, tableName, reportFileName, whereString, options) {
    $.fn.Error.errorCode = 1080;
    var RemoteName = "";
    var TableName = "";
    var ReportFileName = "";
    var WhereString = "";
    var WhereTextString = "";
    var DataSetName = "";
    if ($(dgid) != undefined && $(dgid).length != 0) {
        RemoteName = getInfolightOption($(dgid)).remoteName;
        TableName = getInfolightOption($(dgid)).tableName;
        DataSetName = TableName;
        ReportFileName = getInfolightOption($(dgid)).reportFileName;
        var queryParams = $(dgid).datagrid('options').queryParams;
        if (queryParams.queryWord != "") {
            var queryWord = eval('(' + queryParams.queryWord + ')');
            if (queryWord != undefined && queryWord != null)
                WhereString = queryWord.whereString;
        }
        //WhereString = $(dgid).datagrid('getWhere');
        WhereTextString = $(dgid).datagrid('getWhereText');
    }
    if (remoteName != undefined && remoteName != "undefined" && remoteName != "")
        RemoteName = remoteName;
    if (tableName != undefined && tableName != "undefined" && tableName != "")
        TableName = tableName;
    if (reportFileName != undefined && reportFileName != "undefined" && reportFileName != "")
        ReportFileName = reportFileName;
    if (whereString != undefined && whereString != "undefined" && whereString != "")
        WhereString = whereString;
    if (options && options.DataSetName) {
        DataSetName = options.DataSetName;
    }
    if (RemoteName != "" && TableName != "" && ReportFileName != "") {
        if (ReportFileName.toLowerCase().indexOf('.aspx') > 0) { //jquery 或者 sd 使用reportName
            var developer = $('#_DEVELOPERID').val();
            if (developer) {
                ReportFileName = 'preview' + developer + '/' + ReportFileName;
            }
            //var url = "../DevReportViewerTemplate.aspx?RemoteName=" + RemoteName + "&TableName=" + TableName + "&ReportPath=" + ReportFileName + "&WhereString=" + encodeURIComponent(WhereString) + "&WhereTextString=" + WhereTextString;
            ///因為是匯出EXCEL, 所以在ReportName後面加上一個E
            //ReportFileName = ReportFileName.replace(".aspx", "E.aspx");
            var url = ReportFileName.replace("~", "..") + "?RemoteName=" + RemoteName + "&TableName=" + TableName + "&ReportPath=none&WhereString=" + encodeURIComponent(WhereString) + "&WhereTextString=" + WhereTextString;
            
            url += "&DataSetName=" + DataSetName;
            if (options) {
                if (options.Parameters) {
                    for (var p in options.Parameters) {
                        url += "&RP" + p + "=" + options.Parameters[p];
                    }
                }
                if (options.SP) {
                    url += "&SP=" + options.SP;
                }
                if (options.SPParam) {
                    url += "&SPParam=" + options.SPParam;
                }
                if (options.AssemblyName) {
                    url += "&AssemblyName=" + options.AssemblyName;
                }
                if (options.MethodName) {
                    url += "&MethodName=" + options.MethodName;
                }
                if (options.pdf) {
                    url += "&pdf=true";
                }
                if (options.word) {
                    url += "&word=true";
                }
                if (options.excel) {
                    url += "&excel=true";
                }
                if (options.reportname) {
                    url += "&reportname=" + options.reportname;
                }
            }
            var height = $(window).height() - 20;
            var width = $(window).width() - 20;
            if ($.browser.msie) {
                //alert('msie');
                //alert(url);
                window.open(url, '_blank', 'scrollbars=yes, resizable=yes, location=no, width=' + width + ', height=' + height);
            }
            else {
                //alert('else');
                //alert(url);
                //window.open(url, '_blank', 'scrollbars=yes, resizable=yes, location=no, width=' + width + ', height=' + height);

                var dialog = $('<div/>')
                    .dialog({
                        draggable: false,
                        modal: true,
                        height: height,
                        width: width,
                        title: "報表預覽"//,
                        //maximizable: true
                    });
                $('<iframe style="border: 0px;" src="' + url + '" width="100%" height="95%"></iframe>').appendTo(dialog.find('.panel-body'));
                dialog.dialog('open');
                dialog.dialog('close');
            }
        }
        else { //sd 不使用reportName
            $.fn.Error.errorCode = 1081;
            var DataSetName = options && options.DataSetName ? options.DataSetName : '';
            var HeaderDataSetName = options && options.HeaderDataSetName ? options.HeaderDataSetName : '';
            var HeaderTableName = options && options.HeaderTableName ? options.HeaderTableName : '';
            $.ajax({
                type: 'post',
                dataType: 'text',
                url: getParentFolder() + '../handler/SystemHandler.ashx?type=Menu',
                data: {
                    mode: 'Run',
                    id: ReportFileName,
                    type: 'report',
                    remoteName: RemoteName,
                    whereString: WhereString,
                    whereTextString: WhereTextString,
                    DataSetName: DataSetName,
                    HeaderDataSetName: HeaderDataSetName,
                    HeaderTableName: HeaderTableName
                },
                async: true,
                success: function (url) {
                    url = '../' + url;
                    var height = $(window).height() - 20;
                    var width = $(window).width() - 20;
                    if ($.browser.msie) {
                        window.open(url, '_blank', 'scrollbars=yes, resizable=yes, location=no, width=' + width + ', height=' + height);
                        //window.showModalDialog(url, '', 'dialogHeight=' + height + 'px;dialogWidth=' + width + 'px;location=no;');
                    }
                    else {
                        var dialog = $('<div/>')
                            .dialog({
                                draggable: false,
                                modal: true,
                                height: height,
                                width: width,
                                title: "Report"//,
                                //maximizable: true
                            });
                        $('<iframe style="border: 0px;" src="' + url + '" width="100%" height="100%"></iframe>').appendTo(dialog.find('.panel-body'));
                        //dialog.dialog('maximize');
                        dialog.dialog('open');
                    }
                },
                complete: function () {
                    $.messager.progress('close');
                }
            });
        }
    }
    //    var url = "../handler/jqDataHandle.ashx?RemoteName=S001.Master&TableName=Master";
    //    $.ajax({
    //        type: "POST",
    //        url: url,
    //        data: "mode=reportview",
    //        cache: false,
    //        async: false,
    //        success: function (data) {
    //            window.open(data);
    //        }
    //    });
}

function exportDevReport(dgid, remoteName, tableName, reportFileName, whereString, options) {
    $.fn.Error.errorCode = 1080;
    var RemoteName = "";
    var TableName = "";
    var ReportFileName = "";
    var WhereString = "";
    var WhereTextString = "";
    var DataSetName = "";
    if ($(dgid) != undefined && $(dgid).length != 0) {
        RemoteName = getInfolightOption($(dgid)).remoteName;
        TableName = getInfolightOption($(dgid)).tableName;
        DataSetName = TableName;
        ReportFileName = getInfolightOption($(dgid)).reportFileName;
        var queryParams = $(dgid).datagrid('options').queryParams;
        if (queryParams.queryWord != "") {
            var queryWord = eval('(' + queryParams.queryWord + ')');
            if (queryWord != undefined && queryWord != null)
                WhereString = queryWord.whereString;
        }
        //WhereString = $(dgid).datagrid('getWhere');
        WhereTextString = $(dgid).datagrid('getWhereText');
    }
    if (remoteName != undefined && remoteName != "undefined" && remoteName != "")
        RemoteName = remoteName;
    if (tableName != undefined && tableName != "undefined" && tableName != "")
        TableName = tableName;
    if (reportFileName != undefined && reportFileName != "undefined" && reportFileName != "")
        ReportFileName = reportFileName;
    if (whereString != undefined && whereString != "undefined" && whereString != "")
        WhereString = whereString;
    if (options && options.DataSetName) {
        DataSetName = options.DataSetName;
    }
    if (RemoteName != "" && TableName != "" && ReportFileName != "") {
        if (ReportFileName.toLowerCase().indexOf('.aspx') > 0) { //jquery 或者 sd 使用reportName
            var developer = $('#_DEVELOPERID').val();
            if (developer) {
                ReportFileName = 'preview' + developer + '/' + ReportFileName;
            }
            //var url = "../DevReportViewerTemplate.aspx?RemoteName=" + RemoteName + "&TableName=" + TableName + "&ReportPath=" + ReportFileName + "&WhereString=" + encodeURIComponent(WhereString) + "&WhereTextString=" + WhereTextString;
            var url = ReportFileName.replace("~", "..") + "?RemoteName=" + RemoteName + "&TableName=" + TableName + "&ReportPath=none&WhereString=" + encodeURIComponent(WhereString) + "&WhereTextString=" + WhereTextString;

            url += "&DataSetName=" + DataSetName;
            if (options) {
                if (options.Parameters) {
                    for (var p in options.Parameters) {
                        url += "&RP" + p + "=" + options.Parameters[p];
                    }
                }
                if (options.SP) {
                    url += "&SP=" + options.SP;
                }
                if (options.SPParam) {
                    url += "&SPParam=" + options.SPParam;
                }
                if (options.AssemblyName) {
                    url += "&AssemblyName=" + options.AssemblyName;
                }
                if (options.MethodName) {
                    url += "&MethodName=" + options.MethodName;
                }
                if (options.pdf) {
                    url += "&pdf=true";
                }
                if (options.word) {
                    url += "&word=true";
                }
                if (options.excel) {
                    url += "&excel=true";
                }
                if (options.reportname) {
                    url += "&reportname=" + options.reportname;
                }
            }
            var height = $(window).height() - 20;
            var width = $(window).width() - 20;
            if ($.browser.msie) {
                //alert('msie');
                //alert(url);
                window.open(url, '_blank', 'scrollbars=yes, resizable=yes, location=no, width=' + width + ', height=' + height);
            }
            else {
                //alert('else');
                //alert(url);
                //window.open(url, '_blank', 'scrollbars=yes, resizable=yes, location=no, width=' + width + ', height=' + height);

                var dialog = $('<div/>')
                    .dialog({
                        draggable: false,
                        modal: true,
                        height: height,
                        width: width,
                        title: "報表預覽"//,
                        //maximizable: true
                    });
                $('<iframe style="border: 0px;" src="' + url + '" width="100%" height="95%"></iframe>').appendTo(dialog.find('.panel-body'));
                dialog.dialog('open');
            }
        }
        else { //sd 不使用reportName
            $.fn.Error.errorCode = 1081;
            var DataSetName = options && options.DataSetName ? options.DataSetName : '';
            var HeaderDataSetName = options && options.HeaderDataSetName ? options.HeaderDataSetName : '';
            var HeaderTableName = options && options.HeaderTableName ? options.HeaderTableName : '';
            $.ajax({
                type: 'post',
                dataType: 'text',
                url: getParentFolder() + '../handler/SystemHandler.ashx?type=Menu',
                data: {
                    mode: 'Run',
                    id: ReportFileName,
                    type: 'report',
                    remoteName: RemoteName,
                    whereString: WhereString,
                    whereTextString: WhereTextString,
                    DataSetName: DataSetName,
                    HeaderDataSetName: HeaderDataSetName,
                    HeaderTableName: HeaderTableName
                },
                async: true,
                success: function (url) {
                    url = '../' + url;
                    var height = $(window).height() - 20;
                    var width = $(window).width() - 20;
                    if ($.browser.msie) {
                        window.open(url, '_blank', 'scrollbars=yes, resizable=yes, location=no, width=' + width + ', height=' + height);
                        //window.showModalDialog(url, '', 'dialogHeight=' + height + 'px;dialogWidth=' + width + 'px;location=no;');
                    }
                    else {
                        var dialog = $('<div/>')
                            .dialog({
                                draggable: false,
                                modal: true,
                                height: height,
                                width: width,
                                title: "Report"//,
                                //maximizable: true
                            });
                        $('<iframe style="border: 0px;" src="' + url + '" width="100%" height="100%"></iframe>').appendTo(dialog.find('.panel-body'));
                        //dialog.dialog('maximize');
                        dialog.dialog('open');
                    }
                },
                complete: function () {
                    $.messager.progress('close');
                }
            });
        }
    }
    //    var url = "../handler/jqDataHandle.ashx?RemoteName=S001.Master&TableName=Master";
    //    $.ajax({
    //        type: "POST",
    //        url: url,
    //        data: "mode=reportview",
    //        cache: false,
    //        async: false,
    //        success: function (data) {
    //            window.open(data);
    //        }
    //    });
}


function apply(dgid) {
    //    var editIndex = getEditIndex($(dgid));
    //    if (editIndex == -1) {
    //        applyUpdates($(dgid)); //autoapply为true时，防止第一次保存失败后不能直接保存
    //    }
    //    else {
    if (endEdit($(dgid))) {
        var autoApply = getInfolightOption($(dgid)).autoApply;
        if (!autoApply) {
            $.fn.Error.errorCode = 1004;
            applyUpdates($(dgid));
        }
    }
    //    }
}
function cancel(dgid) {
    var autoApply = getInfolightOption($(dgid)).autoApply;
    if (!autoApply) {
        var editIndex = getEditIndex($(dgid));
        if (editIndex != -1) {
            $(dgid).datagrid('cancelEdit', editIndex);
            setEditIndex($(dgid), -1);
        }
    }
    else {
        $(dgid).datagrid('rejectChanges');
        $(dgid).datagrid('removeLock'); //record lock
        $(dgid).datagrid('changeState', 'normal');
        if ($(dgid).data('imported')) {
            var remoteName = getInfolightOption($(dgid)).remoteName;
            var tableName = getInfolightOption($(dgid)).tableName;
            $(dgid).datagrid({
                url: getDataUrl(),
                queryParams: getRemoteParam({}, remoteName, tableName, true),
                pagination: $(dgid).data('pagination')
            });
            $(dgid).removeData('imported');
        }
        setEditIndex($(dgid), -1);
    }
}
function applyUpdates(dg) {
    var parent = getInfolightOption(dg).parent;
    if (parent != undefined && parent != '') {
        var parentGrid = $('#' + parent + dataGrid_class);
        if (parentGrid.length) {
            applyUpdates(parentGrid);
            return;
        }
    }
    var changedDatas = dg.datagrid('getChangedDatas', true);
    if (changedDatas.length > 0) {
        //    var changedDatas = [];
        //    var changes = dg.datagrid("getChanges");
        //    if (changes.length != 0) {
        //changedDatas.push(getChangedDatas(dg));

        //修改当前master为modify状态
        var remoteName = getInfolightOption(dg).remoteName;
        var tableName = getInfolightOption(dg).tableName;
        if (tableName != changedDatas[0].tableName) {
            var changedRows = {tableName: tableName, inserted: [], updated: [], deleted: []};
            var editIndex = getEditIndex(dg);
            var masterRow = editIndex != -1 ? dg.datagrid('getRows')[editIndex] : dg.datagrid('getSelected');
            var row = {};
            for (var prop in masterRow) {
                var value = masterRow[prop];
                //if (value != undefined && typeof value == 'string') {
                //    row[prop] = encodeURIComponent(value);
                //    row[prop] = row[prop].toString().replace(/(%22)/g, "\"").replace(/(%5C)/g, "\\");
                //}
                //else {
                    row[prop] = value;
                //}
            }
            changedRows.updated.push(row);
            changedDatas.splice(0, 0, changedRows);
        }

        //var url = dg.datagrid('options').url;


        //        if (changedDatas[0].inserted != undefined) {
        //            for (var i = 0; i < changedDatas[0].inserted.length; i++) {
        //                var row = changedDatas[0].inserted[i];
        //                for (var prop in row) {
        //                    var value = row[prop];
        //                    if (value != undefined && typeof value == 'string') {
        //                        row[prop] = value.replace(/&/g, "%26");
        //                    }
        //                }
        //            }
        //        }
        //        if (changedDatas[0].updated != undefined) {
        //            for (var i = 0; i < changedDatas[0].updated.length; i++) {
        //                var row = changedDatas[0].updated[i];
        //                for (var prop in row) {
        //                    var value = row[prop];
        //                    if (value != undefined && typeof value == 'string') {
        //                        row[prop] = value.replace(/&/g, "%26");
        //                    }
        //                }
        //            }
        //        }
        //        if (changedDatas[0].deleted != undefined) {
        //            for (var i = 0; i < changedDatas[0].deleted.length; i++) {
        //                var row = changedDatas[0].deleted[i];
        //                for (var prop in row) {
        //                    var value = row[prop];
        //                    if (value != undefined && typeof value == 'string') {
        //                        row[prop] = value.replace(/&/g, "%26");
        //                    }
        //                }
        //            }
        //        }

        $.ajax({
            type: "POST",
            url: getDataUrl(),
            data: getRemoteParam({data: $.toJSONString(changedDatas), mode: 'update'}, remoteName, tableName, true),
            cache: false,
            async: true,
            beforeSend: function () {
                $.messager.progress({
                    title: 'Please waiting',
                    msg: 'Saving data...'
                });
            },
            success: function (data) {
                var insertdRows = dg.datagrid("getChanges", "inserted");
                var deletedRows = dg.datagrid("getChanges", "deleted");
                var updatedRows = dg.datagrid("getChanges", "updated");
                dg.datagrid('acceptChangesAll');
                dg.datagrid('changeState', 'normal');
                dg.datagrid('removeLock'); //record lock

                if (dg.data('imported')) {
                    var remoteName = getInfolightOption(dg).remoteName;
                    var tableName = getInfolightOption(dg).tableName;
                    dg.datagrid({
                        url: getDataUrl(),
                        queryParams: getRemoteParam({}, remoteName, tableName, true),
                        pagination: dg.data('pagination')
                    });
                    dg.removeData('imported');
                }
                if (insertdRows != null && insertdRows.length > 0) {
                    var onInserted = getInfolightOption(dg).onInserted;
                    if (onInserted) {
                        if (data) {
                            insertdRows = $.parseJSON(data);
                        }
                        onInserted.call(dg, insertdRows[0]);
                    }
                }
                if (deletedRows != null && deletedRows.length > 0) {
                    var onDeleted = getInfolightOption(dg).onDeleted;
                    if (onDeleted) {
                        onDeleted.call(dg, deletedRows[0]);
                    }
                }
                if (updatedRows != null && updatedRows.length > 0) {
                    var onUpdated = getInfolightOption(dg).onUpdated;
                    if (onUpdated) {
                        onUpdated.call(dg, updatedRows[0]);
                    }
                }
            },
            complete: function () {
                $(document).ready(function () {
                    $.messager.progress('close');
                });
            }
        });
    }
    else {
        dg.datagrid('removeLock'); //record lock
        dg.datagrid('changeState', 'normal');
    }
}

function getChangedDatas(dg) {
    var changedRows = {tableName: getInfolightOption(dg).tableName};
    var changes = dg.datagrid("getChanges");
    if (changes.length != 0) {

        var insertdRows = dg.datagrid("getChanges", "inserted");
        if (insertdRows != null) {
            changedRows.inserted = insertdRows;
        }
        var deletedRows = dg.datagrid("getChanges", "deleted");
        if (deletedRows != null) {
            changedRows.deleted = deletedRows;
        }
        var updatedRows = dg.datagrid("getChanges", "updated");
        if (updatedRows != null) {
            changedRows.updated = updatedRows;
        }
    }
    return changedRows;
}
//----------------------------------------------------------------------------------------------------------------

$.extend($.fn.form.methods, {
    setValidateStyle: function (jq, mode) {
        $('input,select,textarea', jq[0]).each(function () {
            var required = getInfolightOption($(this)).required;
            var validType = getInfolightOption($(this)).validType;
            if (required || validType) {
                var captionTd = $(this).closest('td').prev('td');
                if (mode == 'viewed') {
                    captionTd.removeClass('validate-label');
                }
                else {
                    captionTd.addClass('validate-label');
                }
            }
        });
    },
    validateForm: function (jq) {
        if (jq.length > 0) {
            var onBeforeValidate = getInfolightOption($(jq[0])).onBeforeValidate;
            if (onBeforeValidate) {
                var result = onBeforeValidate.call(jq[0]);
                if (!result) {
                    return true;
                }
            }
            var validateStyle = getInfolightOption($(jq[0])).validateStyle;
            if (validateStyle == 'hint') {
                var validate = $(jq[0]).form('validate');
                if (validate) {
                    var chainDataForm = getInfolightOption($(jq[0])).chainDataForm;
                    if (chainDataForm) {
                        return $(chainDataForm).form('validateForm');
                    }
                    else {
                        return true;
                    }
                }
                else {
                    return false;
                }
                //return $(jq[0]).form('validate');
            }
            else if (validateStyle == 'dialog') {
                var messages = [];
                $('input,select,textarea', jq[0]).each(function () {
                    var field = getInfolightOption($(this)).field;
                    var formid = getInfolightOption($(this)).form;
                    if (formid != undefined && field != undefined && $(jq[0]).attr('id') == formid) {
                        var value = "";
                        var controlClass = $(this).attr('class');
                        if (controlClass != undefined) {
                            if (controlClass.indexOf('easyui-datebox') == 0) {
                                value = $(this).datebox('getBindingValue');
                            }
                            else if (controlClass.indexOf('easyui-combobox') == 0) {
                                value = $(this).combobox('getValue');
                            }
                            else if (controlClass.indexOf('easyui-datetimebox') == 0) {
                                value = $(this).datetimebox('getBindingValue');
                            }
                            else if (controlClass.indexOf('easyui-combogrid') == 0) {
                                value = $(this).combogrid('getValue');
                            }
                            else if (controlClass.indexOf('info-combobox') == 0) {
                                value = $(this).combobox('getValue');
                            }
                            else if (controlClass.indexOf('info-combogrid') == 0) {
                                value = $(this).combogrid('getValue');
                            }
                            else if (controlClass.indexOf('info-refval') == 0) {
                                value = $(this).refval('getValue');
                            }
                            else if (controlClass.indexOf('info-options') == 0) {
                                value = $(this).options('getValue');
                            }
                            else if (controlClass.indexOf('info-autocomplete') == 0) {
                                value = $(this).combobox('getValue');
                            }
                            else if (controlClass.indexOf('info-yearmonth') == 0) {
                                value = $(this).yearmonth('getValue');
                            }
                            else {
                                value = $(this).val();
                            }
                        }
                        else {
                            if ($(this).attr('type') == "checkbox") {
                                value = $(this).checkbox('getValue');
                            }
                            else {
                                value = $(this).val();
                            }
                        }
                        var captionValidate = $(this).closest('td').prev('td').html();
                        var required = getInfolightOption($(this)).required;
                        if (value.length == 0) {
                            if (required) {
                                messages.push(captionValidate + ':' + $.fn.validatebox.defaults.missingMessage);
                            }
                        }
                        else {
                            var validType = getInfolightOption($(this)).validType;
                            if (validType) {
                                var v = validType.split(/[\[\]]/g);
                                var rule = v[0];
                                var params = [];
                                if (v.length > 1) {
                                    params = eval('[' + v[1] + ']');
                                }
                                if ($.fn.validatebox.defaults.rules[rule]) {
                                    if (!$.fn.validatebox.defaults.rules[rule].validator.call(this, value, params)) {
                                        var validateMessage = $.fn.validatebox.defaults.rules[rule].message;
                                        for (var i = 0; i < params.length; i++) {
                                            validateMessage = validateMessage.replace('{' + i + '}', params[i].toString());
                                        }
                                        messages.push(captionValidate + ':' + validateMessage);
                                    }
                                }
                            }
                        }
                    }
                });
                if (messages.length > 0) {
                    alert(messages.join('\n'));
                    return false;
                }
                else {
                    var chainDataForm = getInfolightOption($(jq[0])).chainDataForm;
                    if (chainDataForm) {
                        return $(chainDataForm).form('validateForm');
                    }
                    else {
                        return true;
                    }
                }
            }
            else if (validateStyle == 'both') {
                var validate = $(jq[0]).form('validate');
                if (validate) {
                    var chainDataForm = getInfolightOption($(jq[0])).chainDataForm;
                    if (chainDataForm) {
                        return $(chainDataForm).form('validateForm');
                    }
                    else {
                        var messages = [];
                        $('input,select,textarea', jq[0]).each(function () {
                            var field = getInfolightOption($(this)).field;
                            var formid = getInfolightOption($(this)).form;
                            if (formid != undefined && field != undefined && $(jq[0]).attr('id') == formid) {
                                var value = "";
                                var controlClass = $(this).attr('class');
                                if (controlClass != undefined) {
                                    if (controlClass.indexOf('easyui-datebox') == 0) {
                                        value = $(this).datebox('getBindingValue');
                                    }
                                    else if (controlClass.indexOf('easyui-combobox') == 0) {
                                        value = $(this).combobox('getValue');
                                    }
                                    else if (controlClass.indexOf('easyui-datetimebox') == 0) {
                                        value = $(this).datetimebox('getBindingValue');
                                    }
                                    else if (controlClass.indexOf('easyui-combogrid') == 0) {
                                        value = $(this).combogrid('getValue');
                                    }
                                    else if (controlClass.indexOf('info-combobox') == 0) {
                                        value = $(this).combobox('getValue');
                                    }
                                    else if (controlClass.indexOf('info-combogrid') == 0) {
                                        value = $(this).combogrid('getValue');
                                    }
                                    else if (controlClass.indexOf('info-refval') == 0) {
                                        value = $(this).refval('getValue');
                                    }
                                    else if (controlClass.indexOf('info-options') == 0) {
                                        value = $(this).options('getValue');
                                    }
                                    else if (controlClass.indexOf('info-autocomplete') == 0) {
                                        value = $(this).combobox('getValue');
                                    }
                                    else {
                                        value = $(this).val();
                                    }
                                }
                                else {
                                    if ($(this).attr('type') == "checkbox") {
                                        value = $(this).checkbox('getValue');
                                    }
                                    else {
                                        value = $(this).val();
                                    }
                                }
                                var captionValidate = $(this).closest('td').prev('td').html();
                                var required = getInfolightOption($(this)).required;
                                if (value.length == 0) {
                                    if (required) {
                                        messages.push(captionValidate + ':' + $.fn.validatebox.defaults.missingMessage);
                                    }
                                }
                                else {
                                    var validType = getInfolightOption($(this)).validType;
                                    if (validType) {
                                        var v = validType.split(/[\[\]]/g);
                                        var rule = v[0];
                                        var params = [];
                                        if (v.length > 1) {
                                            params = eval('[' + v[1] + ']');
                                        }
                                        if ($.fn.validatebox.defaults.rules[rule]) {
                                            if (!$.fn.validatebox.defaults.rules[rule].validator.call(this, value, params)) {
                                                var validateMessage = $.fn.validatebox.defaults.rules[rule].message;
                                                for (var i = 0; i < params.length; i++) {
                                                    validateMessage = validateMessage.replace('{' + i + '}', params[i].toString());
                                                }
                                                messages.push(captionValidate + ':' + validateMessage);
                                            }
                                        }
                                    }
                                }
                            }
                        });
                        if (messages.length > 0) {
                            alert(messages.join('\n'));
                            return false;
                        }
                    }
                }
                else {
                    return false;
                }

            }
            return true;
        }
    },
    updateRow: function (jq, rowData) {
        if (jq.length > 0) {
            //combogrid特别的设置
            $(".info-combogrid", jq[0]).each(function () {
                var combogrid = $(this);
                var field = getInfolightOption(combogrid).field;
                if (field != undefined) {
                    if (rowData[field] == undefined || rowData[field] == '') {
                    }
                    else {
                        var cgremoteName = getInfolightOption(combogrid).remoteName;
                        var cgtableName = getInfolightOption(combogrid).tableName;
                        var cgvalueField = getInfolightOption(combogrid).valueField;
                        var cgqueryWord = {whereString: cgvalueField + "='" + rowData[field] + "'"};
                        $.fn.Error.errorCode = 1900;
                        $.ajax({
                            type: "POST",
                            url: getDataUrl(),
                            data: getRemoteParam({queryWord: $.toJSONString(cgqueryWord)}, cgremoteName, cgtableName, false),
                            cache: false,
                            async: false,
                            success: function (a) {
                                //alert(a);
                                if (a != null) {
                                    //alert(a);
                                    var rows = $.parseJSON(a);
                                    if (rows.length > 0) {
                                        var oldrows = combogrid.combogrid('grid').datagrid('getRows');
                                        var isexist = false;
                                        for (var i = 0; i < oldrows.length; i++) {
                                            if (oldrows[i][cgvalueField] == rows[0][cgvalueField]) {
                                                isexist = true;
                                                break;
                                            }
                                        }
                                        if (!isexist)
                                            combogrid.combogrid('grid').datagrid('appendRow', rows[0]);
                                    }
                                }
                            },
                            error: function () {
                                alert('error');
                            }
                        });
                        combogrid.combogrid('setValue', rowData[field]);
                    }
                }
            });
            var formData = {};
            $('input,select,textarea', jq[0]).each(function () {
                var field = getInfolightOption($(this)).field;
                var format = getInfolightOption($(this)).format;
                if (format != undefined && rowData[field] != undefined) {
                    formData[field] = getFormatValue(rowData[field], format);
                }
                else if (rowData[field] != undefined) {
                    formData[field] = rowData[field];
                }
            });
            $(jq[0]).form('load', formData);
            $(":checkbox", jq[0]).each(function () {
                var checkbox = $(this);
                var field = getInfolightOption(checkbox).field;
                if (field != undefined && rowData[field] != undefined) {
                    checkbox.checkbox('setValue', rowData[field]);
                }
            });
            $(".info-refval", jq[0]).each(function () {
                var refval = $(this);
                var field = getInfolightOption(refval).field;
                if (field != undefined && rowData[field] != undefined) {
                    refval.refval('setValue', rowData[field]);
                }
            });
        }
    }
});


//form method
var editMode_attr = "editMode";

function getEditMode(f) {
    return f.attr(editMode_attr);
}
function setEditMode(f, mode) {
    f.attr(editMode_attr, mode);
}
function doFlowEdit(formId, mode, dialogId) {
    var form = $("#" + formId);
    if (getEditMode(form) == "inserted") {
        return;
    }
    if (dialogId) {
        $('#' + dialogId).find('#DialogSubmit').show();
    }
    else {
        form.parent().next('div').children('#DialogSubmit').show();
    }
    //$("#DialogSubmit").show();
    setEditMode(form, "updated");
    var target = form.attr('dialogGrid');
    if (target == undefined) target = form.attr('switchGrid');
    if (target == undefined) target = form.attr('continueGrid');
    var editMode = getInfolightOption($(target)).editMode;
    changeFormState(form.parent(), mode, $(target).attr('keyColumns'), editMode);
    if (dialogId) {
        $('#' + dialogId).find(dataGrid_class).each(function () {
            var datagrid = $(this);
            setReadOnly(datagrid, false);
            datagrid.attr("disabled", false);
            datagrid.datagrid('showToolItems');
            var toolbar = datagrid.datagrid('options').toolbar;
            if ($(toolbar) != undefined) {
                $(toolbar).show();
            }
        });
    }
    else {
        form.closest('div.panel').find(dataGrid_class).each(function () {
            var datagrid = $(this);
            setReadOnly(datagrid, false);
            datagrid.attr("disabled", false);
            datagrid.datagrid('showToolItems');
            var toolbar = datagrid.datagrid('options').toolbar;
            if ($(toolbar) != undefined) {
                $(toolbar).show();
            }
        });
    }
}

function changeFormState(fid, mode, keys, editMode) {
    if (mode == 'viewed') {

        $('#FlowEdit').linkbutton('enable');
        if (editMode != undefined && editMode.toLowerCase() == 'continue') {
            $('.infosysbutton-c', fid).each(function () {
                $(this).hide();
            });
        }

        $('input,select,textarea', fid).each(function () {
            if (!(this.disabled == 'disabled' || this.disabled == true)) {
                $(this).attr('isalsoreadonly', false);
            }
            //textarea ie8,9
            if (this.nodeName.toLowerCase() == "textarea" && $.browser.msie && ($.browser.version == "8.0" || $.browser.version == "9.0")) {
                this.readOnly = true;
                $(this).css('color', 'Silver');
            }
            else this.disabled = 'disabled';
            if ($(this).hasClass('easyui-validatebox')) {
                $(this).validatebox('validate');
            }

            var controlClass = $(this).attr('class');
            if (controlClass != undefined) {


                if (controlClass.indexOf('info-refval') == 0) {
                    //                    $.data(this, "inforefval").refval.find("input.refval-text").attr('disabled', true);
                    //                    $.data(this, "inforefval").refval.find("span.icon-view").attr('disabled', true);
                    $(this).refval('disable');
                }
                else if (controlClass.indexOf('info-combobox') == 0) {
                    $(this).combobox('disable');
                    //$(this).combobox({ disabled: true });
                }
                else if (controlClass.indexOf('info-combogrid') == 0) {
                    $(this).combogrid('disable');
                    //$(this).combogrid({ disabled: true });
                }
                else if (controlClass.indexOf('easyui-datebox') == 0) {
                    $(this).datebox('disable');
                    //$(this).datebox({ disabled: true });
                }
                else if (controlClass.indexOf('easyui-datetimebox') == 0) {
                    $(this).datetimebox('disable');
                }
                else if (controlClass.indexOf('easyui-numberbox') == 0) {
                    $(this).numberbox('disable');
                    //$(this).datebox({ disabled: true });
                }
                else if (controlClass.indexOf('easyui-timespinner') == 0) {
                    $(this).timespinner('disable');
                }
                else if (controlClass.indexOf('info-options') == 0) {
                    $(this).options('disable');
                    //$(this).datebox({ disabled: false });
                }
                else if (controlClass.indexOf('info-autocomplete') == 0) {
                    $(this).combobox('disable');
                }
                else if (controlClass.indexOf('info-yearmonth') == 0) {
                    $(this).yearmonth('disable');
                    //$(this).datebox({ disabled: false });
                }
            }

        });
        $('a', fid).each(function () {
            var controlClass = $(this).attr('class');
            if (controlClass != undefined) {
                if (controlClass.indexOf('info-fileUpload-button') == 0) {
                    if (!(this.disabled == 'disabled' || this.disabled == true)) {
                        $(this).attr('isalsoreadonly', false);
                    }
                    $(this).linkbutton({disabled: true});
                }
            }
        });
        $(dataGrid_class, fid).each(function () {
            var datagrid = $(this);
            datagrid.attr("disabled", "disabled");
            setReadOnly(datagrid, true);
            datagrid.datagrid('hideToolItems');
            var toolbar = datagrid.datagrid('options').toolbar;
            if ($(toolbar) != undefined) {
                $(toolbar).hide();
            }
        });
        $('.infosysbutton-s', fid).each(function () {
            $(this).hide();
            $(this).attr('isalsoreadonly', false);
            if (this.id.indexOf('Flow') == 0) {
                $(this).show();
            }
        });
    }
    else {
        $('#FlowEdit').linkbutton('disable');
        if (editMode != undefined && editMode.toLowerCase() == 'continue') {
            $('.infosysbutton-c', fid).each(function () {
                $(this).show();
            });
        }
        $('input,select,textarea', fid).each(function () {
            if ($(this).attr('disabled') == 'disabled' && $(this).attr('isalsoreadonly') == 'false') {
                this.disabled = false;
                var controlClass = $(this).attr('class');
                if (controlClass != undefined) {
                    if (controlClass.indexOf('info-refval') == 0) {
                        //                        $.data(this, "inforefval").refval.find("input.refval-text").attr('disabled', false);
                        //                        $.data(this, "inforefval").refval.find("span.icon-view").attr('disabled', false);
                        $(this).refval('enable');
                    }
                    else if (controlClass.indexOf('info-combobox') == 0) {
                        $(this).combobox('enable');
                        //$(this).combobox({ disabled: false });
                    }
                    else if (controlClass.indexOf('info-combogrid') == 0) {
                        $(this).combogrid('enable');
                        //$(this).combogrid({ disabled: false });
                    }
                    else if (controlClass.indexOf('easyui-datebox') == 0) {
                        $(this).datebox('enable');
                        //$(this).datebox({ disabled: false });
                    }
                    else if (controlClass.indexOf('easyui-datetimebox') == 0) {
                        $(this).datetimebox('enable');
                    }
                    else if (controlClass.indexOf('easyui-numberbox') == 0) {
                        $(this).numberbox('enable');
                        //$(this).datebox({ disabled: false });
                    }
                    else if (controlClass.indexOf('easyui-timespinner') == 0) {
                        $(this).timespinner('enable');
                    }
                    else if (controlClass.indexOf('info-options') == 0) {
                        $(this).options('enable');
                        //$(this).datebox({ disabled: false });
                    }
                    else if (controlClass.indexOf('info-autocomplete') == 0) {
                        $(this).combobox('enable');
                    }
                    else if (controlClass.indexOf('info-yearmonth') == 0) {
                        $(this).yearmonth('enable');
                        //$(this).datebox({ disabled: false });
                    }

                }
            }
            //textarea ie8,9
            if (this.nodeName && this.nodeName.toLowerCase() == "textarea" && $.browser.msie && ($.browser.version == "8.0" || $.browser.version == "9.0") && $(this).attr('isalsoreadonly') == 'false') {
                this.readOnly = false;
                $(this).css('color', 'Black');
            }

            //disable key field
            $('a', fid).each(function () {
                var controlClass = $(this).attr('class');
                if (controlClass != undefined) {
                    if (controlClass.indexOf('info-fileUpload-button') == 0) {
                        if ($(this).attr('isalsoreadonly') == 'false') {
                            $(this).linkbutton({disabled: false});
                        }
                    }
                }
            });
            if (mode == "updated" && keys != undefined) {
                var field = getInfolightOption($(this)).field;
                if (field != undefined) {
                    var keyColumns = keys.split(',');
                    for (var i = 0; i < keyColumns.length; i++) {
                        if (field == keyColumns[i]) {
                            if (!(this.disabled == 'disabled' || this.disabled == true)) {
                                $(this).attr('isalsoreadonly', false);
                            }
                            this.disabled = 'disabled';
                            var controlClass = $(this).attr('class');
                            if ($(this).hasClass('easyui-validatebox')) {
                                $(this).validatebox('validate');
                            }

                            if (controlClass != undefined) {
                                if (controlClass.indexOf('info-refval') == 0) {
                                    //                                    $.data(this, "inforefval").refval.find("span.icon-view").attr('disabled', true);
                                    //                                    $.data(this, "inforefval").refval.find("input.refval-text").attr('disabled', true);
                                    $(this).refval('disable');
                                }
                                else if (controlClass.indexOf('info-combobox') == 0) {
                                    $(this).combobox('disable');
                                    //$(this).combobox({ disabled: true });
                                }
                                else if (controlClass.indexOf('info-combogrid') == 0) {
                                    $(this).combogrid('disable');
                                    //$(this).combogrid({ disabled: true });
                                }
                                else if (controlClass.indexOf('easyui-datebox') == 0) {
                                    $(this).datebox('disable');
                                    //$(this).datebox({ disabled: true });
                                }
                                else if (controlClass.indexOf('easyui-datetimebox') == 0) {
                                    $(this).datetimebox('disable');
                                }
                                else if (controlClass.indexOf('easyui-numberbox') == 0) {
                                    $(this).numberbox('disable');
                                    //$(this).datebox({ disabled: true });
                                }
                                else if (controlClass.indexOf('easyui-timespinner') == 0) {
                                    $(this).timespinner('disable');
                                }
                                else if (controlClass.indexOf('info-options') == 0) {
                                    $(this).options('disable');
                                }
                                else if (controlClass.indexOf('info-autocomplete') == 0) {
                                    $(this).combobox('disable');
                                }
                            }
                        }
                    }
                }
            }
        });

        $(dataGrid_class, fid).each(function () {
            var datagrid = $(this);
            setReadOnly(datagrid, false);
            datagrid.attr("disabled", false);
            datagrid.datagrid('showToolItems');
            var toolbar = datagrid.datagrid('options').toolbar;
            if ($(toolbar) != undefined) {
                $(toolbar).show();
            }

        });
        $('.infosysbutton-s', fid).each(function () {
            $(this).show();
            $(this).attr('isalsoreadonly', true);
        });
    }
    if (getInfolightOption($(fid)).containForm) {
        changeFormState($(getInfolightOption($(fid)).containForm), mode, keys, editMode);
    }
}
function loadFormRow(form, rowData, mode) {
    setEditMode(form, mode);
    form.form('setValidateStyle', mode);
    //changeFormState(fid, mode, keys);

    form.form('clear');
    if (mode == "inserted") {
        rowData = getDefaultValues(form, rowData);
    }

    if (rowData) {
        //combogrid特别的设置
        $(".info-combogrid", form).each(function () {
            var combogrid = $(this);
            var field = getInfolightOption(combogrid).field;
            if (field != undefined) {
                if (rowData[field] == undefined || rowData[field] == '') {
                }
                else {
                    var cgremoteName = getInfolightOption(combogrid).remoteName;
                    var cgtableName = getInfolightOption(combogrid).tableName;
                    var cgvalueField = getInfolightOption(combogrid).valueField;
                    var cgqueryWord = {whereString: cgvalueField + "='" + rowData[field] + "'"};
                    $.fn.Error.errorCode = 1901;
                    $.ajax({
                        type: "POST",
                        url: getDataUrl(),
                        data: getRemoteParam({queryWord: $.toJSONString(cgqueryWord)}, cgremoteName, cgtableName, false),
                        cache: false,
                        async: false,
                        success: function (a) {
                            //alert(a);
                            if (a != null) {
                                //alert(a);
                                var rows = $.parseJSON(a);
                                if (rows.length > 0) {
                                    var oldrows = combogrid.combogrid('grid').datagrid('getRows');
                                    var isexist = false;
                                    for (var i = 0; i < oldrows.length; i++) {
                                        if (oldrows[i][cgvalueField] == rows[0][cgvalueField]) {
                                            isexist = true;
                                            break;
                                        }
                                    }
                                    if (!isexist)
                                        combogrid.combogrid('grid').datagrid('appendRow', rows[0]);
                                }
                            }
                        },
                        error: function () {
                            alert('error');
                        }
                    });
                    combogrid.combogrid('setValue', rowData[field]);
                }
            }
        });

        //设置format
        var formData = {};
        $('input,select,textarea', form).each(function () {
            var field = getInfolightOption($(this)).field;
            var format = getInfolightOption($(this)).format;
            if (format != undefined) {
                if (rowData[field] == undefined) {
                    if (format.toLowerCase().indexOf('image') == 0) {
                        $('input:image', this.parentElement).each(function () {
                            $(this).remove();
                        });
                        $(this).hide();
                    }
                    else if (format.toLowerCase().indexOf('download') == 0) {
                        $('a', this.parentElement).each(function () {
                            $(this).remove();
                        });
                        $(this).hide();
                    }
                    else {
                        if (format.indexOf('YY') == 0) {
                            if ($(this).hasClass('easyui-datebox')) {
                                $(this).datebox('rocYear', true);
                            }
                        }

                        formData[field] = getFormatValue(rowData[field], format);
                    }
                }
                else if (format.toLowerCase().indexOf('image') == 0) {
                    //var path = location.pathname;
                    var height = getInfolightOption($(this)).height;
                    var folder = getInfolightOption($(this)).folder;
                    if (height == undefined) {
                        height = 20;
                    }
                    if (folder == undefined) {
                        folder = "";
                    }
                    if (folder.toLowerCase().indexOf("\\") == 0 || folder.toLowerCase().indexOf("/") == 0) {
                        folder = folder.substring(1);
                    }
                    else if (folder.substring(folder.length - 1).toLowerCase() == "\\" || folder.substring(folder.length - 1).toLowerCase() == "/") {
                        folder = folder.substring(0, folder.length - 1);
                    }
                    var value = rowData[field];
                    if (value.toLowerCase().indexOf("\\") == 0 || value.toLowerCase().indexOf("/") == 0)
                        value = value.substring(1);

                    var developer = $('#_DEVELOPERID').val();

                    folder = "../" + (developer ? ('preview' + developer + '/') : '') + folder + "/";
                    var vpath = folder + value;
                    $('input:image', this.parentElement).each(function () {
                        $(this).remove();
                    });
                    var aval = $('<input type="image" onclick="return false;"  />').insertAfter(this);
                    $(aval).css('height', height);
                    $(aval).attr('src', vpath);
                    $(this).hide();
                    $(this).val(rowData[field]);
                    //var onmouseover = 'infogridimageformatterset(this,"' + vpath + '");';
                    //var onmouseout = 'setTimeout(function(){if($(".info-imagecontainer:first").attr("isShow") != "false") {$(".info-imagecontainer:first").fadeOut(1200);}},500);';

                    //$(aval).unbind().bind('mouseover', function () {
                    //    infogridimageformatterset(this, vpath);
                    //});
                    //$(aval).bind('mouseout', function () {
                    //    setTimeout(function () { if ($(".info-imagecontainer:first").attr("isShow") != "false") { $(".info-imagecontainer:first").fadeOut(1200); } }, 500);
                    //});
                }
                else if (format.toLowerCase().indexOf('download') == 0) {
                    var value = rowData[field];
                    var folder = getInfolightOption($(this)).folder;
                    if (folder == undefined) {
                        folder = "";
                    }
                    if (folder.toLowerCase().indexOf("\\") == 0 || folder.toLowerCase().indexOf("/") == 0) {
                        folder = folder.substring(1);
                    }
                    else if (folder.substring(folder.length - 1).toLowerCase() == "\\" || folder.substring(folder.length - 1).toLowerCase() == "/") {
                        folder = folder.substring(0, folder.length - 1);
                    }
                    var developer = $('#_DEVELOPERID').val();

                    folder = "../" + (developer ? ('preview' + developer + '/') : '') + folder + "/";
                    var vpath = folder + value;
                    var onclick = getInfolightOption($(this)).onclick;
                    var onclickstring = "";
                    if (onclick != undefined) {
                        onclickstring += onclick + "('" + vpath + "');";
                    }
                    onclickstring += "window.open('" + getParentFolder() + "../handler/JqFileHandler.ashx?File=" + vpath + "', 'download');";
                    var aval = '<a href="#" onclick="' + onclickstring + '">' + value + '</a>';
                    $('a', this.parentElement).each(function () {
                        $(this).remove();
                    });
                    $(aval).insertAfter(this);
                    $(this).hide();
                    $(this).val(rowData[field]);
                }
                else {
                    if (format.indexOf('YY') == 0) {
                        if ($(this).hasClass('easyui-datebox')) {
                            $(this).datebox('rocYear', true);
                        }
                    }

                    formData[field] = getFormatValue(rowData[field], format);
                }
            }
            else {
                if (format && format.indexOf('YY') == 0) {
                    if ($(this).hasClass('easyui-datebox')) {
                        $(this).datebox('rocYear', true);
                    }
                }
                formData[field] = rowData[field];
            }
        });
        form.form('load', formData);
        $(".info-combobox", form).each(function () {
            var combobox = $(this);
            var field = getInfolightOption(combobox).field;

            if (combobox.data('comboSearchValue')) {
                var where = combobox.combobox('getWhere');
                if (!where) {
                    where = '';
                }
                combobox.combobox('setWhere', where);
                combobox.removeData('comboSearchValue');
            }
            if (field != undefined && (!rowData[field] && rowData[field] != 0)) {
                combobox.combobox('textbox').val($.fn.combobox.defaults.emptyText);
            }
        });
        $(":checkbox", form).each(function () {
            var checkbox = $(this);
            var field = getInfolightOption(checkbox).field;
            if (field != undefined) {
                checkbox.checkbox('setValue', rowData[field]);
            }
        });
        $(".info-refval", form).each(function () {
            var refval = $(this);
            var field = getInfolightOption(refval).field;
            if (field != undefined) {
                //edit by eva 修改refval在編輯狀態無法自行寫程式給預設值
                //refval.refval('setValue', rowData[field]); 
                refval.refval('setValue', formData[field]);
            }
        });
        $(".info-qrcode", form).each(function () {
            var qrcode = $(this);
            var field = getInfolightOption(qrcode).field;
            if (field != undefined) {
                qrcode.infoqrcode('setValue', rowData[field]);
            }
        });
        $(".info-options", form).each(function () {
            var field = getInfolightOption($(this)).field;
            if (field != undefined) {
                $(this).options('setValue', rowData[field]);
            }
        });
        $(".info-yearmonth", form).each(function () {
            var field = getInfolightOption($(this)).field;
            if (field != undefined) {
                $(this).yearmonth('setValue', rowData[field]);
            }
        });
        $(".easyui-datebox", form).each(function () {
            var field = getInfolightOption($(this)).field;
            if (field != undefined) {
                var value = formData[field];
                var dateFormat = $(this).datebox('dateFormat');
                if (dateFormat == 'nvarchar') {
                    if (value != undefined && value.toString().length == 8) {
                        var s = value.toString().substr(0, 4) + '-' + value.toString().substr(4, 2) + '-' + value.toString().substr(6, 2);
                        var date = $.fn.datebox('parseDate', s);
                        var text = $.fn.datebox.defaults.formatter.call($(this), date);
                        $(this).datebox('setValue', text);
                    }
                }
                else {
                    $(this).datebox('setValue', value);
                }
            }
        });
    }
    var chainDataForm = getInfolightOption(form).chainDataForm;
    if (chainDataForm) {
        loadFormRow($(chainDataForm), rowData, mode);
    }
}

function getFormRow(form) {
    var row = {};
    $('input,select,textarea', form).each(function () {
        var field = getInfolightOption($(this)).field;
        var formid = getInfolightOption($(this)).form;
        if (formid != undefined && field != undefined && form.attr('id') == formid) {
            var value = "";
            var controlClass = $(this).attr('class');
            if (controlClass != undefined) {
                if (controlClass.indexOf('easyui-datebox') == 0) {
                    value = $(this).datebox('getBindingValue');
                }
                else if (controlClass.indexOf('easyui-datetimebox') == 0) {
                    value = $(this).datetimebox('getBindingValue');
                }
                else if (controlClass.indexOf('easyui-combobox') == 0) {
                    value = $(this).combobox('getValue');
                }
                else if (controlClass.indexOf('easyui-combogrid') == 0) {
                    value = $(this).combogrid('getValue');
                }
                else if (controlClass.indexOf('info-combobox') == 0) {
                    value = $(this).combobox('getValue');
                }
                else if (controlClass.indexOf('info-combogrid') == 0) {
                    if (getInfolightOption($(this)).multiple == true)
                        value = $(this).combogrid('getValues').toString();
                    else
                        value = $(this).combogrid('getValue');
                }
                else if (controlClass.indexOf('info-refval') == 0) {
                    value = $(this).refval('getValue');
                }
                else if (controlClass.indexOf('info-options') == 0) {
                    value = $(this).options('getValue');
                }
                else if (controlClass.indexOf('info-autocomplete') == 0) {
                    value = $(this).combobox('getValue');
                }
                else if (controlClass.indexOf('info-yearmonth') == 0) {
                    value = $(this).yearmonth('getValue');
                }
                else if (controlClass.indexOf('easyui-numberbox') == 0) {
                    value = $(this).numberbox('getValue');
                }
                else {
                    value = $(this).val();
                }
            }
            else {
                if ($(this).attr('type') == "checkbox") {
                    value = $(this).checkbox('getValue');
                }
                else {
                    value = $(this).val();
                }
            }
            row[field] = value;
        }
    });
    var chainDataForm = getInfolightOption(form).chainDataForm;
    if (chainDataForm) {
        var chaindRow = getFormRow($(chainDataForm));
        for (var p in chaindRow) {
            row[p] = chaindRow[p];
        }
    }
    return row;
}

function openForm(fid, rowData, mode, editmode, keys) {
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
            if (modal) {
                $(fid).dialog({
                    modal: true,
                    onClose: function () {
                        var listId = Request.getQueryStringByName("LISTID");
                        var navMode = Request.getQueryStringByName("NAVMODE");
                        var isAutoPageClose = getInfolightOption(form).isAutoPageClose;
                        var isshowflowicon = getInfolightOption(form).isshowflowicon;

                        if (isshowflowicon == "true" && (listId != "" || navMode != "")) {
                            if (isAutoPageClose == true) {
                                self.parent.closeCurrentTab();
                            }
                        }
                    }
                });
            }

            if (mode == "inserted") {
                var relatedFields = [];
                var parent = getInfolightOption(form).parent;
                var parentRelations = getInfolightOption(form).parentRelations;
                if (parent != undefined && parentRelations != undefined) {
                    for (var i = 0; i < parentRelations.length; i++) {
                        var parentRelation = parentRelations[i];
                        var masterColumn = parentRelation.parentField;
                        var parentControl = $("[name='" + masterColumn + "']", "#" + parent);
                        if (parentControl.length > 0) {  //form
                            var parentValue = '';
                            if (parentControl.hasClass('refval-text')) {
                                parentValue = parentControl.closest('.refval').prev('.refval-f').refval('getValue')
                            }
                            else {
                                parentValue = $("[name='" + masterColumn + "']", "#" + parent).val();
                            }
                            if (parentValue == '' || parentValue == null) {

                            }
                            else {
                                relatedFields.push(parentValue);
                            }
                        }
                        else if ($('#' + parent).hasClass('info-datagrid')) { //grid
                            var parentValue = $('#' + parent).datagrid('getSelected')[masterColumn];
                            if (parentValue == '' || parentValue == null) {

                            }
                            else {
                                relatedFields.push(parentValue);
                            }
                        }
                    }
                }
                if (relatedFields.length > 0 || parent == undefined || parentRelations.length == 0)
                    $(fid).dialog('open');
            }
            else {
                $(fid).dialog('open');
            }

            var dialogLeft = getInfolightOption($(fid)).dialogLeft;
            var dialogTop = getInfolightOption($(fid)).dialogTop;
            var dialogCenter = getInfolightOption($(fid)).dialogCenter;
            if (dialogCenter != true) {
                //            if (dialogLeft != undefined && dialogTop != undefined) {
                $(fid).window('move', {
                    left: dialogLeft,
                    top: $(document).scrollTop() + dialogTop
                });
                //            }
            }
            else {
                var top = $(document).scrollTop() + ($(window).height() - 250) * 0.5;
                $(fid).window('move', {top: top});
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
        var parentRow = {};
        for (var property in rowData) {
            parentRow[property] = rowData[property];
        }
        if (rowData == null) {
            parentRow = null;
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
        var onLoadSuccess = getInfolightOption(form).onLoadSuccess;
        if (onLoadSuccess) {
            onLoadSuccess.call(form);
        }
    }
}
function closeForm(fid) {
    var formname = getInfolightOption($(fid)).containForm;
    var form = $(formname);
    var onCancel = getInfolightOption(form).onCancel;
    if (onCancel != undefined) {
        var flag = onCancel.call(form);
        if (flag != undefined && flag.toString() == 'false') {
            return;
        }
    }
    $(dataGrid_class, fid).each(function () {
        var datagrid = $(this);
        var index = getEditIndex(datagrid);
        if (index != -1) {
            datagrid.datagrid('cancelEdit', index);
            setEditIndex(datagrid, -1);
        }
    });
    var dialoggrid = form.attr('dialogGrid');
    if (dialoggrid == undefined) dialoggrid = form.attr('switchGrid');
    if (dialoggrid == undefined) dialoggrid = form.attr('continueGrid');
    $(dialoggrid).datagrid('removeLock');


    if (form.attr("switchGrid") != undefined && form.attr("switchGrid") != "") {
        $(form.attr("switchGrid")).datagrid('getPanel').panel('expand');
        //show submit button div 
        if (getInfolightOption($(form.attr("switchGrid"))).CollapseDiv != undefined) {
            $(getInfolightOption($(form.attr("switchGrid"))).CollapseDiv).each(function () {
                $(this).css('display', '');
            });
        }
        else if ($("#" + $(form.attr("switchGrid")).attr('id') + "-SubmitDiv") != undefined) {
            $("#" + $(form.attr("switchGrid")).attr('id') + "-SubmitDiv").css('display', '');
        }
        //end
        $(fid).attr('style', 'display:none');
    }
    else if (form.attr("continueGrid") != undefined) {
        var grid = $(form.attr("continueGrid"));
        var rowData = grid.datagrid('getSelected');

        openForm(fid, rowData, "viewed", 'continue');
        var dataGridTabID = getInfolightOption($(fid)).dataGridTabID;
        if (dataGridTabID != undefined) {
            var tabs = $(fid).closest('.easyui-tabs');
            if (tabs != undefined) {
                var tabslist = $(tabs).tabs('tabs');
                for (var i = 0; i < tabslist.length; i++) {
                    if ($(tabslist[i]).attr('id') == dataGridTabID) {
                        $(tabs).tabs('select', i);
                    }
                }
            }
        }
        //        var mode = getEditMode(form);
        //        var rowData = new Object();
        //        if (mode == "inserted") {
        //            rowData = getDefaultValues(form, rowData);
        //            rowData = getSeq(grid, form, rowData);
        //        }
        //        else
        //            rowData = grid.datagrid('getSelected');
        //        form.form('clear');
        //        form.form('load', rowData);
    }
    else {
        $(fid).window('close');
    }
}
function applyForm(fid) {
    //var formname = getInfolightOption($(fid)).containForm;
    var form = $(fid);
    if (form.form('validateForm')) {
        var changedDatas = [];
        var changedRows = {tableName: getInfolightOption(form).tableName, inserted: [], deleted: [], updated: []};
        var row = getFormRow(form);
        //for (var p in row) {
        //    var value = row[p];
            //if (value) {
            //    row[p] = encodeURIComponent(value);
            //    row[p] = row[p].toString().replace(/(%22)/g, "\"").replace(/(%5C)/g, "\\");
            //}
        //}

        var mode = getEditMode(form);
        if (mode == "updated") {
            changedRows.updated.push(row);
        }
        if (mode == "inserted") {
            changedRows.inserted.push(row);
        }
        changedDatas.push(changedRows);
        var remoteName = getInfolightOption(form).remoteName;
        var tableName = getInfolightOption(form).tableName;
        var duplicateCheckS = true;
        if (getInfolightOption(form).duplicateCheck && mode == "inserted") {
            $.fn.Error.errorCode = 1103;
            $.ajax({
                type: "POST",
                url: getDataUrl(),
                data: getRemoteParam({
                    data: $.toJSONString(changedDatas),
                    mode: 'duplicate'
                }, remoteName, tableName, false),
                cache: false,
                async: false,
                success: function (a) {
                    if (a == "false") {
                        alertMessage("duplicatecheckmsg");
                        duplicateCheckS = false;
                    }
                },
                error: function () {
                    duplicateCheckS = false;
                }
            });
        }
        if (duplicateCheckS) {
            $.fn.Error.errorCode = 1105;
            $('.infosysbutton-s', fid).each(function () {
                $(this).linkbutton('disable');
            });
            $.ajax({
                type: "POST",
                url: getDataUrl(),
                data: getRemoteParam({
                    data: $.toJSONString(changedDatas),
                    mode: 'update'
                }, remoteName, tableName, false),
                cache: false,
                async: true,
                beforeSend: function () {
                    $.messager.progress({
                        title: 'Please waiting',
                        msg: 'Saving data...'
                    });
                },
                success: function (data) {
                    if (mode == "inserted") {
                        //设置form的状态为modify
                        var rows = $.parseJSON(data);
                        if (rows.length > 0) {
                            var rowData = rows[0];
                            $(form).form('updateRow', rowData);
                        }
                        setEditMode(form, "updated");
                    }

                    $(dataGrid_class).each(function () {
                        var datagrid = $(this);
                        var dgremoteName = getInfolightOption(datagrid).remoteName;
                        var dgtableName = getInfolightOption(datagrid).tableName;
                        if (dgremoteName == remoteName && dgtableName == tableName) {
                            //var selectedIndex = getSelectedIndex(datagrid);
                            var flowKey = Request.getQueryStringByName("FORM_PRESENTATION");
                            var navMODE = Request.getQueryStringByName("NAVMODE");
                            if (navMODE == "") navMODE = Request.getFlowStringByName(flowKey, "NAVMODE");
                            if (navMODE != undefined && navMODE != null && navMODE == "Insert") {

                            }
                            else {
                                datagrid.datagrid('reload');
                            }
                        }
                    });
                },
                complete: function () {
                    $(document).ready(function () {
                        $.messager.progress('close');
                    });
                    $('.infosysbutton-s', fid).each(function () {
                        $(this).linkbutton('enable');
                    });
                }
            });
        }
    }
}
function submitForm(fid, ignoreOnApply, afterPostSuccess, doAfterSubmittedForFlow, winId, dataFormId, winTitle, selectedRow) {
    var formname = getInfolightOption($(fid)).containForm;
    if (formname != undefined) {
        $.fn.Error.errorCode = 1106;
        var form = $(formname);
        var disapply = getInfolightOption(form).disapply;
        if (disapply == undefined)
            disapply = false;
        var onApply = getInfolightOption(form).onApply;
        if (onApply != undefined && (ignoreOnApply == undefined || ignoreOnApply == false)) {
            var flag = onApply.call(form);
            if (flag != undefined && flag.toString() == 'false') {
                return;
            }
        }
        if (disapply) { //客户需求不保存但要关闭画面
            if (form.attr("switchGrid") != undefined && form.attr("switchGrid") != "") {
                $(form.attr("switchGrid")).datagrid('getPanel').panel('expand');
                $(fid).attr('style', 'display:none');
                //show submit button div 
                if (getInfolightOption($(form.attr("switchGrid"))).CollapseDiv != undefined) {
                    $(getInfolightOption($(form.attr("switchGrid"))).CollapseDiv).each(function () {
                        $(this).css('display', '');
                    });
                }
                else if ($("#" + $(form.attr("switchGrid")).attr('id') + "-SubmitDiv") != undefined) {
                    $("#" + $(form.attr("switchGrid")).attr('id') + "-SubmitDiv").css('display', '');
                }
                //end
            }
            else if (form.attr("continueGrid") != undefined && form.attr("continueGrid") != "") {
                var grid = $(form.attr("continueGrid"));
                var rowData = grid.datagrid('getSelected');
                openForm(fid, rowData, "viewed", 'continue');
                var dataGridTabID = getInfolightOption($(fid)).dataGridTabID;
                if (dataGridTabID != undefined) {
                    var tabs = $(fid).closest('.easyui-tabs');
                    if (tabs != undefined) {
                        var tabslist = $(tabs).tabs('tabs');
                        for (var i = 0; i < tabslist.length; i++) {
                            if ($(tabslist[i]).attr('id') == dataGridTabID) {
                                $(tabs).tabs('select', i);
                            }
                        }
                    }
                }
            }
            else {
                $(fid).window('close');
            }
        }
        else if (form.form('validateForm')) {
            var beforeApply = getInfolightOption(form).beforeApply;

            if (beforeApply != undefined && (ignoreOnApply == undefined || ignoreOnApply == false)) {
                eval(beforeApply).call(form);
                return;
            }
            var changedDatas = [];
            var changedRows = {tableName: getInfolightOption(form).tableName, inserted: [], deleted: [], updated: []};

            var dialoggrid = form.attr('dialogGrid');
            if (dialoggrid == undefined) dialoggrid = form.attr('switchGrid');
            if (dialoggrid == undefined) dialoggrid = form.attr('continueGrid');
            var autoApply = getInfolightOption($(dialoggrid)).autoApply;
            var row = getFormRow(form);
            var encodeRow = {};
            for (var p in row) {
                var value = row[p];
                //if (value) {
                //    encodeRow[p] = encodeURIComponent(value);
                //    encodeRow[p] = encodeRow[p].toString().replace(/(%22)/g, "\"").replace(/(%5C)/g, "\\");
                //}
                //else {
                    encodeRow[p] = value;
                //}
            }

            if (dialoggrid != undefined && autoApply != undefined && !autoApply) {
                var mode = getEditMode(form);
                if (mode == "updated") {
                    var index = getSelectedIndex($(dialoggrid));
                    if ($(dialoggrid).attr('gridUpdateIndex') != undefined) {
                        index = eval($(dialoggrid).attr('gridUpdateIndex'));
                    }
                    $(dialoggrid).datagrid('beginEdit', index);
                    for (var field in row) {
                        var editor = $(dialoggrid).datagrid('getEditor', {index: index, field: field});
                        if (editor) {
                            editor.actions.setValue(editor.target, row[field]);
                        }
                        else {
                            alert("Datagrid:" + $(dialoggrid).attr('id') + " 找不到欄位:" + field);
                        }
                    }
                    $(dialoggrid).datagrid('endEdit', index);
                    $(dialoggrid).datagrid('changeState', 'editing');
                    //$(dialoggrid).datagrid('updateRow', { index: index, row: row });

                }
                var duplicateCheckS = true;
                if (mode == "inserted") {
                    $.fn.Error.errorCode = 1107;
                    if (getInfolightOption(form).duplicateCheck) {
                        var keycolumns = $(dialoggrid).attr("keyColumns");
                        var keycolumnssplit = keycolumns.split(',');
                        //var isold = false;
                        var rows = $(dialoggrid).datagrid('getRows');
                        for (var i = 0; i < rows.length; i++) {
                            var a = true;
                            for (var j = 0; j < keycolumnssplit.length; j++) {
                                var oldvalue = rows[i][keycolumnssplit[j]];
                                var newvalue = row[keycolumnssplit[j]];
                                if (oldvalue != newvalue) {
                                    a = false;
                                    break;
                                }
                            }
                            if (a) {
                                alertMessage("duplicatecheckmsg");
                                duplicateCheckS = false;
                                break;
                            }
                        }
                    }
                    if (duplicateCheckS) {
                        $(dialoggrid).datagrid('appendRow', row);
                        $(dialoggrid).data('lastInsertedRow', row);
                        $(dialoggrid).datagrid('changeState', 'editing');
                        var index = $(dialoggrid).datagrid('getRows').length - 1;
                        $(dialoggrid).datagrid('selectRow', index);
                        $(dialoggrid).datagrid('setCurrentRow', row);
                    }
                }
                if (duplicateCheckS) {
                    if ($(dialoggrid).datagrid('options').showFooter == true && $(dialoggrid).datagrid('options').pagination == false) {
                        setFooter($(dialoggrid));
                    }
                    if (form.attr("switchGrid") != undefined && form.attr("switchGrid") != "") {
                        $(form.attr("switchGrid")).datagrid('getPanel').panel('expand');
                        $(fid).attr('style', 'display:none');
                        //show submit button div 
                        if (getInfolightOption($(form.attr("switchGrid"))).CollapseDiv != undefined) {
                            $(getInfolightOption($(form.attr("switchGrid"))).CollapseDiv).each(function () {
                                $(this).css('display', '');
                            });
                        }
                        else if ($("#" + $(form.attr("switchGrid")).attr('id') + "-SubmitDiv") != undefined) {
                            $("#" + $(form.attr("switchGrid")).attr('id') + "-SubmitDiv").css('display', '');
                        }
                        //end
                    }
                    else if (form.attr("continueGrid") != undefined && form.attr("continueGrid") != "") {
                        var grid = $(form.attr("continueGrid"));
                        var rowData = grid.datagrid('getSelected');
                        openForm(fid, rowData, "viewed", 'continue');
                        var dataGridTabID = getInfolightOption($(fid)).dataGridTabID;
                        if (dataGridTabID != undefined) {
                            var tabs = $(fid).closest('.easyui-tabs');
                            if (tabs != undefined) {
                                var tabslist = $(tabs).tabs('tabs');
                                for (var i = 0; i < tabslist.length; i++) {
                                    if ($(tabslist[i]).attr('id') == dataGridTabID) {
                                        $(tabs).tabs('select', i);
                                    }
                                }
                            }
                        }
                    }
                    else {
                        $(fid).window('close');
                    }
                    if (mode == "inserted" && getInfolightOption(form).continueAdd) {
                        rowData = getDefaultValues(form);
                        rowData = getSeq($(dialoggrid), form, rowData);
                        var editMode = getInfolightOption($(dialoggrid)).editMode;
                        openForm(fid, rowData, "inserted", editMode);
                    }
                }
            }
            else {
                var mode = getEditMode(form);
                if (mode == "updated") {
                    changedRows.updated.push(encodeRow);
                }
                if (mode == "inserted") {
                    changedRows.inserted.push(encodeRow);
                }
                changedDatas.push(changedRows);
                var validate = true;
                $(dataGrid_class, fid).each(function () {
                    var datagrid = $(this);
                    var parent = getInfolightOption(datagrid).parent;
                    if (parent != undefined && parent == form.attr('id')) {
                        if (!endEdit(datagrid)) {
                            validate = false;
                            return false;
                        }
                        var changes = datagrid.datagrid('getChangedDatas', true);
                        if (changes.length > 0) {
                            for (var i = 0; i < changes.length; i++) {
                                changedDatas.push(changes[i]);
                            }
                        }
                    }
                });
                if (!validate) {
                    return;
                }


                var remoteName = getInfolightOption(form).remoteName;
                var tableName = getInfolightOption(form).tableName;
                var duplicateCheckS = true;
                if (getInfolightOption(form).duplicateCheck && mode == "inserted") {
                    $.fn.Error.errorCode = 1108;
                    $.ajax({
                        type: "POST",
                        url: getDataUrl(),
                        data: getRemoteParam({
                            data: $.toJSONString(changedDatas),
                            mode: 'duplicate'
                        }, remoteName, tableName, false),
                        cache: false,
                        async: false,
                        success: function (a) {
                            if (a == "false") {
                                alertMessage("duplicatecheckmsg");
                                duplicateCheckS = false;
                            }
                        },
                        error: function () {
                            duplicateCheckS = false;
                        }
                    });
                }

                if (duplicateCheckS) {
                    $.fn.Error.errorCode = 1109;

                    $('.infosysbutton-s', fid).each(function () {
                        $(this).linkbutton('disable');
                    });

                    $.ajax({
                        type: "POST",
                        url: getDataUrl(),
                        data: getRemoteParam({
                            data: $.toJSONString(changedDatas),
                            mode: 'update'
                        }, remoteName, tableName, false),
                        cache: false,
                        async: true,
                        beforeSend: function () {
                            //var win = $.messager.progress({
                            //    title: 'Please waiting',
                            //    msg: 'Saving data...'
                            //});
                        },
                        success: function (data) {
                            $(dialoggrid).each(function () {
                                var datagrid = $(this);
                                datagrid.datagrid('removeLock');
                                if (datagrid.datagrid('options').showFooter == true && datagrid.datagrid('options').pagination == false) {
                                    setFooter(datagrid);
                                }

                                //var dgremoteName = getInfolightOption(datagrid).remoteName;
                                //var dgtableName = getInfolightOption(datagrid).tableName;
                                //                            if (dgremoteName == remoteName && dgtableName == tableName) {
                                var selectedIndex = getSelectedIndex(datagrid);

                                if ($(datagrid).attr('gridUpdateIndex') != undefined) {
                                    selectedIndex = eval($(datagrid).attr('gridUpdateIndex'));
                                }

                                if (mode == "updated" && selectedIndex != -1) {
                                    datagrid.datagrid('updateRow', {index: selectedIndex, row: row});
                                }
                                else {
                                    if (mode == "inserted") {
                                        datagrid.data('lastInsertedRow', row);
                                    }

                                    var parent = getInfolightOption(datagrid).parent;
                                    var flowKey = Request.getQueryStringByName("FORM_PRESENTATION");
                                    var flowFileName = Request.getQueryStringByName("FLOWFILENAME");
                                    if (flowFileName == "") flowFileName = Request.getFlowStringByName(flowKey, "FLOWFILENAME");

                                    if (parent) {
                                        var queryParams = datagrid.datagrid('options').queryParams;
                                        var queryWord = $.parseJSON(queryParams.queryWord);
                                        if (queryWord.parentTableName && !queryWord.parentRow) {
                                            datagrid.datagrid('appendRow', row);
                                            datagrid.datagrid('acceptChanges');

                                        }
                                        else {
                                            if (flowFileName == "")
                                                datagrid.datagrid('reload');
                                        }
                                    }
                                    else {
                                        if (flowFileName == "")
                                            datagrid.datagrid('reload');
                                    }
                                }

                                //                            }
                            });
                            $(dataGrid_class, fid).each(function () {
                                var parent = getInfolightOption($(this)).parent;
                                if (parent != undefined && parent == form.attr('id')) {
                                    $(this).datagrid('acceptChanges');
                                }
                            });

                            var flowKey = Request.getQueryStringByName("FORM_PRESENTATION");
                            var navMODE = Request.getQueryStringByName("NAVMODE");
                            if (navMODE == "") navMODE = Request.getFlowStringByName(flowKey, "NAVMODE");
                            var flowFileName = Request.getQueryStringByName("FLOWFILENAME");
                            if (flowFileName == "") flowFileName = Request.getFlowStringByName(flowKey, "FLOWFILENAME");
                            if (form.attr("switchGrid") != undefined && form.attr("switchGrid") != "") {
                                if (flowFileName == "" && Request.getFlowStringByName(flowKey, "NAVIGATOR_MODE") != "2" && navMODE != "2") {
                                    $(form.attr("switchGrid")).datagrid('getPanel').panel('expand');
                                    $(fid).attr('style', 'display:none');
                                    //show submit button div 
                                    if (getInfolightOption($(form.attr("switchGrid"))).CollapseDiv != undefined) {
                                        $(getInfolightOption($(form.attr("switchGrid"))).CollapseDiv).each(function () {
                                            $(this).css('display', '');
                                        });
                                    }
                                    else if ($("#" + $(form.attr("switchGrid")).attr('id') + "-SubmitDiv") != undefined) {
                                        $("#" + $(form.attr("switchGrid")).attr('id') + "-SubmitDiv").css('display', '');
                                    }

                                }
                                else {
                                    $('#DialogSubmit', fid).hide();
                                    var rows = $.parseJSON(data);
                                    if (rows != null && rows.length > 0) {
                                        var rowData = rows[0];
                                        $(form).form('updateRow', rowData);
                                        var parentRow = {};
                                        for (var property in rowData) {
                                            parentRow[property] = rowData[property];
                                        }
                                        $(form).form('updateRow', rowData);
                                        $(dataGrid_class, fid).each(function () {
                                            var datagrid = $(this);
                                            var parent = getInfolightOption(datagrid).parent;
                                            if (parent != undefined && parent == form.attr('id')) {
                                                initInfoDataGrid(datagrid, tableName, parentRow);
                                            }
                                        });
                                    }
                                    setEditMode(form, "viewed");
                                    if (afterPostSuccess != undefined) {
                                        changeFormState(fid, "viewed");
                                        afterPostSuccess();
                                    }
                                    else {
                                        var successeMessage = $.sysmsg('getValue', 'Srvtools/AnyQuery/SaveSuccess');
                                        alert(successeMessage);
                                        changeFormState(fid, "viewed");
                                    }
                                    if (doAfterSubmittedForFlow) {
                                        doAfterSubmittedForFlow(winId, dataFormId, winTitle, selectedRow);
                                    }
                                }
                                //end
                            }
                            else if (form.attr("continueGrid") != undefined) {
                                var grid = $(form.attr("continueGrid"));
                                var rowData = grid.datagrid('getSelected');
                                openForm(fid, rowData, "viewed", 'continue');
                                var dataGridTabID = getInfolightOption($(fid)).dataGridTabID;
                                if (dataGridTabID != undefined) {
                                    var tabs = $(fid).closest('.easyui-tabs');
                                    if (tabs != undefined) {
                                        var tabslist = $(tabs).tabs('tabs');
                                        for (var i = 0; i < tabslist.length; i++) {
                                            if ($(tabslist[i]).attr('id') == dataGridTabID) {
                                                $(tabs).tabs('select', i);
                                            }
                                        }
                                    }
                                }
                            }
                            else {
                                if (flowFileName == "" && Request.getFlowStringByName(flowKey, "NAVIGATOR_MODE") != "2" && navMODE != "2" || getInfolightOption(form).isshowflowicon == "false") {
                                    if (mode == "inserted" && getInfolightOption(form).continueAdd) {

                                        rowData = getDefaultValues(form);
                                        rowData = getSeq($(dialoggrid), form, rowData);
                                        var editMode = getInfolightOption($(dialoggrid)).editMode;
                                        openForm(fid, rowData, "inserted", editMode);

                                    }
                                    else {
                                        $(fid).window('close');
                                    }
                                }
                                else {
                                    $('#DialogSubmit', fid).hide();
                                    var rows = $.parseJSON(data);
                                    if (rows != null && rows.length > 0) {
                                        var rowData = rows[0];
                                        var parentRow = {};
                                        for (var property in rowData) {
                                            parentRow[property] = rowData[property];
                                        }
                                        $(form).form('updateRow', rowData);
                                        $(dataGrid_class, fid).each(function () {
                                            var datagrid = $(this);
                                            var parent = getInfolightOption(datagrid).parent;
                                            if (parent != undefined && parent == form.attr('id')) {
                                                initInfoDataGrid(datagrid, tableName, parentRow);
                                            }
                                        });
                                    }
                                    setEditMode(form, "viewed");
                                    if (afterPostSuccess != undefined) {
                                        changeFormState(fid, "viewed");
                                        afterPostSuccess();
                                    }
                                    else {
                                        var successeMessage = $.sysmsg('getValue', 'Srvtools/AnyQuery/SaveSuccess');
                                        alert(successeMessage);
                                        changeFormState(fid, "viewed");
                                        //alert("Save successfully");
                                    }

                                    if (doAfterSubmittedForFlow) {
                                        doAfterSubmittedForFlow(winId, dataFormId, winTitle, selectedRow);
                                    }
                                }
                            }
                            var onApplied = getInfolightOption(form).onApplied;
                            if (onApplied) {
                                var rows = $.parseJSON(data);
                                onApplied.call(form, rows);
                            }
                        },
                        complete: function () {
                            $(document).ready(function () {
                                $.messager.progress('close');
                            });
                            $('.infosysbutton-s', fid).each(function () {
                                $(this).linkbutton('enable');
                            });
                        }
                    });
                }
            }
        }
    }
    else {
        var onSubmited = getInfolightOption($(fid)).onSubmited;
        if (onSubmited) {
            var ok = onSubmited.call(fid);
            if (ok) {
                $(fid).dialog('close');
            }
        }
    }
}
//-----------------------------------------------------------------------------------------------------------------

function setQueryDefault(pnid) {
    var obj = {};
    var defaultmethods = {};
    var hasRemote = false;
    $(":text,select,:checkbox", pnid).each(function () {
        var text = $(this);
        var defaultValue = getInfolightOption(text).defaultValue;
        if (defaultValue != undefined) {
            var field = getInfolightOption(text).field;
            var condition = getInfolightOption(text).condition;
            if (defaultValue.indexOf("client[") == 0) {
                var methodName = defaultValue.replace("client[", "").replace("]", "");
                obj[field + condition] = eval(methodName).call();
            }
            else if (defaultValue.indexOf("remote[") == 0) {
                defaultmethods[field + condition] = defaultValue.replace("remote[", "").replace("]", "");
                hasRemote = true;
            }
            else {
                obj[field + condition] = defaultValue;
            }
        }
    });
    if (hasRemote) {
        var defaultObjs = getDefault($.toJSONString(defaultmethods));
        var defaultObj = $.parseJSON(defaultObjs);
        for (var property in defaultObj) {
            obj[property] = defaultObj[property];
        }
    }
    $(":text,select,:checkbox", pnid).each(function () {
        var field = getInfolightOption($(this)).field;
        var condition = getInfolightOption($(this)).condition;
        if (obj[field + condition] != undefined) {
            var value = obj[field + condition];
            $(this).val(value);
            if ($(this).is(":checkbox")) {
                $(this).attr('checked', value);
            }
            var controlClass = $(this).attr('class');
            if (controlClass != undefined) {
                if (controlClass.indexOf('easyui-datebox') == 0 && controlClass.indexOf('datebox-f') >= 0) {
                    $(this).datebox('setValue', value);
                }
                else if (controlClass.indexOf('easyui-datetimebox') == 0) {
                    $(this).datetimebox('setValue', value);
                }
                else if (controlClass.indexOf('info-combobox') == 0) {
                    $(this).combobox('setValue', value);
                }
                else if (controlClass.indexOf('info-combogrid') == 0) {
                    $(this).combogrid('setValue', value);
                }
                else if (controlClass.indexOf('info-refval') == 0) {
                    $(this).refval('setValue', value);
                }
                else if (controlClass.indexOf('info-options') == 0) {
                    $(this).options('setValue', value);
                }
                else if (controlClass.indexOf('info-autocomplete') == 0) {
                    $(this).combobox('setValue', value);
                }
                else if (controlClass.indexOf('info-yearmonth') == 0) {
                    $(this).yearmonth('setValue', value);
                }
                else {
                    $(this).val(value);
                }
            }
        }
        var format = getInfolightOption($(this)).format;
        if (format != undefined) {
            if (format.indexOf('YY') == 0) {
                if ($(this).hasClass('easyui-datebox')) {
                    $(this).datebox('rocYear', true);
                }
            }
        }
    });
}
//query
function openQuery(dgid) {
    //var infolightOptions = getInfolightOption($(dgid));
    //if (infolightOptions.queryAutoColumn) {
    //    queryGrid(dgid);
    //}
    //else {
    var pnid = getInfolightOption($(dgid)).queryDialog;
    if (pnid != undefined) {
        clearQuery(dgid);
        //$(":text,select", pnid).val('');
        setQueryDefault(pnid);
        $(pnid).window('open');
        var queryLeft = getInfolightOption($(dgid)).queryLeft;
        var queryTop = getInfolightOption($(dgid)).queryTop;
        if (queryTop != undefined) {
            $(pnid).window('move', {
                top: $(document).scrollTop() + queryTop
            });
        }
        if (queryLeft != undefined) {
            $(pnid).window('move', {
                left: queryLeft
            });
        }
    }
    //}
}
function query(dgid) {
    queryGrid(dgid);
    var pnid = getInfolightOption($(dgid)).queryDialog;
    if (pnid != undefined) {
        if ($(pnid).attr('class') == "easyui-window panel-body panel-body-noborder window-body") {
            $(pnid).window('close');
        }
    }
}
function queryAutoColumn(dgid) {
    var where = $(dgid).datagrid('getWhereAutoQuery');
    var onFilter = $(dgid).datagrid('options').onFilter;
    if (onFilter) {
        where = onFilter.call($(dgid), where);
    }
    $(dgid).datagrid('setWhere', where);
}
function queryGrid(dgid) {
    var where = $(dgid).datagrid('getWhere');
    $(dgid).datagrid('setWhere', where);
}
function formatQueryValue(value, type, isNvarchar) {
    switch (type) {
        case 'string':
        {
            var nvarchar = '';
            if (isNvarchar) {
                nvarchar = 'N';
            }
            return nvarchar + "'" + value.toString().replace(/\'/g, "''") + "'";
        }
        case 'datetime':
        {
            if (databaseType == 'oracle') {
                if (value.length > 10)
                    return "to_date('" + value + "','yyyy-mm-dd HH24:Mi:SS')";
                //value = value.slice(0, 10);
                return "to_date('" + value + "','yyyy-mm-dd')";
            }
            else {
                return "'" + value.toString().replace(/\'/g, "''") + "'";
            }
        }
        default:
        {
            if (value && value.toString().match(/[;\(\)]/g)) {
                alert('您輸入的數據類型不符');
                return 0;
            }
            return value;
        }
    }
}
function clearQuery(dgid) {
    var isQueryAutoColumn = getInfolightOption($(dgid)).queryAutoColumn;
    if (isQueryAutoColumn == true) {
        //var pnid = getInfolightOption($(dgid)).queryDialog;
        var queryTr = $('#queryTr_' + $(dgid)[0].id);
        //var queryParams = $(dgid).datagrid('options').queryParams;
        //var queryWord = {};

        //var where = '';
        $(":text,select", queryTr).each(function () {
            var text = $(this);
            text.val('');
        });
    }
    //    else {
    var pnid = getInfolightOption($(dgid)).queryDialog;
    if (pnid != undefined) {
        // var queryParams = $(dgid).datagrid('options').queryParams;
        // var queryWord = {};

        //var where = '';
        $(":text,select", pnid).each(function () {
            var text = $(this);
            text.val('');
            var controlClass = $(this).attr('class');
            if (controlClass != undefined) {
                if (controlClass.indexOf('easyui-datebox') == 0) {
                    text.datebox('setValue', '');
                }
                else if (controlClass.indexOf('easyui-datetimebox') == 0) {
                    text.datetimebox('setValue', '');
                }
                else if (controlClass.indexOf('info-combobox') == 0) {
                    text.combobox('setValue', '');
                }
                else if (controlClass.indexOf('info-combogrid') == 0) {
                    text.combogrid('setValue', '');
                }
                else if (controlClass.indexOf('combo-text') == 0) {
                    value = '';
                }
                else if (controlClass.indexOf('info-refval') == 0) {
                    text.refval('setValue', '');
                }
                else if (controlClass.indexOf('info-autocomplete') == 0) {
                    text.combobox('setValue', '');
                }
            }
        });
        $(":radio,:checkbox", pnid).each(function () {
            $(this).prop("checked", false);
        });
        //        }
    }
}

//-----------------------------------------------------------------------------------------------------------------

//combobox method
function comboBeforeLoad(param) {
    var value = $(this).combobox('getValue');
    var where = $(this).combobox('getWhere');
    var queryWord = {whereString: ''};
    if (value != '') {
        var textField = $(this).combobox('options').textField;
        queryWord.whereString += '(' + textField + " like '" + value.toString().replace(/\'/g, "''") + "%'";
        var valueField = $(this).combobox('options').valueField;
        queryWord.whereString += " or " + valueField + " like '" + value.toString().replace(/\'/g, "''") + "%')";

        $(this).data('comboSearchValue', value);
    }
    if (where != undefined && where != '') {
        if (value != '') {
            queryWord.whereString += "and (" + where + ")";
        }
        else {
            queryWord.whereString += where;
        }
    }
    param.queryWord = $.toJSONString(queryWord);

    var onBeforeLoad = getInfolightOption($(this)).onBeforeLoad;
    if (onBeforeLoad) {
        return onBeforeLoad.call(this, param);
    }
}
$.extend($.fn.combobox.defaults, {
    emptyText: "Please select"
});

$.extend($.fn.combobox.methods, {
    getWhere: function (jq) {
        return $(jq[0]).attr('whereString');
    },
    setWhere: function (jq, where) {
        jq.each(function () {
            var combobox = $(this);
            var value = $(this).combobox('getValue');
            combobox.combobox('setValue', '');
            combobox.attr('whereString', where);
            combobox.combobox('reload');
            combobox.combobox('setValue', value);
            if (value == undefined || value == "") {
                setTimeout(function () {
                    combobox.combobox('textbox').val($.fn.combobox.defaults.emptyText);
                }, 500);
            }
        });
    }
});

function getTextField(val, row) {
    if ($(this)[0].editor == undefined)
        return;
    var options = $(this)[0].editor.options;
    var items = options.items;
    if (items != undefined) {
        for (var i = 0; i < items.length; i++) {
            if (items[i].value == val) {
                return items[i].text;
            }
        }
        return '';
    }
    else {
        if (options.cacheRelationText == true) {
            if (options.textValues == undefined) {
                options.textValues = {};
            }
            else {
                if (options.textValues[val] != undefined) {
                    return options.textValues[val];
                }
            }
        }

        var whereItem = '';
        if ($(this)[0].editor.type == "inforefval") {
            options.row = row;
            whereItem = $(this).refval('getWhereItem', options);
        }
        var text = getDisplayValue(val, options, whereItem);
        //先只加了refval
        if ($(this)[0].editor.type == "inforefval" && options.showValueAndText != undefined && options.showValueAndText == true) {
            if (!((val == null || val == "") && (text == null || text == "")))
                text = val + ":" + text;
        }

        if (options.cacheRelationText == true) {
            options.textValues[val] = text;
        }

        return text;
    }
}
//-----------------------------------------------------------------------------------------------------------------
//datebox method

$.extend($.fn.datebox.methods, {
    dateFormat: function (jq, format) {
        if (jq.length > 0) {
            if (format == undefined) {
                if ($(jq[0]).datebox('options').dateFormat != undefined) {
                    return $(jq[0]).datebox('options').dateFormat;
                }
                else {
                    return $(jq[0]).data("dateFormat");
                }
            }
            else {
                $(jq[0]).data("dateFormat", format);
            }
        }
    },
    rocYear: function (jq, roc) {
        if (jq.length > 0) {
            if (roc == undefined) {
                return $(jq[0]).data("rocYear");
            }
            else {
                if (roc == true) {
                    //修改parser和formatter
                    $(jq[0]).datebox({
                        formatter: formatroc,
                        parser: parseroc
                    });
                }
                $(jq[0]).data("rocYear", true);
            }
        }
    },
    getBindingValue: function (jq) {
        if (jq.length > 0) {
            var text = $(jq[0]).datebox('getValue');
            if (text != '') {
                var date = $(jq[0]).datebox('options').parser.call($(jq[0]), text);
                var year = date.getFullYear();
                var month = (date.getMonth() + 1).toString();
                if (month.length == 1) {
                    month = '0' + month;
                }
                var date = date.getDate().toString();
                if (date.length == 1) {
                    date = '0' + date;
                }
                var dateFormat = $(jq[0]).datebox('dateFormat');
                if (dateFormat == 'nvarchar') {
                    return year + month + date;
                }
                else {
                    return year + '-' + month + '-' + date;
                }
            }
            else {
                return '';
            }
        }
        return '';
    },
    parseDate: function (jq, s) {
        if (!s) return new Date();
        var sp = s.split(/\s+|T/g);

        var ss = sp[0].split('-');
        var y = parseInt(ss[0], 10);
        var m = parseInt(ss[1], 10);
        var d = parseInt(ss[2], 10);
        if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
            if (sp.length == 1) {
                return new Date(y, m - 1, d);
            }
            else {
                var ss = sp[1].split(':');
                var h = parseInt(ss[0], 10);
                var mi = parseInt(ss[1], 10);
                var s = parseInt(ss[2], 10);
                if (!isNaN(h) && !isNaN(mi) && !isNaN(s)) {
                    return new Date(y, m - 1, d, h, mi, s);
                }
                else {
                    return "Invalid Date";
                }
            }
        } else {
            return "Invalid Date";
        }
    }
});

function formatroc(date) {
    var y = date.getFullYear() - 1911;
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return y + '.' + (m < 10 ? ('0' + m) : m) + '.' + (d < 10 ? ('0' + d) : d);
}
function parseroc(s) {
    if (!s) return new Date();
    var ss = s.split('.');
    var y = parseInt(ss[0], 10);
    var m = parseInt(ss[1], 10);
    var d = parseInt(ss[2], 10);
    if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
        return new Date(y + 1911, m - 1, d);
    } else {
        return new Date();
    }
}
$.extend($.fn.datetimebox.methods, {
    getBindingValue: function (jq) {
        if (jq.length > 0) {
            var text = $(jq[0]).datetimebox('getValue');
            if (text != '') {
                var date = $(jq[0]).datetimebox('options').parser.call($(jq[0]), text);
                var year = date.getFullYear();
                var month = (date.getMonth() + 1).toString();
                if (month.length == 1) {
                    month = '0' + month;
                }
                var day = date.getDate().toString();
                if (day.length == 1) {
                    day = '0' + day;
                }
                var hour = date.getHours().toString();
                if (hour.length == 1) {
                    hour = '0' + hour;
                }
                var minute = date.getMinutes().toString();
                if (minute.length == 1) {
                    minute = '0' + minute;
                }
                var second = date.getSeconds().toString();
                if (second.length == 1) {
                    second = '0' + second;
                }
                var dateFormat = $(jq[0]).datebox('dateFormat');
                if (dateFormat == 'nvarchar') {
                    return year + month + day;
                }
                else {
                    return year + '-' + month + '-' + day + " " + hour + ":" + minute + ":" + second;
                }
            }
            else {
                return '';
            }
        }
        return '';
    }
});

//-----------------------------------------------------------------------------------------------------------------

//default method
function getDefaultValues(element, obj) {
    $.fn.Error.errorCode = 1200;
    if (obj == undefined) {
        obj = {};
    }
    var defaultmethods = {};
    var hasRemote = false;
    $("th,input,select,textarea", element).each(function () {
        var column = $(this);
        var field = getInfolightOption(column).field;
        var datagrid = null;
        if (element.hasClass(dataGrid_class.substring(1))) {
            datagrid = element;
        }
        else {
            var dialoggrid = element.attr('dialogGrid');
            if (dialoggrid == undefined) dialoggrid = element.attr('switchGrid');
            if (dialoggrid == undefined) dialoggrid = element.attr('continueGrid');
            if (dialoggrid) {
                datagrid = $(dialoggrid);
            }
        }

        var carryOn = getInfolightOption(column).carryOn;
        if (datagrid != null && carryOn) {
            var lastInsertedRow = datagrid.data('lastInsertedRow');
            if (lastInsertedRow) {
                obj[field] = lastInsertedRow[field];
                return true;
            }
        }

        var defaultValue = getInfolightOption(column).defaultValue;
        if (defaultValue != undefined) {

            if (defaultValue.indexOf("client[") == 0) {
                var methodName = defaultValue.replace("client[", "").replace("]", "");
                obj[field] = eval(methodName).call();
            }
            else if (defaultValue.indexOf("remote[") == 0) {
                defaultmethods[field] = defaultValue.replace("remote[", "").replace("]", "");
                hasRemote = true;
            }
            else {
                obj[field] = defaultValue;
            }
        }
    });
    if (hasRemote) {
        var defaultObjs = getDefault($.toJSONString(defaultmethods));
        var defaultObj = $.parseJSON(defaultObjs);
        for (var property in defaultObj) {
            obj[property] = defaultObj[property];
        }
    }

    var parent = getInfolightOption(element).parent;
    var parentRelations = getInfolightOption(element).parentRelations;
    if (parent != undefined && parentRelations != undefined) {
        for (var i = 0; i < parentRelations.length; i++) {
            var parentRelation = parentRelations[i];
            var detailColumn = parentRelation.field;
            var masterColumn = parentRelation.parentField;
            var parentControl = $("[name='" + masterColumn + "']", "#" + parent);
            if (parentControl.length > 0) {  //form
                var parentValue = '';
                if (parentControl.hasClass('refval-text')) {
                    parentValue = parentControl.closest('.refval').prev('.refval-f').refval('getValue')
                }
                else {
                    parentValue = $("[name='" + masterColumn + "']", "#" + parent).val();
                }
                if (parentValue == '' || parentValue == null) {
                    $.messager.alert('Error', $.fn.datagrid.defaults.parentEmptyMessage, 'error');
                    return null;
                }
                else {
                    obj[detailColumn] = parentValue;
                }
            }
            else { //grid
                var parentRow = $("#" + parent + dataGrid_class).datagrid('getSelected');
                if (parentRow != null) {
                    if (parentRow[masterColumn] == '') {
                        $.messager.alert('Error', $.fn.datagrid.defaults.parentEmptyMessage, 'error');
                        return null;
                    }
                    else {
                        obj[detailColumn] = parentRow[masterColumn];
                    }
                }
            }
        }
    }
    return obj;
}
function getDefault(method) {
    var obj = '';
    $.ajax({
        type: "POST",
        url: window.currentUrl,
        data: "mode=default&method=" + method,
        cache: false,
        async: false,
        success: function (data) {
            obj = data;
        }, error: function () {
            obj = null;
        }
    });
    return obj;
}
//----------------------------------------------------------------------------------------------------------------

//footer total method
function setFooter(datagrid) {
    var footerRows = datagrid.datagrid('getFooterRows');
    if (footerRows == undefined) {
        return {};
        //var footerRow = new Object();
        //footerRows.add(footerRow);
    }
    var totalCaption = datagrid.attr('totalCaption');
    var totalWord = $.parseJSON(datagrid.attr('totalColumn'));
    var rows = datagrid.datagrid('getRows');

    for (var totaleach in totalWord) {
        var totalsingle = totalWord[totaleach];
        var fun = totalsingle.type;
        var method = totalsingle.method;
        var result = 0;
        for (var i = 0; i < rows.length; i++) {
            var oldvalue = rows[i][totaleach];
            oldvalue = parseFloat(oldvalue);

            if (!isNaN(oldvalue)) {
                if (fun == 'sum' || fun == 'avg') {
                    result = accAdd(result, oldvalue);
                }
                else if (fun == 'max') {
                    if (i == 0) result = oldvalue;
                    if (result < oldvalue) {
                        result = oldvalue;
                    }
                }
                else if (fun == 'min') {
                    if (i == 0) result = oldvalue;
                    if (result > oldvalue) {
                        result = oldvalue;
                    }
                }
            }
        }
        if (fun == 'sum' || fun == 'max' || fun == 'min') {
            footerRows[0][totaleach] = result;
        }
        else if (fun == 'avg') {
            footerRows[0][totaleach] = eval(result / rows.length);
        }
        else if (fun == 'count') {
            footerRows[0][totaleach] = rows.length;
        }
        if (method != undefined && eval(method) != undefined) {
            var result = eval(method).call(datagrid, footerRows[0]);
            if (result != undefined) {
                footerRows[0][totaleach] = result;
            }
        }
    }

    if (footerRows != undefined) {
        datagrid.datagrid('reloadFooter', footerRows);
    }
}
function accAdd(arg1, arg2) {
    var r1, r2, m;
    try {
        r1 = arg1.toString().split(".")[1].length
    } catch (e) {
        r1 = 0
    }
    try {
        r2 = arg2.toString().split(".")[1].length
    } catch (e) {
        r2 = 0
    }
    m = Math.pow(10, Math.max(r1, r2));
    return (arg1 * m + arg2 * m) / m;
}
//----------------------------------------------------------------------------------------------------------------

//seq method
function getSeq(grid, form, rowData) {
    if (rowData == undefined)
        rowData = {};
    var element = grid;
    var eacht = "th,input,select,textarea";
    if (form != undefined) {
        element = form;
        eacht = "input,select,textarea";
    }
    $(eacht, element).each(function () {
        var column = $(this);
        var autoSeq = getInfolightOption(column).autoSeq;
        if (autoSeq != undefined) {
            var field = getInfolightOption(column).field;
            if (autoSeq.length > 0) {
                var numDig = autoSeq[0].numDig;
                var startValue = autoSeq[0].startValue;
                var step = autoSeq[0].step;
               
                var queryWord = $(grid).datagrid('options').queryParams.queryWord;
                var autoApply = getInfolightOption($(grid)).autoApply;
                var parent = getInfolightOption($(grid)).parent;
                if (parent != undefined && !autoApply) {
                    var Rows = $(grid).datagrid("getRows");
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
                    var remoteName = getInfolightOption($(grid)).remoteName;
                    var tableName = getInfolightOption($(grid)).tableName;
                    $.ajax({
                        type: "POST",
                        url: getDataUrl(),
                        data: getRemoteParam({ mode: 'seq', numDig: numDig, startValue: startValue, step: step, field: field, queryWord: queryWord }, remoteName, tableName, true),
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
}
//----------------------------------------------------------------------------------------------------------------

//validate method
$.extend($.fn.validatebox.defaults.rules, {
    remote: {
        validator: function (value, param) {
            var val = '';
            $.ajax({
                type: "POST",
                url: window.currentUrl,
                data: "mode=validate&method=" + param[0] + "&value=" + value,
                cache: false,
                async: false,
                success: function (data) {
                    val = data;
                }, error: function () {
                    val = "";
                }
            });
            if (val.toLowerCase() == 'true') {
                return true;
            }
            else if (val.toLowerCase() == 'false') {
                return false;
            }
            else {
                param[1] = val;
                return false;
            }

        },
        message: "{1}"
    },
    client: {
        validator: function (value, param) {
            var val = eval(param[0]).call(this, value);
            return val.toString() == 'true';
        },
        message: "{1}"
    },
    greater: {
        validator: function (value, param) {
            if (!isNaN(parseFloat(value)) && !isNaN(parseFloat(param[0]))) {
                return parseFloat(value) >= parseFloat(param[0]);
            }
            return value >= param[0];
        },
        message: "value should be greater than {0}"
    },
    less: {
        validator: function (value, param) {
            if (!isNaN(parseFloat(value)) && !isNaN(parseFloat(param[0]))) {
                return parseFloat(value) <= parseFloat(param[0]);
            }
            return value <= param[0];
        },
        message: "value should be less than {0}"
    },
    range: {
        validator: function (value, param) {
            if (!isNaN(parseFloat(value)) && !isNaN(parseFloat(param[0])) && !isNaN(parseFloat(param[1]))) {
                return parseFloat(value) >= parseFloat(param[0]) && parseFloat(value) <= parseFloat(param[1]);
            }
            if (param[0] != '' && value < param[0]) {
                return false;
            }
            return param[1] == '' || value <= param[1];
        },
        message: "value should be between {0} and {1}"
    },
    datetime: {
        validator: function (value, param) {
            var dateString = value.toString().replace(/\./g, '-');
            dateString = dateString.replace(/\//g, '-');
            var date = $.fn.datebox('parseDate', dateString);
            return date != "Invalid Date";
        },
        message: "input value is not datetime."
    },
    rocDatetime: {
        validator: function (value, param) {
            if (!value) return true;
            var ss = value.split('.');
            var y = parseInt(ss[0], 10);
            var m = parseInt(ss[1], 10);
            var d = parseInt(ss[2], 10);
            if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                var date = new Date(y + 1911, m - 1, d);
                return date != "Invalid Date";
            } else {
                return false;
            }
        },
        message: "input value is not datetime."
    },
    checkData: {
        validator: function (value, param) {
            if (value == '' || value == undefined) {
                return true;
            }
            var remoteName = param[0];
            var tableName = param[1];
            var valueField = param[2];
            //var textField = param[3];

            var isNvarChar = false;
            var refval = $(this).closest('span.refval').prev('.info-refval');
            if (refval) {
                var options = getInfolightOption(refval);
                if (options && options.columns) {
                    for (var i = 0; i < options.columns.length; i++) {
                        if (options.columns[i].field == valueField) {
                            if (options.columns[i].table) {
                                valueField = options.columns[i].table + '.' + valueField;
                            }
                            isNvarChar = options.columns[i].isNvarChar;
                        }
                    }
                }
            }

            var returnvalue = true;
            var queryWord = {whereString: valueField + " = " + formatQueryValue(value.toString(), "string", isNvarChar)};
            if (param.length >= 4) {
                var whereItem = param[3];
                if (whereItem != undefined && whereItem != '') {
                    queryWord.whereString += " and " + whereItem;
                }
            }
            $.ajax({
                type: "POST",
                url: getDataUrl(),
                data: getRemoteParam({queryWord: $.toJSONString(queryWord)}, remoteName, tableName, false),
                cache: false,
                async: false,
                success: function (data) {
                    if (eval(data).length == 0) {
                        returnvalue = false;
                    }
                    else {
                        var row = eval(data)[0];
                        returnvalue = row[param[2]] == value.toString();
                    }
                },
                error: function (data) {
                    data.responseText = '';
                    returnvalue = false;
                }
            });

            return returnvalue;
        },
        message: "input data is not valid."
    },
    idCard: {
        validator: function (value, param) {
            if (value == '' || value == undefined) {
                return true;
            }
            return IDCheck(value);
        },
        message: "input idCard is not valid."
    },
    url: {
        validator: function (value, param) {
            if (value == '' || value == undefined) {
                return true;
            }
            return CheckURLFormat(value);
        },
        message: "input url is not valid."
    },
    email: {
        validator: function (value, param) {
            if (value == '' || value == undefined) {
                return true;
            }

            return CheckEmailFormat(value);
        },
        message: "input email is not valid."
    }
});

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


//----------------------------------------------------------------------------------------------------------------

function getRootPath() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);

    var applicationPath = $('#_APPLICATIONPATH');
    return applicationPath.length ? prePath + applicationPath.val() : (prePath + postPath);
}
var i = 0;
//exception method
$(document).ajaxError(function (event, jqXHR) {
    var errorMessage = getErrorMessage(jqXHR.responseText);
    if (errorMessage == 'Timeout, relogon please' || errorMessage.indexOf('is logoned in other place.') > 0) {
        //        if (window.location.toString().indexOf("MainPage_Flow.aspx") > 0) {
        //            window.parent.location.href = "logon.aspx";
        //        }
        //        else {
        //            window.parent.location.href = "../logon.aspx";
        //        }
        window.parent.location.href = getRootPath() + "/timeout.aspx";
    }
    else if (errorMessage != '') {
        var iiscode = getClientInfo('errorCode');
        errorMessage = errorMessage + "<br/>" + "Error Code is :" + $.fn.Error.errorCode + iiscode;

        //$.messager.alert('Error', errorMessage, 'error');
        var errorWindow = $("#errorWindow", "body");
        if (errorWindow.length == 0) {
            $("body").append('<div id="errorWindow"/>');
            errorWindow = $("#errorWindow", "body");
        }
        else {
            if (errorWindow.window('options').closed != true) {
                return; //防止重复弹窗
            }
        }
        var errorStack = getErrorStack(jqXHR.responseText, errorMessage);
        errorWindow.window({
            title: 'Error',
            iconCls: 'icon-no',
            collapsible: false,
            minimizable: false,
            maximizable: false,
            resizable: false,
            href: getRootPath() + '/InnerPages/Error.aspx',
            width: 500,
            height: 380,
            modal: true,
            onLoad: function () {
                $(".errormessage", this).html(errorMessage);
                $(".errorstackmessage", this).html(errorStack);
            }
        });
        errorWindow.window('open');
    }
});

function getErrorMessage(html) {
    if (html == '' || html == null) {
        return '';
    }
    var startIndex = html.indexOf('<title>');
    var endIndex = html.indexOf('</title>');
    if (startIndex > 0 && endIndex > startIndex + 7) {
        return html.substring(startIndex + 7, endIndex);
    }
    else {
        return html;
    }
}
function getErrorStack(html, message) {
    message = message + '\r\n';
    var startIndex = html.indexOf(message);
    if (startIndex == -1) {
        startIndex = html.toLowerCase().lastIndexOf('server stack trace:');
        message = 'server stack trace:';
    }
    var statck = html.substring(startIndex + message.length + 2);
    return statck.replace(/\r\n/g, '<br/>');
}
function showStack() {
    var errStack = $(".errorstack");
    errStack.toggle();
    if (errStack.is(":hidden")) {
        $(".errorstack-button").linkbutton({text: 'ShowStack'});
        //$(".errorstack-button").html('ShowStack');
    }
    else {
        $(".errorstack-button").linkbutton({text: 'HideStack'});
        //$(".errorstack-button").html("HideStack");
    }
}
function feedBack() {
    var error = {
        message: $(".errormessage").html().replace(/<br>/g, '\r\n').replace(/</g, "&lt;").replace(/>/g, "&gt;"),
        description: $(".errordescription").val()
    };
    var stack = $(".errorstackmessage").html().replace(/<br>/g, '\r\n').replace(/</g, "&lt;").replace(/>/g, "&gt;");
    $.fn.Error.errorCode = 1220;
    $.ajax({
        type: "POST",
        url: getParentFolder() + "../handler/jqDataHandle.ashx",
        data: {
            mode: 'feedback',
            error: $.toJSONString(error),
            stack: stack,
            source: (window.location.href != undefined && window.location.href != "") ? window.location.href.substr(window.location.href.lastIndexOf("/") + 1, window.location.href.length - window.location.href.lastIndexOf("/") - 2) : ""
        },
        //data: "mode=feedback&error=" + $.toJSONString(error) + "&stack=" + stack,
        cache: false,
        async: true
    });

    $('#errorWindow').window('close');
}
function userDefineLog(title, description, status) {
    $.fn.Error.errorCode = 1310;
    $.ajax({
        type: "POST",
        url: getParentFolder() + "../handler/SystemHandle.ashx?Type=UserDefineLog&Title=" + title + "&Description=" + description + "&Status=" + status,
        cache: false,
        async: true
    });
}

//----------------------------------------------------------------------------------------------------------------

//expand row
function saveItem(grid, index) {
    var grid = $(grid);
    var divname = '#ddv' + grid.attr('id') + index;
    var form = $(divname);
    //var disapply = false;
    var onApply = getInfolightOption(grid).onApply;
    if (onApply != undefined && (ignoreOnApply == undefined || ignoreOnApply == false)) {
        var flag = onApply.call(form);
        if (flag != undefined && flag.toString() == 'false') {
            return;
        }
    }
    var containform = form;
    var formid = getInfolightOption($(form.find('.expandContent')[0])).containForm;
    if (form.find(formid).length > 0) {
        containform = $(form.find(formid)[0]);
    }
    if (containform.form('validateForm')) {
        var changedDatas = [];
        var changedRows = {tableName: getInfolightOption(grid).tableName, inserted: [], deleted: [], updated: []};
        var autoApply = getInfolightOption(grid).autoApply;
        var row = getFormRow(form);
        var encodeRow = {};
        for (var p in row) {
            var value = row[p];
            //if (value) {
            //    encodeRow[p] = encodeURIComponent(value);
            //    encodeRow[p] = encodeRow[p].toString().replace(/(%22)/g, "\"").replace(/(%5C)/g, "\\");
            //}
            //else {
                encodeRow[p] = value;
            //}
        }
        if (autoApply != undefined && !autoApply) {
            grid.datagrid('beginEdit', index);
            $('input,select,textarea', divname).each(function () {
                var field = getInfolightOption($(this)).field;
                var formid = getInfolightOption($(this)).form;
                if (formid != undefined && field != undefined && form.attr('id') == formid) {
                    //$(dialoggrid).datagrid('getEditor', { index: index, field: field }).target.val(row[field]);
                    var editor = grid.datagrid('getEditor', {index: index, field: field});
                    if (editor) {
                        editor.actions.setValue(editor.target, row[field]);
                    }
                    else {
                        alert("Datagrid:" + grid.attr('id') + " 找不到欄位:" + field);
                    }
                }
            });
            grid.datagrid('endEdit', index);
            grid.datagrid('changeState', 'editing');
            grid.datagrid('collapseRow', index);
            //$(dialoggrid).datagrid('updateRow', { index: index, row: row });
        }
        else {
            var mode = getEditMode(form);
            if (mode == "inserted") {
                changedRows.inserted.push(encodeRow);
            }
            else
                changedRows.updated.push(encodeRow);

            changedDatas.push(changedRows);

            var remoteName = getInfolightOption(grid).remoteName;
            var tableName = getInfolightOption(grid).tableName;
            var duplicateCheckS = true;
            if (getInfolightOption(containform).duplicateCheck && mode == "inserted") {
                $.fn.Error.errorCode = 1051;
                $.ajax({
                    type: "POST",
                    url: getDataUrl(),
                    data: getRemoteParam({
                        data: $.toJSONString(changedDatas),
                        mode: 'duplicate'
                    }, remoteName, tableName, false),
                    cache: false,
                    async: false,
                    success: function (a) {
                        if (a == "false") {
                            alertMessage("duplicatecheckmsg");
                            duplicateCheckS = false;
                        }
                    },
                    error: function () {
                        duplicateCheckS = false;
                    }
                });
            }

            if (duplicateCheckS) {
                $('.infosysbutton-s', form).each(function () {
                    $(this).linkbutton('disable');
                });
                $.fn.Error.errorCode = 1052;

                $.ajax({
                    type: "POST",
                    url: getDataUrl(),
                    data: getRemoteParam({
                        data: $.toJSONString(changedDatas),
                        mode: 'update'
                    }, remoteName, tableName, false),
                    cache: false,
                    async: true,
                    success: function (data) {
                        var onApplied = getInfolightOption(grid).onApplied;
                        if (onApplied) {
                            var rows = $.parseJSON(data);
                            onApplied.call(form, rows);
                        }
                        grid.datagrid('collapseRow', index);
                        //grid.datagrid('reload');
                        if (mode == "inserted") {
                            grid.data('lastInsertedRow', row);
                            grid.datagrid('reload');
                        }
                        else if (index != -1) {
                            grid.datagrid('updateRow', {index: index, row: row});
                        }

                        if (grid.datagrid('options').showFooter == true && grid.datagrid('options').pagination == false) {
                            setFooter(grid);
                        }
                    },
                    complete: function () {
                        $(document).ready(function () {
                            $.messager.progress('close');
                        });
                        $('.infosysbutton-s', form).each(function () {
                            $(this).linkbutton('enable');
                        });
                    }
                });
            }
        }
        grid.datagrid('fixRowHeight');
    }
}
function cancelItem(dgid, index) {
    $(dgid).datagrid('collapseRow', index);
    var autoApply = getInfolightOption($(dgid)).autoApply;
    if (autoApply) {
        $(dgid).datagrid('rejectChanges');
    }
}
//----------------------------------------------------------------------------------------------------------------

//refbutton
//function refButtonClick(button) {
//    var span = $(button).parent();
//    var editDialog = getInfolightOption(span).editDialog;
//    var dialog = $(editDialog);
//    var dataGrid = $(dataGrid_class, dialog);

//    dataGrid.datagrid({
//        pagination: true,
//        collapsible: false,
//        rownumbers: false,
//        singleSelect: true,
//        onClickRow: function (rowindex, rowdata) {
//            var valueField = getInfolightOption(span).valueField;
//            var value = rowdata[valueField];
//            var input = $('input', span);
//            input.val(value);

//            var formid = getInfolightOption($(this)).form;
//            if (formid != undefined) {
//                var columnMatchs = getInfolightOption(span).columnMatches;
//                if (columnMatchs != undefined) {
//                    var defaultmethods = new Object();
//                    var hasRemote = false;
//                    var matches = columnMatchs.split(';');
//                    for (var i = 0; i < matches.length; i++) {
//                        var match = matches[i].replace(/(^\s*)|(\s*$)/g, "");
//                        var columns = match.split('=');
//                        if (columns.length == 2) {
//                            var targetColumn = columns[0].replace(/(^\s*)|(\s*$)/g, "");
//                            var targetInput = $("[name='" + targetColumn + "']", "#" + formid);

//                            var value = columns[1].replace(/(^\s*)|(\s*$)/g, "");
//                            if (defaultValue.indexOf("client[") == 0) {
//                                var methodName = defaultValue.replace("client[", "").replace("]", "");
//                                targetInput.val(eval(methodName).call());
//                            }
//                            else if (defaultValue.indexOf("remote[") == 0) {
//                                var methodName = defaultValue.replace("remote[", "").replace("]", "");
//                                defaultmethods[field] = methodName;
//                                hasRemote = true;
//                            }
//                            else {
//                                targetInput.val(rowdata[value]);
//                            }
//                        }
//                    }
//                    if (hasRemote) {
//                        var defaultObjs = getDefault($.toJSONString(defaultmethods));
//                        var defaultObj = $.parseJSON(defaultObjs);
//                        for (var property in defaultObj) {
//                            var targetInput = $("[name='" + property + "']", "#" + formid);
//                            targetInput.val(defaultObj[property]);
//                        }
//                    }
//                }
//            }

//            $(dialog).dialog('close');
//        }
//    });
//    dataGrid.datagrid('load');
//    $(dialog).dialog('open');

//};

//function refQuery() {
//    var span = $(this).parent;
//    var dataGrid = $(dataGrid_class, span);
//    var queryParams = dataGrid.datagrid('options').queryParams;
//    var queryWord = new Object();
//    var where = '';
//    $(":text", span).each(function () {
//        var text = $(this);
//        var value = text.val();
//        if (value != '') {
//            var fieldName = getInfolightOption(text).field;
//            if (fieldName != undefined) {
//                if (where != '')
//                    where += ' and ';
//                var condition = getInfolightOption(text).condition;
//                var dataType = getInfolightOption(text).dataType;
//                switch (condition) {
//                    case '=': where += fieldName + " = " + formatQueryValue(value, dataType); break;
//                    case '%': where += fieldName + " like '" + value + "%'"; break;
//                    case '%%': where += fieldName + " like '%" + value + "%'"; break;
//                    default:
//                }
//            }
//        }
//    });
//    queryWord.whereString = where;
//    queryParams.queryWord = $.toJSONString(queryWord);
//    dataGrid.datagrid('load');
//}

//function refbuttonclick(fid) {
//    var field = getInfolightOption($(fid)).field;
//    var refdialog = getInfolightOption($(fid)).refdialog;
//    if ($(refdialog) != undefined && $(refdialog).length == 1) {
//        var refdialogpanel = $(refdialog);
//        $(dataGrid_class, refdialogpanel).each(function () {
//            var datagrid = $(this);
//            var remoteName = getInfolightOption(datagrid).remoteName;
//            var tableName = getInfolightOption(datagrid).tableName;
//            datagrid.datagrid({
//                url: '../handler/jqDataHandle.ashx?RemoteName=' + remoteName + "&TableName=" + tableName + "&IncludeRows=true",
//                collapsible: true,
//                rownumbers: false,
//                singleSelect: true,
//                onClickRow: function (rowindex, rowdata) {
//                    var value = rowdata[field];
//                    $('input:first', $(fid.parentElement)).val(value);
//                    $(refdialogpanel).dialog('close');
//                }
//            });
//        });
//        $(refdialogpanel).dialog('open');
//    }
//};

//function refbuttonquery(dgid, pnid) {
//    var queryParams = $(dgid).datagrid('options').queryParams;
//    var queryWord = new Object();
//    var where = '';
//    $(":text", pnid).each(function () {
//        var text = $(this);
//        var value = text.val();
//        if (value != '') {
//            var fieldName = getInfolightOption(text).field;
//            if (fieldName != undefined) {
//                if (where != '')
//                    where += ' and ';
//                var condition = getInfolightOption(text).condition;
//                var dataType = getInfolightOption(text).dataType;
//                switch (condition) {
//                    case '=': where += fieldName + " = " + formatQueryValue(value, dataType); break;
//                    case '%': where += fieldName + " like '" + value + "%'"; break;
//                    case '%%': where += fieldName + " like '%" + value + "%'"; break;
//                    default:
//                }
//            }
//        }
//    });

//    queryWord.whereString = where;
//    queryParams.queryWord = $.toJSONString(queryWord);
//    $(dgid).datagrid('load');
//};

//----------------------------------------------------------------------------------------------------------------

//combogrid method
function combogridQuery(param) {
    var datagrid = $(this).combogrid('grid');
    datagrid.datagrid('clearSelections');
    $(this).combogrid('textbox').val(param);
    var queryParams = datagrid.datagrid('options').queryParams;
    var where = $(this).combogrid('getWhere');
    var queryWord = {whereString: ''};
    if (param != '') {
        var textField = $(this).combogrid('options').textField;
        queryWord.whereString += textField + " like '" + param.toString().replace(/\'/g, "''") + "%'";
        var idField = $(this).combogrid('options').idField;
        queryWord.whereString += " or " + idField + " like '" + param.toString().replace(/\'/g, "''") + "%'";
    }
    if (where != undefined && where != '') {
        if (param != '') {
            queryWord.whereString += "and (" + where + ")";
        }
        else {
            queryWord.whereString += where;
        }
    }
    queryParams.queryWord = $.toJSONString(queryWord);
    datagrid.datagrid({
        onLoadSuccess: function (data) {
        }
    });
    datagrid.datagrid('load');
    //queryParams.queryWord = '';
}
$.extend($.fn.combogrid.methods, {
    getWhere: function (jq) {
        return $(jq[0]).attr('whereString');
    },

    setWhere: function (jq, where) {
        jq.each(function () {
            var datagrid = $(this).combogrid('grid');
            $(this).attr('whereString', where);
            datagrid.datagrid('setWhere', where);
            //datagrid.data('testc', 'c');
            //datagrid.datagrid('options').queryParams.queryWord = '';
        });
    }
});
//-----------------------------------------------------------------------------------------------------------------
//refval method
$.fn.setCursorPosition = function (position) {
    if (this.lengh == 0) return this;
    return $(this).setSelection(position, position);
};

$.fn.setSelection = function (selectionStart, selectionEnd) {
    if (this.lengh == 0) return this;
    input = this[0];

    if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    } else if (input.setSelectionRange) {
        input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    }

    return this;
};

$.fn.focusEnd = function () {
    this.setCursorPosition(this.val().length);
};

function setFirstValue(infoRefval, where) {
    var remoteName = getInfolightOption(infoRefval).remoteName;
    var tableName = getInfolightOption(infoRefval).tableName;
    var valueField = getInfolightOption(infoRefval).valueField;
    $.ajax({
        type: "POST",
        dataType: 'json',
        url: getDataUrl(),
        data: getRemoteParam({rows: 1, whereString: where}, remoteName, tableName, false),
        cache: false,
        async: false,
        success: function (data) {
            if (data.length > 0) {
                var value = data[0][valueField];
                $(infoRefval).refval('setValue', value);
            }
        },
        error: function (data) {
        }
    });
}


$.fn.refval = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.refval.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
};

$.fn.refval.methods = {
    enable: function (jq) {
        $(jq).data("inforefval").refval.find("span.icon-view").attr('disabled', false);
        var selectOnly = getInfolightOption($(jq)).selectOnly;
        if (!selectOnly) {
            $(jq).data("inforefval").refval.find("input.refval-text").attr('disabled', false);
        }
    },
    disable: function (jq) {
        $(jq).data("inforefval").refval.find("input.refval-text").attr('disabled', true);
        $(jq).data("inforefval").refval.find("span.icon-view").attr('disabled', true);
    },
    getValue: function (jq) {
        if (jq.length > 0) {
            return $(jq[0]).refval('selectItem').value;
        }
        else {
            return null;
        }
    },

    setValue: function (jq, value) {
        jq.each(function () {
            if (value == '' || value == undefined) {
                $(this).refval('selectItem', {value: '', text: ''});
            }
            else {
                var options = getInfolightOption($(this));
                if (options.textField == options.valueField) {
                    $(this).refval('selectItem', {value: value, text: value});
                }
                else {
                    var textValues = $(this).data("textValues");
                    if (options.cacheRelationText == true && textValues != undefined && textValues[value] != undefined) {
                        $(this).refval('selectItem', {value: value, text: textValues[value]});
                    }
                    else {
                        var whereItem = $(this).refval('getWhereItem');
                        var text = getDisplayValue(value, options, whereItem);
                        if (options.cacheRelationText == true) {
                            if (textValues == undefined) {
                                $(this).data("textValues", {});
                                textValues = $(this).data("textValues");
                            }
                            textValues[value] = text;
                        }
                        $(this).refval('selectItem', {value: value, text: text});
                    }
                }
            }
            if ($.data(this, "inforefval").refval.find("input.refval-text").is(':focus')) {
                $(this).refval('showValue');
            }
            else {
                $(this).refval('showText');
            }
        });
    },
    selectItem: function (jq, item) {
        if (item == undefined) {
            if (jq.length > 0) {
                var item = $(jq[0]).data("selectItem");
                if (item == null) {
                    return {value: '', text: ''};
                }
                else {
                    return item;
                }
            }
            else {
                return null;
            }
        }
        else {
            jq.each(function () {
                $(this).data("selectItem", item);
            })
        }
    },
    showText: function (jq) {
        jq.each(function () {
            var options = getInfolightOption($(this));
            var text = $(this).refval('selectItem').text;
            var value = $(this).refval('selectItem').value;
            if (options.showValueAndText != undefined && options.showValueAndText == true) {
                if (value == "" && text == "")
                    $.data(this, "inforefval").refval.find("input.refval-text").val(text);
                else
                    $.data(this, "inforefval").refval.find("input.refval-text").val(value + ":" + text);
            }
            else {
                $.data(this, "inforefval").refval.find("input.refval-text").val(text);
            }
            if ($.data(this, "inforefval").refval.find("input.refval-text").hasClass("validatebox-text")) {  //修复有时选回值时不会刷新validate不过的消息
                $.data(this, "inforefval").refval.find("input.refval-text").validatebox('validate');
            }

        });


    },
    showValue: function (jq) {
        jq.each(function () {
            var value = $(this).refval('selectItem').value;
            $.data(this, "inforefval").refval.find("input.refval-text").val(value);
            $.data(this, "inforefval").refval.find("input.refval-text").setCursorPosition(value.length);
        });
    },
    getWhere: function (jq) {
        return $(jq[0]).attr('whereString');
    },
    setWhere: function (jq, where) {
        jq.each(function () {
            var datagrid = $.data(this, "inforefval").panel.find('table.refval-grid');
            $(this).attr('whereString', where);
            datagrid.datagrid('setWhere', where);
        });
    },
    getWhereItem: function (jq, op) {
        if (jq.length > 0) {
            var options = op ? op : getInfolightOption($(jq[0]));

            if (options.whereItems != undefined) {
                var whereObject = {};
                var wheremethods = {};
                var hasRemote = false;
                for (var i = 0; i < options.whereItems.length; i++) {
                    var field = options.whereItems[i].field;
                    var value = options.whereItems[i].value;
                    if (value.indexOf("row[") == 0) {
                        var wherefield = value.replace("row[", "").replace("]", "");


                        var formid = options.form;
                        var value = "";
                        if (formid != undefined) {
                            $('input,select,textarea', "#" + formid).each(function () {
                                var field = getInfolightOption($(this)).field;
                                if (field == wherefield) {
                                    var controlClass = $(this).attr('class');
                                    if (controlClass != undefined) {
                                        if (controlClass.indexOf('easyui-datebox') == 0) {
                                            value = $(this).datebox('getBindingValue');
                                        }
                                        else if (controlClass.indexOf('easyui-combobox') == 0) {
                                            value = $(this).combobox('getValue');
                                        }
                                        else if (controlClass.indexOf('easyui-datetimebox') == 0) {
                                            value = $(this).datetimebox('getBindingValue');
                                        }
                                        else if (controlClass.indexOf('easyui-combogrid') == 0) {
                                            value = $(this).combogrid('getValue');
                                        }
                                        else if (controlClass.indexOf('info-combobox') == 0) {
                                            value = $(this).combobox('getValue');
                                        }
                                        else if (controlClass.indexOf('info-combogrid') == 0) {
                                            value = $(this).combogrid('getValue');
                                        }
                                        else if (controlClass.indexOf('info-refval') == 0) {
                                            value = $(this).refval('getValue');
                                        }
                                        else {
                                            value = $(this).val();
                                        }
                                    }
                                    else {
                                        value = $(this).val();
                                    }
                                    return false;
                                }
                            });
                        }
                        else if (options.row) {
                            value = options.row[wherefield];
                        }
                        whereObject[field] = value;
                    }
                    else if (value.indexOf("client[") == 0) {
                        var methodName = value.replace("client[", "").replace("]", "");
                        whereObject[field] = eval(methodName).call();
                    }
                    else if (value.indexOf("remote[") == 0) {
                        wheremethods[field] = value.replace("remote[", "").replace("]", "");
                        hasRemote = true;
                    }
                    else {
                        whereObject[field] = value;
                    }

                }
                if (hasRemote) {
                    $.fn.Error.errorCode = 1802;

                    var whereObjs = getDefault($.toJSONString(wheremethods));
                    var whereObj = $.parseJSON(whereObjs);
                    for (var property in whereObj) {
                        whereObject[property] = whereObj[property];
                    }
                }
                //var where = '';

                var whereList = [];

                for (var property in whereObject) {
                    //where += property + " = '" + whereObject[property] + "'";
                    //where += " and ";
                    whereList.push(property + " = '" + whereObject[property] + "'");
                }
                var whereString = $(jq[0]).attr('whereString');
                if (whereString) {
                    whereList.push('(' + whereString + ')');
                }
                return whereList.join(' and ');
                //where = where.substring(0, where.lastIndexOf(' and '));
                //return where;
            }
        }
    },
    doColumnMatch: function (jq, rowdata) {
        jq.each(function () {
            if (rowdata == null) {
                return false;
            }
            var columnMatches = getInfolightOption($(this)).columnMatches;
            if (columnMatches.length > 0) {
                var formid = getInfolightOption($(this)).form;
                if (formid != undefined) {
                    var match = {};
                    var methods = {};
                    var hasRemote = false;
                    for (var i = 0; i < columnMatches.length; i++) {
                        if (rowdata) {
                            var value = columnMatches[i].value;
                            if (value.indexOf("client[") == 0) {
                                var methodName = value.replace("client[", "").replace("]", "");
                                match[columnMatches[i].field] = eval(methodName).call();
                            }
                            else if (value.indexOf("remote[") == 0) {
                                methods[columnMatches[i].field] = value.replace("remote[", "").replace("]", "");
                                hasRemote = true;
                            }
                            else {
                                match[columnMatches[i].field] = rowdata[value];
                                if (rowdata[value] == null || rowdata[value] == undefined) {
                                    match[columnMatches[i].field] = "";
                                }
                            }
                        }
                        else {
                            match[columnMatches[i].field] = "";
                        }
                    }
                    if (hasRemote) {
                        var Objs = getDefault($.toJSONString(methods));
                        var Obj = $.parseJSON(Objs);
                        for (var property in Obj) {
                            match[property] = Obj[property];
                        }
                    }
                    $('#' + formid).form('updateRow', match);
                }
            }
        });
    }
};
//-----------------------------------------------------------------------------------------------------------------
//checkbox method
$.fn.checkbox = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.checkbox.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
};

$.fn.checkbox.methods = {
    options: function (jq) {
        var option = {};
        option.on = true;
        option.off = false;
        if ($(jq[0]).attr("data-options") != undefined) {
            var ops = $(jq[0]).attr("data-options").split(',');
            for (var i = 0; i < ops.length; i++) {
                var index = ops[i].indexOf(':');
                if (index > 0) {
                    var pname = ops[i].substr(0, index).replace(/(^\s*)|(\s*$)/g, "");
                    var pvalue = ops[i].substr(index + 1);
                    option[pname] = eval(pvalue);
                }
            }
        }
        if ($(jq[0]).attr("infolight-options") != undefined) {
            var ops = $(jq[0]).attr("infolight-options").split(',');
            for (var i = 0; i < ops.length; i++) {
                var index = ops[i].indexOf(':');
                if (index > 0) {
                    var pname = ops[i].substr(0, index).replace(/(^\s*)|(\s*$)/g, "");
                    var pvalue = ops[i].substr(index + 1);
                    option[pname] = eval(pvalue);
                }
            }
        }

        return option;
    },

    getValue: function (jq) {
        if ($(jq[0]).attr("checked")) {
            return $(jq[0]).checkbox("options").on;
        }
        else {
            return $(jq[0]).checkbox("options").off;
        }
    },

    setValue: function (jq, value) {
        jq.each(function () {
            if (value != undefined) {
                if (value.toString().toLowerCase() == 'true' || value.toString().toLowerCase() == 'y' || value.toString() == '1') {
                    $(this).attr("checked", true);
                }
                else {
                    $(this).removeAttr("checked");
                }
            }
            else {
                $(this).removeAttr("checked");
            }
        });
    }
};
//-----------------------------------------------------------------------------------------------------------------
//options method
$.fn.options = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.options.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
};

$.fn.options.methods = {
    setValue: function (jq, value) {
        jq.each(function () {
            var openDialog = getInfolightOption($(this)).openDialog;
            $(this).data("infooptions").text.val(value);
            if (openDialog) {
                var selectOnly = getInfolightOption($(this)).selectOnly;
                if (selectOnly) {
                    $(this).data('value', value);
                    $(this).data("infooptions").text.val(getDisplayValues(value, getInfolightOption($(this))));
                }
            }
            else {
                var optionspanel = $(this).data("infooptions").panel;
                var optionstext = $(this).data("infooptions").text;
                var selectedValues = optionstext.val().toString().split(",");
                for (var i = 0; i < selectedValues.length; i++) {
                    $(":radio,:checkbox", optionspanel).each(function () {
                        if ($(this).val() == selectedValues[i]) {
                            $(this).prop("checked", true);
                        }
                    });
                }
            }
        });
    },
    getValue: function (jq) {
        var openDialog = getInfolightOption($(jq[0])).openDialog;
        if (openDialog) {
            var selectOnly = getInfolightOption($(jq[0])).selectOnly;
            if (selectOnly) {
                return $(jq[0]).data('value');
            }
            else {
                return $(jq[0]).data("infooptions").text.val();
            }
        }
        else {
            return $(jq[0]).options('getCheckedValue');
        }
    },
    initializePanel: function (jq) {
        jq.each(function () {
            var target = $(this);
            var options = getInfolightOption(target);
            var valueField = options.valueField;
            var textField = options.textField;
            var remoteName = options.remoteName;
            var tableName = options.tableName;
            var panelWidth = options.panelWidth;
            var onSelect = options.onSelect;
            var onWhere = options.onWhere;
            var columnCount = options.columnCount;
            var multiSelect = options.multiSelect;
            //var title = options.title;
            var items = options.items;
            var openDialog = options.openDialog;
            var optionspanel = $(target).data("infooptions").panel;
            var optionstext = $(target).data("infooptions").text;
            if (items != undefined && items.length > 0) {
                $("table", optionspanel).remove();
                var table = $("<table width='" + (panelWidth - 35) + "'></table>").appendTo(optionspanel);
                var datarow = '';
                for (var i = 0; i < items.length; i++) {
                    if (i % columnCount == 0) {
                        if (items.length > 0) {
                            $("<tr>" + datarow + "</tr>").appendTo(table);
                            datarow = '';
                        }
                    }
                    //var id = $(target).attr('id') + "_" + i;
                    var name = $(target).attr('id') + "_" + 0;
                    var type = "radio";
                    if (multiSelect) {
                        name = $(target).attr('id') + "_" + i;
                        type = "checkbox";
                    }
                    var value = items[i].value;
                    var text = items[i].text;
                    datarow += "<td><input name='" + name + "' type='" + type + "' value='" + value + "' >" + text + "</input></td>";
                }

                if (datarow.length > 0) {
                    $("<tr>" + datarow + "</tr>").appendTo(table);
                }
                var selectedValues = optionstext.val().toString().split(",");
                for (var i = 0; i < selectedValues.length; i++) {
                    $(":radio,:checkbox", optionspanel).each(function () {
                        if ($(this).val() == selectedValues[i]) {
                            $(this).prop("checked", true);
                        }
                    });
                }
                if (!openDialog) {
                    $(":radio,:checkbox", optionspanel).click(function () {
                        if (options.onSelect) {
                            onSelect.call(target, $(target).options('getCheckedValue'));
                        }
                    });
                }
            }
            else {
                var queryWord = {};
                if (onWhere != undefined) {
                    queryWord.whereString = onWhere.call(target);
                }


                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: getDataUrl(),
                    data: getRemoteParam({queryWord: $.toJSONString(queryWord)}, remoteName, tableName, true, -1),
                    cache: false,
                    async: false,
                    success: function (data) {
                        $("table", optionspanel).remove();
                        var rows = data.rows;
                        var table = $("<table width='" + (panelWidth - 35) + "'></table>").appendTo(optionspanel);
                        var datarow = '';
                        for (var i = 0; i < rows.length; i++) {
                            if (i % columnCount == 0) {
                                if (datarow.length > 0) {
                                    $("<tr>" + datarow + "</tr>").appendTo(table);
                                    datarow = '';
                                }
                            }
                            //var id = $(target).attr('id') + "_" + i;
                            var name = $(target).attr('id') + "_" + 0;
                            var type = "radio";
                            if (multiSelect) {
                                name = $(target).attr('id') + "_" + i;
                                type = "checkbox";
                            }
                            var value = rows[i][valueField];
                            var text = rows[i][textField];
                            datarow += "<td><input name='" + name + "' type='" + type + "' value='" + value + "' >" + text + "</input></td>";
                        }

                        if (datarow.length > 0) {
                            $("<tr>" + datarow + "</tr>").appendTo(table);
                        }

                        if ($(target).options('getValue')) {
                            var selectedValues = $(target).options('getValue').toString().split(",");
                            for (var i = 0; i < selectedValues.length; i++) {
                                $(":radio,:checkbox", optionspanel).each(function () {
                                    if ($(this).val() == selectedValues[i]) {
                                        $(this).prop("checked", true);
                                    }
                                });
                            }
                        }
                        if (!openDialog) {
                            $(":radio,:checkbox", optionspanel).click(function () {
                                if (options.onSelect) {
                                    onSelect.call(target, $(target).options('getCheckedValue'));
                                }
                            });
                        }

                    }, error: function (data) {

                    }
                });
            }
        });
    },
    getCheckedValue: function (jq) {
        var optionspanel = $(jq[0]).data("infooptions").panel;
        var selectedValues = [];
        $(":checked", optionspanel).each(function () {
            selectedValues.push($(this).val());
        });
        return selectedValues.join();
    },
    selectAll: function (jq) {
        var optionspanel = $(jq[0]).data("infooptions").panel;
        $(":radio,:checkbox", optionspanel).each(function () {
            $(this).prop("checked", true);
        });
    },
    unSelectAll: function (jq) {
        var optionspanel = $(jq[0]).data("infooptions").panel;
        $(":radio,:checkbox", optionspanel).each(function () {
            $(this).prop("checked", false);
        });
    },
    enable: function (jq) {
        $(jq[0]).data("infooptions").button.attr('disabled', false);
        var selectOnly = getInfolightOption($(jq[0])).selectOnly;
        if (!selectOnly) {
            $(jq[0]).data("infooptions").text.attr('disabled', false);
        }
        if (getInfolightOption($(jq[0])).openDialog == false) {
            $('.options-panel', $(jq[0]).next()).each(function () {
                $("input", this).each(function () {
                    this.disabled = false;
                });
            });
        }
    },
    disable: function (jq) {
        $(jq[0]).data("infooptions").text.attr('disabled', true);
        $(jq[0]).data("infooptions").button.attr('disabled', true);
        if (getInfolightOption($(jq[0])).openDialog == false) {
            $('.options-panel', $(jq[0]).next()).each(function () {
                $("input", this).each(function () {
                    this.disabled = "disabled";
                });
            });
        }
    }
};

//-----------------------------------------------------------------------------------------------------------------
//flcomment method

$.fn.flcomment = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.flcomment.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
};

$.fn.flcomment.defaults = {
    stepidText: "S_STEP_ID",
    useridText: "USER_ID",
    usernameText: "USERNAME",
    statusText: "STATUS",
    remarkText: "REMARK",
    updatedateText: "UPDATE_DATE",
    updatetimeText: "UPDATE_TIME"
};
//-----------------------------------------------------------------------------------------------------------------

//yearmonthbox
$.fn.yearmonth = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.yearmonth.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
};

$.fn.yearmonth.methods = {
    enable: function (jq) {
        $(jq).combobox('enable');
    },
    disable: function (jq) {
        $(jq).combobox('disable');
    },
    getValue: function (jq) {
        if (jq.length > 0) {
            var options = getInfolightOption($(jq[0]));
            var type = options.type;
            //var durationMinus = options.durationMinus;
            //var durationPlus = options.durationPlus;
            var format = options.format;
            var datatype = options.datatype;

            var value = $(jq).combobox('getValue');
            if (value != undefined && value != "") {
                if (datatype == "datetime") {
                    if (type == "year") {
                        return value + "-01-01";
                    }
                    else if (type == "month") {
                        var month = value.slice(4);
                        if (month.length == 1) month = "0" + month;
                        return value.slice(0, 4) + "-" + month + "-01";
                    }
                }
                else {
                    if (type == "year") {
                        return value;
                    }
                    else if (type == "month") {
                        var month = value.slice(4);
                        var m = value.slice(4);
                        var year = value.slice(0, 4);
                        if (month.length == 1) {
                            month = '0' + month;
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
                        return format.replace(/yyyy/g, year).replace(/yy/g, yy).replace(/mm/g, month).replace(/m/g, m).replace(/YYY/g, YYY).replace(/YY/g, YY);
                    }
                }
            }
            else return value;
        }
        else {
            return null;
        }
    },

    setValue: function (jq, value) {
        jq.each(function () {
            var options = getInfolightOption($(jq[0]));
            var type = options.type;
            var datatype = options.datatype;
            if (datatype == "datetime") {
                if (value != undefined && value != "") {
                    var newvalue = new Date(value);
                    if (type == 'year') {
                        $(jq).combobox('setValue', newvalue.getFullYear().toString());
                    }
                    else if (type == "month") {
                        $(jq).combobox('setValue', newvalue.getFullYear().toString() + (newvalue.getMonth() + 1).toString());
                    }
                }
                else $(jq).combobox('setValue', value);
            }
            else
                $(jq).combobox('setValue', value);
        });
    }
};
//-----------------------------------------------------------------------------------------------------------------

//alert duplicate message
function alertMessage(type) {
    var localstring = $.sysmsg('getValue', 'JQWebClient/' + type);
    alert(localstring);
}
//-----------------------------------------------------------------------------------------------------------------

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
        if ($.sysmsg.messages[key]) {
        }
        else {
            var keys = [];
            keys.push(key);
            $.sysmsg('load', keys);
        }
        return $.sysmsg.messages[key];
    },
    getValues: function (keys) {
        $.sysmsg('load', keys);
    }
};

function setlanguage() {
    $.sysmsg('getValues', [
        'JQWebClient/dialogbuttontext'
        , 'JQWebClient/deletemessage'
        , 'JQWebClient/fileuploadbutton'
        , 'JQWebClient/greatervalidatemessage'
        , 'JQWebClient/lessvalidatemessage'
        , 'JQWebClient/rangevalidatemessage'
        , 'JQWebClient/checkdatavalidatemessage'
        , 'JQWebClient/parentemptymessage'
        , 'FLRuntime/InstanceManager/CommentTableHeader'
        , 'Srvtools/WebDropDownList/EmptyData'
    ]);


    var dialogbuttontext = $.sysmsg('getValue', 'JQWebClient/dialogbuttontext');
    var dialogbuttontexts = dialogbuttontext.split(',');
    var submitloacl = dialogbuttontexts[0];
    var cancellocal = dialogbuttontexts[6];
    var oklocal = dialogbuttontexts[2];
    var insertlocal = dialogbuttontexts[3];
    var uploadlocal = dialogbuttontexts[4];
    var deletelocal = dialogbuttontexts[5];
    var querylocal = dialogbuttontexts[7];
    var clearlocal = dialogbuttontexts[8];
    $('.infosysbutton-s').each(function () {
        $('.l-btn-text', this).each(function () {
            $(this).text(submitloacl);
        });
    });
    $('.infosysbutton-c').each(function () {
        $('.l-btn-text', this).each(function () {
            $(this).text(cancellocal);
        });
    });
    $('.infosysbutton-o').each(function () {
        $('.l-btn-text', this).each(function () {
            $(this).text(oklocal);
        });
    });
    $('.infosysbutton-q').each(function () {
        $('.l-btn-text', this).each(function () {
            $(this).text(querylocal);
        });
    });
    $('.infosysbutton-cl').each(function () {
        $('.l-btn-text', this).each(function () {
            $(this).text(clearlocal);
        });
    });
    $('.infosysbutton-i').each(function () {
        if ($(this).hasClass('menu-item')) {
            $(".menu-text", this).each(function () {
                $(this).text(insertlocal);
            });
        }
    });
    $('.infosysbutton-u').each(function () {
        if ($(this).hasClass('menu-item')) {
            $(".menu-text", this).each(function () {
                $(this).text(uploadlocal);
            });
        }
    });
    $('.infosysbutton-d').each(function () {
        if ($(this).hasClass('menu-item')) {
            $(".menu-text", this).each(function () {
                $(this).text(deletelocal);
            });
        }
    });
    deleteMessage = $.sysmsg('getValue', 'JQWebClient/deletemessage');
    uploadButtonText = $.sysmsg('getValue', 'JQWebClient/fileuploadbutton');
    if ($.fn.validatebox) {
        $.fn.validatebox.defaults.rules.greater.message = $.sysmsg('getValue', 'JQWebClient/greatervalidatemessage');
        $.fn.validatebox.defaults.rules.less.message = $.sysmsg('getValue', 'JQWebClient/lessvalidatemessage');
        $.fn.validatebox.defaults.rules.range.message = $.sysmsg('getValue', 'JQWebClient/rangevalidatemessage');
        $.fn.validatebox.defaults.rules.checkData.message = $.sysmsg('getValue', 'JQWebClient/checkdatavalidatemessage');
    }
    if ($.fn.datagrid) {
        $.fn.datagrid.defaults.parentEmptyMessage = $.sysmsg('getValue', 'JQWebClient/parentemptymessage');
    }
    if ($.fn.flcomment) {
        var flowCommentTableHeader = $.sysmsg('getValue', 'FLRuntime/InstanceManager/CommentTableHeader');
        var flowCommentTableHeaders = flowCommentTableHeader.split(',');
        $.fn.flcomment.defaults.stepidText = flowCommentTableHeaders[0];
        $.fn.flcomment.defaults.useridText = flowCommentTableHeaders[1];
        $.fn.flcomment.defaults.usernameText = flowCommentTableHeaders[2];
        $.fn.flcomment.defaults.statusText = flowCommentTableHeaders[3];
        $.fn.flcomment.defaults.remarkText = flowCommentTableHeaders[4];
        $.fn.flcomment.defaults.updatedateText = flowCommentTableHeaders[6];
        $.fn.flcomment.defaults.updatetimeText = flowCommentTableHeaders[7];

    }
    if ($.fn.combobox) {
        $.fn.combobox.defaults.emptyText = $.sysmsg('getValue', 'Srvtools/WebDropDownList/EmptyData');
    }

}

//-----------------------------------------------------------------------------------------------------------------

//Submit detail form to Grid
function submiDetailForm(fid, gid) {
    var grid = $(gid);
    var formname = getInfolightOption($(fid)).containForm;
    if ($(formname).form('validateForm')) {
        var editmode = getEditMode($(formname));
        if (editmode == 'inserted') {
            var row = {};
            $('input,select,textarea', fid).each(function () {
                var field = getInfolightOption($(this)).field;
                var formid = getInfolightOption($(this)).form;
                if (formid != undefined && field != undefined && $(formname).attr('id') == formid) {
                    row[field] = $(this).val();
                }
            });
            grid.datagrid('appendRow', row);
            var autoApply = getInfolightOption(grid).autoApply;
            if (autoApply) {
                $.fn.Error.errorCode = 1101;
                applyUpdates(grid);
            }
        }
        else if (editmode == 'updated') {
            var index = getSelectedIndex(grid);
            grid.datagrid('beginEdit', index);
            $('input,select,textarea', fid).each(function () {
                var field = getInfolightOption($(this)).field;
                var formid = getInfolightOption($(this)).form;
                if (formid != undefined && field != undefined && $(formname).attr('id') == formid) {
                    //grid.datagrid('getEditor', { index: index, field: field }).target.val($(this).val());
                    var editor = grid.datagrid('getEditor', {index: index, field: field});
                    if (editor) {
                        editor.actions.setValue(editor.target, $(this).val());
                    }
                    else {
                        alert("Datagrid:" + grid.attr('id') + " 找不到欄位:" + field);
                    }
                }
            });

            grid.datagrid('endEdit', index);
            var autoApply = getInfolightOption(grid).autoApply;
            if (autoApply) {
                $.fn.Error.errorCode = 1102;
                applyUpdates(grid);
            }
        }
        $(fid).window('close');
    }
}
//-----------------------------------------------------------------------------------------------------------------

//TreeView Code
function insertTreeNode(treeview) {
    $.fn.Error.errorCode = 1600;
    var editDialog = getInfolightOption($(treeview)).editDialog;
    $(":text", $(editDialog)).each(function () {
        $(this).val("");
        $(this).removeAttr('disabled');
    });
    $(treeview).attr('editMode', 'insert');

    $(editDialog).window('open');
}
function updateTreeNode(treeview) {
    var editDialog = getInfolightOption($(treeview)).editDialog;
    var idField = getInfolightOption($(treeview)).idField;
    //var textField = getInfolightOption($(treeview)).textField;
    //var parentField = getInfolightOption($(treeview)).parentField;
    $(treeview).attr('editMode', 'update');
    var node = $(treeview).tree('getSelected');
    var data = node.attributes;
    $(editDialog).form('load', data);
    $(":text", $(editDialog)).each(function () {
        if (getInfolightOption($(this)).field == idField) {
            $(this).attr('disabled', 'disabled');
            //$(this).val(node.id);
        }
        //        else if (getInfolightOption($(this)).field == textField) {
        //            $(this).val(node.text);
        //        }
        //        else {
        //            var data = node.attributes;
        //            var field = getInfolightOption($(this)).field;
        //            $(this).val(data[field]);
        //        }
    });
    $(editDialog).window('open');
}
function insertTreeNodeOK(treeview) {
    var remoteName = getInfolightOption($(treeview)).remoteName;
    var tableName = getInfolightOption($(treeview)).tableName;
    var editDialog = getInfolightOption($(treeview)).editDialog;
    var idField = getInfolightOption($(treeview)).idField;
    var textField = getInfolightOption($(treeview)).textField;
    var parentField = getInfolightOption($(treeview)).parentField;
    var id = "";
    var text = "";
    var row = {};
    var srcrow = {};
    $(":text", $(editDialog)).each(function () {
        if (getInfolightOption($(this)).field == idField) {
            id = $(this).val();
            row[idField] = id;
            srcrow[idField] = id;
        }
        else if (getInfolightOption($(this)).field == textField) {
            text = $(this).val();
            row[textField] = text;
            srcrow[textField] = text;
        }
        else {
            var value = $(this).val();
            var field = getInfolightOption($(this)).field;
            row[field] = value;
            srcrow[field] = value;
        }
    });
    if (id != "") {
        var node = $(treeview).tree('getSelected');
        var changedDatas = [];
        var changedRows = {tableName: tableName, inserted: [], deleted: [], updated: []};
        if ($(treeview).attr('editMode') == 'update') {
            row[parentField] = node.attributes[parentField];
            changedRows.updated.push(row);
        }
        if ($(treeview).attr('editMode') == 'insert') {
            row[parentField] = node.id;
            changedRows.inserted.push(row);
        }
        changedDatas.push(changedRows);

        var duplicateCheckS = true;
        if ($(treeview).attr('editMode') == 'insert') {
            $.fn.Error.errorCode = 1601;

            $.ajax({
                type: "POST",
                url: getDataUrl(),
                data: getRemoteParam({
                    data: $.toJSONString(changedDatas),
                    mode: 'duplicate'
                }, remoteName, tableName, false),
                cache: false,
                async: false,
                success: function (a) {
                    if (a == "false") {
                        alertMessage("duplicatecheckmsg");
                        duplicateCheckS = false;
                    }
                },
                error: function () {
                    duplicateCheckS = false;
                }
            });
        }

        if (duplicateCheckS) {
            $.fn.Error.errorCode = 1602;
            $.ajax({
                type: "POST",
                url: getDataUrl(),
                data: getRemoteParam({
                    data: $.toJSONString(changedDatas),
                    mode: 'update'
                }, remoteName, tableName, false),
                cache: false,
                async: true,
                beforeSend: function () {
                    $.messager.progress({
                        title: 'Please waiting',
                        msg: 'Saving data...'
                    });
                },
                success: function (data) {
                    if ($(treeview).attr('editMode') == 'update') {
                        $(treeview).tree('update', {
                            target: node.target,
                            text: text,
                            attributes: srcrow
                        });
                    }
                    if ($(treeview).attr('editMode') == 'insert') {
                        var newdata = {};
                        newdata[idField] = eval(data)[0][idField];
                        newdata[textField] = eval(data)[0][textField];
                        newdata[parentField] = eval(data)[0][parentField];
                        newdata.attributes = eval(data)[0];
                        $(treeview).tree('append', {
                            parent: (node ? node.target : null),
                            data: [newdata]
                        });
                    }
                    $(editDialog).window('close');
                },
                error: function (data) {
                    alert(getErrorMessage(data.responseText));
                    data.responseText = '';
                },
                complete: function () {
                    $(document).ready(function () {
                        $.messager.progress('close');
                    });
                }
            });
        }
    }
}
function closeTreeNodeEditor(treeview) {
    var editDialog = getInfolightOption($(treeview)).editDialog;
    $(editDialog).window('close');
}
function removeTreeNode(treeview) {
    if (confirm(deleteMessage)) {
        var remoteName = getInfolightOption($(treeview)).remoteName;
        var tableName = getInfolightOption($(treeview)).tableName;
        var idField = getInfolightOption($(treeview)).idField;
        var textField = getInfolightOption($(treeview)).textField;
        //var parentField = getInfolightOption($(treeview)).parentField;
        var row = {};
        var node = $(treeview).tree('getSelected');
        row[idField] = node.id;
        row[textField] = node.text;
        var changedDatas = [];
        var changedRows = {tableName: tableName, inserted: [], deleted: [], updated: []};
        changedRows.deleted.push(row);
        changedDatas.push(changedRows);
        $.fn.Error.errorCode = 1603;
        $.ajax({
            type: "POST",
            url: getDataUrl(),
            data: getRemoteParam({data: $.toJSONString(changedDatas), mode: 'update'}, remoteName, tableName, false),
            cache: false,
            async: true,
            beforeSend: function () {
                $.messager.progress({
                    title: 'Please waiting',
                    msg: 'Saving data...'
                });
            },
            success: function () {
                var node = $(treeview).tree('getSelected');
                $(treeview).tree('remove', node.target);
            },
            complete: function () {
                $(document).ready(function () {
                    $.messager.progress('close');
                });
            }
        });
    }
}
//-----------------------------------------------------------------------------------------------------------------

//fileupload support function
jQuery.extend({
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
        var form = jQuery('<form  action="" method="POST" name="' + formId + '" id="' + formId + '" enctype="multipart/form-data"></form>');
        if (data) {
            for (var i in data) {
                jQuery('<input type="hidden" name="' + i + '" value="' + data[i] + '" />').appendTo(form);
            }
        }
        var oldElement = jQuery('#' + fileElementId);
        var newElement = jQuery(oldElement).clone();
        jQuery(oldElement).attr('id', fileId);
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
        //  TODO introduce global settings, allowing the client to modify them for all requests, not only timeout
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
            jQuery.messager.progress('close');
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
                if (s.global && !--jQuery.active)
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
            jQuery.messager.progress({
                title: 'Please waiting',
                msg: 'Uploading data...'
            });
            jQuery(form).submit();

        } catch (e) {
            jQuery.handleError(s, xml, null, e);
            jQuery.messager.progress('close');
        }

        jQuery('#' + frameId).load(uploadCallback);
        return {
            abort: function () {
            }
        };

    },

    uploadHttpData: function (r, type) {
        var data = !type;
        data = type == "xml" || data ? r.responseXML : r.responseText;
        // If the type is "script", eval it in global context
        if (type == "script")
            jQuery.globalEval(data);
        // Get the JavaScript object, if JSON is used.
        if (type == "json") {
            var data2 = data;
            if (data2.indexOf('<pre') != -1) {
                //此段代码时为了兼容火狐和chrome浏览器
                var newDiv = jQuery(document.createElement("div"));
                newDiv.html(data2);
                data = $("pre:first", newDiv).html();
            }
            else if (data2.indexOf('<PRE') != -1) {
                //此段代码时为了兼容火狐和chrome浏览器
                var newDiv = jQuery(document.createElement("div"));
                newDiv.html(data2);
                data = $("PRE:first", newDiv).html();
            }
            eval("data = " + data);
        }
        // evaluate scripts within html
        if (type == "html")
            jQuery("<div>").html(data).evalScripts();
        return data;
    },

    handleError: function (s, xhr, status, e) {
        if (s.error) {
            s.error.call(s.context || window, xhr, status, e);
        }
        if (s.global) {
            (s.context ? jQuery(s.context) : jQuery.event).trigger("ajaxError", [xhr, s, e]);
        }
    }
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
    //根据QueryString参数名称获取值 
    getQueryStringByName2: function (name) {
        var result = location.search.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
        if (result == null || result.length < 1) {
            return Request.getQueryStringByName(name);
            //return "";
        }
        return decodeURI(result[1]);
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
                return Request.getQueryStringByName(name);
            }
            return decodeURI(result[1]);
        }
        else {
            return Request.getQueryStringByName(name);
        }
    }
};

String.format = function () {
    if (arguments.length == 0)
        return null;

    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
};

$.convertRemark = function (remark, flag) {
    if (remark != undefined && remark != null) {
        if (flag) {
            remark = remark.replace(/\?/g, "gimonnhu");
            //remark = encodeURIComponent(remark);
        }
        else
            remark = remark.replace(/gimonnhu/g, "?");
    }
    return remark;
};

$.changeTheme = function (themeName, isSubFolder) {
    var themeHref = "js/themes/" + themeName + "/easyui.css";
    if (isSubFolder != undefined)
        themeHref = isSubFolder + themeHref;
    $("#easyuiTheme").attr("href", themeHref);

    var iframe = $("iframe");
    if (iframe.length > 0) {
        for (var i = 0; i < iframe.length; i++) {
            $(iframe[i]).contents().find("#easyuiTheme").attr("href", "../" + themeHref);
        }
    }
};

$.changeCapsLock = function (target, capsLock) {
    if (capsLock.toLowerCase() == "upper") {
        target.bind('keyup', function () {
            var keyCode;
            if (event.keyCode) {
                keyCode = event.keyCode;
            }
            else {
                keyCode = event.which;
            }
            if (keyCode != 37 && keyCode != 39 && keyCode != 8)
                target.val(target.val().toUpperCase());
        });
    }
    else if (capsLock.toLowerCase() == "lower") {
        target.bind('keyup', function () {
            var keyCode;
            if (event.keyCode) {
                keyCode = event.keyCode;
            }
            else {
                keyCode = event.which;
            }
            if (event.keyCode != 37 && event.keyCode != 39 && keyCode != 8)
                target.val(target.val().toLowerCase());
        });
    }
};
$.changeCapsLock2 = function (target, capsLock) {
    if (capsLock.toLowerCase() == "upper") {
        var keyCode;
        if (event.keyCode) {
            keyCode = event.keyCode;
        }
        else {
            keyCode = event.which;
        }
        if (keyCode != 37 && keyCode != 39 && keyCode != 8)
            target.val(target.val().toUpperCase());
    }
    else if (capsLock.toLowerCase() == "lower") {
        var keyCode;
        if (event.keyCode) {
            keyCode = event.keyCode;
        }
        else {
            keyCode = event.which;
        }
        if (event.keyCode != 37 && event.keyCode != 39 && keyCode != 8)
            target.val(target.val().toLowerCase());
    }
};

$.gotoNextControl = function (target, contorlID) {
    if (target.val().length == target.attr('maxlength')) {
        var inputs = $('input,textarea', contorlID);
        var idx = inputs.index(target);
        if (idx == inputs.length - 1) {
            //inputs[0].focus();
            //inputs[0].select();
        }
        else {
            if (inputs[idx + 1].className == "info-refval refval-f") idx++;
            inputs[idx + 1].focus();
            //inputs[idx + 1].select();
        }
    }
};

$.fn.infoqrcode = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.infoqrcode.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
};
$.fn.infoqrcode.methods = {
    setValue: function (jq, value) {
        if (jq.length > 0) {
            var target = $(jq[0]);
            var render = getInfolightOption(target).render;
            if (render == undefined) render = "table";
            var size = getInfolightOption(target).size;
            var width = size;
            var height = size;
            //var text = getInfolightOption(target).text;

            if (value == undefined) value = "";
            var out = target.infoqrcode('utf16to8', value);
            if (width != undefined && height != undefined) {
                $('canvas', target).remove();
                $('table', target).remove();
                target.qrcode({render: render, width: width, height: height, text: out});
            }
            else {
                $('canvas', target).remove();
                $('table', target).remove();
                target.qrcode({render: render, text: out});
            }
        }
    },
    utf16to8: function (jq, str) {
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
    }
};

$.fn.infoform = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.infoform.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
};
$.fn.infoform.methods = {
    callfunction: function (jq, functionname) {
        jq.each(function () {
            if (functionname == 'insert' || functionname == 'update' || functionname == 'remove'
                || functionname == 'query' || functionname == 'previous' || functionname == 'next') {
                $(this).infoform(functionname);
            }
            else {
                eval(functionname).call($(this));
            }
        });
    },
    insert: function (jq) {
        var form = $(jq[0]);
        var formOnlyRelationGrid = form.attr('formOnlyRelationGrid');
        if (formOnlyRelationGrid != undefined) {
            insertItem(formOnlyRelationGrid);
        }
    },
    update: function (jq) {
        var form = $(jq[0]);
        var formOnlyRelationGrid = form.attr('formOnlyRelationGrid');
        if (formOnlyRelationGrid != undefined) {
            updateItem(formOnlyRelationGrid);
        }
    },
    remove: function (jq) {
        var form = $(jq[0]);
        var formOnlyRelationGrid = form.attr('formOnlyRelationGrid');
        if (formOnlyRelationGrid != undefined) {
            var pagination = $(formOnlyRelationGrid).datagrid('getPager');
            var pageNumber = pagination.pagination('options').pageNumber;
            var total = pagination.pagination('options').total;

            deleteItem(formOnlyRelationGrid);
            $(formOnlyRelationGrid).datagrid('acceptChanges');
            if (pageNumber <= total) {
                pagination.pagination('select', pageNumber);
            }
            else {
                var editDialog = getInfolightOption($(formOnlyRelationGrid)).editDialog;
                openForm(editDialog, null, 'viewed', 'continue');
            }
        }
    },
    query: function (jq) {
        var form = $(jq[0]);
        var formOnlyRelationGrid = form.attr('formOnlyRelationGrid');
        if (formOnlyRelationGrid != undefined) {
            openQuery(formOnlyRelationGrid);
        }
    },
    previous: function (jq) {
        var form = $(jq[0]);
        var formOnlyRelationGrid = form.attr('formOnlyRelationGrid');
        if (formOnlyRelationGrid != undefined) {
            var pagination = $(formOnlyRelationGrid).datagrid('getPager');
            var pageNumber = pagination.pagination('options').pageNumber;
            if (pageNumber > 1) {
                pagination.pagination('select', pageNumber - 1);
            }
        }
    },
    next: function (jq) {
        var form = $(jq[0]);
        var formOnlyRelationGrid = form.attr('formOnlyRelationGrid');
        if (formOnlyRelationGrid != undefined) {
            var pagination = $(formOnlyRelationGrid).datagrid('getPager');
            var pageNumber = pagination.pagination('options').pageNumber;
            var total = pagination.pagination('options').total;
            if (pageNumber < total) {
                pagination.pagination('select', pageNumber + 1);
            }
        }
    }
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
            var drilldown = getInfolightOption($(this)).drillobjectid;
            var target = $('#' + drilldown);
            if (target.length) {
                target.drilldown('showDrillDown', getInfolightOption($(this)).drillfields);
            }
        });
    },
    showDrillDown: function (jq, values) {
        jq.each(function () {
            var drilldown = $(this);
            var beforeDrillDown = getInfolightOption(drilldown).beforeDrillDown;
            if (beforeDrillDown != undefined) {
                var flag = beforeDrillDown.call(drilldown, values);
                if (flag != undefined && flag.toString() == 'false') {
                    return;
                }
            }
            var drillStyle = getInfolightOption(drilldown).drillStyle;
            var tableName = getInfolightOption(drilldown).tableName;
            var remoteName = getInfolightOption(drilldown).remoteName;
            var keyFields = getInfolightOption(drilldown).keyFields;

            if (drillStyle == 'webform') {
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

                var formname = getInfolightOption(drilldown).formName;
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
                    var openMode = getInfolightOption(drilldown).openMode;
                    var urlParam = "?DRILLDOWN=true&REMOTENAME=" + encodeURIComponent(remoteName) + "&TABLENAME=" + encodeURIComponent(tableName) + "&DRILLDOWN_KEYFIELD=" + encodeURIComponent(where);
                    var url = getParentFolder() + "../handler/SystemHandle.ashx?Type=Encrypt";
                    if (developer) {
                        url = getParentFolder() + "../handler/SystemHandler.ashx?type=Encrypt";
                    }
                    $.fn.Error.errorCode = 1700;
                    $.ajax({
                        url: url,
                        data: {param: urlParam},
                        type: 'post',
                        async: true,
                        success: function (param) {
                            var newurl = formname + "?" + param;
                            if (openMode == "newwindow") {
                                window.open(newurl);
                            }
                            else if (openMode == "dialog") {
                                var height = $(window).height() - 20;
                                var width = $(window).width() - 20;
                                var title = getInfolightOption(drilldown).formName;
                                var t = getInfolightOption(drilldown).formName.split("/");
                                var t2 = t[t.length - 1];
                                if (t2.lastIndexOf(".aspx") != -1) {
                                    title = t2.slice(0, t2.lastIndexOf(".aspx"));
                                }
                                var dialog = $('<div/>')
                                    .dialog({
                                        draggable: false,
                                        modal: true,
                                        height: height,
                                        width: width,
                                        title: title
                                    });
                                $('<iframe style="border: 0px;" src="' + newurl + '" width="100%" height="100%"></iframe>').appendTo(dialog.find('.panel-body'));
                                dialog.dialog('open');
                            }
                            else if (openMode == "newtab") {
                                var title = "";
                                var t = getInfolightOption(drilldown).formName.split("/");
                                var t2 = t[t.length - 1];
                                if (t2.lastIndexOf(".aspx") != -1) {
                                    title = t2.slice(0, t2.lastIndexOf(".aspx"));
                                }
                                var mainTab = top.$("#tabsMain");
                                //url = encodeURI(url);加了这段的话，flow的status会被转码2次，所以先拿掉
                                if (mainTab.length == 0) {
                                    mainTab = $("#tabsWorkFlow");
                                }
                                if (mainTab.length == 0) {
                                    mainTab = top.$('#tabMain');
                                }

                                if (window.top.location && window.top.location.href.indexOf("SDMain.aspx") != -1) {
                                    window.open(newurl);
                                }
                                else {
                                    if (newurl.indexOf("../") == 0) {
                                        newurl = newurl.slice(3);
                                    }
                                    if (developer) {
                                        newurl = 'preview' + developer + '/' + newurl;
                                    }
                                    if (mainTab.tabs('exists', title)) {
                                        mainTab.tabs('select', title); //选中并刷新
                                        var currTab = mainTab.tabs('getSelected');
                                        var urlSrc = $(currTab.panel('options').content).attr('src');
                                        if (urlSrc != undefined && currTab.panel('options').title != 'Home') {
                                            mainTab.tabs('update', {
                                                tab: currTab,
                                                options: {
                                                    content: '<iframe scrolling="auto" frameborder="0"  src="' + newurl + '" style="width:100%;height:100%;"></iframe>'
                                                }
                                            });
                                        }
                                    } else {
                                        var content = '<iframe scrolling="auto" frameborder="0"  src="' + newurl + '" style="width:100%;height:100%;"></iframe>';
                                        mainTab.tabs('add', {
                                            title: title,
                                            content: content,
                                            closable: true,
                                            style: 'background-image: url(../img/main.jpg)'
                                        });
                                    }
                                }
                            }
                        }
                    });
                }
            }
            else if (drillStyle == 'mobileform') {
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
                var formname = getInfolightOption(drilldown).formName;
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
                    var openMode = getInfolightOption(drilldown).openMode;
                    var urlParam = "?DRILLDOWN=true&REMOTENAME=" + encodeURIComponent(remoteName) + "&TABLENAME=" + encodeURIComponent(tableName) + "&DRILLDOWN_KEYFIELD=" + encodeURIComponent(where);
                    //var url = "../handler/SystemHandle.ashx?Type=Encrypt";
                    //if (developer) {
                    //    url = "../handler/SystemHandler.ashx?type=Encrypt";
                    //}
                    $.fn.Error.errorCode = 1703;
                    var newurl = formname + urlParam;
                    if (openMode == "newwindow") {
                        window.open(newurl);
                    }
                    else if (openMode == "dialog") {
                        var height = $(window).height() - 20;
                        var width = 480;//mobile
                        var title = getInfolightOption(drilldown).formName;
                        var t = getInfolightOption(drilldown).formName.split("/");
                        var t2 = t[t.length - 1];
                        if (t2.lastIndexOf(".aspx") != -1) {
                            title = t2.slice(0, t2.lastIndexOf(".aspx"));
                        }
                        var dialog = $('<div/>')
                            .dialog({
                                draggable: false,
                                modal: true,
                                height: height,
                                width: width,
                                title: title
                            });
                        $('<iframe style="border: 0px;" src="' + newurl + '" width="100%" height="100%"></iframe>').appendTo(dialog.find('.panel-body'));
                        dialog.dialog('open');
                    }
                    else if (openMode == "newtab") {
                        var title = "";
                        var t = getInfolightOption(drilldown).formName.split("/");
                        var t2 = t[t.length - 1];
                        if (t2.lastIndexOf(".aspx") != -1) {
                            title = t2.slice(0, t2.lastIndexOf(".aspx"));
                        }
                        var mainTab = top.$("#tabsMain");
                        //url = encodeURI(url);加了这段的话，flow的status会被转码2次，所以先拿掉
                        if (mainTab.length == 0) {
                            mainTab = $("#tabsWorkFlow");
                        }
                        if (mainTab.length == 0) {
                            mainTab = top.$('#tabMain');
                        }

                        if (window.top.location && window.top.location.href.indexOf("SDMain.aspx") != -1) {
                            window.open(newurl);
                        }
                        else {
                            if (newurl.indexOf("../") == 0) {
                                newurl = newurl.slice(3);
                            }
                            if (developer) {
                                newurl = 'preview' + developer + '/' + newurl;
                            }
                            if (mainTab.tabs('exists', title)) {
                                mainTab.tabs('select', title); //选中并刷新
                                var currTab = mainTab.tabs('getSelected');
                                var urlSrc = $(currTab.panel('options').content).attr('src');
                                if (urlSrc != undefined && currTab.panel('options').title != 'Home') {
                                    mainTab.tabs('update', {
                                        tab: currTab,
                                        options: {
                                            content: '<iframe scrolling="auto" frameborder="0"  src="' + newurl + '" style="width:100%;height:100%;"></iframe>'
                                        }
                                    });
                                }
                            } else {
                                var content = '<iframe scrolling="auto" frameborder="0"  src="' + newurl + '" style="width:100%;height:100%;"></iframe>';
                                mainTab.tabs('add', {
                                    title: title,
                                    content: content,
                                    closable: true,
                                    style: 'background-image: url(../img/main.jpg)'
                                });
                            }
                        }
                    }
                }
            }
            else if (drillStyle.toLowerCase() == "rdlc") {
                var where = "";
                if (values != undefined) {
                    for (var i = 0; i < values.length; i++) {
                        if (keyFields[i] != undefined) {
                            var value = values[i].value;
                            var field = keyFields[i].field;
                            if (where != "") where += " and ";
                            where += field + "='" + value + "'";
                        }
                    }
                }
                var reportName = getInfolightOption(drilldown).reportName;
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
                    var height = $(window).height() - 20;
                    var width = $(window).width() - 20;
                    if ($.browser.msie) {
                        window.open(url, '_blank', 'scrollbars=yes, resizable=yes, location=no, width=' + width + ', height=' + height);
                    }
                    else {
                        var dialog = $('<div/>')
                            .dialog({
                                draggable: false,
                                modal: true,
                                height: height,
                                width: width,
                                title: "Report"//,
                                //maximizable: true
                            });
                        $('<iframe style="border: 0px;" src="' + url + '" width="100%" height="100%"></iframe>').appendTo(dialog.find('.panel-body'));
                        dialog.dialog('open');
                    }
                }
            }
            else if (drillStyle == "command") {
                $.fn.Error.errorCode = 1702;
                var where = "";
                if (values != undefined) {
                    for (var i = 0; i < values.length; i++) {
                        if (keyFields[i] != undefined) {
                            var value = values[i].value;
                            var field = keyFields[i].field;
                            if (where != "") where += " and ";
                            where += field + "='" + value + "'";
                        }
                    }
                }
                var DisplayFields = getInfolightOption(drilldown).displayFields;
                var FormCaption = getInfolightOption(drilldown).formCaption;
                if (FormCaption == undefined || FormCaption == "") {//空白的title会导致dialog上方的标题行消失，会没有关闭的大叉
                    FormCaption = 'Drill Down Dialog';
                }
                if (DisplayFields != undefined && DisplayFields.length > 0) {
                    var columns = [];
                    for (var i = 0; i < DisplayFields.length; i++) {
                        var column = {
                            field: DisplayFields[i].field,
                            title: DisplayFields[i].caption,
                            width: DisplayFields[i].width
                        };
                        if (DisplayFields[i].drillObjectID != undefined && DisplayFields[i].drillObjectID != "") {
                            column.formatter = formatValue;
                            var drillfieldstring = "";
                            for (var j = 0; j < DisplayFields[i].drillFields.length; j++) {
                                if (drillfieldstring != "") drillfieldstring += ";";
                                drillfieldstring += DisplayFields[i].drillFields[j].field;
                            }
                            if (drillfieldstring == "") drillfieldstring = DisplayFields[i].field;
                            column.format = 'drilldown,drillObjectID:' + DisplayFields[i].drillObjectID + ',drillFields:' + drillfieldstring;
                        }
                        columns.push(column);
                    }
                    var queryParams = {};
                    var queryWord = {whereString: where};
                    queryParams.queryWord = $.toJSONString(queryWord);
                    var div = $('<div />').appendTo('body');
                    $('<table />').appendTo(div)
                        .datagrid({
                            url: getDataUrl(),
                            queryParams: getRemoteParam(queryParams, remoteName, tableName, true),
                            pageNumber: 1,
                            pageSize: 10,
                            pagination: true,
                            singleSelect: true,
                            columns: [columns]

                        });
                    div.dialog({
                        title: FormCaption,
                        width: 600,
                        height: 350,
                        closed: false,
                        cache: false,
                        modal: true,
                        onClose: function () {
                            $(this).remove();
                        }
                    });
                }
            }
        });
    }
};

$.fn.rotator = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.rotator.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
};
$.fn.rotator.methods = {
    slide: function (jq) {
        var rotator = $(jq[0]);
        var options = getInfolightOption(rotator);

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
            var queryWord = {whereString: options.whereString};

            var obj;
            $.ajax({
                type: "POST",
                url: getDataUrl(),
                data: getRemoteParam({queryWord: $.toJSONString(queryWord)}, remoteName, tableName, false),
                cache: false,
                async: false,
                success: function (data) {
                    obj = $.parseJSON(data);
                    var items = [];
                    for (var i = 0; i < obj.length; i++) {
                        if (rotatorType == "text")
                            items.push({text: obj[i][fieldName]});
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

                            items.push({imgUrl: vpath});
                        }
                    }
                    $.metro.changeItemsImageText("Menu" + menu, items);
                }, error: function (data) {
                }
            });
        }
    }
};

$.fn.Error = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.Error.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
};
$.fn.Error.errorCode = 0;

function UnlockAll(userID, callback) {
    var data = {mode: 'removerecordlock'};
    if (userID) {
        data.userID = userID;
    }
    $.ajax({
        type: "POST",
        url: getParentFolder() + "../handler/jqDataHandle.ashx",
        data: data,
        cache: false,
        async: false,
        success: function () {
            if (callback) {
                callback.call(null);
            }
        }
    });

}

function saveHtmlToDB(jqhtmlId, kvs) {
    var infolightOptions = getInfolightOption($('#' + jqhtmlId));
    var keyValues = kvs;
    if (keyValues.length == 0) {
        alert("Please set keys and keyValues first.");
        return;
    }

    var updateValue = "{";
    for (var i = 0; i < keyValues.length; i++) {
        updateValue += '"' + keyValues[i].key + '":"' + keyValues[i].value + '",';
    }
    var content = UE.getEditor(jqhtmlId).getContent();
    content = content.replace(/\"/g, "qnMk").replace(/\&quot;/g, "qnMk");
    content = content.replace(/\\/g, "bhMk");
    content = encodeURI(content);
    updateValue += '"' + infolightOptions.columnName + '":"' + content + '"}';
    updateValue = JSON.parse(updateValue);
    //alert(content);

    var changedData = [];
    var changedRows = {tableName: infolightOptions.tableName, deleted: [], inserted: [], updated: []};
    changedRows.updated.push(updateValue);
    changedData.push(changedRows);
    $.ajax({
        type: "POST",
        dataType: 'json',
        url: getDataUrl(),
        data: getRemoteParam({
            data: $.toJSONString(changedData),
            mode: 'update'
        }, infolightOptions.remoteName, infolightOptions.tableName),
        cache: false,
        async: true,
        success: function () {
            alert("Save success.");
        },
        error: function (ex) {

        },
        complete: function () {

        }
    });
}

function loadHtmlFromDB(jqhtmlId, kvs) {
    var infolightOptions = getInfolightOption($('#' + jqhtmlId));
    if (infolightOptions.mode == "Edit") {
        UE.getEditor(jqhtmlId).setEnabled();
        $("#" + jqhtmlId + "_view").css("display", "none");
    }
    else if (infolightOptions.mode == "Show") {
        UE.getEditor(jqhtmlId).setHide();
    }
    var keyValues = kvs;
    if (keyValues.length == 0) {
        alert("Please set keys and keyValues first.");
        return;
    }

    var queryWord = {whereString: ""};
    for (var i = 0; i < keyValues.length; i++) {
        queryWord.whereString += keyValues[i].key + " = " + formatQueryValue(keyValues[i].value, "string", false) + " and ";
    }
    queryWord.whereString = queryWord.whereString.substr(0, queryWord.whereString.lastIndexOf(' and '));
    //content = content.replace(/\</g,'&lt;').replace(/\>/g,'&rt;');
    //alert(content);

    $.ajax({
        type: "POST",
        dataType: 'json',
        url: getDataUrl(),
        data: getRemoteParam({queryWord: $.toJSONString(queryWord)}, infolightOptions.remoteName, infolightOptions.tableName),
        cache: false,
        async: false,
        success: function (data) {
            var content = "";
            if (data.length > 0 && data[0][infolightOptions.columnName] != null) {
                content = data[0][infolightOptions.columnName];
                content = content.replace(/qnMk/g, "\"");
                content = content.replace(/bhMk/g, "\\");
            }
            if (infolightOptions.mode == "Edit")
                UE.getEditor(jqhtmlId).setContent(content, false);
            else if (infolightOptions.mode == "Show") {
                $("#" + jqhtmlId + "_view")[0].innerHTML = content;
                //UE.getEditor(jqhtmlId).setContent(content, false);
            }
        },
        error: function (ex) {

        },
        complete: function () {

        }
    });
}

$.fn.pivotTable = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.pivotTable.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            $(this).pivotTable('initialize', methodName);
            if (!$(this).hasClass($.fn.pivotTable.foo)) {
                $(this).addClass($.fn.pivotTable.foo)
            }
        });
    }
};

$.fn.pivotTable.foo = 'info-privottable';

$.fn.pivotTable.defaults = {
    title: ""
};
$.fn.pivotTable.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var chartOptions = {};
            var htmlOptions = getInfolightOption($(this)); //html option
            for (var property in htmlOptions) {
                chartOptions[property] = htmlOptions[property];
            }
            chartOptions.whereString = $(this).pivotTable('getWhereItem');
            $(this).data('options', chartOptions);

            //var title = chartOptions.title;

            if (chartOptions.alwaysClose) {
                $(this).pivotTable('options').whereString = '1=0';
            }
            if (chartOptions.renderObjectID != undefined && chartOptions.renderObjectID != "") {
                var renderObject = $('#' + chartOptions.renderObjectID);
                if (renderObject.length) {
                    $(this).appendTo(renderObject);
                }
            }
            $(this).pivotTable('load');

        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return {};
            }
            return $(jq[0]).data('options');
        }
        return {};
    },
    load: function (jq) {
        jq.each(function () {
            var options = $(this).pivotTable('options');
            if (options.remoteName != undefined && options.remoteName != "" && options.tableName != undefined) {
                if (options.onBeforeLoad != undefined) {
                    options.onBeforeLoad.call(this);
                }
                var queryWord = {};
                if (options.whereString != undefined) {
                    queryWord.whereString = options.whereString;
                }
                $(this).pivotTable('loadData');
                //$(this).data('showColumns', options.showColumns);
                //$(this).data('rows', options.rows);
                //$(this).data('cols', options.columns);
                $(this).pivotTable('renderDIV', options);
            }
        });
    },
    loadData: function (jq) {
        var data = [];
        if (jq.length > 0) {
            var options = $(jq[0]).pivotTable('options');
            if (options.remoteName != undefined && options.remoteName != "" && options.tableName != undefined) {
                var queryWord = {};
                if (options.whereString != undefined) {
                    queryWord.whereString = options.whereString;
                }
                $.ajax({
                    type: "POST",
                    url: getDataUrl(),
                    data: getRemoteParam({queryWord: $.toJSONString(queryWord)}, options.remoteName, options.tableName, false),
                    cache: false,
                    async: false,
                    success: function (returndata) {
                        if (returndata != null) {
                            returndata = $.parseJSON(returndata);
                            $(jq[0]).data('data', returndata);
                            data = returndata;
                        }
                    },
                    error: function () {
                        //alert('error');
                    }
                });
            }
            return {data: data};
        }
    },
    getData: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('data') == undefined) {
                return [];
            }
            return $(jq[0]).data('data');
        }
        return [];
    },
    renderDIV: function (jq, options) {
        var truedata = $(jq[0]).pivotTable('getData');
        var showColumns = options.showColumns;
        var colss = options.columns;
        var rowss = options.rows;
        var rendererstype = options.renderers;
        var aggregatorsmode = options.aggregators;
        //var derivers = $.pivotUtilities.derivers;
        var showData = [];
        for (var i = 0; i < truedata.length; i++) {
            var newRow = {};
            for (var j = 0; j < showColumns.length; j++) {
                newRow[showColumns[j]["caption"]] = eval("truedata[i]." + showColumns[j]["field"]);
            }
            showData.push(newRow);
        }
        if (truedata.length == 0) {
            var newRow = {};
            for (var j = 0; j < showColumns.length; j++) {
                newRow[showColumns[j]["caption"]] = "";
            }
            showData.push(newRow);
        }
        var rows = [];
        var columns = [];
        for (var i = 0; i < rowss.length; i++) {
            rows.push(rowss[i]["caption"]);
        }
        for (var i = 0; i < colss.length; i++) {
            columns.push(colss[i]["caption"]);
        }
        //reset renders and aggregators
        var hasProp = {}.hasOwnProperty;
        var syslanRenderers = $.sysmsg('getValue', 'JQWebClient/PivotTable/Renderers').split(";");
        var syslanaggregators = $.sysmsg('getValue', 'JQWebClient/PivotTable/Aggregators').split(";");
        var PivotData, addSeparators, aggregatorTemplates, aggregators, dayNamesEn, derivers, getSort, locales, mthNamesEn, naturalSort, numberFormat, pivotTableRenderer, renderers, sortAs, usFmt, usFmtInt, usFmtPct, zeroPad, makeC3Chart;
        addSeparators = function (nStr, thousandsSep, decimalSep) {
            var rgx, x, x1, x2;
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? decimalSep + x[1] : '';
            rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + thousandsSep + '$2');
            }
            return x1 + x2;
        };
        numberFormat = function (opts) {
            var defaults;
            defaults = {
                digitsAfterDecimal: 2,
                scaler: 1,
                thousandsSep: ",",
                decimalSep: ".",
                prefix: "",
                suffix: "",
                showZero: false
            };
            opts = $.extend(defaults, opts);
            return function (x) {
                var result;
                if (isNaN(x) || !isFinite(x)) {
                    return "";
                }
                if (x === 0 && !opts.showZero) {
                    return "";
                }
                result = addSeparators((opts.scaler * x).toFixed(opts.digitsAfterDecimal), opts.thousandsSep, opts.decimalSep);
                return "" + opts.prefix + result + opts.suffix;
            };
        };
        usFmt = numberFormat();
        usFmtInt = numberFormat({
            digitsAfterDecimal: 0
        });
        usFmtPct = numberFormat({
            digitsAfterDecimal: 1,
            scaler: 100,
            suffix: "%"
        });

        aggregatorTemplates = $.pivotUtilities.aggregatorTemplates;
        aggregators = (function (tpl) {
            var aggregatorslist = {};
            aggregatorslist[0] = tpl.sum(usFmt); //sum
            aggregatorslist[1] = tpl.count(usFmtInt); //Count
            aggregatorslist[2] = tpl.countUnique(usFmtInt); //Count Unique Values
            aggregatorslist[3] = tpl.listUnique(", "); //List Unique Values
            aggregatorslist[4] = tpl.sum(usFmtInt); //Integer Sum
            aggregatorslist[5] = tpl.average(usFmt); //Average
            aggregatorslist[6] = tpl.min(usFmt); //Minimum
            aggregatorslist[7] = tpl.max(usFmt); //Maximum
            aggregatorslist[8] = tpl.sumOverSum(usFmt); //Sum over Sum
            aggregatorslist[9] = tpl.sumOverSumBound80(true, usFmt); //80% Upper Bound
            aggregatorslist[10] = tpl.sumOverSumBound80(false, usFmt); //80% Lower Bound
            aggregatorslist[11] = tpl.fractionOf(tpl.count(), "total", usFmtPct); //Count as Fraction of Total
            aggregatorslist[12] = tpl.fractionOf(tpl.count(), "row", usFmtPct); //Count as Fraction of Rows
            aggregatorslist[13] = tpl.fractionOf(tpl.count(), "col", usFmtPct); //Count as Fraction of Columns
            aggregatorslist[14] = tpl.fractionOf(tpl.sum(), "total", usFmtPct); //Sum as Fraction of Total
            aggregatorslist[15] = tpl.fractionOf(tpl.sum(), "row", usFmtPct); //Sum as Fraction of Rows
            aggregatorslist[16] = tpl.fractionOf(tpl.sum(), "col", usFmtPct); //Sum as Fraction of Columns

            var o = {};
            var vs = aggregatorsmode.toString().split(";");
            for (var i = 0; i < vs.length; i++) {
                o[syslanaggregators[vs[i]]] = aggregatorslist[vs[i]];
            }
            return o;
        })($.pivotUtilities.aggregatorTemplates);

        makeC3Chart = function (chartOpts) {
            if (chartOpts == null) {
                chartOpts = {};
            }
            return function (pivotData, opts) {
                var agg, attrs, base, base1, base2, base3, base4, base5, colKey, colKeys, columns, dataColumns, defaults, fullAggName, groupByTitle, h, hAxisTitle, headers, i, j, k, l, len, len1, len2, len3, len4, m, numCharsInHAxis, numSeries, params, ref, ref1, ref2, ref3, renderArea, result, rotationAngle, row, rowHeader, rowKey, rowKeys, s, scatterData, series, title, titleText, vAxisTitle, val, vals, x, xs;
                defaults = {
                    localeStrings: {
                        vs: "vs",
                        by: "by"
                    },
                    c3: {}
                };
                opts = $.extend(true, defaults, opts);
                if ((base = opts.c3).size == null) {
                    base.size = {};
                }
                if ((base1 = opts.c3.size).width == null) {
                    base1.width = window.innerWidth / 1.4;
                }
                if ((base2 = opts.c3.size).height == null) {
                    base2.height = window.innerHeight / 1.4 - 50;
                }
                if (chartOpts.type == null) {
                    chartOpts.type = "line";
                }
                rowKeys = pivotData.getRowKeys();
                if (rowKeys.length === 0) {
                    rowKeys.push([]);
                }
                colKeys = pivotData.getColKeys();
                if (colKeys.length === 0) {
                    colKeys.push([]);
                }
                headers = (function () {
                    var i, len, results;
                    results = [];
                    for (i = 0, len = colKeys.length; i < len; i++) {
                        h = colKeys[i];
                        results.push(h.join("-"));
                    }
                    return results;
                })();
                rotationAngle = 0;
                fullAggName = pivotData.aggregatorName;
                if (pivotData.valAttrs.length) {
                    fullAggName += "(" + (pivotData.valAttrs.join(", ")) + ")";
                }
                if (chartOpts.type === "scatter") {
                    scatterData = {
                        x: {},
                        y: {},
                        t: {}
                    };
                    attrs = pivotData.rowAttrs.concat(pivotData.colAttrs);
                    vAxisTitle = (ref = attrs[0]) != null ? ref : "";
                    hAxisTitle = (ref1 = attrs[1]) != null ? ref1 : "";
                    groupByTitle = attrs.slice(2).join("-");
                    titleText = vAxisTitle;
                    if (hAxisTitle !== "") {
                        titleText += " " + opts.localeStrings.vs + " " + hAxisTitle;
                    }
                    if (groupByTitle !== "") {
                        titleText += " " + opts.localeStrings.by + " " + groupByTitle;
                    }
                    for (i = 0, len = rowKeys.length; i < len; i++) {
                        rowKey = rowKeys[i];
                        for (j = 0, len1 = colKeys.length; j < len1; j++) {
                            colKey = colKeys[j];
                            agg = pivotData.getAggregator(rowKey, colKey);
                            if (agg.value() != null) {
                                vals = rowKey.concat(colKey);
                                series = vals.slice(2).join("-");
                                if (series === "") {
                                    series = "series";
                                }
                                if ((base3 = scatterData.x)[series] == null) {
                                    base3[series] = [];
                                }
                                if ((base4 = scatterData.y)[series] == null) {
                                    base4[series] = [];
                                }
                                if ((base5 = scatterData.t)[series] == null) {
                                    base5[series] = [];
                                }
                                scatterData.y[series].push((ref2 = vals[0]) != null ? ref2 : 0);
                                scatterData.x[series].push((ref3 = vals[1]) != null ? ref3 : 0);
                                scatterData.t[series].push(agg.format(agg.value()));
                            }
                        }
                    }
                } else {
                    numCharsInHAxis = 0;
                    for (k = 0, len2 = headers.length; k < len2; k++) {
                        x = headers[k];
                        numCharsInHAxis += x.length;
                    }
                    if (numCharsInHAxis > 50) {
                        rotationAngle = 45;
                    }
                    columns = [];
                    for (l = 0, len3 = rowKeys.length; l < len3; l++) {
                        rowKey = rowKeys[l];
                        rowHeader = rowKey.join("-");
                        row = [rowHeader === "" ? pivotData.aggregatorName : rowHeader];
                        for (m = 0, len4 = colKeys.length; m < len4; m++) {
                            colKey = colKeys[m];
                            val = parseFloat(pivotData.getAggregator(rowKey, colKey).value());
                            if (isFinite(val)) {
                                if (val < 1) {
                                    row.push(val.toPrecision(3));
                                } else {
                                    row.push(val.toFixed(3));
                                }
                            } else {
                                row.push(null);
                            }
                        }
                        columns.push(row);
                    }
                    vAxisTitle = pivotData.aggregatorName + (pivotData.valAttrs.length ? "(" + (pivotData.valAttrs.join(", ")) + ")" : "");
                    hAxisTitle = pivotData.colAttrs.join("-");
                    titleText = fullAggName;
                    if (hAxisTitle !== "") {
                        titleText += " " + opts.localeStrings.vs + " " + hAxisTitle;
                    }
                    groupByTitle = pivotData.rowAttrs.join("-");
                    if (groupByTitle !== "") {
                        titleText += " " + opts.localeStrings.by + " " + groupByTitle;
                    }
                }
                title = $("<p>", {
                    style: "text-align: center; font-weight: bold"
                });
                title.text(titleText);
                params = {
                    axis: {
                        y: {
                            label: vAxisTitle
                        },
                        x: {
                            label: hAxisTitle,
                            tick: {
                                rotate: rotationAngle,
                                multiline: false
                            }
                        }
                    },
                    data: {
                        type: chartOpts.type
                    },
                    tooltip: {
                        grouped: false
                    },
                    color: {
                        pattern: ["#3366cc", "#dc3912", "#ff9900", "#109618", "#990099", "#0099c6", "#dd4477", "#66aa00", "#b82e2e", "#316395", "#994499", "#22aa99", "#aaaa11", "#6633cc", "#e67300", "#8b0707", "#651067", "#329262", "#5574a6", "#3b3eac"]
                    }
                };
                $.extend(params, opts.c3);
                if (chartOpts.type === "scatter") {
                    xs = {};
                    numSeries = 0;
                    dataColumns = [];
                    for (s in scatterData.x) {
                        numSeries += 1;
                        xs[s] = s + "_x";
                        dataColumns.push([s + "_x"].concat(scatterData.x[s]));
                        dataColumns.push([s].concat(scatterData.y[s]));
                    }
                    params.data.xs = xs;
                    params.data.columns = dataColumns;
                    params.axis.x.tick = {
                        fit: false
                    };
                    if (numSeries === 1) {
                        params.legend = {
                            show: false
                        };
                    }
                    params.tooltip.format = {
                        title: function () {
                            return fullAggName;
                        },
                        name: function () {
                            return "";
                        },
                        value: function (a, b, c, d) {
                            return scatterData.t[c][d];
                        }
                    };
                } else {
                    params.axis.x.type = 'category';
                    params.axis.x.categories = headers;
                    params.data.columns = columns;
                }
                if (chartOpts.stacked != null) {
                    params.data.groups = [
                        (function () {
                            var len5, n, results;
                            results = [];
                            for (n = 0, len5 = rowKeys.length; n < len5; n++) {
                                x = rowKeys[n];
                                results.push(x.join("-"));
                            }
                            return results;
                        })()
                    ];
                }
                renderArea = $("<div>", {
                    style: "display:none;"
                }).appendTo($("body"));
                result = $("<div>").appendTo(renderArea);
                params.bindto = result[0];
                c3.generate(params);
                result.detach();
                renderArea.remove();
                return $("<div>").append(title, result);
            };
        };

        var rendererslist = {};
        //table
        rendererslist[0] = function (pvtData, opts) {
            return pivotTableRenderer(pvtData, opts);
        };
        //table bar chart
        rendererslist[1] = function (pvtData, opts) {
            return $(pivotTableRenderer(pvtData, opts)).barchart();
        };
        //heatmap
        rendererslist[2] = function (pvtData, opts) {
            return $(pivotTableRenderer(pvtData, opts)).heatmap();
        };
        //row heatmap
        rendererslist[3] = function (pvtData, opts) {
            return $(pivotTableRenderer(pvtData, opts)).heatmap("rowheatmap");
        };
        //col heatmap
        rendererslist[4] = function (pvtData, opts) {
            return $(pivotTableRenderer(pvtData, opts)).heatmap("colheatmap");
        };
        //line cahrt of c3
        rendererslist[5] = makeC3Chart();
        //bar chart of c3
        rendererslist[6] = makeC3Chart({
            type: "bar"
        });
        //stacked bar chart of c3
        rendererslist[7] = makeC3Chart({
            type: "bar",
            stacked: true
        });
        //area chart of c3
        rendererslist[8] = makeC3Chart({
            type: "area",
            stacked: true
        });
        //scatter chart of c3
        rendererslist[9] = makeC3Chart({
            type: "scatter"
        });

        renderers = {};
        var vs2 = rendererstype.toString().split(";");
        for (var i = 0; i < vs2.length; i++) {
            renderers[syslanRenderers[vs2[i]]] = rendererslist[vs2[i]];
        }

        pivotTableRenderer = function (pivotData, opts) {
            var aggregator, c, colAttrs, colKey, colKeys, defaults, i, j, r, result, rowAttrs, rowKey, rowKeys, spanSize, td, th, totalAggregator, tr, txt, val, x;
            defaults = {
                localeStrings: {
                    totals: "Totals"
                }
            };
            opts = $.extend(defaults, opts);
            colAttrs = pivotData.colAttrs;
            rowAttrs = pivotData.rowAttrs;
            rowKeys = pivotData.getRowKeys();
            colKeys = pivotData.getColKeys();
            result = document.createElement("table");
            result.className = "pvtTable";
            spanSize = function (arr, i, j) {
                var l, len, n, noDraw, ref, ref1, stop, x;
                if (i !== 0) {
                    noDraw = true;
                    for (x = l = 0, ref = j; 0 <= ref ? l <= ref : l >= ref; x = 0 <= ref ? ++l : --l) {
                        if (arr[i - 1][x] !== arr[i][x]) {
                            noDraw = false;
                        }
                    }
                    if (noDraw) {
                        return -1;
                    }
                }
                len = 0;
                while (i + len < arr.length) {
                    stop = false;
                    for (x = n = 0, ref1 = j; 0 <= ref1 ? n <= ref1 : n >= ref1; x = 0 <= ref1 ? ++n : --n) {
                        if (arr[i][x] !== arr[i + len][x]) {
                            stop = true;
                        }
                    }
                    if (stop) {
                        break;
                    }
                    len++;
                }
                return len;
            };
            for (j in colAttrs) {
                if (!hasProp.call(colAttrs, j)) continue;
                c = colAttrs[j];
                tr = document.createElement("tr");
                if (parseInt(j) === 0 && rowAttrs.length !== 0) {
                    th = document.createElement("th");
                    th.setAttribute("colspan", rowAttrs.length);
                    th.setAttribute("rowspan", colAttrs.length);
                    tr.appendChild(th);
                }
                th = document.createElement("th");
                th.className = "pvtAxisLabel";
                th.textContent = c;
                tr.appendChild(th);
                for (i in colKeys) {
                    if (!hasProp.call(colKeys, i)) continue;
                    colKey = colKeys[i];
                    x = spanSize(colKeys, parseInt(i), parseInt(j));
                    if (x !== -1) {
                        th = document.createElement("th");
                        th.className = "pvtColLabel";
                        th.textContent = colKey[j];
                        th.setAttribute("colspan", x);
                        if (parseInt(j) === colAttrs.length - 1 && rowAttrs.length !== 0) {
                            th.setAttribute("rowspan", 2);
                        }
                        tr.appendChild(th);
                    }
                }
                if (parseInt(j) === 0) {
                    th = document.createElement("th");
                    th.className = "pvtTotalLabel";
                    th.innerHTML = opts.localeStrings.totals;
                    th.setAttribute("rowspan", colAttrs.length + (rowAttrs.length === 0 ? 0 : 1));
                    tr.appendChild(th);
                }
                result.appendChild(tr);
            }
            if (rowAttrs.length !== 0) {
                tr = document.createElement("tr");
                for (i in rowAttrs) {
                    if (!hasProp.call(rowAttrs, i)) continue;
                    r = rowAttrs[i];
                    th = document.createElement("th");
                    th.className = "pvtAxisLabel";
                    th.textContent = r;
                    tr.appendChild(th);
                }
                th = document.createElement("th");
                if (colAttrs.length === 0) {
                    th.className = "pvtTotalLabel";
                    th.innerHTML = opts.localeStrings.totals;
                }
                tr.appendChild(th);
                result.appendChild(tr);
            }
            for (i in rowKeys) {
                if (!hasProp.call(rowKeys, i)) continue;
                rowKey = rowKeys[i];
                tr = document.createElement("tr");
                for (j in rowKey) {
                    if (!hasProp.call(rowKey, j)) continue;
                    txt = rowKey[j];
                    x = spanSize(rowKeys, parseInt(i), parseInt(j));
                    if (x !== -1) {
                        th = document.createElement("th");
                        th.className = "pvtRowLabel";
                        th.textContent = txt;
                        th.setAttribute("rowspan", x);
                        if (parseInt(j) === rowAttrs.length - 1 && colAttrs.length !== 0) {
                            th.setAttribute("colspan", 2);
                        }
                        tr.appendChild(th);
                    }
                }
                for (j in colKeys) {
                    if (!hasProp.call(colKeys, j)) continue;
                    colKey = colKeys[j];
                    aggregator = pivotData.getAggregator(rowKey, colKey);
                    val = aggregator.value();
                    td = document.createElement("td");
                    td.className = "pvtVal row" + i + " col" + j;
                    td.textContent = aggregator.format(val);
                    td.setAttribute("data-value", val);
                    tr.appendChild(td);
                }
                totalAggregator = pivotData.getAggregator(rowKey, []);
                val = totalAggregator.value();
                td = document.createElement("td");
                td.className = "pvtTotal rowTotal";
                td.textContent = totalAggregator.format(val);
                td.setAttribute("data-value", val);
                td.setAttribute("data-for", "row" + i);
                tr.appendChild(td);
                result.appendChild(tr);
            }
            tr = document.createElement("tr");
            th = document.createElement("th");
            th.className = "pvtTotalLabel";
            th.innerHTML = opts.localeStrings.totals;
            th.setAttribute("colspan", rowAttrs.length + (colAttrs.length === 0 ? 0 : 1));
            tr.appendChild(th);
            for (j in colKeys) {
                if (!hasProp.call(colKeys, j)) continue;
                colKey = colKeys[j];
                totalAggregator = pivotData.getAggregator([], colKey);
                val = totalAggregator.value();
                td = document.createElement("td");
                td.className = "pvtTotal colTotal";
                td.textContent = totalAggregator.format(val);
                td.setAttribute("data-value", val);
                td.setAttribute("data-for", "col" + j);
                tr.appendChild(td);
            }
            totalAggregator = pivotData.getAggregator([], []);
            val = totalAggregator.value();
            td = document.createElement("td");
            td.className = "pvtGrandTotal";
            td.textContent = totalAggregator.format(val);
            td.setAttribute("data-value", val);
            tr.appendChild(td);
            result.appendChild(tr);
            result.setAttribute("data-numrows", rowKeys.length);
            result.setAttribute("data-numcols", colKeys.length);
            return result;
        };
        PivotData = $.pivotUtilities.PivotData;
        locales = {
            en: {
                aggregators: aggregators,
                renderers: renderers,
                localeStrings: {
                    renderError: "An error occurred rendering the PivotTable results.",
                    computeError: "An error occurred computing the PivotTable results.",
                    uiRenderError: "An error occurred rendering the PivotTable UI.",
                    selectAll: "Select All",
                    selectNone: "Select None",
                    tooMany: "(too many to list)",
                    filterResults: "Filter results",
                    totals: "Totals",
                    vs: "vs",
                    by: "by"
                }
            }
        };
        mthNamesEn = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        dayNamesEn = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        zeroPad = function (number) {
            return ("0" + number).substr(-2, 2);
        };
        derivers = {
            bin: function (col, binWidth) {
                return function (record) {
                    return record[col] - record[col] % binWidth;
                };
            },
            dateFormat: function (col, formatString, utcOutput, mthNames, dayNames) {
                var utc;
                if (utcOutput == null) {
                    utcOutput = false;
                }
                if (mthNames == null) {
                    mthNames = mthNamesEn;
                }
                if (dayNames == null) {
                    dayNames = dayNamesEn;
                }
                utc = utcOutput ? "UTC" : "";
                return function (record) {
                    var date;
                    date = new Date(Date.parse(record[col]));
                    if (isNaN(date)) {
                        return "";
                    }
                    return formatString.replace(/%(.)/g, function (m, p) {
                        switch (p) {
                            case "y":
                                return date["get" + utc + "FullYear"]();
                            case "m":
                                return zeroPad(date["get" + utc + "Month"]() + 1);
                            case "n":
                                return mthNames[date["get" + utc + "Month"]()];
                            case "d":
                                return zeroPad(date["get" + utc + "Date"]());
                            case "w":
                                return dayNames[date["get" + utc + "Day"]()];
                            case "x":
                                return date["get" + utc + "Day"]();
                            case "H":
                                return zeroPad(date["get" + utc + "Hours"]());
                            case "M":
                                return zeroPad(date["get" + utc + "Minutes"]());
                            case "S":
                                return zeroPad(date["get" + utc + "Seconds"]());
                            default:
                                return "%" + p;
                        }
                    });
                };
            }
        };
        naturalSort = (function (_this) {
            return function (as, bs) {
                var a, a1, b, b1, rd, rx, rz;
                rx = /(\d+)|(\D+)/g;
                rd = /\d/;
                rz = /^0/;
                if (typeof as === "number" || typeof bs === "number") {
                    if (isNaN(as)) {
                        return 1;
                    }
                    if (isNaN(bs)) {
                        return -1;
                    }
                    return as - bs;
                }
                a = String(as).toLowerCase();
                b = String(bs).toLowerCase();
                if (a === b) {
                    return 0;
                }
                if (!(rd.test(a) && rd.test(b))) {
                    return (a > b ? 1 : -1);
                }
                a = a.match(rx);
                b = b.match(rx);
                while (a.length && b.length) {
                    a1 = a.shift();
                    b1 = b.shift();
                    if (a1 !== b1) {
                        if (rd.test(a1) && rd.test(b1)) {
                            return a1.replace(rz, ".0") - b1.replace(rz, ".0");
                        } else {
                            return (a1 > b1 ? 1 : -1);
                        }
                    }
                }
                return a.length - b.length;
            };
        })(this);
        sortAs = function (order) {
            var i, mapping, x;
            mapping = {};
            for (i in order) {
                x = order[i];
                mapping[x] = i;
            }
            return function (a, b) {
                if ((mapping[a] != null) && (mapping[b] != null)) {
                    return mapping[a] - mapping[b];
                } else if (mapping[a] != null) {
                    return -1;
                } else if (mapping[b] != null) {
                    return 1;
                } else {
                    return naturalSort(a, b);
                }
            };
        };
        getSort = function (sorters, attr) {
            var sort;
            sort = sorters(attr);
            if ($.isFunction(sort)) {
                return sort;
            } else {
                return naturalSort;
            }
        };


        //end

        $(jq[0]).pivotUI(showData, {
            renderers: renderers,
            aggregators: aggregators,
            cols: columns, rows: rows
        });
    },
    getWhereItem: function (jq, op) {
        if (jq.length > 0) {
            var options = op ? op : getInfolightOption($(jq[0]));
            if (options.whereItems != undefined) {
                var where = '';
                var wheremethods = {};
                //var hasRemote = false;
                for (var i = 0; i < options.whereItems.length; i++) {
                    var field = options.whereItems[i].field;
                    var value = options.whereItems[i].value;
                    var condition = options.whereItems[i].condition;
                    var realvalue = "";
                    if (condition == undefined)
                        condition = "=";

                    if (value.indexOf("client[") == 0) {
                        var methodName = value.replace("client[", "").replace("]", "");
                        realvalue = eval(methodName).call();
                    }
                    else if (value.indexOf("remote[") == 0) {
                        wheremethods[field] = value.replace("remote[", "").replace("]", "");
                        $.ajax({
                            type: "POST",
                            url: window.currentUrl,
                            data: "mode=default&method=" + $.toJSONString(wheremethods),
                            cache: false,
                            async: false,
                            success: function (data) {
                                realvalue = data;
                            }, error: function () {
                                realvalue = null;
                            }
                        });
                    }
                    else {
                        realvalue = value;
                    }
                    if (where != "") where += " and ";
                    if (condition == "%") {
                        where += field + " like " + "'%" + realvalue + "'";
                    }
                    else if (condition == "%%") {
                        where += field + " like " + "'%" + realvalue + "%'";
                    }
                    else {
                        where += field + " " + condition + " '" + realvalue + "'";
                    }
                }
                //if (hasRemote) {
                //    var whereObjs = getDefault($.toJSONString(wheremethods));
                //    var whereObj = $.parseJSON(whereObjs);
                //    for (var property in whereObj) {
                //        whereObject[property] = whereObj[property];
                //    }
                //}
                return where;
            }
        }
    },
    setWhere: function (jq, where) {
        jq.each(function () {
            $(this).pivotTable('options').whereString = where;
            var options = $(this).pivotTable('options');
            $(this).pivotTable('loadData');
            $(this).pivotTable('renderDIV', options);

        });
    },
    exportData: function (jq, options) {
        jq.each(function () {
            var where = $(this).pivotTable('options').whereString;
            if (where == undefined) where = "";
            var queryWord = {whereString: where};
            var options = $(this).pivotTable('options');
            $(this).pivotTable('loadData');
            if (options.remoteName != undefined && options.remoteName != "" && options.tableName != undefined) {
                var columns = [];
                for (var i = 0; i < options.showColumns.length; i++) {
                    columns.push({field: options.showColumns[i].field, title: options.showColumns[i].caption});
                }
                $.ajax({
                    type: "POST",
                    url: getDataUrl(),
                    //data: { mode: 'export', columns: $.toJSONString(columns), queryWord: where },
                    data: getRemoteParam({
                        mode: 'export',
                        columns: $.toJSONString(columns),
                        queryWord: $.toJSONString(queryWord)
                    }, options.remoteName, options.tableName, false),
                    cache: false,
                    async: false,
                    success: function (data) {
                        window.open(getParentFolder() + '../handler/JqFileHandler.ashx?File=' + data, 'download');
                    }
                });
            }
        });
    }
};