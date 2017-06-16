<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")
'-------- 重新比對所有table
    
    conn.Execute ("usp_ATableDesc")
    
    
'--------- Owner
    rs.Open "SELECT tbOwner from ATableList GROUP BY tbOwner",conn
    s1="<option value="""" selected>全部</option>" &vbCrLf           
    Do while not rs.EOF
       s1= s1 & "<option value=""" & rs("tbOwner") & """>" & rs("tbOwner") & "</option>" & vbcrlf
    rs.movenext
    Loop
    rs.Close
'--------- 類別
    rs.Open "SELECT tbType from ATableList GROUP BY tbType",conn
    s2="<option value="""" selected>全部</option>" &vbCrLf   
    Do While not rs.eof
       s2= s2 & "<option value=""" & rs("tbType") & """>" & rs("tbType") & "</option>" & vbcrlf    
    rs.MoveNext
    Loop
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">


Sub btn_onClick()
  dim s,t
  t=""
  s=""
  's1ary=Split(document.all("search1").value,";")
  's2ary=Split(document.all("search2").value,";")
  s1ary=document.all("search1").value
  s2ary=document.all("search2").value
  
  s="Owner：" & s1ary & "　類別：" & s2ary & "　Table名稱(英)：含(""" & document.all("search3").value & """)字元"
  t=t & "(tbOwner like '" & s1ary & "%') AND (tbType like '" & s2ary & "%')"
  t=t & " and (tbName like '%" & document.all("search3").value & "%')"

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

</script>
</head>
<body>
<center>
<table width="50%">
  <tr class=dataListTitle align=center>Table搜尋條件</td><tr>
</table>
<table width="50%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">Owner</td>
    <td width="60%" bgcolor=silver>
      <select name="search1" size="1" class=dataListEntry>
        <%=S1%>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">類別</td>
    <td width="60%"  bgcolor=silver>
      <select name="search2" size="1" class=dataListEntry>
        <%=S2%>
      </select>    </td></tr>
<tr><td class=dataListHead width="40%">Table名稱(英)</td>
    <td width="60%"  bgcolor=silver>
     <input class=dataListEntry name="search3" maxlength=50 size=20 style="TEXT-ALIGN: left"></td></tr>      
</table>
<table width="50%" align=right><tr><td></td>
<td align=right>
<input type="button" value=" 查詢 " class=dataListButton name="btn" style="cursor:hand">
<input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td>
</tr></table>
</body>
</html>