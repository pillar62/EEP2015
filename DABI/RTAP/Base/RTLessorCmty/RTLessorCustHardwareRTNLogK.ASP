<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="ET-City管理系統"
  title="ET-City用戶設備物品移轉單異動資料查詢"
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
  formatName="none;項次;異動日;異動類別;異動人員;移轉類別;移轉申請日;移轉申請人;實際移轉人;移轉結案日;結案人員;作廢日;作廢人員;派工單號;轉移轉單人員;移轉數量"
  sqlDelete="SELECT  RTLessorCustRTNHardwareLOG.RCVPRTNO AS RCVPRTNO, " _
                   &"        RTLessorCustRTNHardwareLOG.entryno, " _
                      &"     RTLessorCustRTNHardwareLOG.chgdat, RTCODE.CODENC, RTOBJ_10.CUSNC, " _
                         &"  CASE WHEN RTLessorCustRTNHardwareLOG.DATASRC = '01' THEN '裝機派工領用' " _
                   &"         WHEN RTLessorCustRTNHardwareLOG.DATASRC = '02' THEN '收款派工領用' ELSE " _
                  &"          '' END, RTLessorCustRTNHardwareLOG.APPLYDAT AS APPLYDAT,  " _
                  &"         CASE WHEN RTObj_7.CUSNC = '' OR " _
                  &"         RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                  &"          CASE WHEN RTObj_1.CUSNC = '' OR " _
                  &"         RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                  &"          RTLessorCustRTNHardwareLOG.CLOSEDAT AS CLOSEDAT,  " _
                 &"          RTObj_4.CUSNC AS CUSNC5,  " _
                  &"         RTLessorCustRTNHardwareLOG.CANCELDAT AS CANCELDAT,  " _
                  &"         RTObj_5.CUSNC AS CUSNC6,  " _
                  &"         RTLessorCustRTNHardwareLOG.PRTNO AS PRTNO,  " _
                 &"          RTObj_6.CUSNC AS cusnc7, SUM(RTLessorCustRTNHardwareDTL.QTY) " _
&" FROM             RTEmployee RTEmployee_4 INNER JOIN " _
                 &"          RTObj RTObj_6 ON RTEmployee_4.CUSID = RTObj_6.CUSID RIGHT OUTER JOIN " _
                &"           RTLessorCustRTNHardwareLOG ON  " _
                 &"          RTEmployee_4.EMPLY = RTLessorCustRTNHardwareLOG.RCVUSR LEFT OUTER " _
                 &"           JOIN " _
                 &"          RTEmployee RTEmployee_3 INNER JOIN " _
                 &"          RTObj RTObj_5 ON RTEmployee_3.CUSID = RTObj_5.CUSID ON  " _
                &"           RTLessorCustRTNHardwareLOG.CANCELUSR = RTEmployee_3.EMPLY LEFT OUTER " _
                 &"           JOIN " _
                  &"         RTEmployee RTEmployee_2 INNER JOIN " _
                  &"         RTObj RTObj_4 ON RTEmployee_2.CUSID = RTObj_4.CUSID ON  " _
                 &"          RTLessorCustRTNHardwareLOG.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER " _
                 &"           JOIN " _
                 &"          RTObj RTObj_3 ON  " _
                 &"          RTLessorCustRTNHardwareLOG.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER " _
                 &"           JOIN " _
                 &"          RTObj RTObj_2 ON  " _
                 &"          RTLessorCustRTNHardwareLOG.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER " _
                 &"           JOIN " _
                &"           RTEmployee RTEmployee_1 INNER JOIN " _
               &"           RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON  " _
                &"           RTLessorCustRTNHardwareLOG.REALENGINEER = RTEmployee_1.EMPLY LEFT " _
                 &"           OUTER JOIN " _
                  &"         RTObj RTObj_7 INNER JOIN " _
                 &"          RTEmployee RTEmployee_5 ON RTObj_7.CUSID = RTEmployee_5.CUSID ON  " _
                 &"          RTLessorCustRTNHardwareLOG.ASSIGNENGINEER = RTEmployee_5.EMPLY LEFT " _
                 &"           OUTER JOIN " _
                 &"          RTLessorCustRTNHardwareDTL ON  " _
                 &"          RTLessorCustRTNHardwareLOG.RCVPRTNO = RTLessorCustRTNHardwareDTL.RCVPRTNO " _
                &"            LEFT OUTER JOIN " _
                &"           rtcode ON RTLessorCustRTNHardwareLOG.chgcodE = rtcode.code AND  " _
                &"           rtcode.kind = 'G2' LEFT OUTER JOIN " _
                &"           RTEMPLOYEE RTEMPLOYEE_10 ON  " _
                 &"          RTLessorCustRTNHardwareLOG.CHGUSR = RTEMPLOYEE_10.EMPLY INNER JOIN " _
                 &"          RTOBJ RTOBJ_10 ON RTEMPLOYEE_10.CUSID = RTOBJ_10.CUSID " _
                       &" where RTLessorCustRTNHardwareLOG.RCVPRTNO='' " _
                       &" GROUP BY  RTLessorCustRTNHardwareLOG.RCVPRTNO,  " _
                &"           RTLessorCustRTNHardwareLOG.entryno,  " _
               &"            RTLessorCustRTNHardwareLOG.chgdat, RTCODE.CODENC, RTOBJ_10.CUSNC,  " _
               &"            CASE WHEN RTLessorCustRTNHardwareLOG.DATASRC = '01' THEN '裝機派工領用' " _
               &"             WHEN RTLessorCustRTNHardwareLOG.DATASRC = '02' THEN '收款派工領用' ELSE " _
                &"            '' END, RTLessorCustRTNHardwareLOG.APPLYDAT,  " _
                &"           CASE WHEN RTObj_7.CUSNC = '' OR " _
                &"           RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                &"            CASE WHEN RTObj_1.CUSNC = '' OR " _
                 &"          RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                &"            RTLessorCustRTNHardwareLOG.CLOSEDAT, RTObj_4.CUSNC,  " _
                &"           RTObj_5.CUSNC, RTLessorCustRTNHardwareLOG.PRTNO,  " _
                &"           RTLessorCustRTNHardwareLOG.CANCELDAT, RTObj_6.CUSNC  "
  dataTable="RTlessorcustHARDWARELOG"
  userDefineDelete="Yes"
  numberOfKey=4
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
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=XXLIB"
  sqlxx="select * from usergroup where userid='" & Request.ServerVariables("LOGON_USER") & "'"
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     usergroup=rsxx("group")
  else
     usergroup=""
  end if
  rsxx.Close
  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  '----

  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorCustRTNHardwareLOG .RCVprtno='" & aryparmkey(0) & "' "
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
  'if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T93168" OR _
  '   Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
  
         sqlList="SELECT  RTLessorCustRTNHardwareLOG.RCVPRTNO AS RCVPRTNO, " _
                   &"        RTLessorCustRTNHardwareLOG.entryno, " _
                      &"     RTLessorCustRTNHardwareLOG.chgdat, RTCODE.CODENC, RTOBJ_10.CUSNC, " _
                         &"  RTCODE_2.CODENC, RTLessorCustRTNHardwareLOG.APPLYDAT AS APPLYDAT,  " _
                  &"         CASE WHEN RTObj_7.CUSNC = '' OR " _
                  &"         RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                  &"          CASE WHEN RTObj_1.CUSNC = '' OR " _
                  &"         RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                  &"          RTLessorCustRTNHardwareLOG.CLOSEDAT AS CLOSEDAT,  " _
                 &"          RTObj_4.CUSNC AS CUSNC5,  " _
                  &"         RTLessorCustRTNHardwareLOG.CANCELDAT AS CANCELDAT,  " _
                  &"         RTObj_5.CUSNC AS CUSNC6,  " _
                  &"         RTLessorCustRTNHardwareLOG.PRTNO AS PRTNO,  " _
                 &"          RTObj_6.CUSNC AS cusnc7, SUM(RTLessorCustRTNHardwareDTL.QTY) " _
&" FROM             RTEmployee RTEmployee_4 INNER JOIN " _
                 &"          RTObj RTObj_6 ON RTEmployee_4.CUSID = RTObj_6.CUSID RIGHT OUTER JOIN " _
                &"           RTLessorCustRTNHardwareLOG ON  " _
                 &"          RTEmployee_4.EMPLY = RTLessorCustRTNHardwareLOG.RCVUSR LEFT OUTER " _
                 &"           JOIN " _
                 &"          RTEmployee RTEmployee_3 INNER JOIN " _
                 &"          RTObj RTObj_5 ON RTEmployee_3.CUSID = RTObj_5.CUSID ON  " _
                &"           RTLessorCustRTNHardwareLOG.CANCELUSR = RTEmployee_3.EMPLY LEFT OUTER " _
                 &"           JOIN " _
                  &"         RTEmployee RTEmployee_2 INNER JOIN " _
                  &"         RTObj RTObj_4 ON RTEmployee_2.CUSID = RTObj_4.CUSID ON  " _
                 &"          RTLessorCustRTNHardwareLOG.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER " _
                 &"           JOIN " _
                 &"          RTObj RTObj_3 ON  " _
                 &"          RTLessorCustRTNHardwareLOG.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER " _
                 &"           JOIN " _
                 &"          RTObj RTObj_2 ON  " _
                 &"          RTLessorCustRTNHardwareLOG.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER " _
                 &"           JOIN " _
                &"           RTEmployee RTEmployee_1 INNER JOIN " _
               &"           RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON  " _
                &"           RTLessorCustRTNHardwareLOG.REALENGINEER = RTEmployee_1.EMPLY LEFT " _
                 &"           OUTER JOIN " _
                  &"         RTObj RTObj_7 INNER JOIN " _
                 &"          RTEmployee RTEmployee_5 ON RTObj_7.CUSID = RTEmployee_5.CUSID ON  " _
                 &"          RTLessorCustRTNHardwareLOG.ASSIGNENGINEER = RTEmployee_5.EMPLY LEFT " _
                 &"           OUTER JOIN " _
                 &"          RTLessorCustRTNHardwareDTL ON  " _
                 &"          RTLessorCustRTNHardwareLOG.RCVPRTNO = RTLessorCustRTNHardwareDTL.RCVPRTNO " _
                &"            LEFT OUTER JOIN " _
                &"           rtcode ON RTLessorCustRTNHardwareLOG.chgcodE = rtcode.code AND  " _
                &"           rtcode.kind = 'G2' LEFT OUTER JOIN " _
                &"           RTEMPLOYEE RTEMPLOYEE_10 ON  " _
                 &"          RTLessorCustRTNHardwareLOG.CHGUSR = RTEMPLOYEE_10.EMPLY INNER JOIN " _
                 &"          RTOBJ RTOBJ_10 ON RTEMPLOYEE_10.CUSID = RTOBJ_10.CUSID LEFT OUTER JOIN " _
                 &"          RTCODE RTCODE_2 ON RTLessorCustRTNHardwareLOG.DATASRC=RTCODE_2.CODE AND RTCODE_2.KIND='O1' " _
                       &" where " & searchqry & " " _
                       &" GROUP BY  RTLessorCustRTNHardwareLOG.RCVPRTNO,  " _
                &"           RTLessorCustRTNHardwareLOG.entryno,  " _
               &"            RTLessorCustRTNHardwareLOG.chgdat, RTCODE.CODENC, RTOBJ_10.CUSNC,  " _
               &"            RTCODE_2.CODENC, RTLessorCustRTNHardwareLOG.APPLYDAT,  " _
                &"           CASE WHEN RTObj_7.CUSNC = '' OR " _
                &"           RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                &"            CASE WHEN RTObj_1.CUSNC = '' OR " _
                 &"          RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                &"            RTLessorCustRTNHardwareLOG.CLOSEDAT, RTObj_4.CUSNC,  " _
                &"           RTObj_5.CUSNC, RTLessorCustRTNHardwareLOG.PRTNO,  " _
                &"           RTLessorCustRTNHardwareLOG.CANCELDAT, RTObj_6.CUSNC  "

  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>