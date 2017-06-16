<!--#include virtual="/WebAP/include/PMSOWC.inc" -->
<%
Sub srSpec()
    title="HI-Building 券商營業員業績資料分析"
    diaProgram="Dialogc.asp"
    diaWidth=450
    diaHeight=450
    parmDSN="RTLib"
    parmSQL="SELECT * FROM RTSalesV2 where 轄區" & sql& " and 完工狀況='已完工' "  
    defaultRowField="轄區;縣市"
    defaultColumnField="營業員"
    defaultFilterField="施工廠商;施工人員類別"
    fieldFormatName="客戶名稱"
    fieldFormat="#,##0"
    fieldTotal="合計戶數"
    fieldTotalBase="客戶名稱" 
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="2"
    fieldTotalShow="True"
    defaultcharttype="0"
    defaultchartlabel="2"
    defaultexpandrow=""
    defaultexpandcolumn=""    
End Sub
%>