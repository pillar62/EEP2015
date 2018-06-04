DROP VIEW V_RT312
GO

CREATE VIEW V_RT312 AS 
SELECT          seq, applydat, docketdat, docketdatq, applykind, updcode, updtel, memberid, mak_id, sale_id, cust_kind, 
                            company_name, coboss, cobossid, company_id, case_no, dis_no, cusnc, codenc, socialid, sex_KIND, contact_name, 
                            contact_tel, contact_birth, contact_mobile, agent_cardtype, agent_idno, agent_callname, agent_name, agent_tel, 
                            agent_birth, zip3, cutnc3, township3, raddr3, zip2, cutnc2, township2, raddr2, zip1, cutnc1, township1, raddr1, comn, 
                            ip11s, ip11e, ncicdate, codenc2, secondno, apply_no, paycycle, ID_DIS
FROM              (SELECT          ISNULL(CONVERT(varchar(100), GETDATE(), 112), '') AS seq, ISNULL(CONVERT(varchar(100), 
                                                        a.APPLYDAT, 101), '') AS applydat, ISNULL(CONVERT(varchar(100), a.DOCKETDAT, 101), '') AS docketdat, 
                                                        a.DOCKETDAT AS docketdatq, 'A' AS applykind, '' AS updcode, '' AS updtel, ISNULL(a.MEMBERID, '') 
                                                        AS memberid, 'C51A030000' AS mak_id, '73885' AS sale_id, 
                                                        CASE WHEN a.IDNUMBERTYPE = '02' THEN '51' ELSE '1' END AS cust_kind, 
                                                        CASE WHEN a.IDNUMBERTYPE = '02' THEN isnull(a.cusnc, '') ELSE '' END AS company_name, 
                                                        CASE WHEN a.IDNUMBERTYPE = '02' THEN isnull(coboss, '') ELSE '' END AS coboss, 
                                                        CASE WHEN a.IDNUMBERTYPE = '02' THEN isnull(cobossid, '') ELSE '' END AS cobossid, 
                                                        CASE WHEN a.IDNUMBERTYPE = '02' THEN isnull(cusid, '') ELSE '' END AS company_id, 
                                                        CASE WHEN a.USERRATE = '20M' THEN '1054' WHEN a.USERRATE = '40M' THEN '1053' WHEN a.USERRATE = '70M'
                                                         THEN '1052' ELSE '' END AS case_no, '' AS dis_no, ISNULL(a.CUSNC, '') AS cusnc, 
                                                        ISNULL(dbo.RTCode.CODENC, '') AS codenc, ISNULL(a.SOCIALID, '') AS socialid, 
                                                        CASE WHEN a.IDNUMBERTYPE = '02' THEN CASE WHEN substring(a.cobossid, 2, 1) 
                                                        = '1' THEN '先生' WHEN substring(a.cobossid, 2, 1) 
                                                        = '2' THEN '女士' ELSE '' END ELSE CASE WHEN substring(a.socialid, 2, 1) 
                                                        = '1' THEN '先生' WHEN substring(a.socialid, 2, 1) = '2' THEN '女士' ELSE '' END END AS sex_KIND, 
                                                        CASE WHEN a.IDNUMBERTYPE = '02' THEN isnull(a.cocontact, '') ELSE isnull(a.cusnc, '') 
                                                        END AS contact_name, CASE WHEN a.IDNUMBERTYPE = '02' THEN isnull(a.cocontacttel, '') 
                                                        ELSE isnull(a.contacttel, '') END AS contact_tel, 
                                                        CASE WHEN a.IDNUMBERTYPE = '02' THEN '' ELSE isnull(CONVERT(varchar(100), a.birthday, 101), '') 
                                                        END AS contact_birth, CASE WHEN a.IDNUMBERTYPE = '02' THEN '' ELSE isnull(a.mobile, '') 
                                                        END AS contact_mobile, '' AS agent_cardtype, '' AS agent_idno, '' AS agent_callname, '' AS agent_name, 
                                                        '' AS agent_tel, '' AS agent_birth, ISNULL(dbo.RTCtyTown.ZIP, '') AS zip3, ISNULL(dbo.RTCounty.CUTNC, 
                                                        '') AS cutnc3, ISNULL(a.TOWNSHIP3, '') AS township3, ISNULL(a.RADDR3, '') AS raddr3, ISNULL(d.ZIP, '') 
                                                        AS zip2, ISNULL(b.CUTNC, '') AS cutnc2, ISNULL(a.TOWNSHIP1, '') AS township2, ISNULL(a.RADDR1, '') 
                                                        AS raddr2, ISNULL(c.ZIP, '') AS zip1, ISNULL(e.CUTNC, '') AS cutnc1, ISNULL(a.TOWNSHIP2, '') 
                                                        AS township1, ISNULL(a.RADDR2, '') AS raddr1, ISNULL(f.COMN, '') AS comn, ISNULL(a.IP11, '') AS ip11s,
                                                         ISNULL(a.IP11, '') AS ip11e, ISNULL(CONVERT(varchar(100), GETDATE(), 101), '') AS ncicdate, 
                                                        ISNULL(g.CODENC, '') AS codenc2, ISNULL(a.SECONDNO, '') AS secondno, '' AS apply_no, 
                                                        ISNULL(a.PAYCYCLE, '') AS paycycle, SUBSTRING(i.MEMO, CHARINDEX(',', i.MEMO) + 1, 
                                                        LEN(SUBSTRING(i.MEMO, CHARINDEX(',', i.MEMO), 100)) - 2) AS ID_DIS
                            FROM               dbo.RTLessorAVSCust AS a LEFT OUTER JOIN
                                                        dbo.RTCode ON a.IDNUMBERTYPE = dbo.RTCode.CODE AND dbo.RTCode.KIND = 'J5' LEFT OUTER JOIN
                                                        dbo.RTCtyTown ON a.CUTID3 = dbo.RTCtyTown.CUTID AND 
                                                        a.TOWNSHIP3 = dbo.RTCtyTown.TOWNSHIP LEFT OUTER JOIN
                                                        dbo.RTCounty ON a.CUTID3 = dbo.RTCounty.CUTID LEFT OUTER JOIN
                                                        dbo.RTCounty AS b ON a.CUTID1 = b.CUTID LEFT OUTER JOIN
                                                        dbo.RTCtyTown AS c ON a.CUTID2 = c.CUTID AND a.TOWNSHIP2 = c.TOWNSHIP LEFT OUTER JOIN
                                                        dbo.RTCtyTown AS d ON a.CUTID1 = d.CUTID AND a.TOWNSHIP1 = d.TOWNSHIP LEFT OUTER JOIN
                                                        dbo.RTCounty AS e ON a.CUTID2 = e.CUTID LEFT OUTER JOIN
                                                        dbo.RTFarEastCmtyH AS f ON a.COMQ1 = f.COMQ1 LEFT OUTER JOIN
                                                        dbo.RTCode AS g ON a.SECONDIDTYPE = g.CODE AND g.KIND = 'L3' LEFT OUTER JOIN
                                                        dbo.RTCode AS h ON a.USERRATE = h.CODE AND h.KIND = 'R6' LEFT OUTER JOIN
                                                        dbo.RTBillCharge AS i ON i.CASETYPE = '10' AND i.CASEKIND = a.CASEKIND AND 
                                                        i.PAYCYCLE = a.PAYCYCLE
                            WHERE           (a.COMTYPE = 'B') AND (a.FREECODE <> 'Y') AND (a.DROPDAT IS NULL) AND (a.CANCELDAT IS NULL)) 
                            AS A