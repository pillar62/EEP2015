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
    strSP="USP_RTAVSLINESETUP '" & V(0) & "','" & KEYARY(0) & "','" & KEYARY(1) & "'"
    'Response.Write "STRSP=" & STRSP
    Set ObjRS = connXX.Execute(strSP)      
    connXX.Close
    SET CONNXX=NOTHING 
 '910703 modify end
 '------------------------------------------------------------ 
    title="東森AVS499工務主線裝機統計"
    unit="戶數單位:(戶) "
    diaProgram=""
    diaWidth=550
    diaHeight=400
    parmDSN="RTLib"
    'parmSQL="SELECT * FROM RTAVSLINESETUPTOT WHERE EUSR='" & V(0) & "' "
    parmSQL="SELECT * FROM RTAVSLINESETUPTOT WHERE 派工單號 is not null and EUSR='" & V(0) & "' "
'response.Write parmSQL    
    defaultRowField="拆帳歸屬;測通年;測通月;週;裝機人員;派工種類"
    defaultColumnField=""
    defaultFilterField=""
    fieldFormat="#,##0"
    fieldTotal="主線數"
    fieldTotalBase="社區名稱" 
    
'------ 1:Sum 2:Count 3:Min 4:Max
    fieldTotalFunction="2"
    fieldTotalShow="True"
    defaultcharttype="0"
    defaultchartlabel="2"
   ' defaultexpandrow="年;月"
    defaultexpandcolumn=""
    defaultexpandrow="拆帳歸屬;測通年;測通月;週"
  '  defaultexpandcolumn=""    
End Sub
%>