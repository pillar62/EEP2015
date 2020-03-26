<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT104113.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID");
        var BATCHNO = Request.getQueryStringByName2("BATCHNO");
        var flag = true;

        function InsDefault() {
            if (CUSID != "") {
                return CUSID;
            }
        }

        $(document).ready(function () {
            dgOnloadSuccess();
        })

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = " CUSID='" + CUSID + "' AND BATCHNO = '" + BATCHNO + "'";
                $("#dataGridMaster").datagrid('setWhere', sWhere);
            }
            flag = false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT1041.cmdRT104113" runat="server" AutoApply="True"
                DataMember="cmdRT104113" Pagination="True" QueryTitle="Query"
                Title="用戶應收應付帳款明細查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="12" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="序號" Editor="numberbox" FieldName="SEQ" Format="" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="會計科目" Editor="text" FieldName="Expr2" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="科目名稱" Editor="text" FieldName="ACNAMEC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="立帳年月" Editor="text" FieldName="TYM1" Format="yyyy/mm" MaxLength="0" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="認列年月" Editor="text" FieldName="TYM2" Format="yyyy/mm" MaxLength="0" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="項目名稱" Editor="text" FieldName="ITEMNC" Format="" MaxLength="50" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="正/負" Editor="text" FieldName="PORM" Format="" MaxLength="1" Width="50" />
                    <JQTools:JQGridColumn Alignment="right" Caption="應收(付)金額" Editor="numberbox" FieldName="AMT" Format="" Width="90" />
                    <JQTools:JQGridColumn Alignment="right" Caption="已沖銷金額" Editor="numberbox" FieldName="REALAMT" Format="" Width="90" />
                    <JQTools:JQGridColumn Alignment="right" Caption="未沖帳金額" Editor="numberbox" FieldName="Expr1" Format="" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="產生日" Editor="datebox" FieldName="CDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖帳日" Editor="datebox" FieldName="MDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢原因" Editor="textarea" FieldName="CANCELMEMO" Format="" MaxLength="100" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
