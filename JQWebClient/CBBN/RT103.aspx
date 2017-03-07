<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT103.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var COMQ1 = Request.getQueryStringByName2("COMQ1");
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
            parent.addTab("設備查詢", "CBBN/RT1011.aspx?COMQ1=" + ss + "&LINEQ1=" + s1);
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

        function dgOnloadSuccess() {
            if (flag)
            {
                $("#dataGridView").datagrid('setWhere', "COMQ1='" + COMQ1 + "'");
            }
            if (COMQ1 == "") {
                flag = false;
                $('#btnIns').hide();
                $('#btnsave').hide();
                $('#btncancel').hide();
                //設定唯讀
                setReadOnly($('#dataGridView'), true);
            }
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
            if ($(dg).attr('id') == 'dataGridMaster') {
                var where = $(dg).datagrid('getWhere');
                if (where.length > 0) {
                    if (COMQ1 != "") {
                        where = where + " and COMQ1 = '" + COMQ1 + "'";
                    }
                }
                else {
                    if (COMQ1 != "") {
                        where = " COMQ1 = '" + COMQ1 + "'";
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
                DataMember="RTLessorAVSCmtyLine" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="主線維護" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="True" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Visible="true" Width="120" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.RTLessorAVSCmtyH',tableName:'RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線群組" Editor="text" FieldName="LINEGROUP" Format="" MaxLength="1" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線網路IP" Editor="text" FieldName="LINEIP" Format="" MaxLength="20" Visible="true" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="附掛電話" Editor="text" FieldName="LINETEL" Format="" MaxLength="15" Visible="true" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路ISP" Editor="inforefval" FieldName="LINEISP" Format="" MaxLength="2" Visible="true" Width="80" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'C3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路IP種類" Editor="inforefval" FieldName="LINEIPTYPE" Format="" MaxLength="2" Visible="true" Width="90" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線速率" Editor="inforefval" FieldName="LINERATE" Format="" MaxLength="2" Visible="true" Width="90" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'D3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="right" Caption="IP數量" Editor="numberbox" FieldName="IPCNT" Format="" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路到位日" Editor="datebox" FieldName="HARDWAREDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線測通日" Editor="datebox" FieldName="ADSLAPPLYDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="撤線日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" ID="btnIns" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply"
                        Text="存檔" ID="btnsave" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel"
                        Text="取消" ID="btncancel"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="主線派工" Visible="True" Icon="icon-view" OnClick="LinkRT203" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="設備查詢" Visible="True" Icon="icon-view" OnClick="LinkRT1011" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="用戶維護" Visible="True" Icon="icon-view" OnClick="LinkRT104" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="客服案件" Visible="True" Icon="icon-view" OnClick="LinkRT202" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="到期續約" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="撤線作業" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="做廢" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢反轉" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="歷史異動" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="主線維護" Width="800px">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCmtyLine" HorizontalColumnsCount="2" RemoteName="sRT103.RTLessorAVSCmtyLine" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="True" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Width="180" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.View_RTLessorAVSCmtyH',tableName:'View_RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" Span="1" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Width="20" Span="1" />
                        <JQTools:JQFormColumn Alignment="left" Caption="收件日" Editor="datebox" FieldName="RCVDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線群組" Editor="text" FieldName="LINEGROUP" Format="" maxlength="1" Width="30" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人姓名" Editor="text" FieldName="APPLYNAME" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人身份證(統編)" Editor="text" FieldName="APPLYSOCIAL" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人連絡電話" Editor="text" FieldName="APPLYCONTACTTEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人行動電話" Editor="text" FieldName="APPLYMOBILE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(縣市)" Editor="infocombobox" FieldName="CUTID" Format="" maxlength="2" Width="180" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.RTCounty',tableName:'RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,onSelect:FilterTown,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(鄉鎮)" Editor="infocombobox" FieldName="TOWNSHIP" Format="" maxlength="10" Width="180" EditorOptions="valueField:'TOWNSHIP',textField:'TOWNSHIP',remoteName:'sRT100.RTCtyTown',tableName:'RTCtyTown',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(村/里)" Editor="text" FieldName="VILLAGE" Format="" maxlength="10" Width="180" RowSpan="1" Span="1" />
                        <JQTools:JQFormColumn Alignment="left" Caption=" " Editor="infocombobox" FieldName="COD1" Format="" maxlength="2" Width="180" EditorOptions="items:[{value:'村',text:'村',selected:'true'},{value:'里',text:'里',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" RowSpan="1" Span="1" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(鄰)" Editor="text" FieldName="NEIGHBOR" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption=" " Editor="infocombobox" FieldName="COD2" Format="" maxlength="2" Width="180" EditorOptions="items:[{value:'鄰',text:'鄰',selected:'true'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(路/街)" Editor="text" FieldName="STREET" Format="" maxlength="14" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption=" " Editor="infocombobox" FieldName="COD3" Format="" maxlength="2" Width="180" EditorOptions="items:[{value:'路',text:'路',selected:'true'},{value:'街',text:'街',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(段)" Editor="text" FieldName="SEC" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption=" " Editor="infocombobox" FieldName="COD4" Format="" maxlength="2" Width="180" EditorOptions="items:[{value:'段',text:'段',selected:'true'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(巷)" Editor="text" FieldName="LANE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption=" " Editor="infocombobox" FieldName="COD5" Format="" maxlength="2" Width="180" EditorOptions="items:[{value:'巷',text:'巷',selected:'true'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(弄)" Editor="text" FieldName="ALLEYWAY" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption=" " Editor="infocombobox" FieldName="COD7" Format="" maxlength="2" Width="180" EditorOptions="items:[{value:'弄',text:'弄',selected:'true'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(號)" Editor="text" FieldName="NUM" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption=" " Editor="infocombobox" FieldName="COD8" Format="" maxlength="2" Width="180" EditorOptions="items:[{value:'號',text:'號',selected:'true'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(樓)" Editor="text" FieldName="FLOOR" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption=" " Editor="infocombobox" FieldName="COD9" Format="" maxlength="2" Width="180" EditorOptions="items:[{value:'樓',text:'樓',selected:'true'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(室)" Editor="text" FieldName="ROOM" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption=" " Editor="infocombobox" FieldName="COD10" Format="" maxlength="2" Width="180" EditorOptions="items:[{value:'室',text:'室',selected:'true'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(部落)" Editor="text" FieldName="TOWN" Format="" maxlength="12" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption=" " Editor="infocombobox" FieldName="COD6" Format="" maxlength="4" Width="180" EditorOptions="items:[{value:'部落',text:'部落',selected:'true'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="可供裝範圍" Editor="text" FieldName="SUPPLYRANGE" Format="" maxlength="100" Width="300" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔員" Editor="inforefval" FieldName="EUSR" Format="" maxlength="6" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTEmployee',tableName:'View_RTEmployee',columns:[{field:'EMPLY',title:'編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'NAME',title:'姓名',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}],columnMatches:[],whereItems:[],valueField:'EMPLY',textField:'NAME',valueFieldCaption:'EMPLY',textFieldCaption:'NAME',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日" Editor="datebox" FieldName="EDAT" Format="yyyy/mm/dd" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改員" Editor="infocombobox" FieldName="UUSR" Format="" maxlength="6" Width="180" Span="1" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" Width="180" RowSpan="1" Span="1" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶名稱" Editor="text" FieldName="LOANNAME" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶身份證號" Editor="text" FieldName="LOANSOCIAL" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶連絡電話" Editor="text" FieldName="LOANCONTACTTEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶行動電話" Editor="text" FieldName="LOANMOBILE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="直銷業務轄區" Editor="inforefval" FieldName="AREAID" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTArea',tableName:'RTArea',columns:[],columnMatches:[],whereItems:[{field:'AREATYPE',value:'3'}],valueField:'AREAID',textField:'AREANC',valueFieldCaption:'AREAID',textFieldCaption:'AREANC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="直銷業務員" Editor="infocombobox" FieldName="SALESID" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="經銷商" Editor="infocombobox" FieldName="CONSIGNEE" Format="" maxlength="10" Width="180" EditorOptions="valueField:'cusid',textField:'shortnc',remoteName:'sRT100.V_RTConsignee',tableName:'V_RTConsignee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="二線負責人" Editor="infocombobox" FieldName="DEVELOPERID" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="可推行方案" Editor="inforefval" FieldName="SELECTCASE" Format="" maxlength="30" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路ISP" Editor="inforefval" FieldName="LINEISP" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'C3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路IP種類" Editor="inforefval" FieldName="LINEIPTYPE" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線速率" Editor="inforefval" FieldName="LINERATE" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'C3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="附掛電話" Editor="text" FieldName="LINETEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線網路IP" Editor="text" FieldName="LINEIP" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="光纖號碼" Editor="text" FieldName="FIBERID" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線SUBNET" Editor="infocombobox" FieldName="SUBNET" Format="" maxlength="20" Width="180" EditorOptions="items:[{value:' ',text:' ',selected:'false'},{value:'255.255.255.0',text:'255.255.255.0',selected:'false'},{value:'255.255.255.128',text:'255.255.255.128',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="閘道IP(Gateway)" Editor="text" FieldName="GATEWAY" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="DNS IP" Editor="text" FieldName="DNSIP" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="IP數量" Editor="numberbox" FieldName="IPCNT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="PPPOE撥接帳號" Editor="text" FieldName="PPPOEACCOUNT" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="PPPOE撥接密碼" Editor="text" FieldName="PPPOEPASSWORD" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線勘察日" Editor="datebox" FieldName="INSPECTDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="可否建置(Y)" Editor="infocombobox" FieldName="AGREE" Format="" maxlength="1" Width="180" EditorOptions="items:[{value:' ',text:' ',selected:'false'},{value:'Y',text:'Y',selected:'true'},{value:'N',text:'N',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="不可建置原因" Editor="textarea" FieldName="UNAGREEREASON" Format="" maxlength="200" Width="360" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路到位日" Editor="datebox" FieldName="HARDWAREDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線測通日" Editor="datebox" FieldName="ADSLAPPLYDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="撤線日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="撤線種類" Editor="inforefval" FieldName="DROPKIND" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'N9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢員" Editor="infocombobox" FieldName="CANCELUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註說明" Editor="textarea" FieldName="MEMO" Format="" maxlength="500" Width="500" EditorOptions="height:50" RowSpan="1" Span="2" />
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
