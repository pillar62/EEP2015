<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include virtual="/Webap/include/FrUpdateTrans.inc" -->
<% dim parmkey,aryparmkey,logonid,conn,rs,sql
   On error Resume Next
   parmKey=Request("Key")
   'ADSL中comq1預設為0
   comq1=0
   parmkey=comq1 & ";" & parmkey   
   aryParmKey=Split(parmKey &";;;;;;;;;;;;;;;",";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  

   Set conn=Server.CreateObject("ADODB.Connection")  
   Set rs=server.CreateObject("ADODB.Recordset")
   DSN="DSN=RtLib"
   sql="select * from rtsparqadslcust where  cusid='" & aryparmkey(1) &"' and entryno=" & aryparmkey(2)
   conn.Open DSN
   conn.BeginTrans
   rs.Open sql,conn,3,3
   If (not IsNull(rs("sndinfodat"))) and (IsNull(rs("reqdat"))) then   
      rs("sndinfodat")=Null
      rs("rcvd")=Null      
      rs("sndusr")=""
      oldworkusr=rs("SetSales")
      oldsupp=rs("profac")
      rundesc=rs("unfinishdesc")
      rs.update
      rs.close
      set rs=nothing
      Rtn=SrUpdateTrans(parmkey,"02",V(0),oldworkusr,oldsupp,rundesc)
      if rtn="Y" then
         endpgm="1"
      else
         endpgm="2"
      end if
   Else
      rs.close
      set rs=nothing
      endpgm="2"
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
    if frm1.htmlfld.value="1" then
       msgbox "ADSL撤銷通知技術部指派作業完成",0
       Set winP=window.Opener
       Set docP=winP.document
       docP.all("keyform").Submit
       winP.focus()              
    else
       msgbox "此客戶已發包或尚未通知技術部,不可撤銷通知" & "  " & errmsg
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