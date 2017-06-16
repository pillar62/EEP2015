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
   sqlxx="select * FROM HBADSLCMTYFIXSNDWORK WHERE fixno='" & KEY(0) & "' and prtno='" & key(1) & "' "
  ' RESPONSE.Write SQLXX
  ' RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   endpgm="1"
   CLOSEDAT=RSXX("CLOSEDAT")
   UNCLOSEDAT=RSXX("UNCLOSEDAT")
   CLOSEUSR=RSXX("CLOSEUSR")
   IF ISNULL(RSXX("closeDAT")) AND ISNULL(RSXX("UNcloseDAT")) THEN
      ENDPGM="3"
   elseif LEN(TRIM(RSXX("dropdat"))) <> 0 then
      endpgm="4"
   ELSE
      sqlyy="select max(entryno) as entryno FROM HBADSLCMTYFIXSNDWORKlog WHERE fixno='" & KEY(0) & "' and prtno='" & key(1) & "' "
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("entryno"))) > 0 then
         entryno=rsyy("entryno") + 1
      else
         entryno=1
      end if
      rsyy.close
     ' set rsyy=nothing
      sqlyy="insert into HBADSLCMTYFIXSNDWORKlog " _
           &"SELECT  FIXNO,prtno," & ENTRYNO & ", getdate(), 'FR','" &  v(0) & "', " _
           &"SENDWORKDAT, ASSIGNENGINEER, ASSIGNCONSIGNEE, REALENGINEER, " _
           &"REALCONSIGNEE, DROPDAT, DROPDESC, CLOSEDAT, BONUSCLOSEYM, " _
           &"BONUSCLOSEDAT, BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, " _
           &"STOCKCLOSEDAT, STOCKCLOSEUSR, STOCKFINCHK, MEMO, PRTUSR, " _
           &"PRTDAT, UNCLOSEDAT, DROPUSR, CLOSEUSR, EUSR, EDAT, UUSR, UDAT, " _
           &"FINISHDAT " _
           &"FROM HBADSLCMTYFIXSNDWORK where FIXNO='" & key(0) & "' and prtno='" & key(1) & "' "
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update HBADSLCMTYFIXSNDWORK set CLOSEdat=NULL,UNCLOSEDAT=NULL,CLOSEUSR='' where FIXNO='" & KEY(0) & "' and prtno='" & key(1) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM HBADSLCMTYFIXSNDWORKlog WHERE FIXNO='" & key(0)  & "' and prtno='" & key(1) & "' AND ENTRYNO=" & ENTRYNO
            CONNXX.Execute sqlyy 
         else
            '正常時且為完工結案返轉(未完工結案返轉則不需處理此段)，再產生主機客訴單異動記錄及主機客訴單結案更新
           IF LEN(TRIM(CLOSEDAT)) <> 0 THEN
            sqlyy="select max(entryno) as entryno FROM HBADSLCMTYFIXHlog WHERE fixno='" & KEY(0) & "' "
            rsyy.Open sqlyy,connxx
            if len(trim(rsyy("entryno"))) > 0 then
               entryno=rsyy("entryno") + 1
            else
               entryno=1
            end if
            rsyy.close
            set rsyy=nothing
            sqlyy="insert into HBADSLCMTYFIXHlog " _
                 &"SELECT  FIXNO," & ENTRYNO & ", getdate(), 'FR','" &  v(0) & "', " _
                 &"COMQ1, LINEQ1, COMTYPE,  DEVICETYPE, RCVMAN, RCVTIME, MEMO1, MEMO2, " _
                 &"MEMO3, DROPDAT,  DROPUSR, CLOSEDAT, CLOSEUSR " _
                 &"FROM HBADSLCMTYFIXH where FIXNO='" & key(0) & "' "
            CONNXX.Execute sqlyy        
            If Err.number > 0 then
               endpgm="2"
               errmsg=cstr(Err.number) & "=" & Err.description
               '發生錯誤時，刪除主機維修單異動記錄
               sqlyy="delete * FROM HBADSLCMTYFIXSNDWORKlog WHERE FIXNO='" & key(0)  & "' and prtno='" & key(1) & "' AND ENTRYNO=" & ENTRYNO
               CONNXX.Execute sqlyy 
               '發生錯誤時，還原主機維修單至未結案前
               SQLXX=" update HBADSLCMTYFIXSNDWORK set CLOSEdat=" & CLOSEDAT & ",CLOSEUSR='" & CLOSEUSR & "' where FIXNO='" & KEY(0) & "' and prtno='" & key(1) & "' "
               connxx.Execute SQLXX
            else
               SQLXX=" update HBADSLCMTYFIXH set CLOSEdat=NULL,CLOSEUSR='' where FIXNO='" & KEY(0) & "' "
               connxx.Execute SQLXX
               If Err.number > 0 then
                  endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
                  errmsg=cstr(Err.number) & "=" & Err.description
                  sqlyy="delete * FROM HBADSLCMTYFIXHlog WHERE FIXNO='" & key(0)  & "' AND ENTRYNO=" & ENTRYNO
                  CONNXX.Execute sqlyy 
               else
                  endpgm="1"
                  errmsg=""
               end if      
            end if
          END IF
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
       msgbox "社區主機客訴維修派工單結案返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此主機維修派工單尚未執行完工/未完工結案，不須執行結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此社區主機客訴維修派工單已作廢，不可執行結案返轉作業：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="6" then
       msgbox "此主機維修派工單已執行未完工結案，不可重覆結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    else
       msgbox "無法執行社區主機客訴維修派工單結案作業,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTFAQsndworkfR.ASP ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>