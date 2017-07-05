<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1033.aspx.cs" Inherits="Template_JQueryQuery1" %>

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

        function dgOnloadSuccess() {
            if (flag) {
                var sWhere = " A.COMQ1='" + COMQ1 + "'";
                if (LINEQ1 != "") {
                    sWhere = sWhere + " AND A.LINEQ1='" + LINEQ1 + "'"
                }

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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT103.RT1033" runat="server" AutoApply="True"
                DataMember="RT1033" Pagination="True" QueryTitle="Query"
                Title="主線資料異動記錄查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" OnLoadSuccess="dgOnloadSuccess" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="社區序號" Editor="infocombobox" FieldName="COMQ1" Format="" Width="120" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.View_RTLessorAVSCmtyH',tableName:'View_RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線序號" Editor="numberbox" FieldName="LINEQ1" Format="" Width="40" />
                    <JQTools:JQGridColumn Alignment="right" Caption="異動項次" Editor="numberbox" FieldName="entryno" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動日期" Editor="datebox" FieldName="chgdat" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動別" Editor="text" FieldName="CODENC" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動員" Editor="text" FieldName="cusnc" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="鄉鎮區" Editor="text" FieldName="TOWNSHIP" Format="" MaxLength="10" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線群組" Editor="text" FieldName="LINEGROUP" Format="" MaxLength="1" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線網路IP" Editor="text" FieldName="LINEIP" Format="" MaxLength="20" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="附掛電話" Editor="text" FieldName="LINETEL" Format="" MaxLength="15" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路ISP" Editor="text" FieldName="CODENC1" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="IP種類" Editor="text" FieldName="Expr1" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線速率" Editor="text" FieldName="Expr2" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="IP數量" Editor="numberbox" FieldName="IPCNT" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="通測日" Editor="datebox" FieldName="HINETNOTIFYDAT" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="設備安裝" Editor="datebox" FieldName="HARDWAREDAT" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="測通日" Editor="datebox" FieldName="ADSLAPPLYDAT" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="線路到期" Editor="datebox" FieldName="LINEDUEDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="撤線日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="到期天數" Editor="numberbox" FieldName="Q11" Format="" Width="80" />
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
