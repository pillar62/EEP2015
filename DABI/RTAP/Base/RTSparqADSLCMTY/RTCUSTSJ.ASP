<%
 Dim dspkey(100),DSN
 DSN="DSN=RTLib"
 dspkey(3)=Request.Form("search3")
 dspkey(4)=Request.Form("search4")
 dspkey(5)=Request.Form("search5")
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim s,t
  t=""
  s=""
  '--------------
  s1=document.all("search1").value
  if len(trim(s1)) > 0 then
     s="客戶名稱：含('" & s1 & "')字元"  
     t=t & " rtobj.cusnc like '%" & s1 & "%' "
  else
     s="客戶名稱：全部  "
     t=t & " rtsparqadslcust.cusid <> '*' "
  end if
  '-----安裝進度
  s2=document.all("search2").value
  s2ary=split(s2,";")
  
  s=S & "安裝進度狀況：" & s2ary(1)  
  if s2ary(0)="" then 
     t=t & " and rtsparqadslcust.dropdat is null and rtsparqadslcust.agree <>'N' " 
  '((己報峻))
  elseif s2ary(0)="1" then
     t=t & " and rtsparqadslcust.docketdat is not null "
  '((已完工尚未報峻))
  elseif s2ary(0)="2" then
     t=t & " and rtsparqadslcust.finishdat is not null and rtsparqadslcust.docketdat is null " 
  '((已取得附掛電話))--尚未裝機
  elseif s2ary(0)="3" then
     t=t & " and rtsparqadslcust.workingreply is not null and rtsparqadslcust.finishdat is null and rtsparqadslcust.docketdat is null " 
  '((已送至營運處等待附掛電話))
  elseif s2ary(0)="4" then
     t=t & " and rtsparqadslcust.sendworking is not null and rtsparqadslcust.workingreply is null and rtsparqadslcust.docketdat is null and rtsparqadslcust.finishdat is null " 
  '((中華電信已簽回尚未送營運處))
  elseif s2ary(0)="5" then
     t=t & " and rtsparqadslcust.chtsigndat is not null and rtsparqadslcust.sendworking is null and rtsparqadslcust.workingreply is null and rtsparqadslcust.docketdat is null and rtsparqadslcust.finishdat is null " 
  '((已送件中華電信尚未簽回))
  elseif s2ary(0)="6" then
     t=t & " and rtsparqadslcust.deliverdat is not null and rtsparqadslcust.chtsigndat is null and  rtsparqadslcust.sendworking is null and rtsparqadslcust.workingreply is null and rtsparqadslcust.docketdat is null and rtsparqadslcust.finishdat is null " 
  '((尚未送件))
  elseif s2ary(0)="7" then
     t=t & " and rtsparqadslcust.deliverdat is null and rtsparqadslcust.dropdat is null and rtsparqadslcust.chtsigndat is null and  rtsparqadslcust.sendworking is null and rtsparqadslcust.workingreply is null and rtsparqadslcust.docketdat is null and rtsparqadslcust.finishdat is null " 
  '((不可建置戶))
  elseif s2ary(0)="8" then
     t=t & " and rtsparqadslcust.agree ='N' "
  '((撤銷申請戶))
  elseif s2ary(0)="9" then
     t=t & " and rtsparqadslcust.dropdat is not null and rtsparqadslcust.finishdat is null "
  '((退租申請戶))
  elseif s2ary(0)="10" then
     t=t & " and rtsparqadslcust.dropdat is not null and rtsparqadslcust.finishdat is not null "
  end if
  
  '-------------
  s3=document.all("search3").value
  s4=document.all("search4").value
  s5=document.all("search5").value
  if len(trim(s3))>0 then
     t=t & " and rtsparqadslcust.stockid='" & s3 & "' "
  end if
  if len(trim(s4))>0 then
     t=t & " and rtsparqadslcust.branch='" & s4 & "' "
  end if  
  if len(trim(s5))>0 then
     t=t & " and rtsparqadslcust.bussman='" & s5 & "' "
  end if    
  s6=document.all("search6").value
  if len(trim(s6))>0 then
     t=t & " and RTCOUNTY.CUTNC + rtsparqadslcust.TOWNSHIP2 + rtsparqadslcust.RADDR2 like '%" & s6 & "%' "
     s=S & "　裝機地址：含('" & s6 & "')字元"
  end if  
  s7=document.all("search7").value
  if len(trim(s7))>0 then
     t=t & " and rtsparqadslcust.housename like '%" & s7 & "%' "
     s=S & "　社區名稱：含('" & s7 & "')字元"
  end if    
  s8=document.all("search8").value
  if len(trim(s8))>0 then
     t=t & " and rtsparqadslcust.ORDERNO like '%" & s8 & "%' "
     s=S & "　收件編號：含('" & s8 & "')字元"
  end if    
  s9=document.all("search9").value
  s9ary=split(s9,";")
  if s9ary(0)="" then
     s=S & "　客戶連結狀態：" & s9ary(1)
  else
     t=t & " and rtsparqadslcust.COMQ1=0 "
     s=S & "　客戶連結狀態：" & s9ary(1)
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
<tr><td class=dataListHead width="30%">收件編號</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search8" size="7" maxlength="25" class=dataListEntry>
    </td></tr>    
<tr><td class=dataListHead width="30%">裝機地址</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search6" size="40" maxlength="50" class=dataListEntry>
    </td></tr>    
<tr><td class=dataListHead width="30%">社區名稱</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search7" size="30" maxlength="60" class=dataListEntry>
    </td></tr>        
<tr><td class=dataListHead width="30%">客戶連結狀態</td>
    <td width="70%" bgcolor="silver" >
    <td width="70%" bgcolor="silver" >
      <select name="search9" size="1" class=dataListEntry>
        <option value=";全部(不含撤銷戶、退租戶及不可建置戶)"  selected >(1)全部(不含撤銷戶、退租戶及不可建置戶)</option>
        <option value="1;已報峻客戶">(2)尚未與社區檔連結客戶</option>
      </select>   
    </td></tr>       
<tr><td class=dataListHead width="30%">安裝進度狀況</td>
    <td width="70%" bgcolor="silver" >
      <select name="search2" size="1" class=dataListEntry>
        <option value=";全部(不含撤銷戶、退租戶及不可建置戶)"  selected >(1)全部(不含撤銷戶、退租戶及不可建置戶)</option>
        <option value="1;已報峻客戶">(2)已報峻客戶</option>
        <option value="2;已裝機完工客戶(未報峻)">(3)已裝機完工客戶(未報峻)</option>
        <option value="3;已取得附掛電話客戶(未裝機)">(4)已取得附掛電話客戶(未裝機)</option>        
        <option value="4;已送件至營運處(等待附掛電話)">(5)已送件至營運處(等待附掛電話)</option>
        <option value="5;中華電信已簽回(尚未送營運處)">(6)中華電信已簽回(尚未送營運處)</option>
        <option value="6;已送件至中華電信(尚未簽回)">(7)已送件至中華電信(尚未簽回)</option>
        <option value="7;尚未送件">(8)尚未送件</option>             
        <option value="8;不可建置戶">(9)不可建置戶</option>             
        <option value="9;撤銷申請戶">(10)撤銷申請戶</option>                                                        
        <option value="10;退租戶">(11)退租戶</option>                                                                
      </select>    </td></tr>    
</table>
<table width="80%" align=right><tr><td></td><td align=right>
  <input type="button" value=" 查詢 " class=dataListButton name="btn" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</FORM>
</body>
</html>
