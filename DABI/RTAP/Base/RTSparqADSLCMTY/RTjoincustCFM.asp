<%@ Language=VBScript %>
<%
   key=request("key")
   arykey=split(key,";")
   Set conn=Server.CreateObject("ADODB.Connection")  
   Set rs=Server.CreateObject("ADODB.recordset")    
   DSN="DSN=RtLib"
   conn.Open DSN
  ' On Error Resume Next
   if session("COMQ1")="" or SESSION("COMN")="" then
      endpgm="3"
      errmsg="畫面停止太久,相關資訊已消失,請重新執行本作業!"
   else
      '讀取主線附掛電話
      sql="select * FROM rtsparqadslCMTY WHERE cutyid=" & SESSION("COMQ1")
      RS.Open SQL,CONN
      IF NOT RS.EOF THEN
         EXTTEL=RS("CMTYTEL")
      ELSE 
         EXTTEL=""
      END IF
      RS.Close
      
      
      '讀取該主線下已存在對帳序號的最大值
      'sql="select max(sphnno) as SPHNNO FROM rtsparqadslCust WHERE EXTTEL='" & EXTTEL & "' "
	  sql="select isnull(MAX(case sphnno when '' then 0 else sphnno end),0) AS SPHNNO499 from rtsparq499cust where rtrim(NCICCUSNO)='" & EXTTEL & "' "
	  rs.Open sql,conn
	  SPHNNO499 = Cint(rs("SPHNNO499"))
	  rs.Close
      
      sql="select isnull(MAX(case sphnno when '' then 0 else sphnno end),0) AS SPHNNO399 from rtsparqadslcust where rtrim(EXTTEL)='" & EXTTEL & "' "  
      rs.Open sql,conn
      SPHNNO399 = Cint(rs("SPHNNO399"))
      rs.Close

      if SPHNNO499 >= SPHNNO399 and SPHNNO499 <>0  then
		 NEWSPHNNO=right("000" & cstr(SPHNNO499 +1),3)
      elseif SPHNNO499 < SPHNNO399 then
		 NEWSPHNNO=right("000" & cstr(SPHNNO399 +1),3)
      elseif SPHNNO499 =0 and SPHNNO399 =0 then
		 NEWSPHNNO="001"
      end if


      'response.Write "sql=" & "update rtsparqadslcust set comq1=" & session("COMQ1") & ",housename='" & session("COMN") & "' where cusid='" & arykey(1) & "' and entryno=" & arykey(2)
      updsql="update rtsparqadslcust set comq1=" & session("COMQ1") & ",housename='" & session("COMN") & "' where cusid='" & arykey(1) & "' and entryno=" & arykey(2)
      conn.Execute updsql 
      updsql="update rtsparqadslcust set exttel='" & exttel & "',sphnno='" & newsphnno & "' where cusid='" & arykey(1) & "' and entryno=" & arykey(2) & " and docketdat is not null "
      conn.Execute updsql 
      conn.Close
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         endpgm="1"
         errmsg=""
      end if
   end if
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    on error resume next
    msgbox "aaa=" & frm1.htmlfld.value
    if frm1.htmlfld.value="1" then
    '   msgbox "客戶資料與社區檔已建立連結,請按[重新整理]呈現最新資料!",0
       Set winP=window.Opener
       Set docP=winP.document       
    '   docP.all("keyform").Submit
       winP.close             
       window.close
    else
   '    msgbox "無法建立連結,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTjoincustCFM.asp>
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form>
</html>