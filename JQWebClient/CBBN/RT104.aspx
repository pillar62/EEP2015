<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT104.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var COMQ1 = Request.getQueryStringByName2("COMQ1");
        var LINEQ1 = Request.getQueryStringByName2("LINEQ1");
        var usr = getClientInfo('_usercode');
        
        var flag = true;
        if (COMQ1 == "") {
            flag = false;
        }

        function InsDefault() {
            if (COMQ1 != "") {
                return COMQ1;
            }
        }

        function LinkRT1041(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("維修收款", "CBBN/RT1041.aspx?CUSID=" + ss);
        }

        function btn1Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("裝機派工單資料維護", "CBBN/RT1042.aspx?CUSID=" + ss);
        }

        function btn2Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("用戶續約作業", "CBBN/RT1043.aspx?CUSID=" + ss);
        }

        function btn3Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("用戶復機作業", "CBBN/RT1044.aspx?CUSID=" + ss);
        }

        function btn4Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("用戶退租作業", "CBBN/RT1045.aspx?CUSID=" + ss);
        }

        function btn5Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("用戶應收應付帳款查詢", "CBBN/RT1046.aspx?CUSID=" + ss);
        }

        function btn6Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("用戶客服單資料維護", "CBBN/RT1047.aspx?CUSID=" + ss);
        }

        function btn7Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("設備保管收據列印", "CBBN/RT1048.aspx?CUSID=" + ss);
        }

        function btn9Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("用戶調整到期日資料維護", "CBBN/RT1049.aspx?CUSID=" + ss);
        }

        function btn10Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("用戶設備資料查詢", "CBBN/RT104A.aspx?CUSID=" + ss);
        }

        //作廢
        function btn11Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var COMQ1 = row.COMQ1;
            var LINEQ1 = row.LINEQ1;
            var CUSID = row.CUSID;
            alert(COMQ1);
            
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT104.cmdRT104B', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT104B" + "&parameters=" + COMQ1 + "," + LINEQ1 + "," + CUSID + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }

        //作廢返轉
        function btn12Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var COMQ1 = row.COMQ1;
            var LINEQ1 = row.LINEQ1;
            var CUSID = row.CUSID;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT104.cmdRT104C', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT104C" + "&parameters=" + COMQ1 + "," + LINEQ1 + "," + CUSID + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }

        function btn13Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("用戶異動資料查詢", "CBBN/RT104D.aspx?CUSID=" + ss);
        }

        function InsDefaultLINEQ1() {
            if (LINEQ1 != "") {
                return LINEQ1;
            }
            else
            {
                flag = false;
            }
        }
        function RT201CUSID(val, rowData) {
            return '<a target="_blank" href="../RT201.aspx?CUSID=' + val + '">' + val + '</a>';
        }

        function MySelect(rowIndex, rowData)
        {
            var ss = rowData.CUSID;
            if (ss == "") ss = "ZZZZZ";
            $("#V_RTLessorAVSCustFaqH").datagrid('setWhere', "RTLessorAVSCustFaqH.cusid='" + ss + "'"); //維護單 
            $("#RTLessorAVSCustCont").datagrid('setWhere', "RTLessorAVSCustCont.cusid='" + ss + "'"); //客戶續約單
            $("#RTLessorAVSCustDrop").datagrid('setWhere', "RTLessorAVSCustDrop.cusid='" + ss + "'"); //客戶退租單 
            $("#RTLessorAVSCustReturn").datagrid('setWhere', "RTLessorAVSCustReturn.cusid='" + ss + "'"); //客戶復機單 
            $("#RTLessorAVSCustAR").datagrid('setWhere', "RTLessorAVSCustAR.cusid='" + ss + "'"); //客戶應收付單 
        }

        function dgOnloadSuccess() {
            if (flag) {
                var sWhere = "COMQ1='" + COMQ1 + "'";
                if (LINEQ1 != "") {
                    sWhere = sWhere + " AND LINEQ1='" + LINEQ1 + "'"
                }

                $("#dataGridView").datagrid('setWhere', sWhere);
                $('#btnIns').show();
            }

            if (LINEQ1 == "")
            {
                $('#btnIns').hide();
                $('#btnsave').hide();
                $('#btncancel').hide();
                //設定唯讀
                setReadOnly($('#dataGridView'), true);
            }

            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            if (ss == "") ss = "ZZZZZ";

            if (flag == false)
            {
                $("#V_RTLessorAVSCustFaqH").datagrid('setWhere', "RTLessorAVSCustFaqH.cusid='" + ss + "'");
                $("#RTLessorAVSCustCont").datagrid('setWhere', "RTLessorAVSCustCont.cusid='" + ss + "'");
                $("#RTLessorAVSCustDrop").datagrid('setWhere', "RTLessorAVSCustDrop.cusid='" + ss + "'"); //客戶退租單
                $("#RTLessorAVSCustReturn").datagrid('setWhere', "RTLessorAVSCustReturn.cusid='" + ss + "'"); //客戶復機單
                $("#RTLessorAVSCustAR").datagrid('setWhere', "RTLessorAVSCustAR.cusid='" + ss + "'"); //客戶應收付單 
            }

            flag = false;
        }
        function FilterTown1(val) {
            try {
                $('#dataFormMasterTOWNSHIP1').combobox('setValue', "");
                $('#dataFormMasterTOWNSHIP1').combobox('setWhere', "CUTID = '" + val.CUTID + "'");
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
            try {
                $('#dataFormMasterTOWNSHIP3').combobox('setValue', "");
                $('#dataFormMasterTOWNSHIP3').combobox('setWhere', "CUTID = '" + val.CUTID + "'");
            }
            catch (err) {
                alert(err);
            }
        }
        function OnLoadSuccess(val) {
            try {
                var val = $('#dataFormMasterCUTID1').combobox('getValue');
                $('#dataFormMasterTOWNSHIP1').combobox('setWhere', "CUTID = '" + val + "'");
                var val = $('#dataFormMasterCUTID2').combobox('getValue');
                $('#dataFormMasterTOWNSHIP2').combobox('setWhere', "CUTID = '" + val + "'");
                var val = $('#dataFormMasterCUTID3').combobox('getValue');
                $('#dataFormMasterTOWNSHIP3').combobox('setWhere', "CUTID = '" + val + "'");
            }
            catch (err) {
                alert(err);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT104.RTLessorAVSCust" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCust" Pagination="True" QueryTitle="查詢" EditDialogID="JQDialog1"
                Title="用戶維護" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Panel" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True" OnLoadSuccess="dgOnloadSuccess" OnSelect="MySelect">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Visible="true" Width="120" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.RTLessorAVSCmtyH',tableName:'RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200"/>
                    <JQTools:JQGridColumn Alignment="right" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶代號" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶名" Editor="text" FieldName="CUSNC" Format="" MaxLength="30" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="裝機縣市" Editor="infocombobox" FieldName="CUTID2" Format="" MaxLength="2" Visible="true" Width="60" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.View_RTCounty',tableName:'View_RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="鄉鎮區" Editor="text" FieldName="TOWNSHIP2" Format="" MaxLength="10" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址" Editor="text" FieldName="RADDR2" Format="" MaxLength="60" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="連絡手機" Editor="text" FieldName="MOBILE" Format="" MaxLength="30" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案" Editor="inforefval" FieldName="CASEKIND" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTCode',tableName:'View_RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="IP(XXX.xxx.xxx.xxx)" Editor="text" FieldName="IP11" Format="" MaxLength="3" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報竣日" Editor="datebox" FieldName="DOCKETDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="STRBILLINGDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最近續約計費日" Editor="datebox" FieldName="NEWBILLINGDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日" Editor="datebox" FieldName="DUEDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="公關戶(Y)" Editor="text" FieldName="FREECODE" Format="" MaxLength="1" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" ID="btnIns" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply"
                        Text="存檔" ID="btnsave" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel"
                        Text="取消" ID="btncancel" />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="維修收款" Visible="True" OnClick="LinkRT1041" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="裝機派工" Visible="True" OnClick="btn1Click" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="續約作業" Visible="True" OnClick="btn2Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="復機作業" Visible="True" OnClick="btn3Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="退租作業" Visible="True" OnClick="btn4Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="應收應付" Visible="True" OnClick="btn5Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="客服案件" Visible="True" OnClick="btn6Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="設備保管收據列印" Visible="True" OnClick="btn7Click" Icon="icon-print" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="用戶移動" Visible="True" OnClick="btn8Click" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="調整到期" Visible="True" OnClick="btn9Click" Icon="icon-edit "/>
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="設備查詢" Visible="True" OnClick="btn10Click" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢" Visible="True" OnClick="btn11Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢反轉" Visible="True" OnClick="btn12Click" Icon="icon-undo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="歷史異動" Visible="True" OnClick="btn13Click" Icon="icon-view" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="客戶代號" Condition="%" DataType="string" Editor="text" FieldName="CUSID" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="客戶名" Condition="%" DataType="string" Editor="text" FieldName="CUSNC" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="身份證號" Condition="%" DataType="string" Editor="text" FieldName="SOCIALID" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="繳費週期" Condition="=" DataType="string" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="PAYCYCLE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="繳費方式" Condition="=" DataType="string" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="PAYTYPE" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="連絡電話" Condition="%" DataType="string" Editor="text" FieldName="CONTACTTEL" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="連絡手機" Condition="%" DataType="string" Editor="text" FieldName="MOBILE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="到期日起" Condition="&gt;=" DataType="string" Editor="datebox" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="DUEDAT" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="到期日迄" Condition="&lt;=" DataType="string" Editor="datebox" FieldName="DUEDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶維護" Width="900px">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCust" HorizontalColumnsCount="2" RemoteName="sRT104.RTLessorAVSCust" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >

                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Width="180" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.RTLessorAVSCmtyH',tableName:'RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶代號" Editor="text" FieldName="CUSID" Format="" maxlength="15" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第二戶" Editor="infocombobox" FieldName="SECONDCASE" Format="" maxlength="1" Width="180" EditorOptions="items:[{value:'Y',text:'Y',selected:'false'},{value:'N',text:'N',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公關戶(Y)" Editor="infocombobox" FieldName="FREECODE" Format="" maxlength="1" Width="180" EditorOptions="items:[{value:'Y',text:'Y',selected:'false'},{value:'N',text:'N',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶(公司)名稱" Editor="text" FieldName="CUSNC" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第一證照別" Editor="inforefval" FieldName="IDNUMBERTYPE" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'J5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="身份證字號" Editor="text" FieldName="SOCIALID" Format="" maxlength="10" Width="180" />                        
                        <JQTools:JQFormColumn Alignment="left" Caption="第二證照別" Editor="inforefval" FieldName="SECONDIDTYPE" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'L3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第二證照號碼" Editor="text" FieldName="SECONDNO" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="生日" Editor="datebox" FieldName="BIRTHDAY" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="連絡電話" Editor="text" FieldName="CONTACTTEL" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="連絡手機" Editor="text" FieldName="MOBILE" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="E-Mail" Editor="text" FieldName="EMAIL" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="戶籍(公司)地址" Editor="infocombobox" FieldName="CUTID1" Format="" maxlength="2" Width="180" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.View_RTCounty',tableName:'View_RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,onSelect:FilterTown1,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="戶籍(公司)地址" Editor="infocombobox" FieldName="TOWNSHIP1" Format="" maxlength="10" Width="180" EditorOptions="valueField:'TOWNSHIP',textField:'TOWNSHIP',remoteName:'sRT100.RTCtyTown',tableName:'RTCtyTown',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="戶籍(公司)地址" Editor="text" FieldName="RADDR1" Format="" maxlength="60" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="郵遞區號(戶籍)" Editor="text" FieldName="RZONE1" Format="" maxlength="5" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="裝機地址" Editor="infocombobox" FieldName="CUTID2" Format="" maxlength="2" Width="180" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.View_RTCounty',tableName:'View_RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,onSelect:FilterTown2,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="裝機地址" Editor="infocombobox" FieldName="TOWNSHIP2" Format="" maxlength="10" Width="180" EditorOptions="valueField:'TOWNSHIP',textField:'TOWNSHIP',remoteName:'sRT100.RTCtyTown',tableName:'RTCtyTown',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="裝機地址" Editor="text" FieldName="RADDR2" Format="" maxlength="60" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="郵遞區號(裝機)" Editor="text" FieldName="RZONE2" Format="" maxlength="5" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="帳單地址" Editor="infocombobox" FieldName="CUTID3" Format="" maxlength="2" Width="180" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.View_RTCounty',tableName:'View_RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,onSelect:FilterTown3,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="帳單地址" Editor="infocombobox" FieldName="TOWNSHIP3" Format="" maxlength="10" Width="180" EditorOptions="valueField:'TOWNSHIP',textField:'TOWNSHIP',remoteName:'sRT100.RTCtyTown',tableName:'RTCtyTown',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="帳單地址" Editor="text" FieldName="RADDR3" Format="" maxlength="60" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="郵遞區號(帳單)" Editor="text" FieldName="RZONE3" Format="" maxlength="5" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司連絡人" Editor="text" FieldName="COCONTACT" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司電話" Editor="text" FieldName="COCONTACTTEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司電話分機" Editor="text" FieldName="COTELEXT" Format="" maxlength="5" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司手機" Editor="text" FieldName="COCONTACTMOBILE" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司負責人" Editor="text" FieldName="COBOSS" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="負責人身份證字號" Editor="text" FieldName="COBOSSID" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="行業別" Editor="inforefval" FieldName="COKIND" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'J8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔人" Editor="infocombobox" FieldName="EUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日" Editor="datebox" FieldName="EDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改人" Editor="infocombobox" FieldName="UUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢員" Editor="infocombobox" FieldName="CANCELUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案" Editor="inforefval" FieldName="CASEKIND" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶速率" Editor="text" FieldName="USERRATE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="IPTV用戶別" Editor="inforefval" FieldName="CPEKIND" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'Q8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="元訊傳媒IPTV收款拆帳" Editor="infocombobox" FieldName="RCVMONEYFLAG1" Format="" maxlength="1" Width="180" EditorOptions="items:[{value:' ',text:' ',selected:'false'},{value:'Y',text:'Y',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費週期" Editor="text" FieldName="PAYCYCLE" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費方式" Editor="text" FieldName="PAYTYPE" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="當期收款金額" Editor="numberbox" FieldName="RCVMONEY" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡種類" Editor="text" FieldName="CREDITCARDTYPE" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發卡銀行" Editor="text" FieldName="CREDITBANK" Format="" maxlength="3" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="卡號" Editor="text" FieldName="CREDITCARDNO" Format="" maxlength="16" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="持卡人姓名" Editor="text" FieldName="CREDITNAME" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效截止月" Editor="text" FieldName="CREDITDUEM" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效截止年" Editor="text" FieldName="CREDITDUEY" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" maxlength="12" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="轉應收帳款日" Editor="datebox" FieldName="CDAT" Format="yyyy/mm/dd" Width="180" EditorOptions="" />
                        <JQTools:JQFormColumn Alignment="left" Caption="裝機費" Editor="numberbox" FieldName="SETMONEY" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金序號" Editor="text" FieldName="GTSERIAL" Format="" maxlength="12" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金收據列印人" Editor="text" FieldName="GTPRTUSR" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金收據列印日" Editor="datebox" FieldName="GTPRTDAT" Format="yyyy/mm/dd" Width="180" EditorOptions="" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金" Editor="numberbox" FieldName="GTMONEY" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶保管CPE設備" Editor="text" FieldName="GTEQUIP" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶保管STB設備" Editor="text" FieldName="EQUIP" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金退還日期" Editor="datebox" FieldName="GTREPAYDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="IP(XXX.xxx.xxx.xxx)" Editor="text" FieldName="IP11" Format="" maxlength="3" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="IP(xxx.XXX.xxx.xxx)" Editor="text" FieldName="IP12" Format="" maxlength="3" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="IP(xxx.xxx.XXX.xxx)" Editor="text" FieldName="IP13" Format="" maxlength="3" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="IP(xxx.xxx.xxx.XXX)" Editor="text" FieldName="IP14" Format="" maxlength="3" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶CPE Mac Address" Editor="text" FieldName="MAC" Format="" maxlength="12" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="STB IP" Editor="text" FieldName="STBIP" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="STB Mac Address" Editor="text" FieldName="STBMAC" Format="" maxlength="12" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="PPPoE 帳號" Editor="text" FieldName="PPPOEID" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="PPPoE 密碼" Editor="text" FieldName="PPPOEPW" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="報竣日" Editor="datebox" FieldName="DOCKETDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="STRBILLINGDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="最近續約計費日" Editor="datebox" FieldName="NEWBILLINGDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="可使用期數" Editor="numberbox" FieldName="PERIOD" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="調整日數" Editor="numberbox" FieldName="ADJUSTDAY" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="使用截止日" Editor="datebox" FieldName="DUEDAT" Format="yyyy/mm/dd" Width="180" EditorOptions="" />
                        <JQTools:JQFormColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" maxlength="500" Width="180" Span="2" />
                    </Columns>
                </JQTools:JQDataForm>

                <JQTools:JQDataGrid ID="dataGridDetail" runat="server" AutoApply="False" DataMember="RTLessorAVSCustReturn" Pagination="False" ParentObjectID="dataFormMaster" RemoteName="sRT104.RTLessorAVSCust" Title="明細資料" AlwaysClose="True" >
                    <Columns>
                        <JQTools:JQGridColumn Alignment="left" Caption="CUSID" Editor="text" FieldName="CUSID" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="ENTRYNO" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="APPLYDAT" Editor="datebox" FieldName="APPLYDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="PAYCYCLE" Editor="text" FieldName="PAYCYCLE" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="PERIOD" Editor="numberbox" FieldName="PERIOD" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="SECONDCASE" Editor="text" FieldName="SECONDCASE" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="AMT" Editor="numberbox" FieldName="AMT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="PAYTYPE" Editor="text" FieldName="PAYTYPE" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CREDITCARDTYPE" Editor="text" FieldName="CREDITCARDTYPE" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CREDITBANK" Editor="text" FieldName="CREDITBANK" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CREDITCARDNO" Editor="text" FieldName="CREDITCARDNO" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CREDITNAME" Editor="text" FieldName="CREDITNAME" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CREDITDUEM" Editor="text" FieldName="CREDITDUEM" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CREDITDUEY" Editor="text" FieldName="CREDITDUEY" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="REALAMT" Editor="numberbox" FieldName="REALAMT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="TARDAT" Editor="datebox" FieldName="TARDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="BATCHNO" Editor="text" FieldName="BATCHNO" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="TUSR" Editor="text" FieldName="TUSR" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="FINISHDAT" Editor="datebox" FieldName="FINISHDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CANCELDAT" Editor="datebox" FieldName="CANCELDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CANCELUSR" Editor="text" FieldName="CANCELUSR" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="MEMO" Editor="text" FieldName="MEMO" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="EDAT" Editor="datebox" FieldName="EDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="EUSR" Editor="text" FieldName="EUSR" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="UDAT" Editor="datebox" FieldName="UDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="UUSR" Editor="text" FieldName="UUSR" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="ADJUSTDAY" Editor="numberbox" FieldName="ADJUSTDAY" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="STRBILLINGDAT" Editor="datebox" FieldName="STRBILLINGDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="MAXENTRYNO" Editor="numberbox" FieldName="MAXENTRYNO" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="復機費" Editor="numberbox" FieldName="RETURNMONEY" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="方案類型(KIND='O9')" Editor="text" FieldName="CASEKIND" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="收款日期(復機)" Editor="datebox" FieldName="RCVMONEYDAT" Format="" Width="120" />
                    </Columns>
                    <RelationColumns>
                        <JQTools:JQRelationColumn FieldName="CUSID" ParentFieldName="CUSID" />
                    </RelationColumns>
                    <TooItems>
                        <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                        <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="更改" />
                        <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" />
                    </TooItems>
                </JQTools:JQDataGrid>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="COMQ1" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefaultLINEQ1" FieldName="LINEQ1" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="自動編號" FieldName="CUSID" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today" FieldName="EDAT" RemoteMethod="True" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
                <JQTools:JQDefault ID="defaultDetail" runat="server" BindingObjectID="dataGridDetail" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateDetail" runat="server" BindingObjectID="dataGridDetail" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
        <p>
            <JQTools:JQDataGrid ID="V_RTLessorAVSCustFaqH" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RTLessorAVSCustFaqH" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT104.V_RTLessorAVSCustFaqH" RowNumbers="True" Title="客戶服務單" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="CUSID" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="20">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="客服單號" Editor="text" FieldName="FAQNO" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90" FormatScript="" DrillObjectID="JQDrillDown1">
                        <DrillFields>
                            <JQTools:JQDrillDownFields FieldName="FAQNO" />
                        </DrillFields>
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="來電日" Editor="text" FieldName="RCVDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="類型" Editor="text" FieldName="CODENC" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="40">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="摘要" Editor="text" FieldName="memo15" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="140">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="連絡電話" Editor="text" FieldName="CONTACTTEL" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="行動電話" Editor="text" FieldName="MOBILE" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="派工日" Editor="text" FieldName="SNDWORK" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="派工員" Editor="text" FieldName="CUSNC1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="SNDPRTNO" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90" DrillObjectID="JQDrillDown2">
                        <DrillFields>
                            <JQTools:JQDrillDownFields FieldName="SNDPRTNO" />
                        </DrillFields>
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="派工結案" Editor="text" FieldName="SNDCLOSEDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="客服回覆" Editor="text" FieldName="CALLBACKDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="回覆員" Editor="text" FieldName="CUSNC2" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="客服結案" Editor="text" FieldName="FINISHDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="結案員" Editor="text" FieldName="CUSNC3" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="處理天數" Editor="text" FieldName="PROCESSDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
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
            <JQTools:JQDataGrid ID="RTLessorAVSCustCont" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RTLessorAVSCustCont" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT104.V_RTLessorAVSCustCont" RowNumbers="True" Title="客戶續約單" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
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
            <JQTools:JQDataGrid ID="RTLessorAVSCustDrop" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RTLessorAVSCustDrop" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT104.V_RTLessorAVSCustDrop" RowNumbers="True" Title="客戶退租單" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="CUSID" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="false" Width="20">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="32">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="退租種類" Editor="text" FieldName="CODENC" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="120">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="退租申請日" Editor="text" FieldName="APPLYDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="預定退租日" Editor="text" FieldName="ENDDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="退租結案日" Editor="text" FieldName="FINISHDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="cusnc1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="拆機工單" Editor="text" FieldName="SNDPRTNO" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="轉拆機單日" Editor="text" FieldName="SNDWORK" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="拆機結案日" Editor="text" FieldName="SNDWORKCLOSE" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="處理天數" Editor="text" FieldName="processdat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
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
            <JQTools:JQDataGrid ID="RTLessorAVSCustReturn" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RTLessorAVSCustReturn" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT104.V_RTLessorAVSCustReturn" RowNumbers="True" Title="客戶復機單" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="CUSID" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="false" Width="30">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="32">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="復機申請日" Editor="text" FieldName="APPLYDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70"  Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="text" FieldName="STRBILLINGDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費週期" Editor="text" FieldName="PAYCYCLE" Frozen="False" IsNvarChar="False" MaxLength="2" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="64">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費方式" Editor="text" FieldName="PAYTYPE" Frozen="False" IsNvarChar="False" MaxLength="2" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="64">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="可用期數" Editor="text" FieldName="PERIOD" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="第二戶" Editor="text" FieldName="SECONDCASE" Frozen="False" IsNvarChar="False" MaxLength="1" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="48">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="應收金額" Editor="text" FieldName="AMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="實收金額" Editor="text" FieldName="REALAMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="轉應收帳款日" Editor="text" FieldName="TARDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="100">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="復機結案日" Editor="text" FieldName="FINISHDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
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
            <JQTools:JQDataGrid ID="RTLessorAVSCustAR" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RTLessorAVSCustAR" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT104.V_RTLessorAVSCustAR" RowNumbers="True" Title="客戶應收付帳款" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶編號G+YYMMDD001(YY西元後二位)" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="30">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
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
        </p>
        <p>
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
