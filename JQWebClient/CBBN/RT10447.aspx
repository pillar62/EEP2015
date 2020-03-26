<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT10447.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
     <script>
        var CUSID = Request.getQueryStringByName2("CUSID");
        var flag = true;
        var usr = getClientInfo('_usercode');

        $(document).ready(function () {
            dgOnloadSuccess();
        })

        function dgOnloadSuccess()
        {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = "A.CUSID='" + CUSID + "'";
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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT1044.RT10447" runat="server" AutoApply="True"
                DataMember="RT10447" Pagination="True" QueryTitle="Query"
                Title="用戶復機異動資料查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="text" FieldName="CUSID" Format="" MaxLength="10" Width="120" EditorOptions="" />
                    <JQTools:JQGridColumn Alignment="right" Caption="序號" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="40" />
                    <JQTools:JQGridColumn Alignment="right" Caption="項次" Editor="numberbox" FieldName="SEQ" Format="" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶名稱" Editor="text" FieldName="CUSNC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動日" Editor="datebox" FieldName="CHGDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動類別" Editor="text" FieldName="CODENC" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="SHORTNC" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="復機申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費週期" Editor="text" FieldName="EXPR1" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳費方式" Editor="text" FieldName="EXPR2" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="期數" Editor="numberbox" FieldName="PERIOD" Format="" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="第二戶" Editor="text" FieldName="SECONDCASE" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="right" Caption="應收金額" Editor="numberbox" FieldName="AMT" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="已收金額" Editor="numberbox" FieldName="REALAMT" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轉帳款日" Editor="datebox" FieldName="TARDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轉帳款人員" Editor="text" FieldName="TUSR" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="CANCELUSR" Format="" MaxLength="0" Width="80" />
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
