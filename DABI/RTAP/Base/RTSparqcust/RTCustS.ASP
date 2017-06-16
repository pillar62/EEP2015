<%
 Dim dspkey(100),DSN
 DSN="DSN=RTLib"
 dspkey(3)=Request.Form("search3")
 dspkey(4)=Request.Form("search4")
 dspkey(5)=Request.Form("search5")
 
Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLIB"
    conn.open DSN
'----------經銷商
    S15=""
    rs.Open "SELECT  CASE WHEN RTSparqAdslCmty.CONSIGNEE = '' THEN '直銷' ELSE RTOBJ.SHORTNC END AS shortnc " _
           &"FROM  RTSparqAdslCmty LEFT OUTER JOIN RTOBJ ON RTSparqAdslCmty.CONSIGNEE= RTOBJ.CUSID " _
           &"GROUP BY  CASE WHEN RTSparqAdslCmty.CONSIGNEE = '' THEN '直銷' ELSE RTOBJ.SHORTNC END " _
           &"ORDER BY  CASE WHEN RTSparqAdslCmty.CONSIGNEE = '' THEN '直銷' ELSE RTOBJ.SHORTNC END",CONN
    s15="<option value=""*"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s15=s15 &"<option value=""" & rs("SHORTNC") & """>" &rs("SHORTNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'----------營運點
    S14=""
    rs.Open "SELECT OPERATIONID, OPERATIONNAME FROM RTCtyTown WHERE (OPERATIONNAME <> '') GROUP BY  OPERATIONID, OPERATIONNAME ORDER BY  OPERATIONID ",CONN
    s14="<option value=""*"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s14=s14 &"<option value=""" & rs("OPERATIONNAME") & """>" &rs("OPERATIONNAME") &"</option>"
       rs.MoveNext
    Loop
    s14=s14 &"<option value=""無法歸屬"">無法歸屬</option>"
    rs.Close            
'--------- 廠商 
    S5=""
    rs.Open "SELECT RTObj.CUSID AS CusID, RTObj.SHORTNC AS SHORTNC " _
           &"FROM RTObj INNER JOIN RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
           &"WHERE (((RTObjLink.CUSTYID)='02')) " _
           &"ORDER BY RTObj.SHORTNC ",conn
    s5="<option value=""<>'*';：全部"" selected>全部</option>" &vbCrLf   
    Do While not rs.eof
       s5= s5 & "<option value=""='" & rs("cusid") & "';" & "：" & trim(rs("shortnc")) & """>" & trim(rs("shortNC")) & "</option>" & vbcrlf    
    rs.MoveNext
    Loop
    rs.Close    
'--------- 其它方案移轉 
    S12=""
    rs.Open "SELECT CODE,CODENC FROM RTCODE WHERE KIND='H8'",CONN
    s12="<option value="";全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s12=s12 &"<option value=""" &rs("CODE") & ";" & rs("CODENC") & """>" &rs("CODENC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close    
    conn.close       
    set rs=nothing
    set conn=nothing

%>
<html>
<head>
<meta http-equiv="content-type" content="text/html; charset=big5">
<link REL="stylesheet" HREF="/WebUtilityV3/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub search10_OnChange()
    <%=s11%>
    document.all("search11TD").innerHTML=arygroup(document.all("search10").selectedIndex)
End Sub
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
  '((全部==不含已退租、撤銷或不可建置戶))
  if s2ary(0)="" then 
     t=t & " and rtsparqadslcust.dropdat is null and rtsparqadslcust.agree <>'N' " 
  '((全部))
  elseif s2ary(0)="99" then
     t=t & " and rtsparqadslcust.CUSID IS NOT NULL "
  '((已完工尚未報峻))  
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
  elseif s2ary(0)="11" then
     t=t & " and ( rtsparqadslcust.agree = '' or rtsparqadslcust.agree is null ) "     
  end if
  
  '-------------
  s6=document.all("search6").value
  if len(trim(s6))>0 then
     t=t & " and RTCOUNTY.CUTNC + rtsparqadslcust.TOWNSHIP2 + rtsparqadslcust.RADDR2 like '%" & s6 & "%' "
     s=S & "　裝機地址：含('" & s6 & "')字元"
  end if  
  s7=document.all("search7").value
  if len(trim(s7))>0 then
     t=t & " and RTSparqADSLcmty.comn like '%" & s7 & "%' "
     s=S & "　社區名稱：含('" & s7 & "')字元"
  end if    
  s8=document.all("search8").value
  if len(trim(s8))>0 then
     t=t & " and rtsparqadslcust.ORDERNO like '%" & s8 & "%' "
     s=S & "　收件編號：含('" & s8 & "')字元"
  end if    
  s9=document.all("search9").value
  if len(trim(s9))>0 then
     t=t & " and rtsparqadslcust.EXTTEL + rtsparqadslcust.SPHNNO like '%" & s9 & "%' "
     s=S & "　速博對帳號碼：含('" & s9 & "')字元"
  end if    
  
  s12ary=split(document.all("search12").value,";")  
  IF S12ARY(0) <> "" THEN
     S=S & " 其它方案移轉︰('" & S12ARY(1) & "') "
     T=T & " AND rtsparqadslcust.TNSCUSTCASE='" & S12ARY(0) & "' "
  END IF  
    s14=document.all("search14").value
  if S14 <> "*" and s14<>"無法歸屬" then
     t=t &" AND (RTCTYTOWN.OPERATIONNAME='" & S14 & "') AND RTSparqAdslCmty.consignee='' "
  elseif s14="無法歸屬" then
     t=t &" AND (RTCTYTOWN.OPERATIONNAME='') and RTSparqAdslCmty.consignee='' "
  end if
  s15=document.all("search15").value
  s=S & "經銷商:" &S15 &"  "
  if S15 <> "*" AND S15 <> "直銷" then
     t=t &" AND (rtobj_a.shortnc='" & S15 & "') "
  ELSEIF S15="直銷" THEN 
     t=t &" AND (RTSparqAdslCmty.consignee='') "
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
<tr><td class=dataListHead width="40%">營運點</td>
    <td width="60%" bgcolor="silver">
      <select name="search14" size="1" class=dataListEntry ID="Select1">
        <%=S14%>
    </select>      
    </td></tr>        
<tr><td class=dataListHead width="40%">經銷商</td>
    <td width="60%"  bgcolor="silver">
    <select name="search15" size="1" class=dataListEntry ID="Select1">
        <%=S15%>
    </select>      
    </td>
</tr>    
   
<tr><td class=dataListHead width="30%">客戶名稱</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search1" size="25" maxlength="25" class=dataListEntry>
    </td></tr>
<tr><td class=dataListHead width="30%">收件編號</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search8" size="7" maxlength="25" class=dataListEntry>
    </td></tr>    
<tr><td class=dataListHead width="30%">速博對帳號碼</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search9" size="10" maxlength="10" class=dataListEntry>
    </td></tr>    
<tr><td class=dataListHead width="30%">裝機地址</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search6" size="40" maxlength="50" class=dataListEntry>
    </td></tr>    
<tr><td class=dataListHead width="30%">社區名稱</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search7" size="30" maxlength="60" class=dataListEntry>
    </td></tr>        
<tr><td class=dataListHead width="30%">安裝進度狀況</td>
    <td width="70%" bgcolor="silver" >
      <select name="search2" size="1" class=dataListEntry>
        <option value="99;全部" selected>全部</option>
        <option value=";全部(不含撤銷戶、退租戶及不可建置戶)">(1)全部(不含撤銷戶、退租戶及不可建置戶)</option>
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
        <option value="11;尚未評估戶">(12)尚未評估戶</option>                                                                        
      </select>    </td></tr>    
<tr><td class=dataListHead width="40%">其它方案移轉</td>
    <td width="60%" bgcolor="silver">
      <select name="search12" size="1" class=dataListEntry ID="Select1">
        <%=s12%>
      </select>
    </td></tr>      
</table>
<table width="80%" align=right><tr><td></td><td align=right>
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</FORM>
</body>
</html>
