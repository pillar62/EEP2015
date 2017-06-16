<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM Conn
   Set Conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSYY=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   Conn.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTLessorAVSCmtylinedrop WHERE comq1=" & KEY(0) & " AND lineq1=" & KEY(1) & " and entryno=" & key(2)
   sqlYY="select * FROM RTLessorAVSCmtyline WHERE comq1=" & KEY(0) & " AND lineq1=" & KEY(1) 
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,Conn
   RSYY.OPEN SQLYY,Conn
   endpgm="1"
   '當撤線資料已作廢時，不可結案
   IF LEN(TRIM(RSXX("CANCELdat"))) <> 0 THEN
      ENDPGM="3"
   '當撤線資料已結案時，不可重覆結案
   elseif LEN(TRIM(RSXX("CLOSEDAT"))) <> 0 then
      endpgm="4"      
   '當撤線資料尚未產生拆機派工單時，不可結案
   elseif LEN(TRIM(RSXX("SNDPRTNO"))) = 0 OR ISNULL(RSXX("SNDWORKDAT")) then
      endpgm="5"
   '當撤線資料之拆機派工單尚未結案時，不可結案
   elseif LEN(TRIM(RSXX("SNDPRTNO"))) <> 0 AND ISNULL(RSXX("SNDCLOSEDAT")) then
      endpgm="6"      
   ELSE
     '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorAVSCmtylineDropClose "  & key(0) & "," & key(1) & "," & key(2) & ",'" & V(0) & "'" 
      Set ObjRS = conn.Execute(strSP)
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
   Conn.Close
   SET RSXX=NOTHING
   SET RSYY=NOTHING
   set Conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS-City主線撤線完工結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當撤線資料已作廢時，不可結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "當撤線資料已結案時，不可重覆結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="5" then
       msgbox "當撤線資料尚未產生拆機派工單時，不可結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="6" then
       msgbox "當撤線資料之拆機派工單尚未結案時，不可結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                    
    else
       msgbox "無法執行主線撤線完工結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorAVScmtylinedropclose.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>