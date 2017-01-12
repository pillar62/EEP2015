<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FlowAgent.aspx.cs" Inherits="InnerPages_FlowAgent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Setting Agent</title>
    <link href="../css/MenuUtility.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <InfoLight:WebDataSource ID="WDSAgent" runat="server" AutoApply="True" DataMember="cmdRoleAgent"
                WebDataSetID="wdsAgent">
            </InfoLight:WebDataSource>
            <InfoLight:WebDataSource ID="WDSUsers" runat="server" DataMember="userInfo" WebDataSetID="wdsUsers">
            </InfoLight:WebDataSource>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td id="Td1">
                        <asp:Label ID="lblRoleId" runat="server" Text="RoleID:"></asp:Label>&nbsp;<asp:DropDownList
                            ID="ddlRoleID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRoleID_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="lblRoleName" runat="server" Text="Role Name:"></asp:Label>
                        <asp:TextBox ID="tbRoleName" runat="server" ReadOnly="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        <InfoLight:WebGridView ID="WebGridView1" runat="server" CellPadding="4" DataSourceID="WDSAgent"
                            ForeColor="#333333" GridLines="None" OnRowDataBound="WebGridView1_RowDataBound"
                            SkinID="GridViewManagerSkin1" Width="100%">
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AddNewRowControls>
                                <InfoLight:AddNewRowControlItem ControlID="WebRefVal1" ControlType="RefVal" FieldName="AGENT" />
                                <InfoLight:AddNewRowControlItem ControlID="colFlowDesc" ControlType="DropDownList"
                                    FieldName="FLOW_DESC" />
                                <InfoLight:AddNewRowControlItem ControlID="WebDateTimePicker1" ControlType="DateTimePicker"
                                    FieldName="START_DATE" />
                                <InfoLight:AddNewRowControlItem ControlID="WebDateTimePicker2" ControlType="DateTimePicker"
                                    FieldName="END_DATE" />
                                <InfoLight:AddNewRowControlItem ControlID="DropDownList1" ControlType="DropDownList"
                                    FieldName="PAR_AGENT" />
                            </AddNewRowControls>
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerSettings Mode="NumericFirstLast" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <Columns>
                                <asp:CommandField ButtonType="Image" CancelImageUrl="~/Image/UIPics/Cancel.gif" DeleteImageUrl="~/Image/UIPics/Delete.gif"
                                    EditImageUrl="~/Image/UIPics/Edit.gif" SelectImageUrl="~/Image/UIPics/Select.gif"
                                    ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" UpdateImageUrl="~/Image/UIPics/OK.gif">
                                    <itemstyle wrap="False" />
                                </asp:CommandField>
                                <asp:BoundField DataField="ROLE_ID" HeaderText="ROLE_ID" ReadOnly="True" SortExpression="ROLE_ID">
                                    <headerstyle wrap="False" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="AGENT" SortExpression="AGENT">
                                    <edititemtemplate>
<InfoLight:WebRefVal id="WebRefVal1" runat="server" DataSourceID="WDSUsers" __designer:wfdid="w21" DataValueField="USERID" DataTextField="USERNAME" ButtonImageUrl="../Image/refval/RefVal.gif" BindingValue='<%# Bind("AGENT") %>' AllowAddData="False" DataBindingField="AGENT" MultiLanguage="False" BindingText PostBackButonClick="False" ResxFilePath ResxDataSet UseButtonImage="True" ReadOnly="False"></InfoLight:WebRefVal> 
</edititemtemplate>
                                    <footertemplate>
<InfoLight:WebRefVal id="WebRefVal1" runat="server" DataSourceID="WDSUsers" __designer:wfdid="w22" DataValueField="USERID" DataTextField="USERNAME" ButtonImageUrl="../Image/refval/RefVal.gif" BindingValue='<%# Bind("AGENT") %>' AllowAddData="False" DataBindingField="AGENT" MultiLanguage="False" BindingText PostBackButonClick="False" ResxFilePath ResxDataSet UseButtonImage="True" ReadOnly="False"></InfoLight:WebRefVal> 
</footertemplate>
                                    <itemtemplate>
<asp:Label id="Label4" runat="server" Text='<%# Bind("AGENT") %>' __designer:wfdid="w12"></asp:Label> 
</itemtemplate>
                                    <headerstyle wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FLOW_DESC" SortExpression="FLOW_DESC">
                                    <edititemtemplate>
<asp:HiddenField id="HiddenField1" runat="server" __designer:wfdid="w12" Value='<%# Bind("FLOW_DESC") %>'></asp:HiddenField><asp:DropDownList id="colFlowDesc" runat="server" __designer:wfdid="w13" AutoPostBack="True" OnSelectedIndexChanged="colFlowDesc_SelectedIndexChanged"></asp:DropDownList> 
</edititemtemplate>
                                    <footertemplate>
<asp:HiddenField id="HiddenField1" runat="server" __designer:wfdid="w14" Value='<%# Bind("FLOW_DESC") %>'></asp:HiddenField><asp:DropDownList id="colFlowDesc" runat="server" __designer:wfdid="w15" AutoPostBack="True" OnSelectedIndexChanged="colFlowDesc_SelectedIndexChanged"></asp:DropDownList> 
</footertemplate>
                                    <itemtemplate>
<asp:Label id="Label1" runat="server" Text='<%# Bind("FLOW_DESC") %>' __designer:wfdid="w11"></asp:Label> 
</itemtemplate>
                                    <headerstyle wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="START_DATE" SortExpression="START_DATE">
                                    <edititemtemplate>
<InfoLight:WebDateTimePicker id="WebDateTimePicker1" runat="server" Width="100px" __designer:wfdid="w53" ButtonImageUrl="../Image/datetimepicker/datetimepicker.gif" UseButtonImage="True" DateFormat="ShortDate" DateTimeType="VarChar" UpdatePanelID MinYear="1950" MaxYear="2050" LocalizeForROC="False" Localize="False" DateString='<%# Bind("START_DATE","{0:d}") %>'></InfoLight:WebDateTimePicker> 
</edititemtemplate>
                                    <footertemplate>
<InfoLight:WebDateTimePicker id="WebDateTimePicker1" runat="server" Text='<%# Bind("START_DATE") %>' Width="100px" __designer:wfdid="w3" UseButtonImage="True" DateFormat="ShortDate" DateTimeType="VarChar" UpdatePanelID MinYear="1950" MaxYear="2050" LocalizeForROC="False" Localize="False"></InfoLight:WebDateTimePicker> 
</footertemplate>
                                    <itemtemplate>
<asp:Label id="Label2" runat="server" Text='<%# Bind("START_DATE") %>' __designer:wfdid="w1"></asp:Label> 
</itemtemplate>
                                    <headerstyle wrap="False" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="START_TIME" HeaderText="START_TIME" SortExpression="START_TIME">
                                    <headerstyle wrap="False" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="END_DATE" SortExpression="END_DATE">
                                    <edititemtemplate>
<InfoLight:WebDateTimePicker id="WebDateTimePicker2" runat="server" Width="100px" __designer:wfdid="w58" ButtonImageUrl="../Image/datetimepicker/datetimepicker.gif" UseButtonImage="True" DateFormat="ShortDate" DateTimeType="VarChar" UpdatePanelID MinYear="1950" MaxYear="2050" LocalizeForROC="False" Localize="False" DateString='<%# Bind("END_DATE","{0:d}") %>'></InfoLight:WebDateTimePicker> 
</edititemtemplate>
                                    <footertemplate>
<InfoLight:WebDateTimePicker id="WebDateTimePicker2" runat="server" Text='<%# Bind("END_DATE") %>' Width="100px" __designer:wfdid="w6" UseButtonImage="True" DateFormat="ShortDate" DateTimeType="VarChar" UpdatePanelID MinYear="1950" MaxYear="2050" LocalizeForROC="False" Localize="False"></InfoLight:WebDateTimePicker> 
</footertemplate>
                                    <itemtemplate>
<asp:Label id="Label3" runat="server" Text='<%# Bind("END_DATE") %>' __designer:wfdid="w4"></asp:Label> 
</itemtemplate>
                                    <headerstyle wrap="False" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="END_TIME" HeaderText="END_TIME" SortExpression="END_TIME">
                                    <headerstyle wrap="False" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="PAR_AGENT" SortExpression="PAR_AGENT">
                                    <edititemtemplate>
<asp:DropDownList id="DropDownList1" runat="server" __designer:wfdid="w68" SelectedValue='<%# Bind("PAR_AGENT") %>'><asp:ListItem Selected="True">Y</asp:ListItem>
<asp:ListItem>N</asp:ListItem>
</asp:DropDownList>
</edititemtemplate>
                                    <footertemplate>
<asp:DropDownList id="DropDownList1" runat="server" __designer:wfdid="w69" SelectedValue='<%# Bind("PAR_AGENT") %>'><asp:ListItem Selected="True">Y</asp:ListItem>
<asp:ListItem>N</asp:ListItem>
</asp:DropDownList>
</footertemplate>
                                    <itemtemplate>
<asp:Label id="Label5" runat="server" __designer:wfdid="w66" Text='<%# Bind("PAR_AGENT") %>'></asp:Label>
</itemtemplate>
                                    <headerstyle wrap="False" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="REMARK" HeaderText="REMARK" SortExpression="REMARK">
                                    <headerstyle wrap="False" />
                                </asp:BoundField>
                            </Columns>
                            <NavControls>
                                <InfoLight:ControlItem ControlName="Add" ControlText="add" ControlType="Image" ControlVisible="True"
                                    DisenableImageUrl="../image/uipics/add3.gif" ImageUrl="../image/uipics/add.gif"
                                    MouseOverImageUrl="../image/uipics/add2.gif" Size="25" />
                                <InfoLight:ControlItem ControlName="Query" ControlText="query" ControlType="Image"
                                    ControlVisible="False" DisenableImageUrl="../image/uipics/query3.gif" ImageUrl="../image/uipics/query.gif"
                                    MouseOverImageUrl="../image/uipics/query2.gif" Size="25" />
                                <InfoLight:ControlItem ControlName="Apply" ControlText="apply" ControlType="Image"
                                    ControlVisible="True" DisenableImageUrl="../image/uipics/apply3.gif" ImageUrl="../image/uipics/apply.gif"
                                    MouseOverImageUrl="../image/uipics/apply2.gif" Size="25" />
                                <InfoLight:ControlItem ControlName="Abort" ControlText="abort" ControlType="Image"
                                    ControlVisible="True" DisenableImageUrl="../image/uipics/abort3.gif" ImageUrl="../image/uipics/abort.gif"
                                    MouseOverImageUrl="../image/uipics/abort2.gif" Size="25" />
                                <InfoLight:ControlItem ControlName="OK" ControlText="Insert" ControlType="Image"
                                    ControlVisible="True" DisenableImageUrl="../image/uipics/ok3.gif" ImageUrl="../image/uipics/ok.gif"
                                    MouseOverImageUrl="../image/uipics/ok2.gif" Size="25" />
                                <InfoLight:ControlItem ControlName="Cancel" ControlText="cancel" ControlType="Image"
                                    ControlVisible="True" DisenableImageUrl="../image/uipics/cancel3.gif" ImageUrl="../image/uipics/cancel.gif"
                                    MouseOverImageUrl="../image/uipics/cancel2.gif" Size="25" />
                            </NavControls>
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        </InfoLight:WebGridView>
                    </td>
                </tr>
            </table>
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
