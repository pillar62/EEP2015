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
   endpgm="1"
 '  On Error Resume Next
   '當該主線EBT已回覆時，不可再執行手動回覆
   sqlxx="select * FROM RTEBTCMTYLINE WHERE COMQ1=" & key(0) & " and LINEQ1=" & key(1) & " "
   RSXX.OPEN SQLXX,CONNxx
   if not isnull(rsxx("EBTAPPLYOKRTN")) or len(trim(rsxx("EBTAPPLYOKRTN"))) > 0 then
      endpgm="3"
   ELSEIF isnull(rsxx("APPLYUPLOADDAT")) THEN
      endpgm="4"
   ELSEIF isnull(rsxx("ADSLAPPLYDAT")) THEN
      endpgm="5"      
   end if
   rsxx.Close
   if endpgm="1" then
           SQLXX=" update RTEBTCMTYLINE set EBTAPPLYOKRTN=getdate() where COMQ1=" & key(0) & " and LINEQ1=" & key(1) & " AND EBTAPPLYOKRTN IS NULL "
            connxx.Execute SQLXX
            If Err.number > 0 then
               endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
               errmsg=cstr(Err.number) & "=" & Err.description
            else
               endpgm="1"
               errmsg=""
            end if      
   end if
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS主線手動回覆作業成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當該主線EBT已回覆時，不可再執行手動回覆" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "當該主線回報EBT轉檔審核日為空白時，不可執行手動回覆" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="5" then
       msgbox "當該主線尚未測通，不可執行手動回覆" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
    else
       msgbox "無法執行手動回覆作業,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTEBTCMTYLINEMANUAL.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>