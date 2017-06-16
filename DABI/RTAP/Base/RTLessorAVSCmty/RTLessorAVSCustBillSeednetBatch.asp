<!-- #include virtual="/Webap/include/employeeref.inc" -->

<%
	Response.AddHeader "Content-Disposition","attachment; filename=334."& Year(now()) & right("0"& month(now()),2) & right("0"& day(now()), 2)
	response.ContentType = "text/plain"

	logonid=Request.ServerVariables("LOGON_USER")
	Call SrGetEmployeeRef(Rtnvalue,1,logonid)
	logonid=split(rtnvalue,";")  

    parm=request("key")
    v=split(parm,";")

    Dim rs,conn, formid
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    
	sql="UPDATE RTLessorAVSCustBillingPrt SET BARCODOUTDAT = getdate(), BARCODOUTUSR = '" &logonid(0)& "' WHERE BATCH ='" & v(0) &"' "
    conn.Execute(sql)
    
    Set rs=Server.CreateObject("ADODB.Recordset")    
    sql="select convert(varchar(7), convert(int, convert(varchar(8), dateadd(m, 2, a.duedat), 112)) - 19110000) +';'+ " &_
		"d.csnoticeid +';'+ " &_
		"convert(varchar(3), datepart(yy, a.duedat)-1911)+ substring(convert(varchar(6), a.duedat, 12), 3, 2) +';'+ " &_
		"convert(varchar(9), case b.secondcase when 'Y' then c.amt2 else c.amt end) +';'+ " &_
		"d.cscusid +';'+ " &_
		"' ' +';'+ " &_
		"c.memo +';'+ " &_
		"b.cusnc +';' as bcodesrc " &_
		"from	RTLessorAvsCustBillingPrtSub a " &_
		"inner join RTLessorAvsCust b on a.cusid = b.cusid " &_
		"inner join RTLessorAVSCustBillingBarcode d on d.noticeid = a.noticeid " &_
		"inner join RTBillCharge c on c.casekind = d.casekind and c.paycycle = d.paycycle and c.casetype ='07' " &_
		"where 	a.batch ='" & v(0) &"' " &_
		"order by b.comq1, b.lineq1, b.cusnc, c.paycycle "
'response.Write sql
    rs.Open sql, CONN
	Response.Write rs.GetString(2, -1, ",", vbCrLf, "")
	rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>

