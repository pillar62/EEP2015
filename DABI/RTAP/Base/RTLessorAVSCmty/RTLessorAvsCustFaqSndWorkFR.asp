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
   sqlxx="select * FROM RTLessorAvsCUST WHERE CUSID='" & KEY(0) & "' "
   sqlyy="select * FROM RTLessorAvsCustFaqH WHERE CUSID='" & KEY(0) & "' and Faqno='" & key(1) & "' "
   rsyy.open sqlyy,conn
   rsxx.open sqlxx,conn
   if rsxx.eof then
      '找不到客戶基本檔
      endpgm="7"
   elseif rsyy.eof then
      '找不到客戶客服單資料
      endpgm="13"
   elseif LEN(TRIM(RSXX("cancelDAT"))) > 0 then
      '客戶已作廢,不可結案返轉
      endpgm="8"
   elseif LEN(TRIM(RSyy("callbackdat"))) > 0 then
      '客服單已押回覆日，不可返轉
      endpgm="9"      
   elseif LEN(TRIM(RSyy("finishdat"))) > 0 then
      '客服單已結案，不可返轉
      endpgm="10"          
   elseif LEN(TRIM(RSyy("cancelDAT"))) > 0 then
      '客戶客服單資料已作廢,不可結案返轉
      endpgm="14"      
   '暫存客服單檔中的派工日、派工單號、派工人員
   ELSE
      XXSNDWORK=RSYY("SNDWORK")
      XXSNDUSR=RSYY("SNDUSR")
      XXSNDPRTNO=RSYY("SNDPRTNO")
   END IF
   rsxx.close
   rsyy.close

'上述正確時
if endpgm="" then
   endpgm="1"
  
   sqlxx="select * FROM RTLessorAvsCustfaqsndwork WHERE CUSID='" & KEY(0) & "' and faqno='" & key(1) & "' and prtno='" & key(2) & "' "
   RSXX.OPEN SQLXX,CONN
   BATCHNOXX=RSXX("BATCHNO")
   '檢查應收帳款檔是否已沖帳
   sqlyy="select * FROM RTLessorAvsCUSTAR WHERE CUSID='" & KEY(0) & "' AND BATCHNO='" & BATCHNOXx & "'"
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
   '如果客服單主檔已有其它派工單號時，則不允許執行未完工結案返轉
   '(完工結案返轉不在此限，原因是完工結案返轉只有清除派工單結案日，而未完工結案返轉則會連同派工單號、派工日一起更新)
   ELSEIF (LEN(TRIM(XXSNDPRTNO)) > 0 OR LEN(TRIM(XXSNDWORK)) > 0 ) AND LEN(TRIM(RSXX("unCLOSEDAT")))>0 THEN
      ENDPGM="6"
   ELSE
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorAvsCustFaqSndworkFR " & "'" & key(0) & "','" & key(1) & "','" & key(2) & "','" & V(0) & "','" & batchnoxx & "'" 
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
       msgbox "ET-City用戶維修派工單結案返轉成功",0
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
       msgbox "此維修派工單已作廢，不可返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="6" then
       msgbox "此維修派工單所屬客服單已產生其它派工單，因此不能執行此派工單返轉作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
   elseIF frm1.htmlfld.value="7" then
       msgbox "找不到客戶基本檔，無法返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="8" then
       msgbox "客戶資料已作廢，無法返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
   elseIF frm1.htmlfld.value="9" then
       msgbox "客服單已押回覆日，不可執行派工單結案返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
   elseIF frm1.htmlfld.value="10" then
       msgbox "客服單已結案，不可執行派工單結案返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                          
    elseIF frm1.htmlfld.value="13" then
       msgbox "找不到客戶客服單資料檔，無法返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="14" then
       msgbox "客戶客服單資料已作廢，不可返轉。" & "  " & frm1.htmlfld1.value
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
<form name=frm1 method=post action=RTLessorAvsCustFAQsndworkfr.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>