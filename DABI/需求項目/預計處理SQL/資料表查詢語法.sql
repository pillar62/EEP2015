SELECT * FROM INFORMATION_SCHEMA.Tables
where TABLE_NAME LIKE 'RTLessorAVS%' 

SELECT * FROM INFORMATION_SCHEMA.Tables
where TABLE_NAME LIKE 'RTLessorCMTY%'  OR TABLE_NAME LIKE 'RTLessorCUST%'

select o.name, i.rows
from sysobjects o inner join sysindexes i on o.id = i.id
where i.indid = 1 AND o.name LIKE  'RTLessorAVS%' 
and i.rows <> 0
order by o.name desc

select o.name, i.rows
from sysobjects o inner join sysindexes i on o.id = i.id
where i.indid = 1 AND (o.name LIKE 'RTLessorCMTY%'  OR o.name LIKE 'RTLessorCUST%')
and i.rows <> 0
order by o.name desc

--insert into RTLessorAVSCustSndwork
--select * from RTLessorCustSndwork

select o.name, i.rows
from sysobjects o inner join sysindexes i on o.id = i.id
where i.indid = 1 AND o.name LIKE  'RTPrj%' 
and i.rows <> 0
order by o.name desc
