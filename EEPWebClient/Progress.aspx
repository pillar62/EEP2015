<%@ Page language="c#" Inherits="WebProgressBar.Progress" CodeFile="Progress.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Progress</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<base target="_self">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<asp:Label id="lblMessages" runat="server"></asp:Label>
			<asp:Panel id="panelBarSide" runat="server" Width="300px" BorderStyle="Solid" BorderWidth="1px"
				ForeColor="Silver">
				<asp:Panel id="panelProgress" runat="server" Width="10px" BackColor="Green"></asp:Panel>
			</asp:Panel>
			<asp:Label id="lblPercent" runat="server" ForeColor="Blue"></asp:Label>
		</form>
	</body>
</HTML>
