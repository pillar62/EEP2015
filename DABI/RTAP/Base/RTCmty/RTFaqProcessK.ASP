<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="社區(客戶)客訴處理措施紀錄"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="作廢;作廢反轉"
  functionOptProgram="RTFaqProcessDropk.asp;RTFaqProcessDropRTNk.asp"
  functionOptPrompt="N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="案件編號;項次;處理日期;處理措施;處理員工;處理廠商;作廢日期;作廢人員" 
  sqlDelete="SELECT RTFAQD1.CASENO AS Expr1, RTFAQD1.ENTRYNO AS Expr2, " _
           &"RTFAQD1.LOGDATE AS Expr3, left(RTFAQD1.LOGDESC,20), RTObj.CUSNC AS Expr4, RTObj1.SHORTNC AS Expr5, " _
           &"RTFAQD1.LOGDROPDATE AS Expr6, RTObj2.CUSNC AS Expr7 " _
           &"FROM RTFAQD1 INNER JOIN " _
           &"RTEmployee ON RTFAQD1.LOGUSR = RTEmployee.EMPLY INNER JOIN " _
           &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
           &"RTObj RTObj1 ON RTFAQD1.LOGFAC = RTObj1.CUSID INNER JOIN " _
           &"RTEmployee RTEmployee1 ON " _
           &"RTFAQD1.LOGDROPUSR = RTEmployee1.EMPLY INNER JOIN " _
           &"RTObj RTObj2 ON RTEmployee1.CUSID = RTObj2.CUSID " _
           &"WHERE RTFAQD1.CASENO='*' "
  dataTable="RTFAQD1"
  userDefineDelete=""
  numberOfKey=2
  dataProg="RTFAQProcessD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=FALSE
  goodMorningImage="cbbn.GIF"
  colSplit=1
  keyListPageSize=20
  searchProg="self"
  First=False
' When first time enter this keylist default query string to RTcmty.CASENO <> '*'
  searchFirst=False
  If searchQry="" Then
     searchQry=" RTFAQD1.CASENO='" & aryparmkey(0) & "'"
     searchShow="客訴單號：" & aryParmKey(0)     
   '  searchShow="全部"
  End If
  sqlList="SELECT RTFAQD1.CASENO AS Expr1, RTFAQD1.ENTRYNO AS Expr2, " _
         &"RTFAQD1.LOGDATE AS Expr3, left(RTFAQD1.LOGDESC,20), RTObj.CUSNC AS Expr4, RTObj1.SHORTNC AS Expr5, " _
         &"RTFAQD1.LOGDROPDATE AS Expr6, RTObj2.CUSNC AS Expr7 " _
         &"FROM RTObj RTObj2 INNER JOIN " _
         &"RTEmployee RTEmployee1 ON  " _
         &"RTObj2.CUSID = RTEmployee1.CUSID RIGHT OUTER JOIN " _
         &"RTObj RTObj1 RIGHT OUTER JOIN " _
         &"RTFAQD1 ON RTObj1.CUSID = RTFAQD1.LOGFAC ON  " _
         &"RTEmployee1.EMPLY = RTFAQD1.LOGDROPUSR LEFT OUTER JOIN " _
         &"RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
         &"RTFAQD1.LOGUSR = RTEmployee.EMPLY " _
         &"WHERE RTFAQD1.CASENO<>'*' AND "&searchQry &" " _
         &"ORDER BY RTFAQD1.CASENO,RTFAQD1.ENTRYNO"
' Response.Write "SQL=" & SQLlist
End Sub

%>
