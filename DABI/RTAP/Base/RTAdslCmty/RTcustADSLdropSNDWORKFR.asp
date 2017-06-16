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
   SET RSZZ=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTcustADSLdropsndwork WHERE cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   endpgm="1"
   '當已作廢時，不可執行結案返轉
   IF LEN(TRIM(RSXX("DROPDAT"))) <> 0 THEN
      ENDPGM="3"
   '當完工結案或未完工結案日皆空白時，不可執行結案返轉
   elseif isnull(RSXX("FINISHDAT")) AND isnull(RSXX("UNCLOSEDAT")) then
      endpgm="4"      
   ELSE
      finishdat=rsxx("finishdat")
      sqlyy="select max(seq) as seq FROM RTcustADSLdropsndworklog WHERE cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
      rsZZ.Open sqlyy,connxx
      
      if len(trim(rsZZ("seq"))) > 0 then
         seq=rsZZ("seq") + 1
      else
         seq=1
      end if
      rsZZ.close
      set rsZZ=nothing
      '完工結案返轉
      IF NOT ISNULL(RSXX("FINISHDAT")) THEN
         sqlyy="insert into RTcustADSLdropsndworklog " _ 
           &"SELECT   cusid,entryno,prtno, " & seq & ", GETDATE() ,'FR','" & V(0) & "',COMQ1, SENDWORKDAT, PRTUSR, " _
           &"ASSIGNENGINEER, ASSIGNCONSIGNEE, REALENGINEER, REALCONSIGNEE, DROPDAT, DROPDESC, CLOSEDAT, " _
           &"BONUSCLOSEYM, BONUSCLOSEDAT, BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT, STOCKCLOSEUSR, STOCKFINCHK, " _
           &"MEMO, PRTDAT, UNCLOSEDAT, DROPUSR, CLOSEUSR, EUSR, EDAT, UUSR, UDAT, FINISHDAT " _
           &"FROM RTcustADSLdropsndwork where cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
         STS="1"
      '未完工結案返轉
      ELSE
         sqlyy="insert into RTcustADSLdropsndworklog " _ 
           &"SELECT   cusid,entryno,prtno, " & seq & ", GETDATE() ,'UR','" & V(0) & "',COMQ1, SENDWORKDAT, PRTUSR, " _
           &"ASSIGNENGINEER, ASSIGNCONSIGNEE, REALENGINEER, REALCONSIGNEE, DROPDAT, DROPDESC, CLOSEDAT, " _
           &"BONUSCLOSEYM, BONUSCLOSEDAT, BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT, STOCKCLOSEUSR, STOCKFINCHK, " _
           &"MEMO, PRTDAT, UNCLOSEDAT, DROPUSR, CLOSEUSR, EUSR, EDAT, UUSR, UDAT, FINISHDAT " _
           &"FROM RTcustADSLdropsndwork where cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
         STS="2"
      END IF
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTcustADSLdropsndwork set FINISHdat=NULL,UNCLOSEDAT=NULL,CLOSEUSR='',UUSR='" & V(0) & "',UDAT=GETDATE()  where cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTcustADSLdropsndworklog WHERE cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' and seq=" & seq
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
   SET RSYY=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "中華ADSL399退租戶拆機工單結案返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當已作廢時，不可執行結案返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "當完工結案或未完工結案日皆空白時，不可執行結案返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    else
       msgbox "無法執行退租戶拆機工單結案返轉作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtcustADSLdropsndworkfr.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>