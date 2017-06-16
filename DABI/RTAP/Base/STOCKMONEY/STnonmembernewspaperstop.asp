<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=STOCK;uid=sa;pwd=alittlecat@cbn"
   connXX.Open DSN
   endpgm="1"
 '  On Error Resume Next

   sqlxx="select * FROM Nonmembernewspaper WHERE EMAIL='" & KEY(0) & "' and NEWSPAPERKIND='" & key(1) & "' and NEWSPAPERCODE='" & key(2) & "'"
   rsxx.Open sqlxx,connxx
   CLOSEFLAG=rsxx("CLOSEFLAG")
   RSXX.CLOSE
   if CLOSEFLAG = "Y" then
     endpgm="3"
   else
      SQLXX=" update Nonmembernewspaper set CLOSEFLAG='Y' where EMAIL='" & KEY(0) & "' and NEWSPAPERKIND='" & key(1) & "' and NEWSPAPERCODE='" & key(2) & "'"
      connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            errmsg=cstr(Err.number) & "=" & Err.description
         else
            endpgm="1"
            errmsg=""
         end if      
   END IF
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "非會員電子報訂閱資料強制關閉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此非會員電子報訂閱資料已關閉，不可重複關閉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    else
       msgbox "無法執行非會員電子報訂閱資料強制關閉,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=NONMEMBERNEWSPAPERSTOP.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>