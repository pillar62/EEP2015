<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>本週各組業績目標達成比率表</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  PGM="/report/adsl/himonthweekgoal.asp?parm=" 
  sDATE=document.all("search1").value
  EDATE=document.all("search2").value
  IF LEN(TRIM(SDATE))=0 THEN 
     msgbox "請輸入本週資料日期起日!"
  elseIF LEN(TRIM(EDATE))=0 THEN 
     msgbox "請輸入本週資料日期迄日!"
  else
     pgm=pgm & "W" & ";" & SDATE &";" & EDATE
    ' msgbox SDATE & ";" & EDATE
     
    ' msgbox pgm
     window.open pgm 
     window.close
  end if
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
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">HiBuilding－報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">業績目標達成率週報表</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 

'Sdate--default為本週起日
'edate--Default為本週迄日
    edate=DATEVALUE(now())
    sdate=DATEVALUE(now)-7
%>  
</SELECT>  
 </font></td>
  <tr>
     <td ALIGN="RIGHT"><font face="標楷體">年度/月份 :</font></td>
     <td>
     <input type="text" size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" >
     <input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="....">     
     <input type="text" size="10" maxlength="10" name="search2" align=right class=dataListEntry value="<%=edate%>" >
      <input type="button" id="B2" name="B2" height=100% width=100% style="Z-INDEX: 1" value="....">     
      </td>
   </tr>
</table>   
</table> 
<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center><HR>
 <center><font color="brown">列印時請確定印表機的紙張為 *** A4 SIZE*** 報表紙</font></center>
</BODY> 
</HTML>