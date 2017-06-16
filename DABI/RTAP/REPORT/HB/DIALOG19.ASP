<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>HiBuilding 客戶建置明細表</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  PGM="/report/hb/HBReport19.asp?parm=" 
  symd=document.all("search1").value
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  pgm=pgm &  symd 
 ' msgbox pgm
  window.open pgm 
   window.close
End Sub
Sub cmdcancel_onClick
  window.close
End Sub

sub b1_onclick()
	if isdate(document.all("search1").value) then
		objEF2KDT.varDefaultDateTime=document.all("search1").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search1").value = objEF2KDT.strDateTime
	end if
end sub

</SCRIPT>
</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">HiBuilding－報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">HiBuilding客戶建置明細表</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 

'完工日--default為系統日期

    Edate=DateValue(Now())    
%>  
</SELECT>  
 </font></td>
  <tr><td ALIGN="RIGHT"><font face="標楷體">請輸入申請日期 :</font></td>

<td>
   <input type="text" size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
   <input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="....">
</td></tr>
</table> 
<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center><HR>
</BODY> 
</HTML>