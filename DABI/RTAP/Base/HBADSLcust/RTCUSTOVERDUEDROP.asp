<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% dim parmkey,aryparmkey,logonid,conn,rs,sql
   parmKey=Request("Key")
   aryParmKey=Split(parmKey &";;;;;;;;;;;;;;;",";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   '檢查本退租資料是否已結案或月結
   DSN="DSN=RtLib"
   Set conn=Server.CreateObject("ADODB.Connection")  
   Set rs=server.CreateObject("ADODB.Recordset")
   DSN="DSN=RtLib"
   sql="select * from HBCUSTDROP where HBCustDrop.serno=" & ARYPARMKEY(0) 
   conn.Open DSN
  ' RESPONSE.Write SQL
   rs.Open sql,conn,3,3
   If rs("prtno") <>"" then   
      rs.close
      set rs=nothing
      endpgm="2"      
   Elseif rs("status") <> "欠拆" then   
      rs.close
      set rs=nothing
      endpgm="3"            
   Elseif rs("OVERDUEDROP") = "Y" then   
      rs.close
      set rs=nothing
      endpgm="4"            
   ELSE
      rs("OVERDUEDROP")="Y"
      rs("OVERDUETNSDAT")=DATEVALUE(NOW)
      rs.update
      rs.close
      set rs=nothing
      endpgm="1"
   End if
   conn.Close

%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "欠拆轉退租作業完成",0
       Set winP=window.Opener
       Set docP=winP.document
       docP.all("keyform").Submit
       winP.focus()              
    elseif frm1.htmlfld.value="2" then 
        msgbox "本資料已列印(有拆機單號),不可異動" & "  " & errmsg
    elseif frm1.htmlfld.value="3" then 
        msgbox "非欠拆戶不可執行本項作業" & "  " & errmsg        
    else
       msgbox "已執行轉退租作業，不須重覆執行" & "  " & errmsg
    end if
    window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action="RTFaqDropK.asp">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
</html>