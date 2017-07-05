<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SYS001.aspx.cs" Inherits="Template_JQuerySingle1" %>

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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sSYS001.MENUFUNCTION" runat="server" AutoApply="True"
                DataMember="MENUFUNCTION" Pagination="True" QueryTitle="功能查詢"
                Title="功能架構進度表" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Panel" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="選單代號" Editor="text" FieldName="MENUID" Format="" MaxLength="0" Width="60" ReadOnly="True" />
                    <JQTools:JQGridColumn Alignment="left" Caption="功能名稱(排)" Editor="text" FieldName="NM_SHOW" Format="" MaxLength="0" Width="200" ReadOnly="True" />
                    <JQTools:JQGridColumn Alignment="left" Caption="功能名稱" Editor="text" FieldName="CAPTION" Format="" MaxLength="0" Width="90" ReadOnly="True" />
                    <JQTools:JQGridColumn Alignment="left" Caption="上層選單" Editor="infocombobox" FieldName="PARENT" Format="" MaxLength="0" Width="120" EditorOptions="valueField:'MENUID',textField:'CAPTION',remoteName:'sSYS001.View_MENUFUNCTION',tableName:'View_MENUFUNCTION',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                    <JQTools:JQGridColumn Alignment="left" Caption="程式代號" Editor="text" FieldName="FORM" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始日期" Editor="datebox" FieldName="DT_START" Format="yyyy/mm/dd" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="預計結案日" Editor="datebox" FieldName="DT_PREND" Format="yyyy/mm/dd" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案日期" Editor="datebox" FieldName="DT_END" Format="yyyy/mm/dd" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="是否需要" Editor="infocombobox" FieldName="YN_DO" Format="" MaxLength="0" Width="64" EditorOptions="items:[{value:'Y',text:'Y',selected:'false'},{value:'N',text:'N',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註說明" Editor="textarea" FieldName="NM_DESC" Format="" Width="200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="NO_SORT" Editor="numberbox" FieldName="NO_SORT" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="功能別" Editor="infocombobox" FieldName="FUNCTIONS" Format="" MaxLength="0" Width="120" Visible="True" EditorOptions="items:[{value:'M',text:'選單',selected:'false'},{value:'F',text:'功能',selected:'false'},{value:'P',text:'處理',selected:'false'},{value:'R',text:'報表',selected:'false'},{value:'O',text:'其它',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="系統別" Editor="text" FieldName="ITEMTYPE" Format="" Width="120" EditorOptions="" />
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
                    <JQTools:JQQueryColumn AndOr="and" Caption="選單編號" Condition="%" DataType="string" Editor="text" FieldName="MENUID" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="功能名稱" Condition="%" DataType="string" Editor="text" FieldName="CAPTION" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="需要(Y/N)" Condition="%" DataType="string" Editor="text" FieldName="YN_DO" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="備註" Condition="%" DataType="string" Editor="text" FieldName="NM_DESC" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="系統別" Condition="%" DataType="string" Editor="text" FieldName="ITEMTYPE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="功能別" Condition="%" DataType="string" Editor="infocombobox" EditorOptions="items:[{value:'M',text:'MENU',selected:'false'},{value:'F',text:'FUNCTION',selected:'false'},{value:'P',text:'PROCESS',selected:'false'},{value:'R',text:'REPORT',selected:'false'},{value:'O',text:'OTHER',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="FUNCTIONS" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
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
