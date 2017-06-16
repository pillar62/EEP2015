<%
    parm=request("parm")
    v=split(parm,";")

	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=裝機報竣一覽表.xls"

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


 <Worksheet ss:Name="報竣戶">
  <Table>
<!--
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">元訊寬頻網路股份有限公司</Data></Cell>
   </Row>
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">速博零用戶一覽表</Data></Cell>
   </Row>
-->
   <Row>
    <Cell ss:MergeAcross="13" ss:StyleID="TitleDate"><Data ss:Type="String">報竣日：<%=v(0)%>～<%=v(1)%> </Data></Cell>
   </Row>

   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">直/經銷</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區所屬</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
	<Cell ss:StyleID="HeaderS"><Data ss:Type="String">線路到位日</Data></Cell>
	<Cell ss:StyleID="Header"><Data ss:Type="String">裝機員</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案</Data></Cell>
	<Cell ss:StyleID="HeaderS"><Data ss:Type="String">速率</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">繳費週期</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">完工日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">報竣日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">退租日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">公關線</Data></Cell>
   </Row>
<%
    sql="select 'Sp399' as casetype, case a.consignee when '' then '直銷' else '經銷' end as belongnc, "&_
		"isnull(h.shortnc, g.cusnc) as salesnc, a.comn, a.linearrive as linearrivedat, "&_
		"isnull(isnull(e.shortnc, d.cusnc),'') as setnc, j.cusnc, b.usekind as casekind, replace(b.speed,'bps','') as speed, "&_
		"case b.paytype when 'M' then '月繳' when 'H' then '年約年繳' when 'Y' then '年約月繳' else '' end as paycycle, "&_
		"b.finishdat, b.docketdat, b.dropdat, replace(b.freecode,'N','') as freecode "&_
		"from	RTSparqAdslCmty a "&_
		"inner join RTSparqAdslCust b on a.cutyid = b.comq1 "&_
		"left outer join RTEmployee f inner join RTObj g on f.cusid = g.cusid on a.bussid = f.emply "&_
		"left outer join RTObj h on h.cusid = a.consignee "&_
		"left outer join RTEmployee c inner join RTObj d on c.cusid = d.cusid on b.setsales = c.emply "&_
		"left outer join RTObj e on e.cusid = b.profac "&_
		"left outer join RTObj j on j.cusid = b.cusid "&_
		"where b.docketdat between '"&v(0)&"' and '"&v(1)&"' "&_
		"union "&_
		"select	case a.connecttype when'05' then 'Sp499(On-Net)' else 'Sp499' end as casetype, "&_
		"case a.consignee when '' then '直銷' else '經銷' end as belongnc, "&_
		"isnull(h.shortnc, g.cusnc) as salesnc, i.comn, a.linearrivedat, "&_
		"isnull(d.cusnc,'') as setnc, b.cusnc, isnull(j.codenc,'') as casetype, '' as speed, isnull(k.codenc,'') as paycycle, "&_
		"b.finishdat, b.docketdat, b.dropdat, replace(b.freecode,'N','') as freecode " &_
		"from	RTSparq499CmtyLine a "&_
		"inner join RTSparq499Cust b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 "&_
		"inner join RTSparq499CmtyH i on i.comq1 = b.comq1 "&_
		"left outer join RTEmployee f inner join RTObj g on f.cusid = g.cusid on a.salesid = f.emply "&_
		"left outer join RTObj h on h.cusid = a.consignee "&_
		"left outer join RTEmployee c inner join RTObj d on c.cusid = d.cusid on b.setemply = c.emply "&_
		"left outer join RTCode j on j.code = b.casetype and j.kind ='L9' "&_
		"left outer join RTCode k on k.code = b.PAYTYPE and k.kind ='M1' "&_
		"where	a.canceldat is null and b.canceldat is null "&_
		"and b.docketdat between '"&v(0)&"' and '"&v(1)&"' "&_
		"union "&_
		"select 'Avs City' as casetype, case a.consignee when '' then '直銷' else '經銷' end as belongnc, "&_
		"isnull(h.shortnc, g.cusnc) as salesnc, i.comn, a.hardwaredat as linearrivedat, "&_
		"isnull(isnull(e.shortnc, d.cusnc),'') as setnc, b.cusnc, isnull(k.codenc,'') as casekind, b.userrate as speed, isnull(l.codenc,'') as paycycle, "&_
		"b.finishdat, b.docketdat, b.dropdat, replace(b.freecode,'N','') as freecode " &_
		"from	RTLessorAvsCmtyLine a "&_
		"inner join RTLessorAvsCust b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 "&_
		"inner join RTLessorAvsCmtyH i on i.comq1 = b.comq1 "&_
		"left outer join RTEmployee f inner join RTObj g on f.cusid = g.cusid on a.salesid = f.emply "&_
		"left outer join RTObj h on h.cusid = a.consignee "&_
		"left outer join RTLessorAVSCustSndwork j on j.cusid = b.cusid and j.dropdat is null "&_
		"left outer join RTEmployee c inner join RTObj d on c.cusid = d.cusid on j.realengineer = c.emply "&_
		"left outer join RTObj e on e.cusid = j.realconsignee "&_
		"left outer join RTCode k on k.code = b.casekind and k.kind ='O9' "&_
		"left outer join RTCode l on l.code = b.PAYTYPE and l.kind ='M8' "&_
		"where	a.canceldat is null and b.canceldat is null "&_
		"and b.docketdat between '"&v(0)&"' and '"&v(1)&"' "&_
		"union "&_
		"select	'ET City' as casetype, case a.consignee when '' then '直銷' else '經銷' end as belongnc, "&_
		"isnull(h.shortnc, g.cusnc) as salesnc, i.comn, a.hardwaredat as linearrivedat, "&_
		"isnull(isnull(e.shortnc, d.cusnc),'') as setnc, b.cusnc, isnull(k.codenc,'') as casekind, b.userrate as speed, isnull(l.codenc,'') as paycycle, "&_
		"b.finishdat, b.docketdat, b.dropdat, replace(b.freecode,'N','') as freecode " &_
		"from	RTLessorCmtyLine a "&_
		"inner join RTLessorCust b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 "&_
		"inner join RTLessorCmtyH i on i.comq1 = b.comq1 "&_
		"left outer join RTEmployee f inner join RTObj g on f.cusid = g.cusid on a.salesid = f.emply "&_
		"left outer join RTObj h on h.cusid = a.consignee "&_
		"left outer join RTLessorCustSndwork j on j.cusid = b.cusid and j.dropdat is null "&_
		"left outer join RTEmployee c inner join RTObj d on c.cusid = d.cusid on j.realengineer = c.emply "&_
		"left outer join RTObj e on e.cusid = j.realconsignee "&_
		"left outer join RTCode k on k.code = b.casekind and k.kind ='O9' "&_
		"left outer join RTCode l on l.code = b.PAYTYPE and l.kind ='M8' "&_
		"where	a.canceldat is null and b.canceldat is null "&_
		"and b.docketdat between '"&v(0)&"' and '"&v(1)&"' "&_
		"union "&_
		"select 'So-net' as casetype, case i.consignee when '' then '直銷' else '經銷' end as belongnc, "&_
		"isnull(h.shortnc, g.cusnc) as salesnc, i.comn, a.hardwaredat as linearrivedat, "&_
		"isnull(isnull(e.shortnc, d.cusnc),'') as setnc, b.cusnc, '' as casekind, isnull(k.codenc,'') as speed, isnull(l.codenc,'') as paycycle, "&_
		"b.finishdat, b.docketdat, b.dropdat, replace(b.freecode,'N','') as freecode " &_
		"from	RTSonetCmtyLine a "&_
		"inner join RTSonetCust b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 "&_
		"inner join RTSonetCmtyH i on i.comq1 = b.comq1 "&_
		"left outer join RTEmployee f inner join RTObj g on f.cusid = g.cusid on i.salesid = f.emply "&_
		"left outer join RTObj h on h.cusid = i.consignee "&_
		"left outer join RTSonetSndwrk j on j.cusid = b.cusid and j.canceldat is null and j.wrktyp = '02' "&_
		"left outer join RTEmployee c inner join RTObj d on c.cusid = d.cusid on j.finisheng = c.emply "&_
		"left outer join RTObj e on e.cusid = j.finishcons "&_
		"left outer join RTCode k on k.code = b.userrate and k.kind ='R3' "&_
		"left outer join RTCode l on l.code = b.paycycle and l.kind ='M8' "&_
		"where	a.canceldat is null and b.canceldat is null "&_
		"and b.docketdat between '"&v(0)&"' and '"&v(1)&"' "&_
		"union "&_
		"select '遠傳大寬頻社區型' as casetype, case i.consignee when '' then '直銷' else '經銷' end as belongnc, "&_
		"isnull(h.shortnc, g.cusnc) as salesnc, i.comn, a.hardwaredat as linearrivedat, "&_
		"isnull(isnull(e.shortnc, d.cusnc),'') as setnc, b.cusnc, '' as casekind, isnull(k.codenc,'') as speed, isnull(l.codenc,'') as paycycle, "&_
		"b.finishdat, b.docketdat, b.dropdat, replace(b.freecode,'N','') as freecode " &_
		"from	RTfareastCmtyLine a "&_
		"inner join RTfareastCust b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 "&_
		"inner join RTfareastCmtyH i on i.comq1 = b.comq1 "&_
		"left outer join RTEmployee f inner join RTObj g on f.cusid = g.cusid on i.salesid = f.emply "&_
		"left outer join RTObj h on h.cusid = i.consignee "&_
		"left outer join RTfareastSndwrk j on j.cusid = b.cusid and j.canceldat is null and j.wrktyp = '02' "&_
		"left outer join RTEmployee c inner join RTObj d on c.cusid = d.cusid on j.finisheng = c.emply "&_
		"left outer join RTObj e on e.cusid = j.finishcons "&_
		"left outer join RTCode k on k.code = b.userrate and k.kind ='R6' "&_
		"left outer join RTCode l on l.code = b.paycycle and l.kind ='M8' "&_
		"where	a.canceldat is null and b.canceldat is null "&_
		"and b.docketdat between '"&v(0)&"' and '"&v(1)&"' "

	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casetype") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("belongnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("salesnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("linearrivedat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("setnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casekind") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("speed") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("paycycle") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("finishdat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("docketdat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("freecode") &"</Data></Cell>" &_
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
