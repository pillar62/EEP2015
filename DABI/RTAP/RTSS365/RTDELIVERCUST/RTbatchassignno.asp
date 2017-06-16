
<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
   logonid=session("userid")
   sqllist399=session("SQLlist399")   
   sqllist599=session("SQLlist599")   
   SQLlistx=session("sqllistx")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   Set conn=Server.CreateObject("ADODB.Connection")  
   set rs=server.CreateObject("ADODB.recordset")
   set rs2=server.CreateObject("ADODB.recordset")   
   set rs399599=server.CreateObject("ADODB.recordset")      
 'A  set rs599=server.CreateObject("ADODB.recordset")         
   DSN="DSN=RtLib"
   conn.Open DSN
   '計算挑選資料中399及599的客戶筆數各為幾筆,做為檢查帳號檔現有未使用帳號數量是否足夠之計算基準
   rs.Open sqllist399,conn,1,1
   RScnt399=rs.recordcount
   rs.Close
   rs.Open sqllist599,conn,1,1
   rscnt599=rs.recordcount
   rs.close
   '計算rt365account檔內最大序號
   YY=cstr(datepart("yyyy",now()))
   mm=right(cstr("0" & cstr(datepart("m",now()))),2)
   dd=right(cstr("0" & cstr(datepart("d",now()))),2)
   YYMMDD=yy & mm & dd
   Set rsc=Server.CreateObject("ADODB.Recordset")
   sqlstr2="select max(batchno) AS batchno from rt365account where  batchno like '" & yymmdd & "%'" 
   rsc.open sqlstr2,conn
   newbatchno=""
   if len(rsc("batchno")) > 0 then
      Newbatchno=yymmdd & right("000" & cstr(cint(mid(rsc("batchno"),9,3)) + 1),3)
   else
      Newbatchno=yymmdd & "001"
   end if        
   '計算帳號檔內現有可使用之帳號中399及599數量各為多少
   sql399599="select * from rt365account where ( cusid = '' or cusid is null ) "
   rs2.Open sql399599,conn,1,1 
   RS2Cnt399599=rs2.RecordCount
   rs2.close
 '  sql599="select * from rt365account where ( cusid = '' or cusid is null ) and type='599' "
 '  rs2.Open sql599,conn,1,1 
 '  RS2Cnt599=rs2.RecordCount   
 '  rs2.Close
   '判斷399與599的存號是否足夠
   '(900920:由於不分399與599的帳號....所以本段程式停用
   '========================================================================================================================
   'if rscnt399 > rs2cnt399 then
   '   endpgm="3"
   '   errmsg="先看先贏399帳號不足: 現有帳號與本次所需帳號數量為=>" & rs2cnt399 & ":" & rscnt399 & "差異數:" & rscnt399 - rs2cnt399
   'elseif rscnt599 > rs2cnt599 then
   '   endpgm="4"
   '   errmsg="先看先贏599帳號不足: 現有帳號與本次所需帳號數量為=>" & rs2cnt599 & ":" & rscnt599 & "差異數:" & rscnt599 - rs2cnt599
   'else
   ''----依據客戶類型(399 or 599)賦予不同之帳號
   '   on error resume next
   '   '................+
   '   conn.begintrans
   '   '----------------+
   '   rs399.Open sql399,conn,3,3
   '   rs599.Open sql599,conn,3,3
   '   rs.Open sqllistx,conn,1,1
   '   Do while not rs.EOF 
   '      if LEN(TRIM(rs("ss365")))=0 then
   '         rs399("cusid")=rs("cusid")
   '         rs399("entryno")=rs("entryno")
   '         rs399("usedate")=now()
   '         rs399("batchno")=newbatchno
   '         rs399.update
   '         rs399.movenext         
   '      else
   '         rs599("cusid")=rs("cusid")
   '         rs599("entryno")=rs("entryno")
   '         rs599("usedate")=now()
   '         rs599("batchno")=newbatchno            
   '         rs599.update
   '         rs599.movenext               
   '      end if 
   '      rs.movenext
   '   loop
   '   rs.close
   '   rs399.Close
   '   rs599.Close
   '   set rs=nothing
   '   set rs399=nothing
   '   set rs599=nothing
   'end if
   '=900920 ADD BEGIN===================================================================================================
   rscntTOT=rscnt399 + rscnt599
  ' rs2cntTOT=rs2cnt399 + rs2cnt599
   if rscntTOT > Rs2CNT399599 then
      endpgm="3"
      errmsg="先看先贏帳號不足: 現有帳號與本次所需帳號數量為=>" & rs2cnt399599 & ":" & rscntTOT & "(399與599合計);差異數:" & rscntTOT - rs2cnt399599
   else
   '----賦予不同之帳號
      on error resume next
   '................+
      conn.begintrans
   '----------------+
      rs399599.Open sql399599,conn,3,3
      rs.Open sqllistx,conn,1,1
     ' Response.Write "rs2cnt=" & rscnttot &";rs2cnttot=" & rs2cnttot & ";current=" & rs.recordcount
      Do while not rs.EOF 
            rs399599("cusid")=rs("cusid")
            rs399599("entryno")=rs("entryno")
            rs399599("usedate")=now()
            rs399599("batchno")=newbatchno
            rs399599.update
            rs399599.movenext         
            rs.movenext
      loop
      rs.close
      rs399599.Close
      set rs=nothing
      set rs399=nothing
      set rs599=nothing   
   end if
   '=900920 ADD END=====================================================================================================
   If Err.number <> 0 then
      endpgm="2"
      errmsg=cstr(Err.number) & "=" & Err.description
      conn.rollbacktrans
   elseif endpgm="3" or endpgm="4" then
   else
      endpgm="1"
      errmsg=""
      conn.commitTrans
   end if
   conn.Close

%>
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "整批給號作業完成",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()     
       WINDOW.CLOSE         
    else
       msgbox "給號過程發生錯誤,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=verify.asp>
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
</html>
