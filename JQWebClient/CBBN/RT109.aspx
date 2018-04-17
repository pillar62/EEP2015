<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT109.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>

        function btnReloadClick() {
            $('#dataGridView').datagrid('reload');
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT109.RTBillCharge" runat="server" AutoApply="True"
                DataMember="RTBillCharge" Pagination="True" QueryTitle="查詢" EditDialogID="JQDialog1"
                Title="資費方案" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Panel" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="社區類別" Editor="inforefval" FieldName="CASETYPE" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTCode',tableName:'View_RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'L5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:true,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案類型" Editor="inforefval" FieldName="CASEKIND" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTCode',tableName:'View_RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:true,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費週期" Editor="inforefval" FieldName="PAYCYCLE" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTCode',tableName:'View_RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:true,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="使用期間(月)" Editor="text" FieldName="PERIOD" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="標準收費金額" Editor="numberbox" FieldName="AMT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="第二戶收費金額" Editor="numberbox" FieldName="AMT2" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="退租金額／每月" Editor="numberbox" FieldName="DROPAMT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="退租金額／每月(第二戶)" Editor="numberbox" FieldName="DROPAMT2" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" MaxLength="50" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="BILLCOD" Editor="text" FieldName="BILLCOD" Format="" MaxLength="2" Visible="true" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="存檔" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel" Text="取消"  />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btnReloadClick" Text="資料更新" Visible="True" Icon="icon-reload" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區類別" Condition="%" DataType="string" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'L5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="CASETYPE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="方案類型" Condition="%" DataType="string" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="CASEKIND" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="資費方案">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTBillCharge" HorizontalColumnsCount="2" RemoteName="sRT109.RTBillCharge" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="社區類別" Editor="inforefval" FieldName="CASETYPE" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTCode',tableName:'View_RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'L5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案類型" Editor="inforefval" FieldName="CASEKIND" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTCode',tableName:'View_RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費週期" Editor="inforefval" FieldName="PAYCYCLE" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTCode',tableName:'View_RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="使用期間(月)" Editor="text" FieldName="PERIOD" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="標準收費金額" Editor="numberbox" FieldName="AMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第二戶收費金額" Editor="numberbox" FieldName="AMT2" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="退租金額／每月" Editor="numberbox" FieldName="DROPAMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="退租金額／每月(第二戶)" Editor="numberbox" FieldName="DROPAMT2" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="BILLCOD" Editor="text" FieldName="BILLCOD" Format="" maxlength="2" Width="180" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
</html>
