<%
key=request("key")
keyary=split(key,";")
stat=request("stat")
XXCUSID=KEYARY(0)
xxentryno=keyary(1)
accountXX=request("search1")
USEDATEXX=request("search2")
errflag=""
if stat="Y" and errflag<>"Y" then
'   logonid=session("userid")
'   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
'         V=split(rtnvalue,";")
   Set conn=Server.CreateObject("ADODB.Connection")  
   set rs=server.CreateObject("ADODB.recordset")
   set rs2=server.CreateObject("ADODB.recordset")
   set rs3=server.CreateObject("ADODB.recordset")   
   DSN="DSN=RtLib"
   conn.Open DSN
   RS3.OPEN "select * from rtCUSTADSL where cusid='" & XXCUSID & "' and entryno=" & XXENTRYNO,CONN 
   IF LEN(TRIM(RS3("SS365")))=0 THEN
         CTYPE="399"
   ELSE
         CTYPE="599"
   END IF
   rs3.close
   set rs3=nothing
'------------------------------------------------------------------------------------------------------------------------   
   '計算rt365account檔內最大序號
   YY=cstr(datepart("yyyy",now()))
   mm=right(cstr("0" & cstr(datepart("m",now()))),2)
   dd=right(cstr("0" & cstr(datepart("d",now()))),2)
   YYMMDD=yy & mm & dd
   Set rsc=Server.CreateObject("ADODB.Recordset")
   sqlstr2="select max(batchno) AS batchno from rt365account where  batchno like '" & yymmdd & "%'" 
   rsc.open sqlstr2,conn
   newbatchno=""
   if len(rsc("batchno")) > 0 then
      Newbatchno=yymmdd & right("000" + cstr(cint(mid(rsc("batchno"),9,3)) + 1),3)
   else
      Newbatchno=yymmdd & "001"
   end if           
'------------------------------------------------------------------------------------------------------------------------   
   '檢查帳號資料是否存在(手動給號者,須檢查該帳號是否存在及是否有效,並須檢查其類型399或599是否與該客戶申請之方案相同
   sql="select * from rt365account where SS365ACCOUNT='" & ACCOUNTXX & "'"
   rs.Open sql,conn,1,1
   if RS.EOF then
      endpgm="3"
      errmsg="輸入之帳號不存在帳號資料庫內,請檢查輸入之帳號或帳號資料檔!"
      errflag="Y"
   elseif len(trim(rs("cusid"))) > 0 then
      endpgm="3"
      errmsg="此帳號已被使用,客戶為:" & rs("cusid")
      errflag="Y"      
 ' 900920 : 不分399與599帳號,因為全部399之客戶皆使用599帳號代替
 '  elseif ctype <> rs("type") then
 '     endpgm="3"
 '     errmsg="輸入之帳號為:" & rs("type") & "類型,但客戶申請之方案為:" & ctype & "類型!"       
 '     errflag="Y"      
   end if         
   rs.close
   '------------------------------------------------------------------------------------------------------------------------
   if errflag <> "Y" then
      sql="select * from rt365account where cusid='" & keyary(0) & "' and entryno=" & keyary(1) & " and dropdat is null "
      rs.Open sql,conn,1,1
      if not rs.EOF then
         endpgm="3"
         errmsg="該用戶已給過帳號且該帳號尚號撤銷;帳號為:" & rs("ss365account")
      else
         sql="SELECT * FROM RT365ACCOUNT WHERE ss365account='" & accountxx & "'"
         rs2.Open sql,conn,3,3
         if rs2.recordcount = 0 then
            endpgm="4"
            errmsg="帳號資料檔已無其它帳號可供使用,請檢查帳號資料!"
         else
            rs2("cusid")=xxcusid
            rs2("entryno")=xxentryno
            rs2("usedate")=now()
            rs2("batchno")=newbatchno
            rs2.update
         end if
         rs2.close               
      end if
   end if
end if
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  s1=document.all("search1").value
  errcode=""
  if len(trim(s1))=0 then
     msgbox "帳號不可空白,您可以選擇取消或輸入帳號資料!",vbexclamation+vbokonly,"欄位檢查訊息"
     errcode="1"
     document.all("stat").value="N"
  elseif len(trim(s1)) < 11 then
     msgbox "輸入之帳號長度不足,帳號長度應為11位,您輸入" & len(trim(s1)) & "位!",vbexclamation+vbokonly,"欄位檢查訊息"
     errcode="1"
     document.all("stat").value="N"
  end if
  if errcode="" then
     document.all("stat").value="Y"
     window.document.all("frm1").submit
 
     Dim winP,docP
     Set winP=window.Opener
     Set docP=winP.document
 '    docP.all("keyform").Submit
 '    winP.focus()
 '    window.close
  end if
End Sub
Sub btn1_onClick()
     Dim winP,docP
     Set winP=window.Opener
     Set docP=winP.document
   '  docP.all("keyform").Submit
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
<center>
<form method=post name="frm1" action="rtmanualassignno.asp">
<input type="hidden" name="key" value="<%=key%>">
<input type="hidden" name="CUSXX" value="<%=XXCUSID%>">
<input type="hidden" name="ENTRYXX" value="<%=XXENTRYNO%>">
<input type="hidden" name="stat" value="<%=STAT%>">
<input type="hidden" name="ERRFLAG" value="<%=ERRFLAG%>">
<table width="70%" align="center">
  <tr class=dataListTitle align=center>請輸入先看先贏手動帳號給號號碼</td><tr>
</table>
<table width="70%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="30%">帳號</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search1" size="11" maxlength="11" class=dataListEntry value="<%=ACCOUNTXX%>">
    </td></tr>
<tr><td class=dataListHead width="30%">給號日期</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search2" size="11" maxlength="11" readonly class=dataListEntry  value="<%=USEDATEXX%>">
          <input type="button" id="B2"  name="B2" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">                          
 <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C2"  name="C2"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="Srclear" >  </td>            
    </td></tr>    
</table>
<table width="70%" align=center><tr ><td bgcolor="silver" width="30%">訊息</td><td  bgcolor="silver" width="70%"><%=errmsg%></td></tr>
<table width="70%" align=center><tr><td></td><td align=center>
  <input type="button" value=" 確認 " class=dataListButton name="btn" style="cursor:hand">
  <input type="button" value=" 取消 " class=dataListButton name="btn1" style="cursor:hand">
</table>
</table>
</form>
</body>
</html>