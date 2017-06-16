<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<title>元訊寬頻網路股份有限公司</title>
</head>
<script language=vbscript>
  sub newurl(num)
	  symd=document.all("search1").value
	  xx=(window.screen.width -7)
      yy=(window.screen.height -74)
      features="top=0,left=0,status=yes,location=no,menubar=no,scrollbars=yes" & ",height=" & yy & ",width=" & xx

	  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
      select case num
		case 1
			prog="/RTAP/BASE/RTSparqFTP/RTSparqFtpRPT.asp?parm=" & symd
		case 2
			prog="/WebAP/RTAP/BASE/RTSparqFTP/RTSparqFtpText.asp?parm=" & symd &";"
		case 3
			prog="/WebAP/RTAP/BASE/RTSparqFTP/RTSparqFtpUL.asp?parm=" & symd
		case 4
			prog="/RTAP/BASE/RTSparqFTP/RTSparq499FtpRPT.asp?parm=" & symd
		case 5
			prog="/WebAP/RTAP/BASE/RTSparqFTP/RTSparq499FtpText.asp?parm=" & symd
		case 6
			prog="/WebAP/RTAP/BASE/RTSparqFTP/RTSparq499FtpUL.asp?parm=" & symd
		case 7
			prog="/WebAP/RTAP/BASE/RTSparqFTP/RTSparqFtpText2.asp?parm=" & symd
		case else
	  end select
    result=window.open(prog,"ppAP",features)
    window.event.returnValue=False
  end sub

  sub bdate1_onclick()
	if isdate(document.all("search1").value) then
		objEF2KDT.varDefaultDateTime=document.all("search1").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search1").value = objEF2KDT.strDateTime
	end if
  end sub
</script>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>
<body bgcolor="#c3c9d2">
<table border="0" width="100%">
  <tr>
    <td width="20%"></td>
    <td width="60%" align="middle"><font color="#0000ff">元訊寬頻網路股份有限公司</font></td>
    <td width="20%"><font color="#0000ff">日期：<%=DATEVALUE(NOW)%></font></td>
  </tr>
  <tr>
    <td width="20%">　</td>
    <td width="60%" align="middle"><font color="#0000ff">Sparq轉檔作業</font></td>
    <td width="20%"><font color="#0000ff">時間：<%=timeVALUE(TIME)%></font></td>
  </tr>

</table>
<HR>
<CENTER>
<TABLE width=450 cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1>
  <TR>
     <td colspan=2 align=center>報竣日或退租日：<input size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
	 <input type="button" id="Button1" name="Bdate1" height="100%" width="100%" style="Z-INDEX: 1" value="....">	</td></tr>
  <TR>
    <TD onclick="newurl(1)" colspan=2>
        <FONT style="COLOR: darkorchid;font-size:14pt">
        <INPUT type="BUTTON" style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="報  表  列  印(399)" ID="B1" NAME="B1">
        </FONT></TD></TR>
  <TR>
    <TD onclick="newurl(2)" align=center>
        <FONT style="COLOR: darkorchid;font-size:13pt">
        <INPUT type="BUTTON"  style="WIDTH: 95%;height:30;font-size:16pt"  VALUE="產生Excel文字檔(399)" ID="B2" NAME="B2">
        </FONT></TD>
    <TD onclick="newurl(7)" align=center>
        <FONT style="COLOR: darkorchid;font-size:13pt">
        <INPUT type="BUTTON"  style="WIDTH: 95%;height:30;font-size:16pt"  VALUE="產生Excel市話檔(399)" ID="B7" NAME="B7">
        </FONT></TD></TR>
  <TR>
    <TD onclick="newurl(3)" colspan=2>
        <FONT style="COLOR: darkorchid;font-size:14pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="資料庫Update與FTP上傳(399)" ID="B3" NAME="B3">
        </FONT></TD></TR>    
  <TR>
    <TD onclick="newurl(4)" colspan=2>
        <FONT style="COLOR: darkorchid;font-size:14pt">
        <INPUT type="BUTTON" style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="報  表  列  印(499)" ID="B4" NAME="B4">
        </FONT></TD></TR>
  <TR>
    <TD onclick="newurl(5)" colspan=2>
        <FONT style="COLOR: darkorchid;font-size:14pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="產生EXCEL文字檔(499)" ID="B5" NAME="B5">
        </FONT></TD></TR>
  <TR>
    <TD onclick="newurl(6)" colspan=2>
        <FONT style="COLOR: darkorchid;font-size:14pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="資料庫Update與FTP上傳(499)" ID="B6" NAME="B6">
        </FONT></TD></TR>    
</TABLE>
</CENTER>
</body></HTML>