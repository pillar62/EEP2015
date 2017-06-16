<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<meta name="GENERATOR" content="Microsoft FrontPage 4.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<title>元訊寬頻網路股份有限公司</title>
</head>
<script language=vbscript>
  sub newurl(num)
	  symd=document.all("search1").value
	  eymd=document.all("search2").value
	  ymd=document.all("search3").value
	  symd31=document.all("search31").value
	  eymd32=document.all("search32").value
	  symd41=document.all("search41").value
	  eymd42=document.all("search42").value

	  symd43=document.all("search43").value
	  eymd44=document.all("search44").value
	  symd45=document.all("search45").value
	  eymd46=document.all("search46").value
	  symd47=document.all("search47").value
	  eymd48=document.all("search48").value
	  symd49=document.all("search49").value
	  eymd50=document.all("search50").value
	  xx=(window.screen.width -7)
      yy=(window.screen.height -74)
      features="top=0,left=0,status=yes,location=no,menubar=no,scrollbars=yes" & ",height=" & yy & ",width=" & xx

	  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
 	  IF LEN(TRIM(EYMD))=0 THEN EYMD="2999/12/31"
 	  IF LEN(TRIM(YMD))=0 THEN YMD="2999/12/31"
	  'IF LEN(TRIM(symd43))=0 THEN symd43="1900/01/01"
 	  'IF LEN(TRIM(eymd44))=0 THEN eymd44="2999/12/31"
	  'IF LEN(TRIM(symd45))=0 THEN symd45="1900/01/01"
 	  'IF LEN(TRIM(eymd46))=0 THEN eymd46="2999/12/31"
 	  
      select case num
		case 1
			prog="/webap/RTAP/Report/SparqVoIP/SparqVoIPBonusExcel.asp?parm=" & symd &";"& eymd
		case 2
			prog="/webap/RTAP/Report/SparqVoIP/SparqVoIPScoreExcel.asp?parm=" & ymd
		case 3
			prog="/webap/RTAP/Report/SparqVoIP/RTSparqWagalyFtpText.asp?parm=" & symd31 &";"& eymd32
		case 4
			prog="/webap/RTAP/Report/SparqVoIP/RTSparqWagalyList.asp?parm=" & symd41 &";"& eymd42
		case 5
			prog="/webap/RTAP/Report/SparqVoIP/RTSparqWagalyP_S.asp?parm=" & symd43 &";"& eymd44 &";"& symd45 &";"& eymd46 &";"& symd47 &";"& eymd48 &";"& symd49 &";"& eymd50 &";"
	  end select
    result=window.open(prog,"ppAP",features)
    window.event.returnValue=False
  end sub

   Sub Srbtnonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,6,len(window.event.srcElement.id)-1)
       clickkey="search" & clickid
	   if isdate(document.all(clickkey).value) then
	      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
       end if
       call objEF2KDT.show(1)
       if objEF2KDT.strDateTime <> "" then
          document.all(clickkey).value = objEF2KDT.strDateTime
       end if
   END SUB

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
    <td width="60%" align="middle"><font color="#0000ff">SparqVoIP報表作業</font></td>
    <td width="20%"><font color="#0000ff">時間：<%=timeVALUE(TIME)%></font></td>
  </tr>

</table>
<HR>
<CENTER>
<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1>
<TR><td>報竣日：<input size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
	<input type="button" id="Bdate1" name="Bdate1" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
	～<input size="10" maxlength="10" name="search2" align=right class=dataListEntry value="<%=edate%>" readonly>
	<input type="button" id="Bdate2" name="Bdate2" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">	</td></tr>
<TR><TD onclick="newurl(1)" >
	<FONT style="COLOR: darkorchid;font-size:14pt">
	<INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="工程師獎金明細(Excel)" ID="B1" NAME="B1">
	</FONT></TD></TR></TABLE>

<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1 ID="Table1">
<TR><td>統計截止日：<input size="10" maxlength="10" name="search3" align=right class=dataListEntry value="<%=edate%>" readonly ID="Text1">
	<input type="button" id="Bdate3" name="Bdate3" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
	</td></tr>
<TR><TD onclick="newurl(2)" >
	<FONT style="COLOR: darkorchid;font-size:14pt">
	<INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="VoIP業績總表(Excel)" ID="B2" NAME="B2">
	</FONT></TD></TR></TABLE>

<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1>
	<TR><td>申請日：<input size="10" maxlength="10" name="search31" align=right class=dataListEntry value="<%=Sdate%>" readonly>
			<input type="button" id="Bdate31" name="Bdate31" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">～
			
			<input size="10" maxlength="10" name="search32" align=right class=dataListEntry value="<%=edate%>" readonly>
			<input type="button" id="Bdate32" name="Bdate32" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">	
		</td>
	</tr>
	<TR><TD onclick="newurl(3)" ><FONT style="COLOR: darkorchid;font-size:14pt">
			<INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="WTL申請轉檔清單(Excel)" ID="B3" NAME="B3">
		</FONT></TD>
	</TR>
</TABLE>

<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1>
	<TR><td>申請日：<input size="10" maxlength="10" name="search41" align=right class=dataListEntry value="<%=Sdate%>" readonly>
			<input type="button" id="Bdate41" name="Bdate41" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">～
			
			<input size="10" maxlength="10" name="search42" align=right class=dataListEntry value="<%=edate%>" readonly>
			<input type="button" id="Bdate42" name="Bdate42" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">	
		</td>
	</tr>
	<TR><TD onclick="newurl(4)" ><FONT style="COLOR: darkorchid;font-size:14pt">
			<INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="WTL用戶清單(Excel)" ID="B4" NAME="B4">
		</FONT></TD>
	</TR>
</TABLE>

<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1>
	<TR><td>派工日：<input size="10" maxlength="10" name="search43" align=right class=dataListEntry value="<%=Sdate%>" readonly>
			<input type="button" id="Bdate43" name="Bdate43" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">～
			
			<input size="10" maxlength="10" name="search44" align=right class=dataListEntry value="<%=edate%>" readonly>
			<input type="button" id="Bdate44" name="Bdate44" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">	
		</td>
	</tr>
	<TR><td>完工日：<input size="10" maxlength="10" name="search45" align=right class=dataListEntry value="<%=Sdate%>" readonly>
			<input type="button" id="Bdate45" name="Bdate45" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">～
			
			<input size="10" maxlength="10" name="search46" align=right class=dataListEntry value="<%=edate%>" readonly>
			<input type="button" id="Bdate46" name="Bdate46" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">	
		</td>
	</tr>
	<TR><td>退租日：<input size="10" maxlength="10" name="search47" align=right class=dataListEntry value="<%=Sdate%>" readonly>
			<input type="button" id="Bdate47" name="Bdate47" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">～
			
			<input size="10" maxlength="10" name="search48" align=right class=dataListEntry value="<%=edate%>" readonly>
			<input type="button" id="Bdate48" name="Bdate48" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">	
		</td>
	</tr>
	<TR><td>作廢日：<input size="10" maxlength="10" name="search49" align=right class=dataListEntry value="<%=Sdate%>" readonly>
			<input type="button" id="Bdate49" name="Bdate49" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">～
			
			<input size="10" maxlength="10" name="search50" align=right class=dataListEntry value="<%=edate%>" readonly>
			<input type="button" id="Bdate50" name="Bdate50" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">	
		</td>
	</tr>
	<TR><TD onclick="newurl(5)" ><FONT style="COLOR: darkorchid;font-size:14pt">
			<INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:16pt"  VALUE="P&S用戶列表(Excel)" ID="B5" NAME="B5">
		</FONT></TD>
	</TR>
</TABLE>

</CENTER>
</body></HTML>
