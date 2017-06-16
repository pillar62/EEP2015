<%@ Language=VBScript%>
<% 
  
  key=Split(Request("key"),";")

  if key(0) ="Comn" and key(1)<>"" then		
		sql=" and c.comn like '%" &key(1)& "%' "
  elseif key(0) ="Cusnc" and key(2)<>"" then	
		sql=" and c.cusnc like '%" &key(2)& "%' "
  elseif key(0) ="Workno" then	
		sql="  "
  else
		sql=" and c.comtype ='*' "
  end if

  Dim conn, rs, sql, comn
  set conn=server.CreateObject("ADODB.Connection")
  set rs=server.CreateObject("ADODB.Recordset")
  DSN="DSN=RTLib"
  Conn.Open DSN

		'"select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		'"c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, b.t1attachtel as LINETEL, '' as gateway, " &_
		'"e.snmpip as CMTYIP, f.codenc as LINERATE, b.t1arrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, " &_
		'"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, " &_
		'"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
		'"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
		'"'' as secondcase, '' as nciccusno " &_
		'"from 	RTCust a " &_
		'"inner join RTCmty b on a.comq1 = b.comq1 " &_
		'"inner join HBAdslCmtyCust c on a.cusid = c.cusid and a.entryno = c.entryno " &_
		'"left outer join RTsnmp e on e.comq1 = b.comq1 and e.comkind ='3' " &_
		'"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		'"where 	(c.comtype ='1' or c.comtype ='4') " & sql &_ 

		'"UNION " &_

		'"select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		'"c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, b.cmtytel as LINETEL, '' as gateway, " &_
		'"b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, " &_
		'"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, " &_
		'"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
		'"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
		'"'' as secondcase, '' as nciccusno " &_
		'"from 	RTCustAdsl a " &_
		'"inner join RTCustAdslCmty b on a.comq1 = b.cutyid " &_
		'"inner join HBAdslCmtyCust c on a.cusid = c.cusid and a.entryno = c.entryno " &_
		'"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		'"where 	c.comtype ='2' " & sql &_ 

		'"UNION " &_

		'"select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		'"c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		'"b.lineip as CMTYIP, f.codenc as LINERATE, b.hinetnotifydat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
		'"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, g.codenc as CASEKIND, h.codenc as paycycle, " &_
		'"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
		'"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
		'"'' as secondcase, '' as nciccusno " &_
		'"from 	RTEbtCust a " &_
		'"inner join RTEbtCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
		'"inner join RTEbtCmtyH d on d.comq1 = b.comq1 " &_
		'"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
		'"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		'"left outer join RTCode g on g.code = a.casetype and g.kind ='H5' " &_
		'"left outer join RTCode h on h.code = a.paytype and h.kind ='G6' " &_
		'"where 	c.comtype ='5' " & sql &_ 

		'"UNION " &_

'社區搜尋
if key(0) ="Comn" then
	sql="select	c.comtype, c.comq1, c.lineq1, '' as cusid, null as entryno, " &_
		"j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, " &_
		"convert(varchar(5), b.cutyid) as comq, b.comn, b.cmtytel as LINETEL, '' as gateway, " &_
		"b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, " &_
		"''as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, '' as CASEKIND, '' as paycycle, " &_
		"'' as paytype, '' as overdue, '' as freecode, " &_
		"'' as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
		"'' as secondcase, '' as nciccusno, '' as sp499cons, null as WtlApplyDat, '' as workno, " &_
		"'' as faqman, '' as faqreason, '' as faqmemo, '' as finishmemo " &_
		"from RTSparqAdslCmty b " &_
		"inner join HBAdslCmty c on c.comq1 = b.cutyid " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
		"where 	c.comtype ='3' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, '' as cusid, null as entryno, " &_
		"j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, " &_
		"convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"replace(str(b.lineipstr1) +'.'+ str(b.lineipstr2) +'.'+ str(b.lineipstr3) +'.'+ str(b.lineipstr4) +'~'+ str(b.lineipend),' ','')  as CMTYIP, " &_
		"f.codenc as LINERATE, b.linearrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, b.idslamip, " &_
		"''as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, '' as CASEKIND, '' as paycycle, " &_
		"'' as paytype, '' as overdue, '' as freecode, " &_
		"'' as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
		"'' as secondcase, '' as nciccusno, '' as sp499cons, null as WtlApplyDat, '' as workno, " &_
		"'' as faqman, '' as faqreason, '' as faqmemo, '' as finishmemo " &_
		"from RTSparq499CmtyLine b " &_
		"inner join RTSparq499CmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmty c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1 " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
		"where 	c.comtype ='6' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, '' as cusid, null as entryno, " &_
		"j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, " &_
		"convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
		"'' as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, " &_
		"'' as CASEKIND, '' as paycycle, '' as paytype, '' as overdue, " &_
		"'' as freecode, null as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
		"'' as secondcase, '' as nciccusno, '' as sp499cons, null as WtlApplyDat, '' as workno, " &_
		"'' as faqman, '' as faqreason, '' as faqmemo, '' as finishmemo " &_
		"from RTLessorAvsCmtyLine b " &_
		"inner join RTLessorAvsCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmty c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1 " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
		"where 	c.comtype ='7' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, '' as cusid, null as entryno, " &_
		"j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, " &_
		"convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
		"'' as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, " &_
		"'' as CASEKIND, '' as paycycle, '' as paytype, '' as overdue, " &_
		"'' as freecode, null as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
		"'' as secondcase, '' as nciccusno, '' as sp499cons, null as WtlApplyDat, '' as workno, " &_
		"'' as faqman, '' as faqreason, '' as faqmemo, '' as finishmemo " &_
		"from RTLessorCmtyLine b " &_
		"inner join RTLessorCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmty c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1 " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
		"where 	c.comtype ='8' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, '' as cusid, null as entryno, " &_
		"j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, " &_
		"convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.arrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
		"'' as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, " &_
		"'' as CASEKIND, '' as paycycle, '' as paytype, '' as overdue, " &_
		"'' as freecode, null as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
		"'' as secondcase, '' as nciccusno, '' as sp499cons, null as WtlApplyDat, '' as workno, " &_
		"'' as faqman, '' as faqreason, '' as faqmemo, '' as finishmemo " &_
		"from RTPrjCmtyLine b " &_
		"inner join RTPrjCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmty c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1 " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
		"where 	c.comtype ='9' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, '' as cusid, null as entryno, " &_
		"j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, " &_
		"convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
		"'' as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, " &_
		"'' as CASEKIND, '' as paycycle, '' as paytype, '' as overdue, " &_
		"'' as freecode, null as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
		"'' as secondcase, '' as nciccusno, '' as sp499cons, null as WtlApplyDat, '' as workno, " &_
		"'' as faqman, '' as faqreason, '' as faqmemo, '' as finishmemo " &_
		"from RTSonetCmtyLine b " &_
		"inner join RTSonetCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmty c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1 " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
		"where 	c.comtype ='A' " & sql &_

		"order by c.comtype, c.comq "
'用戶搜尋		
elseif key(0) ="Cusnc" then	
	sql="select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		"c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, b.cmtytel as LINETEL, '' as gateway, " &_
		"b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, " &_
		"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, " &_
		"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
		"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
		"'' as secondcase, a.exttel +'-'+ a.sphnno as nciccusno, '' as sp499cons, null as WtlApplyDat, '' as workno, " &_
		"'' as faqman, '' as faqreason, '' as faqmemo, '' as finishmemo " &_
		"from 	RTSparqAdslCust a " &_
		"inner join RTSparqAdslCmty b on a.comq1 = b.cutyid " &_
		"inner join HBAdslCmtyCust c on a.cusid = c.cusid and a.entryno = c.entryno " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"where 	c.comtype ='3' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		"c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"replace(str(b.lineipstr1) +'.'+ str(b.lineipstr2) +'.'+ str(b.lineipstr3) +'.'+ str(b.lineipstr4) +'~'+ str(b.lineipend),' ','')  as CMTYIP, " &_
		"f.codenc as LINERATE, b.linearrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, b.idslamip, " &_
		"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, g.codenc as CASEKIND, h.codenc as paycycle, " &_
		"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
		"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
		"'' as secondcase, a.nciccusno +'-'+ a.sphnno as nciccusno, case a.consignee when '12973008' then '原遠端用戶' else '' end as Sp499cons, WtlApplyDat, '' as workno, " &_
		"'' as faqman, '' as faqreason, '' as faqmemo, '' as finishmemo " &_
		"from 	RTSparq499Cust a " &_
		"inner join RTSparq499CmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
		"inner join RTSparq499CmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode g on g.code = a.casetype and g.kind ='L9' " &_
		"left outer join RTCode h on h.code = a.paytype and h.kind ='M1' " &_
		"where 	c.comtype ='6' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		"c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
		"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
		"g.codenc as CASEKIND, h.codenc as paycycle, i.codenc as paytype, replace(a.overdue,'N','') as overdue, " &_
		"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, c.canceldat, " &_
		"replace(a.secondcase,'N','') as secondcase, '' as nciccusno, '' as sp499cons, null as WtlApplyDat, '' as workno, " &_
		"'' as faqman, '' as faqreason, '' as faqmemo, '' as finishmemo " &_
		"from 	RTLessorAvsCust a " &_
		"inner join RTLessorAvsCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
		"inner join RTLessorAvsCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode g on g.code = a.casekind and g.kind ='O9' " &_
		"left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
		"left outer join RTCode i on i.code = a.paytype and i.kind ='M9' " &_
		"where 	c.comtype ='7' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		"c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
		"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
		"g.codenc as CASEKIND, h.codenc as paycycle, i.codenc as paytype, replace(a.overdue,'N','') as overdue, " &_
		"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, c.canceldat, " &_
		"replace(a.secondcase,'N','') as secondcase, '' as nciccusno, '' as sp499cons, null as WtlApplyDat, '' as workno, " &_
		"'' as faqman, '' as faqreason, '' as faqmemo, '' as finishmemo " &_
		"from 	RTLessorCust a " &_
		"inner join RTLessorCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
		"inner join RTLessorCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode g on g.code = a.casekind and g.kind ='O9' " &_
		"left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
		"left outer join RTCode i on i.code = a.paytype and i.kind ='M9' " &_
		"where 	c.comtype ='8' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		"c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.arrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
		"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
		"'' as CASEKIND, '' as paycycle, '' as paytype, '' as overdue, " &_
		"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
		"'' as secondcase, '' as nciccusno, '' as sp499cons, null as WtlApplyDat, '' as workno, " &_
		"'' as faqman, '' as faqreason, '' as faqmemo, '' as finishmemo " &_
		"from 	RTPrjCust a " &_
		"inner join RTPrjCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
		"inner join RTPrjCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"where 	c.comtype ='9' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		"c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
		"c.cusnc, c.contacttel, c.companytel, c.raddr, a.ip11 as CUSTIP, " &_
		"'' as CASEKIND, h.codenc as paycycle, '' as paytype, convert(varchar(10), a.overduedat,111) as overdue, " &_
		"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
		"'' as secondcase, a.memberid as nciccusno, '' as sp499cons, null as WtlApplyDat, '' as workno, " &_
		"'' as faqman, '' as faqreason, '' as faqmemo, '' as finishmemo " &_
		"from RTSonetCust a " &_
		"inner join RTSonetCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
		"inner join RTSonetCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
		"where 	c.comtype ='A' " & sql &_

		"order by c.comtype, c.comq, c.cusnc "

'派工單搜尋		
elseif key(0) ="Workno" then	
	rs.Open "select emply from RTEmployee where netid ='"& session("userid") &"' ",conn
	logonusr = rs("emply")
	rs.Close

	if logonusr <>"T89039" then '測試用
		sql =sql & " and l.assigneng ='"& logonusr &"' "
	end if 
	
	sql="select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		"c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, b.cmtytel as LINETEL, '' as gateway, " &_
		"b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, " &_
		"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, " &_
		"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
		"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
		"'' as secondcase, a.exttel +'-'+ a.sphnno as nciccusno, '' as sp499cons, null as WtlApplyDat, l.workno, " &_
		"k.faqman, m.codenc as faqreason, k.memo as faqmemo, l.memo as finishmemo " &_
		"from 	RTSparqAdslCust a " &_
		"inner join RTSparqAdslCmty b on a.comq1 = b.cutyid " &_
		"inner join HBAdslCmtyCust c on a.cusid = c.cusid and a.entryno = c.entryno " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"inner join RTFaqM k on k.cusid = a.cusid and k.entryno = a.entryno " &_
		"inner join RTSndWork l on l.linkno = k.caseno and l.sndwrkdat >= dateadd(d, -7, getdate()) " &_
		"left outer join RTCode m on m.code = k.faqreason and m.kind ='P7' " &_
		"where 	c.comtype ='3' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		"c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"replace(str(b.lineipstr1) +'.'+ str(b.lineipstr2) +'.'+ str(b.lineipstr3) +'.'+ str(b.lineipstr4) +'~'+ str(b.lineipend),' ','')  as CMTYIP, " &_
		"f.codenc as LINERATE, b.linearrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, b.idslamip, " &_
		"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, g.codenc as CASEKIND, h.codenc as paycycle, " &_
		"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
		"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
		"'' as secondcase, a.nciccusno +'-'+ a.sphnno as nciccusno, case a.consignee when '12973008' then '原遠端用戶' else '' end as Sp499cons, WtlApplyDat, l.workno, " &_
		"k.faqman, m.codenc as faqreason, k.memo as faqmemo, l.memo as finishmemo " &_
		"from 	RTSparq499Cust a " &_
		"inner join RTSparq499CmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
		"inner join RTSparq499CmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode g on g.code = a.casetype and g.kind ='L9' " &_
		"left outer join RTCode h on h.code = a.paytype and h.kind ='M1' " &_
		"inner join RTFaqM k on k.comq1 = a.comq1 and k.lineq1 = a.lineq1 and k.cusid = a.cusid " &_
		"inner join RTSndWork l on l.linkno = k.caseno and l.sndwrkdat >= dateadd(d, -7, getdate()) " &_
		"left outer join RTCode m on m.code = k.faqreason and m.kind ='P7' " &_
		"where 	c.comtype ='6' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		"c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
		"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
		"g.codenc as CASEKIND, h.codenc as paycycle, i.codenc as paytype, replace(a.overdue,'N','') as overdue, " &_
		"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, c.canceldat, " &_
		"replace(a.secondcase,'N','') as secondcase, '' as nciccusno, '' as sp499cons, null as WtlApplyDat, l.workno, " &_
		"k.faqman, m.codenc as faqreason, k.memo as faqmemo, l.memo as finishmemo " &_
		"from 	RTLessorAvsCust a " &_
		"inner join RTLessorAvsCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
		"inner join RTLessorAvsCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode g on g.code = a.casekind and g.kind ='O9' " &_
		"left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
		"left outer join RTCode i on i.code = a.paytype and i.kind ='M9' " &_
		"inner join RTFaqM k on k.comq1 = a.comq1 and k.lineq1 = a.lineq1 and k.cusid = a.cusid " &_
		"inner join RTSndWork l on l.linkno = k.caseno and l.sndwrkdat >= dateadd(d, -7, getdate()) " &_
		"left outer join RTCode m on m.code = k.faqreason and m.kind ='P7' " &_
		"where 	c.comtype ='7' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		"c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
		"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
		"g.codenc as CASEKIND, h.codenc as paycycle, i.codenc as paytype, replace(a.overdue,'N','') as overdue, " &_
		"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, c.canceldat, " &_
		"replace(a.secondcase,'N','') as secondcase, '' as nciccusno, '' as sp499cons, null as WtlApplyDat, l.workno, " &_
		"k.faqman, m.codenc as faqreason, k.memo as faqmemo, l.memo as finishmemo " &_
		"from 	RTLessorCust a " &_
		"inner join RTLessorCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
		"inner join RTLessorCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode g on g.code = a.casekind and g.kind ='O9' " &_
		"left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
		"left outer join RTCode i on i.code = a.paytype and i.kind ='M9' " &_
		"inner join RTFaqM k on k.comq1 = a.comq1 and k.lineq1 = a.lineq1 and k.cusid = a.cusid " &_
		"inner join RTSndWork l on l.linkno = k.caseno and l.sndwrkdat >= dateadd(d, -7, getdate()) " &_
		"left outer join RTCode m on m.code = k.faqreason and m.kind ='P7' " &_
		"where 	c.comtype ='8' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		"c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.arrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
		"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
		"'' as CASEKIND, '' as paycycle, '' as paytype, '' as overdue, " &_
		"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
		"'' as secondcase, '' as nciccusno, '' as sp499cons, null as WtlApplyDat, l.workno, " &_
		"k.faqman, m.codenc as faqreason, k.memo as faqmemo, l.memo as finishmemo " &_
		"from 	RTPrjCust a " &_
		"inner join RTPrjCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
		"inner join RTPrjCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"inner join RTFaqM k on k.comq1 = a.comq1 and k.lineq1 = a.lineq1 and k.cusid = a.cusid " &_
		"inner join RTSndWork l on l.linkno = k.caseno and l.sndwrkdat >= dateadd(d, -7, getdate()) " &_
		"left outer join RTCode m on m.code = k.faqreason and m.kind ='P7' " &_
		"where 	c.comtype ='9' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, c.cusid, c.entryno, " &_
		"c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
		"c.cusnc, c.contacttel, c.companytel, c.raddr, a.ip11 as CUSTIP, " &_
		"'' as CASEKIND, h.codenc as paycycle, '' as paytype, convert(varchar(10), a.overduedat,111) as overdue, " &_
		"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
		"'' as secondcase, a.memberid as nciccusno, '' as sp499cons, null as WtlApplyDat, l.workno, " &_
		"k.faqman, m.codenc as faqreason, k.memo as faqmemo, l.memo as finishmemo " &_
		"from RTSonetCust a " &_
		"inner join RTSonetCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
		"inner join RTSonetCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
		"inner join RTFaqM k on k.comq1 = a.comq1 and k.lineq1 = a.lineq1 and k.cusid = a.cusid " &_
		"inner join RTSndWork l on l.linkno = k.caseno and l.sndwrkdat >= dateadd(d, -7, getdate()) " &_
		"left outer join RTCode m on m.code = k.faqreason and m.kind ='P7' " &_
		"where 	c.comtype ='A' " & sql &_

		"order by c.comtype, c.comq, c.cusnc "
end if
		
'Response.Write SQL

  rs.CursorType = 3
  rs.Open sql,conn
  s1=""
  rscnt = rs.RecordCount
  if rscnt <= 200 then
		Do While Not rs.Eof
			if rs("overdue") ="Y" then 
				dropflag ="[欠拆]"
			else
				if len(rs("dropdat")) >0 then 
					dropflag ="[退租]"
				else 
					dropflag =""
				end if
			end if

			s1=s1 &"<option value=""" & _
				rs("comtype")	&";"& rs("comq1")		&";"& rs("lineq1")			&";"& rs("cusid")			&";"& _
				rs("entryno")	&";"& rs("comtypenc")	&";"& rs("belongnc")		&";"& rs("salesnc")			&";"& _
				rs("comq")		&";"& rs("comn")		&";"& rs("linetel")			&";"& rs("cmtyip")			&";"& _
				rs("linerate")	&";"& rs("arrivedat")	&";"& rs("rcomdrop")		&";"& rs("cusnc")			&";"& _
				rs("contacttel")&";"& rs("companytel")	&";"& rs("raddr")			&";"& rs("custip")			&";"& _
				rs("casekind")	&";"& rs("paycycle")	&";"& rs("paytype")			&";"& rs("overdue")			&";"& _
				rs("freecode")	&";"& rs("docketdat")	&";"& rs("strbillingdat")	&";"& rs("newbillingdat")	&";"& _
				rs("duedat")	&";"& rs("dropdat")		&";"& rs("canceldat")		&";"& rs("secondcase")		&";"& _
				rs("idslamip")	&";"& rs("gateway")		&";"& rs("nciccusno")		&";"& rs("sp499cons")		&";"& _
				rs("wtlapplydat") &";"& rs("workno")	&";"& rs("faqman")			&";"& rs("faqreason")		&";"& _
				rs("faqmemo")		&";"& rs("finishmemo")		&";"& _
				""">"& dropflag & rs("comtypenc") &"["& rs("cusnc") &"]-["&rs("COMQ")&"]" &rs("COMN") &" [" & rs("raddr") & "]</option>"
			rs.MoveNext
		Loop
  else
  		s1=s1 &"<option>搜尋結果共: "&rscnt&" 筆資料，請縮小搜尋範圍!!</option>"
  end if 
  rs.Close    

  conn.Close   
  set rs=Nothing   
  set conn=Nothing
%>
<HTML>
<HEAD>
	<meta http-equiv="Content-Type" content="text/html; charset=Big5">
	<TITLE>社區&客戶選擇清單</TITLE>
</HEAD>
<BODY style="BACKGROUND: lightblue">
<SCRIPT LANGUAGE="VBScript">
  Sub lstOrder1_onclick()
      selno=lstorder1.selectedIndex
      if selno >=0 then
         window.document.all("cmdtext").value= lstOrder1(selno).innerHTML
         window.document.all("cmdtext1").value=lstOrder1(selno).value
         window.document.all("cmdtext2").value="Y"         
      end if
  End Sub

  Sub cmdSure_onClick()
    ReturnValue=""
    'if len(trim(window.document.all("cmdtext").value)) = 0 then
    '   msgbox "請選擇鄉鎮市區!",vbokonly,"錯誤訊息視窗"
    'else    
       'returnvalue= window.document.all("cmdtext2").value &";"& window.document.all("cmdtext1").value &";"& window.document.all("cmdtext").value 
       returnvalue= window.document.all("cmdtext2").value &";"& window.document.all("cmdtext1").value
       window.close
    'end if
  End Sub

  Sub cmdCancel_onClick()
      returnvalue=""
      window.close
  End Sub

  Sub lstOrder1_onkeypress()
		'enter
  		if window.event.keycode =13 then 
			lstOrder1_onclick
			cmdSure_onClick()
		'ESC			
  		elseif window.event.keycode =27 then 
			cmdCancel_onClick
		end if
  End Sub
</SCRIPT>
<Fieldset STYLE="HEIGHT: 390px; LEFT: 16px; POSITION: absolute; TOP: 45px; WIDTH: 600px" ID="select0">
	<LEGEND>社區選擇清單</LEGEND> 
	
	<FIELDSET STYLE="HEIGHT: 308px; LEFT: 16px; POSITION: absolute; TOP: 20px; WIDTH: 570px" ID="select1">
		<LEGEND>社區名稱</LEGEND>
		<SELECT style="font-family:細明體;HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 25px; WIDTH: 550px" id="lstOrder1" size="5">
				<%=s1%>
		</SELECT>
	</FIELDSET>&nbsp;
</Fieldset>&nbsp; <font style="LEFT: 30px; POSITION: absolute; TOP: 380px">目前選擇內容 </font>
		<INPUT id="cmdtext" style="LEFT: 130px; POSITION: absolute; TOP: 380px; " size="58" type="text" readonly>
		<INPUT id="cmdtext1" style="LEFT: 130px; POSITION: absolute; TOP: 380px;display:none" size="30" type="text" readonly>
		<INPUT id="cmdtext2" style="LEFT: 130px; POSITION: absolute; TOP: 380px; display:none" size="30" type="text" readonly>
		<INPUT id="cmdCancel" style="LEFT: 490px; POSITION: absolute; TOP: 405px" type="button" value="取消">
		<INPUT id="cmdSure" style="LEFT: 436px; POSITION: absolute; TOP: 405px" type="button" value="確定">
	</BODY>
</HTML>
