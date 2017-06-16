<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
Sub SrEnvironment()
  company=application("company")
  system="HI-Building管理系統"
  title="施工轄區與縣市別關係維護"
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
  formatName="轄區代碼;none;轄區名稱;轄區類別;縣市名稱;生效日期;截止日期"
  sqlDelete="SELECT RTAreacty.AREAID,  RTAreaCty.CUTID,RTArea.AREANC, case areatype when '1' then '業務轄區' when '2' then '施工轄區' END AS AreaTypeC, " _
           &"RTCounty.CUTNC, RTAreaCty.TDAT, RTAreaCty.EXDAT " _
           &"FROM RTAreacty INNER JOIN " _
           &"RTArea ON RTAreacty.AREAID = RTArea.AREAID INNER JOIN " _
           &"RTCounty ON RTAreaCty.CUTID = RTCounty.CUTID WHERE rtarea.areatype='2' and RTareacty.areaid='*' "
  dataTable="rtareacty"
  numberOfKey=2
  dataProg="RTareaOprCtyDataList.asp"
  dataWindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature="width=640,height=460,scrollbars=yes"
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="rtareaOprsearch.asp"
  searchFirst=false
  If searchQry="" Then
     searchQry=" and RTareacty.areaid='" & aryparmkey(0) & "'"
     searchShow=FrGetAreaDesc(aryParmKey(0))       
  End If
  sqlList="SELECT RTAreacty.AREAID, RTAreaCty.CUTID,RTArea.AREANC,case areatype when '1' then '業務轄區' when '2' then '施工轄區' END AS AreaTypeC,  " _
           &"RTCounty.CUTNC, RTAreaCty.TDAT, RTAreaCty.EXDAT " _
           &"FROM RTAreacty INNER JOIN " _
           &"RTArea ON RTAreacty.AREAID = RTArea.AREAID INNER JOIN " _
           &"RTCounty ON RTAreaCty.CUTID = RTCounty.CUTID WHERE rtarea.areatype='2' " &searchqry
'Response.Write "SQL=" & sqllist
End Sub
%>
<!-- #include file="RTGetAreaDesc.inc" -->