
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
  '-----------------------------------------------------------------------------------
  s2=document.all("search2").value
  if len(trim(s2)) > 0 then
     s="  客戶名稱：含('" & s2 & "')字元"  
     t=t & " rtobj.cusnc like '%" & s2 & "%'"
  else
     s=s & "  客戶名稱：全部  "
     t=t & " rtcust.cusid <>'*'"
  end if  
  '-----------------------------------------------------------------------------------  
  s3=document.all("search3").value
  if len(trim(s3)) > 0 then
     s="  HN聯單號碼：含('" & s3 & "')字元"  
     t=t & " and rtCUST.cusno like '%" & s3 & "%'"
  else
     s=s & "  HN聯單號碼：全部  "
     t=t & " and rtcust.cusNO <>'*'"
  end if  
  '-----------------------------------------------------------------------------------  
  s4=document.all("search4").value
  if len(trim(s4)) > 0 then
     s="  客戶地址：含('" & s4 & "')字元"  
     t=t & " and (rtcounty.cutnc + rtcust.township1 + rtcust.raddr1 )  like '%" & s4 & "%'"
  end if      
  '-----------------------------------------------------------------------------------  
  s5=document.all("search5").value
  S5ARY=SPLIT(S5,";")
  if len(trim(s5ARY(0))) > 0 then
     s="  連接方式：" & s5ARY(1) 
     IF S5ARY(0)="1" THEN '計量+單轉計
        t=t & " and (rtCUST.USEKIND IN ('計量制','單機轉計量')) "
     ELSEIF S5ARY(0)="2" THEN '單轉計
        t=t & " and  (rtCUST.USEKIND IN ('單機轉計量')) "
     ELSEIF S5ARY(0)="3" THEN '計量
        t=t & " and (rtCUST.USEKIND IN ('計量制')) "
     ELSEIF S5ARY(0)="4" THEN '單
        t=t & " and (rtCUST.USEKIND IN ('單機型')) "
     END IF
  end if    
  '-----------------------------------------------------------------------------------
  s6=document.all("search6").value
  S6ARY=SPLIT(S6,";")
  if len(trim(s6ARY(0))) > 0 then
     s="  客戶是否已撤銷退租：" & s6ARY(1) 
     IF S6ARY(0)="1" THEN		'全部
        t=t 
     ELSEIF S6ARY(0)="2" THEN	'未撤銷
        t=t & " and (rtCUST.DROPDAT is null or (rtCUST.DROPDAT is not null and OVERDUE ='Y')) "
     ELSEIF S6ARY(0)="3" THEN	'已撤銷
        t=t & " and (rtCUST.DROPDAT is not null and OVERDUE <>'Y') "
     END IF
  end if    
  '=======================================================================================      
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
<tr><td class=dataListHead width="40%">客戶名稱</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search2" size="25" maxlength="25" class=dataListEntry>
    </td></tr>
<tr><td class=dataListHead width="40%">HN聯單號碼</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search3" size="10" maxlength="10" class=dataListEntry>
    </td></tr>    
<tr><td class=dataListHead width="40%">客戶地址</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search4" size="40" maxlength="60" class=dataListEntry>
    </td></tr>        
<tr><td class=dataListHead width="40%">連接方式</td>
    <td width="60%"  bgcolor="silver">
      <select name="search5" size="1" class=dataListEntry ID="Select1">
        <option value=";全部" selected>全部</option>
        <option value="1;計量+單機轉計量">計量+單機轉計量</option>
        <option value="2;單機轉計量">單機轉計量</option>
        <option value="3;計量型">計量型</option>        
        <option value="4;單機型">單機型</option>
      </select>
    </td></tr>        
<tr><td class=dataListHead width="40%">是否已撤銷退租</td>
    <td width="60%"  bgcolor="silver">
      <select name="search6" size="1" class=dataListEntry>
        <option value="1;全部" selected>全部</option>
        <option value="2;未撤銷">未撤銷</option>
        <option value="3;已撤銷">已撤銷</option>        
      </select>
    </td></tr>        
</table>
<table width="70%" align=right><tr><td></td><td align=right>
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</body>
</html>