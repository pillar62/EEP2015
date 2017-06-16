<HTML>
	<HEAD>
		<meta http-equiv="Content-Type" content="text/html; charset=Big5">
		<TITLE>HB移轉FTTB拆除設備社區清單</TITLE>
		<SCRIPT language="VBScript">
Sub cmdSure_onClick
  rpttype=document.all("search1").value
  'symd=document.all("search3").value
  
  'IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  'IF LEN(TRIM(EYMD))=0 THEN SYMD="2070/12/31"
  
  if rpttype="A" then
  		PGM="/report/FTTB/HBReport3a.asp?parm="
  else
  		PGM="/report/FTTB/HBReport3b.asp?parm=" & rpttype & ";"
  end if
  pgm=pgm & symd
  set wHandle=window.open (pgm,"win1")
  ' window.close

End Sub

Sub cmdcancel_onClick
  window.close
End Sub
		</SCRIPT>
	</HEAD>
	<BODY style="BACKGROUND: lightblue">
		<DIV align="center"><i><font face="標楷體" size="5" color="#ff00ff">HiBuilding報表列印</font></i>
		</DIV>
		<DIV align="center"><i><font face="標楷體" size="3" color="#ff00ff">HB移轉FTTB拆除設備社區清單</font></i>
		</DIV>
		<P>
		<P>
			<table align="center" width="90%" border="0" cellPadding="0" cellSpacing="0">
				<tr>
					<td ALIGN="RIGHT"><font face="標楷體">列印種類 :</font></td>
					<td><select name="search1" size="1" class="dataListEntry">
							<option value="A" selected>社區總表(Excel)</option>
							<option value="B">交付證明單(元訊)</option>
							<option value="C">交付證明單(久津)</option>
						</select>
					</td>
				</tr>
			</table>
		<p><center><font face="標楷體"> <INPUT TYPE="button" VALUE="送出" ID="cmdSure" style="COLOR: #ff0000; CURSOR: hand; FONT-FAMILY: 標楷體">
					<INPUT TYPE="button" VALUE="取消" ID="cmdcancel" style="COLOR: #ff0000; CURSOR: hand; FONT-FAMILY: 標楷體">
			</center>
			<HR>
		</FONT>
	</BODY>
</HTML>
