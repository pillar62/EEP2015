<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   dim ObjRS,strSP,endpgm,prtno
       prtno=session("COTcanprtno")
       strSP="USP_okcancel 'E','"& prtno &"','"& V(0) &"'"
   Set conn=Server.CreateObject("ADODB.Connection")  
       DSN="DSN=RtLib"
       conn.Open DSN
   On Error Resume Next
       Set ObjRS = conn.Execute(strSP)      
       conn.Close
   If Err.number > 0 then
      endpgm="2"
      errmsg=cstr(Err.number) & "=" & Err.description
   else
      endpgm="1"
      errmsg=""
   end if
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.HTMLfld.value="1" then
       msgbox "COT«Ø¸m¦Û¥IÃB¼f®ÖºM¾P",0
      Set winP=window.Opener
      Set docP=winP.document
      docP.all("keyform").Submit
      winP.focus()       
    else
       msgbox "µLªk¦C¦L,¿ù»~°T®§¡G" & "  " & errmsg
    end if
  window.close
 end sub
</script>   
<form name=frm1 method=post action=verify.asp>
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form> 