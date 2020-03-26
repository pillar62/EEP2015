<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT10411.aspx.cs" Inherits="Template_JQueryQuery1" %>

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
                var sWhere = "CUSID='" + CUSID + "' AND BATCHNO = '" + BATCHNO+"'";
                $("#dataGridMaster").datagrid('setWhere', sWhere);
            }
            flag = false;
        }

        function queryGrid(dg) { //查詢后添加固定條件
            if ($(dg).attr('id') == 'dataGridMaster') {
                var where = $(dg).datagrid('getWhere');
                var st = $("#CANCELDAT_Query").combobox('getValue'); //應收應付帳款狀態
                if (where != "") {
                    where = " 1=1 ";
                    if (st == '1')
                        where = "  RTLessorAVSCustar.canceldat is null and ( RTLessorAVSCustar.amt - RTLessorAVSCustar.realamt = 0 ) ";
                    if (st == '2')
                        where = "  RTLessorAVSCustar.canceldat is null and ( RTLessorAVSCustar.amt - RTLessorAVSCustar.realamt <> 0 ) ";
                    if (st == '3')
                        where = "  RTLessorAVSCustar.canceldat is not null ";
                    if (st == '4')
                        where = "  RTLessorAVSCustar.canceldat is null ";
                }
                where = where + " AND CUSID='" + CUSID + "' AND BATCHNO = '" + BATCHNO + "'";

                $(dg).datagrid('setWhere', where);
            }
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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT1041.cmdRT10411" runat="server" AutoApply="True"
                DataMember="cmdRT10411" Pagination="True" QueryTitle="查詢條件"
                Title="用戶應收應付帳款查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶編號" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="12" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="AR/AP" Editor="text" FieldName="CODENC" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="right" Caption="明細項期數" Editor="numberbox" FieldName="PERIOD" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="right" Caption="應沖金額" Editor="numberbox" FieldName="AMT" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="已沖金額" Editor="numberbox" FieldName="REALAMT" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="未沖金額" Editor="numberbox" FieldName="AMTL" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立要項一" Editor="text" FieldName="COD1" Format="" MaxLength="30" Width="180" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立要項二" Editor="text" FieldName="COD2" Format="" MaxLength="30" Width="180" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立要項三" Editor="text" FieldName="COD3" Format="" MaxLength="30" Width="180" />
                    <JQTools:JQGridColumn Alignment="left" Caption="段" Editor="text" FieldName="COD4" Format="" MaxLength="30" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="巷" Editor="text" FieldName="COD5" Format="" MaxLength="30" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="產生日" Editor="datebox" FieldName="CDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖銷日" Editor="datebox" FieldName="MDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="90" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="沖　　帳" Visible="True" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn2Click" Text="沖帳明細" Visible="True" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn3Click" Text="帳款明細" Visible="True" Icon="icon-view" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="應收應付帳款狀態" Condition="%" DataType="string" Editor="infocombobox" EditorOptions="items:[{value:'0',text:'全部',selected:'false'},{value:'1',text:'已結案',selected:'false'},{value:'2',text:'未結案',selected:'false'},{value:'3',text:'已作廢',selected:'false'},{value:'4',text:'全部(不含作廢)',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="CANCELDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
