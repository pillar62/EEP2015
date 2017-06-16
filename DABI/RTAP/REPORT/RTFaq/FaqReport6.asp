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


 <Worksheet ss:Name="客訴一覽表">
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
    <Cell ss:StyleID="TitleDate"><Data ss:Type="String">受理時間：<%=v(0)%> ~ <%=v(1)%> <% if len(trim(v(8))) >0 or len(trim(v(9))) then response.write "　完工時間："& v(8) &" ~ "& v(9) end if %> </Data></Cell>
   </Row>


    

   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客訴單號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶來源</Data></Cell>
	<Cell ss:StyleID="HeaderS"><Data ss:Type="String">方案別</Data></Cell>
	<Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">直經銷</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">工程師</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">聯絡人</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">聯絡電話</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">聯絡手機</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">用戶地址</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">進出線</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客訴原因</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客服處理</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">受理時間</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">受理人</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">結案時間</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">結案人</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">來電詢問方案</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">WTL申請日</Data></Cell>

    <Cell ss:StyleID="Header"><Data ss:Type="String">追件項次</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">進出線</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">追件處理</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">追件時間</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">追件人</Data></Cell>

    <Cell ss:StyleID="HeaderY"><Data ss:Type="String">派工時間</Data></Cell>
    <Cell ss:StyleID="HeaderY"><Data ss:Type="String">派工處理</Data></Cell>
    <Cell ss:StyleID="HeaderY"><Data ss:Type="String">預定施工人</Data></Cell>
    <Cell ss:StyleID="HeaderY"><Data ss:Type="String">實際施工人</Data></Cell>
    <Cell ss:StyleID="HeaderY"><Data ss:Type="String">完工種類</Data></Cell>
    <Cell ss:StyleID="HeaderY"><Data ss:Type="String">完工時間</Data></Cell>
    <Cell ss:StyleID="HeaderY"><Data ss:Type="String">完工人</Data></Cell>
    <Cell ss:StyleID="HeaderY"><Data ss:Type="String">即時完修</Data></Cell>
    <Cell ss:StyleID="HeaderY"><Data ss:Type="String">績效點數</Data></Cell>
    <Cell ss:StyleID="HeaderY"><Data ss:Type="String">績效項目</Data></Cell>
   </Row>
<%
    Dim rsxx, rsyy, rszz,conn
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rsxx=Server.CreateObject("ADODB.Recordset")
    Set rsyy=Server.CreateObject("ADODB.Recordset")
    Set rszz=Server.CreateObject("ADODB.Recordset")

	if len(trim(v(2))) >0 then comtype = " and a.comtype = '" &v(2)& "' "
	if len(trim(v(3))) >0 then rcvusr= " and a.rcvusr = '" &v(3)& "' "
	if len(trim(v(4))) >0 or len(trim(v(5))) >0 or len(trim(v(8))) >0 or len(trim(v(9))) >0 then 
		assignwrk = " inner "
	else
		assignwrk = " left outer "
	end if

	if len(trim(v(7))) >0 then 
		areancjoin = " inner "
		areanc = " and case q.areanc when '' then q.groupnc else q.areanc end = '" &v(7)& "' "
	else
		areancjoin = " left outer "
		areanc =" "
	end if

	if v(6) ="2" then		'未結案
		finish = " and a.closedat is null "
		finish1 = " and t.closedat is null "
	elseif v(6) ="1" then	'未完工
		assignwrk = " inner "
		finish = ""
		finish1 = " and s.finishdat is null "
	else
		finish = ""		:	finish1 =""
	end if

	if len(trim(v(8))) >0 or len(trim(v(9))) then
		finish2 = " and	s.finishdat between '" &v(8)& "' and dateadd(s, -1, dateadd(d, 1, '" &v(9)& "')) "
	else
		finish2 =""
	end if

	sqlxx=	"select r.codenc as comtypenc, isnull(q.areanc,'') as areanc, case q.groupnc when '' then '直銷' else '經銷' end as belongnc, " &_
			"case q.groupnc when '' then q.leader else q.groupnc end as salesnc, q.comn, isnull(o.addcount,0) as addcount, isnull(p.wrkcount, 0) as wrkcount, " &_
			"a.caseno, a.faqman, a.tel, a.mobile, c.raddr, j.codenc as iobound, k.codenc as faqreason, replace(convert(varchar(25), a.rcvdat, 120),' ','T') as rcvdat, " &_
			"f.cusnc as rcvusr, a.memo, replace(convert(varchar(25), a.closedat, 120),' ','T') as closedat, n.cusnc as closeusr, u.codenc as askcasenc, v.codenc as custsrcnc, g.wtlapplydat " &_
			"from RTFaqM a " &_
			"left outer join HBAdslCmtyCust c on a.comtype = c.comtype and a.comq1 = c.comq1 and a.lineq1 = c.lineq1 and a.cusid = c.cusid and a.entryno = c.entryno " &_
			"left outer join RTEmployee e inner join RTObj f on f.cusid = e.cusid on e.emply = a.rcvusr " &_
			"left outer join RTCode j on a.iobound = j.code and j.kind ='P8' " &_
			"left outer join RTCode k on a.faqreason = k.code and k.kind ='P7' " &_
			"left outer join RTCode u on a.askcase = u.code and u.kind ='Q2' " &_
			"left outer join RTCode v on a.custsrc = v.code and v.kind ='Q3' " &_
			"left outer join RTCode r on a.comtype = r.code and r.kind ='P5' " &_			
			"left outer join RTEmployee m inner join RTObj n on n.cusid = m.cusid on m.emply = a.closeusr " &_

			"left outer join RTSparq499Cust g on g.comq1 = a.comq1 and g.lineq1 = a.lineq1 and g.cusid = a.cusid and a.comtype='6' " &_

			areancjoin & " join HBADSLCMTY q on q.comq1 = a.comq1 and q.lineq1 = a.lineq1 and a.comtype = q.comtype " &_
			" left outer join (	select x.caseno, count(*) as addcount " &_
			"					from RTFaqAdd x inner join RTFaqM y on x.caseno = y.caseno " &_
			"					where x.canceldat is null and y.canceldat is null " &_
			"					and	y.rcvdat between '" &v(0)& "' and dateadd(s, -1, dateadd(d, 1, '" &v(1)& "')) " &_
			"					group by x.caseno ) o on o.caseno = a.caseno " &_
			assignwrk &" join (	select t.caseno, count(*) as wrkcount " &_
			"					from RTSndWork s inner join RTFaqM t on s.linkno = t.caseno " &_
			"					where s.canceldat is null and t.canceldat is null " &_
			"					and	(s.worktype ='01' or s.worktype ='09') " & finish1 &_
			"					and	t.rcvdat between '" &v(0)& "' and dateadd(s, -1, dateadd(d, 1, '" &v(1)& "')) " &_
			"					and	s.assigneng like '%"&v(4)&"%' and s.assigncons like '%"&v(5)&"%' " & finish2 &_
			"					group by t.caseno ) p on p.caseno = a.caseno " &_
			"where a.canceldat is null " & comtype & rcvusr & areanc & finish &_
			" and		a.rcvdat between '" &v(0)& "' and dateadd(s, -1, dateadd(d, 1, '" &v(1)& "')) " &_
			" order by a.caseno "

'RESPONSE.WRITE sqlxx

   	if len(trim(v(2))) >0 then comtype = " and b.comtype = '" &v(2)& "' "
   	if len(trim(v(3))) >0 then rcvusr= " and b.rcvusr = '" &v(3)& "' "
	if v(6) ="2" then		'未結案
		finish = " and b.closedat is null "
	elseif v(6) ="1" then	'未完工
		finish = ""
		finish1 = " and s.finishdat is null "
	else
		finish = ""		:	finish1 =""
	end if
	if len(trim(v(4))) >0 or len(trim(v(5))) >0 or v(6) ="1" or len(trim(v(8))) >0 or len(trim(v(9))) >0 then 
		assignwrk =" inner join (select t.caseno, count(*) as wrkcount " &_
			"					from RTSndWork s inner join RTFaqM t on s.linkno = t.caseno " &_
			"					where s.canceldat is null and t.canceldat is null " &_
			"					and	(s.worktype ='01' or s.worktype ='09') " & finish1 &_
			"					and	t.rcvdat between '" &v(0)& "' and dateadd(s, -1, dateadd(d, 1, '" &v(1)& "')) " &_
			"					and	s.assigneng like '%"&v(4)&"%' and s.assigncons like '%"&v(5)&"%' " & finish2 &_
			"					group by t.caseno ) p on p.caseno = a.caseno "
	else
		assignwrk = ""
	end if

	if len(trim(v(7))) >0 then 
		areancjoin = " inner join HBADSLCMTY q on q.comq1 = b.comq1 and q.lineq1 = b.lineq1 and b.comtype = q.comtype "
		areanc = " and case q.areanc when '' then q.groupnc else q.areanc end = '" &v(7)& "' "		
	else
		areancjoin = " "
		areanc =" "
	end if

	sqlyy=	"select a.caseno, a.entryno, j.codenc as addiobound, replace(convert(varchar(25), a.adddat, 120),' ', 'T') as adddat, f.cusnc as addusr, a.memo as addmemo " &_
			"from	RTFaqAdd a " &_
			"inner join RTFaqM b on a.caseno = b.caseno " & assignwrk &_
			"left outer join RTCode j on a.iobound = j.code and j.kind ='P8' " &_
			"left outer join RTEmployee e inner join RTObj f on f.cusid = e.cusid on e.emply = a.addusr " &_
			areancjoin &_
			" where a.canceldat is null and b.canceldat is null " & comtype & rcvusr & areanc & finish &_
			" and b.rcvdat between '" &v(0)& "' and dateadd(s, -1, dateadd(d, 1, '" &v(1)& "')) " &_
			" order by a.caseno, a.entryno "

'RESPONSE.WRITE sqlyy

   if len(trim(v(2))) >0 then comtype = " and b.comtype = '" &v(2)& "' "
   if len(trim(v(3))) >0 then rcvusr= " and b.rcvusr = '" &v(3)& "' "
	if v(6) ="2" then		'未結案
		finish = " and b.closedat is null "
	elseif v(6) ="1" then	'未完工
		finish = " and a.finishdat is null "
	else
		finish = ""
	end if
	if len(trim(v(7))) >0 then 
		areancjoin = " inner join HBADSLCMTY q on q.comq1 = b.comq1 and q.lineq1 = b.lineq1 and b.comtype = q.comtype "
		areanc = " and case q.areanc when '' then q.groupnc else q.areanc end = '" &v(7)& "' "				
	else
		areancjoin = " "
		areanc =" "
	end if

	if len(trim(v(8))) >0 or len(trim(v(9))) then
		finish3 = " and	a.finishdat between '" &v(8)& "' and dateadd(s, -1, dateadd(d, 1, '" &v(9)& "')) "
	else
		finish3 =""
	end if

	sqlzz=	"select b.caseno, replace(convert(varchar(25), a.sndwrkdat, 120),' ', 'T') as sndwrkdat, " &_
			"isnull(k.shortnc, i.name) as assignusr, isnull(d.shortnc, c.name) as finishnc, isnull(l.codenc, '') as finishtypnc, " &_
			"replace(convert(varchar(25), a.finishdat, 120),' ', 'T') as finishdat, f.cusnc as finishusr, a.memo as wrkmemo, " &_
			"case when datediff(hh, a.sndwrkdat, isnull(a.finishdat, getdate())) <= 48 and a.finishdat is not null then 'Y' when datediff(hh, a.sndwrkdat, isnull(a.finishdat, getdate())) > 48 and a.finishdat is not null then 'N' else '' end as uptodate, " &_
			"convert(smallmoney,replace(SCORE01,'Y',2))+ " &_
			"convert(smallmoney,replace(SCORE02,'Y',1))+ " &_
			"convert(smallmoney,replace(SCORE03,'Y',1))+ " &_
			"convert(smallmoney,replace(SCORE04,'Y',1))+ " &_
			"convert(smallmoney,replace(SCORE05,'Y',1))+ " &_
			"convert(smallmoney,replace(SCORE06,'Y',1))+ " &_
			"convert(smallmoney,replace(SCORE07,'Y',1))+ " &_
			"convert(smallmoney,replace(SCORE08,'Y',1))+ " &_
			"convert(smallmoney,replace(SCORE09,'Y',1))+ " &_
			"convert(smallmoney,replace(SCORE10,'Y',0.8))+ " &_
			"convert(smallmoney,replace(SCORE11,'Y',0.3))+ " &_
			"convert(smallmoney,replace(SCORE12,'Y',1))+ " &_
			"convert(smallmoney,replace(SCORE13,'Y',1))+ " &_
			"convert(smallmoney,replace(SCORE14,'Y',0.3))+ " &_
			"convert(smallmoney,replace(SCORE15,'Y',0.3)) as scoresum, " &_
			"replace(SCORE01,'Y','申請AVS、FBB(業務)&#10;')+ " &_
			"replace(SCORE02,'Y','安裝Sonet﹑SQ﹑AVS Call Out續約與其他&#10;')+ " &_
			"replace(SCORE03,'Y','勸用戶退租與撤線&#10;')+ " &_
			"replace(SCORE04,'Y','主機安裝&#10;')+ " &_
			"replace(SCORE05,'Y','主機開通&#10;')+ " &_
			"replace(SCORE06,'Y','用戶端裝機&#10;') + " &_
			"replace(SCORE07,'Y','主機拆回、拆 Port&#10;')+ " &_
			"replace(SCORE08,'Y','退租拆機&#10;')+ " &_
			"replace(SCORE09,'Y','用戶端設備及文件收回&#10;')+ " &_
			"replace(SCORE10,'Y','維修(需有派工單)&#10;')+ " &_
			"replace(SCORE11,'Y','線上維修&#10;')+ " &_
			"replace(SCORE12,'Y','發DM(需有派工單)&#10;')+ " &_
			"replace(SCORE13,'Y','社區深耕活動(需有派工單)&#10;')+ " &_
			"replace(SCORE14,'Y','放DM、整理DM&#10;')+ " &_
			"replace(SCORE15,'Y','社區電信室勘查') as scoreitem " &_
			"from	RTSndWork a " &_
			"inner join RTFaqM b on a.linkno = b.caseno " &_
			"left outer join RTEmployee e inner join RTObj f on f.cusid = e.cusid on e.emply = a.finishusr " &_
			"left outer join RTEmployee i on i.emply = a.assigneng " &_
			"left outer join RTObj k on k.cusid = a.assigncons " &_
			"left outer join RTCode l on a.finishtyp = l.code and l.kind ='P9' " &_
			"left outer join RTEmployee c on c.emply = a.finisheng " &_
			"left outer join RTObj d on d.cusid = a.finishcons " &_
			areancjoin &_
			" where a.canceldat is null and b.canceldat is null " &_
			" and	(a.worktype ='01' or a.worktype ='09') " & comtype & rcvusr & areanc & finish & finish3 &_
			" and	b.rcvdat between '" &v(0)& "' and dateadd(s, -1, dateadd(d, 1, '" &v(1)& "')) " &_
			" and	a.assigneng like '%"&v(4)&"%' and a.assigncons like '%"&v(5)&"%' " &_
			" order by b.caseno, a.workno "

'RESPONSE.WRITE sqlzz

	rsxx.Open sqlxx, CONN
	rsyy.Open sqlyy, CONN
	rszz.Open sqlzz, CONN

	afirst = true
	Do While Not rsxx.Eof
		'客訴單
		if afirst = true then 
			caseno = rsxx("caseno")		:	comtypenc = rsxx("comtypenc")	:	belongnc = rsxx("belongnc")
			salesnc = rsxx("salesnc")	:	comn = rsxx("comn")				:	faqman = rsxx("faqman")
			iobound = rsxx("iobound")	:	rcvdat = rsxx("rcvdat")			:	rcvusr = rsxx("rcvusr")
			closedat = rsxx("closedat")	:	closeusr = rsxx("closeusr")		:	memo = server.HTMLEncode(rsxx("memo"))
			tel = rsxx("tel")			:	mobile = rsxx("mobile")			:	faqreason = rsxx("faqreason")
			raddr = rsxx("raddr")		:	areanc = rsxx("areanc")			:	askcasenc = rsxx("askcasenc")
			custsrcnc = rsxx("custsrcnc") : wtlapplydat = rsxx("wtlapplydat")
		else
			caseno = ""		:	comtypenc = ""	:	belongnc = ""
			salesnc = ""	:	comn = ""		:	faqman = ""
			iobound = ""	:	rcvdat = ""		:	rcvusr = ""
			closedat = ""	:	closeusr = ""	:	memo = ""
			tel = ""		:	mobile	=""		:	faqreason = ""
			raddr =""		:	areanc =""		:	askcasenc = ""
			custsrcnc =""	:	wtlapplydat =""
		end if
		if rsxx("addcount") >= rsxx("wrkcount") then
			mergecount = rsxx("addcount")
		else
			mergecount = rsxx("wrkcount")
		end if

		'追件
		entryno = ""	:	addiobound = ""	:	adddat = ""
		addusr = ""		:	addmemo = ""
		if not rsyy.EOF then 
			if rsyy("caseno") = rsxx("caseno") then
				entryno = rsyy("entryno")	:	addiobound = rsyy("addiobound")	:	adddat = rsyy("adddat")
				addusr = rsyy("addusr")		:	addmemo = server.HTMLEncode(rsyy("addmemo"))
				rsyy.MoveNext
			end if 
		end if	

		'派工單
		sndwrkdat =""	:	assignusr = ""	:	finishdat = ""	:	finishtypnc = ""
		finishusr = ""	:	wrkmemo = ""	:	finishnc = ""	:	uptodate = ""
		scoresum =0 	: 	scoreitem = ""
		if not rszz.EOF then 
			if rszz("caseno") = rsxx("caseno") then
				sndwrkdat = rszz("sndwrkdat")	:	assignusr = rszz("assignusr")					:	finishdat = rszz("finishdat")
				finishusr = rszz("finishusr")	:	wrkmemo = server.HTMLEncode(rszz("wrkmemo"))	:	finishnc = rszz("finishnc")
				uptodate = rszz("uptodate")		:	finishtypnc = rszz("finishtypnc")				:	scoresum = rszz("scoresum")
				scoreitem = rszz("scoreitem")
				rszz.MoveNext
			end if
		end if

	    response.Write "<Row>"
		if afirst = true then
			if mergecount -1 > 0 then
				mergedown = " ss:MergeDown="""& mergecount -1 &""" "
			else
				mergedown = ""
			end if

			response.Write _
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& caseno &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& custsrcnc &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& comtypenc &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& areanc &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& belongnc &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& salesnc &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& comn &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& faqman &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& tel &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& mobile &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& raddr &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& iobound &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& faqreason &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& memo &"</Data></Cell>"

			if len(trim(rcvdat)) = 0 or isnull(rcvdat) then
				response.Write "<Cell "& mergedown &" ss:StyleID=""toDateTime""/>"
			else
				response.Write "<Cell "& mergedown &" ss:StyleID=""toDateTime""><Data ss:Type=""DateTime"">"& rcvdat &"</Data></Cell>"
			end if

			response.Write _
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& rcvusr &"</Data></Cell>"

			if len(trim(closedat)) = 0 or isnull(closedat) then
				response.Write "<Cell "& mergedown &" ss:StyleID=""toDateTime""/>"
			else
				response.Write "<Cell "& mergedown &" ss:StyleID=""toDateTime""><Data ss:Type=""DateTime"">"& closedat &"</Data></Cell>"
			end if

			response.Write _
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& closeusr &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& askcasenc &"</Data></Cell>" &_
				"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& wtlapplydat &"</Data></Cell>"				
		end if

		if mergecount -1 > 0 and afirst = false then 
			mergedown = " ss:Index=""21"" "
		else
			mergedown = ""
		end if

		if len(trim(entryno)) = 0 or isnull(entryno) then
			response.Write "<Cell "& mergedown &" ss:StyleID=""toNumBrown""/>"
		else
			response.Write "<Cell "& mergedown &" ss:StyleID=""toNumBrown""><Data ss:Type=""Number"">"& entryno &"</Data></Cell>"
		end if
			
		response.Write _
			"<Cell ss:StyleID=""toCharBrown""><Data ss:Type=""String"">"& addiobound &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toCharBrown""><Data ss:Type=""String"">"& addmemo &"</Data></Cell>"

		if len(trim(adddat)) = 0 or isnull(adddat) then
			response.Write "<Cell ss:StyleID=""toDateTimeBrown""/>"
		else
			response.Write "<Cell ss:StyleID=""toDateTimeBrown""><Data ss:Type=""DateTime"">"& adddat &"</Data></Cell>"
		end if

		response.Write _
			"<Cell ss:StyleID=""toCharBrown""><Data ss:Type=""String"">"& addusr &"</Data></Cell>"

		if len(trim(sndwrkdat)) = 0 or isnull(sndwrkdat) then
			response.Write "<Cell ss:StyleID=""toDateTime""/>"
		else
			response.Write "<Cell ss:StyleID=""toDateTime""><Data ss:Type=""DateTime"">"& sndwrkdat &"</Data></Cell>"
		end if

		response.Write _
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& wrkmemo &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& assignusr &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& finishnc &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& finishtypnc &"</Data></Cell>"

		if len(trim(finishdat)) = 0 or isnull(finishdat) then
			response.Write "<Cell ss:StyleID=""toDateTime""/>"
		else
			response.Write "<Cell ss:StyleID=""toDateTime""><Data ss:Type=""DateTime"">"& finishdat &"</Data></Cell>"
		end if

		response.Write _
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& finishusr &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& uptodate &"</Data></Cell>"
			
		if scoresum = 0 then
			response.Write "<Cell ss:StyleID=""toNum""/>"
		else
			response.Write "<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& scoresum &"</Data></Cell>"
		end if

		response.Write _
			"<Cell ss:StyleID=""toCharWrap""><Data ss:Type=""String"">"& scoreitem &"</Data></Cell>" &_
			"</Row>"

		bend = false
		if rsyy.EOF then
			bend =true
		else
			if rsxx("caseno") < rsyy("caseno") then bend =true
		end if 

		cend = false
		if rszz.EOF then
			cend =true
		else
			if rsxx("caseno") < rszz("caseno") then cend =true
		end if 

		afirst = false
		if bend = true and cend = true then
			afirst = true
			rsxx.MoveNext
		end if
    Loop

    rsxx.Close
    rsyy.Close
    rszz.Close

%>
  </Table>
 </Worksheet>


</Workbook>
<%
	conn.Close
	set rsxx = nothing
	set rsyy = nothing	
	set rszz = nothing	
	set conn = nothing
%>

