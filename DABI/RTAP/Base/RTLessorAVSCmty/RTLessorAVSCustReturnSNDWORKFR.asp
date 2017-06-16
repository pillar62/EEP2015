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
   sqlxx="select * FROM RTLessorAVSCUST WHERE CUSID='" & KEY(0) & "' "
   sqlyy="select * FROM RTLessorAVSCustReturn WHERE CUSID='" & KEY(0) & "' and entryno=" & key(1)
   rsyy.open sqlyy,conn
   rsxx.open sqlxx,conn
   if rsxx.eof then
      '找不到客戶基本檔
      endpgm="7"
   elseif rsyy.eof then
      '找不到客戶復機資料
      endpgm="13"
 '  elseif LEN(TRIM(RSXX("DROPDAT"))) <> 0 then
      '客戶已退租,不可返轉
 '     endpgm="12"
   elseif LEN(TRIM(RSXX("cancelDAT"))) <> 0 then
      '客戶已作廢,不可返轉
      endpgm="8"
   elseif LEN(TRIM(RSyy("cancelDAT"))) <> 0 then
      '客戶復機資料已作廢,不可返轉
      endpgm="14"      
   else
      '暫存客戶可使用期數
      tempperiod=rsyy("period")
      temprcvmoney=rsyy("amt")
      temppaytype=rsyy("paytype")
      tempcardno=rsyy("CREDITCARDNO")
   end if
   rsxx.close
   rsyy.close
   '記錄客戶主檔異動檔的異動項次最大值(若派工單結案時已記錄的異動項次小於目前的最大值時，表示已經有其它異動發生，則不允許返轉。
   sqlyy="select max(entryno)as entryno from RTLessorAVSCUSTlog where CUSID='" & KEY(0)& "'"
   rsyy.open sqlyy,conn
   if rsyy.eof then
      xxmaxentryno=0
   else
      xxmaxentryno=rsyy("entryno")
   end if
   rsyy.close

'上述正確時
if endpgm="" then
   endpgm="1"
  
   sqlxx="select * FROM RTLessorAVSCustReturnsndwork WHERE CUSID='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
   RSXX.OPEN SQLXX,CONN
   BATCHNOXX=RSXX("BATCHNO")
   '檢查應收帳款檔是否已沖帳
   sqlyy="select * FROM RTLessorAVSCUSTAR WHERE CUSID='" & KEY(0) & "' AND BATCHNO='" & BATCHNOXx & "'"
   rsyy.open sqlyy,conn
   if rsyy.eof THEN
   ELSE
      if len(trim(rsyy("mdat")))>0 OR RSYY("REALAMT") > 0 then
      '應收帳款已沖帳不可結案返轉
         endpgm="4"
      end if
   end if
   RSyy.CLOSE     
   if endpgm="1" then    
   '當派工單尚未結案時，不可返轉
   IF isnull(RSXX("CLOSEDAT")) and isnull(RSXX("unCLOSEDAT"))  THEN
      ENDPGM="3"
   elseif len(trim(rsxx("dropdat"))) > 0 then
      ENDPGM="5"
   elseif xxmaxentryno > rsxx("maxentryno") then
      ENDPGM="6"      
   ELSE
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorAVSCustReturnARSndworkFR " & "'" & key(0) & "'," & key(1) & ",'" & key(2) & "','" & V(0) & "','" & batchnoxx & "'" 
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
   end if
   RSXX.CLOSE
end if
conn.Close
SET RSXX=NOTHING
SET RSYY=NOTHING
set conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS-City用戶收款派工單結案返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此派工單尚未結案，不可執行結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="4" then
       msgbox "應收帳款已沖帳，不可執行結案返轉作業(請與資訊部連繫)" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="5" then
       msgbox "此派工單已作廢，不可返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="6" then
       msgbox "客戶主檔已進行其它異動，因此無法執行派工單返轉作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                    
   elseIF frm1.htmlfld.value="7" then
       msgbox "找不到客戶基本檔，無法返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="8" then
       msgbox "客戶已退租或作廢，無法返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="12" then
       msgbox "此客戶主檔已退租，不可返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close               
    elseIF frm1.htmlfld.value="13" then
       msgbox "找不到客戶復機資料檔，無法返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="14" then
       msgbox "客戶復機資料已作廢，不可返轉。" & "  " & frm1.htmlfld1.value
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
<form name=frm1 method=post action=RTLessorAVSCustReturnsndworkf.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>