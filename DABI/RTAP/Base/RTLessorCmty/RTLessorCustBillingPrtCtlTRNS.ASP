<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim s,t,t2
  '續約資料狀態
  s1=document.all("search1").value 
  s2=document.all("search2").value 
  s3=document.all("search3").value 
  if len(trim(s1))=0 or s1=0 then
     msgbox "續約年度不可空白"
  elseif len(trim(s2))=0 or s2=0 then
     msgbox "續約月份不可空白"
  else 
     s="續約年度︰" & s1 & "，續約月份︰" & s2 & "(不含已退租戶)，續約起日︰" & s3
     t=s1
     t2=s2
     t3=s3
     Dim winP,docP
     Set winP=window.Opener
     Set docP=winP.document
     docP.all("searchQry").value=t
     docP.all("searchQry2").value=t2
     docP.all("searchQry3").value=t3
     docP.all("searchShow").value=s
     docP.all("keyform").Submit
     winP.focus()
     window.close
  end if
End Sub

Sub btn1_onClick()  
  Dim winP
  Set winP=window.Opener
  winP.focus()
  window.close  
End Sub
-->
</script>
</head>

<body>
<table width="100%" ID="Table1">
  <tr class=dataListTitle align=center>ET-City用戶續約資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0 ID="Table2">
<tr><td class=dataListHead width="30%">續約年月</td>
    <td class=dataListentry>
    <%   if datepart("m",now())="12" then
             syy=cint(datepart("yyyy",now()))+1
             smm=1
          else
             syy=cint(datepart("yyyy",now()))
             smm=cint(datepart("m",now()))+1
          end if
    %>
        <select name="search1" ID="Select1">
        <% xyy=cint(datepart("yyyy",now()))-4 %> 
        <%for i= 0 to 9
            if xyy=syy then 
               xxselyy=" selected "
            else
               xxselyy=""
            end if
        %>
             <option <%=xxselyy%> value=<%=xyy%> ><%=xyy%></option>
        <% xyy=xyy+1
        next%>
    </select>
    
    <font size=2>-</font>
    
    <select name="search2" ID="Select2">
        <%for i=1 to 12%>
             <%if i=smm then 
                  xxselmm=" selected "
               else
                  xxselmm=""
               end if
             %>
             <option <%=xxselmm%> value=<%=i%> ><%=i%></option>
        <%next%>
    </select>
    
    <font size=2>-</font>
    
    <select name="search3" ID="Select3">
		<%
			if datepart("d",now()) <= 15 then 
				xxseld1 = " selected "
				xxseld2 = ""
			else
				xxseld2 = " selected "
				xxseld1 = ""
			end if
		%>
		<option <%=xxseld1%> value="01">上期</option>
		<option <%=xxseld2%> value="16">下期</option>
    </select>
     </td>
</tr>    
</table>
<p>
<table width="100%" align=right ID="Table3"><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand" ID="Submit1">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand" ID="Button1">
</td></tr></table>
</body>
</html>