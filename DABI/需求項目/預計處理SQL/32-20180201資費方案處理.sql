insert into RTBillCharge
select * from (
select CASETYPE
, case CASEKIND 
when '01' then '39' 
when '02' then '44' 
when '03' then '35' 
when '04' then '36' 
when '05' then '45' 
when '06' then '' 
when '07' then '46' 
when '08' then '34' 
when '09' then '38' 
when '10' then '37' 
when '11' then '40' 
when '14' then '' 
when '15' then '31' 
when '16' then '33' 
when '17' then '41' 
when '18' then '42' 
when '19' then '43' 
when '20' then '32' 
end as casekind
, PAYCYCLE, PERIOD, AMT, AMT2, DROPAMT, DROPAMT2, MEMO, BILLCOD 
from RTBillCharge
where CASETYPE = '07') a
where casekind <> '' and CASETYPE+casekind + PAYCYCLE not in (select CASETYPE+casekind + PAYCYCLE from  RTBillCharge
where CASETYPE = '07')

ªôµn³Ó