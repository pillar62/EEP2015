<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>HiBuilding客戶 Call Out 名單</TITLE>

<SCRIPT language=VBScript>
Sub cmdSure_onClick
  drop=document.all("search1").value
  cutid=document.all("search2").value
  comn=document.all("search3").value
  addr1=document.all("search4").value
  if len(comn) =0 then
  	 comn ="*"
  end if
  if len(addr1) =0 then
  	 addr1 ="*"
  end if

  if drop="N" then
		pgm="/report/HB/HBReport23a.asp?parm=" & cutid &";"& comn &";"& addr1
  elseif drop ="Y" then
		pgm="/report/HB/HBReport23b.asp?parm=" & cutid &";"& comn &";"& addr1
  end if    
  set wHandle=window.open (pgm,"win1")
  ' window.close
End Sub

Sub cmdcancel_onClick
  window.close
End Sub

</SCRIPT>

</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="BACKGROUND: lightblue">

<DIV align=center><i><font face="標楷體" size="5" color="#ff00ff">各方案報表列印</font></i> </DIV>
<DIV align=center><i><font face="標楷體" size="3" color="#ff00ff">HiBuilding客戶 Call Out 名單</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 
    Edate=DateValue(Now())    
%>  
</SELECT></FONT></TD>
<tr><td ALIGN="RIGHT"><font face="標楷體">類別：</font></td>  
	<td><select name="search1" size="1" class=dataListEntry>
	   		<option value="N" selected>HB客戶 Call Out名單(有效戶)</option>
			<option value="Y">HB客戶 Call Out名單(退租戶)</option></select></td></tr>
<tr><td ALIGN="RIGHT"><font face="標楷體">區域：</font></td>  
	<td><select name="search2" size="1" class=dataListEntry>
	   		<option value="A1" selected>新竹以北</option>
	   		<option value="A2">中區</option>
			<option value="A3">南區</option></select></td></tr>
<tr><td ALIGN="RIGHT"><font face="標楷體">社區名稱：</font></TD>
	<TD><INPUT TYPE="TEXT" NAME="search3" SIZE="30"></td></tr>
<tr><td ALIGN="RIGHT"><font face="標楷體">客戶地址：</font></TD>
	<TD><INPUT TYPE="TEXT" NAME="search4" SIZE="50"></td></tr>


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
