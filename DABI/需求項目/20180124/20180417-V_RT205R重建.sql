DROP VIEW V_RT205R
GO

CREATE VIEW V_RT205R AS 
SELECT          A.*, B.ENTRYNO, B.IOBOUND, C.CODENC AS NM_IOBOUND, B.ADDUSR, D .NAME, B.ADDDAT, 
                            B.MEMO AS MEMOADD
FROM              (SELECT          a.workno, a.linkno, g.comtype, l.CODENC, b.codenc AS worktype, isnull(c.shortnc, d .name) 
                                                        AS assigneng, isnull(i.shortnc, f.name) AS finisheng, h.comn, g.faqman, e.name AS sndwrkusr, 
                                                        a.sndwrkdat, j.name AS finishsur, k.codenc AS FINISH_NM, a.canceldat, G.CUSID, G.RCVDAT, G.COMQ1, 
                                                        G.LINEQ1, m.LINERATE, m.idslamip, m.gateway, m.CUSTIP, m.CASEKIND, m.secondcase, m.raddr, 
                                                        m.docketdat, m.duedat, m.contacttel + '/' + m.companytel AS tel, m.cusnc, a.FINISHDAT, g.FAQREASON, 
                                                        n.CODENC AS faqreasonnm, g.MEMO, m.CMTYIP, m.strbillingdat, m.dropdat, m.overdue, m.WtlApplyDat, 
                                                        m.paycycle, m.paytype, m.freecode, m.comtypenc, LEFT
                                                            ((SELECT          LINEIP + ','
                                                                FROM              RTLessorAVSCmtyLine
                                                                WHERE          COMQ1 = G.COMQ1 FOR XML PATH('')), len
                                                            ((SELECT          LINEIP + ','
                                                                FROM              RTLessorAVSCmtyLine
                                                                WHERE          COMQ1 = G.COMQ1 FOR XML PATH(''))) - 1) AS PP, m.SPEED
                            FROM               RTSndWork a LEFT OUTER JOIN
                                                        RTCode b ON b.code = a.worktype AND b.kind = 'P6' LEFT OUTER JOIN
                                                        RTObj c ON c.cusid = a.assigncons LEFT OUTER JOIN
                                                        RTEmployee d ON d .emply = a.assigneng LEFT OUTER JOIN
                                                        RTEmployee e ON e.emply = a.sndwrkusr LEFT OUTER JOIN
                                                        RTEmployee j ON j.emply = a.finishusr LEFT OUTER JOIN
                                                        RTEmployee f ON f.emply = a.finisheng LEFT OUTER JOIN
                                                        RTObj i ON i.cusid = a.finishcons LEFT OUTER JOIN
                                                        RTFaqM g ON g.caseno = a.linkno LEFT OUTER JOIN
                                                        RTLessorAVSCmtyH h ON g.comq1 = h.comq1 LEFT OUTER JOIN
                                                        RTCode k ON k.code = a.finishtyp AND k.kind = 'P9' LEFT OUTER JOIN
                                                        RTCode l ON l.CODE = g.COMTYPE AND l.kind = 'P5' LEFT OUTER JOIN
                                                        V_RT205 m ON m.COMQ1 = g.COMQ1 AND m.LINEQ1 = g.LINEQ1 AND 
                                                        m.CUSID = g.CUSID LEFT OUTER JOIN
                                                        RTCode n ON n.CODE = g.FAQREASON AND n.kind = 'P7'
                            WHERE 1=1) A LEFT OUTER JOIN
                            RTFaqAdd B ON B.CASENO = A.LINKNO LEFT OUTER JOIN
                            RTCODE C ON C.KIND = 'P8' AND C.CODE = B.IOBOUND LEFT OUTER JOIN
                            RTEmployee d ON d .emply = B.ADDUSR