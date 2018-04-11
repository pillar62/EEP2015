SELECT * FROM RTCode
WHERE KIND = 'D3'

INSERT INTO RTCode (KIND, CODE, CODENC, PARM1) VALUES ('D3', '25', '100M/100M', '23 AVSCity')
INSERT INTO RTCode (KIND, CODE, CODENC, PARM1) VALUES ('D3', '26', '500M/250M', '26 AVSCity')
INSERT INTO RTCode (KIND, CODE, CODENC, PARM1) VALUES ('D3', '27', '1G/600M', '27 AVSCity')

UPDATE RTCode
SET CODENC = '35M/6M'
WHERE  KIND = 'D3' AND CODE = '20'
