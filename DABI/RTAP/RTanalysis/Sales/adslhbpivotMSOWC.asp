<!--#include virtual="/WEBAP/INCLUDE/PMSOWCT.inc" -->
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
    title="ADSL+HI-Building 業績(總業績)數量 分析"
    unit="戶數單位:(戶) "
    diaProgram=""
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    'parmSQL="SELECT * FROM RTCUSTTOT where temply='" & V(0) & "' order by 年,月 "
    '---910703修改為不挑執行人員
    parmSQL="SELECT * FROM RTCUSTTOT order by 年,月 "    
    defaultRowField="利潤中心;拆帳模式;方案;年;月"
    defaultColumnField="客戶數;拆帳歸屬;客戶開發"
    defaultFilterField=""
    fieldFormat="#,##0"
    fieldTotal="完工戶;報竣戶;退租戶;欠拆戶;公關戶;增減戶"
    fieldTotalBase="完工;報竣;退租;欠拆;公關;增減" 
    
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="1;1;1;1;1;1"
    fieldTotalShow="True;True;True;True;True;True"
    defaultcharttype="0"
    defaultchartlabel="2"
   ' defaultexpandrow="年;月"
    defaultexpandcolumn=""
    defaultexpandrow=""
  '  defaultexpandcolumn=""    
End Sub
%>