<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONN
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RS=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   conn.Open DSN
   endpgm="1"
 '  On Error Resume Next

   sql="select * FROM RTSparq0809Cust WHERE cusid='" & KEY(0) & "' "  
   rs.Open sql,conn
   DROPDAT=rs("DROPDAT")
   CANCELDAT=rs("CANCELDAT")
   RS.CLOSE
   if LEN(TRIM(CANCELDAT)) > 0 then
     endpgm="2"
   ELSEif LEN(TRIM(DROPDAT)) > 0 then
     endpgm="3"
   else
     endpgm="1"
     sql="update RTSparq0809Cust set canceldat=getdate() where cusid='" & key(0) & "' "
     conn.Execute sql
   end if
   conn.Close
   SET RS=NOTHING
   set conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "用戶資料作廢成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="2" then
       msgbox "此用戶已作廢，不可重覆作廢" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="3" then
       msgbox "此用戶已退租，不可作廢" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    else
       msgbox "無法執行用戶資料作廢,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcmtyhardwaredrop.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>