<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT105.aspx.cs" Inherits="Template_JQuerySingle1" %>

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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT105.RTLessorAVSCase" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCase" Pagination="True" QueryTitle="Query"
                Title="方案資料維護" EditDialogID="JQDialog1">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="方案編號" Editor="numberbox" FieldName="CASEID" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案名稱" Editor="text" FieldName="CASENAME" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始日" Editor="datebox" FieldName="STARTDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結束日" Editor="datebox" FieldName="ENDDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建案人員" Editor="text" FieldName="CRTUSR" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" MaxLength="500" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建檔員" Editor="text" FieldName="EUSR" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="輸入日期" Editor="datebox" FieldName="EDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="修改員" Editor="text" FieldName="UUSR" Format="" MaxLength="6" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="" Width="120" />
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
            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="">
                <JQTools:JQDataForm ID="JQDataForm1" runat="server" AlwaysReadOnly="False" ChainDataFormID="JQDataForm1" Closed="False" ContinueAdd="False" DataMember="RTLessorAVSCase" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalColumnsCount="1" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" RemoteName="sRT105.RTLessorAVSCase" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0">
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="方案編號" Editor="text" FieldName="CASEID" MaxLength="10" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案名稱" Editor="text" FieldName="CASENAME" MaxLength="30" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="開始日" Editor="text" FieldName="STARTDAT" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結束日" Editor="text" FieldName="ENDDAT" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建案人員" Editor="text" FieldName="CRTUSR" MaxLength="6" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" MaxLength="500" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔員" Editor="text" FieldName="EUSR" MaxLength="6" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="輸入日期" Editor="text" FieldName="EDAT" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改員" Editor="text" FieldName="UUSR" MaxLength="6" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日" Editor="text" FieldName="UDAT" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                    </Columns>
                </JQTools:JQDataForm>
            </JQTools:JQDialog>
        </div>
    <JQTools:JQDefault runat="server" BindingObjectID="dataGridMaster" BorderStyle="NotSet" Enabled="True" EnableTheming="True" ClientIDMode="Inherit" ID="defaultMaster" EnableViewState="True" ViewStateMode="Inherit" >
	<Columns>
		<JQTools:JQDefaultColumn FieldName="EUSR" DefaultValue="_usercode" RemoteMethod="True" CarryOn="False" />
		<JQTools:JQDefaultColumn FieldName="UUSR" DefaultValue="_usercode" RemoteMethod="True" CarryOn="False" />
		<JQTools:JQDefaultColumn FieldName="UDAT" DefaultValue="_today" RemoteMethod="True" CarryOn="False" />
	</Columns>

</JQTools:JQDefault>
<JQTools:JQValidate runat="server" BindingObjectID="dataGridMaster" BorderStyle="NotSet" Enabled="True" EnableTheming="True" ClientIDMode="Inherit" ID="validateMaster" EnableViewState="True" ViewStateMode="Inherit" >
	<Columns>
		<JQTools:JQValidateColumn FieldName="CASEID" CheckNull="True" ValidateType="None" RemoteMethod="True" />
	</Columns>

</JQTools:JQValidate>
</form>
</body>
</html>
