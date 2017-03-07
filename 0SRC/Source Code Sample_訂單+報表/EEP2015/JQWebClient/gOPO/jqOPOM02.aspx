<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jqOPOM02.aspx.cs" Inherits="gOPO_bOPOM02" %>

<%@ Register Assembly="JQClientTools" Namespace="JQClientTools" TagPrefix="JQClientTools" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客戶訂單維護</title>
    <script>
        function keyevent() {
            if (event.keyCode == 40) {
                var dialogIsOpen = !$("#JQDialog1").parent().is(":hidden");
                if (dialogIsOpen) {
                    insertItem($("#dataGridDetail"));
                }
            }
        }
        document.onkeydown = keyevent;

        var fullWindowDialogs = "";
        var centerWindowDialogs = "#JQDialog1;#JQDialogFileUpload;#JQDialogTempSelect;#JQDialogSign;#JQDialogMultiSelect1;#JQDialogFivinvfinal;#JQDialogBOX";

        function openForm(fid, rowData, mode, editmode, keys) {
            uOpenForm(fid, rowData, mode, editmode, keys);
        }

        $(document).ready(function () {
            var tColumns = [["dataGridPRODCATEID,120,dataGrid", "dataGridMultiSelect1,420,dataGrid"], ["dataGridMultiSelect2,600,dataGrid,2"]];
            uInitTable("lbMultiSelect1", 840, tColumns);
            var tColumnT1 = [["dataGridBILLNO1,150,dataGrid", "dataGridTransferForm,420,dataGrid"], ["dataGridTransferForm2,600,dataGrid,2"]];
            uInitTable("lbTransferForm1", 840, tColumnT1);

        });

        //產品多選-開窗
        function openProdMultiSelect() {
            initInfoDataGrid($("#dataGridPRODCATEID"));
            initInfoDataGrid($("#dataGridMultiSelect1"));
            initInfoDataGrid($("#dataGridMultiSelect2"));
            uOpenMulitDialog("JQDialogMultiSelect1", "dataGridMultiSelect1", "dataGridMultiSelect2");
        }

        function doneGetBack() {
            $('#JQBatchMove1').BatchMove('Move');
            return true;
        }

        function getPrice(dataRow) {
            uBatchGetPrice("dataFormMaster1", "CUSTID", "CURRENCYID", "INVOICETYPE", dataRow.PRODUCTID, "22", "dataGridDetail", "SUMQUALITY", "TOTALAMOUNT", "SUMAMOUNT", "TAXAMOUNT");
        }

        function TransferGetPrice(dataRow) {
            uTransferGetPrice("dataFormMaster1", "dataGridDetail", "SUMQUALITY", "TOTALAMOUNT", "SUMAMOUNT", "TAXAMOUNT");

        }


        function dataGridPRODCATEIDSelect(index, row) {
            var PRODCATEID = row.PRODCATEID;
            var setstr = " BASPRODUCT.PRODCATEID='" + PRODCATEID + "'";
            $("#dataGridMultiSelect1").datagrid('setWhere', setstr);
        }

        //現有庫存查詢
        function openStockGrid() {
            initInfoDataGrid($("#dataGridFivinvfinal"));
            uOpenDialog("JQDialogFivinvfinal", "dataGridDetail", "PRODUCTID", "PRODUCTID");
        }

        //產品售價查詢
        function openPriceGrid() {
            initInfoDataGrid($("#dataGridProdPrice"));
            uOpenDialog("JQDialogProdPrice", "dataGridDetail", "PRODUCTID", "PRODUCTID");
        }


        //審核
        function showSignDiv() {
            uOpenSignDiv();
        }

        function sign() {
            uDoSign("dgView", "22", "OPOORDER2_M");
        }


        function selectPRODUCTID() {
            //取得refal當前選中的資料row 
            var refRow = $(this).data('inforefval').panel.find('table.refval-grid').datagrid('getSelected');
            uGetPriceAfterSelectProd(refRow, "dataFormMaster1", "CUSTID", "CURRENCYID", "INVOICETYPE", "22", "dataGridDetail", "PRODCNAME", "PRODSTRUCTURE", "QUANTITY", "UNIT", "PRICE", "SUBAMOUNT", "TAXPRICE", "TAXAMOUNT", "SUMQUALITY", "TOTALAMOUNT", "SUMAMOUNT", "TAXAMOUNT");
        }

        //$Edit 20160810 by alex：修改refval離開后呼叫事件
        function refvalOnBlur(refRow, refValueField) {
            if (refValueField == "PRODUCTID")
                uGetPriceAfterSelectProd(refRow, "dataFormMaster1", "CUSTID", "CURRENCYID", "INVOICETYPE", "22", "dataGridDetail", "PRODCNAME", "PRODSTRUCTURE", "QUANTITY", "UNIT", "PRICE", "SUBAMOUNT", "TAXPRICE", "TAXAMOUNT", "SUMQUALITY", "TOTALAMOUNT", "SUMAMOUNT", "TAXAMOUNT");
        }

        function openReport() {
            uOpenRDLC("dgView", "OPOORDER2_M.BILLNO", "cbSingleChoice", "TemplateList", "OPOM02", "srOPO2.cmdOPOM02", "cmdOPOM02", "gOPO");
        }


        var loadOnce = 0;
        var closeDateStr;
        var afterInsert = false;
        var insertBILLNO = "";
        function dgViewLoadSuccess() {
            //For other Bill Reference
            var billno = uGetQueryString("BILLNO");

            if (billno != null) {
                if (loadOnce == 0) {
                    var setstr = " OPOORDER2_M.BILLNO='" + billno + "'";
                    $('#dgView').datagrid('setWhere', setstr);
                    $('#dgView').datagrid('reload');
                }
                else if (loadOnce == 1) {
                    var rows = $('#dgView').datagrid('getSelected');
                    openForm('#JQDialog1', rows, "viewed", 'dialog');
                }
                loadOnce++;
            }

            //AfterInsert, SetWhere position
            if (afterInsert) {
                var setstr = " BILLNO = '" + insertBILLNO + "'";
                var dataGrid = $("#dgView");
                dataGrid.datagrid('setWhere', setstr);
                dataGrid.datagrid('reload');
                //var rows = dataGrid.datagrid('getRows');
                //openForm("#JQDialog1", rows, "viewed", "dialog");
                afterInsert = false;
            }

            uGetRDLC("OPOM02", "TemplateList");

            //Get taxRate and numberic decimal from parameter
            /** Read From Storage about Decimal Setup **/
            uGetDecimalFromStorage();
            uGetDecimalParam();

            //Write to MainPage.js, and Recode in Storage
            closeDateStr = window.sessionStorage.getItem("paraCloseDate");
            departmentID = window.sessionStorage.getItem("paraDepartID");
            currRate = window.sessionStorage.getItem("paraCurrRate");
        }

        //get Close date
        function uAfterExecSQL(data) {
            var rows = $.parseJSON(data);
            if (rows.length > 0) {
                if (typeof (rows[0].PARAVALUE) != "undefined")
                    closeDateStr = rows[0].PARAVALUE;
                else if (typeof (rows[0].BILLNO) != "undefined") {
                    insertBILLNO = rows[0].BILLNO;

                }
            }
        }

        //InUsing BeforeEdit
        function dgViewOnUpdate(row) {
            var BILLNO = row.BILLNO;
            var parameters = "@Tag@|Edit|KEYID|" + BILLNO;
            return uSysEditCheck(parameters);
        }

        //InUsing Before Delete
        function dgViewOnDelete(row) {
            var BILLNO = row.BILLNO;
            var parameters = "@Tag@|Delete|KEYID|" + BILLNO;
            return uSysEditCheck(parameters);
        }

        function getSEQNO() {
            var rows = $('#dataGridDetail').datagrid('getRows');
            var SEQNO = 0;
            for (var i = 0; i < rows.length; i++) {
                if (SEQNO < rows[i].SEQNO)
                    SEQNO = rows[i].SEQNO;
            }
            SEQNO++;
            return SEQNO;
        }

        function getUNIQUENO() {
            return -1 * getSEQNO();
        }

        //below code maybe need modify by hand
        function getWAREID() {
            return uGetQueryValue("dataFormMaster1WAREID");
        }

        function getMasterWAREID() {
            return uGetWareID();
        }

        var departmentID = "";
        function getDEPARTMENTID() {
            return departmentID;
        }

        function getCURRENCYID() {
            return uGetCURRENCYID();
        }

        var currRate = "";
        function getCURRERATE() {
            return currRate;
        }

        //Bill Copy
        var jqDetailRows = [];
        var jqCopyStatus = "";
        function copyData(dgViewID) {
            uOpenCopyDialog("JQDialog1", "dgView", "dataGridDetail", "BILLNO", "BILLNO", "BILLNO", "UNIQUENO");
        }

        var isopen = 'N';
        function dataFormMasterLoadSuccess() {
            $('#dataFormMaster1INVOICETYPE').combobox("setWhere", "  KEYTAG ='INVOICETYPE'");
            $('#dataFormMaster1TAXCLASS').combobox("setWhere", "  KEYTAG ='TAXCLASS'");
            $('#dataFormMaster1PAYMETHOD').combobox("setWhere", "  KEYTAG ='PAYMETHOD'");
            var mode = getEditMode($("#dataFormMaster1"));
            var INVOICETYPE = uGetQueryValue("dataFormMaster1INVOICETYPE");
            changeColumn(INVOICETYPE);

            if (mode == "inserted") {
                $("#dataFormMaster2PREPAYAMT").attr('disabled', false);
                $("#toolItemdataGridDetail複製").hide();
                $("#toolItemdataGridDetail導入Excel").show();
                $("#toolItemdataGridDetail產品多選").show();
                $("#dataFormMaster3CREATE_USER").attr('value', getClientInfo('_usercode'));
                $("#dataFormMaster3CREATE_DATE").datebox('setValue', getClientInfo('_ToDay'));
                $("#dataFormMaster3UPDATE_USER").attr('value', "");
                $("#dataFormMaster3UPDATE_DATE").datebox('setValue', "");
                $("#dataFormMaster3SIGN_USER").attr('value', "");
                $("#dataFormMaster3SIGN_DATE").datebox('setValue', "");
            }
            else if (mode == "updated") {
                $("#toolItemdataGridDetail導入Excel").show();
                $("#toolItemdataGridDetail複製").show();
                $("#toolItemdataGridDetail產品多選").show();
                $("#dataFormMaster2PREPAYAMT").attr('disabled', 'disabled');
                $("#dataFormMaster3UPDATE_USER").attr('value', getClientInfo('_usercode'));
                $("#dataFormMaster3UPDATE_DATE").datebox('setValue', getClientInfo('_ToDay'));
            }
            else {
                $("#toolItemdataGridDetail導入Excel").hide();
                $("#toolItemdataGridDetail複製").show();
                $("#toolItemdataGridDetail產品多選").hide();
            }

            isopen = uFileUploadOpenDialog("dataFormMaster2", "DATAPATH", "JQDialogFileUpload", "dgFileUpload", "someFiles", isopen, "OPOORDER2_M", "BILLNO", "dgView", "dfFileUpload", "FILEUPLOAD", "dataFormMaster");
            uFileUploadInitForGrid("dfFileUpload", "FILEUPLOAD");

        }
        function FileUploadOnSuccessForGrid(fileName) {
            uFileUploadOnSuccessForGrid(fileName, "dgFileUpload", "dfFileUpload", "FILEUPLOAD");
        }
        function FileUploadOnSuccess(fileName) {
            uFileUpload(fileName, "dataFormMaster3", "DATAPATH", "samplephoto");
        }

        function dataGridDetailLoadSuccess() {
            var mode = getEditMode($("#dataFormMaster1"));
            if (mode == "inserted" || mode == "updated")
                $("#JQDialog1").find(".icon-copy").hide();
            else if (mode == "viewed")
                $("#JQDialog1").find(".icon-copy").show();
            uCopyDetail("dataGridDetail");

            uSumDetailAfterLoad("dataGridDetail", "dataFormMaster1", "SUMQUALITY", "SUMAMOUNT");
        }

        function dataFormMasterApply() {
            submitFlag = true;
            uCopyApply("dataGridDetail");
            endEdit($('#dataGridDetail'));
            var billdateStr = uGetQueryValue("dataFormMasterBILLDATE");
            var billdate = uStrToDate(billdateStr);
            var closeDate = uStrToDate(closeDateStr);
            if (billdate < closeDate) {
                alert("該日期已關帳，請重新輸入日期！關帳日期是：" + uDateToStr(closeDate));
                return false;
            }
            if (!uCheckDetail("dataGridDetail")) return false;
        }

        var jqUploadNames = "";
        function dataFormMasterApplied() {
            submitFlag = false;
            var mode = getEditMode($("#dataFormMaster1"));
            if (mode == "inserted") {
                afterInsert = true;
                var billdate = uGetQueryValue("dataFormMasterBILLDATE");
                var sql = "SELECT TOP 1 BILLNO FROM OPOORDER2_M WHERE BILLDATE = '" + billdate + "' ORDER BY BILLNO DESC";
                uExecSQL(sql, "Y");
            }
            else {
                $('#dgView').datagrid('reload');
            }
        }

        //Query Before Leave
        function closeForm(dialogID) {
            uCloseMasterForm(dialogID, "dataFormMaster");
        }

        function quantityCalculate() {
            uCalculateDetail("dataGridDetail", "dataFormMaster1", "SUMQUALITY;TOTALAMOUNT;SUMAMOUNT;TAXAMOUNT", "QUANTITY", "QUANTITY*PRICE=SUBAMOUNT", "QUANTITY*TAXPRICE=TAXAMOUNT");
        }

        function priceCalculate() {
            uCalculateDetail("dataGridDetail", "dataFormMaster1", "SUMQUALITY;TOTALAMOUNT;SUMAMOUNT;TAXAMOUNT", "PRICE", "QUANTITY*PRICE=SUBAMOUNT", "QUANTITY*TAXPRICE=TAXAMOUNT");
        }

        function taxPriceCalculate() {
            uCalculateDetail("dataGridDetail", "dataFormMaster1", "SUMQUALITY;TOTALAMOUNT;SUMAMOUNT;TAXAMOUNT", "TAXPRICE", "QUANTITY*PRICE=SUBAMOUNT", "QUANTITY*TAXPRICE=TAXAMOUNT");
        }

        function subAmountCalculate() {
            uCalculateDetail("dataGridDetail", "dataFormMaster1", "SUMQUALITY;TOTALAMOUNT;SUMAMOUNT;TAXAMOUNT", "SUBAMOUNT", "QUANTITY*PRICE=SUBAMOUNT", "QUANTITY*TAXPRICE=TAXAMOUNT");
        }

        function taxAmountCalculate() {
            uCalculateDetail("dataGridDetail", "dataFormMaster1", "SUMQUALITY;TOTALAMOUNT;SUMAMOUNT;TAXAMOUNT", "TAXAMOUNT", "QUANTITY*PRICE=SUBAMOUNT", "QUANTITY*TAXPRICE=TAXAMOUNT");
        }

        //Sun after Delete Detail
        function dataGridDetailDelete(rowData) {
            uDeleteDetail("dataFormMaster1", "SUMQUALITY", "TOTALAMOUNT", "SUMAMOUNT", "TAXAMOUNT", rowData.QUANTITY, rowData.SUBAMOUNT, rowData.TAXAMOUNT);
        }

        //交易歷史查詢
        function openCustQuery() {
            uOpenNewTab("dataGridDetail", "PRODUCTID", "交易歷史查詢", "gOPO", "jqOPOM01_1.aspx", "dataFormMaster1", "CUSTID");
        }

        function INVOICETYPEOnSelect(row) {
            var INVOICETYPE = row.KEYID;
            changeColumn(INVOICETYPE);
        }

        //根據【發票聯式】切換Detail欄位。二聯式：【含稅單價/含稅金額】  三聯式：【單價/金額】
        function changeColumn(INVOICETYPE) {
            //1,二聯式 2,三聯式 3,計算機二聯式 4,計算機三聯式 5,收銀機二聯式 6,收銀機三聯式
            if (INVOICETYPE == 1 || INVOICETYPE == 3 || INVOICETYPE == 5) {
                $("#dataGridDetail").datagrid("hideColumn", "PRICE");//單價
                $("#dataGridDetail").datagrid("hideColumn", "SUBAMOUNT");//金額
                $("#dataGridDetail").datagrid("showColumn", "TAXPRICE");//含稅單價
                $("#dataGridDetail").datagrid("showColumn", "TAXAMOUNT");  //含稅金額
            }
            else if (INVOICETYPE == 2 || INVOICETYPE == 4 || INVOICETYPE == 6) {
                $("#dataGridDetail").datagrid("showColumn", "PRICE");//單價
                $("#dataGridDetail").datagrid("showColumn", "SUBAMOUNT");//金額
                $("#dataGridDetail").datagrid("hideColumn", "TAXPRICE");//含稅單價
                $("#dataGridDetail").datagrid("hideColumn", "TAXAMOUNT");//含稅金額
            }
        }
        //主檔修改倉庫時，同時帶入到明細檔
        function WAREIDOnSelect(row) {
            var WAREID = row.WAREID;
            var grid = $("#dataGridDetail");
            var rows = grid.datagrid("getRows");

            for (var i = 0; i < rows.length; i++) {
                grid.datagrid('beginEdit', i);
                uSetDetailValue(grid, i, "WAREID", WAREID);
                grid.datagrid('endEdit', i);
            }
        }

        //報價單轉訂單
        function openTransferForm() {
            var IDNO = uGetQueryValue("dataFormMaster1CUSTID");
            var CURRID = uGetQueryValue("dataFormMaster1CURRENCYID");
            var setstr = " CUSTID='" + IDNO + "'" + " AND CURRENCYID='" + CURRID + "'";
            $("#dataGridBILLNO1").datagrid('setWhere', setstr);
            $("#dataGridBILLNO1").datagrid('reload');
            $("#dataGridTransferForm").datagrid('reload');
            $("#dataGridTransferForm2").datagrid('reload');
            uOpenTransferForm("JQDialogTransferForm", "dataFormMaster1", "CUSTID;CURRENCYID", "CUSTID;CURRENCYID", "BILLDATE", "請先輸入客戶及幣別代號！");
        }

        function doneTransfer() {
            $('#JQBatchMove3').BatchMove('Move');
            return true;
        }

        function dataGridBILLNO1Select(index, row) {
            var BILLNO = row.BILLNO;
            var setstr = " BILLNO='" + BILLNO + "'";
            $("#dataGridTransferForm").datagrid('setWhere', setstr);
        }

        function openQueryMemo() {
            var JQDialog = $("#JQDialogQueryMemo");
            JQDialog.find(".infosysbutton-s").hide();
            JQDialog.find(".infosysbutton-c").hide();

            JQDialog.dialog({
                title: "電話查詢",
                buttons: [{
                    text: "確認", handler: function () {
                        var memo = $("#tbMemo").val();

                        var setstr = " BILLNO IN (SELECT DISTINCT  BILLNO FROM OPOORDER2_D WHERE MEMOCHAR2 LIKE '%" + memo + "%') ";
                        $('#dgView').datagrid('setWhere', setstr);
                        $('#dgView').datagrid('reload');
                        JQDialog.dialog('close');
                    }
                },
                    { text: "取消", handler: function () { JQDialog.dialog('close'); } }]
            });

            uOpenDialog("JQDialogQueryMemo");
        }

        function OPOtoINV() {
            if (confirm("是否要批次轉出貨單？")) {
                var DATE1 = $($('#querydgView').find('input')[1]).val();
                var DATE2 = $($('#querydgView').find('input')[4]).val();
                var loginID = getClientInfo('_usercode');
                if (DATE1 == "" || DATE2 == "")
                { alert("未填寫日期區間！"); }
                else {
                    var SPParam = DATE1 + "|" + DATE2 + "|" + loginID;
                    $.ajax({
                        type: 'POST',
                        url: '../handler/jqDataHandle.ashx?RemoteName=smOPOM02.icTemp',
                        data: 'mode=method&method=OPOtoINVtoDB&parameters=' + SPParam,
                        cache: false,
                        async: false,
                        success: function (data) {
                            if (data != "Y") { alert(data); } else { alert("批次轉出貨單完成") }
                        }
                    });
                }
            }
        }

        function GetTomorrow() {
            var date = new Date();
            var year = date.getFullYear().toString();
            var month = (date.getMonth() + 1).toString();
            if (month.length == 1) {
                month = "0" + month;
            }
            var day = (date.getDate() + 1).toString();
            if (day.length == 1) {
                day = "0" + day;
            }
            return year + '-' + month + '-' + day;
        }

        function GetONHANDDATE() {
            return $("#dataFormMasterONHANDDATE").datebox('getValue');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <JQClientTools:JQScriptManager runat="server" ID="ScriptManager" LocalScript="true" />
        <JQTools:JQDialog runat="server" ID="JQDialogBOX" Title="多單位輸入" EditMode="Dialog" DialogLeft="200px" DialogTop="100px" Width="400px">
            <JQTools:JQLabel ID="Label2" runat="server" Text=" " />
            <JQTools:JQLabel ID="CaptionPACKINGNUMBER1" runat="server" Text="包裝數量：" />
            <JQTools:JQLabel ID="lbPACKINGNUMBER1" runat="server" />
            <br />
            <JQTools:JQLabel ID="CaptionSTANDARDCOST" runat="server" Text="計量單位：" />
            <JQTools:JQLabel ID="lbSTANDARDCOST" runat="server" />
            <br />
            <JQTools:JQLabel ID="CaptiontbBOXQTY" runat="server" Text="箱數：" />
            <JQClientTools:JQTextBox ID="tbBOXQTY" runat="server" />
            <br />
            <JQTools:JQLabel ID="CaptionBOXPRICE" runat="server" Text="每箱單價：" />
            <JQClientTools:JQTextBox ID="tbBOXPRICE" runat="server" />
        </JQTools:JQDialog>
        <JQTools:JQDialog runat="server" ID="JQDialogQueryMemo" Title="電話查詢" EditMode="Dialog" DialogLeft="50%" DialogTop="100px" Width="300px">
            <JQClientTools:JQTextBox ID="tbMemo" runat="server" Width="230px" Height="70px" />
        </JQTools:JQDialog>
        <JQClientTools:JQDialog ID="JQDialogFileUpload" runat="server" Title="上傳文檔" Width="600px" DialogLeft="400px" DialogTop="200px">
            <JQClientTools:JQDataForm runat="server" ID="dfFileUpload" RemoteName="gServerDataModuleComm.cmdFileUpload" DataMember="cmdFileUpload" HorizontalColumnsCount="2" DuplicateCheck="False" IsShowFlowIcon="False" IsAutoSubmit="False" IsAutoPause="False" IsAutoPageClose="False" IsRejectON="False" IsNotifyOFF="False" ContinueAdd="False" ShowApplyButton="False" IsRejectNotify="False" disapply="False" HorizontalGap="20" VerticalGap="2" DivFramed="False" ValidateStyle="Hint" Closed="False" AlwaysReadOnly="False">
                <Columns>
                    <JQClientTools:JQFormColumn Alignment="left" Caption="附　　檔" Editor="infofileupload" EditorOptions="filter:'',isAutoNum:false,upLoadFolder:'Files/OPOM02',showButton:false,showLocalFile:true,onSuccess:FileUploadOnSuccessForGrid,fileSizeLimited:'500'" FieldName="FILEUPLOAD" MaxLength="0" NewRow="true" ReadOnly="false" RowSpan="1" Span="5" Visible="true" Width="450" />
                </Columns>
            </JQClientTools:JQDataForm>
            <JQClientTools:JQDataGrid runat="server" ID="dgFileUpload" RemoteName="gServerDataModuleComm.cmdFileUpload" DataMember="cmdFileUpload" AutoApply="True" AlwaysClose="False" EditOnEnter="True" MultiSelect="False" AllowAdd="True" AllowUpdate="false" AllowDelete="true" ViewCommandVisible="True" UpdateCommandVisible="False" DeleteCommandVisible="false" BufferView="False" Title="上傳文檔" QueryTitle="Query" Pagination="True" RecordLock="False" RecordLockMode="None" CheckOnSelect="True" PageSize="10" PageList="10,20,30,40,50" QueryMode="Window" QueryAutoColumn="False" TotalCaption="Total:" ColumnsHibeable="False" NotInitGrid="False" DuplicateCheck="False" EditDialogID="JQDialogFormExtendQuery" EditMode="Dialog" InsertCommandVisible="True" QueryLeft="" QueryTop="">
                <Columns>
                    <JQClientTools:JQGridColumn FieldName="FILEUPLOAD" Caption="檔案名稱" IsNvarChar="false" Alignment="left" Width="400" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="True" />
                </Columns>
                <RelationColumns>
                </RelationColumns>
                <TooItems>
                    <JQClientTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" Text="刪除" OnClick="uFileUploadDelete" Visible="true" Enabled="true" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQClientTools:JQDataGrid>
        </JQClientTools:JQDialog>
        <JQClientTools:JQDialog runat="server" ID="JQDialogTempSelect" Title="選擇列印格式" Closed="true" EditMode="Dialog" ShowModal="true" DialogLeft="50%" DialogTop="50%" Width="220px">
            <JQClientTools:JQCheckBox ID="cbSingleChoice" runat="server" />
            <JQClientTools:JQLabel runat="server" ID="Label1" Text="僅列印本單??" />
            <br></br>
            <JQClientTools:JQComboBox ID="TemplateList" runat="server">
                <Items>
                    <JQClientTools:JQComboItem Selected="False" Text="OPOM02.rdlc" Value="OPOM02_01.rdlc" />
                </Items>
            </JQClientTools:JQComboBox>
        </JQClientTools:JQDialog>
        <JQClientTools:JQDialog runat="server" ID="JQDialogSign" Title="審核" Closed="true" EditMode="Dialog" ShowModal="true" DialogLeft="50%" DialogTop="50%" Width="400px">
            <JQClientTools:JQLabel runat="server" ID="lbSignStatus" Text="審核狀態" />
            <JQClientTools:JQOptions ID="optionSignStatus" runat="server" OpenDialog="False" Width="100px">
                <Items>
                    <JQClientTools:JQComboItem Selected="False" Text="通過" Value="Z" />
                    <JQClientTools:JQComboItem Selected="False" Text="不通過" Value="Q" />
                </Items>
            </JQClientTools:JQOptions>
            <br />
            <JQClientTools:JQLabel ID="lbSignMemo" runat="server" Text="審核意見" />
            <JQClientTools:JQTextArea ID="taSignMemo" runat="server" Height="80px" Width="270px" />
        </JQClientTools:JQDialog>
        <JQClientTools:JQDialog runat="server" ID="JQDialogMultiSelect1" Title="產品多選" Closed="true" EditMode="Dialog" ShowModal="true" DialogLeft="120px" DialogTop="4px" Width="1000px" Height="550px" OnSubmited="doneGetBack">
            <JQTools:JQLabel ID="lbMultiSelect1" runat="server" Text=" " />
            <JQClientTools:JQDataGrid runat="server" ID="dataGridPRODCATEID" RemoteName="gServerDataModuleComm.cmdBASPRODCATEGORY" DataMember="cmdBASPRODCATEGORY" AutoApply="True" AlwaysClose="False" EditOnEnter="True" MultiSelect="false" AllowAdd="False" AllowUpdate="False" AllowDelete="False" ViewCommandVisible="False" UpdateCommandVisible="False" DeleteCommandVisible="False" BufferView="False" Title="產品類別列表" QueryTitle="查詢" Pagination="false" RecordLock="False" RecordLockMode="None" CheckOnSelect="True" PageList="10,20,30,40,50" PageSize="10" QueryMode="Window" QueryAutoColumn="False" TotalCaption="Total:" ColumnsHibeable="False" NotInitGrid="False" DuplicateCheck="False" EditMode="Dialog" InsertCommandVisible="False" Height="300px" Width="180px" OnSelect="dataGridPRODCATEIDSelect">
                <Columns>
                    <JQClientTools:JQGridColumn FieldName="PRODCATEID" Caption="類別編號" IsNvarChar="false" Alignment="left" Width="120" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="false" />
                    <JQClientTools:JQGridColumn FieldName="PRODCATECNAME" Caption="產品類別" IsNvarChar="false" Alignment="left" Width="135" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                </Columns>
                <TooItems>
                </TooItems>
                <QueryColumns>
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="類別編號" Condition="%%" DataType="string" Editor="text" FieldName="PRODCATEID" IsNvarChar="False" NewLine="True" RemoteMethod="False" Width="125" />
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="產品類別" Condition="%%" DataType="string" Editor="text" FieldName="PRODCATECNAME" IsNvarChar="False" NewLine="True" RemoteMethod="False" Width="125" />
                </QueryColumns>
            </JQClientTools:JQDataGrid>
            <JQClientTools:JQDataGrid runat="server" ID="dataGridMultiSelect1" RemoteName="ssBASM501.BASPRODUCT" DataMember="BASPRODUCT" AutoApply="True" AlwaysClose="True" EditOnEnter="True" MultiSelect="True" AllowAdd="False" AllowUpdate="False" AllowDelete="False" ViewCommandVisible="False" UpdateCommandVisible="False" DeleteCommandVisible="False" BufferView="False" Title="產品列表" QueryTitle="查詢" Pagination="True" RecordLock="False" RecordLockMode="None" CheckOnSelect="True" PageList="10,20,30,40,50" PageSize="10" QueryMode="Window" QueryAutoColumn="False" TotalCaption="Total:" ColumnsHibeable="False" NotInitGrid="True" DuplicateCheck="False" Width="750px" MultiSelectGridID="dataGridMultiSelect2" EditMode="Dialog" InsertCommandVisible="False" QueryLeft="" QueryTop="" Height="300px" WidthRowNumbers="True">
                <Columns>
                    <JQClientTools:JQGridColumn FieldName="PRODUCTID" Caption="產品編號" IsNvarChar="false" Alignment="left" Width="80" Editor="textarea" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn Alignment="left" Caption="條碼編號" Editor="text" FieldName="SCANCODE" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQClientTools:JQGridColumn>
                    <JQClientTools:JQGridColumn FieldName="PRODCNAME" Caption="產品品名" IsNvarChar="false" Alignment="left" Width="120" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRODSTRUCTURE" Caption="產品規格" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn Alignment="left" Caption="產品品牌" Editor="text" FieldName="INVOICEPRODNAME" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQClientTools:JQGridColumn>
                    <JQClientTools:JQGridColumn Alignment="left" Caption="產品類別" Editor="text" FieldName="PRODCATECNAME" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQClientTools:JQGridColumn>
                    <JQClientTools:JQGridColumn FieldName="SALEPRICE" Caption="建議售價" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn Alignment="left" Caption="計量單位" Editor="text" FieldName="CALCUNIT" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQClientTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQClientTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" Text="查詢" OnClick="openQuery" Visible="true" Enabled="true" />
                </TooItems>
                <QueryColumns>
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="產品類別" Condition="=" DataType="string" Editor="infocombobox" EditorOptions="valueField:'PRODCATECNAME',textField:'PRODCATECNAME',remoteName:'gServerDataModuleComm.cmdBASPRODCATEGORY',tableName:'cmdBASPRODCATEGORY',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="PRODCATECNAME" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="產品編號" Condition="%" DataType="string" Editor="text" FieldName="PRODUCTID" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="產品品名" Condition="%" DataType="string" Editor="text" FieldName="PRODCNAME" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="規格" Condition="%" DataType="string" Editor="text" FieldName="PRODSTRUCTURE" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQClientTools:JQDataGrid>
            <JQClientTools:JQDataGrid runat="server" ID="dataGridMultiSelect2" RemoteName="ssBASM501.BASPRODUCT" DataMember="BASPRODUCT" AutoApply="False" AlwaysClose="True" EditOnEnter="True" MultiSelect="False" AllowAdd="False" AllowUpdate="True" AllowDelete="True" ViewCommandVisible="False" UpdateCommandVisible="False" DeleteCommandVisible="True" BufferView="False" Title="" QueryTitle="Query" Pagination="False" RecordLock="False" RecordLockMode="None" CheckOnSelect="True" PageList="5,10,20,30,40,50" PageSize="5" QueryMode="Window" QueryAutoColumn="False" Height="120px" Width="933px" NotInitGrid="True" ColumnsHibeable="False" DuplicateCheck="False" EditMode="Dialog" InsertCommandVisible="False" QueryLeft="" QueryTop="" RowNumbers="True" TotalCaption="Total:">
                <Columns>
                    <JQClientTools:JQGridColumn FieldName="PRODUCTID" Caption="產品編號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRODCNAME" Caption="產品品名" IsNvarChar="false" Alignment="left" Width="120" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRODSTRUCTURE" Caption="產品規格" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="CALCUNIT" Caption="計量單位" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="STANDARDCOST" Caption="標準成本" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                </Columns>
                <TooItems>
                </TooItems>
                <QueryColumns>
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="產品編號" Condition="%%" DataType="string" Editor="text" FieldName="PRODUCTID" IsNvarChar="False" NewLine="True" RemoteMethod="False" Width="125" />
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="產品品名" Condition="%%" DataType="string" Editor="text" FieldName="PRODCNAME" IsNvarChar="False" NewLine="True" RemoteMethod="False" Width="125" />
                </QueryColumns>
            </JQClientTools:JQDataGrid>
        </JQClientTools:JQDialog>
        <JQClientTools:JQDialog runat="server" ID="JQDialogFivinvfinal" Title="現有庫存查詢" Closed="true" EditMode="Dialog" ShowModal="true" DialogLeft="100px" DialogTop="100px" Width="600px">
            <JQClientTools:JQDataGrid runat="server" ID="dataGridFivinvfinal" RemoteName="ssBASM501.cmdFivinvfinal" DataMember="cmdFivinvfinal" AutoApply="true" AlwaysClose="false" EditOnEnter="true" MultiSelect="false" AllowAdd="false" AllowUpdate="false" AllowDelete="false" ViewCommandVisible="true" UpdateCommandVisible="false" DeleteCommandVisible="false" BufferView="false" Title="現有庫存查詢" QueryTitle="Query" Pagination="true" RecordLock="false" RecordLockMode="None" CheckOnSelect="true" PageSize="10" PageList="10,20,30,40,50" QueryMode="Window" QueryAutoColumn="false" TotalCaption="Total:" ColumnsHibeable="false" NotInitGrid="false" DuplicateCheck="false">
                <Columns>
                    <JQClientTools:JQGridColumn FieldName="PRODUCTID" Caption="產品編號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRODSTRUCTURE" Caption="規格" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="WAREID" Caption="倉庫編號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="LOWPOINTSTOCK" Caption="安全存量" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="NOWQUANTITY" Caption="現有數量" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="NOWCOST" Caption="分倉成本" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="BORROWOUTQTY" Caption="借出數量" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="BORROWINQTY" Caption="借入數量" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="RETURNINQTY" Caption="還入數量" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="RETURNOUTQTY" Caption="還出數量" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                </Columns>
                <RelationColumns>
                </RelationColumns>
                <QueryColumns>
                </QueryColumns>
            </JQClientTools:JQDataGrid>
        </JQClientTools:JQDialog>
        <JQClientTools:JQDialog runat="server" ID="JQDialogProdPrice" Title="產品售價查詢" Closed="true" EditMode="Dialog" ShowModal="true" DialogLeft="100px" DialogTop="100px" Width="600px">
            <JQClientTools:JQDataGrid runat="server" ID="dataGridProdPrice" RemoteName="gServerDataModuleComm.cmdBASPRODUCT" DataMember="cmdBASPRODUCT" AutoApply="true" AlwaysClose="false" EditOnEnter="true" MultiSelect="false" AllowAdd="false" AllowUpdate="false" AllowDelete="false" ViewCommandVisible="true" UpdateCommandVisible="false" DeleteCommandVisible="false" BufferView="false" Title="產品售價查詢" QueryTitle="Query" Pagination="true" RecordLock="false" RecordLockMode="None" CheckOnSelect="true" PageSize="10" PageList="10,20,30,40,50" QueryMode="Window" QueryAutoColumn="false" TotalCaption="Total:" ColumnsHibeable="false" NotInitGrid="false" DuplicateCheck="false">
                <Columns>
                    <JQClientTools:JQGridColumn FieldName="PRODUCTID" Caption="產品編號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRODCNAME" Caption="產品品名" IsNvarChar="false" Alignment="left" Width="200" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="CURRENCYID" Caption="幣別編號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="SALEPRICE" Caption="建議售價" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRICEA" Caption="售 價 A" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRICEB" Caption="售 價 B" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRICEC" Caption="售 價 C" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRICED" Caption="售 價 D" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRICEE" Caption="售 價 E" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="ISSERIALNO" Caption="序號管制" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PACKINGNUMBER2" Caption="每箱單價" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PACKINGUNIT2" Caption="包裝單位2" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="STANDARDCOST2" Caption="幣別標準成本" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                </Columns>
                <RelationColumns>
                </RelationColumns>
                <QueryColumns>
                </QueryColumns>
            </JQClientTools:JQDataGrid>
        </JQClientTools:JQDialog>
        <JQClientTools:JQDialog runat="server" ID="JQDialogTransferForm" Title="報價轉訂單" Closed="true" EditMode="Dialog" ShowModal="true" DialogLeft="100px" DialogTop="4px" Width="900px" Height="550px" OnSubmited="doneTransfer">
            <JQTools:JQLabel ID="lbTransferForm1" runat="server" Text=" " />
            <JQClientTools:JQDataGrid runat="server" ID="dataGridBILLNO1" RemoteName="smOPOM02.cmdOPO12OPO2" DataMember="cmdOPO12OPO2" AutoApply="True" AlwaysClose="false" EditOnEnter="True" MultiSelect="false" AllowAdd="False" AllowUpdate="False" AllowDelete="False" ViewCommandVisible="False" UpdateCommandVisible="False" DeleteCommandVisible="False" BufferView="False" Title="報價單列表" QueryTitle="查詢" Pagination="false" RecordLock="False" RecordLockMode="None" CheckOnSelect="True" PageList="10,20,30,40,50" PageSize="10" QueryMode="Window" QueryAutoColumn="False" TotalCaption="Total:" ColumnsHibeable="False" NotInitGrid="False" DuplicateCheck="False" EditMode="Dialog" InsertCommandVisible="False" Height="300px" OnSelect="dataGridBILLNO1Select">
                <Columns>
                    <JQClientTools:JQGridColumn FieldName="BILLDATE" Caption="日期" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" Format="yyyy-mm-dd" />
                    <JQClientTools:JQGridColumn FieldName="BILLNO" Caption="單號" IsNvarChar="false" Alignment="left" Width="100" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                </Columns>
                <TooItems>
                </TooItems>
                <QueryColumns>
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="日期" Condition="=" DataType="string" Editor="text" FieldName="BILLDATE" IsNvarChar="False" NewLine="True" RemoteMethod="False" Width="125" />
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="單號" Condition="%%" DataType="string" Editor="text" FieldName="BILLNO" IsNvarChar="False" NewLine="True" RemoteMethod="False" Width="125" />
                </QueryColumns>
            </JQClientTools:JQDataGrid>
            <JQClientTools:JQDataGrid runat="server" ID="dataGridTransferForm" RemoteName="sqOPO.cmdOPO12OPO2" DataMember="cmdOPO12OPO2" AutoApply="True" AlwaysClose="False" EditOnEnter="True" MultiSelect="True" AllowAdd="False" AllowUpdate="False" AllowDelete="False" ViewCommandVisible="False" UpdateCommandVisible="False" DeleteCommandVisible="False" BufferView="False" Title="報價轉訂單" QueryTitle="Query" Pagination="True" RecordLock="False" RecordLockMode="None" CheckOnSelect="True" PageList="5,10,20,30,40,50" PageSize="10" QueryMode="window" QueryAutoColumn="False" TotalCaption="Total:" ColumnsHibeable="False" NotInitGrid="False" DuplicateCheck="False" EditMode="Dialog" InsertCommandVisible="False" QueryLeft="200px" QueryTop="200px" Height="300px" MultiSelectGridID="dataGridTransferForm2">
                <Columns>
                    <JQClientTools:JQGridColumn FieldName="NOTOUTQTY" Caption="未轉數量" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="BILLDATE" Caption="原單日期" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" Format="yyyy-mm-dd" />
                    <JQClientTools:JQGridColumn FieldName="BILLNO" Caption="報價單號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="SEQNO" Caption="序號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRODUCTID" Caption="產品編號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRODCNAME" Caption="產品品名" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRODSTRUCTURE" Caption="產品規格" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="QUANTITY" Caption="數量" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="UNIT" Caption="單位" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRICE" Caption="單價" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="ONHANDDATE" Caption="預交貨日" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" Format="yyyy-mm-dd" />
                    <JQClientTools:JQGridColumn FieldName="MEMODOC" Caption="備註" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="WAREID" Caption="倉庫" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="CUSTID" Caption="客戶編號" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="false" />
                </Columns>
                <TooItems>
                    <JQClientTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" Text="查詢" OnClick="openQuery" Visible="true" Enabled="true" />
                </TooItems>
                <QueryColumns>
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="日期區間" Condition="&gt;=" DataType="string" Editor="datebox" FieldName="BILLDATE" IsNvarChar="False" NewLine="True" RemoteMethod="False" Width="125" DefaultMethod="uGetMonthFirstDateDash" />
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="～" Condition="&lt;=" DataType="string" Editor="datebox" FieldName="BILLDATE" IsNvarChar="False" NewLine="False" RemoteMethod="False" Width="125" DefaultMethod="uGetMonthLastDateDash" />
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="單號區間" Condition="&gt;=" DataType="string" Editor="text" FieldName="BILLNO" IsNvarChar="False" NewLine="True" RemoteMethod="False" Width="125" />
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="～" Condition="&lt;=" DataType="string" Editor="text" FieldName="BILLNO" IsNvarChar="False" NewLine="False" RemoteMethod="False" Width="125" />
                </QueryColumns>
            </JQClientTools:JQDataGrid>
            <JQClientTools:JQDataGrid runat="server" ID="dataGridTransferForm2" RemoteName="sqOPO.cmdOPO12OPO2" DataMember="cmdOPO12OPO2" AutoApply="False" AlwaysClose="True" EditOnEnter="True" MultiSelect="False" AllowAdd="False" AllowUpdate="True" AllowDelete="True" ViewCommandVisible="False" UpdateCommandVisible="False" DeleteCommandVisible="True" BufferView="False" Title="報價轉訂單" QueryTitle="Query" Pagination="False" RecordLock="False" RecordLockMode="None" CheckOnSelect="True" PageList="5,10,20,30,40,50" PageSize="5" QueryMode="Window" QueryAutoColumn="False" TotalCaption="Total:" ColumnsHibeable="False" NotInitGrid="False" DuplicateCheck="False" EditMode="Dialog" InsertCommandVisible="False" QueryLeft="200px" QueryTop="200px" Height="150px" RowNumbers="True">
                <Columns>
                    <JQClientTools:JQGridColumn FieldName="NOTOUTQTY" Caption="未轉數量" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="BILLDATE" Caption="原單日期" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" Format="yyyy-mm-dd" />
                    <JQClientTools:JQGridColumn FieldName="BILLNO" Caption="報價單號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="SEQNO" Caption="序號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRODUCTID" Caption="產品編號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRODCNAME" Caption="產品品名" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRODSTRUCTURE" Caption="產品規格" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="QUANTITY" Caption="數量" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="UNIT" Caption="單位" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRICE" Caption="單價" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="ONHANDDATE" Caption="預交貨日" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" Format="yyyy-mm-dd" />
                    <JQClientTools:JQGridColumn FieldName="MEMODOC" Caption="備註" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="WAREID" Caption="倉庫" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="CUSTID" Caption="客戶編號" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="false" />
                </Columns>
                <TooItems>
                </TooItems>
                <QueryColumns>
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="日期區間" Condition="&gt;=" DataType="string" Editor="datebox" FieldName="BILLDATE" IsNvarChar="False" NewLine="True" RemoteMethod="False" Width="125" DefaultMethod="uGetMonthFirstDateDash" />
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="～" Condition="&lt;=" DataType="string" Editor="datebox" FieldName="BILLDATE" IsNvarChar="False" NewLine="False" RemoteMethod="False" Width="125" DefaultMethod="uGetMonthLastDateDash" />
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="單號區間" Condition="&gt;=" DataType="string" Editor="text" FieldName="BILLNO" IsNvarChar="False" NewLine="True" RemoteMethod="False" Width="125" />
                    <JQClientTools:JQQueryColumn AndOr="and" Caption="～" Condition="&lt;=" DataType="string" Editor="text" FieldName="BILLNO" IsNvarChar="False" NewLine="False" RemoteMethod="False" Width="125" />
                </QueryColumns>
            </JQClientTools:JQDataGrid>
        </JQClientTools:JQDialog>
        <JQClientTools:JQDataGrid runat="server" ID="dgView" RemoteName="smOPOM02.Master" DataMember="Master" AutoApply="true" AlwaysClose="false" EditOnEnter="true" MultiSelect="true" AllowAdd="true" AllowUpdate="true" AllowDelete="true" ViewCommandVisible="true" UpdateCommandVisible="true" DeleteCommandVisible="true" EditDialogID="JQDialog1" BufferView="false" Title="客戶訂單維護" QueryTitle="快速查詢" Pagination="true" RecordLock="false" RecordLockMode="None" CheckOnSelect="true" PageSize="10" PageList="10,20,30,50,100" QueryMode="Panel" QueryAutoColumn="false" TotalCaption="Total:" ColumnsHibeable="true" NotInitGrid="false" DuplicateCheck="false" ReportFileName="~/gOPO/RS_REPORT/OPOM01_報價單.rdlc" OnLoadSuccess="dgViewLoadSuccess" OnUpdate="dgViewOnUpdate" OnDelete="dgViewOnDelete">
            <Columns>
                <JQClientTools:JQGridColumn FieldName="BILLDATE" Caption="日　　期" Editor="datebox" IsNvarChar="false" Alignment="left" Width="80" MaxLength="7" Sortable="true" Frozen="false" ReadOnly="false" Visible="true" Format="yyyy-mm-dd" />
                <JQClientTools:JQGridColumn FieldName="BILLNO" Caption="訂單編號" Editor="text" IsNvarChar="false" Alignment="left" Width="80" MaxLength="12" Sortable="true" Frozen="false" ReadOnly="false" Visible="true" />
                <JQClientTools:JQGridColumn FieldName="REFBILLNO" Caption="客戶單號" Editor="text" IsNvarChar="false" Alignment="left" Width="80" MaxLength="30" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                <JQClientTools:JQGridColumn FieldName="ONHANDDATE" Caption="預交貨日" Editor="datebox" IsNvarChar="false" Alignment="left" Width="80" MaxLength="7" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" Format="yyyy-mm-dd" />
                <JQClientTools:JQGridColumn FieldName="CUSTID" Caption="客戶編號" Editor="text" IsNvarChar="false" Alignment="left" Width="80" MaxLength="10" Sortable="true" Frozen="false" ReadOnly="false" Visible="true" />
                <JQClientTools:JQGridColumn FieldName="PROJECTID" Caption="專案編號" Editor="text" IsNvarChar="false" Alignment="left" Width="80" MaxLength="20" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                <JQClientTools:JQGridColumn FieldName="PERSONID" Caption="業務人員" Editor="text" IsNvarChar="false" Alignment="left" Width="80" MaxLength="10" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                <JQClientTools:JQGridColumn FieldName="WAREID" Caption="倉庫代號" Editor="text" IsNvarChar="false" Alignment="left" Width="80" MaxLength="4" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                <JQClientTools:JQGridColumn FieldName="SIGN_STATUS" Caption="審核狀態" Editor="text" IsNvarChar="false" Alignment="left" Width="80" MaxLength="20" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" DrillObjectID="JQDrillDown1">
                    <DrillFields>
                        <%--<JQClientTools:JQDrillDownFields FieldName="SIGN_USER" />--%>
                        <JQClientTools:JQDrillDownFields FieldName="BILLNO" />
                    </DrillFields>
                </JQClientTools:JQGridColumn>
                <JQClientTools:JQGridColumn Alignment="left" Caption="AR_BILLNO" Editor="text" FieldName="AR_BILLNO" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="True" Sortable="False" Visible="False" Width="80">
                </JQClientTools:JQGridColumn>
            </Columns>
            <TooItems>
                <JQClientTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" Text="新增" OnClick="insertItem" Visible="true" Enabled="true" />
                <JQClientTools:JQToolItem Icon="icon-sign" ItemType="easyui-linkbutton" Text="審核" OnClick="showSignDiv" Visible="true" Enabled="true" />
                <JQClientTools:JQToolItem Icon="icon-table" ItemType="easyui-linkbutton" Text="電話查詢" OnClick="openQueryMemo" Visible="true" Enabled="true" />
                <JQClientTools:JQToolItem Icon="icon-table" ItemType="easyui-linkbutton" Text="訂單批轉出貨" OnClick="OPOtoINV" Visible="true" Enabled="true" />
                <JQClientTools:JQToolItem Icon="icon-print" ItemType="easyui-linkbutton" Text="列印" OnClick="uShowTempSelectDiv" Visible="true" Enabled="true" />
            </TooItems>
            <QueryColumns>
                <JQClientTools:JQQueryColumn FieldName="BILLDATE" IsNvarChar="false" Caption="日　　期" Width="125" Condition=">=" Editor="datebox" RemoteMethod="false" AndOr="and" DataType="string" TableName="OPOORDER2_M" NewLine="true" DefaultMethod="GetTomorrow" />
                <JQClientTools:JQQueryColumn FieldName="BILLDATE" IsNvarChar="false" Caption=" ～ " Width="125" Condition="<=" Editor="datebox" RemoteMethod="false" AndOr="and" DataType="string" TableName="OPOORDER2_M" NewLine="false" DefaultMethod="GetTomorrow" />
                <JQClientTools:JQQueryColumn FieldName="BILLNO" IsNvarChar="false" Caption="訂單編號" Width="125" Condition="%%" Editor="text" RemoteMethod="false" AndOr="and" DataType="string" TableName="OPOORDER2_M" NewLine="false" />
                <JQClientTools:JQQueryColumn FieldName="CUSTID" IsNvarChar="false" Caption="客戶編號" Width="125" Condition="%%" Editor="inforefval" EditorOptions="title:'客戶資料維護',panelWidth:500,remoteName:'gServerDataModuleComm.cmdBASCUSTOMER',tableName:'cmdBASCUSTOMER', columns:[{field:'CUSTID',title:'客戶編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'CORPCNAME',title:'客戶名稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'CORPSHORTNAME',title:'客戶簡稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'CUSTCATEID',title:'客戶類別',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'CORPUNICODE',title:'統一編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'CONNECTPERSON',title:'連 絡 人',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'PERSONID',title:'業務人員',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'TEL1',title:'電話號碼1',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''} ],columnMatches:[],whereItems:[],valueField:'CUSTID',textField:'CORPSHORTNAME',valueFieldCaption:'客戶編號', textFieldCaption:'客戶簡稱',cacheRelationText:true,checkData:false,dialogCenter:true,showValueAndText:false,selectOnly:false" RemoteMethod="false" AndOr="and" DataType="string" TableName="OPOORDER2_M" NewLine="false" />
                <JQClientTools:JQQueryColumn FieldName="CORPMEMO" IsNvarChar="false" Caption="注意事項" Width="125" Condition="%%" Editor="text" RemoteMethod="false" AndOr="and" DataType="string" TableName="OPOORDER2_M" NewLine="false" />

            </QueryColumns>
        </JQClientTools:JQDataGrid>
        <JQClientTools:JQDialog runat="server" ID="JQDialog1" Title="客戶訂單維護" BindingObjectID="dataFormMaster" Closed="true" EditMode="Dialog" ShowModal="true" DialogLeft="80px" DialogTop="2px" Width="960px" Height="550px">
            <JQClientTools:JQDataForm runat="server" ID="dataFormMaster" RemoteName="smOPOM02.Master" DataMember="Master" HorizontalColumnsCount="3" DuplicateCheck="False" IsShowFlowIcon="False" IsAutoSubmit="False" IsAutoPause="False" IsAutoPageClose="False" IsRejectON="False" IsNotifyOFF="False" ContinueAdd="False" ShowApplyButton="False" IsRejectNotify="False" disapply="False" HorizontalGap="20" VerticalGap="1" DivFramed="false" ValidateStyle="Hint" Closed="False" ChainDataFormID="dataFormMaster1" OnApplied="dataFormMasterApplied" OnApply="dataFormMasterApply" OnLoadSuccess="dataFormMasterLoadSuccess">
                <Columns>
                    <JQClientTools:JQFormColumn FieldName="BILLDATE" Caption="日　　期" Alignment="left" Width="180" Editor="datebox" MaxLength="7" ReadOnly="false" Span="1" RowSpan="1" NewRow="true" Visible="true" Format="yyyy-mm-dd" />
                    <JQClientTools:JQFormColumn FieldName="BILLNO" Caption="　訂單編號" Alignment="left" Width="180" Editor="text" MaxLength="12" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                    <JQClientTools:JQFormColumn FieldName="SDTBILLNO" Caption="　自編單號" Alignment="left" Width="180" Editor="text" MaxLength="12" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                    <JQClientTools:JQFormColumn FieldName="ISSIGN" Caption="簽　　回" Alignment="left" Width="180" Editor="text" MaxLength="1" ReadOnly="false" Span="1" RowSpan="1" NewRow="true" Visible="true" />
                    <JQClientTools:JQFormColumn FieldName="REFBILLNO" Caption="　客戶單號" Alignment="left" Width="180" Editor="text" MaxLength="30" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                    <JQClientTools:JQFormColumn FieldName="ONHANDDATE" Caption="　預交貨日" Alignment="left" Width="180" Editor="datebox" MaxLength="7" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" Format="yyyy-mm-dd" />

                </Columns>
            </JQClientTools:JQDataForm>
            <JQClientTools:JQDefault runat="server" ID="defaultMaster" BindingObjectID="dataFormMaster">
                <Columns>
                    <JQClientTools:JQDefaultColumn FieldName="BILLDATE" DefaultValue="_TODAY" RemoteMethod="false" CarryOn="false" />
                    <JQClientTools:JQDefaultColumn FieldName="BILLNO" DefaultValue="autoNum" RemoteMethod="false" CarryOn="false" />
                    <JQClientTools:JQDefaultColumn FieldName="ISSIGN" DefaultValue="N" RemoteMethod="false" CarryOn="false" />

                </Columns>
            </JQClientTools:JQDefault>
            <JQClientTools:JQValidate runat="server" ID="validateMaster" BindingObjectID="dataFormMaster">
                <Columns>
                    <JQClientTools:JQValidateColumn FieldName="BILLDATE" CheckNull="true" ValidateType="None" RemoteMethod="true" />
                    <JQClientTools:JQValidateColumn FieldName="BILLNO" CheckNull="true" ValidateType="None" RemoteMethod="true" />
                    <JQClientTools:JQValidateColumn FieldName="ONHANDDATE" CheckNull="true" ValidateType="None" RemoteMethod="true" />
                </Columns>
            </JQClientTools:JQValidate>
            <JQClientTools:JQTab runat="server" ID="Tab1" Width="888px" Height="204px">
                <JQClientTools:JQTabItem runat="server" ID="TabItem1" Title="主要明細">
                    <JQClientTools:JQDataForm runat="server" ID="dataFormMaster1" RemoteName="smOPOM02.Master" DataMember="Master" HorizontalColumnsCount="3" DuplicateCheck="False" IsShowFlowIcon="False" IsAutoSubmit="False" IsAutoPause="False" IsAutoPageClose="False" IsRejectON="False" IsNotifyOFF="False" ContinueAdd="False" ShowApplyButton="False" IsRejectNotify="False" disapply="False" HorizontalGap="20" VerticalGap="1" DivFramed="False" ValidateStyle="Hint" ChainDataFormID="dataFormMaster2" AlwaysReadOnly="False" Closed="False">
                        <Columns>
                            <JQClientTools:JQFormColumn FieldName="WAREID" Caption="倉庫代號" Alignment="left" Width="180" Editor="inforefval" EditorOptions="title:'倉庫資料維護',panelWidth:600,remoteName:'gServerDataModuleComm.cmdBASWAREHOUSE',tableName:'cmdBASWAREHOUSE',columns:[{field:'WAREID',title:'倉庫編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'WARECNAME',title:'倉庫名稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'WAREENAME',title:'英文名稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'WARESHORTNAME',title:'倉庫簡稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'WAREHOUSETYPE',title:'倉庫屬性',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'TEL',title:'連絡電話',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'FAX',title:'傳　　真',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}],columnMatches:[],whereItems:[],valueField:'WAREID',textField:'WARECNAME',valueFieldCaption:'倉庫編號',textFieldCaption:'倉庫名稱',cacheRelationText:true,checkData:true,showValueAndText:false,dialogCenter:true,onSelect:WAREIDOnSelect,selectOnly:false,capsLock:'none'" MaxLength="4" ReadOnly="false" Span="1" RowSpan="1" NewRow="true" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="PERSONID" Caption="業務人員" Alignment="left" Width="180" Editor="inforefval" EditorOptions="title:'人員資料維護',panelWidth:600,remoteName:'gServerDataModuleComm.cmdBASPERSON',tableName:'cmdBASPERSON', columns:[{field:'PERSONID',title:'員工編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'PERSONCNAME',title:'員工姓名',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'ENGLISHNAME',title:'英文姓名',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'JOBNAME',title:'員工職稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'BIRTHDAY',title:'出生日期',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'DEPARTMENTID',title:'部門編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'CELLPHONE',title:'行動電話',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'FIRSTWORKDATE',title:'到 職 日',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''} ],columnMatches:[{field:'DEPARTMENTID',value:'DEPARTMENTID'} ],whereItems:[],valueField:'PERSONID',textField:'PERSONCNAME',valueFieldCaption:'員工編號',textFieldCaption:'姓名', cacheRelationText:true,checkData:true,dialogCenter:true,showValueAndText:false,selectOnly:false" MaxLength="10" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="DEPARTMENTID" Caption="部門編號" Alignment="left" Width="180" Editor="inforefval" EditorOptions="title:'部門資料維護',panelWidth:600,remoteName:'gServerDataModuleComm.cmdBASDEPARTMENT',tableName:'cmdBASDEPARTMENT', columns:[{field:'DEPARTMENTID',title:'部門編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'DEPARTMENTCNAME',title:'部門名稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'DEPARTMENTENAME',title:'英文名稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'MEMOCHAR',title:'備    註',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''} ],columnMatches:[ ],whereItems:[],valueField:'DEPARTMENTID',textField:'DEPARTMENTCNAME',valueFieldCaption:'部門編號',textFieldCaption:'部門名稱', cacheRelationText:true,checkData:true,dialogCenter:true,showValueAndText:false,selectOnly:false" MaxLength="4" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="CUSTID" Caption="客戶編號" Alignment="left" Width="180" Editor="inforefval" EditorOptions="title:'客戶資料維護',panelWidth:600,remoteName:'gServerDataModuleComm.cmdBASCUSTOMER',tableName:'cmdBASCUSTOMER',columns:[{field:'CUSTID',title:'客戶編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'CORPCNAME',title:'客戶名稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'CORPSHORTNAME',title:'客戶簡稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'CUSTCATEID',title:'客戶類別',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'CORPUNICODE',title:'統一編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'CONNECTPERSON',title:'連 絡 人',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'PERSONID',title:'業務人員',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''},{field:'TEL1',title:'電話號碼1',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}],columnMatches:[{field:'DELIVERADDRESS',value:'DELIVERADDRESS'},{field:'CONNECTPERSON',value:'CONNECTPERSON'},{field:'PERSONID',value:'PERSONID'},{field:'CURRENCYID',value:'CURRENCYID'},{field:'PAYMENTWAY',value:'PAYMENTWAY'},{field:'DEPARTMENTID',value:'DEPARTMENTID'}],whereItems:[],valueField:'CUSTID',textField:'CORPSHORTNAME',valueFieldCaption:'客戶編號',textFieldCaption:'客戶簡稱',cacheRelationText:true,checkData:true,showValueAndText:false,dialogCenter:true,selectOnly:false,capsLock:'none'" MaxLength="10" ReadOnly="false" Span="1" RowSpan="1" NewRow="true" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="PROJECTID" Caption="專案編號" Alignment="left" Width="180" Editor="inforefval" EditorOptions="title:'專案資料維護',panelWidth:600,remoteName:'gServerDataModuleComm.cmdPMSPROJECT',tableName:'cmdPMSPROJECT', columns:[{field:'PROJECTID',title:'專案代號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'PROJECTCLASSID',title:'專案類別',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'PROJ_CNAME',title:'中文名稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'PROJ_BOSS',title:'專案主管',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'PROJ_STARTDATE',title:'起始日期',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'PROJ_ENDDATE',title:'結束日期',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''} ],columnMatches:[ ],whereItems:[],valueField:'PROJECTID',textField:'PROJECTCLASSID',valueFieldCaption:'專案代號',textFieldCaption:'專案類別', cacheRelationText:true,checkData:true,dialogCenter:true,showValueAndText:false,selectOnly:false" MaxLength="20" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="CORPMEMO" Caption="注意事項" Alignment="left" Width="180" Editor="text" MaxLength="60" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="PAYMETHOD" Caption="付款方式" Alignment="left" Width="180" Editor="infocombobox" MaxLength="1" ReadOnly="false" Span="1" RowSpan="1" NewRow="true" Visible="true" EditorOptions="valueField:'KEYID',textField:'KEYVALUE',remoteName:'gServerDataModuleComm.cmdKEYVALUE',tableName:'cmdKEYVALUE',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                            <JQClientTools:JQFormColumn FieldName="DELIVERADDRESS" Caption="送貨地址" Alignment="left" Width="472" Editor="text" MaxLength="200" ReadOnly="false" Span="2" RowSpan="1" NewRow="false" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="CURRENCYID" Caption="幣別編號" Alignment="left" Width="180" Editor="inforefval" EditorOptions="title:'幣別資料維護',panelWidth:600,remoteName:'gServerDataModuleComm.cmdBASCURRENCY',tableName:'cmdBASCURRENCY', columns:[{field:'CURRENCYID',title:'幣別編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'CURRENCYCNAME',title:'幣別名稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'CURRENCYENAME',title:'英文名稱',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'CURRRATEIN',title:'買入匯率',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'CURRRATEOUT',title:'賣出匯率',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''} ],columnMatches:[{field:'CURRRATE',value:'CURRRATEOUT'} ],whereItems:[],valueField:'CURRENCYID',textField:'CURRENCYCNAME',valueFieldCaption:'幣別編號',textFieldCaption:'幣別名稱', cacheRelationText:true,checkData:true,dialogCenter:true,showValueAndText:false,selectOnly:false" MaxLength="4" ReadOnly="false" Span="1" RowSpan="1" NewRow="true" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="CURRRATE" Caption="匯率金額" Alignment="right" Width="180" Editor="text" MaxLength="33" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" Format="N" />
                            <JQClientTools:JQFormColumn FieldName="CONNECTPERSON" Caption="聯 絡 人" Alignment="left" Width="180" Editor="text" MaxLength="60" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="INVOICETYPE" Caption="發票聯式" Alignment="right" Width="180" Editor="infocombobox" MaxLength="1" ReadOnly="false" Span="1" RowSpan="1" NewRow="true" Visible="true" EditorOptions="valueField:'KEYID',textField:'KEYVALUE',remoteName:'gServerDataModuleComm.cmdKEYVALUE',tableName:'cmdKEYVALUE',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,onSelect:INVOICETYPEOnSelect,panelHeight:200" />
                            <JQClientTools:JQFormColumn FieldName="TAXCLASS" Caption="課稅類別" Alignment="right" Width="180" Editor="infocombobox" MaxLength="1" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" EditorOptions="valueField:'KEYID',textField:'KEYVALUE',remoteName:'gServerDataModuleComm.cmdKEYVALUE',tableName:'cmdKEYVALUE',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                            <JQClientTools:JQFormColumn FieldName="SUMQUALITY" Caption="數量合計" Alignment="right" Width="180" Editor="text" MaxLength="33" ReadOnly="true" Span="1" RowSpan="1" NewRow="false" Visible="true" Format="N" />
                            <JQClientTools:JQFormColumn FieldName="TOTALAMOUNT" Caption="未稅合計" Alignment="right" Width="180" Editor="text" MaxLength="33" ReadOnly="false" Span="1" RowSpan="1" NewRow="true" Visible="true" Format="N" />
                            <JQClientTools:JQFormColumn FieldName="TAXAMOUNT" Caption="營 業 稅" Alignment="right" Width="180" Editor="text" MaxLength="33" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" Format="N" />
                            <JQClientTools:JQFormColumn FieldName="SUMAMOUNT" Caption="總　　計" Alignment="right" Width="180" Editor="text" MaxLength="33" ReadOnly="true" Span="1" RowSpan="1" NewRow="false" Visible="true" Format="N" />
                        </Columns>
                        <RelationColumns>
                        </RelationColumns>
                    </JQClientTools:JQDataForm>
                    <JQClientTools:JQDefault runat="server" ID="defaultMaster1" BindingObjectID="dataFormMaster1">
                        <Columns>
                            <JQClientTools:JQDefaultColumn FieldName="WAREID" DefaultMethod="getMasterWAREID" RemoteMethod="false" CarryOn="false" />
                            <JQClientTools:JQDefaultColumn FieldName="DEPARTMENTID" DefaultMethod="getDEPARTMENTID" RemoteMethod="false" CarryOn="false" />
                            <JQClientTools:JQDefaultColumn FieldName="CURRENCYID" DefaultMethod="getCURRENCYID" RemoteMethod="false" CarryOn="false" />
                            <JQClientTools:JQDefaultColumn FieldName="CURRRATE" DefaultMethod="getCURRERATE" RemoteMethod="false" CarryOn="false" />
                            <JQClientTools:JQDefaultColumn FieldName="INVOICETYPE" DefaultValue="2" RemoteMethod="false" CarryOn="false" />
                            <JQClientTools:JQDefaultColumn FieldName="TAXCLASS" DefaultValue="0" RemoteMethod="false" CarryOn="false" />
                            <JQClientTools:JQDefaultColumn CarryOn="False" DefaultValue="0" FieldName="PAYMETHOD" RemoteMethod="True" />
                        </Columns>
                    </JQClientTools:JQDefault>
                    <JQClientTools:JQValidate runat="server" ID="validateMaster1" BindingObjectID="dataFormMaster1">
                        <Columns>
                            <JQClientTools:JQValidateColumn FieldName="PERSONID" CheckNull="true" ValidateType="None" RemoteMethod="true" />
                            <JQClientTools:JQValidateColumn FieldName="CUSTID" CheckNull="true" ValidateType="None" RemoteMethod="true" />
                            <JQClientTools:JQValidateColumn FieldName="WAREID" CheckNull="true" ValidateType="None" RemoteMethod="true" />
                            <JQClientTools:JQValidateColumn FieldName="DEPARTMENTID" CheckNull="true" ValidateType="None" RemoteMethod="true" />
                            <JQClientTools:JQValidateColumn FieldName="CURRENCYID" CheckNull="true" ValidateType="None" RemoteMethod="true" />

                        </Columns>
                    </JQClientTools:JQValidate>
                </JQClientTools:JQTabItem>
                <JQClientTools:JQTabItem runat="server" ID="TabItem2" Title="其他明細">
                    <JQClientTools:JQDataForm runat="server" ID="dataFormMaster2" RemoteName="smOPOM02.Master" DataMember="Master" HorizontalColumnsCount="3" DuplicateCheck="false" IsShowFlowIcon="false" IsAutoSubmit="false" IsAutoPause="false" IsAutoPageClose="false" IsRejectON="false" IsNotifyOFF="false" ContinueAdd="false" ShowApplyButton="false" IsRejectNotify="false" disapply="false" HorizontalGap="20" VerticalGap="1" DivFramed="false" ValidateStyle="Hint" ChainDataFormID="dataFormMaster3" AlwaysReadOnly="False" Closed="False">
                        <Columns>
                            <JQClientTools:JQFormColumn FieldName="MEMODOC" Caption="備　　註" Alignment="left" Width="472" Editor="textarea" MaxLength="2000" ReadOnly="false" Span="5" RowSpan="1" NewRow="true" Visible="true" EditorOptions="Height:50" />
                            <JQClientTools:JQFormColumn FieldName="DATAPATH" Caption="附檔" Alignment="left" Editor="infofileupload" EditorOptions="filter:'',isAutoNum:false,upLoadFolder:'Files/OPOM02',showButton:false,showLocalFile:true" MaxLength="100" NewRow="true" ReadOnly="false" RowSpan="1" Span="5" Visible="true" Width="472" />
                            <JQClientTools:JQFormColumn Alignment="left" Caption="訂金" Editor="text" FieldName="PREPAYAMT" Format="N" MaxLength="12" NewRow="True" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                            <JQClientTools:JQFormColumn Alignment="left" Caption="收款單號" Editor="text" FieldName="AR_BILLNO" MaxLength="20" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                            <JQClientTools:JQFormColumn FieldName="PAYMENTWAY" Caption="收款方式" Alignment="left" Width="180" Editor="text" MaxLength="100" ReadOnly="false" Span="1" RowSpan="1" NewRow="true" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="SHIPMENT" Caption="交貨方式" Alignment="left" Width="180" Editor="text" MaxLength="100" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="BILLHEADNO" Caption="表頭條文" Alignment="left" Width="180" Editor="inforefval" EditorOptions="title:'報表頭文設定',panelWidth:600,remoteName:'gServerDataModuleComm.cmdCOMHEADDOC',tableName:'cmdCOMHEADDOC', columns:[{field:'TAILID',title:'表頭代號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'TAILDESC',title:'表頭說明',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''} ],columnMatches:[ ],whereItems:[],valueField:'TAILID',textField:'TAILDESC',valueFieldCaption:'表頭代號',textFieldCaption:'表頭說明', cacheRelationText:true,checkData:true,dialogCenter:true,showValueAndText:false,selectOnly:false" MaxLength="4" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="VALIDITYDATE" Caption="有效日期" Alignment="left" Width="180" Editor="datebox" MaxLength="7" ReadOnly="false" Span="1" RowSpan="1" NewRow="true" Visible="true" Format="yyyy-mm-dd" />
                            <JQClientTools:JQFormColumn FieldName="VALIDITYDOC" Caption="有效日說明" Alignment="left" Width="180" Editor="text" MaxLength="50" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="BILLTAILNO" Caption="表尾條文" Alignment="left" Width="180" Editor="inforefval" EditorOptions="title:'報表尾文設定',panelWidth:600,remoteName:'gServerDataModuleComm.cmdCOMTAILDOC',tableName:'cmdCOMTAILDOC', columns:[{field:'TAILID',title:'表尾代號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'TAILDESC',title:'表尾說明',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''} ],columnMatches:[ ],whereItems:[],valueField:'TAILID',textField:'TAILDESC',valueFieldCaption:'表尾代號',textFieldCaption:'表尾說明', cacheRelationText:true,checkData:true,dialogCenter:true,showValueAndText:false,selectOnly:false" MaxLength="4" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                        </Columns>
                        <RelationColumns>
                        </RelationColumns>
                    </JQClientTools:JQDataForm>
                </JQClientTools:JQTabItem>
                <JQClientTools:JQTabItem runat="server" ID="TabItem3" Title="擴充欄位">
                    <JQClientTools:JQDataForm runat="server" ID="dataFormMaster3" RemoteName="smOPOM02.Master" DataMember="Master" HorizontalColumnsCount="3" DuplicateCheck="false" IsShowFlowIcon="false" IsAutoSubmit="false" IsAutoPause="false" IsAutoPageClose="false" IsRejectON="false" IsNotifyOFF="false" ContinueAdd="false" ShowApplyButton="false" IsRejectNotify="false" disapply="false" HorizontalGap="20" VerticalGap="1" DivFramed="false" ValidateStyle="Hint" AlwaysReadOnly="False" Closed="False">
                        <Columns>
                            <JQClientTools:JQFormColumn FieldName="DATAEXTEND1" Caption="擴充欄位1" Alignment="left" Width="472" Editor="text" MaxLength="200" ReadOnly="false" Span="5" RowSpan="1" NewRow="true" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="DATAEXTEND2" Caption="擴充欄位2" Alignment="left" Width="472" Editor="text" MaxLength="200" ReadOnly="false" Span="5" RowSpan="1" NewRow="true" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="DATAEXTEND3" Caption="擴充欄位3" Alignment="left" Width="180" Editor="text" MaxLength="200" ReadOnly="false" Span="1" RowSpan="1" NewRow="true" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="DATAEXTEND4" Caption="擴充欄位4" Alignment="left" Width="180" Editor="text" MaxLength="200" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="DATAEXTEND5" Caption="擴充欄位5" Alignment="left" Width="180" Editor="text" MaxLength="200" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="DATAEXTEND6" Caption="擴充欄位6" Alignment="left" Width="180" Editor="text" MaxLength="200" ReadOnly="false" Span="1" RowSpan="1" NewRow="true" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="DATAEXTEND7" Caption="擴充欄位7" Alignment="right" Width="180" Editor="text" MaxLength="33" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" Format="N" />
                            <JQClientTools:JQFormColumn FieldName="DATAEXTEND8" Caption="擴充欄位8" Alignment="right" Width="180" Editor="text" MaxLength="33" ReadOnly="false" Span="1" RowSpan="1" NewRow="false" Visible="true" Format="N" />
                            <JQClientTools:JQFormColumn FieldName="CREATE_USER" Caption="建檔人員" Alignment="left" Width="180" Editor="text" MaxLength="20" ReadOnly="true" Span="1" RowSpan="1" NewRow="true" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="UPDATE_USER" Caption="修改人員" Alignment="left" Width="180" Editor="text" MaxLength="20" ReadOnly="true" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="SIGN_USER" Caption="覆核人員" Alignment="left" Width="180" Editor="text" MaxLength="20" ReadOnly="true" Span="1" RowSpan="1" NewRow="false" Visible="true" />
                            <JQClientTools:JQFormColumn FieldName="CREATE_DATE" Caption="建檔日期" Alignment="left" Width="180" Editor="datebox" MaxLength="7" ReadOnly="true" Span="1" RowSpan="1" NewRow="true" Visible="true" Format="yyyy-mm-dd" />
                            <JQClientTools:JQFormColumn FieldName="UPDATE_DATE" Caption="修改日期" Alignment="left" Width="180" Editor="datebox" MaxLength="7" ReadOnly="true" Span="1" RowSpan="1" NewRow="false" Visible="true" Format="yyyy-mm-dd" />
                            <JQClientTools:JQFormColumn FieldName="SIGN_DATE" Caption="覆核日期" Alignment="left" Width="180" Editor="datebox" MaxLength="10" ReadOnly="true" Span="1" RowSpan="1" NewRow="false" Visible="true" Format="yyyy-mm-dd" />
                        </Columns>
                        <RelationColumns>
                        </RelationColumns>
                    </JQClientTools:JQDataForm>
                </JQClientTools:JQTabItem>

            </JQClientTools:JQTab>
            <JQClientTools:JQDataGrid runat="server" ID="dataGridDetail" RemoteName="smOPOM02.Master" DataMember="Detail" AutoApply="false" AlwaysClose="false" EditOnEnter="true" MultiSelect="false" AllowAdd="true" AllowUpdate="true" AllowDelete="true" ViewCommandVisible="false" UpdateCommandVisible="false" DeleteCommandVisible="true" ParentObjectID="dataFormMaster" BufferView="false" Title="" QueryTitle="Query" Pagination="false" RecordLock="false" RecordLockMode="None" CheckOnSelect="true" PageSize="10" PageList="10,20,30,50,100" QueryMode="Window" QueryAutoColumn="false" TotalCaption="Total:" ColumnsHibeable="false" NotInitGrid="false" DuplicateCheck="false" EditMode="Dialog" InsertCommandVisible="True" QueryLeft="" QueryTop="" OnLoadSuccess="dataGridDetailLoadSuccess" Width="888px" Height="196px">
                <Columns>
                    <JQClientTools:JQGridColumn FieldName="BILLNO" Caption="訂單編號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="12" Sortable="false" Frozen="false" ReadOnly="false" Visible="false" />
                    <JQClientTools:JQGridColumn FieldName="SEQNO" Caption="欄號" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="3" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" Format="N" />
                    <JQClientTools:JQGridColumn FieldName="WAREID" Caption="倉庫代號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="4" Sortable="false" Frozen="false" ReadOnly="false" Visible="True" />
                    <JQClientTools:JQGridColumn FieldName="CUSTID" Caption="客戶編號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="10" Sortable="false" Frozen="false" ReadOnly="false" Visible="false" />
                    <JQClientTools:JQGridColumn FieldName="PRODUCTID" Caption="產品編號" IsNvarChar="false" Alignment="left" Width="80" Editor="inforefval" EditorOptions="title:'產品資料維護',panelWidth:600,remoteName:'gServerDataModuleComm.cmdBASPRODUCT',tableName:'cmdBASPRODUCT', columns:[{field:'PRODUCTID',title:'產品編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'SCANCODE',title:'條碼編號',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'PRODCNAME',title:'產品品名',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'PRODSTRUCTURE',title:'產品規格',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'INVOICEPRODNAME',title:'產品品牌',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'PRODCATEID',title:'產品類別',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'SALEPRICE',title:'建議售價',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''}, {field:'CALCUNIT',title:'計量單位',width:80,align:'left',table:'',isNvarChar:false,queryCondition:''} ],columnMatches:[{field:'PRODCNAME',value:'PRODCNAME'},{field:'PRODSTRUCTURE',value:'PRODSTRUCTURE'},{field:'UNIT',value:'CALCUNIT'},{field:'PRICE',value:'PRICE'} ],whereItems:[],valueField:'PRODUCTID',textField:'PRODUCTID',valueFieldCaption:'產品編號',textFieldCaption:'產品編號', cacheRelationText:true,checkData:true,dialogCenter:true,showValueAndText:false,onSelect:selectPRODUCTID,selectOnly:false" MaxLength="0" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRODCNAME" Caption="品名" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="30" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRODENAME" Caption="英文名稱" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="50" Sortable="false" Frozen="false" ReadOnly="false" Visible="false" />
                    <JQClientTools:JQGridColumn FieldName="PRODSTRUCTURE" Caption="規格" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="30" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="QUANTITY" Caption="數量" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="33" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" Format="N" EditorOptions="onblur:'quantityCalculate',capsLock:'none'" FormatScript="uSetQtyDecimal" />
                    <JQClientTools:JQGridColumn FieldName="UNIT" Caption="單位" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="10" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="PRICE" Caption="單價" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="33" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" Format="N" EditorOptions="onblur:'priceCalculate',capsLock:'none'" FormatScript="uSetPriceDecimal" />
                    <JQClientTools:JQGridColumn FieldName="TAXPRICE" Caption="含稅單價" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="33" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" Format="N" EditorOptions="onblur:'taxPriceCalculate',capsLock:'none'" FormatScript="uSetTaxDecimal" />
                    <JQClientTools:JQGridColumn FieldName="PRODCOST" Caption="產品當時成本" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="33" Sortable="false" Frozen="false" ReadOnly="false" Visible="false" Format="N" />
                    <JQClientTools:JQGridColumn FieldName="SUBAMOUNT" Caption="金額" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="33" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" Format="N" EditorOptions="onblur:'calcAmount',capsLock:'none'" />
                    <JQClientTools:JQGridColumn FieldName="TAXAMOUNT" Caption="含稅金額" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="33" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" Format="N" EditorOptions="onblur:'taxAmountCalculate',capsLock:'none'" FormatScript="uSetTaxDecimal" />
                    <JQClientTools:JQGridColumn FieldName="ONHANDDATE" Caption="預交貨日" IsNvarChar="false" Alignment="left" Width="80" Editor="datebox" MaxLength="10" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" Format="yyyy-mm-dd" />
                    <JQClientTools:JQGridColumn FieldName="ASSEMBLEQTY" Caption="組合數量" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="33" Sortable="false" Frozen="false" ReadOnly="false" Visible="false" Format="N" />
                    <JQClientTools:JQGridColumn FieldName="SOURCEBILLNO" Caption="原單單號" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="12" Sortable="false" Frozen="false" ReadOnly="false" Visible="false" />
                    <JQClientTools:JQGridColumn FieldName="SOURCEUNIQUENO" Caption="原單序號" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="3" Sortable="false" Frozen="false" ReadOnly="false" Visible="false" Format="N" />
                    <JQClientTools:JQGridColumn FieldName="ISNEEDMADE" Caption="生產否" IsNvarChar="false" Alignment="left" Width="80" Editor="infocombobox" MaxLength="1" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" EditorOptions="items:[{value:'Y',text:'Y',selected:'false'},{value:'N',text:'N',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:42" />
                    <JQClientTools:JQGridColumn FieldName="MEMOCHAR1" Caption="備註1" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="30" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="MEMOCHAR2" Caption="連絡電話" IsNvarChar="false" Alignment="left" Width="80" Editor="text" MaxLength="30" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" />
                    <JQClientTools:JQGridColumn FieldName="ISGIFTPROD" Caption="贈品" IsNvarChar="false" Alignment="left" Width="80" Editor="infocombobox" MaxLength="1" Sortable="false" Frozen="false" ReadOnly="false" Visible="true" EditorOptions="items:[{value:'Y',text:'Y',selected:'false'},{value:'N',text:'N',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:42" />
                    <JQClientTools:JQGridColumn FieldName="BOXQTY" Caption="雙單位數量" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="33" Sortable="false" Frozen="false" ReadOnly="false" Visible="false" Format="N" />
                    <JQClientTools:JQGridColumn FieldName="BOXPRICE" Caption="雙單位單價" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="33" Sortable="false" Frozen="false" ReadOnly="false" Visible="false" Format="N" />
                    <JQClientTools:JQGridColumn FieldName="UNIQUENO" Caption="唯一序號" IsNvarChar="false" Alignment="right" Width="80" Editor="text" MaxLength="3" Sortable="false" Frozen="false" ReadOnly="false" Visible="false" Format="N" />

                </Columns>
                <RelationColumns>
                    <JQClientTools:JQRelationColumn ParentFieldName="BILLNO" FieldName="BILLNO" />

                </RelationColumns>
                <TooItems>
                    <JQClientTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" Text="新增" OnClick="insertItem" Visible="true" Enabled="true" />
                    <JQClientTools:JQToolItem Icon="icon-cancel" ItemType="easyui-linkbutton" Text="取消" OnClick="cancel" Visible="true" Enabled="true" />
                    <JQClientTools:JQToolItem Icon="icon-copy2" ItemType="easyui-linkbutton" Text="複製" OnClick="copyData" Visible="true" Enabled="true" />
                    <JQClientTools:JQToolItem Icon="icon-table" ItemType="easyui-linkbutton" Text="多單位輸入" OnClick="uopenBOXDiv" Visible="true" Enabled="true" />
                    <JQClientTools:JQToolItem Icon="icon-report" ItemType="easyui-linkbutton" Text="庫存查詢" OnClick="openStockGrid" Visible="true" Enabled="true" />
                    <JQClientTools:JQToolItem Icon="icon-table" ItemType="easyui-linkbutton" Text="產品多選" OnClick="openProdMultiSelect" Visible="true" Enabled="true" />
                    <JQClientTools:JQToolItem Icon="icon-tip" ItemType="easyui-linkbutton" Text="產品售價查詢" OnClick="openPriceGrid" Visible="true" Enabled="true" />
                    <JQClientTools:JQToolItem Icon="icon-report" ItemType="easyui-linkbutton" Text="交易歷史查詢" OnClick="openCustQuery" Visible="true" Enabled="true" />
                    <JQClientTools:JQToolItem Icon="icon-table" ItemType="easyui-linkbutton" Text="報價轉訂單" OnClick="openTransferForm" Visible="true" Enabled="true" />
                </TooItems>
            </JQClientTools:JQDataGrid>
        </JQClientTools:JQDialog>
        <JQClientTools:JQDefault runat="server" ID="defaultDetail" BindingObjectID="dataGridDetail">
            <Columns>
                <JQClientTools:JQDefaultColumn FieldName="SEQNO" DefaultMethod="getSEQNO" RemoteMethod="false" CarryOn="false" />
                <JQClientTools:JQDefaultColumn FieldName="WAREID" DefaultMethod="getWAREID" RemoteMethod="false" CarryOn="false" />
                <JQClientTools:JQDefaultColumn FieldName="QUANTITY" DefaultValue="1" RemoteMethod="false" CarryOn="false" />
                <JQClientTools:JQDefaultColumn FieldName="ISNEEDMADE" DefaultValue="N" RemoteMethod="false" CarryOn="false" />
                <JQClientTools:JQDefaultColumn FieldName="ISGIFTPROD" DefaultValue="N" RemoteMethod="false" CarryOn="false" />
                <JQClientTools:JQDefaultColumn FieldName="UNIQUENO" DefaultMethod="getUNIQUENO" RemoteMethod="false" CarryOn="false" />
                <JQClientTools:JQDefaultColumn CarryOn="False" DefaultMethod="GetONHANDDATE" FieldName="ONHANDDATE" RemoteMethod="False" />

            </Columns>
        </JQClientTools:JQDefault>
        <JQClientTools:JQValidate runat="server" ID="validateDetail" BindingObjectID="dataGridDetail">
            <Columns>
            </Columns>
        </JQClientTools:JQValidate>
        <JQClientTools:JQDrillDown ID="JQDrillDown1" runat="server" DrillStyle="Command" OpenMode="Dialog" RemoteName="gServerDataModuleComm.cmdSIGNLOG" DataMember="cmdSIGNLOG" FormCaption="審核記錄">
            <KeyFields>
                <%--<JQClientTools:JQDrillDownKeyFields FieldName="SIGN_PERSON" />--%>
                <JQClientTools:JQDrillDownKeyFields FieldName="SOURCEBILLNO" />
            </KeyFields>
            <DisplayFields>
                <JQClientTools:JQDrillDownDisplayFields Caption="原單編號" FieldName="SOURCEBILLNO" Width="100">
                </JQClientTools:JQDrillDownDisplayFields>
                <JQClientTools:JQDrillDownDisplayFields Caption="審核人員" FieldName="SIGN_PERSON" Width="100">
                </JQClientTools:JQDrillDownDisplayFields>
                <JQClientTools:JQDrillDownDisplayFields Caption="審核日期" FieldName="SIGN_DATE" Width="100">
                </JQClientTools:JQDrillDownDisplayFields>
                <JQClientTools:JQDrillDownDisplayFields Caption="審核狀態" FieldName="SIGN_STATUS" Width="100">
                </JQClientTools:JQDrillDownDisplayFields>
                <JQClientTools:JQDrillDownDisplayFields Caption="審核記錄" FieldName="SIGN_MEMO" Width="200">
                </JQClientTools:JQDrillDownDisplayFields>
            </DisplayFields>
        </JQClientTools:JQDrillDown>
        <JQClientTools:JQBatchMove ID="JQBatchMove1" runat="server" DesDataGrid="dataGridDetail" SrcDataGrid="dataGridMultiSelect2" OnEachMove="getPrice" SrcSelectAll="True">
            <MatchColumns>
                <JQClientTools:JQBatchMoveColumns DesColumn="PRODUCTID" SrcColumn="PRODUCTID" />
                <JQClientTools:JQBatchMoveColumns DesColumn="PRODCNAME" SrcColumn="PRODCNAME" />
                <JQClientTools:JQBatchMoveColumns DesColumn="PRODSTRUCTURE" SrcColumn="PRODSTRUCTURE" />
                <JQClientTools:JQBatchMoveColumns DesColumn="UNIT" SrcColumn="CALCUNIT" />
                <JQClientTools:JQBatchMoveColumns DesColumn="PRICE" SrcColumn="SALEPRICE" />
                <JQClientTools:JQBatchMoveColumns DesColumn="SUBAMOUNT" SrcColumn="SALEPRICE" />
                <JQClientTools:JQBatchMoveColumns DesColumn="TAXPRICE" SrcColumn="SALEPRICE" />
                <JQClientTools:JQBatchMoveColumns DesColumn="TAXAMOUNT" SrcColumn="SALEPRICE" />
            </MatchColumns>
        </JQClientTools:JQBatchMove>
        <JQClientTools:JQBatchMove ID="JQBatchMove3" runat="server" DesDataGrid="dataGridDetail" SrcDataGrid="dataGridTransferForm2" OnEachMove="TransferGetPrice" SrcSelectAll="True">
            <MatchColumns>
                <JQClientTools:JQBatchMoveColumns DesColumn="PRODUCTID" SrcColumn="PRODUCTID" />
                <JQClientTools:JQBatchMoveColumns DesColumn="PRODCNAME" SrcColumn="PRODCNAME" />
                <JQClientTools:JQBatchMoveColumns DesColumn="PRODENAME" SrcColumn="PRODENAME" />
                <JQClientTools:JQBatchMoveColumns DesColumn="PRODSTRUCTURE" SrcColumn="PRODSTRUCTURE" />
                <JQClientTools:JQBatchMoveColumns DesColumn="QUANTITY" SrcColumn="QUANTITY" />
                <JQClientTools:JQBatchMoveColumns DesColumn="UNIT" SrcColumn="UNIT" />
                <JQClientTools:JQBatchMoveColumns DesColumn="PRICE" SrcColumn="PRICE" />
                <JQClientTools:JQBatchMoveColumns DesColumn="SUBAMOUNT" SrcColumn="SUBAMOUNT" />
                <JQClientTools:JQBatchMoveColumns DesColumn="TAXPRICE" SrcColumn="TAXPRICE" />
                <JQClientTools:JQBatchMoveColumns DesColumn="TAXAMOUNT" SrcColumn="TAXAMOUNT" />
                <JQClientTools:JQBatchMoveColumns DesColumn="SOURCEBILLNO" SrcColumn="BILLNO" />
                <JQClientTools:JQBatchMoveColumns DesColumn="SOURCEUNIQUENO" SrcColumn="UNIQUENO" />
                <JQClientTools:JQBatchMoveColumns DesColumn="WAREID" SrcColumn="WAREID" />
                <JQClientTools:JQBatchMoveColumns DesColumn="CUSTID" SrcColumn="CUSTID" />
                <JQClientTools:JQBatchMoveColumns DesColumn="ONHANDDATE" SrcColumn="ONHANDDATE" />
            </MatchColumns>
        </JQClientTools:JQBatchMove>
        <JQTools:JQMultiLanguage ID="JQMultiLanguage1" runat="server" Active="True" DBAlias="gGexDemo" GroupIndex="ChineseTra" />
        <JQTools:JQSecurity ID="JQSecurity1" runat="server" DBAlias="gGexDemo" MenuID="jqOPOM02" />
    </form>
    <script>
        $('#JQDialog1').dialog({
            onBeforeClose: function () {
                if (!submitFlag) {
                    closeForm('#JQDialog1');
                    return closeWin;
                }
            }
        });
    </script>
</body>
</html>
