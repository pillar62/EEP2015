<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT901.aspx.cs" Inherits="CBBN_RT901" %>

<%@ Register assembly="DevExpress.Web.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="UTF-8">
	<title>社區用戶查詢</title>
	<link rel="stylesheet" type="text/css" href="../js/themes/default/easyui.css">
	<link rel="stylesheet" type="text/css" href="../js/themes/icon.css">
	<script type="text/javascript" src="../js/jquery.min.js"></script>
	<script type="text/javascript" src="../js/jquery.easyui.min.js"></script>
	<%--自定義--%>
	<link href="../css/DabiStyles.css" rel="stylesheet" />
	<link href="../css/DabiReportConditionStyle.css" rel="stylesheet" />
	<style type="text/css">
		.auto-style1 {
			width: 80px;
		}

.dxeTextBoxDefaultWidthSys,
.dxeButtonEditSys 
{
	width: 170px;
}

.dxeTextBoxSys, 
.dxeMemoSys 
{
	border-collapse:separate!important;
}

.dxeTrackBar, 
.dxeIRadioButton, 
.dxeButtonEdit, 
.dxeTextBox, 
.dxeRadioButtonList, 
.dxeCheckBoxList, 
.dxeMemo, 
.dxeListBox, 
.dxeCalendar, 
.dxeColorTable
{
	-webkit-tap-highlight-color: rgba(0,0,0,0);
}

.dxeTextBox,
.dxeButtonEdit,
.dxeIRadioButton,
.dxeRadioButtonList,
.dxeCheckBoxList
{
	cursor: default;
}

.dxeTextBox
{
	background-color: white;
	border: 1px solid #9f9f9f;
	font: 12px Tahoma, Geneva, sans-serif;
}

.dxeMemoEditAreaSys, /*Bootstrap correction*/
input[type="text"].dxeEditAreaSys, /*Bootstrap correction*/
input[type="password"].dxeEditAreaSys /*Bootstrap correction*/
{
	display: block;
	-webkit-box-shadow: none;
	-moz-box-shadow: none;
	box-shadow: none;
	-webkit-transition: none;
	-moz-transition: none;
	-o-transition: none;
	transition: none;
	-webkit-border-radius: 0px;
	-moz-border-radius: 0px;
	border-radius: 0px;
}
.dxeEditAreaSys,
.dxeMemoEditAreaSys, /*Bootstrap correction*/
input[type="text"].dxeEditAreaSys, /*Bootstrap correction*/
input[type="password"].dxeEditAreaSys /*Bootstrap correction*/
{
	font: inherit;
	line-height: normal;
	outline: 0;
}

input[type="text"].dxeEditAreaSys, /*Bootstrap correction*/
input[type="password"].dxeEditAreaSys /*Bootstrap correction*/
{
	margin-top: 0;
	margin-bottom: 0;
}
.dxeEditAreaSys,
input[type="text"].dxeEditAreaSys, /*Bootstrap correction*/
input[type="password"].dxeEditAreaSys /*Bootstrap correction*/
{
	padding: 0px 1px 0px 0px; /* B146658 */
}

.dxeTextBox .dxeEditArea
{
	background-color: white;
}
.dxeEditAreaSys 
{
	border: 0px!important;
	background-position: 0 0; /* iOS Safari */
	-webkit-box-sizing: content-box; /*Bootstrap correction*/
	-moz-box-sizing: content-box; /*Bootstrap correction*/
	box-sizing: content-box; /*Bootstrap correction*/
}

.dxeEditArea
{
	border: 1px solid #A0A0A0;
}

input{
	text-transform:uppercase;
}

		.auto-style2 {
			width: 100%;
			overflow: hidden;
			padding-left: 3px;
			padding-right: 3px;
			padding-top: 3px;
			padding-bottom: 2px;
		}
		.auto-style3 {
			width: 100%;
		}
		.auto-style4 {
			border-width: 0px;
			padding: 0px 19px;
		}
	</style>
</head>
<body>
		<script type="text/javascript">
		function ShowProgressBar() {

		}
	</script>
<form name="form1" method="post" id="form1" runat="server">
		<div id="query" class="easyui-panel" data-options="iconCls:'icon-search',closed:false,collapsible:true,maximizable:false,minimizable:false" title="社區用戶匯出" style="padding: 10px">
			<table>
<%--				<tr >
					<td class="auto-style1">訂單編號：
					</td>
					<td class="Conditiontext_2C_L">
						<dx:ASPxTextBox ID="txtSSHNO" runat="server" Style="margin-bottom: 0px"
							Width="100%">
							
						</dx:ASPxTextBox>
					</td>
					<td>至
					</td>
					<td class="Conditiontext_2C_R">
						<dx:ASPxTextBox ID="txtESHNO" runat="server" Style="margin-bottom: 0px"
							Width="100%">
						</dx:ASPxTextBox>
					</td>
					<td>
						<dx:ASPxButton ID="aspxBtnSearch" runat="server" Font-Size="Small" Height="20px" OnClick="aspxBtnSearch_Click" Text="查詢" Theme="iOS" Width="80%">
							<ClientSideEvents Click="function(s, e) {return ShowProgressBar();}" />
						</dx:ASPxButton>
					</td>
				</tr>--%>
				<tr>
					<td class="auto-style1">
						<dx:ASPxButton ID="aspxbtnExportExcel" runat="server" Font-Size="Small" OnClick="ASPxButton2_Click" Text="匯出Excel" Theme="iOS" Width="100%">
						</dx:ASPxButton>
					</td>
				</tr>
			</table>
		</div>

		<dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="ASPxGridView1">
		</dx:ASPxGridViewExporter>

		<div class="easyui-panel" style="height: 100%; padding: 5px;">

			<dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" EnableTheming="True" Theme="Office2010Silver" Font-Size="Small">
				<SettingsPager Position="Top" NumericButtonCount="20" PageSize="20">
				</SettingsPager>
				<Settings ShowGroupPanel="True" ShowTitlePanel="True" GroupFormat="{1} {2}" />
				<SettingsBehavior AutoExpandAllGroups="True" ColumnResizeMode="Control" SortMode="DisplayText" AllowFixedGroups="True" />
				<SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
				<Columns>
					<dx:GridViewDataTextColumn FieldName="COMN" VisibleIndex="0" Caption="社區名稱" Width="120px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="CUSNC" VisibleIndex="1" Caption="客戶名" Width="100px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="CUTNC" VisibleIndex="2" Caption="縣市" Width="60px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="TOWNSHIP2" VisibleIndex="4" Caption="鄉鎮市" Width="60px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="RADDR2" VisibleIndex="5" Caption="裝機地址" Width="180px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="CONTACTTEL" VisibleIndex="6" Caption="室內電話" Width="120px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="MOBILE" VisibleIndex="7" Caption="連絡手機" Width="120px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="PROJNM" VisibleIndex="8" Caption="方案別" Width="90px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="PROJ2NM" VisibleIndex="9" Caption="資費" Width="90px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="IP11" VisibleIndex="10" Caption="IP" Width="90px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn FieldName="APPLYDAT" VisibleIndex="11" Caption="用戶申請日" Width="100px">
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn FieldName="FINISHDAT" VisibleIndex="12" Caption="完工日" Width="90px">
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn FieldName="DOCKETDAT" VisibleIndex="13" Caption="報竣日" Width="90px">
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn FieldName="STRBILLINGDAT" VisibleIndex="14" Caption="開始計費日" Width="90px">
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn FieldName="NEWBILLINGDAT" VisibleIndex="15" Caption="最近續約日" Width="90px">
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn FieldName="DUEDAT" VisibleIndex="16" Caption="到期日" Width="90px">
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn FieldName="FREECODE" VisibleIndex="17" Caption="公關戶" Width="50px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn FieldName="DROPDAT" VisibleIndex="19" Visible="False">
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn FieldName="QT_CC" ReadOnly="True" VisibleIndex="18" Caption="客訴次數" Width="150px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="ADDR" ReadOnly="True" VisibleIndex="3" Visible="False">
					</dx:GridViewDataTextColumn>
				</Columns>
				<GroupSummary>
					<dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="TOTQTY" ShowInGroupFooterColumn="數量" SummaryType="Sum" />
				</GroupSummary>
				<Styles AdaptiveDetailButtonWidth="22">
					<Header Font-Size="Small">
					</Header>
					<GroupRow Font-Size="Small">
					</GroupRow>
					<Cell Font-Size="Small">
					</Cell>
					<GroupFooter Font-Size="Small">
					</GroupFooter>
					<GroupPanel Font-Size="Small">
						<Border BorderStyle="Groove" />
					</GroupPanel>
				</Styles>
			</dx:ASPxGridView>

			<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:RTlibNConnectionString %>" SelectCommand="SELECT COMN, CUSNC, CUTNC, TOWNSHIP2, RADDR2, CONTACTTEL, MOBILE, PROJNM, PROJ2NM, IP11, APPLYDAT, FINISHDAT, DOCKETDAT, STRBILLINGDAT, NEWBILLINGDAT, DUEDAT, FREECODE, DROPDAT, QT_CC, ADDR FROM VIEW_CUSDATA WHERE (1 = 1)"></asp:SqlDataSource>

		</div>
	</form>
</body>
</html>
