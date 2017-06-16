<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Sub SrEnvironment()
  company=application("company")
  system="HI-Building管理系統"
  title="業務轄區與鄉鎮市區關係維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLIb"
  formatName="none;none;none;none;縣市名稱;鄉鎮市區;生效日期;截止日期;區域別"
  sqlDelete="SELECT RTAreatownship.AREAID,RTAreatownship.groupID, RTAreatownship.CUTID,RTAreatownship.township, " _
           &"RTCounty.CUTNC,RTAreatownship.township, RTAreatownship.TDAT, RTAreatownship.EXDAT,case when RTAreatownship.distancecode='1' then '本縣市' when RTAreatownship.distancecode='2' then '外縣市' else '' end  " _
           &"FROM RTAreatownship INNER JOIN " _
           &"RTArea ON RTAreatownship.AREAID = RTArea.AREAID INNER JOIN " _
           &"RTCounty ON RTAreatownship.CUTID = RTCounty.CUTID WHERE rtarea.areatype='1' and RTareatownship.areaid='*' "
  dataTable="rtareaTOWNSHIP"
  numberOfKey=4
  dataProg="RTareatownshipD.asp"
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
  colSplit=2
  keyListPageSize=40
  searchProg="self"
  searchFirst=False
  If searchQry="" Then
     searchQry=" and RTareatownship.areaid='" & aryparmkey(0) & "' and rtareatownship.groupid='" & aryparmkey(1) & "'"
     searchShow=FrGetAreatownshipDesc(aryParmKey(0),aryparmkey(1),aryparmkey(2))       
  End If
  sqlList="SELECT RTAreatownship.AREAID,RTAreatownship.groupid, RTAreatownship.CUTID,RTAreatownship.township, " _
           &"RTCounty.CUTNC,RTAreatownship.township, RTAreatownship.TDAT, RTAreatownship.EXDAT,case when RTAreatownship.distancecode='1' then '本縣市' when RTAreatownship.distancecode='2' then '外縣市' else '' end   " _
           &"FROM RTAreatownship INNER JOIN " _
           &"RTArea ON RTAreatownship.AREAID = RTArea.AREAID INNER JOIN " _
           &"RTCounty ON RTAreatownship.CUTID = RTCounty.CUTID WHERE rtarea.areatype='1' " &searchqry
End Sub
%>
<!-- #include file="RTGetAreatownshipDesc.inc" -->