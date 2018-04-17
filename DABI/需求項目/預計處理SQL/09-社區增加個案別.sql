
alter table RTLessorAVSCmtyH add COMTYPE varchar(4) null default('');
go

alter table RTLessorAVSCmtyLine add RADDR varchar(60) null default('');
go

update RTLessorAVSCmtyH set COMTYPE = '7';

