<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONN
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   conn.Open DSN
   'conn.BeginTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   '因為當STORE PROCEDURE內OPEN太多TABLE時，ASP無法控制CURSOR而會發生錯誤(不明瞭可以將BEGIN、COMMIT、ROLLBACK的MARK移除並執行後即知)
   '檢查客戶資料
   sqlxx="select * FROM RTLessorCMTYLINE WHERE COMQ1=" & KEY(0) & " AND LINEQ1=" & KEY(1)
   sqlyy="select * FROM RTLessorCmtyLineFAQH WHERE COMQ1=" & KEY(0) & " AND LINEQ1=" & KEY(1) & " and FAQNO='" & key(2) & "' "
  ' response.write sqlyy
   rsyy.open sqlyy,conn
   rsxx.open sqlxx,conn
   if rsxx.eof then
      '找不到客戶基本檔
      endpgm="7"
   elseif rsyy.eof then
      '找不到此維修派工單所屬客服單資料
      endpgm="10"
   elseif LEN(TRIM(RSXX("cancelDAT"))) <> 0 then
      '主線已作廢
      endpgm="8"
   elseif LEN(TRIM(RSXX("dropdat"))) <> 0 then
      '主線已撤線
      endpgm="15"      
  elseif LEN(TRIM(RSyy("cancelDAT"))) <> 0 then
      '主線客訴單資料已作廢
      endpgm="11"      
   elseif len(trim(rsyy("SNDCLOSEDAT"))) > 0 then
      '此維修派工單所屬客服單已有派工單結案日，請連絡資訊部
      endpgm="12"            
   end if
   rsxx.close
   rsyy.close
   '檢查該派工單下的設備是否皆已辦妥物品領用程序
   sqlxx="select count(*) as CNT FROM RTLessorCMTYLINEFAQHardware WHERE COMQ1=" & KEY(0) & " AND LINEQ1=" & KEY(1) & " AND faqno='" & key(2) & "' and prtno='" & key(3) & "' and dropdat is null and rcvfinishdat is null "
   'response.write sqlxx
   'response.end
   RSXX.OPEN SQLXX,CONN
   IF RSXX.EOF THEN
   ELSEIF RSXX("CNT") > 0 THEN
      ENDPGM="9"
   END IF
   RSXX.CLOSE

'上述正確時
if endpgm="" then
   endpgm="1"
  
   sqlxx="select * FROM RTLessorCmtylineFAQsndwork WHERE COMQ1=" & KEY(0) & " AND LINEQ1=" & KEY(1) & " AND faqno='" & key(2) & "' and prtno='" & key(3) & "' "
   sqlyy="select count(*) as cnt FROM RTLessorCmtylineFaqSndworkFixCode WHERE COMQ1=" & KEY(0) & " AND LINEQ1=" & KEY(1) & " AND faqno='" & key(2) & "' and prtno='" & key(3) & "' "
   RSXX.OPEN SQLXX,CONN
   RSyy.OPEN SQLyy,CONN
   '當已作廢時，不可執行完工結案或未完工結案
   IF LEN(TRIM(RSXX("DROPDAT"))) <> 0 THEN
      ENDPGM="3"
   elseif LEN(TRIM(RSXX("CLOSEDAT"))) <> 0 OR LEN(TRIM(RSXX("UNCLOSEDAT"))) <> 0 then
      endpgm="4"
   elseif LEN(TRIM(RSXX("REALENGINEER"))) = 0 AND LEN(TRIM(RSXX("REALCONSIGNEE"))) = 0 then
      endpgm="6"
   elseif LEN(TRIM(RSXX("BONUSCLOSEYM"))) <> 0 OR LEN(TRIM(RSXX("STOCKCLOSEYM"))) <> 0 then
      endpgm="5"
   elseif LEN(TRIM(RSXX("BATCHNO"))) <> 0  then
      endpgm="13"      
   elseif rsyy("cnt") < 1 then
      endpgm="14"            
   ELSE
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorCmtyLineFAQSndworkF " & key(0) & "," & key(1) & ",'" & key(2) & "','" & key(3) & "','" & V(0) & "'"
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
   RSyy.CLOSE
end if
conn.Close
SET RSXX=NOTHING
SET RSyy=NOTHING
set conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "ET-City主線維修派工單完工結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當已作廢時，不可執行完工結案或未完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此維修派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此維修派工單已月結，不可異動" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    elseIF frm1.htmlfld.value="6" then
       msgbox "此維修派工單完工時，必須先輸入實際裝機人員或實際裝機經銷商" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    elseIF frm1.htmlfld.value="7" then
       msgbox "找不到主線基本檔，無法結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="8" then
       msgbox "主線已作廢，無法結案(派工單必須作廢)。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="15" then
       msgbox "主線已撤線，無法結案(派工單必須作廢)。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                 
    elseIF frm1.htmlfld.value="9" then
       msgbox "此維修派工單設備資料中，尚有設備未辦妥物品領用程序(未領用或領用未結案)，不可執行完工結案作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="10" then
       msgbox "找不到此維修派工單所屬客服單資料" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                 
    elseIF frm1.htmlfld.value="11" then
       msgbox "此維修派工單所屬客服單資料已作廢，不可執行完工結案作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close               
    elseIF frm1.htmlfld.value="12" then
       msgbox "此維修派工單所屬客服單已有派工單結案日，請連絡資訊部" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                    
    elseIF frm1.htmlfld.value="13" then
       msgbox "此維修派工單已產生應收帳款，無法重複結案，請連絡資訊部" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="14" then
       msgbox "維修派工單結案時，必須勾選維修處理過程說明或輸入處理方式" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                                  
    else
       msgbox "無法執行維修派工單完工結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorCmtylineFAQsndworkf.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>