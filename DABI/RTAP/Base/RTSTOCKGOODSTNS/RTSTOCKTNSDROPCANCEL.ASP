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
  ' On Error Resume Next
   SQLXX=" update RTSTOCKTNSH  set dropdat=null,dropusr='' where transNO='" & KEY(0) & "' "
   connxx.Execute SQLXX
   SQLXX=" insert into RTSTOCKTNstranslog (transno,ENTRYNO,TRANSdat,OUTWAREHOUSE,INWAREHOUSE,OUTDEPT,OUTSIGN,INDEPT,INSIGN,INSIGNDAT,eusr,edat,uusr,udat,checkdept,CHECKUSR,dropdat,dropusr,transd,transcode,transusr) " _
        &" SELECT A.TRANSNO, C.ENTRYNO + 1 AS Expr2, A.TRANSDAT, A.OUTWAREHOUSE, " _
        &"A.INWAREHOUSE,A.OUTDEPT,A.OUTSIGN,A.INDEPT,A.INSIGN,A.INSIGNDAT, A.EUSR, A.EDAT, A.UUSR, A.UDAT, A.CHECKDEPT,A.CHECKUSR,A.DROPDAT,a.dropusr, '" & datevalue(now()) & "'," _
        &"'R' AS Expr1,'" & v(0) & "' "  _
        &"FROM RTSTOCKTNSH A CROSS JOIN " _
        &"(SELECT ISNULL(MAX(entryno), 0) AS ENTRYNO " _
        &"FROM rtstockTNStranslog " _
        &"WHERE          TRANSno ='" & key(0) & "') C " _
        &"WHERE         (A.TRANSNO = '" & key(0) & "') "
   'RESPONSE.Write sqlxx
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
       msgbox "調撥單作廢返轉完成",0
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