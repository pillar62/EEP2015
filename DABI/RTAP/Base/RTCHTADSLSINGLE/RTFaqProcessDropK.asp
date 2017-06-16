<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% dim parmkey,aryparmkey,logonid,conn,rs,sql
   parmKey=Request("Key")
   aryParmKey=Split(parmKey &";;;;;;;;;;;;;;;",";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   '檢查本客訴單號是否已結案或月結
   Set conn2=Server.CreateObject("ADODB.Connection")  
   Set rs2=server.CreateObject("ADODB.Recordset")
   DSN="DSN=RtLib"
   sql2="select * from RTFaqh where CASENO='" & aryparmkey(0) & "'"
   conn2.Open DSN
   rs2.Open sql2,conn2,1,1
   'prelevel="Y"時才可作廢
   prelevel=""
   if rs2("monthclose") <>"Y" and IsNull(rs2("finishdate")) then
      prelevel="Y"
   end if
   rs2.Close
   set rs2=nothing
   conn2.Close
   set conn2=nothing
   '--------------------------
   Set conn=Server.CreateObject("ADODB.Connection")  
   Set rs=server.CreateObject("ADODB.Recordset")
   DSN="DSN=RtLib"
   sql="select * from RTFaqD1 where CASENO='" & aryparmkey(0) & "' and entryno=" & aryparmkey(1)
   
   conn.Open DSN
   rs.Open sql,conn,3,3
   If prelevel="Y" and Isnull(rs("logdropdate")) then   
      rs("LOGDROPDATE")=now()
      rs("LOGDROPUSR")=V(0)
      rs.update
      rs.close
      set rs=nothing
      endpgm="1"
   Elseif Not IsNull(rs("logdropdate")) then   
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
       msgbox "客訴處理單作廢成功",0
       Set winP=window.Opener
       Set docP=winP.document
       docP.all("keyform").Submit
       winP.focus()              
    elseif frm1.htmlfld.value="3" then 
        msgbox "此客訴處理措施紀錄已作廢，不可重覆作廢" & "  " & errmsg
    else
       msgbox "此客訴單已結案(或月結)，不可作廢" & "  " & errmsg
    end if
    window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action="RTFaqDropK.asp">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
</html>