var fImageUrl_Menu = null;

function getClientInfo(callback) {
    $.ajax({
        type: 'post',
        dataType: 'json',
        url: '../handler/SystemHandler_Security.ashx?type=ClientInfo',
        async: true,
        cache: false,
        success: function (clientInfo) {
            if (callback) {
                callback(clientInfo);
            }
        }
    });
}

function renderMenu() {
    var columnTitle = $.sysmsg('getValue', 'EEPManager/frmSecurityMain/MenuCaption');
    var columnTitles = columnTitle.split(",");
    $('#lMenuId_Menu').text(columnTitles[0]);
    $('#lCaption_Menu').text(columnTitles[1]);
    $('#lParentId_Menu').text(columnTitles[2]);
    $('#lMeduleType_Menu').text(columnTitles[3]);
    $('#lImageUrl_Menu').text(columnTitles[4]);
    //$('#lPackage_Menu').text(columnTitles[5]);
    $('#lItemParams_Menu').text(columnTitles[6]);
    $('#lFormName_Menu').text(columnTitles[7]);
    $('#lSolution_Menu').text(columnTitles[8]);
    $('#lSequence_Menu').text(columnTitles[9]);

    var buttonCaption = $.sysmsg('getValue', 'Srvtools/InfoNavigator/NavText');
    var buttonCaptions = buttonCaption.split(";");
    $('#btnAdd_Menu').linkbutton({ text: buttonCaptions[4] });
    $('#btnModify_Menu').linkbutton({ text: buttonCaptions[6] });
    $('#btnDelete_Menu').linkbutton({ text: buttonCaptions[5] });
    $('#btnOk_Menu').linkbutton({ text: buttonCaptions[9] });
    $('#btnCancel_Menu').linkbutton({ text: buttonCaptions[10] });

    var accessCaption = $.sysmsg('getValue', 'EEPManager/frmSecurityMain/AccessCaption');
    var accessCaptions = accessCaption.split(",");
    $('#btnAccessUsers_Menu').linkbutton({ text: accessCaptions[0] });
    $('#btnAccessGroups_Menu').linkbutton({ text: accessCaptions[1] });


    var solutionId = "";
    $.ajax({
        type: "POST",
        url: "../handler/SystemHandler_Security.ashx?type=Solution",
        //data: params,
        cache: false,
        async: false,
        dataType: 'json',
        success: function (data) {
            //data = eval('(' + data + ')');
            $('#cbSolution_Menu').combobox("loadData", data);
            if (data.length > 0) {
                getClientInfo(function (clientInfo) {
                    $('#cbSolution_Menu').combobox('setValue', clientInfo.Solution);
                    solutionId = clientInfo.Solution;
                });
                //                $('#cbSolution_Menu').combobox("select", data[0].value);
                //                solutionId = data[0].value;
            }
        },
        error: function (data) {
            //data.responseText = '';
            //obj = "[{\"" + textField + "\":\"\"}]";
        }
    });

    $('#cbSolution_Menu').combobox({
        onSelect: function (record) {
            loadMenuTables(record.value);
        }
    });
    $('#cbLanguage_Menu').combobox({
        onSelect: function (record) {
            var selected = $('#treeMenus_Menu').tree("getSelected");
            switch (record.value) {
                case 'Default': $("#vbCaption_Menu").val(selected.attributes.CAPTION); break;
                case 'English': $("#vbCaption_Menu").val(selected.attributes.CAPTION0); break;
                case 'Traditional Chinese': $("#vbCaption_Menu").val(selected.attributes.CAPTION1); break;
                case 'Simplified Chinese': $("#vbCaption_Menu").val(selected.attributes.CAPTION2); break;
                case 'HongKong': $("#vbCaption_Menu").val(selected.attributes.CAPTION3); break;
                case 'Japanese': $("#vbCaption_Menu").val(selected.attributes.CAPTION4); break;
                case 'Korean': $("#vbCaption_Menu").val(selected.attributes.CAPTION5); break;
                case 'User-defined1': $("#vbCaption_Menu").val(selected.attributes.CAPTION6); break;
                case 'User-defined2': $("#vbCaption_Menu").val(selected.attributes.CAPTION7); break;
            }
        }
    });
    $.messager.progress({ msg: 'loading menus ...' });
    $('#treeMenus_Menu').tree({
        //url: '../handler/SystemHandler_Security.ashx?type=MenuTables',
        //data: { mode: "Select", solutionid: $('#cbSolution_Menu').combobox('getText') },
        onLoadSuccess: function () {
            $.messager.progress('close');
        },
        onLoadError: function () {
            $.messager.progress('close');
        },
        loadFilter: function (res) {
            var dataSource = [];
            var count = res.length;
            for (var i = 0; i < count; i++) {
                createTreeNode(undefined, res[i], dataSource);
            }
            return dataSource;
        },
        onSelect: function (node) {
            setMenuDetails(node);
        },
        onBeforeSelect: function (node) {
            var oldSelected = $('#treeMenus_Menu').tree("getSelected");
            if (oldSelected != null) {
                getMenuDetails(oldSelected);
            }
        }
    });
    function createTreeNode(parent, data, dataSource) {
        if (data.MENUID != null) {
            var treeNode = {};
            treeNode.id = data.MENUID;
            if (data.IMAGEURL != null) {
                if ($(document).attr('menunoicon') == 'true')
                { }
                else
                {
                    treeNode.iconCls = 'menuicon-' + data.IMAGEURL.replace(/\./g, '');
                }
            }
            treeNode.text = data.CAPTION;//getMenuCaption(data);
            treeNode.attributes = {};
            treeNode.attributes.CAPTION = data.CAPTION;
            treeNode.attributes.CAPTION0 = data.CAPTION0;
            treeNode.attributes.CAPTION1 = data.CAPTION1;
            treeNode.attributes.CAPTION2 = data.CAPTION2;
            treeNode.attributes.CAPTION3 = data.CAPTION3;
            treeNode.attributes.CAPTION4 = data.CAPTION4;
            treeNode.attributes.CAPTION5 = data.CAPTION5;
            treeNode.attributes.CAPTION6 = data.CAPTION6;
            treeNode.attributes.CAPTION7 = data.CAPTION7;
            if (parent == undefined) {
                treeNode.attributes.Level = 1;
            } else {
                treeNode.attributes.Level = parent.attributes.Level + 1;
            }
            treeNode.attributes.FORM = data.FORM;
            treeNode.attributes.ITEMPARAM = decodeURI(data.ITEMPARAM);
            treeNode.attributes.ITEMTYPE = data.ITEMTYPE;
            treeNode.attributes.PACKAGE = data.PACKAGE;
            treeNode.attributes.MODULETYPE = data.MODULETYPE;
            treeNode.attributes.PARENT = data.PARENT;
            treeNode.attributes.SEQ_NO = data.SEQ_NO;
            treeNode.attributes.IMAGEURL = data.IMAGEURL;
            treeNode.children = [];
            treeNode.attributes.children = [];
            if (parent == undefined) {
                //if (data.PACKAGE != undefined && data.PACKAGE != '')
                //    dataSource.push(treeNode);
                //else if (data.MENUTABLE1 != undefined && data.MENUTABLE1.length > 0)
                dataSource.push(treeNode);
            } else {
                parent.children.push(treeNode);
                parent.attributes.children.push(treeNode);
            }
            if (data.MENUTABLE1 != undefined && data.MENUTABLE1.length > 0) {
                for (var i = 0; i < data.MENUTABLE1.length; i++) {
                    createTreeNode(treeNode, data.MENUTABLE1[i]);
                }
            }
        }
    }
    disableAll();
    loadMenuTables(solutionId);

    fImageUrl_Menu = $('#fImageUrl_Menu');
    fImageUrl_Menu.next().remove()
    initInfoFileUpload(fImageUrl_Menu);
}
function loadMenuTables(solutionId) {
    var sId = "";
    if (solutionId != undefined) {
        sId = solutionId;
    }
    else {
        sId = $('#cbSolution_Menu').combobox('getValue').split("@")[0];
    }
    $.ajax({
        type: "POST",
        url: '../handler/SystemHandler_Security.ashx?type=MenuTables',
        data: { mode: "Select", solutionid: sId },
        cache: false,
        async: false,
        dataType: 'json',
        success: function (data) {
            //data = eval('(' + data + ')');
            $('#treeMenus_Menu').tree("loadData", data);
        },
        error: function (data) {
        }
    });
}

function setMenuDetails(node) {
    //$('#cbLanguage_Menu').combobox("setValue", "Default");
    var lanIndex = $('#cbLanguage_Menu').combobox('getValue');
    var text = node.text;
    switch (lanIndex) {
        case 'Default': text = node.attributes.CAPTION; break;
        case 'English': text = node.attributes.CAPTION0; break;
        case 'Traditional Chinese': text = node.attributes.CAPTION1; break;
        case 'Simplified Chinese': text = node.attributes.CAPTION2; break;
        case 'HongKong': text = node.attributes.CAPTION3; break;
        case 'Japanese': text = node.attributes.CAPTION4; break;
        case 'Korean': text = node.attributes.CAPTION5; break;
        case 'User-defined1': text = node.attributes.CAPTION6; break;
        case 'User-defined2': text = node.attributes.CAPTION7; break;
    }

    if (node != undefined) {
        $("#vbMenuId_Menu").val(node.id);
        $("#vbCaption_Menu").val(text);
        $("#vbParentId_Menu").val(node.attributes.PARENT);
        $("#cbMeduleType_Menu").combobox("setValue", node.attributes.MODULETYPE);
        $("#vbPackage_Menu").val(node.attributes.PACKAGE);
        $("#vbItemParams_Menu").val(node.attributes.ITEMPARAM);
        $("#vbFormName_Menu").val(node.attributes.FORM);
        $("#vbSolution_Menu").val(node.attributes.ITEMTYPE);
        $("#vbSequence_Menu").val(node.attributes.SEQ_NO);
        $("#vbImageUrl_Menu").val(node.attributes.IMAGEURL);
    }
    else {
        var selected = $('#treeMenus_Menu').tree("getSelected");
        if (selected == null) {
            selected = {};
            selected.id = "";
            selected.attributes = {};
            selected.attributes.MODULETYPE = "";
            selected.attributes.ITEMTYPE = "";
        }
        $("#vbMenuId_Menu").val("");
        $("#vbCaption_Menu").val("");
        $("#vbParentId_Menu").val(selected.id);
        if (selected.attributes.ITEMTYPE != selected.attributes.MODULETYPE)
            $("#cbMeduleType_Menu").combobox("setValue", selected.attributes.MODULETYPE);
        else
            $("#cbMeduleType_Menu").combobox("setValue", "J");
        $("#vbPackage_Menu").val("");
        $("#vbItemParams_Menu").val("");
        $("#vbFormName_Menu").val("");
        if (selected.attributes.ITEMTYPE != "")
            $("#vbSolution_Menu").val(selected.attributes.ITEMTYPE);
        else {
            var sId = $("#cbSolution_Menu").combobox("getValue").split("@")[0];
            $("#vbSolution_Menu").val(sId);
        }
        $("#vbSequence_Menu").val("");
        $("#vbImageUrl_Menu").val("");
    }
}
function getMenuDetails(node) {
    if ($("#vbMenuId_Menu").val() != "") {
        node.id = $("#vbMenuId_Menu").val();
        node.text = $("#vbCaption_Menu").val();
        node.attributes.PARENT = $("#vbParentId_Menu").val();
        node.attributes.MODULETYPE = $("#cbMeduleType_Menu").combobox("getValue");
        node.attributes.PACKAGE = $("#vbPackage_Menu").val();
        node.attributes.ITEMPARAM = $("#vbItemParams_Menu").val();
        node.attributes.FORM = $("#vbFormName_Menu").val();
        node.attributes.ITEMTYPE = $("#vbSolution_Menu").val();
        node.attributes.SEQ_NO = $("#vbSequence_Menu").val();
        node.attributes.IMAGEURL = $('#vbImageUrl_Menu').val();
    }
}

function btnAdd_MenuClick() {
    enableAll();
    setMenuDetails();
    $("#vbMenuId_Menu").attr("disabled", false);
    self.mode = "Add";
    $.ajax({
        type: "POST",
        url: '../handler/SystemHandler_Security.ashx?type=MenuTables',
        data: { mode: "AutoSeqMenuID" },
        cache: false,
        async: false,
        success: function (data) {
            $('#vbMenuId_Menu').val(data);
        },
        error: function (data) {
        }
    });
}
function btnModify_MenuClick() {
    enableAll();
    self.mode = "Modify";
}
function btnDelete_MenuClick() {
    if (confirm("Do you want to delete this menu?")) {
        disableAll();
        var node = $('#treeMenus_Menu').tree("getSelected");
        $("#vbMenuId_Menu").attr("disabled", true);
        var data = {};
        data.mode = "Delete";
        data.menuid = node.id;
        $.ajax({
            type: "POST",
            url: "../handler/SystemHandler_Security.ashx?type=MenuTables",
            data: data,
            cache: false,
            async: false,
            success: function (data) {
                $('#treeMenus_Menu').tree("remove", node.target);
                setMenuDetails();
            },
            error: function (data) {
            }
        });
    }
}
function btnOk_MenuClick() {
    disableAll();
    var lanIndex = $('#cbLanguage_Menu').combobox('getValue');
    var node = $('#treeMenus_Menu').tree("getSelected");
    if (node == null) {
        node = {};
        node.attributes = {};
    }
    getMenuDetails(node);
    $("#vbMenuId_Menu").attr("disabled", true);
    var data = {};
    data.mode = self.mode;
    data.menuid = node.id;
    data.caption = node.attributes.CAPTION;
    data.caption0 = node.attributes.CAPTION0;
    data.caption1 = node.attributes.CAPTION1;
    data.caption2 = node.attributes.CAPTION2;
    data.caption3 = node.attributes.CAPTION3;
    data.caption4 = node.attributes.CAPTION4;
    data.caption5 = node.attributes.CAPTION5;
    data.caption6 = node.attributes.CAPTION6;
    data.caption7 = node.attributes.CAPTION7;
    switch (lanIndex) {
        case 'Default': data.caption = node.attributes.CAPTION = node.text; break;
        case 'English': data.caption0 = node.attributes.CAPTION0 = node.text; break;
        case 'Traditional Chinese': data.caption1 = node.attributes.CAPTION1 = node.text; break;
        case 'Simplified Chinese': data.caption2 = node.attributes.CAPTION2 = node.text; break;
        case 'HongKong': data.caption3 = node.attributes.CAPTION3 = node.text; break;
        case 'Japanese': data.caption4 = node.attributes.CAPTION4 = node.text; break;
        case 'Korean': data.caption5 = node.attributes.CAPTION5 = node.text; break;
        case 'User-defined1': data.caption6 = node.attributes.CAPTION6 = node.text; break;
        case 'User-defined2': data.caption7 = node.attributes.CAPTION7 = node.text; break;
    }
    data.parent = node.attributes.PARENT;
    data.moduletype = node.attributes.MODULETYPE;
    data.package = node.attributes.PACKAGE;
    data.itemparam = node.attributes.ITEMPARAM;
    data.form = node.attributes.FORM;
    data.itemtype = node.attributes.ITEMTYPE;
    data.seq_no = node.attributes.SEQ_NO;
    var fileName = $('#infoFileUploadfImageUrl_Menu').val();
    if (fileName != undefined && fileName != "") {
        infoFileUploadMethod(fImageUrl_Menu, afterUpload);
        fileName = fileName.substring(fileName.lastIndexOf("\\") + 1);
        data.imageurl = fileName;
    }
    $.ajax({
        type: "POST",
        url: "../handler/SystemHandler_Security.ashx?type=MenuTables",
        data: data,
        cache: false,
        async: false,
        success: function () {
            if (self.mode == "Add") {
                loadMenuTables();
            }
            else if (self.mode == "Modify") {
                if (node) {
                    if ($('#cbLanguage_Menu').combobox("getValue") == "Default") {
                        $('#treeMenus_Menu').tree('update', {
                            target: node.target,
                            text: node.text
                        });
                    }
                }
            }
        },
        error: function (data) {
            //data.responseText = '';
            //obj = "[{\"" + textField + "\":\"\"}]";
        }
    });
}
function afterUpload(fileName) {
    var realFileName = fileName;
    //fileName = fileName.replace(/__/g, "&nbsp;");
    $("#vbImageUrl_Menu").val(realFileName);
}
function btnCancel_MenuClick() {
    var node = $('#treeMenus_Menu').tree("getSelected");
    setMenuDetails(node);
    disableAll();
    $("#vbMenuId_Menu").attr("disabled", true);
}
function disableAll() {
    $(".dis").each(function () {
        $(this).attr("disabled", true);
    });
    $("#cbMeduleType_Menu").combobox('disable');
    $("#btnPackage_Menu").linkbutton('disable');
    $("#btnOk_Menu").linkbutton('disable');
    $("#btnCancel_Menu").linkbutton('disable');

    $("#btnAdd_Menu").linkbutton('enable');
    $("#btnModify_Menu").linkbutton('enable');
    $("#btnDelete_Menu").linkbutton('enable');
}
function enableAll() {
    $(".dis").each(function () {
        $(this).attr("disabled", false);
    });
    $("#cbMeduleType_Menu").combobox('enable');
    $("#btnPackage_Menu").linkbutton('enable');
    $("#btnOk_Menu").linkbutton('enable');
    $("#btnCancel_Menu").linkbutton('enable');

    $("#btnAdd_Menu").linkbutton('disable');
    $("#btnModify_Menu").linkbutton('disable');
    $("#btnDelete_Menu").linkbutton('disable');
}
function fImageUrl_MenuChange(sender, e) {
    var v = "";
}
function btnPackage_MenuClick() {
    var buttons = [{
        text: 'Ok',
        handler: function () {
            var row = $("#gridPage_SelectPage").datagrid("getSelected");
            $("#vbFormName_Menu").val(row.PageName);
            $('#winSelectPage').dialog().dialog('close');
        }
    }, {
        text: 'Cancel',
        handler: function () {
            $('#winSelectPage').dialog({
                onClose: function () {
                }
            }).dialog('close');
        }
    }]
    createAndOpenWizardDialog("winSelectPage", "Select Page", "250", "350", "SelectPage.aspx", function () {
        $('#gridPage_SelectPage').datagrid({
            columns: [[
                        { field: 'PageName', title: 'Page Name', sortable: true, width: 80 }
            ]],
            fitColumns: false
        });
        var sId = $('#cbSolution_Menu').combobox('getValue').split("@")[0];
        loadDataForDataGrid("gridPage_SelectPage", "../handler/SystemHandler_Security.ashx?type=MenuTables", undefined, { mode: "Pages", solutionid: sId })
    }, buttons);
}

function btnAccessGroups_MenuClick() {
    var buttonCaption = $.sysmsg('getValue', 'Srvtools/InfoNavigator/NavText');
    var buttonCaptions = buttonCaption.split(";");

    var accessCaption = $.sysmsg('getValue', 'EEPManager/frmSecurityMain/AccessCaption');
    var accessCaptions = accessCaption.split(",");

    var node = $('#treeMenus_Menu').tree("getSelected");
    if (node == null) {
        alert("Please a menu first.");
    }
    else {
        var buttons = [{
            text: buttonCaptions[9],
            handler: function () {
                var groups = "";
                var rows = $("#gridGroupsTo_AccessGroup").datagrid("getData").rows;
                for (var i = 0; i < rows.length; i++) {
                    groups += rows[i].GROUPID + ";";
                }
                $.ajax({
                    type: "POST",
                    url: "../handler/SystemHandler_Security.ashx?type=MenuTables",
                    data: { mode: "InsertGroupMenus", menuid: node.id, groupids: groups },
                    cache: false,
                    async: false,
                    success: function () {
                        $('#winAccessGroup').dialog().dialog('close');
                    },
                    error: function (data) {
                        //data.responseText = '';
                        //obj = "[{\"" + textField + "\":\"\"}]";
                    }
                });
            }
        }, {
            text: buttonCaptions[10],
            handler: function () {
                $('#winAccessGroup').dialog({
                    onClose: function () {
                    }
                }).dialog('close');
            }
        }, {
            text: accessCaptions[2],
            handler: function () {
                var selectedGroup = $("#gridGroupsTo_AccessGroup").datagrid("getSelected");
                if (selectedGroup == null) {
                    alert("Please select a group first.");
                    return;
                }
                var title = "Group:(" + selectedGroup.GROUPID + ")" + selectedGroup.GROUPNAME + "/Menu:(" + node.id + ")" + node.text;
                createAndOpenWizardDialog("winAccessControl", title, "650", "350", "AccessControl.aspx", function () {
                    var editIndex = undefined;
                    var dataGrid = $('#dgAccessControl');

                    var dataGridColumnCaption = $.sysmsg('getValue', 'EEPManager/frmSecurityMain/AccessCaption');
                    var dataGridColumnCaptions = dataGridColumnCaption.split(",");

                    dataGrid.datagrid({
                        remoteSort: false,
                        pagination: false,
                        rownumbers: false,
                        singleSelect: true,
                        checkOnSelect: false,
                        frozenColumns: [[
                            { field: 'CONTROLNAME', title: dataGridColumnCaptions[3], width: 100 },
                            { field: 'DESCRIPTION', title: dataGridColumnCaptions[4], width: 150 }
                        ]],
                        columns: [[
                            { field: 'GROUPID', hidden: true },
                            { field: 'MENUID', title: 'MENUID', hidden: true },
                            { field: 'TYPE', title: 'TYPE', width: 100, hidden: true },
                            { field: 'ENABLED', title: dataGridColumnCaptions[5], width: 70, editor: { type: "checkbox", options: { on: 'Y', off: 'N' } } },
                            { field: 'VISIBLE', title: dataGridColumnCaptions[6], width: 70, editor: { type: "checkbox", options: { on: 'Y', off: 'N' } } },
                            { field: 'ALLOWADD', title: dataGridColumnCaptions[7], width: 70, editor: { type: "checkbox", options: { on: 'Y', off: 'N' } } },
                            { field: 'ALLOWUPDATE', title: dataGridColumnCaptions[8], width: 75, editor: { type: "checkbox", options: { on: 'Y', off: 'N' } } },
                            { field: 'ALLOWDELETE', title: dataGridColumnCaptions[9], width: 75, editor: { type: "checkbox", options: { on: 'Y', off: 'N' } } },
                            { field: 'ALLOWPRINT', title: dataGridColumnCaptions[10], width: 70, editor: { type: "checkbox", options: { on: 'Y', off: 'N' } } }
                        ]],
                        fitColumns: false,
                        onClickRow: function (rowIndex, rowData) {
                            if (editIndex != rowIndex) {
                                if (editIndex != undefined)
                                    dataGrid.datagrid('endEdit', editIndex);
                                dataGrid.datagrid('selectRow', rowIndex).datagrid('beginEdit', rowIndex);
                                editIndex = rowIndex;
                            }
                            dataGrid.datagrid('selectRow', rowIndex).datagrid('beginEdit', rowIndex);
                        }
                    });

                    loadDataForDataGrid("dgAccessControl", "../handler/SystemHandler_Security.ashx?type=GetGroupMenuControl", function (data) {
                        dataGrid.datagrid("loadData", data);
                    }, { groupId: selectedGroup.GROUPID, menuId: node.id })

                    $("#btnAdd", "#AccessControlMain").click(function () {
                        $.ajax({
                            type: "POST",
                            url: "../handler/SystemHandler_Security.ashx?type=GetSecurity",
                            data: { menuid: node.id },
                            cache: false,
                            async: false,
                            dataType: 'json',
                            success: function (data) {
                                //data = eval('(' + data + ')');
                                var rows = dataGrid.datagrid("getRows");
                                var appandRows = [];
                                for (var j = 0; j < data.length; j++) {
                                    var isExisted = false;
                                    for (var i = 0; i < rows.length; i++) {
                                        if (rows[i].CONTROLNAME == data[j].CONTROLNAME) {
                                            isExisted = true;
                                            break;
                                        }
                                    }
                                    if (isExisted == false) {
                                        appandRows.push({ GROUPID: selectedGroup.GROUPID, MENUID: node.id, CONTROLNAME: data[j].CONTROLNAME, TYPE: data[j].TYPE, DESCRIPTION: data[j].DESCRIPTION, ENABLED: "N", VISIBLE: "Y", ALLOWADD: "Y", ALLOWUPDATE: "Y", ALLOWDELETE: "Y", ALLOWPRINT: "Y" });
                                    }
                                }
                                for (var i = 0; i < appandRows.length; i++)
                                    dataGrid.datagrid("appendRow", appandRows[i]);
                            },
                            error: function (data) {
                                //data.responseText = '';
                                //obj = "[{\"" + textField + "\":\"\"}]";
                            }
                        });
                    });

                    $("#btnDelete", "#AccessControlMain").click(function () {
                        if (editIndex != undefined) {
                            dataGrid.datagrid('deleteRow', editIndex);
                            editIndex = undefined;
                        }
                    });

                    $("#btnSave", "#AccessControlMain").click(function () {
                        dataGrid.datagrid('acceptChanges');
                        var rows = new Object();
                        rows.inserted = dataGrid.datagrid('getData').rows;
                        if (rows.inserted != null) {
                            var htmlEncode = function (key, value) {
                                if (value && typeof value == 'string') {
                                    return value.replace(/</g, "&lt;").replace(/>/g, "&gt;");
                                }
                                else {
                                    return value;
                                }
                            };
                            $.ajax({
                                type: "POST",
                                url: '../handler/SystemHandler_Security.ashx?type=SaveGroupMenuControl',
                                data: { groupId: selectedGroup.GROUPID, menuId: node.id, rows: JSON.stringify(rows, htmlEncode) },
                                cache: false,
                                async: false,
                                success: function (data) {
                                    alert("Save successed.")
                                },
                                error: function (data) {
                                }
                            });
                        }
                    });

                    $("#btnCancel", "#AccessControlMain").click(function () {
                        $("#winAccessControl").dialog("close");
                    });
                });
            }
        }]
        createAndOpenWizardDialog("winAccessGroup", $('#btnAccessGroups_Menu').text(), "450", "350", "AccessGroup.aspx", function () {
            var titleCaption = $.sysmsg('getValue', 'Srvtools/UGControl/Caption_Group');
            var titleCaptions = titleCaption.split(";");
            $('#gridGroupsFrom_AccessGroup').datagrid({
                columns: [[
                            { field: 'GROUPID', title: titleCaptions[0], sortable: true, width: 80 },
                            { field: 'GROUPNAME', title: titleCaptions[1], sortable: true, width: 87 }
                ]],
                fitColumns: false
            });
            $('#gridGroupsTo_AccessGroup').datagrid({
                columns: [[
                            { field: 'GROUPID', title: titleCaptions[0], sortable: true, width: 80 },
                            { field: 'GROUPNAME', title: titleCaptions[1], sortable: true, width: 87 }
                ]],
                fitColumns: false
            });

            var selectedData = [];
            loadDataForDataGrid("gridGroupsTo_AccessGroup", "../handler/SystemHandler_Security.ashx?type=MenuTables", function (data) {
                selectedData = [];
                for (var i = 0 ; i < data.length; i++) {
                    selectedData.push(data[i]);
                }
            }, { mode: "GroupMenus", menuid: node.id })
            loadDataForDataGrid("gridGroupsFrom_AccessGroup", "../handler/SystemHandler_Security.ashx?type=Security", undefined, { type: "group" }, function (data) {
                var newData = [];
                for (var i = 0 ; i < data.rows.length; i++) {
                    //if (selectedData.length == 0) break;
                    var isExisted = -1;
                    for (var j = 0 ; j < selectedData.length; j++) {
                        if (data.rows[i].GROUPID == selectedData[j].GROUPID) {
                            isExisted = j;
                            break;
                        }
                    }
                    if (isExisted != -1) {
                        selectedData.splice(isExisted, 1);
                    }
                    else {
                        newData.push(data.rows[i]);
                    }
                }
                return newData;
            })
        }, buttons);
    }
}
function btnGroupsTo_AccessGroupClick() {
    fromTo("gridGroupsFrom_AccessGroup", "gridGroupsTo_AccessGroup");
}
function btnGroupsBack_AccessGroup() {
    fromTo("gridGroupsTo_AccessGroup", "gridGroupsFrom_AccessGroup");
}
function btnAccessUsers_MenuClick() {
    var buttonCaption = $.sysmsg('getValue', 'Srvtools/InfoNavigator/NavText');
    var buttonCaptions = buttonCaption.split(";");

    var accessCaption = $.sysmsg('getValue', 'EEPManager/frmSecurityMain/AccessCaption');
    var accessCaptions = accessCaption.split(",");

    var node = $('#treeMenus_Menu').tree("getSelected");
    if (node == null) {
        alert("Please a menu first.");
    }
    else {
        var buttons = [{
            text: buttonCaptions[9],
            handler: function () {
                var users = "";
                var rows = $("#gridUsersTo_AccessUser").datagrid("getData").rows;
                for (var i = 0; i < rows.length; i++) {
                    users += rows[i].USERID + ";";
                }
                $.ajax({
                    type: "POST",
                    url: "../handler/SystemHandler_Security.ashx?type=MenuTables",
                    data: { mode: "InsertUserMenus", menuid: node.id, userids: users },
                    cache: false,
                    async: false,
                    success: function () {
                        $('#winAccessUser').dialog().dialog('close');
                    },
                    error: function (data) {
                        //data.responseText = '';
                        //obj = "[{\"" + textField + "\":\"\"}]";
                    }
                });
            }
        }, {
            text: buttonCaptions[10],
            handler: function () {
                $('#winAccessUser').dialog({
                    onClose: function () {
                    }
                }).dialog('close');
            }
        }, {
            text: accessCaptions[2],
            handler: function () {
                var selectedUser = $("#gridUsersTo_AccessUser").datagrid("getSelected");
                if (selectedUser == null) {
                    alert("Please select a user first.");
                    return;
                }
                var title = "User:(" + selectedUser.USERID + ")" + selectedUser.USERNAME + "/Menu:(" + node.id + ")" + node.text;
                createAndOpenWizardDialog("winAccessControl", title, "650", "350", "AccessControl.aspx", function () {
                    var editIndex = undefined;
                    var dataGrid = $('#dgAccessControl');

                    var dataGridColumnCaption = $.sysmsg('getValue', 'EEPManager/frmSecurityMain/AccessCaption');
                    var dataGridColumnCaptions = dataGridColumnCaption.split(",");

                    dataGrid.datagrid({
                        remoteSort: false,
                        pagination: false,
                        rownumbers: false,
                        singleSelect: true,
                        checkOnSelect: false,
                        frozenColumns: [[
                            { field: 'CONTROLNAME', title: dataGridColumnCaptions[3], width: 100 },
                            { field: 'DESCRIPTION', title: dataGridColumnCaptions[4], width: 150 }
                        ]],
                        columns: [[
                            { field: 'USERID', hidden: true },
                            { field: 'MENUID', title: 'MENUID', hidden: true },
                            { field: 'TYPE', title: 'TYPE', width: 100, hidden: true },
                            { field: 'ENABLED', title: dataGridColumnCaptions[5], width: 70, editor: { type: "checkbox", options: { on: 'Y', off: 'N' } } },
                            { field: 'VISIBLE', title: dataGridColumnCaptions[6], width: 70, editor: { type: "checkbox", options: { on: 'Y', off: 'N' } } },
                            { field: 'ALLOWADD', title: dataGridColumnCaptions[7], width: 70, editor: { type: "checkbox", options: { on: 'Y', off: 'N' } } },
                            { field: 'ALLOWUPDATE', title: dataGridColumnCaptions[8], width: 75, editor: { type: "checkbox", options: { on: 'Y', off: 'N' } } },
                            { field: 'ALLOWDELETE', title: dataGridColumnCaptions[9], width: 75, editor: { type: "checkbox", options: { on: 'Y', off: 'N' } } },
                            { field: 'ALLOWPRINT', title: dataGridColumnCaptions[10], width: 70, editor: { type: "checkbox", options: { on: 'Y', off: 'N' } } }
                        ]],
                        fitColumns: false,
                        onClickRow: function (rowIndex, rowData) {
                            if (editIndex != rowIndex) {
                                if (editIndex != undefined)
                                    dataGrid.datagrid('endEdit', editIndex);
                                dataGrid.datagrid('selectRow', rowIndex).datagrid('beginEdit', rowIndex);
                                editIndex = rowIndex;
                            }
                            dataGrid.datagrid('selectRow', rowIndex).datagrid('beginEdit', rowIndex);
                        }
                    });

                    loadDataForDataGrid("dgAccessControl", "../handler/SystemHandler_Security.ashx?type=GetUserMenuControl", function (data) {
                        dataGrid.datagrid("loadData", data);
                    }, { userId: selectedUser.USERID, menuId: node.id })

                    $("#btnAdd", "#AccessControlMain").click(function () {
                        $.ajax({
                            type: "POST",
                            url: "../handler/SystemHandler_Security.ashx?type=GetSecurity",
                            data: { menuid: node.id },
                            cache: false,
                            async: false,
                            dataType: 'json',
                            success: function (data) {
                                //data = eval('(' + data + ')');
                                var rows = dataGrid.datagrid("getRows");
                                var appandRows = [];
                                for (var j = 0; j < data.length; j++) {
                                    var isExisted = false;
                                    for (var i = 0; i < rows.length; i++) {
                                        if (rows[i].CONTROLNAME == data[j].CONTROLNAME) {
                                            isExisted = true;
                                            break;
                                        }
                                    }
                                    if (isExisted == false) {
                                        appandRows.push({ USERID: selectedUser.USERID, MENUID: node.id, CONTROLNAME: data[j].CONTROLNAME, TYPE: data[j].TYPE, DESCRIPTION: data[j].DESCRIPTION, ENABLED: "N", VISIBLE: "Y", ALLOWADD: "Y", ALLOWUPDATE: "Y", ALLOWDELETE: "Y", ALLOWPRINT: "Y" });
                                    }
                                }
                                for (var i = 0; i < appandRows.length; i++)
                                    dataGrid.datagrid("appendRow", appandRows[i]);
                            },
                            error: function (data) {
                                //data.responseText = '';
                                //obj = "[{\"" + textField + "\":\"\"}]";
                            }
                        });
                    });

                    $("#btnDelete", "#AccessControlMain").click(function () {
                        if (editIndex != undefined) {
                            dataGrid.datagrid('deleteRow', editIndex);
                            editIndex = undefined;
                        }
                    });

                    $("#btnSave", "#AccessControlMain").click(function () {
                        dataGrid.datagrid('acceptChanges');
                        var rows = new Object();
                        rows.inserted = dataGrid.datagrid('getData').rows;
                        if (rows.inserted.length != null) {
                            var htmlEncode = function (key, value) {
                                if (value && typeof value == 'string') {
                                    return value.replace(/</g, "&lt;").replace(/>/g, "&gt;");
                                }
                                else {
                                    return value;
                                }
                            };
                            $.ajax({
                                type: "POST",
                                url: '../handler/SystemHandler_Security.ashx?type=SaveUserMenuControl',
                                data: { userId: selectedUser.USERID, menuId: node.id, rows: JSON.stringify(rows, htmlEncode) },
                                cache: false,
                                async: false,
                                success: function (data) {
                                    alert("Save successed.")
                                },
                                error: function (data) {
                                }
                            });
                        }
                    });

                    $("#btnCancel", "#AccessControlMain").click(function () {
                        $("#winAccessControl").dialog("close");
                    });
                });
            }
        }]
        createAndOpenWizardDialog("winAccessUser", $('#btnAccessUsers_Menu').text(), "450", "350", "AccessUser.aspx", function () {
            var titleCaption = $.sysmsg('getValue', 'Srvtools/UGControl/Caption_User');
            var titleCaptions = titleCaption.split(";");
            $('#gridUsersFrom_AccessUser').datagrid({
                columns: [[
                            { field: 'USERID', title: titleCaptions[0], sortable: true, width: 80 },
                            { field: 'USERNAME', title: titleCaptions[1], sortable: true, width: 87 }
                ]],
                fitColumns: false
            });
            $('#gridUsersTo_AccessUser').datagrid({
                columns: [[
                            { field: 'USERID', title: titleCaptions[0], sortable: true, width: 80 },
                            { field: 'USERNAME', title: titleCaptions[1], sortable: true, width: 87 }
                ]],
                fitColumns: false
            });

            var selectedData = [];
            loadDataForDataGrid("gridUsersTo_AccessUser", "../handler/SystemHandler_Security.ashx?type=MenuTables", function (data) {
                selectedData = [];
                for (var i = 0 ; i < data.length; i++) {
                    selectedData.push(data[i]);
                }
            }, { mode: "UserMenus", menuid: node.id })
            loadDataForDataGrid("gridUsersFrom_AccessUser", "../handler/SystemHandler_Security.ashx?type=Security", undefined, { type: "user" }, function (data) {
                var newData = [];
                for (var i = 0 ; i < data.rows.length; i++) {
                    //if (selectedData.length == 0) break;
                    var isExisted = -1;
                    for (var j = 0 ; j < selectedData.length; j++) {
                        if (data.rows[i].USERID == selectedData[j].USERID) {
                            isExisted = j;
                            break;
                        }
                    }
                    if (isExisted != -1) {
                        selectedData.splice(isExisted, 1);
                    }
                    else {
                        newData.push(data.rows[i]);
                    }
                }
                return newData;
            })
        }, buttons);
    }
}
function btnUsersTo_AccessUserClick() {
    fromTo("gridUsersFrom_AccessUser", "gridUsersTo_AccessUser");
}
function btnUsersBack_AccessUser() {
    fromTo("gridUsersTo_AccessUser", "gridUsersFrom_AccessUser");
}
function fromTo(fromDataGrid, toDataGrid) {
    var to_Data = $('#' + toDataGrid).datagrid('getData');
    var from_Data = $('#' + fromDataGrid).datagrid('getData');
    var from_Selected = $('#' + fromDataGrid).datagrid('getSelected');
    var from_SelectedIndex = $('#' + fromDataGrid).datagrid('getRowIndex', from_Selected);
    if (from_SelectedIndex > -1) {
        to_Data.rows.push(from_Data.rows[from_SelectedIndex]);
        from_Data.rows.splice(from_SelectedIndex, 1);
    }
    $('#' + toDataGrid).datagrid('loadData', to_Data.rows);
    $('#' + fromDataGrid).datagrid('loadData', from_Data.rows);
}

function createAndOpenWizardDialog(winId, title, width, height, formUrl, onLoadHandler, buttons) {
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
        buttons: buttons
        /*,
        buttons: [{
            text: '确定',
            handler: onOkHandler
        }, {
            text: '取消',
            handler: function () {
                $('#' + winId).dialog({
                    onClose: function () {
                        if (onCancelHandler != undefined) {
                            onCancelHandler();
                        }
                    }
                }).dialog('close');
            }
        }]*/
    });
    //if (isSubPath) {
    //    formUrl = "../" + formUrl;
    //}
    $('#' + winId).dialog('refresh', formUrl);
}

function loadDataForDataGrid(dg, u, onAfterLoadSuccess, params, onBeforeLoadData) {
    $.ajax({
        type: "POST",
        url: u,
        data: params,
        cache: false,
        async: false,
        dataType: 'json',
        success: function (data) {
            //data = eval('(' + data + ')');
            if (onBeforeLoadData != undefined) {
                data = onBeforeLoadData(data);
            }
            $('#' + dg).datagrid("loadData", data);
            if (onAfterLoadSuccess != undefined) {
                onAfterLoadSuccess(data);
            }
        },
        error: function (data) {
            //data.responseText = '';
            //obj = "[{\"" + textField + "\":\"\"}]";
        }
    });
}
