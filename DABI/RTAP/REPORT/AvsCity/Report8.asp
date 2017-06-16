<%
    parm=request("parm")
    v=split(parm,";")
	filename= dateadd("d",-1, cdate(v(0)))

	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=自營經銷商拆帳("& year(filename) & right("0"& month(filename),2) & ").xls"

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


 <Worksheet ss:Name="AVS明細">
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
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">用戶代號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">用戶名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">繳款方式</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">繳款週期</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">完工日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">開始計費日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">續約日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">到期日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">退租日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">列帳年</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">列帳月</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">認列收入</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">經銷商拆帳比率</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">轄區所屬</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">經銷商拆帳金額</Data></Cell>
   </Row>
<%
	sql="select convert(varchar(6), a.COMQ1) +'-'+ convert(varchar(2), a.lineq1) as comq, " &_
		"b.comn, a.CUSID, a.cusnc, h.codenc as paytype, g.codenc as paycycle, " &_
		"a.FINISHDAT, a.STRBILLINGDAT, a.newBILLINGDAT, a.DUEDAT, " &_
		"a.DROPDAT, d.TYY, d.TMM,  	sum(d.amt) as amt, " &_
		"case c.consignee when '' then 0 else 0.5 end as ratio, isnull(e.shortnc, i.cusnc) as belongnc, " &_
		"sum(case c.consignee when '' then 0 else 0.5 end * d.amt) as shareamt " &_
		"from 	RTLessorAVSCust a " &_
		"inner join RTLessorAVSCmtyH b on b.comq1 = a.comq1 " &_
		"inner join RTLessorAVScmtyline c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 " &_
		"inner join RTLessorAVSCustARDTL d on d.cusid = a.cusid " &_
		"left outer join rtobj e on e.cusid = c.consignee " &_
		"left outer join rtcode g on g.code = a.paycycle and g.kind ='M8' " &_
		"left outer join rtcode h on h.code = a.paytype and h.kind ='M9' " &_
		"left outer join rtemployee j inner join rtobj i on i.cusid = j.cusid on j.emply = c.salesid " &_
		"WHERE	a.freecode <>'Y' " &_
		"and d.TYY = datepart(yyyy, dateadd(d, -1,'" &v(0)& "')) " &_
		"and d.TMM = datepart(m, dateadd(d, -1,'" &v(0)& "')) " &_
		"and d.canceldat is null " &_
		"and (a.dropdat is null  or a.dropdat >= dateadd(m, -1, '" &v(0)& "')) " &_
		"and (d.ITEMNC LIKE '%網路服務費%') " &_
		"group by isnull(e.shortnc, i.cusnc), convert(varchar(6), a.COMQ1) +'-'+ convert(varchar(2), a.lineq1), " &_
		"b.comn, a.CUSID, a.cusnc, h.codenc, g.codenc, " &_
		"a.FINISHDAT, a.STRBILLINGDAT, a.newBILLINGDAT, a.DUEDAT, " &_
		"a.DROPDAT, d.TYY, d.TMM, " &_
		"case c.consignee when '' then 0 else 0.5 end " &_
		"having sum(amt) >0 " &_
		"order by 1,3,5 "

	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("paytype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("paycycle") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& datevalue(rs("finishdat")) &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("strbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("newbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("duedat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("tyy") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("tmm") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("amt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("ratio") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("belongnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("shareamt") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="ET明細">
  <Table>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">用戶代號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">用戶名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">繳款方式</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">繳款週期</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">完工日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">開始計費日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">續約日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">到期日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">退租日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">列帳年</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">列帳月</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">認列收入</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">經銷商拆帳比率</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">轄區所屬</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">經銷商拆帳金額</Data></Cell>
   </Row>
<%
	sql="select convert(varchar(6), a.COMQ1) +'-'+ convert(varchar(2), a.lineq1) as comq, " &_
		"b.comn, a.CUSID, a.cusnc, h.codenc as paytype, g.codenc as paycycle, " &_
		"a.FINISHDAT, a.STRBILLINGDAT, a.newBILLINGDAT, a.DUEDAT, a.DROPDAT, " &_
		"d.TYY, d.TMM, sum(d.amt) as amt, case c.consignee when '' then 0 else 0.5 end as ratio, " &_
		"isnull(e.shortnc, i.cusnc) as belongnc, sum(case c.consignee when '' then 0 else 0.5 end * d.amt) as shareamt " &_
		"from 	RTLessorCust a " &_
		"inner join RTLessorCmtyH b on b.comq1 = a.comq1 " &_
		"inner join RTLessorcmtyline c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 " &_
		"inner join RTLessorCustARDTL d on d.cusid = a.cusid " &_
		"left outer join rtobj e on e.cusid = c.consignee " &_
		"left outer join rtcode g on g.code = a.paycycle and g.kind ='M8' " &_
		"left outer join rtcode h on h.code = a.paytype and h.kind ='M9' " &_
		"left outer join rtemployee j inner join rtobj i on i.cusid = j.cusid on j.emply = c.salesid " &_
		"WHERE	a.freecode <>'Y' " &_
		"and 	d.TYY = datepart(yyyy, dateadd(d, -1,'" &v(0)& "')) " &_
		"and 	d.TMM = datepart(m, dateadd(d, -1,'" &v(0)& "')) " &_
		"and 	d.canceldat is null " &_
		"and	(a.dropdat is null  or a.dropdat >= dateadd(m, -1, '" &v(0)& "')) " &_
		"and	(d.ITEMNC LIKE '%網路服務費%') " &_
		"group by 	isnull(e.shortnc, i.cusnc), convert(varchar(6), a.COMQ1) +'-'+ convert(varchar(2), a.lineq1), " &_
		"b.comn, a.CUSID, a.cusnc, h.codenc, g.codenc, " &_
		"a.FINISHDAT, a.STRBILLINGDAT, a.newBILLINGDAT, a.DUEDAT, " &_
		"a.DROPDAT, d.TYY, d.TMM, " &_
		"case c.consignee when '' then 0 else 0.5 end " &_
		"having sum(amt) >0 " &_
		"order by 1,3,5 "

	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("paytype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("paycycle") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& datevalue(rs("finishdat")) &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("strbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("newbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("duedat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("tyy") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("tmm") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("amt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("ratio") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("belongnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("shareamt") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="IPTV明細">
  <Table>
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">主線</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">用戶代號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">用戶名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">繳款方式</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">繳款週期</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">完工日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">開始計費日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">續約日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">到期日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">退租日</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">列帳年</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">列帳月</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">認列收入</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">經銷商拆帳比率</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">轄區所屬</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">經銷商拆帳金額</Data></Cell>
   </Row>
<%
	sql="select convert(varchar(6), a.COMQ1) +'-'+ convert(varchar(2), a.lineq1) as comq, " &_
	"b.comn, a.CUSID, a.cusnc, h.codenc as paytype, g.codenc as paycycle, " &_
	"a.FINISHDAT, a.STRBILLINGDAT, a.newBILLINGDAT, a.DUEDAT, " &_
	"a.DROPDAT, d.TYY, d.TMM,  	sum(d.amt) as amt, 0.5 as ratio, '傳媒' as belongnc, " &_
	"sum(d.amt * 0.5) as shareamt " &_
	"from 	RTLessorAVSCust a " &_
	"inner join RTLessorAVSCmtyH b on b.comq1 = a.comq1 " &_
	"inner join RTLessorAVScmtyline c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 " &_
	"inner join RTLessorAVSCustARDTL d on d.cusid = a.cusid " &_
	"left outer join rtobj e on e.cusid = c.consignee " &_
	"left outer join rtcode g on g.code = a.paycycle and g.kind ='M8' " &_
	"left outer join rtcode h on h.code = a.paytype and h.kind ='M9' " &_
	"left outer join rtemployee j inner join rtobj i on i.cusid = j.cusid on j.emply = c.salesid " &_
	"WHERE	a.freecode <>'Y' " &_
	"and 	d.TYY = datepart(yyyy, dateadd(d, -1,'" &v(0)& "')) " &_
	"and 	d.TMM = datepart(m, dateadd(d, -1,'" &v(0)& "')) " &_
	"and 	d.canceldat is null " &_
	"and	(a.dropdat is null or a.dropdat >= dateadd(m, -1, '" &v(0)& "')) " &_
	"and	(d.ITEMNC LIKE '%網路服務費%') " &_
	"and 	b.comn like '江山萬里%' " &_
	"group by isnull(e.shortnc, i.cusnc), convert(varchar(6), a.COMQ1) +'-'+ convert(varchar(2), a.lineq1), " &_
	"b.comn, a.CUSID, a.cusnc, h.codenc, g.codenc, " &_
	"a.FINISHDAT, a.STRBILLINGDAT, a.newBILLINGDAT, a.DUEDAT, " &_
	"a.DROPDAT, d.TYY, d.TMM, " &_
	"case c.consignee when '' then 0 else 0.5 end " &_
	"having sum(amt) >0 " &_
	"order by 	 1,3,5 "

	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("paytype") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("paycycle") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& datevalue(rs("finishdat")) &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("strbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("newbillingdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("duedat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rs("dropdat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("tyy") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("tmm") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("amt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("ratio") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("belongnc") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("shareamt") &"</Data></Cell>" &_
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
