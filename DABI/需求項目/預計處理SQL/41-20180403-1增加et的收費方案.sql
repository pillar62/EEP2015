INSERT INTO RTBillCharge
SELECT A.CASETYPE, C.CODE, A.PAYCYCLE, A.PERIOD, A.AMT, A.AMT2, A.DROPAMT, A.DROPAMT2, A.MEMO, A.BILLCOD
FROM RTBillCharge A
LEFT JOIN RTCode B ON B.KIND='L5' AND B.CODE=A.CASETYPE
LEFT JOIN RTCode c ON C.KIND='O9' AND A.MEMO LIKE '%'+C.CODENC+'��%'
WHERE b.PARM1='8' AND C.CODE <> A.CASEKIND
AND A.CASETYPE + C.CODE +A.PAYCYCLE
NOT IN (SELECT A.CASETYPE+ A.CASEKIND +A.PAYCYCLE FROM RTBillCharge)
and A.CASETYPE = '06' and C.CODE > '05'
AND C.CODE IN (
SELECT CODE
 FROM RTCode
WHERE KIND = 'O9' AND PARM1='8') AND ISNULL(C.CODE, '') <> ''

/*
SELECT * FROM RTBillCharge
WHERE CASETYPE = '06'

SELECT CODE, CODENC
 FROM RTCode
WHERE KIND = 'O9' AND PARM1='8'
*/