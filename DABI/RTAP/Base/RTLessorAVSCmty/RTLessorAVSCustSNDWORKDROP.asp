<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM conn
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSZZ=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   conn.Open DSN
   'conn.BeginTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   sqlxx="select * FROM RTLessorAVSCUSTsndwork WHERE CUSID='" & KEY(0) & "' and prtno='" & key(1) & "' "
   sqlzz="select * FROM RTLessorAVSCustRCVHardware WHERE prtno='" & KEY(1) & "' and canceldat is null "
   RSXX.OPEN SQLXX,conn
   RSZZ.OPEN SQLZZ,conn
   endpgm="1"
   '當完工結案日或未完工結案日不為空白時，表示AVS-City竣工確認單已結案，不可作廢
   IF LEN(TRIM(RSXX("closeDAT"))) <> 0 or LEN(TRIM(RSXX("uncloseDAT"))) <> 0 THEN
      ENDPGM="3"
   elseif LEN(TRIM(RSXX("dropdat"))) <> 0 then
      endpgm="4"
   elseif not rszz.eof then
      endpgm="5"            
   ELSE
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorAVSCustARSndworkDrop " & "'" & key(0) & "'" & ",'" & key(1) & "','" & V(0) & "'" 
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
   RSZZ.CLOSE
   conn.Close
   SET RSXX=NOTHING
   SET RSZZ=NOTHING
   set conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS-City用戶派工單作廢成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此派工單已完工結案，不可作廢(欲作廢請先清除裝機完工日)：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此派工單已作廢，不可重覆執行作廢作業：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="5" then
       msgbox "此派工單已產生物品領用單，不可直接作廢(請先返轉物品領用單)" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close             
    else
       msgbox "無法執行派工單作廢作業,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorAVScustsndworkdrop.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>