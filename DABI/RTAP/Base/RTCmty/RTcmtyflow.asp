<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
   KEY=REQUEST("KEY")
   KEYARY=SPLit(key,";")
   Set conn=Server.CreateObject("ADODB.Connection")  
   Set rs=Server.CreateObject("ADODB.Recordset")  
   DSN="DSN=RtLib"
   conn.Open DSN
   SQL="select snmpip from Rtsnmp where comkind='1' and comq1=" & keyary(0)
   rs.Open sql,conn,1,1
   if not rs.EOF then
      snmpip=trim(rs("snmpip")) & ".htm"
   else
      snmpip="error.htm"
   end if
   rs.close
   conn.close   
   On error Resume next
   If Err.number <= 0 then
      endpgm="1"
      errmsg=""
   else
      endpgm="2"
      errmsg=cstr(Err.number) & "=" & Err.description
   end if
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       prog="http://w3c.cbbn.com.tw/mrtg/" & frm1.htmlfld2.value
       Set winP=window.Opener
  '     Set docP=winP.document       
  '     docP.all("keyform").Submit
  '     winP.focus()              
  '     window.frm2.submit
   '    DiaWidth=window.screen.width
   '    DiaHeight=window.screen.height - 30
       Scrxx=window.screen.width
       Scryy=window.screen.height - 30
       StrFeature="Top=0,left=0,scrollbars=yes,status=yes," _
                    &"location=no,menubar=no,width=" & scrxx & "px" _
                    &",height=" & scryy & "px"
       Set diagWindow=Window.Open(prog,"diag",strFeature)
     '  diagWindow=Window.showmodaldialog(prog,"d2","dialogWidth:" & scrxx & "px;dialogHeight:" & scryy &"px;")  
    '   winP.focus()
       window.close
    else
       msgbox "執行過程發生異常,請通知資訊人員。錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
      ' winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action="RTcmtyflow.asp">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
<input type="text" name=HTMLfld2 style=display:none value="<%=snmpip%>">
</form>
<!--
<form name=frm2 method=post action="RTFaQP.asp">
-->
</form>
</html>