<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>發票明細表列印</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  PGM="InvReport2.asp?parm=" 
  yymmdd=document.all("search1").value
  yymmdd2=document.all("search2").value
  if yymmdd="" then yymmdd=1900
  if yymmdd2="" then yymmdd2=2999
  'wrkplace=document.all("sEARCH3").value
  pgm=pgm & yymmdd &";"& yymmdd2
 ' msgbox pgm
  window.open pgm 
   'window.close
End Sub
Sub cmdcancel_onClick
  window.close
End Sub

Sub Srbtnonclick()
   Dim ClickID
   ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
   clickkey="search" & clickid
   if isdate(document.all(clickkey).value) then
      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
   end if
   call objEF2KDT.show(1)
   if objEF2KDT.strDateTime <> "" then
      document.all(clickkey).value = objEF2KDT.strDateTime
   end if
END SUB


</SCRIPT>
</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">發票一覽表</font></i> </DIV>
<P><P>

<table align="center" border=0 cellPadding=0 cellSpacing=0>
	<tr><td align="right" width="50%"><font face="標楷體">發票年份起迄(請填入西元年)：</font></td>
		<td align="left" width="50%">
		   <input type="text" size="5" maxlength="4" name="search1" class=dataListEntry value="<%=Sdate%>">～
		   <input type="text" size="5" maxlength="4" name="search2" class=dataListEntry value="<%=Edate%>">
		</td>
	</tr>

	<!--
	<tr><td ALIGN="RIGHT"><font face="標楷體">發票日期(起) :</font></td>
	<td>
	   <input type="text" size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Edate%>" readonly>
	   <input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="....">
	</td></tr>

	<tr><td ALIGN="RIGHT"><font face="標楷體">發票日期(迄) :</font></td>
	<td>
	   <input type="text" size="10" maxlength="10" name="search2" align=right class=dataListEntry value="<%=Edate%>" readonly>
	   <input type="button" id="B2" name="B2" height=100% width=100% style="Z-INDEX: 1" value="....">
	</td></tr>
	-->
</table> 

<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center></font>
  <HR>
</BODY> 
</HTML>