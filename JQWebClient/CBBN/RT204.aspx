<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT204.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        function queryGrid(dg) { //查詢后添加固定條件
            if ($(dg).attr('id') == 'dataGridMaster')
            {
                var where = $(dg).datagrid('getWhere');
                var due1S, due1E, due2S, due2E, COMQ1, COMN, CUSNC;
                COMQ1 = $("#COMQ1_Query").combobox('getValue'); //年
                COMN = $("#COMN_Query").combobox('getValue'); //月
                CUSNC = $("#CUSNC_Query").combobox('getValue'); //期別
                if (spr == '1') {
                    due1S = syr + '/' + smm + '/01';
                    due1E = syr + '/' + smm + '/15';
                }
                else {
                    due1S = syr + '/' + smm + '/16';
                    var sdt = new Date(due1S + ' 00:00:00');
                    //將月份移至下個月份
                    sdt.setMonth(sdt.getMonth() + 1);
                    //設定為下個月份的第一天
                    sdt.setDate(1);
                    //將日期-1為當月的最後一天
                    var dayOfMonth = sdt.getDate();
                    sdt.setDate(dayOfMonth - 1);
                    due1E = sdt.getFullYear() + "/" + (sdt.getMonth() + 1) + "/" + sdt.getDate();
                }

                due2S = "1999/1/1";
                due2E = "1999/1/1";

                if (where.length > 0) {
                    //取得查詢條件的值
                    where = "a.DROPDAT is null and a.CANCELDAT is null and a.FINISHDAT is not null and a.freecode<>'Y' and b.DROPDAT is null and b.CANCELDAT is null ";
                    where = where + " and (a.duedat between '" + due1S + "' and '" + due1E + "' OR a.duedat between '" + due2S + "' and '" + due2E + "') ";
                }
                $(dg).datagrid('setWhere', where);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT204.cmRT204" runat="server" AutoApply="True"
                DataMember="cmRT204" Pagination="True" QueryTitle="查詢"
                Title="用戶應收應付帳款查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="COMN" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客編" Editor="text" FieldName="CUSID" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶" Editor="text" FieldName="CUSNC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="AR/AP" Editor="text" FieldName="CODENC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="期數" Editor="numberbox" FieldName="PERIOD" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="應沖金額" Editor="numberbox" FieldName="AMT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="已沖金額" Editor="numberbox" FieldName="REALAMT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="未沖金額" Editor="numberbox" FieldName="DIFFAMT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖帳日" Editor="datebox" FieldName="MDAT" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖帳員" Editor="text" FieldName="MUSR" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立項一" Editor="text" FieldName="COD1" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖立項二" Editor="text" FieldName="COD2" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="COD3" Editor="text" FieldName="COD3" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="產生日" Editor="datebox" FieldName="CDAT" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢員" Editor="text" FieldName="CANCELUSR" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢原因" Editor="text" FieldName="CANCELMEMO" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="Dropdat" Format="yyyy/mm/dd" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區序號" Editor="text" FieldName="COMQ1" Frozen="False" IsNvarChar="False" MaxLength="0" QueryCondition="" ReadOnly="False" Sortable="False" Visible="True" Width="80">
                    </JQTools:JQGridColumn>
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="沖  帳" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="沖帳明細" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="帳款明細" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="超商未沖(Excel)" Visible="True" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區序號" Condition="%" DataType="string" Editor="text" FieldName="COMQ1" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="社區名稱" Condition="%" DataType="string" Editor="text" FieldName="COMN" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="用戶名稱" Condition="%" DataType="string" Editor="text" FieldName="CUSNC" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="沖帳日起" Condition="&gt;=" DataType="string" Editor="datebox" FieldName="MDAT" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="沖帳日迄" Condition="&lt;=" DataType="string" Editor="datebox" FieldName="MDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="應收應付帳款狀態" Condition="%" DataType="string" Editor="infocombobox" EditorOptions="items:[{value:' ',text:' ',selected:'false'},{value:'0',text:'全部',selected:'false'},{value:'1',text:'已沖帳',selected:'false'},{value:'2',text:'未沖帳或部份沖帳',selected:'false'},{value:'3',text:'已作廢',selected:'false'},{value:'4',text:'全部(不含作廢)',selected:'false'},{value:'5',text:'未沖金額為負',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="COD1" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
