<!--#include virtual="/WebAP/include/PMSOWC4.inc" -->
<%
Sub srSpec()
    title="HI-Building業績資料分析"
    diaProgram="Dialogc.asp"
    diaWidth=450
    diaHeight=450
    parmDSN="RTLib"
    parmSQL="SELECT * FROM RTTEST1 where 轄區" & sql  
    defaultRowField="轄區;縣市;工程師"
    defaultColumnField="客戶類別"
    defaultFilterField="施工人員;施工人員分類"
    fieldFormatName="客戶名稱"
    fieldFormat="#,##0"
    fieldTotal="合計戶數"
    fieldTotalBase="客戶名稱" 
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="2"
    fieldTotalShow="True"
End Sub
%>