<%
    parm=request("parm")
    v=split(parm,";")

	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=自營有效+到期+退租名單("& year(v(0))-1911 & right("0"& month(v(0)),2) & right("0"& day(v(0)),2) & ").xls"

    Dim rs,conn
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
%>

<?xml version="1.0" encoding="Big5"?>
<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
 xmlns:o="urn:schemas-microsoft-com:office:office"
 xmlns:x="urn:schemas-microsoft-com:office:excel"
 xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
 xmlns:html="http://www.w3.org/TR/REC-html40">

 <ExcelWorkbook xmlns="urn:schemas-microsoft-com:office:excel">
  <ActiveSheet>0</ActiveSheet>
  <ProtectStructure>False</ProtectStructure>
  <ProtectWindows>False</ProtectWindows>
 </ExcelWorkbook>
 
 <Styles>
	<Style ss:ID="Default" ss:Name="Normal">
		<Alignment ss:Vertical="Center"/>
		<Borders/>
		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Size="12"/>
		<Interior/>
		<NumberFormat/>
		<Protection/>
  	</Style>

  <Style ss:ID="Title">
   <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
   <Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Size="12" ss:Bold="1"/>
  </Style>

  <Style ss:ID="TitleDate">
   <Alignment ss:Horizontal="Right" ss:Vertical="Center" ss:WrapText="1"/>
   <Borders>
    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   </Borders>
   <Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman"/>
  </Style>

	<Style ss:ID="Header">
		<Alignment ss:Horizontal="Center"/>
   		<Borders>
    		<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Bold="1"/>
   		<Interior ss:Color="peachpuff" ss:Pattern="Solid"/>
  	</Style>

	<Style ss:ID="HeaderS">
		<Alignment ss:Horizontal="Center"/>
   		<Borders>
    		<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Bold="1"/>
   		<Interior ss:Color="Silver" ss:Pattern="Solid"/>
  	</Style>

	<Style ss:ID="toChar">
		<Borders>
    		<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman"/>
   		<NumberFormat ss:Format="@"/>
  	</Style>

	<Style ss:ID="toDate">
   		<Borders>
		    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman"/>
		<NumberFormat ss:Format="Short Date"/>
  	</Style>

	<Style ss:ID="toNum">
   		<Borders>
		    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman"/>
  	</Style>
 </Styles>


 <Worksheet ss:Name="ET有效">
  <Table>
<!--
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">元訊寬頻網路股份有限公司</Data></Cell>
   </Row>
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">速博退租戶一覽表</Data></Cell>
   </Row>
   <Row>
    <Cell ss:MergeAcross="8" ss:StyleID="TitleDate"><Data ss:Type="String">統計截止日期：<%=datevalue(now())%> </Data></Cell>
   </Row>
-->   
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶代碼</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶名</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">週期</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">開始計費日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">續約日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">到期日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">退租日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">欠拆</Data></Cell>
   </Row>
<%
	sql="select	'ET City' as casetype,  isnull(h.shortnc, j.cusnc) as areanc, " &_
		"convert(varchar(5), a.comq1) +'-'+convert(varchar(2), a.lineq1) as comq, " &_
		"c.comn, y.cusid, y.cusnc, g.codenc as casekindnc, k.codenc as paycyclenc, " &_
		"y.strbillingdat, y.newbillingdat, y.duedat, y.dropdat, y.overdue " &_
		"from	RTLessorCmtyLine a " &_
		"inner join RTLessorCmtyH c on c.COMQ1 = a.COMQ1 " &_
		"inner join RTLessorCust y on a.comq1 = y.comq1 and a.lineq1 = y.lineq1 and y.freecode <>'Y' " &_
		"	and (y.strbillingdat <='" &v(0)& "' or y.newbillingdat <='" &v(0)& "') " &_
		"	and (y.dropdat is null or y.dropdat >'" &v(0)& "' or overdue ='Y') and y.canceldat is null " &_
		"left outer join rtcounty e on e.cutid = a.cutid " &_
		"left outer join RTobj h on a.consignee = h.cusid " &_
		"left outer join RTEmployee i inner join RTObj j on i.cusid = j.cusid on i.emply = a.salesid " &_
		"left join RTCtyTown d inner join RTArea f on f.areaid = d.areaid and f.areatype ='3' " &_
		"	on d.cutid = a.cutid and d.township = a.township " &_
		"left outer join RTCode g on g.code = y.casekind and g.kind ='O9' " &_
		"left outer join RTCode k on k.code = y.paycycle and k.kind ='M8' " &_
		"where	(a.dropdat is null or a.dropdat > '" &v(0)& "') " &_
		"and a.canceldat is null " &_
		"and y.duedat >= '" &v(0)& "' "

	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casetype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casekindnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("paycyclenc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("strbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("newbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("duedat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("overdue") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="ET到期未退">
  <Table>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶代碼</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶名</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">週期</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">開始計費日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">續約日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">到期日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">退租日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">欠拆</Data></Cell>
   </Row>
<%
	sql="select	'ET City' as casetype,  isnull(h.shortnc, j.cusnc) as areanc, " &_
		"convert(varchar(5), a.comq1) +'-'+convert(varchar(2), a.lineq1) as comq, " &_
		"c.comn, y.cusid, y.cusnc, g.codenc as casekindnc, k.codenc as paycyclenc, " &_
		"y.strbillingdat, y.newbillingdat, y.duedat, y.dropdat, y.overdue " &_
		"from	RTLessorCmtyLine a " &_
		"inner join RTLessorCmtyH c on c.COMQ1 = a.COMQ1 " &_
		"inner join RTLessorCust y on a.comq1 = y.comq1 and a.lineq1 = y.lineq1 and y.freecode <>'Y' " &_
		"	and (y.strbillingdat <='" &v(0)& "' or y.newbillingdat <='" &v(0)& "') " &_
		"	and (y.dropdat is null or y.dropdat >'" &v(0)& "' or overdue ='Y') and y.canceldat is null " &_
		"left outer join rtcounty e on e.cutid = a.cutid " &_
		"left outer join RTobj h on a.consignee = h.cusid " &_
		"left outer join RTEmployee i inner join RTObj j on i.cusid = j.cusid on i.emply = a.salesid " &_
		"left join RTCtyTown d inner join RTArea f on f.areaid = d.areaid and f.areatype ='3' " &_
		"	on d.cutid = a.cutid and d.township = a.township " &_
		"left outer join RTCode g on g.code = y.casekind and g.kind ='O9' " &_
		"left outer join RTCode k on k.code = y.paycycle and k.kind ='M8' " &_
		"where	(a.dropdat is null or a.dropdat > '" &v(0)& "') " &_
		"and a.canceldat is null	" &_
		"and y.duedat < '" &v(0)& "' "

	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casetype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casekindnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("paycyclenc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("strbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("newbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("duedat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("overdue") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="ET退租(全)">
  <Table>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶代碼</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶名</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">週期</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">開始計費日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">續約日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">到期日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">退租日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">欠拆</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">預定退租日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">退租單結案日</Data></Cell>
   </Row>
<%
	sql="select	'ET City' as casetype,  isnull(h.shortnc, j.cusnc) as areanc, " &_
		"convert(varchar(5), a.comq1) +'-'+convert(varchar(2), a.lineq1) as comq, " &_
		"c.comn, y.cusid, y.cusnc, g.codenc as casekindnc, k.codenc as paycyclenc, " &_
		"y.strbillingdat, y.newbillingdat, y.duedat, y.dropdat, y.overdue, b.enddat, b.finishdat " &_
		"from	RTLessorCmtyLine a " &_
		"inner join RTLessorCmtyH c on c.COMQ1 = a.COMQ1 " &_
		"inner join RTLessorCust y on a.comq1 = y.comq1 and a.lineq1 = y.lineq1 and y.freecode <>'Y' " &_
		"	and (y.strbillingdat <='" &v(0)& "' or y.newbillingdat <='" &v(0)& "') " &_
		"	and y.dropdat <= '" &v(0)& "' and y.canceldat is null " &_
		"left outer join RTLessorCustDrop b inner join (select cusid, max(entryno) as maxentryno from rtlessorcustdrop " &_
		"												where canceldat is null group by cusid) l " &_
		"	on l.cusid = b.cusid and b.entryno = l.maxentryno on y.cusid = b.cusid " &_
		"left outer join rtcounty e on e.cutid = a.cutid " &_
		"left outer join RTobj h on a.consignee = h.cusid " &_
		"left outer join RTEmployee i inner join RTObj j on i.cusid = j.cusid on i.emply = a.salesid " &_
		"left join RTCtyTown d inner join RTArea f on f.areaid = d.areaid and f.areatype ='3' " &_
		"	on d.cutid = a.cutid and d.township = a.township " &_
		"left outer join RTCode g on g.code = y.casekind and g.kind ='O9' " &_
		"left outer join RTCode k on k.code = y.paycycle and k.kind ='M8' " &_
		"where y.dropdat is not null "

	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casetype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casekindnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("paycyclenc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("strbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("newbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("duedat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("overdue") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("enddat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("finishdat") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="AVS有效">
  <Table>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶代碼</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶名</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">週期</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">開始計費日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">續約日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">到期日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">退租日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">欠拆</Data></Cell>
   </Row>
<%
	sql="select	'AVS City' as casetype,  isnull(h.shortnc, j.cusnc) as areanc, " &_
		"convert(varchar(5), a.comq1) +'-'+convert(varchar(2), a.lineq1) as comq, " &_
		"c.comn, y.cusid, y.cusnc, g.codenc as casekindnc, k.codenc as paycyclenc, " &_
		"y.strbillingdat, y.newbillingdat, y.duedat, y.dropdat, y.overdue " &_
		"from	RTLessorAvsCmtyLine a " &_
		"inner join RTLessorAvsCmtyH c on c.COMQ1 = a.COMQ1 " &_
		"inner join RTLessorAvsCust y on a.comq1 = y.comq1 and a.lineq1 = y.lineq1 and y.freecode <>'Y' " &_
		"	and (y.strbillingdat <='" &v(0)& "' or y.newbillingdat <='" &v(0)& "') " &_
		"	and (y.dropdat is null or y.dropdat >'" &v(0)& "' or overdue ='Y') and y.canceldat is null " &_
		"left outer join rtcounty e on e.cutid = a.cutid " &_
		"left outer join RTobj h on a.consignee = h.cusid " &_
		"left outer join RTEmployee i inner join RTObj j on i.cusid = j.cusid on i.emply = a.salesid " &_
		"left join RTCtyTown d inner join RTArea f on f.areaid = d.areaid and f.areatype ='3' " &_
		"	on d.cutid = a.cutid and d.township = a.township " &_
		"left outer join RTCode g on g.code = y.casekind and g.kind ='O9' " &_
		"left outer join RTCode k on k.code = y.paycycle and k.kind ='M8' " &_
		"where	(a.dropdat is null or a.dropdat > '" &v(0)& "') " &_
		"and a.canceldat is null " &_
		"and y.duedat >= '" &v(0)& "' "

	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casetype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casekindnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("paycyclenc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("strbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("newbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("duedat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("overdue") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="AVS到期未退">
  <Table>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶代碼</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶名</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">週期</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">開始計費日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">續約日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">到期日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">退租日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">欠拆</Data></Cell>
   </Row>
<%
	sql="select	'AVS City' as casetype,  isnull(h.shortnc, j.cusnc) as areanc, " &_
		"convert(varchar(5), a.comq1) +'-'+convert(varchar(2), a.lineq1) as comq, " &_
		"c.comn, y.cusid, y.cusnc, g.codenc as casekindnc, k.codenc as paycyclenc, " &_
		"y.strbillingdat, y.newbillingdat, y.duedat, y.dropdat, y.overdue " &_
		"from	RTLessorAvsCmtyLine a " &_
		"inner join RTLessorAvsCmtyH c on c.COMQ1 = a.COMQ1 " &_
		"inner join RTLessorAvsCust y on a.comq1 = y.comq1 and a.lineq1 = y.lineq1 and y.freecode <>'Y' " &_
		"	and (y.strbillingdat <='" &v(0)& "' or y.newbillingdat <='" &v(0)& "') " &_
		"	and (y.dropdat is null or y.dropdat >'" &v(0)& "' or overdue ='Y') and y.canceldat is null " &_
		"left outer join rtcounty e on e.cutid = a.cutid " &_
		"left outer join RTobj h on a.consignee = h.cusid " &_
		"left outer join RTEmployee i inner join RTObj j on i.cusid = j.cusid on i.emply = a.salesid " &_
		"left join RTCtyTown d inner join RTArea f on f.areaid = d.areaid and f.areatype ='3' " &_
		"	on d.cutid = a.cutid and d.township = a.township " &_
		"left outer join RTCode g on g.code = y.casekind and g.kind ='O9' " &_
		"left outer join RTCode k on k.code = y.paycycle and k.kind ='M8' " &_
		"where	(a.dropdat is null or a.dropdat > '" &v(0)& "') " &_
		"and a.canceldat is null	" &_
		"and y.duedat < '" &v(0)& "' "

	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casetype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casekindnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("paycyclenc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("strbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("newbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("duedat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("overdue") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="AVS退租(全)">
  <Table>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶代碼</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶名</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">週期</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">開始計費日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">續約日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">到期日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">退租日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">欠拆</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">預定退租日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">退租單結案日</Data></Cell>
   </Row>
<%
	sql="select	'AVS City' as casetype,  isnull(h.shortnc, j.cusnc) as areanc, " &_
		"convert(varchar(5), a.comq1) +'-'+convert(varchar(2), a.lineq1) as comq, " &_
		"c.comn, y.cusid, y.cusnc, g.codenc as casekindnc, k.codenc as paycyclenc, " &_
		"y.strbillingdat, y.newbillingdat, y.duedat, y.dropdat, y.overdue, b.enddat, b.finishdat " &_
		"from	RTLessorAvsCmtyLine a " &_
		"inner join RTLessorAvsCmtyH c on c.COMQ1 = a.COMQ1 " &_
		"inner join RTLessorAvsCust y on a.comq1 = y.comq1 and a.lineq1 = y.lineq1 and y.freecode <>'Y' " &_
		"	and (y.strbillingdat <='" &v(0)& "' or y.newbillingdat <='" &v(0)& "') " &_
		"	and y.dropdat <= '" &v(0)& "' and y.canceldat is null " &_
		"left outer join RTLessorAvsCustDrop b inner join (select cusid, max(entryno) as maxentryno from rtLessorAvscustDrop " &_
		"												where canceldat is null group by cusid) l " &_
		"	on l.cusid = b.cusid and b.entryno = l.maxentryno on y.cusid = b.cusid " &_
		"left outer join rtcounty e on e.cutid = a.cutid " &_
		"left outer join RTobj h on a.consignee = h.cusid " &_
		"left outer join RTEmployee i inner join RTObj j on i.cusid = j.cusid on i.emply = a.salesid " &_
		"left join RTCtyTown d inner join RTArea f on f.areaid = d.areaid and f.areatype ='3' " &_
		"	on d.cutid = a.cutid and d.township = a.township " &_
		"left outer join RTCode g on g.code = y.casekind and g.kind ='O9' " &_
		"left outer join RTCode k on k.code = y.paycycle and k.kind ='M8' " &_
		"where y.dropdat is not null "

	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casetype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casekindnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("paycyclenc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("strbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("newbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("duedat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("overdue") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("enddat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("finishdat") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


<%
	conn.Close
	set rs = nothing
	set conn = nothing
%>

</Workbook>
