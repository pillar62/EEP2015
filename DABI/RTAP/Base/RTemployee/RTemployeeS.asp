<!-- #include virtual="/webap/include/lockright.inc" -->
<%
    Dim rs,i,conn
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")
'--------- 縣市別
    rs.Open "SELECT cutid,cutnc from rtcounty " _
           &"ORDER BY cutid ",conn
    s1="<option value=""<>'*';：全部"" selected>全部</option>" &vbCrLf           
    Do while not rs.EOF
       s1= s1 & "<option value=""='" & rs("cutid") & "';" & "：" &  rs("cutnc") & """>" & rs("CUTNC") & "</option>" & vbcrlf
    rs.movenext
    Loop
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV2/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim s,t
  t=""
  s=""

  s4ary=" AREAID like '" & document.all("search4").value &"' "	  

  s1ary = document.all("search1").value
  if s1ary ="有" then 
	s1ary= " and healthins+laborins >0"
  elseif s1ary ="無" then 
	s1ary= " and healthins+laborins =0"
  else 
	s1ary= "" 
  end if

  s2ary=Split(document.all("search2").value,";")
  s3ary=" and TRAN2 like '" & document.all("search3").value &"' "	
  
  s="有無保費：" & s1ary & "　員工姓名：含('" & document.all("search2").value & "')字元"
  t=t & s4ary &" AND (c.cusnc like '%" & document.all("search2").value & "%')" & s3ary & s1ary
  
  'ALARM T
  
  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchShow").value=s
  docP.all("keyform").Submit
  winP.focus()
  window.close
End Sub

Sub btn1_onClick()
  window.close
End Sub  
-->
</script>
</head>
<body>
<center>
<table width="50%">
  <tr class=dataListTitle align=center>員工資料搜尋條件</td><tr>
</table>
<table width="50%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">有無勞健保</td>
	<td bgcolor=silver>
		<select name="search1" size="1" class=dataListEntry ID="Select1">
	   		<option value="有">有</option>
			<option value="無">無</option>
			<option value="全部" selected>全部</option>
		</select>
	</td></tr>
<tr><td class=dataListHead width="40%">姓名</td>
    <td width="60%" bgcolor=silver>
    <input class=dataListEntry name="search2" maxlength=10 size=10 style="TEXT-ALIGN: left">   </td></tr>
    
<tr><td class=dataListHead width="40%">離職否</td>
		<td bgcolor=silver><select name="search3" size="1" class=dataListEntry>
	   		<option value="">未離職</option>
			<option value="10">已離職</option>
			<option value="%" selected>全部</option>
		</select></td></tr>

<tr><td class=dataListHead width="40%">轄區別</td>
		<td bgcolor=silver><select name="search4" size="1" class=dataListEntry>
	   		<option value="%" selected>全部</option>
			<option value="C1">台北</option>
			<option value="C2">桃園</option>
			<option value="C3">台中</option>
			<option value="C4">高雄</option>
		</select></td></tr>

</table>
<table width="50%" align=right><tr><td></td><td align=right>
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">  
</td></tr></table>
</body>
</html>