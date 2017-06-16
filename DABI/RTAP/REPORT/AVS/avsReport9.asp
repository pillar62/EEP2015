<style>
<!--
.toChar
	{font-size:10.0pt;mso-number-format:"\@";border:0.5pt solid black;}
.toNum
	{font-size:10.0pt;border:0.5pt solid black;}
.titleY
	{font-size:10.0pt;font-weight:bold;background:peachpuff;border:0.5pt solid black;}
.titleN
	{font-size:10.0pt;font-weight:bold;background:silver;border:1.0pt solid black;}
.title
	{font-size:10.0pt;font-weight:bold;border:1.0pt solid black;}
-->
</style>

<%
    parm=request("parm")
    v=split(parm,";")

    Dim rs,conn, formid
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    sqlstr="select	a.comq1, a.lineq1, a.cusid, c.comn, a.cusnc, a.socialid, "_
		  &"case d.telno  when '' then '　' else d.telno end as telno, "_
		  &"case d.telno  when '' then 0 else 1 end as telnum, "_
		  &"a.avsno, a.docketdat, '己報竣退租' as remark "_
		  &"from	rtebtcust a "_
		  &"inner join rtebtcmtyline b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 "_
		  &"inner join rtebtcmtyh c on c.comq1 = b.comq1 "_
		  &"left outer join rtebtcustext d on d.comq1 = a.comq1 and d.lineq1 = a.lineq1 "_
		  &"and d.cusid = a.cusid and d.dropdat is null "_
		  &"WHERE	a.dropdat Between '"& v(0) &"' and '"& v(1) &"' "_
		  &"order by 4,5 "
'response.Write sqlstr    
    rs.Open sqlstr, CONN
    
    
    ' 明細表 ===================================================================================
	'Response.Charset ="BIG5"    
	'Response.ContentType = "Content-Language;content=zh-tw"     
	'response.ContentType = "application/vnd.ms-excel"
	'Response.AddHeader "Content-Disposition","filename=AVS退租回覆清單.xls"
	response.Write "<table>"
	'response.Write "<tr><td align =center colspan=7><b>元訊寬頻網路股份有限公司</b></td></tr>"
	'response.Write "<tr><td align =center colspan=2><font size=2><u>退租日"& v(0)&"～"& v(1)&"用戶</u></font></td></tr>"
	'response.Write "<tr><td align =right colspan=11><font size=2>製表日期：" &now()& "</font></td></tr>"
	response.Write "<tr><td align =center colspan=7 rowspan=5 class=title><b><font size=4>元訊退租 / 回覆清單</font></b></td>"
	response.Write "<td align =left colspan=2 class=title><font size=2>進件日期：" &v(0)& "</font></td></tr>"
	response.Write "<tr><td align =left colspan=2 class=title><font size=2>進件單位：元訊</font></td></tr>"	
	response.Write "<tr><td align =left colspan=2 class=title><font size=2>聯絡人員：曹姿怡</font></td></tr>"
	response.Write "<tr><td align =left colspan=2 class=title><font size=2>聯絡電話：02-26552888#311</font></td></tr>"
	response.Write "<tr><td align =left colspan=2 class=title><font size=2>傳真電話：02-26552940</font></td></tr>"
    response.Write "<TR>" &_
				   "<td class=titleY>流水號</td>" &_
				   "<td class=titleY>社區名稱</td>" &_
				   "<td class=titleY>用戶名稱</td>" &_
				   "<td class=titleY>統編 / ID</td>" &_
				   "<td class=titleY>申裝號碼</td>" &_
				   "<td class=titleY>申請線數</td>" &_
				   "<td class=titleY>合約編號</td>" &_
				   "<td class=titleY>報竣日</td>" &_
				   "<td class=titleY>備註</td>" &_
				   "</TR>"
	SERNO =0
	CbcSum = 0
	Do While Not rs.Eof
		SERNO = SERNO +1
		CbcSum = CbcSum + rs("telnum")
	    response.Write "<TR>" &_
				   "<td class=toNum>"& SERNO &"</td>" &_
				   "<td class=tochar>"& rs("COMN") &"</td>" &_
				   "<td class=tochar>"& rs("CUSNC") &"</td>" &_
				   "<td class=tochar>"& rs("SOCIALID") &"</td>" &_
				   "<td class=tochar>"& rs("TELNO") &"</td>" &_
				   "<td class=toNum>"& rs("TELNUM") &"</td>" &_
				   "<td class=tochar>"& rs("AVSNO") &"</td>" &_
				   "<td class=tochar>"& rs("DOCKETDAT") &"</td>" &_
				   "<td class=tochar>"& rs("REMARK") &"</td>" &_
				   "</TR>"
      rs.MoveNext
    Loop
    rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
	response.Write "</table>"  
	

' 小計表 ========================================================================================
	response.Write "<br><br><table>"
    response.Write "<TR>" &_
				   "<td colspan=2 style='mso-ignore:colspan'></td>" &_
   				   "<td colspan=2><font size=2>送件單位專用:</font></td>" &_
   				   "<td colspan=2></td>" &_
   				   "<td colspan=2><font size=2>開通中心專用:</font></td>" &_
				   "</TR>" &_
					
				   "<TR>" &_
				   "<td colspan=2 style='mso-ignore:colspan'></td>" &_
   				   "<td class=titleY align=center>產品</td>" &_
				   "<td class=titleY align=center>件數/線數</td>" &_
   				   "<td colspan=2></td>" &_
   				   "<td class=titleY align=center>產品</td>" &_
				   "<td class=titleY align=center>件數/線數</td>" &_
				   "</TR>" &_
				   
				   "<TR>" &_
				   "<td colspan=2 style='mso-ignore:colspan'></td>" &_
   				   "<td class=titleN>AVS</td>" &_
				   "<td class=tochar>　" &SERNO& "　件　" &SERNO&"　線</td>" &_
   				   "<td colspan=2></td>" &_
   				   "<td class=titleN>AVS</td>" &_
				   "<td class=tochar>　　件　　線</td>" &_
				   "</TR>" &_
				   
				   "<TR>" &_
				   "<td colspan=2 style='mso-ignore:colspan'></td>" &_
   				   "<td class=titleN>CBC</td>" &_
				   "<td class=tochar>　" &CbcSum& "　件　" &CbcSum&"　線</td>" &_
   				   "<td colspan=2></td>" &_
   				   "<td class=titleN>CBC</td>" &_
				   "<td class=tochar>　　件　　線</td>" &_
				   "</TR>" &_

				   
				   "<TR>" &_
				   "<td colspan=2 style='mso-ignore:colspan'></td>" &_
   				   "<td class=titleN>EM</td>" &_
				   "<td class=tochar>　　件　　線</td>" &_
   				   "<td colspan=2></td>" &_
   				   "<td class=titleN>AVS</td>" &_
				   "<td class=tochar>　　件　　線</td>" &_
				   "</TR>" &_

				   "</table>"

%>
