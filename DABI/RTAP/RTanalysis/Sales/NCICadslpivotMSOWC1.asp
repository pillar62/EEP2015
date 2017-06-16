<!--#include virtual="/webap/rtap/rtanalysis/sales/PMSOWCTncic.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
Sub srSpec()
 '---呼叫 STORE PROCEDURE "USP_HBADSLMONTHSCORE" 產生RTCUSTTOT
 '---910703 改由sql設定時間執行procedure，不由程式呼叫
 '---910703 modify start
    KEY=REQUEST("KEY")
    KEYARY=SPLIT(KEY,";")
    IF LEN(TRIM(KEYARY(0)))=0 THEN KEYARY(0)="1911/01/01 00:00:00"
    IF LEN(TRIM(KEYARY(1)))=0 THEN KEYARY(1)="9999/12/31 23:59:59"
    logonid=session("userid")
    Call SrGetEmployeeRef(Rtnvalue,1,logonid)
    V=split(rtnvalue,";")  
    Set connXX=Server.CreateObject("ADODB.Connection")  
    DSNXX="DSN=RtLib"
    
    connXX.Open DSNXX
    strSP="USP_RTNCICCUSTOT " & V(0) & ",'" & KEYARY(0) & "','" & KEYARY(1) & "'"
   ' Response.Write "STRSP=" & STRSP
    Set ObjRS = connXX.Execute(strSP)      
    connXX.Close
    SET CONNXX=NOTHING 
 '910703 modify end
 '------------------------------------------------------------ 
    title="速博399業績統計表"
    unit="戶數單位:(戶) "
    diaProgram=""
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    'parmSQL="SELECT * FROM RTCUSTTOT where temply='" & V(0) & "' order by 年,月 "
    '---910703修改為不挑執行人員
    parmSQL="SELECT * FROM RTNCICCUSTTOT WHERE EUSR='" & V(0) & "'  order by 拆帳歸屬,業務組別,方案 "    
    defaultRowField="方案;拆帳歸屬;業務組別"
    defaultColumnField=""
    defaultFilterField=""
    fieldFormat="#,##0"
    fieldTotal="總送線數;總開通數;申請戶;完工戶;報竣戶"
    fieldTotalBase="送件線數;開通線數;申請戶數;完工戶數;報竣戶數" 
    
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="1;1;1;1;1"
    fieldTotalShow="True;True;True;True;True"
    defaultcharttype="0"
    defaultchartlabel="1"
   ' defaultexpandrow="年;月"
    defaultexpandcolumn=""
    defaultexpandrow="方案;拆帳歸屬"
  '  defaultexpandcolumn=""    
End Sub
%>