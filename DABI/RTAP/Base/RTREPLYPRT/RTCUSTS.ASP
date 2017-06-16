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
  '--------------
  s1=document.all("search1").value
  if len(trim(s1)) > 0 then
     s="客戶名稱：含('" & s1 & "')字元"  
     t=t & " rtobj.cusnc like '%" & s1 & "%' "
  else
     s="客戶名稱：全部  "
     t=t & " rtcustadsl.cusid <> '*' "
  end if
  '-----安裝進度
  s2=document.all("search2").value
  s2ary=split(s2,";")
  
  s=S & "客戶狀態：" & s2ary(1)  
  '全部(不含退租或撤銷戶)
  if s2ary(0)="" then
     t=t & " and rtcustadsl.docketdat is not null "
  '((不可建置客戶))
  elseif s2ary(0)="1" then
     t=t & " and rtcustadsl.docketdat is not null "
  '((可建置,未達三人))
  elseif s2ary(0)="2" then
     t=t & " and rtcustadsl.finishdat is not null and rtcustadsl.docketdat is null " 
  '((可建置,未達三人))
  elseif s2ary(0)="3" then
     t=t & " and rtcustadsl.workingreply is not null and rtcustadsl.finishdat is null and rtcustadsl.docketdat is null " 
  '((退租戶或撤銷戶))
  elseif s2ary(0)="4" then
     t=t & " and rtcustadsl.sendworking is not null and rtcustadsl.workingreply is null and rtcustadsl.docketdat is null and rtcustadsl.finishdat is null " 
  end if
  
  '-------------
  s3=document.all("search3").value
  s4=document.all("search4").value
  s5=document.all("search5").value
  if len(trim(s3))>0 then
     t=t & " and rtcustadsl.stockid='" & s3 & "' "
  end if
  if len(trim(s4))>0 then
     t=t & " and rtcustadsl.branch='" & s4 & "' "
  end if  
  if len(trim(s5))>0 then
     t=t & " and rtcustadsl.bussman='" & s5 & "' "
  end if    
  s6=document.all("search6").value
  if len(trim(s6))>0 then
     t=t & " and RTCOUNTY.CUTNC + RTCUSTADSL.TOWNSHIP2 + RTCUSTADSL.RADDR2 like '%" & s6 & "%' "
     s=S & "　裝機地址：含('" & s6 & "')字元"
  end if  
  s7=document.all("search7").value
  if len(trim(s7))>0 then
     t=t & " and RTCUSTADSL.housename like '%" & s7 & "%' "
     s=S & "　社區名稱：含('" & s7 & "')字元"
  end if    
  s8=document.all("search8").value
  if len(trim(s8))>0 then
     t=t & " and RTCUSTADSL.ORDERNO like '%" & s8 & "%' "
     s=S & "　收件編號：含('" & s8 & "')字元"
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
   End Sub 
Sub SrClear()
      Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="search" & clickid       
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"     codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
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
<tr><td class=dataListHead width="30%">申請日期</td>
    <td width="70%" bgcolor="silver" >
    <input READONLY type=text name="search9" size="10" maxlength="60" class=dataListEntry><FONT SIZE="2">(含)以前</FONT>
         <input type="button" id="B9"  name="B9"   width="100%" style="Z-INDEX: 1"  value="..." onclick="Srbtnonclick()"  >                  
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C9"  name="C9"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrCLEAR()" >     
    </td></tr>            
<tr><td class=dataListHead width="30%">客戶狀態</td>
    <td width="70%" bgcolor="silver" >
      <select name="search2" size="1" class=dataListEntry>
        <option value=";全部(不含退租戶)"  selected >全部(不含退租戶)</option>
        <option value="1;不可建置">不可建置客戶</option>
        <option value="2;可建置,未達三人">可建置,未達三人</option>
        <option value="3;可建置,">可建置</option>        
        <option value="4;退租戶">退租戶</option>                                                                
      </select>    </td></tr>    
<tr><td class=dataListHead width="30%">列印狀態</td>
    <td width="70%" bgcolor="silver" >
      <select name="search2" size="1" class=dataListEntry>
        <option value="1;未列印" selected>未列印</option>
        <option value="2;已列印">已列印</option>
        <option value="3;全部,">全部</option>        
      </select>    </td></tr>    
</table>
<table width="80%" align=right><tr><td></td><td align=right>
  <input type="button" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</FORM>
</body>
</html>
<!-- #include file="rtgetBRANCHBUSS.inc" -->