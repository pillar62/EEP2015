<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT107.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var COMQ1 = Request.getQueryStringByName2("COMQ1");
        var LINEQ1 = Request.getQueryStringByName2("LINEQ1"); 
        var COMTYPE = Request.getQueryStringByName2("COMTYPE");
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

        function InsDefaultLINEQ1() {
            if (LINEQ1 != "") {
                return LINEQ1;
            }
            else
            {
                flag = false;
            }
        }

        function getkind() {
            
        }

        function btn1Click(val) {
            var sMODE = "I";
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var comq1 = row.COMQ1;
            var lineq1 = row.LINEQ1;
            var comtype = row.COMTYPE;
            var cusid = row.CUSID;
            parent.addTab("各方案客訴維護", "CBBN/RT205.aspx?comq1=" + COMQ1 + "&sMODE=" + sMODE + "&comq1=" + comq1 + "&lineq1=" + lineq1 + "&comtype=" + comtype + "&cusid=" + cusid);
        }

        function MySelect(rowIndex, rowData)
        {
            var ss = rowData.CUSID;
            if (ss == "") ss = "ZZZZZ";            
            $("#dataRT205").datagrid('setWhere', "a.cusid='" + ss + "'");
        }

        function dgOnloadSuccess() {
            if (flag) {
                var sWhere = "A.COMQ1='" + COMQ1 + "'";
                if (LINEQ1 != "") {
                    sWhere = sWhere + " AND A.LINEQ1='" + LINEQ1 + "'"
                }
                if (COMTYPE != "") {
                    sWhere = sWhere + " AND A.COMTYPE='" + COMTYPE + "'"
                }
                $('#dataGridView').datagrid('setWhere', sWhere);//篩選資料
            }
            else
            {
                var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
                var ss = row.CUSID;
                if (ss == "") ss = "ZZZZZ";

                if (flag == false)
                {
                    $("#dataRT205").datagrid('setWhere', "a.cusid='" + ss + "'");                
                }
            }

            flag = false;
        }

        /*
        function queryGrid(dg) { //查詢后添加固定條件
            if ($(dg).attr('id') == 'dataGridView') {
                var where = $(dg).datagrid('getWhere');
                alert(where);
                if (where != "") {
                    where = " 1=1 ";

                    COMN = $("#COMN_Query").val(); //社區名稱
                    CUSNC = $("#CUSNC_Query").val(); //客戶名稱
                    MOBILE = $("#MOBILE_Query").val(); //行動電話
                    RADDR2 = $("#RADDR2_Query").val(); //地址
                    SOCIALID = $("#SOCIALID_Query").val(); //身分證號
                    CONTACTTEL = $("#CONTACTTEL_Query").val(); //市內電話
                    QQ = $("#A.QQ_Query").val(); //市內電話

                    if (COMN != "")
                        where = where + " and B.COMN like '%" + COMN + "%'";
                    if (RADDR2 != "")
                        where = where + " and A.RADDR2 like '%" + RADDR2 + "%'";
                   
                    if (MOBILE != "")
                        where = where + " and A.MOBILE LIKE '%" + MOBILE + "%'";
                    if (SOCIALID != "")
                        where = where + " and A.SOCIALID LIKE '%" + SOCIALID + "%'";
                    if (CONTACTTEL != "")
                        where = where + " and A.CONTACTTEL LIKE '%" + CONTACTTEL + "%'";

                }
                $(dg).datagrid('setWhere', where);
                $('#dataGridView').datagrid('reload');
            }
        }*/

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
                    <JQTools:JQGridColumn Alignment="left" Caption="個案別" Editor="inforefval" FieldName="COMTYPE" Visible="true" Width="80" EditorOptions="title:'方案別',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'"/>
                    <JQTools:JQGridColumn Alignment="right" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Visible="true" Width="120" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.RTLessorAVSCmtyH',tableName:'RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" MaxLength="0" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶代號" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶名" Editor="text" FieldName="CUSNC" Format="" MaxLength="30" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶來源" Editor="inforefval" EditorOptions="title:'客戶來源',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'Q3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="CUSTSRC" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="裝機縣市" Editor="infocombobox" FieldName="CUTID2" Format="" MaxLength="2" Visible="true" Width="60" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.View_RTCounty',tableName:'View_RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案" Editor="inforefval" FieldName="CASEKIND" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTCode',tableName:'View_RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="鄉鎮區" Editor="text" FieldName="TOWNSHIP2" Format="" MaxLength="10" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="地址" Editor="text" FieldName="RADDR2" Format="" MaxLength="60" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="連絡手機" Editor="text" FieldName="MOBILE" Format="" MaxLength="30" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="IP(XXX.xxx.xxx.xxx)" Editor="text" FieldName="IP11" Format="" Visible="true" Width="90" MaxLength="3" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報竣日" Editor="datebox" FieldName="DOCKETDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="STRBILLINGDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最近續約計費日" Editor="datebox" FieldName="NEWBILLINGDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日" Editor="datebox" FieldName="DUEDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="公關戶(Y)" Editor="text" FieldName="FREECODE" Format="" Visible="true" Width="120" MaxLength="1" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" ID="btnIns" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply"
                        Text="存檔" ID="btnsave" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel"
                        Text="取消" ID="btncancel" />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="客訴處理" Visible="True" OnClick="btn1Click" Icon="icon-view" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區名稱" Condition="%%" DataType="string" Editor="text" FieldName="B.COMN" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="客戶名稱" Condition="%%" DataType="string" Editor="text" FieldName="A.CUSNC" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="連絡手機" Condition="%%" DataType="string" Editor="text" FieldName="A.MOBILE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="住戶地址" Condition="%%" DataType="string" Editor="text" FieldName="A.RADDR2" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="身份證號" Condition="%%" DataType="string" Editor="text" FieldName="A.SOCIALID" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="連絡電話" Condition="%%" DataType="string" Editor="text" FieldName="A.CONTACTTEL" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶維護" Width="800px" style="margin-bottom: 0px">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCust" HorizontalColumnsCount="2" RemoteName="sRT104.RTLessorAVSCust" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >

                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶來源" Editor="inforefval" EditorOptions="title:'客戶來源',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'Q3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="CUSTSRC" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案別" Editor="inforefval" EditorOptions="title:'方案別',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="COMTYPE" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="inforefval" EditorOptions="title:'社區查詢',panelWidth:350,panelHeight:200,remoteName:'sRT101.RTLessorAVSCmtyH',tableName:'RTLessorAVSCmtyH',columns:[],columnMatches:[],whereItems:[],valueField:'COMQ1',textField:'COMN',valueFieldCaption:'代號',textFieldCaption:'社區名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="COMQ1" Format="" ReadOnly="False" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線序號" Editor="inforefval" EditorOptions="title:'主線挑選',panelWidth:350,panelHeight:200,remoteName:'sRT103.View_RTLessorAVSCmtyLine',tableName:'View_RTLessorAVSCmtyLine',columns:[],columnMatches:[],whereItems:[{field:'COMQ1',value:'row[COMQ1]'}],valueField:'LINEQ1',textField:'LINEQ1',valueFieldCaption:'主線',textFieldCaption:'主線',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="LINEQ1" Format="" ReadOnly="False" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶代號" Editor="text" FieldName="CUSID" Format="" maxlength="15" ReadOnly="True" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第二戶" Editor="infocombobox" EditorOptions="items:[{value:'Y',text:'Y',selected:'false'},{value:'N',text:'N',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="SECONDCASE" Format="" maxlength="1" Width="200" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公關戶(Y)" Editor="infocombobox" EditorOptions="items:[{value:'Y',text:'Y',selected:'false'},{value:'N',text:'N',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="FREECODE" Format="" maxlength="1" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶(公司)名稱" Editor="text" FieldName="CUSNC" Format="" maxlength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第一證照別" Editor="inforefval" EditorOptions="title:'證照別',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'J5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="IDNUMBERTYPE" Format="" maxlength="2" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="身份證字號" Editor="text" FieldName="SOCIALID" Format="" maxlength="10" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第二證照別" Editor="inforefval" EditorOptions="title:'證照別',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'L3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="SECONDIDTYPE" Format="" maxlength="2" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第二證照號碼" Editor="text" FieldName="SECONDNO" Format="" maxlength="15" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="生日" Editor="datebox" FieldName="BIRTHDAY" Format="yyyy/mm/dd" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="連絡電話" Editor="text" FieldName="CONTACTTEL" Format="" maxlength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="連絡手機" Editor="text" FieldName="MOBILE" Format="" maxlength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="E-Mail" Editor="text" FieldName="EMAIL" Format="" maxlength="50" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="裝機地址" Editor="infocombobox" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.View_RTCounty',tableName:'View_RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,onSelect:FilterTown2,panelHeight:200" FieldName="CUTID2" Format="" maxlength="2" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="裝機地址" Editor="infocombobox" EditorOptions="valueField:'TOWNSHIP',textField:'TOWNSHIP',remoteName:'sRT100.RTCtyTown',tableName:'RTCtyTown',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="TOWNSHIP2" Format="" maxlength="10" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="裝機地址" Editor="text" FieldName="RADDR2" Format="" maxlength="60" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="郵遞區號(裝機)" Editor="text" FieldName="RZONE2" Format="" maxlength="5" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="帳單地址" Editor="infocombobox" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.View_RTCounty',tableName:'View_RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,onSelect:FilterTown3,panelHeight:200" FieldName="CUTID3" Format="" maxlength="2" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="帳單地址" Editor="infocombobox" EditorOptions="valueField:'TOWNSHIP',textField:'TOWNSHIP',remoteName:'sRT100.RTCtyTown',tableName:'RTCtyTown',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="TOWNSHIP3" Format="" maxlength="10" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="帳單地址" Editor="text" FieldName="RADDR3" Format="" maxlength="60" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="郵遞區號(帳單)" Editor="text" FieldName="RZONE3" Format="" maxlength="5" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司連絡人" Editor="text" FieldName="COCONTACT" Format="" maxlength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司電話" Editor="text" FieldName="COCONTACTTEL" Format="" maxlength="15" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司電話分機" Editor="text" FieldName="COTELEXT" Format="" maxlength="5" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司手機" Editor="text" FieldName="COCONTACTMOBILE" Format="" maxlength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司負責人" Editor="text" FieldName="COBOSS" Format="" maxlength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="負責人身份證字號" Editor="text" FieldName="COBOSSID" Format="" maxlength="10" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="行業別" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'J8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="COKIND" Format="" maxlength="2" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔人" Editor="infocombobox" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="EUSR" Format="" maxlength="6" ReadOnly="True" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日" Editor="datebox" FieldName="EDAT" Format="yyyy/mm/dd" ReadOnly="True" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改人" Editor="infocombobox" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="UUSR" Format="" maxlength="6" ReadOnly="True" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" ReadOnly="True" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢員" Editor="infocombobox" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="CANCELUSR" Format="" maxlength="6" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="CASEKIND" Format="" maxlength="2" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶速率" Editor="text" FieldName="USERRATE" Format="" maxlength="10" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費週期" Editor="inforefval" FieldName="PAYCYCLE" Format="" maxlength="2" Width="200" EditorOptions="title:'JQRefval',panelWidth:350,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'代碼名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費方式" Editor="inforefval" FieldName="PAYTYPE" Format="" maxlength="2" Width="200" EditorOptions="title:'JQRefval',panelWidth:350,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'代碼名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="當期收款金額" Editor="numberbox" FieldName="RCVMONEY" Format="" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡種類" Editor="inforefval" FieldName="CREDITCARDTYPE" Format="" maxlength="2" Width="200" EditorOptions="title:'信用卡類別',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M6'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發卡銀行" Editor="inforefval" FieldName="CREDITBANK" Format="" maxlength="3" Width="200" EditorOptions="title:'銀行',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTBank',tableName:'RTBank',columns:[],columnMatches:[],whereItems:[{field:'CREDITCARD',value:'Y'}],valueField:'HEADNO',textField:'HEADNC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="卡號" Editor="text" FieldName="CREDITCARDNO" Format="" maxlength="16" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="持卡人姓名" Editor="text" FieldName="CREDITNAME" Format="" maxlength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效截止月" Editor="text" FieldName="CREDITDUEM" Format="" maxlength="2" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效截止年" Editor="text" FieldName="CREDITDUEY" Format="" maxlength="2" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" maxlength="12" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="轉應收帳款日" Editor="datebox" EditorOptions="" FieldName="CDAT" Format="yyyy/mm/dd" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="裝機費" Editor="numberbox" FieldName="SETMONEY" Format="" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金序號" Editor="text" FieldName="GTSERIAL" Format="" maxlength="12" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金收據列印人" Editor="text" FieldName="GTPRTUSR" Format="" maxlength="10" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金收據列印日" Editor="datebox" EditorOptions="" FieldName="GTPRTDAT" Format="yyyy/mm/dd" Width="160" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金" Editor="numberbox" FieldName="GTMONEY" Format="" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶保管CPE設備" Editor="text" FieldName="GTEQUIP" Format="" maxlength="10" Width="200" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶保管STB設備" Editor="text" FieldName="EQUIP" Format="" maxlength="2" Width="200" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金退還日期" Editor="datebox" FieldName="GTREPAYDAT" Format="yyyy/mm/dd" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="IP(999.999.999.999)" Editor="text" FieldName="IP11" Format="" maxlength="3" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶CPE Mac Address" Editor="text" FieldName="MAC" Format="" maxlength="12" Width="200" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" maxlength="0" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="報竣日" Editor="datebox" FieldName="DOCKETDAT" Format="yyyy/mm/dd" maxlength="0" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="STRBILLINGDAT" Format="yyyy/mm/dd" maxlength="0" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="最近續約計費日" Editor="datebox" FieldName="NEWBILLINGDAT" Format="yyyy/mm/dd" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="可使用期數" Editor="numberbox" FieldName="PERIOD" Format="" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="調整日數" Editor="numberbox" FieldName="ADJUSTDAY" Format="" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="使用截止日" Editor="datebox" EditorOptions="" FieldName="DUEDAT" Format="yyyy/mm/dd" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="textarea" EditorOptions="height:60" FieldName="MEMO" Format="" maxlength="500" Span="2" Width="360" />
                    </Columns>
                </JQTools:JQDataForm>

                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="COMQ1" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefaultLINEQ1" FieldName="LINEQ1" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="自動編號" FieldName="CUSID" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today" FieldName="EDAT" RemoteMethod="True" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="02" FieldName="CUSTSRC" RemoteMethod="False" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
        <p>
            <JQTools:JQDataGrid ID="dataRT205" data-options="pagination:true,view:commandview" RemoteName="sRT205.RT205" runat="server" AutoApply="False"
                DataMember="RT205" Pagination="True" QueryTitle="查詢條件"
                Title="客訴資料維護" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Panel" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="True" Visible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客訴單號" Editor="text" FieldName="caseno" Format="" MaxLength="10" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案別" Editor="text" FieldName="comtype" Format="" MaxLength="1" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="經銷" Editor="text" FieldName="ANGENCY" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="業務" Editor="text" FieldName="leader" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案別" Editor="text" FieldName="codenc" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線" Editor="text" FieldName="COMLINE" Format="" MaxLength="0" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="comn" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶退租日" Editor="datebox" FieldName="dropdat" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡人" Editor="text" FieldName="faqman" Format="" MaxLength="50" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="進出線" Editor="text" FieldName="codenc1" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報修原因" Editor="text" FieldName="codenc2" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="受理人" Editor="text" FieldName="CUSNC" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="受理時間" Editor="text" FieldName="RCVDATE" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案時間" Editor="datebox" FieldName="closedat" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶來源" Editor="text" FieldName="codenc3" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="追件數" Editor="numberbox" FieldName="QT_CASE" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="預定施工人" Editor="text" FieldName="SNAME" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="已完工" Editor="text" FieldName="finishnum" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="comq1" Editor="text" FieldName="comq1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="80"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="lineq1" Editor="text" FieldName="lineq1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="80"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="cusid" Editor="text" FieldName="cusid" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="80"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="entryno" Editor="text" FieldName="entryno" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="80"></JQTools:JQGridColumn>
                </Columns>
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
