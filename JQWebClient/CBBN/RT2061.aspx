<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT2061.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var INVNO = Request.getQueryStringByName2("INVNO");
        var flag = true;

        function InsDefault() {
            if (INVNO != "") {
                return INVNO;
            }
        }

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = "INVNO='" + INVNO + "'";
                $("#dataGridView").datagrid('setWhere', sWhere);
            }
            flag = false;
        }

        function cacuAmt()
        {
            //金額=數量*單價
            $('#dataFormMasterSALEAMT').numberbox('setValue',$('#dataFormMasterQTY').val() * $('#dataFormMasterUNITAMT').val());
            //稅額=金額*0.05
            $('#dataFormMasterTAXAMT').numberbox('setValue', $('#dataFormMasterSALEAMT').val() * 0.05);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT206.RTInvoiceSub" runat="server" AutoApply="True"
                DataMember="RTInvoiceSub" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="發票明細" OnLoadSuccess="dgOnloadSuccess" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="發票號碼" Editor="text" FieldName="INVNO" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="numberbox" FieldName="ENTRY" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="產品名稱" Editor="text" FieldName="PRODNC" Format="" MaxLength="100" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="數量" Editor="numberbox" FieldName="QTY" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="單價" Editor="numberbox" FieldName="UNITAMT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="銷售額" Editor="numberbox" FieldName="SALEAMT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="稅額" Editor="numberbox" FieldName="TAXAMT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMOSUB" Format="" MaxLength="250" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最後異動人員" Editor="text" FieldName="UUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最後異動日期" Editor="datebox" FieldName="UDAT" Format="" Visible="true" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="True"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="發票明細">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTInvoiceSub" HorizontalColumnsCount="2" RemoteName="sRT206.RTInvoiceSub" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="發票號碼" Editor="text" FieldName="INVNO" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="項次" Editor="numberbox" FieldName="ENTRY" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="產品名稱" Editor="text" FieldName="PRODNC" Format="" maxlength="100" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="數量" Editor="numberbox" FieldName="QTY" Format="" Width="180" OnBlur="cacuAmt" />
                        <JQTools:JQFormColumn Alignment="left" Caption="單價" Editor="numberbox" FieldName="UNITAMT" Format="N4" Width="180" OnBlur="cacuAmt" EditorOptions="precision:4" />
                        <JQTools:JQFormColumn Alignment="left" Caption="銷售額" Editor="numberbox" FieldName="SALEAMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="稅額" Editor="numberbox" FieldName="TAXAMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMOSUB" Format="" maxlength="250" Width="400" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="最後異動人員" Editor="infocombobox" FieldName="UUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="最後異動日期" Editor="datebox" FieldName="UDAT" Format="" Width="180" ReadOnly="True" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQAutoSeq ID="JQAutoSeq1" runat="server" BindingObjectID="dataFormMaster" FieldName="ENTRY" />
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="INVNO" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_usercode" FieldName="UUSR" RemoteMethod="True" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today" FieldName="UDAT" RemoteMethod="True" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
</html>
