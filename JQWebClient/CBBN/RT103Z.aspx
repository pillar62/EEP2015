<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT103Z.aspx.cs" Inherits="Template_JQuerySingle1" %>

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
            alert("1");
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

            parent.addTab("撤線作業", "CBBN/RT103Z.aspx?COMQ1=" + ss + "&LINEQ1=" + s1);
        }

        $(document).ready(function () {
            dgOnloadSuccess();
        })

        function dgOnloadSuccess() {
            alert("2");
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
            alert("3");
        }

        function FilterTown(val) {
            alert("4");
            try {
                $('#dataFormMasterTOWNSHIP').combobox('setValue', "");
                $('#dataFormMasterTOWNSHIP').combobox('setWhere', "CUTID = '" + val.CUTID + "'");
            }
            catch (err) {
                alert(err);
            }
        }
        function OnLoadSuccess(val) {
            alert("5");
            try {
                var val = $('#dataFormMasterCUTID').combobox('getValue');
                $('#dataFormMasterTOWNSHIP').combobox('setWhere', "CUTID = '" + val + "'");
            }
            catch (err) {
                alert(err);
            }
        }

        function queryGrid(dg) { //查詢后添加固定條件
            alert("6");
            if ($(dg).attr('id') == 'dataGridView') {
                var where = $(dg).datagrid('getWhere');
                if (where.length > 0) {
                    if (COMQ1 != "") {
                        where = where + " and COMQ1 = '" + COMQ1 + "'";
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
                DataMember="RTLessorAVSCmtyLine" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="RT103Z">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="社區序號" Editor="numberbox" FieldName="COMQ1" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線群組" Editor="text" FieldName="LINEGROUP" Format="" MaxLength="1" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="經銷商" Editor="text" FieldName="CONSIGNEE" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="直銷業務轄區(AREATYPE='3')" Editor="text" FieldName="AREAID" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="---" Editor="text" FieldName="GROUPID" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="直銷業務員" Editor="text" FieldName="SALESID" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="二線負責人" Editor="text" FieldName="DEVELOPERID" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線網路IP" Editor="text" FieldName="LINEIP" Format="" MaxLength="20" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="閘道IP(Gateway)" Editor="text" FieldName="GATEWAY" Format="" MaxLength="20" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線SUBNET" Editor="text" FieldName="SUBNET" Format="" MaxLength="20" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="DNS IP" Editor="text" FieldName="DNSIP" Format="" MaxLength="20" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="PPPOE撥接帳號" Editor="text" FieldName="PPPOEACCOUNT" Format="" MaxLength="15" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="PPPOE撥接密碼" Editor="text" FieldName="PPPOEPASSWORD" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="附掛電話" Editor="text" FieldName="LINETEL" Format="" MaxLength="15" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線速率(KIND='D3')" Editor="text" FieldName="LINERATE" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路ISP(KIND='C3')" Editor="text" FieldName="LINEISP" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路IP種類(KIND='M5')" Editor="text" FieldName="LINEIPTYPE" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="IP數量" Editor="numberbox" FieldName="IPCNT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="--- 線路到期日" Editor="datebox" FieldName="LINEDUEDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址(縣市)" Editor="text" FieldName="CUTID" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址(鄉鎮)" Editor="text" FieldName="TOWNSHIP" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址(村)" Editor="text" FieldName="VILLAGE" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="村" Editor="text" FieldName="COD1" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址(鄰)" Editor="text" FieldName="NEIGHBOR" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="鄰" Editor="text" FieldName="COD2" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址(路)" Editor="text" FieldName="STREET" Format="" MaxLength="14" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="路" Editor="text" FieldName="COD3" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址(段)" Editor="text" FieldName="SEC" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="段" Editor="text" FieldName="COD4" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址(巷)" Editor="text" FieldName="LANE" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="巷" Editor="text" FieldName="COD5" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址(部落)" Editor="text" FieldName="TOWN" Format="" MaxLength="12" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="部落" Editor="text" FieldName="COD6" Format="" MaxLength="4" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址(弄)" Editor="text" FieldName="ALLEYWAY" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="弄" Editor="text" FieldName="COD7" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址(號)" Editor="text" FieldName="NUM" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="號" Editor="text" FieldName="COD8" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址(樓)" Editor="text" FieldName="FLOOR" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="樓" Editor="text" FieldName="COD9" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址(室)" Editor="text" FieldName="ROOM" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="室" Editor="text" FieldName="COD10" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="設備位置" Editor="text" FieldName="ADDROTHER" Format="" MaxLength="30" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="郵遞區號" Editor="text" FieldName="RZONE" Format="" MaxLength="5" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="收件日" Editor="datebox" FieldName="RCVDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線勘察日" Editor="datebox" FieldName="INSPECTDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="可否建置(Y)" Editor="text" FieldName="AGREE" Format="" MaxLength="1" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="不可建置原因" Editor="text" FieldName="UNAGREEREASON" Format="" MaxLength="200" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="--- CHT通知測通日" Editor="datebox" FieldName="HINETNOTIFYDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路到位日" Editor="datebox" FieldName="HARDWAREDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線測通日" Editor="datebox" FieldName="ADSLAPPLYDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建檔員" Editor="text" FieldName="EUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="建檔日" Editor="datebox" FieldName="EDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="修改員" Editor="text" FieldName="UUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢員" Editor="text" FieldName="CANCELUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="撤線日" Editor="datebox" FieldName="DROPDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="可供裝範圍" Editor="text" FieldName="SUPPLYRANGE" Format="" MaxLength="100" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="借名用戶名稱" Editor="text" FieldName="LOANNAME" Format="" MaxLength="30" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="借名用戶身份證號" Editor="text" FieldName="LOANSOCIAL" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註說明" Editor="text" FieldName="MEMO" Format="" MaxLength="500" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路申請日" Editor="datebox" FieldName="APPLYDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="申請人姓名" Editor="text" FieldName="APPLYNAME" Format="" MaxLength="30" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="申請人身份證(統編)" Editor="text" FieldName="APPLYSOCIAL" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="申請人連絡電話" Editor="text" FieldName="APPLYCONTACTTEL" Format="" MaxLength="15" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="申請人行動電話" Editor="text" FieldName="APPLYMOBILE" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="借名用戶連絡電話" Editor="text" FieldName="LOANCONTACTTEL" Format="" MaxLength="15" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="借名用戶行動電話" Editor="text" FieldName="LOANMOBILE" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="---" Editor="text" FieldName="AUTOIP" Format="" MaxLength="1" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="--- 續約申請日" Editor="datebox" FieldName="CONTAPPLYDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="撤線種類(KIND='N9')" Editor="text" FieldName="DROPKIND" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="可推行方案" Editor="text" FieldName="SELECTCASE" Format="" MaxLength="30" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="光纖號碼" Editor="text" FieldName="FIBERID" Format="" MaxLength="15" Visible="true" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply"
                        Text="存檔" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel"
                        Text="取消"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="RT103Z">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCmtyLine" HorizontalColumnsCount="2" RemoteName="sRT103.RTLessorAVSCmtyLine" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="numberbox" FieldName="COMQ1" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線群組" Editor="text" FieldName="LINEGROUP" Format="" maxlength="1" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="經銷商" Editor="text" FieldName="CONSIGNEE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="直銷業務轄區(AREATYPE='3')" Editor="text" FieldName="AREAID" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="---" Editor="text" FieldName="GROUPID" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="直銷業務員" Editor="text" FieldName="SALESID" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="二線負責人" Editor="text" FieldName="DEVELOPERID" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線網路IP" Editor="text" FieldName="LINEIP" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="閘道IP(Gateway)" Editor="text" FieldName="GATEWAY" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線SUBNET" Editor="text" FieldName="SUBNET" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="DNS IP" Editor="text" FieldName="DNSIP" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="PPPOE撥接帳號" Editor="text" FieldName="PPPOEACCOUNT" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="PPPOE撥接密碼" Editor="text" FieldName="PPPOEPASSWORD" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="附掛電話" Editor="text" FieldName="LINETEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線速率(KIND='D3')" Editor="text" FieldName="LINERATE" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路ISP(KIND='C3')" Editor="text" FieldName="LINEISP" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路IP種類(KIND='M5')" Editor="text" FieldName="LINEIPTYPE" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="IP數量" Editor="numberbox" FieldName="IPCNT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="--- 線路到期日" Editor="datebox" FieldName="LINEDUEDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(縣市)" Editor="text" FieldName="CUTID" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(鄉鎮)" Editor="text" FieldName="TOWNSHIP" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(村)" Editor="text" FieldName="VILLAGE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="村" Editor="text" FieldName="COD1" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(鄰)" Editor="text" FieldName="NEIGHBOR" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="鄰" Editor="text" FieldName="COD2" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(路)" Editor="text" FieldName="STREET" Format="" maxlength="14" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="路" Editor="text" FieldName="COD3" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(段)" Editor="text" FieldName="SEC" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="段" Editor="text" FieldName="COD4" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(巷)" Editor="text" FieldName="LANE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="巷" Editor="text" FieldName="COD5" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(部落)" Editor="text" FieldName="TOWN" Format="" maxlength="12" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="部落" Editor="text" FieldName="COD6" Format="" maxlength="4" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(弄)" Editor="text" FieldName="ALLEYWAY" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="弄" Editor="text" FieldName="COD7" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(號)" Editor="text" FieldName="NUM" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="號" Editor="text" FieldName="COD8" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(樓)" Editor="text" FieldName="FLOOR" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="樓" Editor="text" FieldName="COD9" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="地址(室)" Editor="text" FieldName="ROOM" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="室" Editor="text" FieldName="COD10" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="設備位置" Editor="text" FieldName="ADDROTHER" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="郵遞區號" Editor="text" FieldName="RZONE" Format="" maxlength="5" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="收件日" Editor="datebox" FieldName="RCVDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線勘察日" Editor="datebox" FieldName="INSPECTDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="可否建置(Y)" Editor="text" FieldName="AGREE" Format="" maxlength="1" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="不可建置原因" Editor="text" FieldName="UNAGREEREASON" Format="" maxlength="200" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="--- CHT通知測通日" Editor="datebox" FieldName="HINETNOTIFYDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路到位日" Editor="datebox" FieldName="HARDWAREDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線測通日" Editor="datebox" FieldName="ADSLAPPLYDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔員" Editor="text" FieldName="EUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日" Editor="datebox" FieldName="EDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改員" Editor="text" FieldName="UUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢員" Editor="text" FieldName="CANCELUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="撤線日" Editor="datebox" FieldName="DROPDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="可供裝範圍" Editor="text" FieldName="SUPPLYRANGE" Format="" maxlength="100" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶名稱" Editor="text" FieldName="LOANNAME" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶身份證號" Editor="text" FieldName="LOANSOCIAL" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註說明" Editor="text" FieldName="MEMO" Format="" maxlength="500" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路申請日" Editor="datebox" FieldName="APPLYDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人姓名" Editor="text" FieldName="APPLYNAME" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人身份證(統編)" Editor="text" FieldName="APPLYSOCIAL" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人連絡電話" Editor="text" FieldName="APPLYCONTACTTEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人行動電話" Editor="text" FieldName="APPLYMOBILE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶連絡電話" Editor="text" FieldName="LOANCONTACTTEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶行動電話" Editor="text" FieldName="LOANMOBILE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="---" Editor="text" FieldName="AUTOIP" Format="" maxlength="1" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="--- 續約申請日" Editor="datebox" FieldName="CONTAPPLYDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="撤線種類(KIND='N9')" Editor="text" FieldName="DROPKIND" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="可推行方案" Editor="text" FieldName="SELECTCASE" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="光纖號碼" Editor="text" FieldName="FIBERID" Format="" maxlength="15" Width="180" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
</html>
