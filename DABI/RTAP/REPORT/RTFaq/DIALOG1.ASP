<%
    Dim rs,conn
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
'---受理人員
    rs.Open "select a.rcvusr, c.cusnc from rtfaqh a inner join rtemployee b on a.rcvusr=b.emply inner join rtobj c on c.cusid = b.cusid where a.dropdate is null and b.tran2 <>'10' group by a.rcvusr, c.cusnc order by left(a.rcvusr, 1),2 ",CONN
    s3="<option value=""*"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s3=s3 &"<option value=""" &rs("rcvusr") & """>" &rs("cusnc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'---轄區
    rs.Open "select b.codenc from RTCmty a inner join RTcode b on a.comtype = b.code where b.kind ='B3' and b.parm1 <>'AA' group by b.codenc " &_
			"union " &_
			"select b.codenc from RTCustAdslCmty a inner join RTcode b on a.comtype = b.code where b.kind ='B3' and b.parm1 <>'AA' group by b.codenc " &_
			"union " &_
			"select b.shortnc as codenc from RTSparqAdslCmty a inner join RTObj b on a.consignee = b.cusid group by b.shortnc " &_
			"union " &_
			"select b.shortnc as codenc from RTSparq499CmtyLine a inner join RTObj b on a.consignee = b.cusid group by b.shortnc " &_
			"union " &_
			"select b.shortnc as codenc from RTEBTCmtyLine a inner join RTObj b on a.consignee = b.cusid group by b.shortnc " &_
			"union " &_
			"select b.shortnc as codenc from RTLessorCmtyLine a inner join RTObj b on a.consignee = b.cusid group by b.shortnc " &_
			"union " &_
			"select b.shortnc as codenc from RTLessorAvsCmtyLine a inner join RTObj b on a.consignee = b.cusid group by b.shortnc ",CONN
    s7="<option value=""*"" selected>全部</option>" &vbCrLf
    s7=s7 & "<option value=""台北"">台北</option>" &vbCrLf
    s7=s7 & "<option value=""桃園"">桃園</option>" &vbCrLf
    s7=s7 & "<option value=""台中"">台中</option>" &vbCrLf
    s7=s7 & "<option value=""高雄"">高雄</option>" &vbCrLf            
    Do While Not rs.Eof
       s7=s7 &"<option value=""" &trim(rs("codenc")) & """>" &rs("codenc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'---客訴排除人員
    rs.Open "select a.finishusr, c.cusnc from rtfaqh a inner join rtemployee b on a.finishusr=b.emply inner join rtobj c on c.cusid = b.cusid where a.dropdate is null and b.tran2 <>'10' group by  a.finishusr, c.cusnc order by left(a.finishusr, 1), 2 ",CONN
    s5="<option value=""*"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s5=s5 &"<option value=""" &rs("finishusr") & """>" &rs("cusnc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'---客訴排除廠商
    rs.Open "select a.finishfac, b.shortnc from rtfaqh a inner join rtobj b on a.finishfac = b.cusid group by a.finishfac, b.shortnc order by 2 ",CONN
    s6="<option value=""*"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &trim(rs("finishfac")) & """>" &rs("shortnc") &"</option>"
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
<TITLE>客訴一覽表</TITLE>

<SCRIPT language=VBScript>
Sub cmdSure_onClick
  symd=document.all("search1").value
  eymd=document.all("search2").value 
	IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
	IF LEN(TRIM(EYMD))=0 THEN SYMD="2070/12/31"
  rcvusr=document.all("search3").value
  casetype=document.all("search4").value  
  finishusr=document.all("search5").value  
  finishfac=document.all("search6").value 
  areanc=document.all("search7").value    
  finish=document.all("search8").value      
  
  pgm="/report/RTFaq/FaqReport1.asp?parm=" & symd &";"& eymd &";"& casetype &";"& rcvusr &";"& finishusr &";"& finishfac &";"& areanc &";"& finish
  set wHandle=window.open (pgm,"win1")
  ' window.close
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
	width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="BACKGROUND: lightblue">

<DIV align=center><i><font face="標楷體" size="5" color="#ff00ff">客服報表列印</font></i> </DIV>
<DIV align=center><i><font face="標楷體" size="3" color="#ff00ff">客訴一覽表</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 
    'Edate=DateValue(Now())    
%>  
</SELECT></FONT></TD>


<tr><td ALIGN="right"><font face="標楷體">受理期間：</font></td>
	<td><input size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
		<input type="button" id="B1" name="B1" height="100%" width="100%" style="Z-INDEX: 1" value="...."> ～
		<input size="10" maxlength="10" name="search2" align=right class=dataListEntry value="<%=Edate%>" readonly>
		<input type="button" id="B2" name="B2" height="100%" width="100%" style="Z-INDEX: 1" value="...."></td></tr>

<tr><td ALIGN="right"><font face="標楷體">受理人員：</font></td>
    <td><select name="search3" size="1" class=dataListEntry><%=s3%></select></td></tr>
		
<tr><td ALIGN="right"><font face="標楷體">方案別：</font></td>
    <td><select name="search4" size="1" class=dataListEntry>
        <option value="*">全部</option>
        <option value="1">元訊599</option>
        <option value="2">Cht399</option>
        <option value="3">Sparq399</option>
        <option value="4">東訊599</option> 
<!--        <option value="5">EBT</option>	-->
        <option value="6">Sparq499</option>
        <option value="7">AVS-City</option>
        <option value="8">ET-City</option>
        </select></td></tr>

<tr><td ALIGN="right"><font face="標楷體">結案：</font></td>
    <td><select name="search8" size="1" class=dataListEntry>
        <option value="全部">全部</option>    
        <option value="已結案">已結案</option>
        <option value="未結案">未結案</option></select></td></tr>

<tr><td ALIGN="right"><font face="標楷體">轄區：</font></td>
    <td><select name="search7" size="1" class=dataListEntry><%=s7%></select></td></tr>

<tr><td ALIGN="right"><font face="標楷體">排除員工：</font></td>
    <td><select name="search5" size="1" class=dataListEntry"><%=s5%></select></td></tr>

<tr><td ALIGN="right"><font face="標楷體">排除廠商：</font></td>
    <td><select name="search6" size="1" class=dataListEntry"><%=s6%></select></td></tr>

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
