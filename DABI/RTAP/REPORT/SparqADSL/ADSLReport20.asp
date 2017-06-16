<%
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=速博報竣戶一覽表.xls"

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


 <Worksheet ss:Name="Sparq399報竣戶">
  <Table>
<!--
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">元訊寬頻網路股份有限公司</Data></Cell>
   </Row>
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">速博報竣戶一覽表</Data></Cell>
   </Row>
   <Row>
    <Cell ss:MergeAcross="4" ss:StyleID="TitleDate"><Data ss:Type="String">製表日期：<%=now()%> </Data></Cell>
   </Row>
-->
   <Row>
    <Cell ss:StyleID="Header"><Data ss:Type="String">NO.</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">轄區</Data></Cell>
	<Cell ss:StyleID="Header"><Data ss:Type="String">業務</Data></Cell>	
    <Cell ss:StyleID="Header"><Data ss:Type="String">社區序號</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">客戶名稱</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">IP</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">對帳代號</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">報竣日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">退租日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">TEL(H)</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">TEL(O)</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">手機</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">裝機地址</Data></Cell>
   </Row>
<%
	sql="select h.areanc as areanc, isnull(i.shortnc, k.cusnc) as belongnc, "_
		&"Convert(varchar(5), c.cutyid) as comq1, c.comn, d.cusnc, '' as IP, b.exttel+'-'+b.sphnno as ncicno, "_
		&"b.docketdat, b.dropdat, b.home, b.office + case when b.office<>'' and b.extension<>'' then ' # ' else '' end+ b.extension as office, b.mobile, "_
		&"isnull(e.cutnc, '')+b.township2+b.raddr2 as addr "_
		&"from 	RTSparqAdslCust b "_
		&"inner join RTSparqAdslCmty c on b.comq1 = c.cutyid "_
		&"inner join RTObj d on d.cusid = b.cusid "_
		&"left outer join RTCounty e on e.cutid = b.cutid2 "_
		&"left outer join RTObj i on i.cusid = c.consignee "_
		&"left outer join RTCtyTown g inner join rtarea h on h.areaid = g.areaid and h.areatype ='3' on g.cutid = c.cutid and g.township= c.township "_
		&"left outer join RTEmployee j inner join RTObj k on k.cusid = j.cusid on j.emply = c.bussid "_
		&"where	 b.freecode <>'Y' "_
		&"and	b.docketdat between '" &v(0)& "' and '" &v(1)& "' "_
		&"order by i.cusnc, 2, 1, 4 "
	rs.Open sql, CONN
	serno=0				   
	Do While Not rs.Eof
	    serno = serno+1
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& serno &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("belongnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq1") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("ip") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("ncicno") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("docketdat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("home") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("office") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("mobile") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="Sparq499報竣戶">
  <Table>
   <Row>
    <Cell ss:StyleID="Header"><Data ss:Type="String">NO.</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">轄區</Data></Cell>
	<Cell ss:StyleID="Header"><Data ss:Type="String">業務</Data></Cell>	
    <Cell ss:StyleID="Header"><Data ss:Type="String">社區序號</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">客戶名稱</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">IP</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">對帳代號</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">報竣日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">退租日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">TEL(H)</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">TEL(O)</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">手機</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">裝機地址</Data></Cell>
   </Row>
<%
	sql="select h.areanc as areanc, isnull(i.shortnc, k.cusnc) as belongnc, "_
		&"convert(varchar(5), c.comq1) +'-'+ convert(varchar(2), c.lineq1) as comq1, f.comn, b.cusnc, b.custip1+'.'+b.custip2+'.'+b.custip3+'.'+b.custip4 as IP, "_
		&"b.nciccusno+'-'+b.sphnno as ncicno, b.docketdat, b.dropdat, b.contacttel as home, '' as office, b.mobile, "_
		&"isnull(e.cutnc, '')+b.township2+b.raddr2 as addr "_
		&"from 	RTSparq499Cust b "_
		&"inner join RTSparq499CmtyLine c on b.comq1 = c.comq1 and b.lineq1 = c.lineq1 "_
		&"inner join RTSparq499CmtyH f on c.comq1 = f.comq1 "_
		&"left outer join RTCounty e on e.cutid = b.cutid2 "_
		&"left outer join RTObj i on i.cusid = c.consignee "_
		&"left outer join RTCtyTown g inner join rtarea h on h.areaid = g.areaid and h.areatype ='3' on g.cutid = c.cutid and g.township= c.township "_
		&"left outer join RTEmployee j inner join RTObj k on k.cusid = j.cusid on j.emply = c.salesid "_
		&"where	b.canceldat is null and b.freecode <>'Y' "_
		&"and	b.docketdat between '" &v(0)& "' and '" &v(1)& "' "_
		&"order by i.cusnc, 2, 1, 4 "
	rs.Open sql, CONN
	serno=0				   
	Do While Not rs.Eof
	    serno = serno+1
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& serno &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("belongnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq1") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("ip") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("ncicno") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("docketdat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("home") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("office") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("mobile") &"</Data></Cell>" &_
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
