<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<meta name="GENERATOR" content="Microsoft FrontPage 4.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<title>客服其他專案作業</title>
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
        prog="hbcalloutprojectk.asp"
      case 2
        prog="/webap/rtap/rtanalysis/service/CALLOUTSERVICE.asp"
      case 3
        prog="/webap/rtap/base/rtsalesschedule/rtsalesschedule.asp"
      case 4
        prog="HBPotCustK.asp"
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
      <P style="COLOR: yellow;font-size:16pt"" align=center><FONT size=3>其他專案查詢</FONT></P></TD></TR>
  <TR>
    <TD>
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON" onclick="newurl(1)" VALUE="客戶CALLOUT專案建檔作業" style="WIDTH: 100%;height:30;font-size:12pt" ID="Button2" NAME="B1">
        </FONT></TD>
  </TR>
  <TR>
    <TD>
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON" onclick="newurl(2)" VALUE="客戶CALLOUT資料分析" DISABLED style="WIDTH: 100%;height:30;font-size:12pt" ID="B1" NAME="B1">
        </FONT></TD>
  </TR>
  <TR>
    <TD>
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON" onclick="newurl(3)" VALUE="業務行程管理作業" DISABLED style="WIDTH: 100%;height:30;font-size:12pt" ID="Button1" NAME="B1" >
        </FONT></TD>
  </TR>
  <TR>
    <TD>
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON" onclick="newurl(4)" VALUE="潛在客戶作業" style="WIDTH: 100%;height:30;font-size:12pt" ID="Button3" NAME="B1">
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
