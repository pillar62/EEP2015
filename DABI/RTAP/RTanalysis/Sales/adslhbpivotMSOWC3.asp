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
    title="直銷業績 分析"
    unit="戶數單位:(戶) "
    diaProgram=""
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    'parmSQL="SELECT * FROM RTCUSTTOT where temply='" & V(0) & "' order by 年,月 "
    '---910703修改為不挑執行人員
    parmSQL="SELECT * FROM RTCUSTTOT  where 拆帳歸屬='自營' order by 年,月 "    
    defaultRowField="業務組別;年;月"
    defaultColumnField="客戶數;方案"
    defaultFilterField="拆帳歸屬"
    fieldFormat="#,##0"
    fieldTotal="增減戶"
    fieldTotalBase="增減" 
    
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="1"
    fieldTotalShow="True"
    defaultcharttype="0"
    defaultchartlabel="2"
   ' defaultexpandrow="年;月"
    defaultexpandcolumn="客戶數"
    defaultexpandrow=""
  '  defaultexpandcolumn=""    
End Sub
%>