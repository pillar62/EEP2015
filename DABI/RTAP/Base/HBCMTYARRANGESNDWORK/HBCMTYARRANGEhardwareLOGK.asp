<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Hi-Building管理系統"
  title="社區整線派工單設備異動資料查詢"
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
  formatName="none;none;none;none;項次;異動日期;異動類別;異動人員;設備名稱;規格;數量;作廢日"
  sqlDelete="SELECT HBCmtyarrangeHardwareLog.COMQ1, HBCmtyarrangeHardwareLog.COMTYPE, HBCmtyarrangeHardwareLog.PRTNO, " _
           &"HBCmtyarrangeHardwareLog.SEQ, HBCmtyarrangeHardwareLog.SEQ AS Expr1, HBCmtyarrangeHardwareLog.CHGDAT, RTCode.CODENC, RTObj.CUSNC, " _
           &"RTProdH.PRODNC, RTProdD1.SPEC, HBCmtyarrangeHardwareLog.QTY,  HBCmtyarrangeHardwareLog.DROPDAT " _
           &"FROM RTProdD1 RIGHT OUTER JOIN  HBCmtyarrangeHardwareLog INNER JOIN   RTCode ON HBCmtyarrangeHardwareLog.CHGCODE = RTCode.CODE AND " _
           &"RTCode.KIND = 'G2' ON  RTProdD1.PRODNO = HBCmtyarrangeHardwareLog.PRODNO AND  RTProdD1.ITEMNO = HBCmtyarrangeHardwareLog.ITEMNO LEFT OUTER JOIN " _
           &"RTProdH ON HBCmtyarrangeHardwareLog.PRODNO = RTProdH.PRODNO LEFT OUTER JOIN  RTObj INNER JOIN  RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
           &"HBCmtyarrangeHardwareLog.CHGUSR = RTEmployee.EMPLY " _
           &"where HBCmtyarrangeHardwareLog.COMQ1=0"
  dataTable="HBCmtyarrangeHardwareLog"
  userDefineDelete="Yes"
  numberOfKey=4
  dataProg="none"
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
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  connYY.Open dsnYY
  if aryparmkey(1)="01" then
     comtype="Hi-Buildinf"
     sqlYY="select * from RTCMTY LEFT OUTER JOIN RTCOUNTY ON RTCMTY.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0)
     rsYY.Open sqlYY,connYY
     if not rsYY.EOF then
        COMN=rsYY("COMN")
     else
        COMN=""
     end if
     rsYY.Close
  elseif  aryparmkey(1)="02" then
     comtype="中華399"
     sqlYY="select * from RTcustadslCMTY LEFT OUTER JOIN RTCOUNTY ON RTcustadslCMTY.CUTID=RTCOUNTY.CUTID where cutyid=" & ARYPARMKEY(0)
     rsYY.Open sqlYY,connYY
     if not rsYY.EOF then
        COMN=rsYY("COMN")
     else
        COMN=""
     end if
     rsYY.Close
  elseif  aryparmkey(1)="03" then
     comtype="速博399"
     sqlYY="select * from RTsparqadslCMTY LEFT OUTER JOIN RTCOUNTY ON RTsparqadslCMTY.CUTID=RTCOUNTY.CUTID where cutyid=" & ARYPARMKEY(0)
     rsYY.Open sqlYY,connYY
     if not rsYY.EOF then
        COMN=rsYY("COMN")
     else
        COMN=""
     end if
     rsYY.Close
  end if
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" HBCmtyarrangeHardwareLog.ComQ1=" & aryparmkey(0) & " and HBCmtyarrangeHardwareLog.COMTYPE=" & aryparmkey(1) & " and HBCmtyarrangeHardwareLog.prtno='" & aryparmkey(2) & "' and HBCmtyarrangeHardwareLog.entryno=" &  aryparmkey(3)
     searchShow="社區序號︰"& aryparmkey(0) & ",社區名稱︰" & COMN & ",社區類別︰" & COMtype & ",整線派工單號︰" & aryparmkey(2) & ",設備項次︰" &  aryparmkey(3)
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
            DAreaID="='A1'"
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
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
  
    If searchShow="全部" Then
         sqlList="SELECT HBCmtyarrangeHardwareLog.COMQ1, HBCmtyarrangeHardwareLog.COMTYPE, HBCmtyarrangeHardwareLog.PRTNO, " _
           &"HBCmtyarrangeHardwareLog.SEQ, HBCmtyarrangeHardwareLog.SEQ AS Expr1, HBCmtyarrangeHardwareLog.CHGDAT, RTCode.CODENC, RTObj.CUSNC, " _
           &"RTProdH.PRODNC, RTProdD1.SPEC, HBCmtyarrangeHardwareLog.QTY,  HBCmtyarrangeHardwareLog.DROPDAT " _
           &"FROM RTProdD1 RIGHT OUTER JOIN  HBCmtyarrangeHardwareLog INNER JOIN   RTCode ON HBCmtyarrangeHardwareLog.CHGCODE = RTCode.CODE AND " _
           &"RTCode.KIND = 'G2' ON  RTProdD1.PRODNO = HBCmtyarrangeHardwareLog.PRODNO AND  RTProdD1.ITEMNO = HBCmtyarrangeHardwareLog.ITEMNO LEFT OUTER JOIN " _
           &"RTProdH ON HBCmtyarrangeHardwareLog.PRODNO = RTProdH.PRODNO LEFT OUTER JOIN  RTObj INNER JOIN  RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
           &"HBCmtyarrangeHardwareLog.CHGUSR = RTEmployee.EMPLY " _
           &"where " & searchqry
    Else
         sqlList="SELECT HBCmtyarrangeHardwareLog.COMQ1, HBCmtyarrangeHardwareLog.COMTYPE, HBCmtyarrangeHardwareLog.PRTNO, " _
           &"HBCmtyarrangeHardwareLog.SEQ, HBCmtyarrangeHardwareLog.SEQ AS Expr1, HBCmtyarrangeHardwareLog.CHGDAT, RTCode.CODENC, RTObj.CUSNC, " _
           &"RTProdH.PRODNC, RTProdD1.SPEC, HBCmtyarrangeHardwareLog.QTY,  HBCmtyarrangeHardwareLog.DROPDAT " _
           &"FROM RTProdD1 RIGHT OUTER JOIN  HBCmtyarrangeHardwareLog INNER JOIN   RTCode ON HBCmtyarrangeHardwareLog.CHGCODE = RTCode.CODE AND " _
           &"RTCode.KIND = 'G2' ON  RTProdD1.PRODNO = HBCmtyarrangeHardwareLog.PRODNO AND  RTProdD1.ITEMNO = HBCmtyarrangeHardwareLog.ITEMNO LEFT OUTER JOIN " _
           &"RTProdH ON HBCmtyarrangeHardwareLog.PRODNO = RTProdH.PRODNO LEFT OUTER JOIN  RTObj INNER JOIN  RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
           &"HBCmtyarrangeHardwareLog.CHGUSR = RTEmployee.EMPLY " _
           &"where " & searchqry
    End If  
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>