<%
    Dim rs,conn, maxbatch
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    

	' 最大批次號碼
	SQL="SELECT Max(BATCH) as maxbatch FROM RTInvoice "
	rs.Open SQL,conn
    If not rs.Eof Then	  
		maxbatch = rs("maxbatch")
	else
		maxbatch = 0
	end if
	rs.close

'----------
    conn.Close
    Set rs=Nothing
    Set conn=Nothing

	Sdate="1900/1/1"
    Edate=DateValue(Now())
%>

<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>發票列印</TITLE>
<SCRIPT language=VBScript>

	Sub cmdSure_onClick
		yymmdd=document.all("search1").value
  		yymmdd2=document.all("search2").value
  		batch=document.all("search3").value
  		if len(batch) = 0 then
  			batch = 0
  		end if
  		pgm="/rtap/Base/RTInvoice/RTInvReport1.asp?parm=" & batch &";"& yymmdd &";"& yymmdd2
 		' msgbox pgm
  		window.open pgm 
   		'window.close
	End Sub

	Sub cmdcancel_onClick
		window.close()
	End Sub

   Sub Srbtnonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="SEARCH" & clickid
	   if isdate(document.all(clickkey).value) then
	      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
       end if
       call objEF2KDT.show(1)
       if objEF2KDT.strDateTime <> "" then
          document.all(clickkey).value = objEF2KDT.strDateTime
       end if
   End Sub 

</SCRIPT>
</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">發票明細表</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>

<tr><td ALIGN="RIGHT"><font face="標楷體">列印批次 :</font></td>
	<td><input type="text" size="5" maxlength="8" name="search3" value ="<%=maxbatch%>"class=dataListEntry>
	<font size="2" color="blue">批次為0或清空時，表列印所有批次</font>
	</td>
	
</tr>

<tr><td ALIGN="RIGHT"><font face="標楷體">發票日期(起) :</font></td>
<td>
   <input type="text" size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
   <input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
</td></tr>

<tr><td ALIGN="RIGHT"><font face="標楷體">發票日期(迄) :</font></td>
<td>
   <input type="text" size="10" maxlength="10" name="search2" align=right class=dataListEntry value="<%=Edate%>" readonly>
   <input type="button" id="B2" name="B2" height=100% width=100% style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
</td></tr>

</table> 
<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center></font>
  <HR>
</BODY> 
</HTML>