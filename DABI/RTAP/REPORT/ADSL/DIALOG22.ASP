<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>ADSL社區各組累計報竣數統計表</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  symd=document.all("search1").value
  eymd=document.all("search10").value
  yesno=document.all("search6").value      
  dropyn=document.all("search7").value      
  pgm= "/report/adsl/adslReport22.asp?parm=" & symd &";"& dropyn &";"& yesno &";"& eymd
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

sub b3_onclick()
	if isdate(document.all("search10").value) then
		objEF2KDT.varDefaultDateTime=document.all("search10").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search10").value = objEF2KDT.strDateTime
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
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">ADSL券商專案－報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">ADSL社區各組累計報竣數統計表</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 
	Sdate="1900/01/01"
    Edate=DateValue(Now())    
%>  
</SELECT>  
 </font></td>
  <tr><td ALIGN="RIGHT"><font face="標楷體">報竣日期(起) :</font></td>
  <td>
      <input type="text" size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
      <input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="...."></td></tr>

  <tr><td ALIGN="RIGHT"><font face="標楷體">報竣日期(迄) :</font></td>
  <td>
      <input type="text" size="10" maxlength="10" name="search10" align=right class=dataListEntry value="<%=Edate%>" readonly>
      <input type="button" id="B3" name="B3" height=100% width=100% style="Z-INDEX: 1" value="...."></td></tr>
      
  <tr><td ALIGN="RIGHT"><font face="標楷體">若社區號碼空白，是否列印 :</font></td>
    <td><select name="search6" size="1" class=dataListEntry>
        <option value="N">否</option>
        <option value="Y">是</option>
        </select></td></tr>

  <tr><td ALIGN="RIGHT"><font face="標楷體">已撤線是否列印 :</font></td>
      <td><select name="search7" size="1" class=dataListEntry>
          <option value="N">否</option>
          <option value="Y">是</option>
          </select>
      </td>
  </tr>
</table> 
<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center><HR>
</BODY> 
</HTML>