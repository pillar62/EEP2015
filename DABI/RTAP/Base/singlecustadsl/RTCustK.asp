<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="ADSL客戶基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="通知技術;撤銷通知;客訴處理"
  functionOptProgram="RTSndInfo.asp;RTDropInfo.asp;RTFaqK.ASP"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;客戶名稱;申請單號;社區名稱;申請日;送件日期;完工日;裝機地址;聯絡電話"
   sqlDelete="SELECT SINGLECustADSL.CUSID, SINGLECustADSL.ENTRYNO, RTObj.SHORTNC,SINGLEcustadsl.orderno,SINGLEcustadsl.HOUSENAME, " _
         &"SINGLECustADSL.RCVD,SINGLECUSTADSL.DELIVERDAT, " _
         &"SINGLECUSTADSL.finishdat," _
         &"RTCOUNTY.CUTNC + SINGLECUSTADSL.TOWNSHIP2 + SINGLECUSTADSL.RADDR2, " _         
         &"SINGLECUSTADSL.HOME " _
         &"FROM SINGLECUSTADSL INNER JOIN " _
         &"RTObj ON SINGLECUSTADSL.CUSID = RTObj.CUSID LEFT OUTER JOIN " _
         &"RTCounty ON SINGLECUSTADSL.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RTCode RTCode1 ON SINGLECUSTADSL.ISP = RTCode1.CODE AND  " _
         &"RTCode1.KIND = 'C3' LEFT OUTER JOIN " _
         &"RTCode ON SINGLECUSTADSL.SETTYPE = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'A7' " _
         &"WHERE SINGLECUSTADSL.cusid='*' " _
         &"ORDER BY RTCOUNTY.CUTNC, SINGLECUSTADSL.TOWNSHIP2, SINGLECUSTADSL.RADDR2,rtobj.shortnc "
  dataTable="singlecustadsl"
  userDefineDelete=""
  extTable=""
  numberOfKey=2
  dataProg="RTCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=True
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=30
  searchProg="RTCustS.asp"
  searchFirst=false
  If searchQry="" Then
     searchShow="全部"
     searchQry="singlecustadsl.CUSID<>'*' "
  ELSE
     SEARCHFIRST=FALSE
  End If  
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'USERLEVEL=2業務人員(只能看到所屬業務組別資料)
  IF USERLEVEL = 2 THEN
  sqllist="SELECT  SINGLECUSTADSL.CUSID, SINGLECUSTADSL.ENTRYNO, RTObj.SHORTNC,SINGLECUSTADSL.orderno,SINGLECUSTADSL.HOUSENAME, " _
         &"SINGLECUSTADSL.RCVD,SINGLECUSTADSL.DELIVERDAT, " _
         &"SINGLECUSTADSL.finishdat," _
         &"RTCOUNTY.CUTNC + SINGLECUSTADSL.TOWNSHIP2 + SINGLECUSTADSL.RADDR2, " _         
         &"SINGLECUSTADSL.HOME " _
         &"FROM SINGLECUSTADSL INNER JOIN " _
         &"RTObj ON SINGLECUSTADSL.CUSID = RTObj.CUSID INNER JOIN " _
         &"RTSalesGroupREF ON " _
         &"SINGLECUSTADSL.BUSSID = RTSalesGroupREF.EMPLY LEFT OUTER JOIN " _
         &"RTCounty ON SINGLECUSTADSL.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RTCode RTCode1 ON SINGLECUSTADSL.ISP = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'C3' LEFT OUTER JOIN " _
         &"RTCode ON SINGLECUSTADSL.SETTYPE = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'A7' " _
         &"WHERE " & searchqry & " " & " AND " _
         &"(RTSalesGroupREF.AREAID + RTSalesGroupREF.GROUPID = " _
         &"(SELECT areaid + groupid " _
         &"FROM RTSalesGroupREF " _
         &"WHERE emply = '" &emply & "')) " _
         &"ORDER BY RTCounty.CUTNC, SINGLECUSTADSL.TOWNSHIP2, SINGLECUSTADSL.RADDR2, " _
         &"RTObj.SHORTNC "

  ELSE
  sqllist="SELECT  SINGLECUSTADSL.CUSID, SINGLECUSTADSL.ENTRYNO, RTObj.SHORTNC,SINGLECUSTADSL.orderno,SINGLECUSTADSL.HOUSENAME, " _
         &"SINGLECUSTADSL.RCVD,SINGLECUSTADSL.DELIVERDAT, " _
         &"SINGLECUSTADSL.finishdat," _
         &"RTCOUNTY.CUTNC + SINGLECUSTADSL.TOWNSHIP2 + SINGLECUSTADSL.RADDR2, " _         
         &"SINGLECUSTADSL.HOME " _
         &"FROM SINGLECUSTADSL INNER JOIN " _
         &"RTObj ON SINGLECUSTADSL.CUSID = RTObj.CUSID LEFT OUTER JOIN " _
         &"RTCounty ON SINGLECUSTADSL.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RTCode RTCode1 ON SINGLECUSTADSL.ISP = RTCode1.CODE AND  " _
         &"RTCode1.KIND = 'C3' LEFT OUTER JOIN " _
         &"RTCode ON SINGLECUSTADSL.SETTYPE = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'A7' " _
         &"WHERE " & searchqry & " " _
         &"ORDER BY RTCOUNTY.CUTNC, SINGLECUSTADSL.TOWNSHIP2, SINGLECUSTADSL.RADDR2,rtobj.shortnc "
   END IF
 ' Response.Write "sql=" & SQLLIST
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
