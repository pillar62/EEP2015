<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   endpgm="1"
   SQLXX="select * FROM RTEBTcust WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and cusid='" & key(2) & "' "
   'response.Write sqlxx
   RSXX.Open SQLxx,CONNXX
   select case KEY(3)
     'AVS用戶申請
     CASE "A"
         '已報竣，不能清除申請轉檔記錄
         IF LEN(TRIM(RSXX("DOCKETDAT"))) > 0 THEN
            ENDPGM=3
         '尚未提出申請，不能清除申請轉檔記錄
         ELSEIF ISNULL(RSXX("APPLYDAT")) OR ISNULL(RSXX("APPLYTNSDAT")) THEN
            ENDPGM=4
         '已有用戶avs合約編號，不能清除申請轉檔記錄
         ELSEIF LEN(TRIM(RSXX("avsNO")))  > 0 THEN
            ENDPGM=5
         END IF         
         if endpgm=1 then
            sqlxx="update rtebtcust set APPLYTNSDAT=null,TRANSNOAPPLY='' FROM rtebtcUST WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' "
            connxx.execute sqlxx
            If Err.number > 0 then
                  endpgm="4"
                  errmsg=cstr(Err.number) & "=" & Err.description
            else
               sqlxx="update RTEBTFtpAvsparaRpl set CLRFLAG=getdate(),clrusr='" & v(0) & "' where comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and flag='" & key(3) & "' "
               connxx.execute sqlxx
            end if
         end if
     'AVS用戶報竣轉檔
     CASE "F"
         'AVS用戶無報竣轉檔記錄或AVS用戶尚未報竣，不能清除報竣轉檔記錄
         IF ISNULL(RSXX("DOCKETDAT")) OR ISNULL(RSXX("TRANSDAT")) OR ISNULL(RSXX("TRANSNODOCKET")) THEN
            ENDPGM=6
         END IF         
         if endpgm=1 then
            sqlxx="update rtebtCUST set TRANSDAT=null,TRANSNODOCKET='' FROM rtebtCUST WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' "
            connxx.execute sqlxx
            If Err.number > 0 then
                  endpgm="4"
                  errmsg=cstr(Err.number) & "=" & Err.description
            else
               sqlxx="update RTEBTFtpAvsparaRpl set CLRFLAG=getdate(),clrusr='" & v(0) & "' where comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and flag='" & key(3) & "' "
               connxx.execute sqlxx
            end if
         end if
     '
     CASE ELSE
   END SELECT   
   RSXX.CLOSE
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS用戶電子轉檔異常資料清除申請記錄成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此用戶已報竣，不可執行申請記錄清除作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此用戶尚未申請，不須執行清除申請記錄作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此用戶已取得AVS合約編號，不可執行清除申請記錄作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="6" then
       msgbox "AVS用戶無報竣轉檔記錄，不可清除報竣轉檔記錄" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    else
       msgbox "無法執行AVS用戶電子轉檔異常資料清除申請記錄作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcmtylinesndworkdrop.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>