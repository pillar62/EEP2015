<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONN
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSYY=Server.CreateObject("ADODB.RECORDSET") 
   SET RSZZ=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   conn.Open DSN
   'conn.BeginTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   '因為當STORE PROCEDURE內OPEN太多TABLE時，ASP無法控制CURSOR而會發生錯誤(不明瞭可以將BEGIN、COMMIT、ROLLBACK的MARK移除並執行後即知)
 
   sqlxx="select * FROM RTLessorAVSCMTYLINEdrop WHERE COMQ1=" & KEY(0) & " AND LINEQ1=" & KEY(1) & " and entryno=" & key(2)
   sqlYY="select COUNT(*) AS CNT FROM RTLessorAVSCMTYLINEHardware WHERE COMQ1=" & KEY(0) & " AND LINEQ1=" & KEY(1) & " and dropdat is null and rcvfinishdat is not null and qty > 0 "
   sqlzz="select COUNT(*) AS CNT FROM RTLessorAVSCMTYLINEfaqHardware WHERE COMQ1=" & KEY(0) & " AND LINEQ1=" & KEY(1) & " and dropdat is null and rcvfinishdat is not null and qty > 0 "
   RSXX.OPEN SQLXX,CONN
   RSYY.OPEN SQLYY,CONN
   RSZZ.OPEN SQLZZ,CONN
   '當已作廢時，不可轉派工單
   IF LEN(TRIM(RSXX("CANCELDAT"))) <> 0 THEN
      ENDPGM="3"
   '已結案時，不可轉派工單
   elseif LEN(TRIM(RSXX("closedat"))) <> 0 then
      endpgm="4"
   '已有派工單時，不可重覆執行
   elseif LEN(TRIM(RSXX("SNDPRTNO"))) <> 0  then
      endpgm="5"
   '若無設備，則不可執行轉拆機派工單
 '  elseif rsyy("cnt")=0 and rszz("cnt")=0  then
 '     endpgm="6"      
   ELSE
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorAVSCmtyLineDropTRNSndwork "  & key(0) & "," & key(1) & "," & key(2) & ",'" & V(0) & "'" 
    '  response.write strSP
    '  response.end     
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
   RSZZ.CLOSE
conn.Close
SET RSXX=NOTHING
SET RSYY=NOTHING
SET RSZZ=NOTHING
set conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS-City主線撤線單轉拆機派工單成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當已作廢時，不可轉拆機派工單" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "已結案時，不可轉拆機派工單" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "已有拆機派工單時，不可重覆執行" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    elseIF frm1.htmlfld.value="6" then
       msgbox "此主線無任何設備資料，不可產生拆機派工單" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close             
    else
       msgbox "無法執行撤線單轉拆機派工單作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorAVScmtylinedropTRNSndwork.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>