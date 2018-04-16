alter table RTLessorAVSCust alter column IP11 varchar(20)
alter table RTLessorAVSCust alter column CONTACTTEL varchar(50)
alter table RTLessorAVSCust alter column MAC varchar(20)
alter table RTLessorAVSCust alter column EQUIP varchar(4)
alter table RTLessorAVSCust alter column GTEQUIP varchar(4)

update RTLessorAVSCust
set IP11 = IP11+'.'+IP12+'.'+IP13+'.'+IP14
where isnull(IP11, '') <> '' and Len(IP11) <=3