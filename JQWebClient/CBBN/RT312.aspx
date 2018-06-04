<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RT312.aspx.cs" Inherits="Template_JQueryQuery1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
     <script>
        //列印
        function btnPrintClick() {
            var WhereString = "";
            exportDevReport("#dataGridMaster", "sRT312.RT312", "RT312", "~/CBBN/DevReportForm/RT312RF.aspx", WhereString);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <JQTools:JQScriptManager ID="JQScriptManager1" runat="server" />
            <JQTools:JQDataGrid ID="dataGridMaster" data-options="pagination:true,view:commandview" RemoteName="sRT312.RT312" runat="server" AutoApply="True"
                DataMember="RT312" Pagination="True" QueryTitle="查詢條件"
                Title="遠傳大寬頻報竣客戶一覽表" AllowDelete="False" AllowInsert="False" AllowUpdate="False" QueryMode="Panel" AlwaysClose="True" AllowAdd="False" ViewCommandVisible="False" BufferView="False" CheckOnSelect="True" ColumnsHibeable="False" DeleteCommandVisible="False" DuplicateCheck="False" EditMode="Dialog" EditOnEnter="True" InsertCommandVisible="False" MultiSelect="False" NotInitGrid="False" PageList="10,20,30,40,50" PageSize="10" QueryAutoColumn="False" QueryLeft="" QueryTop="" RecordLock="False" RecordLockMode="None" RowNumbers="True" TotalCaption="Total:" UpdateCommandVisible="False">
                <Columns>
                    <JQTools:JQGridColumn Alignment="left" Caption="流水號" Editor="text" FieldName="seq" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="申請日期" Editor="text" FieldName="applydat" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="竣工日期" Editor="text" FieldName="docketdat" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="docketdatq" Editor="datebox" FieldName="docketdatq" Format="" Width="80" Visible="False" />
                    <JQTools:JQGridColumn Alignment="left" Caption="申請種類" Editor="text" FieldName="applykind" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="異動代碼" Editor="text" FieldName="updcode" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="電話號碼" Editor="text" FieldName="updtel" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="對帳號碼" Editor="text" FieldName="memberid" Format="" MaxLength="16" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="協力商代碼" Editor="text" FieldName="mak_id" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="業務員代碼" Editor="text" FieldName="sale_id" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="客戶類別" Editor="text" FieldName="cust_kind" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="公司名稱" Editor="text" FieldName="company_name" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="公司負責人" Editor="text" FieldName="coboss" Format="" MaxLength="30" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="負責人身份證字號" Editor="text" FieldName="cobossid" Format="" MaxLength="10" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="公司統編" Editor="text" FieldName="company_id" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="服務方案" Editor="text" FieldName="case_no" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="優惠代碼" Editor="text" FieldName="ID_DIS" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="用戶名稱" Editor="text" FieldName="cusnc" Format="" MaxLength="30" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡人證照類別" Editor="text" FieldName="codenc" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡人證照號碼" Editor="text" FieldName="socialid" Format="" MaxLength="10" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡人稱謂" Editor="text" FieldName="sex_KIND" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡人名稱" Editor="text" FieldName="contact_name" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡人聯絡電話" Editor="text" FieldName="contact_tel" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡人出生日期" Editor="text" FieldName="contact_birth" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="聯絡人行動電話" Editor="text" FieldName="contact_mobile" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="代理人證照類別" Editor="text" FieldName="agent_cardtype" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="代理人證照號碼" Editor="text" FieldName="agent_idno" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="代理人稱謂" Editor="text" FieldName="agent_callname" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="代理人名稱" Editor="text" FieldName="agent_name" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="代理人聯絡電話" Editor="text" FieldName="agent_tel" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="代理人出生日期" Editor="text" FieldName="agent_birth" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳寄郵遞區號" Editor="text" FieldName="zip3" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳寄縣市" Editor="text" FieldName="cutnc3" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳寄鄉鎮市區" Editor="text" FieldName="township3" Format="" MaxLength="10" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="帳寄地址" Editor="text" FieldName="raddr3" Format="" MaxLength="60" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="戶籍郵遞區號" Editor="text" FieldName="zip2" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="戶籍縣市" Editor="text" FieldName="cutnc2" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="戶籍鄉鎮市區" Editor="text" FieldName="township2" Format="" MaxLength="10" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="戶籍地址" Editor="text" FieldName="raddr2" Format="" MaxLength="60" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="裝機郵遞區號" Editor="text" FieldName="zip1" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="裝機縣市" Editor="text" FieldName="cutnc1" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="裝機鄉鎮市區" Editor="text" FieldName="township1" Format="" MaxLength="10" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="裝機地址" Editor="text" FieldName="raddr1" Format="" MaxLength="60" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="社區名稱" Editor="text" FieldName="comn" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="IP ADDRESS FROM" Editor="text" FieldName="ip11s" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="IP ADDRESS END" Editor="text" FieldName="ip11e" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="NCIC預處理日期" Editor="text" FieldName="ncicdate" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="第二證照類別" Editor="text" FieldName="codenc2" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="第二證照號碼" Editor="text" FieldName="secondno" Format="" MaxLength="15" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="申請書編號" Editor="text" FieldName="apply_no" Format="" MaxLength="0" Width="80" />
                    <JQTools:JQGridColumn Alignment="left" Caption="PAYCYCLE" Editor="text" FieldName="paycycle" Format="" MaxLength="2" Width="80" Visible="False" />
                </Columns>
                <TooItems>
                    <JQTools:JQToolItem Icon="icon-search" ItemType="easyui-linkbutton"
                        OnClick="openQuery" Text="查詢" Visible="False" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="btnPrintClick" Text="匯出Excel檔" Visible="True" />
                    <JQTools:JQToolItem Enabled="True" Icon="icon-excel" ItemType="easyui-linkbutton" OnClick="exportGrid" Text="匯出Excel" Visible="True" />
                </TooItems>
                <QueryColumns>
                    <JQTools:JQQueryColumn AndOr="and" Caption="報竣日期起" Condition="&gt;=" DataType="datetime" Editor="datebox" FieldName="docketdatq" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                    <JQTools:JQQueryColumn AndOr="and" Caption="報竣日期迄" Condition="&lt;=" DataType="datetime" DefaultValue="_today" Editor="datebox" FieldName="docketdatq" IsNvarChar="False" NewLine="False" RemoteMethod="False" RowSpan="0" Span="0" Width="125" />
                </QueryColumns>
            </JQTools:JQDataGrid>
        </div>

    </form>
</body>
</html>
