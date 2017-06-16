<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
   logonid=session("userid")
   Key=split(request("key"),";")
   Scaseno=key(0)
   Ecaseno=key(0)   
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  

   Set conn=Server.CreateObject("ADODB.Connection")  
   Set rs=Server.CreateObject("ADODB.Recordset")  
   DSN="DSN=RtLib"
   conn.Open DSN
   SQL="select * from RtFAQH  where caseno>='" & Scaseno & "' and caseno <='" & Ecaseno & "'"
   rs.Open sql,conn,3,3
   if not rs.EOF then
      rs("faqprtdate")=now()
      rs.update
   end if
   rs.close
   conn.close   
   On error Resume next
   If Err.number <= 0 then
      endpgm="1"
      errmsg=""
      session("Scaseno")=  Scaseno 
      session("Ecaseno")=  Ecaseno 
   else
      endpgm="2"
      errmsg=cstr(Err.number) & "=" & Err.description
   end if
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "客訴狀況處理表列印完成",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.frm2.submit
    else
       msgbox "無法列印,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action="RTFaQV.asp">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
<form name=frm2 method=post action="RTFaQP.asp">
</form>
</html>