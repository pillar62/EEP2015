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
   sqlYY="select COUNT(*) AS CNT FROM RTLessorAVSCmtylinedrop WHERE comq1=" & KEY(0) & " AND lineq1=" & KEY(1) & " and entryno<>" & key(2) & " AND CANCELDAT IS NULL "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,Conn
   RSYY.OPEN SQLYY,Conn
   endpgm="1"
   '當尚未作廢時，不可作廢返轉
   IF isnull(RSXX("canceldat")) THEN
      ENDPGM="3"
   '若存在其它撤線資料且該資料未作廢時，不可進行作廢返轉(否則會同時存在兩筆)
   ELSEIF RSYY("CNT") > 0 then
      endpgm="4"
   ELSE
     '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorAVSCmtylineDropCancelRTN "  & key(0) & "," & key(1) & "," & key(2) & ",'" & V(0) & "'" 
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
   RSyy.CLOSE
   Conn.Close
   SET RSXX=NOTHING
   SET RSyy=NOTHING
   set Conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS-City主線撤線資料作廢返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此主線撤線資料尚未作廢，不可返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    else
       msgbox "無法執行主線撤線資料作廢返轉作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorAVSCMTYLINEdropCANCELRTN.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>