CREATE VIEW V_BARCODE AS 
SELECT          (SELECT          COUNT(*) AS Expr1
                            FROM               dbo.RTLessorAVSCustBillingBarcode AS RD
                            WHERE           (NOTICEID = R.NOTICEID) AND (CASEKIND = R.CASEKIND) AND (PAYCYCLE <= R.PAYCYCLE)) 
                            AS ITEMNO, R.CSNOTICEID, R.CSCUSID, R.NOTICEID, R.CASEKIND, R.PAYCYCLE, R.CSBARCOD1, R.CSBARCOD2 
                            , R.CSBARCOD3, R.ATMCOD, j.MEMO, '¡¼' + j.MEMO + '(' + CONVERT(varchar(6), j.AMT) + '¤¸)' AS CASEPAYD
							, PERIOD AS QT_MON
FROM dbo.RTLessorAVSCustBillingBarcode AS R 
LEFT OUTER JOIN dbo.RTBillCharge AS j ON j.PAYCYCLE = R.PAYCYCLE AND j.CASEKIND = R.CASEKIND and CASETYPE = '07'

