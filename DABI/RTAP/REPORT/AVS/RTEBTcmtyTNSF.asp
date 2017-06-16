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
   sqlxx="select * FROM RTEBTFtpBuildingRpl WHERE comq1=" & KEY(0) & " and flag='" & key(1) & "' " 
   RSXX.OPEN SQLXX,CONNxx
     '當已作廢時，不可執行結案作業
     IF LEN(TRIM(RSXX("DROPDAT"))) <> 0 THEN
        ENDPGM="3"
     '已結案時，不可重複結案
     elseif LEN(TRIM(RSXX("CLOSEDAT"))) <> 0  then
        endpgm="2"
     '結案前，請先執行清除轉檔記錄
     elseif LEN(TRIM(RSXX("CLRFLAG"))) = 0 then
        endpgm="5"
     ELSE
          SQLXX=" update RTEBTFtpBuildingRpl set CLOSEdat=getdate(),CLOSEUSR='" & V(0) & "' where comq1=" & KEY(0)  & " and flag='" & key(1) & "' "
           connxx.Execute SQLXX
           If Err.number > 0 then
              endpgm="4"
            '發生錯誤時，刪除異動檔所新增的異動資料
              errmsg=cstr(Err.number) & "=" & Err.description
          end if
      END IF
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
       msgbox "AVS社區電子轉檔異常資料結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="5" then
       msgbox "執行結案前，請先清除電子轉檔記錄" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="2" then
       msgbox "此資料已結案，不須重複執行結案作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="3" then
       msgbox "此資料已作廢，不可執行結案作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    else
       msgbox "無法執行AVS社區電子轉檔異常資料結案作業" & "  " & frm1.htmlfld1.value
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