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
   sqlxx="select * FROM RTSonetSndWrk WHERE wrkno ='" & KEY(0) & "' "
   RSXX.OPEN SQLXX,CONNxx
   '已作廢的派工單不可列印
   IF not isnull(RSXX("canceldat")) THEN
      ENDPGM="3"
   ELSE
      endpgm="1"
      errmsg=""
      key0=KEY(0)
      key1=KEY(1)
      key2=KEY(2)
      key3=KEY(3)
	  key4=RSXX("wrktyp")
      key5=V(0)
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
       'msgbox "Sonet客戶派工單列印完成",0
       Set winP=window.Opener
       Set docP=winP.document       
       winP.focus()           
       docP.all("keyform").Submit
       'select case document.all("key4").value 
		'	case "02"
		'		WINDOW.Form2.action="/RTAP/REPORT/Sonet/RTSonetCmtyLineSndWrkP02.asp" & "?parm=" & document.all("key0").value & ";"
		'	case "03"
		'		WINDOW.Form2.action="/RTAP/REPORT/Sonet/RTSonetCmtyLineSndWrkP03.asp" & "?parm=" & document.all("key0").value & ";"
		'	case "04"
		'		WINDOW.Form2.action="/RTAP/REPORT/Sonet/RTSonetCmtyLineSndWrkP04.asp" & "?parm=" & document.all("key0").value & ";"
		'	case "05"
		'		WINDOW.Form2.action="/RTAP/REPORT/Sonet/RTSonetCmtyLineSndWrkP05.asp" & "?parm=" & document.all("key0").value & ";"
		'	case else
       'end select 
     'msgbox WINDOW.Form2.action
       WINDOW.Form2.action="/RTAP/REPORT/Sonet/RTSonetCustSndWrkP.asp" & "?parm=" & document.all("key0").value &";"& document.all("key4").value &";"
       window.frm2.submit   
     '  window.CLOSE
       docP.all("keyform").Submit     
       window.focus()   
    elseIF frm1.htmlfld.value="3" then
       msgbox "此派工單已作廢，不可列印" & "  " & frm1.htmlfld1.value
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
<form name=frm1 method=post action="RTSonetCustSNDWORKPV.asp" ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
<input type="text" name="key0" style=display:none value="<%=key0%>" ID="Text0">
<input type="text" name="key1" style=display:none value="<%=key1%>" ID="Text1">
<input type="text" name="key2" style=display:none value="<%=key2%>" ID="Text2">
<input type="text" name="key3" style=display:none value="<%=key3%>" ID="Text3">
<input type="text" name="key4" style=display:none value="<%=key4%>" ID="Text4">
<input type="text" name="key5" style=display:none value="<%=key5%>" ID="Text5">
</form>
<form name=frm2 method=post action="<%=PGMNAME%>" ID="Form2">
</html>