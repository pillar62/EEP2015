<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="ADSL已送件客戶資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="手動給號;單筆給號;整批給號;電訪記錄;DM單筆列印;DM整批列印"
  functionOptProgram="rtmanualassignno.asp;rtsingleassignno.asp;rtbatchassignno.asp;rttelvisitK.asp;verify2.ASP;verify.ASP;RTACCOUNTPRTDROP.ASP"
  functionOptOpen="2;1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;客戶<br>名稱;身份證號;單次;社區名稱;帳號;密碼;送件日;預定<br>完工日;完工日;到期日;可用<br>日數;批次代號;方案;列印批號"
sqlDelete="SELECT RTCustADSL.CUSID, RTCustADSL.ENTRYNO, RTObj.SHORTNC,rtcustadsl.socialid, rtcustadsl.entryno, RTCustADSL.HOUSENAME, " _
         &"RT365account.ss365ACCOUNT,RT365account.ss365PWD,RTCustADSL.DELIVERDAT, " _
         &"case when RTCUSTADSL.WORKINGREPLY IS NOT NULL and DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) < '2001/08/20' then '2001/08/20' " _
         &"WHEN RTCUSTADSL.WORKINGREPLY IS NOT NULL THEN DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) " _
         &"when RTCUSTADSL.CHTSIGNDAT IS NOT NULL and DateADD(dd, 14, RTCUSTADSL.chtsigndat) < '2001/08/20' then '2001/08/20' " _
         &"WHEN RTCUSTADSL.CHTSIGNDAT IS NOT NULL THEN DateADD(dd, 14, RTCUSTADSL.chtsigndat) " _
         &"when rtcustADSL.DELIVERDAT IS NOT NULL and DATEADD(mm, 1,rtcustadsl.deliverdat) < '2001/08/20' then '2001/08/20' " _
         &"when rtcustADSL.DELIVERDAT IS NOT NULL THEN DATEADD(mm, 1,rtcustadsl.deliverdat) end , " _
         &"rtcustadsl.finishdat, " _
         &"rt365account.deadline, " _
         &"case when rt365account.applydat is not null then DateDiff(day,getdate(),case when rt365account.type = '399' and rt365account.applydat is not null then dateadd(month,3,rt365account.applydat) " _
         &"when rt365account.type = '599' and rt365account.applydat is not null then dateadd(month,15,rt365account.applydat) " _
         &"when rt365account.type = '1199' and rt365account.applydat is not null then dateadd(month,24,rt365account.applydat) end) else 0 end ," _          
         &"rt365account.batchno," _
         &"case when rtcustadsl.ss365='' then '399' else '599' end, rt365account.prtno " _
         &"FROM RTCustADSL left outer JOIN RTCounty ON RTCustADSL.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RT365ACCOUNT ON RTCustADSL.CUSID = RT365ACCOUNT.CUSID AND " _
         &"RTCustADSL.ENTRYNO = RT365ACCOUNT.ENTRYNO LEFT OUTER JOIN " _
         &"RTObj ON RTCustADSL.CUSID = RTObj.CUSID " _
         &"WHERE (RTCustADSL.DELIVERDAT IS NOT NULL) AND " _
         &"(RTCustADSL.DROPDAT IS NULL) and " & searchqry _
         &" order by rtobj.shortnc  "
  dataTable="RTCust"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=2
  dataProg="/webap/rtap/base/rtcustadslbranch/RTCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=520,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=True
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="rtdelivercusts.asp"
  searchFirst=TRUE
  If searchQry="" Then
     searchQry=" RTCustadsl.CUSID='*' " 
     searchShow="全部"
  ELSE
     searchFirst=False
  End If
  sqllist="SELECT RTCustADSL.CUSID, RTCustADSL.ENTRYNO, RTObj.SHORTNC,rtcustadsl.socialid, rtcustadsl.entryno, RTCustADSL.HOUSENAME, " _
         &"RT365account.ss365ACCOUNT,RT365account.ss365PWD,RTCustADSL.DELIVERDAT, " _
         &"case when RTCUSTADSL.WORKINGREPLY IS NOT NULL and DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) < '2001/08/20' then '2001/08/20' " _
         &"WHEN RTCUSTADSL.WORKINGREPLY IS NOT NULL THEN DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) " _
         &"when RTCUSTADSL.CHTSIGNDAT IS NOT NULL and DateADD(dd, 14, RTCUSTADSL.chtsigndat) < '2001/08/20' then '2001/08/20' " _
         &"WHEN RTCUSTADSL.CHTSIGNDAT IS NOT NULL THEN DateADD(dd, 14, RTCUSTADSL.chtsigndat) " _
         &"when rtcustADSL.DELIVERDAT IS NOT NULL and DATEADD(mm, 1,rtcustadsl.deliverdat) < '2001/08/20' then '2001/08/20' " _
         &"when rtcustADSL.DELIVERDAT IS NOT NULL THEN DATEADD(mm, 1,rtcustadsl.deliverdat) end , " _
         &"rtcustadsl.finishdat, " _
         &"rt365account.deadline, " _
         &"case when rt365account.applydat is not null then DateDiff(day,getdate(),case when rt365account.type = '399' and rt365account.applydat is not null then dateadd(month,3,rt365account.applydat) " _
         &"when rt365account.type = '599' and rt365account.applydat is not null then dateadd(month,15,rt365account.applydat) " _
         &"when rt365account.type = '1199' and rt365account.applydat is not null then dateadd(month,24,rt365account.applydat) end) else 0 end, " _           
         &"rt365account.batchno," _
         &"case when rtcustadsl.ss365='' then '399' else '599' end, rt365account.prtno  " _
         &"FROM RTCustADSL left outer JOIN RTCounty ON RTCustADSL.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RT365ACCOUNT ON RTCustADSL.CUSID = RT365ACCOUNT.CUSID AND " _
         &"RTCustADSL.ENTRYNO = RT365ACCOUNT.ENTRYNO LEFT OUTER JOIN " _
         &"RTObj ON RTCustADSL.CUSID = RTObj.CUSID " _
         &"WHERE (RTCustADSL.DELIVERDAT IS NOT NULL) AND " _
         &"(RTCustADSL.DROPDAT IS NULL) and " & searchqry _
         &" order by RT365account.ss365ACCOUNT  "
  'SQllist399:為了計算符合條件之399筆數(未給號的筆數,,因sqllist可能含蓋已給號資料故須將之另行排除之       
  sqllist399="SELECT RTCustADSL.CUSID, RTCustADSL.ENTRYNO, RTObj.SHORTNC,rtcustadsl.socialid, rtcustadsl.entryno, RTCustADSL.HOUSENAME, " _
         &"RT365account.ss365ACCOUNT,RT365account.ss365PWD,RTCustADSL.DELIVERDAT, " _
         &"case when RTCUSTADSL.WORKINGREPLY IS NOT NULL and DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) < '2001/08/20' then '2001/08/20' " _
         &"WHEN RTCUSTADSL.WORKINGREPLY IS NOT NULL THEN DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) " _
         &"when RTCUSTADSL.CHTSIGNDAT IS NOT NULL and DateADD(dd, 14, RTCUSTADSL.chtsigndat) < '2001/08/20' then '2001/08/20' " _
         &"WHEN RTCUSTADSL.CHTSIGNDAT IS NOT NULL THEN DateADD(dd, 14, RTCUSTADSL.chtsigndat) " _
         &"when rtcustADSL.DELIVERDAT IS NOT NULL and DATEADD(mm, 1,rtcustadsl.deliverdat) < '2001/08/20' then '2001/08/20' " _
         &"when rtcustADSL.DELIVERDAT IS NOT NULL THEN DATEADD(mm, 1,rtcustadsl.deliverdat) end , " _
         &"rtcustadsl.finishdat, " _
         &"rt365account.deadline, " _
         &"Case when rt365account.applydat is not null then Datediff(mi,RT365account.applyDAT, getdate())/1440 else 0 end, " _         
         &"rt365account.batchno," _
         &"case when rtcustadsl.ss365='' then '399' else '599' end, RTCUSTADSL.SS365 " _
         &"FROM RTCustADSL left outer JOIN RTCounty ON RTCustADSL.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RT365ACCOUNT ON RTCustADSL.CUSID = RT365ACCOUNT.CUSID AND " _
         &"RTCustADSL.ENTRYNO = RT365ACCOUNT.ENTRYNO LEFT OUTER JOIN " _
         &"RTObj ON RTCustADSL.CUSID = RTObj.CUSID " _
         &"WHERE (RTCustADSL.DELIVERDAT IS NOT NULL) AND " _
         &"(RTCustADSL.DROPDAT IS NULL) and (rt365account.cusid is null or rt365account.cusid = '') and rtcustadsl.ss365='' and " & searchqry _
         &" order by RT365account.ss365ACCOUNT  "         
  'SQllist599:為了計算符合條件之399筆數(未給號的筆數,,因sqllist可能含蓋已給號資料故須將之另行排除之       
  sqllist599="SELECT RTCustADSL.CUSID, RTCustADSL.ENTRYNO, RTObj.SHORTNC,rtcustadsl.socialid, rtcustadsl.entryno, RTCustADSL.HOUSENAME, " _
         &"RT365account.ss365ACCOUNT,RT365account.ss365PWD,RTCustADSL.DELIVERDAT, " _
         &"case when RTCUSTADSL.WORKINGREPLY IS NOT NULL and DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) < '2001/08/20' then '2001/08/20' " _
         &"WHEN RTCUSTADSL.WORKINGREPLY IS NOT NULL THEN DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) " _
         &"when RTCUSTADSL.CHTSIGNDAT IS NOT NULL and DateADD(dd, 14, RTCUSTADSL.chtsigndat) < '2001/08/20' then '2001/08/20' " _
         &"WHEN RTCUSTADSL.CHTSIGNDAT IS NOT NULL THEN DateADD(dd, 14, RTCUSTADSL.chtsigndat) " _
         &"when rtcustADSL.DELIVERDAT IS NOT NULL and DATEADD(mm, 1,rtcustadsl.deliverdat) < '2001/08/20' then '2001/08/20' " _
         &"when rtcustADSL.DELIVERDAT IS NOT NULL THEN DATEADD(mm, 1,rtcustadsl.deliverdat) end , " _
         &"rtcustadsl.finishdat, " _
         &"rt365account.deadline, " _
         &"Case when rt365account.applydat is not null then Datediff(mi,RT365account.applyDAT, getdate())/1440 else 0 end, " _         
         &"rt365account.batchno," _
         &"case when rtcustadsl.ss365='' then '399' else '599' end, RTCUSTADSL.SS365  " _
         &"FROM RTCustADSL left outer JOIN RTCounty ON RTCustADSL.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RT365ACCOUNT ON RTCustADSL.CUSID = RT365ACCOUNT.CUSID AND " _
         &"RTCustADSL.ENTRYNO = RT365ACCOUNT.ENTRYNO LEFT OUTER JOIN " _
         &"RTObj ON RTCustADSL.CUSID = RTObj.CUSID " _
         &"WHERE (RTCustADSL.DELIVERDAT IS NOT NULL) AND " _
         &"(RTCustADSL.DROPDAT IS NULL) and (rt365account.cusid is null or rt365account.cusid = '') and rtcustadsl.ss365 <>'' and " & searchqry _
         &" order by RT365account.ss365ACCOUNT  "                  
  'SQllistX:為399及599全部資料內容      
  sqllistx="SELECT RTCustADSL.CUSID, RTCustADSL.ENTRYNO, RTObj.SHORTNC,rtcustadsl.socialid, rtcustadsl.entryno, RTCustADSL.HOUSENAME, " _
         &"RT365account.ss365ACCOUNT,RT365account.ss365PWD,RTCustADSL.DELIVERDAT, " _
         &"case when RTCUSTADSL.WORKINGREPLY IS NOT NULL and DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) < '2001/08/20' then '2001/08/20' " _
         &"WHEN RTCUSTADSL.WORKINGREPLY IS NOT NULL THEN DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) " _
         &"when RTCUSTADSL.CHTSIGNDAT IS NOT NULL and DateADD(dd, 14, RTCUSTADSL.chtsigndat) < '2001/08/20' then '2001/08/20' " _
         &"WHEN RTCUSTADSL.CHTSIGNDAT IS NOT NULL THEN DateADD(dd, 14, RTCUSTADSL.chtsigndat) " _
         &"when rtcustADSL.DELIVERDAT IS NOT NULL and DATEADD(mm, 1,rtcustadsl.deliverdat) < '2001/08/20' then '2001/08/20' " _
         &"when rtcustADSL.DELIVERDAT IS NOT NULL THEN DATEADD(mm, 1,rtcustadsl.deliverdat) end , " _
         &"rtcustadsl.finishdat, " _
         &"rt365account.deadline, " _
         &"Case when rt365account.applydat is not null then Datediff(mi,RT365account.applyDAT, getdate())/1440 else 0 end, " _         
         &"rt365account.batchno," _
         &"case when rtcustadsl.ss365='' then '399' else '599' end, RTCUSTADSL.SS365,usedate,batchno  " _
         &"FROM RTCustADSL left outer JOIN RTCounty ON RTCustADSL.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RT365ACCOUNT ON RTCustADSL.CUSID = RT365ACCOUNT.CUSID AND " _
         &"RTCustADSL.ENTRYNO = RT365ACCOUNT.ENTRYNO LEFT OUTER JOIN " _
         &"RTObj ON RTCustADSL.CUSID = RTObj.CUSID " _
         &"WHERE RTCustADSL.DELIVERDAT IS NOT NULL AND " _
         &"RTCustADSL.DROPDAT IS NULL and (rt365account.cusid is null or rt365account.cusid='')  and " & searchqry _
         &" order by rtobj.shortnc  "
'Response.Write sqllist
   sqlstring1="WHERE RTCustADSL.DELIVERDAT IS NOT NULL AND " _
         &"RTCustADSL.DROPDAT IS NULL and " & searchqry
   SQLSTRING2=" order by RT365account.ss365ACCOUNT"  
   sqlstring1=replace(sqlstring1,"'","""")
   sqlstring2=replace(sqlstring2,"'","""")   
 session("SQLlist399")=SQllist399
 session("SQLlist599")=SQllist599 
 session("SQLlistx")=SQllistx  
 session("sqlstring1")=sqlstring1
 session("sqlstring2")=sqlstring2
End Sub
%>
