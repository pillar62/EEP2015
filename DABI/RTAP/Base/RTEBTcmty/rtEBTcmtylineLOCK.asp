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
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
    endpgm="1"
    sqlxx="select * FROM RTEBTCMTYLINE WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) 
    RSXX.OPEN SQLXX,CONNxx
   '當主線已被鎖定，不可重覆執行
    IF LEN(TRIM(RSXX("LOCKDAT"))) <> 0 THEN
        ENDPGM="3"
    '此主線尚未測通，不可執行鎖定動作
    elseif ISNULL(RSXX("ADSLapplyDAT")) then
      endpgm="4"
    ELSE
      SQLXX=" update RTEBTCMTYline set LOCKDAT=getdate() where comq1=" & KEY(0) & " and lineq1=" & key(1) 
      connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
         else
            endpgm="1"
            errmsg=""
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
       msgbox "主線鎖定作業完成",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "主線已鎖定，不須重覆執行" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "主線尚未測通，不須執行鎖定作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    else
       msgbox "無法執行主線鎖定作業,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcmtylineclrprtno.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>