DROP VIEW V_RT205

CREATE VIEW V_RT205 AS 
SELECT          c.comtype, A.COMQ1, NULL AS LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, 
                            b.t1attachtel AS LINETEL, '' AS gateway, e.snmpip AS CMTYIP, f.codenc AS LINERATE, b.t1arrive AS ARRIVEDAT, 
                            b.rcomdrop, '' AS idslamip, c.cusnc, c.contacttel, c.companytel, c.raddr, '' AS CUSTIP, a.usekind AS CASEKIND, 
                            '' AS paycycle, '' AS paytype, replace(a.overdue, 'N', '') AS overdue, replace(a.freecode, 'N', '') AS freecode, 
                            c.docketdat, NULL AS strbillingdat, NULL AS newbillingdat, NULL AS duedat, c.dropdat, c.canceldat, '' AS secondcase, 
                            '' AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat, LEFT
                                ((SELECT          NETIP + '/ '
                                    FROM              RTCmty
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH('')), len
                                ((SELECT          NETIP + '/ '
                                    FROM              RTCmty
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH(''))) - 1) AS PP, a.MOBILE, a.HOME, '01' AS CUSTSRC, 
                            A.SPEED, (SELECT TOP 1 Y.CODENC FROM RTCmty X LEFT JOIN RTCode Y ON Y.KIND='D3' 
							        AND Y.CODE=X.LINERATE WHERE X.COMQ1=A.COMQ1 ORDER BY X.LINERATE DESC) AS LINERATEM
FROM              RTCust a INNER JOIN
                            RTCmty b ON a.comq1 = b.comq1 LEFT OUTER JOIN
                            HBAdslCmtyCust c ON a.cusid = c.cusid AND a.entryno = c.entryno LEFT OUTER JOIN
                            RTsnmp e ON e.comq1 = b.comq1 AND e.comkind = '3' LEFT OUTER JOIN
                            RTCode f ON f.code = b.linerate AND f.kind = 'D3'
WHERE          (c.comtype = '1' OR c.comtype = '4')
UNION ALL
SELECT          c.comtype, A.COMQ1, '' AS LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, 
                            b.cmtytel AS LINETEL, '' AS gateway, b.ipaddr AS CMTYIP, f.codenc AS LINERATE, b.linearrive AS ARRIVEDAT, 
                            b.rcomdrop, '' AS idslamip, c.cusnc, c.contacttel, c.companytel, c.raddr, '' AS CUSTIP, a.usekind AS CASEKIND, 
                            '' AS paycycle, '' AS paytype, replace(a.overdue, 'N', '') AS overdue, replace(a.freecode, 'N', '') AS freecode, 
                            c.docketdat, NULL AS strbillingdat, NULL AS newbillingdat, NULL AS duedat, c.dropdat, c.canceldat, '' AS secondcase, 
                            '' AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat, LEFT
                                ((SELECT          ipaddr + '/ '
                                    FROM              RTCmty
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH('')), len
                                ((SELECT          ipaddr + '/ '
                                    FROM              RTCmty
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH(''))) - 1) AS PP, a.MOBILE, a.HOME, '01' AS CUSTSRC, 
                            A.SPEED, (SELECT TOP 1 Y.CODENC FROM RTCustAdslCmty X LEFT JOIN RTCode Y ON Y.KIND='D3' 
							        AND Y.CODE=X.LINERATE WHERE X.CUTYID=A.COMQ1 ORDER BY X.LINERATE DESC) AS LINERATE
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
                            a.exttel + '-' + a.sphnno AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat, LEFT
                                ((SELECT          ipaddr + '/ '
                                    FROM              RTSparqAdslCmty
                                    WHERE          cutyid = a.COMQ1 FOR XML PATH('')), len
                                ((SELECT          ipaddr + '/ '
                                    FROM              RTSparqAdslCmty
                                    WHERE          cutyid = a.COMQ1 FOR XML PATH(''))) - 1) AS PP, a.MOBILE, a.HOME, '01' AS CUSTSRC, 
                            A.SPEED, (SELECT TOP 1 Y.CODENC FROM RTSparqAdslCmty X LEFT JOIN RTCode Y ON Y.KIND='D3' 
							        AND Y.CODE=X.LINERATE WHERE X.CUTYID=A.COMQ1 ORDER BY X.LINERATE DESC) AS LINERATE
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
                            '' AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat, LEFT
                                ((SELECT          lineip + '/ '
                                    FROM              RTEbtCmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH('')), len
                                ((SELECT          lineip + '/ '
                                    FROM              RTEbtCmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH(''))) - 1) AS PP, a.MOBILE, a.CONTACTTEL, '01' AS CUSTSRC, 
                            '' AS SPEED, (SELECT TOP 1 Y.CODENC FROM RTEbtCmtyLine X LEFT JOIN RTCode Y ON Y.KIND='D3' 
							        AND Y.CODE=X.LINERATE WHERE X.COMQ1=A.COMQ1 ORDER BY X.LINERATE DESC) AS LINERATE
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
                            CASE a.consignee WHEN '12973008' THEN '­ì»·ºÝ¥Î¤á' ELSE '' END AS Sp499cons, a.WtlApplyDat, LEFT
                                ((SELECT DISTINCT 
                                                                replace(str(lineipstr1) + '.' + str(lineipstr2) + '.' + str(lineipstr3) + '.' + str(lineipstr4) 
                                                                + '~' + str(lineipend), ' ', '') + '/ '
                                    FROM              RTSparq499CmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH('')), len
                                ((SELECT DISTINCT 
                                                                replace(str(lineipstr1) + '.' + str(lineipstr2) + '.' + str(lineipstr3) + '.' + str(lineipstr4) 
                                                                + '~' + str(lineipend), ' ', '') + '/ '
                                    FROM              RTSparq499CmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH(''))) - 1) AS PP, a.MOBILE, a.CONTACTTEL, '01' AS CUSTSRC, 
                            '' AS SPEED, (SELECT TOP 1 Y.CODENC FROM RTSparq499CmtyLine X LEFT JOIN RTCode Y ON Y.KIND='D3' 
							        AND Y.CODE=X.LINERATE WHERE X.COMQ1=A.COMQ1 ORDER BY X.LINERATE DESC) AS LINERATE
FROM              RTSparq499Cust a INNER JOIN
                            RTSparq499CmtyLine b ON a.comq1 = b.comq1 AND a.lineq1 = b.lineq1 INNER JOIN
                            RTSparq499CmtyH d ON d .comq1 = b.comq1 LEFT OUTER JOIN
                            HBAdslCmtyCust c ON c.comq1 = a.comq1 AND c.lineq1 = a.lineq1 AND a.cusid = c.cusid LEFT OUTER JOIN
                            RTCode f ON f.code = b.linerate AND f.kind = 'D3' LEFT OUTER JOIN
                            RTCode g ON g.code = a.casetype AND g.kind = 'L9' LEFT OUTER JOIN
                            RTCode h ON h.code = a.paytype AND h.kind = 'M1'
WHERE          c.comtype = '6'
UNION ALL
SELECT          a.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.codenc AS comtypenc, k.CONT AS belongnc, l.NAME AS salesnc, 
                            LTRIM(STR(a.comq1)) + '-' + LTRIM(STR(A.LINEQ1)) AS comq, d .comn, b.linetel AS LINETEL, b.gateway, 
                            b.lineip AS CMTYIP, f.codenc AS LINERATE, b.hardwaredat AS ARRIVEDAT, b.dropdat AS RCOMDROP, '' AS idslamip, 
                            a.cusnc, a.contacttel, d .CONTACTTEL AS companytel, j.CUTNC + a.TOWNSHIP2 + a.RADDR2 AS raddr, replace(a.ip11, 
                            '...', '') AS CUSTIP, g.codenc AS CASEKIND, h.codenc AS paycycle, i.codenc AS paytype, replace(a.overdue, 'N', '') 
                            AS overdue, replace(a.freecode, 'N', '') AS freecode, a.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, a.dropdat, 
                            a.canceldat, replace(a.secondcase, 'N', '') AS secondcase, '' AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat, 
                            LEFT
                                ((SELECT          LINEIP + '/ '
                                    FROM              RTLessorAVSCmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH('')), len
                                ((SELECT          LINEIP + '/ '
                                    FROM              RTLessorAVSCmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH(''))) - 1) AS PP, a.MOBILE, a.CONTACTTEL, isnull(A.CUSTSRC, 
                            '01') AS CUSTSRC, A.USERRATE, (SELECT TOP 1 Y.CODENC FROM RTLessorAVSCmtyLine X LEFT JOIN RTCode Y ON Y.KIND='D3' 
							        AND Y.CODE=X.LINERATE WHERE X.COMQ1=A.COMQ1 ORDER BY X.LINERATE DESC) AS LINERATE
FROM              RTLessorAvsCust a LEFT OUTER JOIN
                            RTLessorAvsCmtyLine b ON a.comq1 = b.comq1 AND a.lineq1 = b.lineq1 LEFT OUTER JOIN
                            RTLessorAvsCmtyH d ON d .comq1 = b.comq1 LEFT OUTER JOIN
                            RTCode c ON c.code = a.comtype AND c.kind = 'P5' LEFT OUTER JOIN
                            RTCode f ON f.code = b.linerate AND f.kind = 'D3' LEFT OUTER JOIN
                            RTCode g ON g.code = a.casekind AND g.kind = 'O9' LEFT OUTER JOIN
                            RTCode h ON h.code = a.paycycle AND h.kind = 'M8' LEFT OUTER JOIN
                            RTCode i ON i.code = a.paytype AND i.kind = 'M9' LEFT OUTER JOIN
                            RTCounty j ON j.CUTID = a.CUTID2 LEFT OUTER JOIN
                            RTConsignee k ON k.CUSID = b.CONSIGNEE LEFT OUTER JOIN
                            RTEmployee l ON l.EMPLY = b.SALESID
UNION ALL
SELECT          c.comtype, A.COMQ1, A.LINEQ1, A.CUSID, c.comtypenc, c.belongnc, c.salesnc, c.comq, d .comn, b.linetel AS LINETEL, 
                            b.gateway, b.lineip AS CMTYIP, f.codenc AS LINERATE, b.hardwaredat AS ARRIVEDAT, b.dropdat AS RCOMDROP, 
                            '' AS idslamip, c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11 + '.' + a.ip12 + '.' + a.ip13 + '.' + a.ip14, '...', 
                            '') AS CUSTIP, g.codenc AS CASEKIND, h.codenc AS paycycle, i.codenc AS paytype, replace(a.overdue, 'N', '') 
                            AS overdue, replace(a.freecode, 'N', '') AS freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, 
                            c.canceldat, replace(a.secondcase, 'N', '') AS secondcase, '' AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat, 
                            LEFT
                                ((SELECT          lineip + '/ '
                                    FROM              RTLessorCmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH('')), len
                                ((SELECT          lineip + '/ '
                                    FROM              RTLessorCmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH(''))) - 1) AS PP, a.MOBILE, a.CONTACTTEL, '01' AS CUSTSRC, 
                            A.USERRATE, (SELECT TOP 1 Y.CODENC FROM RTLessorCmtyLine X LEFT JOIN RTCode Y ON Y.KIND='D3' 
							        AND Y.CODE=X.LINERATE WHERE X.COMQ1=A.COMQ1 ORDER BY X.LINERATE DESC) AS LINERATE
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
                            '' AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat, LEFT
                                ((SELECT          lineip + '/ '
                                    FROM              RTPrjCmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH('')), len
                                ((SELECT          lineip + '/ '
                                    FROM              RTPrjCmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH(''))) - 1) AS PP, a.CONTACTTEL, a.CONTACTTEL, 
                            '01' AS CUSTSRC, '' AS SPEED, (SELECT TOP 1 Y.CODENC FROM RTPrjCmtyLine X LEFT JOIN RTCode Y ON Y.KIND='D3' 
							        AND Y.CODE=X.LINERATE WHERE X.COMQ1=A.COMQ1 ORDER BY X.LINERATE DESC) AS LINERATE
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
                            a.memberid AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat, LEFT
                                ((SELECT          lineip + '/ '
                                    FROM              RTSonetCmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH('')), len
                                ((SELECT          lineip + '/ '
                                    FROM              RTSonetCmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH(''))) - 1) AS PP, a.MOBILE, a.CONTACTTEL, '01' AS CUSTSRC, 
                            A.USERRATE, (SELECT TOP 1 Y.CODENC FROM RTSonetCmtyLine X LEFT JOIN RTCode Y ON Y.KIND='D3' 
							        AND Y.CODE=X.LINERATE WHERE X.COMQ1=A.COMQ1 ORDER BY X.LINERATE DESC) AS LINERATE
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
                            a.memberid AS nciccusno, '' AS Sp499cons, NULL AS WtlApplyDat, LEFT
                                ((SELECT          lineip + '/ '
                                    FROM              RTfareastCmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH('')), len
                                ((SELECT          lineip + '/ '
                                    FROM              RTfareastCmtyLine
                                    WHERE          COMQ1 = a.COMQ1 FOR XML PATH(''))) - 1) AS PP, a.MOBILE, a.CONTACTTEL, '01' AS CUSTSRC, 
                            A.USERRATE, (SELECT TOP 1 Y.CODENC FROM RTfareastCmtyLine X LEFT JOIN RTCode Y ON Y.KIND='D3' 
							        AND Y.CODE=X.LINERATE WHERE X.COMQ1=A.COMQ1 ORDER BY X.LINERATE DESC) AS LINERATE
FROM              RTfareastCust a INNER JOIN
                            RTfareastCmtyLine b ON a.comq1 = b.comq1 AND a.lineq1 = b.lineq1 INNER JOIN
                            RTfareastCmtyH d ON d .comq1 = b.comq1 LEFT OUTER JOIN
                            HBAdslCmtyCust c ON c.comq1 = a.comq1 AND c.lineq1 = a.lineq1 AND a.cusid = c.cusid LEFT OUTER JOIN
                            RTCode f ON f.code = b.linerate AND f.kind = 'D3' LEFT OUTER JOIN
                            RTCode h ON h.code = a.paycycle AND h.kind = 'M8'
WHERE          c.comtype = 'B'