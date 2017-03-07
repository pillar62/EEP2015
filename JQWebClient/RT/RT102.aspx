<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT102.aspx.cs" Inherits="Template_JQuerySingle1" %>

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
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT105.RTLessorAVSCase" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCase" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="RT102">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="方案編號" Editor="numberbox" FieldName="CASEID" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案名稱" Editor="text" FieldName="CASENAME" Format="" MaxLength="30" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始日" Editor="datebox" FieldName="STARTDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結束日" Editor="datebox" FieldName="ENDDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建案人員" Editor="inforefval" FieldName="CRTUSR" Format="" MaxLength="6" Visible="true" Width="120" >
                        <DrillFields>
                            <JQTools:JQDrillDownFields FieldName="CASEID" />
                            <JQTools:JQDrillDownFields FieldName="CASENAME" />
                        </DrillFields>
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" MaxLength="500" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建檔員" Editor="text" FieldName="EUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="輸入日期" Editor="datebox" FieldName="EDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="修改員" Editor="text" FieldName="UUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="" Visible="true" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply"
                        Text="存檔" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel"
                        Text="取消"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="RT102" EditMode="Switch">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCase" HorizontalColumnsCount="2" RemoteName="sRT105.RTLessorAVSCase" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="方案編號" Editor="numberbox" FieldName="CASEID" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案名稱" Editor="text" FieldName="CASENAME" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="開始日" Editor="datebox" FieldName="STARTDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結束日" Editor="datebox" FieldName="ENDDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建案人員" Editor="text" FieldName="CRTUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" maxlength="500" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔員" Editor="text" FieldName="EUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="輸入日期" Editor="datebox" FieldName="EDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改員" Editor="text" FieldName="UUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="" Width="180" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
            <JQTools:JQAutoSeq ID="JQAutoSeq1" runat="server" />
        </div>
    </form>
</body>
</html>
