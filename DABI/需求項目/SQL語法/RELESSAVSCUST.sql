alter table RTLessorAVSCust add COMTYPE varchar(4) null default('')

update RTLessorAVSCust set COMTYPE = '7'

alter table RTLessorAVSCust add CUSTSRC varchar(2) null default('')

update RTLessorAVSCust set CUSTSRC = '01'

