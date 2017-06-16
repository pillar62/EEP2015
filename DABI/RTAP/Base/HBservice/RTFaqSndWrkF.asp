<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTSndWork WHERE workno='" & KEY(0) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   endpgm="1"
   if LEN(TRIM(RSXX("CANCELdat"))) <> 0 then
      endpgm="4"	'已作廢
   elseif LEN(TRIM(RSXX("finishdat"))) <> 0 then
      endpgm="5"	'已完工
   ELSE

      if RSXX("finisheng") ="" and RSXX("finishcons") ="" then 
      	strSP ="update RTSndWork set finishdat=convert(datetime,convert(varchar(10),getdate(),111)),finishusr='" &key(2)& "',finishtyp='" &key(1)& "', finisheng=assigneng, finishcons=assigncons WHERE workno='" & key(0) & "' "
      else
        strSP ="update RTSndWork set finishdat=convert(datetime,convert(varchar(10),getdate(),111)),finishusr='" &key(2)& "',finishtyp='" &key(1)& "' WHERE workno='" & key(0) & "' "
	  end if
	  
	  '轉匯至行程表	  
	  if key(3) ="Y" then 
		strSP = strSP &_
				" DELETE FROM RTSalesSch WHERE workno<>'' and workno ='"& key(0) &"' and canceldat is null " &_
				"INSERT INTO RTSalesSch (SCHNO, COMTYPE, COMQ1, LINEQ1, " &_
				"CUSID, ENTRYNO, WORKNO, DEALUSR, DEALDAT, UUSR, " &_
				"SCORE01, SCORE02, SCORE03, SCORE04, SCORE05, SCORE06, SCORE07, SCORE08, " &_
				"SCORE09, SCORE10, SCORE11, SCORE12, SCORE13, SCORE14, SCORE15) " &_
				"SELECT c.maxschno, b.COMTYPE, b.COMQ1, b.LINEQ1, b.CUSID, b.ENTRYNO, a.WORKNO, " &_
				"a.FINISHENG, isnull(a.FINISHDAT, convert(datetime,convert(varchar(10),getdate(),111))), a.FINISHENG, " &_
				"a.SCORE01, a.SCORE02, a.SCORE03, a.SCORE04, a.SCORE05, a.SCORE06, a.SCORE07, a.SCORE08, " &_
				"a.SCORE09, a.SCORE10, a.SCORE11, a.SCORE12, a.SCORE13, a.SCORE14, a.SCORE15 " &_
				"FROM RTSndWork a " &_
				"INNER JOIN RTFaqM b ON a.LINKNO = b.CASENO " &_
				"inner join (select 'S'+ left(convert(varchar(8), getdate(),112),6)+ " &_
				"isnull(right('000'+convert(varchar(4),convert(smallint,right(max(SCHNO),4))+1),4), '0001') as maxschno " &_
				"from RTSalesSch where SCHNO like 'S'+left(convert(varchar(8), getdate(),112),6)+'%') c on 1=1 " &_
				"where a.workno ='"& key(0) &"' "
	  end if
      Set ObjRS = connXX.Execute(strSP)
	  
      If Err.number = 0 then
         ENDPGM="1"
         ERRMSG=""
         'conn.CommitTrans
      else
         ENDPGM="2"
         errmsg=cstr(Err.number) & "=" & Err.description
         'conn.rollbackTrans
      end if         
   END IF
   RSXX.CLOSE
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
%> 
<HTML>
<Head>
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "派工單結案成功",0
       Set winDialog=window.Opener
       Set winP=winDialog.Opener
       Set docP=winP.document
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
       winDialog.close
    elseIF frm1.htmlfld.value="4" then
       msgbox "派工單已作廢，不能結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    elseIF frm1.htmlfld.value="5" then
       msgbox "請勿重覆押完工。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    else
       msgbox "無法執行派工單作廢作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTFaqSndWrkF.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>