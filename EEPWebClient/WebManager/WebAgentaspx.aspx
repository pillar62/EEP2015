<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebAgentaspx.aspx.cs" Inherits="WebManager_WebAgentaspx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Setting Agent</title>
    <link href="../css/MenuUtility.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <InfoLight:WebDataSource ID="WDSAgent" runat="server" DataMember="cmdRoleAgent" WebDataSetID="wdsAgent" AutoApply="True">
        </InfoLight:WebDataSource>
        <InfoLight:WebDataSource ID="WDSUsers" runat="server" DataMember="userInfo" WebDataSetID="wdsUsers">
        </InfoLight:WebDataSource>
        <table cellpadding="1" cellspacing="1" width="100%"><tr><td id="NavigatorContent">
        <InfoLight:WebNavigator ID="WebNavigator1" runat="server" AnyQueryID="WebNavigator1"
            BackColor="PowderBlue" BindingObject="WebGridView1" BorderColor="#E0E0E0" BorderStyle="Groove"
            BorderWidth="2px" ShowDataStyle="GridStyle"
            StatusStrip="WebStatusStrip1"><NavStates>
<InfoLight:WebNavigatorStateItem StateText="Initial"></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Browsed"></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Inserting"></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Editing"></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Applying"></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Changing"></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Querying"></InfoLight:WebNavigatorStateItem>
<InfoLight:WebNavigatorStateItem StateText="Printing"></InfoLight:WebNavigatorStateItem>
</NavStates>
<NavControls>
<InfoLight:ControlItem ControlName="First" ControlText="掸" ControlType="Image" ImageUrl="../image/uipics/first.gif" MouseOverImageUrl="../image/uipics/first2.gif" DisenableImageUrl="../image/uipics/first3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Previous" ControlText="掸" ControlType="Image" ImageUrl="../image/uipics/previous.gif" MouseOverImageUrl="../image/uipics/previous2.gif" DisenableImageUrl="../image/uipics/previous3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Next" ControlText="掸" ControlType="Image" ImageUrl="../image/uipics/next.gif" MouseOverImageUrl="../image/uipics/next2.gif" DisenableImageUrl="../image/uipics/next3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Last" ControlText="ソ掸" ControlType="Image" ImageUrl="../image/uipics/last.gif" MouseOverImageUrl="../image/uipics/last2.gif" DisenableImageUrl="../image/uipics/next3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Add" ControlText="穝糤" ControlType="Image" ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif" DisenableImageUrl="../image/uipics/add3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="OK" ControlText="絋粄" ControlType="Image" ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif" DisenableImageUrl="../image/uipics/ok3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Cancel" ControlText="" ControlType="Image" ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif" DisenableImageUrl="../image/uipics/cancel3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Apply" ControlText="郎" ControlType="Image" ImageUrl="../image/uipics/apply.gif" MouseOverImageUrl="../image/uipics/apply2.gif" DisenableImageUrl="../image/uipics/apply3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Abort" ControlText="斌" ControlType="Image" ImageUrl="../image/uipics/abort.gif" MouseOverImageUrl="../image/uipics/abort2.gif" DisenableImageUrl="../image/uipics/abort3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Query" ControlText="琩高" ControlType="Image" ImageUrl="../image/uipics/query.gif" MouseOverImageUrl="../image/uipics/query2.gif" DisenableImageUrl="../image/uipics/query3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Print" ControlText="ゴ" ControlType="Image" ImageUrl="../image/uipics/print.gif" MouseOverImageUrl="../image/uipics/print2.gif" DisenableImageUrl="../image/uipics/print3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Export" ControlType="Image" ImageUrl="../Image/UIPics/Export.gif" MouseOverImageUrl="../Image/UIPics/Export2.gif" DisenableImageUrl="../Image/UIPics/Export3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
</NavControls>
<QueryFields>
<InfoLight:WebQueryField Caption="订单编号" Condition="=" Mode="" DefaultValue="" FieldName="OrderID"></InfoLight:WebQueryField>
</QueryFields>
</InfoLight:WebNavigator>
        </td></tr><tr><td id="NavigatorContent">
        <asp:Label ID="lblRoleId" runat="server" Text="RoleID:"></asp:Label>
        <asp:TextBox ID="tbRoleID" runat="server" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="lblRoleName" runat="server" Text="Role Name:"></asp:Label>
        <asp:TextBox ID="tbRoleName" runat="server" ReadOnly="True"></asp:TextBox></td></tr><tr><td>
        <InfoLight:WebGridView ID="WebGridView1" runat="server" CellPadding="4" DataSourceID="WDSAgent"
            ForeColor="#333333" GridLines="None" OnRowDataBound="WebGridView1_RowDataBound" Width="100%" skinid="GridViewManagerSkin1">
<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></FooterStyle>

<EditRowStyle BackColor="#999999"></EditRowStyle>
<AddNewRowControls>
<InfoLight:AddNewRowControlItem FieldName="AGENT" ControlType="RefVal" ControlID="WebRefVal1"></InfoLight:AddNewRowControlItem>
<InfoLight:AddNewRowControlItem FieldName="FLOW_DESC" ControlType="DropDownList" ControlID="colFlowDesc"></InfoLight:AddNewRowControlItem>
<InfoLight:AddNewRowControlItem FieldName="START_DATE" ControlType="DateTimePicker" ControlID="WebDateTimePicker1"></InfoLight:AddNewRowControlItem>
<InfoLight:AddNewRowControlItem FieldName="END_DATE" ControlType="DateTimePicker" ControlID="WebDateTimePicker2"></InfoLight:AddNewRowControlItem>
<InfoLight:AddNewRowControlItem FieldName="PAR_AGENT" ControlType="DropDownList" ControlID="DropDownList1"></InfoLight:AddNewRowControlItem>
</AddNewRowControls>

<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></HeaderStyle>

<PagerSettings Mode="NumericFirstLast"></PagerSettings>

<RowStyle BackColor="#F7F6F3" ForeColor="#333333"></RowStyle>
<Columns>
<asp:CommandField CancelImageUrl="~/Image/UIPics/Cancel.gif" DeleteImageUrl="~/Image/UIPics/Delete.gif" EditImageUrl="~/Image/UIPics/Edit.gif" SelectImageUrl="~/Image/UIPics/Select.gif" ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" UpdateImageUrl="~/Image/UIPics/OK.gif" ButtonType="Image">
<ItemStyle Wrap="False"></ItemStyle>
</asp:CommandField>
<asp:BoundField DataField="ROLE_ID" HeaderText="ROLE_ID" ReadOnly="True" SortExpression="ROLE_ID">
<HeaderStyle Wrap="False"></HeaderStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="AGENT" SortExpression="AGENT"><EditItemTemplate>
<InfoLight:WebRefVal id="WebRefVal1" runat="server" DataSourceID="WDSUsers" __designer:wfdid="w21" DataValueField="USERID" DataTextField="USERNAME" ButtonImageUrl="../Image/refval/RefVal.gif" BindingValue='<%# Bind("AGENT") %>' AllowAddData="False" DataBindingField="AGENT" MultiLanguage="False" BindingText PostBackButonClick="False" ResxFilePath ResxDataSet UseButtonImage="True" ReadOnly="False"></InfoLight:WebRefVal> 
</EditItemTemplate>
<FooterTemplate>
<InfoLight:WebRefVal id="WebRefVal1" runat="server" DataSourceID="WDSUsers" __designer:wfdid="w22" DataValueField="USERID" DataTextField="USERNAME" ButtonImageUrl="../Image/refval/RefVal.gif" BindingValue='<%# Bind("AGENT") %>' AllowAddData="False" DataBindingField="AGENT" MultiLanguage="False" BindingText PostBackButonClick="False" ResxFilePath ResxDataSet UseButtonImage="True" ReadOnly="False"></InfoLight:WebRefVal> 
</FooterTemplate>
<ItemTemplate>
<asp:Label id="Label4" runat="server" Text='<%# Bind("AGENT") %>' __designer:wfdid="w12"></asp:Label> 
</ItemTemplate>

<HeaderStyle Wrap="False"></HeaderStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="FLOW_DESC" SortExpression="FLOW_DESC"><EditItemTemplate>
<asp:HiddenField id="HiddenField1" runat="server" __designer:wfdid="w16" Value='<%# Bind("FLOW_DESC") %>'></asp:HiddenField><asp:DropDownList id="colFlowDesc" runat="server" __designer:wfdid="w17" OnSelectedIndexChanged="colFlowDesc_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList> 
</EditItemTemplate>
<FooterTemplate>
<asp:HiddenField id="HiddenField1" runat="server" __designer:wfdid="w18" Value='<%# Bind("FLOW_DESC") %>'></asp:HiddenField><asp:DropDownList id="colFlowDesc" runat="server" __designer:wfdid="w19" OnSelectedIndexChanged="colFlowDesc_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList> 
</FooterTemplate>
<ItemTemplate>
<asp:Label id="Label1" runat="server" Text='<%# Bind("FLOW_DESC") %>' __designer:wfdid="w15"></asp:Label> 
</ItemTemplate>

<HeaderStyle Wrap="False"></HeaderStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="START_DATE" SortExpression="START_DATE"><EditItemTemplate>
<InfoLight:WebDateTimePicker id="WebDateTimePicker1" runat="server" Width="100px" __designer:wfdid="w53" Text='<%# Bind("START_DATE") %>' UseButtonImage="True" DateFormat="ShortDate" DateTimeType="VarChar" UpdatePanelID MinYear="1950" MaxYear="2050" LocalizeForROC="False" Localize="False"></InfoLight:WebDateTimePicker>
</EditItemTemplate>
<FooterTemplate>
<InfoLight:WebDateTimePicker id="WebDateTimePicker1" runat="server" Width="100px" __designer:wfdid="w55" Text='<%# Bind("START_DATE") %>' UseButtonImage="True" DateFormat="ShortDate" DateTimeType="VarChar" UpdatePanelID MinYear="1950" MaxYear="2050" LocalizeForROC="False" Localize="False"></InfoLight:WebDateTimePicker>
</FooterTemplate>
<ItemTemplate>
<asp:Label id="Label2" runat="server" __designer:wfdid="w51" Text='<%# Bind("START_DATE") %>'></asp:Label>
</ItemTemplate>

<HeaderStyle Wrap="False"></HeaderStyle>
</asp:TemplateField>
<asp:BoundField DataField="START_TIME" HeaderText="START_TIME" SortExpression="START_TIME">
<HeaderStyle Wrap="False"></HeaderStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="END_DATE" SortExpression="END_DATE"><EditItemTemplate>
<InfoLight:WebDateTimePicker id="WebDateTimePicker2" runat="server" Width="100px" __designer:wfdid="w58" Text='<%# Bind("END_DATE") %>' UseButtonImage="True" DateFormat="ShortDate" DateTimeType="VarChar" UpdatePanelID MinYear="1950" MaxYear="2050" LocalizeForROC="False" Localize="False"></InfoLight:WebDateTimePicker>
</EditItemTemplate>
<FooterTemplate>
<InfoLight:WebDateTimePicker id="WebDateTimePicker2" runat="server" Width="100px" __designer:wfdid="w59" Text='<%# Bind("END_DATE") %>' UseButtonImage="True" DateFormat="ShortDate" DateTimeType="VarChar" UpdatePanelID MinYear="1950" MaxYear="2050" LocalizeForROC="False" Localize="False"></InfoLight:WebDateTimePicker>
</FooterTemplate>
<ItemTemplate>
<asp:Label id="Label3" runat="server" __designer:wfdid="w56" Text='<%# Bind("END_DATE") %>'></asp:Label>
</ItemTemplate>

<HeaderStyle Wrap="False"></HeaderStyle>
</asp:TemplateField>
<asp:BoundField DataField="END_TIME" HeaderText="END_TIME" SortExpression="END_TIME">
<HeaderStyle Wrap="False"></HeaderStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="PAR_AGENT" SortExpression="PAR_AGENT"><EditItemTemplate>
<asp:DropDownList id="DropDownList1" runat="server" __designer:wfdid="w68" SelectedValue='<%# Bind("PAR_AGENT") %>'><asp:ListItem Selected="True">Y</asp:ListItem>
<asp:ListItem>N</asp:ListItem>
</asp:DropDownList>
</EditItemTemplate>
<FooterTemplate>
<asp:DropDownList id="DropDownList1" runat="server" __designer:wfdid="w69" SelectedValue='<%# Bind("PAR_AGENT") %>'><asp:ListItem Selected="True">Y</asp:ListItem>
<asp:ListItem>N</asp:ListItem>
</asp:DropDownList>
</FooterTemplate>
<ItemTemplate>
<asp:Label id="Label5" runat="server" __designer:wfdid="w66" Text='<%# Bind("PAR_AGENT") %>'></asp:Label>
</ItemTemplate>

<HeaderStyle Wrap="False"></HeaderStyle>
</asp:TemplateField>
<asp:BoundField DataField="REMARK" HeaderText="REMARK" SortExpression="REMARK">
<HeaderStyle Wrap="False"></HeaderStyle>
</asp:BoundField>
</Columns>
<NavControls>
<InfoLight:ControlItem ControlName="Add" ControlText="add" ControlType="Image" ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif" DisenableImageUrl="../image/uipics/add3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Query" ControlText="query" ControlType="Image" ImageUrl="../image/uipics/query.gif" MouseOverImageUrl="../image/uipics/query2.gif" DisenableImageUrl="../image/uipics/query3.gif" Size="25" ControlVisible="False"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Apply" ControlText="apply" ControlType="Image" ImageUrl="../image/uipics/apply.gif" MouseOverImageUrl="../image/uipics/apply2.gif" DisenableImageUrl="../image/uipics/apply3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Abort" ControlText="abort" ControlType="Image" ImageUrl="../image/uipics/abort.gif" MouseOverImageUrl="../image/uipics/abort2.gif" DisenableImageUrl="../image/uipics/abort3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="OK" ControlText="Insert" ControlType="Image" ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif" DisenableImageUrl="../image/uipics/ok3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Cancel" ControlText="cancel" ControlType="Image" ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif" DisenableImageUrl="../image/uipics/cancel3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
</NavControls>

<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>

<PagerStyle HorizontalAlign="Center" BackColor="#284775" ForeColor="White"></PagerStyle>

<AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
</InfoLight:WebGridView>
        </td></tr></table>
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
        <InfoLight:WebDefault ID="WebDefault1" runat="server" DataSourceID="WDSAgent">
            <Fields>
                <InfoLight:DefaultFieldItem DefaultValue="DefRoleId()" FieldName="ROLE_ID" />
                <InfoLight:DefaultFieldItem DefaultValue="000000" FieldName="START_TIME" />
                <InfoLight:DefaultFieldItem DefaultValue="235959" FieldName="END_TIME" />
            </Fields>
        </InfoLight:WebDefault>
        <InfoLight:WebValidate ID="WebValidate1" runat="server" AltRowCss="" DataMember=""
            DataSourceID="WDSAgent" ForeColor="Red" RowCss=""><Fields>
<InfoLight:ValidateFieldItem FieldName="AGENT" CheckNull="True"></InfoLight:ValidateFieldItem>
<InfoLight:ValidateFieldItem FieldName="FLOW_DESC" CheckNull="True"></InfoLight:ValidateFieldItem>
<InfoLight:ValidateFieldItem FieldName="START_DATE" CheckNull="True"></InfoLight:ValidateFieldItem>
<InfoLight:ValidateFieldItem FieldName="END_DATE" CheckNull="True"></InfoLight:ValidateFieldItem>
</Fields>
</InfoLight:WebValidate>
    
    </div>
    </form>
</body>
</html>
