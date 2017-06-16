<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
logonid=session("userid")
Call SrGetEmployeeRef(Rtnvalue,1,logonid)
V=split(rtnvalue,";")  
dim ObjRS,strSP,endpgm,prtno
       prtno=session("COTcanpprtno")
       strSP="USP_okcancel 'I','"& prtno &"','"& V(0) &"'"
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
       msgbox "COT建置自付額明細列印撤銷完成",0
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

