<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="AVS用戶欠費拆機工單異動資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
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
  formatName="none;none;none;none;none;主線;竣工單號;項次;異動日期;異動類別;異動人員;派工日;作廢日;異動原因;完工結案;未完工結案;獎金計算年月"
  sqlDelete="SELECT  RTEBTCUSTm2m3SNDWORKLOG.avsno, " _
           &"RTEBTCUSTm2m3SNDWORKLOG.m2m3,RTEBTCUSTm2m3SNDWORKLOG.seq,RTEBTCUSTm2m3SNDWORKLOG.prtno, " _
           &"RTEBTCUSTm2m3SNDWORKLOG.ENTRYNO, rtrim(convert(char(6),RTEBTCUST.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTCUST.lineQ1))  as comqline," _
           &"RTEBTCUSTm2m3SNDWORKLOG.PRTNO,RTEBTCUSTm2m3SNDWORKLOG.ENTRYNO,RTEBTCUSTm2m3SNDWORKLOG.CHGDAT, RTCode.CODENC, RTObj.CUSNC, " _
           &"RTEBTCUSTm2m3SNDWORKLOG.SENDWORKDAT, RTEBTCUSTm2m3SNDWORKLOG.DROPDAT, " _
           &"RTEBTCUSTm2m3SNDWORKLOG.DROPDESC, RTEBTCUSTm2m3SNDWORKLOG.CLOSEDAT, RTEBTCUSTm2m3SNDWORKLOG.unCLOSEDAT, " _
           &"RTEBTCUSTm2m3SNDWORKLOG.BONUSCLOSEYM FROM RTCode RIGHT OUTER JOIN " _
           &"RTEBTCUSTm2m3SNDWORKLOG ON RTCode.CODE = RTEBTCUSTm2m3SNDWORKLOG.CHGCODE " _
           &"AND RTCode.KIND = 'G2' LEFT OUTER JOIN RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
           &"RTEBTCUSTm2m3SNDWORKLOG.CHGUSR = RTEmployee.EMPLY " _
           &"LEFT OUTER  JOIN RTEBTCUST ON RTEBTCUSTM2M3SNDWORKLOG.AVSNO = RTEBTCUST.AVSNO " _
           &"LEFT OUTER JOIN  RTEBTCMTYH ON RTEBTCUST.COMQ1 = RTEBTCMTYH.COMQ1 " _
           &"where RTEBTCUSTm2m3SNDWORKLOG.COMQ1=0"
  dataTable="RTEBTCUSTm2m3SNDWORKLOG"
  userDefineDelete="Yes"
  numberOfKey=5
  dataProg="None"
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
  keyListPageSize=25
  searchProg="self"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTEBTCUSTm2m3SNDWORKLOG.avsno='" & aryparmkey(0) & "' and RTEBTCUSTm2m3SNDWORKLOG.m2m3='" & aryparmkey(1) & "' and RTEBTCUSTm2m3SNDWORKLOG.seq=" & aryparmkey(2) & " and RTEBTCUSTm2m3SNDWORKLOG.prtno='" & aryparmkey(3) & "' "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  Domain=Mid(Emply,1,1)
  select case Domain
         case "T"
            DAreaID="<>'*'"
         case "P"
            DAreaID="='A1'"                        
         case "C"
            DAreaID="='A2'"         
         case "K"
            DAreaID="='A3'"         
         case else
            DareaID="=''"
  end select
  '高階主管可讀取全部資料
  'if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR _
  '   Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31  then DAreaID="<>'*'"
  
         sqlList="SELECT  RTEBTCUSTm2m3SNDWORKLOG.avsno, " _
           &"RTEBTCUSTm2m3SNDWORKLOG.m2m3,RTEBTCUSTm2m3SNDWORKLOG.seq,RTEBTCUSTm2m3SNDWORKLOG.prtno, " _
           &"RTEBTCUSTm2m3SNDWORKLOG.ENTRYNO, rtrim(convert(char(6),RTEBTCUST.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTCUST.lineQ1))  as comqline, " _
           &"RTEBTCUSTm2m3SNDWORKLOG.PRTNO,RTEBTCUSTm2m3SNDWORKLOG.ENTRYNO,RTEBTCUSTm2m3SNDWORKLOG.CHGDAT, RTCode.CODENC, RTObj.CUSNC, " _
           &"RTEBTCUSTm2m3SNDWORKLOG.SENDWORKDAT, RTEBTCUSTm2m3SNDWORKLOG.DROPDAT, " _
           &"RTEBTCUSTm2m3SNDWORKLOG.DROPDESC, RTEBTCUSTm2m3SNDWORKLOG.CLOSEDAT, RTEBTCUSTm2m3SNDWORKLOG.unCLOSEDAT, " _
           &"RTEBTCUSTm2m3SNDWORKLOG.BONUSCLOSEYM FROM RTCode RIGHT OUTER JOIN " _
           &"RTEBTCUSTm2m3SNDWORKLOG ON RTCode.CODE = RTEBTCUSTm2m3SNDWORKLOG.CHGCODE " _
           &"AND RTCode.KIND = 'G2' LEFT OUTER JOIN RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
           &"RTEBTCUSTm2m3SNDWORKLOG.CHGUSR = RTEmployee.EMPLY " _
           &"LEFT OUTER  JOIN RTEBTCUST ON RTEBTCUSTM2M3SNDWORKLOG.AVSNO = RTEBTCUST.AVSNO " _
           &"LEFT OUTER JOIN  RTEBTCMTYH ON RTEBTCUST.COMQ1 = RTEBTCMTYH.COMQ1 " _
           &"where " & searchqry

  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>