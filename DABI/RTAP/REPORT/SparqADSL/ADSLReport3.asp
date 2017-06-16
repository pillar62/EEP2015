<%
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=速博主線資料(含撤線).xls"

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


 <Worksheet ss:Name="Sparq399">
  <Table>
<!--
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">元訊寬頻網路股份有限公司</Data></Cell>
   </Row>
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">速博零用戶一覽表</Data></Cell>
   </Row>
   <Row>
    <Cell ss:MergeAcross="4" ss:StyleID="TitleDate"><Data ss:Type="String">製表日期：<%=now()%> </Data></Cell>
   </Row>
-->

   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">直/經銷</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區所屬</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">線路速率</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">附掛電話</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">用戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">申請日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">撤線日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
   </Row>
<%
    sql="select case a.consignee when '' then '直銷' else '經銷' end as belongid, " &_
		"isnull(isnull(h.shortnc, j.cusnc), '') as belongnc, a.comn, " &_
		"isnull(b.codenc,'') as lineratenc, cmtytel,  isnull(f.NUM,0) as custnum, " &_
		"a.rcvd, a.rcomdrop, isnull(e.cutnc, '')+a.township+a.addr  as addr " &_
		"from	RTSparqAdslCmty a " &_
		"left outer join RTCode b on a.linerate = b.code and b.kind ='D3' " &_
		"left outer join rtcounty e on e.cutid = a.cutid " &_
		"left outer join (select comq1, count(*) as num from RTSparqAdslCmty x  " &_
		"				inner join RTSparqAdslCust y on x.cutyid = y.comq1 and y.freecode <>'Y' " &_
		"				and y.docketdat is not null and y.dropdat is null " &_
		"				group by comq1) f on a.cutyid = f.comq1 " &_
		"left outer join RTobj h on a.consignee = h.cusid " &_
		"left outer join RTEmployee i inner join RTObj j on i.cusid = j.cusid on i.emply = a.bussid " &_
		"where a.rcvd <='" &v(0)& "' and (a.rcomdrop is null or a.rcomdrop >='" &v(0)& "') " &_
		"order by 1,2,3 "
	rs.Open sql, CONN
	'i=0
	Do While Not rs.Eof
		'i=i+1
	    response.Write "<Row>" &_
   			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("belongid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("belongnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("lineratenc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cmtytel") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("rcvd") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("rcomdrop") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="Sparq499">
  <Table>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">直/經銷</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區所屬</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">線路速率</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">附掛電話</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">用戶數</Data></Cell>
	<Cell ss:StyleID="HeaderS"><Data ss:Type="String">申請日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">撤線日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">作廢日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
   </Row>
<%
    sql="select case a.consignee when '' then '直銷' else '經銷' end as belongid, " &_
		"isnull(isnull(h.shortnc, j.cusnc), '') as belongnc, g.comn, " &_
		"b.codenc as lineratenc, linetel, isnull(f.num, 0) as custnum, " &_
		"a.adslapplydat, a.dropdat, a.canceldat, isnull(e.cutnc, '')+a.township+a.raddr as addr  " &_
		"from 	rtsparq499cmtyline a " &_
		"left outer join RTCode b on a.linerate = b.code and b.kind ='D3' " &_
		"left outer join rtcounty e on e.cutid = a.cutid " &_
		"inner join rtsparq499cmtyh g on a.comq1 = g.comq1 " &_
		"left outer join (select x.comq1, x.lineq1, count(*) as num	from rtsparq499cmtyline x " &_
		"				inner join rtsparq499cust y on x.comq1 = y.comq1 and x.lineq1 = y.lineq1 " &_
		"				and y.freecode <>'Y' and y.docketdat is not null and y.dropdat is null " &_
		"				and y.canceldat is null  group by x.comq1, x.lineq1) f on a.comq1 = f.comq1 and a.lineq1 = f.lineq1 " &_
		"left outer join RTobj h on a.consignee = h.cusid " &_
		"left outer join RTEmployee i inner join RTObj j on i.cusid = j.cusid on i.emply = a.salesid " &_
		"where a.adslapplydat <='" &v(0)& "' and (a.dropdat is null or a.dropdat >='" &v(0)& "') " &_
		"and (a.canceldat is null or a.canceldat >='" &v(0)& "') " &_
		"order by 1,2,3 "
	rs.Open sql, CONN
	'i=0
	Do While Not rs.Eof
		'i=i+1
	    response.Write "<Row>" &_
   			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("belongid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("belongnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("lineratenc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linetel") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("adslapplydat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("canceldat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close

	conn.Close
	set rs = nothing
	set conn = nothing
%>
  </Table>
 </Worksheet>


</Workbook>
