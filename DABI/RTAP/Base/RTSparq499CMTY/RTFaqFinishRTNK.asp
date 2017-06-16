<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% dim parmkey,aryparmkey,logonid,conn,rs,sql
   parmKey=Request("Key")
   aryParmKey=Split(parmKey &";;;;;;;;;;;;;;;",";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   Set conn=Server.CreateObject("ADODB.Connection")  
   Set rs=server.CreateObject("ADODB.Recordset")
   DSN="DSN=RtLib"
   sql="select * from RTFaqH where CASENO='" & aryparmkey(0) & "'"
   conn.Open DSN
   rs.Open sql,conn,3,3
   If Not IsNull(rs("Finishdate")) and rs("monthclose")<>"Y" then   
      rs("finishdate")=Null
      rs("finishusr")=""
      rs("finishfac")=""
      rs.update
      rs.close
      set rs=nothing
      endpgm="1"
   Elseif rs("monthclose") ="Y" then
      rs.close
      set rs=nothing
      endpgm="3"            
   Else
      rs.close
      set rs=nothing
      endpgm="2"      
   End if
   conn.Close

%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "客訴處理單結案反轉成功",0
       Set winP=window.Opener
       Set docP=winP.document
       docP.all("keyform").Submit
       winP.focus()              
    elseif frm1.htmlfld.value="3" then
       msgbox "此客訴單已月結，不可反轉" & "  " & errmsg       
    else
       msgbox "此客訴單尚未結案，不須反轉" & "  " & errmsg
    end if
    window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action="RTFaqfinishrtnK.asp">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
</html>