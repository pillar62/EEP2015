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
   sqlxx="select * FROM RTLessorAVSCMTYLINEcont WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and entryno=" & key(2)
   sqlYY="select * FROM RTLessorAVSCMTYLINE WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) 
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   RSYY.OPEN SQLYY,CONNxx
   endpgm="1"
   '線路續約已作廢，不可結案
   IF LEN(TRIM(RSXX("CANCELDAT"))) <> 0   THEN
      ENDPGM="3"
   '續約資料已結案，不可重複結案
   elseif LEN(TRIM(RSXX("CLOSEDAT"))) <> 0 then
      endpgm="4"
   '續約資料無測通日，不可結案
   elseif ISNULL(RSXX("ADSLAPPLYDAT")) then
      endpgm="5"      
   '續約資料無到期日，不可結案
   elseif ISNULL(RSXX("LINEDUEDAT")) then
      endpgm="6"           
  '此主線之主檔資料已撤線，不可執行續約結案作業
   elseif LEN(TRIM(RSYY("DROPDAT"))) <> 0 then
      endpgm="7"              
  '此主線之主檔資料已作廢，不可執行續約結案作業
   elseif LEN(TRIM(RSYY("CANCELDAT"))) <> 0 then
      endpgm="8"                
  '此主線之主檔資料尚未測通，不可執行續約結案作業
   elseif ISNULL(RSYY("ADSLAPPLYDAT")) then
      endpgm="9"                  
   ELSE
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorAVSCMTYLINEcontCLOSE " & key(0) & "," & key(1) & "," & key(2) & ",'" & V(0) & "'" 
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
   RSYY.CLOSE
   connXX.Close
   SET RSXX=NOTHING
   SET RSYY=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS-City主線續約結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "線路續約已作廢，不可結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "續約資料已結案，不可重複結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="5" then
       msgbox "續約資料無測通日，不可結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close        
   elseIF frm1.htmlfld.value="6" then
       msgbox "續約資料無到期日，不可結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
   elseIF frm1.htmlfld.value="7" then
       msgbox "此主線之主檔資料已撤線，不可執行續約結案作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
   elseIF frm1.htmlfld.value="8" then
       msgbox "此主線之主檔資料已作廢，不可執行續約結案作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
   elseIF frm1.htmlfld.value="9" then
       msgbox "此主線之主檔資料尚未測通，不可執行續約結案作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                                  
    else
       msgbox "無法執行主線續約結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorAVSCMTYLINEcontCLOSE.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>