<%
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=速博及自營方案低用戶社區.xls"

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


 <Worksheet ss:Name="零戶">
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
    <Cell ss:MergeAcross="13" ss:StyleID="TitleDate"><Data ss:Type="String">統計截止日期：<%=datevalue(now())%> </Data></Cell>
   </Row>

   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">連接方式</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">規模戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線速率</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">附掛電話</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">主線到位日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">備註</Data></Cell>
   </Row>
<%
	sql="usp_RTCmtyLowCust 0"
	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casetype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("connectnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("comcnt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linespeed") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linetel") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("linearrivedat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("remark") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="一戶">
  <Table>
   <Row>
    <Cell ss:MergeAcross="14" ss:StyleID="TitleDate"><Data ss:Type="String">統計截止日期：<%=datevalue(now())%> </Data></Cell>
   </Row>

   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">連接方式</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">規模戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線速率</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">附掛電話</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">主線到位日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">備註</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">客戶名稱</Data></Cell>
   </Row>
<%
	sql="usp_RTCmtyLowCust 1"
	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casetype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("connectnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("comcnt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linespeed") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linetel") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("linearrivedat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("remark") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="二戶">
  <Table ID="Table2">
   <Row>
    <Cell ss:MergeAcross="13" ss:StyleID="TitleDate"><Data ss:Type="String">統計截止日期：<%=datevalue(now())%> </Data></Cell>
   </Row>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">連接方式</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">規模戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線速率</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">附掛電話</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">主線到位日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">備註</Data></Cell>
   </Row>
<%
	sql="usp_RTCmtyLowCust 2"
	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casetype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("connectnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("comcnt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linespeed") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linetel") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("linearrivedat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("remark") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


<Worksheet ss:Name="三戶">
  <Table ID="Table3">
   <Row>
    <Cell ss:MergeAcross="13" ss:StyleID="TitleDate"><Data ss:Type="String">統計截止日期：<%=datevalue(now())%> </Data></Cell>
   </Row>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">連接方式</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">規模戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線速率</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">附掛電話</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">主線到位日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">備註</Data></Cell>
   </Row>
<%
	sql="usp_RTCmtyLowCust 3"
	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casetype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("connectnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("comcnt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linespeed") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linetel") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("linearrivedat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("remark") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


<Worksheet ss:Name="四戶">
  <Table ID="Table4">
   <Row>
    <Cell ss:MergeAcross="13" ss:StyleID="TitleDate"><Data ss:Type="String">統計截止日期：<%=datevalue(now())%> </Data></Cell>
   </Row>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">連接方式</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">規模戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線速率</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">附掛電話</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">主線到位日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">備註</Data></Cell>
   </Row>
<%
	sql="usp_RTCmtyLowCust 4"
	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casetype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("connectnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("comcnt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linespeed") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linetel") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("linearrivedat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("remark") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


<Worksheet ss:Name="五戶">
  <Table ID="Table5">
   <Row>
    <Cell ss:MergeAcross="13" ss:StyleID="TitleDate"><Data ss:Type="String">統計截止日期：<%=datevalue(now())%> </Data></Cell>
   </Row>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">連接方式</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">規模戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線速率</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">附掛電話</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">主線到位日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">鄉鎮</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">地址</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">備註</Data></Cell>
   </Row>
<%
	sql="usp_RTCmtyLowCust 5"
	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("casetype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("connectnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("comcnt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linespeed") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linetel") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("linearrivedat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("custnum") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("addr") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("remark") &"</Data></Cell>" &_
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
