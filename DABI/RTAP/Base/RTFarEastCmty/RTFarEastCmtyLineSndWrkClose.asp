<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   'SET RSyy=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   'sqlYY="select COUNT(*) AS CNT FROM RTfareastCUST WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CANCELDAT IS NULL AND DROPDAT IS NULL AND STRBILLINGDAT IS NOT NULL "
   'RSYY.OPEN SQLYY,CONNxx
   sqlxx="select * FROM RTfareastSndWrk WHERE wrkno ='" & KEY(0) & "' "
   RSXX.OPEN SQLXX,CONNxx
   wrktyp = RSXX("wrktyp")
   linedroptyp = RSXX("linedroptyp")
   workingdat = RSXX("workingdat")
   endpgm="1"
   if wrktyp="10" and LEN(workingdat) = 0 then
      endpgm="6"
   elseif wrktyp="07" and LEN(workingdat) = 0 then
      endpgm="7"
   elseif LEN(TRIM(RSXX("finishdat"))) = 0 then
      endpgm="3"
   elseif LEN(TRIM(RSXX("CANCELdat"))) <> 0 then
      endpgm="4"
   elseif LEN(TRIM(RSXX("closedat"))) <> 0 then
      endpgm="5"      
   ELSE
      '呼叫store procedure更新相關檔案
      strSP="update RTfareastSndWrk set closedat=getdate(),closeusr='" &V(0)& "' where wrkno='" &key(0)&"' "
      if wrktyp ="10" then
			strSP = strSP & " update RTfareastCmtyLine set hardwaredat='" & workingdat &"' where comq1=" &key(1)&" and lineq1 = " &key(2)
      elseif wrktyp ="07" then
			strSP = strSP & " update RTfareastCmtyLine set dropdat='" & workingdat &"', dropkind='" & linedroptyp &"' where comq1=" &key(1)&" and lineq1 = " &key(2)
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
   'RSYY.CLOSE
   connXX.Close
   SET RSXX=NOTHING
   'SET RSYY=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "遠傳大寬頻社區型主線派工單結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "主線派工單未完工，不可結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="4" then
       msgbox "主線派工單已作廢，不可結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="5" then
       msgbox "主線派工單已結案，不可重覆結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    elseIF frm1.htmlfld.value="6" then
       msgbox "派工單無[預定到位日]，不可結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    elseIF frm1.htmlfld.value="7" then
       msgbox "派工單無[預定撤線日]，不可結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    else
       msgbox "無法執行主線派工單結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action="" ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>