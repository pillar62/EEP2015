function renderMain() {
    //var treeURL = "handler/SystemHandle.ashx";
    //$('#treeMain').tree({
    //    url: treeURL,
    //    loadFilter: function (res) {
    //        var dataSource = [];
    //        var root = {};
    //        root.id = "-1";
    //        root.text = "所有菜单";
    //        root.attributes = {};
    //        root.attributes.Level = 1;
    //        root.children = [];
    //        dataSource.push(root);
    //        root.attributes.children = [];

    //        var count = res.length;
    //        for (var i = 0; i < count; i++) {
    //            createTreeNode(root, res[i]);
    //        }
    //        return dataSource;
    //    },
    //    onClick: function (node) {
    //        var url = node.attributes.PACKAGE + "/" + node.attributes.FORM + ".aspx";
    //        addTab(node.text, url);
    //    }
    //});

    //function createTreeNode(parent, data) {
    //    if (data.MENUID != null) {
    //        var treeNode = {};
    //        treeNode.id = data.MENUID;
    //        treeNode.text = data.CAPTION;
    //        treeNode.attributes = { state: 'closed' };
    //        treeNode.attributes.Level = parent.attributes.Level + 1;
    //        treeNode.attributes.FORM = data.FORM;
    //        treeNode.attributes.ITEMPARAM = data.ITEMPARAM;
    //        treeNode.attributes.ITEMTYPE = data.ITEMTYPE;
    //        treeNode.attributes.PACKAGE = data.PACKAGE;
    //        treeNode.children = [];
    //        parent.children.push(treeNode);
    //        treeNode.attributes.children = [];
    //        parent.attributes.children.push(treeNode);

    //        if (data.MENUTABLE1 != undefined && data.MENUTABLE1.length > 0) {
    //            for (var i = 0; i < data.MENUTABLE1.length; i++) {
    //                createTreeNode(treeNode, data.MENUTABLE1[i]);
    //            }
    //        }
    //    }
    //}

    //$('#mmMenus1').menu({
    //    onClick: function (item) {
    //        if (item.text == "SingleTest") {
    //            var title = "Test/SingleTest.aspx";
    //            addTab("SingleTest", title);
    //        }
    //        else if (item.text == "MasterDetail") {
    //            var title = "Test/MasterDetailTest2.aspx";
    //            addTab("MasterDetail", title);
    //        }
    //    }
    //});

    var mainPageText = $.sysmsg('getValue', 'JQWebClient/mainpagelinkbutton');
    var mainPageTexts = mainPageText.split(',');
    var hometab = $('#tabsMain').tabs('getTab', 0)
    $('#tabsMain').tabs('update', {
        tab: hometab,
        options: {
            title: mainPageTexts[5]
        }
    });

    $.ajax({
        type: "POST",
        url: 'handler/SystemHandle.ashx?type=IsWorkflow',
        cache: false,
        async: true,
        success: function (data) {
            if (data == "true") {
                addTab(mainPageTexts[6], "WorkflowPage.aspx", true);
            }
        },
        error: function (data) {
            //data.responseText = '';
            //obj = "[{\"" + textField + "\":\"\"}]";
        }
    });

    $('#ddlSolution').combobox({
        url: 'handler/SystemHandle.ashx?type=GetSolution',
        onLoadSuccess: function () {
            $.ajax({
                type: "POST",
                url: 'handler/SystemHandle.ashx?type=SetCurrentSolution',
                cache: false,
                async: false,
                success: function (data) {
                    $('#ddlSolution').combobox("setValue", data);
                },
                error: function (data) {
                    //data.responseText = '';
                    //obj = "[{\"" + textField + "\":\"\"}]";
                }
            });
        },
        onSelect: function (record) {
            $.ajax({
                type: "POST",
                url: 'handler/SystemHandle.ashx?Type=RefreshMenu&SolutionId=' + record.ID,
                cache: false,
                async: false,
                success: function (data) {
                    data = eval('(' + data + ')');
                    $('#mmMenus').empty();

                    for (var i = 0; i < data.length; i++) {
                        if (data[i].MENUTABLE1 != null) {
                            var captionText = getMenuCaption(data[i]);
                            $('#mmMenus').menu('appendItem', {
                                id: data[i].MENUID,
                                text: captionText,
                                CAPTION: captionText,
                                PACKAGE: data[i].PACKAGE,
                                FORM: data[i].FORM,
                                ITEMPARAM: decodeURI(data[i].ITEMPARAM)
                                //iconCls: 'icon-ok',
                            });
                            createSubMenu(data[i].MENUTABLE1, captionText);
                        }
                        else {//if (data[i].PACKAGE != null && data[i].PACKAGE != "")
                            $('#mmMenus').menu('appendItem', {
                                id: data[i].MENUID,
                                text: getMenuCaption(data[i]),
                                CAPTION: getMenuCaption(data[i]),
                                PACKAGE: data[i].PACKAGE,
                                FORM: data[i].FORM,
                                ITEMPARAM: decodeURI(data[i].ITEMPARAM)
                                //iconCls: 'icon-ok',
                            });
                        }
                    }
                },
                error: function (data) {
                    //data.responseText = '';
                    //obj = "[{\"" + textField + "\":\"\"}]";
                }
            });

            $.ajax({
                type: "POST",
                url: 'handler/SystemHandle.ashx?SolutionId=' + record.ID,
                cache: false,
                async: false,
                success: function (menus) {
                    $("#JQTree1").tree('loadData', []);
                    menus = eval('(' + menus + ')');
                    var parent = undefined;
                    var newNodes = [];
                    for (var i = 0; i < menus.length; i++) {
                        var data = menus[i];
                        var treeNode = createTreeNode(parent, data);
                        if (treeNode != null)
                            newNodes.push(treeNode);
                    }
                    $("#JQTree1").tree('append', {
                        data: newNodes
                    });
                    //$("#JQTree1").tree("loadData", data);
                },
                error: function (data) {
                    //data.responseText = '';
                    //obj = "[{\"" + textField + "\":\"\"}]";
                }
            });
        }
    });

    function createSubMenu(menuInfo, parentText) {
        var parent = $('#mmMenus').menu('findItem', parentText);
        if (parent != null) {
            for (var i = 0; i < menuInfo.length; i++) {
                $('#mmMenus').menu('appendItem', {
                    id: menuInfo[i].MENUID,
                    parent: parent.target,
                    text: getMenuCaption(menuInfo[i]),
                    CAPTION: getMenuCaption(menuInfo[i]),
                    PACKAGE: menuInfo[i].PACKAGE,
                    FORM: menuInfo[i].FORM,
                    ITEMPARAM: decodeURI(menuInfo[i].ITEMPARAM)
                    //iconCls: 'icon-ok',
                });
                if (menuInfo[i].MENUTABLE1 != null) {
                    createSubMenu(menuInfo[i].MENUTABLE1, getMenuCaption(menuInfo[i]));
                }
            }
        }
    }
    //[id^="mmMenus"]
    $('.menu-top.menu').each(function () {
        if ($(this).id == "mm3") return;
        $(this).menu({
            onClick: function (item) {
                if (item.id != undefined) {
                    $.ajax({
                        type: "POST",
                        url: 'handler/SystemHandle.ashx?Type=GetMenu&MENUID=' + item.id,
                        cache: false,
                        async: false,
                        success: function (menuItem) {
                            menuItem = eval('(' + menuItem + ')');
                            menuItem = menuItem[0];
                            var itemparam = decodeURI(menuItem.ITEMPARAM);
                            if (menuItem.MODULETYPE == "W") {
                                addTab(menuItem.CAPTION, "InnerPages/EEPSingleSignOn.aspx?Package=" + menuItem.PACKAGE + "&Form=" + menuItem.FORM + "&" + encodeURI(itemparam));
                            }
                            else if (menuItem.MODULETYPE == 'C') {
                                addTab(menuItem.CAPTION, "InnerPages/EEPSingleSignOn.aspx?Type=win&Package=" + menuItem.PACKAGE + "&Form=" + menuItem.FORM + "&" + encodeURI(itemparam));
                            }
                            else if (menuItem.MODULETYPE == "O") {
                                addTab(menuItem.CAPTION, "InnerPages/FlowDesigner.aspx?FlowFileName=" + menuItem.FORM);
                            }
                            else {
                                addTab(menuItem.CAPTION, menuItem.PACKAGE + "/" + menuItem.FORM + ".aspx?" + encodeURI(itemparam));
                            }
                        },
                        error: function (data) {
                            //data.responseText = '';
                            //obj = "[{\"" + textField + "\":\"\"}]";
                        }
                    });
                }
            }
        });
    })


    //$("#tabsMain").tabs("select", 1);
}

function addTab(title, url, isSelected) {
    var mainTab = top.$("#tabsMain");
    //url = encodeURI(url);加了这段的话，flow的status会被转码2次，所以先拿掉
    if (mainTab.length == 0) {
        mainTab = $("#tabsWorkFlow")
    }
    if (mainTab.tabs('exists', title)) {
        mainTab.tabs('select', title); //选中并刷新
        var currTab = mainTab.tabs('getSelected');
        var urlSrc = $(currTab.panel('options').content).attr('src');
        if (urlSrc != undefined && currTab.panel('options').title != 'Home') {
            mainTab.tabs('update', {
                tab: currTab,
                options: {
                    content: createFrame(url)
                }
            });
        }
    } else {
        var content = createFrame(url);
        var selected = true;
        if (isSelected != undefined)
            selected = isSelected;
        mainTab.tabs('add', {
            title: title,
            content: content,
            closable: true,
            selected: selected,
            style: 'background-image: url(../img/main.jpg)'
        });
    }

    //tabClose();
}

function createFrame(url) {
    var s = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
    return s;
}

var captionLang = '';

function getMenuCaption(data) {
    var currentLang = navigator.language;   //判断除IE外其他浏览器使用语言
    if (!currentLang) {//判断IE浏览器使用语言
        currentLang = navigator.browserLanguage;
    }
    if (!captionLang) {
        $.ajax({
            type: "POST",
            url: window.currentUrl,
            data: "mode=language",
            cache: false,
            async: false,
            success: function (data) {
                currentLang = data;
                captionLang = currentLang;
            }, error: function (data) {

            }
        });
    }
    else {
        currentLang = captionLang;
    }
    var caption = "";
    switch (currentLang.toLowerCase()) {
        case 'zh-hk': caption = data.CAPTION3; break;
        case 'zh-hans-cn': caption = data.CAPTION2; break;
        case 'zh-cn': caption = data.CAPTION2; break;
        case 'zh-hant-tw': caption = data.CAPTION1; break;
        case 'zh-tw': caption = data.CAPTION1; break;
        default: caption = data.CAPTION0; break;
    }

    if (currentLang.toLowerCase().indexOf("en") == 0) {
        caption = data.CAPTION0;
    }

    if (caption == undefined || caption == null || caption == "") {
        caption = data.CAPTION;
    }
    var reg = new RegExp("/{2,}", "g");
    caption = caption.replace(reg, "/");
    return caption;
}

function closeCurrentTab() {
    var tab = $('#tabsMain').tabs('getSelected');
    var index = $('#tabsMain').tabs('getTabIndex', tab);
    $('#tabsMain').tabs('close', index);
}

function changeTitle(title) {
    var tab = $('#tabsMain').tabs('getSelected');
    tab.panel('options').tab.find(".tabs-title").html(title);
}

function changePassword() {
    var changePasswordWindow = $("#changePasswordWindow", "body");
    if (changePasswordWindow.length == 0) {
        $("body").append('<div id="changePasswordWindow" style="padding:0px;"/>');
        changePasswordWindow = $("#changePasswordWindow", "body");
    }
    changePasswordWindow.window({
        title: $('#changePasswordMenuButton').text(),
        collapsible: false,
        minimizable: false,
        maximizable: false,
        resizable: false,
        href: 'InnerPages/ChangePassword.aspx',
        width: 351,
        height: 350,
        modal: true,
        onLoad: function () {
            var NewPasswordErrorMessage = "兩次輸入的密碼不一致";
            var ChangeSucceed = "更改密碼成功";
            var ChangeError = "更改密碼失敗";
            var OldNewErrorMessage = "新舊密碼不能相同";

            var localstring = $.sysmsg('getValue', 'Srvtools/UPWDControl/LabelName');
            var local = localstring.split(';');
            var useridloacl = local[0];
            var oldpwdlocal = local[2];
            var newpwdlocal = local[3];
            var confimlocal = local[4];
            $('#labelUserID').text(useridloacl);
            $('#labelOPassword').text(oldpwdlocal);
            $('#labelNPassword').text(newpwdlocal);
            $('#labelCPassword').text(confimlocal);

            localstring = $.sysmsg('getValue', 'JQWebClient/dialogbuttontext');
            local = localstring.split(',');
            $('#ok')[0].value = local[2];
            NewPasswordErrorMessage = $.sysmsg('getValue', 'Srvtools/UPWDControl/NewPasswordErrorMessage');
            ChangeSucceed = $.sysmsg('getValue', 'Srvtools/UPWDControl/ChangeSucceed');
            ChangeError = $.sysmsg('getValue', 'Srvtools/UPWDControl/ChangeError');
            OldNewErrorMessage = $.sysmsg('getValue', 'Srvtools/UPWDControl/OldNewErrorMessage');

            $("#ok", this).unbind().bind("click", function () {
                var userID = $("#userID", changePasswordWindow).val();
                var oPassword = $("#oPassword", changePasswordWindow).val();
                var nPassword = $("#nPassword", changePasswordWindow).val();
                var cPassword = $("#cPassword", changePasswordWindow).val();
                if (nPassword != cPassword) {
                    alert(NewPasswordErrorMessage);
                }
                else if (oPassword == nPassword) {
                    alert(OldNewErrorMessage);
                }
                else {
                    $.ajax({
                        type: "POST",
                        url: "handler/SystemHandle.ashx?Type=ChangePassword&UserID=" + userID + "&OPassword=" + oPassword + "&NPassword=" + nPassword,
                        cache: false,
                        async: true,
                        success: function (data) {
                            if (data == 'o') {
                                alert(ChangeSucceed);
                                changePasswordWindow.window('close');
                            }
                            else if (data == "e") {
                                alert(ChangeError);
                            }
                            else if (data.length > 0) {
                                alert(data);
                            }
                            else {
                                alert(ChangeError);
                            }
                        }
                    });
                }
            });
        }
    });
    changePasswordWindow.window('open');
}

//window.top["collapseMenu"] = function () {
//    collapseMenu();
//};
function collapseMenu() {
    $("#mainBodyLayout").layout("collapse", "west");
}
