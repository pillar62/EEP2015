<%@ LANGUAGE = VBScript %>

<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
	logonid=Request.ServerVariables("LOGON_USER")
	Call SrGetEmployeeRef(Rtnvalue,1,logonid)
	logonid=split(rtnvalue,";")  

    parm=request("key")
    v=split(parm,";")
%>

<!-- #include virtual="/Webap/include/uploadx.asp" -->
<!-- #include virtual="/Webap/include/database.inc" -->
<%
'Response.Write "<br>Name=""" & GetFormVal("name") & """"
'Response.Write "<br>Sex=""" & GetFormVal("sex") & """"
'Response.Write "<br>province=""" & GetFormVal("province") & """"
'Response.Write "<br>city=""" & GetFormVal("city") & """"
'Response.Write "<br>lover=""" & GetFormVal("lover") & """"
dim filename
path = Server.MapPath("./") & "\barcode\"
filename = SaveFile("fruit",path,4096,0)
If	filename <> "*TooBig*" Then
    Set conn=Server.CreateObject("ADODB.Connection")
	conn.Open DSN
	'開暫存table
	sql="if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[##RTLessorCustBillingTemp]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) " &_
		"drop table [dbo].[##RTLessorCustBillingTemp]"
    conn.Execute(sql)
	sql="CREATE TABLE [dbo].[##RTLessorCustBillingTemp] ( " &_
		"	[CSCUSID] [varchar] (15) COLLATE Chinese_Taiwan_Stroke_CI_AS NOT NULL , " &_
		"	[CSBARCOD] [varchar] (40) COLLATE Chinese_Taiwan_Stroke_CI_AS NOT NULL , " &_
		"	[ATMCOD] [varchar] (16) COLLATE Chinese_Taiwan_Stroke_CI_AS NOT NULL )"
    conn.Execute(sql)

    '匯入文字檔至暫存table
    sql= "BULK INSERT dbo.##RTLessorCustBillingTemp FROM '"& path & filename &"' "&_
      	 "WITH (FIRSTROW =1, FORMATFILE ='"& Server.MapPath("./") & "\upload.fmt') "
    conn.Execute(sql)
    
    '更新mis資料庫    
    sql="declare @countTemp int, @countJoin int " &_

		"select @countTemp = count(*)  from	##RTLessorCustBillingTemp a " &_

		"select 	@countJoin = count(*) " &_
		"from	##RTLessorCustBillingTemp a " &_
		"		inner join RTLessorCustBillingBarcode b on a.cscusid = b.cscusid " &_
		"		inner join RTLessorCustBillingPrtSub c on c.noticeid = b. noticeid " &_
		"where 	c.batch ='" & v(0) &"' " &_

		"IF @countTemp = @countJoin BEGIN" &_
		"	update RTLessorCustBillingBarcode " &_
		"	set csbarcod1 =  left(a.csbarcod, 9), csbarcod2 = substring(a.csbarcod, 10, 16), " &_
		"		csbarcod3 = right(a.csbarcod, 15), atmcod=a.atmcod " &_
		"	from	##RTLessorCustBillingTemp a " &_
		"			inner join RTLessorCustBillingBarcode b on a.cscusid = b.cscusid " &_
		"			inner join RTLessorCustBillingPrtSub c on c.noticeid = b. noticeid " &_
		"	where 	c.batch ='" & v(0) &"' " &_
		
		"	update RTLessorCustBillingPrt set BARCODINDAT = getdate(), BARCODINUSR ='" &logonid(0)& "' where batch ='" & v(0) &"' " &_
		"END "
    conn.Execute(sql)

    set conn = nothing
    Response.Write "<br><br>""" & filename & """條碼檔上傳完畢..."
Else
	Response.Write "<br><br>文件大小超出 4 MB的限制"
End IF

'filename = SaveFile("fruit2",path,1024,0)
'If filename <> "*TooBig*" Then
'Response.Write "<br><br>""" & filename & """已經上傳"
'Else
'Response.Write "<br><br>文件超出限制太大"
'End IF
%>
 

<html>
	<head>
		<title>上傳結果</title>
		
<script language=vbscript>
Sub cmdcancel_onClick
  window.close
End Sub
</script>		

	</head>
	<BODY style="BACKGROUND: lightblue">
		<p><center>
			  <INPUT TYPE="button" VALUE="關閉" ID="cmdcancel" NAME="cmdcancel">
		</center><HR>			  
		</p>
	</body>
</html>
