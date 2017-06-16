<!--#include virtual="/WEBAP/INCLUDE/PMSOWC.inc" -->
<%
Sub srSpec()
    title="HI-Building 完工業績--(年/季/月/週/日)資料分析"
    unit="戶數單位:(戶) "
    diaProgram="DIALOGa.ASP"
    diaWidth=600
    diaHeight=450
    parmDSN="RTLib"
    parmSQL="SELECT * FROM RTSalesV2 where " & sql & " and 完工狀況='已完工' order by 週 "
    defaultRowField="年;季;月;週;日"
    defaultColumnField="社區類別;轄區;客戶類別;業務員"
    defaultFilterField="施工廠商;施工人員類別"
    fieldFormat="#,##0"
    fieldTotal="合計戶數"
    fieldTotalBase="客戶名稱" 
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="2"
    fieldFormat="#,##0.00"     
    fieldTotalShow="True"
    defaultcharttype="0"
    defaultchartlabel="2"
    defaultexpandrow="年"
    defaultexpandcolumn="社區類別"
End Sub
%>