<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% dim parmkey,aryparmkey,logonid,conn,rs,sql
   On error resume next
   parmKey=Request("Key")
   aryParmKey=Split(parmKey &";;;;;;;;;;;;;;;",";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  

   Set conn=Server.CreateObject("ADODB.Connection")  
   Set rs=server.CreateObject("ADODB.Recordset")
   DSN="DSN=RtLib"
   sql="select * from rtcusthbchg where  cusid='" & aryparmkey(1) &"' and entryno=" & aryparmkey(2) & " and modifycode='" & aryparmkey(3) & "' and modifydat ='" & aryparmkey(4) & "' "
   conn.Open DSN
   conn.BeginTrans
   rs.Open sql,conn,3,3
   If IsNull(rs("dropdat")) then   
      rs("dropdat")=datevalue(now())
      rs("dropusr")=V(0)      
      rs.update
      rs.close
      endpgm="1"
   Elseif Not Isnull(rs("dropdat")) then
      rs.close
      set rs=nothing
      endpgm="3"
   Elseif Not Isnull(rs("transdat")) then
      rs.close
      set rs=nothing
      endpgm="2"      
   else
      rs.Close
      set rs=nothing
      endpgm="4"
   End if
   if Err.number > 0 then
      conn.RollbackTrans
   else
      conn.CommitTrans
   end if
   conn.Close
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    errmsg=frm1.htmlfld1.value
    if frm1.htmlfld.value="1" then
       msgbox "本項異動資料已作廢成功!",0
       Set winP=window.Opener
       Set docP=winP.document
       docP.all("keyform").Submit
       winP.focus()              
    elseIF frm1.htmlfld.value="2" then
       msgbox "本項異動資料已報竣或已轉檔，不可作廢!" & "  " & errmsg
    elseIF frm1.htmlfld.value="3" then
       msgbox "本項異動資料已作廢，不須重覆作廢!" & "  " & errmsg       
    else frm1.htmlfld.value="4" 
       msgbox "作廢過程發生錯誤，請通知資訊人員!" & "  " & errmsg              
    end if
    window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action="rtsndinfo.asp">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
</html>