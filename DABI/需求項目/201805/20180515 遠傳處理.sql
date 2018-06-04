SELECT RTCtyTown.ZIP AS zip 
FROM RTLessorAVSCmtyH
INNER JOIN RTCtyTown ON RTLessorAVSCmtyH.CUTID = RTCtyTown.CUTID AND RTLessorAVSCmtyH.TOWNSHIP = RTCtyTown.TOWNSHIP 
where RTLessorAVSCmtyH.comq1 = 100

alter table RTLessorAVSCust
add MEMBERID varchar(16) null

--select b.MEMBERID, a.* 
update a set a.MEMBERID=b.MEMBERID
from RTLessorAVSCust a
left join RTfareastCust b on b.CUSID=a.CUSID
where isnull(b.MEMBERID, '') <> ''