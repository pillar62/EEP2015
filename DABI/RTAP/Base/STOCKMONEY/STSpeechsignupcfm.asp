<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=STOCK;uid=sa;pwd=alittlecat@cbn"
   connXX.Open DSN
   endpgm="1"
 '  On Error Resume Next

   sqlxx="select * FROM speechsignup WHERE yymmdd='" & KEY(0) & "' and email='" & key(1) & "' " 
   rsxx.Open sqlxx,connxx
   canceldat=rsxx("canceldat")
   confirmdat=rsxx("confirmdat")
   RSXX.CLOSE
   if LEN(TRIM(CANCELDAT)) > 0 then
     endpgm="3"
   ELSEif LEN(TRIM(confirmdat)) > 0 then
     endpgm="4"  
   else
      sqlyy="select max(SEQ) as SEQ FROM Speechsignuplog WHERE YYMMDD='" & KEY(0) & "' and EMAIL='" & key(1) & "'"
      rsyy.Open sqlyy,connxx
      if len(trim(rsyy("SEQ"))) > 0 then
         SEQ=rsyy("SEQ") + 1
      else
         SEQ=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into Speechsignuplog " _
           &"SELECT  YYMMDD, EMAIL," & SEQ & ",getdate(),'CF','" & V(0) & "', NAME, TEL,  LIVEORNET, CONFIRMDAT, CONFIRMUSR, CANCELDAT, CANCELUSR,  APPLYDAT   " _
           &"FROM SpeechSignup where yymmdd='" & key(0) & "' and email='" & key(1) & "' "  
    ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update speechsignup set confirmdat=getdate(),confirmUSR='" & V(0) & "' where yymmdd='" & key(0) & "' and email='" & key(1) & "' "  
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM SpeechSignuplog WHERE yymmdd='" & key(0) & "' and email='" & key(1) & "' and SEQ=" & SEQ
            CONNXX.Execute sqlyy 
         else
            endpgm="1"
            errmsg=""
         end if      
       end if
     END IF
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "演講報名資料審核成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此演講報名資料已作廢，不可執行審核作業(欲審核請先執行作廢返轉)" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此演講報名資料已審核完成，不可重複執行" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    else
       msgbox "無法執行演講報名審核,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=STSPEECHSIGNUPCFM.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>