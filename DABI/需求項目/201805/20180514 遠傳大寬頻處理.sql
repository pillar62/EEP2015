SELECT * FROM RTBillCharge

SELECT * FROM RTCode 
WHERE KIND = 'O9'

SELECT * FROM RTCode 
WHERE KIND = 'M8'

--新增遠傳大寬頻資料
INSERT INTO RTCode
(KIND, CODE, CODENC, EUSR, EDAT, UUSR, UDAT, PARM1)
VALUES 
('L5', '10', '遠傳大寬頻', '', GETDATE() , '', NULL, 'B')

--新增收費項目
INSERT INTO RTBillCharge
(CASETYPE, CASEKIND, PAYCYCLE, PERIOD, AMT, AMT2, DROPAMT, DROPAMT2, MEMO, BILLCOD)
VALUES
('10', '54', '06', 1, 0, 0, 0, 0, '遠傳/20M/月繳', ''),
('10', '54', '02', 12, 0, 0, 0, 0, '遠傳/20M/年繳', ''),
('10', '54', '03', 24, 0, 0, 0, 0, '遠傳/20M/兩年繳', ''),
('10', '55', '06', 1, 0, 0, 0, 0, '遠傳/40M/月繳', ''),
('10', '55', '02', 12, 0, 0, 0, 0, '遠傳/40M/年繳', ''),
('10', '55', '03', 24, 0, 0, 0, 0, '遠傳/40M/兩年繳', ''),
('10', '56', '06', 1, 0, 0, 0, 0, '遠傳/70M/月繳', ''),
('10', '56', '02', 12, 0, 0, 0, 0, '遠傳/70M/年繳', ''),
('10', '56', '03', 24, 0, 0, 0, 0, '遠傳/70M/兩年繳', '')