<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT10479.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT1047.RT10479" runat="server" AutoApply="True"
                DataMember="RT10479" Pagination="True" QueryTitle="Query"
                Title="用戶客服單異動查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="10" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客服單號" Editor="text" FieldName="FAQNO" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="類別" Editor="text" FieldName="CODENC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動日" Editor="datebox" FieldName="CHGDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="CUSNC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="來電日" Editor="datebox" FieldName="RCVDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="類型" Editor="text" FieldName="CODENC1" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="摘　　　　　　　　　　要" Editor="text" FieldName="EXPR6" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="CONTACTTEL" Editor="text" FieldName="CONTACTTEL" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="MOBILE" Editor="text" FieldName="MOBILE" Format="" MaxLength="15" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="EMAIL" Editor="text" FieldName="EMAIL" Format="" MaxLength="30" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工日期" Editor="datebox" FieldName="SNDWORK" Format="" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="CUSNC1" Editor="text" FieldName="CUSNC1" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="SNDPRTNO" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="列印日" Editor="datebox" FieldName="PRTDAT" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工結案" Editor="datebox" FieldName="SNDCLOSEDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客服回覆日" Editor="datebox" FieldName="CALLBACKDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="回覆人員" Editor="text" FieldName="EXPR1" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客服結案" Editor="datebox" FieldName="FINISHDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案員" Editor="text" FieldName="EXPR2" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="EXPR3" Editor="text" FieldName="EXPR3" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="EXPR4" Editor="text" FieldName="EXPR4" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="EXPR5" Editor="text" FieldName="EXPR5" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="right" Caption="處理天數" Editor="numberbox" FieldName="FINISHDAT1" Format="" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
