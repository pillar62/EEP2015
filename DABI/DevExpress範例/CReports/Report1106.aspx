<%@ Page Title="門市維修機盤點表" Language="C#" MasterPageFile="~/JASONMaster.master" AutoEventWireup="true"
    CodeFile="Report1106.aspx.cs" Inherits="Reports_Report1106" Trace="False" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/CalendarExtension_Cal_Theme.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/ReportConditionStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        //民國年轉換
        function Conversion_TextBox1() {
            document.getElementById('<%=txtSDATE.ClientID %>').value = document.getElementById('<%=txtSDATE.ClientID %>').value.substr(1, 3) + document.getElementById('<%=txtSDATE.ClientID %>').value.substr(4, 6);
        }
        function Conversion_TextBox2() {
            document.getElementById('<%=txtEDATE.ClientID %>').value = document.getElementById('<%=txtEDATE.ClientID %>').value.substr(1, 3) + document.getElementById('<%=txtEDATE.ClientID %>').value.substr(4, 6);
        }
        function exportExcel(divID) {

            document.getElementById(divID).style.height = "";
            var html = '<!DOCTYPE html><html><head><meta http-equiv="content-type" content="application/vnd.ms-excel; charset=UTF-8" /><title>銷貨產品統計表</title></head>';
            html += '<body>';
            html += document.getElementById('PrintDiv').innerHTML + '</body></html>';
            window.open('data:application/vnd.ms-excel,' + encodeURIComponent(html));

            document.getElementById(divID).style.height = "100px";
        }
    </script>
    <style type="text/css">
        .style2
        {
            text-align: right;
        }
        .style5
        {
            text-align: left;
            color: #003366;
        }
        .style6
        {
            width: 5px;
        }
        .style7
        {
            height: 30px;
            text-align: right;
            background-color: #D8D8D8;
            color: #003366;
            width: 165px;
        }
        .style8
        {
            height: 2px;
            text-align: right;
            background-color: #D8D8D8;
            color: #003366;
            width: 156px;
        }
        .style9
        {
            height: 30px;
            width: 156px;
            text-align: right;
            background-color: #D8D8D8;
            color: #003366;
        }
        .style10
        {
            width: 125px;
            text-align: right;
        }
        .style11
        {
            width: 125px;
        }
        .style12
        {
            width: 31px;
            text-align: center;
        }
        .style13
        {
            height: 36px;
        }
        </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table class="baseTable">
        <tr>
            <td colspan="3" class="style13">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="right" width="50px" bgcolor="#FF71AD" style="border-left-style: solid;
                            border-left-width: 1px; border-bottom-style: solid; border-bottom-width: 1px;
                            border-bottom-color: #999999; border-left-color: #999999" class="style2">
                            <asp:label id="Label33" runat="server" font-names="微軟正黑體" font-size="Medium" forecolor="#000066"
                                text="公告："></asp:label>
                        </td>
                        <td align="left" valign="middle" bgcolor="#FF71AD" style="border-bottom-style: solid;
                            border-bottom-width: 1px; border-bottom-color: #999999; border-right-style: solid;
                            border-right-width: 1px; border-right-color: #999999; font-family: 微軟正黑體;">
                            <marquee style="width: 100%"><font color="#000066" size="4"><asp:Label ID="MARQUEELabel" runat="server" Text="Label"></asp:Label></font></marquee>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table bgcolor="#D8D8D8" border="0" cellpadding="0" cellspacing="2" class="tableCondition">
                    <caption class="title">
                        <strong>門市維修機盤點表</strong></caption>
                    <tr>
                        <td style="text-align: right; " class="ConditionLabel">
                            門市代號：
                        </td>
                        <td class="Conditiontext_2C_L">
                            <dx:ASPxComboBox ID="cmbSCUNO" runat="server" DataSourceID="CustEntityDataSource1"
                                Font-Size="Medium" Height="25px" TextField="MCMCU2" Theme="Metropolis" ValueField="MCMCU"
                                Width="100%" NullText="選擇門市">
                                <ClearButton Text="清除" Visibility="True">
                                    <Image AlternateText="✔">
                                    </Image>
                                </ClearButton>
                            </dx:ASPxComboBox>
                        </td>
                        <td class="Conditiontext_to">
                            至
                        </td>
                        <td class="Conditiontext_2C_R">
                            <dx:ASPxComboBox ID="cmbECUNO" runat="server" DataSourceID="CustEntityDataSource1"
                                Font-Size="Medium" Height="25px" TextField="MCMCU2" Theme="Metropolis" ValueField="MCMCU"
                                Width="100%" NullText="選擇門市">
                                <ClearButton Text="清除" Visibility="True">
                                </ClearButton>
                            </dx:ASPxComboBox>
                        </td>
                        <td class="ConditionLabel">
                            門市多選：</td>
                        <td align="left"  class="Condition3Column2One">
                            <asp:ImageButton ID="IB_MultiCUS" runat="server" CommandArgument='<%# Eval("SRNO") %>'
                                Height="25px" ImageUrl="~/IMAGES/SearchBlue.png" OnClick="IB_MultiCUS_Click" />
                            <asp:Label ID="lblMultiCUNOCount" runat="server"></asp:Label>
                            <asp:Label ID="lblMultiCUNO" runat="server" Visible="False"></asp:Label>
                            </td>
                        <td class="LastTD">
                            <img height="25" name="B1" onclick="exportExcel('PrintDiv');" 
                                src="../IMAGES/EXPORTExcelIcon.png" /></td>
                    </tr>
                    <tr>
                        <td class="ConditionLabel">
                            維修日：
                        </td>
                        <td class="Conditiontext_2C_L">
                            <asp:textbox id="txtSDATE" runat="server" height="25px" width="100%" 
                                Font-Size="Medium"></asp:textbox>
                            <asp:calendarextender id="txtSDATE_CalendarExtender" runat="server" cleartime="True"
                                cssclass="Cal_Theme" format="yyyy/MM/dd" targetcontrolid="txtSDATE" 
                                onclientdateselectionchanged="Conversion_TextBox1">
                            </asp:calendarextender>
                        </td>
                        <td class="Conditiontext_to">
                            &nbsp;</td>
                        <td class="Conditiontext_2C_R">
                            <asp:textbox id="txtEDATE" runat="server" height="25px" width="100%" 
                                Font-Size="Medium" Visible="False"></asp:textbox>
                            <asp:calendarextender id="txtEDATE_CalendarExtender" runat="server" cleartime="True"
                                cssclass="Cal_Theme" format="yyyy/MM/dd" targetcontrolid="txtEDATE" 
                                onclientdateselectionchanged="Conversion_TextBox2">
                            </asp:calendarextender>
                        </td>
                        <td class="ConditionLabel">
                            產品多選：</td>
                        <td align="left" class="Condition3Column2One">
                            <asp:ImageButton ID="IB_MultiPROD" runat="server" CommandArgument='<%# Eval("SRNO") %>'
                                Height="25px" ImageUrl="~/IMAGES/SearchBlue.png" 
                                onclick="IB_MultiPROD_Click" />
                            <asp:Label ID="lblMultiPRODCount" runat="server"></asp:Label>
                            <asp:Label ID="lblMultiPROD" runat="server" Visible="False"></asp:Label>
                            </td>
                        <td class="LastTD">
                            <dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" Text="查詢"
                                Width="100px" Font-Size="Medium" Theme="Metropolis" Height="25px">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                    </table>
            </td>
            <td align="center">
                &nbsp;
            </td>
            <td align="center" class="style6">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Theme="Mulberry" 
                    DocumentViewerInternal="" 
                    oncachereportdocument="ASPxDocumentViewer1_CacheReportDocument" 
                    
                    onrestorereportdocumentfromcache="ASPxDocumentViewer1_RestoreReportDocumentFromCache" 
                    ClientInstanceName="aspxViewer1">
                    <SettingsReportViewer ShouldDisposeReport="False" />
                    <SettingsLoadingPanel ImagePosition="Right" />
                    <StylesToolbar Alignment="Center">
                    </StylesToolbar>
                    <ToolbarItems>
                        <dx:ReportToolbarButton ItemKind="Search" Text="尋找" />
                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton ItemKind="PrintReport" Text="列印" />
                        <dx:ReportToolbarButton ItemKind="PrintPage" Text="列印本頁" />
                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" Text="首頁" />
                        <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" Text="上頁" />
                        <dx:ReportToolbarLabel ItemKind="PageLabel" Text="頁次" />
                        <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                        </dx:ReportToolbarComboBox>
                        <dx:ReportToolbarLabel ItemKind="OfLabel" Text="／" />
                        <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                        <dx:ReportToolbarButton ItemKind="NextPage" Text="下頁" />
                        <dx:ReportToolbarButton ItemKind="LastPage" Text="末頁" />
                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton ItemKind="SaveToDisk" Text="輸出檔案" />
                        <dx:ReportToolbarButton ItemKind="SaveToWindow" Text="新視窗瀏覽" />
                        <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                            <Elements>
                                <dx:ListElement Value="pdf" />
                                <dx:ListElement Value="xls" />
                                <dx:ListElement Value="xlsx" />
                                <dx:ListElement Value="rtf" />
                                <dx:ListElement Value="mht" />
                                <dx:ListElement Value="html" />
                                <dx:ListElement Value="txt" />
                                <dx:ListElement Value="csv" />
                                <dx:ListElement Value="png" />
                            </Elements>
                        </dx:ReportToolbarComboBox>
                    </ToolbarItems>
                    <ClientSideEvents Init="function(s, e) {
	aspxViewer1.GotoPage(0);

}" />
                </dx:ASPxDocumentViewer>
            </td>
        </tr>
    </table>

    
    <%--開窗(門市多選)linkbutton--%>
    <asp:LinkButton ID="lbtnMultiCus" runat="server" Style="display: none;"></asp:LinkButton>
    <%--開窗(門市多選)ajax元件--%>
    <asp:ModalPopupExtender ID="mpeMultiCus" runat="server" TargetControlID="lbtnMultiCus"
        PopupControlID="pnl_MultiCus" BehaviorID="MyMPEBehaviorCus" BackgroundCssClass="modalBackground"
        DropShadow="false">
    </asp:ModalPopupExtender>
    <%--開窗(門市多選)Panel--%>
    <asp:Panel ID="pnl_MultiCus" runat="server" DefaultButton="ibPopSearchWinCus">
        <table bgcolor="White" cellpadding="5" cellspacing="0" style="border: medium solid #BF0052;
            font-family: 微軟正黑體; font-size: small;">
            <tr>
                <td class="popupMutiSelectTD">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="搜尋條件："></asp:Label>
                                <asp:TextBox ID="tbBaseSearch1Cus" runat="server" autocomplete="off" AutoCompleteType="None"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="tbBaseSearch2Cus" runat="server" Width="128px" autocomplete="off"
                                    AutoCompleteType="None"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="tbBaseSearch3Cus" runat="server" autocomplete="off" AutoCompleteType="None"></asp:TextBox>
                            </td>
                            <td class="style5">
                                <asp:ImageButton ID="ibPopSearchWinCus" runat="server" ImageUrl="~/IMAGES/SearchBlue.png"
                                    Height="25px" OnClick="ibPopSearchWinCus_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblParams1Cus" runat="server" Visible="False"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                                <asp:Label ID="lblParams2Cus" runat="server" Visible="False"></asp:Label>
                            </td>
                            <td style="width: 25px">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" class="popupMutiSelectTD">
                    <%--開窗GridView--%>
                    <asp:GridView ID="gdvSearchCus" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                        CellPadding="3" DataSourceID="CustEntityDataSource1" Width="100%" OnPageIndexChanged="gdvSearchCus_PageIndexChanged"
                        OnRowDataBound="gdvSearchCus_RowDataBound" DataKeyNames="MEMCU2">
                        <Columns>
                            <asp:TemplateField HeaderText="勾選">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkBoxCus" runat="server" OnCheckedChanged="chkBoxCus_CheckedChanged" />
                                    <asp:Label ID="lblLinkCus" runat="server" Text='<%# Eval("MCMCU") %>' Visible="False"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MEMCU2" HeaderText="MEMCU2" ReadOnly="True" SortExpression="MEMCU2"
                                Visible="False" />
                            <asp:BoundField DataField="MCMCU" HeaderText="客戶代號" SortExpression="MCMCU" />
                            <asp:BoundField DataField="MCMCU2" HeaderText="MCMCU2" SortExpression="MCMCU2" Visible="False" />
                            <asp:BoundField DataField="MCDL01" HeaderText="客戶名稱" SortExpression="MCDL01" />
                            <asp:BoundField DataField="MCSTYL" HeaderText="MCSTYL" SortExpression="MCSTYL" Visible="False" />
                            <asp:BoundField DataField="MCDL02" HeaderText="MCDL02" SortExpression="MCDL02" Visible="False" />
                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <HeaderStyle BackColor="#7D0035" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                    </asp:GridView>
                    <asp:entitydatasource ID="CustEntityDataSource1" runat="server" 
                        connectionstring="name=ACTIONEntity" defaultcontainername="ACTIONEntity" 
                        enableflattening="False" entitysetname="VIEW_F0006" 
                        onquerycreated="CustEntityDataSource1_QueryCreated" orderby="it.[MCMCU]" 
                        where="">
                    </asp:entitydatasource>
                    <%--開窗的資料集--%>
                </td>
            </tr>
            <tr>
                <td align="center" class="style15">
                    <asp:Button ID="btnMultiSelectConfirmCus" runat="server" BackColor="#003366" BorderColor="#003366"
                        BorderStyle="Double" BorderWidth="3px" Font-Names="微軟正黑體" Font-Size="Small" ForeColor="White"
                        OnClick="btnMultiSelectConfirmCus_Click" Text="確認" />
                    &nbsp;
                    <asp:Button ID="btnCancelCus" runat="server" Text="關閉" BackColor="#C4003A" BorderColor="#C4003A"
                        BorderStyle="Double" BorderWidth="3px" Font-Names="微軟正黑體" 
                        Font-Size="Small" ForeColor="White"
                        OnClick="btnCancelCus_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>

        <%--開窗(產品多選)linkbutton--%>
    <asp:LinkButton ID="lbtn_MultiProd" runat="server" Style="display: none;"></asp:LinkButton>
    <%--開窗(產品多選)ajax元件--%>
    <asp:ModalPopupExtender ID="mpeMultiProd" runat="server" TargetControlID="lbtn_MultiProd"
        PopupControlID="pnl_MultiProd" BehaviorID="MyMPEBehaviorProd" BackgroundCssClass="modalBackground"
        DropShadow="false">
    </asp:ModalPopupExtender>
    <%--開窗(產品多選)Panel--%>
    <asp:Panel ID="pnl_MultiProd" runat="server" DefaultButton="ibPopSearchWinProd">
        <table bgcolor="White" cellpadding="5" cellspacing="0" style="border: medium solid #BF0052;
            font-family: 微軟正黑體; font-size: small;">
            <tr>
                <td class="popupMutiSelectTD">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="搜尋條件："></asp:Label>
                                <asp:TextBox ID="tbBaseSearch1Prod" runat="server" autocomplete="off" AutoCompleteType="None"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="tbBaseSearch2Prod" runat="server" Width="128px" autocomplete="off"
                                    AutoCompleteType="None"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="tbBaseSearch3Prod" runat="server" autocomplete="off" AutoCompleteType="None"></asp:TextBox>
                            </td>
                            <td class="style5">
                                <asp:ImageButton ID="ibPopSearchWinProd" runat="server" ImageUrl="~/IMAGES/SearchBlue.png"
                                    Height="25px" OnClick="ibPopSearchWinProd_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblParams1Prod" runat="server" Visible="False"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                                <asp:Label ID="lblParams2Prod" runat="server" Visible="False"></asp:Label>
                            </td>
                            <td style="width: 25px">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" class="popupMutiSelectTD">
                    <%--開窗GridView--%>
                    <asp:GridView ID="gdvSearchProd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                        CellPadding="3" DataSourceID="PRODEntityDataSource1" Width="100%" OnPageIndexChanged="gdvSearchProd_PageIndexChanged"
                        OnRowDataBound="gdvSearchProd_RowDataBound" DataKeyNames="IMITM">
                        <Columns>
                            <asp:TemplateField HeaderText="勾選">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkBoxProd" runat="server" 
                                        oncheckedchanged="chkBoxProd_CheckedChanged" />
                                    <asp:Label ID="lblLinkProd" runat="server" Text='<%# Eval("IMLITM") %>' 
                                        Visible="False"></asp:Label>
                                </ItemTemplate>
                                <ControlStyle Width="25px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="IMITM" HeaderText="IMITM" ReadOnly="True" 
                                SortExpression="IMITM" Visible="False" />
                            <asp:BoundField DataField="IMLITM" HeaderText="產品編號" SortExpression="IMLITM" />
                            <asp:BoundField DataField="IMDSC1" HeaderText="產品品名" SortExpression="IMDSC1" />
                            <asp:BoundField DataField="IMLITM2" HeaderText="IMLITM2" 
                                SortExpression="IMLITM2" Visible="False" />
                            <asp:BoundField DataField="IMSRP1" HeaderText="IMSRP1" SortExpression="IMSRP1" 
                                Visible="False" />
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:CheckBox ID="chBoxSelectAllArea" runat="server" 
                                OnCheckedChanged="chBoxSelectAllPord_CheckedChanged" />
                        </EmptyDataTemplate>
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <HeaderStyle BackColor="#7D0035" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                    </asp:GridView>
                    <%--開窗的資料集--%>
                    <asp:entitydatasource ID="PRODEntityDataSource1" runat="server" 
                        connectionstring="name=ReportViewEntities" 
                        defaultcontainername="ReportViewEntities" enableflattening="False" 
                        entitysetname="VIEW_F4101" onquerycreated="PRODEntityDataSource1_QueryCreated" 
                        orderby="it.[IMLITM]">
                    </asp:entitydatasource>
                    
                </td>
            </tr>
            <tr>
                <td align="center" class="style15">
                    <asp:Button ID="Button1" runat="server" BackColor="#003366" BorderColor="#003366"
                        BorderStyle="Double" BorderWidth="3px" Font-Names="微軟正黑體" 
                        Font-Size="Small" ForeColor="White"
                        OnClick="btnMultiSelectConfirmProd_Click" Text="確認" />
                    &nbsp;
                    <asp:Button ID="Button2" runat="server" Text="關閉" BackColor="#C4003A" BorderColor="#C4003A"
                        BorderStyle="Double" BorderWidth="3px" Font-Names="微軟正黑體" 
                        Font-Size="Small" ForeColor="White"
                        OnClick="btnCancelProd_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <div id="PrintDiv" style="visibility: hidden; position: absolute; overflow: hidden; height: 1px;">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="OFFNO" HeaderText="門市代號" />
                <asp:BoundField DataField="OFFNM" HeaderText="門市名稱" />
                <asp:BoundField DataField="FIXDATE" HeaderText="維修日期" />
                <asp:BoundField DataField="FIXNO" HeaderText="維修單號" />
                <asp:BoundField DataField="PRODNM" HeaderText="產品名稱" />
                <asp:BoundField DataField="IMEI1" HeaderText="IMEI" />
                <asp:BoundField DataField="SN" HeaderText="SN" />
                <asp:BoundField DataField="KIND1NM" HeaderText="庫存" />
                <asp:BoundField DataField="ITEM1NM" HeaderText="收件項目" />
                <asp:BoundField DataField="STATUS1NM" HeaderText="進度" />
                <asp:BoundField DataField="CUSTNA" HeaderText="客戶姓名" />
                <asp:BoundField DataField="TEL1" HeaderText="客戶電話" />
                <asp:BoundField DataField="SUPCUSNM" HeaderText="廠商名稱" />
                <asp:BoundField DataField="RMANNM" HeaderText="收件人員" />
                <asp:BoundField DataField="" HeaderText="備用機" />
            </Columns>
        </asp:GridView>
    </div>      
</asp:Content>
