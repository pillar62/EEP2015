<%
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=社區主線戶數("& year(now())-1911 & right("0"& month(now()),2) & right("0"& day(now()),2) & ").xls"

    'parm=request("parm")
    'v=split(parm,";")

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


 <Worksheet ss:Name="AVS-City">
  <Table>
<!--
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">元訊寬頻網路股份有限公司</Data></Cell>
   </Row>
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">速博退租戶一覽表</Data></Cell>
   </Row>
-->
   <Row>
    <Cell ss:MergeAcross="8" ss:StyleID="TitleDate"><Data ss:Type="String">統計截止日期：<%=datevalue(now())%> </Data></Cell>
   </Row>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線速率</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">客戶數</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">公關戶數</Data></Cell>
   </Row>
<%
	sql="select isnull(g.shortnc, i.cusnc) as areanc, " &_
		"convert(varchar(5), e.comq1)+'-'+convert(varchar(5), e.lineq1) as comq, " &_
		"a.comn, d.cutnc, e.township, " &_
		"e.village + case e.village when '' then '' else e.cod1 end+ " &_
		"e.neighbor + case e.neighbor when '' then '' else e.cod2 end+ " &_
		"e.street + case e.street when '' then '' else e.cod3 end+ " &_
		"e.sec + case e.sec when '' then '' else e.cod4 end+ " &_
		"e.lane + case e.lane when '' then '' else e.cod5 end+ " &_
		"e.town + case e.town when '' then '' else e.cod6 end+ " &_
		"e.alleyway + case e.alleyway when '' then '' else e.cod7 end+ " &_
		"e.num + case e.num when '' then '' else e.cod8 end+ " &_
		"e.floor + case e.floor when '' then '' else e.cod9 end+ " &_
		"e.room + case e.room when '' then '' else e.cod10 end as addr, " &_
		"b.codenc as linespeed, isnull(c.custnum,0) as custnum, isnull(f.freenum,0) as freenum " &_
		"from rtlessoravscmtyH a " &_
		"inner join rtlessoravscmtyline e on e.comq1 = a.comq1 " &_
		"left outer join RTObj g on g.cusid = e.consignee " &_
		"left outer join RTEmployee h inner join RTObj i on i.cusid = h.cusid on h.emply = e.salesid " &_
		"left outer join rtcode b on b.code = e.linerate and b.kind ='D3' " &_
		"left outer join rtcounty d on d.cutid = e.cutid " &_
		"left outer join (select comq1, lineq1, count(*) as custnum from rtlessoravscust " &_
		"			where dropdat is null and canceldat is null	" &_
		"			and	(strbillingdat is not null or newbillingdat is not null) " &_
		"			and freecode <>'Y' and overdue <>'Y' " &_
		"			group by comq1, lineq1) c on e.comq1 = c.comq1 and e.lineq1 = c.lineq1 " &_
		"left outer join (select comq1, lineq1, count(*) as freenum from rtlessoravscust " &_
		"			where freecode ='Y'	" &_
		"			group by comq1, lineq1) f on e.comq1 = f.comq1 and e.lineq1 = f.lineq1 " &_
		"where e.dropdat is null and e.canceldat is null " &_
		"group by isnull(g.shortnc, i.cusnc), " &_
		"convert(varchar(5), e.comq1)+'-'+convert(varchar(5), e.lineq1), " &_
		"a.comn, d.cutnc, e.township, " &_
		"e.village + case e.village when '' then '' else e.cod1 end+ " &_
		"e.neighbor + case e.neighbor when '' then '' else e.cod2 end+ " &_
		"e.street + case e.street when '' then '' else e.cod3 end+ " &_
		"e.sec + case e.sec when '' then '' else e.cod4 end+ " &_
		"e.lane + case e.lane when '' then '' else e.cod5 end+ " &_
		"e.town + case e.town when '' then '' else e.cod6 end+ " &_
		"e.alleyway + case e.alleyway when '' then '' else e.cod7 end+ " &_
		"e.num + case e.num when '' then '' else e.cod8 end+ " &_
		"e.floor + case e.floor when '' then '' else e.cod9 end+ " &_
		"e.room + case e.room when '' then '' else e.cod10 end, " &_
		"b.codenc, isnull(c.custnum,0), isnull(f.freenum,0) " &_
		"order by 1,3 "
	
	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linespeed") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("freenum") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="ET-City">
  <Table>
   <Row>
    <Cell ss:MergeAcross="8" ss:StyleID="TitleDate"><Data ss:Type="String">統計截止日期：<%=datevalue(now())%> </Data></Cell>
   </Row>

   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線速率</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">客戶數</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">公關戶數</Data></Cell>
   </Row>
<%
	sql="select isnull(g.shortnc, i.cusnc) as areanc, " &_
		"convert(varchar(5), e.comq1)+'-'+convert(varchar(5), e.lineq1) as comq, " &_
		"a.comn, d.cutnc, e.township, " &_
		"e.village + case e.village when '' then '' else e.cod1 end+ " &_
		"e.neighbor + case e.neighbor when '' then '' else e.cod2 end+ " &_
		"e.street + case e.street when '' then '' else e.cod3 end+ " &_
		"e.sec + case e.sec when '' then '' else e.cod4 end+ " &_
		"e.lane + case e.lane when '' then '' else e.cod5 end+ " &_
		"e.town + case e.town when '' then '' else e.cod6 end+ " &_
		"e.alleyway + case e.alleyway when '' then '' else e.cod7 end+ " &_
		"e.num + case e.num when '' then '' else e.cod8 end+ " &_
		"e.floor + case e.floor when '' then '' else e.cod9 end+ " &_
		"e.room + case e.room when '' then '' else e.cod10 end as addr, " &_
		"b.codenc as linespeed, isnull(c.custnum,0) as custnum, isnull(f.freenum,0) as freenum " &_
		"from rtlessorcmtyH a " &_
		"inner join rtlessorcmtyline e on e.comq1 = a.comq1 " &_
		"left outer join RTObj g on g.cusid = e.consignee " &_
		"left outer join RTEmployee h inner join RTObj i on i.cusid = h.cusid on h.emply = e.salesid " &_
		"left outer join rtcode b on b.code = e.linerate and b.kind ='D3' " &_
		"left outer join rtcounty d on d.cutid = e.cutid " &_
		"left outer join (select comq1, lineq1, count(*) as custnum from rtlessorcust " &_
		"			where dropdat is null and canceldat is null	" &_
		"			and	(strbillingdat is not null or newbillingdat is not null) " &_
		"			and freecode <>'Y' and overdue <>'Y' " &_
		"			group by comq1, lineq1) c on e.comq1 = c.comq1 and e.lineq1 = c.lineq1 " &_
		"left outer join (select comq1, lineq1, count(*) as freenum from rtlessorcust " &_
		"			where freecode ='Y'	" &_
		"			group by comq1, lineq1) f on e.comq1 = f.comq1 and e.lineq1 = f.lineq1 " &_
		"where e.dropdat is null and e.canceldat is null " &_
		"group by isnull(g.shortnc, i.cusnc), " &_
		"convert(varchar(5), e.comq1)+'-'+convert(varchar(5), e.lineq1), " &_
		"a.comn, d.cutnc, e.township, " &_
		"e.village + case e.village when '' then '' else e.cod1 end+ " &_
		"e.neighbor + case e.neighbor when '' then '' else e.cod2 end+ " &_
		"e.street + case e.street when '' then '' else e.cod3 end+ " &_
		"e.sec + case e.sec when '' then '' else e.cod4 end+ " &_
		"e.lane + case e.lane when '' then '' else e.cod5 end+ " &_
		"e.town + case e.town when '' then '' else e.cod6 end+ " &_
		"e.alleyway + case e.alleyway when '' then '' else e.cod7 end+ " &_
		"e.num + case e.num when '' then '' else e.cod8 end+ " &_
		"e.floor + case e.floor when '' then '' else e.cod9 end+ " &_
		"e.room + case e.room when '' then '' else e.cod10 end, " &_
		"b.codenc, isnull(c.custnum,0), isnull(f.freenum,0) " &_
		"order by 1,3 "

	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linespeed") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("freenum") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="專案社區">
  <Table ID="Table1">
   <Row>
    <Cell ss:MergeAcross="8" ss:StyleID="TitleDate"><Data ss:Type="String">統計截止日期：<%=datevalue(now())%> </Data></Cell>
   </Row>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線速率</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">客戶數</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">公關戶數</Data></Cell>
   </Row>
<%
	sql="select isnull(g.shortnc, i.cusnc) as areanc, " &_
		"convert(varchar(5), e.comq1)+'-'+convert(varchar(5), e.lineq1) as comq, " &_
		"a.comn, d.cutnc, a.township, a.raddr as addr, " &_
		"b.codenc as linespeed, isnull(c.custnum,0) as custnum, isnull(f.freenum,0) as freenum " &_
		"from RTPrjCmtyH a " &_
		"inner join RTPrjCmtyLine e on e.comq1 = a.comq1 " &_
		"left outer join RTObj g on g.cusid = a.consignee " &_
		"left outer join RTEmployee h inner join RTObj i on i.cusid = h.cusid on h.emply = a.salesid " &_
		"left outer join rtcode b on b.code = e.linerate and b.kind ='D3' " &_
		"left outer join rtcounty d on d.cutid = a.cutid " &_
		"left outer join (select comq1, lineq1, count(*) as custnum from RTPrjCust " &_
		"			where dropdat is null and canceldat is null " &_
		"			and	strbillingdat is not null " &_
		"			and freecode <>'Y' " &_
		"			group by comq1, lineq1) c on e.comq1 = c.comq1 and e.lineq1 = c.lineq1 " &_
		"left outer join (select comq1, lineq1, count(*) as freenum from RTPrjCust " &_
		"			where freecode ='Y' " &_
		"			group by comq1, lineq1) f on e.comq1 = f.comq1 and e.lineq1 = f.lineq1 " &_
		"where e.dropdat is null and e.canceldat is null " &_
		"group by isnull(g.shortnc, i.cusnc), " &_
		"convert(varchar(5), e.comq1)+'-'+convert(varchar(5), e.lineq1), " &_
		"a.comn, d.cutnc, a.township, a.raddr, " &_
		"b.codenc, isnull(c.custnum,0), isnull(f.freenum,0)	" &_
		"order by 1,3 "
	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linespeed") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("freenum") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="So-net社區">
  <Table ID="Table1">
   <Row>
    <Cell ss:MergeAcross="9" ss:StyleID="TitleDate"><Data ss:Type="String">統計截止日期：<%=datevalue(now())%> </Data></Cell>
   </Row>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線速率</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">報竣戶</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">計費戶</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">公關戶數</Data></Cell>
   </Row>
<%
	sql="select isnull(g.shortnc, i.cusnc) as areanc, " &_
		"convert(varchar(5), e.comq1)+'-'+convert(varchar(5), e.lineq1) as comq, " &_
		"a.comn, d.cutnc, a.township, a.raddr as addr, " &_
		"b.codenc as linespeed, isnull(c.custnum,0) as custnum, isnull(j.billnum,0) as billnum, isnull(f.freenum,0) as freenum " &_
		"from RTSonetCmtyH a " &_
		"inner join RTSonetCmtyLine e on e.comq1 = a.comq1 " &_
		"left outer join RTObj g on g.cusid = a.consignee " &_
		"left outer join RTEmployee h inner join RTObj i on i.cusid = h.cusid on h.emply = a.salesid " &_
		"left outer join rtcode b on b.code = e.linerate and b.kind ='D3' " &_
		"left outer join rtcounty d on d.cutid = a.cutid " &_
		"left outer join (select comq1, lineq1, count(*) as custnum from RTSonetCust " &_
		"			where dropdat is null and canceldat is null " &_
		"			and	docketdat is not null " &_
		"			and freecode <>'Y' " &_
		"			group by comq1, lineq1) c on e.comq1 = c.comq1 and e.lineq1 = c.lineq1 " &_
		"left outer join (select comq1, lineq1, count(*) as freenum from RTSonetCust " &_
		"			where freecode ='Y' " &_
		"			group by comq1, lineq1) f on e.comq1 = f.comq1 and e.lineq1 = f.lineq1 " &_
		"left outer join (select comq1, lineq1, count(*) as billnum from RTSonetCust " &_
		"			where dropdat is null and canceldat is null " &_
		"			and	strbillingdat is not null " &_
		"			and freecode <>'Y' " &_
		"			group by comq1, lineq1) j on e.comq1 = j.comq1 and e.lineq1 = j.lineq1 " &_
		"where e.dropdat is null and e.canceldat is null " &_
		"group by isnull(g.shortnc, i.cusnc), " &_
		"convert(varchar(5), e.comq1)+'-'+convert(varchar(5), e.lineq1), " &_
		"a.comn, d.cutnc, a.township, a.raddr, " &_
		"b.codenc, isnull(c.custnum,0),  isnull(j.billnum,0), isnull(f.freenum,0)	" &_
		"order by 1,3 "
	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linespeed") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("billnum") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("freenum") &"</Data></Cell>" &_
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
