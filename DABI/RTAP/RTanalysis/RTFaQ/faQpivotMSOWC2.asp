<!--#include virtual="/WEBAP/INCLUDE/PMSOWC.inc" -->
<%
Sub srSpec()
    title="HI-Building 客訴案件資料分析"
    unit="戶數單位:(戶) "
    diaProgram="DIALOGa.ASP"
    diaWidth=600
    diaHeight=450
    parmDSN="RTLib"
    parmSQL="SELECT * FROM RTfaqV1 where " & sql
    defaultRowField="客訴產品;處理進度"
    defaultColumnField="受理年;受理月"
    defaultFilterField="裝機員工;裝機廠商;裝機員類別"
    fieldFormat="#,##0"
    fieldTotal="合計戶數"
    fieldTotalBase="客戶名稱" 
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="2"
    fieldFormat="#,##0.00"     
    fieldTotalShow="True"
    defaultcharttype="13"
    defaultchartlabel="2"
    defaultexpandrow=""
    defaultexpandcolumn="受理年"
End Sub
%>