<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT1061.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
     <script>
        var COMQ1 = Request.getQueryStringByName2("COMQ1");        
        var usr = getClientInfo('_usercode');
        
        var flag = true;
        if (COMQ1 == "") {
            flag = false;
        }

        function InsDefault() {
            if (COMQ1 != "") {
                return COMQ1;
            }
        }

        $(document).ready(function () {
            dgOnloadSuccess();
        })

        function dgOnloadSuccess() {
            if (flag) {
                var sWhere = "";
                 if (COMQ1 != "") {
                    sWhere = sWhere + " A.COMQ1='" + COMQ1 + "'"
                }

                 $("#dataGridMaster").datagrid('setWhere', sWhere);
                flag = false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT106.RT1061" runat="server" AutoApply="True"
                DataMember="RT1061" Pagination="True" QueryTitle="Query"
                Title="社區合約資料異動查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="社區" Editor="infocombobox" FieldName="COMQ1" Format="" Width="120" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.View_RTLessorAVSCmtyH',tableName:'View_RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="合約編號" Editor="text" FieldName="CONTRACTNO" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="right" Caption="異動項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="64" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動日" Editor="datebox" FieldName="CHGDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動別" Editor="text" FieldName="CODENC" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動員" Editor="text" FieldName="CUSNC" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="合約起算日" Editor="datebox" FieldName="STRDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最近續約日" Editor="datebox" FieldName="CONTDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="續約次數" Editor="numberbox" FieldName="CONTCNT" Format="" Width="64" />
                    <JQTools:JQGridColumn Alignment="left" Caption="合約到期日" Editor="datebox" FieldName="ENDDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="簽約對象" Editor="text" FieldName="CONTRACTOBJ" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="OBJTEL" Editor="text" FieldName="OBJTEL" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="OBJMOBILE" Editor="text" FieldName="OBJMOBILE" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="簽約部門" Editor="text" FieldName="DEPTN4" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="簽約人" Editor="text" FieldName="CUSNC1" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="NMBANK" Editor="text" FieldName="NMBANK" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="AC" Editor="text" FieldName="AC" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="電費補助種類" Editor="text" FieldName="CODENC1" Format="" MaxLength="0" Width="96" />
                    <JQTools:JQGridColumn Alignment="left" Caption="先付後付" Editor="text" FieldName="Expr1" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="每月(度)金額" Editor="numberbox" FieldName="SCALEAMT" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="付款方式" Editor="text" FieldName="Expr2" Format="" MaxLength="0" Width="64" />
                    <JQTools:JQGridColumn Alignment="left" Caption="付款週期" Editor="text" FieldName="Expr3" Format="yyyy/mm/dd" MaxLength="0" Width="64" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案日" Editor="datebox" FieldName="CLOSEDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="結案員" Editor="text" FieldName="Expr4" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢員" Editor="text" FieldName="Expr5" Format="" MaxLength="0" Width="120" />
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
