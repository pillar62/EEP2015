IF Exists (SELECT * FROM tempdb..sysobjects WHERE ID =Object_id('tempdb..#temRcvRPT')) 
    DROP TABLE #temRcvRPT
CREATE TABLE #temRcvRPT(
	CODENC varchar(200), paytype varchar(200), belongnc varchar(20),	qty int,		unino varchar(10),	invtitle  varchar(50), 
	comn varchar(150),	cusnc varchar(250),	raddr varchar(250),	tel varchar(130),
	docketdat datetime,	rcvmoneydat datetime,	strbillingdat datetime,	duedat datetime,	
	memo varchar(130),	paytype2 varchar(12),	casetype  varchar(100),
	rcvnc  varchar(130),	rcvmoney  int,	setnc varchar(130),	setmoney  int,
	gtnc  varchar(130),	gtmoney  int, 	returnnc varchar(130),	returnmoney  int,
	gtserial varchar(115)
)

insert into #temRcvRPT
SELECT B.CODENC, A.*
FROM
	(	select '02' as paytype, isnull(e.shortnc, isnull(g.cusnc, '')) AS belongnc, 1 AS qty, '' AS unino, '' AS invtitle
	, d.comn, b.cusnc, isnull(h.cutnc,'')+ b.township2+b.raddr2 AS raddr,
			b.contacttel, b.docketdat, b.docketdat AS rcvmoneydat, b.strbillingdat, null AS duedat, 
			'專案社區' AS memo, '' AS paytype2, '專案社區' AS casetype,
			 '' AS rcvnc, 0 AS rcvmoney, '' AS setnc, 0 AS setmoney, '保證金' AS gtnc, b.gtmoney, '' AS returnnc, 0 AS returnmoney, '' AS gtserial
		from	RTPrjCust b 
			inner join RTPrjCmtyLine c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1
			inner join RTPrjCmtyH d on d.comq1 = c.comq1
			left outer join RTObj e on e.cusid = d.consignee
			left outer join RTEmployee f inner join RTObj g on g.cusid = f.cusid on f.emply = d.salesid
			left outer join RTCounty h on  h.cutid  = b.cutid2
		where	b.canceldat is null and b.dropdat is null
			-- 專案社區維修收款
	union all
	select  a.paytype,	isnull(e.shortnc, isnull(g.cusnc, '')), 1, '', '', comn, b.cusnc, isnull(h.cutnc,'')+ b.township2+b.raddr2,
		b.contacttel, b.docketdat, a.rcvmoneydat, null, null,
		'專案社區維修', '', '專案社區',
		 '移機費', a.moveamt, '設定費', a.setamt, '設備費'+isnull(i.codenc,''), a.equipamt, '', 0,''
	from	RTPrjCustRepair a
		inner join RTPrjCust b on a.cusid = b.cusid
		inner join RTPrjCmtyLine c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1
		inner join RTPrjCmtyH d on d.comq1 = c.comq1
		left outer join RTObj e on e.cusid = d.consignee
		left outer join RTEmployee f inner join RTObj g on g.cusid = f.cusid on f.emply = d.salesid
		left outer join RTCounty h on  h.cutid  = b.cutid2
		left outer join RTCode i on i.code = a.equip and i.kind ='P2'
	where	b.canceldat is null and a.canceldat is null
	and	a.finishdat is not null

	-- ET申裝
	union all
	select  b.paytype,	isnull(e.shortnc, isnull(g.cusnc, '')), 1, '', '', comn, b.cusnc, isnull(h.cutnc,'')+ b.township3+b.raddr3,
		case  b.mobile  when ''  then  b.contacttel else b.mobile end, b.docketdat,  a.rcvmoneydat, b.strbillingdat, b.duedat, 
		'ET新申裝', b.paytype, 'ET',
		 '網路費', i.amt, '安裝費', b.setmoney, '保證金', b.gtmoney, '', 0,''
	from	RTLessorCustSndwork a
		inner join RTLessorCust b on a.cusid = b.cusid
		inner join RTLessorCmtyLine c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1
		inner join RTLessorCmtyH d on d.comq1 = c.comq1
		inner join RTLessorCustAR i on i.batchno = a.batchno
		left outer join RTObj e on e.cusid = c.consignee
		left outer join RTEmployee f inner join RTObj g on g.cusid = f.cusid on f.emply = c.salesid
		left outer join RTCounty h on  h.cutid  = b.cutid3
	where	b.canceldat is null and a.dropdat is null
	-- ET復機
	union all
	select  a.paytype,	isnull(e.shortnc, isnull(g.cusnc, '')), 1, '', '', comn, b.cusnc, isnull(h.cutnc,'')+ b.township3+b.raddr3,
		case  b.mobile  when ''  then  b.contacttel else b.mobile end, b.docketdat, a.rcvmoneydat, a.strbillingdat, b.duedat, 
		'ET復機', a.paytype, 'ET',
		 '網路費', a.amt, '復機費', a.returnmoney, '', 0, '', 0,''
	from	RTLessorCustReturn a
		inner join RTLessorCust b on a.cusid = b.cusid
		inner join RTLessorCmtyLine c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1
		inner join RTLessorCmtyH d on d.comq1 = c.comq1
		left outer join RTObj e on e.cusid = c.consignee
		left outer join RTEmployee f inner join RTObj g on g.cusid = f.cusid on f.emply = c.salesid
		left outer join RTCounty h on  h.cutid  = b.cutid3
	where	b.canceldat is null and a.canceldat is null
	-- ET續約
	union all
	select  a.paytype,	isnull(e.shortnc, isnull(g.cusnc, '')), 1, '', '', comn, b.cusnc, isnull(h.cutnc,'')+ b.township3+b.raddr3,
		case  b.mobile  when ''  then  b.contacttel else b.mobile end, b.docketdat, a.rcvmoneydat, a.strbillingdat, b.duedat, 
		'ET續約', a.paytype, 'ET',
		 '網路費', a.amt, '', 0, '', 0, '', 0,''
	from	RTLessorCustCont a
		inner join RTLessorCust b on a.cusid = b.cusid
		inner join RTLessorCmtyLine c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1
		inner join RTLessorCmtyH d on d.comq1 = c.comq1
		left outer join RTObj e on e.cusid = c.consignee
		left outer join RTEmployee f inner join RTObj g on g.cusid = f.cusid on f.emply = c.salesid
		left outer join RTCounty h on  h.cutid  = b.cutid3
	where	b.canceldat is null and a.canceldat is null
	-- ET維修收款
	union all
	select  a.paytype,	isnull(e.shortnc, isnull(g.cusnc, '')), 1, '', '', comn, b.cusnc, isnull(h.cutnc,'')+ b.township3+b.raddr3,
		case  b.mobile  when ''  then  b.contacttel else b.mobile end, b.docketdat, a.rcvmoneydat, null, null,
		'ET維修', b.paytype, 'ET',
		 '移機費', a.moveamt, '設定費', a.setamt, '設備費'+isnull(i.codenc,''), a.equipamt, '', 0,''
	from	RTLessorCustRepair a
		inner join RTLessorCust b on a.cusid = b.cusid
		inner join RTLessorCmtyLine c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1
		inner join RTLessorCmtyH d on d.comq1 = c.comq1
		left outer join RTObj e on e.cusid = c.consignee
		left outer join RTEmployee f inner join RTObj g on g.cusid = f.cusid on f.emply = c.salesid
		left outer join RTCounty h on  h.cutid  = b.cutid3
		left outer join RTCode i on i.code = a.equip and i.kind ='P2'
	where	b.canceldat is null and a.canceldat is null
	-- AVS申裝
	union all
	select  b.paytype,	isnull(e.shortnc, isnull(g.cusnc, '')), 1, '', '', comn, b.cusnc, isnull(h.cutnc,'')+ b.township3+b.raddr3,
		case  b.mobile  when ''  then  b.contacttel else b.mobile end, b.docketdat, a.rcvmoneydat, b.strbillingdat, b.duedat, 
		'AVS新申裝', b.paytype, 'AVS',
		 '網路費', i.amt, '安裝費', b.setmoney, '保證金', b.gtmoney, '', 0,''
	from	RTLessorAVSCustSndwork a
		inner join RTLessorAVSCust b on a.cusid = b.cusid
		inner join RTLessorAVSCmtyLine c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1
		inner join RTLessorAVSCmtyH d on d.comq1 = c.comq1
		inner join RTLessorAVSCustAR i on i.batchno = a.batchno
		left outer join RTObj e on e.cusid = c.consignee
		left outer join RTEmployee f inner join RTObj g on g.cusid = f.cusid on f.emply = c.salesid
		left outer join RTCounty h on  h.cutid  = b.cutid3
	where	b.canceldat is null and a.dropdat is null
	-- AVS續約
	union all
	select  a.paytype,	isnull(e.shortnc, isnull(g.cusnc, '')), 1, '', '', comn, b.cusnc, isnull(h.cutnc,'')+ b.township3+b.raddr3,
		case  b.mobile  when ''  then  b.contacttel else b.mobile end, b.docketdat, a.rcvmoneydat, a.strbillingdat, b.duedat, 
		'AVS續約', a.paytype, 'AVS',
		 '網路費', a.amt, '', 0, '', 0, '', 0,''
	from	RTLessorAVSCustCont a
		inner join RTLessorAVSCust b on a.cusid = b.cusid
		inner join RTLessorAVSCmtyLine c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1
		inner join RTLessorAVSCmtyH d on d.comq1 = c.comq1
		left outer join RTObj e on e.cusid = c.consignee
		left outer join RTEmployee f inner join RTObj g on g.cusid = f.cusid on f.emply = c.salesid
		left outer join RTCounty h on  h.cutid  = b.cutid3
	where	b.canceldat is null and a.canceldat is null
	-- AVS復機
	union all
	select  a.paytype,	isnull(e.shortnc, isnull(g.cusnc, '')), 1, '', '', comn, b.cusnc, isnull(h.cutnc,'')+ b.township3+b.raddr3,
		case  b.mobile  when ''  then  b.contacttel else b.mobile end, b.docketdat, a.rcvmoneydat, a.strbillingdat, b.duedat, 
		'AVS復機', a.paytype, 'AVS',
		 '網路費', a.amt, '復機費', a.returnmoney, '', 0, '', 0,''
	from	RTLessorAVSCustReturn a
		inner join RTLessorAVSCust b on a.cusid = b.cusid
		inner join RTLessorAVSCmtyLine c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1
		inner join RTLessorAVSCmtyH d on d.comq1 = c.comq1
		left outer join RTObj e on e.cusid = c.consignee
		left outer join RTEmployee f inner join RTObj g on g.cusid = f.cusid on f.emply = c.salesid
		left outer join RTCounty h on  h.cutid  = b.cutid3
	where	b.canceldat is null and a.canceldat is null
	-- AVS維修收款
	union all
	select  a.paytype,	isnull(e.shortnc, isnull(g.cusnc, '')), 1, '', '', comn, b.cusnc, isnull(h.cutnc,'')+ b.township3+b.raddr3,
		case  b.mobile  when ''  then  b.contacttel else b.mobile end, b.docketdat, a.rcvmoneydat, null, null,
		'AVS維修', b.paytype, 'AVS',
		 '移機費', a.moveamt, '設定費', a.setamt, '設備費'+isnull(i.codenc,''), a.equipamt, '', 0,''
	from	RTLessorAVSCustRepair a
		inner join RTLessorAVSCust b on a.cusid = b.cusid
		inner join RTLessorAVSCmtyLine c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1
		inner join RTLessorAVSCmtyH d on d.comq1 = c.comq1
		left outer join RTObj e on e.cusid = c.consignee
		left outer join RTEmployee f inner join RTObj g on g.cusid = f.cusid on f.emply = c.salesid
		left outer join RTCounty h on  h.cutid  = b.cutid3
		left outer join RTCode i on i.code = a.equip and i.kind ='P2'
	where	b.canceldat is null and a.canceldat is null
	and	a.finishdat is not null
) A
LEFT JOIN rtcode B ON B.kind ='M9' AND B.CODE = A.paytype
ORDER BY 1


select	belongnc, rcvnc as amtnc, qty, rcvmoney as amt, unino, invtitle, comn,  cusnc,  raddr, tel, 
	docketdat, rcvmoneydat, strbillingdat, duedat, memo, gtserial
from 	#temRcvRPT
where 	rcvmoney >0
UNION
select	belongnc, setnc as amtnc, qty, setmoney as amt, unino, invtitle, comn,  cusnc,  raddr, tel, 
	docketdat, rcvmoneydat, strbillingdat, duedat, memo, gtserial
from 	#temRcvRPT
where 	setmoney >0
UNION
select	belongnc, gtnc as amtnc, qty, gtmoney as amt, unino, invtitle, comn,  cusnc,  raddr, tel, 
	docketdat, rcvmoneydat, strbillingdat, duedat, memo, gtserial
from 	#temRcvRPT
where 	gtmoney >0
UNION
select	belongnc, returnnc as amtnc, qty, returnmoney as amt, unino, invtitle, comn,  cusnc,  raddr, tel, 
	docketdat, rcvmoneydat, strbillingdat, duedat, memo, gtserial
from 	#temRcvRPT
where 	  	returnmoney >0
order  by  	rcvmoneydat, belongnc, comn, cusnc, amtnc