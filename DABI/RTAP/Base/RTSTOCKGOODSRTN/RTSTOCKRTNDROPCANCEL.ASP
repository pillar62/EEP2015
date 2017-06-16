<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   DSN="DSN=RtLib"
   connXX.Open DSN
   On Error Resume Next
   SQLXX=" update RTSTOCKRETURNH set dropdat=null,dropusr='' where RETURNNO='" & KEY(0) & "' "
   connxx.Execute SQLXX
   SQLXX=" insert into RTSTOCKRTNtranslog (RETURNno,ENTRYNO,RETURNdat,factory,checkusr,eusr,edat,uusr,udat,checkdept,transdat,transcode,transusr) " _
        &" SELECT A.RETURNNO, C.ENTRYNO + 1 AS Expr2, A.RETURNDAT, A.FACTORY, " _
        &"A.CHECKUSR, A.EUSR, A.EDAT, A.UUSR, A.UDAT, A.CHECKDEPT,'" & DATEVALUE(NOW()) & "', " _
        &"'R' AS Expr1, '" & V(0) & "' "  _
        &"FROM RTSTOCKRETURNH A CROSS JOIN " _
        &"(SELECT ISNULL(MAX(entryno), 0) AS ENTRYNO " _
        &"FROM rtstockRTNtranslog " _
        &"WHERE          RETURNno ='" & key(0) & "') C " _
        &"WHERE         (A.RETURNNO = '" & key(0) & "') "
   connxx.Execute SQLXX
   If Err.number > 0 then
      endpgm="2"
      errmsg=cstr(Err.number) & "=" & Err.description
   else
      endpgm="1"
      errmsg=""
   end if
   connXX.Close
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "退貨單作廢返轉完成",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    else
       msgbox "無法執行作廢返轉,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTSTOCKDROP.asp>
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
</html>