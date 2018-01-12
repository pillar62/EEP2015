<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT401.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        function fileuploadsuccess(value) {
            alert("檔案[" + value + "]上傳成功!!");
            $('#JQFileUpload1').next().remove()
            initInfoFileUpload($('#JQFileUpload1'));
            serverMethod(value);
        }
        function serverMethod(value) {

            //var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var sfile = value; //年

            $.ajax({
                type: "POST",
                url: '../handler/jqDataHandle.ashx?RemoteName=sRT401.cmdRT401', //連接的Server端，command
                //method后的參數為server的Method名稱  parameters后為端的到后端的參數這裡傳入選中資料的CustomerID欄位
                data: "mode=method&method=" + "smRT401" + "&parameters=" + sfile,
                cache: false,
                async: false,
                success: function (data) {
                    //var rows = $.parseJSON(data);//將JSon轉會到Object類型提供給Grid顯示
                    //$('#dataGridMaster0').datagrid('loadData', rows);//通過loadData方法清除掉原有Grid中的舊有資料並填補新資料
                    alert(data);
                    parent.addTab("發票列印", "CBBN/RT306.aspx");
                }
            });
        }
    </script>
</head>
<body>
    請指定xls檔案。
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT401.RTInvTemp" runat="server" AutoApply="True"
                DataMember="RTInvTemp" Pagination="True" QueryTitle="Query"
                Title="匯入Excel檔" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="true" AllowAdd="False" ViewCommandVisible="False" Visible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="BATCH" Editor="numberbox" FieldName="BATCH" Format="" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

        <JQTools:JQFileUpload ID="JQFileUpload1" runat="server" Filter="" ShowLocalFile="True" UpLoadFolder="excel" Width="500px" onSuccess="fileuploadsuccess" />

    </form>
</body>
</html>
