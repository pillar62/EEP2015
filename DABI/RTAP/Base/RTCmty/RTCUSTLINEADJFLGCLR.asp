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
   sqlxx="UPDATE RTCUST SET CUSTLINEADJFLG=''  FROM RTCUST WHERE rtcust.COMQ1=" & KEY(0) & " AND rtcust.CUSID='" & KEY(1) & "' and rtcust.ENTRYNO=" & key(2) & " "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   CONNXX.Execute SQLXX
   endpgm="1"
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       Set winP=window.Opener
       Set docP=winP.document       
       winP.focus()           
       docP.all("keyform").Submit     
       WINDOW.CLOSE
    else
       msgbox "無法執行清除主線調整狀態作業,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action="RTEBTCUSTLINEADJFLGCLR.asp" ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
<input type="text" name="key1" style=display:none value="<%=key1%>" ID="Text3">
<input type="text" name="key2" style=display:none value="<%=key2%>" ID="Text4">
<input type="text" name="key3" style=display:none value="<%=key3%>" ID="Text5">
</form>
</html>
