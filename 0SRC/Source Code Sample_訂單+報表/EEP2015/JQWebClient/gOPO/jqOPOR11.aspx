<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jqOPOR11.aspx.cs" Inherits="jqOPOR11" %>

<%@ Register Assembly="JQClientTools" Namespace="JQClientTools" TagPrefix="JQClientTools" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>已訂購未出貨明細表</title>
    <script>
        $(document).ready(function () {
            var tColumns = [["lbBILLDATE1,80,label", "BILLDATE1Query,180,datebox", "lbBILLDATE2,20,label", "BILLDATE2Query,180,datebox","lbBILLNO1,80,label", "BILLNO1Query,100,text", "lbBILLNO2,20,label", "BILLNO2Query,160,text"],["btOpenIDNO,80,buttonToLabel", "IDNO1Query,180,refval", "lbIDNO2,20,label", "IDNO2Query,180,refval","btOpenPRODUCTID,80,buttonToLabel", "PRODUCTID1Query,180,refval", "lbPRODUCTID2,20,label", "PRODUCTID2Query,180,refval"],["btOpenPERSONID,80,buttonToLabel", "PERSONID1Query,180,refval", "lbPERSONID2,20,label", "PERSONID2Query,180,refval","lbONHANDDATE1,80,label", "ONHANDDATE1Query,180,datebox", "lbONHANDDATE2,20,label", "ONHANDDATE2Query,180,datebox", "btMultiQuery,80,button,2"]];
            //
            uInitTable("queryPanel", 1100, tColumns);
            
            
        });

        var loadOnce = true;
        function dataGridMasterLoadSuccess() {
            //Mapping SYS_RPTDEFINE.FUNCTAG, ToGet Report List
            uGetRDLC('OPOR11', 'TemplateList');
            
            if (loadOnce) {
                //When PageLoad, Erase GEXBAS_TMP Temp Date
                uDeleteGEXBASTM('OPOR11');
                
                //Set Default(1.DateField 2.RangeField 3.Single Filter Field)
                uSetQueryDefault("BILLDATE;ONHANDDATE", "BILLDATE;BILLNO;IDNO;PRODUCTID;PERSONID;ONHANDDATE", "");
                loadOnce = false;
            }
        }

        function openReport() {
            uOpenRDLC("dataGridMaster", "", "", "TemplateList", "OPOR11", "srOPO.VIEW_RPT_OPOR11", "VIEW_RPT_OPOR11", "gOPO", true);
        }

        //Define MultiSelect Array
        var msBASCUSTOMER = [], msBASPRODUCT = [], msBASPERSON = [];

        //multiTableSeries parameter
        var multiTableSeries = [["BASCUSTOMER", "CUSTID", "CORPSHORTNAME","客戶編號", "客戶名稱", "客戶查詢", "已選客戶編號"],["BASPRODUCT", "PRODUCTID", "PRODUCTID","產品編號", "品名規格", "產品查詢", "已選產品編號"],["BASPERSON", "PERSONID", "PERSONCNAME","員工編號", "員工姓名", "員工查詢", "已選員工編號"]];
           
        function openMulitWinIDNO() {
            uOpenMultiWin('BASCUSTOMER' ,'OPOR11');
        }
        function openMulitWinPRODUCTID() {
            uOpenMultiWin('BASPRODUCT' ,'OPOR11');
        }
        function openMulitWinPERSONID() {
            uOpenMultiWin('BASPERSON' ,'OPOR11');
        }

        function multiQuery() {
            //dataGridID, queryStr1-rangeQuery, queryStr2-LikeQuery, queryStr3-singleQuery
            uReportMultiQuery("dataGridMaster", "BILLDATE;BILLNO;IDNO;PRODUCTID;PERSONID;ONHANDDATE", "", "", "BASCUSTOMER,CUSTID;BASPRODUCT,PRODUCTID;BASPERSON,PERSONID");
        }
        
        function Hyperlink(value, row, index) {
            var retStr = "";
                retStr = "<a href='javascript: void(0)' onclick='uLinkReply(this, \"OPOM02 客戶訂單維護\", \"gOPO\", \"jqOPOM02.aspx\", \"BILLNO\");'>" + value + "</a>";
            return retStr;
        }

        //self define to excel fields, if not set, all fields
        var excelFields = "";
        function exportExcel(dgid) {
            uExportExcel(dgid, "已訂購未出貨明細表", "VIEW_RPT_OPOR11", "gOPO_jqOPOR11", "VIEW_RPT_OPOR11", excelFields);
        }
        function openPivotDialog() {
            uOpenPivotDialogView("OPOR11樞紐分析", "gSYS/jqPivotTable.aspx", "srOPO.VIEW_RPT_OPOR11", "VIEW_RPT_OPOR11", "BILLDATE;BILLNO;IDNO;PRODUCTID;PERSONID;ONHANDDATE", "", "", "BASCUSTOMER,CUSTID;BASPRODUCT,PRODUCTID;BASPERSON,PERSONID", "IDNO,BILLDATE,PRODUCTID,PERSONID,SUBAMOUNT,QUANTITY,NOTOUTQTY", "客戶編號,日期,產品編號,人員編號,原幣金額,數量,未出數量", "產品編號", "客戶編號");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQClientTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQClientTools:JQDialog runat="server" ID="JQDialogTempSelect" Title="選擇報表" Closed="true" EditMode="Dialog" ShowModal="true" DialogLeft="50%" DialogTop="50%" Width="250px">
                <JQClientTools:JQComboBox ID="TemplateList" runat="server">
                    <Items>
                        <JQClientTools:JQComboItem Selected="False" Text="OPOR11_01.rdlc" Value="OPOR11_01.rdlc" />
                    </Items>
                </JQClientTools:JQComboBox>
                <br />
                <JQClientTools:JQCheckBox ID="cbPDFPrint" runat="server" />
                <JQClientTools:JQLabel ID="lbPDFPrint" runat="server" Text="直接轉 PDF 列印" Width="300px" />
            </JQClientTools:JQDialog>
            <JQClientTools:JQPanel ID="queryPanel" runat="server" Width="100%" Title="查詢" Collapsible="true">
            </JQClientTools:JQPanel>
            <JQClientTools:JQLabel ID="lbBILLDATE1" runat="server" Text="日期區間：" />
            <JQClientTools:JQDateBox ID="BILLDATE1Query" runat="server" />
            <JQClientTools:JQLabel ID="lbBILLDATE2" runat="server" Text="～" />
            <JQClientTools:JQDateBox ID="BILLDATE2Query" runat="server" />
            <JQClientTools:JQLabel ID="lbBILLNO1" runat="server" Text="單號區間：" />
            <JQClientTools:JQTextBox ID="BILLNO1Query" runat="server" />
            <JQClientTools:JQLabel ID="lbBILLNO2" runat="server" Text="～" />
            <JQClientTools:JQTextBox ID="BILLNO2Query" runat="server" />
            <JQClientTools:JQButton ID="btOpenIDNO" runat="server" Text="客戶區間：" OnClick="openMulitWinIDNO" />
            <JQClientTools:JQRefval ID="IDNO1Query" runat="server" RemoteName="gServerDataModuleComm.cmdBASCUSTOMER" DisplayMember="CORPSHORTNAME" ValueMember="CUSTID" CheckData="false" DialogCenter="true" DialogWidth="700">
                <Columns>
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="客戶編號" FieldName="CUSTID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="客戶名稱" FieldName="CORPCNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="客戶簡稱" FieldName="CORPSHORTNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="客戶類別" FieldName="CUSTCATEID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="統一編號" FieldName="CORPUNICODE" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="連 絡 人" FieldName="CONNECTPERSON" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="業務人員" FieldName="PERSONID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="電話號碼1" FieldName="TEL1" />
                </Columns>
            </JQClientTools:JQRefval>
            <JQClientTools:JQLabel ID="lbIDNO2" runat="server" Text="～" />
            <JQClientTools:JQRefval ID="IDNO2Query" runat="server" RemoteName="gServerDataModuleComm.cmdBASCUSTOMER" DisplayMember="CORPSHORTNAME" ValueMember="CUSTID" CheckData="false" DialogCenter="true" DialogWidth="700">
                <Columns>
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="客戶編號" FieldName="CUSTID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="客戶名稱" FieldName="CORPCNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="客戶簡稱" FieldName="CORPSHORTNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="客戶類別" FieldName="CUSTCATEID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="統一編號" FieldName="CORPUNICODE" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="連 絡 人" FieldName="CONNECTPERSON" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="業務人員" FieldName="PERSONID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="電話號碼1" FieldName="TEL1" />
                </Columns>
            </JQClientTools:JQRefval>
            <JQClientTools:JQButton ID="btOpenPRODUCTID" runat="server" Text="產品區間：" OnClick="openMulitWinPRODUCTID" />
            <JQClientTools:JQRefval ID="PRODUCTID1Query" runat="server" RemoteName="gServerDataModuleComm.cmdBASPRODUCT" DisplayMember="PRODUCTID" ValueMember="PRODUCTID" CheckData="false" DialogCenter="true" DialogWidth="700">
                <Columns>
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="產品編號" FieldName="PRODUCTID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="條碼編號" FieldName="SCANCODE" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="產品品名" FieldName="PRODCNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="產品規格" FieldName="PRODSTRUCTURE" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="產品品牌" FieldName="INVOICEPRODNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="產品類別" FieldName="PRODCATEID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="建議售價" FieldName="SALEPRICE" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="計量單位" FieldName="CALCUNIT" />
                </Columns>
            </JQClientTools:JQRefval>
            <JQClientTools:JQLabel ID="lbPRODUCTID2" runat="server" Text="～" />
            <JQClientTools:JQRefval ID="PRODUCTID2Query" runat="server" RemoteName="gServerDataModuleComm.cmdBASPRODUCT" DisplayMember="PRODUCTID" ValueMember="PRODUCTID" CheckData="false" DialogCenter="true" DialogWidth="700">
                <Columns>
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="產品編號" FieldName="PRODUCTID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="條碼編號" FieldName="SCANCODE" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="產品品名" FieldName="PRODCNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="產品規格" FieldName="PRODSTRUCTURE" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="產品品牌" FieldName="INVOICEPRODNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="產品類別" FieldName="PRODCATEID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="建議售價" FieldName="SALEPRICE" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="計量單位" FieldName="CALCUNIT" />
                </Columns>
            </JQClientTools:JQRefval>
            <JQClientTools:JQButton ID="btOpenPERSONID" runat="server" Text="人員區間：" OnClick="openMulitWinPERSONID" />
            <JQClientTools:JQRefval ID="PERSONID1Query" runat="server" RemoteName="gServerDataModuleComm.cmdBASPERSON" DisplayMember="PERSONCNAME" ValueMember="PERSONID" CheckData="false" DialogCenter="true" DialogWidth="700">
                <Columns>
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="員工編號" FieldName="PERSONID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="員工姓名" FieldName="PERSONCNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="英文姓名" FieldName="ENGLISHNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="員工職稱" FieldName="JOBNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="出生日期" FieldName="BIRTHDAY" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="部門編號" FieldName="DEPARTMENTID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="行動電話" FieldName="CELLPHONE" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="到 職 日" FieldName="FIRSTWORKDATE" />
                </Columns>
            </JQClientTools:JQRefval>
            <JQClientTools:JQLabel ID="lbPERSONID2" runat="server" Text="～" />
            <JQClientTools:JQRefval ID="PERSONID2Query" runat="server" RemoteName="gServerDataModuleComm.cmdBASPERSON" DisplayMember="PERSONCNAME" ValueMember="PERSONID" CheckData="false" DialogCenter="true" DialogWidth="700">
                <Columns>
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="員工編號" FieldName="PERSONID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="員工姓名" FieldName="PERSONCNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="英文姓名" FieldName="ENGLISHNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="員工職稱" FieldName="JOBNAME" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="出生日期" FieldName="BIRTHDAY" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="部門編號" FieldName="DEPARTMENTID" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="行動電話" FieldName="CELLPHONE" />
                    <JQClientTools:JQGridColumn Alignment="left" Editor="text" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="%%" ReadOnly="False" Sortable="False" Visible="True" Width="80" Caption="到 職 日" FieldName="FIRSTWORKDATE" />
                </Columns>
            </JQClientTools:JQRefval>
            <JQClientTools:JQLabel ID="lbONHANDDATE1" runat="server" Text="預交日區間：" />
            <JQClientTools:JQDateBox ID="ONHANDDATE1Query" runat="server" />
            <JQClientTools:JQLabel ID="lbONHANDDATE2" runat="server" Text="～" />
            <JQClientTools:JQDateBox ID="ONHANDDATE2Query" runat="server" />

            <JQClientTools:JQButton ID="btMultiQuery" runat="server" Text="查詢" OnClick="multiQuery" />
            <JQClientTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="srOPO.VIEW_RPT_OPOR11" runat="server" AutoApply="True" DataMember="VIEW_RPT_OPOR11" Pagination="True" QueryTitle="查詢" Title="已訂購未出貨明細表" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="window" AlwaysClose="true" AllowAdd="False" ViewCommandVisible="False" ReportFileName="~/gOPO/RS_REPORT/OPOR11_01.rdlc" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" TotalCaption="Total:" UpdateCommandVisible="False" OnLoadSuccess="dataGridMasterLoadSuccess">
                <Columns>
                    <JQClientTools:JQGridColumn Alignment="left" Caption="客戶編號" Editor="text" FieldName="IDNO" Format="" MaxLength="0" Sortable="True" Width="70" />
                    <JQClientTools:JQGridColumn Alignment="left" Caption="公司全稱" Editor="text" FieldName="CORPCNAME" Format="" MaxLength="0" Sortable="True" Width="90" />
                    <JQClientTools:JQGridColumn Alignment="left" Caption="日期" Editor="text" FieldName="BILLDATE" Format="yyyy-mm-dd" MaxLength="0" Sortable="True" Width="70" />
                    <JQClientTools:JQGridColumn Alignment="left" Caption="單號" Editor="text" FieldName="BILLNO" Format="" MaxLength="0" Sortable="True" Width="75" FormatScript="Hyperlink" />
                    <JQClientTools:JQGridColumn Alignment="left" Caption="產品編號" Editor="text" FieldName="PRODUCTID" Format="" MaxLength="0" Sortable="True" Width="90" />
                    <JQClientTools:JQGridColumn Alignment="left" Caption="品名規格" Editor="text" FieldName="PRODCNAME" Format="" MaxLength="0" Sortable="True" Width="200" />
                    <JQClientTools:JQGridColumn Alignment="left" Caption="規格" Editor="text" FieldName="PRODSTRUCTURE" Format="" MaxLength="0" Sortable="True" Width="70" />
                    <JQClientTools:JQGridColumn Alignment="left" Caption="人員編號" Editor="text" FieldName="PERSONID" Format="" MaxLength="0" Sortable="True" Width="70" />
                    <JQClientTools:JQGridColumn Alignment="left" Caption="業務員" Editor="text" FieldName="PERSONCNAME" Format="" MaxLength="0" Sortable="True" Width="70" />
                    <JQClientTools:JQGridColumn Alignment="left" Caption="預交貨日" Editor="text" FieldName="ONHANDDATE" Format="yyyy-mm-dd" MaxLength="0" Sortable="True" Width="70" />
                    <JQClientTools:JQGridColumn Alignment="left" Caption="幣別" Editor="text" FieldName="CURRENCYID" Format="" MaxLength="0" Sortable="True" Width="70" />
                    <JQClientTools:JQGridColumn Alignment="right" Caption="匯率" Editor="text" FieldName="CURRRATE" Format="N" MaxLength="0" Sortable="True" Width="70" />
                    <JQClientTools:JQGridColumn Alignment="right" Caption="單價" Editor="text" FieldName="PRICE" Format="N" MaxLength="0" Sortable="True" Width="70" />
                    <JQClientTools:JQGridColumn Alignment="right" Caption="原幣金額" Editor="text" FieldName="SUBAMOUNT" Format="N" MaxLength="0" Sortable="True" Width="70" />
                    <JQClientTools:JQGridColumn Alignment="right" Caption="數量" Editor="text" FieldName="QUANTITY" Format="N" MaxLength="0" Sortable="True" Width="70" />
                    <JQClientTools:JQGridColumn Alignment="right" Caption="未出數量" Editor="text" FieldName="NOTOUTQTY" Format="N" MaxLength="0" Sortable="True" Width="70" />
                    <JQClientTools:JQGridColumn Alignment="right" Caption="唯一序號" Editor="text" FieldName="UNIQUENO" Format="N" MaxLength="0" Sortable="True" Width="70" />

                </Columns>
                <TooItems>
                    <JQClientTools:JQToolItem Icon="icon-print" ItemType="easyui-linkbutton" OnClick="uShowTempSelectDiv" Text="列印" Visible="True" />
                    <JQClientTools:JQToolItem Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportExcel" Text="Excel" Visible="True" />
                    <JQClientTools:JQToolItem Icon="icon-pivottable" ItemType="easyui-linkbutton" OnClick="openPivotDialog" Text="樞紐分析" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQClientTools:JQDataGrid>
            <JQClientTools:JQMultiLanguage ID="JQMultiLanguage1" runat="server" DBAlias="gGexDemo" />
            <JQClientTools:JQLabel ID="lbMultiLanguage" runat="server" Text="" ForeColor="Yellow" />
        </div>
        <div id="winAccessUser"></div>
    </form>
</body>
</html>
