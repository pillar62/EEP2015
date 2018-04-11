drop view V_RT2051

create view V_RT2051 as 
SELECT          *
FROM              HBAdslCmty
where comtype <> '7' and comtype <> '8' and comtype <> '9' 
UNION ALL
SELECT          A.COMQ1, B.LINEQ1, A.COMN, A.RADDR, A.COMCNT, C.QQ AS USERCNT, A.COMTYPE, B.ADSLAPPLYDAT, 
                            '' AS COMSOURCE, D .GROUPNC, '' AS LEADER, '' AS COMAGREE, CONTACT, '' AS SIGNEDAT, 0 AS SIGNIFICANTCNT,
                             E.AREANC, DROPDAT, A.CONTACT, A.CONTACTTEL, B.LINEIP, B.LINETEL, B.CANCELDAT
FROM              RTLessorAVSCmtyLine B LEFT JOIN
                            RTLessorAVSCmtyH A ON A.COMQ1 = B.COMQ1 LEFT JOIN
                                (SELECT          COMQ1, COUNT(*) AS QQ
                                  FROM               RTLessorAVSCust
                                  GROUP BY    COMQ1) C ON C.COMQ1 = A.COMQ1 LEFT JOIN
                            RTSalesGroup D ON D .GROUPID = B.GROUPID LEFT JOIN
                            RTArea E ON E.AREAID = B.AREAID