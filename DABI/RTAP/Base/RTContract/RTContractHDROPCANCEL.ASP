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
   SQLXX="update HBContractH set dropdat=NULL,dropusr='' where CTNO='" & KEY(0) & "' "
   connxx.Execute SQLXX
   SQLXX=" insert into HBContractHlog ( CTNO, ENTRYNO, CTproperty, CTTree1, CTTree2, CTOBJECT, CTOBJNAME, " _
        &"CTTELNO, CTCONTACT, CTEDIT, CUTID, TOWNSHIP, ADDRESS, RZONE, MEMO, CTSTRDAT, CTENDDAT, ALARMDAT, " _
        &"RCVORPAY, ARAP, AUTOEXTEND, STRBILLINGYM, MONTHLYDAT, ARAPALARMDAT, SPLITRATE, REMITBANK1, REMITBANK2," _
        &"AC, ACNO, SIGNDEPT, SIGNPERSON, RCVDAT, EDAT, EUSR,  CONTRACTNO, AREACODE, VOLUME, PAGECNT, COMTYPE, " _
        &"DROPDAT, DROPUSR, TRANSDAT, TRANSUSR, TRANSCODE) " _
        &" SELECT A.CTNO, C.ENTRYNO + 1, A.CTproperty, A.CTTree1, A.CTTree2, A.CTOBJECT, A.CTOBJNAME, " _
        &"A.CTTELNO, A.CTCONTACT, A.CTEDIT, A.CUTID, A.TOWNSHIP, A.ADDRESS, A.RZONE, A.MEMO, A.CTSTRDAT, A.CTENDDAT, A.ALARMDAT, " _
        &"A.RCVORPAY, A.ARAP, A.AUTOEXTEND, A.STRBILLINGYM, A.MONTHLYDAT, A.ARAPALARMDAT, A.SPLITRATE, A.REMITBANK1, A.REMITBANK2," _
        &"A.AC, A.ACNO, A.SIGNDEPT, A.SIGNPERSON, A.RCVDAT, A.EDAT, A.EUSR,  A.CONTRACTNO, A.AREACODE, A.VOLUME, A.PAGECNT, A.COMTYPE, " _
        &"A.DROPDAT, A.DROPUSR,'" & DATEVALUE(NOW()) & "','" & V(0) & "','R' " _
        &"FROM HBContractH A CROSS JOIN " _
        &"(SELECT ISNULL(MAX(entryno), 0) AS ENTRYNO " _
        &"FROM HBContractHlog " _
        &"WHERE          CTNO ='" & key(0) & "') C " _
        &"WHERE         (A.CTNO = '" & key(0) & "') "
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
       msgbox "合約作廢返轉完成",0
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