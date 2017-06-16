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
   If IsNull(rs("reqdat")) and (len(trim(rs("profac"))) > 0 or len(trim(rs("setsales"))) > 0 ) And not IsNull(rs("sndinfodat")) and rs("ar") > 0 then   
      rs("reqdat")=now()
      oldworkusr=rs("SetSales")
      oldsupp=rs("profac")
      rundesc=rs("unfinishdesc")
      rs.update
      rs.close
      set rs=nothing
      Rtn=SrUpdateTrans(parmkey,"03",V(0),oldworkusr,oldsupp,rundesc)
      if rtn="Y" then
         endpgm="1"
      else
         endpgm="2"
      end if
   Else
      if rs("AR") > 0 then 
         endpgm="2"
      else
         endpgm="3"
      end if
      rs.close
      set rs=nothing
   End if
   conn.Close

%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "RT發包完成",0
       Set winP=window.Opener
       Set docP=winP.document
       docP.all("keyform").Submit
       winP.focus()              
    elseif frm1.htmlfld.value="2" then
       msgbox "此客戶(尚未通知發包)或(已發包不可重複發包)或(未指定施工人員)" & "  " & errmsg
    elseif frm1.htmlfld.value="3" then
       msgbox "應收金額必須大於零" & "  " & errmsg       
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