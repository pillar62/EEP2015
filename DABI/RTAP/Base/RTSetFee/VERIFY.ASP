<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
   logonid=session("userid")
   SqlStr=Session("SQLSTRREVPRT")
   ExistPRTNO=Session("ExistPrtNo")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   dim ObjRS,strSP,endpgm
   strSP="USP_SetPrtNo 'B'" & ",'" & V(0) & "','" & SQLSTR & "'"
   Set conn=Server.CreateObject("ADODB.Connection")  
   DSN="DSN=RtLib"
   conn.Open DSN
   On Error Resume Next
   Set ObjRS = conn.Execute(strSP)  
   If ExistPrtNo ="" then   
      PRTNO=objRS("RCVDTLNO") 
   Else
      PRTNO=ExistPrtNo
   End if
   conn.Close
   If Err.number > 0 then
      endpgm="2"
      errmsg=cstr(Err.number) & "=" & Err.description
   else
      endpgm="1"
      errmsg=""
      session("SetFeePrtNo")= PRTNO 
   end if
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "RT施工費支付明細表列印完成",0
       Set winP=window.Opener
       Set docP=winP.document
       docP.all("keyform").Submit
       winP.focus()              
       window.frm2.submit
    else
       msgbox "無法列印,錯誤訊息：" & "  " & frm1.htmlfld1.value
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
<form name=frm2 method=post action=RTSetFeePrtV.asp>
</form>
</html>