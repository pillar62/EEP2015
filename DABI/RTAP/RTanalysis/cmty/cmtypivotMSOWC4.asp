<!--#include virtual="/WEBAP/INCLUDE/PMSOWC.inc" -->
<%
Sub srSpec()
    title="HI-Building客戶資料分析"
    unit="戶數單位:(戶) "
    diaProgram="DIALOGC.ASP"
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    parmSQL="SELECT * FROM RTSalesV2 where 轄區" & sql 
    defaultChartType="0"
    defaultChartLabel="1"
    defaultRowField="轄區;性別;縣市"
    defaultColumnField="客戶類別;完工狀況"
    defaultFilterField="施工廠商;安裝工程師"
    fieldFormat="#,##0"
    fieldTotal="合計戶數"
    fieldTotalBase="客戶名稱" 
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="2"
    fieldFormat="#,##0.00"     
    fieldTotalShow="True"
    defaultexpandrow="轄區"
    defaultexpandcolumn=""
End Sub
%>