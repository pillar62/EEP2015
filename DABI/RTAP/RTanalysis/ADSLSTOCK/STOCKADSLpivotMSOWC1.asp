<!--#include virtual="/WEBAP/INCLUDE/PMSOWC.inc" -->
<%
Sub srSpec()
    title="ADSL券商專案--業務組別業績分析"
    unit="戶數單位:(戶) "
    diaProgram="DIALOGC.ASP"
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    parmSQL="SELECT * FROM ADSLV1 where " & sql & " order by 收件週 "
    defaultRowField="轄區;業務員"
    defaultColumnField="業務組別"
    defaultFilterField="評估結果"
    fieldFormat="#,##0"
    fieldTotal="合計戶數"
    fieldTotalBase="客戶名稱" 
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="2"
    fieldFormat="#,##0.00"     
    fieldTotalShow="True"
    defaultcharttype="0"
    defaultchartlabel="3"
    defaultexpandrow=""
    defaultexpandcolumn=""
End Sub
%>