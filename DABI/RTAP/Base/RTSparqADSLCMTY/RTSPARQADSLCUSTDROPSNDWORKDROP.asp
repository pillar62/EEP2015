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
   sqlxx="select * FROM RTsparqadslcustdropsndwork WHERE cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   endpgm="1"
   '當裝機完成日不為空白時，表示主線派工單已結案，不可作廢
   IF LEN(TRIM(RSXX("closeDAT"))) <> 0 OR LEN(TRIM(RSXX("UncloseDAT"))) <> 0 OR LEN(TRIM(RSXX("FINISHDAT"))) <> 0 THEN
      ENDPGM="3"
   elseif LEN(TRIM(RSXX("dropdat"))) <> 0 then
      endpgm="4"
   ELSE
      sqlyy="select max(SEQ) as SEQ FROM RTsparqadslcustdropsndworkLOG WHERE cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("SEQ"))) > 0 then
         SEQ=rsyy("SEQ") + 1
      else
         SEQ=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into RTsparqadslcustdropsndworkLOG " _
           &"SELECT   cusid,entryno,prtno, " & SEQ & ", getdate(), 'C','" &  v(0) & "', " _
           &"COMQ1, SENDWORKDAT, PRTUSR, " _
           &"ASSIGNENGINEER, ASSIGNCONSIGNEE, REALENGINEER, REALCONSIGNEE, DROPDAT, DROPDESC, CLOSEDAT, " _
           &"BONUSCLOSEYM, BONUSCLOSEDAT, BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT, STOCKCLOSEUSR, STOCKFINCHK, " _
           &"MEMO, PRTDAT, UNCLOSEDAT, DROPUSR, CLOSEUSR, EUSR, EDAT, UUSR, UDAT, FINISHDAT " _
           &"FROM RTsparqadslcustdropsndwork where cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTsparqadslcustdropsndwork set dropdat=getdate(),dropdesc='退租拆機工單作廢' where cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTsparqadslcustdropsndworkLOG WHERE cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' AND SEQ=" & SEQ
            CONNXX.Execute sqlyy 
         else
            endpgm="1"
            errmsg=""
         end if      
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
       msgbox "退租戶拆機派工單作廢成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此拆機工單已完工/未完工結案，不可作廢(欲作廢請先清除拆機完工日)：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此拆機工單已作廢，不可重覆執行作廢作業：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    else
       msgbox "無法執行退租戶拆機工單作廢作業,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtsparqadslcustdropsndworkDROP.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>