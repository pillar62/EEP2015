<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM conn
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSYY=Server.CreateObject("ADODB.RECORDSET")   
   SET RSzz=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   conn.Open DSN
   ' conn.BEGINTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   '因為當STORE PROCEDURE內OPEN太多TABLE時，ASP無法控制CURSOR而會發生錯誤(不明瞭可以將BEGIN、COMMIT、ROLLBACK的MARK移除並執行後即知)
   sqlxx="select * FROM RTLessorCustCont WHERE CUSID='" & KEY(0) & "' and entryno=" & key(1) 
   sqlYY="select * FROM RTLessorCUST WHERE CUSID='" & KEY(0) & "' "
   sqlzz="select count(*) as cnt FROM RTLessorCUSTcontsndwork WHERE CUSID='" & KEY(0) & "' and entryno=" & key(1) & " and dropdat is null and unclosedat is null and closedat is null "
   rsyy.open sqlyy,conn
   RSXX.OPEN SQLXX,conn
   RSzz.OPEN SQLzz,conn
     endpgm="1"
     '找不到客戶續約主檔資料
     IF RSXX.EOF THEN
        ENDPGM="7"
     '找不到客戶主檔資料
     ELSEIF RSYY.EOF THEN
        ENDPGM="8" 
     '當客戶續約資料已作廢時，不可執行轉應收帳款作業
     ELSEIF not isnull(RSXX("canceldat")) THEN
        ENDPGM="6"
     '當客戶資料作廢時，必須作廢
     ELSEIF not isnull(RSYY("canceldat")) THEN
        ENDPGM="9"        
     '當客戶已退租時，必須作廢
     ELSEIF not isnull(RSYY("DROPdat")) THEN
        ENDPGM="10"           
     '開始計費日空白時，不可轉應收結案
     ELSEIF isnull(RSXX("strbillingdat")) THEN
        ENDPGM="11"             
     'batchno不為空白或結案日不為空白時，表示此筆續約資料已轉應收帳款，不可重複產生
     elseif LEN(TRIM(RSXX("batchno"))) <> 0 OR LEN(TRIM(RSXX("FINISHDAT"))) > 0 then
        endpgm="3"
     '應收金額為0者，不可產生
     elseif RSXX("amt")=0 then
        endpgm="4"      
     '繳費方式為現金付款時，必須由收款派工單產生應收帳款
     elseif RSXX("paytype")="02" then
        endpgm="5"      
    '不論繳費方式為何，若有存在收款派工單資料，則必須由派工單進行結案產生應收帳款
     elseif RSzz("cnt") > 0 then
        endpgm="12"              
     ELSE
        '呼叫store procedure更新相關檔案
        strSP="usp_RTLessorCustContTRNAR " & "'" & key(0) & "'" & "," & key(1) & ",'" & V(0) & "'"
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
     RSzz.CLOSE
   conn.Close
   SET RSXX=NOTHING
   SET RSYY=NOTHING
   sET RSzz=NOTHING
   set conn=nothing
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "ET-City用戶續約轉應收帳款成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
   elseIF frm1.htmlfld.value="3" then
       msgbox "此筆續約資料已轉應收帳款，不可重複產生" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "應收金額為0者，不可產生應收帳款" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close          
    elseIF frm1.htmlfld.value="5" then
       msgbox "繳費方式為現金付款時，必須由收款派工單產生應收帳款" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                
    elseIF frm1.htmlfld.value="6" then
       msgbox "續約資料已作廢時，不可執行轉應收帳款作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
     elseIF frm1.htmlfld.value="7" then
       msgbox "找不到客戶續約主檔資料，無法執行轉應收帳款結案作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="8" then
       msgbox "找不到客戶主檔資料，無法執行轉應收帳款結案作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="9" then
       msgbox "客戶資料已作廢，必須作廢續約資料" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                
    elseIF frm1.htmlfld.value="10" then
       msgbox "客戶資料已退租，必須作廢續約資料。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    elseIF frm1.htmlfld.value="11" then
       msgbox "開始計費日空白時不可轉應收結案作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    elseIF frm1.htmlfld.value="12" then
       msgbox "此續約資料已存在收款派工單，必須由派工單進行結案作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
    else
       msgbox "無法執行用戶續約轉應收帳款作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action="RTLessorCustContTRNAR.asp" ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>