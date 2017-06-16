<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="元訊用戶回娘家抽獎活動(第一重)抽獎資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="作廢"
  functionOptProgram="HB2002ACT1DROP.ASP"
  functionOptPrompt="Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;用戶名稱;性別;公司電話;分機;住家電話;行動電話;縣市鄉鎮;申請日;E-MAIL;確認日;作廢日"
 ' sqlDelete="SELECT RTCust.COMQ1,RTCust.CUSID, RTCust.ENTRYNO, RTObj.shortnc, RTCust.CUSTYPE, " _
 '          &"RTCust.LINETYPE, RTCust.RCVD, RTCust.HOME," _
 '          &"RTCust.OFFICE + ' ' + RTCust.EXTENSION  AS Office,RTCust.SNDINFODAT ,rtcust.reqdat " _
 '          &"FROM RTCust INNER JOIN RTObj ON RTCust.CUSID = RTObj.CUSID " _
 '          &"WHERE RTCust.COMQ1=0 " _
 '          &"ORDER BY RTCust.CUSID, RTCust.ENTRYNO "
   sqlDelete="SELECT HB2002ACT1.SERNO, HB2002ACT1.NAME, " _
            &"CASE WHEN SEXC = 'M' THEN '男' WHEN SEXC = 'F' THEN '女' ELSE '' END AS " _
            &"SEX, HB2002ACT1.TELC, HB2002ACT1.EXT, HB2002ACT1.TELH, " _
            &"HB2002ACT1.MOBILE, " _
            &"RTCounty.CUTNC + HB2002ACT1.TOWNSHIP + HB2002ACT1.raddr AS EXpr3, " _
            &"CONVERT(varchar(10), HB2002ACT1.EDAT, 111) AS EDAT,HB2002ACT1.EMAIL, " _
            &"HB2002ACT1.cfmDAT,HB2002ACT1.dropdAT  " _
            &"FROM HB2002ACT1 INNER JOIN " _
            &"RTCounty ON HB2002ACT1.CUTID = RTCounty.CUTID " _
            &"ORDER BY  HB2002ACT1.EDAT, HB2002ACT1.NAME " 
  dataTable="HB2002ACT1"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=1
  dataProg="/webap/rtap/ACT2002/ACT1/RTCustd.asp"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
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
  searchProg="rtcusts.asp"
  searchFirst=TRUE
  If searchQry="" Then
     searchQry=" HB2002ACT1.SERNO<>0 "
     searchShow="全部"
  ELSE
     searchFirst=False
  End If
  sqllist="SELECT HB2002ACT1.SERNO, HB2002ACT1.NAME, " _
            &"CASE WHEN SEXC = 'M' THEN '男' WHEN SEXC = 'F' THEN '女' ELSE '' END AS " _
            &"SEX, HB2002ACT1.TELC, HB2002ACT1.EXT, HB2002ACT1.TELH, " _
            &"HB2002ACT1.MOBILE, " _
            &"RTCounty.CUTNC + HB2002ACT1.TOWNSHIP + HB2002ACT1.raddr AS Expr3, " _
            &"CONVERT(varchar(10), HB2002ACT1.EDAT, 111) AS EDAT,HB2002ACT1.EMAIL,  " _
            &"HB2002ACT1.cfmDAT,HB2002ACT1.dropdAT " _
            &"FROM HB2002ACT1 INNER JOIN " _
            &"RTCounty ON HB2002ACT1.CUTID = RTCounty.CUTID " _
            &"WHERE " & SearchQry & " " _
            &"ORDER BY  HB2002ACT1.EDAT, HB2002ACT1.NAME " 
 ' Response.Write "sql=" & SQLLIST
End Sub
%>
