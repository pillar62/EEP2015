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
   sql="select * from RTCUST where COMQ1=" & aryparmkey(0) & " AND CUSID='" & ARYPARMKEY(1) & "' AND ENTRYNO=" & ARYPARMKEY(2)
   conn.Open DSN
   rs.Open sql,conn,3,3
   If IsNull(rs("FINISHDAT")) and IsNull(rs("dropdat"))  and IsNull(rs("DOCKETDAT")) then   
      rs("DROPDAT")=now()
      rs.update
      rs.close
      set rs=nothing
      endpgm="1"
   elseif not IsNull(rs("dropdat")) then
      rs.Close
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
       msgbox "客戶作廢成功",0
       Set winP=window.Opener
       Set docP=winP.document
       docP.all("keyform").Submit
       winP.focus()              
    elseif frm1.htmlfld.value="3" then 
        msgbox "此客戶已作廢或退租，不可重覆作廢" & "  " & errmsg
    else
       msgbox "此客戶已完工或報竣，不可作廢" & "  " & errmsg
    end if
    window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action="RTCUSTDrop.asp">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
</html>