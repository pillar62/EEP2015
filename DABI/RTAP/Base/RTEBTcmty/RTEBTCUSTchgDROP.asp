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
   sqlxx="select * FROM RTEBTCUSTCHG WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and CUSID='" & key(2) & "' AND ENTRYNO=" & KEY(3)
   rsxx.Open sqlxx,connxx
   DROPDAT=rsxx("DROPDAT")
   TRANSCHKDAT=rsxx("TRANSCHKDAT")
   FINISHDAT=rsxx("FINISHDAT")
   RSXX.CLOSE
   if LEN(TRIM(DROPDAT)) > 0 OR NOT ISNULL(DROPDAT) then
     endpgm="3"
   ELSEif LEN(TRIM(TRANSCHKDAT)) > 0  OR NOT ISNULL(TRANSCHKDAT) then
     endpgm="4"
   ELSEif LEN(TRIM(FINISHDAT)) > 0  OR NOT ISNULL(FINISHDAT) then
     endpgm="5"     
   else
     sqlXX="select max(seq) as seq FROM RTEBTCUSTCHGLOG WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and CUSID='" & key(2) & "' and entryno=" & key(3) 
     rsXX.Open sqlXX,connxx
     if len(trim(rsXX("seq"))) > 0 then
        seq=rsXX("seq") + 1
     else
        seq=1
     end if
     rsXX.close
     sqlXX="insert into RTEBTCUSTCHGlog " _
          &"select  COMQ1, LINEQ1, CUSID, ENTRYNO," & SEQ & ", GETDATE(),'C','" & V(0) & "', " _
                       &" CHGCOD1, CHGCOD2, CHGCOD3, CHGCOD4, CHGCOD4OPT, APPLYDAT, " _
                       &" NCUSNC, NSOCIALID, NCUTID2, NTOWNSHIP2, NVILLAGE2, NCOD21, " _
                       &"NNEIGHBOR2, NCOD22, NSTREET2, NCOD23, NSEC2, NCOD24, NLANE2, " _
                       &"NCOD25, NTOWN2, NALLEYWAY2, NCOD26, NNUM2, NCOD27, NFLOOR2, " _
                       &"NCOD28, NROOM2, NCOD29, NBIRTHDAY, NCUTID3, NTOWNSHIP3, " _
                       &"NVILLAGE3, NCOD31, NNEIGHBOR3, NCOD32, NSTREET3, NCOD33, NSEC3, " _
                       &"NCOD34, NLANE3, NCOD35, NTOWN3, NALLEYWAY3, NCOD36, NNUM3, " _
                       &"NCOD37, NFLOOR3, NCOD38, NROOM3, NCOD39, NEMAIL, NCONTACT, " _
                       &"NCONTACTTEL, NMOBILE, DROPDAT, DROPUSR, TRANSCHKDAT, TRANSDAT, " _
                       &"TRANSNO, EBTREPLYDAT, EBTREPLYCOD, EUSR, EDAT, UUSR, UDAT, " _
                       &"FINISHDAT, FINISHCHKDAT, FINISHTNSDAT, FINISHTNSNO, NCOMQ1, " _
                       &"NLINEQ1, NCUTID1, NTOWNSHIP1, NVILLAGE1, NCOD11, NNEIGHBOR1, " _
                       &"NCOD12, NSTREET1, NCOD13, NSEC1, NCOD14, NLANE1, NCOD15, NTOWN1, " _
                       &"NALLEYWAY1, NCOD16, NNUM1, NCOD17, NFLOOR1, NCOD18, NROOM1, " _
                       &"NCOD19, NRZONE1, NRZONE2, NRZONE3, NTELNO " _ 
          &"FROM RTEBTCUSTCHG where comq1=" & key(0) & " and lineq1=" & key(1) & " and CUSID='" & key(2) & "' AND ENTRYNO=" & KEY(3)
    ' Response.Write sqlyy
     CONNXX.Execute sqlXX    
     If Err.number > 0 then
        endpgm="2"
        errmsg=cstr(Err.number) & "=" & Err.description
     else
        SQLXX=" update RTEBTCUSTCHG set dropdat=getdate(),dropUSR='" & V(0) & "' where comq1=" & KEY(0) & " and lineq1=" & key(1) & " and CUSID='" & key(2) & "' and entryno=" & key(3)
        connxx.Execute SQLXX
        If Err.number > 0 then
           endpgm="2"
           '發生錯誤時，刪除異動檔所新增的異動資料
           errmsg=cstr(Err.number) & "=" & Err.description
           sqlXX="delete * FROM RTEBTCUSTCHGlog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " and CUSID='" & key(2) & "' and entryno=" & key(3) & " AND seq=" & seq
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
       msgbox "用戶異動資料作廢成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此筆資料已被作廢，不可重覆作廢" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此筆異動已向EBT提出申請，不可作廢" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    else
       msgbox "無法執行用戶異動資料作廢,錯誤訊息：" & "  " & frm1.htmlfld1.value
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