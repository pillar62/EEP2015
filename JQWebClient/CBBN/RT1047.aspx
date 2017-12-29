<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1047.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID");
        var kind = Request.getQueryStringByName2("kind");
        var flag = true;
        var usr = getClientInfo('_usercode');

        function InsDefault() {
            if (CUSID != "") {
                return CUSID;
            }
        }

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                if (kind == 'Q')
                {
                    var sWhere = " 1=1 ";
                    $("#toolItemdataGridView新增").hide();
                }
                else
                {
                    var sWhere = " CUSID='" + CUSID + "'";
                }
                $("#dataGridView").datagrid('setWhere', sWhere);
            }
            flag = false;
        }

        //轉派工單
        function btn1Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var FAQNO = row.FAQNO;
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1047.cmdRT1047A', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT1047A" + "&parameters=" + CUSID + "," + FAQNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                },
                error:function(xhr, ajaxOptions, thrownError){ 
                alert(xhr.status); 
                alert(thrownError); 
            }
            });
        }

        //派工單查詢
        function btn2Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            var s2 = row.FAQNO;
            parent.addTab("用戶維修派工單資料維護", "CBBN/RT10471.aspx?CUSID=" + ss + "&FAQNO=" + s2);
        }

        //押回覆日
        function btn3Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var FAQNO = row.FAQNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1047.cmdRT10476', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10476" + "&parameters=" + CUSID + "," + FAQNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        //取消回覆
        function btn4Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var FAQNO = row.FAQNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1047.cmdRT10477', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10477" + "&parameters=" + CUSID + "," + FAQNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        //客服結案
        function btn5Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var FAQNO = row.FAQNO;
            
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1047.cmdRT10478', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10478" + "&parameters=" + CUSID + "," + FAQNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        //客服結案返轉
        function btn6Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var FAQNO = row.FAQNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1047.cmdRT10479', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT10479" + "&parameters=" + CUSID + "," + FAQNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        //作廢
        function btn7Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var FAQNO = row.FAQNO;
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1047.cmdRT1047B', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT1047B" + "&parameters=" + CUSID + "," + FAQNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        //作廢返轉
        function btn8Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var FAQNO = row.FAQNO;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1047.cmdRT1047C', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT1047C" + "&parameters=" + CUSID + "," + FAQNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }

        //派工單查詢
        function btn9Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            var s2 = row.FAQNO;
            parent.addTab("用戶維修派工單資料維護", "CBBN/RT10479.aspx?CUSID=" + ss + "&FAQNO=" + s2);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT1047.RTLessorAVSCustFaqH" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustFaqH" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="用戶客服單資料維護" OnLoadSuccess="dgOnloadSuccess" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶編號" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客服單號" Editor="text" FieldName="FAQNO" Format="" MaxLength="13" Visible="true" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="來電日" Editor="datebox" FieldName="RCVDAT" Format="yyyy/mm/dd" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="服務類型" Editor="inforefval" FieldName="SERVICETYPE" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'N4'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="摘要" Editor="text" FieldName="MEMO" Format="" MaxLength="1024" Visible="true" Width="200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="連絡電話" Editor="text" FieldName="CONTACTTEL" Format="" MaxLength="15" Visible="true" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="行動電話" Editor="text" FieldName="MOBILE" Format="" MaxLength="30" Visible="true" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="EMAIL" Editor="text" FieldName="EMAIL" Format="" Visible="False" Width="120" MaxLength="50" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工日期" Editor="datebox" FieldName="SNDWORK" Format="yyyy/mm/dd" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工人員" Editor="infocombobox" FieldName="SNDUSR" Format="" MaxLength="6" Visible="False" Width="90" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="SNDPRTNO" Format="" Visible="true" Width="90" MaxLength="13" />
                    <JQTools:JQGridColumn Alignment="left" Caption="列印日" Editor="datebox" FieldName="PRTDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工結案日" Editor="datebox" FieldName="SNDCLOSEDAT" Format="yyyy/mm/dd" MaxLength="0" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客服回覆" Editor="datebox" FieldName="CALLBACKDAT" Format="yyyy/mm/dd" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="回覆人員" Editor="infocombobox" FieldName="CALLBACKUSR" Format="" MaxLength="6" Visible="true" Width="90" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案日期" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案人員" Editor="infocombobox" FieldName="FUSR" Format="" MaxLength="6" Visible="true" Width="90" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Visible="true" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" Format="" MaxLength="6" Visible="true" Width="90" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="處理天數" Editor="numberbox" FieldName="qt_day" Format="" MaxLength="0" Visible="true" Width="64" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="True"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="轉派工單" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn2Click" Text="派工查詢" Visible="True" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn3Click" Text="押回覆日" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn4Click" Text="取消回覆" Visible="True" Icon="icon-undo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn5Click" Text="客服結案" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn6Click" Text="結案返轉" Visible="True" Icon="icon-undo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn7Click" Text="客服作廢" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn8Click" Text="作廢返轉" Visible="True" Icon="icon-undo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn9Click" Text="歷史異動" Visible="True" Icon="icon-view" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶客服單資料維護">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCustFaqH" HorizontalColumnsCount="2" RemoteName="sRT1047.RTLessorAVSCustFaqH" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶編號" Editor="infocombobox" FieldName="CUSID" Format="" maxlength="15" Width="180" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="服務單號" Editor="text" FieldName="FAQNO" Format="" maxlength="13" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="報修人" Editor="text" FieldName="FAQMAN" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="inbound / outbound" Editor="infocombobox" FieldName="IOLINE" Format="" maxlength="1" Width="180" EditorOptions="items:[{value:'I',text:'用戶來電',selected:'false'},{value:'O',text:'客服Call Out',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="連絡電話" Editor="text" FieldName="CONTACTTEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="行動電話" Editor="text" FieldName="MOBILE" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="EMAIL" Editor="text" FieldName="EMAIL" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="受理人員" Editor="infocombobox" FieldName="RCVUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客服收件日" Editor="datebox" FieldName="RCVDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客服類別" Editor="inforefval" FieldName="SERVICETYPE" Format="" maxlength="2" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'N4'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔人員" Editor="infocombobox" FieldName="EUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日期" Editor="datebox" FieldName="EDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改人員" Editor="infocombobox" FieldName="UUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日期" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="轉派工單日期" Editor="datebox" FieldName="SNDWORK" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="轉派單人員" Editor="infocombobox" FieldName="SNDUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="SNDPRTNO" Format="" maxlength="13" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工單列印人員" Editor="infocombobox" FieldName="PRTUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工單列印日" Editor="datebox" FieldName="PRTDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工單結案日" Editor="datebox" FieldName="SNDCLOSEDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客服回CALL日" Editor="datebox" FieldName="CALLBACKDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客服回CALL人員" Editor="infocombobox" FieldName="CALLBACKUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客服單結案日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案人員" Editor="infocombobox" FieldName="FUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="摘要" Editor="textarea" FieldName="MEMO" Format="" maxlength="1024" Width="400" EditorOptions="height:100" Span="2" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="CUSID" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="自動編號" FieldName="FAQNO" RemoteMethod="True" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
</html>
