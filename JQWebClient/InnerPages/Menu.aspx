<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../js/themes/default/easyui.css" rel="stylesheet" />
    <link href="../js/themes/icon.css" rel="stylesheet" />
    <script src="../js/jquery-1.8.0.min.js"></script>
    <script src="../js/jquery.easyui.min.js"></script>
    <script src="../js/jquery.json.js" type="text/javascript"></script>
    <script src="../js/jquery.infolight.js"></script>
    <script src="Menu.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            renderMenu();

        });

        function afterSelectPackage() {
            var fileName = $("#File1")[0].value;
            if (fileName != undefined && fileName != "") {
                if (fileName.indexOf("C:\\fakepath\\") != -1) {
                    fileName = fileName.replace("C:\\fakepath\\", "");
                    $("#vbFormName_Menu").val(fileName.split('.')[0]);
                }
                else {
                    var fileNames = fileName.split("\\");
                    $("#vbPackage_Menu").val(fileNames[fileNames.length - 2]);
                    $("#vbFormName_Menu").val(fileNames[fileNames.length - 1].split('.')[0]);

                }
            }
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 88px;
        }
    </style>
</head>
<body style="height: 400px">
    <div class="easyui-layout" data-options="fit:true">
        <div data-options="region:'west',split:false,border:true" style="width: 240px; padding: 5px" style="overflow: scroll">
            <div class="easyui-layout" data-options="fit:true">
                <div data-options="region:'north',split:false,border:false" style="height: 60px; padding: 5px">
                    <select id="cbSolution_Menu" class="easyui-combobox" data-options="valueField:'value',textField:'text'" name="state" style="width: 210px">
                    </select>
                    <select id="cbLanguage_Menu" class="easyui-combobox" data-options="editable:false" name="state" style="width: 210px">
                        <option value="Default">Default</option>
                        <option value="English">English</option>
                        <option value="Traditional Chinese">Traditional Chinese</option>
                        <option value="Simplified Chinese">Simplified Chinese</option>
                        <option value="HongKong">HongKong</option>
                        <option value="Japanese">Japanese</option>
                        <option value="Korean">Korean</option>
                        <option value="User-defined1">User-defined1</option>
                        <option value="User-defined2">User-defined2</option>
                    </select>
                </div>
                <div data-options="region:'south',split:false,border:false" style="height: 30px; padding: 5px">
                    <a id="btnAccessUsers_Menu" href="#" class="easyui-linkbutton" data-options="plain:false" onclick="btnAccessUsers_MenuClick()">Access Users</a>
                    <a id="btnAccessGroups_Menu" href="#" class="easyui-linkbutton" data-options="plain:false" onclick="btnAccessGroups_MenuClick()">Access Groups</a>
                </div>
                <div data-options="region:'center',split:false,border:false" style="padding: 5px">
                    <ul id="treeMenus_Menu" class="easyui-tree" style="height: 300px;">
                    </ul>
                </div>
            </div>
        </div>
        <div data-options="region:'center',border:true" style="padding: 5px">
            <table style="border-collapse: separate; border-spacing: 11px;" aria-disabled="true">
                <tr>
                    <td style="line-height: 2em" class="auto-style1">Menu ID</td>
                    <td>
                        <input id="vbMenuId_Menu" class="easyui-validatebox" data-options="required:true" style="width: 300px;" disabled="disabled" /></td>
                </tr>
                <tr>
                    <td class="auto-style1">Caption</td>
                    <td>
                        <input id="vbCaption_Menu" class="easyui-validatebox dis" style="width: 300px;" /></td>
                </tr>
                <tr>
                    <td class="auto-style1">Parent ID</td>
                    <td>
                        <input id="vbParentId_Menu" class="easyui-validatebox dis" style="width: 300px;" /></td>
                </tr>
                <tr>
                    <td class="auto-style1">Module Type</td>
                    <td>
                        <select id="cbMeduleType_Menu" class="easyui-combobox" data-options="editable:false" name="state" style="width: 300px;">
                            <option value="J">JQueryWebForm</option>
                            <option value="M">JQueryMobileForm</option>
                        </select></td>
                </tr>
                <tr>
                    <td class="auto-style1">Image Url</td>
                    <td>
                        <input id="vbImageUrl_Menu" class="easyui-validatebox dis" style="width: 220px;" />
                        <JQTools:JQFileUpload ID="fImageUrl_Menu" runat="server" Width="130" Filter="" UpLoadFolder="Image/MenuTree" ShowButton="False" ShowLocalFile="True" />
                        <%--<input id="fImageUrl_Menu" type="file" onselect="fImageUrl_MenuChange()" style="width: 220px;" />--%>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Package</td>
                    <td>
                        <input id="vbPackage_Menu" class="easyui-validatebox dis" style="width: 220px;" />
                        <input id="File1" type="file" onchange="afterSelectPackage();" style="width: 170px" />
                        <%--<a id="btnPackage_Menu" href="#" class="easyui-linkbutton" data-options="plain:false" onclick="btnPackage_MenuClick()">...</a>--%>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Item Params</td>
                    <td>
                        <input id="vbItemParams_Menu" class="easyui-validatebox dis" style="width: 300px;" /></td>
                </tr>
                <tr>
                    <td class="auto-style1">Form Name</td>
                    <td>
                        <input id="vbFormName_Menu" class="easyui-validatebox dis" style="width: 300px;" /></td>
                </tr>
                <tr>
                    <td class="auto-style1">Solution</td>
                    <td>
                        <input id="vbSolution_Menu" class="easyui-validatebox" style="width: 300px;" disabled="disabled" /></td>
                </tr>
                <tr>
                    <td class="auto-style1">Sequence</td>
                    <td>
                        <input id="vbSequence_Menu" class="easyui-validatebox dis" style="width: 300px;" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <a id="btnAdd_Menu" href="#" class="easyui-linkbutton" data-options="plain:false" onclick="btnAdd_MenuClick()">Add</a>
                        <a id="btnModify_Menu" href="#" class="easyui-linkbutton" data-options="plain:false" onclick="btnModify_MenuClick()">Modify</a>
                        <a id="btnDelete_Menu" href="#" class="easyui-linkbutton" data-options="plain:false" onclick="btnDelete_MenuClick()">Delete</a>
                        <a id="btnOk_Menu" href="#" class="easyui-linkbutton" data-options="plain:false" onclick="btnOk_MenuClick()">OK</a>
                        <a id="btnCancel_Menu" href="#" class="easyui-linkbutton" data-options="plain:false" onclick="btnCancel_MenuClick()">Cancel</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="winAccessGroup"></div>
    <div id="winAccessUser"></div>
    <div id="winSelectPage"></div>
    <div id="winAccessControl"></div>
</body>
</html>
