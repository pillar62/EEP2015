<%@ Language=VBScript %>
<% KEY=SPLIT(REQUEST("KEY"),";")
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTfareastSndWrk WHERE wrkno ='" & KEY(0) & "' "
   RSXX.OPEN SQLXX,CONNxx
   wrktyp = RSXX("wrktyp")
   endpgm="1"
   IF ISNULL(RSXX("closedat"))   THEN
      ENDPGM="3"
  ELSE
      strSP="update RTfareastSndWrk set closedat=null, closeusr='' where wrkno ='" & KEY(0) & "' "
      if wrktyp ="02" then
			strSP = strSP & " update RTfareastCust set finishdat = null where comq1=" &key(1)&" and lineq1 = " &key(2)&" and cusid = '" &key(3) &"' "
      elseif wrktyp ="03" then
			strSP = strSP & " update RTfareastCust set overduedat = null where comq1=" &key(1)&" and lineq1 = " &key(2)&" and cusid = '" &key(3) &"' "
      elseif wrktyp ="04" then
			strSP = strSP & " update RTfareastCust set dropdat = null where comq1=" &key(1)&" and lineq1 = " &key(2)&" and cusid = '" &key(3) &"' "
      elseif wrktyp ="05" then	'復機
      		SET RSYY=Server.CreateObject("ADODB.RECORDSET")
			sqlYY="select max(workingdat) AS Dropdat FROM RTfareastSndWrk where cusid ='" & KEY(3) & "' and wrktyp='04' AND CANCELDAT IS NULL AND closedat IS NOT NULL "
			RSYY.OPEN SQLYY,CONNxx
			orgDropdat = RSYY("Dropdat")
			RSYY.CLOSE
			SET RSYY=NOTHING
			strSP = strSP & " update RTfareastCust set dropdat = '"& orgDropdat &"' where comq1=" &key(1)&" and lineq1 = " &key(2)&" and cusid = '" &key(3) &"' "
	  end if

      Set ObjRS = connXX.Execute(strSP)
      If Err.number = 0 then
         ENDPGM="1"
         ERRMSG=""
         'conn.CommitTrans
      else
         ENDPGM="2"
         errmsg=cstr(Err.number) & "=" & Err.description
         'conn.rollbackTrans
      end if         
   END IF
   RSXX.CLOSE
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "遠傳大寬頻社區型客戶派工單結案返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document
       docP.all("keyform").Submit
       winP.focus()
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "客戶派工單尚未結案，不可執行結案返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    else
       msgbox "無法執行客戶派工單結案返轉作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close
 end sub
</script> 
</head>  
<form name=frm1 method=post action="" ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>