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
   sqlxx="select * FROM hbcmtyarrangesndwork WHERE comq1=" & KEY(0) & " and comtype='" & key(1)  & "' and prtno='" & key(2) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   endpgm="1"
   '當尚未作廢時，不可執行作廢返轉
   IF ISNULL(RSXX("DROPDAT")) THEN
      ENDPGM="3"
   '當獎金計算或庫存計算不為空白，不可執行作廢返轉
   elseif LEN(TRIM(RSXX("BONUSCLOSEYM"))) <> 0 OR LEN(TRIM(RSXX("STOCKCLOSEYM"))) <> 0 then
      endpgm="5"
   ELSE
      sqlyy="select max(entryno) as entryno FROM hbcmtyarrangesndworklog WHERE comq1=" & KEY(0) & " and comtype='" & key(1) & "'  and prtno='" & key(2) & "' "
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("entryno"))) > 0 then
         entryno=rsyy("entryno") + 1
      else
         entryno=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into hbcmtyarrangesndworklog " _
           &"SELECT    COMQ1, COMTYPE, PRTNO, " & ENTRYNO & ", getdate(), 'R','" &  v(0) & "', " _
           &" SNDDAT, ASSIGNENGINEER, ASSIGNCONSIGNEE, REALENGINEER, REALCONSIGNEE, DROPDAT, " _
           &" CLOSEDAT, EUSR, EDAT, UUSR, UDAT, CLOSEUSR, DROPUSR, EQUIPCUTID, EQUIPTOWNSHIP, EQUIPADDR, ERZONE, " _
           &" PRTDAT, PRTUSR, MEMO, BONUSCLOSEYM, BONUSCLOSEDAT, BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, " _
           &" STOCKCLOSEDAT, STOCKCLOSEUSR, STOCKFINCHK " _
           &" FROM hbcmtyarrangesndwork where comq1=" & key(0) & " and comtype='" & key(1) & "' and prtno='" & key(2) & "' "
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update hbcmtyarrangesndwork set DROPdat=NULL,DROPUSR='' where comq1=" & KEY(0) & " and comtype='" & key(1)  & "' and prtno='" & key(2) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM hbcmtyarrangesndworklog WHERE comq1=" & key(0) & " and comtype='" & key(1) & "' and prtno='" & key(2) & "' AND ENTRYNO=" & ENTRYNO
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
       msgbox "社區整線派工單作廢返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當尚未作廢時，不可執行作廢返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="5" then
       msgbox "此整線派工單已月結，不可異動" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    else
       msgbox "無法執行整線派工單作廢返轉作業,錯誤訊息" & "  " & frm1.htmlfld1.value
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