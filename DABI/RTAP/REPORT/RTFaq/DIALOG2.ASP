<%
    Dim rs,conn
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
'---經銷商
    rs.Open "select b.codenc as consignee from rtcmty a inner join rtcode b on a.comtype = b.code and b.kind ='B3' group by b.codenc "_
		   &"union "_
		   &"select b.codenc as consignee from rtcustadslcmty a inner join rtcode b on a.comtype = b.code and b.kind ='B3' group by b.codenc "_
		   &"union "_
		   &"select b.shortnc as consignee from rtsparqadslcmty a inner join rtobj b on a.consignee = b.cusid group by b.shortnc "_
		   &"union "_
		   &"select b.shortnc as consignee from rtsparq499cmtyline a inner join rtobj b on a.consignee = b.cusid group by b.shortnc "_
		   &"union "_
		   &"select b.shortnc as consignee from rtebtcmtyline a inner join rtobj b on a.consignee = b.cusid group by b.shortnc order by 1 ",CONN

    s3="<option value=""*"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s3=s3 &"<option value=""" &rs("consignee") & """>" &rs("consignee") &"</option>"
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
<TITLE>CALL OUT 記錄社區總表</TITLE>

<SCRIPT language=VBScript>
Sub cmdSure_onClick
  s1value=document.all("search1").value
  s2value=document.all("search2").value  
  s3value=document.all("search3").value  
  s4value=document.all("search4").value 
  
  pgm="/report/RTFaq/FaqReport2.asp?parm=" & s1value &";"& s2value &";"& s3value &";"& s4value
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

<DIV align=center><i><font face="標楷體" size="5" color="#ff00ff">客服報表列印</font></i> </DIV>
<DIV align=center><i><font face="標楷體" size="3" color="#ff00ff">CALL OUT 記錄社區總表</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 
    'Edate=DateValue(Now())    
%>  
</SELECT></FONT></TD>


<tr><td ALIGN="right"><font face="標楷體">方案別：</font></td>
    <td><select name="search1" size="1" class=dataListEntry>
        <option value="*">全部</option>    
        <option value="Cht599">中華599</option>
        <option value="Cht399">中華399</option>
        <option value="Sparq399">速博399</option>
        <option value="Sparq499">速博499</option>
        <option value="EBT">東森</option></select></td></tr>

<tr><td ALIGN="right"><font face="標楷體">地區：</font></td>
    <td><select name="search2" size="1" class=dataListEntry>
        <option value="*">全部</option>    
        <option value="台北">台北</option>
        <option value="桃園">桃園</option>
        <option value="台中">台中</option>
        <option value="高雄">高雄</option></select></td></tr>

<tr><td ALIGN="right"><font face="標楷體">經銷商：</font></td>
    <td><select name="search3" size="1" class=dataListEntry><%=s3%></select></td></tr>

<tr><td ALIGN="right"><font face="標楷體">社區名稱：</font></td>
    <td><input name="search4" maxlength="30" size="30" value="*" class="dataListEntry"></td></tr>


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
