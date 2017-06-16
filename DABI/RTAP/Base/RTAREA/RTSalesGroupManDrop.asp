<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% dim parmkey,aryparmkey,logonid,conn,rs,sql
   parmKey=Request("Key")
   aryParmKey=Split(parmKey &";;;;;;;;;;;;;;;",";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   Set conn=Server.CreateObject("ADODB.Connection")  
   Set rs=server.CreateObject("ADODB.Recordset")
   DSN="DSN=RtLib"
   sql="select Version,sdate,edate from RTSalesGroupRef where " _
      &"version=(select MAX(version) FROM RTSalesGroupRef WHERE areaid='" & aryparmkey(0) & "' and groupid='" & aryparmkey(1) & "' and emply='" & aryparmkey(2) & "')"
   conn.Open DSN
   rs.Open sql,conn,3,3
   '-----當刪除之版次小於檔案中該轄區/組別/業務員的最大版次時=>表示仍有最新之異動紀錄，故控制不可刪除
   If len(trim(rs("version"))) > 0 or rs("version")=0 then
      MAXNO=rs("version")
      '----紀錄該筆異動之起迄日期的年度及月份(做為檢查月結控制檔的基準--若截止日期edate is null表示該筆資料目前正生效中，以系統日期為截止日)
      if not IsNull(rs("SDATE")) then
         SYY=datepart("yyyy",rs("SDate"))
         SMM=right("0" & datepart("m",rs("Sdate")),2)
         SYYMM=SYY & SMM
      else
         SYY=0
         SMM=0
      End if
      if not IsNull(rs("EDATE")) then
         EYY=datepart("yyyy",rs("EDate"))
         EMM=right("0" & datepart("m",rs("Edate")),2)
         EYYMM=EYY & EMM
      else
         EYY=right("0" & datepart("yyyy",now()),2)
         EMM=right("0" & datepart("m",now()),2)
         EYYMM=EYY & EMM
      End if
      '------------
      if cstr(aryparmkey(3)) < cstr(maxno) then
         errmsg=maxno
         endpgm="2"
      else
         '----檢查該版次其生效日期至截止日期中，是否曾有月結記錄(以轄區及年度月份至月結控制檔讀取月結紀錄)
         Set rs2=server.CreateObject("ADODB.Recordset")
         sql2="SELECT COUNT(*) AS CNT FROM RTClosingCTl " _
             &"WHERE LTRIM(STR(cyy)) + RIGHT('0' + LTRIM(STR(cmm)), 2) >= '" & Syymm & "' AND " _
             &"LTRIM(STR(cyy)) + RIGHT('0' + LTRIM(STR(cmm)), 2) <= '" & EYYMM & "' AND closing = 'Y' "
         rs2.Open sql2,conn           
         '---該筆異動資料中已有月結記錄時，不可刪除
         if rs2("cnt") > 0 then
            endpgm="3"
         '---正常，刪除之（生效日至截止日中無月結記錄）
         else
            endpgm="1"
         end if
         rs2.close
         set rs2=nothing
       end if
    end if
    rs.close
    set rs=nothing    
    '----刪除資料
    if endpgm="1" then
       sql="delete  from  RTSalesGroupRef where areaid='" & aryparmkey(0) & "' and groupid='" & aryparmkey(1) & "' and " _
          &"emply='" & aryparmkey(2) & "' and version=" & aryparmkey(3)
       conn.Execute sql 
    end if
    conn.Close

%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "業務組別與業務員關係資料作廢成功",0
       Set winP=window.Opener
       Set docP=winP.document
       docP.all("keyform").Submit
       winP.focus()              
    elseif frm1.htmlfld.value="2" then 
        msgbox "此業務員已有最新的異動版次(版次為:'" & frm1.htmlfld1.value & "')" & "請從最近一次異動刪除" 
    elseif frm1.htmlfld.value="3" then 
       msgbox "該異動版次的生效日期至截止日期間，已執行月結處理，故不可刪除" & "  " & errmsg
    end if
    window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action="RTFaqDropK.asp">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
</html>