<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT205.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var usr = getClientInfo('_usercode');
        var comtype = Request.getQueryStringByName2("comtype"); //個案編號
        var lineq1 = Request.getQueryStringByName2("lineq1"); //個案編號
        var comq1 = Request.getQueryStringByName2("comq1"); //個案編號
        var cusid = Request.getQueryStringByName2("cusid"); //個案編號
        var flag = true;

        function dgOnloadSuccess() {
            
            if (flag) {
                var sWhere = " 1=1 ";
                if (comq1 != "") {
                    sWhere = sWhere + " AND A.COMQ1='" + comq1 + "'";
                }

                if (lineq1 != "") {
                    sWhere = sWhere + " AND A.LINEQ1='" + lineq1 + "'"
                }

                if (cusid != "") {
                    sWhere = sWhere + " AND A.cusid='" + cusid + "'"
                }

                /*
                if (comtype != "") {
                    sWhere = sWhere + " AND A.comtype='" + comtype + "'"
                }
                */
            
                if (comq1 == "") {
                    sWhere = sWhere + " and a.closedat is null and a.canceldat is null ";
                }

                $("#dataGridMaster").datagrid('setWhere', sWhere);
                $(document).ready(function () {
                    $('#dataGridMaster').datagrid({
                        rowStyler: function (index, row) {
                            if (row.CANCELDAT == null || row.CANCELDAT == "") {
                                return 'color:black;';
                            }
                            else {
                                return 'color:red; height:50px; font-size: 22px;';
                            }

                        }
                    });
                })
            }
            

            flag = false;
            
            if (flag == false) {
                var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
                
                var ss = row.caseno;
                
                if (ss == "") ss = "ZZZZZ";
                
                $("#FAQADD").datagrid('setWhere', "CASENO='" + ss + "'");
            }                
        }

        function queryGrid(dg) { //查詢后添加固定條件            
            if ($(dg).attr('id') == 'dataGridMaster') {
                var where = $(dg).datagrid('getWhere');
                
                if (where != "") {
                    where = " 1=1 ";
                    comn = $("#comn_Query").val(); //社區名稱
                    faqman = $("#faqman_Query").val(); //客戶名稱
                    dropdat = $("#dropdat_Query").datebox('getValue'); //受理時間起
                    RCVDATE = $("#RCVDATE_Query").datebox('getValue'); //受理時間迄
                    closedat = $("#closedat_Query").datebox('getValue'); //結案時間起
                    codenc3 = $("#codenc3_Query").datebox('getValue'); //結案時間迄
                    CUSNC = $("#CUSNC_Query").combobox('getValue'); //受理人員
                    ANGENCY = $("#ANGENCY_Query").combobox('getValue'); //直經銷
                    codenc1 = $("#codenc1_Query").combobox('getValue'); //結案狀態
                    SNAME = $("#SNAME_Query").combobox('getValue'); //預定施工人員

                    
                    if (comn != "")
                        where = where + " and n.comn like '%" + comn + "%'";
                    if (faqman != "")
                        where = where + " and a.faqman like '%" + faqman + "%'";
                    //
                    if (dropdat != "")
                        where = where + " and a.RCVDAT >= '" + dropdat + "' ";
                    if (RCVDATE != "")
                        where = where + " and  a.RCVDAT <= '" + RCVDATE + "' ";
                    //
                    if (closedat!= "")
                        where = where + " AND a.closedat >= '"+closedat+"' ";
                    if (codenc3 != "")
                        where = where + " AND a.closedat <= '" + codenc3 + "' ";
                    if (CUSNC != "")
                        where = where + " AND a.rcvusr = '"+CUSNC+"' ";
                    if (ANGENCY != "0")
                        where = where + " and case n.groupnc when '' then '1' else '2' end = '"+ANGENCY+"' ";
                   
                    if (codenc1 == 1)
                        where = where + " and a.closedat is null and a.canceldat is null ";
                    if (codenc1 == 2)
                        where = where + " and a.closedat is not null and a.canceldat is null ";
                    //
                    if (SNAME != "")
                        where = where + " AND isnull(k.shortnc,i.name) = '"+SNAME+ "'";
                }                
                $(dg).datagrid('setWhere', where);
            }
        }

        function btnReloadClick() {
            //$("#dataGridView").datagrid("setWhere", "");
            $('#dataGridMaster').datagrid('reload');            
        }

        
        function btnRT103Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var comq1 = row.comq1;
            var lineq1 = row.lineq1;
            parent.addTab("主線查詢", "CBBN/RT103.aspx?COMQ1=" + comq1);
        }

        function btn1Click(val) {
            var sMODE = "I";
            
            parent.addTab("客訴資料新增", "CBBN/RT2051.aspx?caseno=自動編號" + "&sMODE=" + sMODE + "&comq1=" + comq1 + "&lineq1=" + lineq1 + "&comtype=" + comtype + "&cusid=" + cusid);
            //$('#dataGridMaster').datagrid('reload');

        }


        function btn2Click(val) {
            var sMODE = "E";
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var caseno = row.caseno;
            var custid = row.cusid;
            parent.addTab("客訴資料修改", "CBBN/RT2051.aspx?caseno=" + caseno + "&sMODE=" + sMODE);
            //$('#dataGridMaster').datagrid('reload');
        }

        function btnViewClick(val) {
            var sMODE = "B";
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var caseno = row.caseno;
            var custid = row.cusid;
            parent.addTab("客訴資料查詢", "CBBN/RT2051.aspx?caseno=" + caseno + "&sMODE=" + sMODE);
            //$('#dataGridMaster').datagrid('reload');
        }

        function btn3Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.caseno;
            parent.addTab("客服單追件", "CBBN/RT2053.aspx?caseno=" + ss);
        }

        function btn4Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.caseno;
            parent.addTab("客訴派工單維護", "CBBN/RT2054.aspx?caseno=" + ss);
        }

        //
        function btn5Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var caseno = row.caseno;

            if (confirm("是否結案？")) {
                $.ajax({
                    type: "POST",
                    url: '../handler/jqDataHandle.ashx?RemoteName=sRT205.cmd', //連接的Server端，command
                    //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                    data: "mode=method&method=" + "smRT2055" + "&parameters=" + caseno + "," + usr,
                    cache: false,
                    async: false,
                    success: function (data) {
                        alert(data);
                        $('#dataGridView').datagrid('reload');
                    }
                });
            }
        }
        function btn6Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var caseno = row.caseno;

            if (confirm("是否結案返轉？")) {
                $.ajax({
                    type: "POST",
                    url: '../handler/jqDataHandle.ashx?RemoteName=sRT205.cmd', //連接的Server端，command
                    //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                    data: "mode=method&method=" + "smRT2056" + "&parameters=" + caseno + "," + usr,
                    cache: false,
                    async: false,
                    success: function (data) {
                        alert(data);
                        $('#dataGridView').datagrid('reload');
                    }
                });
            }
        }
        function btn7Click(val) {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var caseno = row.caseno;

            if (confirm("是否作廢？")) {
                $.ajax({
                    type: "POST",
                    url: '../handler/jqDataHandle.ashx?RemoteName=sRT205.cmd', //連接的Server端，command
                    //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                    data: "mode=method&method=" + "smRT2057" + "&parameters=" + caseno + "," + usr,
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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT205.RT205" runat="server" AutoApply="False"
                DataMember="RT205" Pagination="True" QueryTitle="查詢條件"
                Title="客訴資料維護" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Panel" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客訴單號" Editor="text" FieldName="caseno" Format="" MaxLength="10" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案別" Editor="text" FieldName="comtype" Format="" MaxLength="1" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="經銷" Editor="text" FieldName="ANGENCY" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="業務" Editor="text" FieldName="leader" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案別" Editor="text" FieldName="codenc" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線" Editor="text" FieldName="COMLINE" Format="" MaxLength="0" Width="40" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="comn" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡人" Editor="text" FieldName="faqman" Format="" Width="80" MaxLength="50" />
                    <JQTools:JQGridColumn Alignment="left" Caption="室內電話" Editor="datebox" FieldName="tel" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡手機" Editor="text" FieldName="MOBILE" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="進出線" Editor="text" FieldName="codenc1" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報修原因" Editor="text" FieldName="codenc2" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="受理人" Editor="text" FieldName="CUSNC" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="受理時間" Editor="text" FieldName="RCVDATE" Format="" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案時間" Editor="datebox" FieldName="closedat" Format="yyyy/mm/dd HH:MM" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶來源" Editor="text" FieldName="codenc3" Format="" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="right" Caption="追件數" Editor="numberbox" FieldName="QT_CASE" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="預定施工人" Editor="text" FieldName="SNAME" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="已完工" Editor="text" FieldName="finishnum" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="60" Format=""></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="comq1" Editor="text" FieldName="comq1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="80"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="lineq1" Editor="text" FieldName="lineq1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="80"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="cusid" Editor="text" FieldName="cusid" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="80"></JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="entryno" Editor="text" FieldName="entryno" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢時間" Editor="text" FieldName="CANCELDAT" Format="yyyy/mm/dd HH:MM:SS" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="btn2Click" Text="更改" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-view" ItemType="easyui-linkbutton" OnClick="btnViewClick" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-view" ItemType="easyui-linkbutton" OnClick="btnRT103Click" Text="主線查詢" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn3Click" Text="追 件" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn4Click" Text="派工單" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn5Click" Text="結 案" Visible="True" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btn6Click" Text="結案返轉" Visible="True" Icon="icon-undo" />
                    <JQTools:JQToolItem ItemType="easyui-linkbutton" OnClick="btn7Click" Text="作 廢" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btnReloadClick" Text="資料更新" Visible="True" Icon="icon-reload" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區名稱" Condition="%" DataType="string" Editor="text" FieldName="comn" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="聯絡人" Condition="%" DataType="string" Editor="text" FieldName="faqman" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="受理時間起" Condition="&gt;=" DataType="datetime" Editor="datebox" FieldName="dropdat" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="受理時間迄" Condition="=" DataType="datetime" Editor="datebox" FieldName="RCVDATE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="結案時間起" Condition="=" DataType="datetime" Editor="datebox" FieldName="closedat" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="結案時間迄" Condition="=" DataType="datetime" Editor="datebox" FieldName="codenc3" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="受理人員" Condition="%" DataType="string" Editor="infocombobox" FieldName="CUSNC" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" EditorOptions="valueField:'CODE',textField:'NAME',remoteName:'sRT205.R_2051',tableName:'R_2051',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="直經銷" Condition="%" DataType="string" Editor="infocombobox" EditorOptions="items:[{value:'0',text:'全部',selected:'false'},{value:'1',text:'直銷',selected:'false'},{value:'2',text:'經銷',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="ANGENCY" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="結案狀態" Condition="%" DataType="string" Editor="infocombobox" EditorOptions="items:[{value:'0',text:'全部',selected:'false'},{value:'1',text:'未結案',selected:'true'},{value:'2',text:'已結案',selected:'false'},{value:'3',text:'未完工',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="codenc1" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="預定施工人員" Condition="%" DataType="string" Editor="infocombobox" FieldName="SNAME" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" EditorOptions="valueField:'NAME',textField:'NAME',remoteName:'sRT205.R_2052',tableName:'R_2052',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>
    <JQTools:JQDefault runat="server" BindingObjectID="dataGridMaster" BorderStyle="NotSet" Enabled="True" EnableTheming="True" ClientIDMode="Inherit" ID="defaultMaster" EnableViewState="True" ViewStateMode="Inherit" >
</JQTools:JQDefault>
<JQTools:JQValidate runat="server" BindingObjectID="dataGridMaster" BorderStyle="NotSet" Enabled="True" EnableTheming="True" ClientIDMode="Inherit" ID="validateMaster" EnableViewState="True" ViewStateMode="Inherit" >
</JQTools:JQValidate>
        <asp:Panel ID="Panel1" runat="server">
            <JQTools:JQDataGrid ID="FAQADD" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" data-options="pagination:true,view:commandview" DataMember="RTFaqAdd" DeleteCommandVisible="False" DuplicateCheck="False" EditDialogID="" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT205.RTFaqAdd" RowNumbers="True" Title="客訴追件" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客訴單號+聯絡人" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT205.View_RTFaqM',tableName:'View_RTFaqM',columns:[{field:'CASENO',title:'客訴單號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'FAQMAN',title:'報修聯絡人',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}],columnMatches:[],whereItems:[],valueField:'CASENO',textField:'FAQMAN',valueFieldCaption:'CASENO',textFieldCaption:'單號+聯絡人',cacheRelationText:false,checkData:false,showValueAndText:true,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="CASENO" Format="" MaxLength="10" Visible="true" Width="120">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" Format="" Visible="true" Width="30">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="進出線" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="IOBOUND" Format="" MaxLength="1" Visible="true" Width="50">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" MaxLength="1600" Visible="true" Width="200">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="追件人員" Editor="infocombobox" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="ADDUSR" Format="" MaxLength="6" Visible="true" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="追件時間" Editor="datebox" FieldName="ADDDAT" Format="yyyy/mm/dd HH:MM:SS" Visible="true" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="異動人員" Editor="infocombobox" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="UUSR" Format="" MaxLength="6" Visible="true" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="異動時間" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd HH:MM:SS" Visible="true" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="infocombobox" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="CANCELUSR" Format="" MaxLength="6" Visible="true" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢時間" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd HH:MM:SS" Visible="true" Width="90">
                    </JQTools:JQGridColumn>
                </Columns>
            </JQTools:JQDataGrid>
        </asp:Panel>
</form>
</body>
<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>
</html>
