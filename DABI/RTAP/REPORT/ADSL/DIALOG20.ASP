<%
    Dim rs,conn,S6
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")
    rs.Open "SELECT EMPLY FROM RTEmployee WHERE NETID ='"& Session("userid") & "' ", conn
    S6 = rs("EMPLY")
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>

<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>中華電信報竣客戶明細表(轉檔)</TITLE>

<SCRIPT language=VBScript>
Sub cmdSure_onClick
  symd=document.all("search1").value
  emy =document.all("search2").value 
  typ =document.all("search3").value 
  obj =document.all("search4").value 
  
  if typ = "3" then  '轉檔
     PGM="/report/adsl/adslReport20b.asp?parm="
  else               '報表 
     PGM="/report/adsl/adslReport20a.asp?parm="     
  end if   
  
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  pgm=pgm &  symd  &";"& emy &";"& typ &";"& obj
  set wHandle=window.open (pgm,"win1")
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

</SCRIPT>

</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="BACKGROUND: lightblue">

<DIV align=center><i><font face="標楷體" size="5" color="#ff00ff">HiBuilding報表列印或轉檔</font></i> </DIV>
<DIV align=center><i><font face="標楷體" size="3" color="#ff00ff">中華電信報竣客戶明細表(轉檔)</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 

    Edate=DateValue(Now())    
%>  
</SELECT></FONT></TD>
  <tr><td ALIGN="right"><font face="標楷體">請輸入報竣日期 :</font></td>

<td>
   <input size="10" maxlength="10" name="search1" align=right class=dataListEntry value=" <%=Sdate%>" readonly>
   <input type="button" id="B1" name="B1" height="100%" width="100%" style="Z-INDEX: 1" value="....">
</td></tr>

<p>
<input type=hidden name="search2" value="<%=S6%>"> 
  <P></P>

<tr><td ALIGN="right"><font face="標楷體">印表種類 :</font></td>
		<td><select name="search3" size="1" class=dataListEntry>
	   		<option value="1" 
        selected>測試報表</option>
			<option value="2">正式報表</option>
			<option value="3">產生轉檔資料</option> 
		</select></td></tr>
<tr><td ALIGN="right"><font face="標楷體">印表對象 :</font></td>
		<td><select name="search4" size="1" class=dataListEntry>
	   		<option value="A" selected>元訊自有社區</option>
			<option value="B">元訊經銷社區</option>
		</select></td></tr>
</p>
		
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