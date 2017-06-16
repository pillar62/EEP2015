<!--#include virtual="/WEBAP/INCLUDE/PMSOWC.inc" -->
<%
Sub srSpec()
    title="HI-Building 完工業績--施工類別(業務/技術部/廠商)安裝比率 分析"
    unit="戶數單位:(戶) "
    diaProgram="DIALOGa.ASP"
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    parmSQL="SELECT * FROM RTSalesV2 where " & sql & " and 完工狀況='已完工' order by 週 "
    defaultRowField="社區類別;施工人員類別;轄區;縣市"
    defaultColumnField="年;月;週"
    defaultFilterField="施工廠商;安裝工程師"
    fieldFormat="#,##0"
    fieldTotal="合計戶數"
    fieldTotalBase="客戶名稱" 
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="2"
    fieldTotalShow="True"
    defaultcharttype="12"
    defaultchartlabel="1"
    defaultexpandrow="社區類別"
    defaultexpandcolumn="年"
End Sub
%>