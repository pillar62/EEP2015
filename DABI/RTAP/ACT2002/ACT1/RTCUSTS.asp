
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
  s2=document.all("search2").value
  if len(trim(s2)) > 0 then
     s="  客戶名稱：含('" & s2 & "')字元"  
     t=t & " HB2002ACT1.name like '%" & s2 & "%'"
  else
     s=s & "  客戶名稱：全部  "
     t=t & " HB2002ACT1.name <>'' "
  end if  
  s3=document.all("search3").value
  s4=document.all("search4").value  
  if len(trim(s3))= 0 then
     s3="2002/02/01"
  end if
  if len(trim(s4))=0 then
     s4="9999/12/31"
  end if  
  s="  申請日期︰自('" & s3 & "')-('" & s4 & "')"  
  t=t & " and hb2002act1.edat >= '" & s3 & "' and hb2002act1.edat <='" & S4 & "' "
  
  s5=document.all("search5").value
  s5ary=split(s5,";")
  s=s & "  資料狀態：'" & s5ary(1) & "' "  
  '---已確認
 ' if s5ary(0)="Y" then
 '    t=t & " and hb2002ACT1.cfmdat is not null and hb2002act1.dropdat is null "
 ' end if
  '---未確認
 ' if s5ary(0)="N" then
 '    t=t & " and hb2002ACT1.cfmdat is null and hb2002act1.dropdat is null "
 ' end if  
  '---已作廢
  if s5ary(0)="D" then
     t=t & " and hb2002ACT1.dropdat is not null "
  end if  
  
  s6=document.all("search6").value
  if len(trim(s6)) > 0 then
     s=s & "  客戶地址：含('" & s6 & "')字元"  
     t=t & " and (RTCounty.CUTNC + HB2002ACT1.TOWNSHIP + HB2002ACT1.RADDR )  like '%" & s6 & "%'"
  end if      
  s7=document.all("search7").value
  if len(trim(s7)) > 0 then
     s=s & "  郵件信箱：含('" & s7 & "')字元"  
     t=t & " and HB2002ACT1.email like '%" & s7 & "%'"
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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"      codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
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
<tr><td class=dataListHead width="40%">申請日期</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search3" size="10" maxlength="10" class=dataListEntry>
     <input type="button" id="B3"  name="B3" height="70%" width="70%" style="Z-INDEX: 1" value="..." onclick="SrBtnOnclick"> 
     <font size="2">至
    <input type=text name="search4" size="10" maxlength="10" class=dataListEntry>
     <input type="button" id="B4"  name="B4" height="70%" width="70%" style="Z-INDEX: 1" value="..." onclick="SrBtnOnclick"> 
    </td></tr>    
<tr><td class=dataListHead width="40%">資料狀態</td>
    <td width="60%" bgcolor="silver" >
      <select name="search5" size="1" class=dataListEntry>
        <option value="*;全部" selected>全部</option>
        <option value="D;已作廢">已作廢</option>    
    </td></tr>   
<tr><td class=dataListHead width="40%">客戶地址</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search6" size="40" maxlength="50" class=dataListEntry>
    </td></tr>      
<tr><td class=dataListHead width="40%">郵件信箱</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search7" size="35" maxlength="50" class=dataListEntry>
    </td></tr>          
</table>
<table width="70%" align=right><tr><td></td><td align=right>
  <input type="button" value=" 查詢 " class=dataListButton name="btn" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</body>
</html>