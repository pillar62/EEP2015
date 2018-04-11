<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1046.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID");
        var flag = true;

        function InsDefault() {
            if (CUSID != "") {
                return CUSID;
            }
        }

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = " CUSID='" + CUSID + "'";
                $("#dataGridMaster").datagrid('setWhere', sWhere);
            }
            flag = false;
        }

        function btn1Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var CUSID = row.CUSID;
            var BATCHNO = row.BATCHNO;
            var CANCELDAT = row.CANCELDAT; //作廢日期
            if (CANCELDAT != null && CANCELDAT != "") {
                alert(CANCELDAT);
                alert("已作廢!!");
                return false;
            }
            //parent.addTab("用戶應收應付帳款沖帳", "CBBN/RT10461.aspx?CUSID=" + CUSID + "&BATCHNO=" + BATCHNO);
            parent.addTab("用戶應收應付帳款沖帳", "CBBN/RT104111.aspx?CUSID=" + CUSID + "&BATCHNO=" + BATCHNO);
        }

        function btn2Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var CUSID = row.CUSID;
            var BATCHNO = row.BATCHNO;
            var CANCELDAT = row.CANCELDAT; //作廢日期
            parent.addTab("用戶應收應付帳款沖帳明細查詢", "CBBN/RT104112.aspx?CUSID=" + CUSID + "&BATCHNO=" + BATCHNO);
        }

        function btn3Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var CUSID = row.CUSID;
            var BATCHNO = row.BATCHNO;
            var CANCELDAT = row.CANCELDAT; //作廢日期
            parent.addTab("用戶應收應付帳款明細查詢", "CBBN/RT104113.aspx?CUSID=" + CUSID + "&BATCHNO=" + BATCHNO);
        }

        function btn4Click(val) {
            alert("開始沖帳");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT1046.RT1046" runat="server" AutoApply="True"
                DataMember="RT1046" Pagination="True" QueryTitle="Query"
                Title="用戶應收應付帳款明細查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" MaxLength="15" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" MaxLength="12" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="AR/AP" Editor="text" FieldName="CODENC" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="明細項期數" Editor="text" FieldName="PERIOD" MaxLength="10" Width="90" />
                    <JQTools:JQGridColumn Alignment="right" Caption="應沖金額" Editor="text" FieldName="AMT" MaxLength="10" Width="90" />
                    <JQTools:JQGridColumn Alignment="right" Caption="已沖金額" Editor="text" FieldName="REALAMT" MaxLength="10" Width="90" />
                    <JQTools:JQGridColumn Alignment="right" Caption="未沖金額" Editor="text" FieldName="Expr1" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立要項一" Editor="text" FieldName="COD1" MaxLength="30" Width="160" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立要項二" Editor="text" FieldName="COD2" MaxLength="30" Width="160" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立要項三" Editor="text" FieldName="COD3" Width="160" MaxLength="30" />
                    <JQTools:JQGridColumn Alignment="left" Caption="段" Editor="text" FieldName="COD4" Width="60" MaxLength="30" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="巷" Editor="text" FieldName="COD5" Width="60" MaxLength="30" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="產生日" Editor="text" FieldName="CDAT" Width="90" Format="yyyy/mm/dd" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖帳日" Editor="text" FieldName="MDAT" Width="90" Format="yyyy/mm/dd" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="text" FieldName="CANCELDAT" Width="90" Format="yyyy/mm/dd" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="Export" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton"
                        OnClick="btn1Click" Text="沖  帳" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-view" ItemType="easyui-linkbutton" OnClick="btn2Click" Text="沖帳明細" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-view" ItemType="easyui-linkbutton" OnClick="btn3Click" Text="帳款明細" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>

</html>
