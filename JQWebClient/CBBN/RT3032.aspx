<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT3032.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var TYY = Request.getQueryStringByName2("TYY");
        var TMM = Request.getQueryStringByName2("TMM");
        var flag = true;
        if (TMM == "") {
            flag = false;
        }

        $(document).ready(function () {
            dgOnloadSuccess();
        })

        function dgOnloadSuccess() {
            if (flag) {
                $("#dataGridMaster").datagrid('setWhere', " TYY='" + TYY + "' AND TMM ='" + TMM + "'");
            }
            flag = false;
        }

        function queryGrid(dg) { //查詢后添加固定條件
            if ($(dg).attr('id') == 'dataGridMaster') {
                var where = $(dg).datagrid('getWhere');
                where = " TYY='" + TYY + "' AND TMM ='" + TMM + "'";
                }
            $(dg).datagrid('setWhere', where);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT303.cmRT3032" runat="server" AutoApply="True"
                DataMember="cmRT3032" Pagination="True" QueryTitle="Query"
                Title="用戶應收應付帳款明細查詢" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="true" AllowAdd="False" ViewCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款年" Editor="numberbox" FieldName="TYY" Format="" Width="48" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款月" Editor="numberbox" FieldName="TMM" Format="" Width="48" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="comn" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶" Editor="text" FieldName="cusnc" Format="" MaxLength="0" Width="90" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶編號G+YYMMDD001(YY西元後二位)" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="12" Width="100" />
                    <JQTools:JQGridColumn Alignment="right" Caption="序號" Editor="numberbox" FieldName="SEQ" Format="" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="會計科目" Editor="text" FieldName="Expr2" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="ACNAMEC" Editor="text" FieldName="ACNAMEC" Format="" MaxLength="0" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="立帳年月" Editor="text" FieldName="TYM" Format="yyyy/mm" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="項目名稱" Editor="text" FieldName="ITEMNC" Format="" MaxLength="50" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="正/負" Editor="text" FieldName="PORM" Format="" MaxLength="1" Width="40" />
                    <JQTools:JQGridColumn Alignment="right" Caption="應收付金額" Editor="numberbox" FieldName="AMT" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="已沖消金額" Editor="numberbox" FieldName="REALAMT" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="right" Caption="未沖銷金額" Editor="numberbox" FieldName="Expr1" Format="" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="產生日" Editor="datebox" FieldName="CDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖銷日" Editor="datebox" FieldName="MDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢原因" Editor="textarea" FieldName="CANCELMEMO" Format="" MaxLength="100" Width="200" />
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
