<%
    Dim rs,conn,S6
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    S6=""
    rs.Open "SELECT applyprtno from RTEBTCmtyLine GROUP BY applyprtno",CONN
    s6="<option value='*' selected>全部資料</option>" &vbCrLf    
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &rs("applyprtno") & """>" &rs("applyprtno") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'----------
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>AVS ADSL主線申請書</TITLE>

<SCRIPT language=VBScript>
Sub cmdSure_onClick
  applyprtno =document.all("sEARCH6").value
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  IF LEN(TRIM(EYMD))=0 THEN SYMD="2070/12/31"
  pgm="/report/avs/AVSReport1.asp?parm=" & applyprtno
  set wHandle=window.open (pgm,"win1")
  'window.close
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

<BODY style="BACKGROUND: lightblue">

<DIV align=center><i><font face="標楷體" size="5" color="#ff00ff">HiBuilding報表列印或轉檔</font></i> </DIV>
<DIV align=center><i><font face="標楷體" size="3" color="#ff00ff">AVS ADSL主線申請書</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>

<tr><td ALIGN="RIGHT"><font face="標楷體">列印批號 :</font></td>
<td>
	<select name="search6" size="1" class=dataListEntry ID="Select1">
      <%=s6%>
      </select></td></tr>
		
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
