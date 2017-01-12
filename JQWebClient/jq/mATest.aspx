<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mATest.aspx.cs"
    Inherits="Template_JQMobileSingle1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <JQMobileTools:JQScriptManager ID="JQScriptManager1" runat="server" JQueryMobileVersion="1.3.2" />
        <JQMobileTools:JQDataGrid ID="dataGridView" runat="server" RenderFooter="False"
            RenderHeader="True" EditFormID="dataFormMaster" DataMember="ATest" RemoteName="SCustomers.ATest" Title="mATest">
            <Columns>
                <JQMobileTools:JQGridColumn Alignment="left" Caption="ATestID" FieldName="ATestID" Format="" Width="120" />
                <JQMobileTools:JQGridColumn Alignment="left" Caption="AName" FieldName="AName" Format="" Width="120" />
                <JQMobileTools:JQGridColumn Alignment="left" Caption="ADateTime" FieldName="ADateTime" Format="" Width="120" />
                <JQMobileTools:JQGridColumn Alignment="left" Caption="AArea" FieldName="AArea" Format="" Width="120" />
                <JQMobileTools:JQGridColumn Alignment="left" Caption="ABit" FieldName="ABit" Format="" Width="120" />
            </Columns>
            <ToolItems>
                <JQMobileTools:JQToolItem Icon="plus" Name="grid-insert" Text="Insert" Visible="True" />
                <JQMobileTools:JQToolItem Icon="arrow-l" Name="grid-previous" Text="Previous page" Visible="True" />
                <JQMobileTools:JQToolItem Icon="arrow-r" Name="grid-next" Text="Next page" Visible="True" />
                <JQMobileTools:JQToolItem Icon="search" Name="grid-query" Text="Query" Visible="True" />
                <JQMobileTools:JQToolItem Icon="refresh" Name="grid-refresh" Text="Refresh" Visible="True" />
                <JQMobileTools:JQToolItem Icon="back" Name="grid-return" Text="Back" Visible="True" />
            </ToolItems>
            <QueryColumns>
            </QueryColumns>
        </JQMobileTools:JQDataGrid>
        <JQMobileTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="ATest" RemoteName="SCustomers.ATest" Title="mATest" AppliedClose="True" AutoPause="False" AutoSubmit="False" DuplicateCheck="False" IsNotifyOFF="False" IsShowFlowIcon="False" Theme="b">
            <Columns>
                <JQMobileTools:JQFormColumn Caption="ATestID" Editor="text" EditorOptions="{}" FieldName="ATestID" Width="120" />
                <JQMobileTools:JQFormColumn Caption="AName" Editor="refval" EditorOptions="{RemoteName:'SCustomers.Customers',DisplayMember:'CompanyName',ValueMember:'CustomerID',Columns:[],WhereItems:[],ColumnMatches:[],QueryColumns:[],PageSize:20,DialogTitle:'Select Item',DialogWidth:250,CacheMode:None}" FieldName="AName" Width="120" />
                <JQMobileTools:JQFormColumn Caption="ADateTime" Editor="text" EditorOptions="{}" FieldName="ADateTime" Width="120" />
                <JQMobileTools:JQFormColumn Caption="AArea" Editor="text" EditorOptions="{}" FieldName="AArea" Width="120" />
                <JQMobileTools:JQFormColumn Caption="ABit" Editor="text" EditorOptions="{}" FieldName="ABit" Width="120" />
            </Columns>
        </JQMobileTools:JQDataForm>
    <JQMobileTools:JQDefault runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" Enabled="True" EnableTheming="True" ClientIDMode="Inherit" ID="defaultMaster" EnableViewState="True" ViewStateMode="Inherit" >
</JQMobileTools:JQDefault>
<JQMobileTools:JQValidate runat="server" BindingObjectID="dataFormMaster" DuplicateCheck="False" BorderStyle="NotSet" Enabled="True" EnableTheming="True" ClientIDMode="Inherit" ID="validateMaster" EnableViewState="True" ViewStateMode="Inherit" >
</JQMobileTools:JQValidate>
</form>
</body>
</html>
