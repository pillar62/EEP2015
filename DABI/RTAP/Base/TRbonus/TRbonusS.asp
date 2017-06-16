<%
    Dim rs,i,conn
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")
    rs.Open "SELECT areaid,areanc from RTarea where AreaType='1' ",conn
    s1="<option value=""<>'*';：全部"" selected>全部</option>" &vbCrLf           
    Do while not rs.EOF
       s1= s1 & "<option value=""='" & rs("areaid") & "';" & "：" &  rs("areanc") & """>" & rs("AreaNC") & "</option>" & vbcrlf
    rs.movenext
    Loop
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<html>
<head>

<script language="VBScript">
<!-- 
Sub btn_onClick()
  Dim winP,docP,S,t
  t=""
  s=""
  s1ary=split(document.all("D1").value,";")
  s2=document.all("search2").value
  s=s & "業績獎金審核狀況：" & s1ary(1) & "  " & "列印批號：" & s2
  if s1ary(0)="1" then
     t=t & " and datalength(rtrim(A.RCVDTLNo)) >0 "
  elseif s1ary(0)="2" then
     t=t & " and  a.FINRDFMDAT Is not null "
  elseif s1ary(0)="3" then
     t=t & " AND a.FINRDFMDAT Is null "
  end if
  if len(trim(s2))> 0 then
     t=t & " and a.rcvdtlno='" & s2 & "'"
  end if  
  '---t transfor to array
  t=t & ";" & s2   
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
<!-- #include virtual="/WebUtility/DBAUDI/DataList.css" -->
<form name="form">
<center>
<div class=datalisttitle>請輸入（選擇）業績獎金審核搜尋條件</div><p>
<table border=1 cellspacing=0 cellpadding=0>
<tr>
<td class=datalisthead>轄區</td>
<td bgcolor="silver">
	<select class=dataLISTENTRY  SIZE=1 name="D1">
		<%=S1%>
	</select>
</td>
</TR>
<tr>
    <td class=datalisthead>年度</td>
    <td bgcolor="silver"><input class=datalistentry name="year" size="4" maxlength="4">
    <%%Y)%>
    </td>
</tr>
<tr>
    <td class=datalisthead>月份</td>
    <td bgcolor="silver"><input class=datalistentry name="month" size="3" maxlength="3">
    <%(%m-1)%>
    </td>
</tr>
<tr>
    <td class=datalisthead>業務員</td>
    <td bgcolor="silver"><input class=datalistentry name="sales" size="6" maxlength="6">
    </td>
</tr>


</table>
<table><tr><td></td><td align="right">
	<input class=datalistbutton type="submit" name="btn" onsubmit="btn_onclick" value="查　詢">
    <input class=datalistbutton type="button" name="btn1" value="結　束">
</td></tr></table>
</form>
</body>
</html>