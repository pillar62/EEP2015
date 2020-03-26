<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT10326.aspx.cs" Inherits="Template_JQueryQuery1" %>

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
                if (LINEQ1 != "") {
                    sWhere = sWhere + " AND A.LINEQ1='" + LINEQ1 + "'"
                }

                $("#dataGridView").datagrid('setWhere', sWhere);
            }

            flag = false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT1032.RT10326" runat="server" AutoApply="True"
                DataMember="RT10326" Pagination="True" QueryTitle="Query"
                Title="主線撤線異動資料查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="社區" Editor="infocombobox" FieldName="COMQ1" Format="" Width="120" EditorOptions="valueField:'COMQ1',textField:'COMN',remoteName:'sRT101.View_RTLessorAVSCmtyH',tableName:'View_RTLessorAVSCmtyH',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="comn" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線" Editor="numberbox" FieldName="LINEQ1" Format="" Width="40" />
                    <JQTools:JQGridColumn Alignment="right" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="40" />
                    <JQTools:JQGridColumn Alignment="right" Caption="異動項次" Editor="numberbox" FieldName="seq1" Format="" MaxLength="0" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動類別" Editor="text" FieldName="codenc" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動日" Editor="datebox" FieldName="chgdat" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動員" Editor="text" FieldName="cusnc" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="撤線種類" Editor="text" FieldName="CODENC1" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="撤線申請日" Editor="datebox" FieldName="DROPAPPLYDAT" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="預定撤線日" Editor="datebox" FieldName="SCHDROPDAT" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="撤線原因" Editor="text" FieldName="DROPREASON" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="拆機單號" Editor="text" FieldName="SNDPRTNO" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工日期" Editor="datebox" FieldName="SNDWORKDAT" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="拆機結案日" Editor="datebox" FieldName="SNDCLOSEDAT" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="撤線結案日" Editor="datebox" FieldName="CLOSEDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢員" Editor="text" FieldName="CUSNC1" Format="" Width="80" />
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
