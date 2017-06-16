
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
     s="社區名稱：含('" & s1 & "')字元"  
     t=t & " rtcustadsl.housename like '%" & s1 & "%' "
  else
     s="社區名稱：全部  "
     t=t & " rtcustadsl.housename <> '' "
  end if
  s2=document.all("search2").value
  if len(trim(s2)) > 0 then
     s="  客戶名稱：含('" & s2 & "')字元"  
     t=t & " and rtobj.cusnc like '%" & s2 & "%'"
  else
     s=s & "  客戶名稱：全部  "
     t=t & " and rtcustadsl.cusid <>''"
  end if  
  s3=document.all("search3").value
  if len(trim(s3)) > 0 then
     s="  身份證字號：含('" & s3 & "')字元"  
     t=t & " and rtCUSTadsl.socialid like '%" & s3 & "%'"
  else
     s=s & "  身份證字號：全部  "
     t=t & " and rtcustadsl.socialid <>'*'"
  end if    
  s4=document.all("search4").value
  if len(trim(s4)) > 0 then
     s=s & "  HN聯單號碼：含('" & s4 & "')字元"  
     t=t & " and rtCUSTadsl.cusno like '%" & s4 & "%'"
  else
     s=s & "  HN聯單號碼：全部  "
     t=t & " and rtcustadsl.cusno <>'*'"
  end if      
  s5=document.all("search5").value
  s5ary=split(s5,";")
  s=s & "  方案類別：'" & s5ary(1) & "'  "
  if s5ary(0)="*" then
     t=t & " and rtcustadsl.ss365<>'*' "
  elseif s5ary(0)="1" then
    ' t=t & " and (rt365account.type='399') "
     t=t & " and ( rtcustadsl.ss365 = '') " 
  elseif s5ary(0)="2" then
     't=t & " and (rt365account.type='599') "
     t=t & " and ( rtcustadsl.ss365 <> '') "     
  end if        
  '送件日
  s6=document.all("search6").value
  if len(trim(s6)) > 0 then
     s=s & "  送件日期：自 '" & s6 & "' 至 "
     t=t & " and rtCUSTadsl.deliverdat >= '" & s6 & "'"
  else
 '    s=s & "  送件日期：自 2000/01/01 至 "
 '    t=t & " and rtcustadsl.deliverdat >= '2000/01/01' "
  end if        
  s7=document.all("search7").value
  if len(trim(s7)) > 0 then
     s=s & s7 & " "
     t=t & " and rtCUSTadsl.deliverdat <= '" & s7 & "'"
  else
 '    s=s & " 9999/12/31 "
 '    t=t & " and rtcustadsl.deliverdat <= '9999/12/31' "
  end if          
  s10=document.all("search10").value
  s8=document.all("search8").value
  s9=document.all("search9").value
  s9ary=split(s9,";")
  if len(trim(s10)) > 0 then
     if len(trim(s8)) = 0 then
        s8=now()
     end if
     if s9ary(0)="F" then
        S10X=dateadd("d",-s10,s8)
        s=s & "  送件日期：自 " & s8 & " 起往前 " & s10 & " 天 "
        t=t & " and rtcustadsl.deliverdat >='" & S10X & "' and rtcustadsl.deliverdat <='" & S8 & "' "
     else
        S10X=dateadd("d",s10,s8)
        s=s & "  送件日期：自 " & s8 & " 起往後 " & s10 & " 天 "
        t=t & " and rtcustadsl.deliverdat >='" & S8 & "' and rtcustadsl.deliverdat <='" & S10X & "' "
     end if
     
  end if
  '完工日
    s11=document.all("search11").value
  if len(trim(s11)) > 0 then
     s=s & "  完工日期：自 '" & s11 & "' 至 "
     t=t & " and rtCUSTadsl.finishdat >= '" & s11 & "'"
  else
   '  s=s & "  完工日期：自 2000/01/01 至 "
   '  t=t & " and rtcustadsl.finishdat >= '2000/01/01' "
  end if        
  s12=document.all("search12").value
  if len(trim(s12)) > 0 then
     s=s & s12 & " "
     t=t & " and rtCUSTadsl.finishdat <= '" & s12 & "'"
  else
  '   s=s & " 9999/12/31 "
  '   t=t & " and rtcustadsl.finishdat <= '9999/12/31' "
  end if          
  s15=document.all("search15").value
  s13=document.all("search13").value  
  s14=document.all("search14").value    
  s14ary=split(s14,";")
  if len(trim(s15)) > 0 then
     if len(trim(s13)) = 0 then
        s13=now()
     end if
     if s14ary(0)="F" then
        S15X=dateadd("d",-s15,s13)
        s=s & "  完工日期：自 " & s13 & " 起往前 " & s15 & " 天 "
        t=t & " and rtcustadsl.finishdat >='" & S15X & "' and rtcustadsl.finishdat <='" & S13 & "' "
     else
        S15X=dateadd("d",s15,s13)
        s=s & "  完工日期：自 " & s13 & " 起往後 " & s15 & " 天 "
        t=t & " and rtcustadsl.finishdat >='" & S13 & "' and rtcustadsl.finishdat <='" & S15X & "' "
     end if
  end if
  s16=document.all("search16").value
  s16ary=split(s16,";")
  s=s & " 帳號發放狀態：" & s16ary(1)
  if s16ary(0)="Y" then
     t=t & " and (rt365account.cusid <> '' and rt365account.dropdat is null ) "
  elseif  s16ary(0)="N" then
     t=t & " and (rt365account.cusid is null or rt365account.cusid = '' )"
  end if
  s17=document.all("search17").value      
  if len(trim(s17)) > 0 then
      s17X=dateadd(day,now(),s17)
      s=s & " 密碼到期天數：" & s17 & "天內"
      t=t & " and rt365account.deadline <='" & s17X & "'"
  end if
  s18=document.all("search18").value      
  if len(trim(s18)) > 0 then
      s=s & " 批次代號：(含'" & s18 & "'字元)"
      t=t & " and rt365account.batchno like '%" & s18 & "%' "
  end if
  s19=document.all("search19").value      
  if len(trim(s19)) > 0 then
      s=s & " 列印序號：(含'" & s19 & "'字元)"
      t=t & " and rt365account.prtno like '%" & s19 & "%' "
  end if  
  s20=document.all("search20").value      
  s20ary=split(s20,";")
  s=s & " 列印狀態：" & s20ARY(1) 
  if s20ary(0)="Y" then
     t=t & " and RT365account.prtno <> '' " 
  elseif s20ary(0)="N" then
     t=t & " and RT365account.prtno ='' "
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
   Sub Srbtnonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="search" & clickid
	   if isdate(document.all(clickkey).value) then
	      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
       end if
       call objEF2KDT.show(1)
       if objEF2KDT.strDateTime <> "" then
          document.all(clickkey).value = objEF2KDT.strDateTime
       end if
   End Sub 
Sub btn1_onClick()
  window.close
End Sub
-->
</script>
</head>
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"     codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<body>
<center>
<table width="80%">
  <tr class=dataListTitle align=center>請輸入(選擇)客戶資料搜尋條件</td><tr>
</table>
<table width="70%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="30%">社區名稱</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search1" size="25" maxlength="15" class=dataListEntry>
    </td></tr>
<tr><td class=dataListHead width="30%">客戶名稱</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search2" size="25" maxlength="25" class=dataListEntry>
    </td></tr>
<tr><td class=dataListHead width="30%">身份證字號</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search3" size="10" maxlength="10" class=dataListEntry>
    </td></tr>    
<tr><td class=dataListHead width="30%">HN聯單號碼</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search4" size="10" maxlength="10" class=dataListEntry>
    </td></tr>    
<tr><td class=dataListHead width="30%">批次代號</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search18" size="11" maxlength="11" class=dataListEntry>
    </td></tr>         
<tr><td class=dataListHead width="30%">列印序號</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search19" size="11" maxlength="11" class=dataListEntry>
    </td></tr>                
<tr><td class=dataListHead width="30%">方案類別</td>
    <td width="70%" bgcolor="silver" >
      <select name="search5" size="1" class=dataListEntry>
        <option value="*;全部" selected>全部</option>
        <option value="1;399">399</option>
        <option value="2;599">599</option>
      </select>
    </td></tr>        
<tr><td class=dataListHead width="30%">送件日期</td>
    <td width="70%" bgcolor="silver" ><font size="2">(1)
    <input type=text name="search6" size="10" maxlength="10" class=dataListEntry>
    <input type="button" id="B6"  name="B6" height="70%" width="70%" style="Z-INDEX: 1" value="..." onclick="SrBtnOnclick">
    <font size="2">至
    <input type=text name="search7" size="10" maxlength="10" class=dataListEntry><font size="2">
    <input type="button" id="B7"  name="B7" height="70%" width="70%" style="Z-INDEX: 1" value="..." onclick="SrBtnOnclick">    
    <br><font size="2">(2)    
    <input type=text name="search8" size="10" maxlength="10" class=dataListEntry>
    <input type="button" id="B8"  name="B8" height="70%" width="70%" style="Z-INDEX: 1" value="..." onclick="SrBtnOnclick">    
    <font size="2">起往
      <select name="search9" size="1" class=dataListEntry>
        <option value="F;前">前</option>
        <option value="B;後">後</option>
      </select>        
    <input type=text name="search10" size="5" maxlength="10" class=dataListEntry><font size="2">天
    </td></tr>                
<tr><td class=dataListHead width="30%">完工日期</td>
    <td width="70%" bgcolor="silver" ><font size="2">(1)
    <input type=text name="search11" size="10" maxlength="10" class=dataListEntry>
    <input type="button" id="B11"  name="B11" height="70%" width="70%" style="Z-INDEX: 1" value="..." onclick="SrBtnOnclick">    
    <font size="2">至
    <input type=text name="search12" size="10" maxlength="10" class=dataListEntry>
    <input type="button" id="B12"  name="B12" height="70%" width="70%" style="Z-INDEX: 1" value="..." onclick="SrBtnOnclick">    
    <font size="2">
    <br><font size="2">(2)
    <input type=text name="search13" size="10" maxlength="10" class=dataListEntry>
    <input type="button" id="B13"  name="B13" height="70%" width="70%" style="Z-INDEX: 1" value="..." onclick="SrBtnOnclick">    
    <font size="2">起往
      <select name="search14" size="1" class=dataListEntry>
        <option value="F;前">前</option>
        <option value="B;後">後</option>
      </select>    
    <input type=text name="search15" size="5" maxlength="10" class=dataListEntry><font size="2">天
    </td></tr>            
<tr><td class=dataListHead width="30%">帳號發放狀態</td>
    <td width="70%"  bgcolor="silver">
      <select name="search16" size="1" class=dataListEntry>
        <option value="*;全部" selected>全部</option>
        <option value="Y;已發送">已發送</option>
        <option value="N;未發送">未發送</option>
      </select>
     </td>
</tr>    
<tr><td class=dataListHead width="30%">DM列印狀態</td>
    <td width="70%"  bgcolor="silver">
      <select name="search20" size="1" class=dataListEntry>
        <option value="*;全部" selected>全部</option>
        <option value="Y;已發送">已列印</option>
        <option value="N;未發送">未列印</option>
      </select>
     </td>
</tr>    
<tr><td class=dataListHead width="30%">密碼到期天數</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search17" size="5" maxlength="10" class=dataListEntry><font size="2">天
    </td></tr>            
</table>
<table width="80%" align=right><tr><td></td><td align=right>
  <input type="button" value=" 查詢 " class=dataListButton name="btn" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</body>
</html>