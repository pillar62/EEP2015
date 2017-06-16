<%
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=社區列表.xls"

    parm=request("parm")
    v=split(parm,";")

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

  <Style ss:ID="SubTitle">
   <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
   		<Borders>
    		<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   <Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Size="10"/>
   <Interior ss:Color="#FFFF99" ss:Pattern="Solid"/>
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
   		<NumberFormat ss:Format="#,##0_ ;[Red]\-#,##0\ "/>
  	</Style>

	<Style ss:ID="toSum">
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Color="Blue" ss:Bold="1"/>
   		<NumberFormat ss:Format="#,##0_ ;[Red]\-#,##0\ "/>
  	</Style>
 </Styles>


 <Worksheet ss:Name="Sparq499社區">
  <Table>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">工程師</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">規模戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">用戶數</Data></Cell>
	<Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
   </Row>
<%
    sql="select f.areanc, isnull(isnull(h.shortnc, j.cusnc), '') as salesnc, a.comq1, a.comn, a.comcnt, " &_
		"d.linenum, isnull(c.custnum,0) as custnum, k.cutnc, a.township, a.raddr " &_
		"from	RTSparq499CmtyH a " &_
		"inner join RTSparq499CmtyLine b on a.comq1 = b.comq1 " &_
		"left outer join (select x.comq1, count(*) as custnum from rtsparq499cmtyline x " &_
		"	inner join rtsparq499cust y on x.comq1 = y.comq1 and x.lineq1 = y.lineq1 " &_
		"	and y.freecode <>'Y' and y.docketdat is not null and y.dropdat is null and y.canceldat is null " &_
		"	group by x.comq1) c on c.comq1 = a.comq1 " &_
		"left outer join (select comq1 , count(*) as linenum from rtsparq499cmtyline where dropdat  is null and canceldat is null group by comq1) d on d.comq1 = a.comq1 " &_
		"left outer join RTCtyTown e inner join RTArea f on f.areaid = e.areaid and f.areatype='3' on e.cutid = a.cutid and e.township = a.township " &_
		"left outer join RTCode g on b.linerate = g.code and g.kind ='D3' " &_
		"left outer join RTobj h on b.consignee = h.cusid " &_
		"left outer join RTEmployee i inner join RTObj j on i.cusid = j.cusid on i.emply = b.salesid " &_
		"left outer join RTCounty k on k.cutid = a.cutid " &_
		"where	b.dropdat  is null and b.canceldat is null " &_
		"group by f.areanc, isnull(isnull(h.shortnc, j.cusnc), ''), a.comq1, a.comn, a.comcnt, " &_
		"	d.linenum, isnull(c.custnum,0), k.cutnc, a.township, a.raddr, e.areaid " &_
		"order by e.areaid, 2, a.comn "
	rs.Open sql, CONN
	'i=0
	Do While Not rs.Eof
		'i=i+1
	    response.Write "<Row>" &_
   			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq1") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("salesnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("comcnt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("linenum") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("raddr") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="Sparq399社區">
  <Table>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">工程師</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">規模戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">用戶數</Data></Cell>
	<Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
   </Row>
<%
    sql="select f.areanc, isnull(isnull(h.shortnc, j.cusnc), '') as salesnc, a.cutyid, a.comn, a.comcnt, " &_
 		"1 as linenum, isnull(c.custnum,0) as custnum, k.cutnc, a.township, a.addr " &_
		"from	RTSparqAdslCmty a " &_
		"left outer join (select y.comq1, count(*) as custnum from RTSparqAdslCmty x " &_
		"	inner join RTSparqAdslCust y on x.cutyid = y.comq1 and y.docketdat is not null and y.dropdat is null " &_
		"	and y.freecode <>'Y' group by y.comq1) c on c.comq1 = a.cutyid " &_
		"left outer join RTCtyTown e inner join RTArea f on f.areaid = e.areaid and f.areatype='3' on e.cutid = a.cutid and e.township = a.township " &_
		"left outer join RTCode g on a.linerate = g.code and g.kind ='D3' " &_
		"left outer join RTobj h on a.consignee = h.cusid " &_
		"left outer join RTEmployee i inner join RTObj j on i.cusid = j.cusid on i.emply = a.bussid " &_
		"left outer join RTCounty k on k.cutid = a.cutid " &_
		"where a.rcomdrop is null and linearrive is not null " &_
		"group by f.areanc, isnull(isnull(h.shortnc, j.cusnc), ''), a.cutyid, a.comn, a.comcnt, " &_
		"isnull(c.custnum,0), k.cutnc, a.township, a.addr, e.areaid, a.consignee " &_
		"order by a.consignee, e.areaid, 2, a.comn "
	rs.Open sql, CONN
	'i=0
	Do While Not rs.Eof
		'i=i+1
	    response.Write "<Row>" &_
   			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutyid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("salesnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("comcnt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("linenum") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="Avs-City社區">
  <Table>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">工程師</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">規模戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">用戶數</Data></Cell>
	<Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
   </Row>
<%
    sql="select f.areanc, isnull(isnull(h.shortnc, j.cusnc), '') as salesnc, a.comq1, a.comn, a.comcnt, " &_
		"d.linenum, isnull(c.custnum,0) as custnum, k.cutnc, a.township, a.raddr " &_
		"from	RTLessorAvsCmtyH a " &_
		"inner join RTLessorAvsCmtyLine b on a.comq1 = b.comq1 " &_
		"left outer join (select x.comq1, count(*) as custnum from rtLessorAvscmtyline x " &_
		"	inner join rtLessorAvscust y on x.comq1 = y.comq1 and x.lineq1 = y.lineq1 " &_
		"	and y.freecode <>'Y' and y.dropdat is null and y.canceldat is null and (y.strbillingdat is not null or y.newbillingdat is not null) " &_
		"	group by x.comq1) c on c.comq1 = a.comq1 " &_
		"left outer join (select comq1 , count(*) as linenum from rtLessorAvscmtyline where dropdat  is null and canceldat is null and hardwaredat is not null group by comq1) d on d.comq1 = a.comq1 " &_
		"left outer join RTCtyTown e inner join RTArea f on f.areaid = e.areaid and f.areatype='3' on e.cutid = a.cutid and e.township = a.township " &_
		"left outer join RTCode g on b.linerate = g.code and g.kind ='D3' " &_
		"left outer join RTobj h on b.consignee = h.cusid " &_
		"left outer join RTEmployee i inner join RTObj j on i.cusid = j.cusid on i.emply = b.salesid " &_
		"left outer join RTCounty k on k.cutid = a.cutid " &_
		"where	b.dropdat  is null and b.canceldat is null " &_
		"group by f.areanc, isnull(isnull(h.shortnc, j.cusnc), ''), a.comq1, a.comn, a.comcnt, " &_
		"	d.linenum, isnull(c.custnum,0), k.cutnc, a.township, a.raddr, e.areaid " &_
		"order by e.areaid, 2, custnum desc "
	rs.Open sql, CONN
	'i=0
	Do While Not rs.Eof
		'i=i+1
	    response.Write "<Row>" &_
   			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq1") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("salesnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("comcnt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("linenum") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("raddr") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="ET-City社區">
  <Table>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">工程師</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">規模戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">用戶數</Data></Cell>
	<Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
   </Row>
<%
    sql="select f.areanc, isnull(isnull(h.shortnc, j.cusnc), '') as salesnc, a.comq1, a.comn, a.comcnt, " &_
		"d.linenum, isnull(c.custnum,0) as custnum, k.cutnc, a.township, a.raddr " &_
		"from	RTLessorCmtyH a " &_
		"inner join RTLessorCmtyLine b on a.comq1 = b.comq1 " &_
		"left outer join (select x.comq1, count(*) as custnum from rtLessorcmtyline x " &_
		"	inner join rtLessorcust y on x.comq1 = y.comq1 and x.lineq1 = y.lineq1 " &_
		"	and y.freecode <>'Y' and y.dropdat is null and y.canceldat is null and (y.strbillingdat is not null or y.newbillingdat is not null) " &_
		"	group by x.comq1) c on c.comq1 = a.comq1 " &_
		"left outer join (select comq1 , count(*) as linenum from rtLessorcmtyline where dropdat  is null and canceldat is null and hardwaredat is not null group by comq1) d on d.comq1 = a.comq1 " &_
		"left outer join RTCtyTown e inner join RTArea f on f.areaid = e.areaid and f.areatype='3' on e.cutid = a.cutid and e.township = a.township " &_
		"left outer join RTCode g on b.linerate = g.code and g.kind ='D3' " &_
		"left outer join RTobj h on b.consignee = h.cusid " &_
		"left outer join RTEmployee i inner join RTObj j on i.cusid = j.cusid on i.emply = b.salesid " &_
		"left outer join RTCounty k on k.cutid = a.cutid " &_
		"where	b.dropdat  is null and b.canceldat is null " &_
		"group by f.areanc, isnull(isnull(h.shortnc, j.cusnc), ''), a.comq1, a.comn, a.comcnt, " &_
		"	d.linenum, isnull(c.custnum,0), k.cutnc, a.township, a.raddr, e.areaid " &_
		"order by e.areaid, 2, custnum desc "
	rs.Open sql, CONN
	'i=0
	Do While Not rs.Eof
		'i=i+1
	    response.Write "<Row>" &_
   			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq1") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("salesnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("comcnt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("linenum") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("raddr") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="So-net社區">
  <Table>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">工程師</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">規模戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">用戶數</Data></Cell>
	<Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
   </Row>
<%
    sql="select f.areanc, isnull(isnull(h.shortnc, j.cusnc), '') as salesnc, a.comq1, a.comn, a.comcnt, " &_
		"d.linenum, isnull(c.custnum,0) as custnum, k.cutnc, a.township, a.raddr " &_
		"from	RTSonetCmtyH a " &_
		"inner join RTSonetCmtyLine b on a.comq1 = b.comq1 " &_
		"left outer join (select x.comq1, count(*) as custnum from RTSonetCmtyLine x " &_
		"	inner join rtSonetcust y on x.comq1 = y.comq1 and x.lineq1 = y.lineq1 " &_
		"	and y.freecode <>'Y' and y.dropdat is null and y.canceldat is null and y.strbillingdat is not null " &_
		"	group by x.comq1) c on c.comq1 = a.comq1 " &_
		"inner join (select comq1 , count(*) as linenum from rtSonetcmtyline where dropdat  is null and canceldat is null and hardwaredat is not null group by comq1) d on d.comq1 = a.comq1 " &_
		"left outer join RTCtyTown e inner join RTArea f on f.areaid = e.areaid and f.areatype='3' on e.cutid = a.cutid and e.township = a.township " &_
		"left outer join RTCode g on b.linerate = g.code and g.kind ='D3' " &_
		"left outer join RTobj h on a.consignee = h.cusid " &_
		"left outer join RTEmployee i inner join RTObj j on i.cusid = j.cusid on i.emply = a.engid " &_
		"left outer join RTCounty k on k.cutid = a.cutid " &_
		"where	b.dropdat  is null and b.canceldat is null " &_
		"group by f.areanc, isnull(isnull(h.shortnc, j.cusnc), ''), a.comq1, a.comn, a.comcnt, " &_
		"	d.linenum, isnull(c.custnum,0), k.cutnc, a.township, a.raddr, e.areaid " &_
		"order by e.areaid, 2, custnum desc "
	rs.Open sql, CONN
	'i=0
	Do While Not rs.Eof
		'i=i+1
	    response.Write "<Row>" &_
   			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq1") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("salesnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("comcnt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("linenum") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("raddr") &"</Data></Cell>" &_
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
