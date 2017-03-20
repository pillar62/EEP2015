<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT305.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT305.cmRT305" runat="server" AutoApply="True"
                DataMember="cmRT305" Pagination="True" QueryTitle="Query"
                Title="客戶資料查詢">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="comtype" Editor="text" FieldName="comtype" Format="" MaxLength="2" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="comq1" Editor="numberbox" FieldName="comq1" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="lineq1" Editor="numberbox" FieldName="lineq1" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="cusid" Editor="text" FieldName="cusid" Format="" MaxLength="20" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="entryno" Editor="text" FieldName="entryno" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="comn" Editor="text" FieldName="comn" Format="" MaxLength="50" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="cusnc" Editor="text" FieldName="cusnc" Format="" MaxLength="100" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="Expr1" Editor="text" FieldName="Expr1" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="comtypenc" Editor="text" FieldName="comtypenc" Format="" MaxLength="20" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="rcvdat" Editor="datebox" FieldName="rcvdat" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="finishdat" Editor="datebox" FieldName="finishdat" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="docketdat" Editor="datebox" FieldName="docketdat" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="dropdat" Editor="datebox" FieldName="dropdat" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="canceldat" Editor="datebox" FieldName="canceldat" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="RADDR" Editor="text" FieldName="RADDR" Format="" MaxLength="150" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="CASENO" Editor="numberbox" FieldName="CASENO" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="yn_close" Editor="numberbox" FieldName="yn_close" Format="" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="更改" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="確定" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel" Text="取消" />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="查詢" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>
    <JQTools:JQDefault runat="server" BindingObjectID="dataGridMaster" BorderStyle="NotSet" Enabled="True" EnableTheming="True" ClientIDMode="Inherit" ID="defaultMaster" EnableViewState="True" ViewStateMode="Inherit" >
</JQTools:JQDefault>
<JQTools:JQValidate runat="server" BindingObjectID="dataGridMaster" BorderStyle="NotSet" Enabled="True" EnableTheming="True" ClientIDMode="Inherit" ID="validateMaster" EnableViewState="True" ViewStateMode="Inherit" >
</JQTools:JQValidate>
</form>
</body>
</html>
