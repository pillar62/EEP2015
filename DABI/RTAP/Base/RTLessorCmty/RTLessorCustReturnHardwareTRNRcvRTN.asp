<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM Conn
   Set Conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   Conn.Open DSN
 '  檢查該派工單是否已結案或作廢或已無可轉設備項目
   sqlxx="select * FROM RTLessorCustReturnSndwork WHERE CUSID='" & KEY(0) & "' and entryno=" & key(1) & " AND PRTNO='" & KEY(2) & "' "
   RSXX.OPEN SQLXX,Conn
   '找不到派工單檔
   if rsxx.eof then
      endpgm="3"
   '派工單已完工結案或未完工結案，不可返轉物品領用單
   elseif len(trim(rsxx("closedat" ))) > 0 or len(trim(rsxx("unclosedat" ))) > 0 then
      endpgm="4"
   '派工單已作廢，不可返轉物品領用單
   elseif len(trim(rsxx("dropdat"))) > 0 then
      endpgm="5"
   '派工單已產生應收帳款，不可返轉物品領用單
   elseif len(trim(rsxx("cdat"))) > 0 or len(trim(rsxx("batchno"))) > 0 then
      endpgm="6"      
   end if
   rsxx.close
   '檢查設備檔條件
   sqlxx="select * FROM RTLessorCustReturnHardware WHERE CUSID='" & KEY(0) & "' and entryno=" & key(1) & " AND PRTNO='" & KEY(2) & "' and seq=" & key(3)
 '  response.write sqlxx
  ' response.end
   RSXX.OPEN SQLXX,Conn
   '無可轉設備資料
   if RSXX.EOF  then
      endpgm="10"   
   ELSEIF len(trim(rsxx("dropdat"))) > 0 then
      endpgm="7"
   elseif len(trim(rsxx("rcvprtno"))) = 0 then
      endpgm="8"
   elseif len(trim(rsxx("rcvfinishdat"))) > 0 then
      endpgm="9"
   ELSE
      XXRCVPRTNO=rsxx("rcvprtno")
   end if
   rsxx.close
  
if endpgm="" then
   '呼叫store procedure更新相關檔案
   strSP="usp_RTLessorCustHardwareTRNRCVRTNExe " & "'" & XXRCVPRTNO & "','" & V(0) & "'" 
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
Conn.Close
SET RSXX=NOTHING
set Conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "ET-City用戶派工設備返轉物品領用單作業成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "找不到派工單檔!" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "派工單已完工結案或未完工結案，不可返轉物品領用單" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="5" then
       msgbox "派工單已作廢，不可返轉物品領用單" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                
    elseIF frm1.htmlfld.value="6" then
       msgbox "派工單已產生應收帳款，不返可轉物品領用單" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="7" then
       msgbox "此設備已作廢，不可返轉物品領用單" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    elseIF frm1.htmlfld.value="8" then
       msgbox "此設備尚未轉物品領用單，不可返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
    elseIF frm1.htmlfld.value="9" then
       msgbox "此設備之物品領用單已經結案，不可返轉(欲返轉請先取消物品領用單結案作業)" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                 
    elseIF frm1.htmlfld.value="10" then
       msgbox "此設備之設備檔資料不存在，不可返轉(請通知資訊部)" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                                     
    else
       msgbox "無法執行用戶派工設備返轉物品領用單作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorCustReturnhardwaretrnrcvrtn.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>