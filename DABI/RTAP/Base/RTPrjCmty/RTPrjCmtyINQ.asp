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
        prog="RTPrjCmtyK.asp"
      case 2
        prog="RTPrjCmtyLineK.asp"
      case 3
        prog="RTPrjCustK.asp"
      case 4
        prog="RTPrjCustARK1.asp"
      case 5
        prog="RTPrjCustARK3.asp"
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
    <td width="60%" align="middle"><font color="#0000ff">元訊寬頻網路股份有限公司</font></td>
    <td width="20%"><font color="#0000ff">日期：<%=DATEVALUE(NOW)%></font></td>
  </tr>
  <tr>
    <td width="20%">　</td>
    <td width="60%" align="middle"><font color="#0000ff">專案社區各項查詢作業</font></td>
    <td width="20%"><font color="#0000ff">時間：<%=timeVALUE(TIME)%></font></td>
  </tr>

</table>
<HR>
<CENTER>

<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1>
  <TR>
    <TD style="WIDTH: 25%"  bgColor=#3e7373>
      <P style="COLOR: yellow;font-size:12pt" align=center>基本資料維護</P></TD></TR>
  <TR>
    <TD onclick="newurl(1)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON" style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="社  區  查  詢" ID="B1" NAME="B1">
        </FONT></TD>
  </TR>
  <TR>
    <TD onclick="newurl(2)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="主  線  查  詢" ID="B2" NAME="B2">
        </FONT></TD>
  </TR>
  <TR>
    <TD onclick="newurl(3)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="用  戶  查  詢" ID="B3" NAME="B3">
        </FONT></TD>
  </TR>    
</TABLE>

<p></p>

<TABLE style="WIDTH: 300px; HEIGHT: 30px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1 ID="Table1">
  <TR>
    <TD style="WIDTH: 25%"  bgColor=#3e7373 >
    	<P style="COLOR: yellow;font-size:12pt" align=center>其他作業維護</P></TD>
  </TR>
 <TR>
    <TD onclick="newurl(4)">
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON" style="WIDTH: 100%;height:30;font-size:12pt" VALUE="應收應付帳款沖銷作業" ID="B4" NAME="B4">
        </FONT></TD>
  </TR>
 <TR>
    <TD onclick="newurl(5)">
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON" style="WIDTH: 100%;height:30;font-size:12pt" VALUE="每月應收應付帳款查詢" ID="B5" NAME="B5">
        </FONT></TD>
  </TR>
 <TR>
    <TD onclick="newurl(9)" disabled>
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON" disabled style="WIDTH: 100%;height:30;font-size:12pt" VALUE="其他作業" ID="BB" NAME="BB">
        </FONT></TD>
  </TR>
</table>

</CENTER>
</body></HTML>
