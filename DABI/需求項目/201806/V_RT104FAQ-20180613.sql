DROP VIEW V_RT104FAQ 
GO

CREATE VIEW V_RT104FAQ AS
SELECT          A.CUSID AS CUSIDF, ISNULL(C.QT_CC, 0) AS QT_CC, D.CUTNC+A.TOWNSHIP2+A.RADDR2 AS ADDR
FROM              dbo.RTLessorAVSCust AS A 
LEFT OUTER JOIN
(SELECT          CUSID, COUNT(CASENO) AS QT_CC
FROM               dbo.RTFaqM
GROUP BY    CUSID) AS C ON C.CUSID = A.CUSID
LEFT OUTER JOIN RTCounty D ON D.CUTID=A.CUTID2