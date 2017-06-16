<!--#include virtual="/WEBAP/INCLUDE/PMSOWC.inc" -->
<%
Sub srSpec()
    title="HI-Building 業績(總業績)數量 分析"
    unit="戶數單位:(戶) "
    diaProgram="DIALOGC.ASP"
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    parmSQL="SELECT * FROM RTSalesV2 where " & sql & " order by 週 "
    defaultRowField="年;月;週;日"
    defaultColumnField="社區類別;T1開通狀況;客戶類別;完工狀況"
    defaultFilterField="施工人員類別;轄區"
    fieldFormat="#,##0"
    fieldTotal="合計戶數"
    fieldTotalBase="客戶名稱" 
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="2"
    fieldTotalShow="True"
    defaultcharttype="0"
    defaultchartlabel="2"
    defaultexpandrow="年"
    defaultexpandcolumn="社區類別"
End Sub
%>