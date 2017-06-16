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
   sqlxx="select * FROM RTEBTCUSTDROP WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and CUSID='" & key(2) & "' AND ENTRYNO=" & KEY(3)
   rsxx.Open sqlxx,connxx
   DROPDAT=rsxx("DROPDAT")
   RSXX.CLOSE
   if LEN(TRIM(DROPDAT)) < 1 then
     endpgm="3"
   else
     sqlXX="select max(seq) as seq FROM RTEBTCUSTDROPLOG WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and CUSID='" & key(2) & "' and entryno=" & key(3) 
     rsXX.Open sqlXX,connxx
     if len(trim(rsXX("seq"))) > 0 then
        seq=rsXX("seq") + 1
     else
        seq=1
     end if
     rsXX.close
     sqlXX="insert into RTEBTCUSTDROPlog " _
          &"SELECT  COMQ1, LINEQ1, CUSID, ENTRYNO," & SEQ & ", GETDATE(),'R','" & V(0) & "', " _
                          &"APPLYDAT, EXPECTDAT, REFUNDBANK, REFUNDBANKBRANCH, " _
                          &"REFUNDSAVINGTYPE, REFUNDACCOUNT1, REFUNDACCOUNT2, " _
                          &"REFUNDACCOUNT3, REFUNDACCOUNT4, POSTOFFICENAME, " _
                          &"POSTOFFICEBRANCH, POSTOFFICEACCOUNT11, POSTOFFICEACCOUNT12, " _
                          &"POSTOFFICEACCOUNT21, POSTOFFICEACCOUNT22, DROPCOD1, DROPCOD2, " _
                          &"DROPCOD3, DROPCOD4, DROPCOD5, DROPCOD6, DROPCOD7, DROPCOD8, " _
                          &"DROPCOD9, DROPCOD10, DROPCOD11, DROPCOD12, DROPCOD13, " _
                          &"DROPCOD14, DROPCOD15, DROPOTHER, DROPDESC, DROPDAT, DROPUSR, " _
                          &"TRANSCHKDAT, TRANSDAT, TRANSNO, EBTREPLYDAT, EBTREPLYCOD, EUSR, " _
                          &"EDAT, UUSR, UDAT " _
          &"FROM RTEBTCUSTDROP where comq1=" & key(0) & " and lineq1=" & key(1) & " and CUSID='" & key(2) & "' AND ENTRYNO=" & KEY(3)
    ' Response.Write sqlyy
     CONNXX.Execute sqlXX    
     If Err.number > 0 then
        endpgm="2"
        errmsg=cstr(Err.number) & "=" & Err.description
     else
        SQLXX=" update RTEBTCUSTDROP set dropdat=NULL,dropUSR='' where comq1=" & KEY(0) & " and lineq1=" & key(1) & " and CUSID='" & key(2) & "' and entryno=" & key(3)
        connxx.Execute SQLXX
        If Err.number > 0 then
           endpgm="2"
           '發生錯誤時，刪除異動檔所新增的異動資料
           errmsg=cstr(Err.number) & "=" & Err.description
           sqlXX="delete * FROM RTEBTCUSTDROPlog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " and CUSID='" & key(2) & "' and entryno=" & key(3) & " AND seq=" & seq
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
       msgbox "用戶服務終止資料作廢返轉成功",0
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
       msgbox "無法執行用戶服務終止資料作廢返轉,錯誤訊息：" & "  " & frm1.htmlfld1.value
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