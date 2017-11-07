<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT2051.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var caseno = Request.getQueryStringByName2("caseno"); //個案編號
        var sMODE = Request.getQueryStringByName2("sMODE");
        var gcomtype = Request.getQueryStringByName2("comtype"); //個案編號
        var glineq1 = Request.getQueryStringByName2("lineq1"); //個案編號
        var gcomq1 = Request.getQueryStringByName2("comq1"); //個案編號
        var gcusid = Request.getQueryStringByName2("cusid"); //個案編號

        var flag = true;

        function InsDefault() {
            if (caseno != "") {
                return caseno;
            }
        }
        function InsDefault1() {
            if (gcomtype != "") {
                return gcomtype;
            }
        }
        function InsDefault2() {
            if (glineq1 != "") {
                return glineq1;
            }
        }
        function InsDefault3() {
            if (gcomq1 != "") {
                return gcomq1;
            }
        }
        function InsDefault4() {
            if (gcusid != "") {
                
                return gcusid;
            }
        }

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                if (sMODE != ""){
                    if (sMODE != 'I') {
                        var sWhere = " caseno='" + caseno + "'";
                        $("#dataGridView").datagrid('setWhere', sWhere);
                    }
                    $("#dataGridView").datagrid("selectRow", 0);
                }
                else
                {

                }
                flag = false;
            }
            else
            {
            }
            flag = false;
        }
       
        function dgSelect()
        {
            if (sMODE != "") {
                setTimeout(function () {
                    if (sMODE == 'I') {
                        openForm('#JQDialog1', $('#dataGridView').datagrid('getSelected'), "inserted", 'Dialog');
                        sMODE = "";
                        $('#dataFormMasterCUSID').data("inforefval").refval.find("input.refval-text").focus();
                        $('#dataFormMasterMEMO').focus();
                    }
                    else
                        if (sMODE == 'E') {
                            openForm('#JQDialog1', $('#dataGridView').datagrid('getSelected'), "updated", 'Dialog');
                            var ss = "";
                            $("#JQDataForm1").datagrid('setWhere', ss);
                            sMODE = "";
                            $('#dataFormMasterCUSID').data("inforefval").refval.find("input.refval-text").focus();
                            $('#dataFormMasterMEMO').focus();
                        }
                        else
                            if (sMODE == 'B') {
                                openForm('#JQDialog1', $('#dataGridView').datagrid('getSelected'), "viewed", 'Dialog');
                                sMODE = "";
                            }
                }, 500);
            }
            else
            {
                //alert("1");
            }
        }

        function btnRT2054Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.caseno;
            parent.addTab("客訴派工單維護", "CBBN/RT2054.aspx?caseno=" + ss);
        }
        
        function CloseTab()
        {
            window.parent.closeCurrentTab();
            alert("有資料異動請點選資料更新!!");
        }

        function getcust()
        {
            var ss = $("#dataFormMasterCUSID").refval('getValue');
            alert(ss);//
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" LocaleAuto="True" UseFlow="False" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT2051.RTFaqM" runat="server" AutoApply="True"
                DataMember="RTFaqM" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="客訴資料維護" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True" OnLoadSuccess="dgOnloadSuccess" OnSelect="dgSelect">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客訴單號" Editor="text" FieldName="CASENO" MaxLength="10" Visible="true" Width="20" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶方案" Editor="text" FieldName="CASEKIND" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="個案別" Editor="inforefval" FieldName="COMTYPE" MaxLength="1" Visible="true" Width="80" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'方案代號',textFieldCaption:'方案名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" MaxLength="10" Visible="true" Width="60" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.View_RTLessorAVSCmtyH',tableName:'View_RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線序號" Editor="inforefval" FieldName="LINEQ1" MaxLength="3" Visible="true" Width="60" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT103.View_RTLessorAVSCmtyLine',tableName:'View_RTLessorAVSCmtyLine',columns:[],columnMatches:[],whereItems:[{field:'COMQ1',value:'row[COMQ1]'}],valueField:'LINEQ1',textField:'LINEQ1',valueFieldCaption:'主線序號',textFieldCaption:'主線序號',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶編號" Editor="inforefval" FieldName="CUSID" MaxLength="15" Visible="true" Width="60" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',columns:[],columnMatches:[],whereItems:[{field:'COMQ1',value:'row[COMQ1]'},{field:'LINEQ1',value:'row[LINEQ1]'}],valueField:'CUSID',textField:'CUSNC',valueFieldCaption:'用戶代號',textFieldCaption:'用戶名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" MaxLength="3" Visible="true" Width="30" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報修聯絡人" Editor="text" FieldName="FAQMAN" Visible="true" Width="50" MaxLength="50" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡電話" Editor="text" FieldName="TEL" MaxLength="50" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="手機" Editor="text" FieldName="MOBILE" MaxLength="50" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報修原因" Editor="inforefval" FieldName="FAQREASON" Visible="true" Width="60" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P7'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" MaxLength="2" />
                    <JQTools:JQGridColumn Alignment="left" Caption="進出線" Editor="infocombobox" FieldName="IOBOUND" Visible="true" Width="40" EditorOptions="items:[{value:'I',text:'用戶來電',selected:'false'},{value:'O',text:'客服Call Out',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" MaxLength="1" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" MaxLength="1500" Visible="true" Width="200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="受理人員" Editor="text" FieldName="RCVUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="30"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="受理時間" Editor="text" FieldName="RCVDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90" Format="yyyy/mm/dd"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="CLOSEUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="30"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="結案時間" Editor="text" FieldName="CLOSEDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="40" Format="yyyy/mm/dd"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="UUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="40"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="異動時間" Editor="text" FieldName="UDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90" Format="yyyy/mm/dd"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="CANCELUSR" Frozen="False" IsNvarChar="False" MaxLength="6" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="30"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢時間" Editor="text" FieldName="CANCELDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="40" Format="yyyy/mm/dd"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶來電詢問方案(Q2)" Editor="text" FieldName="ASKCASE" Frozen="False" IsNvarChar="False" MaxLength="2" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶來源別 (Q3)" Editor="text" FieldName="CUSTSRC" Frozen="False" IsNvarChar="False" MaxLength="2" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80"></JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton"
                        OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem"
                        Text="修改" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem"
                        Text="刪除"  />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton"
                        OnClick="apply" Text="確認" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="取消" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="btnRT2054" Text="派工單" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="客訴資料維護" Width="606px">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTFaqM" HorizontalColumnsCount="2" RemoteName="sRT2051.RTFaqM" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="客訴單號" Editor="text" FieldName="CASENO" maxlength="10" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶方案" Editor="text" FieldName="CASEKIND" maxlength="0" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="個案別" Editor="inforefval" FieldName="COMTYPE" maxlength="1" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="inforefval" FieldName="COMQ1" maxlength="10" Width="180" EditorOptions="title:'社區查詢',panelWidth:350,panelHeight:200,remoteName:'sRT101.View_RTLessorAVSCmtyH',tableName:'View_RTLessorAVSCmtyH',columns:[],columnMatches:[],whereItems:[{field:'comtype',value:'row[COMTYPE]'}],valueField:'COMQ1',textField:'COMN',valueFieldCaption:'社區代號',textFieldCaption:'社區名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線序號" Editor="inforefval" EditorOptions="title:'主線查詢',panelWidth:350,panelHeight:200,remoteName:'sRT205.V_RT2052',tableName:'V_RT2052',columns:[],columnMatches:[],whereItems:[{field:'COMQ1',value:'row[COMQ1]'},{field:'comtype',value:'row[COMTYPE]'}],valueField:'LINEQ1',textField:'LINEQ1',valueFieldCaption:'主線代號',textFieldCaption:'主線代號',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="LINEQ1" maxlength="3" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶編號" Editor="inforefval" FieldName="CUSID" MaxLength="15" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT205.V_RT205',tableName:'V_RT205',columns:[],columnMatches:[{field:'comtypenc',value:'comtypenc'},{field:'belongnc',value:'belongnc'},{field:'salesnc',value:'salesnc'},{field:'comq',value:'comq'},{field:'comn',value:'comn'},{field:'LINETEL',value:'LINETEL'},{field:'gateway',value:'gateway'},{field:'CMTYIP',value:'CMTYIP'},{field:'LINERATE',value:'LINERATE'},{field:'ARRIVEDAT',value:'ARRIVEDAT'},{field:'rcomdrop',value:'rcomdrop'},{field:'idslamip',value:'idslamip'},{field:'cusnc',value:'cusnc'},{field:'contacttel',value:'contacttel'},{field:'companytel',value:'companytel'},{field:'raddr',value:'raddr'},{field:'CUSTIP',value:'CUSTIP'},{field:'CASEKIND',value:'CASEKIND'},{field:'paycycle',value:'paycycle'},{field:'paytype',value:'paytype'},{field:'overdue',value:'overdue'},{field:'freecode',value:'freecode'},{field:'docketdat',value:'docketdat'},{field:'strbillingdat',value:'strbillingdat'},{field:'newbillingdat',value:'newbillingdat'},{field:'duedat',value:'duedat'},{field:'dropdat',value:'dropdat'},{field:'canceldat',value:'canceldat'},{field:'secondcase',value:'secondcase'},{field:'nciccusno',value:'nciccusno'},{field:'Sp499cons',value:'Sp499cons'},{field:'WtlApplyDat',value:'WtlApplyDat'},{field:'TEL',value:'contacttel'},{field:'MOBILE',value:'companytel'},{field:'FAQMAN',value:'cusnc'}],whereItems:[{field:'COMQ1',value:'row[COMQ1]'},{field:'LINEQ1',value:'row[LINEQ1]'},{field:'comtype',value:'row[COMTYPE]'}],valueField:'CUSID',textField:'cusnc',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" OnBlur="getcust" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案別" Editor="text" FieldName="comtypenc" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="直經銷" Editor="text" FieldName="belongnc" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="轄區業務" Editor="text" FieldName="salesnc" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區IP" Editor="text" FieldName="CMTYIP" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶來源別" Editor="inforefval" FieldName="CUSTSRC" MaxLength="2" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'Q3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="gateway IP" Editor="text" FieldName="gateway" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線速率" Editor="text" FieldName="LINERATE" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="附掛電話" Editor="text" FieldName="LINETEL" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路到位日" Editor="text" FieldName="ARRIVEDAT" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" Format="yyyy/mm/dd" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶名稱" Editor="text" FieldName="cusnc" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶地址" Editor="text" FieldName="raddr" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="2" Visible="True" Width="450" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公關機" Editor="text" FieldName="freecode" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶IP" Editor="text" FieldName="CUSTIP" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費方式" Editor="text" FieldName="paytype" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費週期" Editor="text" FieldName="paycycle" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="報竣日" Editor="text" FieldName="docketdat" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" Format="yyyy/mm/dd" />
                        <JQTools:JQFormColumn Alignment="left" Caption="退租日" Editor="text" FieldName="dropdat" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" Format="yyyy/mm/dd" />
                        <JQTools:JQFormColumn Alignment="left" Caption="欠拆" Editor="text" FieldName="overdue" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" Format="yyyy/mm/dd" />
                        <JQTools:JQFormColumn Alignment="left" Caption="開始計費日" Editor="text" FieldName="strbillingdat" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" Format="yyyy/mm/dd" />
                        <JQTools:JQFormColumn Alignment="left" Caption="續約日" Editor="text" FieldName="newbillingdat" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" Format="yyyy/mm/dd" />
                        <JQTools:JQFormColumn Alignment="left" Caption="是否為第二戶" Editor="text" FieldName="secondcase" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="到期日" Editor="text" FieldName="duedat" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" Format="yyyy/mm/dd" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="text" FieldName="canceldat1" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" Format="yyyy/mm/dd" />
                        <JQTools:JQFormColumn Alignment="left" Caption="對帳代號" Editor="text" FieldName="nciccusno" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶項次" Editor="text" FieldName="ENTRYNO" maxlength="3" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="報修聯絡人" Editor="text" FieldName="FAQMAN" Width="180" MaxLength="50" />
                        <JQTools:JQFormColumn Alignment="left" Caption="聯絡電話" Editor="text" FieldName="TEL" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="手機" Editor="text" FieldName="MOBILE" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="報修原因" Editor="inforefval" FieldName="FAQREASON" Width="180" MaxLength="2" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P7'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="進出線" Editor="inforefval" FieldName="IOBOUND" Width="180" MaxLength="1" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="textarea" FieldName="MEMO" maxlength="1500" Width="400" EditorOptions="height:60" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="受理人員" Editor="infocombobox" FieldName="RCVUSR" MaxLength="6" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="受理時間" Editor="datebox" FieldName="RCVDAT" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" Format="yyyy/mm/dd HH:MM:SS" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案人員" Editor="infocombobox" FieldName="CLOSEUSR" MaxLength="6" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案時間" Editor="datebox" FieldName="CLOSEDAT" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" Format="yyyy/mm/dd HH:mm:SS" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動人員" Editor="infocombobox" FieldName="UUSR" MaxLength="6" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動時間" Editor="datebox" FieldName="UDAT" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" Format="yyyy/mm/dd HH:MM:SS" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" MaxLength="6" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢時間" Editor="datebox" FieldName="CANCELDAT" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" Format="yyyy/mm/dd HH:mm:SS" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶來電詢問方案" Editor="inforefval" FieldName="ASKCASE" MaxLength="2" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'Q2'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="自動編號" FieldName="CASENO" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_usercode" FieldName="UUSR" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today2" FieldName="UDAT" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault1" FieldName="COMTYPE" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault2" FieldName="LINEQ1" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault3" FieldName="COMQ1" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault4" FieldName="CUSID" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_usercode" FieldName="RCVUSR" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today2" FieldName="RCVDAT" RemoteMethod="False" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
</html>
