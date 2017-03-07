<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT3021.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var flag = true;

        function OnLoadSuccess()
        {
            if (flag)
            {
                var data, json, i, yr;
                var Today = new Date();
                yr = Today.getFullYear();
                try {
                    json = "";
                    for (var i = 4; i >= 1; i--) {
                        if (json == "")
                        {
                            json = '{"value":"' + (yr - i) + '","text":"' + (yr - i) + '"}';
                        }
                        else
                        {
                            json = json + ',{"value":"' + (yr - i) + '","text":"' + (yr - i) + '"}';
                        }
                        
                    }
                    json = json + ',{"value":"' + (yr) + '","text":"' + (yr) + '","selected":true}';
                    for (var i = 1; i <= 4; i++) {
                        if (json == "") {
                            json = '{"value":"' + (yr + i) + '","text":"' + (yr + i) + '"}';
                        }
                        else {
                            json = json + ',{"value":"' + (yr + i) + '","text":"' + (yr + i) + '"}';
                        }

                    }
                    json = '[' + json + ']';
                    //json = '[{"id":"2015","text":"2015年計劃","value":"2015","selected":true},{"id":"2016","text":"2016年計劃","value":"2016"}]';
                    data = $.parseJSON(json);
                    $("#DUEDAT_Query").combobox("loadData", data);
                }
                catch (err) {
                    alert(err.message);
                }
            }
            flag = false;
        }

        function queryGrid(dg) { //查詢后添加固定條件
            if ($(dg).attr('id') == 'dataGridMaster')
            {
                var where = $(dg).datagrid('getWhere');
                var due1S, due1E, due2S, due2E, syr, smm, spr;
                syr = $("#DUEDAT_Query").combobox('getValue'); //年
                smm = $("#DROPDAT_Query").combobox('getValue'); //月
                spr = $("#cusnc_Query").combobox('getValue'); //期別
                alert(syr);
                if (spr == '1')
                {
                    due1S = syr + '/' + smm + '/01';
                    due1E = syr + '/' + smm + '/15';
                }
                else
                {
                    due1S = syr + '/' + smm + '/16';
                    var sdt = new Date(due1S+' 00:00:00');
                    //將月份移至下個月份
                    sdt.setMonth(sdt.getMonth()+1);
                    //alert(sdt);
                    //設定為下個月份的第一天
                    sdt.setDate(1);
                    //將日期-1為當月的最後一天
                    var dayOfMonth = sdt.getDate();
                    sdt.setDate(dayOfMonth - 1);
                    due1E = sdt.getFullYear() + "/" + (sdt.getMonth()+1) + "/" + sdt.getDate();
                    alert(due1E);
                }

                due2S = "1999/1/1";
                due2E = "1999/1/1";

                alert(where);
                if (where.length > 0) {
                    //取得查詢條件的值
                    where = "a.DROPDAT is null and a.CANCELDAT is null and a.FINISHDAT is not null and a.freecode<>'Y' and b.DROPDAT is null and b.CANCELDAT is null ";
                    where = where + " and (a.duedat between '" + due1S + "' and '" + due1E + "' OR a.duedat between '" + due2S + "' and '" + due2E + "') ";

                    alert(where);
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
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT302.RT3021" runat="server" AutoApply="True"
                DataMember="RT3021" Pagination="True" QueryTitle="Query"
                Title="每月續約帳單轉檔作業" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="true" AllowAdd="False" ViewCommandVisible="False" OnLoadSuccess="OnLoadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="right" Caption="社區序號" Editor="numberbox" FieldName="comq1" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線序號" Editor="numberbox" FieldName="lineq1" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶代號" Editor="text" FieldName="cusid" Format="" MaxLength="15" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="主線" Editor="text" FieldName="comqline" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區" Editor="text" FieldName="comn" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶名" Editor="text" FieldName="cusnc" Format="" MaxLength="30" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="週期" Editor="text" FieldName="codenc" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="繳款" Editor="text" FieldName="codenc1" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="連絡電話" Editor="text" FieldName="TEL" Format="" MaxLength="0" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="申請日" Editor="datebox" FieldName="APPLYDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="開始計費" Editor="datebox" FieldName="STRBILLINGDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="最近續約日" Editor="datebox" FieldName="newBILLINGDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="調整日數" Editor="numberbox" FieldName="adjustday" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="到期日" Editor="datebox" FieldName="DUEDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="DROPDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="期數" Editor="numberbox" FieldName="PERIOD" Format="" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="可用日數" Editor="numberbox" FieldName="validdat" Format="" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" />
                    <JQTools:JQToolItem Enabled="True" ItemType="easyui-linkbutton" Text="轉續約單" Visible="True" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="續約年" Condition="&gt;=" DataType="string" Editor="infocombobox" EditorOptions="items:[{value:'2013',text:'2013',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="DUEDAT" Format="yyyy/mm" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="月" Condition="=" DataType="string" Editor="infocombobox" EditorOptions="items:[{value:'01',text:'01',selected:'false'},{value:'02',text:'02',selected:'false'},{value:'03',text:'03',selected:'false'},{value:'04',text:'04',selected:'false'},{value:'05',text:'05',selected:'false'},{value:'06',text:'06',selected:'false'},{value:'07',text:'07',selected:'false'},{value:'08',text:'08',selected:'false'},{value:'09',text:'09',selected:'false'},{value:'10',text:'10',selected:'false'},{value:'11',text:'11',selected:'false'},{value:'12',text:'12',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="DROPDAT" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="期別" Condition="=" DataType="string" Editor="infocombobox" EditorOptions="items:[{value:'1',text:'上期',selected:'false'},{value:'16',text:'下期',selected:'false'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" FieldName="cusnc" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>
    </form>
</body>
</html>
