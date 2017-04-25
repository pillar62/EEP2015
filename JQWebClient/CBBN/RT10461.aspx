<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT10461.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var CUSID = Request.getQueryStringByName2("CUSID");
        var BATCHNO = Request.getQueryStringByName2("BATCHNO");
        var flag = true;

        function InsDefault() {
            if (CUSID != "") {
                return CUSID;
            }
        }

        function dgOnloadSuccess() {
            if (flag) {
                //查詢出該用戶的資料
                var sWhere = "CUSID='" + CUSID + "' AND BATCHNO = '" + BATCHNO+"'";
                $("#dataGridMaster").datagrid('setWhere', sWhere);
                var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
                var CODENC = row.CODENC;
                var PERIOD = row.PERIOD;
                var COD1 = row.COD1;
                var COD2 = row.COD2;
                var COD3 = row.COD3;
                var CANCELDAT = row.CANCELDAT;
                var AMT = row.AMT;
                var REALAMT = row.REALAMT;
                var AMTL = row.AMTL;
                var COD4 = row.COD4;
                $("#JQDataForm1CUSID").val(CUSID);
                $("#JQDataForm1BATCHNO").val(BATCHNO);
                $("#JQDataForm1CODENC").val(CODENC);
                $("#JQDataForm1PERIOD").val(PERIOD);
                $("#JQDataForm1COD1").val(COD1);
                $("#JQDataForm1COD2").val(COD2);
                $("#JQDataForm1COD3").val(COD3);
                $("#JQDataForm1CANCELDAT").val(CANCELDAT);
                $("#JQDataForm1AMT").val(AMT);
                $("#JQDataForm1REALAMT").val(REALAMT);
                $("#JQDataForm1AMTL").val(AMTL);
                $("#JQDataForm1COD4").val(AMTL);
            }
            flag = false;
        }

        function mySelect() {
            //點選時開啟應收付資料 用來判斷是否已經沖銷
        }

        function btn1Click()
        {
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var COD4 =  $("#JQDataForm1COD4").val(); //金額
            var usr = getClientInfo('_usercode');
            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT1046.cmdRT10461', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT104111" + "&parameters=" + CUSID + "," + BATCHNO + "," + COD4 + "," + usr,
                cache: false,
                async: false,
                success: function (data) {
                    if (data > 0)
                    {
                        alert("沖帳完成!");
                        window.parent.closeCurrentTab();
                    }
                }
            });
        }

        function btn2Click() {
            window.parent.closeCurrentTab();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div hidden="hidden">
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT1046.RTLessorAVSCustAR" runat="server" AutoApply="True"
                DataMember="RTLessorAVSCustAR" Pagination="True" QueryTitle="Query"
                Title="應收應付帳款沖帳" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="true" AllowAdd="False" ViewCommandVisible="False" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶編號G+YYMMDD001(YY西元後二位)" Editor="text" FieldName="CUSID" Format="" MaxLength="15" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" Format="" MaxLength="12" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="CODENC" Editor="text" FieldName="CODENC" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="使用期間(月)" Editor="numberbox" FieldName="PERIOD" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="付款金額" Editor="numberbox" FieldName="AMT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="已沖消金額" Editor="numberbox" FieldName="REALAMT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="AMTL" Editor="numberbox" FieldName="AMTL" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="鄉鎮市區" Editor="text" FieldName="COD1" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="鄰" Editor="text" FieldName="COD2" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="路街" Editor="text" FieldName="COD3" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="段" Editor="text" FieldName="COD4" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="巷" Editor="text" FieldName="COD5" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="轉應收帳款日" Editor="datebox" FieldName="CDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="沖銷日" Editor="datebox" FieldName="MDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日(未完工時才可執行)" Editor="datebox" FieldName="CANCELDAT" Format="" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

        <JQTools:JQPanel ID="JQPanel1" runat="server" Title="應收應付帳款沖銷">
            <JQTools:JQDataForm ID="JQDataForm1" runat="server" AlwaysReadOnly="False" Closed="False" ContinueAdd="False" DataMember="cmdRT10411" disapply="False" DivFramed="False" DuplicateCheck="False" HorizontalColumnsCount="1" HorizontalGap="0" IsAutoPageClose="False" IsAutoPause="False" IsAutoSubmit="False" IsNotifyOFF="False" IsRejectNotify="False" IsRejectON="False" IsShowFlowIcon="False" RemoteName="sRT1041.cmdRT10411" ShowApplyButton="False" ValidateStyle="Hint" VerticalGap="0">
                <Columns>
                    <JQTools:JQFormColumn Alignment="left" Caption="用戶序號" Editor="text" FieldName="CUSID" MaxLength="15" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                    <JQTools:JQFormColumn Alignment="left" Caption="應收帳款編號" Editor="text" FieldName="BATCHNO" MaxLength="12" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                    <JQTools:JQFormColumn Alignment="left" Caption="AR/AP" Editor="text" FieldName="CODENC" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="80" />
                    <JQTools:JQFormColumn Alignment="left" Caption="明細項期數" Editor="text" FieldName="PERIOD" MaxLength="10" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                    <JQTools:JQFormColumn Alignment="left" Caption="沖立要項一" Editor="text" FieldName="COD1" MaxLength="30" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                    <JQTools:JQFormColumn Alignment="left" Caption="沖立要項二鄰" Editor="text" FieldName="COD2" MaxLength="30" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                    <JQTools:JQFormColumn Alignment="left" Caption="沖立要項三" Editor="text" FieldName="COD3" MaxLength="30" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                    <JQTools:JQFormColumn Alignment="left" Caption="作廢日" Editor="text" FieldName="CANCELDAT" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                    <JQTools:JQFormColumn Alignment="left" Caption="應沖金額" Editor="text" FieldName="AMT" MaxLength="10" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                    <JQTools:JQFormColumn Alignment="left" Caption="已沖金額" Editor="text" FieldName="REALAMT" MaxLength="10" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="180" />
                    <JQTools:JQFormColumn Alignment="left" Caption="未沖金額" Editor="text" FieldName="AMTL" MaxLength="0" NewRow="False" ReadOnly="True" RowSpan="1" Span="1" Visible="True" Width="80" />
                    <JQTools:JQFormColumn Alignment="left" Caption="實際沖帳金額" Editor="text" FieldName="COD4" MaxLength="30" NewRow="False" ReadOnly="False" RowSpan="1" Span="1" Visible="True" Width="180" />
                </Columns>
            </JQTools:JQDataForm>
            <JQTools:JQButton ID="btn1" runat="server" OnClick="btn1Click" Text="沖帳確認" />
            <JQTools:JQButton ID="btn2" runat="server" OnClick="btn2Click" Text="結　　束" />
        </JQTools:JQPanel>

    </form>
</body>
</html>
