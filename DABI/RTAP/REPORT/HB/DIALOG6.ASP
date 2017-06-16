<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>HiBuilding 客戶密碼條信封列印</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  symd=document.all("search1").value
  eymd=document.all("search2").value
  comtype=document.all("search3").value
  rpttype=document.all("search4").value
  hnnos =document.all("search5").value
  hnnoe =document.all("search6").value
  
  if rpttype="A" then			'列印報表
     PGM="/report/hb/HBReport6.asp?parm="   	 
  elseif rpttype ="B" then		'下載文字檔
     PGM="/report/hb/HBReport6a.asp?parm="
  end if    
  
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  IF LEN(TRIM(EYMD))=0 THEN SYMD="2070/12/31"
  pgm=pgm &  symd &";"& eymd &";"& comtype &";"& hnnos &";"& hnnoe
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
sub b2_onclick()
	if isdate(document.all("search2").value) then
		objEF2KDT.varDefaultDateTime=document.all("search2").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search2").value = objEF2KDT.strDateTime
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
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">HiBuilding－客戶密碼條信封列印</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 

'完工日--default為系統日期

    Edate=DateValue(Now())    
%>  
</SELECT>  
 </font></td>
  <tr><td ALIGN="RIGHT"><font face="標楷體">列印總類：</font></td>  
	<td><select name="search4" size="1" class=dataListEntry>
	   		<option value="A" selected>信封列印</option>
			<option value="B">一覽表下載(.xls)</option></select></td></tr>
 
  <tr><td ALIGN="RIGHT"><font face="標楷體">請輸入報竣日起迄：</font></td>
	<td><input type="text" size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
		<input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="...."></td></tr>
   
  <tr><td>&nbsp;</td>
	<td><input type="text" size="10" maxlength="10" name="search2" align=right class=dataListEntry value="<%=Edate%>" readonly>
		<input type="button" id="B2" name="B2" height=100% width=100% style="Z-INDEX: 1" value="...."></td></tr>


  <tr><td ALIGN="RIGHT"><font face="標楷體">請輸入HN號碼起迄：</font></td>
	<td><input type="text" size="10" maxlength="10" name="search5" align=right class=dataListEntry value="00000000"> ~ 
	<input type="text" size="10" maxlength="10" name="search6" align=right class=dataListEntry value="99999999"></td></tr>
		

		
  <tr><td ALIGN="RIGHT"><font face="標楷體">社區經銷所屬：</font></td>  
	<td><select name="search3" size="1" class=dataListEntry>
	   		<option value="A" selected>元訊自有社區</option>
			<option value="B">元訊經銷社區</option></select></td></tr>
</table> 
<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center></p><HR>
 <P><table width="100%" ID="Table2">
  注意事項：<BR>
  	印表機請選擇-- LQ2080C<BR>
	紙張選擇-- 自訂大小，寬度：210mm，高度：<font color="red">127</font>mm<BR>
	紙張來源-- 牽引器
	<font color="Brown">一覽表若需轉存文字檔，請存檔成.rtf為佳</font>
  </TABLE"></font></p> 
 </BODY> 
</HTML>
