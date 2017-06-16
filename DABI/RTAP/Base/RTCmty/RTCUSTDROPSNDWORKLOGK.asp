<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building管理系統"
  title="中華HB599退租戶拆機派工異動資料查詢"
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
  formatName="none;none;none;none;拆機單號;項次;異動日期;異動類別;異動人員;派工日;作廢日;作廢原因;裝機完工日;獎金計算年月"
  sqlDelete="SELECT  RTcustdropsndworkLOG.CUSID,RTcustdropsndworkLOG.ENTRYNO, " _
           &"RTcustdropsndworkLOG.PRTNO, " _
           &"RTcustdropsndworkLOG.SEQ," _
           &"RTcustdropsndworkLOG.PRTNO,RTcustdropsndworkLOG.seq,RTcustdropsndworkLOG.CHGDAT, RTCode.CODENC, RTObj.CUSNC, " _
           &"RTcustdropsndworkLOG.SENDWORKDAT, RTcustdropsndworkLOG.DROPDAT, " _
           &"RTcustdropsndworkLOG.DROPDESC, RTcustdropsndworkLOG.CLOSEDAT, " _
           &"RTcustdropsndworkLOG.BONUSCLOSEYM FROM RTCode RIGHT OUTER JOIN " _
           &"RTcustdropsndworkLOG ON RTCode.CODE = RTcustdropsndworkLOG.CHGCODE " _
           &"AND RTCode.KIND = 'G2' LEFT OUTER JOIN RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
           &"RTcustdropsndworkLOG.CHGUSR = RTEmployee.EMPLY where RTcustdropsndworkLOG.CUTYID=0"
  dataTable="RTcustdropsndworkLOG"
  userDefineDelete="Yes"
  numberOfKey=3
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
'     searchQry=" RTCmty.CUTYID=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.CUTYID <> 0
  '----
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTcustdropsndworkLOG.cusid='" & aryparmkey(0)  & "' and RTcustdropsndworkLOG.entryno=" & aryparmkey(1) & " and RTcustdropsndworkLOG.prtno='" & aryparmkey(2) & "' "
     searchShow=""
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
  'if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="P92010" OR _
  '   Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
  
  sqllist="SELECT  RTcustdropsndworkLOG.CUSID,RTcustdropsndworkLOG.ENTRYNO, " _
           &"RTcustdropsndworkLOG.PRTNO, " _
           &"RTcustdropsndworkLOG.SEQ," _
           &"RTcustdropsndworkLOG.PRTNO,RTcustdropsndworkLOG.seq,RTcustdropsndworkLOG.CHGDAT, RTCode.CODENC, RTObj.CUSNC, " _
           &"RTcustdropsndworkLOG.SENDWORKDAT, RTcustdropsndworkLOG.DROPDAT, " _
           &"RTcustdropsndworkLOG.DROPDESC, RTcustdropsndworkLOG.CLOSEDAT, " _
           &"RTcustdropsndworkLOG.BONUSCLOSEYM FROM RTCode RIGHT OUTER JOIN " _
           &"RTcustdropsndworkLOG ON RTCode.CODE = RTcustdropsndworkLOG.CHGCODE " _
           &"AND RTCode.KIND = 'G2' LEFT OUTER JOIN RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
           &"RTcustdropsndworkLOG.CHGUSR = RTEmployee.EMPLY " _
           &"where " & searchqry
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>