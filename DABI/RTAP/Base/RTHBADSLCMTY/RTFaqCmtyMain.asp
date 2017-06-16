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
        prog="RTCustK.asp"
      'case 2
      '  prog="RTResetK.asp"
      case 3
        prog="RTFaqK2.ASP"
      case 4
        prog="/webap/rtap/base/hbadslcust/rtcustk.asp"
      case 5
        prog="/webap/RTAP/base/rtcmty/rtfaqALLk.asp"
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
<HR>
<CENTER>
<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1>
  
  <TR>
    <TD style="WIDTH: 25%"  bgColor=#3e7373>
      <P style="COLOR: yellow;font-size:16pt"" align=center><FONT size=3>社區主線查詢</FONT></P></TD></TR>
  <TR>
    <TD onclick="newurl(1)" disabled>
        <FONT style="COLOR:darkorchid;font-size:12pt">
        <INPUT type="BUTTON" disabled style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="社區資料查詢(舊)" ID="Button2" NAME="B1">
        </FONT></TD>
  </TR>
  <TR>
    <TD onclick="newurl(3)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON" style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="社區主機客訴查詢" ID="Button1" NAME="B1">
        </FONT></TD>
  </TR>
  <tr><td> &nbsp; </td></tr>
  <TR>
    <TD onclick="newurl(4)" disabled>
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON" disabled style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="ADSL+HB 客戶資料查詢(舊)" ID="Button3" NAME="B1">
        </FONT></TD>
  </TR>
  <TR>
    <TD onclick="newurl(5)" disabled>
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON" disabled style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="客訴案件追蹤作業(舊)" ID="Button4" NAME="B1">
        </FONT></TD>
  </TR>
  
</TABLE>
<p>
<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1 ID="Table1">
  <!--
  <TR>
    <TD style="WIDTH: 25%"  bgColor=#3e7373>
      <P style="COLOR: yellow;font-size:16pt"" align=center><FONT size=3>主線異動派工</FONT></P></TD></TR>
  <TR>
    <TD onclick="newurl(4)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="東森AVS主線異動派工" ID="Button5" NAME="B3">
        </FONT></TD>
  </TR> 
  -->     
</TABLE>
</CENTER>
</body></HTML>
