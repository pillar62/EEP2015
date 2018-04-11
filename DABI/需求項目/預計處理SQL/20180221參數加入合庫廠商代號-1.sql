delete from rtcode where KIND = 'S1' and CODE = '01'
delete from rtcode where KIND = 'S1' and CODE = '02'

INSERT INTO RTCode
(KIND, CODE, CODENC, EUSR, EDAT, PARM1)
VALUES
('S1', '01', '6FF', 'SYS', GETDATE(), '合庫代號')

INSERT INTO RTCode
(KIND, CODE, CODENC, EUSR, EDAT, PARM1)
VALUES
('S1', '02', 'G287', 'SYS', GETDATE(), '合庫代號')