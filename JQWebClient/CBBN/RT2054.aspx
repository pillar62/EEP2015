<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT2054.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var usr = getClientInfo('_usercode');
        var caseno = Request.getQueryStringByName2("caseno"); //個案編號
        var gcaseno = Request.getQueryStringByName2("caseno"); //個案編號
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

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = " LINKNO='" + caseno + "'";
                $("#dataGridView").datagrid('setWhere', sWhere);
                flag = false;
            }
            flag = false;
        }

        function MySelect(rowIndex, rowData) {
        }


        //作廢
        function btn1Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var workno = row.WORKNO;
           
            if (confirm("是否要作廢派工單？")) {
                $.ajax({
                    type: "POST",
                    url: '../handler/jqDataHandle.ashx?RemoteName=sRT2054.cmd', //連接的Server端，command
                    //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                    data: "mode=method&method=" + "smRT20541" + "&parameters=" + workno + "," + usr,
                    cache: false,
                    async: false,
                    success: function (data) {
                        alert(data);
                        $('#dataGridView').datagrid('reload');
                    }
                });
            }
        }

        //列印派工單
        function btn2Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.WORKNO;
            $("#JQDataGrid1").datagrid('setWhere', "a.workno='" + ss + "'"); //維護單 
            
            var WhereString = "";
            exportDevReport("#JQDataGrid1", "sRT2054.cmdRT2054R", "cmdRT2054R", "~/CBBN/DevReportForm/RT205RF.aspx", WhereString);
        }

        //完工
        function btn3Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var workno = row.WORKNO;
            $('#plFinish').show();
            //parent.addTab("客訴派工單完工作業", "CBBN/RT20541.aspx?workno=" + workno);
        }
        //完工處理
        function btn31Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var workno = row.WORKNO;
            var ftyp = $('#JQDataForm2FINISHTYP').refval('getValue');
            var syn = $('#JQDataForm2WORKTYPE').combobox('getValue');
            if (ftyp == "")
            {
                alert("請輸入完工種類!!");
                return false;
            }

            $('#plFinish').hide();
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT2054.cmd', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT20543" + "&parameters=" + workno + "," + ftyp + "," + usr + "," + syn,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data);
                    $('#dataGridView').datagrid('reload');
                }
            });
        }
        //完工取消
        function btn32Click(val) {
            $('#plFinish').hide();
        }

        //完工反轉
        function btn4Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var workno = row.WORKNO;

            if (confirm("是否要執行派工單返轉？")) {
                $.ajax({
                    type: "POST",
                    url: '../handler/jqDataHandle.ashx?RemoteName=sRT2054.cmd', //連接的Server端，command
                    //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                    data: "mode=method&method=" + "smRT20544" + "&parameters=" + workno + "," + usr,
                    cache: false,
                    async: false,
                    success: function (data) {
                        alert(data);
                        $('#dataGridView').datagrid('reload');
                    }
                });
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT2054.RTSndWork" runat="server" AutoApply="True"
                DataMember="RTSndWork" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="客訴派工單維護" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False" OnLoadSuccess="dgOnloadSuccess" OnSelect="MySelect">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="WORKNO" Format="" MaxLength="12" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客訴單號" Editor="text" FieldName="LINKNO" Format="" MaxLength="15" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工別" Editor="inforefval" FieldName="WORKTYPE" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P6'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="預定施工員工" Editor="infocombobox" FieldName="ASSIGNENG" Format="" MaxLength="6" Visible="true" Width="120" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="預定施工經銷商" Editor="text" FieldName="ASSIGNCONS" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="實際施工人員" Editor="infocombobox" FieldName="FINISHENG" Format="" MaxLength="6" Visible="true" Width="120" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="實際完工經銷商" Editor="text" FieldName="FINISHCONS" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="comn" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡人" Editor="text" FieldName="faqman" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工人" Editor="infocombobox" FieldName="SNDWRKUSR" Format="" Visible="true" Width="120" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" MaxLength="6" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工日" Editor="datebox" FieldName="SNDWRKDAT" Format="yyyy/mm/dd HH:MM:SS" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工人員" Editor="infocombobox" FieldName="FINISHUSR" Format="" Visible="true" Width="120" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" MaxLength="6" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd HH:MM:SS" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工方式" Editor="inforefval" FieldName="FINISHTYP" Format="" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" MaxLength="2" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd HH:MM:SS" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="comtype" Editor="text" FieldName="comtype" Visible="False" Width="80" MaxLength="0" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="True"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="作 廢" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn2Click" Text="列 印" Visible="True" Icon="icon-print" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn3Click" Text="完 工" Visible="True" Icon="icon-ok" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn4Click" Text="完工返轉" Visible="True" Icon="icon-undo" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="客訴派工單維護" Width="800px">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTSndWork" HorizontalColumnsCount="2" RemoteName="sRT2054.RTSndWork" AlwaysReadOnly="False" ChainDataFormID="JQDataForm1" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="WORKNO" Format="" maxlength="12" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客訴單" Editor="text" FieldName="LINKNO" Format="" maxlength="15" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工別" Editor="inforefval" FieldName="WORKTYPE" Format="" maxlength="2" Width="180" EditorOptions="title:'派工別',panelWidth:350,panelHeight:200,remoteName:'sRT100.RT20543',tableName:'RT20543',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P6'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="預定施工員工" Editor="inforefval" FieldName="ASSIGNENG" Format="" maxlength="6" Width="180" EditorOptions="title:'員工',panelWidth:350,panelHeight:200,remoteName:'sRT100.RT20542',tableName:'RT20542',columns:[],columnMatches:[],whereItems:[],valueField:'CODE',textField:'CODENC',valueFieldCaption:'編號',textFieldCaption:'姓名',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="預定施工經銷商" Editor="inforefval" FieldName="ASSIGNCONS" Format="" maxlength="10" Width="180" EditorOptions="title:'經銷商',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTObj',tableName:'RTObj',columns:[],columnMatches:[],whereItems:[],valueField:'CUSID',textField:'SHORTNC',valueFieldCaption:'編號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="textarea" FieldName="MEMO" Format="" maxlength="600" Width="500" EditorOptions="height:60" Span="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工人員" Editor="infocombobox" FieldName="SNDWRKUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="派工日" Editor="datebox" FieldName="SNDWRKDAT" Format="yyyy/mm/dd HH:MM:SS" Width="180" EditorOptions="dateFormat:'datetime',showTimeSpinner:true,showSeconds:false,editable:true" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="實際完工工桯師" Editor="inforefval" FieldName="FINISHENG" Format="" maxlength="6" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RT20541',tableName:'RT20541',columns:[],columnMatches:[],whereItems:[],valueField:'emply',textField:'cusnc',valueFieldCaption:'員工編號',textFieldCaption:'員工姓名',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="實際完工經銷商" Editor="inforefval" FieldName="FINISHCONS" Format="" maxlength="10" Width="180" EditorOptions="title:'經銷商',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTObj',tableName:'RTObj',columns:[],columnMatches:[],whereItems:[],valueField:'CUSID',textField:'SHORTNC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="完工人員" Editor="infocombobox" FieldName="FINISHUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd HH:MM:SS" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動員" Editor="infocombobox" FieldName="UUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動日" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd HH:MM:SS" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢員" Editor="infocombobox" FieldName="CANCELUSR" Format="" maxlength="6" Width="180" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd HH:MM:SS" Width="180" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="完工方式" Editor="inforefval" FieldName="FINISHTYP" Format="" maxlength="2" Width="160" EditorOptions="title:'完工方式',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="預定施工日期" Editor="datebox" FieldName="ASSIGNDAT" Format="yyyy/mm/dd HH:MM:SS" Width="180" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="預定施工時間" Editor="inforefval" FieldName="ASSIGNTIME" Format="" maxlength="5" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'Q4'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" Visible="False" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDataForm ID="JQDataForm1" runat="server" AlwaysReadOnly="False" ChainDataFormID="" Closed="False" ContinueAdd="False" DataMember="RTSndWork" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalColumnsCount="1" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ParentObjectID="JQDataForm1" RemoteName="sRT2054.RTSndWork" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0">
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="(2點) 申請AVS、FBB(業務)" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE01" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(1點) 安裝Sonet、SQ、AVS Call Out續約與其他" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE02" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(1點) 勸用戶退租與撤線" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE03" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(1點) 主機安裝" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE04" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(1點) 主機開通" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE05" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(1點) 用戶端裝機" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE06" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(1點) 主機拆回、拆 Port" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE07" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(1點) 退租拆機" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE08" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(1點) 用戶端設備及文件收回" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE09" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(0.8點) 維修(需有派工單)" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE10" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(0.3點) 線上維修" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE11" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(1點) 發DM(需有派工單)" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE12" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(1點) 社區深耕活動(需有派工單)" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE13" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(0.3點) 放DM、整理DM" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE14" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="(0.3點) 社區電信室勘查" Editor="checkbox" EditorOptions="on:'Y',off:''" FieldName="SCORE15" MaxLength="1" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                    </Columns>
                    <RelationColumns>
                        <JQTools:JQRelationColumn FieldName="WORKNO" ParentFieldName="WORKNO" />
                    </RelationColumns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="LINKNO" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="自動編號" FieldName="WORKNO" RemoteMethod="True" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_usercode" FieldName="SNDWRKUSR" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_usercode" FieldName="UUSR" RemoteMethod="True" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today2" FieldName="SNDWRKDAT" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today2" FieldName="UDAT" RemoteMethod="False" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
        <div id="plFinish" aria-hidden="False" hidden="hidden">
        <JQTools:JQPanel ID="JQPanel1" runat="server" Title="完工設定" Visible="True">
            <JQTools:JQDataForm ID="JQDataForm2" runat="server" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" DataMember="View_RTSndWork" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalColumnsCount="1" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" RemoteName="sRT2054.View_RTSndWork" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0">
                <Columns>
                    <JQTools:JQFormColumn Alignment="left" Caption="請選擇完工種類：" Editor="inforefval" FieldName="FINISHTYP" MaxLength="2" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQFormColumn Alignment="left" Caption="另匯出至行程表：" Editor="infocombobox" FieldName="WORKTYPE" MaxLength="2" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" EditorOptions="items:[{value:'N',text:'N',selected:'true'},{value:'Y',text:'Y',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                </Columns>
            </JQTools:JQDataForm>
            <JQTools:JQButton ID="JQButton1" runat="server" Text="送出" OnClick="btn31Click" />
            <JQTools:JQButton ID="JQButton2" runat="server" OnClick="btn32Click" Text="取消" />
        </JQTools:JQPanel>
        </div>
        <div hidden="hidden">
        <JQTools:JQDataGrid ID="JQDataGrid1" runat="server" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" AutoApply="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="cmdRT2054R" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT2054.cmdRT2054R" RowNumbers="True" Title="JQDataGrid" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
            <Columns>
                <JQTools:JQGridColumn Alignment="left" Caption="workno" Editor="text" FieldName="workno" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="linkno" Editor="text" FieldName="linkno" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="comtype" Editor="text" FieldName="comtype" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="CODENC" Editor="text" FieldName="CODENC" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="worktype" Editor="text" FieldName="worktype" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="assigneng" Editor="text" FieldName="assigneng" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="finisheng" Editor="text" FieldName="finisheng" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="comn" Editor="text" FieldName="comn" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="faqman" Editor="text" FieldName="faqman" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="sndwrkusr" Editor="text" FieldName="sndwrkusr" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="sndwrkdat" Editor="text" FieldName="sndwrkdat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="finishsur" Editor="text" FieldName="finishsur" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="finishdat" Editor="text" FieldName="finishdat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="codenc1" Editor="text" FieldName="codenc1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="canceldat" Editor="text" FieldName="canceldat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="CUSID" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="RCVDAT" Editor="text" FieldName="RCVDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="COMQ1" Editor="text" FieldName="COMQ1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="LINEQ1" Editor="text" FieldName="LINEQ1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="LINERATE" Editor="text" FieldName="LINERATE" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="idslamip" Editor="text" FieldName="idslamip" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="gateway" Editor="text" FieldName="gateway" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="CUSTIP" Editor="text" FieldName="CUSTIP" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="CASEKIND" Editor="text" FieldName="CASEKIND" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="secondcase" Editor="text" FieldName="secondcase" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="raddr" Editor="text" FieldName="raddr" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="docketdat" Editor="text" FieldName="docketdat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="duedat" Editor="text" FieldName="duedat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="tel" Editor="text" FieldName="tel" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="cusnc" Editor="text" FieldName="cusnc" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="LINKNO1" Editor="text" FieldName="LINKNO1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="FINISHDAT1" Editor="text" FieldName="FINISHDAT1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="FAQREASON" Editor="text" FieldName="FAQREASON" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="faqreasonnm" Editor="text" FieldName="faqreasonnm" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Frozen="False" IsNvarChar="False" MaxLength="600" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="1200">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="CMTYIP" Editor="text" FieldName="CMTYIP" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="strbillingdat" Editor="text" FieldName="strbillingdat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="dropdat" Editor="text" FieldName="dropdat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="overdue" Editor="text" FieldName="overdue" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="WtlApplyDat" Editor="text" FieldName="WtlApplyDat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="paycycle" Editor="text" FieldName="paycycle" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="paytype" Editor="text" FieldName="paytype" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
                <JQTools:JQGridColumn Alignment="left" Caption="freecode" Editor="text" FieldName="freecode" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                </JQTools:JQGridColumn>
            </Columns>
            <TooItems>
                <JQTools:JQToolItem Enabled="True" Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="Insert" Visible="True" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="Update" Visible="True" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="Delete" Visible="True" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="Apply" Visible="True" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="Cancel" Visible="True" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="True" />
                <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="Export" Visible="True" />
            </TooItems>
        </JQTools:JQDataGrid>
        </div>
    </form>
</body>
</html>
