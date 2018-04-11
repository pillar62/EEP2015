insert into RTCode (KIND, CODE, CODENC, PARM1) values ('R7', 'T90047', '¾G´_Ä}', '1')
insert into RTCode (KIND, CODE, CODENC, PARM1) values ('R7', 'C92026', '¬x·ç§»', '2')
insert into RTCode (KIND, CODE, CODENC, PARM1) values ('R7', 'K89002', '©P·G©¾', '3')

SELECT * FROM RTCODE
WHERE KIND = 'R7'
ORDER BY PARM1 