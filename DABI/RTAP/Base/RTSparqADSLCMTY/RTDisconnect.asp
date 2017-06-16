<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% dim parmkey,aryparmkey,logonid,conn,rs,sql
   parmKey=Request("Key")
   aryParmKey=Split(parmKey &";;;;;;;;;;;;;;;",";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   '清除客戶檔之社區序號及社區名稱以達到剔除連結的效果
   '--------------------------
   Set conn=Server.CreateObject("ADODB.Connection")  
   Set rs=server.CreateObject("ADODB.Recordset")
   DSN="DSN=RtLib"
   sql="select * from rtsparqadslcust where cusid='" & aryparmkey(1) & "' and entryno=" & aryparmkey(2)
  ' Response.Write "SQL=" & SQL
   conn.Open DSN
   rs.Open sql,conn,3,3
   If Isnull(rs("TRANSDAT")) then   
 '     rs("TRANSDAT")=now()
 '     rs("transuser")=V(0)
 '     rs("transcnt")= rs("transcnt") + 1
      rs("comq1")=0
      rs("housename")=""
      rs.update
      rs.close
      set rs=nothing
      endpgm="1"
   Elseif Not IsNull(rs("transdat")) then   
      rs.close
      set rs=nothing
      endpgm="3"      
   End if
   conn.Close

%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "客戶資料已從社區檔剔除連結成功",0
       Set winP=window.Opener
       Set docP=winP.document
       docP.all("keyform").Submit
       winP.focus()              
    elseif frm1.htmlfld.value="3" then 
        msgbox "此客戶已轉檔至中華電信報峻，不可剔除連結" & "  " & errmsg
    end if
    window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action="RTdisconnect.asp">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
</html>