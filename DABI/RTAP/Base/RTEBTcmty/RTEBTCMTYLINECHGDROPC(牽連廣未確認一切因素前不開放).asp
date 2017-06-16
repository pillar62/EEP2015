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
   sqlxx="select * FROM RTEBTCMTYLINECHG WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' "
   rsxx.Open sqlxx,connxx
   DROPDAT=rsxx("DROPDAT")
   RSXX.CLOSE
   if LEN(TRIM(DROPDAT)) < 1 OR ISNULL(DROPDAT) then
     endpgm="3"
   else
     sqlXX="select max(ENTRYNO) as ENTRYNO FROM RTEBTCMTYLINECHGLOG WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' " 
     rsXX.Open sqlXX,connxx
     if len(trim(rsXX("ENTRYNO"))) > 0 then
        ENTRYNO=rsXX("ENTRYNO") + 1
     else
        ENTRYNO=1
     end if
     rsXX.close
     sqlXX="insert into RTEBTCMTYLINECHGlog " _
          &"select  COMQ1, LINEQ1, PRTNO," & ENTRYNO & ", GETDATE(),'R','" & V(0) & "', " _
          &"APPLYDAT, PRTDAT, PRTUSR, CHGCOD1, CHGCOD2, CHGCOD3, NCOMQ1, " _
          &"NLINEQ1, UPDEBTCHKDAT, UPDEBTTNSDAT, UPDEBTTNSNO, EBTREPLYDAT, " _
          &"EBTREPLYSTS, EUSR, EDAT, UUSR, UDAT, MEMO, FINISHDAT, DOCKETDAT, " _
          &"TRANSDAT,TRANSNO, EBTREPLYFHDAT, EBTREPLYFHSTS,DROPDAT,DROPUSR " _
          &"FROM RTEBTCMTYLINECHG where comq1=" & key(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' "
    ' Response.Write sqlyy
     CONNXX.Execute sqlXX    
     If Err.number > 0 then
        endpgm="2"
        errmsg=cstr(Err.number) & "=" & Err.description
     else
        SQLXX=" update RTEBTCMTYLINECHG set dropdat=NULL,dropUSR='' where comq1=" & KEY(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' "
        connxx.Execute SQLXX
        If Err.number > 0 then
           endpgm="2"
           '發生錯誤時，刪除異動檔所新增的異動資料
           errmsg=cstr(Err.number) & "=" & Err.description
           sqlXX="delete * FROM RTEBTCMTYLINECHGlog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' and entryno=" & key(3) 
           CONNXX.Execute sqlXX
        else
           endpgm="1"
           errmsg=""
        end if      
      end if
    END IF
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "主線異動資料作廢返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此筆資料尚未作廢，不可執行作廢返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    else
       msgbox "無法執行主線異動資料作廢返轉,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcmtyhardwaredrop.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>