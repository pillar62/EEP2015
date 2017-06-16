<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
Sub SrEnvironment()
  company=application("company")
  system="HI-Building管理系統"
  title="施工轄區資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="縣市別關係"
  functionOptProgram="rtareaOprctyKeyList.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLIb"
  formatName="轄區代碼;轄區名稱;轄區類別;none;輸入人員;輸入日期;none;異動人員;異動日期"
  sqlDelete="SELECT RTArea.AREAID, RTArea.AREANC, " _
           &"CASE rtarea.areatype WHEN '1' THEN '業務轄區' WHEN '2' THEN '施工轄區' END AS AreaTypeC, " _
           &"RTArea.EUSR, RTObj.CUSNC, RTArea.EDAT, RTArea.UUSR, RTObj1.CUSNC, " _
           &"RTArea.UDAT " _
           &"FROM RTObj RIGHT OUTER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID RIGHT OUTER JOIN " _
           &"RTObj RTObj1 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee1 ON  " _
           &"RTObj1.CUSID = RTEmployee1.CUSID RIGHT OUTER JOIN " _
           &"RTArea ON RTEmployee1.EMPLY = RTArea.UUSR ON " _
           &"RTEmployee.EMPLY = RTArea.EUSR " _
           &"WHERE rtarea.areatype='2' and rtarea.areaid='*' "
  dataTable="rtarea"
  numberOfKey=1
  dataProg="rtareaOprDataList.asp"
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
  colSplit=1
  keyListPageSize=20
  searchProg="self"
  If searchQry="" Then
     searchQry=" and areaid<>'*' "
     searchShow="施工轄區：全部"
  End If  
  sqlList="SELECT RTArea.AREAID, RTArea.AREANC, " _
           &"CASE rtarea.areatype WHEN '1' THEN '業務轄區' WHEN '2' THEN '施工轄區' END AS AreaTypeC, " _
           &"RTArea.EUSR, RTObj.CUSNC, RTArea.EDAT, RTArea.UUSR, RTObj1.CUSNC, " _
           &"RTArea.UDAT " _
           &"FROM RTObj RIGHT OUTER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID RIGHT OUTER JOIN " _
           &"RTObj RTObj1 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee1 ON  " _
           &"RTObj1.CUSID = RTEmployee1.CUSID RIGHT OUTER JOIN " _
           &"RTArea ON RTEmployee1.EMPLY = RTArea.UUSR ON " _
           &"RTEmployee.EMPLY = RTArea.EUSR " _
           &"WHERE areatype='2'" 
End Sub
%>