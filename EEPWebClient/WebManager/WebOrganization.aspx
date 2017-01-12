<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebOrganization.aspx.cs"
    Inherits="WebManager_WebOrganization" Theme="ControlSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/MenuUtility.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <infolight:webdatasource id="WDSOrgKind" runat="server" datamember="cmdOrgKind" webdatasetid="wOrgKind">
        </infolight:webdatasource>
            <infolight:webdatasource id="WDSOrgRoles" runat="server" datamember="cmdOrgRoles"
                webdatasetid="wOrgRoles">
        </infolight:webdatasource>
            <infolight:webdatasource id="WDSOrgLevel" runat="server" datamember="cmdOrgLevel"
                webdatasetid="wOrgLevel">
        </infolight:webdatasource>
            <infolight:webdatasource id="WDSGroup" runat="server" datamember="groupInfo" selectalias=""
                webdatasetid="wGroup">
        </infolight:webdatasource>
            <table style="width: 100%;" id="MenuCotent">
                <tr>
                    <td style="width: 250px">
                        <asp:DropDownList ID="cmbOrgKind" runat="server" TabIndex="13" Width="160px">
                        </asp:DropDownList>
                    </td>
                    <td style="border-left-style: dotted">
                        <table style="position: relative; width: 100%;" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Button ID="btnOrgAdd" runat="server" Text="add" OnClick="btnOrgAdd_Click" Width="60px"
                                        TabIndex="2" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';"
                                        onmouseover="this.className='btn_mouseover';" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnOrgUpdate" runat="server" Text="update" OnClick="btnOrgUpdate_Click"
                                        Width="60px" TabIndex="3" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';"
                                        onmouseover="this.className='btn_mouseover';" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnOrgDelete" runat="server" Text="delete" OnClick="btnOrgDelete_Click"
                                        Width="60px" TabIndex="4" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';"
                                        onmouseover="this.className='btn_mouseover';" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="width: 250px">
                        <asp:TreeView ID="tView" runat="server" OnSelectedNodeChanged="tView_SelectedNodeChanged">
                        </asp:TreeView>
                    </td>
                    <td style="border-left-style: dotted">
                        <table style="width: 30%;">
                            <tr>
                                <td nowrap="nowrap">
                                    <asp:Label ID="lblOrgNo" runat="server" Text="OrgNo"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOrgNo" runat="server" Width="150px" AutoPostBack="True" OnTextChanged="txtOrgNo_TextChanged"
                                        TabIndex="5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblOrgDesc" runat="server" Text="OrgDesc"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOrgDesc" runat="server" Width="150px" TabIndex="6"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblUpperOrg" runat="server" Text="UpperOrg"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbUpperOrg" runat="server" Width="150px" TabIndex="7">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblOrgManager" runat="server" Text="OrgManager"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbOrgManager" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbOrgManager_SelectedIndexChanged"
                                        Width="150px" TabIndex="8">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:TextBox ID="lblUser" runat="server" ReadOnly="True" Width="300px" TabIndex="9"
                                        Enabled="False"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblLevelNo" runat="server" Text="LevelNo"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbLevelNo" runat="server" Width="150px" TabIndex="10">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="width: 250px">
                    </td>
                    <td style="border-left-style: dotted">
                        <table style="width: 100%;">
                            <tr>
                                <td width="400">
                                    <infolight:webgridview id="dgvOrgRoles" runat="server" backcolor="White" bordercolor="#DEDFDE"
                                        borderstyle="None" borderwidth="1px" cellpadding="4" datasourceid="WDSOrgRoles"
                                        forecolor="Black" gridlines="Vertical" skinid="GridViewManagerSkin1">
<FooterStyle BackColor="#CCCC99"></FooterStyle>
<AddNewRowControls>
<InfoLight:AddNewRowControlItem FieldName="ROLE_ID" ControlType="RefVal" ControlID="WebRefVal1"></InfoLight:AddNewRowControlItem>
<InfoLight:AddNewRowControlItem FieldName="GROUPNAME" ControlType="TextBox" ControlID="TextBox1"></InfoLight:AddNewRowControlItem>
</AddNewRowControls>

<HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White"></HeaderStyle>

<PagerSettings Mode="NumericFirstLast"></PagerSettings>

<RowStyle BackColor="#F7F7DE"></RowStyle>
<Columns>
<asp:CommandField CancelImageUrl="~/Image/UIPics/Cancel.gif" DeleteImageUrl="~/Image/UIPics/Delete.gif" EditImageUrl="~/Image/UIPics/Edit.gif" SelectImageUrl="~/Image/UIPics/Select.gif" ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" UpdateImageUrl="~/Image/UIPics/OK.gif" ButtonType="Image"></asp:CommandField>
<asp:BoundField DataField="ORG_NO" HeaderText="ORG_NO" ReadOnly="True" SortExpression="ORG_NO">
<HeaderStyle Wrap="False"></HeaderStyle>

<ItemStyle VerticalAlign="Middle" Wrap="False"></ItemStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="ROLE_ID" SortExpression="ROLE_ID"><EditItemTemplate>
                                    <InfoLight:WebRefVal ID="WebRefVal1" runat="server" AllowAddData="False" 
                                        BindingText="" BindingValue='<%# Bind("ROLE_ID") %>' 
                                        ButtonImageUrl="../Image/refval/RefVal.gif" DataBindingField="ROLE_ID" 
                                        DataSourceID="WDSGroup" DataTextField="GROUPID" DataValueField="GROUPID" 
                                        MultiLanguage="False" PostBackButonClick="False" ReadOnly="False" 
                                        ResxDataSet="" ResxFilePath="" UseButtonImage="True">
                                        <columnmatch>
                                            <InfoLight:WebColumnMatch DestControlID="TextBox1" SrcField="GROUPNAME" />
                                        </columnmatch>
                                        <whereitem>
                                            <InfoLight:WebWhereItem Condition="=" FieldName="ISROLE" Value="Y" />
                                        </whereitem>
                                    </InfoLight:WebRefVal>
                                
</EditItemTemplate>
<FooterTemplate>
                                    <InfoLight:WebRefVal ID="WebRefVal1" runat="server" AllowAddData="False" 
                                        BindingText="" BindingValue='<%# Bind("ROLE_ID") %>' 
                                        ButtonImageUrl="../Image/refval/RefVal.gif" DataBindingField="ROLE_ID" 
                                        DataSourceID="WDSGroup" DataTextField="GROUPID" DataValueField="GROUPID" 
                                        MultiLanguage="False" PostBackButonClick="False" ReadOnly="False" 
                                        ResxDataSet="" ResxFilePath="" UseButtonImage="True">
                                        <columnmatch>
                                            <InfoLight:WebColumnMatch DestControlID="TextBox1" SrcField="GROUPNAME" />
                                        </columnmatch>
                                        <whereitem>
                                            <InfoLight:WebWhereItem Condition="=" FieldName="ISROLE" Value="Y" />
                                        </whereitem>
                                    </InfoLight:WebRefVal>
                                
</FooterTemplate>
<ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("ROLE_ID") %>'></asp:Label>
                                
</ItemTemplate>

<HeaderStyle Wrap="False"></HeaderStyle>

<ItemStyle VerticalAlign="Middle" Wrap="False"></ItemStyle>
</asp:TemplateField>
<asp:BoundField DataField="ORG_KIND" HeaderText="ORG_KIND" ReadOnly="True" SortExpression="ORG_KIND">
<HeaderStyle Wrap="False"></HeaderStyle>

<ItemStyle VerticalAlign="Middle" Wrap="False"></ItemStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="GROUPNAME" SortExpression="GROUPNAME"><EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("GROUPNAME") %>'></asp:TextBox>
                                
</EditItemTemplate>
<FooterTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("GROUPNAME") %>'></asp:TextBox>
                                
</FooterTemplate>
<ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("GROUPNAME") %>'></asp:Label>
                                
</ItemTemplate>

<HeaderStyle Wrap="False"></HeaderStyle>

<ItemStyle VerticalAlign="Middle" Wrap="False"></ItemStyle>
</asp:TemplateField>
</Columns>
<NavControls>
<InfoLight:ControlItem ControlName="Add" ControlText="add" ControlType="Image" ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif" DisenableImageUrl="../image/uipics/add3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Query" ControlText="query" ControlType="Image" ImageUrl="../image/uipics/query.gif" MouseOverImageUrl="../image/uipics/query2.gif" DisenableImageUrl="../image/uipics/query3.gif" Size="25" ControlVisible="False"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Apply" ControlText="apply" ControlType="Image" ImageUrl="../image/uipics/apply.gif" MouseOverImageUrl="../image/uipics/apply2.gif" DisenableImageUrl="../image/uipics/apply3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Abort" ControlText="abort" ControlType="Image" ImageUrl="../image/uipics/abort.gif" MouseOverImageUrl="../image/uipics/abort2.gif" DisenableImageUrl="../image/uipics/abort3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="OK" ControlText="Insert" ControlType="Image" ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif" DisenableImageUrl="../image/uipics/ok3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
<InfoLight:ControlItem ControlName="Cancel" ControlText="cancel" ControlType="Image" ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif" DisenableImageUrl="../image/uipics/cancel3.gif" Size="25" ControlVisible="True"></InfoLight:ControlItem>
</NavControls>

<SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White"></SelectedRowStyle>

<PagerStyle HorizontalAlign="Right" BackColor="#F7F7DE" ForeColor="Black"></PagerStyle>

<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
</infolight:webgridview>
                                    <infolight:webdefault id="WebDefault1" runat="server" datasourceid="WDSOrgRoles">
                                    <Fields>
                                        <InfoLight:DefaultFieldItem DefaultValue="GetOrgNo()" FieldName="ORG_NO"></InfoLight:DefaultFieldItem>
                                        <InfoLight:DefaultFieldItem DefaultValue="GetOrgKind()" FieldName="ORG_KIND"></InfoLight:DefaultFieldItem>
                                        <InfoLight:DefaultFieldItem DefaultValue="0" FieldName="ORG_KIND"></InfoLight:DefaultFieldItem>
                                    </Fields>
                                </infolight:webdefault>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 250px">
                        <table style="position: relative; width: 100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtQuery" runat="server" Width="150px"></asp:TextBox>
                                    <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" TabIndex="1" Text="Query"
                                        CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';" onmouseover="this.className='btn_mouseover';" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="border-left-style: dotted">
                        <table style="position: relative; width: 100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" Text="OK" Width="60px"
                                        TabIndex="11" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';"
                                        onmouseover="this.className='btn_mouseover';" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                                        Width="60px" TabIndex="12" CssClass="btn_mouseout" onmouseout="this.className='btn_mouseout';"
                                        onmouseover="this.className='btn_mouseover';" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
