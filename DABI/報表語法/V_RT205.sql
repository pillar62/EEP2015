DROP VIEW V_RT205;

CREATE VIEW V_RT205 AS 
select	c.comtype, A.COMQ1, '' AS LINEQ1, A.CUSID,c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, b.t1attachtel as LINETEL, '' as gateway, 
e.snmpip as CMTYIP, f.codenc as LINERATE, b.t1arrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, 
c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, 
'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, 
c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, 
'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat 
from 	RTCust a 
inner join RTCmty b on a.comq1 = b.comq1 
left outer join HBAdslCmtyCust c on a.cusid = c.cusid and a.entryno = c.entryno 
left outer join RTsnmp e on e.comq1 = b.comq1 and e.comkind ='3' 
left outer join RTCode f on f.code = b.linerate and f.kind ='D3' 
where (c.comtype ='1' or c.comtype ='4') 
UNION ALL
select c.comtype, A.COMQ1, '' AS LINEQ1, A.CUSID,c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, b.cmtytel as LINETEL, '' as gateway, 
b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, 
c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, 
'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, 
c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, 
'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat 
from 	RTCustAdsl a 
inner join RTCustAdslCmty b on a.comq1 = b.cutyid 
left outer join HBAdslCmtyCust c on a.cusid = c.cusid and a.entryno = c.entryno 
left outer join RTCode f on f.code = b.linerate and f.kind ='D3' 
where 	c.comtype ='2' 
UNION ALL
select c.comtype, A.COMQ1, '' AS LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, b.cmtytel as LINETEL, '' as gateway, 
b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, 
c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, 
'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, 
c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, 
'' as secondcase, a.exttel +'-'+ a.sphnno as nciccusno, '' as Sp499cons, null as WtlApplyDat 
from 	RTSparqAdslCust a 
inner join RTSparqAdslCmty b on a.comq1 = b.cutyid 
left outer join HBAdslCmtyCust c on a.cusid = c.cusid and a.entryno = c.entryno 
left outer join RTCode f on f.code = b.linerate and f.kind ='D3' 
where 	c.comtype ='3' 
UNION ALL
select c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, 
b.lineip as CMTYIP, f.codenc as LINERATE, b.hinetnotifydat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, 
c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, g.codenc as CASEKIND, h.codenc as paycycle, 
'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, 
c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, 
'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat 
from 	RTEbtCust a 
inner join RTEbtCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 
inner join RTEbtCmtyH d on d.comq1 = b.comq1 
left outer join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid 
left outer join RTCode f on f.code = b.linerate and f.kind ='D3' 
left outer join RTCode g on g.code = a.casetype and g.kind ='H5' 
left outer join RTCode h on h.code = a.paytype and h.kind ='G6' 
where 	c.comtype ='5' 
UNION ALL
select c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, 
replace(str(b.lineipstr1) +'.'+ str(b.lineipstr2) +'.'+ str(b.lineipstr3) +'.'+ str(b.lineipstr4) +'~'+ str(b.lineipend),' ','')  as CMTYIP, 
f.codenc as LINERATE, b.linearrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, b.idslamip, 
c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.custip1+'.'+ a.custip2 +'.'+ a.custip3 +'.'+ a.custip4, '...', '') as CUSTIP, 
g.codenc as CASEKIND, h.codenc as paycycle, 
'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, 
c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, 
'' as secondcase, a.nciccusno +'-'+ a.sphnno as nciccusno, case a.consignee when '12973008' then '­ì»·ºÝ¥Î¤á' else '' end as Sp499cons, a.WtlApplyDat 
from 	RTSparq499Cust a 
inner join RTSparq499CmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 
inner join RTSparq499CmtyH d on d.comq1 = b.comq1 
left outer join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid 
left outer join RTCode f on f.code = b.linerate and f.kind ='D3' 
left outer join RTCode g on g.code = a.casetype and g.kind ='L9' 
left outer join RTCode h on h.code = a.paytype and h.kind ='M1' 
where 	c.comtype ='6' 
UNION ALL
select	c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, 
b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, 
c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, 
g.codenc as CASEKIND, h.codenc as paycycle, i.codenc as paytype, replace(a.overdue,'N','') as overdue, 
replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, c.canceldat, 
replace(a.secondcase,'N','') as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat 
from 	RTLessorAvsCust a 
inner join RTLessorAvsCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 
inner join RTLessorAvsCmtyH d on d.comq1 = b.comq1 
left outer join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid 
left outer join RTCode f on f.code = b.linerate and f.kind ='D3' 
left outer join RTCode g on g.code = a.casekind and g.kind ='O9' 
left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' 
left outer join RTCode i on i.code = a.paytype and i.kind ='M9' 
where 	c.comtype ='7' 
UNION ALL
select	c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, 
b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, 
c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, 
g.codenc as CASEKIND, h.codenc as paycycle, i.codenc as paytype, replace(a.overdue,'N','') as overdue, 
replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, c.canceldat, 
replace(a.secondcase,'N','') as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat 
from 	RTLessorCust a 
inner join RTLessorCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 
inner join RTLessorCmtyH d on d.comq1 = b.comq1 
left outer join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid 
left outer join RTCode f on f.code = b.linerate and f.kind ='D3' 
left outer join RTCode g on g.code = a.casekind and g.kind ='O9' 
left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' 
left outer join RTCode i on i.code = a.paytype and i.kind ='M9' 
where 	c.comtype ='8' 
UNION ALL
select	c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, 
b.lineip  as CMTYIP, f.codenc as LINERATE, b.arrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, 
c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, 
'' as CASEKIND, '' as paycycle, '' as paytype, '' as overdue, 
replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, 
'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat 
from 	RTPrjCust a 
inner join RTPrjCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 
inner join RTPrjCmtyH d on d.comq1 = b.comq1 
left outer join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid 
left outer join RTCode f on f.code = b.linerate and f.kind ='D3' 
where 	c.comtype ='9' 
UNION ALL
select	c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, 
b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, 
c.cusnc, c.contacttel, c.companytel, c.raddr, a.ip11 as CUSTIP, 
'' as CASEKIND, h.codenc as paycycle, '' as paytype, convert(varchar(10), a.overduedat,111) as overdue, 
replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, 
'' as secondcase, a.memberid as nciccusno, '' as Sp499cons, null as WtlApplyDat 
from 	RTSonetCust a 
inner join RTSonetCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 
inner join RTSonetCmtyH d on d.comq1 = b.comq1 
left outer join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid 
left outer join RTCode f on f.code = b.linerate and f.kind ='D3' 
left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' 
where 	c.comtype ='A' 
UNION ALL
select	c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, 
b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, 
c.cusnc, c.contacttel, c.companytel, c.raddr, a.ip11 as CUSTIP, 
'' as CASEKIND, h.codenc as paycycle, '' as paytype, convert(varchar(10), a.overduedat,111) as overdue, 
replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, 
'' as secondcase, a.memberid as nciccusno, '' as Sp499cons, null as WtlApplyDat 
from 	RTfareastCust a 
inner join RTfareastCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 
inner join RTfareastCmtyH d on d.comq1 = b.comq1 
left outer join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid 
left outer join RTCode f on f.code = b.linerate and f.kind ='D3' 
left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' 
where 	c.comtype ='B' 
