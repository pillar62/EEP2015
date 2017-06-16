<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City用戶物品領用單明細資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
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
  formatName="領用單號;項次;產品編號;產品類別;規格;領用數量;單位;備註"
  sqlDelete="SELECT RTLessorAVSCustRCVHardwareDTL.RCVPRTNO, RTLessorAVSCustRCVHardwareDTL.ENTRYNO, " _
                     &"     RTLessorAVSCustRCVHardwareDTL.PRODNO + '-' + RTLessorAVSCustRCVHardwareDTL.ITEMNO " _
                      &"      AS Expr1, RTProdH.PRODNC, RTProdD1.SPEC, " _
                     &"      RTLessorAVSCustRCVHardwareDTL.QTY, RTCode.CODENC, " _
                  &"         RTLessorAVSCustRCVHardwareDTL.MEMO " _
 &" FROM             RTLessorAVSCustRCVHardwareDTL LEFT OUTER JOIN " _
               &"            RTCode ON RTLessorAVSCustRCVHardwareDTL.UNIT = RTCode.CODE AND " _
                 &"          RTCode.KIND = 'B5' LEFT OUTER JOIN " _
                &"           RTProdD1 ON " _
                &"           RTLessorAVSCustRCVHardwareDTL.PRODNO = RTProdD1.PRODNO AND " _
                 &"          RTLessorAVSCustRCVHardwareDTL.ITEMNO = RTProdD1.ITEMNO LEFT OUTER JOIN " _
                &"           RTProdH ON RTLessorAVSCustRCVHardwareDTL.PRODNO = RTProdH.PRODNO " _
                &" where RTLessorAVSCustRCVHardwareDTL.rcvprtno='' "
  dataTable="RTLessorAVSCustRCVHardwareDTL"
  userDefineDelete="Yes"
  numberOfKey=1
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
     searchQry=" RTLessorAVSCustRCVHardwareDTL.rcvprtno='" & aryparmkey(0) & "' "
     searchShow="領用單號︰" & aryparmkey(0)
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
  if userlevel=31 then DAreaID="<>'*'"
  
    If searchShow="全部" Then
          sqlList="SELECT RTLessorAVSCustRCVHardwareDTL.RCVPRTNO, RTLessorAVSCustRCVHardwareDTL.ENTRYNO, " _
                     &"     RTLessorAVSCustRCVHardwareDTL.PRODNO + '-' + RTLessorAVSCustRCVHardwareDTL.ITEMNO " _
                      &"      AS Expr1, RTProdH.PRODNC, RTProdD1.SPEC, " _
                     &"      RTLessorAVSCustRCVHardwareDTL.QTY, RTCode.CODENC, " _
                  &"         RTLessorAVSCustRCVHardwareDTL.MEMO " _
 &" FROM             RTLessorAVSCustRCVHardwareDTL LEFT OUTER JOIN " _
               &"            RTCode ON RTLessorAVSCustRCVHardwareDTL.UNIT = RTCode.CODE AND " _
                 &"          RTCode.KIND = 'B5' LEFT OUTER JOIN " _
                &"           RTProdD1 ON " _
                &"           RTLessorAVSCustRCVHardwareDTL.PRODNO = RTProdD1.PRODNO AND " _
                 &"          RTLessorAVSCustRCVHardwareDTL.ITEMNO = RTProdD1.ITEMNO LEFT OUTER JOIN " _
                &"           RTProdH ON RTLessorAVSCustRCVHardwareDTL.PRODNO = RTProdH.PRODNO " _
                &" where " & searchqry & " " _
                &"           order by RTLessorAVSCustRCVHardwareDTL.entryno  "
    Else
         sqlList="SELECT RTLessorAVSCustRCVHardwareDTL.RCVPRTNO, RTLessorAVSCustRCVHardwareDTL.ENTRYNO, " _
                  &"         RTLessorAVSCustRCVHardwareDTL.PRODNO + '-' + RTLessorAVSCustRCVHardwareDTL.ITEMNO " _
                  &"          AS Expr1, RTProdH.PRODNC, RTProdD1.SPEC, " _
                  &"         RTLessorAVSCustRCVHardwareDTL.QTY, RTCode.CODENC, " _
                  &"         RTLessorAVSCustRCVHardwareDTL.MEMO " _
 &" FROM             RTLessorAVSCustRCVHardwareDTL LEFT OUTER JOIN " _
                 &"          RTCode ON RTLessorAVSCustRCVHardwareDTL.UNIT = RTCode.CODE AND " _
                  &"         RTCode.KIND = 'B5' LEFT OUTER JOIN " _
                  &"         RTProdD1 ON " _
                   &"        RTLessorAVSCustRCVHardwareDTL.PRODNO = RTProdD1.PRODNO AND " _
                   &"        RTLessorAVSCustRCVHardwareDTL.ITEMNO = RTProdD1.ITEMNO LEFT OUTER JOIN " _
                   &"        RTProdH ON RTLessorAVSCustRCVHardwareDTL.PRODNO = RTProdH.PRODNO " _
                   &" where " & searchqry & " " _
                   &" order by RTLessorAVSCustRCVHardwareDTL.entryno  "

    End If  
   'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>