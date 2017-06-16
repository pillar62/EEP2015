
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
  '----社區名稱
  S1=document.all("search1").value  
  If Len(s1)=0 Or s1="" Then
     t=t &" AND (RTEBTCmtyH.ComN<> '*' )" 
  Else
     s=s &"  社區名稱:包含('" &S1 & "'字元)"
     t=t &" AND (RTEBTCmtyH.ComN LIKE '%" &S1 &"%')" 
  End If
  '----社區序號
  s2=document.all("search2").value
  IF LEN(TRIM(S2)) > 0 THEN
     s=s &"  社區序號:" &s2 & " "
     t=t &" AND (RTEBTFtpAvsparaRpl.COMQ1 =" & S2 & ") "
  END IF
 '----主線序號  
  s3=document.all("search3").value  
  IF LEN(TRIM(S3)) > 0 THEN
     s=s &"  主線序號:" &s3 & " "
     t=t &" AND (RTEBTFtpAvsparaRpl.LINEQ1 =" & S3 & ") "
  END IF  
  '----轉檔類別
  s4ary=split(document.all("search4").value,";")  
  If Len(trim(s4ary(0)))=0 Or s4ary(0)="" Then
     s=s &"  轉檔類別:全部  "
  Elseif s4ary(0)="A" then
     s=s &"  轉檔類別:" &s4ary(1)
     t=t &" AND (RTEBTFtpAvsparaRpl.FLAG='" & S4ARY(0) & "') "
  Elseif s4ary(0)="F" then
     s=s &"  轉檔類別:" &s4ary(1)
     t=t &" AND (RTEBTFtpAvsparaRpl.FLAG='" & S4ARY(0) & "') "
  elseif s4ary(0)="C" then
     s=s &"  轉檔類別:" &s4ary(1)
     t=t &" AND (RTEBTFtpAvsparaRpl.FLAG='" & S4ARY(0) & "') "
  End If        
  '----處理狀態
  s5ary=split(document.all("search5").value,";")  
  If Len(trim(s5ary(0)))=0 Or s5ary(0)="" Then
  Elseif s5ary(0)="1" then
     '未結案
     s=s &"  處理狀態:" &s5ary(1)
     t=t &" AND (RTEBTFtpAvsparaRpl.CLOSEDAT IS NULL AND RTEBTFtpAvsparaRpl.DROPDAT IS NULL ) "
  Elseif s5ary(0)="2" then
     '已作廢
     s=s &"  處理狀態:" &s5ary(1)
     t=t &" AND (RTEBTFtpAvsparaRpl.DROPDAT IS not NULL ) "
  elseif s5ary(0)="3" then
     '已結案
     s=s &"  處理狀態:" &s5ary(1)
     t=t &" AND (RTEBTFtpAvsparaRpl.CLOSEDAT IS NOT NULL ) "
  elseif s5ary(0)="4" then
     '已清除轉檔記錄但尚未結案
     s=s &"  處理狀態:" &s5ary(1)
     t=t &" AND (RTEBTFtpAvsparaRpl.CLOSEDAT IS NULL AND RTEBTFtpAvsparaRpl.CLRFLAG IS not NULL ) "
  ELSE
      s=s &"  處理狀態:全部"     
  End If        
    
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
  Dim winP
  Set winP=window.Opener
  winP.focus()
  window.close  
End Sub
-->
</script>
</head>
<body>
<table width="100%">
  <tr class=dataListTitle align=center>AVS用戶資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry ID="Text1"> 
    </td></tr>
<tr><td class=dataListHead width="40%">社區/主線序號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="5" name="search2" class=dataListEntry ID="Text5"> 
      <font size=2>-</font>
      <input type="text" size="5" name="search3" class=dataListEntry ID="Text6"> 
    </td></tr>        
<tr><td class=dataListHead width="40%">轉檔類別</td>
    <td width="60%"  bgcolor="silver">
      <select name="search4" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="A;申請">申請</option>
        <option value="F;測通回報">測通回報</option>
        <option value="C;取消">取消</option>
      </select>
     </td>
</tr>
<tr><td class=dataListHead width="40%">處理狀態</td>
    <td width="60%"  bgcolor="silver">
      <select name="search5" size="1" class=dataListEntry>
        <option value="1;未結案" selected>未結案</option>
        <option value="2;已作廢">已作廢</option>
        <option value="3;已結案">已結案</option>
        <option value="4;已清除轉檔記錄但尚未結案">已清除轉檔記錄但尚未結案</option>
        <option value=";全部">全部</option>
      </select>
     </td>
</tr>
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>