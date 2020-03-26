<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT104412.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var PRTNO = Request.getQueryStringByName2("PRTNO"); //維修單號
        
        var flag = true;

        function InsDefault() {
            if (PRTNO != "") {
                return PRTNO;
            }
        }

        $(document).ready(function () {
            dgOnloadSuccess();
        })

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = "PRTNO = '" + PRTNO + "'";
                $("#dataGridMaster").datagrid('setWhere', sWhere);
                $("#dataGridMaster").datagrid("selectRow", 0);
            }
            flag = false;
        }
       
        
        function CloseTab()
        {
            window.parent.closeCurrentTab();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT1042.RT10422" runat="server" AutoApply="True"
                DataMember="RT10422" Pagination="True" QueryTitle="Query"
                Title="物品領用單資料維護" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="領用單號" Editor="text" FieldName="RCVPRTNO" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="領用類別" Editor="text" FieldName="CODENC" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="領用申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="領用申請人" Editor="text" FieldName="CUSNC1" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="實際領用人" Editor="text" FieldName="CUSNC2" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="領用結案日" Editor="datebox" FieldName="CLOSEDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案人員" Editor="text" FieldName="CUSNC5" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢人員" Editor="text" FieldName="CUSNC6" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號" Editor="text" FieldName="PRTNO1" Format="" MaxLength="0" Width="90" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轉領用單人員" Editor="text" FieldName="cusnc7" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="right" Caption="領用數量" Editor="numberbox" FieldName="QTY" Format="" Width="60" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem ID="btn1" Enabled="True" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="列印領用單" Visible="True" />
                    <JQTools:JQToolItem ID="btn2" Enabled="True" ItemType="easyui-linkbutton" OnClick="btn2Click" Text="領用明細" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
