<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>FTTB申請清單</TITLE>

<SCRIPT language=VBScript>
Sub cmdSure_onClick
  rpttype=document.all("search1").value
  symd=document.all("search3").value
  
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  'IF LEN(TRIM(EYMD))=0 THEN SYMD="2070/12/31"
  
  if rpttype="A" then
  		PGM="/report/FTTB/HBReport2a.asp?parm=" 
  elseif rpttype ="B" then
  		PGM="/report/FTTB/HBReport2b.asp?parm=" 
  end if
  pgm=pgm & symd
  set wHandle=window.open (pgm,"win1")
  ' window.close

End Sub

Sub cmdcancel_onClick
  window.close
End Sub

sub b3_onclick()
	if isdate(document.all("search3").value) then
		objEF2KDT.varDefaultDateTime=document.all("search3").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search3").value = objEF2KDT.strDateTime
	end if
end sub

</SCRIPT>

</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="BACKGROUND: lightblue">

<DIV align=center><i><font face="標楷體" size="5" color="#ff00ff">HiBuilding報表列印或轉檔</font></i> </DIV>
<DIV align=center><i><font face="標楷體" size="3" color="#ff00ff">FTTB申請清單(Excel)</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 
    Edate=DateValue(Now())    
%>  
</SELECT></FONT></TD>
<tr><td ALIGN="RIGHT"><font face="標楷體">列印種類 :</font></td>
    <td><select name="search1" size="1" class=dataListEntry>
		<option value="A" selected>FTTB申請清單</option>
		<option value="B">FTTB申請保留原 Mail 清單</option>
        </select>
    </td></tr>
<tr><td ALIGN="right"><font face="標楷體">申請日:</font></td>
<td>
   <input size="10" maxlength="10" name="search3" align=right class=dataListEntry value="<%=Sdate%>" readonly>
   <input type="button" id="B3" name="B3" height="100%" width="100%" style="Z-INDEX: 1" value="....">
</td></tr>

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
