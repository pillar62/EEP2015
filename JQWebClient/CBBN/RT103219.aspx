<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT103219.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var COMQ1 = Request.getQueryStringByName2("COMQ1");
        var LINEQ1 = Request.getQueryStringByName2("LINEQ1");
        var usr = getClientInfo('_usercode');
        
        var flag = true;
        if (COMQ1 == "") {
            flag = false;
        }
        $(document).ready(function () {
            dgOnloadSuccess();
        })

        function dgOnloadSuccess() {
            if (flag) {
                var sWhere = " A.COMQ1='" + COMQ1 + "'";
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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT1032.RT103219" runat="server" AutoApply="True"
                DataMember="RT103219" Pagination="True" QueryTitle="Query"
                Title="用戶撤線派工單異動資料查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" OnLoadSuccess="dgOnloadSuccess" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="comq1" Editor="numberbox" FieldName="comq1" Format="" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="right" Caption="lineq1" Editor="numberbox" FieldName="lineq1" Format="" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="right" Caption="entryno" Editor="numberbox" FieldName="entryno" Format="" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="PRTNO" Editor="text" FieldName="PRTNO" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="right" Caption="seq" Editor="numberbox" FieldName="seq" Format="" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線" Editor="text" FieldName="comqline" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO1" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="項次" Editor="numberbox" FieldName="ENTRYNO1" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動日期" Editor="datebox" FieldName="CHGDAT" Format="yyyy/mm/dd" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動類別" Editor="text" FieldName="CODENC" Format="" MaxLength="500" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="CUSNC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工日" Editor="datebox" FieldName="SENDWORKDAT" Format="yyyy/mm/dd" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動原因" Editor="text" FieldName="DROPDESC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工結案" Editor="datebox" FieldName="CLOSEDAT" Format="yyyy/mm/dd" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="未完工結案" Editor="datebox" FieldName="unCLOSEDAT" Format="yyyy/mm/dd" Width="70" />
                    <JQTools:JQGridColumn Alignment="left" Caption="獎金計算年月" Editor="text" FieldName="BONUSCLOSEYM" Format="yyyy/mm" MaxLength="0" Width="80" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
