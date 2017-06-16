<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="社區整線派工作業維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  

  '國霖、惠忠為審核人員
  IF EMPLY="T89015" or EMPLY="T90055" THEN
     functionOptName=" 列 印 ;審核確認;審核返轉; 作 廢 ;作廢返轉;設備維護;歷史異動"
     functionOptProgram="HBcmtyarrangesndworkpv.ASP;HBcmtyarrangesndworkCFMF.ASP;HBcmtyarrangesndworkCFMFR.ASP;HBcmtyarrangesndworkDROP.ASP;HBcmtyarrangesndworkDROPC.ASP;HBcmtyarrangeHardwareK.ASP;HBcmtyarrangesndworkLOGK.ASP"
     functionOptPrompt="N;Y;Y;Y;Y;N;N"
  ELSE
     functionOptName=" 列 印 ; 結 案 ;結案返轉; 作 廢 ;作廢返轉;設備維護;歷史異動"
     functionOptProgram="HBcmtyarrangesndworkpv.ASP;HBcmtyarrangesndworkF.ASP;HBcmtyarrangesndworkFR.ASP;HBcmtyarrangesndworkDROP.ASP;HBcmtyarrangesndworkDROPC.ASP;HBcmtyarrangeHardwareK.ASP;HBcmtyarrangesndworkLOGK.ASP"
     functionOptPrompt="N;Y;Y;Y;Y;N;N"
  END IF
  if userlevel=31 THEN
     functionOptName=" 列 印 ; 結 案 ;結案返轉;審核確認;審核返轉; 作 廢 ;作廢返轉;設備維護;歷史異動"
     functionOptProgram="HBcmtyarrangesndworkpv.ASP;HBcmtyarrangesndworkF.ASP;HBcmtyarrangesndworkFR.ASP;HBcmtyarrangesndworkCFMF.ASP;HBcmtyarrangesndworkCFMFR.ASP;HBcmtyarrangesndworkDROP.ASP;HBcmtyarrangesndworkDROPC.ASP;HBcmtyarrangeHardwareK.ASP;HBcmtyarrangesndworkLOGK.ASP"
     functionOptPrompt="N;Y;Y;Y;Y;Y;Y;N;N"
  END IF
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;整線單號;社區名稱;社區方案;派工日期;預計整線人員;實際整線人員;作廢日;結案日;審核日" 
  sqlDelete="SELECT  HBCMTYARRANGESNDWORK.COMQ1, HBCMTYARRANGESNDWORK.COMTYPE, HBCMTYARRANGESNDWORK.PRTNO, RTCmty.COMN AS COMNXX, " _
         &"'Hi-Building', HBCMTYARRANGESNDWORK.SNDDAT, RTObj.CUSNC, RTObj_1.SHORTNC,RTObj_2.CUSNC , RTObj_3.SHORTNC AS Expr2, " _
         &"HBCMTYARRANGESNDWORK.DROPDAT,  HBCMTYARRANGESNDWORK.CLOSEDAT,  HBCMTYARRANGESNDWORK.AUDITDAT " _
         &"FROM RTObj RTObj_3 RIGHT OUTER JOIN HBCMTYARRANGESNDWORK INNER JOIN RTCmty ON HBCMTYARRANGESNDWORK.COMQ1 = RTCmty.COMQ1 ON " _
         &"RTObj_3.CUSID = HBCMTYARRANGESNDWORK.REALCONSIGNEE LEFT OUTER JOIN  RTEmployee RTEmployee_1 INNER JOIN " _
         &"RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON HBCMTYARRANGESNDWORK.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
         &"RTObj RTObj_1 ON HBCMTYARRANGESNDWORK.ASSIGNCONSIGNEE = RTObj_1.CUSID LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON HBCMTYARRANGESNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY " _
         &"where  HBCMTYARRANGESNDWORK.COMTYPE in ('1','4') " _
         &"union " _
         &"SELECT HBCMTYARRANGESNDWORK.COMQ1, HBCMTYARRANGESNDWORK.COMTYPE, HBCMTYARRANGESNDWORK.PRTNO,RTcustadslCmty.COMN AS COMNXX,'399A案', " _
         &"HBCMTYARRANGESNDWORK.SNDDAT, RTObj.CUSNC, RTObj_1.SHORTNC, RTObj_2.CUSNC AS Expr1, RTObj_3.SHORTNC AS Expr2, " _
         &"HBCMTYARRANGESNDWORK.DROPDAT, HBCMTYARRANGESNDWORK.CLOSEDAT,  HBCMTYARRANGESNDWORK.AUDITDAT FROM RTObj RTObj_3 RIGHT OUTER JOIN " _
         &"HBCMTYARRANGESNDWORK INNER JOIN RTcustadslCmty ON HBCMTYARRANGESNDWORK.COMQ1 = RTcustadslCmty.cutyid ON " _
         &"RTObj_3.CUSID = HBCMTYARRANGESNDWORK.REALCONSIGNEE LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
         &"RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON HBCMTYARRANGESNDWORK.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
         &"RTObj RTObj_1 ON HBCMTYARRANGESNDWORK.ASSIGNCONSIGNEE = RTObj_1.CUSID LEFT OUTER JOIN RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON HBCMTYARRANGESNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY " _
         &"where  HBCMTYARRANGESNDWORK.COMTYPE='2' " _
         &"union " _
         &"SELECT HBCMTYARRANGESNDWORK.COMQ1, HBCMTYARRANGESNDWORK.COMTYPE,HBCMTYARRANGESNDWORK.PRTNO,RTsparqadslCmty.COMN AS COMNXX,'399B案', " _
         &"HBCMTYARRANGESNDWORK.SNDDAT,RTObj.CUSNC,RTObj_1.SHORTNC,RTObj_2.CUSNC AS Expr1,RTObj_3.SHORTNC AS Expr2,HBCMTYARRANGESNDWORK.DROPDAT, " _ 
         &"HBCMTYARRANGESNDWORK.CLOSEDAT,  HBCMTYARRANGESNDWORK.AUDITDAT FROM RTObj RTObj_3 RIGHT OUTER JOIN HBCMTYARRANGESNDWORK INNER JOIN RTsparqadslCmty " _
         &"ON HBCMTYARRANGESNDWORK.COMQ1 = RTsparqadslCmty.cutyid ON RTObj_3.CUSID=HBCMTYARRANGESNDWORK.REALCONSIGNEE LEFT OUTER JOIN " _
         &"RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON " _
         &"HBCMTYARRANGESNDWORK.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_1 ON " _
         &"HBCMTYARRANGESNDWORK.ASSIGNCONSIGNEE = RTObj_1.CUSID LEFT OUTER JOIN RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON HBCMTYARRANGESNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY " _
         &"where  HBCMTYARRANGESNDWORK.COMTYPE='3' " _
         &"ORDER BY COMNXX "
  dataTable="HBCMTYARRANGESNDWORK"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="HBcmtyarrangesndworkD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="HBcmtyarrangesndworkS.asp"
' Open search program when first entry this keylist
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=false
  If searchQry="" Then
    ' searchQry=" RTCUSTADSLCMTY.CUTYID<>0 and rtcustadsl.dropdat is null and rtcustadsl.agree <>'N' "
     searchQry=" and HBCMTYARRANGESNDWORK.COMQ1=" & aryparmkey(0) & " and HBCMTYARRANGESNDWORK.comtype='" & aryparmkey(2) & "' "
    ' searchShow="全部(不含退租、撤銷、不可建置戶)"
     searchQry2=" "
    searchShow="全部" 
  ELSE
     SEARCHFIRST=FALSE
  End If
 
  sqllist="SELECT  HBCMTYARRANGESNDWORK.COMQ1, HBCMTYARRANGESNDWORK.COMTYPE, HBCMTYARRANGESNDWORK.PRTNO, RTCmty.COMN AS COMNXX, " _
         &"'Hi-Building', HBCMTYARRANGESNDWORK.SNDDAT, case when RTObj.cusnc is null then RTObj_1.SHORTNC else RTObj.cusnc end ,case when RTObj_2.CUSNC is null then RTObj_3.SHORTNC  else RTObj_2.CUSNC end , " _
         &"HBCMTYARRANGESNDWORK.DROPDAT,  HBCMTYARRANGESNDWORK.CLOSEDAT,  HBCMTYARRANGESNDWORK.AUDITDAT " _
         &"FROM RTObj RTObj_3 RIGHT OUTER JOIN HBCMTYARRANGESNDWORK INNER JOIN RTCmty ON HBCMTYARRANGESNDWORK.COMQ1 = RTCmty.COMQ1 ON " _
         &"RTObj_3.CUSID = HBCMTYARRANGESNDWORK.REALCONSIGNEE LEFT OUTER JOIN  RTEmployee RTEmployee_1 INNER JOIN " _
         &"RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON HBCMTYARRANGESNDWORK.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
         &"RTObj RTObj_1 ON HBCMTYARRANGESNDWORK.ASSIGNCONSIGNEE = RTObj_1.CUSID LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON HBCMTYARRANGESNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY " _
         &"where  HBCMTYARRANGESNDWORK.COMTYPE in ('1','4') " & searchqry  _
         &"union " _
         &"SELECT HBCMTYARRANGESNDWORK.COMQ1, HBCMTYARRANGESNDWORK.COMTYPE, HBCMTYARRANGESNDWORK.PRTNO,RTcustadslCmty.COMN AS COMNXX,'399A案(中華399)', " _
         &"HBCMTYARRANGESNDWORK.SNDDAT, case when RTObj.cusnc is null then RTObj_1.SHORTNC else RTObj.cusnc end ,case when RTObj_2.CUSNC is null then RTObj_3.SHORTNC else RTObj_2.CUSNC end , " _
         &"HBCMTYARRANGESNDWORK.DROPDAT, HBCMTYARRANGESNDWORK.CLOSEDAT,  HBCMTYARRANGESNDWORK.AUDITDAT FROM RTObj RTObj_3 RIGHT OUTER JOIN " _
         &"HBCMTYARRANGESNDWORK INNER JOIN RTcustadslCmty ON HBCMTYARRANGESNDWORK.COMQ1 = RTcustadslCmty.cutyid ON " _
         &"RTObj_3.CUSID = HBCMTYARRANGESNDWORK.REALCONSIGNEE LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
         &"RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON HBCMTYARRANGESNDWORK.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
         &"RTObj RTObj_1 ON HBCMTYARRANGESNDWORK.ASSIGNCONSIGNEE = RTObj_1.CUSID LEFT OUTER JOIN RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON HBCMTYARRANGESNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY " _
         &"where  HBCMTYARRANGESNDWORK.COMTYPE='2' " & searchqry  _
         &"union " _
         &"SELECT HBCMTYARRANGESNDWORK.COMQ1, HBCMTYARRANGESNDWORK.COMTYPE,HBCMTYARRANGESNDWORK.PRTNO,RTsparqadslCmty.COMN AS COMNXX,'399B案(速博399)', " _
         &"HBCMTYARRANGESNDWORK.SNDDAT, case when RTObj.cusnc is null then RTObj_1.SHORTNC else RTObj.cusnc end ,case when RTObj_2.CUSNC is null then RTObj_3.SHORTNC else RTObj_2.CUSNC end , " _
         &"HBCMTYARRANGESNDWORK.DROPDAT, " _ 
         &"HBCMTYARRANGESNDWORK.CLOSEDAT,  HBCMTYARRANGESNDWORK.AUDITDAT FROM RTObj RTObj_3 RIGHT OUTER JOIN HBCMTYARRANGESNDWORK INNER JOIN RTsparqadslCmty " _
         &"ON HBCMTYARRANGESNDWORK.COMQ1 = RTsparqadslCmty.cutyid ON RTObj_3.CUSID=HBCMTYARRANGESNDWORK.REALCONSIGNEE LEFT OUTER JOIN " _
         &"RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON " _
         &"HBCMTYARRANGESNDWORK.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_1 ON " _
         &"HBCMTYARRANGESNDWORK.ASSIGNCONSIGNEE = RTObj_1.CUSID LEFT OUTER JOIN RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON HBCMTYARRANGESNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY " _
         &"where  HBCMTYARRANGESNDWORK.COMTYPE='3' " &  searchqry  _
         &"union " _
         &"SELECT HBCMTYARRANGESNDWORK.COMQ1, HBCMTYARRANGESNDWORK.COMTYPE,HBCMTYARRANGESNDWORK.PRTNO,RTEBTCMTYH.COMN AS COMNXX,'東森AVS499', " _
         &"HBCMTYARRANGESNDWORK.SNDDAT, case when RTObj.cusnc is null then RTObj_1.SHORTNC else RTObj.cusnc end ,case when RTObj_2.CUSNC is null then RTObj_3.SHORTNC else RTObj_2.CUSNC end , " _
         &"HBCMTYARRANGESNDWORK.DROPDAT, " _ 
         &"HBCMTYARRANGESNDWORK.CLOSEDAT,  HBCMTYARRANGESNDWORK.AUDITDAT FROM RTObj RTObj_3 RIGHT OUTER JOIN HBCMTYARRANGESNDWORK INNER JOIN RTEBTCMTYH " _
         &"ON HBCMTYARRANGESNDWORK.COMQ1 = RTEBTCMTYH.COMQ1 ON RTObj_3.CUSID=HBCMTYARRANGESNDWORK.REALCONSIGNEE LEFT OUTER JOIN " _
         &"RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON " _
         &"HBCMTYARRANGESNDWORK.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_1 ON " _
         &"HBCMTYARRANGESNDWORK.ASSIGNCONSIGNEE = RTObj_1.CUSID LEFT OUTER JOIN RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON HBCMTYARRANGESNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY " _
         &"where  HBCMTYARRANGESNDWORK.COMTYPE='5' " &  searchqry  _
         &"ORDER BY COMNXX "
         
         SESSION("LINEQ1")=ARYPARMKEY(1)

'response.Write SQLLIST
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>