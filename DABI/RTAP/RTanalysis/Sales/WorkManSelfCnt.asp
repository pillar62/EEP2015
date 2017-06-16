<!--#include virtual="/WEBAP/INCLUDE/PMSOWC.inc" -->
<%
Sub srSpec()
    title="HI-Building 完工業績--各業務員自行安裝 數量 分析"
    unit="戶數單位:(戶) "
    diaProgram="DIALOGa.ASP"
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    parmSQL="SELECT * FROM RTSalesV2 where " & sql _
          & " and 完工狀況='已完工' and 施工人員類別='業務自行安裝' order by 週 "
    defaultRowField="轄區;安裝工程師;社區類別;縣市"
    defaultColumnField="年;月;週"
    defaultFilterField="施工廠商;施工人員類別"
    fieldFormat="#,##0"
    fieldTotal="合計戶數"
    fieldTotalBase="客戶名稱" 
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="2"
    fieldTotalShow="True"
    defaultcharttype="0"
    defaultchartlabel="2"
    defaultexpandrow="轄區;安裝工程師"
    defaultexpandcolumn="年"
End Sub
%>