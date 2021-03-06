﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT102.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        //"續約作業"
        function btn1click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            parent.addTab("用戶維護", "CBBN/RT104.aspx?COMQ1=" + ss);
        }
        //"合約結案"
        function btn2click(val) {
            
        }
        //        "結案返轉"
        function btn3click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            parent.addTab("用戶維護", "CBBN/RT104.aspx?COMQ1=" + ss);
        }
        //        "作　　廢"
        function btn4click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var COMQ1 = row.COMQ1;
            var CONTRACTNO = row.CONTRACTNO;
            var usr = getClientInfo('_usercode');
            var CLOSEDAT = row.CLOSEDAT;
            var CANCELDAT = row.CANCELDAT;
            if (CLOSEDAT != null && CLOSEDAT != "") {
                alert(CLOSEDAT);
                alert("已結案，請先結案返回!!");
                return false;
            }
            if (CANCELDAT != null && CANCELDAT != "") {
                alert(CANCELDAT);
                alert("已作廢，請勿重複作廢!!");
                return false;
            }
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT102.ucRT1021', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT1021" + "&parameters=" + COMQ1 + "," + CONTRACTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                }
            });
        }
        //        "作廢返轉"

        function btn5click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var COMQ1 = row.COMQ1;
            var CONTRACTNO = row.CONTRACTNO;
            var usr = getClientInfo('_usercode');
            var CLOSEDAT = row.CLOSEDAT;
            var CANCELDAT = row.CANCELDAT;
            if (CLOSEDAT != null && CLOSEDAT != "") {
                alert(CLOSEDAT);
                alert("已結案，請先結案返回!!");
                return false;
            }
            if (CANCELDAT != null && CANCELDAT != "") {
                alert(CANCELDAT);
                alert("已作廢，請勿重複作廢!!");
                return false;
            }
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT102.ucRT1021', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT1021" + "&parameters=" + COMQ1 + "," + CONTRACTNO + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                }
            });
        }
        //        "異動查詢"
        function btn6click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.COMQ1;
            parent.addTab("用戶維護", "CBBN/RT104.aspx?COMQ1=" + ss);
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT102.RTLessorAVSCmtyContract" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCmtyContract" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="社區合約資料維護" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Visible="true" Width="120" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.RTLessorAVSCmtyH',tableName:'RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區合約編號" Editor="text" FieldName="CONTRACTNO" Format="" MaxLength="12" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="合約起始日" Editor="datebox" FieldName="STRDAT" Format="yyyy/mm/dd" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最近續約日" Editor="datebox" FieldName="CONTDAT" Format="yyyy/mm/dd" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="續約次數" Editor="numberbox" FieldName="CONTCNT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="合約終止日" Editor="datebox" FieldName="ENDDAT" Format="yyyy/mm/dd" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="簽約對象" Editor="text" FieldName="CONTRACTOBJ" Format="" MaxLength="30" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="連絡電話" Editor="text" FieldName="OBJTEL" Format="" MaxLength="15" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="行動電話" Editor="text" FieldName="OBJMOBILE" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="電費補助種類" Editor="inforefval" FieldName="POWERBILLKIND" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="電費補助方式" Editor="inforefval" FieldName="POWERBILLPAYKIND" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O4'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="right" Caption="每度補助金額" Editor="numberbox" FieldName="SCALEAMT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="付款方式" Editor="inforefval" FieldName="PAYKIND" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'F5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="付款週期" Editor="inforefval" FieldName="PAYCYCLE" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'F9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案日" Editor="datebox" FieldName="CLOSEDAT" Format="yyyy/mm/dd" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案人員" Editor="infocombobox" FieldName="CLOSEUSR" Format="" MaxLength="6" Visible="true" Width="120" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" Format="" MaxLength="6" Visible="true" Width="120" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="50" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轉應收帳款日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轉應收帳款人員" Editor="infocombobox" FieldName="TUSR" Format="" MaxLength="6" Visible="true" Width="120" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="False"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn1click" Text="續約作業" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn2click" Text="合約結案" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn3click" Text="結案返轉" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn4click" Text="作　　廢" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn5click" Text="作廢返轉" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn6click" Text="異動查詢" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="社區合約資料維護">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCmtyContract" HorizontalColumnsCount="2" RemoteName="sRT102.RTLessorAVSCmtyContract" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Visible="true" Width="180" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.RTLessorAVSCmtyH',tableName:'RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區合約編號" Editor="text" FieldName="CONTRACTNO" Format="" maxlength="12" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="合約起始日" Editor="datebox" FieldName="STRDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="最近續約日" Editor="datebox" FieldName="CONTDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="續約次數" Editor="numberbox" FieldName="CONTCNT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="合約終止日" Editor="datebox" FieldName="ENDDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="簽約對象" Editor="text" FieldName="CONTRACTOBJ" Format="" maxlength="30" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="連絡電話" Editor="text" FieldName="OBJTEL" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="行動電話" Editor="text" FieldName="OBJMOBILE" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="元訊簽約部門" Editor="infocombobox" FieldName="SIGNDEPT" Format="" maxlength="8" Width="180" EditorOptions="valueField:'DEPT',textField:'DEPTN4',remoteName:'sRT100.RTDept',tableName:'RTDept',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="元訊簽約人員" Editor="inforefval" FieldName="SIGNPERSON" Format="" maxlength="6" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',columns:[],columnMatches:[],whereItems:[{field:'DEPT',value:'row[SIGNDEPT]'}],valueField:'EMPLY',textField:'NAME',valueFieldCaption:'員工編號',textFieldCaption:'員工姓名',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="付款銀行" Editor="infocombobox" FieldName="REMITBANK1" Format="" maxlength="3" Width="180" EditorOptions="valueField:'HEADNO',textField:'HEADNC',remoteName:'sRT100.RTBank',tableName:'RTBank',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="分行" Editor="inforefval" FieldName="REMITBANK2" Format="" maxlength="4" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTBankBranch',tableName:'RTBankBranch',columns:[],columnMatches:[],whereItems:[{field:'HEADNO',value:'row[REMITBANK1]'}],valueField:'BRANCHNO',textField:'BRANCHNC',valueFieldCaption:'分行代號',textFieldCaption:'分行名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="帳號" Editor="text" FieldName="ACNO" Format="" maxlength="16" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="戶名" Editor="text" FieldName="AC" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="電費補助種類" Editor="inforefval" FieldName="POWERBILLKIND" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="電費補助方式" Editor="inforefval" FieldName="POWERBILLPAYKIND" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O4'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="每度補助金額" Editor="numberbox" FieldName="SCALEAMT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="付款方式" Editor="inforefval" FieldName="PAYKIND" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'F5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="付款週期" Editor="inforefval" FieldName="PAYCYCLE" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'F9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案日" Editor="datebox" FieldName="CLOSEDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案人員" Editor="infocombobox" FieldName="CLOSEUSR" Format="" MaxLength="6" Visible="true" Width="120" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" FieldName="CANCELUSR" Format="" MaxLength="6" Visible="true" Width="120" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔員" Editor="text" FieldName="EUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="輸入日期" Editor="datebox" FieldName="EDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改員" Editor="text" FieldName="UUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" maxlength="500" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="轉應收帳款日" Editor="datebox" FieldName="TARDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="轉應收帳款人員" Editor="infocombobox" FieldName="TUSR" Format="" MaxLength="6" Visible="true" Width="120" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
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
<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>
</html>
