<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
Sub SrEnvironment()
  company=application("company")
  system="HI-Building管理系統"
  title="對象類別資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  'functionOptName=""
  'functionOptProgram=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLIb"
  formatName="類別代碼;類別名稱"
  sqlDelete="SELECT CUSTYID,CUSTN FROM rtobjKind WHERE custyid='*'"
  dataTable="rtobjKind"
  numberOfKey=1
  dataProg="rtobjKindDataList.asp"
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
  colSplit=3
  keyListPageSize=60
  searchProg="self"
  If searchqry="" Then
     searchqry=" custyid<>'*'"
     searchshow="對象類別：全部" 
  End If
  sqlList="SELECT CUSTYID,CUSTN FROM rtobjKind WHERE " &searchqry
End Sub
%>