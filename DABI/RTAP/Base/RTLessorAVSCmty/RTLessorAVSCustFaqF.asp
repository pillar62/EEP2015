<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONN
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   conn.Open DSN
   'conn.BeginTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   '因為當STORE PROCEDURE內OPEN太多TABLE時，ASP無法控制CURSOR而會發生錯誤(不明瞭可以將BEGIN、COMMIT、ROLLBACK的MARK移除並執行後即知)
 
   sqlxx="select * FROM RTLessorAVSCUSTFaqH WHERE CUSID='" & KEY(0) & "' and FAQNO='" & key(1) & "' "
'response.Write sqlxx
'response.End
   RSXX.OPEN SQLXX,CONN
   '當已作廢時，不可執行客服單結案
   IF LEN(TRIM(RSXX("CANCELDAT"))) <> 0 THEN
      ENDPGM="3"
   elseif LEN(TRIM(RSXX("FINISHDAT"))) <> 0 then
      endpgm="4"
   elseif LEN(TRIM(RSXX("SNDPRTNO"))) <> 0 AND ISNULL(rsxx("SNDCLOSEDAT")) then
      endpgm="5"
   elseif LEN(TRIM(RSXX("SNDPRTNO"))) <> 0 AND LEN(TRIM(RSXX("SNDCLOSEDAT"))) <> 0 AND ISNULL(rsxx("CALLBACKDAT")) then
      endpgm="6"      
   ELSE
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorAVSCustFAQF " & "'" & key(0) & "'" & ",'" & key(1) & "','" & V(0) & "'" 
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
   RSXX.CLOSE
conn.Close
SET RSXX=NOTHING
set conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS-City用戶客服單結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當已作廢時，不可執行客服單結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此客服單已結案，不可重複執行" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此客服單已轉派工單，派工單需結案後始可執行客服單結案作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    elseIF frm1.htmlfld.value="6" then
       msgbox "此客服單已轉派工單，請先回覆用戶確認故障已排除再執行押回覆日後，始可執行客服單結案作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close             
    else
       msgbox "無法執行客服單結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorAVScustFAQf.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>