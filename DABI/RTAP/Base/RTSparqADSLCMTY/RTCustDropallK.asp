<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Sparq* 管理系統"
  title="速博ADSL退租戶拆機派工作業"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="拆機派工"
  functionOptProgram="RTsparqadslcustdropsndworkk.asp"
  functionOptPrompt ="N"
  functionoptopen   ="1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;社區名稱;用戶名稱;異動別;異動日;異動報竣日;異動轉檔日;拆機單號;派拆機日;預定拆機人員"
   sqlDelete="SELECT RTSparqAdslChg.CUSID, RTSparqAdslChg.ENTRYNO, RTSparqAdslChg.COMQ1,RTSPARQADSLCMTY.COMN,RTOBJ.CUSNC, RTCode.CODENC, " _
            &"RTSparqAdslChg.MODIFYDAT, RTSparqAdslChg.DOCKETDAT, RTSparqAdslChg.TRANSDAT,RTSparqadslcustdropsndwork.PRTNO, RTSparqadslcustdropsndwork.SENDWORKDAT, " _
            &"CASE WHEN rtobj_1.cusnc <> '' THEN rtobj_1.cusnc ELSE rtobj_2.shortnc END " _
            &"FROM  RTObj RTObj_2 RIGHT OUTER JOIN RTSparqadslcustdropsndwork ON " _
            &"RTObj_2.CUSID = RTSparqadslcustdropsndwork.ASSIGNCONSIGNEE LEFT OUTER JOIN RTEmployee LEFT OUTER JOIN " _
            &"RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID ON " _
            &"RTSparqadslcustdropsndwork.ASSIGNENGINEER = RTEmployee.EMPLY RIGHT OUTER JOIN RTSparqAdslChg INNER JOIN " _
            &"RTCode ON RTSparqAdslChg.MODIFYCODE = RTCode.CODE AND RTCode.KIND = 'K1' LEFT OUTER JOIN " _
            &"RTSparqAdslCmty ON RTSparqAdslChg.COMQ1 = RTSparqAdslCmty.CUTYID LEFT OUTER JOIN " _
            &"RTObj ON RTSparqAdslChg.CUSID = RTObj.CUSID ON RTSparqadslcustdropsndwork.CUSID = RTSparqAdslChg.CUSID AND " _
            &"RTSparqadslcustdropsndwork.ENTRYNO = RTSparqAdslChg.ENTRYNO AND RTSparqadslcustdropsndwork.DROPDAT IS NULL " _
            &"WHERE (RTSparqAdslChg.DROPDAT IS NULL) AND (RTSparqAdslChg.MODIFYCODE = 'DR') " 
  dataTable="RTSparqAdslChg"
  userDefineDelete=""
  extTable=""
  numberOfKey=2
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage=""
  colSplit=1
  keyListPageSize=20
  searchFirst=false
 ' response.Write "k0=" & aryparmkey(0) & ";k1=" & aryparmkey(1) & ";k2=" & aryparmkey(2)
  searchShow="全部待拆機派工"
  searchProg="RTCustDropAllS.asp"
 
  If searchQry="" Then
     searchQry=" AND RTSparqadslcustdropsndwork.closedat is null "
     searchShow="全部(尚未拆機回報)"
  ELSE
     SEARCHFIRST=FALSE
  End If
    
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  sqllist="SELECT RTSparqAdslChg.CUSID, RTSparqAdslChg.ENTRYNO, RTSparqAdslChg.COMQ1,RTSPARQADSLCMTY.COMN,RTOBJ.CUSNC, RTCode.CODENC, " _
         &"RTSparqAdslChg.MODIFYDAT, RTSparqAdslChg.DOCKETDAT, RTSparqAdslChg.TRANSDAT,RTSparqadslcustdropsndwork.PRTNO, RTSparqadslcustdropsndwork.SENDWORKDAT, " _
         &"CASE WHEN rtobj_1.cusnc <> '' THEN rtobj_1.cusnc ELSE rtobj_2.shortnc END " _
         &"FROM  RTObj RTObj_2 RIGHT OUTER JOIN RTSparqadslcustdropsndwork ON " _
         &"RTObj_2.CUSID = RTSparqadslcustdropsndwork.ASSIGNCONSIGNEE LEFT OUTER JOIN RTEmployee LEFT OUTER JOIN " _
         &"RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID ON " _
         &"RTSparqadslcustdropsndwork.ASSIGNENGINEER = RTEmployee.EMPLY RIGHT OUTER JOIN RTSparqAdslChg INNER JOIN " _
         &"RTCode ON RTSparqAdslChg.MODIFYCODE = RTCode.CODE AND RTCode.KIND = 'K1' LEFT OUTER JOIN " _
         &"RTSparqAdslCmty ON RTSparqAdslChg.COMQ1 = RTSparqAdslCmty.CUTYID LEFT OUTER JOIN " _
         &"RTObj ON RTSparqAdslChg.CUSID = RTObj.CUSID ON RTSparqadslcustdropsndwork.CUSID = RTSparqAdslChg.CUSID AND " _
         &"RTSparqadslcustdropsndwork.ENTRYNO = RTSparqAdslChg.ENTRYNO AND RTSparqadslcustdropsndwork.DROPDAT IS NULL " _
         &"where (RTSparqAdslChg.DROPDAT IS NULL) AND (RTSparqadslcustdropsndwork.DROPDAT IS NULL) AND (RTSparqAdslChg.MODIFYCODE = 'DR') " & searchqry _
         &" order by 7 desc "
   'response.write "SQL=" & sqllist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->
