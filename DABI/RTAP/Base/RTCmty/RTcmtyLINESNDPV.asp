<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=Request.ServerVariables("LOGON_USER")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTCMTYSndwork WHERE COMQ1=" & KEY(0) & " and prtno='" & key(1) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   endpgm="1"
   '已作廢的派工單不可列印
   IF LEN(TRIM(RSXX("DROPDAT"))) <> 0 THEN
      ENDPGM="3"
   elseif LEN(TRIM(RSXX("CLOSEdat"))) <> 0 then
      endpgm="4"
   ELSE
      endpgm="1"
      errmsg=""
      key1=KEY(0)
      key2=KEY(1)
      key3=V(0)
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
       msgbox "主線派工單列印完成",0
       Set winP=window.Opener
       Set docP=winP.document       
       winP.focus()           
       docP.all("keyform").Submit     
       WINDOW.Form2.action="rtCMTYLINEsndp.asp" & "?key=" & document.all("key1").value & "," & document.all("key2").value & "," & document.all("key3").value 
     '  msgbox WINDOW.Form2.action
       window.frm2.submit   
     '  window.CLOSE
       docP.all("keyform").Submit     
       window.focus()   
    elseIF frm1.htmlfld.value="3" then
       msgbox "此派工單作廢，不可列印" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此派工單已完工結案，不可列印" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    else
       msgbox "無法執行派工單列印作業,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action="rtebtcmtylinesndworkdrop.asp" ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
<input type="text" name="key1" style=display:none value="<%=key1%>" ID="Text3">
<input type="text" name="key2" style=display:none value="<%=key2%>" ID="Text4">
<input type="text" name="key3" style=display:none value="<%=key3%>" ID="Text5">
</form>
<form name=frm2 method=post action="<%=PGMNAME%>" ID="Form2">
</html>
