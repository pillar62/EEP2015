<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<meta name="GENERATOR" content="Microsoft FrontPage 4.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<title>元訊寬頻網路股份有限公司</title>
</head>
<script language="JavaScript" fptype="dynamicanimation">
<!--
function dynAnimation() {}
function clickSwapImg() {}
//-->
</script>
<script language="JavaScript1.2" fptype="dynamicanimation" src="animate.js">
</script>
<script language=vbscript>
  sub newurl(num)
      select case num
		case 1
			prog="/WebAP/RTAP/Base/RTCustYearContract/RTConsigneeStatK.asp?parm="
		case 2
			prog="/WebAP/RTAP/Base/RTCustYearContract/RTCust1YearD.asp?parm="
		case 3
			prog="/WebAP/RTAP/Base/RTCustYearContract/RTCust1YearTOTCUSTK.asp?parm="			
		case else
	  end select
    xx=(window.screen.width -7)
    yy=(window.screen.height -74)
    features="top=0,left=0,status=yes,location=no,menubar=no,scrollbars=yes" & ",height=" & yy & ",width=" & xx
    result=window.open(prog,"ppAP",features)
    window.event.returnValue=False
  end sub

</script>
<body bgcolor="#c3c9d2">
<table border="0" width="100%">
  <tr>
    <td width="20%"></td>
    <td width="60%" align="middle" ><font color="#0000ff" >元訊寬頻網路股份有限公司</font></td>
    <td width="20%"><font color="#0000ff">日期：<%=DATEVALUE(NOW)%></font></td>
  <tr>
    <td width="20%">　</td>
    <td width="60%" align="middle"><font color="#0000ff">計量制年約同意書查詢作業</font></td>
    <td width="20%"><font color="#0000ff">時間：<%=timeVALUE(TIME)%></font></td>
  </tr>    
  </tr>
</table>
<HR>
<CENTER>
<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1>
<TR>
    <TD style="WIDTH: 25%"  bgColor=#3e7373>
      <P style="COLOR: yellow;font-size:16pt"" align=center>查詢方式</P></TD></TR>
  <TR>
    <TD onclick="newurl(1)" >
        <FONT style="COLOR: darkorchid;font-size:14pt">
        <INPUT type="BUTTON" style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="年約客戶未滿一年退租統計" ID="Button2" NAME="B1">
        </FONT></TD>
  </TR>
  <TR>
    <TD onclick="newurl(2)" >
        <FONT style="COLOR: darkorchid;font-size:14pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="未滿一年退租客戶查詢" ID="Button3" NAME="B2">
        </FONT></TD>
  </TR>
  <TR>
    <TD onclick="newurl(3)" >
        <FONT style="COLOR: darkorchid;font-size:14pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="經銷商年約同意書客戶查詢" ID="B3" NAME="B3">
        </FONT></TD>
  </TR>          

</TABLE>
</CENTER>
</body></HTML>
