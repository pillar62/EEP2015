<%@ Language=VBScript %>
<%
   key=request("key")
   keyary=split(key,";")
   session("key1")=keyary(0)
   session("key2")=keyary(1)
 '  otherstring=" and rtcustadsl.cusid='" & keyary(0) & "' and rtcustadsl.enrtyno=" & keyary(1)
 '  session("sqlstring1")=session("sqlstring1") & replace(otherstring,"'","""")  
  ' Response.Write session("sqlstring1")
   on error resume next
   If Err.number <> 0 then
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
    if frm1.htmlfld.value="1" then
       msgbox "先看先贏帳號 DM 單筆列印完成",0
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
<form name=frm1 method=post action=verify2.asp>
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
<form name=frm2 method=post action=RTaccountPrt2.asp>
</form>
</html>