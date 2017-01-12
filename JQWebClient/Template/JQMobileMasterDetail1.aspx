<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JQMobileMasterDetail1.aspx.cs"
    Inherits="Template_JQMobileMasterDetail1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <JQMobileTools:JQScriptManager ID="JQScriptManager1" runat="server" />
        <JQMobileTools:JQDataGrid ID="dataGridView" runat="server" RenderFooter="False"
            RenderHeader="True" EditFormID="JQTab1" DetailObjectID="dataGridDetail">
            <Columns>
            </Columns>
            <ToolItems>
            </ToolItems>
            <QueryColumns>
            </QueryColumns>
        </JQMobileTools:JQDataGrid>
        <br />
        <br />
        <JQMobileTools:JQTab ID="JQTab1" runat="server" Theme="b" Title="JQTab表單">
            <JQMobileTools:JQTabItem ID="JQTabItem1" runat="server" Title="主檔">
            </JQMobileTools:JQTabItem>
            <JQMobileTools:JQTabItem ID="JQTabItem2" runat="server" Title="明細檔">
            </JQMobileTools:JQTabItem>
        </JQMobileTools:JQTab>
        <p>
        <JQMobileTools:JQDataForm ID="dataFormMaster" runat="server">
            
            </JQMobileTools:JQDataForm>
        <JQMobileTools:JQDataGrid ID="dataGridDetail" runat="server" RenderFooter="False"
            RenderHeader="True" EditFormID="dataFormDetail">
            <Columns>
            </Columns>
            <ToolItems>
                <JQMobileTools:JQToolItem Icon="plus" Name="grid-insert" Text="Insert" Visible="True" />
                <JQMobileTools:JQToolItem Icon="arrow-l" Name="grid-previous" Text="Previous page"
                    Visible="True" />
                <JQMobileTools:JQToolItem Icon="arrow-r" Name="grid-next" Text="Next page" Visible="True" />
                <JQMobileTools:JQToolItem Icon="search" Name="grid-query" Text="Query" Visible="True" />
                <JQMobileTools:JQToolItem Icon="refresh" Name="grid-refresh" Text="Refresh" Visible="True" />
                <JQMobileTools:JQToolItem Icon="back" Name="grid-return" Text="Back" Visible="True" />
            </ToolItems>
            <QueryColumns>
            </QueryColumns>
        </JQMobileTools:JQDataGrid>
        <JQMobileTools:JQDataForm ID="dataFormDetail" runat="server">
            <Columns>
            </Columns>
        </JQMobileTools:JQDataForm>
        </p>
    </form>
</body>
</html>
