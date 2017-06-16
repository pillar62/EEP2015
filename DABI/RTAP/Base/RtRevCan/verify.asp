<%@ Language=VBScript %>

<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
logonid=session("userid")
Call SrGetEmployeeRef(Rtnvalue,1,logonid)
V=split(rtnvalue,";")  
dim ObjRS,strSP,endpgm,prtno
       prtno=session("revcanprtno")
       strSP="USP_okcancel 'B','"& prtno &"','"& V(0) &"'"
       Set conn=Server.CreateObject("ADODB.Connection")  
       DSN="DSN=RtLib"
       conn.Open DSN
       Set ObjRS = conn.Execute(strSP)      
       conn.Close
       endpgm="1"
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.HTMLfld.value="1" then
       msgbox "RT¦¬´Ú¼f®ÖºM¾P§¹¦¨",0
      Set winP=window.Opener
      Set docP=winP.document
      docP.all("keyform").Submit
      winP.focus()              
      window.close
    end if
 end sub
</script>   
<form name=frm1 method=post action=verify.asp>
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">

</form> 
































