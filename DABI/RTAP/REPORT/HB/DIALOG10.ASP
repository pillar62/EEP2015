<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>北區直銷社區名單</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  PGM="/report/hb/HBReport10.asp?parm=" 
  'yyyymm=document.all("search3").value 
  'if Len(Rtrim(yyyymm)) = 1 then yyyymm = "0" & yyyymm
  'yyyymm=document.all("search1").value & yyyymm
  areaid=split(document.all("search6").value, ";")
  'eusr=document.all("search4").value 
  pgm=pgm & areaid(0) &";"& areaid(1)
  'msgbox pgm
  window.open pgm 
   window.close
End Sub
Sub cmdcancel_onClick
  window.close
End Sub

</SCRIPT>
</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">Hibuilding 社區－輸出EXCEL檔</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">直銷社區名單(含各方案)</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
</SELECT>  
 </font></td> 
  <tr><td ALIGN="RIGHT"><font face="標楷體">轄區：</font></td>  
	<td><select name="search6" size="1" class=dataListEntry">
   		<option value="C1;台北縣市" selected>台北縣市</option>	
   		<option value="C1;台北">台北</option>	
		<option value="C2;桃園">桃園</option>
		<option value="C3;台中">台中</option>
		<option value="C4;高雄">高雄</option>
		<option value="C;全部">全部</option></select></td></tr>

</table> 
<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center>
  <HR><P>
  <table width="100%">
  <TABLE></font></p> 
</BODY> 
</HTML>
