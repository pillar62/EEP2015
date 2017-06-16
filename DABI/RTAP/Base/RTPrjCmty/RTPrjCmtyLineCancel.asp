<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTPrjCmtyLine WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) 
   sqlYY="select COUNT(*) AS CNT FROM RTPrjCust WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CANCELDAT IS NULL AND DROPDAT IS NULL AND (DOCKETDAT IS NOT NULL OR FINISHDAT IS NOT NULL ) "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   RSYY.OPEN SQLYY,CONNxx
   endpgm="1"

   '線路已作廢，不可重複作廢
   if LEN(TRIM(RSXX("CANCELdat"))) <> 0 then
      endpgm="4"

   '此線路有尚未退租的用戶，不可直接作廢主線
   elseif RSYY("CNT") > 0 then
      endpgm="7"

   '線路已撤線，不須再作廢
   elseif LEN(TRIM(RSXX("DROPDAT"))) <> 0 then
      endpgm="5"

   '線路已到位，不可作廢
   elseif LEN(TRIM(RSXX("ARRIVEDAT"))) <> 0 then
      endpgm="6"
   ELSE
      '呼叫store procedure更新相關檔案
      'strSP="usp_RTPrjCmtyLineCANCEL " & key(0) & "," & key(1) & ",'" & V(0) & "'" 
      strSP ="update RTPrjCmtyLine set canceldat=getdate(),cancelusr='"& V(0) &"' where comq1=" & key(0) & " and lineq1=" & key(1)
      Set ObjRS = connXX.Execute(strSP)
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
   connXX.Close
   SET RSXX=NOTHING
   SET RSYY=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "專案社區主線作廢成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="4" then
       msgbox "線路已作廢，不可重複作廢。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="5" then
       msgbox "線路已撤線，不須再作廢。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close        
    elseIF frm1.htmlfld.value="6" then
       msgbox "線路已到位，不可作廢。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="7" then
       msgbox "此線路有尚未退租的用戶，不可直接作廢主線。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                
    else
       msgbox "無法執行用戶申請作廢作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTPrjCmtyLineDROP.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>