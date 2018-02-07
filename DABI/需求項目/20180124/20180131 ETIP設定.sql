update RTLessorAVSCust
set ip11 = ip11 + '.' + IP12 + '.' + IP13 + '.' + IP14
where COMTYPE = '8' and len(ip11) <= 3