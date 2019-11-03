<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT101.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        function LinkRT103(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;            
            parent.addTab("主線維護", "CBBN/RT103.aspx?COMQ1=" + ss);
        }

        function LinkRT104(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            parent.addTab("用戶維護", "CBBN/RT104.aspx?COMQ1=" + ss);
        }

        function LinkRT1011(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            parent.addTab("設備查詢", "CBBN/RT1011.aspx?COMQ1=" + ss);
        }

        function LinkRT106(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            parent.addTab("社區合約資料維護", "CBBN/RT106.aspx?COMQ1=" + ss);
        }

        function FilterTown(val) {
            try {
                $('#dataFormMasterTOWNSHIP').combobox('setValue', "");
                $('#dataFormMasterTOWNSHIP').combobox('setWhere', "CUTID = '" + val.CUTID + "'");
            }
            catch (err) {
                alert(err);
            }
        }

        function FilterTown2(val) {
            try {
                $('#dataFormMasterTOWNSHIP2').combobox('setValue', "");
                $('#dataFormMasterTOWNSHIP2').combobox('setWhere', "CUTID = '" + val.CUTID + "'");
            }
            catch (err) {
                alert(err);
            }
        }

        function FilterTown3(val) {
            /*
            try {
                $('#dataFormMasterTOWNSHIP3').combobox('setValue', "");
                $('#dataFormMasterTOWNSHIP3').combobox('setWhere', "CUTID = '" + val.CUTID + "'");
            }
            catch (err) {
                alert(err);
            }
            */
        }

        function DownloadFile()
        {
            //下載上傳的檔案
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.UNAGREEDESC;
            if (ss != "")
            {
                window.location.href = "../handler/JqFileHandler2.ashx?File=" + ss;
            }
            else
            {
                alert("未指定合約檔案!!");
            }
            
        }

        function OnLoadSuccess(val) {
            try {
                //var val = $('#dataFormMasterCUTID').combobox('getValue');
                //$('#dataFormMasterTOWNSHIP').combobox('setWhere', "CUTID = '" + val + "'");
                //var val = $('#dataFormMasterCUTID2').combobox('getValue');
                //$('#dataFormMasterTOWNSHIP2').combobox('setWhere', "CUTID = '" + val + "'");
                //$('#dataFormMasterBUILDTYPE').combobox('setWhere', "KIND = 'C2'");
                //$('#dataGridViewBUILDTYPE').combobox('setWhere', "KIND = 'C2'");
                //$("#dataFormMasterCUTID").css('background-color', "#FEFFAF"); //text設定背景色
                //$("#dataFormMasterCUTID").next(".datebox").find(".combo-text").css('background-color', "#FEFFAF"); //datebox 設定背景色
                $("#dataFormMasterCUTID").combobox('textbox').css('background-color', "#FEFFAF"); //combobox 設定背景色
                //$("#dataFormMasterCUTID").data("inforefval").refval.find("input.refval-text").css('background-color', "#FEFFAF"); //refval設定背景色
            }
            catch (err) {
                alert(err);
            }
        }

        function queryGrid(dg) { //查詢后添加固定條件
            if ($(dg).attr('id') == 'dataGridView') {
                var where = $(dg).datagrid('getWhere');
                var ssub = "";
                if (where != "")
                {
                    where = " 1=1 ";
                    COMN = $("#COMN_Query").val(); //社區名稱
                    DTS = $("#DTS_Query").datebox("getValue");; //規模戶數
                    DTE = $("#DTE_Query").datebox("getValue");; //規模戶數
                    Q1S = $("#Q1S_Query").val(); //規模戶數
                    Q1E = $("#Q1E_Query").val(); //規模戶數
                    Q2S = $("#Q2S_Query").val(); //規模戶數
                    Q2E = $("#Q2E_Query").val(); //規模戶數
                    ADDR = $("#ADDR_Query").val(); //社區地址

                    if (COMN!="")
                        where = where + " and A.COMN like '%" + COMN + "%'";

                    if (ADDR != "")
                        where = where + " and C.CUTNC+A.TOWNSHIP+A.RADDR like '%" + ADDR + "%'";
                    
                    if (DTS != "" || DTE != "")
                    {
                        ssub = " AND A.COMQ1 IN (SELECT COMQ1 FROM RTLessorAVSCmtyLine WHERE 1=1 "
                        if (DTS != "")
                            ssub = ssub + " and APPLYDAT >= '" + DTS + "'";
                        if (DTE != "")
                            ssub = ssub + " and APPLYDAT <= '" + DTE + "'";
                        ssub = ssub + " ) ";

                        where = where + ssub;
                    }
                    //
                    if (Q1S != "")
                        where = where + " and B.QT_CUST >= " + Q1S;
                    if (Q1E != "")
                        where = where + " and B.QT_CUST <= " + Q1E;
                    //
                    if (Q2S != "")
                        where = where + " and A.COMCNT >= " + Q2S;
                    if (Q2E != "")
                        where = where + " and A.COMCNT <= " + Q2E;
                }
                $(dg).datagrid('setWhere', where);
            }
        }

        function btnReloadClick() {
            //$("#dataGridView").datagrid("setWhere", "");
            $('#dataGridView').datagrid('reload');
        }
    </script>
    <style>
        #dataFormMasterCONSTRUCTIONMEMO{
            height: 350px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT101.RTLessorAVSCmtyH" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCmtyH" Pagination="True" QueryTitle="查詢條件" EditDialogID="JQDialog1"
                Title="社區查詢" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Panel" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False" OnLoadSuccess="OnLoadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="社區序號" Editor="numberbox" FieldName="COMQ1" Format="" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="COMN" Format="" MaxLength="30" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="郵遞區號" Editor="text" FieldName="RZONE" Format="" MaxLength="5" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="縣市" Editor="infocombobox" FieldName="CUTID" Format="" MaxLength="2" Visible="true" Width="60" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.RTCounty',tableName:'RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="鄉鎮" Editor="text" FieldName="TOWNSHIP" Format="" MaxLength="10" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區地址" Editor="text" FieldName="RADDR" Format="" MaxLength="60" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區聯絡電話" Editor="text" FieldName="CONTACTTEL" Format="" MaxLength="30" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="規模戶數" Editor="numberbox" FieldName="COMCNT" Format="" MaxLength="0" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="戶數" Editor="text" FieldName="QT_CUST" Format="" MaxLength="50" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線數" Editor="text" FieldName="QT_LINE" Format="" MaxLength="60" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="合約" Editor="text" FieldName="REMITNO" Format="" MaxLength="7" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建檔日" Editor="datebox" FieldName="EDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="個案別" Editor="inforefval" FieldName="COMTYPE" Visible="False" Width="80" EditorOptions="title:'個案別',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="合約檔案" Editor="text" FieldName="UNAGREEDESC" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="地址" Editor="text" FieldName="ADDR" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="False"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="主線維護" Visible="True" OnClick="LinkRT103" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="社區用戶" Visible="True" Icon="icon-view" OnClick="LinkRT104" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="設備查詢" Visible="True" Icon="icon-view" OnClick="LinkRT1011" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="合約維護" Visible="False" Icon="icon-edit" OnClick="LinkRT106" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-view" ItemType="easyui-linkbutton" OnClick="DownloadFile" Text="開啟合約檔" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btnReloadClick" Text="資料更新" Visible="True" Icon="icon-reload" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區名稱" Condition="%%" DataType="string" Editor="text" FieldName="COMN" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="開通日起" Condition="&gt;=" DataType="datetime" Editor="datebox" FieldName="DTS" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="150" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="開通日迄" Condition="&lt;=" DataType="datetime" Editor="datebox" FieldName="DTE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="150" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="寬頻使用戶數起" Condition="&gt;=" DataType="string" Editor="text" FieldName="Q1S" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="寬頻使用戶數迄" Condition="&lt;=" DataType="string" Editor="text" FieldName="Q1E" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區總戶數起" Condition="&gt;=" DataType="string" Editor="text" FieldName="Q2S" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區總戶數迄" Condition="&lt;=" DataType="string" Editor="text" FieldName="Q2E" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區地址" Condition="%%" DataType="string" Editor="text" FieldName="ADDR" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />                                    
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="社區查詢" Width="960px" DialogLeft="20px" HorizontalAlign="Left">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCmtyH" HorizontalColumnsCount="3" RemoteName="sRT101.RTLessorAVSCmtyH" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" OnLoadSuccess="OnLoadSuccess" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="numberbox" FieldName="COMQ1" Format="" Width="180" Visible="True" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="COMN" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區地址" Editor="infocombobox" FieldName="CUTID" Format="" maxlength="2" Width="180" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.RTCounty',tableName:'RTCounty',pageSize:'-1',checkData:true,selectOnly:false,cacheRelationText:false,panelHeight:200" NewRow="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="鄉鎮市" Editor="inforefval" FieldName="TOWNSHIP" Format="" maxlength="10" Width="180" EditorOptions="title:'鄉鎮市',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCtyTown',tableName:'RTCtyTown',columns:[],columnMatches:[{field:'RZONE',value:'ZIP'}],whereItems:[{field:'CUTID',value:'row[CUTID]'}],valueField:'TOWNSHIP',textField:'TOWNSHIP',valueFieldCaption:'鄉鎮市',textFieldCaption:'TOWNSHIP',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="郵遞區號" Editor="text" FieldName="RZONE" Format="" maxlength="5" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="  " Editor="text" FieldName="RADDR" Format="" maxlength="60" Width="430" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="可供裝範圍" Editor="text" FieldName="RADDR3" Format="" maxlength="60" Width="180" Span="1" />
                        <JQTools:JQFormColumn Alignment="left" Caption="規模戶數" Editor="numberbox" FieldName="COMCNT" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="合約簽約日" Editor="datebox" FieldName="AGREEDAT" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="是否可建置" Editor="infocombobox" FieldName="AGREE" Format="" maxlength="1" Width="180" Visible="False" Span="1" EditorOptions="items:[{value:'Y',text:'Y',selected:'false'},{value:'N',text:'N',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="合約查詢" Editor="infofileupload" EditorOptions="filter:'',isAutoNum:true,upLoadFolder:'download',showButton:true,showLocalFile:false,fileSizeLimited:'50000'" FieldName="UNAGREEDESC" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="80" />
                        <JQTools:JQFormColumn Alignment="left" Caption="機房架構" Editor="inforefval" FieldName="TELCOMBOX" Format="" maxlength="4" Width="180" Visible="True" EditorOptions="title:'機房架構',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTCode',tableName:'View_RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'R2'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區聯絡人" Editor="text" FieldName="CONTACT" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區聯絡電話" Editor="text" FieldName="CONTACTTEL" Format="" Width="180" MaxLength="30" />
                        <JQTools:JQFormColumn Alignment="left" Caption="匯款銀行" Editor="inforefval" FieldName="REMITBANK" Format="" maxlength="3" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,remoteName:'sRT100.RTBank',tableName:'RTBank',columns:[],columnMatches:[],whereItems:[],valueField:'HEADNO',textField:'HEADNC',valueFieldCaption:'銀行代號',textFieldCaption:'銀行名稱',cacheRelationText:true,checkData:true,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="匯款分行" Editor="inforefval" FieldName="BANKBRANCH" Format="" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,remoteName:'sRT100.RTBankBranch',tableName:'RTBankBranch',columns:[{field:'BRANCHNO',title:'分行代號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'BRANCHNC',title:'分行名稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}],columnMatches:[],whereItems:[{field:'HEADNO',value:'row[REMITBANK]'}],valueField:'BRANCHNO',textField:'BRANCHNC',valueFieldCaption:'分行代號',textFieldCaption:'分行名稱',cacheRelationText:true,checkData:true,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" MaxLength="4" />
                        <JQTools:JQFormColumn Alignment="left" Caption="匯款帳號" Editor="text" FieldName="REMITACCOUNT" Format="" Width="180" MaxLength="15" />
                        <JQTools:JQFormColumn Alignment="left" Caption="匯款戶名" Editor="text" FieldName="REMITNAME" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="支票抬頭" Editor="text" FieldName="CHECKTITLE" Format="" maxlength="60" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="支票寄送地址" Editor="infocombobox" FieldName="CCUTID" Format="" Width="180" MaxLength="2" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.View_RTCounty',tableName:'View_RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" NewRow="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="鄉鎮市" Editor="inforefval" FieldName="CTOWNSHIP" Format="" maxlength="10" Width="180" EditorOptions="title:'鄉鎮市',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTCtyTown',tableName:'View_RTCtyTown',columns:[],columnMatches:[{field:'CZONE',value:'ZIP'}],whereItems:[{field:'CUTID',value:'row[CCUTID]'}],valueField:'TOWNSHIP',textField:'TOWNSHIP',valueFieldCaption:'鄉鎮市',textFieldCaption:'鄉鎮市',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="郵遞區號" Editor="text" FieldName="CZONE" Width="80" MaxLength="0" Span="1" />
                        <JQTools:JQFormColumn Alignment="left" Caption="  " Editor="text" FieldName="CADDR" Format="" maxlength="60" Width="430" ReadOnly="False" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔員" Editor="text" FieldName="EUSR" Format="" maxlength="6" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日" Editor="datebox" FieldName="EDAT" Format="" Width="180" MaxLength="0" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改員" Editor="text" FieldName="UUSR" Format="" Width="180" MaxLength="6" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" maxlength="0" Width="180" Format="" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="個案別" Editor="inforefval" EditorOptions="title:'方案別',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="COMTYPE" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="False" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="工程備註" Editor="textarea" FieldName="CONSTRUCTIONMEMO" MaxLength="0" NewRow="True" ReadOnly="False" RowSpan="50" Span="3" Visible="True" Width="700" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="999999" FieldName="COMQ1" RemoteMethod="True" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="7" FieldName="COMTYPE" RemoteMethod="True" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>
</html>
