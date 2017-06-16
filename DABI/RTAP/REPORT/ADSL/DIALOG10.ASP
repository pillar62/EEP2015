<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>ADSL券商專案－各類報表列印</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  pgm="/report/adsl/ADSLFindAddr.asp?parm=" 
  symd=document.all("search1").value

  pgm=pgm & symd
  'msgbox pgm
  window.open pgm 
   window.close
End Sub
Sub cmdcancel_onClick
  window.close
End Sub

</SCRIPT>
</HEAD>
<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">ADSL券商專案－報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">ADSL券商專案客戶(地址一覽表)</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>

</SELECT>  
 </font></td>
  <tr><td ALIGN="RIGHT"><font face="標楷體">請輸入欲查詢地址 :</font></td>

<td>
   <input type="text" size="10" maxlength="30" name="search1" align=right value="" >
</td></tr>

</table><p>

<center><font face="標楷體">
  <INPUT TYPE="button" VALUE="送出" ID="cmdSure" style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel" style="font-family: 標楷體; color: #FF0000;cursor:hand">
</center><HR>

</BODY> 
</HTML>