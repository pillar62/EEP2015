<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>ADSL券商專案－各類報表列印</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  pgm="/report/adsl/ADSLReport17.asp?parm=" 
  'msgbox pgm
  yesno=document.all("search6").value    
  dropyn=document.all("search7").value    
  rptkind=document.all("search8").value    
  symd=document.all("search9").value
  Eymd=document.all("search10").value
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  IF LEN(TRIM(EYMD))=0 THEN EYMD="2999/12/31"
  
  pgm=pgm & yesno &";"& dropyn &";"& rptkind &";"& symd & ";" & eymd  
  window.open pgm 
  window.close
End Sub

Sub cmdcancel_onClick
  window.close
End Sub

sub b1_onclick()
	if isdate(document.all("search9").value) then
		objEF2KDT.varDefaultDateTime=document.all("search9").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search9").value = objEF2KDT.strDateTime
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
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">券商專案社區資料表</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>

</SELECT>  
 </font></td>
 
<tr><td ALIGN="RIGHT"><font face="標楷體">若社區號碼空白，是否列印 :</font></td>
    <td><select name="search6" size="1" class=dataListEntry>
        <option value="N">否</option>
        <option value="Y">是</option>
        </select>
    </td>
</tr>
<tr><td ALIGN="RIGHT"><font face="標楷體">已撤線是否列印 :</font></td>
    <td><select name="search7" size="1" class=dataListEntry>
        <option value="N">否</option>
        <option value="Y">是</option>
        </select>
    </td>
</tr>
<tr><td ALIGN="RIGHT"><font face="標楷體">總表或明細表 :</font></td>
    <td><select name="search8" size="1" class=dataListEntry>
        <option value="1">明細表</option>
        <option value="2">總表</option>
        </select>
    </td>
</tr>

<tr><td ALIGN="RIGHT"><font face="標楷體">請輸入竣工日期(起) :</font></td>
	<td><input type="text" size="10" maxlength="10" name="search9" align=right class=dataListEntry value="<%=Sdate%>" readonly ID="Text1">
		<input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="...."></td></tr>

<tr><td ALIGN="RIGHT"><font face="標楷體">請輸入竣工日期(迄) :</font></td>
	<td><input type="text" size="10" maxlength="10" name="search10" align=right class=dataListEntry value="<%=Edate%>" readonly ID="Text2">
		<input type="button" id="B3" name="B3" height=100% width=100% style="Z-INDEX: 1" value="...."></td></tr>

</table><p>

<center><font face="標楷體">
  <INPUT TYPE="button" VALUE="送出" ID="cmdSure" style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel" style="font-family: 標楷體; color: #FF0000;cursor:hand">
</center><HR>

</BODY> 
</HTML>