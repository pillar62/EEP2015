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
   sqlxx="select * FROM hbcmtyarrangehardware WHERE comq1=" & KEY(0) & " and comtype='" & key(1)  & "' and prtno='" & key(2) & "' and entryno=" & key(3) 
   sqlZZ="select * FROM hbcmtyarrangeSNDWORK WHERE comq1=" & KEY(0) & " and comtype='" & key(1)  & "' and prtno='" & key(2) & "' "

   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   RSZZ.OPEN SQLZZ,CONNxx
   endpgm="1"
   '當設備明細尚未作廢時，不可作廢返轉
   IF ISNULL(RSXX("DROPDAT"))  THEN
      ENDPGM="3"
   '當派工單已結案時，不可執行設備明細作廢返轉
   elseif LEN(TRIM(RSZZ("CLOSEDAT"))) > 0 then
      endpgm="6"
   '當派工單已作廢時，不可執行設備明細作廢返轉
   elseif LEN(TRIM(RSZZ("DROPDAT"))) > 0 then
      endpgm="4"      
   '當獎金計算或庫存計算不為空白，不可執行作廢返轉
   elseif LEN(TRIM(RSZZ("BONUSCLOSEYM"))) <> 0 OR LEN(TRIM(RSZZ("STOCKCLOSEYM"))) <> 0 then
      endpgm="5"
   ELSE
      sqlyy="select max(seq) as seq FROM hbcmtyarrangehardwarelog WHERE comq1=" & KEY(0) & " and comtype='" & key(1) & "'  and prtno='" & key(2) & "'  and entryno=" & key(3) 
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("seq"))) > 0 then
         seq=rsyy("seq") + 1
      else
         seq=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into hbcmtyarrangehardwarelog " _
           &"SELECT  COMQ1, COMTYPE, PRTNO, ENTRYNO, " & SEQ & ", getdate(), 'R', '" & V(0) & "', PRODNO, ITEMNO, QTY, DROPDAT, DROPREASON, WAREHOUSE, ASSETNO,  DROPUSR, UNIT, EUSR, EDAT, UUSR, UDAT " _
           &" FROM hbcmtyarrangehardware where comq1=" & key(0) & " and comtype='" & key(1) & "' and prtno='" & key(2) & "'  and entryno=" & key(3) 
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update hbcmtyarrangehardware set DROPdat=NULL,DROPUSR='' where comq1=" & KEY(0) & " and comtype='" & key(1)  & "' and prtno='" & key(2) & "'  and entryno=" & key(3) 
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM hbcmtyarrangehardwarelog WHERE comq1=" & key(0) & " and comtype='" & key(1) & "' and prtno='" & key(2) & "' AND ENTRYNO=" & dspkey(3) & " and seq=" & seq
            CONNXX.Execute sqlyy 
         else
            endpgm="1"
            errmsg=""
         end if      
      end if
   END IF
   RSXX.CLOSE
   RSZZ.CLOSE
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "整線派工單設備明細作廢返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當設備資料尚未作廢時，不可作廢返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="6" then
       msgbox "此整線派工單已完工結案，設備明細不可作廢返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此整線派工單已月結，設備明細不可異動" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    elseIF frm1.htmlfld.value="4" then
       msgbox "此整線派工單已作廢，設備明細不可作廢返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close             
    else
       msgbox "無法執行整線派工單設備作廢返轉作業,錯誤訊息" & "  " & frm1.htmlfld1.value
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