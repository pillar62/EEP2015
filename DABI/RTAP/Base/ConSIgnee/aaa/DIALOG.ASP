<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>經銷商對帳月報表</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  PGM="himonthweekgoal.asp?parm=" 
  syy=document.all("search1").value
  smm=document.all("search2").value
  IF LEN(TRIM(SYY))=0 THEN 
     msgbox "請輸入資料年度!"
  elseIF LEN(TRIM(SMM))=0 THEN 
     msgbox "請輸入資料月份!"
  else
     Sdate=Syy & "/" & Smm & "/" & "1"
     edate=cdate(Syy & "/" & smm + 1 & "/" & "01" ) -1
     pgm=pgm & "M" & ";" & SDATE &";" & EDATE
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
	call objEF2KDT.show(0)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search1").value = objEF2KDT.strDateTime
	end if
end sub
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

<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">速博ADSL專案</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">經銷商對帳月報表</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 

'SYY--default為系統年度
'SMM--Default為系統月份 -1
    syy=datepart("yyyy",Now())    
    smm=datepart("m",Now()) -1
%>  
</SELECT>  
 </font></td>
  <tr>
     <td ALIGN="RIGHT"><font face="標楷體">年度/月份 :</font></td>
     <td>
     <input type="text" size="4" maxlength="4" name="search1" align=right class=dataListEntry value="<%=Syy%>" >
     <input type="text" size="2" maxlength="2" name="search2" align=right class=dataListEntry value="<%=Smm%>" >
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