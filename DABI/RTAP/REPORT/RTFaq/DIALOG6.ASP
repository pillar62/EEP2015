<%
    Dim rs,conn
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    

'---受理人員
    rs.Open "select rcvusr, b.name from RTFaqm a inner join RTEmployee b on a.rcvusr = b.emply where b.tran2 <>'10' group by rcvusr, b.name ",CONN
    s3="<option value="""" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s3=s3 &"<option value=""" &rs("rcvusr") & """>" &rs("name") &"</option>"
       rs.MoveNext
    Loop
    rs.Close

'---預定施工人員
    rs.Open "select a.assigneng, b.name from RTSndWork a inner join RTEmployee b on a.assigneng = b.emply where b.tran2 <>'10' and b.emply <> 'SYSTEM' group by a.assigneng, b.name ",CONN
    s5="<option value="""" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s5=s5 &"<option value=""" &rs("assigneng") & """>" &rs("name") &"</option>"
       rs.MoveNext
    Loop
    rs.Close

'---客訴施工經銷商
    rs.Open "select a.assigncons, b.shortnc from RTSndWork a inner join RTObj b on a.assigncons = b.cusid group by a.assigncons, b.shortnc order by 2 ",CONN
    s6="<option value="""" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &trim(rs("assigncons")) & """>" &rs("shortnc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close

'---轄區
    rs.Open "select groupnc from HBADSLCMTY where groupnc >'' and comtype not in ('1','2','4','5') group by groupnc ",CONN
    s7="<option value="""" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s7=s7 &"<option value=""" &rs("groupnc") & """>" &rs("groupnc") &"</option>"
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
<TITLE>客訴一覽表(Excel)</TITLE>

<SCRIPT language=VBScript>
Sub cmdSure_onClick
  'rpttype=document.all("search5").value
  symd=document.all("search1").value
  eymd=document.all("search2").value
  rcvusr=document.all("search3").value
  casetype=document.all("search4").value
  assigneng=document.all("search5").value
  assigncons=document.all("search6").value
  areanc=document.all("search7").value
  finish=document.all("search8").value
  finishS=document.all("search9").value
  finishE=document.all("search10").value

  'if rpttype="A" then
		pgm="/report/RTFaq/FaqReport6.asp?parm=" & symd &";"& eymd &";"& casetype &";"& rcvusr &";"& assigneng &";"& assigncons &";"& finish &";"& areanc &";"& finishS &";"& finishE &";"
  'elseif rpttype ="B" then
		'pgm="/report/HB/HBReport17b.asp?parm=" & eymd
  'end if    
  set wHandle=window.open (pgm,"win1")
  ' window.close
End Sub

Sub cmdcancel_onClick
  window.close
End Sub

   Sub Srbtnonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="search" & clickid
	   if isdate(document.all(clickkey).value) then
	      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
       end if
       call objEF2KDT.show(1)
       if objEF2KDT.strDateTime <> "" then
          document.all(clickkey).value = objEF2KDT.strDateTime
       end if
   END SUB

</SCRIPT>

</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="BACKGROUND: lightblue">

<DIV align=center><i><font face="標楷體" size="5" color="#ff00ff">客訴案報表列印</font></i> </DIV>
<DIV align=center><i><font face="標楷體" size="3" color="#ff00ff">客訴一覽表(Excel)</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 
    Edate=DateValue(Now())
    Sdate=DateValue(Now())
%>  
</SELECT></FONT></TD>
<!--
<tr><td ALIGN="RIGHT"><font face="標楷體">類別：</font></td>  
	<td><select name="search4" size="1" class=dataListEntry>
	   		<option value="A" selected>總表</option>
			<option value="B">明細</option></select></td></tr>
-->
<tr><td ALIGN="right"><font face="標楷體">受理時間:</font></td>
<td>
   <input size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
   <input type="button" id="B1" name="B1" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
	～
   <input size="10" maxlength="10" name="search2" align=right class=dataListEntry value="<%=Edate%>" readonly>
   <input type="button" id="B2" name="B2" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
</td></tr>

<tr><td ALIGN="right"><font face="標楷體">完工時間:</font></td>
<td>
   <input size="10" maxlength="10" name="search9" align=right class=dataListEntry value="" readonly>
   <input type="button" id="B9" name="B9" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
	～
   <input size="10" maxlength="10" name="search10" align=right class=dataListEntry value="" readonly>
   <input type="button" id="B10" name="B10" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
</td></tr>

<tr><td ALIGN="right"><font face="標楷體">受理人員：</font></td>
    <td><select name="search3" size="1" class=dataListEntry><%=s3%></select></td>
</tr>

<tr><td ALIGN="right"><font face="標楷體">方案別：</font></td>
    <td><select name="search4" size="1" class=dataListEntry>
        <option value="">全部</option>
<!--        <option value="1">元訊599</option>
        <option value="2">Cht399</option>		-->
        <option value="3">Sparq399</option>
<!--        <option value="4">東訊599</option> 
        <option value="5">EBT</option>			-->
        <option value="6">Sparq499</option>
        <option value="7">AVS-City</option>
        <option value="8">ET-City</option>
        </select>
	</td>
</tr>

<tr><td ALIGN="right"><font face="標楷體">轄區：</font></td>
	<td><select name="search7" size="1" class=dataListEntry><%=s7%>
	        <option value="台北">台北</option>
	        <option value="桃園">桃園</option>
	        <option value="台中">台中</option>
	        <option value="高雄">高雄</option>
		</select>
	</td>
</tr>

<tr><td ALIGN="right"><font face="標楷體">預定施工人員：</font></td>
    <td><select name="search5" size="1" class=dataListEntry><%=s5%></select></td>
</tr>

<tr><td ALIGN="right"><font face="標楷體">預定施工經銷商：</font></td>
    <td><select name="search6" size="1" class=dataListEntry><%=s6%></select></td>
</tr>

<tr><td ALIGN="right"><font face="標楷體">狀態別：</font></td>
    <td><select name="search8" size="1" class=dataListEntry>
	        <option value="">全部</option>
	        <option value="1">未完工</option>
	        <option value="2">未結案</option>
		</select>
	</td>
</tr>


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
