 <% Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLIB"
    conn.open DSN
'----------經銷商
    S6=""
    rs.Open "SELECT  CASE WHEN rtcode.parm1 = 'AA' THEN '直銷' ELSE RTCODE.CODENC END AS shortnc " _
           &"FROM  rtCUSTADSLcmty LEFT OUTER JOIN RTcode ON rtCUSTADSLcmty.comtype = RTcode.code AND rtcode.kind = 'B3' " _
           &"GROUP BY  CASE WHEN rtcode.parm1 = 'AA' THEN '直銷' ELSE RTCODE.CODENC END " _
           &"ORDER BY  CASE WHEN rtcode.parm1 = 'AA' THEN '直銷' ELSE RTCODE.CODENC END",CONN
    s6="<option value=""*"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s6=s6 &"<option value=""" & rs("SHORTNC") & """>" &rs("SHORTNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'----------營運點
    S3=""
    rs.Open "SELECT OPERATIONID, OPERATIONNAME FROM RTCtyTown WHERE (OPERATIONNAME <> '') GROUP BY  OPERATIONID, OPERATIONNAME ORDER BY  OPERATIONID ",CONN
    s3="<option value=""*"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s3=s3 &"<option value=""" & rs("OPERATIONNAME") & """>" &rs("OPERATIONNAME") &"</option>"
       rs.MoveNext
    Loop
    s3=s3 &"<option value=""無法歸屬"">無法歸屬</option>"
    rs.Close    
'----------社區類別
    S11=""
    rs.Open "SELECT CODE,CODENC FROM RTCODE WHERE KIND='B3' ",CONN
    s11="<option value=""<>'*';全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s11=s11 &"<option value=""='" &rs("CODE") & "';" & rs("CODENC") & """>" &rs("CODENC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close    
    conn.close       
    set rs=nothing
    set conn=nothing
 %>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV3/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"     codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<script language="VBScript">
<!--
Sub search3_OnChange()
    <%=s%>
    document.all("search6TD").innerHTML=arygroup(document.all("search3").selectedIndex)
End Sub
Sub btn_onClick()
  '----社區名稱
  r=document.all("search1").value  
  If Len(r)=0 Or r="" Then
     s=s &"  社區名稱:全部 "  
     t=t &"  (RTCUSTADSLcmty.ComN <> '*') "
  Else
     s=s &"  社區名稱:" &r & " "
     t=t &"  (RTCUSTADSLcmty.ComN LIKE '%" &r &"%') " 
  End If
  '----社區號碼
  r=document.all("search5").value  
  If Len(r)=0 Or r="" Then
     s=s &"  社區HB號碼:全部 "  
     t=t &" and (RTCUSTADSLcmty.HBNO <> '*') "
  Else
     s=s &"  社區HB號碼:" &r & " "
     t=t &" and (RTCUSTADSLcmty.hbno LIKE '%" &r &"%') " 
  End If
  '----設備地址
  r=document.all("search9").value  
  If Len(r)=0 Or r="" Then
     s=s &"  設備地址:全部 "  
  Else
     s=s &"  設備地址:" &r & " "
     t=t &" and (RTCUSTADSLcmty.EQUIPADDR LIKE '%" &trim(r) &"%') " 
  End If
  '----進度狀況
  arystr=split(document.all("search2").value,";")
  s=s &"  進度狀況:" & aryStr(1)
  
 '----社區類別
  arystr=split(document.all("search11").value,";")  
  s=s &"  社區類別: " & arystr(1)
  t=t & " and rtcustadslcmty.comtype" & arystr(0) 
  if aryStr(0)="" then
     t=t & " and (rtCUSTADSL.dropdat is null and rtcustadsl.agree <>'N' ) "
  elseif aryStr(0)="1" then
     t=t & ""     
  elseif aryStr(0)="2" then
     t=t & " and (rtCUSTADSLcmty.ADSLapply is not null ) "
  elseif aryStr(0)="3" then
     t=t & " and (rtCUSTADSLcmty.LINEARRIVE is NOT null) AND (RTCUSTADSLCMTY.ADSLAPPLY IS NULL) "
  elseif aryStr(0)="4" then
     t=t & " and (rtCUSTADSLcmty.SNDWRKPLACE is NOT null) AND (RTCUSTADSLCMTY.ADSLAPPLY IS NULL) " _
         & " AND (rtCUSTADSLcmty.LINEARRIVE is null)  "     
  elseif aryStr(0)="5" then
     t=t & " and (rtCUSTADSLcmty.EQUIPARRIVE is NOT null) AND (RTCUSTADSLCMTY.ADSLAPPLY IS NULL) " _
         & " AND (rtCUSTADSLcmty.LINEARRIVE is null) AND (RTCUSTADSLCMTY.SNDWRKPLACE IS NULL) "     
  elseif aryStr(0)="6" then
     t=t & " and (rtCUSTADSLcmty.CASESNDWRK is NOT null) AND (RTCUSTADSLCMTY.ADSLAPPLY IS NULL) " _
         & " AND (rtCUSTADSLcmty.LINEARRIVE is null) AND (RTCUSTADSLCMTY.SNDWRKPLACE IS NULL) " _
         & " AND (RTCUSTADSLCMTY.EQUIPARRIVE IS NULL ) "
  elseif aryStr(0)="7" then
     t=t & " and (rtCUSTADSLcmty.rcvd is NOT null) AND (RTCUSTADSLCMTY.ADSLAPPLY IS NULL) " _
         & " AND (rtCUSTADSLcmty.LINEARRIVE is null) AND (RTCUSTADSLCMTY.SNDWRKPLACE IS NULL) " _
         & " AND (RTCUSTADSLCMTY.EQUIPARRIVE IS NULL ) AND (RTCUSTADSLCMTY.CASESNDWRK IS NULL) " 
  elseif aryStr(0)="8" then
     t=t & " and (rtCUSTADSLcmty.SURVYDAT is NOT null) AND (RTCUSTADSLCMTY.ADSLAPPLY IS NULL) " _
         & " AND (rtCUSTADSLcmty.LINEARRIVE is null) AND (RTCUSTADSLCMTY.SNDWRKPLACE IS NULL) " _
         & " AND (RTCUSTADSLCMTY.EQUIPARRIVE IS NULL ) AND (RTCUSTADSLCMTY.CASESNDWRK IS NULL) " _
         & " AND (RTCUSTADSLCMTY.RCVD IS NULL ) "        
  end if
  s3=document.all("search3").value
  if S3 <> "*" and s3<>"無法歸屬" then
     t=t &" AND (RTCTYTOWN.OPERATIONNAME='" & S3 & "') AND rtcode.parm1='AA' "
  elseif s3="無法歸屬" then
     t=t &" AND (RTCTYTOWN.OPERATIONNAME='') and rtcode.parm1='AA' "
  end if
  s4=document.all("search4").value
  s4ary=split(s4,";")
  t1=""
  if s4ary(0)="" then
     S=S & "  異常資料:不篩選  "
  else
     s=s & "  異常資料:" & s4ary(1) & "  "
     t1=t1 & "  having (SUM(CASE WHEN rtcustadsl.cusid IS NOT NULL OR rtcustadsl.cusid <> '' THEN 1 ELSE 0 END)- " _
         &"SUM(CASE WHEN rtcustadsl.docketdat IS NOT NULL OR rtcustadsl.docketdat <> '' THEN 1 ELSE 0 END)- " _
         &"SUM(CASE WHEN rtcustadsl.dropdat   IS NOT NULL OR rtcustadsl.dropdat <> ''   THEN 1 ELSE 0 END)) > 0 " 

  end if  
  s6=document.all("search6").value
  s=S & "經銷商:" &S6 &"  "
  if S6 <> "*" AND S6 <> "直銷" then
     t=t &" AND (RTCODE.CODENC='" & S6 & "') "
  ELSEIF S6="直銷" THEN 
     t=t &" AND (RTCODE.PARM1='AA') "
  end if
  
  s7=document.all("search7").value    
  if isdate(s7) then
     s=s & "申請日自︰" & s7 & " 以來，"
     t=t & " and rtcustadsl.rcvd >= '" & s7 & "' "
  end if
  s8=document.all("search8").value  
  iF Isnumeric(S8) then
     s=s & "裝機時間超過︰" & s8 & " 天 "
     t=t & " and (( datediff(dd,rtcustadsl.rcvd,rtcustadsl.finishdat) > " & S8 & " and rtcustadsl.rcvd is not null) or ( datediff(dd,rtcustadsl.rcvd,getdate()) > " & S8 & " and rtcustadsl.finishdat is null)) "
  end if  
  s10=document.all("search10").value  
  if len(trim(s10)) <> "" then
     s=s & "  附掛電話包含︰" & s10 &  "  "
     t=t & " and (rtcustadslcmty.cmtytel like '%" & s10 & "%') "
  end if
  s12=document.all("search12").value  
  if len(trim(s12)) <> "" then
     s=s & "  社區線路IP含︰(" & s12 &  ")  "
     t=t & " and (rtcustadslcmty.IPADDR like '%" & s12 & "%') "
  end if  
  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchQry2").value=t1
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
      <select name="search3" size="1" class=dataListEntry ID="Select1">
        <%=S3%>
    </select>      
    </td></tr>        
<tr><td class=dataListHead width="40%">經銷商</td>
    <td width="60%"  bgcolor="silver">
    <select name="search6" size="1" class=dataListEntry ID="Select1">
        <%=S6%>
    </select>      
    </td>
</tr>    
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry> 
    </td></tr> 
<tr><td class=dataListHead width="40%">社區HB號碼</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search5" class=dataListEntry> 
    </td></tr> 
<tr><td class=dataListHead width="40%">社區線路IP</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search12" class=dataListEntry ID="Text2"> 
    </td></tr>     
<tr><td class=dataListHead width="40%">社區類別</td>
    <td width="60%" bgcolor="silver">
      <select name="search11" size="1" class=dataListEntry>
        <%=s11%>
      </select>
    </td></tr>     
<tr><td class=dataListHead width="40%">附掛電話號碼</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search10" class=dataListEntry ID="Text1"> 
    </td></tr>     
<tr><td class=dataListHead width="40%">設備地址</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="40" name="search9" class=dataListEntry> 
    </td></tr> 
<tr><td class=dataListHead width="40%">異常資料</td>
    <td width="60%" bgcolor="silver">
      <select name="search4" size="1" class=dataListEntry>
        <option value=";不篩選">不篩選</option>
        <option value="1;只挑選有異常客戶之社區">只挑選有異常客戶之社區</option>
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
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>