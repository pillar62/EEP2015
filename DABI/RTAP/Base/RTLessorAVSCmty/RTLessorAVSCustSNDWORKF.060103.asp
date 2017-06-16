<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONN
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   conn.Open DSN
   'conn.BeginTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   '因為當STORE PROCEDURE內OPEN太多TABLE時，ASP無法控制CURSOR而會發生錯誤(不明瞭可以將BEGIN、COMMIT、ROLLBACK的MARK移除並執行後即知)
   '檢查客戶資料
   sqlxx="select * FROM RTLessorAVSCUST WHERE CUSID='" & KEY(0) & "' "
   rsxx.open sqlxx,conn
   if rsxx.eof then
      '找不到客戶基本檔
      endpgm="7"
   elseif LEN(TRIM(RSXX("DROPDAT"))) <> 0 or LEN(TRIM(RSXX("cancelDAT"))) <> 0 then
      '客戶已退租或作廢
      endpgm="8"
   elseif rsxx("rcvmoney")=0 then
      '應收金額=0 (無法轉應收帳款)
      endpgm="9"
   elseif len(trim(rsxx("batchno"))) > 0 then
      '己產生應收帳款
      endpgm="10"      
   elseif len(trim(rsxx("finishdat"))) > 0 then
      '此客戶已完工結案，不可重複執行
      endpgm="14"            
   else
      '暫存客戶可使用期數
      tempperiod=rsxx("period")
      temprcvmoney=rsxx("rcvmoney")
      temppaytype=rsxx("paytype")
      tempcardno=rsxx("CREDITCARDNO")
   end if
   rsxx.close
   '檢查該派工單下的設備是否皆已辦妥物品領用程序
   sqlxx="select count(*) as CNT FROM RTLessorAVSCUSTHardware WHERE CUSID='" & KEY(0) & "' and prtno='" & key(1) & "' and dropdat is null and rcvfinishdat is null "
   RSXX.OPEN SQLXX,CONN
   IF RSXX.EOF THEN
   ELSEIF RSXX("CNT") > 0 THEN
      ENDPGM="12"
   END IF
   RSXX.CLOSE
   '檢查該派工單下的設備是否皆已辦妥物品領用程序
   sqlxx="select count(*) as CNT FROM RTLessorAVSCUSTHardware WHERE CUSID='" & KEY(0) & "' and prtno='" & key(1) & "' and dropdat is null "
   RSXX.OPEN SQLXX,CONN
   IF RSXX.EOF OR RSXX("CNT") < 1 THEN
      ENDPGM="13"
   END IF
   RSXX.CLOSE   
'上述正確時
if endpgm="" then
   endpgm="1"
  
   sqlxx="select * FROM RTLessorAVSCUSTsndwork WHERE CUSID='" & KEY(0) & "' and prtno='" & key(1) & "' "
   RSXX.OPEN SQLXX,CONN
   '當已作廢時，不可執行完工結案或未完工結案
   IF LEN(TRIM(RSXX("DROPDAT"))) <> 0 THEN
      ENDPGM="3"
   elseif LEN(TRIM(RSXX("CLOSEDAT"))) <> 0 OR LEN(TRIM(RSXX("UNCLOSEDAT"))) <> 0 then
      endpgm="4"
   elseif LEN(TRIM(RSXX("REALENGINEER"))) = 0 AND LEN(TRIM(RSXX("REALCONSIGNEE"))) = 0 then
      endpgm="6"
   '用戶網卡MAC不可空白
   elseif LEN(TRIM(RSXX("MAC"))) = 0 then
      endpgm="15"      
 '用戶網卡MAC長度為17位    
   elseif LEN(TRIM(RSXX("MAC"))) <> 17 then
      endpgm="16"       
 '用戶上層接續設備代號不可空白  
   elseif LEN(TRIM(RSXX("HOSTNO"))) = 0 then
      endpgm="17"     
 '用戶上層接續設備PORT號不可空白  
   elseif LEN(TRIM(RSXX("HOSTPORT"))) = 0 then
      endpgm="18"                
   elseif LEN(TRIM(RSXX("BONUSCLOSEYM"))) <> 0 OR LEN(TRIM(RSXX("STOCKCLOSEYM"))) <> 0 then
      endpgm="5"
   elseif LEN(TRIM(RSXX("BATCHNO"))) <> 0  then
      endpgm="11"      
   ELSE
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorAVSCustARSndworkF " & "'" & key(0) & "'" & ",'" & key(1) & "','" & V(0) & "'" & "," & tempperiod & "," & temprcvmoney & ",'" & temppaytype & "','" & tempcardno & "'"
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
       msgbox "AVS-City用戶裝機派工單完工結案成功",0
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
       msgbox "此裝機派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此裝機派工單已月結，不可異動" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    elseIF frm1.htmlfld.value="6" then
       msgbox "此裝機派工單完工時，必須先輸入實際裝機人員或實際裝機經銷商" & "  " & frm1.htmlfld1.value
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
       msgbox "此用戶申裝資料已產生應收帳款檔，不可重複執行(請洽資訊部)。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                 
    elseIF frm1.htmlfld.value="11" then
       msgbox "此裝機派工單已產生應收帳款檔，不可重複執行(請洽資訊部)。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                   
    elseIF frm1.htmlfld.value="12" then
       msgbox "此裝機派工單設備資料中，尚有設備未辦妥物品領用程序，不可執行完工結案作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="13" then
       msgbox "此裝機派工單未建立用戶設備資料，不可執行完工結案作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                    
    elseIF frm1.htmlfld.value="14" then
       msgbox "客戶主檔已完工結案，不可重複執行結案作業(請聯絡資訊部)。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close             
    elseIF frm1.htmlfld.value="15" then
       msgbox "裝機結案時，用戶網卡MAC不可空白。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="16" then
       msgbox "用戶網卡MAC長度必須為20位，請確認。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close         
    elseIF frm1.htmlfld.value="17" then
       msgbox "裝機結案時，用戶上層接續設備代號不可空白" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close             
    elseIF frm1.htmlfld.value="18" then
       msgbox "裝機結案時，用戶上層接續設備PORT號不可空白" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                               
    else
       msgbox "無法執行裝機派工單完工結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorAVScustsndworkf.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>