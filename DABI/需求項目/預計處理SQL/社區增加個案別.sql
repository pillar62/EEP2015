
alter table RTLessorAVSCmtyH add COMTYPE varchar(4) null default('')

update RTLessorAVSCmtyH set COMTYPE = '7'
