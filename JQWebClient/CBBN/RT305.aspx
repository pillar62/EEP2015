<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT305.aspx.cs" Inherits="Template_JQuerySingle1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script>
        var COMQ1 = Request.getQueryStringByName2("COMQ1");
        var LINEQ1 = Request.getQueryStringByName2("LINEQ1");
        var COMTYPE = Request.getQueryStringByName2("COMTYPE");
        var usr = getClientInfo('_usercode');
        var flag = true;

        $(document).ready(function () {
            dgOnloadSuccess();
        })

        function dgOnloadSuccess() {
            if (flag) {
                var sWhere = " A.COMQ1='" + COMQ1 + "'";
                if (LINEQ1 != "") {
                    sWhere = sWhere + " AND A.LINEQ1='" + LINEQ1 + "'"
                }
                if (COMTYPE != "") {
                    sWhere = sWhere + " AND A.COMTYPE='" + COMTYPE + "'"
                }
                $('#dataGridMaster').datagrid('setWhere', sWhere);//篩選資料                
            }

            flag = false;
        }

        function btn1Click(val) {
            var sMODE = "I";
            var row = $('#dataGridMaster').datagrid('getSelected');//取得當前主檔中選中的那個Data
            var comq1 = row.COMQ1;
            var lineq1 = row.LINEQ1;
            var comtype = row.COMTYPE;
            var cusid = row.CUSID;
            parent.addTab("各方案客訴維護", "CBBN/RT205.aspx?comq1=" + COMQ1 + "&sMODE=" + sMODE + "&comq1=" + comq1 + "&lineq1=" + lineq1 + "&comtype=" + comtype + "&cusid=" + cusid);
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT305.cmRT305" runat="server" AutoApply="True"
                DataMember="cmRT305" Pagination="True" QueryTitle="Query"
                Title="客戶資料查詢" AllowAdd="True" AllowDelete="True" AllowUpdate="True" AlwaysClose="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="True" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="False" InsertCommandVisible="True" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryMode="Window" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="True" ViewCommandVisible="True" OnLoadSuccess="dgOnloadSuccess">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="個案別" Editor="text" FieldName="COMTYPE" Format="" MaxLength="2" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="right" Caption="社區" Editor="numberbox" FieldName="COMQ1" Format="" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="right" Caption="主線" Editor="numberbox" FieldName="LINEQ1" Format="" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶編號" Editor="text" FieldName="CUSID" Format="" MaxLength="20" Width="120" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="項次" Editor="text" FieldName="ENTRYNO" Format="" Width="40" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="COMN" Format="" MaxLength="50" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶名稱" Editor="text" FieldName="CUSNC" Format="" MaxLength="100" Width="60" />
                    <JQTools:JQGridColumn Alignment="left" Caption="個案別" Editor="text" FieldName="COMTYPENC" Format="" Width="120" MaxLength="20" />
                    <JQTools:JQGridColumn Alignment="left" Caption="申請日" Editor="datebox" FieldName="RCVDAT" Format="yyyy/mm/dd" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工日" Editor="datebox" FieldName="FINISHDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="報竣日" Editor="datebox" FieldName="DOCKETDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="退租日" Editor="datebox" FieldName="DROPDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="作廢日" Editor="datebox" FieldName="CANCELDAT" Format="yyyy/mm/dd" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="裝機地址" Editor="text" FieldName="RADDR" Format="" MaxLength="150" Width="120" />
                    <JQTools:JQGridColumn Alignment="right" Caption="客訴次數" Editor="numberbox" FieldName="CASENO" Format="" Width="60" />
                    <JQTools:JQGridColumn Alignment="right" Caption="未結數" Editor="numberbox" FieldName="yn_close" Format="" Width="60" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-add" ItemType="easyui-linkbutton" OnClick="insertItem" Text="新增" />
                    <JQTools:JQToolItem Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="updateItem" Text="更改" />
                    <JQTools:JQToolItem Icon="icon-remove" ItemType="easyui-linkbutton" OnClick="deleteItem" Text="刪除" />
                    <JQTools:JQToolItem Icon="icon-save" ItemType="easyui-linkbutton" OnClick="apply" Text="確定" />
                    <JQTools:JQToolItem Icon="icon-undo" ItemType="easyui-linkbutton" OnClick="cancel" Text="取消" />
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton" OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-edit" ItemType="easyui-linkbutton" OnClick="btn1Click" Text="客訴維護" Visible="True" />
                </TooItems>
                <QueryColumns>
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>
    <JQTools:JQDefault runat="server" BindingObjectID="dataGridMaster" BorderStyle="NotSet" Enabled="True" EnableTheming="True" ClientIDMode="Inherit" ID="defaultMaster" EnableViewState="True" ViewStateMode="Inherit" >
</JQTools:JQDefault>
<JQTools:JQValidate runat="server" BindingObjectID="dataGridMaster" BorderStyle="NotSet" Enabled="True" EnableTheming="True" ClientIDMode="Inherit" ID="validateMaster" EnableViewState="True" ViewStateMode="Inherit" >
</JQTools:JQValidate>
</form>
</body>
</html>
