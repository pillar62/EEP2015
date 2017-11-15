/*
select a.caseno, a.comtype, case when p.consignee ='12973008' then '原遠端戶' else n.groupnc end AS ANGENCY, n.leader
, b.codenc, convert(varchar(5), n.comq1)+case when n.lineq1=0 then '' else '-'+convert(varchar(5), n.lineq1) end COMLINE, n.comn, 
c.dropdat, a.faqman, j.codenc, d.codenc, isnull(f.cusnc,'') AS CUSNC, left(convert(varchar(25),a.rcvdat, 20), 16) AS RCVDATE
, a.closedat, o.codenc, count(g.caseno) AS QT_CASE, isnull(k.shortnc,i.name) AS SNAME, 
case when l.finishnum >0 then '' else 'Y' end as finishnum , n.comq1, n.lineq1, c.cusid, c.entryno, a.tel, a.MOBILE from RTFaqM a left outer join RTCode b on a.comtype = b.code and b.kind ='P5' left outer join HBAdslCmtyCust c on a.comtype = c.comtype and a.comq1 = c.comq1 and a.lineq1 = c.lineq1 and a.cusid = c.cusid and a.entryno = c.entryno 
left outer join HBAdslCmty n on a.comtype = n.comtype and a.comq1 = n.comq1 and a.lineq1 = n.lineq1 left outer join RTCode o on a.custsrc = o.code and o.kind ='Q3' 
left outer join RTCode d on a.faqreason = d.code and d.kind ='P7' left outer join RTEmployee e inner join RTObj f on f.cusid = e.cusid on e.emply = a.rcvusr 
left outer join RTFaqAdd g on g.caseno = a.caseno and g.canceldat is null left outer join RTCode j on a.iobound = j.code and j.kind ='P8' 
left outer join RTSndWork h inner join (select max(workno) as maxworkno, linkno from RTSndWork where canceldat is null 			and (worktype ='01' or worktype ='09') group by linkno) m on m.linkno = h.linkno and m.maxworkno = h.workno 
on h.linkno = a.caseno  left outer join RTEmployee i on i.emply = h.assigneng left outer join RTObj k on k.cusid = h.assigncons left outer join (select linkno, count(*) as finishnum from RTSndwork 
 where finishdat is null and canceldat is null and (worktype ='01' or worktype ='09') 
 group by linkno) l on a.caseno = l.linkno left outer join RTSparq499Cust p on p.comq1 = c.comq1 and p.lineq1 = c.lineq1 and p.cusid = c.cusid and c.comtype ='6' 
where a.caseno >'' 
 group by a.caseno, a.comtype, case when p.consignee ='12973008' then '原遠端戶' else n.groupnc end, n.leader, b.codenc, convert(varchar(5), n.comq1)+case when n.lineq1=0 then '' else '-'+convert(varchar(5), n.lineq1) end, n.comn, 
a.faqman, c.dropdat, j.codenc, d.codenc, isnull(f.cusnc,''), left(convert(varchar(25), a.rcvdat, 20), 16), a.closedat, o.codenc, isnull(k.shortnc,i.name), 
case when l.finishnum >0 then '' else 'Y' end , n.comq1, n.lineq1, c.cusid, c.entryno, a.tel, a.MOBILE order by left(convert(varchar(25), a.rcvdat, 20), 16) desc 
*/

CREATE VIEW V_RT2051 AS 
SELECT * FROM HBAdslCmty
UNION ALL
SELECT A.COMQ1, B.LINEQ1, A.COMN, A.RADDR, A.COMCNT, C.QQ AS USERCNT, A.COMTYPE, B.ADSLAPPLYDAT, '' as COMSOURCE
, D.GROUPNC, '' AS LEADER, '' AS COMAGREE, CONTACT, '' AS SIGNEDAT, 0 AS SIGNIFICANTCNT, E.AREANC, DROPDAT, A.CONTACT, A.CONTACTTEL
, B.LINEIP, B.LINETEL, B.CANCELDAT
FROM RTLessorAVSCmtyLine B
LEFT JOIN RTLessorAVSCmtyH A ON A.COMQ1=B.COMQ1
LEFT JOIN (SELECT COMQ1, COUNT(*) AS QQ FROM RTLessorAVSCust GROUP BY COMQ1) C ON C.COMQ1=A.COMQ1
LEFT JOIN RTSalesGroup D ON D.GROUPID=B.GROUPID
LEFT JOIN RTArea E ON E.AREAID=B.AREAID

SELECT * FROM RTLessorAVSCmtyH
SELECT * FROM RTLessorAVSCmtyLine
SELECT * FROM RTArea
SELECT * FROM RTLessorAVSCust