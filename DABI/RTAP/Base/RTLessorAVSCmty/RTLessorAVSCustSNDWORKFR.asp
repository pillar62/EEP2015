<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM conn
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   conn.Open DSN
   ' conn.BEGINTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   '因為當STORE PROCEDURE內OPEN太多TABLE時，ASP無法控制CURSOR而會發生錯誤(不明瞭可以將BEGIN、COMMIT、ROLLBACK的MARK移除並執行後即知)
   sqlxx="select * FROM RTLessorAVSCUSTsndwork WHERE CUSID='" & KEY(0) & "' and prtno='" & key(1) & "' "
   sqlYY="select * FROM RTLessorAVSCUST WHERE CUSID='" & KEY(0) & "' "
   RSYY.Open SQLYY,conn
   ENDPGM=""
   IF RSYY.EOF THEN
      ENDPGM="6"
   ELSE
      BATCHNOX=RSYY("BATCHNO")
  '    IF LEN(TRIM(RSYY("strbillingDAT"))) > 0 or LEN(TRIM(RSYY("newbillingDAT"))) > 0 THEN ENDPGM="7"
   END IF
   RSYY.CLOSE
   '檢查應收帳款檔是否已沖帳
   sqlyy="select * FROM RTLessorAVSCUSTAR WHERE CUSID='" & KEY(0) & "' AND BATCHNO='" & BATCHNOX & "'"
   rsyy.open sqlyy,conn
   if rsyy.eof THEN
   ELSE
      if len(trim(rsyy("mdat")))>0 OR RSYY("REALAMT") > 0 then
      '應收帳款已沖帳不可結案返轉
         endpgm="8"
      end if
   end if
   RSyy.CLOSE   
   
   IF ENDPGM="" THEN
     RSXX.OPEN SQLXX,conn
     endpgm="1"
     '暫存BATCHNO
     BATCHNOXX=RSXX("BATCHNO")
     '當已作廢時，不可執行完工結案或未完工結案
     'IF2S
     IF not isnull(RSXX("DROPDAT")) THEN
        ENDPGM="4"
     elseif LEN(TRIM(RSXX("BONUSCLOSEYM"))) <> 0 OR LEN(TRIM(RSXX("STOCKCLOSEYM"))) <> 0 then
        endpgm="5"
     elseif isnull(RSXX("CLOSEDAT")) and isnull(RSXX("UNCLOSEDAT")) then
        endpgm="3"      
     ELSE
        '呼叫store procedure更新相關檔案
        strSP="usp_RTLessorAVSCustARSndworkFR " & "'" & key(0) & "'" & ",'" & key(1) & "','" & V(0) & "'" & ",'" & batchnoxx & "'" 
       ' response.write strSP
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
   END IF

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
       msgbox "AVS-City用戶裝機派工單完工/未完工結案返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此裝機派工單尚未結案，不可執行結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此裝機派工單已作廢，不可執行結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="5" then
       msgbox "此裝機派工單已月結，不可異動" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="6" then
       msgbox "無法找到此裝機派工單之用戶主檔資料，請確認AVS-City用戶主檔資料正常" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
    elseIF frm1.htmlfld.value="7" then
       msgbox "用戶已開始計費，不可執行完工結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    elseIF frm1.htmlfld.value="8" then
       msgbox "應收帳款已沖帳，不可結案返轉(請會計人員作廢沖帳記錄)" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
    else
       msgbox "無法執行裝機派工單完工結案返轉作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorAVScustsndworkfr.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>