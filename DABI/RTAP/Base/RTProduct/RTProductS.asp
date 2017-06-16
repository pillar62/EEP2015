<!-- #include virtual="/webap/include/lockright.inc" -->
<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")
'--------- 產品類別
    rs.Open "SELECT CODE,CODENC from rtcoDE WHERE KIND='E2' " _
           &"ORDER BY CODENC ",conn
    s1="<option value=""<>'*';：全部"" selected>全部</option>" &vbCrLf           
    Do while not rs.EOF
       s1= s1 & "<option value=""='" & rs("cODE") & "';" & "：" &  rs("CODENC") & """>" & rs("CODENC") & "</option>" & vbcrlf
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
  s1ary=Split(document.all("search1").value,";")
  S2=document.all("search2").value
  s3=document.all("search3").value
  s=s & "產品類別︰" & s1ary(1) & " "
  t=t & " rtprodh.prodtyp " & s1ary(0) & " "
  if len(trim(s2)) > 0 then
    s=s & " 產品編號：含(""" & document.all("search2").value & """)字元 " 
    t=t & " and rtprodh.prodno like '%" & S2 & "%' "
  end if
  if len(trim(s3)) > 0 then
    s=s & " 產品名稱：含(""" & document.all("search3").value & """)字元 " 
    t=t & " and rtprodh.prodnc like '%" & S3 & "%' "
  end if  
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
<table width="70%">
  <tr class=dataListTitle align=center>產品資料搜尋條件</td><tr>
</table>
<table width="70%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">產品類別</td>
    <td width="60%" bgcolor=silver>
      <select name="search1" size="1" class=dataListEntry>
        <%=S1%>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">產品編號</td>
    <td width="60%"  bgcolor=silver>
     <input class=dataListEntry name="search2" maxlength=10 size=10 style="TEXT-ALIGN: left">
     </td></tr>
<tr><td class=dataListHead width="40%">產品名稱</td>
    <td width="60%"  bgcolor=silver>
     <input class=dataListEntry name="search3" maxlength=50 size=30 style="TEXT-ALIGN: left"></td></tr>
</table>
<table width="50%" align=right><tr><td></td>
<td align=right>
<input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
<input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td>
</tr></table>
</body>
</html>