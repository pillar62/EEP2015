<%
    yyyy = request("search1")
    mm = request("search3")
    Dim rs,conn,S6
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    'S6=""
    rs.Open "SELECT GROUPID, GROUPNC FROM RTSALESGROUP ",CONN
    s6="<option value=""*"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &rs("GROUPID") & """>" &rs("GROUPNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close

    rs.Open "select emply from rtemployee where netid ='"& session("userid") &"'" ,CONN
    s1 = rs("emply")	
    rs.close 
    
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>獎金明細表列印</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  PGM="/report/hb/HBReport9.asp?parm=" 
  
  yyyymm=document.all("search3").value 
  if Len(Rtrim(yyyymm)) = 1 then yyyymm = "0" & yyyymm
  yyyymm=document.all("search1").value & yyyymm
  groupid=document.all("sEARCH6").value  '業務群組代號
  eusr=document.all("search4").value 
  pgm=pgm & yyyymm & ";" & Rtrim(groupid) & ";" & eusr
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
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">獎金明細表－報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">機櫃建置,用戶裝機, 業務開發,社區開通及車資 </font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
</SELECT>  
 </font></td> 

<tr><td ALIGN="RIGHT"><font face="標楷體">業務群組 :</font></td>
    <td><select name="search6" size="1" class=dataListEntry>
        <%=s6%>
        </select>
    </td>
</tr>
 
  <tr><td ALIGN="RIGHT"><font face="標楷體">請輸入獎金年份(西元) :</font></td>
<td>
   <input type="text" size="5" maxlength="4" name="search1" align=right class=dataListEntry value="<%=yyyy%>">
</td></tr>
  
  <tr><td ALIGN="RIGHT"><font face="標楷體">請輸入獎金月份 :</font></td>
<td>
   <input type="text" size="3" maxlength="2" name="search3" align=right class=dataListEntry value="<%=mm%>">
</td></tr>
   <input type="hidden" size="6" name="search4" value="<%=s1%>">
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
  <TD WIDTH="85%"><font size="2">各組業助自行列印，並與excel檔的獎金互相校對</FONT></TD>
  </TR>
  <TABLE></font></p> 
</BODY> 
</HTML>
