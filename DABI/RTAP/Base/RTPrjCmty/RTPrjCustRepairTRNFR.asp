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
   DSN="DSN=RtLib"
   conn.Open DSN

   ' conn.BEGINTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   '因為當STORE PROCEDURE內OPEN太多TABLE時，ASP無法控制CURSOR而會發生錯誤(不明瞭可以將BEGIN、COMMIT、ROLLBACK的MARK移除並執行後即知)
   '記錄客戶主檔異動檔的異動項次最大值(若派工單結案時已記錄的異動項次小於目前的最大值時，表示已經有其它異動發生，則不允許返轉。
   'sqlyy="select max(entryno)as entryno from RTPrjCUSTlog where CUSID='" & KEY(0)& "'"
   'rsyy.open sqlyy,conn
   'if rsyy.eof then
   '   xxmaxentryno=0
   'else
   '   xxmaxentryno=rsyy("entryno")
   'end if
   'rsyy.close

   '條件檢查
   sqlxx="select * FROM RTPrjCustRepair WHERE CUSID='" & KEY(0) & "' and entryno=" & key(1) & " "
   sqlYY="select * FROM RTPrjCUST WHERE  CUSID='" & KEY(0) & "' "
   rsyy.open sqlyy,conn
   RSXX.OPEN SQLXX,conn
     endpgm=""
     '當客戶維修收款資料已作廢時，不可執行返轉應收結案作業
     IF not isnull(RSXX("canceldat")) THEN
        ENDPGM="6"
     '當客戶資料作廢時，不可執行返轉應收結案作業
     ELSEIF not isnull(RSYY("canceldat")) THEN
        ENDPGM="9"        
     '當客戶已退租時，不可執行返轉應收結案作業
     ELSEIF not isnull(RSYY("DROPdat")) THEN
        ENDPGM="10"           
    'batchno為空白或結案日為空白時，表示此筆維修收款資料尚未轉應收帳款，不可執行返轉作業
     elseif LEN(TRIM(RSXX("batchno"))) = 0 OR isnull(RSXX("FINISHDAT")) then
        endpgm="3"
     ELSE
        BATCHNOXX=RSXX("BATCHNO")
     END IF
     RSXX.CLOSE
     RSYY.CLOSE
     
   '檢查應收帳款檔是否已沖帳
   sqlyy="select * FROM RTPrjCUSTAR WHERE CUSID='" & KEY(0) & "' AND BATCHNO='" & BATCHNOXx & "'"
   rsyy.open sqlyy,conn
   if rsyy.eof THEN
   ELSE
      if len(trim(rsyy("mdat")))>0 OR RSYY("REALAMT") > 0 then
      '應收帳款已沖帳不可結案返轉
         endpgm="4"
      end if
   end if
   RSyy.CLOSE     
   
   if endpgm="" then
      '呼叫store procedure更新相關檔案
      strSP="usp_RTPrjCustRepairAR " & "'" & key(0) & "'" & "," & key(1) & ",'" & V(0) & "', 'MR', '" & BATCHNOXX & "'"
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
       msgbox "專案社區用戶維修收款返轉應收結案作業成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
   elseIF frm1.htmlfld.value="3" then
       msgbox "此筆維修收款資料尚未轉應收帳款，不可執行返轉應收結案作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
   elseIF frm1.htmlfld.value="4" then
       msgbox "應收帳款已沖帳，不可執行返轉應收結案作業。(須返轉沖帳資料)" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
    elseIF frm1.htmlfld.value="6" then
       msgbox "維修收款資料已作廢時，不可執行返轉應收結案作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
    elseIF frm1.htmlfld.value="9" then
       msgbox "客戶資料已作廢，不可執行返轉應收結案作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
    elseIF frm1.htmlfld.value="10" then
       msgbox "客戶資料已退租，不可執行返轉應收結案作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
    else
       msgbox "無法執行用戶維修收款返轉應收結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
    end if
    winP.focus()              
    window.CLOSE
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTPrjCustRepairTRNFR.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>