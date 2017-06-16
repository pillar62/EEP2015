<!--#include virtual="/WEBAP/INCLUDE/PMSOWC.inc" -->
<%
Sub srSpec()
    title="HI-Building RT,ADSL 裝機費收款金額分析"
    unit="金額單位:新台幣元"
    diaProgram="DIALOGC.ASP"
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    parmSQL="usp_RTReceiptEIS " & sql
    defaultRowField="方案;拆帳歸屬;客戶開發"
    defaultColumnField="完工年;完工月"
    defaultFilterField=""
    fieldFormat="#,##0"
    fieldTotal="金額(千元)"
    fieldTotalBase="收款金額"
    'fieldTotalBase="實收金額" 
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="1"
    fieldFormat="#,##0.00"     
    fieldTotalShow="True"
    defaultcharttype="12"
    defaultchartlabel="2"
    defaultexpandrow="方案"
    'defaultexpandcolumn="完工年"
End Sub
%>