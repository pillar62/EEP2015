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
   'conn.BeginTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   endpgm="1"
 '  On Error Resume Next
   '當所屬派工單已計算庫存後，不可作廢返轉之(stockcloseym<>'')
   sqlxx="select * FROM RTlessorcusthardware WHERE cusid='" & key(0) & "'  and prtno='" & key(1) & "' and entryno=" & key(2)
   sqlyy="select * FROM RTlessorcustsndwork WHERE cusid='" & key(0) & "' and prtno='" & key(1) & "' "
   rsxx.Open sqlxx,conn
   rsyy.Open sqlyy,conn
    endpgm="1"
     '未作廢
     IF isnull(rsxx("dropdat"))THEN
        ENDPGM="3"
     elseIF len(trim(rsyy("closedat"))) > 0 or len(trim(rsyy("unclosedat"))) > 0 THEN
      ENDPGM="4"      
        
     ELSE
        '呼叫store procedure更新相關檔案
        strSP="usp_RTLessorCustHardwareDropC " & "'" & key(0) & "'" & ",'" & key(1) & "'," & key(2) & ",'" & V(0) & "'" 
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
    RSyy.CLOSE
   conn.Close
   SET RSXX=NOTHING
   SET RSyy=NOTHING
   set conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "設備安裝資料作廢返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此筆設備尚未作廢，不可作廢返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "所屬派工單已結案，不可執行作廢返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    else
       msgbox "無法執行設備安裝資料作廢返轉,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtlessorcusthardwaredropc.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>