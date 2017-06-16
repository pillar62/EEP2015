<%
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=客訴一覽表.xls"
    v=split(request("parm"),";")
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
   <Alignment ss:Horizontal="Left" ss:Vertical="Center"/>
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

	<Style ss:ID="HeaderY">
		<Alignment ss:Horizontal="Center"/>
   		<Borders>
    		<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Bold="1"/>
   		<Interior ss:Color="#FFFF99" ss:Pattern="Solid"/>
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

	<Style ss:ID="toCharBrown">
		<Alignment ss:Vertical="Center"/>
		<Borders>
    		<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Color="brown"/>
   		<NumberFormat ss:Format="@"/>
  	</Style>

	<Style ss:ID="toCharWrap">
		<Alignment ss:Vertical="Center" ss:WrapText="1"/>
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

	<Style ss:ID="toNumBrown">
   		<Borders>
		    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Color="brown"/>
  	</Style>

	<Style ss:ID="toDate">
   		<Borders>
		    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman"/>
		<NumberFormat ss:Format="yyyy/m/d;@"/>
  	</Style>

	<Style ss:ID="toDateTime">
   		<Borders>
		    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman"/>
		<NumberFormat ss:Format="yyyy/m/d\ h:mm;@"/>
  	</Style>

	<Style ss:ID="toDateTimeBrown">
   		<Borders>
		    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Color="brown"/>
		<NumberFormat ss:Format="yyyy/m/d\ h:mm;@"/>
  	</Style>

 </Styles>


 <Worksheet ss:Name="工程師行程一覽表">
  <Table ID="Table1">
<!--
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">元訊寬頻網路股份有限公司</Data></Cell>
   </Row>
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">速博退租戶一覽表</Data></Cell>
   </Row>
-->    
   <Row>
    <Cell ss:MergeAcross="15" ss:StyleID="TitleDate"><Data ss:Type="String">處理日期：<%=v(0)%> ~ <%=v(1)%> </Data></Cell>
   </Row>

   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">行程單號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">工程師</Data></Cell>
	<Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
	<Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區序號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">派工單號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">派工日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">處理日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">點數</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">點數項目</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">聯絡人</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客訴原因</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客服備註</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">完工備註</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">行程備註</Data></Cell>
   </Row>
<%
    Dim conn, rsxx
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rsxx=Server.CreateObject("ADODB.Recordset")

	sqlxx=	"select a.schno, f.cusnc as engsales, isnull(b.codenc,'') as casenc, " &_
			"isnull(convert(varchar(5), n.comq1)+case when n.lineq1=0 then '' else '-'+convert(varchar(5), n.lineq1) end,'') as comq, " &_
			"isnull(n.comn,'') as comn, isnull(c.cusnc,'') as cusnc, a.workno, convert(datetime,convert(varchar(10),g.sndwrkdat,111)) as sndwrkdat, a.dealdat, " &_
			"convert(varchar(5), case a.score01 when 'Y' then convert(float,aa.parm1) else 0 end + " &_
			"case a.score02 when 'Y' then convert(float,bb.parm1) else 0 end + " &_
			"case a.score03 when 'Y' then convert(float,cc.parm1) else 0 end + " &_
			"case a.score04 when 'Y' then convert(float,dd.parm1) else 0 end + " &_
			"case a.score05 when 'Y' then convert(float,ee.parm1) else 0 end + " &_
			"case a.score06 when 'Y' then convert(float,ff.parm1) else 0 end + " &_
			"case a.score07 when 'Y' then convert(float,gg.parm1) else 0 end + " &_
			"case a.score08 when 'Y' then convert(float,hh.parm1) else 0 end + " &_
			"case a.score09 when 'Y' then convert(float,ii.parm1) else 0 end + " &_
			"case a.score10 when 'Y' then convert(float,jj.parm1) else 0 end + " &_
			"case a.score11 when 'Y' then convert(float,kk.parm1) else 0 end + " &_
			"case a.score12 when 'Y' then convert(float,ll.parm1) else 0 end + " &_
			"case a.score13 when 'Y' then convert(float,mm.parm1) else 0 end + " &_
			"case a.score14 when 'Y' then convert(float,nn.parm1) else 0 end + " &_
			"case a.score15 when 'Y' then convert(float,oo.parm1) else 0 end + " &_
			"case a.score16 when 'Y' then convert(float,pp.parm1) else 0 end) as score, " &_
			"case a.score01 when 'Y' then '('+aa.parm1+')'+aa.codenc+'' else '' end + " &_
			"case a.score02 when 'Y' then '('+bb.parm1+')'+bb.codenc+'' else '' end + " &_
			"case a.score03 when 'Y' then '('+cc.parm1+')'+cc.codenc+'' else '' end + " &_
			"case a.score04 when 'Y' then '('+dd.parm1+')'+dd.codenc+'' else '' end + " &_
			"case a.score05 when 'Y' then '('+ee.parm1+')'+ee.codenc+'' else '' end + " &_
			"case a.score06 when 'Y' then '('+ff.parm1+')'+ff.codenc+'' else '' end + " &_
			"case a.score07 when 'Y' then '('+gg.parm1+')'+gg.codenc+'' else '' end + " &_
			"case a.score08 when 'Y' then '('+hh.parm1+')'+hh.codenc+'' else '' end + " &_
			"case a.score09 when 'Y' then '('+ii.parm1+')'+ii.codenc+'' else '' end + " &_
			"case a.score10 when 'Y' then '('+jj.parm1+')'+jj.codenc+'' else '' end + " &_
			"case a.score11 when 'Y' then '('+kk.parm1+')'+kk.codenc+'' else '' end + " &_
			"case a.score12 when 'Y' then '('+ll.parm1+')'+ll.codenc+'' else '' end + " &_
			"case a.score13 when 'Y' then '('+mm.parm1+')'+mm.codenc+'' else '' end + " &_
			"case a.score14 when 'Y' then '('+nn.parm1+')'+nn.codenc+'' else '' end + " &_
			"case a.score15 when 'Y' then '('+oo.parm1+')'+oo.codenc+'' else '' end + " &_
			"case a.score16 when 'Y' then '('+pp.parm1+')'+pp.codenc else '' end as itemnc, " &_
			"isnull(h.faqman,'') as faqman, isnull(i.codenc,'') as faqreason, " &_
			"isnull(h.memo,'') as faqmemo, 	isnull(g.memo,'') as finishmemo, a.memo as schmemo " &_
			"FROM RTSalesSch a " &_
			"left outer join RTCode b on a.comtype = b.code and b.kind ='P5' " &_
			"left outer join HBAdslCmtyCust c on a.comtype = c.comtype and a.comq1 = c.comq1 and a.lineq1 = c.lineq1 and a.cusid = c.cusid and a.entryno = c.entryno " &_
			"left outer join RTEmployee e inner join RTObj f on f.cusid = e.cusid on e.emply = a.DEALUSR " &_
			"left outer join RTSndwork g inner join RTFaqM h on g.linkno = h.caseno on g.workno = a.workno " &_
			"left outer join RTCode i on i.code = h.faqreason and i.kind ='P7' " &_
			"left outer join HBAdslCmty n on a.comtype = n.comtype and a.comq1 = n.comq1 and a.lineq1 = n.lineq1 " &_
			"left outer join RTCode aa on replace(a.score01,'Y','01') = aa.code and aa.kind ='R5' " &_
			"left outer join RTCode bb on replace(a.score02,'Y','02') = bb.code and bb.kind ='R5' " &_
			"left outer join RTCode cc on replace(a.score03,'Y','03') = cc.code and cc.kind ='R5' " &_
			"left outer join RTCode dd on replace(a.score04,'Y','04') = dd.code and dd.kind ='R5' " &_
			"left outer join RTCode ee on replace(a.score05,'Y','05') = ee.code and ee.kind ='R5' " &_
			"left outer join RTCode ff on replace(a.score06,'Y','06') = ff.code and ff.kind ='R5' " &_
			"left outer join RTCode gg on replace(a.score07,'Y','07') = gg.code and gg.kind ='R5' " &_
			"left outer join RTCode hh on replace(a.score08,'Y','08') = hh.code and hh.kind ='R5' " &_
			"left outer join RTCode ii on replace(a.score09,'Y','09') = ii.code and ii.kind ='R5' " &_
			"left outer join RTCode jj on replace(a.score10,'Y','10') = jj.code and jj.kind ='R5' " &_
			"left outer join RTCode kk on replace(a.score11,'Y','11') = kk.code and kk.kind ='R5' " &_
			"left outer join RTCode ll on replace(a.score12,'Y','12') = ll.code and ll.kind ='R5' " &_
			"left outer join RTCode mm on replace(a.score13,'Y','13') = mm.code and mm.kind ='R5' " &_
			"left outer join RTCode nn on replace(a.score14,'Y','14') = nn.code and nn.kind ='R5' " &_
			"left outer join RTCode oo on replace(a.score15,'Y','15') = oo.code and oo.kind ='R5' " &_
			"left outer join RTCode pp on replace(a.score16,'Y','16') = pp.code and pp.kind ='R5' " &_
			"where 	a.canceldat is null " &_
			"and a.dealdat between '"& v(0) &"' and '"& v(1) &"' " &_
			"order by a.dealusr, a.dealdat desc, n.comn, a.schno desc "
			
'response.Write "<Row><Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& sqlxx &"</Data></Cell></Row>"

	rsxx.Open sqlxx, CONN
	Do While Not rsxx.Eof
	    response.Write "<Row>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsxx("schno") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsxx("engsales") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsxx("casenc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsxx("comq") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsxx("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsxx("cusnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsxx("workno") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rsxx("sndwrkdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rsxx("dealdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rsxx("score") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsxx("itemnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsxx("faqman") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsxx("faqreason") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsxx("faqmemo") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsxx("finishmemo") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsxx("schmemo") &"</Data></Cell>" &_
			"</Row>"
      rsxx.MoveNext
    Loop
    rsxx.Close
%>
  </Table>
 </Worksheet>

<%
	conn.Close
	set rsxx = nothing
	set conn = nothing
%>
</Workbook>

