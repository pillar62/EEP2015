<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT104235.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID"); //用戶編號
        var PRTNO = Request.getQueryStringByName2("PRTNO"); //維修單號
        var ENTRYNO = Request.getQueryStringByName2("ENTRYNO"); //項次
        var usr = getClientInfo('_usercode');

        var flag = true;

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = "RTLessorAVScustHARDWARELOG.PRTNO = '" + PRTNO + "' AND RTLessorAVScustHARDWARELOG.CUSID='" + CUSID + "' AND RTLessorAVScustHARDWARELOG.ENTRYNO=" + ENTRYNO;
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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT10423.V_RT104235" runat="server" AutoApply="True"
                DataMember="V_RT104235" Pagination="True" QueryTitle="Query"
                Title="用戶派工單安裝設備異動資料查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="cusid" Format="" MaxLength="0" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="40" />
                    <JQTools:JQGridColumn Alignment="right" Caption="流水號" Editor="numberbox" FieldName="SEQ" Format="" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動日" Editor="datebox" FieldName="CHGDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動類別" Editor="text" FieldName="CODENC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="CUSNC" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="設備名稱/規格" Editor="text" FieldName="PROD_NM" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="數量" Editor="numberbox" FieldName="QTY" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="出貨倉庫" Editor="text" FieldName="WARENAME" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="資產編號" Editor="text" FieldName="ASSETNO" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轉應收帳款日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" Width="90" />
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
