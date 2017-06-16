<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>P&S用戶列表</TITLE>

<SCRIPT language=VBScript>
Sub cmdSure_onClick
	  symd43=document.all("search43").value
	  eymd44=document.all("search44").value
	  symd45=document.all("search45").value
	  eymd46=document.all("search46").value
	  symd47=document.all("search47").value
	  eymd48=document.all("search48").value
	  symd49=document.all("search49").value
	  eymd50=document.all("search50").value
  'IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  'IF LEN(TRIM(EYMD))=0 THEN SYMD="2070/12/31"
  
  pgm="/report/sparqVoIP/RTSparqWagalyP_S.asp?parm=" & symd43 &";"& eymd44 &";"& symd45 &";"& eymd46 &";"& symd47 &";"& eymd48 &";"& symd49 &";"& eymd50 &";"
  set wHandle=window.open (pgm,"win1")
  ' window.close
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

<BODY style="BACKGROUND: lightblue">

<DIV align=center><i><font face="標楷體" size="5" color="#ff00ff">遠傳報表列印</font></i> </DIV>
<DIV align=center><i><font face="標楷體" size="3" color="#ff00ff">P&S用戶列表(Excel)</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 
    'Edate=DateValue(Now())    
%>  

<tr>
	<td ALIGN="right"><font face="標楷體">請輸入派工日:</font></td>
	<td>
	   <input size="10" maxlength="10" name="search43" align=right class=dataListEntry value="<%=Sdate%>" readonly>
	   <input type="button" id="B43" name="B43" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">～
	   <input size="10" maxlength="10" name="search44" align=right class=dataListEntry value="<%=Edate%>" readonly>
	   <input type="button" id="B44" name="B44" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
	</td>
</tr>
<tr>
	<td ALIGN="right"><font face="標楷體">請輸入完工日:</font></td>
	<td>
	   <input size="10" maxlength="10" name="search45" align=right class=dataListEntry value="<%=Sdate%>" readonly>
	   <input type="button" id="B45" name="B45" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">～
	   <input size="10" maxlength="10" name="search46" align=right class=dataListEntry value="<%=Edate%>" readonly>
	   <input type="button" id="B46" name="B46" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
	</td>
</tr>
<tr>
	<td ALIGN="right"><font face="標楷體">請輸入退租日:</font></td>
	<td>
	   <input size="10" maxlength="10" name="search47" align=right class=dataListEntry value="<%=Sdate%>" readonly>
	   <input type="button" id="B47" name="B47" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">～
	   <input size="10" maxlength="10" name="search48" align=right class=dataListEntry value="<%=Edate%>" readonly>
	   <input type="button" id="B48" name="B48" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
	</td>
</tr>
<tr>
	<td ALIGN="right"><font face="標楷體">請輸入作廢日:</font></td>
	<td>
	   <input size="10" maxlength="10" name="search49" align=right class=dataListEntry value="<%=Sdate%>" readonly>
	   <input type="button" id="B49" name="B49" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">～
	   <input size="10" maxlength="10" name="search50" align=right class=dataListEntry value="<%=Edate%>" readonly>
	   <input type="button" id="B50" name="B50" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
	</td>
</tr>

		
</table> 
<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="COLOR: #ff0000; CURSOR: hand; FONT-FAMILY: 標楷體"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="COLOR: #ff0000; CURSOR: hand; FONT-FAMILY: 標楷體">
 </center><HR>
</FONT>
</BODY> 
</HTML>
