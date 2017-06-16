<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
Sub SrEnvironment()
  company=application("company")
  system="HI-Building管理系統"
  title="鄉鎮區資料維護檔"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLIb"
  formatName="none;縣市名稱;鄉鎮區名稱"
  sqlDelete="SELECT a.cutid,b.cutnc,a.TownShip from RTCtyTown a,rtcounty b Where a.cutid*=b.cutid and Cutid='*'"
  dataTable="rtctyTown"
  numberOfKey=2
  dataProg="RTCtyTownDataList.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature="width=640,height=460,scrollbars=yes"
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=4
  keyListPageSize=80
  searchProg="rtctysearch.asp"
  If searchQry="" Then
     searchQry=" a.cutid='" & aryparmkey(0) & "'"
     searchShow=FrGetctyDesc(aryParmKey(0))       
  End If
  sqlList="SELECT a.cutid,b.cutnc,a.TownShip from RTCtyTown a,rtcounty b Where a.cutid=b.cutid and " &searchqry
End Sub
%>
<!-- #include file="RTGetCtyDesc.inc" -->