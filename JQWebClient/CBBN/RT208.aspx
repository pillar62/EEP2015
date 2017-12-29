<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT208.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridView" data-options="pagination:true,view:commandview" RemoteName="sRT208.RTFaqM" runat="server" AutoApply="True"
                DataMember="RTFaqM" Pagination="True" QueryTitle="Query" EditDialogID="JQDialog1"
                Title="用戶客服單資料維護" AlwaysClose="True" BufferView="True" QueryMode="Panel" DeleteCommandVisible="False" UpdateCommandVisible="False" ViewCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客訴單號(yyymmddxxx)" Editor="text" FieldName="CASENO" Format="" MaxLength="10" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="方案別 (P5)" Editor="text" FieldName="COMTYPE" Format="" MaxLength="1" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="社區序號" Editor="numberbox" FieldName="COMQ1" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線序號" Editor="text" FieldName="LINEQ1" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶編號" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶項次" Editor="text" FieldName="ENTRYNO" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報修聯絡人" Editor="text" FieldName="FAQMAN" Format="" MaxLength="50" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡電話" Editor="text" FieldName="TEL" Format="" MaxLength="50" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="手機" Editor="text" FieldName="MOBILE" Format="" MaxLength="50" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報修原因(P7)" Editor="text" FieldName="FAQREASON" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="進出線(I:用戶來電, O:客服Call Out)" Editor="text" FieldName="IOBOUND" Format="" MaxLength="1" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" MaxLength="1500" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="受理人員" Editor="text" FieldName="RCVUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="受理時間" Editor="datebox" FieldName="RCVDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="CLOSEUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案時間" Editor="datebox" FieldName="CLOSEDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="UUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動時間" Editor="datebox" FieldName="UDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="CANCELUSR" Format="" MaxLength="6" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢時間" Editor="datebox" FieldName="CANCELDAT" Format="" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶來電詢問方案(Q2)" Editor="text" FieldName="ASKCASE" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶來源別 (Q3)" Editor="text" FieldName="CUSTSRC" Format="" MaxLength="2" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="SALESID" Editor="text" FieldName="SALESID" Format="" MaxLength="0" Visible="true" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="CONSIGNEE" Editor="text" FieldName="CONSIGNEE" Format="" MaxLength="0" Visible="true" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="修改" Visible="True" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" Visible="True"  />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="viewItem" Text="瀏覽" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區名稱" Condition="%" DataType="string" Editor="text" FieldName="comn" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="客戶名稱" Condition="%" DataType="string" Editor="text" FieldName="faqman" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="受理時間起" Condition="&gt;=" DataType="datetime" Editor="datebox" FieldName="dropdat" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="受理時間迄" Condition="=" DataType="datetime" Editor="datebox" FieldName="RCVDATE" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="結案時間起" Condition="=" DataType="datetime" Editor="datebox" FieldName="closedat" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="結案時間迄" Condition="=" DataType="datetime" Editor="datebox" FieldName="codenc3" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="受理人員" Condition="%" DataType="string" Editor="text" FieldName="CUSNC" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="直經銷" Condition="%" DataType="string" Editor="infocombobox" EditorOptions="items:[{value:'0',text:'全部',selected:'false'},{value:'1',text:'直銷',selected:'false'},{value:'2',text:'經銷',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="ANGENCY" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="結案狀態" Condition="%" DataType="string" Editor="infocombobox" EditorOptions="items:[{value:'0',text:'全部',selected:'false'},{value:'1',text:'未結案',selected:'true'},{value:'2',text:'已結案',selected:'false'},{value:'3',text:'未完工',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="codenc1" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="預定施工人員" Condition="%" DataType="string" Editor="text" FieldName="SNAME" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>

            <JQTools:JQDialog ID="JQDialog1" runat="server" BindingObjectID="dataFormMaster" Title="用戶客服單資料維護">
                <JQTools:JQDataForm ID="dataFormMaster" runat="server" DataMember="RTFaqM" HorizontalColumnsCount="2" RemoteName="sRT208.RTFaqM" >
                    <Columns>
                        <JQTools:JQFormColumn Alignment="left" Caption="客訴單號(yyymmddxxx)" Editor="text" FieldName="CASENO" Format="" maxlength="10" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="方案別 (P5)" Editor="text" FieldName="COMTYPE" Format="" maxlength="1" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="社區序號" Editor="numberbox" FieldName="COMQ1" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="主線序號" Editor="text" FieldName="LINEQ1" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶編號" Editor="text" FieldName="CUSID" Format="" maxlength="15" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶項次" Editor="text" FieldName="ENTRYNO" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="報修聯絡人" Editor="text" FieldName="FAQMAN" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="聯絡電話" Editor="text" FieldName="TEL" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="手機" Editor="text" FieldName="MOBILE" Format="" maxlength="50" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="報修原因(P7)" Editor="text" FieldName="FAQREASON" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="進出線(I:用戶來電, O:客服Call Out)" Editor="text" FieldName="IOBOUND" Format="" maxlength="1" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="備註" Editor="text" FieldName="MEMO" Format="" maxlength="1500" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="受理人員" Editor="text" FieldName="RCVUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="受理時間" Editor="datebox" FieldName="RCVDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="CLOSEUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="結案時間" Editor="datebox" FieldName="CLOSEDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="UUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="異動時間" Editor="datebox" FieldName="UDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="CANCELUSR" Format="" maxlength="6" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="作廢時間" Editor="datebox" FieldName="CANCELDAT" Format="" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶來電詢問方案(Q2)" Editor="text" FieldName="ASKCASE" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="客戶來源別 (Q3)" Editor="text" FieldName="CUSTSRC" Format="" maxlength="2" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="SALESID" Editor="text" FieldName="SALESID" Format="" maxlength="0" Width="180" />
                        <JQTools:JQFormColumn Alignment="left" Caption="CONSIGNEE" Editor="text" FieldName="CONSIGNEE" Format="" maxlength="0" Width="180" />
                    </Columns>
                </JQTools:JQDataForm>
                <JQTools:JQDefault ID="defaultMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQDefault>
                <JQTools:JQValidate ID="validateMaster" runat="server" BindingObjectID="dataFormMaster" BorderStyle="NotSet" ClientIDMode="Inherit" Enabled="True" EnableTheming="True" EnableViewState="True" ViewStateMode="Inherit">
                </JQTools:JQValidate>
            </JQTools:JQDialog>
        </div>
    </form>
</body>
</html>
