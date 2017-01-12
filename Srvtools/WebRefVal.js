function _refChangeField(ctrls, autoUpperCase, checkData, queryConfig) {
    var ddl = document.getElementById(ctrls['ddl']);
    var val = document.getElementById(ctrls['val']);
    var txt = document.getElementById(ctrls['txt']);
    var show = document.getElementById(ctrls['show']);
    var getVal = false;
    for (var j = 0; j < ddl.length; j++) {
        var showValue = autoUpperCase ? show.value.toUpperCase() : show.value;
        if (ddl.item(j) != null && ddl.item(j).value == showValue) {
            txt.value = ddl.item(j).text;
            val.value = showValue;
            getVal = true;
            break;
        }
    }
    if (queryConfig && queryConfig.onAfterChange) {
        if (txt.value != show.value) {
            val.value = show.value
        }

        queryConfig.onAfterChange(show.value, getVal);
    }
    if (!checkData && !getVal) {
        val.value = txt.value = show.value;
    }
}

function _refMatch(ctrls, value) {
    var ddl = document.getElementById(ctrls['ddl']);
    var show = document.getElementById(ctrls['show']);
    show.value = value;
//    for (var j = 0; j < ddl.length; j++) {
//        if (ddl.item(j) != null && ddl.item(j).value == value) {
//            show.value = ddl.item(j).text;
//            break;
//        }
//    }
}

function refvalQuery(config) {
    $.ajax({
        type: 'post',
        url: config.requestUrl,
        data: {
            id: config.id,
            valuefield: config.valuefield,
            displayfield: config.displayfield,
            sql: config.sql
        },
        dataType: 'json',
        success: config.callbackMethod
    });
}

function resetCombo(data) {
    var cmb = document.getElementById(data.id);
    if (cmb) {
        var selValue = cmb.value;
        cmb.options.length = 0;
        for (var i = 0; i < data.data.length; i++) {
            var option = document.createElement('option');
            cmb.options.add(option);
            option.value = data.data[i][data.valuefield];
            option.innerText = data.data[i][data.displayfield];
            if (data.data[i].posid == selValue) {
                option.selected = 'selected';
            }
        }
    }
}

function _refFocus(ctrls) {
    var val = document.getElementById(ctrls['val']);
    var show = document.getElementById(ctrls['show']);

    show.value = val.value;
    show.select();
}

function _refReceiveServerData(arg, context) {
    var ddl = document.getElementById(context['ddl']);
    var val = document.getElementById(context['val']);
    var txt = document.getElementById(context['txt']);
    var show = document.getElementById(context['show']);
    var autoUpperCase = context['autoUpperCase'];
    var checkData = context['checkData'];

    var valFound = false;
    var argValue = arg.substring(0, arg.indexOf(';'));
    var argMatch = arg.substring(arg.indexOf(';') + 1);
    for (var j = 0; j < ddl.length; j++) {
        var showValue = autoUpperCase ? show.value.toUpperCase() : show.value;
        if (ddl.item(j) != null && ddl.item(j).value == showValue) {
            show.value = ddl.item(j).text;
            val.value = showValue;
            txt.value = ddl.item(j).text;
            valFound = true;
            eval(argMatch);
            break;
        }
    }
    var popupTable = document.getElementById('popupTable');
    if (popupTable) {
        popupTable.style.display = 'none';
    }
    if (!valFound) {
        if (argValue !== '') {
            val.value = show.value;
            txt.value = show.value = argValue;
            eval(argMatch);
        }
        else {
            if (checkData) {
                var checkvalue = show.value;
                if (!checkvalue.endWith('?') && checkvalue.trim() !== '') {
                    alertMessage(context['ref'], context['msg']);
                    show.focus();
                }
            }
            else {
                val.value = txt.value = show.value;
            }
        }
    }
}

String.prototype.endWith = function(s) {
    return this.substring(this.length - 1) == s;
}

String.prototype.trim = function() {
    var re = /^\s+|\s+$/g;
    return function() { return this.replace(re, ""); };
} ();

function alertMessage(refCtrl, message) {
    var alertTable = document.getElementById(refCtrl).parentElement;
    var popupTableBody = document.createElement('tbody');
    var popupTableRow = document.createElement('tr');
    var calloutCell = document.createElement('td');
    var calloutTable = document.createElement('table');
    var calloutTableBody = document.createElement('tbody');
    var calloutTableRow = document.createElement('tr');
    var iconCell = document.createElement('td');
    var closeCell = document.createElement('td');
    var popupTable = document.createElement('table');
    var calloutArrowCell = document.createElement('td');
    var warningIconImage = document.createElement('img');
    var closeImage = document.createElement('img');
    var errorMessageCell = document.createElement('td');
    // popupTable
    popupTable.id = 'popupTable';
    popupTable.style.zIndex = '2';
    popupTable.style.position = 'absolute';
    popupTable.cellPadding = 0;
    popupTable.cellSpacing = 0;
    popupTable.border = 0;
    popupTable.width = '100px';
    // popupTableRow
    popupTableRow.vAlign = 'top';
    popupTableRow.style.height = '100%';
    // calloutCell
    calloutCell.width = '20px';
    calloutCell.align = 'right';
    calloutCell.style.height = '100%';
    calloutCell.style.verticalAlign = 'top';
    // calloutTable
    calloutTable.cellPadding = 0;
    calloutTable.cellSpacing = 0;
    calloutTable.border = 0;
    calloutTable.style.height = '100%';
    // _calloutArrowCell
    calloutArrowCell.align = 'right';
    calloutArrowCell.vAlign = 'top';
    calloutArrowCell.style.fontSize = '1px';
    calloutArrowCell.style.paddingTop = '8px';
    // iconCell
    iconCell.width = '20px';
    iconCell.style.borderTop = '1px solid black';
    iconCell.style.borderLeft = '1px solid black';
    iconCell.style.borderBottom = '1px solid black';
    iconCell.style.padding = '5px';
    iconCell.style.backgroundColor = 'LemonChiffon';
    // warningIconImage
    warningIconImage.border = 0;
    warningIconImage.src = '../Image/Ajax/alert-large.gif';
    // errorMessageCell
    errorMessageCell.style.backgroundColor = 'LemonChiffon';
    errorMessageCell.style.fontFamily = 'verdana';
    errorMessageCell.style.fontSize = '10px';
    errorMessageCell.style.padding = '5px';
    errorMessageCell.style.borderTop = '1px solid black';
    errorMessageCell.style.borderBottom = '1px solid black';
    errorMessageCell.width = '100%';
    errorMessageCell.style.color = 'blue';
    errorMessageCell.innerHTML = message;
    // closeCell
    closeCell.style.borderTop = '1px solid black';
    closeCell.style.borderRight = '1px solid black';
    closeCell.style.borderBottom = '1px solid black';
    closeCell.style.backgroundColor = 'lemonchiffon';
    closeCell.style.verticalAlign = 'top';
    closeCell.style.textAlign = 'right';
    closeCell.style.padding = '2px';
    // closeImage
    closeImage.src = '../Image/Ajax/close.gif';
    closeImage.style.cursor = 'pointer';
    if (closeImage.addEventListener) {
        closeImage.addEventListener("onclick", function () { popupTable.style.display = 'none'; }, false);
    }
    else {
        closeImage.attachEvent("onclick", function () { popupTable.style.display = 'none'; });
    }
    
    if (alertTable.childNodes['popupTable'] != undefined) {
        alertTable.removeChild(popupTable);
    }
    // Create the DOM tree
    alertTable.appendChild(popupTable);
    popupTable.appendChild(popupTableBody);
    popupTableBody.appendChild(popupTableRow);
    popupTableRow.appendChild(calloutCell);
    calloutCell.appendChild(calloutTable);
    calloutTable.appendChild(calloutTableBody);
    calloutTableBody.appendChild(calloutTableRow);
    calloutTableRow.appendChild(calloutArrowCell);
    popupTableRow.appendChild(iconCell);
    iconCell.appendChild(warningIconImage);
    popupTableRow.appendChild(errorMessageCell);
    popupTableRow.appendChild(closeCell);
    closeCell.appendChild(closeImage);
    // initialize callout arrow
    var div = document.createElement('div');
    div.style.fontSize = '1px';
    div.style.position = 'relative';
    div.style.left = '1px';
    div.style.borderTop = '1px solid black';
    div.style.width = '15px';
    calloutArrowCell.appendChild(div);
    for (var i = 14; i > 0; i--) {
        var line = document.createElement('div');
        line.style.width = i.toString() + 'px';
        line.style.height = '1px';
        line.style.overflow = 'hidden';
        line.style.backgroundColor = 'LemonChiffon';
        line.style.borderLeft = '1px solid black';
        div.appendChild(line);
    }
}