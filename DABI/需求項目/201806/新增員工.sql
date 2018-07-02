select c.cusnc, b.emply 
from RTAreaSales a 
inner join RTEmployee b on a.cusid = b.emply 
inner join RTobj c on b.cusid = c.cusid 
where b.tran2 <>'10' and a.areaid LIKE 'C%'

SELECT * FROM RTEmployee
WHERE EMPLY='T03001'

SELECT * FROM RTobj
WHERE CUSID='A123456789'

SELECT * FROM RTAreaSales
WHERE CUSID='T03001'

INSERT INTO RTEmployee (CUSID, EMPLY, NAME) VALUES ('T07001', 'T07001', '¤ý¬F©þ')
INSERT INTO RTobj (CUSID, CUSNC, SHORTNC) VALUES ('T07001', '¤ý¬F©þ', '¤ý¬F©þ')
INSERT INTO RTAreaSales (CUSID, AREAID) VALUES ('T07001', 'C1')