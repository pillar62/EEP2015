
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
  '----應收帳款年月
  S1=document.all("search1").value  
  if len(trim(s1))=0 then
     msgbox "請輸入應收帳款年月!"
  else
     SYMD=CDATE(MID(S1,1,4) & "/" & MID(S1,5,2) & "/" & "01")
     SYMDX=MID(S1,1,4) & "/" & MID(S1,5,2)
     s=s &"  應收帳款年月:" &Symdx & ""
     t=t &" (RTEBTCUST.STRBILLINGDAT < '" & Symd & "' ) and ( RTEBTCUST.DROPDAT >='" &  Symd & "' or RTEBTCUST.DROPDAT is null) " 
     t1=symd

     Dim winP,docP
     Set winP=window.Opener
     Set docP=winP.document
     docP.all("searchQry").value=t
     docP.all("searchQry2").value=t1
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
<table width="100%">
  <tr class=dataListTitle align=center>AVS欠費用戶資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">應收帳款年月</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="6" name="search1" class=dataListEntry ID="Text5"> 
    </td></tr>  
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>