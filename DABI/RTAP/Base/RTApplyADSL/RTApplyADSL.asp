<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="ADSL線上申請資料查詢"
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
  formatName="none;姓名;社區;性別;電話(H);電話(O);分機;手機;地址;申請種類;申請日;E-MAIL"
  sqlDelete="SELECT  RTApplyADSL.SERNO, RTApplyADSL.CUSNC,RTApplyADSL.HOUSENAME, " _
           &"CASE WHEN RTApplyADSL.SEX = 'M' THEN '男' ELSE '女' END AS SEXC, " _
           &"RTApplyADSL.HOME, RTApplyADSL.OFFICE, RTApplyADSL.EXTENSION, " _
           &"RTApplyADSL.MOBILE, RTCounty.CUTNC + RTApplyADSL.RADDR, " _
           &"CASE WHEN RTApplyADSL.APPLYTYPE = 'A1' THEN '分享型599' WHEN RTApplyADSL.APPLYTYPE " _
           &"= 'A2' THEN '分享型399' WHEN RTApplyADSL.APPLYTYPE = 'A3' THEN '標準型1199' " _
           &"ELSE '' END, RTApplyADSL.EDAT, RTApplyADSL.EMAIL " _
           &"FROM RTApplyADSL INNER JOIN " _
           &"RTCounty ON RTApplyADSL.COUNTY = RTCounty.CUTID " _
           &"where rtapplyadsl.cusnc='*' " _
           &"order by RTApplyADSL.EDAT desc" 
  dataTable="RTAPPLYADSL"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=1
  dataProg=""
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
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
  searchProg="self"
  searchFirst=TRUE
  If searchQry="" Then
     searchQry=" RTAPPLYADSL.CUSNC<>'*' "
     searchShow="全部"
  ELSE
     searchFirst=False
  End If
  sqllist="SELECT  RTApplyADSL.SERNO, RTApplyADSL.CUSNC,RTApplyADSL.HOUSENAME, " _
           &"CASE WHEN RTApplyADSL.SEX = 'M' THEN '男' ELSE '女' END AS SEXC, " _
           &"RTApplyADSL.HOME, RTApplyADSL.OFFICE, RTApplyADSL.EXTENSION, " _
           &"RTApplyADSL.MOBILE, RTCounty.CUTNC + RTApplyADSL.RADDR, " _
           &"CASE WHEN RTApplyADSL.APPLYTYPE = 'A1' THEN '分享型599' WHEN RTApplyADSL.APPLYTYPE " _
           &"= 'A2' THEN '分享型399' WHEN RTApplyADSL.APPLYTYPE = 'A3' THEN '標準型1199' " _
           &"ELSE '' END, RTApplyADSL.EDAT, RTApplyADSL.EMAIL " _
           &"FROM RTApplyADSL INNER JOIN " _
           &"RTCounty ON RTApplyADSL.COUNTY = RTCounty.CUTID order by RTApplyADSL.EDAT desc" 
'  Response.Write "sql=" & SQLLIST
End Sub
%>
