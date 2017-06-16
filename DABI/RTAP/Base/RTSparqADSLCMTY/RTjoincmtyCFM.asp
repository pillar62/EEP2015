<%@ Language=VBScript %>
<%
   key=request("key")
   set connXX=server.CreateObject("ADODB.connection")
   set rsXX=server.CreateObject("ADODB.recordset")
   dsnxx="DSN=RTLIB"
   sqlxx="select COMN from rtsparqadslcmty where cutyid=" & key
   connxx.Open dsnxx
   rsxx.Open sqlxx,connxx
   if not rsxx.EOF then
      session("COMN")=rsxx("COMN")
   else
      SESSION("COMN")=""
   end if
   session("COMQ1")=key
   rsxx.Close
   connxx.Close
   set rsxx=nothing
   set connxx=nothing  
   On Error Resume Next
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
    if frm1.htmlfld.value="1" then
   '    msgbox "客戶資料與社區檔已建立連結,請按[重新整理]呈現最新資料!",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.close             
       window.close
    else
       msgbox "無法建立連結,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTjoincustCFM.asp>
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
</html>