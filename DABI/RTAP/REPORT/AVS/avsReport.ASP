<%
stocknc=session("name")
%>
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
        prog="dialog1.asp"
      case 2
        prog="dialog2.asp"
      case 3
        prog="dialog3.asp"        
      case 5
        prog="dialog5.asp"        
      case 6
        prog="dialog6.asp"        
      case 7
        prog="dialog7.asp"        
      case 8
        prog="dialog8.asp"        
      case 9
        prog="dialog9.asp"        

      case else
    end select
    xx=(window.screen.width -7)/1.5
    yy=(window.screen.height -74)/1.5
    xx2=(window.screen.width - XX)/4
    yy2=(window.screen.height - yy)/1.5
    features="top=" & xx2 & ",left=" & yy2 & ",status=yes,location=no,menubar=no,scrollbars=yes" & ",height=" & yy & ",width=" & xx
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
    <td width="60%" align="middle"><font color="#0000ff">東森 AVS － 各類報表列印</font></td>
    <td width="20%"><font color="#0000ff">時間：<%=timeVALUE(TIME)%></font></td>
  </tr>

</table>
<HR>
<CENTER>
<TABLE style="WIDTH: 587px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 width=587 bgColor=antiquewhite border=1>
  <TR>
    <TD style="WIDTH: 33%" width="27%" bgColor=#3e7373>
      <P style="COLOR: yellow" align=center>週報表</P></TD>
    <TD style="WIDTH: 33%" width="27%" bgColor=#3e7373>
      <P style="COLOR: yellow" align=center>月報表</P></TD>
    <TD style="WIDTH: 33%" width="46" bgColor=#3e7373>
      <P style="COLOR: yellow" align=center>其它管理報表</P></TD></TR>
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(2)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(1)AVS 主線送件清單</FONT></TD></TR>
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(1)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(2)AVS 主線申請書</FONT></TD></TR>    
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(3)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(3)AVS 申請/回覆清單</FONT></TD></TR>    
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(5)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(4)AVS 社區主線資料表</FONT></TD></TR>    

    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(6)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(5)AVS 社區送線一覽表</FONT></TD></TR>    
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(7)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(6)AVS 報竣戶一覽表(by裝機人)</FONT></TD></TR>    
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(8)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(7)AVS 報竣戶一覽表(by業務員)</FONT></TD></TR>    
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(9)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(8)AVS 退租回覆清單</FONT></TD></TR>    

    </TABLE>
    
    <p>
</CENTER>
</body></HTML>
