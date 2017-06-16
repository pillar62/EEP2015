<!--#include virtual="/WEBAP/INCLUDE/PMSOWCT.inc" -->
<!--#include virtual="/Webap/include/employeeref.inc" -->
<%
Sub srSpec()
 '---呼叫 STORE PROCEDURE "USP_HBADSLMONTHSCORE" 產生RTCUSTTOT
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
 '------------------------------------------------------------ 
    title="各方案線數、戶數統計(by年、月)"
    unit="單位:(線數、戶數) "
    diaProgram=""
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    parmSQL="SELECT * FROM RTCustALL order by 年,月 "
    defaultRowField="年;月"
    defaultColumnField="方案"
    defaultFilterField="轄區;直經銷;業務"
    fieldTotal="進線;撤線;有效(線);報竣戶;退租戶;有效(戶)"
    fieldTotalBase="進線數;撤線數;線數異動;報竣數;退租數;戶數異動"
    
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="1;1;1;1;1;1;1"
    fieldTotalShow="True;True;True;True;True;True;True"
    fieldFormat="#,##0"
    defaultcharttype="0"
    defaultchartlabel="3"
	defaultexpandcolumn=""
	defaultexpandrow=""
End Sub
%>
