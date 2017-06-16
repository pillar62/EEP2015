
<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt, search12pt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    
    Set rs=Server.CreateObject("ADODB.Recordset")
'----------經銷商
    S11=""
    rs.Open "SELECT  CASE WHEN rtcode.parm1 = 'AA' THEN '直銷' ELSE rtcode.CODENC END AS shortnc " _
           &"FROM  rtcmty LEFT OUTER JOIN RTcode ON rtcmty.comtype = RTcode.code AND rtcode.kind = 'B3' " _
           &"GROUP BY  CASE WHEN rtcode.parm1 = 'AA' THEN '直銷' ELSE RTCODE.CODENC END " _
           &"ORDER BY  CASE WHEN rtcode.parm1 = 'AA' THEN '直銷' ELSE RTCODE.CODENC END",CONN
    s11="<option value=""*"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s11=s11 &"<option value=""" & rs("SHORTNC") & """>" &rs("SHORTNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'----------營運點
    S10=""
    rs.Open "SELECT OPERATIONID, OPERATIONNAME FROM RTCtyTown WHERE (OPERATIONNAME <> '') GROUP BY  OPERATIONID, OPERATIONNAME ORDER BY  OPERATIONID ",CONN
    s10="<option value=""*"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s10=s10 &"<option value=""" & rs("OPERATIONNAME") & """>" &rs("OPERATIONNAME") &"</option>"
       rs.MoveNext
    Loop
    s10=s10 &"<option value=""無法歸屬"">無法歸屬</option>"
    rs.Close    
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"    codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<script language="VBScript">
<!--
Sub btn_onClick()
  dim s,t
  t=""
  s=""
  s1=document.all("search1").value
  if len(trim(s1)) > 0 then
     s="社區名稱：含('" & s1 & "')字元"  
     t=t & " rtcmty.comn like '%" & s1 & "%' "
  else
     s="社區名稱：全部  "
     t=t & " rtcust.comq1 <> 0 "
  end if
  s2=document.all("search2").value
  if len(trim(s2)) > 0 then
     s="  客戶名稱：含('" & s2 & "')字元"  
     t=t & " and rtobj.cusnc like '%" & s2 & "%'"
  else
     s=s & "  客戶名稱：全部  "
     t=t & " and rtcust.cusid <>'*'"
  end if  
  s3=document.all("search3").value
  if len(trim(s3)) > 0 then
     s="  HN聯單號碼：含('" & s3 & "')字元"  
     t=t & " and rtCUST.cusno like '%" & s3 & "%'"
  else
     s=s & "  HN聯單號碼：全部  "
     t=t & " and rtcust.cusNO <>'*'"
  end if  
  s8=document.all("search8").value
  if len(trim(s8)) > 0 then
     s="  HN聯單號碼：含('" & s8 & "')字元"  
     t=t & " and rtCUST.idnumber like '%" & s8 & "%'"
  else
  end if  
  s9=document.all("search9").value
  if len(trim(s9)) > 0 then
     s="  IP位址：含('" & s9 & "')字元"  
     t=t & " and rtCUST.IP like '%" & s9 & "%'"
  else
  end if  
  s4=document.all("search4").value
  if len(trim(s4)) > 0 then
     s=S & "  裝機地址：含('" & s4 & "')字元"  
     t=t & " and (rtcounty.cutnc + rtcust.township1 + rtcust.raddr1 )  like '%" & s4 & "%'"
  end if      
  s5=document.all("search5").value
  s5ary=split(s5,";")
  if s5ary(0) = "01" then
     s=s & " 客戶連結型態：" & s5ary(1)
     t=t & " and rtcust.usekind ='單機型' "
  elseif s5ary(0) = "02" then
     s=s & "  客戶連結型態：" & s5ary(1)    
     t=t & " and rtcust.usekind ='計量制' "  
  else
     S=S & " 客戶連結型態︰全部 "
  end if        
  s7=document.all("search7").value    
  if isdate(s7) then
     s=s & "申請日自︰" & s7 & " 以來，"
     t=t & " and rtcust.rcvd >= '" & s7 & "' "
  end if
  s6=document.all("search6").value  
  iF Isnumeric(S6) then
     s=s & "裝機時間超過︰" & s6 & " 天 "
     t=t & " and (( datediff(dd,rtcust.rcvd,rtcust.finishdat) > " & S6 & " and rtcust.rcvd is not null) or ( datediff(dd,rtcust.rcvd,getdate()) > " & S6 & " and rtcust.finishdat is null)) "
  end if
 '----營運點
  S10=document.all("search10").value
  s="營運點:" &S10 &"  "
  if S10 <> "*" and s10<>"無法歸屬" then
     t=t &" AND (RTCTYTOWN.OPERATIONNAME='" & S10 & "') AND rtcode_A.parm1='AA' "
  elseif s10="無法歸屬" then
     t=t &" AND (RTCTYTOWN.OPERATIONNAME='') and rtcode_A.parm1='AA' "
  end if
  '----經銷商
  S11=document.all("search11").value
  s=S & "經銷商:" &S11 &"  "
  if S11 <> "*" AND S11 <> "直銷" then
     t=t &" AND (RTCODE_A.CODENC='" & S11 & "') "
  ELSEIF S11="直銷" THEN 
     t=t &" AND (RTCODE_A.PARM1='AA') "
  end if  
  s12=document.all("search12").value
  s12ary=split(s12,";")
  if s12ary(0) = "01" then
     s=s & " 客戶狀態：" & s12ary(1)
     t=t & " and rtcust.DOCKETDAT IS NOT NULL AND rtcust.DROPDAT IS NULL "
  elseif s12ary(0) = "02" then
     s=s & "  客戶狀態：" & s12ary(1)    
     t=t & " and rtcust.DOCKETDAT IS NOT NULL AND rtcust.DROPDAT IS NOT NULL "
  elseif s12ary(0) = "03" then
     S=S & " 客戶狀態：" & s12ary(1)    
     t=t & " and rtcust.DOCKETDAT IS NULL AND rtcust.DROPDAT IS NULL AND rtcust.CANCELDAT IS NULL"
  elseif s12ary(0) = "04" then
     S=S & " 客戶狀態：" & s12ary(1)    
     t=t & " and rtcust.CANCELDAT IS NULL"     
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
<center>
<table width="70%">
  <tr class=dataListTitle align=center>請輸入(選擇)客戶資料搜尋條件</td><tr>
</table>
<table width="70%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">營運點</td>
    <td width="60%" bgcolor="silver">
      <select name="search10" size="1" class=dataListEntry ID="Select1">
        <%=S10%>
    </select>      
    </td></tr>        
<tr><td class=dataListHead width="40%">經銷商</td>
    <td width="60%"  bgcolor="silver">
    <select name="search11" size="1" class=dataListEntry ID="Select1">
        <%=S11%>
    </select>      
    </td>
</tr>    
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search1" size="25" maxlength="15" class=dataListEntry>
    </td></tr>
<tr><td class=dataListHead width="40%">客戶連結方式</td>
    <td width="60%"  bgcolor="silver">
      <select name="search5" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="01;單機型">單機型</option>
        <option value="02;計量制">計量制</option>
      </select>
     </td>
</tr>    
<tr><td class=dataListHead width="40%">身份證字號</td>
    <td width="60%" bgcolor="silver" >
    <input type=password name="search8" size="10" maxlength="10" class=dataListEntry>
    </td></tr>    
<tr><td class=dataListHead width="40%">客戶名稱</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search2" size="25" maxlength="25" class=dataListEntry>
    </td></tr>
<tr><td class=dataListHead width="40%">HN聯單號碼</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search3" size="10" maxlength="10" class=dataListEntry>
    </td></tr>    
<tr><td class=dataListHead width="40%">IP位址</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search9" size="20" maxlength="15" class=dataListEntry>
    </td></tr>    
<tr><td class=dataListHead width="40%">裝機地址</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search4" size="40" maxlength="60" class=dataListEntry>
    </td></tr>        
<tr><td class=dataListHead width="40%">申請日自</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search7" size="10" maxlength="60" class=dataListdata readonly>
    <input type="button" id="B7"  name="B7" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C7"  name="C7"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
    </td></tr>            
<tr><td class=dataListHead width="40%">裝機時間超過︰</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search6" size="3" maxlength="60" class=dataListEntry>
    天</td></tr>      
<tr><td class=dataListHead width="40%">用戶狀態</td>
    <td width="60%" bgcolor="silver" >
    <select name="search12" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="01;有效戶">有效戶</option>
        <option value="02;退租戶">退租戶</option>
        <option value="03;未完工戶">未完工戶</option>
        <option value="04;作廢戶">作廢戶</option>
      </select>
    </td></tr>            
</table>
<table width="70%" align=right><tr><td></td><td align=right>
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</body>
</html>