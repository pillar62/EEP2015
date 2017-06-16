<%
    parm=request("parm")
    v=split(parm,";")

	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=P&S用戶列表("& year(now())-1911 & right("0"& month(now()),2) & right("0"& day(now()),2) & ").xls"

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

 <Worksheet ss:Name="P&S用戶列表">
  <Table>

   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="9" ss:StyleID="Title"><Data ss:Type="String">元訊寬頻網路股份有限公司</Data></Cell>
   </Row>
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="9" ss:StyleID="Title"><Data ss:Type="String">P&S用戶列表</Data></Cell>
   </Row>
   <Row>
   <%
		if  len(v(0))=0 or len(v(1)) =0 then sndwrknc="" else finishnc ="派工日：" &v(0)& " ~ " &v(1)& "　" end if
		if  len(v(2))=0 or len(v(3)) =0 then finishnc="" else sndwrknc ="完工日：" &v(2)& " ~ " &v(3)& "　" end if
		if  len(v(4))=0 or len(v(5)) =0 then dropnc="" else dropnc ="退租日：" &v(4)& " ~ " &v(5)& "　" end if
		if  len(v(6))=0 or len(v(7)) =0 then cancelnc="" else cancelnc ="作廢日：" &v(6)& " ~ " &v(7)& "　" end if
   %>
    <Cell ss:MergeAcross="9" ss:StyleID="TitleDate"><Data ss:Type="String"><%= sndwrknc & finishnc & dropnc & cancelnc %></Data></Cell>
   </Row>

   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">T 帳號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">用戶名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">聯絡電話</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">手機</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">派工日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">完工日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">實際完工人</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">退租日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">作廢日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">裝機地址</Data></Cell>
   </Row>
<%
	if len(v(0))=0 or len(v(1))=0 then 
		sndwrkdat =" "
	else
		sndwrkdat =" and convert(datetime, convert(varchar(10), c.sndwrkdat, 111)) between '" &V(0)& "' and '" &V(1)& "' "
	end if

	if len(v(2))=0 or len(v(3))=0 then 
		finishdat =" a.cusid <>'' "
	else
		finishdat =" a.finishdat between '" &V(2)& "' and '" &V(3)& "' "
	end if

	if len(v(4))=0 or len(v(5))=0 then 
		dropdat =" "
	else
		dropdat =" and a.dropdat is not null and convert(datetime, convert(varchar(10), a.dropdat, 111)) between '" &V(4)& "' and '" &V(5)& "' "
	end if

	if len(v(6))=0 or len(v(7))=0 then 
		canceldat =" "
	else
		canceldat =" and a.canceldat is not null and convert(datetime, convert(varchar(10), a.canceldat, 111)) between '" &V(6)& "' and '" &V(7)& "' "
	end if

	sql="select a.NCICCUSNO, a.CUSNC, a.CONTACTTEL, a.MOBILE, c.sndwrkdat, a.finishdat, isnull(c.finishnc,'') as finishusr, " &_
		"a.dropdat, a.canceldat, isnull(b.cutnc,'')+a.township2+a.raddr2 as raddr " &_
		"from RTSparqWagalyCust a " &_
		"left outer join RTCounty b on a.cutid2 = b.cutid " &_
		"left outer join (select o.cusid, isnull(r.shortnc, q.cusnc) as finishnc, o.sndwrkdat " &_
		"				from RTSparqWagalySndWrk o " &_
		"				left outer join RTEmployee p inner join RTObj q on q.cusid = p.cusid on o.finisheng = p.emply " &_
		"				left outer join RTObj r on o.finishcons = r.cusid " &_
		"				where o.worktype ='01' and o.canceldat is null) c on c.cusid = a.cusid " &_
		"where " & finishdat & sndwrkdat & dropdat & canceldat &_
		" order by a.finishdat, c.sndwrkdat "
'response.write sql
	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("nciccusno") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CONTACTTEL") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("MOBILE") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("sndwrkdat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("finishdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("finishusr") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("canceldat") &"</Data></Cell>" &_
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
