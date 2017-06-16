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
	  symd=document.all("search1").value
	  eymd=document.all("search2").value
	  xx=(window.screen.width -7)
      yy=(window.screen.height -74)
      features="top=0,left=0,status=yes,location=no,menubar=no,scrollbars=yes" & ",height=" & yy & ",width=" & xx

	  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
 	  IF LEN(TRIM(EYMD))=0 THEN EYMD="2999/12/31"
      select case num
		case 1
			prog="/webap/RTAP/Report/KTS/KTSBonusExcel.asp?parm=" & symd &";"& eymd
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
  sub bdate2_onclick()
	if isdate(document.all("search2").value) then
		objEF2KDT.varDefaultDateTime=document.all("search2").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search2").value = objEF2KDT.strDateTime
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
    <td width="60%" align="middle"><font color="#0000ff">KTS報表作業</font></td>
    <td width="20%"><font color="#0000ff">時間：<%=timeVALUE(TIME)%></font></td>
  </tr>

</table>
<HR>
<CENTER>
<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1>
  <TR>
     <td>送件日：<input size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
	 <input type="button" id="Button1" name="Bdate1" height="100%" width="100%" style="Z-INDEX: 1" value="....">
     ～<input size="10" maxlength="10" name="search2" align=right class=dataListEntry value="<%=edate%>" readonly>
	 <input type="button" id="Button2" name="Bdate2" height="100%" width="100%" style="Z-INDEX: 1" value="....">	</td>	 
	 </tr>
  <TR>
    <TD onclick="newurl(1)" >
        <FONT style="COLOR: darkorchid;font-size:14pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="工程師獎金明細(Excel)" ID="B1" NAME="B1">
        </FONT></TD>
  </TR>
</TABLE>
</CENTER>
</body></HTML>
