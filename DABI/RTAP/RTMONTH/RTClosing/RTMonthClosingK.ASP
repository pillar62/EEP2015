<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="業績獎金月結控制檔維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";Y;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="年度;月份;none;轄區;月結;最後月結日;最後月結人員;最後回復日;最後回復人員;月結總次數;執行鎖定 " 
  sqlDelete="SELECT RTClosingCtl.CYY, RTClosingCtl.CMM,RTClosingCtl.AREAID, RTArea.AREANC, RTClosingCtl.CLOSING, " _
           &"RTClosingCtl.LASTRUNDAT, RTObj.CUSNC, RTClosingCtl.LASTRSTDAT, RTObj1.CUSNC, " _
           &"RTClosingCtl.RUNCNT, RTClosingCtl.RUNLOCK " _
           &"FROM RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID RIGHT OUTER JOIN " _
           &"RTArea INNER JOIN " _
           &"RTClosingCtl ON RTArea.AREAID = RTClosingCtl.AREAID ON " _
           &"RTArea.AREATYPE = '1' AND " _
           &"RTEmployee.EMPLY = RTClosingCtl.LASTRUNUSR LEFT OUTER JOIN " _
           &"RTObj RTObj1 INNER JOIN " _
           &"RTEmployee RTEmployee1 ON RTObj1.CUSID = RTEmployee1.CUSID ON " _
           &"RTClosingCtl.LASTRSTUSR = RTEmployee1.EMPLY " _
           &"WHERE RTClosingCtl.CYY = 0 " 
  dataTable="RTClosingCtl"
  userDefineDelete=""
  numberOfKey=3
  dataProg="RTMonthClosingD.asp"
  datawindowFeature=""
  searchWindowFeature="width=400,height=300,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=true
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="RTMonthClosingS.asp"
  parmkey=""
First=False
' When first time enter this keylist default query string to RTcmty.CASENO <> '*'
  searchFirst=False
  If searchQry="" Then
     searchQry=" RTClosingCtl.CYY <> 0 " 
     searchShow="全部"
  End If
  sqlList="SELECT RTClosingCtl.CYY, RTClosingCtl.CMM,RTClosingCtl.AREAID, RTArea.AREANC, RTClosingCtl.CLOSING, " _
           &"RTClosingCtl.LASTRUNDAT, RTObj.CUSNC, RTClosingCtl.LASTRSTDAT, RTObj1.CUSNC, " _
           &"RTClosingCtl.RUNCNT, RTClosingCtl.RUNLOCK " _
           &"FROM RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID RIGHT OUTER JOIN " _
           &"RTArea INNER JOIN " _
           &"RTClosingCtl ON RTArea.AREAID = RTClosingCtl.AREAID ON " _
           &"RTArea.AREATYPE = '1' AND " _
           &"RTEmployee.EMPLY = RTClosingCtl.LASTRUNUSR LEFT OUTER JOIN " _
           &"RTObj RTObj1 INNER JOIN " _
           &"RTEmployee RTEmployee1 ON RTObj1.CUSID = RTEmployee1.CUSID ON " _
           &"RTClosingCtl.LASTRSTUSR = RTEmployee1.EMPLY " _
           &"WHERE  " &searchQry &" " _
           &"ORDER BY RTClosingCtl.CYY,RTClosingCtl.CMM"
 'Response.Write "SQL=" & SQLlist
End Sub

%>
