<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="財訊先看先贏帳號資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=""
  functionOptProgram=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="帳號;密碼;帳號期<BR>限(月);客戶身份證;客戶名稱;單次;給號日期;帳號開通日;帳號<br>類型;帳號註銷日;帳號失效日;距離<br>天數"
sqlDelete="SELECT RT365ACCOUNT.SS365ACCOUNT, RT365ACCOUNT.SS365PWD, "_
         &"RT365ACCOUNT.ACCOUNTLIFE, ISNULL(RTCustADSL.SOCIALID, '') , " _ 
         &"ISNULL(RTObj.SHORTNC, '') , ISNULL(RT365ACCOUNT.ENTRYNO, '') " _
         &",RT365ACCOUNT.USEDAT,rt365account.applydat, " _ 
         &"RT365ACCOUNT.TYPE, RT365ACCOUNT.DROPDAT, " _
         &"rt365account.deadline, " _
         &"case when rt365account.applydat is not null then DateDiff(day,getdate(),case when rt365account.type = '399' and rt365account.applydat is not null then dateadd(month,3,rt365account.applydat) " _
         &"when rt365account.type = '599' and rt365account.applydat is not null then dateadd(month,15,rt365account.applydat) " _
         &"when rt365account.type = '1199' and rt365account.applydat is not null then dateadd(month,24,rt365account.applydat) end) else 0 end " _                  
         &"FROM RT365ACCOUNT LEFT OUTER JOIN " _
         &"RTCustADSL ON RT365ACCOUNT.CUSID = RTCustADSL.CUSID AND " _
         &"RT365ACCOUNT.ENTRYNO = RTCustADSL.ENTRYNO LEFT OUTER JOIN " _
         &"RTObj ON RT365ACCOUNT.CUSID = RTObj.CUSID " _
         &"WHERE RT365ACCOUNT.SS365ACCOUNT = '*' "
  dataTable="RT365account"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=1
  dataProg="RT365accountD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature="width=400,height=200,scrollbars=yes"
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=True
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="rt365accounts.asp"
  searchFirst=TRUE
  If searchQry="" Then
     searchQry=" and RT365account.ss365account='*' "
     searchShow="全部"
  ELSE
     searchFirst=False
  End If
  sqllist="SELECT RT365ACCOUNT.SS365ACCOUNT, RT365ACCOUNT.SS365PWD, "_
         &"RT365ACCOUNT.ACCOUNTLIFE, ISNULL(RTCustADSL.SOCIALID, '') , " _ 
         &"ISNULL(RTObj.SHORTNC, ''), ISNULL(RT365ACCOUNT.ENTRYNO, '') " _
         &",RT365ACCOUNT.USEDATE ,rt365account.applydat, " _ 
         &"RT365ACCOUNT.TYPE, RT365ACCOUNT.DROPDAT, " _
         &"rt365account.deadline, " _
         &"case when rt365account.applydat is not null then DateDiff(day,getdate(),case when rt365account.type = '399' and rt365account.applydat is not null then dateadd(month,3,rt365account.applydat) " _
         &"when rt365account.type = '599' and rt365account.applydat is not null then dateadd(month,15,rt365account.applydat) " _
         &"when rt365account.type = '1199' and rt365account.applydat is not null then dateadd(month,24,rt365account.applydat) end) else 0 end " _         
         &"FROM RT365ACCOUNT LEFT OUTER JOIN " _
         &"RTCustADSL ON RT365ACCOUNT.CUSID = RTCustADSL.CUSID AND " _
         &"RT365ACCOUNT.ENTRYNO = RTCustADSL.ENTRYNO LEFT OUTER JOIN " _
         &"RTObj ON RT365ACCOUNT.CUSID = RTObj.CUSID " _
         &"WHERE rt365account.ss365account <> '*' " & searchqry _
         &" order by rt365account.ss365account  "
' Response.Write "sql=" & SQLLIST
End Sub
%>
