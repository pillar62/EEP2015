using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;

namespace JQClientTools
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [Designer(typeof(ControlDesigner), typeof(IDesigner))]
    public class JQTree : Control
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.WriteLine("<ul id=\"" + this.ClientID + "\" href=\"#\" class=\"easyui-tree\" />");
        }

        public bool CollapseAll
        {
            set;
            get;
        }

        public bool FetchAll
        {
            set;
            get;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.RenderJQTree();
            }
        }

        public void RenderJQTree()
        {
            if (!String.IsNullOrEmpty(this.ClientID))
            {
                //render tree
                StringBuilder renderScript = new StringBuilder();
                String mode = "";
                String childrenCount = "data.MENUTABLE1.length";
                String collapseAll = "expandAll";
                if (CollapseAll) collapseAll = "collapseAll";
                if (!FetchAll)
                {
                    mode = "forRoot";
                    childrenCount = "0";
                }
                string strRenderScript = @"
$('#" + this.ClientID + @"').tree({
    onClick: function (node) {
        if (node.state == 'closed') {
            $('#" + this.ClientID + @"').tree('expand', node.target);
        }
        else {
            $('#" + this.ClientID + @"').tree('collapse', node.target);
        }
        if(node.attributes.FORM != null && node.attributes.FORM != '' && node.attributes.FORM != 'null' && node.attributes.FORM.indexOf('http://////') == 0) {
            var formUrl = node.attributes.FORM.replace('http://////','http://');     
            for (var b = formUrl.indexOf('///') ,a = formUrl; b > 0; a = a.replace('///', '/')) {
                b = a.indexOf('///');
                formUrl = a;
            }
            addTab(node.text, formUrl);
        }
        else if (node.attributes.PACKAGE != null && node.attributes.PACKAGE != '' && node.attributes.PACKAGE != 'null'
                    && node.attributes.FORM != null && node.attributes.FORM != '' && node.attributes.FORM != 'null') {
            if (node.attributes.MODULETYPE == 'W') {
                var openUri = 'InnerPages/EEPSingleSignOn.aspx?Package=' + node.attributes.PACKAGE + '&Form=' + node.attributes.FORM;
                if (node.attributes.ITEMPARAM != undefined && node.attributes.ITEMPARAM != null && node.attributes.ITEMPARAM != '') {
                    openUri += '&' + encodeURI(node.attributes.ITEMPARAM);
                }
                addTab(node.text, openUri);
            }
            else if (node.attributes.MODULETYPE == 'C') {
                var openUri = 'InnerPages/EEPSingleSignOn.aspx?Type=win&Package=' + node.attributes.PACKAGE + '&Form=' + node.attributes.FORM;
                if (node.attributes.ITEMPARAM != undefined && node.attributes.ITEMPARAM != null && node.attributes.ITEMPARAM != '') {
                    openUri += '&' + encodeURI(node.attributes.ITEMPARAM);
                }
                addTab(node.text, openUri);
            }
            else if (node.attributes.MODULETYPE == 'O') {
                addTab(node.text, 'InnerPages/FlowDesigner.aspx?FlowFileName=' + node.attributes.FORM);
            }
            else {
                addTab(node.text, node.attributes.PACKAGE.replace('///', '/') + '/' + node.attributes.FORM + '.aspx?' + encodeURI(node.attributes.ITEMPARAM));
            }
        }
        else if('" + mode + @"' == 'forRoot')
        {
            var children = $('#JQTree1').tree('getChildren', node.target);
            if(children.length > 0)
            {
                for(var i=0; i < children.length; i++ )
                {
                    $('#" + this.ClientID + @"').tree('remove', children[i].target);
                }
            }
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: 'handler/SystemHandle.ashx?Type=forClick',
                data: { parentId:node.id },
                async: true,
                cache: false,
                success: function (menus) {
                    var newNodes = [];
                    var parent = node;
                    parent.children = [];
                    for (var i = 0; i < menus.length; i++) {
                        var data = menus[i];
                        var treeNode = createTreeNode(parent, data);
                        newNodes.push(treeNode);
                    }
                    if(newNodes.length > 0)
                    {                    
                        $('#" + this.ClientID + @"').tree('append', {
                            parent: parent.target,
                            data:  newNodes
                        });
                        $('#" + this.ClientID + @"').tree('expandTo', $('#" + this.ClientID + @"').tree('find', newNodes[0].id).target);
                    }
                }
            });
        }
        openFormLog(node.text);
    },
    onLoadSuccess: function (node, data) {
        $('#" + this.ClientID + @"').tree('" + collapseAll + @"');
    },
    onExpand: function (node) {

    }
});
$.ajax({
    type: 'POST',
    dataType: 'json',
    url: 'handler/SystemHandle.ashx?Type=" + mode + @"',
    async: true,
    cache: false,
    success: function (menus) {
        var parent = undefined;
        var newNodes = [];
        for (var i = 0; i < menus.length; i++) {
            var data = menus[i];
            var treeNode = createTreeNode(parent, data);
            if(treeNode != null)
                newNodes.push(treeNode);
        }
        $('#" + this.ClientID + @"').tree('append', {
            data:  newNodes
        });
    }
});
function createTreeNode(parent, data) {
    var treeNode = {};
    if (data.MENUID != null) {
        treeNode.id = data.MENUID;
        if (data.IMAGEURL != null && data.IMAGEURL != '') {
            if($(document).attr('menunoicon') == 'true')
            {
                
            }
            else
            {
                treeNode.iconCls = 'menuicon-' + data.IMAGEURL.replace(/\./g, '');
            }
        }

        treeNode.text = getMenuCaption(data);
        treeNode.attributes = {};
        if (parent == undefined) {
            treeNode.attributes.Level = 1;
        } else {
            treeNode.attributes.Level = parent.attributes.Level + 1;
        } 
        treeNode.attributes.FORM = data.FORM;
        treeNode.attributes.ITEMPARAM = decodeURI(data.ITEMPARAM);
        treeNode.attributes.ITEMTYPE = data.ITEMTYPE;
        treeNode.attributes.PACKAGE = data.PACKAGE;
        if((data.PACKAGE == null || data.PACKAGE == '') && (data.IMAGEURL == null || data.IMAGEURL == '')){
            treeNode.iconCls = 'menu-folder';
        }
        treeNode.attributes.MODULETYPE = data.MODULETYPE;
        treeNode.children = [];
        treeNode.attributes.children = [];
        if (parent == undefined) {
            if('" + mode + @"' != 'forRoot'){
                if((data.PACKAGE == undefined || data.PACKAGE == '') && (data.MENUTABLE1 == undefined || data.MENUTABLE1.length == 0)){
                    return null;
                }
            }
        } else {
            parent.children.push(treeNode);
            parent.attributes.children.push(treeNode);
        }
        if (data.MENUTABLE1 != undefined && data.MENUTABLE1.length > 0) {
            for (var i = 0; i < " + childrenCount + @"; i++) {
                createTreeNode(treeNode, data.MENUTABLE1[i]);
            }
        }
    }
    return treeNode;
}";

                //renderScript.Append("$('#" + this.ClientID + "').tree({\r\n");
                //renderScript.Append("url: \"handler/SystemHandle.ashx\",\r\n");
                //renderScript.Append("loadFilter: function (res) {\r\n");
                //renderScript.Append("var dataSource = [];\r\n");
                ////renderScript.Append("var root = {};\r\n");
                ////renderScript.Append("root.id = '-1';\r\n");
                ////renderScript.Append("root.text = '所有菜单';\r\n");
                ////renderScript.Append("root.attributes = {};\r\n");
                ////renderScript.Append("root.attributes.Level = 1;\r\n");
                ////renderScript.Append("root.children = [];\r\n");
                ////renderScript.Append("dataSource.push(root);\r\n");
                ////renderScript.Append("root.attributes.children = [];\r\n");
                //renderScript.Append("var count = res.length;\r\n");
                //renderScript.Append("for (var i = 0; i < count; i++) {\r\n");
                //renderScript.Append("createTreeNode(undefined, res[i], dataSource);\r\n");
                //renderScript.Append("}\r\n");
                //renderScript.Append("return dataSource;\r\n");
                //renderScript.Append("},\r\n");
                //renderScript.Append("onClick: function (node) {\r\n");

                //renderScript.AppendLine("if(node.state == 'closed'){");
                ////renderScript.AppendLine("debugger;");
                //renderScript.AppendLine("$('#" + this.ClientID + "').tree('expand', node.target);}");
                //renderScript.AppendLine("else{");
                //renderScript.AppendLine("$('#" + this.ClientID + "').tree('collapse', node.target);}");

                //renderScript.AppendLine("if(node.attributes.PACKAGE != '' && node.attributes.FORM != ''){");
                //renderScript.AppendLine("if(node.attributes.MODULETYPE == 'W')");
                //renderScript.AppendLine("{");
                //renderScript.AppendLine("var openUri = 'InnerPages/EEPSingleSignOn.aspx?Package=' + node.attributes.PACKAGE + '&Form=' + node.attributes.FORM;");
                //renderScript.AppendLine("if(node.attributes.ITEMPARAM != undefined && node.attributes.ITEMPARAM != null && node.attributes.ITEMPARAM != \"\"){");
                //renderScript.AppendLine("openUri += \"&\" + node.attributes.ITEMPARAM;} ");
                //renderScript.AppendLine("addTab(node.text, openUri);");
                ////renderScript.AppendLine("addTab(node.text, 'InnerPages/EEPSingleSignOn.aspx?Package=' + node.attributes.PACKAGE + '&Form=' + node.attributes.FORM + '&ItemParam=' + node.attributes.ITEMPARAM);");
                //renderScript.AppendLine("}");
                //renderScript.AppendLine("else if(node.attributes.MODULETYPE == 'O')");
                //renderScript.AppendLine("{");
                //renderScript.AppendLine("addTab(node.text, 'InnerPages/FlowDesigner.aspx?FlowFileName=' + node.attributes.FORM);");
                //renderScript.AppendLine("}");
                //renderScript.AppendLine("else");
                //renderScript.AppendLine("{");
                //renderScript.Append("addTab(node.text, node.attributes.PACKAGE + '/' + node.attributes.FORM + '.aspx?' + node.attributes.ITEMPARAM);\r\n");
                //renderScript.AppendLine("}");
                //renderScript.AppendLine("}");
                //renderScript.AppendLine("},");
                //renderScript.AppendLine("onLoadSuccess: function (node, data) {\r\n");
                //if (CollapseAll)
                //    renderScript.AppendLine("$('#" + this.ClientID + "').tree('collapseAll');");
                //renderScript.AppendLine("}");
                //renderScript.Append("});\r\n");

                ////createTreeNode
                //renderScript.Append("function createTreeNode(parent, data, dataSource) {\r\n");
                //renderScript.Append("if (data.MENUID != null) {\r\n");
                //renderScript.Append("var treeNode = {};\r\n");
                //renderScript.Append("treeNode.id = data.MENUID;\r\n");

                //renderScript.AppendLine("if(data.IMAGEURL != null){");
                //renderScript.AppendLine("treeNode.iconCls = 'menuicon-' + data.IMAGEURL.replace(/\\./g, '');");
                //renderScript.AppendLine("}");

                ////renderScript.Append("treeNode.iconCls = 'icon-ok';\r\n");
                //renderScript.Append("treeNode.text = getMenuCaption(data);\r\n");
                //renderScript.Append("treeNode.attributes = {};\r\n");
                ////renderScript.Append("treeNode.attributes = { state: 'closed' };\r\n");
                //renderScript.Append("if(parent == undefined) {");
                //renderScript.Append("treeNode.attributes.Level = 1;\r\n");
                //renderScript.Append("}");
                //renderScript.Append("else {");
                //renderScript.Append("treeNode.attributes.Level = parent.attributes.Level + 1;\r\n");
                //renderScript.Append("}");
                //renderScript.Append("treeNode.attributes.FORM = data.FORM;\r\n");
                //renderScript.Append("treeNode.attributes.ITEMPARAM = data.ITEMPARAM;\r\n");
                //renderScript.Append("treeNode.attributes.ITEMTYPE = data.ITEMTYPE;\r\n");
                //renderScript.Append("treeNode.attributes.PACKAGE = data.PACKAGE;\r\n");
                //renderScript.Append("treeNode.attributes.MODULETYPE = data.MODULETYPE;\r\n");
                //renderScript.Append("treeNode.children = [];\r\n");
                //renderScript.Append("treeNode.attributes.children = [];\r\n");
                //renderScript.Append("if(parent == undefined) {");
                //renderScript.Append("dataSource.push(treeNode);\r\n");
                //renderScript.Append("}");
                //renderScript.Append("else {");
                //renderScript.Append("parent.children.push(treeNode);\r\n");
                //renderScript.Append("parent.attributes.children.push(treeNode);\r\n");
                //renderScript.Append("}");
                //renderScript.Append("if (data.MENUTABLE1 != undefined && data.MENUTABLE1.length > 0) {\r\n");
                //renderScript.Append("for (var i = 0; i < data.MENUTABLE1.length; i++) {\r\n");
                //renderScript.Append("createTreeNode(treeNode, data.MENUTABLE1[i]);\r\n");
                //renderScript.Append("}\r\n");
                //renderScript.Append("}\r\n");
                //renderScript.Append("}\r\n");
                //renderScript.Append("}\r\n");
                //$('#" + this.ClientID + @"').tree('collapseAll'); 91行
                ////renderScript.Append("$('#" + this.ClientID + "').tree('collapseAll');");
                //        var node = $('#" + this.ClientID + @"').tree('find', '12'); 
                //        $('#" + this.ClientID + @"').tree('collapse', node.target); 
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), strRenderScript, true);
            }
        }
    }
}