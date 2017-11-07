drop view V_RT205

create view V_RT205 as 
SELECT          c.comtype, A.COMQ1, '' AS LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, 
                            b.t1attachtel AS LINETEL, '' AS gateway, e.snmpip AS CMTYIP, f.codenc AS LINERATE, b.t1arrive AS ARRIVEDAT, 
                            b.rcomdrop, '' AS idslamip, c.cusnc, c.contacttel, c.companytel, c.raddr, '' AS CUSTIP, a.usekind AS CASEKIND, 
                            '' AS paycycle, '' AS paytype, replace(a.overdue, 'N', '') AS overdue, replace(a.freecode, 'N', '') AS freecode, 
                            c.docketdat, NULL AS strbillingdat, NULL AS newbillingdat, NULL AS duedat, c.dropdat, c.canceldat, '' AS secondcase, 
                            '' AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat
FROM              RTCust a INNER JOIN
                            RTCmty b ON a.comq1 = b.comq1 LEFT OUTER JOIN
                            HBAdslCmtyCust c ON a.cusid = c.cusid AND a.entryno = c.entryno LEFT OUTER JOIN
                            RTsnmp e ON e.comq1 = b.comq1 AND e.comkind = '3' LEFT OUTER JOIN
                            RTCode f ON f.code = b.linerate AND f.kind = 'D3'
WHERE          (c.comtype = '1' OR
                            c.comtype = '4')
UNION ALL
SELECT          c.comtype, A.COMQ1, '' AS LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, 
                            b.cmtytel AS LINETEL, '' AS gateway, b.ipaddr AS CMTYIP, f.codenc AS LINERATE, b.linearrive AS ARRIVEDAT, 
                            b.rcomdrop, '' AS idslamip, c.cusnc, c.contacttel, c.companytel, c.raddr, '' AS CUSTIP, a.usekind AS CASEKIND, 
                            '' AS paycycle, '' AS paytype, replace(a.overdue, 'N', '') AS overdue, replace(a.freecode, 'N', '') AS freecode, 
                            c.docketdat, NULL AS strbillingdat, NULL AS newbillingdat, NULL AS duedat, c.dropdat, c.canceldat, '' AS secondcase, 
                            '' AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat
FROM              RTCustAdsl a INNER JOIN
                            RTCustAdslCmty b ON a.comq1 = b.cutyid LEFT OUTER JOIN
                            HBAdslCmtyCust c ON a.cusid = c.cusid AND a.entryno = c.entryno LEFT OUTER JOIN
                            RTCode f ON f.code = b.linerate AND f.kind = 'D3'
WHERE          c.comtype = '2'
UNION ALL
SELECT          c.comtype, A.COMQ1, '' AS LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, 
                            b.cmtytel AS LINETEL, '' AS gateway, b.ipaddr AS CMTYIP, f.codenc AS LINERATE, b.linearrive AS ARRIVEDAT, 
                            b.rcomdrop, '' AS idslamip, c.cusnc, c.contacttel, c.companytel, c.raddr, '' AS CUSTIP, a.usekind AS CASEKIND, 
                            '' AS paycycle, '' AS paytype, replace(a.overdue, 'N', '') AS overdue, replace(a.freecode, 'N', '') AS freecode, 
                            c.docketdat, NULL AS strbillingdat, NULL AS newbillingdat, NULL AS duedat, c.dropdat, c.canceldat, '' AS secondcase, 
                            a.exttel + '-' + a.sphnno AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat
FROM              RTSparqAdslCust a INNER JOIN
                            RTSparqAdslCmty b ON a.comq1 = b.cutyid LEFT OUTER JOIN
                            HBAdslCmtyCust c ON a.cusid = c.cusid AND a.entryno = c.entryno LEFT OUTER JOIN
                            RTCode f ON f.code = b.linerate AND f.kind = 'D3'
WHERE          c.comtype = '3'
UNION ALL
SELECT          c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d .comn, b.linetel AS LINETEL, 
                            b.gateway, b.lineip AS CMTYIP, f.codenc AS LINERATE, b.hinetnotifydat AS ARRIVEDAT, b.dropdat AS RCOMDROP, 
                            '' AS idslamip, c.cusnc, c.contacttel, c.companytel, c.raddr, '' AS CUSTIP, g.codenc AS CASEKIND, 
                            h.codenc AS paycycle, '' AS paytype, replace(a.overdue, 'N', '') AS overdue, replace(a.freecode, 'N', '') AS freecode, 
                            c.docketdat, NULL AS strbillingdat, NULL AS newbillingdat, NULL AS duedat, c.dropdat, c.canceldat, '' AS secondcase, 
                            '' AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat
FROM              RTEbtCust a INNER JOIN
                            RTEbtCmtyLine b ON a.comq1 = b.comq1 AND a.lineq1 = b.lineq1 INNER JOIN
                            RTEbtCmtyH d ON d .comq1 = b.comq1 LEFT OUTER JOIN
                            HBAdslCmtyCust c ON c.comq1 = a.comq1 AND c.lineq1 = a.lineq1 AND a.cusid = c.cusid LEFT OUTER JOIN
                            RTCode f ON f.code = b.linerate AND f.kind = 'D3' LEFT OUTER JOIN
                            RTCode g ON g.code = a.casetype AND g.kind = 'H5' LEFT OUTER JOIN
                            RTCode h ON h.code = a.paytype AND h.kind = 'G6'
WHERE          c.comtype = '5'
UNION ALL
SELECT          c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d .comn, b.linetel AS LINETEL, 
                            b.gateway, replace(str(b.lineipstr1) + '.' + str(b.lineipstr2) + '.' + str(b.lineipstr3) + '.' + str(b.lineipstr4) 
                            + '~' + str(b.lineipend), ' ', '') AS CMTYIP, f.codenc AS LINERATE, b.linearrivedat AS ARRIVEDAT, 
                            b.dropdat AS RCOMDROP, b.idslamip, c.cusnc, c.contacttel, c.companytel, c.raddr, 
                            replace(a.custip1 + '.' + a.custip2 + '.' + a.custip3 + '.' + a.custip4, '...', '') AS CUSTIP, g.codenc AS CASEKIND, 
                            h.codenc AS paycycle, '' AS paytype, replace(a.overdue, 'N', '') AS overdue, replace(a.freecode, 'N', '') AS freecode, 
                            c.docketdat, NULL AS strbillingdat, NULL AS newbillingdat, NULL AS duedat, c.dropdat, c.canceldat, '' AS secondcase, 
                            a.nciccusno + '-' + a.sphnno AS nciccusno, 
                            CASE a.consignee WHEN '12973008' THEN '­ì»·ºÝ¥Î¤á' ELSE '' END AS Sp499cons, a.WtlApplyDat
FROM              RTSparq499Cust a INNER JOIN
                            RTSparq499CmtyLine b ON a.comq1 = b.comq1 AND a.lineq1 = b.lineq1 INNER JOIN
                            RTSparq499CmtyH d ON d .comq1 = b.comq1 LEFT OUTER JOIN
                            HBAdslCmtyCust c ON c.comq1 = a.comq1 AND c.lineq1 = a.lineq1 AND a.cusid = c.cusid LEFT OUTER JOIN
                            RTCode f ON f.code = b.linerate AND f.kind = 'D3' LEFT OUTER JOIN
                            RTCode g ON g.code = a.casetype AND g.kind = 'L9' LEFT OUTER JOIN
                            RTCode h ON h.code = a.paytype AND h.kind = 'M1'
WHERE          c.comtype = '6'
UNION ALL
SELECT          a.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.codenc as comtypenc, k.CONT as belongnc, l.NAME as salesnc, a.comq1 as comq
, d.comn, b.linetel AS LINETEL, b.gateway, b.lineip AS CMTYIP, f.codenc AS LINERATE, b.hardwaredat AS ARRIVEDAT, b.dropdat AS RCOMDROP
, '' AS idslamip, a.cusnc, a.contacttel, d.CONTACTTEL as companytel, j.CUTNC+a.TOWNSHIP2+a.RADDR2 as raddr
, replace(a.ip11 + '.' + a.ip12 + '.' + a.ip13 + '.' + a.ip14, '...', '') AS CUSTIP, g.codenc AS CASEKIND, h.codenc AS paycycle, i.codenc AS paytype
, replace(a.overdue, 'N', '') AS overdue, replace(a.freecode, 'N', '') AS freecode, a.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, a.dropdat, 
a.canceldat, replace(a.secondcase, 'N', '') AS secondcase, '' AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat
FROM              RTLessorAvsCust a 
INNER JOIN RTLessorAvsCmtyLine b ON a.comq1 = b.comq1 AND a.lineq1 = b.lineq1 
INNER JOIN RTLessorAvsCmtyH d ON d .comq1 = b.comq1 
LEFT OUTER JOIN RTCode c ON c.code = a.comtype AND c.kind = 'P5' 
LEFT OUTER JOIN RTCode f ON f.code = b.linerate AND f.kind = 'D3' 
LEFT OUTER JOIN RTCode g ON g.code = a.casekind AND g.kind = 'O9' 
LEFT OUTER JOIN RTCode h ON h.code = a.paycycle AND h.kind = 'M8' 
LEFT OUTER JOIN RTCode i ON i.code = a.paytype AND i.kind = 'M9' 
LEFT OUTER JOIN	RTCounty j on j.CUTID=a.CUTID2
LEFT OUTER JOIN RTConsignee k ON k.CUSID = b.CONSIGNEE
LEFT OUTER JOIN RTEmployee l ON l.EMPLY = b.SALESID
UNION ALL
SELECT          c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d .comn, b.linetel AS LINETEL, 
                            b.gateway, b.lineip AS CMTYIP, f.codenc AS LINERATE, b.hardwaredat AS ARRIVEDAT, b.dropdat AS RCOMDROP, 
                            '' AS idslamip, c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11 + '.' + a.ip12 + '.' + a.ip13 + '.' + a.ip14, '...', 
                            '') AS CUSTIP, g.codenc AS CASEKIND, h.codenc AS paycycle, i.codenc AS paytype, replace(a.overdue, 'N', '') 
                            AS overdue, replace(a.freecode, 'N', '') AS freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, 
                            c.canceldat, replace(a.secondcase, 'N', '') AS secondcase, '' AS nciccusno, '' AS Sp499cons, NULL 
                            AS WtlApplyDat
FROM              RTLessorCust a INNER JOIN
                            RTLessorCmtyLine b ON a.comq1 = b.comq1 AND a.lineq1 = b.lineq1 INNER JOIN
                            RTLessorCmtyH d ON d .comq1 = b.comq1 LEFT OUTER JOIN
                            HBAdslCmtyCust c ON c.comq1 = a.comq1 AND c.lineq1 = a.lineq1 AND a.cusid = c.cusid LEFT OUTER JOIN
                            RTCode f ON f.code = b.linerate AND f.kind = 'D3' LEFT OUTER JOIN
                            RTCode g ON g.code = a.casekind AND g.kind = 'O9' LEFT OUTER JOIN
                            RTCode h ON h.code = a.paycycle AND h.kind = 'M8' LEFT OUTER JOIN
                            RTCode i ON i.code = a.paytype AND i.kind = 'M9'
WHERE          c.comtype = '8'
UNION ALL
SELECT          c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d .comn, b.linetel AS LINETEL, 
                            b.gateway, b.lineip AS CMTYIP, f.codenc AS LINERATE, b.arrivedat AS ARRIVEDAT, b.dropdat AS RCOMDROP, 
                            '' AS idslamip, c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11 + '.' + a.ip12 + '.' + a.ip13 + '.' + a.ip14, '...', 
                            '') AS CUSTIP, '' AS CASEKIND, '' AS paycycle, '' AS paytype, '' AS overdue, replace(a.freecode, 'N', '') AS freecode, 
                            c.docketdat, a.strbillingdat, NULL AS newbillingdat, NULL AS duedat, c.dropdat, c.canceldat, '' AS secondcase, 
                            '' AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat
FROM              RTPrjCust a INNER JOIN
                            RTPrjCmtyLine b ON a.comq1 = b.comq1 AND a.lineq1 = b.lineq1 INNER JOIN
                            RTPrjCmtyH d ON d .comq1 = b.comq1 LEFT OUTER JOIN
                            HBAdslCmtyCust c ON c.comq1 = a.comq1 AND c.lineq1 = a.lineq1 AND a.cusid = c.cusid LEFT OUTER JOIN
                            RTCode f ON f.code = b.linerate AND f.kind = 'D3'
WHERE          c.comtype = '9'
UNION ALL
SELECT          c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d .comn, b.linetel AS LINETEL, 
                            b.gateway, b.lineip AS CMTYIP, f.codenc AS LINERATE, b.hardwaredat AS ARRIVEDAT, b.dropdat AS RCOMDROP, 
                            '' AS idslamip, c.cusnc, c.contacttel, c.companytel, c.raddr, a.ip11 AS CUSTIP, '' AS CASEKIND, h.codenc AS paycycle, 
                            '' AS paytype, CONVERT(varchar(10), a.overduedat, 111) AS overdue, replace(a.freecode, 'N', '') AS freecode, 
                            c.docketdat, a.strbillingdat, NULL AS newbillingdat, NULL AS duedat, c.dropdat, c.canceldat, '' AS secondcase, 
                            a.memberid AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat
FROM              RTSonetCust a INNER JOIN
                            RTSonetCmtyLine b ON a.comq1 = b.comq1 AND a.lineq1 = b.lineq1 INNER JOIN
                            RTSonetCmtyH d ON d .comq1 = b.comq1 LEFT OUTER JOIN
                            HBAdslCmtyCust c ON c.comq1 = a.comq1 AND c.lineq1 = a.lineq1 AND a.cusid = c.cusid LEFT OUTER JOIN
                            RTCode f ON f.code = b.linerate AND f.kind = 'D3' LEFT OUTER JOIN
                            RTCode h ON h.code = a.paycycle AND h.kind = 'M8'
WHERE          c.comtype = 'A'
UNION ALL
SELECT          c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d .comn, b.linetel AS LINETEL, 
                            b.gateway, b.lineip AS CMTYIP, f.codenc AS LINERATE, b.hardwaredat AS ARRIVEDAT, b.dropdat AS RCOMDROP, 
                            '' AS idslamip, c.cusnc, c.contacttel, c.companytel, c.raddr, a.ip11 AS CUSTIP, '' AS CASEKIND, h.codenc AS paycycle, 
                            '' AS paytype, CONVERT(varchar(10), a.overduedat, 111) AS overdue, replace(a.freecode, 'N', '') AS freecode, 
                            c.docketdat, a.strbillingdat, NULL AS newbillingdat, NULL AS duedat, c.dropdat, c.canceldat, '' AS secondcase, 
                            a.memberid AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat
FROM              RTfareastCust a INNER JOIN
                            RTfareastCmtyLine b ON a.comq1 = b.comq1 AND a.lineq1 = b.lineq1 INNER JOIN
                            RTfareastCmtyH d ON d .comq1 = b.comq1 LEFT OUTER JOIN
                            HBAdslCmtyCust c ON c.comq1 = a.comq1 AND c.lineq1 = a.lineq1 AND a.cusid = c.cusid LEFT OUTER JOIN
                            RTCode f ON f.code = b.linerate AND f.kind = 'D3' LEFT OUTER JOIN
                            RTCode h ON h.code = a.paycycle AND h.kind = 'M8'
WHERE          c.comtype = 'B'