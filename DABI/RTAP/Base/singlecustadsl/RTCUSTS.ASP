<%
 Dim dspkey(100),DSN
 DSN="DSN=RTLib"
 dspkey(3)=Request.Form("search3")
 dspkey(4)=Request.Form("search4")
 dspkey(5)=Request.Form("search5")
%>
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
     s="客戶名稱：含('" & s1 & "')字元"  
     t=t & " rtobj.cusnc like '%" & s1 & "%' "
  else
     s="客戶名稱：全部  "
     t=t & " singlecustadsl.cusid <> '*' "
  end if
  s2=document.all("search2").value
  s2ary=split(s2,";")
  
  s=S & "客服回覆狀況：" & s2ary(1)  
  if s2ary(0) <> "" then
     t=t & " and singlecustadsl.REPLYDATE IS " & s2ary(0) 
  end if
  s3=document.all("search3").value
  s4=document.all("search4").value
  s5=document.all("search5").value
  if len(trim(s3))>0 then
     t=t & " and singlecustadsl.stockid='" & s3 & "' "
  end if
  if len(trim(s4))>0 then
     t=t & " and singlecustadsl.branch='" & s4 & "' "
  end if  
  if len(trim(s5))>0 then
     t=t & " and singlecustadsl.bussman='" & s5 & "' "
  end if    
  s6=document.all("search6").value
  if len(trim(s6))>0 then
     t=t & " and RTCOUNTY.CUTNC + singlecustadsl.TOWNSHIP2 + singlecustadsl.RADDR2 like '%" & s6 & "%' "
     s=S & "　裝機地址：含('" & s6 & "')字元"
  end if  
  s7=document.all("search7").value
  if len(trim(s7))>0 then
     t=t & " and singlecustadsl.housename like '%" & s7 & "%' "
     s=S & "　社區名稱：含('" & s7 & "')字元"
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
  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  winP.focus()
  window.close
End Sub
Sub SrReNew()
  Window.form.Submit
End Sub
-->
</script>
</head>
<body>
<form method="post" id="form">
<center>
<table width="80%">
  <tr class=dataListTitle align=center>請輸入(選擇)客戶資料搜尋條件</td><tr>
</table>
<table width="80%" border=1 cellPadding=0 cellSpacing=0>
        <td width="30%" class="dataListHead" height="32">證券/分行/營業員</td>                                      
        <td width="70%" height="32" bgcolor="silver">
<%  Call SrGetBRANCHBUSS(accessmode,"E",0,dspkey(3),dspkey(4),dspkey(5),s,t,U)%>        
           <select size="1" name="search3" onChange="SrRenew()" class="dataListEntry">      
             <option value="""" selected>(全部)</option>"                                        
              <%=s%>
           </select>
           <select size="1" name="search4"  onChange="SrRenew()" class="dataListEntry">      
            <option value="""" selected>(全部)</option>"                                        
              <%=T%>
           </select>
           <select size="1" name="search5"  class="dataListEntry">               
            <option value="""" selected>(全部)</option>"                               
              <%=U%>
           </select> 
        </td>     
<tr><td class=dataListHead width="30%">客戶名稱</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search1" size="25" maxlength="25" class=dataListEntry>
    </td></tr>
<tr><td class=dataListHead width="30%">裝機地址</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search6" size="40" maxlength="50" class=dataListEntry>
    </td></tr>    
<tr><td class=dataListHead width="30%">社區名稱</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search7" size="30" maxlength="60" class=dataListEntry>
    </td></tr>        
<tr><td class=dataListHead width="30%">元訊客服回覆狀況</td>
    <td width="70%" bgcolor="silver" >
      <select name="search2" size="1" class=dataListEntry>
        <option value=";全部"  selected >全部</option>
        <option value="NOT NULL;已收件">已回覆</option>
        <option value="NULL;未收件">未回覆</option>
      </select>    </td></tr>    
</table>
<table width="80%" align=right><tr><td></td><td align=right>
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</FORM>
</body>
</html>
<!-- #include file="rtgetBRANCHBUSS.inc" -->