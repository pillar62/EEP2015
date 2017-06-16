<!--#include virtual="/webap/rtap/rtanalysis/sales/PMSOWCTavs2.inc" -->
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
    strSP="USP_RTAVSCUSTstatus " & V(0) & ",'" & KEYARY(0) & "','" & KEYARY(1) & "'"
   ' Response.Write "STRSP=" & STRSP
    Set ObjRS = connXX.Execute (strSP)      
    connXX.Close
    SET CONNXX=NOTHING 
 '910703 modify end
 '------------------------------------------------------------ 
    title="東森AVS499用戶派工狀態統計"
    unit="戶數單位:(戶) "
    diaProgram=""
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    'parmSQL="SELECT * FROM RTCUSTTOT where temply='" & V(0) & "' order by 年,月 "
    '---910703修改為不挑執行人員
    parmSQL="SELECT * FROM RTAVSCUSTSTATUS WHERE EUSR='" & V(0) & "'  "    
    defaultRowField="預計施工;派工年;派工月;派工週"
    defaultColumnField="實際施工"
    defaultFilterField="拆帳歸屬;組別"
    fieldFormat="#,##0"
    fieldTotal="派工件數"
    fieldTotalBase="派工日" 
    
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="2"
    fieldTotalShow="True"
    defaultcharttype="0"
    defaultchartlabel="2"
   ' defaultexpandrow="年;月"
    defaultexpandcolumn=""
    defaultexpandrow=""
  '  defaultexpandcolumn=""    
End Sub
%>