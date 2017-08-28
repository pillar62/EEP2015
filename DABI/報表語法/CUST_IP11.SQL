alter table RTLessorAVSCust alter column IP11 varchar(20)

update RTLessorAVSCust
set IP11 = IP11+'.'+IP12+'.'+IP13+'.'+IP14
where isnull(IP11, '') <> '' and Len(IP11) <=3