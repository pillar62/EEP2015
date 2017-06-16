<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<meta http-equiv="Content-Type" content="text/html; charset=big5">
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"
		    codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<script language="VBScript">
<!--
Sub btn_onClick()
  '----社區名稱 -------------------------------------------------------------------
  r=document.all("search1").value  
  If Len(r)=0 Or r="" Then
     s=s &"  社區名稱:全部 "  
     t=t &"  (e.comn <> '*') "
  Else
     s=s &"  社區名稱:" &r & " "
     t=t &"  (e.comn LIKE '%" &r &"%') " 
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
  s3=document.all("search3").value
  s3ary=split(s3,";")
  if s3ary(0)="" then
     S=S & "  業務組別:全部  "
  else
     s=s & "  業務組別:" & s3ary(1) & "  "
     t=t & " and (RTSparqAdslCmty.GROUPID='" & s3ary(0) & "') "
  end if
  
  '方案別 --------------------------------------------------------------------
  s4=document.all("search4").value
  s4ary=split(s4,";")
  IF S4ARY(0) <> "" THEN
     s=s & "方案︰" & s4ary(1) & " "
     t=t & " and a.CASETYPE='" & s4ary(0) & "' "
  END IF
  
  s7=document.all("search7").value    
  if isdate(s7) then
     s=s & "申請日自︰" & s7 & " 以來，"
     t=t & " and rtsparqadslcust.formaldat >= '" & s7 & "' "
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
  <tr class=dataListTitle align=center>電費基本資料查詢搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>

<tr><td class=dataListHead width="40%">轄區</td>
    <td width="60%" bgcolor="silver">
 <% Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLIB"
    conn.open DSN
    sql="select isnull(p.codenc,'1') as comtypeid, isnull(p.codenc, r.areanc) as comtypenc "_
		&"from rtcmty o "_
		&"left outer join rtcode p on o.comtype = p.code and p.kind ='B3' and p.parm1 <>'AA' "_
		&"left outer join rtctytown q inner join rtarea r on q.areaid = r.areaid and r.areatype ='3' "_
		&"on q.cutid = o.cutid and q.township = o.township "_
		&"union "_
		&"select  isnull(p.codenc,'1') as comtypeid, isnull(p.codenc, r.areanc) as comtypenc from rtcustadslcmty o "_
		&"left outer join rtcode p on o.comtype = p.code and p.kind ='B3' and p.parm1 <>'AA' "_
		&"left outer join rtctytown q inner join rtarea r on q.areaid = r.areaid and r.areatype ='3' "_
		&"on q.cutid = o.cutid and q.township = o.township "_
		&"union "_
		&"select	isnull(p.shortnc, '1') as comtypeid, isnull(p.shortnc, r.areanc) as comtypenc from rtsparqadslcmty o "_
		&"left outer join rtobj p on o.consignee = p.cusid "_
		&"left outer join rtctytown q inner join rtarea r on q.areaid = r.areaid and r.areatype ='3' "_
		&"on q.cutid = o.cutid and q.township = o.township "_
		&"union "_
		&"select	isnull(p.shortnc, '1') as comtypeid, isnull(p.shortnc, r.areanc) as comtypenc from rtebtcmtyh o "_
		&"inner join rtebtcmtyline s on o.comq1 = s.comq1 "_
		&"inner join (select comq1, min(lineq1) as minlineq1 from rtebtcmtyline group by comq1) t on s.comq1 = t.comq1 and s.lineq1 = t.minlineq1 "_
		&"left outer join rtobj p on s.consignee = p.cusid "_
		&"left outer join rtctytown q inner join rtarea r on q.areaid = r.areaid and r.areatype ='3' "_
		&"on q.cutid = o.cutid and q.township = o.township "_
		&"order by 1,2 "
    
    s="<option value="";"" >(全部)</option>"   
    rs.Open sql,conn
    If rs.Eof Then s="<option value="";"" >(全部)</option>"
    sx=""
    Do While Not rs.Eof
		if len(rs(comtypenc).value) >0 then
			s=s &"<option value=""" &rs("comtypeid") & ";" & rs("comtypenc") &"""" &sx &">" &rs("GROUPnc") &"</option>"
		rs.MoveNext
		sx=""
    Loop
    rs.Close    
    conn.close       
    set rs=nothing
    set conn=nothing
 %>
      <select name="search3" size="1" class=dataListEntry>
        <%=s%>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry> 
    </td></tr>    
<tr><td class=dataListHead width="40%">線路數</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search10" class=dataListEntry> 
    </td></tr>   
<tr><td class=dataListHead width="40%">方案</td>
    <td width="60%" bgcolor="silver">    
  <%Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLIB"
    conn.open DSN
    sql="SELECT code,CODENC FROM RTCODE WHERE KIND='L5' order by CODE "
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
       <select name="search4" size="1" class=dataListEntry>
        <%=s%>
      </select>
    </td></tr>
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>