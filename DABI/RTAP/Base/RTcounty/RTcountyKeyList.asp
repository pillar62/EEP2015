<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
Sub SrEnvironment()
  company=application("company")
  system="HI-Building管理系統"
  title="縣市代碼資料檔維護"
  buttonName=" 新增 ; 刪除 ; 結束 ; 重新整理 ; 頁數 ; 功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="鄉鎮區明細"
  functionOptProgram="RTCtyTownKeylist.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLIb"
  formatName="縣市代碼;縣市名稱"
  sqlDelete="Select CUTID,CUTNC from RTCounty WHERE cutid='*' "
  dataTable="rtcounty"
  numberOfKey=1
  dataProg="rtcountyDataList.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature="width=640,height=460,scrollbars=yes"
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=True
  goodMorningImage="cbbn.jpg"
  colSplit=2
  keyListPageSize=30
  searchProg="self"
  If searchQry="" Then
     searchQry=" cutid <> '*'"
     searchshow="縣市：全部"
  End If
  sqlList="Select CUTID,CUTNC from RTCounty where " &searchqry
End Sub
%>