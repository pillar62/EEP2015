<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JQTest.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type ="text/javascript">
        //
        function Import() {
            var ext = $("#FileUpload1").val().split(".").pop().toLowerCase();
            //if ($.inArray(ext, ["xlsx"]) == -1) {
            //    alert('請上傳EXCEL2007版本以上格式的文件!');
            //    document.getElementById('FileUpload1').outerHTML = document.getElementById('FileUpload1').outerHTML;
            //    return false;
            //}
            //var CustomerNo = $("#JQRefval1").refval("getValue");
            //if (CustomerNo == "") {
            //    alert('請選擇客戶');
            //    return;
            //}
            $.ajaxFileUpload({
                url: "../handler/xlsxHandler.ashx?", //需要链接到服务器地址   
                secureuri: false,
                data: {
                    mode: "import",
                    filename: $("#FileUpload1").val(),
                    customerno: $('#dataFormMasterTestID').val()
                },
                fileElementId: "FileUpload1", //文件选择框的id属性   
                dataType: 'json', //服务器返回的格式，可以是json   
                success: function (data) {
                    if (typeof data == "string") {
                        var message = data;
                        $.messager.alert('Error', message, 'error');
                    }
                    else {
                        var message = data.SuccessMsg;
                        alert(message);
                    }
                },
                error: function (data) {
                    if (typeof data == "string") {
                        var message = data;
                        $.messager.alert('Error', message, 'error');
                    }
                }
            });
        }
        //
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
                    data2 = data;
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
            }
        });
        // Danny end
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sTest.TestMaster" runat="server" AutoApply="True"
                DataMember="TestMaster" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="JQTest">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="TestID" Editor="text" FieldName="TestID" Format="" MaxLength="50" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="TestName" Editor="text" FieldName="TestName" Format="" MaxLength="50" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="TestMark" Editor="checkbox" FieldName="TestMark" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="FlowFlag" Editor="text" FieldName="FlowFlag" Format="" MaxLength="1" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶編號" Editor="text" FieldName="CustomerID" Format="" MaxLength="50" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply"
                        Text="存檔" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel"
                        Text="取消" />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="JQTest" DialogLeft="0px" DialogTop="0px" Width="800px">
                <table style="width:100%;">
                    <tr>
                        <td>
                            <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="TestMaster" HorizontalColumnsCount="2" RemoteName="sTest.TestMaster" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0">
                                <Columns>
                                    <JQTools:JQFormColumn Alignment="left" Caption="TestID" Editor="text" FieldName="TestID" Format="" maxlength="50" Width="180" />
                                    <JQTools:JQFormColumn Alignment="left" Caption="TestName" Editor="text" FieldName="TestName" Format="" maxlength="50" Width="180" />
                                </Columns>
                            </JQTools:JQDataForm>
                        </td>
                        <td>
                            <input id="FileUpload1" name="FileUpload1" type="file" />
                            <input id="Button1" type="button" onclick="Import()" value="上傳" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <JQTools:JQDataGrid ID="dataGridDetail" runat="server" AutoApply="False" DataMember="TestDetail" EditDialogID="JQDialog2" Pagination="False" ParentObjectID="dataFormMaster" RemoteName="sTest.TestMaster" Title="明細資料">
                                <Columns>
                                    <JQTools:JQGridColumn Alignment="left" Caption="TestID" Editor="text" FieldName="TestID" Format="" Width="120" />
                                    <JQTools:JQGridColumn Alignment="left" Caption="TestSeq" Editor="text" FieldName="TestSeq" Format="" Width="120" />
                                    <JQTools:JQGridColumn Alignment="left" Caption="TestTry" Editor="text" FieldName="TestTry" Format="" Width="120" />
                                    <JQTools:JQGridColumn Alignment="left" Caption="CustomerID" Editor="text" FieldName="CustomerID" Format="" Width="120" />
                                </Columns>
                                <RelationColumns>
                                    <JQTools:JQRelationColumn FieldName="TestID" ParentFieldName="TestID" />
                                </RelationColumns>
                                <TooItems>
                                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="更改" />
                                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" />
                                </TooItems>
                            </JQTools:JQDataGrid>
                        </td>
                    </tr>
                </table>

                <JQTools:JQDialog ID="JQDialog2" runat="server" BindingObjectID="dataFormDetail">
                    <JQTools:JQDataForm ID="dataFormDetail" runat="server" ParentObjectID="dataFormMaster" DataMember="TestDetail" HorizontalColumnsCount="2" RemoteName="sTest.TestMaster" >
                        <Columns>
                            <JQTools:JQFormColumn Alignment="left" Caption="TestID" Editor="text" FieldName="TestID" Format="" Width="120" />
                            <JQTools:JQFormColumn Alignment="left" Caption="TestSeq" Editor="text" FieldName="TestSeq" Format="" Width="120" />
                            <JQTools:JQFormColumn Alignment="left" Caption="TestTry" Editor="text" FieldName="TestTry" Format="" Width="120" />
                            <JQTools:JQFormColumn Alignment="left" Caption="CustomerID" Editor="text" FieldName="CustomerID" Format="" Width="120" />
                        </Columns>
                        <RelationColumns>
                            <JQTools:JQRelationColumn FieldName="TestID" ParentFieldName="TestID" />
                        </RelationColumns>
                    </JQTools:JQDataForm>
                </JQTools:JQDialog>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
                <JQTools:JQDefault ID="defaultDetail" runat="server" BindingObjectID="dataFormDetail" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateDetail" runat="server" BindingObjectID="dataFormDetail" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
</html>
