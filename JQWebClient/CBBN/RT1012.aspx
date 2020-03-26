<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1012.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var COMQ1 = Request.getQueryStringByName2("COMQ1");
        var LINEQ1 = Request.getQueryStringByName2("LINEQ1");
        var flag = true;
        $(document).ready(function () {
            dgOnloadSuccess();
        })

        function dgOnloadSuccess() {
            if (flag) {
                $("#dataGridMaster").datagrid('setWhere', "COMQ1='" + COMQ1 + "' AND LINEQ1=" + LINEQ1);
            }
            
            flag = false;
        }

        function btn1Click() {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var ss = row.comq1;
            var s1 = row.lineq1;
            parent.addTab("設備異動查詢", "CBBN/RT10121.aspx?COMQ1=" + ss + "&LINEQ1=" + s1);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT101.RT1012" runat="server" AutoApply="True"
                DataMember="RT1012" Pagination="True" QueryTitle="Query"
                Title="主線派工設備資料查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" OnLoadSuccess="dgOnloadSuccess" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    
                    <JQTools:JQGridColumn Alignment="left" Caption="社區" Editor="infocombobox" FieldName="comq1" Format="" Width="160" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.View_RTLessorAVSCmtyH',tableName:'View_RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線" Editor="numberbox" FieldName="lineq1" Format="" Width="32" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="FAQNO" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" MaxLength="0" Width="100" Visible="True" />
                    <JQTools:JQGridColumn Alignment="right" Caption="seq" Editor="numberbox" FieldName="seq" Format="" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="類型" Editor="text" FieldName="KD" Format="" MaxLength="0" Width="64" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工日期" Editor="datebox" FieldName="SENDWORKDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="設備名稱/規格" Editor="text" FieldName="PROD" Format="" MaxLength="0" Width="200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="數量" Editor="numberbox" FieldName="QTY" Format="" Width="32" />
                    <JQTools:JQGridColumn Alignment="right" Caption="金額" Editor="numberbox" FieldName="amt" Format="" Width="64" />
                    <JQTools:JQGridColumn Alignment="left" Caption="出庫別" Editor="text" FieldName="WARENAME" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日期" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢原因" Editor="text" FieldName="DROPREASON" Format="" MaxLength="0" Width="200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="CUSNC" Editor="text" FieldName="CUSNC" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="0" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轉應收帳款日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="領用單號" Editor="text" FieldName="rcvprtno" Format="" MaxLength="0" Width="100" />
                    <JQTools:JQGridColumn Alignment="left" Caption="領用結案日" Editor="datebox" FieldName="rcvfinishdat" Format="" Width="80" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-view" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="異動查詢" Visible="True" />
                </TooItems>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
<script>
    $("#toolbardataGridMaster").css("'display', 'block'");
</script>
</html>
