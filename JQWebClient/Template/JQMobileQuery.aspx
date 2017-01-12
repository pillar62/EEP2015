<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JQMobileQuery.aspx.cs" Inherits="Template_JQMobileSingle1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <JQMobileTools:JQScriptManager ID="JQScriptManager1" runat="server" />
        <JQMobileTools:JQDataGrid ID="dataGridMaster" runat="server" RenderFooter="False" RenderHeader="True" EditFormID="dataFormMaster">
            <Columns>
            </Columns>
            <ToolItems>
            </ToolItems>
            <QueryColumns>
            </QueryColumns>
        </JQMobileTools:JQDataGrid>

    </form>
</body>
</html>
