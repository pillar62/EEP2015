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
   SQLXX="select * FROM RTEBTCMTYLINE WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) 
   'response.Write sqlxx
   RSXX.Open SQLxx,CONNXX
   select case KEY(2)
     '主線申請
     CASE "A"
         '已測通，不能清除申請轉檔記錄
         IF LEN(TRIM(RSXX("ADSLAPPLYDAT"))) > 0 THEN
            ENDPGM=3
         '尚未提出申請，不能清除申請轉檔記錄
         ELSEIF ISNULL(RSXX("UPDEBTDAT")) OR ISNULL(RSXX("UPDEBTCHKDAT")) THEN
            ENDPGM=4
         '已有主線合約編號，不能清除申請轉檔記錄
         ELSEIF LEN(TRIM(RSXX("CONTRACTNO")))  > 0 THEN
            ENDPGM=5
         END IF         
         if endpgm=1 then
            sqlxx="update rtebtcmtyline set UPDEBTDAT=null,TRANSNOAPPLY='' FROM rtebtcmtyline WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) 
            connxx.execute sqlxx
            If Err.number > 0 then
                  endpgm="4"
                  errmsg=cstr(Err.number) & "=" & Err.description
            else
               sqlxx="update RTEBTFtpAdslRpl set CLRFLAG=getdate(),clrusr='" & v(0) & "' where comq1=" & key(0) & " and lineq1=" & key(1) & " and flag='" & key(2) & "' "
               connxx.execute sqlxx
            end if
         end if
     '主線測通回報
     CASE "F"
         '主線無測通回報轉檔記錄或主線尚未測通，不能清除測通回報轉檔記錄
         IF ISNULL(RSXX("APPLYUPLOADTNS")) OR ISNULL(RSXX("APPLYUPLOADDAT")) OR ISNULL(RSXX("ADSLAPPLYDAT")) THEN
            ENDPGM=6
         END IF         
         if endpgm=1 then
            sqlxx="update rtebtcmtyline set applyuploadtns=null,TRANSNOdocket='' FROM rtebtcmtyline WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) 
            connxx.execute sqlxx
            If Err.number > 0 then
                  endpgm="4"
                  errmsg=cstr(Err.number) & "=" & Err.description
            else
               sqlxx="update RTEBTFtpAdslRpl set CLRFLAG=getdate(),clrusr='" & v(0) & "' where comq1=" & key(0) & " and lineq1=" & key(1) & " and flag='" & key(2) & "' "
               connxx.execute sqlxx
            end if
         end if
     '主線申請後取消(測通失敗)
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
       msgbox "AVS主線電子轉檔異常資料清除申請記錄成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此主線已測通，不可執行申請記錄清除作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此主線尚未申請，不須執行清除申請記錄作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此主線已取得合約編號，不可執行清除申請記錄作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="6" then
       msgbox "AVS主線無測通回報轉檔記錄，不可清除測通回報記錄" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    else
       msgbox "無法執行AVS主線電子轉檔異常資料清除申請記錄作業" & "  " & frm1.htmlfld1.value
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