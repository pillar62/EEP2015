<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT301.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        function MySelect(rowIndex, rowData) {
            if (flag == false) {
                var ss = rowData.CUSID;
                $("#RTLessorAVSCustCont").datagrid('setWhere', "RTLessorAVSCustCont.cusid='" + ss + "'"); //客戶續約單
                $("#RTLessorAVSCustAR").datagrid('setWhere', "RTLessorAVSCustAR.cusid='" + ss + "'"); //客戶應收付單 
            }
        }

        function btnRT205Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            var comq1 = row.COMQ1;
            var linq1 = row.LINEQ1;
            var comtype = '7';

            parent.addTab("用戶客服單資料維護-RT205", "CBBN/RT205.aspx?cusid=" + ss + "&comq1=" + comq1 + "&lineq1=" + linq1 + "&comtype=" + comtype);
        }

        function queryGrid(dg) { //查詢后添加固定條件
            if ($(dg).attr('id') == 'dataGridMaster') {
                var where = $(dg).datagrid('getWhere');

                if (where != "") {
                    where = " 1=1 ";
                    paycycle = $("#paycycle_Query").refval('getValue'); //繳款周期
                    payTYPE = $("#payTYPE_Query").refval('getValue'); //繳款方式
                    SOCIALID = $("#SOCIALID_Query").val(); //身分證
                    FINISHDAT = $("#FINISHDAT_Query").datebox('getValue'); //到期日起
                    DUEDAT = $("#DUEDAT_Query").datebox('getValue'); //到期日迄
                    COMN = $("#COMN_Query").val(); //社區名稱
                    CNM = $("#CNM_Query").val(); //用戶名稱
                    ADR = $("#LINEIP_Query").val(); //裝機地址

                    //
                    if (paycycle != "")
                        where = where + " and a.paycycle = '" + paycycle + "'";
                    if (payTYPE != "")
                        where = where + " and a.payTYPE = '" + payTYPE + "'";
                    if (FINISHDAT != "")
                        where = where + " and a.duedat >= '" + FINISHDAT + "'";
                    if (DUEDAT != "")
                        where = where + " and a.duedat <= '" + DUEDAT + "'";
                    if (CNM != "")
                        where = where + " and A.CUSNC like  '%" + CNM + "%' ";
                    if (ADR != "")
                        where = where + " and A.RADDR2 like  '%" + ADR + "%' ";
                    if (COMN != "")
                        where = where + " and C.COMN like  '%" + COMN + "%' ";
                }
                $(dg).datagrid('setWhere', where);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT301.cmdRT301" runat="server" AutoApply="True"
                DataMember="cmdRT301" Pagination="True" QueryTitle="請輸入查詢條件"
                Title="應退租或續約用戶查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="False" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" OnSelect="MySelect">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="社區序號" Editor="numberbox" FieldName="COMQ1" Format="" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶代號" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區主線" Editor="text" FieldName="COMLINE" Format="" MaxLength="0" Width="72" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="COMN" Format="" MaxLength="0" Width="120" Sortable="True" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶名稱" Editor="text" FieldName="CNM" Format="" MaxLength="0" Width="90" Sortable="True" />
                    <JQTools:JQGridColumn Alignment="left" Caption="週期" Editor="text" FieldName="CODENC" Format="" MaxLength="0" Width="64" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳款" Editor="text" FieldName="CODENC1" Format="" MaxLength="0" Width="64" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡電話" Editor="text" FieldName="TEL" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Width="82" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Width="82" Sortable="True" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費" Editor="datebox" FieldName="STRBILLINGDAT" Format="yyyy/mm/dd" Width="82" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最近續約日" Editor="datebox" FieldName="newBILLINGDAT" Format="yyyy/mm/dd" Width="82" />
                    <JQTools:JQGridColumn Alignment="right" Caption="調整日數" Editor="numberbox" FieldName="adjustday" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日" Editor="datebox" FieldName="DUEDAT" Format="yyyy/mm/dd" Width="82" Sortable="True" />
                    <JQTools:JQGridColumn Alignment="left" Caption="公關" Editor="text" FieldName="FREECODE" Format="" MaxLength="1" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="82" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="82" />
                    <JQTools:JQGridColumn Alignment="right" Caption="期數" Editor="numberbox" FieldName="PERIOD" Format="" Width="88" />
                    <JQTools:JQGridColumn Alignment="right" Caption="可用日數" Editor="numberbox" FieldName="validdat" Format="" Width="88" Sortable="True" />
                    <JQTools:JQGridColumn Alignment="right" Caption="最近收款額" Editor="numberbox" FieldName="rcvmoney" Format="" Width="88" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="客服案件" Visible="True" Icon="icon-view" OnClick="btnRT205Click" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="繳款週期" Condition="%" DataType="string" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="paycycle" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="繳款方式" Condition="%" DataType="string" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'編號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="payTYPE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="身份證/統編" Condition="%" DataType="string" Editor="text" FieldName="SOCIALID" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="到期日起" Condition="%" DataType="string" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="到期日迄" Condition="%" DataType="string" Editor="datebox" FieldName="DUEDAT" Format="yyyy/mm/dd" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區名稱" Condition="%" DataType="string" Editor="text" FieldName="COMN" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="裝機地址" Condition="%" DataType="string" Editor="text" FieldName="LINEIP" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="用戶名稱" Condition="%" DataType="string" Editor="text" FieldName="CNM" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

        <p>
            <JQTools:JQDataGrid ID="RTLessorAVSCustCont" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="False" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RTLessorAVSCustCont" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT104.V_RTLessorAVSCustCont" RowNumbers="True" Title="客戶續約單" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶序號" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="30">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="32">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="續約申請日" Editor="text" FieldName="APPLYDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="text" FieldName="STRBILLINGDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費週期" Editor="text" FieldName="PAYCYCLE" Frozen="False" IsNvarChar="False" MaxLength="2" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="40">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費方式" Editor="text" FieldName="PAYTYPE" Frozen="False" IsNvarChar="False" MaxLength="2" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="40">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="可用期數" Editor="text" FieldName="PERIOD" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="60">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="第二戶" Editor="text" FieldName="SECONDCASE" Frozen="False" IsNvarChar="False" MaxLength="1" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="60">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="應收金額" Editor="text" FieldName="AMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="實收金額" Editor="text" FieldName="REALAMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款日" Editor="text" FieldName="TARDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="續約結案日期" Editor="text" FieldName="FINISHDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="處理天數" Editor="text" FieldName="PROCESSDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="64">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Enabled="True" Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="Insert" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="Update" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="Delete" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="Apply" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="Cancel" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="Export" Visible="False" />
                </TooItems>
            </JQTools:JQDataGrid>
            <JQTools:JQDataGrid ID="RTLessorAVSCustAR" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="False" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RTLessorAVSCustAR" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT104.V_RTLessorAVSCustAR" RowNumbers="True" Title="客戶應收付帳款" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="True" Visible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶編號G+YYMMDD001(YY西元後二位)" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="30">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90" DrillObjectID="">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="使用期間(月)" Editor="text" FieldName="PERIOD" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="付款金額" Editor="text" FieldName="AMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="已沖消金額" Editor="text" FieldName="REALAMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="未沖金額" Editor="text" FieldName="DIFFAMT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立項一" Editor="text" FieldName="COD1" Frozen="False" IsNvarChar="False" MaxLength="30" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="160">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立項二" Editor="text" FieldName="COD2" Frozen="False" IsNvarChar="False" MaxLength="30" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="160">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="沖帳日" Editor="text" FieldName="MDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Enabled="True" Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="Insert" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="Update" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="Delete" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="Apply" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="Cancel" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="Export" Visible="False" />
                </TooItems>
            </JQTools:JQDataGrid>
            <JQTools:JQDrillDown ID="JQDrillDown1" runat="server" DataMember="RTLessorAVSCustFaqH" FormCaption="客戶服務單" FormName="~/CBBN/RT201.aspx" OpenMode="NewTab" RemoteName="sRT201.RTLessorAVSCustFaqH" ReportName="客戶服務單">
                <KeyFields>
                    <JQTools:JQDrillDownKeyFields FieldName="FAQNO" />
                </KeyFields>
            </JQTools:JQDrillDown>
            <JQTools:JQDrillDown ID="JQDrillDown2" runat="server" DataMember="RTLessorAVSCmtyLineSNDWORK" FormCaption="派工資料維護" FormName="~/CBBN/RT1011.aspx" RemoteName="sRT203.RTLessorAVSCmtyLineSNDWORK" OpenMode="NewTab">
                <KeyFields>
                    <JQTools:JQDrillDownKeyFields FieldName="PRTNO" />
                </KeyFields>
            </JQTools:JQDrillDown>
        </p>

    </form>
</body>
</html>
