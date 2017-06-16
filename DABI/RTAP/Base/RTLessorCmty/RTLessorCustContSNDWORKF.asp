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
   sqlxx="select * FROM RTLessorCUST WHERE CUSID='" & KEY(0) & "' "
   sqlyy="select * FROM RTLessorCUSTcont WHERE  CUSID='" & KEY(0) & "' and entryno=" & key(1)
  ' response.write sqlyy
   rsyy.open sqlyy,conn
   rsxx.open sqlxx,conn
   if rsxx.eof then
      '找不到客戶基本檔
      endpgm="7"
   elseif rsyy.eof then
      '找不到客戶續約資料
      endpgm="13"
   elseif LEN(TRIM(RSXX("DROPDAT"))) <> 0 then
      '客戶已退租(必須採復機作業)
      endpgm="12"
   elseif LEN(TRIM(RSXX("cancelDAT"))) <> 0 then
      '客戶已作廢
      endpgm="8"
   elseif isnull(RSyy("strbillingDAT")) <> 0 then
      '續約資料開始計費日，不可空白
      endpgm="15"            
   elseif LEN(TRIM(RSyy("cancelDAT"))) <> 0 then
      '客戶續約資料已作廢
      endpgm="14"      
   elseif rsyy("amt")=0 then
      '續約內容之應收金額=0 (無法轉應收帳款)
      endpgm="9"
   elseif len(trim(rsyy("batchno"))) > 0 then
      '續約資料己產生應收帳款
      endpgm="10"      
   elseif len(trim(rsyy("finishdat"))) > 0 then
      '續約資料已結案，不可重複結案(請聯絡資訊部)
      endpgm="17"            
   else
      '暫存客戶可使用期數
      tempperiod=rsyy("period")
      temprcvmoney=rsyy("amt")
      temppaytype=rsyy("paytype")
      tempcardno=rsyy("CREDITCARDNO")
   end if
   rsxx.close
   rsyy.close
   '檢查該派工單下的設備是否皆已辦妥物品領用程序
   sqlxx="select count(*) as CNT FROM RTLessorCUSTCONTHardware WHERE CUSID='" & KEY(0) & "' AND entryno=" & key(1) & " and prtno='" & key(2) & "' and dropdat is null and rcvfinishdat is null "
   'response.write sqlxx
   'response.end
   RSXX.OPEN SQLXX,CONN
   IF RSXX.EOF THEN
   ELSEIF RSXX("CNT") > 0 THEN
      ENDPGM="16"
   END IF
   RSXX.CLOSE

'上述正確時
if endpgm="" then
   endpgm="1"
  
   sqlxx="select * FROM RTLessorCUSTContsndwork WHERE CUSID='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
   RSXX.OPEN SQLXX,CONN
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
      endpgm="11"      
   ELSE
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorCustContARSndworkF " & "'" & key(0) & "'" & "," & key(1) & ",'" & key(2) & "','" & V(0) & "'" & "," & tempperiod & "," & temprcvmoney & ",'" & temppaytype & "','" & tempcardno & "'"
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
       msgbox "ET-City用戶收款派工單完工結案成功",0
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
       msgbox "此收款派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此收款派工單已月結，不可異動" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    elseIF frm1.htmlfld.value="6" then
       msgbox "此收款派工單完工時，必須先輸入實際裝機人員或實際裝機經銷商" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    elseIF frm1.htmlfld.value="7" then
       msgbox "找不到客戶基本檔，無法結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="8" then
       msgbox "客戶已退租或作廢，無法結案(派工單必須作廢)。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="9" then
       msgbox "用戶基本檔應收金額為零，無法產生應收帳款資料，請確認。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="10" then
       msgbox "此用戶續約資料已產生應收帳款檔，不可重複執行(請洽資訊部)。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                 
    elseIF frm1.htmlfld.value="11" then
       msgbox "此收款派工單已產生應收帳款檔，不可重複執行(請洽資訊部)。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                
    elseIF frm1.htmlfld.value="12" then
       msgbox "此客戶主檔已退租，必須改採復機作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close               
    elseIF frm1.htmlfld.value="13" then
       msgbox "找不到客戶續約資料檔，無法結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="14" then
       msgbox "客戶續約資料已作廢，不可結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                   
    elseIF frm1.htmlfld.value="15" then
       msgbox "執行結案作業時，續約資料中[開始計費日]不可空白。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
    elseIF frm1.htmlfld.value="16" then
       msgbox "此收款派工單設備資料中，尚有設備未辦妥物品領用程序(未領用或領用未結案)，不可執行完工結案作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                          
    elseIF frm1.htmlfld.value="17" then
       msgbox "客戶續約主檔已結案，不可重複執行結案作業(請聯絡資訊部)。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                    
    else
       msgbox "無法執行收款派工單完工結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorcustContsndworkf.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>