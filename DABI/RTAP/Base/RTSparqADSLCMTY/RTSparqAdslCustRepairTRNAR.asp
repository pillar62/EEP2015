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
   DSN="DSN=RtLib"
   conn.Open DSN
   ' conn.BEGINTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   '因為當STORE PROCEDURE內OPEN太多TABLE時，ASP無法控制CURSOR而會發生錯誤(不明瞭可以將BEGIN、COMMIT、ROLLBACK的MARK移除並執行後即知)
   sqlxx="select * FROM RTSparqAdslCustRepair WHERE CUSID='" & KEY(0) & "' and entryno=" & key(1) 
   sqlYY="select * FROM RTSparqAdslCust WHERE CUSID='" & KEY(0) & "' "
   rsyy.open sqlyy,conn
   RSXX.OPEN SQLXX,conn

     endpgm="1"
     '當客戶維修收款資料已作廢時，不可執行轉應收帳款作業
     IF not isnull(RSXX("canceldat")) THEN
        ENDPGM="3"
     '無收款日, 不可產生
     ELSEIF isnull(RSXX("rcvmoneydat")) THEN
        ENDPGM="5"
     '當客戶已退租時，必須作廢
     ELSEIF not isnull(RSYY("DROPdat")) THEN
        ENDPGM="10"           
     'batchno不為空白或結案日不為空白時，表示此筆維修收款資料已轉應收帳款，不可重複產生
     elseif LEN(TRIM(RSXX("batchno"))) <> 0 OR LEN(TRIM(RSXX("FINISHDAT"))) > 0 then
        endpgm="3"
     '應收金額為0者，不可產生
     elseif RSXX("moveamt")=0 and RSXX("equipamt")=0 and RSXX("setamt")=0 and RSXX("returnamt")=0 then
        endpgm="4"      
     ELSE
        '呼叫store procedure更新相關檔案
        strSP="usp_RTSparqAdslCustRepairAR " & "'" & key(0) & "'" & "," & key(1) & ",'" & V(0) & "', 'MF', '' "
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
       msgbox "速博399用戶維修收款轉應收帳款成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
	elseIF frm1.htmlfld.value="3" then
       msgbox "此筆維修收款資料已轉應收帳款，不可重複產生" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
    elseIF frm1.htmlfld.value="4" then
       msgbox "應收金額為0者，不可產生應收帳款" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
    elseIF frm1.htmlfld.value="5" then
       msgbox "無收款日，不可產生應收帳款" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
    elseIF frm1.htmlfld.value="6" then
       msgbox "維修收款資料已作廢時，不可執行轉應收帳款作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
    elseIF frm1.htmlfld.value="10" then
       msgbox "客戶資料已退租，必須作廢維修收款資料。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
    else
       msgbox "無法執行用戶維修收款轉應收帳款作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
    end if
    winP.focus()
    window.close
 end sub
</script> 
</head>  
<form name=frm1 method=post action="RTSparqAdslCustRepairTRNAR.asp" ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>
