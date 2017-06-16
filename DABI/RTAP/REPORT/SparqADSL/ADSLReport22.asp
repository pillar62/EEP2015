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
-->
</style>

<%
    parm=request("parm")
    v=split(parm,";")

    Dim rs,conn, formid
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    sql="usp_RTSparq499AreaScore '"&v(0)&"' "

   rs.Open sql, CONN
     
'response.Write sqlstr    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=Sparq499各組累計總表"
	response.Write "<table>"
	response.Write "<tr><td align =center colspan=8><b>元訊寬頻網路股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=8><b>Sparq499各組累計總表</b></td></tr>"
	response.Write "<tr><td align =left colspan=8><font size=2><u>統計期間 至： "&v(0)&" </u></font></td></tr>"
	response.Write "<tr><td align =right colspan=8><font size=2>製表日期：" &now()& "</font></td></tr>"
	
    response.Write "<TR>" &_
    			   "<td class=titleY>業務組別</td>" &_
				   "<td class=titleY>總社區數</td>" &_
				   "<td class=titleY>總主線數</td>" &_
				   "<td class=titleY>總申請戶數</td>" &_
				   "<td class=titleY>總開通數</td>" &_
				   "<td class=titleY>總安裝戶數</td>" &_
				   "<td class=titleY>累計報竣數</td>" &_
				   "<td class=titleY>平均戶數</td>" &_				   				   				   
				   "</TR>"
	
	sumflag =rs("breakid")
	sumCmty = 0
	sumLine = 0
	sumApply = 0
	sumOpen = 0
	sumFinish = 0	
	sumDocket = 0		
	sumAvg = 0		
	Do While Not rs.Eof
		if sumflag <> rs("breakid") then
			response.Write "<TR>" &_
				   "<td class=titleN>各組總計</td>" &_
   				   "<td class=titleN>" &sumCmty& "</td>" &_				   
				   "<td class=titleN>" &sumLine& "</td>" &_
				   "<td class=titleN>" &sumApply& "</td>" &_
				   "<td class=titleN>" &sumOpen& "</td>" &_
				   "<td class=titleN>" &sumFinish& "</td>" &_
				   "<td class=titleN>" &sumDocket& "</td>" &_
				   "<td class=titleN>" &sumAvg& "</td>" &_				   
				   "</TR>"
			totalCmty = sumCmty
			totalLine = sumLine
			totalApply = sumApply
			totalOpen = sumOpen
			totalFinish = sumFinish		
			totalDocket = sumDocket					
			totalAvg = sumAvg
			sumCmty = 0
			sumLine = 0
			sumApply = 0
			sumOpen = 0
			sumFinish = 0	
			sumDocket = 0		
			sumAvg = 0		
		end if 

	    response.Write "<TR>" &_
				   "<td class=tochar>"& rs("breaknc") &"</td>" &_				   
				   "<td class=tonum>"& rs("numCmty") &"</td>" &_
				   "<td class=tonum>"& rs("numLine") &"</td>" &_
				   "<td class=tonum>"& rs("numApply") &"</td>" &_
				   "<td class=tonum>"& rs("numOpen") &"</td>" &_
				   "<td class=tonum>"& rs("numFinish") &"</td>" &_
				   "<td class=tonum>"& rs("numDocket") &"</td>" 
		if rs("numOpen") = 0 then 
			response.Write "<td class=tonum>0</td>"
		else				   		
			response.Write "<td class=tonum>"& Round(rs("numDocket")/rs("numOpen"), 1) &"</td>" 
		end if				   
		response.Write "</TR>"

		sumCmty = sumCmty + rs("numCmty")
		sumLine = sumLine + rs("numLine")
		sumApply = sumApply + rs("numApply")
		sumOpen = sumOpen + rs("numOpen")
		sumFinish = sumFinish + rs("numFinish")
		sumDocket = sumDocket + rs("numDocket")
		if rs("numOpen") <> 0 then sumAvg = sumAvg + Round(rs("numDocket")/rs("numOpen"), 1)
      
      sumflag =rs("breakid")      
      rs.MoveNext      
    Loop

	response.Write "<TR>" &_
		   "<td class=titleN>經銷商總計</td>" &_
		   "<td class=titleN>" &sumCmty& "</td>" &_				   
		   "<td class=titleN>" &sumLine& "</td>" &_
		   "<td class=titleN>" &sumApply& "</td>" &_
		   "<td class=titleN>" &sumOpen& "</td>" &_
		   "<td class=titleN>" &sumFinish& "</td>" &_
		   "<td class=titleN>" &sumDocket& "</td>" &_
		   "<td class=titleN>" &sumAvg& "</td>" &_				   
		   "</TR>"

	totalCmty = totalCmty + sumCmty
	totalLine = totalLine + sumLine
	totalApply = totalApply + sumApply
	totalOpen = totalOpen + sumOpen
	totalFinish = totalFinish + sumFinish
	totalDocket = totalDocket + sumDocket
	totalAvg = totalAvg + sumAvg
	response.Write "<TR>" &_
		   "<td class=titleY>全部合計</td>" &_
   		   "<td class=titleY>" &totalCmty& "</td>" &_				   
		   "<td class=titleY>" &totalLine& "</td>" &_
		   "<td class=titleY>" &totalApply& "</td>" &_
		   "<td class=titleY>" &totalOpen& "</td>" &_
		   "<td class=titleY>" &totalFinish& "</td>" &_
		   "<td class=titleY>" &totalDocket& "</td>" &_
		   "<td class=titleY>" &totalAvg& "</td>" &_		   		   		   
		   "</TR>"
    
	response.Write "</table>"  
    
    rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>
