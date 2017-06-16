<%@ Language=VBScript %>
<%
   key=request("key")
   arykey=split(key,";")
   Set conn=Server.CreateObject("ADODB.Connection")  
   DSN="DSN=RtLib"
   conn.Open DSN
   On Error Resume Next
   if session("COMQ1")="" or SESSION("COMN")="" then
      endpgm="3"
      errmsg="畫面停止太久,相關資訊已消失,請重新執行本作業!"
   else
      updsql="update rtcustadsl set comq1=" & session("COMQ1") & ",housename='" & session("COMN") & "' where cusid='" & arykey(0) & "' and entryno=" & arykey(1)
      conn.Execute updsql 
      conn.Close
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         endpgm="1"
         errmsg=""
      end if
   end if
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    on error resume next
    if frm1.htmlfld.value="1" then
    '   msgbox "客戶資料與社區檔已建立連結,請按[重新整理]呈現最新資料!",0
       Set winP=window.Opener
       Set docP=winP.document       
    '   docP.all("keyform").Submit
       winP.close             
       window.close
    else
   '    msgbox "無法建立連結,錯誤訊息：" & "  " & frm1.htmlfld1.value
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