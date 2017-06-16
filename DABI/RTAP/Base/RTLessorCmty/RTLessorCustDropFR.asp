<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONN
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")  
   SET RSzz=Server.CreateObject("ADODB.RECORDSET")     
   DSN="DSN=RtLib"
   conn.Open DSN
   'conn.BeginTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   '因為當STORE PROCEDURE內OPEN太多TABLE時，ASP無法控制CURSOR而會發生錯誤(不明瞭可以將BEGIN、COMMIT、ROLLBACK的MARK移除並執行後即知)
 
   sqlxx="select * FROM RTLessorCUSTDROP WHERE CUSID='" & KEY(0) & "' and ENTRYNO=" & key(1) 
   sqlyy="select * FROM RTLessorCUST WHERE CUSID='" & KEY(0) & "'"
   RSXX.OPEN SQLXX,CONN
   RSyy.OPEN SQLyy,CONN
   if LEN(TRIM(rsxx("batchno"))) = 0 THEN 
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorCustDROPFR " & "'" & key(0) & "'," & key(1) & ",'" & V(0) & "'" 
      Set ObjRS = conn.Execute(strSP)
      If Err.number = 0 then
         ENDPGM="1"
         ERRMSG=""
         'conn.CommitTrans
      else
         ENDPGM="2"
         errmsg=cstr(Err.number) & "=" & Err.description
         'conn.rollbackTrans
      end if         
   ELSE
   sqlzz="select * FROM RTLessorCUSTar WHERE CUSID='" & KEY(0) & "' and batchno='" & rsxx("batchno") & "' "   
   rszz.open sqlzz,conn
   '此退租單尚未結案，不可執行結案返轉
'   response.write sqlzz
  '    response.end
   IF ISNULL(RSXX("FINISHDAT")) THEN
      ENDPGM="3"
    elseif isnull(RSyy("dropdat"))   then
      endpgm="4"            
    elseif RSzz("realamt") <> 0 or len(trim(rszz("mdat"))) > 0  then
      endpgm="5"                
   ELSE
'    response.write "bbb"
'      response.end
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorCustDROPFR " & "'" & key(0) & "'," & key(1) & ",'" & V(0) & "'" 
    '  response.write strSP
    '  response.end     
      Set ObjRS = conn.Execute(strSP)
      If Err.number = 0 then
         ENDPGM="1"
         ERRMSG=""
         'conn.CommitTrans
      else
         ENDPGM="2"
         errmsg=cstr(Err.number) & "=" & Err.description
         'conn.rollbackTrans
      end if         
   END IF
   RSzz.CLOSE
   
   END IF
   RSXX.CLOSE
   RSyy.CLOSE

   
conn.Close
SET RSXX=NOTHING
SET RSyy=NOTHING
SET RSzz=NOTHING
set conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "ET-City用戶退租單結案返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此退租單尚未結案，不可執行結案返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="4" then
       msgbox "此客戶目前並非退租狀態，不可執行退租結案返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="5" then
       msgbox "此退租單之應收帳款已沖帳，不可執行結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                   
    else
       msgbox "無法執行退租單結案返轉作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorcustDROPfr.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>