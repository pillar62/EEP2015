<%@ Page Language="VB" AutoEventWireup="true" CodeFile="VBWebCMasterDetail5.aspx.vb" Inherits="Template_VBWebCMasterDetail5" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <InfoLight:WebDataSource ID="Master" runat="server" AutoApply="True" WebDataSetID="WMaster">
            </InfoLight:WebDataSource>
            <InfoLight:WebDataSource ID="Detail1" runat="server" MasterDataSource="Master" WebDataSetID="WMaster">
            </InfoLight:WebDataSource>
            <InfoLight:WebDataSource ID="Detail2" runat="server" MasterDataSource="Master" WebDataSetID="WMaster">
            </InfoLight:WebDataSource>
        </div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td colspan="2">
                    <InfoLight:WebStatusStrip ID="WebStatusStrip1" runat="server" BackColor="PowderBlue"
                        BorderColor="LightGray" BorderStyle="Groove" BorderWidth="2px" ContentBackColor=""
                        ContentForeColor="White" Font-Bold="True" ForeColor="MediumBlue" ShowCompany="False"
                        ShowDate="True" ShowEEPAlias="True" ShowNavigatorStatus="True" ShowSolution="False"
                        ShowTitle="True" ShowUserID="True" ShowUserName="True" SkinID="StatusStripSkin1"
                        StatusBackColor="White" StatusForeColor="MediumBlue" TitleBackColor="MediumBlue"
                        TitleForeColor="White" Width="100%" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <InfoLight:WebNavigator ID="WebNavigator1" runat="server" BackColor="PowderBlue"
                        BindingObject="wfvMaster" BorderColor="#E0E0E0" BorderStyle="Groove" BorderWidth="2px"
                        OnCommand="WebNavigator1_Command" ShowDataStyle="FormViewStyle" StatusStrip="WebStatusStrip1"
                        Width="100%">
                        <NavControls>
                            <InfoLight:ControlItem ControlName="First" ControlText="首筆" ControlType="Image" ControlVisible="True"
                                ImageUrl="../image/uipics/first.gif" MouseOverImageUrl="../image/uipics/first2.gif"
                                Size="25" />
                            <InfoLight:ControlItem ControlName="Previous" ControlText="上筆" ControlType="Image"
                                ControlVisible="True" ImageUrl="../image/uipics/previous.gif" MouseOverImageUrl="../image/uipics/previous2.gif"
                                Size="25" />
                            <InfoLight:ControlItem ControlName="Next" ControlText="下筆" ControlType="Image" ControlVisible="True"
                                ImageUrl="../image/uipics/next.gif" MouseOverImageUrl="../image/uipics/next2.gif"
                                Size="25" />
                            <InfoLight:ControlItem ControlName="Last" ControlText="末筆" ControlType="Image" ControlVisible="True"
                                ImageUrl="../image/uipics/last.gif" MouseOverImageUrl="../image/uipics/last2.gif"
                                Size="25" />
                            <InfoLight:ControlItem ControlName="Add" ControlText="新增" ControlType="Image" ControlVisible="True"
                                ImageUrl="../image/uipics/add.gif" MouseOverImageUrl="../image/uipics/add2.gif"
                                Size="25" />
                            <InfoLight:ControlItem ControlName="Update" ControlText="更改" ControlType="Image"
                                ControlVisible="True" ImageUrl="../image/uipics/edit.gif" MouseOverImageUrl="../image/uipics/edit2.gif"
                                Size="25" />
                            <InfoLight:ControlItem ControlName="Delete" ControlText="刪除" ControlType="Image"
                                ControlVisible="True" ImageUrl="../image/uipics/delete.gif" MouseOverImageUrl="../image/uipics/delete2.gif"
                                Size="25" />
                            <InfoLight:ControlItem ControlName="OK" ControlText="確認" ControlType="Image" ControlVisible="True"
                                ImageUrl="../image/uipics/ok.gif" MouseOverImageUrl="../image/uipics/ok2.gif"
                                Size="25" />
                            <InfoLight:ControlItem ControlName="Cancel" ControlText="取消" ControlType="Image"
                                ControlVisible="True" ImageUrl="../image/uipics/cancel.gif" MouseOverImageUrl="../image/uipics/cancel2.gif"
                                Size="25" />
                            <InfoLight:ControlItem ControlName="Apply" ControlText="存檔" ControlType="Image" ControlVisible="True"
                                ImageUrl="../image/uipics/apply.gif" MouseOverImageUrl="../image/uipics/apply2.gif"
                                Size="25" />
                            <InfoLight:ControlItem ControlName="Abort" ControlText="放棄" ControlType="Image" ControlVisible="True"
                                ImageUrl="../image/uipics/abort.gif" MouseOverImageUrl="../image/uipics/abort2.gif"
                                Size="25" />
                            <InfoLight:ControlItem ControlName="Query" ControlText="查詢" ControlType="Image" ControlVisible="True"
                                ImageUrl="../image/uipics/query.gif" MouseOverImageUrl="../image/uipics/query2.gif"
                                Size="25" />
                            <InfoLight:ControlItem ControlName="Print" ControlText="打印" ControlType="Image" ControlVisible="True"
                                ImageUrl="../image/uipics/print.gif" MouseOverImageUrl="../image/uipics/print2.gif"
                                Size="25" />
                        </NavControls>
                        <NavStates>
                            <InfoLight:WebNavigatorStateItem StateText="Initial" />
                            <InfoLight:WebNavigatorStateItem StateText="Browsed" />
                            <InfoLight:WebNavigatorStateItem StateText="Inserting" />
                            <InfoLight:WebNavigatorStateItem StateText="Editing" />
                            <InfoLight:WebNavigatorStateItem StateText="Applying" />
                            <InfoLight:WebNavigatorStateItem StateText="Changing" />
                            <InfoLight:WebNavigatorStateItem StateText="Querying" />
                            <InfoLight:WebNavigatorStateItem StateText="Printing" />
                        </NavStates>
                    </InfoLight:WebNavigator>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align: text-top; text-align: left">
                    <InfoLight:WebFormView ID="wfvMaster" runat="server" AllowPaging="True" CellPadding="4"
                        DataSourceID="Master" ForeColor="#333333" LayOutColNum="2" OnAfterInsertLocate="wfvMaster_AfterInsertLocate"
                        OnCanceled="wfvMaster_Canceled" SkinID="FormViewSkin2" Width="100%">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <RowStyle BackColor="#EFF3FB" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    </InfoLight:WebFormView>
                    <InfoLight:WebMultiViewCaptions ID="WebMultiViewCaptions1" runat="server" MultiViewID="MultiView1"
                        TableStyle="Style3" Width="100%">
                        <Captions>
                            <InfoLight:WebMultiViewCaption Caption="Detail1" />
                            <InfoLight:WebMultiViewCaption Caption="Detail2" />
                        </Captions>
                    </InfoLight:WebMultiViewCaptions>
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="View1" runat="server">
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                        </asp:View>
                    </asp:MultiView>
                    &nbsp;
                    <InfoLight:WebGridView ID="wgvDetail1" runat="server" DataSourceID="Detail1" SkinID="GridViewSkin2"
                        Width="100%">
                        <PagerSettings Mode="NumericFirstLast" />
                        <Columns>
                            <asp:CommandField ButtonType="Image" CancelImageUrl="~/Image/UIPics/Cancel.gif" DeleteImageUrl="~/Image/UIPics/Delete.gif"
                                EditImageUrl="~/Image/UIPics/Edit.gif" SelectImageUrl="~/Image/UIPics/Select.gif"
                                ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" UpdateImageUrl="~/Image/UIPics/OK.gif" />
                        </Columns>
                        <NavControls>
                            <InfoLight:ControlItem ControlName="Add" ControlText="add" ControlType="Image" ControlVisible="True"
                                DisenableImageUrl="../image/uipics/add3.gif" ImageUrl="../image/uipics/add.gif"
                                MouseOverImageUrl="../image/uipics/add2.gif" Size="25" />
                            <InfoLight:ControlItem ControlName="OK" ControlText="Insert" ControlType="Image"
                                ControlVisible="True" DisenableImageUrl="../image/uipics/ok3.gif" ImageUrl="../image/uipics/ok.gif"
                                MouseOverImageUrl="../image/uipics/ok2.gif" Size="25" />
                            <InfoLight:ControlItem ControlName="Cancel" ControlText="cancel" ControlType="Image"
                                ControlVisible="True" DisenableImageUrl="../image/uipics/cancel3.gif" ImageUrl="../image/uipics/cancel.gif"
                                MouseOverImageUrl="../image/uipics/cancel2.gif" Size="25" />
                        </NavControls>
                    </InfoLight:WebGridView>
                </td>
            </tr>
        </table>
        <InfoLight:WebGridView ID="wgvDetail2" runat="server" DataSourceID="Detail2" SkinID="GridViewSkin2"
            Width="100%">
            <PagerSettings Mode="NumericFirstLast" />
            <Columns>
                <asp:CommandField ButtonType="Image" CancelImageUrl="~/Image/UIPics/Cancel.gif" DeleteImageUrl="~/Image/UIPics/Delete.gif"
                    EditImageUrl="~/Image/UIPics/Edit.gif" SelectImageUrl="~/Image/UIPics/Select.gif"
                    ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" UpdateImageUrl="~/Image/UIPics/OK.gif" />
            </Columns>
            <NavControls>
                <InfoLight:ControlItem ControlName="Add" ControlText="add" ControlType="Image" ControlVisible="True"
                    DisenableImageUrl="../image/uipics/add3.gif" ImageUrl="../image/uipics/add.gif"
                    MouseOverImageUrl="../image/uipics/add2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="OK" ControlText="Insert" ControlType="Image"
                    ControlVisible="True" DisenableImageUrl="../image/uipics/ok3.gif" ImageUrl="../image/uipics/ok.gif"
                    MouseOverImageUrl="../image/uipics/ok2.gif" Size="25" />
                <InfoLight:ControlItem ControlName="Cancel" ControlText="cancel" ControlType="Image"
                    ControlVisible="True" DisenableImageUrl="../image/uipics/cancel3.gif" ImageUrl="../image/uipics/cancel.gif"
                    MouseOverImageUrl="../image/uipics/cancel2.gif" Size="25" />
            </NavControls>
        </InfoLight:WebGridView>
    
    </div>
    </form>
</body>
</html>
