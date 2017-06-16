<%
if not Session("passed") then
   Response.Redirect "http://www.cbbn.com.tw/Consignee/logon.asp"
end if
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">

<script language="VBScript">

sub btn_onClick()
  ConsigneeID = document.all("hidden1").value

  t =" RTSparqAdslCmtyX.Consignee ='" &Cstr(ConsigneeID)& "' "
  
  '----社區名稱
  r=document.all("search1").value  
  If Len(r)=0 Or r="" Then
     s=s &"  社區名稱:全部 "  
     t=t &" and (RTSparqAdslCmtyX.Comn <> '*') "
  Else
     s=s &"  社區名稱:" &r & " "
     t=t &" and (RTSparqAdslCmtyX.ComN LIKE '%" &r &"%') " 
  End If
  
  '----進度狀況
  arystr=split(document.all("search2").value,";")
  s=s &"  進度狀況:" & aryStr(1)
  if aryStr(0)="" then
     t=t & " and (rtsparqadslcustX.dropdat is null and rtsparqadslcustX.agree <>'N' ) "
  elseif aryStr(0)="1" then
     t=t & ""     
  elseif aryStr(0)="2" then
     t=t & " and (RTSparqAdslCmtyX.AGREE ='Y') "
  elseif aryStr(0)="3" then
     t=t & " and (RTSparqAdslCmtyX.AGREE = 'N') "
  elseif aryStr(0)="4" then
     t=t & " and (RTSparqAdslCmtyX.SURVYDAT is null) "     
  end if
  
  Dim winP,docP
  Set winP=window.opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchShow").value=s
  docP.all("keyform").Submit
  winP.focus()
  window.close
End sub

Sub btn1_onClick()  
  Dim winP
  Set winP=window.Opener
  winP.focus()
  window.close  
End Sub
</script>

</head>
<body>
<table width="100%">
  <tr class=dataListTitle align=center>ADSL未送件社區資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>

<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry> 
    </td></tr>    
<tr><td class=dataListHead width="40%">進度狀況</td>
    <td width="60%"  bgcolor="silver">
      <select name="search2" size="1" class=dataListEntry>
   <!--
        <option value=";全部(不含撤銷、退租、不可建置戶)" selected>全部(不含撤銷、退租、不可建置戶)</option>
        -->
        <option value="1;全部" selected>全部</option>
        <option value="2;同意建置">同意建置</option>
        <option value="3;不同意建置">不同意建置</option>
        <option value="4;尚未堪察">尚未堪察</option>
      </select>
     </td>
</tr>
</table>
<table width="100%" align=right><tr><TD><input type="hidden" name ="hidden1" value="<%=Session("UserID")%>"></td><td align="right">
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" style="cursor:hand" onsubmit="btn_onclick">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>