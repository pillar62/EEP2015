
alter table RTLessorAVSCmtyH add COMTYPE varchar(4) null default('');
alter table RTLessorAVSCmtyLine add RADDR varchar(60) null default('');

update RTLessorAVSCmtyH set COMTYPE = '7';

