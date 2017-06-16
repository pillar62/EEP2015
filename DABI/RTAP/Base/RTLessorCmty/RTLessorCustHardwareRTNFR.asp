<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM conn
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   conn.Open DSN
   'conn.BeginTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   endpgm="1"
   sqlxx="select * FROM RTLessorCustRtnHardware WHERE rcvprtno='" & key(0) & "'"
   rsxx.open SQLXX,conn
   if rsxx.eof then
      endpgm="6"
      xxdatasrc=""
      xxprtno=""
   else
      xxdatasrc=rsxx("datasrc")
      xxprtno=rsxx("prtno")
   end if
   rsxx.close
   '依據派工資料來源，分別檢查所屬派工單狀態是否符合條件
   '(主線撤線拆機派工單)
   if xxdatasrc="01" then
      sqlxx="select * FROM RTLessorCmtyLineDropsndwork WHERE prtno='" & xxprtno & "'"
   end if
   rsxx.open sqlxx,conn
   if rsxx.eof then
      endpgm="7"
   '物品領用單所屬派工單已完工(或未完工)結案、已作廢、已轉應收帳款時，不可執行領用單結案返轉
   elseif len(trim(rsxx("dropdat")))> 0 THEN
      endpgm="5"
   ELSEIF len(trim(rsxx("closedat")))> 0 or len(trim(rsxx("unclosedat")))> 0 THEN
      endpgm="8"
   ELSEIF len(trim(rsxx("batchno")))> 0 then
      endpgm="9"
   end if
   rsxx.close
   '檢查物品移轉單狀態
if endpgm="1" then
   sqlxx="select * FROM RTLessorCustRTNHardware WHERE rcvprtno='" & key(0) & "'"
   RSXX.OPEN SQLXX,conn
  '已作廢，不可結案返轉
   IF LEN(TRIM(RSXX("CANCELdat"))) <> 0 THEN
      ENDPGM="3"
   '尚未結案，不可結案返轉
   elseIF isnull(rsxx("closedat")) then
      ENDPGM="4"
   ELSE
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorCustHardwareRTNFR " & "'" & key(0) & "','" & rsxx("datasrc") & "','" & V(0) & "'" 
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
end if
conn.Close
SET RSXX=NOTHING
set conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "物品移轉單結案返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "物品移轉單已作廢，不可結案返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "物品移轉單尚未結案，不可結案返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="5" then
       msgbox "物品移轉單所屬派工單已作廢，不可執行移轉單結案返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="6" then
       msgbox "讀取物品移轉單發生錯誤，無法執行物品移轉單結案返轉(請通知資訊部)" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="7" then
       msgbox "讀取物品移轉單所屬派工單發生錯誤，無法執行物品移轉單結案返轉(請通知資訊部)" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="8" then
       msgbox "物品移轉單所屬派工單已完工結案或未完工結案，不可執行移轉單結案返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                
    elseIF frm1.htmlfld.value="9" then
       msgbox "物品移轉所屬派工單已轉應收帳款，不可執行移轉單結案返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                
    else
       msgbox "無法執行物品移轉單結案返轉作業(請通知資訊部)" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorCustHardwareRTNFR.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>