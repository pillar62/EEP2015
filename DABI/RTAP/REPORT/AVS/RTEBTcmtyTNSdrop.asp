<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   endpgm="1"
   SQLXX="select * FROM RTEBTFtpBuildingRpl WHERE comq1=" & KEY(0) & " and flag='" & key(1) & "' "
   'response.Write sqlxx
   RSXX.Open SQLxx,CONNXX
   '此資料已清除申請，不能作廢
   IF LEN(TRIM(RSXX("CLRFLAG"))) > 0 THEN
      ENDPGM=3
   '此資料已結案，不能作廢
   ELSEIF LEN(TRIM(RSXX("closedat"))) > 0 THEN
      ENDPGM=4
   '此資料已作廢，不可重覆作廢
   ELSEIF LEN(TRIM(RSXX("dropdat"))) > 0 THEN
      ENDPGM=5
   END IF         
   if endpgm=1 then
      sqlxx="update RTEBTFtpBuildingRpl set dropdat=getdate(),dropusr='" & v(0) & "' FROM RTEBTFtpBuildingRpl WHERE comq1=" & KEY(0) & " and flag='" & key(1) & "' "
      connxx.execute sqlxx
      If Err.number > 0 then
         endpgm="4"
         errmsg=cstr(Err.number) & "=" & Err.description
      end if
   end if
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
       msgbox "AVS社區電子轉檔異常資料作廢成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此資料清除申請，不能作廢" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此資料已結案，不能作廢" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此資料已作廢，不可重覆作廢" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    else
       msgbox "無法執行AVS社區電子轉檔異常資料作廢作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcmtylinesndworkdrop.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>