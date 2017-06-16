<%@ Language=VBScript %>
<%
dim returncode,ObjRS,strSP,kk
    kk=request("HTMLfld")
    response.write "kk=" &kk &"<br>"
    if kk=true then
       strSP="USP_okcancel" &"'C'," &"'456'," &"'T89039'"
       Set conn=Server.CreateObject("ADODB.Connection")  
       conn.Open DSN
       Set ObjRS = conn.Execute(strSP)      
       conn.Close    

    end if
%> 
  
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
   ' returncode=msgbox("請按[審核確認]入帳或[取消]按鈕結束",1,"收款金額審核")
    if returncode=vbok then
       frm1.HTMLfld.value=true
       window.document.forms("frm1").submit
    else
    frm1.HTMLfld.value=false
       window.close
    end if
 end sub
</script>   
<form name=frm1 method=post action=verify.asp>
<input type="text" name=HTMLfld style=display:none>

</form> 