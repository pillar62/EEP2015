<!--#include virtual="/WEBAP/INCLUDE/PMSOWCT.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
Sub srSpec()
 '---呼叫 STORE PROCEDURE "USP_HBCUSTDROPEXEC" 產生HBCUSTDROPEXEC
    logonid=session("userid")
    Call SrGetEmployeeRef(Rtnvalue,1,logonid)
    V=split(rtnvalue,";")  
    Set connXX=Server.CreateObject("ADODB.Connection")  
    DSNXX="DSN=RtLib"
    
    connXX.Open DSNXX
    strSP="USP_HBCUSTDROPEXEC " & V(0)
    Set ObjRS = connXX.Execute(strSP)      
    connXX.Close
    SET CONNXX=NOTHING 
 '910703 modify end
 '------------------------------------------------------------ 
    title="退租欠拆復機執行率數值 分析"
    unit="戶數單位:(戶) "
    diaProgram=""
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    'parmSQL="SELECT * FROM RTCUSTTOT where temply='" & V(0) & "' order by 年,月 "
    '---910703修改為不挑執行人員
    parmSQL="SELECT * FROM HBCUSTDROPEXEC WHERE FLAG='" & V(0) & "' " 
    defaultRowField="區域;方案;狀態"
    defaultColumnField="年;月"
    defaultFilterField=""
    fieldFormat="#,##0"
    fieldTotal="拆機數;欠轉退;回報數;待拆總數"
    fieldTotalBase="拆機戶;欠轉退戶;回報戶;全部欠退戶" 
'   fieldFormatName="用戶"    
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="1;1;1;1"
    fieldTotalShow="True;True;True;True"
    defaultcharttype="0"
    defaultchartlabel="2"
   ' defaultexpandrow="年;月"
    defaultexpandcolumn=""
    defaultexpandrow=""
  '  defaultexpandcolumn=""    
End Sub
%>