<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT103.aspx.cs" Inherits="Template_JQuerySingle1" %>

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

        if (COMQ1 == "")
        {
            flag = false;
            $('#btnIns').hide();
            $('#btnsave').hide();
            $('#btncancel').hide();
            //設定唯讀
            setReadOnly($('#dataGridView'), true);
        }

        function InsDefault()
        {
            if (COMQ1 != "")
            {
                return COMQ1;
            }            
        }

        function LinkRT104(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            var s1 = row.LINEQ1;
            parent.addTab("用戶維護", "CBBN/RT104.aspx?COMQ1=" + ss + "&LINEQ1=" + s1);
        }

        function LinkRT1011(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            var s1 = row.LINEQ1;
            parent.addTab("設備查詢", "CBBN/RT1012.aspx?COMQ1=" + ss + "&LINEQ1=" + s1);
        }

        function LinkRT202(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            var s1 = row.LINEQ1;
            parent.addTab("主線客服單維護", "CBBN/RT202.aspx?COMQ1=" + ss + "&LINEQ1=" + s1);
        }

        function LinkRT203(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            var s1 = row.LINEQ1;
            parent.addTab("主線派工", "CBBN/RT203.aspx?COMQ1=" + ss +"&LINEQ1="+s1);
        }

        function LinkRT1031(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            var s1 = row.LINEQ1;
            
            parent.addTab("到期續約", "CBBN/RT1031.aspx?COMQ1=" + ss + "&LINEQ1=" + s1);
        }

        function LinkRT1032(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            var s1 = row.LINEQ1;

            parent.addTab("撤線作業", "CBBN/RT1032.aspx?COMQ1=" + ss + "&LINEQ1=" + s1);
        }

        function LinkRT1033(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            var s1 = row.LINEQ1;

            parent.addTab("主線資料異動記錄查詢", "CBBN/RT1033.aspx?COMQ1=" + ss + "&LINEQ1=" + s1);
        }

        //作　　廢
        function btn1Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var COMQ1 = row.COMQ1;//主線
            var LINEQ1 = row.LINEQ1;;//主線

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT103.cmdRT1031', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT1031" + "&parameters=" + COMQ1 + "," + LINEQ1 + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        //作廢返轉
        function btn2Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var COMQ1 = row.COMQ1;//主線
            var LINEQ1 = row.LINEQ1;;//主線

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT103.cmdRT1032', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT1032" + "&parameters=" + COMQ1 + "," + LINEQ1 + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        function dgOnloadSuccess() {
            if (flag)
            {
                var ss = "";
                if (LINEQ1 != "")
                {
                    ss = " AND LINEQ1 = " + LINEQ1;
                }

                $("#dataGridView").datagrid('setWhere', "COMQ1='" + COMQ1 + "'" + ss);
            }

            /*if (COMQ1 == "") {
                flag = false;
                $('#btnIns').hide();
                $('#btnsave').hide();
                $('#btncancel').hide();
                //設定唯讀
                setReadOnly($('#dataGridView'), true);
            }*/
            flag = false;
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
        function OnLoadSuccess(val) {
            try {
                var val = $('#dataFormMasterCUTID').combobox('getValue');
                $('#dataFormMasterTOWNSHIP').combobox('setWhere', "CUTID = '" + val + "'");
            }
            catch (err) {
                alert(err);
            }
        }

        function queryGrid(dg) { //查詢后添加固定條件
            if ($(dg).attr('id') == 'dataGridView') {
                var where = $(dg).datagrid('getWhere');
                
                if (where.length > 0) {
                    if (COMQ1 != "") {
                        where = where + " and COMQ1 = '" + COMQ1 + "'";
                    }

                    var sYN = $("#AGREE_Query").combobox('getValue'); //直經銷
                    
                    if (sYN=='Y')
                    {
                        where = where.replace("AGREE='Y'", "isnull(DROPDAT, '') = '' AND isnull(CANCELDAT, '') = ''");

                    }
                    if (sYN == 'N') {
                        where = where.replace("AGREE='N'", " (isnull(DROPDAT, '') <> '' OR isnull(CANCELDAT, '') <> '') ");                        
                    }
                    if (sYN == 'A') {
                        where = where.replace("AGREE='A'", "1=1");
                    }

                    var sNM = $("#APPLYNAME_Query").val(); //直經銷

                    if (sNM != "") {
                        where = where.replace("APPLYNAME='"+sNM+"'", " COMQ1 IN (SELECT COMQ1 FROM RTLessorAVSCmtyH WHERE COMN LIKE '%"+sNM+"%') ");
                    }
                }
                else {
                    if (COMQ1 != "") {
                        where = where + " COMQ1 = '" + COMQ1 + "'";
                    }
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
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT103.RTLessorAVSCmtyLine" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCmtyLine" Pagination="True" QueryTitle="查詢" EditDialogID="JQDialog1"
                Title="主線維護" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Panel" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Visible="true" Width="120" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.View_RTLessorAVSCmtyH',tableName:'View_RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線群組" Editor="text" FieldName="LINEGROUP" Format="" MaxLength="1" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線網路IP" Editor="text" FieldName="LINEIP" Format="" MaxLength="20" Visible="true" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="附掛電話" Editor="text" FieldName="LINETEL" Format="" MaxLength="15" Visible="true" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="PPPOE撥接帳號" Editor="text" FieldName="PPPOEACCOUNT" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="PPPOE撥接密碼" Editor="text" FieldName="PPPOEPASSWORD" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路ISP" Editor="inforefval" FieldName="LINEISP" Format="" MaxLength="2" Visible="true" Width="80" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'C3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線速率" Editor="inforefval" FieldName="LINERATE" Format="" Visible="true" Width="90" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'D3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" MaxLength="2" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路到位日" Editor="datebox" FieldName="HARDWAREDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="撤線日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="text" FieldName="CANCELDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="True"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="主線派工" Visible="False" Icon="icon-view" OnClick="LinkRT203" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="設備查詢" Visible="True" Icon="icon-view" OnClick="LinkRT1011" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="用戶維護" Visible="False" Icon="icon-view" OnClick="LinkRT104" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="客服案件" Visible="False" Icon="icon-view" OnClick="LinkRT202" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="到期續約" Visible="False" OnClick="LinkRT1031" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="撤線作業" Visible="False" Icon="icon-edit" OnClick="LinkRT1032" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢" Visible="True" Icon="icon-edit" OnClick="btn1Click" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢反轉" Visible="True" Icon="icon-undo" OnClick="btn2Click" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="歷史異動" Visible="False" Icon="icon-view" OnClick="LinkRT1033" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="主線速率起" Condition="&gt;=" DataType="string" Editor="inforefval" FieldName="LINERATE" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" EditorOptions="title:'主線速率',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTCode',tableName:'View_RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'D3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="主線速率迄" Condition="&lt;=" DataType="string" Editor="inforefval" FieldName="LINERATE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" EditorOptions="title:'主線速率',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTCode',tableName:'View_RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'D3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="主線申請日起" Condition="&gt;=" DataType="datetime" Editor="datebox" EditorOptions="" FieldName="APPLYDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" Format="yyyy/mm/dd" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="線路申請日迄" Condition="&lt;=" DataType="datetime" Editor="datebox" FieldName="APPLYDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" EditorOptions="" Format="yyyy/mm/dd" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="撤線日起" Condition="&gt;=" DataType="datetime" Editor="datebox" FieldName="DROPDAT" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" Format="yyyy/mm/dd" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="撤線日迄" Condition="&lt;=" DataType="datetime" Editor="datebox" FieldName="DROPDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" Format="yyyy/mm/dd" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="作廢日起" Condition="&gt;=" DataType="datetime" Editor="datebox" FieldName="CANCELDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" Format="yyyy/mm/dd" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="作廢日迄" Condition="&lt;=" DataType="datetime" Editor="datebox" FieldName="CANCELDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" Format="yyyy/mm/dd" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區名稱" Condition="=" DataType="string" Editor="text" FieldName="APPLYNAME" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="使用中" Condition="=" DataType="string" Editor="infocombobox" EditorOptions="items:[{value:'Y',text:'Y',selected:'false'},{value:'N',text:'N',selected:'false'},{value:'A',text:'全部',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="AGREE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="主線維護" Width="800px">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCmtyLine" HorizontalColumnsCount="2" RemoteName="sRT103.RTLessorAVSCmtyLine" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="True" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Width="180" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.View_RTLessorAVSCmtyH',tableName:'View_RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" Span="1" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Width="180" Span="1" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人姓名" Editor="text" FieldName="APPLYNAME" Format="" Width="180" MaxLength="30" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人身份證(統編)" Editor="text" FieldName="APPLYSOCIAL" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人連絡電話" Editor="text" FieldName="APPLYCONTACTTEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人行動電話" Editor="text" FieldName="APPLYMOBILE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址" Editor="infocombobox" FieldName="CUTID" Format="" maxlength="2" Width="180" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.RTCounty',tableName:'RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,onSelect:FilterTown,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="鄉鎮市" Editor="infocombobox" FieldName="TOWNSHIP" Format="" maxlength="10" Width="180" EditorOptions="valueField:'TOWNSHIP',textField:'TOWNSHIP',remoteName:'sRT100.RTCtyTown',tableName:'RTCtyTown',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="  " Editor="text" FieldName="STREET" Format="" maxlength="14" Width="360" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔員" Editor="inforefval" FieldName="EUSR" Format="" maxlength="6" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTEmployee',tableName:'View_RTEmployee',columns:[{field:'EMPLY',title:'編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'NAME',title:'姓名',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}],columnMatches:[],whereItems:[],valueField:'EMPLY',textField:'NAME',valueFieldCaption:'EMPLY',textFieldCaption:'NAME',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日" Editor="datebox" FieldName="EDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" maxlength="0" RowSpan="1" Span="1" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改員" Editor="infocombobox" FieldName="UUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" RowSpan="1" Span="1" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" maxlength="0" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶名稱" Editor="text" FieldName="LOANNAME" Format="" maxlength="30" Width="180" ReadOnly="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶身份證號" Editor="text" FieldName="LOANSOCIAL" Format="" maxlength="10" Width="180" ReadOnly="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶連絡電話" Editor="text" FieldName="LOANCONTACTTEL" Format="" maxlength="15" Width="180" ReadOnly="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶行動電話" Editor="text" FieldName="LOANMOBILE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="直銷業務轄區" Editor="inforefval" FieldName="AREAID" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTArea',tableName:'RTArea',columns:[],columnMatches:[],whereItems:[{field:'AREATYPE',value:'3'}],valueField:'AREAID',textField:'AREANC',valueFieldCaption:'AREAID',textFieldCaption:'AREANC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="直銷業務員" Editor="infocombobox" FieldName="SALESID" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="經銷商" Editor="infocombobox" FieldName="CONSIGNEE" Format="" maxlength="10" Width="180" EditorOptions="valueField:'cusid',textField:'shortnc',remoteName:'sRT100.V_RTConsignee',tableName:'V_RTConsignee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路ISP" Editor="inforefval" FieldName="LINEISP" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'C3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線速率" Editor="inforefval" FieldName="LINERATE" Format="" maxlength="2" Width="180" EditorOptions="title:'主線速率',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode_D3',tableName:'RTCode_D3',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'D3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="附掛電話" Editor="text" FieldName="LINETEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線網路IP" Editor="text" FieldName="LINEIP" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="光纖號碼" Editor="text" FieldName="FIBERID" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="PPPOE撥接帳號" Editor="text" FieldName="PPPOEACCOUNT" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="PPPOE撥接密碼" Editor="text" FieldName="PPPOEPASSWORD" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Width="180" maxlength="0" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路到位日" Editor="datebox" FieldName="HARDWAREDAT" Format="yyyy/mm/dd" Width="180" maxlength="0" />
                        <JQTools:JQFormColumn Alignment="left" Caption="撤線日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="180" maxlength="0" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" maxlength="0" Width="180" Span="1" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢員" Editor="infocombobox" FieldName="CANCELUSR" Format="" Width="180" maxlength="6" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註說明" Editor="textarea" FieldName="MEMO" Format="" maxlength="500" Width="500" EditorOptions="height:50" ReadOnly="False" Span="2" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="COMQ1" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="999" FieldName="LINEQ1" RemoteMethod="True" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
</html>
