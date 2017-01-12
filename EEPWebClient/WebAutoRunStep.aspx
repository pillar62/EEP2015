<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebAutoRunStep.aspx.cs" Inherits="WebAutoRunStep" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="labelUserID" runat="server" Text="UserID:"></asp:Label><asp:TextBox ID="txtUserID" runat="server"></asp:TextBox><asp:Label ID="labelPackageName" runat="server" Text="Package Name:"></asp:Label>
        <asp:TextBox ID="txtPackageName" runat="server"></asp:TextBox><br />
        <asp:Label ID="labelDB" runat="server" Text="Data Base:"></asp:Label><asp:TextBox ID="txtDB" runat="server"></asp:TextBox><asp:Label ID="labelFormName" runat="server" Text="Form Name:"></asp:Label><asp:TextBox ID="txtFormName" runat="server"></asp:TextBox><br />
        <asp:Label ID="labelPassword" runat="server" Text="Password:"></asp:Label><asp:TextBox
            ID="txtPassword" runat="server"></asp:TextBox><asp:Label ID="labelTimes" runat="server" Text="Times:"></asp:Label><asp:TextBox ID="txtTimes" runat="server"></asp:TextBox><br />
        <asp:Label ID="labelSolution" runat="server" Text="Solution:"></asp:Label><asp:TextBox ID="txtSolution" runat="server"></asp:TextBox>
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" /><br />
        &nbsp; &nbsp;<br />
        &nbsp; &nbsp; &nbsp;
        <br />
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
        <br />
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp;&nbsp;
        </div>
    </form>
</body>
</html>
