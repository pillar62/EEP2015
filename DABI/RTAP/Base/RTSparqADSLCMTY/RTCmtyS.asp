<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt, search12pt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    
    Set rs=Server.CreateObject("ADODB.Recordset")
'----------經銷商
    s5="<option value=""*"" selected>全部</option>" &vbCrLf    
    sql="select a.consignee, b.shortnc from	rtsparqadslcmty a inner join RTObj b on a.consignee = b.cusid "_
	   &"group by a.consignee, b.shortnc order by b.shortnc "
    rs.Open sql,conn
    Do While Not rs.Eof
       s5=s5 &"<option value=""" & rs("consignee") & """>" &rs("shortnc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close    
'----------直銷員工
    rs.Open "select bussid, c.cusnc from RTSparqAdslCmty a " &_
			"inner join RTEmployee b on a.bussid = b.emply " &_
			"inner join RTObj c on c.cusid = b.cusid " &_
			"group by bussid, c.cusnc order by bussid",CONN
    s15="<option value=""*"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s15=s15 &"<option value=""" & rs("bussid") & """>" &rs("cusnc") &"</option>"
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
    
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<link REL="stylesheet" HREF="/WebUtilityV3/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"      codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<script language="VBScript">
<!--
Sub btn_onClick()
  '----社區名稱
  r=document.all("search1").value  
  If Len(r)=0 Or r="" Then
     s=s &"  社區名稱:全部 "  
     t=t &"  (RTSparqAdslCmty.ComN <> '*') "
  Else
     s=s &"  社區名稱:" &r & " "
     t=t &"  (RTSparqAdslCmty.ComN LIKE '%" &r &"%') " 
  End If
  '----進度狀況
  arystr=split(document.all("search2").value,";")
  s=s &"  進度狀況:" & aryStr(1)
  if aryStr(0)="" then
     t=t & " and (rtsparqadslcust.dropdat is null and rtsparqadslcust.agree <>'N' ) "
  elseif aryStr(0)="1" then
     t=t & ""     
  elseif aryStr(0)="2" then
     t=t & " and (RTSparqAdslCmty.ADSLapply is not null ) "
  elseif aryStr(0)="3" then
     t=t & " and (RTSparqAdslCmty.LINEARRIVE is NOT null) AND (RTSparqAdslCmty.ADSLAPPLY IS NULL) "
  elseif aryStr(0)="4" then
     t=t & " and (RTSparqAdslCmty.SNDWRKPLACE is NOT null) AND (RTSparqAdslCmty.ADSLAPPLY IS NULL) " _
         & " AND (RTSparqAdslCmty.LINEARRIVE is null)  "     
  elseif aryStr(0)="5" then
     t=t & " and (RTSparqAdslCmty.EQUIPARRIVE is NOT null) AND (RTSparqAdslCmty.ADSLAPPLY IS NULL) " _
         & " AND (RTSparqAdslCmty.LINEARRIVE is null) AND (RTSparqAdslCmty.SNDWRKPLACE IS NULL) "     
  elseif aryStr(0)="6" then
     t=t & " and (RTSparqAdslCmty.CASESNDWRK is NOT null) AND (RTSparqAdslCmty.ADSLAPPLY IS NULL) " _
         & " AND (RTSparqAdslCmty.LINEARRIVE is null) AND (RTSparqAdslCmty.SNDWRKPLACE IS NULL) " _
         & " AND (RTSparqAdslCmty.EQUIPARRIVE IS NULL ) "
  elseif aryStr(0)="7" then
     t=t & " and (RTSparqAdslCmty.rcvd is NOT null) AND (RTSparqAdslCmty.ADSLAPPLY IS NULL) " _
         & " AND (RTSparqAdslCmty.LINEARRIVE is null) AND (RTSparqAdslCmty.SNDWRKPLACE IS NULL) " _
         & " AND (RTSparqAdslCmty.EQUIPARRIVE IS NULL ) AND (RTSparqAdslCmty.CASESNDWRK IS NULL) " 
  elseif aryStr(0)="8" then
     t=t & " and (RTSparqAdslCmty.SURVYDAT is NOT null) AND (RTSparqAdslCmty.ADSLAPPLY IS NULL) " _
         & " AND (RTSparqAdslCmty.LINEARRIVE is null) AND (RTSparqAdslCmty.SNDWRKPLACE IS NULL) " _
         & " AND (RTSparqAdslCmty.EQUIPARRIVE IS NULL ) AND (RTSparqAdslCmty.CASESNDWRK IS NULL) " _
         & " AND (RTSparqAdslCmty.RCVD IS NULL ) "        
  end if
 
  s4=document.all("search4").value
  s4ary=split(s4,";")
  IF S4ARY(0) <> "" THEN
     s=s & "方案︰" & s4ary(1) & " "
     t=t & " and rtsparqadslcmty.connecttype='" & s4ary(0) & "' "
  END IF
  
  s5=document.all("search5").value
  if len(trim(s5)) <> "" and s5 <> "*" then
     s=s & "經銷商︰" & S5 & " "
     t=t & " and rtsparqadslcmty.consignee='" & S5 & "' "
  END IF

  s15=document.all("search15").value
  if len(trim(s15)) <> "" and s15 <> "*" then
	s=S & "直銷員工:" &S15 &"  "
	t=t &" AND rtsparqadslcmty.bussid='" & S15 & "' "
  end if  

  s7=document.all("search7").value    
  if isdate(s7) then
     s=s & "申請日自︰" & s7 & " 以來，"
     t=t & " and rtsparqadslcust.formaldat >= '" & s7 & "' "
  end if
  s8=document.all("search8").value  
  iF Isnumeric(S8) then
     s=s & "裝機時間超過︰" & s8 & " 天 "
     t=t & " and (( datediff(dd,rtsparqadslcust..formaldat,rtsparqadslcust.finishdat) > " & S8 & " and rtsparqadslcust..formaldat is not null) or ( datediff(dd,rtsparqadslcust..formaldat,getdate()) > " & S8 & " and rtsparqadslcust.finishdat is null)) "
  end if    
    '----設備地址
  r=document.all("search9").value  
  If Len(r)=0 Or r="" Then
     s=s &"  設備地址:全部 "  
  Else
     s=s &"  設備地址:" &r & " "
     t=t &" and (IsNull(RTCounty.CUTNC,'')+RTSparqAdslCmty.TOWNSHIP+RTSparqAdslCmty.ADDR LIKE '%" &trim(r) &"%') " 
  End If
  s10=document.all("search10").value  
  if len(trim(s10)) <> "" then
     s=s & "  附掛電話包含︰" & s10 &  "  "
     t=t & " and (rtsparqadslcmty.cmtytel like '%" & s10 & "%') "
  end if
  s11=document.all("search11").value  
  if len(trim(s11)) <> "" then
     s=s & "  社區主線IP含︰(" & s11 &  ")  "
     t=t & " and (rtsparqadslcmty.IPADDR like '%" & s11 & "%') "
  end if  
  s14=document.all("search14").value
  if S14 <> "*" and s14<>"無法歸屬" then
     t=t &" AND (RTCTYTOWN.OPERATIONNAME='" & S14 & "') AND RTSparqAdslCmty.consignee='' "
  elseif s14="無法歸屬" then
     t=t &" AND (RTCTYTOWN.OPERATIONNAME='') and RTSparqAdslCmty.consignee='' "
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
  Dim winP
  Set winP=window.Opener
  winP.focus()
  window.close  
End Sub
Sub Srbtnonclick()
    Dim ClickID
    ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
    clickkey="SEARCH" & clickid
    if isdate(document.all(clickkey).value) then
	   objEF2KDT.varDefaultDateTime=document.all(clickkey).value
    end if
    call objEF2KDT.show(1)
    if objEF2KDT.strDateTime <> "" then
       document.all(clickkey).value = objEF2KDT.strDateTime
    end if
END SUB
Sub SrClear()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="SEARCH" & clickid       
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
          '當處理人員及處理廠商皆為空白時，才可清除此欄位資料
       end if
End Sub    
   
Sub ImageIconOver()
       self.event.srcElement.style.borderBottom = "black 1px solid"
       self.event.srcElement.style.borderLeft="white 1px solid"
       self.event.srcElement.style.borderRight="black 1px solid"
       self.event.srcElement.style.borderTop="white 1px solid"   
End Sub
   
Sub ImageIconOut()
       self.event.srcElement.style.borderBottom = ""
       self.event.srcElement.style.borderLeft=""
       self.event.srcElement.style.borderRight=""
       self.event.srcElement.style.borderTop=""
End Sub          
-->
</script>
</head>
<body>
<table width="100%">
  <tr class=dataListTitle align=center>ADSL社區基本資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>

<tr><td class=dataListHead width="40%">營運點</td>
    <td width="60%" bgcolor="silver">
      <select name="search14" size="1" class=dataListEntry ID="Select1">
        <%=S14%>
    </select>      
    </td></tr>        

<tr><td class=dataListHead width="40%">直銷業務</td>
    <td width="60%"  bgcolor="silver">
    <select name="search15" size="1" class=dataListEntry ID="Select1">
        <%=S15%>
    </select>      
    </td>
</tr>

<tr><td class=dataListHead width="40%">經銷商</td>
    <td width="60%" bgcolor="silver">    
       <select name="search5" size="1" class=dataListEntry ID="Select2">
        <%=s5%>
      </select>
    </td>
</tr>

<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry> 
    </td></tr>    
<tr><td class=dataListHead width="40%">附掛電話號碼</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search10" class=dataListEntry ID="Text1"> 
    </td></tr>   
<tr><td class=dataListHead width="40%">社區主線IP</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search11" class=dataListEntry ID="Text3"> 
    </td></tr>       
<tr><td class=dataListHead width="40%">設備地址</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="40" name="search9" class=dataListEntry ID="Text2"> 
    </td></tr>         
<tr><td class=dataListHead width="40%">方案</td>
    <td width="60%" bgcolor="silver">    
     <% Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLIB"
    conn.open DSN
    sql="SELECT code,CODENC FROM RTCODE WHERE KIND='G5' order by CODE "
    s="<option value="";"" >(全部)</option>"   
    rs.Open sql,conn
    If rs.Eof Then s="<option value="";"" >(全部)</option>"
    sx=""
    Do While Not rs.Eof
       s=s &"<option value=""" &rs("CODE") & ";" & rs("CODENC") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close    
    conn.close       
    set rs=nothing
    set conn=nothing
 %>
       <select name="search4" size="1" class=dataListEntry ID="Select1">
        <%=s%>
      </select>
    </td></tr>

<tr><td class=dataListHead width="40%">進度狀況</td>
    <td width="60%"  bgcolor="silver">
      <select name="search2" size="1" class=dataListEntry>
   <!--
        <option value=";全部(不含撤銷、退租、不可建置戶)" selected>全部(不含撤銷、退租、不可建置戶)</option>
        -->
        <option value="1;全部" selected>全部</option>
        <option value="2;已測通">已測通</option>
        <option value="3;線路已到位">線路已到位</option>
        <option value="4;已送至營運處">已送至營運處</option>
        <option value="5;設備已到位">設備已到位</option>
        <option value="6;機櫃已派工">機櫃已派工</option>
        <option value="7;已提出申請">已提出申請</option>
        <option value="8;社區已堪察">社區已堪察</option>
      </select>
     </td>
</tr>
<tr><td class=dataListHead width="40%">申請日自</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search7" size="10" maxlength="60" class=dataListdata readonly>
    <input type="button" id="B7"  name="B7" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C7"  name="C7"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
    </td></tr>            
<tr><td class=dataListHead width="40%">裝機時間超過︰</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search8" size="3" maxlength="60" class=dataListEntry>
    天</td></tr>        
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>