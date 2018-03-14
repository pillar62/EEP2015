<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT204.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        function queryGrid(dg) { //查詢后添加固定條件
            
            if ($(dg).attr('id') == 'dataGridMaster')
            {
                var where = " 1=1 ";
                
                var COD1 = $("#COD1_Query").combobox('getValue'); //年
                if (COD1==""){
                    where = where +  " and A.cusid <> '' ";}                
                if ( COD1=="1"){
                    where = where + " and A.canceldat is null and ( A.amt - A.realamt = 0 ) ";
                }
                if ( COD1=="2"){
                    where = where + " and A.canceldat is null and ( A.amt - A.realamt <> 0 ) ";
                }
                if ( COD1=="3"){
                    where = where + " and A.canceldat is not null  ";
                }
                if ( COD1=="4"){
                    where = where + " and A.canceldat is null  ";
                }
                if ( COD1=="5"){
                    where = where + " and A.canceldat is null and ( A.amt - A.realamt < 0 ) ";
                }
               
                var COMN = $("#COMN_Query").val();
                if (COMN != "") {
                    where = where + " and A.COMN like '%" + COMN + "%'";
                }

                var COMQ1 = $("#COMQ1_Query").val();
                if (COMQ1 != "") {
                    where = where + " and A.COMQ1 = " + COMQ1;
                }

                var CUSNC = $("#CUSNC_Query").val();
                if (CUSNC != "") {
                    where = where + " and A.CUSNC like '%" + CUSNC + "%'";
                }

                var MDAT = $("#MDAT_Query").datebox('getValue');
                if (MDAT != "") {
                    where = where + " and A.MDAT >= '" + MDAT + "'";
                }

                var CANCELDAT = $("#CANCELDAT_Query").datebox('getValue');
                if (CANCELDAT != "") {
                    where = where + " and A.MDAT <= '" + CANCELDAT + "'";
                }

                if (where.length > 0) {
                    //取得查詢條件的值
                    
                }
                
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
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var CUSID = row.CUSID;
            var BATCHNO = row.BATCHNO;
            var CANCELDAT = row.CANCELDAT; //作廢日期
            parent.addTab("超商未沖(Excel)", "CBBN/RT2044.aspx?CUSID=" + CUSID + "&BATCHNO=" + BATCHNO);
        }

        function btn5Click(val) {
            //var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var CUSID = "";
            var BATCHNO = "";
            var CANCELDAT = ""; //作廢日期
            parent.addTab("轉入超商銷帳檔", "CBBN/RT2045.aspx");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT204.cmRT204" runat="server" AutoApply="True"
                DataMember="cmRT204" Pagination="True" QueryTitle="查詢"
                Title="用戶應收應付帳款查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="0" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="COMN" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客編" Editor="text" FieldName="CUSID" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶" Editor="text" FieldName="CUSNC" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="AR/AP" Editor="text" FieldName="CODENC" Format="" MaxLength="0" Width="70" />
                    <JQTools:JQGridColumn Alignment="right" Caption="期數" Editor="numberbox" FieldName="PERIOD" Format="" Width="40" />
                    <JQTools:JQGridColumn Alignment="right" Caption="應沖金額" Editor="numberbox" FieldName="AMT" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="right" Caption="已沖金額" Editor="numberbox" FieldName="REALAMT" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="right" Caption="未沖金額" Editor="numberbox" FieldName="DIFFAMT" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖帳日" Editor="datebox" FieldName="MDAT" Format="yyyy/mm/dd" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖帳員" Editor="text" FieldName="MUSR" Format="" MaxLength="0" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立項一" Editor="text" FieldName="COD1" Format="" MaxLength="0" Width="160" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立項二" Editor="text" FieldName="COD2" Format="" MaxLength="0" Width="160" />
                    <JQTools:JQGridColumn Alignment="left" Caption="COD3" Editor="text" FieldName="COD3" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="產生日" Editor="datebox" FieldName="CDAT" Format="yyyy/mm/dd" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢員" Editor="text" FieldName="CANCELUSR" Format="" MaxLength="0" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢原因" Editor="text" FieldName="CANCELMEMO" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="Dropdat" Format="yyyy/mm/dd" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區序號" Editor="text" FieldName="COMQ1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="沖  帳" Visible="True" OnClick="btn1Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="沖帳明細" Visible="True" OnClick="btn2Click" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="帳款明細" Visible="True" OnClick="btn3Click" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="超商未沖(Excel)" Visible="True" OnClick="btn4Click" Icon="icon-excel" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn5Click" Text="轉入超商銷帳檔" Visible="True" Icon="icon-save" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區序號" Condition="%" DataType="string" Editor="text" FieldName="COMQ1" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區名稱" Condition="%" DataType="string" Editor="text" FieldName="COMN" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="用戶名稱" Condition="%" DataType="string" Editor="text" FieldName="CUSNC" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="沖帳日起" Condition="&gt;=" DataType="string" Editor="datebox" FieldName="MDAT" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="沖帳日迄" Condition="&lt;=" DataType="string" Editor="datebox" FieldName="CANCELDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="應收應付帳款狀態" Condition="%" DataType="string" Editor="infocombobox" EditorOptions="items:[{value:' ',text:' ',selected:'false'},{value:'0',text:'全部',selected:'false'},{value:'1',text:'已沖帳',selected:'false'},{value:'2',text:'未沖帳或部份沖帳',selected:'false'},{value:'3',text:'已作廢',selected:'false'},{value:'4',text:'全部(不含作廢)',selected:'false'},{value:'5',text:'未沖金額為負',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="COD1" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
