function overtimeQueryLoaded(controlId, dataFormId, winId) {

    try {
        var flowUIText = $.sysmsg('getValue', 'Web/webClientMainFlow/QueryCaption');
        var flowUITexts = flowUIText.split(';');
        $('#btnSearch_FromFlowQuery')[0].firstChild.firstChild.innerHTML = flowUITexts[9];
        $('#btnClear_FromFlowQuery')[0].firstChild.firstChild.innerHTML = flowUITexts[10];

        $('#lSearch_SendToID')[0].innerHTML = flowUITexts[11];
        $('#lSearch_Form_Presentation_CT')[0].innerHTML = flowUITexts[3];

        $('#lSearch_UPDATE_WHOLE_TIME_Form')[0].innerHTML = flowUITexts[5];
        $('#lSearch_UPDATE_WHOLE_TIME_To')[0].innerHTML = flowUITexts[6];

        $('#btnClear_FromFlowQuery').click(function () {
            if ($('#btnClear_FromFlowQuery').attr("href") == "#") {
                $('#formFlowQuery').form("clear");
            }
        });

        $('#btnSearch_FromFlowQuery').click(function () {
            if ($('#btnSearch_FromFlowQuery').attr("href") == "#") {
                var filter = "";
                var Search_SendToID = $('#Search_SendToID').val();
                if (Search_SendToID != "") {
                    filter += "(SENDTO_ID like '%" + Search_SendToID + "%' OR SENDTO_ID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID LIKE '%" + Search_SendToID + "') ) and ";
                }
                var Search_Form_Presentation_CT = $('#Search_Form_Presentation_CT').val();
                if (Search_Form_Presentation_CT != "") {
                    filter += "FORM_PRESENT_CT like '%" + Search_Form_Presentation_CT.replace(/'/g, "''") + "%' and ";
                }
                var Search_UPDATE_WHOLE_TIME_Form = $('#Search_UPDATE_WHOLE_TIME_Form').datebox("getValue");
                if (Search_UPDATE_WHOLE_TIME_Form != "") {
                    var date = new Date(Search_UPDATE_WHOLE_TIME_Form);
                    var m = date.getMonth() + 1;
                    if (m.toString().length == 1)
                        m = "0" + m.toString();
                    var d = date.getDate();
                    if (d.toString().length == 1)
                        d = "0" + d.toString();
                    var sDate = date.getFullYear() + "-" + m + "-" + d;
                    filter += "UPDATE_DATE >= '" + sDate + "' and ";
                }
                var Search_UPDATE_WHOLE_TIME_To = $('#Search_UPDATE_WHOLE_TIME_To').datebox("getValue");
                if (Search_UPDATE_WHOLE_TIME_To != "") {
                    var date = new Date(Search_UPDATE_WHOLE_TIME_To);
                    var m = date.getMonth() + 1;
                    if (m.toString().length == 1)
                        m = "0" + m.toString();
                    var d = date.getDate();
                    if (d.toString().length == 1)
                        d = "0" + d.toString();
                    var sDate = date.getFullYear() + "-" + m + "-" + d;
                    filter += "UPDATE_DATE <= '" + sDate + "' and ";
                }
                filter = filter.substring(0, filter.lastIndexOf("and"));

                var type = "Delay";
                $('#' + winId).dialog('close');
                $('#' + controlId).datagrid('options').queryParams.Filter = encodeURI(filter);
                $('#' + controlId).datagrid('load');
            }
        });

        $('#btnClear_FromFlowQuery').click(function () {
            if ($('#btnClear_FromFlowQuery').attr("href") == "#") {
                $('#' + winId).dialog('close');
                //$("#formFlowQuery").form("clear");
            }
        });
    }
    catch (err) {
        //alert("System.XML Version Too Old");
    }
}