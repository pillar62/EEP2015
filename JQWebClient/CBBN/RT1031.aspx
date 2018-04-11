<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1031.aspx.cs" Inherits="Template_JQuerySingle1" %>

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

        function InsDefaultLINEQ1() {
            if (LINEQ1 != "") {
                return LINEQ1;
            }
            else
            {
                flag = false;
            }
        }

        //續約結案
        function btn1Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var COMQ1 = row.COMQ1;
            var LINEQ1 = row.LINEQ1;
            var ENTRYNO = row.ENTRYNO;
            alert(ENTRYNO);

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1031.cmdRT10311', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10311" + "&parameters=" + COMQ1 + "," + LINEQ1 + "," + ENTRYNO + "," + usr,
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

        //作廢
        function btn2Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var COMQ1 = row.COMQ1;
            var LINEQ1 = row.LINEQ1;
            var ENTRYNO = row.ENTRYNO;
            alert(COMQ1);

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1031.cmdRT10312', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10312" + "&parameters=" + COMQ1 + "," + LINEQ1 + "," + ENTRYNO + "," + usr,
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
        function btn3Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var COMQ1 = row.COMQ1;
            var LINEQ1 = row.LINEQ1;
            var ENTRYNO = row.ENTRYNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1031.cmdRT10313', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10313" + "&parameters=" + COMQ1 + "," + LINEQ1 + "," + ENTRYNO + "," + usr,
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

        function btn4Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            var s1 = row.LINEQ1;
            parent.addTab("用戶異動資料查詢", "CBBN/RT10311.aspx?COMQ1=" + ss + "&LINEQ1=" + s1);
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
            flag = false;

        }        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT1031.RTLessorAVSCmtyLineCont" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCmtyLineCont" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="主線續約資料維護" OnLoadSuccess="dgOnloadSuccess" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Visible="true" Width="120" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.View_RTLessorAVSCmtyH',tableName:'View_RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線" Editor="numberbox" FieldName="LINEQ1" Format="" Visible="true" Width="40" />
                    <JQTools:JQGridColumn Alignment="right" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Visible="true" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線IP" Editor="text" FieldName="LINEIP" Format="" MaxLength="20" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="附掛電話" Editor="text" FieldName="LINETEL" Format="" MaxLength="15" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路ISP" Editor="inforefval" FieldName="LINEISP" Format="" MaxLength="2" Visible="true" Width="80" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'C3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路IP種類" Editor="inforefval" FieldName="LINEIPTYPE" Format="" MaxLength="2" Visible="true" Width="100" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="ADSL主線頻寬" Editor="text" FieldName="LINERATE" Format="" MaxLength="2" Visible="true" Width="100" />
                    <JQTools:JQGridColumn Alignment="right" Caption="IP數量" Editor="numberbox" FieldName="IPCNT" Format="" MaxLength="0" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="續約申請日" Editor="datebox" FieldName="CONTAPPLYDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路到期日" Editor="datebox" FieldName="LINEDUEDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="HINET線班通知開通日" Editor="datebox" FieldName="HINETNOTIFYDAT" Format="" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線測通日" Editor="datebox" FieldName="ADSLAPPLYDAT" Format="" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案日期" Editor="datebox" FieldName="CLOSEDAT" Format="" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="" Visible="true" Width="80" />
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
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="續約結案" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="btn2Click" Text="作　　廢" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="btn3Click" Text="作廢返轉" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-view" ItemType="easyui-linkbutton" OnClick="btn4Click" Text="異動查詢" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="主線續約資料維護">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCmtyLineCont" HorizontalColumnsCount="2" RemoteName="sRT1031.RTLessorAVSCmtyLineCont" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Width="180" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.View_RTLessorAVSCmtyH',tableName:'View_RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="續約申請日" Editor="datebox" FieldName="CONTAPPLYDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人姓名" Editor="text" FieldName="APPLYNAME" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人身份證(統編)" Editor="text" FieldName="APPLYSOCIAL" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人連絡電話" Editor="text" FieldName="APPLYCONTACTTEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="申請人行動電話" Editor="text" FieldName="APPLYMOBILE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶連絡電話" Editor="text" FieldName="LOANCONTACTTEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="借名用戶行動電話" Editor="text" FieldName="LOANMOBILE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="ADSL主線頻寬" Editor="text" FieldName="LINERATE" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路IP種類" Editor="inforefval" FieldName="LINEIPTYPE" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路ISP" Editor="inforefval" FieldName="LINEISP" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'C3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="線路到期日" Editor="datebox" FieldName="LINEDUEDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="附掛電話" Editor="text" FieldName="LINETEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="IP數量" Editor="numberbox" FieldName="IPCNT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線網路IP" Editor="text" FieldName="LINEIP" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="GATEWAY IP" Editor="text" FieldName="GATEWAY" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線網路IP網段" Editor="text" FieldName="SUBNET" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="DNS IP" Editor="text" FieldName="DNSIP" Format="" maxlength="20" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="PPPOE撥接帳號" Editor="text" FieldName="PPPOEACCOUNT" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="PPPOE撥接密碼" Editor="text" FieldName="PPPOEPASSWORD" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="HINET線班通知開通日" Editor="datebox" FieldName="HINETNOTIFYDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線測通日" Editor="datebox" FieldName="ADSLAPPLYDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案日期" Editor="datebox" FieldName="CLOSEDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案人員" Editor="infocombobox" FieldName="CLOSEUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" maxlength="500" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔員" Editor="infocombobox" FieldName="EUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="輸入日期" Editor="datebox" FieldName="EDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改員" Editor="infocombobox" FieldName="UUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="藉名用戶名稱" Editor="text" FieldName="LOANNAME" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="藉名用戶身份證號" Editor="text" FieldName="LOANSOCIAL" Format="" maxlength="10" Width="180" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="COMQ1" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefaultLINEQ1" FieldName="LINEQ1" RemoteMethod="False" />
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
