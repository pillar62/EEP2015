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
   sqlxx="select * FROM KTSWANTGOCUST WHERE CUSID='" & KEY(0) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   endpgm="1"
   '當已作廢時，不可拉單
   IF NOT ISNULL(RSXX("CANCELDAT")) THEN
      ENDPGM="3"
   '當已拉單時，不可重覆拉單
   elseif NOT ISNULL(RSXX("CBBNPULLDAT"))  then
      endpgm="4"
   ELSE
      '計算KTSCUST的KEY值
       Set rsc=Server.CreateObject("ADODB.Recordset")
       cusidxx="K" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
       sql="select max(cusid) AS cusid from KTSCUST where cusid like '" & cusidxx & "%' "
      ' response.Write sql
       rsc.open sql,connxx
       if len(rsc("cusid")) > 0 then
          NEWKEY=cusidxx & right("000" & cstr(cint(right(rsc("cusid"),3)) + 1),3)
       else
          NEWKEY=cusidxx & "001"
       end if
       rsc.close
      '新增用戶資料到KTSCUST 及 KTSCUSTD1
      SQLXX="INSERT INTO KTSCUST SELECT '" & NEWKEY & "', CUSNC, SOCIALID, BUSINESSTYPE, COTEL11, COTEL12, COFAX11, COFAX12, COEMAIL, " _
           &"CUTID1, TOWNSHIP1, RADDR1, RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, CUTID3, TOWNSHIP3, RADDR3, RZONE3,COBOSS," _
           &"BOSSSOCIALID, COCONTACTMAN, COCONTACTTEL11, COCONTACTTEL12, COCONTACTTEL13, COCONTACTFAX11, COCONTACTFAX12, " _
           &"COCONTACTMOBILE, COCONTACTEMAIL, APFORMAPPLYDAT, APPLYDAT, APPLYTNSDAT, CONTRACTSTRDAT, NCICAPPLYREPLYDAT, " _
           &"NCICCUSID, NCICOPENDAT, FINISHDAT, DOCKETDAT, TRANSDAT, CANCELDAT,CANCELUSR, DROPDAT, NCICDROPFLAG, " _
           &"RUNONCEBILLDAT,RUNONCESALESDAT, CONSIGNEE1, CONSIGNEE2, EMPLY, EUSR, EDAT, " _
           &"UUSR, UDAT, LISTTELDETAIL, SERVICE0809,GETDATE(),'" & V(0) & "' " _
           &"FROM KTSWANTGOCUST WHERE CUSID='" & KEY(0) & "' "
      connxx.Execute SQLXX    
       '計算KTSCUSTD1的ENTRYNO值
      rsc.open "select max(ENTRYNO) AS ENTRYNO from KTSCUSTD1 where cusid='" & NEWKEY & "' " ,connxx
      if NOT RSC.EOF then
         STRENTRYNO=1
      else
         STRENTRYNO=RSC("ENTRYNO") + 1
      end if
      rsc.close     
      '新增電話明細
      SQLXX="SELECT * from KTSWANTGOCUSTD1 WHERE CUSID='" & KEY(0) & "' and canceldat is null"
      RSyy.Open SQLXX,CONNXX
      DO WHILE NOT RSyy.EOF
         SQLZZ="INSERT INTO KTSCUSTD1(CUSID,ENTRYNO,TEL11,TEL12,APPLYDAT,applyno,dropno) " _
              &"VALUES('" & NEWKEY & "'," & STRENTRYNO & ",'" & RSyy("TEL11") & "','" & RSyy("TEL12") & "','" & RSyy("applydat") & "','','') " 
       '  response.Write sqlzz
         CONNXX.Execute SQLZZ
         STRENTRYNO=STRENTRYNO + 1
         RSyy.MOVENext
      LOOP
      RSyy.CLOSE
      If Err.number > 0 then
         endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
         errmsg=cstr(Err.number) & "=" & Err.description
         sqlyy="delete * FROM KTSCUST WHERE CUSID='" & NEWKEY & "' "
         CONNXX.Execute sqlyy 
      ELSE
        '新增成功後壓拉單日
         SQLXX=" update KTSWANTGOCUST set CBBNPULLDAT=getdate(),CBBNPULLUSR='" & V(0) & "' where CUSID='" & KEY(0) & "'"
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            '刪除已新增的客戶檔
            sqlyy="delete * FROM KTSCUST WHERE CUSID='" & NEWKEY & "'"
            CONNXX.Execute sqlyy 
            '刪除已新增的電話檔
            sqlyy="delete * FROM KTSCUSTd1 WHERE CUSID='" & NEWKEY & "'"
            CONNXX.Execute sqlyy             
         else
            endpgm="1"
            errmsg=""
         end if      
       END IF
     
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
       msgbox "KTS用戶拉單成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當已作廢時，不可拉單" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "當已拉單時，不可重覆拉單" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    else
       msgbox "發生異常，無法執行KTS用戶拉單作業，請通知資訊部" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=KTSCUSTPULL.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>