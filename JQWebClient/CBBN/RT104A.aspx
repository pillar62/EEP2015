<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT104A.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID");
        var flag = true;

        function InsDefault() {
            if (CUSID != "") {
                return CUSID;
            }
        }

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = " CUSID='" + CUSID + "'";
                $("#dataGridMaster").datagrid('setWhere', sWhere);
            }
            flag = false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT104.RT104A" runat="server" AutoApply="True"
                DataMember="RT104A" Pagination="True" QueryTitle="Query"
                Title="用戶設備資料查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="15" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" MaxLength="12" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工種類" Editor="text" FieldName="Column1" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客服單號" Editor="text" FieldName="Column2" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="PRODNO" Editor="text" FieldName="PRODNO" Format="" MaxLength="6" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="ITEMNO" Editor="text" FieldName="ITEMNO" Format="" MaxLength="3" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="設備名稱" Editor="text" FieldName="PRODNC" Format="" MaxLength="0" Width="200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="規格" Editor="text" FieldName="SPEC" Format="" MaxLength="0" Width="200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="數量" Editor="numberbox" FieldName="QTY" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="right" Caption="金額" Editor="numberbox" FieldName="AMT" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單結案日" Editor="datebox" FieldName="CLOSEDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="12" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="沖帳金額" Editor="numberbox" FieldName="Column3" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="領用單號" Editor="text" FieldName="RCVPRTNO" Format="" MaxLength="13" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="實際領用日" Editor="datebox" FieldName="Expr1" Format="yyyy/mm/dd" Width="90" />
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
<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>
</html>
