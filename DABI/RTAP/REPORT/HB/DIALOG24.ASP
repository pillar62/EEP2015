<%
    Dim rs,conn,S1
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
	'縣市
    rs.Open "SELECT CUTID, CUTNC FROM RTCounty ",CONN
    s1="<option value=""*"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s1=s1 &"<option value=""" &rs("CUTID") & """>" &rs("CUTNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
    '營運點
    rs.Open "SELECT operationname FROM RTCtyTown where operationname <>'' group by operationname",CONN
    s2="<option value=""*"" selected>全部</option>" &vbCrLf
    s2=s2 & "<option value=""未知"">未知</option>" &vbCrLf
    Do While Not rs.Eof
       s2=s2 &"<option value=""" &rs("operationname") & """>" &rs("operationname") &"</option>"
       rs.MoveNext
    Loop
    rs.Close

    '客戶鄉鎮
    rs.Open "SELECT cutid2, township2 FROM  RTCustAdsl "&_
			"where	docketdat is not null "&_
			"and 	(dropdat is null or overdue ='Y') "&_
			"and	township2 <>'' "&_
			"union "&_
			"SELECT  cutid1, township1 FROM  RTCust "&_
			"where	docketdat is not null "&_
			"and 	(dropdat is null or overdue ='Y') "&_
			"and	township1 <>'' "&_
			"group by  cutid1, township1 "&_
			"order by 1,2 ",CONN
    s3="<option value=""*"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s3=s3 &"<option value=""" &rs("township2") & """>" &rs("township2") &"</option>"
       rs.MoveNext
    Loop
    rs.Close

    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>

<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>中華客戶信封列印</TITLE>
<SCRIPT language=VBScript>

Sub cmdSure_onClick
  cutid=document.all("search1").value
  operationname=document.all("search2").value
  township2=document.all("search3").value
  rpttype=document.all("search4").value
  'stat=document.all("search3").value
  'if len(comn)=0 then
 ' 		comn ="*"
  'end if

  if rpttype="A" then
  		PGM="/report/HB/HBReport24a.asp?parm=" 
  elseif rpttype ="B" then
  		PGM="/report/HB/HBReport24b.asp?parm=" 
  'elseif rpttype ="C" then
  '		PGM="/report/fttb/HBReport1c.asp?parm=" 
  'elseif rpttype ="E" then
  '		PGM="/report/fttb/HBReport1e.asp?parm=" 
  'elseif rpttype ="F" then
  '		PGM="/report/fttb/HBReport1f.asp?parm=" 
  'elseif rpttype ="G" then
  '		PGM="/report/fttb/HBReport1g.asp?parm=" 
  end if

  'pgm=pgm & comm & ";" & Rtrim(groupid) & ";" & eusr
  pgm=pgm & cutid &";"& operationname &";"& township2
  window.open pgm 
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

<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">中華－報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">中華客戶信封列印 </font></i> </DIV>
<P><P>

<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<tr><td ALIGN="RIGHT"><font face="標楷體">列印總類：</font></td>  
	<td><select name="search4" size="1" class=dataListEntry>
	   		<option value="A" selected>信封列印</option>
			<option value="B">一覽表下載(.xls)</option></select></td></tr>
<tr><td ALIGN="RIGHT"><font face="標楷體">客戶地址(縣市) :</font></td>
    <td><select name="search1" size="1" class=dataListEntry>
        <%=s1%>
        </select></td></tr>
<tr><td ALIGN="RIGHT"><font face="標楷體">客戶地址(鄉鎮) :</font></td>
    <td><select name="search3" size="1" class=dataListEntry>
        <%=s3%>
        </select></td></tr>
<tr><td ALIGN="RIGHT"><font face="標楷體">營運點 :</font></td>
    <td><select name="search2" size="1" class=dataListEntry>
        <%=s2%>
        </select></td></tr>
<!--
<tr><td ALIGN="RIGHT"><font face="標楷體">客戶狀態 :</font></td>
    <td><select name="search3" size="1" class=dataListEntry>
		<option value="A">全部</option>
		<option value="B">有效戶</option>
		<option value="C">退租戶</option>
		<option value="D">欠拆戶</option>
		<option value="E" selected>有效+欠拆</option>
        </select>
    </td></tr>
<tr><td ALIGN="RIGHT"><font face="標楷體">請輸入社區名稱 :</font></td>
	<td><input type="text" size="20" maxlength="30" name="search2" class=dataListEntry >
	</td></tr>
-->
</table>

<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center>
  <HR>
  <P>PS.信封的列印：請設定列印格式, 將紙張大小由[A4]更改為[CBBN信封], 或自行設定寬度21cm, 高度12.7cm
  </P>
</BODY> 
</HTML>

