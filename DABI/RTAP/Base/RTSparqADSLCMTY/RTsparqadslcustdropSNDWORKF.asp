<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTsparqadslcustdropsndwork WHERE cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   sndtype=""
   endpgm="1"
   '當已作廢時，不可執行完工結案或未完工結案
   IF LEN(TRIM(RSXX("DROPDAT"))) <> 0 THEN
      ENDPGM="3"
   '當裝機完工日空白時,不可執行完工或未完工結案
   elseif ISNULL(RSXX("closedat"))  then
      endpgm="7"            
   '當主線完工結案日或未完工結案日不為空白時，不可執行完工結案
   elseif LEN(TRIM(RSXX("finishDAT"))) <> 0 OR LEN(TRIM(RSXX("UNCLOSEDAT"))) <> 0 then
      endpgm="4"      
   ELSE
      sqlyy="select max(seq) as seq FROM RTsparqadslcustdropsndworklog WHERE cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("seq"))) > 0 then
         seq=rsyy("seq") + 1
      else
         seq=1
      end if
      rsyy.close
     ' set rsyy=nothing
      sqlyy="insert into RTsparqadslcustdropsndworklog " _ 
           &"SELECT   cusid,entryno, PRTNO, " & seq & ", GETDATE() ,'F','" & V(0) & "',COMQ1, SENDWORKDAT, PRTUSR, " _
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
         SQLXX=" update RTsparqadslcustdropsndwork set finishdat=getdate() ,CLOSEUSR='" & V(0) & "' where cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTsparqadslcustdropsndworklog WHERE cusid='" & KEY(0) & "' and entryno=" & key(1) & " and prtno='" & key(2) & "' AND seq=" & seq
            CONNXX.Execute sqlyy 
         ELSE
            endpgm="1"
            errmsg=""
         end if      
      end if
'      rsyy.close
   END IF

   RSXX.CLOSE
   connXX.Close
   SET RSXX=NOTHING
   SET RSyy=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "速博ADSL退租戶拆機工單結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當已作廢時，不可執行完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "當完工結案日或未完工結案日不為空白時，不可執行完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="7" then
       msgbox "當拆機完工日空白時,不可執行完工或未完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
    else
       msgbox "無法執行拆機工單完工結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtsparqadslcustdropsndworkf.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>