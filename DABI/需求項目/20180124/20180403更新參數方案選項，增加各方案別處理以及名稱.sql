--select a.CASETYPE, a.CASEKIND, a.PAYCYCLE, b.CODENC, c.CODENC, a.MEMO

update a set a.memo='AVS¡þ'+b.CODENC+'¡þ'+c.CODENC
from RTBillCharge a
left join RTCode b On b.KIND = 'O9' and b.code=a.CASEKIND
left join RTCode c On c.KIND = 'M8' and c.code=a.PAYCYCLE
where a.casetype = '07'

select * from RTCode where KIND = 'L5'
select * from RTCode where KIND = 'O9'
select * from RTCode where KIND = 'M8'
update rtcode set PARM1='8' where kind='L5' and code='06'
update rtcode set PARM1='7' where kind='L5' and code='07'
update rtcode set PARM1='9' where kind='L5' and code='08'