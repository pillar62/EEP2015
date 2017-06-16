<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="營運事業部業績目標"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & "Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=""
  functionOptProgram=""
  functionOptOpen="'"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="年度;月份;業務組別;產品名稱;目標戶數;實際戶數;達成率(%);組長姓名"
sqlDelete="SELECT RTTEAMGOAL.NYY , RTTEAMGOAL.NMM , RTSalesGroup.GROUPNC, RTProduct.PNAME, RTTEAMGOAL.GOAL, " _
         &"RTTEAMGOAL.ACTUAL,case when rtteamgoal.actual > 0 then round(RTTEAMGOAL.ACTUAL/RTTEAMGOAL.GOAL,2) * 100 else 0 end, RTObj.SHORTNC FROM RTObj RIGHT OUTER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID RIGHT OUTER JOIN " _
         &"RTTEAMGOAL LEFT OUTER JOIN RTProduct ON RTTEAMGOAL.PRODUCT = RTProduct.PID LEFT OUTER JOIN " _
         &"RTSalesGroup ON RTTEAMGOAL.AREAID = RTSalesGroup.AREAID AND " _
         &"RTTEAMGOAL.GROUPID = RTSalesGroup.GROUPID ON RTEmployee.EMPLY = RTTEAMGOAL.TEAMLEADER " _
         &" order by RTSalesGroup.GROUPNC,RTTEAMGOAL.NYY,RTTEAMGOAL.NMM   "
  dataTable="RTTeamGoal"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=5
  dataProg="RTTeamGoalD.asp"
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
  searchProg="RTTeamGoalS.asp"
  searchFirst=false
  If searchQry="" Then
     searchQry=" RTSalesGroup.GROUPNC<>'*' " 
     searchShow="全部"
  ELSE
     searchFirst=False
  End If
  sqllist="SELECT RTTEAMGOAL.NYY , RTTEAMGOAL.NMM , RTSalesGroup.GROUPNC, RTProduct.PNAME, RTTEAMGOAL.GOAL, " _
         &"RTTEAMGOAL.ACTUAL,case when rtteamgoal.actual > 0 then round(RTTEAMGOAL.ACTUAL/RTTEAMGOAL.GOAL,2) * 100 else 0 end , " _
         &"RTObj.SHORTNC FROM RTObj RIGHT OUTER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID RIGHT OUTER JOIN " _
         &"RTTEAMGOAL LEFT OUTER JOIN RTProduct ON RTTEAMGOAL.PRODUCT = RTProduct.PID LEFT OUTER JOIN " _
         &"RTSalesGroup ON RTTEAMGOAL.AREAID = RTSalesGroup.AREAID AND " _
         &"RTTEAMGOAL.GROUPID = RTSalesGroup.GROUPID ON RTEmployee.EMPLY = RTTEAMGOAL.TEAMLEADER " _
         &"where " & searchqry _
         &" order by RTSalesGroup.GROUPNC,RTTEAMGOAL.NYY,  RTTEAMGOAL.NMM "
 ' Response.Write sqllist
End Sub
%>
