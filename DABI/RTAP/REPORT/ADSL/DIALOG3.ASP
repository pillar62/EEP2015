<%
    Dim rs,conn,S6
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    S6=""
    rs.Open "SELECT RTStock.CUSID, RTObj.SHORTNC FROM RTStock INNER JOIN " _
           &"RTObj ON RTStock.CUSID = RTObj.CUSID",CONN
  '  s1="<option value=""<>'*';全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &rs("CUSID") & """>" &rs("SHORTNC") &"</option>"
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
<TITLE>ADSL券商專案－各類報表列印</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  PGM="/report/adsl/adslstockdetail.asp?parm=" 
  stockid=document.all("sEARCH6").value
  yymmdd=document.all("search1").value
  pgm=pgm & stockid & ";" & yymmdd & ";" & "2"  '1:送件明細表 2:完工明細表
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

</SCRIPT>
</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">ADSL券商專案－報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">本週完工客戶明細表</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 

'完工日--default為系統日期

    Edate=DateValue(Now())    
%>  
</SELECT>  
 </font></td><tr><input type="text" name="SID" value="<%=stockid%>" style="display:none">
  <tr><td ALIGN="RIGHT"><font face="標楷體">證券公司 :</font></td>
  <td><select name="search6" size="1" class=dataListEntry>
      <%=s6%>
      </select></td></tr>
  <tr><td ALIGN="RIGHT"><font face="標楷體">請輸入資料截止日期 :</font></td>

<td>
   <input type="text" size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Edate%>" readonly>
   <input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="....">
</td></tr>
</table> 
<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center>
  <HR><P>
  <table width="100%"><tr>
  <td width="15%"> 說明：</td>
  <TD WIDTH="85%"><font size="2">(1)本表資料範圍，定義以資料截止日期往前七天為（本週）</FONT></TD>
  </TR>
  <TABLE></font></p> 
</BODY> 
</HTML>