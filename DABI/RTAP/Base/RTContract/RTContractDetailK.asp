<%
parmkey=request("key")
aryParmKey=Split(parmKey &";;;;;;;;;;;;;;;",";")
DIM conn,rs,dsn,sql
SET conn=server.CreateObject("ADODB.Connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
conn.Open dsn
SQL="select * from HBContractH where CTNO=" & aryparmkey(0)
rs.Open sql,conn
if NOT rs.EOF THEN
   IF rs("CTPROPERTY")="A" or rs("CTPROPERTY")="B" or rs("CTPROPERTY")="E" or rs("CTPROPERTY")="F"  then
      PGMID="1"
      PGMNAME="RTContractkind1k.asp" & "?key=" & parmkey
   ELSE
      PGMID="2"
      PGMNAME="RTContractkind2k.asp" & "?key=" & parmkey      
   end if
END IF
rs.close
'檢查是否存在業務類產品明細資料 ==> IF YES,則不管業務類別為何...依產品明細檔存在之資料為主,帶出明細資料所屬之畫面
SQL="select  COUNT(*) as CNT from HBContractSalesD where CTNO=" & aryparmkey(0)
rs.Open sql,conn
IF RS("CNT") > 0 THEN
   PGMNAME="RTContractkind1k.asp" & "?key=" & parmkey
END if
rs.close
'檢查是否存在管理類產品明細資料 ==> IF YES,則不管業務類別為何...依產品明細檔存在之資料為主,帶出明細資料所屬之畫面
SQL="select COUNT(*) AS CNT from HBContractMaintain where CTNO=" & aryparmkey(0)
rs.Open sql,conn
IF RS("CNT") > 0  THEN
   PGMNAME="RTContractkind2k.asp" & "?key=" & parmkey
END if
rs.Close
conn.Close
set rs=nothing
set conn=nothing
%>
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       window.frm2.submit
    else
       window.frm2.submit
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=verify.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=PGMID%>" ID="Text1">
</form>
<form name=frm2 method=post action=<%=pgmname%> ID="Form2">
</form>
</html>