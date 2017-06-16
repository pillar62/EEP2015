
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV3/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim s,t
  t=""
  s=""
  s1=document.all("search1").value
  if len(trim(s1)) > 0 then
     s="  社區名稱：含('" & s1 & "')字元"  
     t=t & " AND rtcustadslcmty.cOMN like '%" & s1 & "%'"
  else
     s=s & "  社區名稱：全部  "
     t=t & " AND rtcustadslcmty.COMN <>'*'"
  end if    
  s2=document.all("search2").value
  if len(trim(s2)) > 0 then
     s="  客戶名稱：含('" & s2 & "')字元"  
     t=t & " AND rtobj.cusnc like '%" & s2 & "%'"
  else
     s=s & "  客戶名稱：全部  "
     t=t & " AND rtcustADSL.cusid <>'*'"
  end if  
  s3=document.all("search3").value
  if len(trim(s3)) > 0 then
     s="  HN聯單號碼：含('" & s3 & "')字元"  
     t=t & " and rtCUSTADSL.cusno like '%" & s3 & "%'"
  else
     s=s & "  HN聯單號碼：全部  "
     t=t & " and rtcustADSL.cusNO <>'*'"
  end if  
  s4=document.all("search4").value
  if len(trim(s4)) > 0 then
     s="  裝機地址：含('" & s4 & "')字元"  
     t=t & " and (rtcounty.cutnc + rtcustADSL.township2 + rtcustADSL.raddr2 )  like '%" & s4 & "%'"
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
  <tr class=dataListTitle align=center>請輸入(選擇)客戶資料搜尋條件</td><tr>
</table>
<table width="70%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search1" size="25" maxlength="25" class=dataListEntry>
    </td></tr>
<tr><td class=dataListHead width="40%">客戶名稱</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search2" size="25" maxlength="25" class=dataListEntry ID="Text1">
    </td></tr>    
<tr><td class=dataListHead width="40%">HN聯單號碼</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search3" size="10" maxlength="10" class=dataListEntry>
    </td></tr>    
<tr><td class=dataListHead width="40%">裝機地址</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search4" size="40" maxlength="60" class=dataListEntry>
    </td></tr>        
</table>
<table width="70%" align=right><tr><td></td><td align=right>
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</body>
</html>