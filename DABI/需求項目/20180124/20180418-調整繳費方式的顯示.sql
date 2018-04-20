drop view V_BARCODE
go

CREATE VIEW V_BARCODE AS
SELECT          (SELECT          COUNT(*) AS Expr1
                            FROM               dbo.RTLessorAVSCustBillingBarcode AS RD
                            WHERE           (NOTICEID = R.NOTICEID) AND (CASEKIND = R.CASEKIND) AND (PAYCYCLE <= R.PAYCYCLE)) 
                            AS ITEMNO, R.CSNOTICEID, R.CSCUSID, R.NOTICEID, R.CASEKIND, R.PAYCYCLE, R.CSBARCOD1, R.CSBARCOD2, 
                            R.CSBARCOD3, R.ATMCOD, j.MEMO, '¡¼' + c.CODENC + '(' + CONVERT(varchar(6), j.AMT) + '¤¸)' AS CASEPAYD, 
                            j.PERIOD AS QT_MON
FROM              dbo.RTLessorAVSCustBillingBarcode AS R 
LEFT OUTER JOIN dbo.RTBillCharge AS j ON j.PAYCYCLE = R.PAYCYCLE AND j.CASEKIND = R.CASEKIND AND j.CASETYPE = '07'
LEFT OUTER JOIN RTCode c on c.KIND='M8' and CODE=j.PAYCYCLE