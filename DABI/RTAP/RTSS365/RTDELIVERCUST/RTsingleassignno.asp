
<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
   logonid=session("userid")
   key=request("key")
   keyary=split(key,";")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")
   Set conn=Server.CreateObject("ADODB.Connection")  
   set rs=server.CreateObject("ADODB.recordset")
   set rs2=server.CreateObject("ADODB.recordset")
   set rs3=server.CreateObject("ADODB.recordset")   
   DSN="DSN=RtLib"
   conn.Open DSN
   RS3.OPEN "select * from rtCUSTADSL where cusid='" & keyary(0) & "' and entryno=" & keyary(1),CONN 
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
   '   Newbatchno=yymmdd & right("000" & cstr(cint(mid(rsc("batchno"),7,3)) + 1),3)
      Newbatchno=yymmdd & cstr(cint(mid(rsc("batchno"),9,3)) + 1)
   else
      Newbatchno=yymmdd & "001"
   end if           
   '------------------------------------------------------------------------------------------------------------------------
   sql="select * from rt365account where cusid='" & keyary(0) & "' and entryno=" & keyary(1) & " and dropdat is null "
   rs.Open sql,conn,1,1
   if not rs.EOF then
      endpgm="3"
      errmsg="該用戶已給過帳號且該帳號尚號撤銷;帳號為:" & rs("ss365account")
   else
     '900920修改為不分399與599==>因為無399帳號而以599帳號代替
     ' sql="SELECT * FROM RT365ACCOUNT WHERE (CUSID = '' or cusid is null ) and type='" & ctype & "' ORDER BY  SS365ACCOUNT"
      sql="SELECT * FROM RT365ACCOUNT WHERE (CUSID = '' or cusid is null )  ORDER BY  SS365ACCOUNT"
      rs2.Open sql,conn,3,3
      if rs2.recordcount = 0 then
         endpgm="4"
         errmsg="帳號資料檔已無其它帳號可供使用,請檢查帳號資料!"
      else
         rs2("cusid")=keyary(0)
         rs2("entryno")=keyary(1)
         rs2("usedate")=now()
         rs2("batchno")=newbatchno
         rs2.update
      end if
      rs2.close               
   end if
   On Error Resume Next
   rs.Close
   conn.Close
   If Err.number <> 0 then
      endpgm="2"
      errmsg=cstr(Err.number) & "=" & Err.description
   elseif endpgm="3" or endpgm="4" then
   else
      endpgm="1"
      errmsg=""
   end if
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "單筆給號作業完成",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.close
    else
       msgbox "給號過程發生錯誤,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=verify.asp>
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
</html>
