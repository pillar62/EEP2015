create view V_RT312 as 
select * from (
select isnull(CONVERT(varchar(100), GETDATE(), 112),'') as seq,isnull(CONVERT(varchar(100),a.applydat,101),'')  as applydat,
isnull(CONVERT(varchar(100),a.docketdat,101),'') as docketdat, a.docketdat as docketdatq,
'A' as applykind,'' as updcode,''  as updtel,
isnull(a.memberid,'') as memberid,'C51A030000' as mak_id,'73885' as sale_id,
case when a.IDNUMBERTYPE='02' then '51' else '1' end as cust_kind,
case when a.IDNUMBERTYPE='02' then isnull(a.cusnc,'') else '' end as company_name,
case when  a.IDNUMBERTYPE='02' then isnull(coboss,'') else '' end as coboss,
case when  a.IDNUMBERTYPE='02' then isnull(cobossid,'') else '' end as cobossid,
case when  a.IDNUMBERTYPE='02' then isnull(cusid,'') else '' end as company_id,
case when h.codenc='20M' then '1054' when   h.codenc='40M' then '1053'  
when   h.codenc='70M' then '1052' else '' end  as case_no,'' as dis_no,isnull(a.cusnc,'') as cusnc
,isnull(rtcode.codenc,'') as codenc,isnull(a.socialid,'') as socialid,
case when  a.IDNUMBERTYPE='02'  then case when substring(a.cobossid,2,1)='1' then '先生'  when substring(a.cobossid,2,1)='2' then '女士'  else '' end 
       else case when substring(a.socialid,2,1)='1' then '先生'  when substring(a.socialid,2,1)='2' then '女士'  else '' end end as sex_KIND,
case when  a.IDNUMBERTYPE='02'  then isnull(a.cocontact,'') else isnull(a.cusnc,'') end as contact_name,
case when  a.IDNUMBERTYPE='02'  then isnull(a.cocontacttel,'') else isnull(a.contacttel,'') end as contact_tel,
case when  a.IDNUMBERTYPE='02' then '' else isnull(CONVERT(varchar(100),a.birthday, 101),'') end as contact_birth,
case when  a.IDNUMBERTYPE='02' then '' else isnull(a.mobile,'') end as contact_mobile,
'' as agent_cardtype,'' as agent_idno,'' as agent_callname,'' as agent_name,'' as agent_tel,'' as agent_birth,
isnull(rtctytown.zip,'') as zip3,isnull(rtcounty.cutnc,'') as cutnc3,isnull(a.township3,'') as township3,isnull(a.raddr3,'') as raddr3,
isnull(d.zip,'') as zip2,isnull(b.cutnc,'') as cutnc2,isnull(a.township1,'') as township2,isnull(a.raddr1,'') as raddr2,
isnull(c.zip,'') as zip1,isnull(e.cutnc,'') as cutnc1,isnull(a.township2,'') as township1,isnull(a.raddr2,'') as raddr1,
isnull(f.comn,'') as comn,isnull(a.ip11,'') as ip11s,
isnull(a.ip11,'') as ip11e,isnull(CONVERT(varchar(100), GETDATE(), 101),'') as ncicdate,isnull(g.codenc,'') as codenc2,isnull(a.secondno,'') as secondno,'' as apply_no,
isnull(a.paycycle,'') as paycycle
from rtfareastcust a
left outer join rtcode on a.idnumbertype=rtcode.code and rtcode.kind='J5'
left outer join rtctytown on a.cutid3=rtctytown.cutid and  a.township3=rtctytown.township 
left outer join rtcounty on a.cutid3=rtcounty.cutid
left outer join rtcounty b on a.cutid1=b.cutid
left outer join rtctytown c on a.cutid2=c.cutid and  a.township2=c.township 
left outer join rtctytown d on a.cutid1=d.cutid and  a.township1=d.township 
left outer join rtcounty e on a.cutid2=e.cutid
left outer join rtfareastcmtyh f on a.comq1=f.comq1
left outer join rtcode g on a.secondidtype=g.code and g.kind='L3'
left outer join rtcode h on a.userrate=h.code and h.kind='R6'
where a.freecode<>'Y' and a.dropdat is null 
and a.canceldat is null and a.overduedat is null) A