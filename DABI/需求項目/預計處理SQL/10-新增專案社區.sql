insert into RTLessorAVSCmtyH
(           COMN, CUTID, TOWNSHIP, RADDR, RZONE, COMCNT, CONTACT, CONTACTTEL, UUSR, UDAT
							,COMTYPE)
SELECT           COMN, CUTID, TOWNSHIP, RADDR, RZONE, COMCNT, CONTACT, CONTACTTEL, UUSR, UDAT, '9'
FROM              RTPrjCmtyH