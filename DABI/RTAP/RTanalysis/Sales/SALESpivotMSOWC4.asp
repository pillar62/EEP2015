<!--#include virtual="/WEBAP/INCLUDE/PMSOWC.inc" -->
<%
Sub srSpec()
    title="HI-Building 完工業績--(轄區/縣市/業務員)資料分析"
    unit="戶數單位:(戶) "
    diaProgram="DIALOGa.ASP"
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    parmSQL="SELECT * FROM RTSalesV2 where " & sql & " and 完工狀況='已完工' order by 週 "
    defaultRowField="轄區;社區類別;客戶類別;業務員;縣市"
    defaultColumnField="年;季;月;週;日"
    defaultFilterField="完工狀況"
    fieldFormat="#,##0"
    fieldTotal="合計戶數"
    fieldTotalBase="客戶名稱" 
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="2"
    fieldFormat="#,##0.00"     
    fieldTotalShow="True"
    defaultcharttype="12"
    defaultchartlabel="2"
    defaultexpandrow="轄區;社區類別"
    defaultexpandcolumn="年"
End Sub
%>