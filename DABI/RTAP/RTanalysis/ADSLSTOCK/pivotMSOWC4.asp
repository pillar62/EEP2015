<!--#include virtual="/WebUtility/MSOWC/PMSOWC4.inc" -->
<%
Sub srSpec()
    title="化纖銷售資料分析"
    unit="數量單位:KG*100  &nbsp;&nbsp;金額單位:新台幣仟元"
    diaProgram=""
    diaWidth=450
    diaHeight=450
    selYY=Right("00" &Request("selYY"),2)
    selMM=Right("00" &Request("selMM"),2)
    selDateS=selYY &selMM &"01"
    selDateE=selYY &selMM &"31"   
    parmDSN="dsn=coLib"
    parmSQL="SELECT Sdate, Dept, Stype, Ptype,  '銷售量' AS Item,Int(Sum(Sqty)/100) AS Amt " _
           &"FROM sale WHERE Code='05' AND Sdate BETWEEN '" &selDateS &"' AND '" &selDateE &"' " _
           &"GROUP BY Sdate,Dept,Stype,Ptype " _
           &"UNION SELECT Sdate,Dept,Stype,Ptype,'銷貨收入' AS Item ,Int(Sum(SinR)/1000) AS Amt " _
           &"FROM sale WHERE Code='05' AND Sdate BETWEEN '" &selDateS &"' AND '" &selDateE &"' " _
           &"GROUP BY Sdate,Dept,Stype,Ptype " _
           &"UNION SELECT Sdate,Dept,Stype,Ptype,'銷貨成本' AS Item ,Int(Sum(CostR)/1000) AS Amt " _
           &"FROM sale WHERE Code='05' AND Sdate BETWEEN '" &selDateS &"' AND '" &selDateE &"' " _
           &"GROUP BY Sdate,Dept,Stype,Ptype "
    defaultChartType=0
    defaultChartLabel=0
    defaultFilterField="部門"
    defaultRowField="項目"
    defaultColumnField="日期"
    fieldName="日期;部門;銷售別;產品類;項目;值"
    fieldFormatName="值"
    fieldFormat="#,##0" 
    fieldTotal="值合計" 
    fieldTotalBase="值" 
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="1"
    fieldTotalShow="True"
End Sub
%>