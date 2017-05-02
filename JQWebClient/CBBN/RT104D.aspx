<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT104D.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID");
        var flag = true;
        var usr = getClientInfo('_usercode');

        function InsDefault() {
            if (CUSID != "") {
                return CUSID;
            }
        }

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = " RTLessorAVScust.CUSID='" + CUSID + "'";
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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT104.RT104D" runat="server" AutoApply="True"
                DataMember="RT104D" Pagination="True" QueryTitle="Query"
                Title="用戶異動資料查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="infocombobox" FieldName="CUSID" Format="" MaxLength="10" Width="120" EditorOptions="valueField:'CUSID',textField:'CUSNC',remoteName:'sRT104.View_RTLessorAVSCust',tableName:'View_RTLessorAVSCust',pageSize:'-1',checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                    <JQTools:JQGridColumn Alignment="right" Caption="項次" Editor="numberbox" FieldName="ENTRYNO" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線" Editor="text" FieldName="comqline" Format="" MaxLength="0" Width="30" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動日期" Editor="datebox" FieldName="CHGDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動類別" Editor="text" FieldName="CODENC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動人員" Editor="text" FieldName="CUSNC" Format="" MaxLength="50" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶申請日" Editor="datebox" FieldName="APPLYDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費日" Editor="datebox" FieldName="strbillingDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最近計費日" Editor="datebox" FieldName="NEWBILLINGDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="公關機" Editor="text" FieldName="FREECODE" Format="" MaxLength="0" Width="50" />
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
