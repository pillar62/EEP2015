<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include virtual="/Webap/include/FrUpdateTrans.inc" -->
<% dim parmkey,aryparmkey,logonid,conn,rs,sql
   parmKey=Request("Key")
   aryParmKey=Split(parmKey &";;;;;;;;;;;;;;;",";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  

   Set conn=Server.CreateObject("ADODB.Connection")  
   Set rs=server.CreateObject("ADODB.Recordset")
   DSN="DSN=RtLib"
   sql="select * from rtcust where comq1=" & aryparmkey(0) & " and cusid='" & aryparmkey(1) &"' and entryno=" & aryparmkey(2)
   conn.Open DSN
   rs.Open sql,conn,3,3
   If IsNull(rs("finishdat")) and not IsNull(rs("reqdat"))  then   
      rs("finishdat")=now()
      oldworkusr=rs("SetSales")
      oldsupp=rs("profac")
      rundesc=rs("unfinishdesc")
      rs.update
      rs.close
      set rs=nothing
      Rtn=SrUpdateTrans(parmkey,"07",V(0),oldworkusr,oldsupp,rundesc)
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
   conn.Close

%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "RT完工作業完成",0
       Set winP=window.Opener
       Set docP=winP.document
       docP.all("keyform").Submit
       winP.focus()              
    else
       msgbox "此客戶已完工不可重複執行或尚未發包" & "  " & errmsg
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