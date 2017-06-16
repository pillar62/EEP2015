<!--#include virtual="/webap/rtap/rtanalysis/sales/PMSOWCT.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
Sub srSpec()
 '---呼叫 STORE PROCEDURE "USP_HBADSLMONTHSCORE" 產生RTCUSTTOT
 '---910703 改由sql設定時間執行procedure，不由程式呼叫
 '---910703 modify start
 '   logonid=session("userid")
 '   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
 '   V=split(rtnvalue,";")  
 '   Set connXX=Server.CreateObject("ADODB.Connection")  
 '   DSNXX="DSN=RtLib"
    
 '   connXX.Open DSNXX
 '   strSP="USP_HBADSLMONTHSCORE " & V(0)
 '   Set ObjRS = connXX.Execute(strSP)      
 '   connXX.Close
 '   SET CONNXX=NOTHING 
 '910703 modify end
 '------------------------------------------------------------ 
    title="方案別客戶成長趨勢 分析"
    unit="戶數單位:(戶) "
    diaProgram=""
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    'parmSQL="SELECT * FROM RTCUSTTOT where temply='" & V(0) & "' order by 年,月 "
    '---910703修改為不挑執行人員
    parmSQL="SELECT 方案, 年, 月, SUM(增減)  FROM RTCustTOT GROUP BY  方案, 年, 月 ORDER BY  方案, 年, 月 "    
    defaultRowField="年;月"
    defaultColumnField=""
    defaultFilterField=""
    fieldFormat="#,##0"
    fieldTotal="累計戶"
    fieldTotalBase="累計" 
    
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="1"
    fieldTotalShow="True"
    defaultcharttype="12"
    defaultchartlabel="0"
   ' defaultexpandrow="年;月"
    defaultexpandcolumn="年"
    defaultexpandrow=""
  '  defaultexpandcolumn=""    
End Sub
%>