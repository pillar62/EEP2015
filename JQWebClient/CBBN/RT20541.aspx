<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT20541.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

     <script>
        var usr = getClientInfo('_usercode');
        var workno = Request.getQueryStringByName2("workno"); //個案編號
        var flag = true;
        
        function dgOnloadSuccess() {

            $("#querydataGridMaster").find(".infosysbutton-cl").hide();
            $("#querydataGridMaster").find(".infosysbutton-q").hide();

        }        
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT2054.View_RTSndWork" runat="server" AutoApply="True"
                DataMember="View_RTSndWork" Pagination="False" QueryTitle="完工設定"
                Title="客訴派工單完工作業" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="True" NotInitGrid="False" OnLoadSuccess="dgOnloadSuccess" ColumnsHibeable="True" DuplicateCheck="True" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="派工別 (P6)" Editor="text" FieldName="WORKTYPE" Format="" MaxLength="2" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="完工方式 (P9)" Editor="text" FieldName="FINISHTYP" Format="" MaxLength="2" Width="120" />
                    <JQTools:JQGridColumn Alignment="left" Caption="派工單號(Wyyyymmddxxx)" Editor="text" FieldName="WORKNO" Format="" MaxLength="12" Width="120" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-ok" ItemType="easyui-linkbutton"
                        OnClick="btn1Click" Text="完工" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="請選擇完工種類：" Condition="%" DataType="string" Editor="infocombogrid" FieldName="FINISHTYP" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" EditorOptions="title:'JQRefval',panelWidth:350,panelHeight:200,remoteName:'sRT100.RTCode',tableName:'RTCode',columns:[],columnMatches:[],whereItems:[{field:'KIND',value:'P9'}],valueField:'CODE',textField:'CODENC',valueFieldCaption:'CODE',textFieldCaption:'CODENC',cacheRelationText:false,checkData:false,showValueAndText:false,dialogCenter:false,selectOnly:false,capsLock:'none',fixTextbox:'false'" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="另匯出至行程表：" Condition="%" DataType="string" Editor="infocombobox" FieldName="WORKTYPE" IsNvarChar="False" NewLine="True" RemoteMethod="False" RowSpan="0" Span="0" Width="125" EditorOptions="items:[{value:'Y',text:'Y',selected:'false'},{value:'N',text:'N',selected:'true'}],checkData:false,selectOnly:false,cacheRelationText:false,panelHeight:200" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
