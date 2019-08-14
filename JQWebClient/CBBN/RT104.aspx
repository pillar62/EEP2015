<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT104.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var COMQ1 = Request.getQueryStringByName2("COMQ1");
        var LINEQ1 = Request.getQueryStringByName2("LINEQ1");
        var usr = getClientInfo('_usercode');

        function ChangeToTable(printDatagrid) {
            var tableString = '<table cellspacing="0" class="pb">';
            var frozenColumns = printDatagrid.datagrid("options").frozenColumns; //  obtain frozenColumns object 
            var columns = printDatagrid.datagrid("options").columns;  //  obtain columns object 
            var nameList = new Array();

            //  load title
            if (typeof columns != 'undefined' && columns != '') {
                $(columns).each(function (index) {
                    tableString += '\n<tr>';
                    if (typeof frozenColumns != 'undefined' && typeof frozenColumns[index] != 'undefined') {
                        for (var i = 0; i < frozenColumns[index].length; ++i) {
                            if (!frozenColumns[index][i].hidden) {
                                tableString += '\n<th width="' + frozenColumns[index][i].width + '"';
                                if (typeof frozenColumns[index][i].rowspan != 'undefined' && frozenColumns[index][i].rowspan > 1) {
                                    tableString += ' rowspan="' + frozenColumns[index][i].rowspan + '"';
                                }
                                if (typeof frozenColumns[index][i].colspan != 'undefined' && frozenColumns[index][i].colspan > 1) {
                                    tableString += ' colspan="' + frozenColumns[index][i].colspan + '"';
                                }
                                if (typeof frozenColumns[index][i].field != 'undefined' && frozenColumns[index][i].field != '') {
                                    nameList.push(frozenColumns[index][i]);
                                }
                                tableString += '>' + frozenColumns[0][i].title + '</th>';
                            }
                        }
                    }
                    for (var i = 0; i < columns[index].length; ++i) {
                        if (!columns[index][i].hidden) {
                            tableString += '\n<th width="' + columns[index][i].width + '"';
                            if (typeof columns[index][i].rowspan != 'undefined' && columns[index][i].rowspan > 1) {
                                tableString += ' rowspan="' + columns[index][i].rowspan + '"';
                            }
                            if (typeof columns[index][i].colspan != 'undefined' && columns[index][i].colspan > 1) {
                                tableString += ' colspan="' + columns[index][i].colspan + '"';
                            }
                            if (typeof columns[index][i].field != 'undefined' && columns[index][i].field != '') {
                                nameList.push(columns[index][i]);
                            }
                            tableString += '>' + columns[index][i].title + '</th>';
                        }
                    }
                    tableString += '\n</tr>';
                });
            }
            //  Load content 
            var rows = printDatagrid.datagrid("getRows"); //  This code is to get all the lines of the current page 
            for (var i = 0; i < rows.length; ++i) {
                tableString += '\n<tr>';
                for (var j = 0; j < nameList.length; ++j) {
                    var e = nameList[j].field.lastIndexOf('_0');

                    tableString += '\n<td';
                    if (nameList[j].align != 'undefined' && nameList[j].align != '') {
                        tableString += ' style="text-align:' + nameList[j].align + ';"';
                    }
                    tableString += '>';
                    if (e + 2 == nameList[j].field.length) {
                        tableString += rows[i][nameList[j].field.substring(0, e)];
                    }
                    else
                        tableString += rows[i][nameList[j].field];
                    tableString += '</td>';
                }
                tableString += '\n</tr>';
            }
            tableString += '\n</table>';
            return tableString;
        }

        
        function Export(strXlsName, exportGrid) {
            var f = $('<form action="../export.aspx" method="post" id="fm1"></form>');
            var i = $('<input type="hidden" id="txtContent" name="txtContent" />');
            var l = $('<input type="hidden" id="txtName" name="txtName" />');
            alert("1");
            i.val(ChangeToTable(exportGrid));
            alert("1");
            i.appendTo(f);
            alert("2");
            l.val(strXlsName);
            alert("3");
            l.appendTo(f);
            alert("4");
            f.appendTo(document.body).submit();
            alert("5");
            document.body.removeChild(f);
        }
        

        var flag = true;
        var bIns = false;
        if (COMQ1 == "") {
            flag = false;
        }

        function InsDefault() {
            if (COMQ1 != "") {
                return COMQ1;
            }
        }

        //轉excel
        
        function btnCreateClick() {
            //$('#dg').datagrid('toExcel', 'PI.xls');
            Export('用戶明細', $('#dataGridView'));
            //exportGrid('#dg');
            //exportGrid('#dataGridMaster');
        }

        function btnExportExcelClick() {
            var where = $('#dataGridView').datagrid('getWhere');
            $('#dgEXCEL').datagrid('setWhere', where);
            exportGrid('#dgEXCEL');
        }

        function LinkRT1041(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("維修收款", "CBBN/RT1041.aspx?CUSID=" + ss);
        }

        function btn1Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("裝機派工單資料維護", "CBBN/RT1042.aspx?CUSID=" + ss);
        }

        function btn2Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            var s1 = row.DUEDAT;
            var s2 = row.COMTYPE;
            parent.addTab("用戶續約作業", "CBBN/RT1043.aspx?CUSID=" + ss + "&COMTYPE=" + s2 + "&DUEDAT=" + s1);
        }

        function btn3Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            var s1 = row.DUEDAT;
            var s2 = row.COMTYPE;
            parent.addTab("用戶復機作業", "CBBN/RT1044.aspx?CUSID=" + ss + "&COMTYPE=" + s2 + "&DUEDAT=" + s1);
        }

        function btn4Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("用戶退租作業", "CBBN/RT1045.aspx?CUSID=" + ss);
        }

        function btn5Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("用戶應收應付帳款查詢", "CBBN/RT1046.aspx?CUSID=" + ss);
        }

        function btn6Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            var comq1 = row.COMQ1;
            var linq1 = row.LINEQ1;
            var comtype = row.COMTYPE;
            
            parent.addTab("用戶客服單資料維護-RT205", "CBBN/RT205.aspx?cusid=" + ss + "&comq1=" + comq1 + "&lineq1=" + linq1 + "&comtype=" + comtype);
        }

        function btn7Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("設備保管收據列印", "CBBN/RT1048.aspx?CUSID=" + ss);
        }

        function btn9Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("用戶調整到期日資料維護", "CBBN/RT1049.aspx?CUSID=" + ss);
        }

        function btn10Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("用戶設備資料查詢", "CBBN/RT104A.aspx?CUSID=" + ss);
        }

        //作廢
        function btn11Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var COMQ1 = row.COMQ1;
            var LINEQ1 = row.LINEQ1;
            var CUSID = row.CUSID;
            
            
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT104.cmdRT104B', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT104B" + "&parameters=" + COMQ1 + "," + LINEQ1 + "," + CUSID + "," + usr,
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

        //取得遠傳會員編號
        function getMemberID() {
            var COMQ1 = $('#dataFormMasterCOMQ1').refval('getValue');
            var COMTYPE = $('#dataFormMasterCOMTYPE').refval('getValue');
            var MEMBERID = $('#dataFormMasterMEMBERID').val();

            if (MEMBERID=="" && COMTYPE=="B"){
                $.ajax({
                    type: "POST",
                    url: '../handler/jqDataHandle.ashx?RemoteName=sRT104.cmd', //連接的Server端，command
                    //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                    data: "mode=method&method=" + "smRT104FAR" + "&parameters=" + COMQ1,
                    cache: false,
                    async: false,
                    success: function (data) {
                        $('#dataFormMasterMEMBERID').val(data);
                        return data;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
        }

        //作廢返轉
        function btn12Click() {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var COMQ1 = row.COMQ1;
            var LINEQ1 = row.LINEQ1;
            var CUSID = row.CUSID;

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT104.cmdRT104C', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT104C" + "&parameters=" + COMQ1 + "," + LINEQ1 + "," + CUSID + "," + usr,
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

        function btn13Click(val) {
            var row = $('#dataGridView').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.CUSID;
            parent.addTab("用戶異動資料查詢", "CBBN/RT104D.aspx?CUSID=" + ss);
        }

        function InsDefaultLINEQ1() {
            if (LINEQ1 != "") {
                return LINEQ1;
            }
            else
            {
                return 1;
                flag = false;
            }
        }
        function RT201CUSID(val, rowData) {
            return '<a target="_blank" href="../RT201.aspx?CUSID=' + val + '">' + val + '</a>';
        }

        function MySelect(rowIndex, rowData)
        {
            if (flag == false) {
                var ss = rowData.CUSID;
                if (ss == "") ss = "ZZZZZ";
                $("#V_RTLessorAVSCustFaqH").datagrid('setWhere', "A.cusid='" + ss + "'"); //維護單 
                $("#RTLessorAVSCustCont").datagrid('setWhere', "RTLessorAVSCustCont.cusid='" + ss + "'"); //客戶續約單
                $("#RTLessorAVSCustDrop").datagrid('setWhere', "RTLessorAVSCustDrop.cusid='" + ss + "'"); //客戶退租單 
                $("#RTLessorAVSCustReturn").datagrid('setWhere', "RTLessorAVSCustReturn.cusid='" + ss + "'"); //客戶復機單 
                $("#RTLessorAVSCustAR").datagrid('setWhere', "cusid='" + ss + "'"); //客戶應收付單 
            }
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

            /*if (LINEQ1 == "")
            {
                $('#btnIns').hide();
                $('#btnsave').hide();
                $('#btncancel').hide();
                //設定唯讀
                setReadOnly($('#dataGridView'), true);
            }*/

            var rows = $("#dataGridView").datagrid("getRows");
            if (rows > 0) {
                var row = $("#dataGridView").datagrid("getSelected");//取得當前主檔中選中的那個Data

                var ss = row.CUSID;
                if (ss == "") ss = "ZZZZZ";
            }
            else
                ss = "ZZZZZZ";

            if (flag == false)
            {
                $("#V_RTLessorAVSCustFaqH").datagrid('setWhere', "A.cusid='" + ss + "'");
                $("#RTLessorAVSCustCont").datagrid('setWhere', "RTLessorAVSCustCont.cusid='" + ss + "'");
                $("#RTLessorAVSCustDrop").datagrid('setWhere', "RTLessorAVSCustDrop.cusid='" + ss + "'"); //客戶退租單
                $("#RTLessorAVSCustReturn").datagrid('setWhere', "RTLessorAVSCustReturn.cusid='" + ss + "'"); //客戶復機單
                $("#RTLessorAVSCustAR").datagrid('setWhere', "cusid='" + ss + "'"); //客戶應收付單 
            }

            flag = false;
        }
        function FilterTown1(val) {
            try {
                $('#dataFormMasterTOWNSHIP1').combobox('setValue', "");
                //$('#dataFormMasterTOWNSHIP1').combobox('setWhere', "CUTID = '" + val.CUTID + "'");
            }
            catch (err) {
                alert(err);
            }
        }
        function FilterTown2(val) {
            try {
                $('#dataFormMasterTOWNSHIP2').refval('setValue', "");
                //$('#dataFormMasterTOWNSHIP2').combobox('setWhere', "CUTID = '" + val.CUTID + "'");
            }
            catch (err) {
                alert(err);
            }
        }
        function FilterTown3(val) {
            try {
                $('#dataFormMasterTOWNSHIP3').refval('setValue', "");
                //$('#dataFormMasterTOWNSHIP3').combobox('setWhere', "CUTID = '" + val.CUTID + "'");
            }
            catch (err) {
                alert(err);
            }
        }
        function OnLoadSuccess(val) {
            try {
                /*var val = $('#dataFormMasterCUTID1').combobox('getValue');
                $('#dataFormMasterTOWNSHIP1').combobox('setWhere', "CUTID = '" + val + "'");
                var val = $('#dataFormMasterCUTID2').combobox('getValue');
                $('#dataFormMasterTOWNSHIP2').combobox('setWhere', "CUTID = '" + val + "'");
                var val = $('#dataFormMasterCUTID3').combobox('getValue');
                $('#dataFormMasterTOWNSHIP3').combobox('setWhere', "CUTID = '" + val + "'");*/

                $('#dataGridView').datagrid({
                    rowStyler: function (index, row) {
                        if (row.DROPDAT == null && row.CANCELDAT == null)
                        {
                            return 'background-color:white;color:black;';
                        }                    
                        else {
                            return 'background-color:red;color:blue; height:50px; font-size: 22px;';
                        }    
                    }
                });
            }
            catch (err) {
                alert(err);
            }
        }

        function faddr()
        {
            if ($('#dataFormMasterCUTID3').combobox('getValue') == "") {
                $('#dataFormMasterCUTID3').combobox('setValue', $('#dataFormMasterCUTID2').combobox('getValue'));
                $('#dataFormMasterTOWNSHIP3').refval('setValue', $('#dataFormMasterTOWNSHIP2').refval('getValue'));
                $('#dataFormMasterRADDR3').val($('#dataFormMasterRADDR2').val());
                $('#dataFormMasterRZONE3').val($('#dataFormMasterRZONE2').val());
            }
        }


        $(document).ready(function () {
            $('#dataGridView').datagrid({
                rowStyler: function (index, row) {
                    if (flag == false) {
                        if (row.DROPDAT == null && row.CANCELDAT == null) {
                            return 'color:black;';
                        }
                        else {
                            return 'color:red; height:50px; font-size: 22px;';
                        }
                    }
                }
            });
        })

        function dataFormMaster_OnApplied(rows) {
            //新增之後取得後端的鍵值資料後顯示新增的資料
            if (getEditMode($(this)) == "inserted") {
                var ss = rows[0].CUSID;
                if (ss != "") {
                    sWhere = " CUSID='" + ss + "' ";
                    $("#dataGridView").datagrid('setWhere', sWhere);
                }
            }
            else
            {
                $("#dataGridView").datagrid('reload');
            }
        }

        function btnReloadClick() {            
            $('#dataGridView').datagrid('reload');
        }

        function btnReloadClick() {
            $('#dataGridView').datagrid('reload');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT104.V_RTLessorAVSCust" runat="server" AutoApply="True"
                DataMember="V_RTLessorAVSCust" Pagination="True" QueryTitle="查詢" EditDialogID="JQDialog1"
                Title="用戶維護" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Panel" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False" OnLoadSuccess="dgOnloadSuccess" OnSelect="MySelect">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Visible="true" Width="120" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.View_RTLessorAVSCmtyH',tableName:'View_RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200"/>
                    <JQTools:JQGridColumn Alignment="right" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Visible="False" Width="50" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶代號" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Visible="False" Width="80" EditorOptions="" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶名" Editor="text" FieldName="CUSNC" Format="" MaxLength="30" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="縣市" Editor="infocombogrid" FieldName="CUTID2" MaxLength="0" Visible="true" Width="80" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.View_RTCounty',tableName:'View_RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,onSelect:FilterTown2,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="鄉鎮市" Editor="text" FieldName="TOWNSHIP2" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="裝機地址" Editor="text" FieldName="RADDR2" MaxLength="60" Visible="true" Width="160" Format="" />
                    <JQTools:JQGridColumn Alignment="left" Caption="連絡手機" Editor="text" FieldName="MOBILE" Visible="True" Width="80" Format="" MaxLength="30" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案別" Editor="inforefval" FieldName="COMTYPE" MaxLength="0" Visible="true" Width="80" EditorOptions="title:'方案別',panelWidth:350,panelHeight:200,remoteName:'sRT100.RT104P5',tableName:'RT104P5',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'個案代號',textFieldCaption:'個案名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="資費" Editor="inforefval" FieldName="CASEKIND" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.View_RTCode',tableName:'View_RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQGridColumn Alignment="left" Caption="IP" Editor="text" FieldName="IP11" Format="" MaxLength="3" Visible="true" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報竣日" Editor="datebox" FieldName="DOCKETDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="STRBILLINGDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最近續約計費日" Editor="datebox" FieldName="NEWBILLINGDAT" Format="yyyy/mm/dd" Visible="true" Width="80" MaxLength="0" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日" Editor="datebox" FieldName="DUEDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="公關戶" Editor="text" FieldName="FREECODE" Format="" MaxLength="1" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="客訴次數" Editor="text" FieldName="QT_CC" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="False"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="btnExportExcelClick" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="維修收款" Visible="True" OnClick="LinkRT1041" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="新戶入帳" Visible="True" OnClick="btn1Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="續約作業" Visible="True" OnClick="btn2Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="復機作業" Visible="True" OnClick="btn3Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="退租作業" Visible="True" OnClick="btn4Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="應收應付" Visible="True" OnClick="btn5Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="客服案件" Visible="True" OnClick="btn6Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="設備保管收據列印" Visible="False" OnClick="btn7Click" Icon="icon-print" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="用戶移動" Visible="False" OnClick="btn8Click" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="調整到期" Visible="True" OnClick="btn9Click" Icon="icon-edit "/>
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="設備查詢" Visible="False" OnClick="btn10Click" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢" Visible="True" OnClick="btn11Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢反轉" Visible="True" OnClick="btn12Click" Icon="icon-undo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="歷史異動" Visible="False" OnClick="btn13Click" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btnReloadClick" Text="資料更新" Visible="True" Icon="icon-reload" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區名稱" Condition="%%" DataType="string" Editor="text" FieldName="COMN" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="用戶(公司)名稱" Condition="%%" DataType="string" Editor="text" FieldName="CUSNC" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="連絡手機" Condition="%%" DataType="string" Editor="text" FieldName="MOBILE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="裝機地址" Condition="%%" DataType="string" Editor="text" FieldName="RADDR2" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="身份證號" Condition="%%" DataType="string" Editor="text" FieldName="SOCIALID" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="室內電話" Condition="%%" DataType="string" Editor="text" FieldName="CONTACTTEL" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="方案別" Condition="%" DataType="string" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="COMTYPE" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="140" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="資費" Condition="=" DataType="string" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="CASEKIND" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="篩選已過期(未作廢/退租)" Condition="=" DataType="string" Editor="infooptions" EditorOptions="title:'JQOptions',panelWidth:300,remoteName:'',tableName:'',valueField:'',textField:'',columnCount:2,multiSelect:false,openDialog:true,selectAll:false,selectOnly:false,items:[{text:'過期',value:'Y'},{text:'正常',value:''},{text:'作廢或退租',value:'E'}]" FieldName="EXPIRED_YN" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶維護" Width="1000px">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTLessorAVSCust" HorizontalColumnsCount="3" RemoteName="sRT104.RTLessorAVSCust" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0" OnApplied="dataFormMaster_OnApplied" >

                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="inforefval" EditorOptions="title:'社區查詢',panelWidth:350,panelHeight:200,remoteName:'sRT101.View_RTLessorAVSCmtyH',tableName:'View_RTLessorAVSCmtyH',columns:[],columnMatches:[{field:'COMTYPE',value:'COMTYPE'}],whereItems:[],valueField:'COMQ1',textField:'COMN',valueFieldCaption:'社區代號',textFieldCaption:'社區名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,onSelect:getMemberID,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="COMQ1" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="200" Format="" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案別" Editor="inforefval" FieldName="COMTYPE" Width="200" EditorOptions="title:'方案別',panelWidth:350,panelHeight:200,remoteName:'sRT100.RT104P5',tableName:'RT104P5',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'個案代號',textFieldCaption:'個案名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,onSelect:getMemberID,selectOnly:true,capsLock:'none',fixTextbox:'false'" ReadOnly="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="資費" Editor="inforefval" FieldName="CASEKIND" Format="" Width="200" ReadOnly="False" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'},{field:'PARM1',value:'row[COMTYPE]'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" MaxLength="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶來源" Editor="infocombobox" FieldName="CUSTSRC" maxlength="0" Width="80" ReadOnly="False" EditorOptions="items:[{value:'01',text:'元訊客戶',selected:'true'},{value:'02',text:'社區潛戶',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線序號" Editor="inforefval" FieldName="LINEQ1" Format="" maxlength="0" Width="200" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT103.View_RTLessorAVSCmtyLine',tableName:'View_RTLessorAVSCmtyLine',columns:[],columnMatches:[],whereItems:[{field:'COMQ1',value:'row[COMQ1]'}],valueField:'LINEQ1',textField:'LINEQ1',valueFieldCaption:'主線序號',textFieldCaption:'主線序號',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" Visible="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶代號" Editor="text" FieldName="CUSID" Format="" maxlength="15" Width="200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第二戶" Editor="infocombobox" FieldName="SECONDCASE" Format="" Width="200" EditorOptions="items:[{value:'Y',text:'Y',selected:'false'},{value:'N',text:'N',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" MaxLength="1" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶(公司)名稱" Editor="text" FieldName="CUSNC" Format="" maxlength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" maxlength="0" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公關戶(Y)" Editor="infocombobox" FieldName="FREECODE" Format="" maxlength="1" Width="200" EditorOptions="items:[{value:'Y',text:'Y',selected:'false'},{value:'N',text:'N',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="室內電話" Editor="text" FieldName="CONTACTTEL" Format="" MaxLength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="連絡手機" Editor="text" FieldName="MOBILE" Format="" maxlength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第一證照別" Editor="inforefval" FieldName="IDNUMBERTYPE" Format="" maxlength="2" Width="200" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'J5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第一證件號碼" Editor="text" FieldName="SOCIALID" Format="" maxlength="10" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第二證照別" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'L3'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="SECONDIDTYPE" Format="" maxlength="2" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="第二證照號碼" Editor="text" FieldName="SECONDNO" Format="" maxlength="15" RowSpan="1" Span="1" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="生日" Editor="datebox" FieldName="BIRTHDAY" Format="yyyy/mm/dd" maxlength="0" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="E-Mail" Editor="text" FieldName="EMAIL" Format="" maxlength="50" Width="200" RowSpan="1" />
                        <JQTools:JQFormColumn Alignment="left" Caption="裝機地址" Editor="infocombobox" FieldName="CUTID2" Format="" maxlength="2" Width="200" RowSpan="1" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.View_RTCounty',tableName:'View_RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,onSelect:FilterTown2,panelHeight:200" NewRow="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="鄉鎮市" Editor="inforefval" FieldName="TOWNSHIP2" Format="" maxlength="10" NewRow="False" RowSpan="0" Span="1" Width="200" EditorOptions="title:'鄉鎮市區查詢',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCtyTown',tableName:'RTCtyTown',columns:[],columnMatches:[{field:'RZONE2',value:'ZIP'}],whereItems:[{field:'CUTID',value:'row[CUTID2]'}],valueField:'TOWNSHIP',textField:'TOWNSHIP',valueFieldCaption:'鄉鎮區',textFieldCaption:'鄉鎮區',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="郵遞區號" Editor="text" FieldName="RZONE2" Format="" maxlength="5" Width="200" RowSpan="0" />
                        <JQTools:JQFormColumn Alignment="left" Caption="  " Editor="text" FieldName="RADDR2" Format="" maxlength="60" Width="780" NewRow="True" OnBlur="faddr" RowSpan="0" Span="3" />
                        <JQTools:JQFormColumn Alignment="left" Caption="帳單地址" Editor="infocombobox" FieldName="CUTID3" Format="" maxlength="2" Width="200" EditorOptions="valueField:'CUTID',textField:'CUTNC',remoteName:'sRT100.View_RTCounty',tableName:'View_RTCounty',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,onSelect:FilterTown3,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="鄉鎮市" Editor="inforefval" FieldName="TOWNSHIP3" Format="" maxlength="10" Width="200" Span="1" EditorOptions="title:'鄉鎮市區查詢',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCtyTown',tableName:'RTCtyTown',columns:[],columnMatches:[{field:'RZONE3',value:'ZIP'}],whereItems:[{field:'CUTID',value:'row[CUTID3]'}],valueField:'TOWNSHIP',textField:'TOWNSHIP',valueFieldCaption:'鄉鎮區',textFieldCaption:'鄉鎮區',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />                        
                        <JQTools:JQFormColumn Alignment="left" Caption="郵遞區號" Editor="text" FieldName="RZONE3" Format="" maxlength="5" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="   " Editor="text" FieldName="RADDR3" Format="" maxlength="60" Width="780" Span="3" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司連絡人" Editor="text" FieldName="COCONTACT" Format="" maxlength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司電話" Editor="text" FieldName="COCONTACTTEL" Format="" maxlength="15" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發票抬頭" Editor="text" FieldName="INVTITLE" Format="" maxlength="200" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發票統編" Editor="text" FieldName="UNINO" Format="" maxlength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="公司負責人" Editor="text" FieldName="COBOSS" Format="" maxlength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="負責人身份證字號" Editor="text" FieldName="COBOSSID" Format="" maxlength="10" Width="200" ReadOnly="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="行業別" Editor="inforefval" FieldName="COKIND" Format="" maxlength="2" Width="200" ReadOnly="False" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'J8'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔人" Editor="infocombobox" FieldName="EUSR" Format="" maxlength="6" Width="200" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="建檔日" Editor="datebox" FieldName="EDAT" Format="yyyy/mm/dd" maxlength="0" Width="200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改人" Editor="infocombobox" FieldName="UUSR" Format="" maxlength="6" Width="200" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="修改日" Editor="datebox" FieldName="UDAT" Format="yyyy/mm/dd" maxlength="0" Width="200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" maxlength="0" Width="200" ReadOnly="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢員" Editor="infocombobox" FieldName="CANCELUSR" Format="" Width="200" ReadOnly="False" MaxLength="6" EditorOptions="valueField:'EMPLY',textField:'NAME',remoteName:'sRT100.RTEmployee',tableName:'RTEmployee',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶速率" Editor="text" FieldName="USERRATE" Format="" maxlength="10" Width="200" ReadOnly="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費週期" Editor="inforefval" FieldName="PAYCYCLE" Format="" Width="200" ReadOnly="False" EditorOptions="title:'JQRefval',panelWidth:350,remoteName:'sRT100.cmdRTBillCharge',tableName:'cmdRTBillCharge',columns:[],columnMatches:[{field:'PERIOD',value:'PERIOD'},{field:'RCVMONEY',value:'AMT'}],whereItems:[{field:'PARM1',value:'row[COMTYPE]'},{field:'CASEKIND',value:'row[CASEKIND]'}],valueField:'PAYCYCLE',textField:'MEMO',valueFieldCaption:'代碼',textFieldCaption:'備註',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" MaxLength="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="繳費方式" Editor="inforefval" FieldName="PAYTYPE" Format="" Width="200" EditorOptions="title:'JQRefval',panelWidth:350,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'代碼名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" MaxLength="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="當期收款金額" Editor="numberbox" FieldName="RCVMONEY" Format="" maxlength="0" Width="200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡種類" Editor="inforefval" FieldName="CREDITCARDTYPE" Format="" maxlength="2" Width="200" EditorOptions="title:'信用卡類別',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'M6'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="發卡銀行" Editor="inforefval" FieldName="CREDITBANK" Format="" maxlength="3" Width="200" EditorOptions="title:'銀行',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTBank',tableName:'RTBank',columns:[],columnMatches:[],whereItems:[{field:'CREDITCARD',value:'Y'}],valueField:'HEADNO',textField:'HEADNC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                        <JQTools:JQFormColumn Alignment="left" Caption="卡號" Editor="text" FieldName="CREDITCARDNO" Format="" maxlength="16" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="持卡人姓名" Editor="text" FieldName="CREDITNAME" Format="" maxlength="30" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效截止月" Editor="text" FieldName="CREDITDUEM" Format="" Width="200" MaxLength="2" />
                        <JQTools:JQFormColumn Alignment="left" Caption="信用卡有效截止年" Editor="text" FieldName="CREDITDUEY" Format="" maxlength="2" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" maxlength="12" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="轉應收帳款日" Editor="datebox" FieldName="CDAT" Format="yyyy/mm/dd" maxlength="0" Width="200" EditorOptions="" />
                        <JQTools:JQFormColumn Alignment="left" Caption="裝機費" Editor="numberbox" FieldName="SETMONEY" Format="" maxlength="0" Width="200" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金序號" Editor="text" FieldName="GTSERIAL" Format="" maxlength="12" Width="200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金收據列印人" Editor="text" FieldName="GTPRTUSR" Format="" maxlength="10" Width="200" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金收據列印日" Editor="datebox" FieldName="GTPRTDAT" Format="yyyy/mm/dd" maxlength="0" Width="200" EditorOptions="" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金" Editor="numberbox" FieldName="GTMONEY" Format="" Width="200" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="保證金退還日期" Editor="datebox" FieldName="GTREPAYDAT" Format="yyyy/mm/dd" Width="200" MaxLength="0" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="A端" Editor="text" FieldName="COTIN" maxlength="0" Width="200" Visible="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="B端" Editor="text" FieldName="PORT" maxlength="0" Width="200" Visible="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="C端" Editor="text" FieldName="COTOUT" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="共線" Editor="infocombobox" FieldName="YN_COLINE" Width="200" EditorOptions="items:[{value:'Y',text:'Y',selected:'false'},{value:'N',text:'N',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="IP(XXX.xxx.xxx.xxx)" Editor="text" FieldName="IP11" Format="" Width="200" MaxLength="20" />
                        <JQTools:JQFormColumn Alignment="left" Caption="用戶CPE Mac Address" Editor="text" FieldName="MAC" Format="" maxlength="12" Width="200" Visible="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" maxlength="0" Width="200" Visible="True" ReadOnly="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="報竣日" Editor="datebox" FieldName="DOCKETDAT" Format="yyyy/mm/dd" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="STRBILLINGDAT" Format="yyyy/mm/dd" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="最近續約計費日" Editor="datebox" FieldName="NEWBILLINGDAT" Width="200" Format="yyyy/mm/dd" ReadOnly="False" />
                        <JQTools:JQFormColumn Alignment="left" Caption="可使用期數" Editor="numberbox" FieldName="PERIOD" Format="" Width="200" MaxLength="0" Span="1" ReadOnly="True" />
                        <JQTools:JQFormColumn Alignment="left" Caption="調整日數" Editor="numberbox" FieldName="ADJUSTDAY" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="200" Format="" />
                        <JQTools:JQFormColumn Alignment="left" Caption="使用截止日" Editor="datebox" EditorOptions="" FieldName="DUEDAT" Format="yyyy/mm/dd" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="200" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="textarea" EditorOptions="height:30" FieldName="MEMO" Format="" MaxLength="500" NewRow="False" ReadOnly="False" RowSpan="1" Span="2" Visible="True" Width="500" />
                        <JQTools:JQFormColumn Alignment="left" Caption="遠傳用戶編號" Editor="text" FieldName="MEMBERID" MaxLength="0" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                    </Columns>
                </JQTools:JQDataForm>

                <JQTools:JQDataGrid ID="dataGridDetail" runat="server" AutoApply="False" DataMember="RTLessorAVSCustReturn" Pagination="False" ParentObjectID="dataFormMaster" RemoteName="sRT104.RTLessorAVSCust" Title="明細資料" AlwaysClose="True" Visible="False" >
                    <Columns>
                        <JQTools:JQGridColumn Alignment="left" Caption="CUSID" Editor="text" FieldName="CUSID" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="ENTRYNO" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="APPLYDAT" Editor="datebox" FieldName="APPLYDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="PAYCYCLE" Editor="text" FieldName="PAYCYCLE" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="PERIOD" Editor="numberbox" FieldName="PERIOD" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="SECONDCASE" Editor="text" FieldName="SECONDCASE" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="AMT" Editor="numberbox" FieldName="AMT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="PAYTYPE" Editor="text" FieldName="PAYTYPE" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CREDITCARDTYPE" Editor="text" FieldName="CREDITCARDTYPE" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CREDITBANK" Editor="text" FieldName="CREDITBANK" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CREDITCARDNO" Editor="text" FieldName="CREDITCARDNO" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CREDITNAME" Editor="text" FieldName="CREDITNAME" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CREDITDUEM" Editor="text" FieldName="CREDITDUEM" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CREDITDUEY" Editor="text" FieldName="CREDITDUEY" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="REALAMT" Editor="numberbox" FieldName="REALAMT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="TARDAT" Editor="datebox" FieldName="TARDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="BATCHNO" Editor="text" FieldName="BATCHNO" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="TUSR" Editor="text" FieldName="TUSR" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="FINISHDAT" Editor="datebox" FieldName="FINISHDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CANCELDAT" Editor="datebox" FieldName="CANCELDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="CANCELUSR" Editor="text" FieldName="CANCELUSR" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="MEMO" Editor="text" FieldName="MEMO" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="EDAT" Editor="datebox" FieldName="EDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="EUSR" Editor="text" FieldName="EUSR" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="UDAT" Editor="datebox" FieldName="UDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="UUSR" Editor="text" FieldName="UUSR" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="ADJUSTDAY" Editor="numberbox" FieldName="ADJUSTDAY" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="STRBILLINGDAT" Editor="datebox" FieldName="STRBILLINGDAT" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="MAXENTRYNO" Editor="numberbox" FieldName="MAXENTRYNO" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="right" Caption="復機費" Editor="numberbox" FieldName="RETURNMONEY" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="方案類型(KIND='O9')" Editor="text" FieldName="CASEKIND" Format="" Width="120" />
                        <JQTools:JQGridColumn Alignment="left" Caption="收款日期(復機)" Editor="datebox" FieldName="RCVMONEYDAT" Format="" Width="120" />
                    </Columns>
                    <RelationColumns>
                        <JQTools:JQRelationColumn FieldName="CUSID" ParentFieldName="CUSID" />
                    </RelationColumns>
                    <TooItems>
                        <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                        <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="更改" />
                        <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" />
                    </TooItems>
                </JQTools:JQDataGrid>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" EnableTheming="True">
                    <Columns>
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefault" FieldName="COMQ1" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultMethod="InsDefaultLINEQ1" FieldName="LINEQ1" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="自動編號" FieldName="CUSID" RemoteMethod="False" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="_today" FieldName="EDAT" RemoteMethod="True" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="01" FieldName="CUSTSRC" RemoteMethod="True" />
                        <JQTools:JQDefaultColumn CarryOn="False" DefaultValue="N" FieldName="FREECODE" RemoteMethod="False" />
                    </Columns>
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
                <JQTools:JQDefault ID="defaultDetail" runat="server" BindingObjectID="dataGridDetail" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateDetail" runat="server" BindingObjectID="dataGridDetail" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
        <p>
            <JQTools:JQDataGrid ID="V_RTLessorAVSCustFaqH" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="RT205" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT205.RT205" RowNumbers="True" Title="客戶服務單" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="True" Visible="False">
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
                <TooItems>
                    <JQTools:JQToolItem Enabled="True" Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="Insert" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="Update" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="Delete" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="Apply" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="Cancel" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="Export" Visible="False" />
                </TooItems>
            </JQTools:JQDataGrid>
            <JQTools:JQDataGrid ID="RTLessorAVSCustCont" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RTLessorAVSCustCont" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT104.V_RTLessorAVSCustCont" RowNumbers="True" Title="客戶續約單" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶序號" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="30">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="32">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="續約申請日" Editor="text" FieldName="APPLYDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="text" FieldName="STRBILLINGDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費週期" Editor="text" FieldName="PAYCYCLE" Frozen="False" IsNvarChar="False" MaxLength="2" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="40">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費方式" Editor="text" FieldName="PAYTYPE" Frozen="False" IsNvarChar="False" MaxLength="2" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="40">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="可用期數" Editor="text" FieldName="PERIOD" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="60">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="第二戶" Editor="text" FieldName="SECONDCASE" Frozen="False" IsNvarChar="False" MaxLength="1" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="60">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="應收金額" Editor="text" FieldName="AMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="實收金額" Editor="text" FieldName="REALAMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款日" Editor="text" FieldName="TARDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="續約結案日期" Editor="text" FieldName="FINISHDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="處理天數" Editor="text" FieldName="PROCESSDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="64">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Enabled="True" Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="Insert" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="Update" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="Delete" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="Apply" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="Cancel" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="Export" Visible="False" />
                </TooItems>
            </JQTools:JQDataGrid>
            <JQTools:JQDataGrid ID="RTLessorAVSCustDrop" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RTLessorAVSCustDrop" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT104.V_RTLessorAVSCustDrop" RowNumbers="True" Title="客戶退租單" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="CUSID" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="false" Width="20">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="32">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="退租種類" Editor="text" FieldName="CODENC" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="120">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="退租申請日" Editor="text" FieldName="APPLYDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="預定退租日" Editor="text" FieldName="ENDDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="退租結案日" Editor="text" FieldName="FINISHDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="cusnc1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="拆機工單" Editor="text" FieldName="SNDPRTNO" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="轉拆機單日" Editor="text" FieldName="SNDWORK" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="拆機結案日" Editor="text" FieldName="SNDWORKCLOSE" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="處理天數" Editor="text" FieldName="processdat" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Enabled="True" Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="Insert" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="Update" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="Delete" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="Apply" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="Cancel" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="Export" Visible="False" />
                </TooItems>
            </JQTools:JQDataGrid>
            <JQTools:JQDataGrid ID="RTLessorAVSCustReturn" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RTLessorAVSCustReturn" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT104.V_RTLessorAVSCustReturn" RowNumbers="True" Title="客戶復機單" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="CUSID" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="false" Width="30">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="32">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="復機申請日" Editor="text" FieldName="APPLYDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70"  Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="text" FieldName="STRBILLINGDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費週期" Editor="text" FieldName="PAYCYCLE" Frozen="False" IsNvarChar="False" MaxLength="2" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="64">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費方式" Editor="text" FieldName="PAYTYPE" Frozen="False" IsNvarChar="False" MaxLength="2" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="64">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="可用期數" Editor="text" FieldName="PERIOD" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="第二戶" Editor="text" FieldName="SECONDCASE" Frozen="False" IsNvarChar="False" MaxLength="1" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="48">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="應收金額" Editor="text" FieldName="AMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="實收金額" Editor="text" FieldName="REALAMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="轉應收帳款日" Editor="text" FieldName="TARDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="100">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="復機結案日" Editor="text" FieldName="FINISHDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="處理天數" Editor="text" FieldName="PROCESSDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="64">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Enabled="True" Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="Insert" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="Update" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="Delete" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="Apply" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="Cancel" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="Export" Visible="False" />
                </TooItems>
            </JQTools:JQDataGrid>
            <JQTools:JQDataGrid ID="RTLessorAVSCustAR" runat="server" AllowAdd="False" AllowDelete="False" AllowUpdate="False" AlwaysClose="True" AutoApply="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DataMember="V_RTLessorAVSCustAR" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" Pagination="True" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTitle="Query" QueryTop="" RecordLock="False" RecordLockMode="None" RemoteName="sRT104.V_RTLessorAVSCustAR" RowNumbers="True" Title="客戶應收付帳款" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="True">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶編號G+YYMMDD001(YY西元後二位)" Editor="text" FieldName="CUSID" Frozen="False" IsNvarChar="False" MaxLength="15" QueryCondition="" ReadOnly="False" Sortable="False" Visible="False" Width="30">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Frozen="False" IsNvarChar="False" MaxLength="12" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="使用期間(月)" Editor="text" FieldName="PERIOD" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="付款金額" Editor="text" FieldName="AMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="已沖消金額" Editor="text" FieldName="REALAMT" Frozen="False" IsNvarChar="False" MaxLength="10" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="未沖金額" Editor="text" FieldName="DIFFAMT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="90">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="設定費" Editor="text" FieldName="SETAMT" Format="#,##0" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="移機費" Editor="text" FieldName="MOVEAMT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="設備費" Editor="text" FieldName="EQUIPAMT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="保證金" Editor="text" FieldName="GTAMT" Format="" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立項一" Editor="text" FieldName="COD1" Frozen="False" IsNvarChar="False" MaxLength="30" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="160">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立項二" Editor="text" FieldName="COD2" Frozen="False" IsNvarChar="False" MaxLength="30" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="160">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="沖帳日" Editor="text" FieldName="MDAT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="70" Format="yyyy/mm/dd">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Enabled="True" Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="Insert" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="Update" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="Delete" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="Apply" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-cancel" ItemType="easyui-linkbutton" OnClick="cancel" Text="Cancel" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="Query" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="Export" Visible="False" />
                </TooItems>
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
            <div hidden="hidden">
            <JQTools:JQDataGrid ID="dgEXCEL" data-options="pagination:true,view:commandview" RemoteName="sRT104.cmdRT104Excel" runat="server" AutoApply="True"
                DataMember="cmdRT104Excel" Pagination="True" QueryTitle="查詢" EditDialogID="JQDialog1"
                Title="用戶維護" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="True" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Panel" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" ViewCommandVisible="False" OnLoadSuccess="dgOnloadSuccess" OnSelect="MySelect">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="社區" Editor="text" FieldName="COMN" Format="" Visible="true" Width="120" EditorOptions=""/>
                    <JQTools:JQGridColumn Alignment="right" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Visible="False" Width="50" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶代號" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Visible="False" Width="80" EditorOptions="" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶名" Editor="text" FieldName="CUSNC" Format="" MaxLength="30" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="縣市" Editor="text" FieldName="CUTNC" MaxLength="0" Visible="true" Width="80" EditorOptions="" />
                    <JQTools:JQGridColumn Alignment="left" Caption="鄉鎮市" Editor="text" FieldName="TOWNSHIP2" MaxLength="0" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="裝機地址" Editor="text" FieldName="RADDR2" MaxLength="60" Visible="true" Width="160" Format="" />
                    <JQTools:JQGridColumn Alignment="left" Caption="連絡手機" Editor="text" FieldName="MOBILE" Visible="True" Width="80" Format="" MaxLength="30" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案別" Editor="text" FieldName="COMTYPENM" MaxLength="0" Visible="true" Width="80" EditorOptions="" />
                    <JQTools:JQGridColumn Alignment="left" Caption="資費" Editor="text" FieldName="CASEKINDNM" Format="" MaxLength="2" Visible="true" Width="120" EditorOptions="" />
                    <JQTools:JQGridColumn Alignment="left" Caption="IP" Editor="text" FieldName="IP11" Format="" MaxLength="3" Visible="true" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報竣日" Editor="datebox" FieldName="DOCKETDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="STRBILLINGDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最近續約計費日" Editor="datebox" FieldName="NEWBILLINGDAT" Format="yyyy/mm/dd" Visible="true" Width="80" MaxLength="0" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日" Editor="datebox" FieldName="DUEDAT" Format="yyyy/mm/dd" Visible="true" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="公關戶" Editor="text" FieldName="FREECODE" Format="" MaxLength="1" Visible="true" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                    <JQTools:JQGridColumn Alignment="left" Caption="客訴次數" Editor="text" FieldName="QT_CC" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="False"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="btnCreateClick" Text="匯出Excel" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="維修收款" Visible="True" OnClick="LinkRT1041" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="新戶入帳" Visible="True" OnClick="btn1Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="續約作業" Visible="True" OnClick="btn2Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="復機作業" Visible="True" OnClick="btn3Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="退租作業" Visible="True" OnClick="btn4Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="應收應付" Visible="True" OnClick="btn5Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="客服案件" Visible="True" OnClick="btn6Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="設備保管收據列印" Visible="False" OnClick="btn7Click" Icon="icon-print" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="用戶移動" Visible="False" OnClick="btn8Click" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="調整到期" Visible="True" OnClick="btn9Click" Icon="icon-edit "/>
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="設備查詢" Visible="False" OnClick="btn10Click" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢" Visible="True" OnClick="btn11Click" Icon="icon-edit" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="作廢反轉" Visible="True" OnClick="btn12Click" Icon="icon-undo" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="歷史異動" Visible="False" OnClick="btn13Click" Icon="icon-view" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" OnClick="btnReloadClick" Text="資料更新" Visible="True" Icon="icon-reload" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區名稱" Condition="%%" DataType="string" Editor="text" FieldName="B.COMN" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="用戶(公司)名稱" Condition="%%" DataType="string" Editor="text" FieldName="A.CUSNC" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="連絡手機" Condition="%%" DataType="string" Editor="text" FieldName="A.MOBILE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="裝機地址" Condition="%%" DataType="string" Editor="text" FieldName="C.ADDR" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="身份證號" Condition="%%" DataType="string" Editor="text" FieldName="A.SOCIALID" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="室內電話" Condition="%%" DataType="string" Editor="text" FieldName="A.CONTACTTEL" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="方案別" Condition="%" DataType="string" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P5'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代號',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="A.COMTYPE" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="140" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="資費" Condition="=" DataType="string" Editor="inforefval" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'O9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'代碼',textFieldCaption:'名稱',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" FieldName="A.CASEKIND" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
</div>
        </p>
    </form>
</body>

<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>
</html>
