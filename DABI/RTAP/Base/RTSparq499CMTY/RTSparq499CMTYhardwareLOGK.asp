<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="AVS主線派工單安裝設備異動資料查詢"
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
  formatName="none;none;none;none;項次;異動日;異動類別;異動人員;設備名稱/規格;數量;作廢日;出貨倉庫;資產編號"
  sqlDelete="SELECT       RTSparq499CmtyHARDWARELOG.COMQ1, RTSparq499CmtyHARDWARELOG.LINEQ1, " _
           &"RTSparq499CmtyHARDWARELOG.PRTNO, RTSparq499CmtyHARDWARELOG.ENTRYNO, RTSparq499CmtyHARDWARELOG.SEQ, RTSparq499CmtyHARDWARELOG.CHGDAT, " _
           &"RTCode.CODENC, RTObj.CUSNC, RTProdH.PRODNC+'--'+ RTProdD1.SPEC, RTSparq499CmtyHARDWARELOG.QTY, RTSparq499CmtyHARDWARELOG.DROPDAT, " _
           &"HBwarehouse.WARENAME, RTSparq499CmtyHARDWARELOG.ASSETNO FROM RTProdD1 RIGHT OUTER JOIN RTSparq499CmtyHARDWARELOG LEFT OUTER JOIN " _
           &"HBwarehouse ON RTSparq499CmtyHARDWARELOG.WAREHOUSE = HBwarehouse.WAREHOUSE ON  RTProdD1.PRODNO = RTSparq499CmtyHARDWARELOG.PRODNO AND " _
           &"RTProdD1.ITEMNO = RTSparq499CmtyHARDWARELOG.ITEMNO LEFT OUTER JOIN RTProdH ON RTSparq499CmtyHARDWARELOG.PRODNO = RTProdH.PRODNO " _
           &"LEFT OUTER JOIN RTCode ON RTSparq499CmtyHARDWARELOG.CHGCODE = RTCode.CODE AND RTCode.KIND = 'G3' LEFT OUTER JOIN RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTSparq499CmtyHARDWARELOG.CHGUR = RTEmployee.EMPLY WHERE RTSparq499CmtyHARDWARELOG.COMQ1 = 0 "
  dataTable="RTSparq499CmtyHARDWARElog"
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
  sqlYY="select * from RTSparq499CmtyH LEFT OUTER JOIN RTCOUNTY ON RTSparq499CmtyH.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0)
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTSparq499Cmtyline LEFT OUTER JOIN RTCOUNTY ON RTSparq499Cmtyline.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0) & " and lineq1=" & aryparmkey(1)
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     comaddr=""
     COMaddr=rsYY("cutnc") & rsyy("township") & rsyy("raddr")
  else
     COMaddr=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTSparq499CmtyHARDWARElog.ComQ1=" & aryparmkey(0) & " and RTSparq499CmtyHARDWARElog.lineq1=" & aryparmkey(1) & " and RTSparq499CmtyHARDWARElog.prtno='" & aryparmkey(2) & "' AND RTSparq499CmtyHARDWARElog.ENTRYNO=" & ARYPARMKEY(3)
     searchShow="社區︰"& aryparmkey(0) & ",名稱︰" & COMN &",主線︰" & aryparmkey(1) & ",位址︰" & COMADDR & ",派工單號︰" & aryparmkey(2) & ",項次︰" & ARYPARMKEY(3)
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
  
    If searchShow="全部" Then
         sqlList="SELECT       RTSparq499CmtyHARDWARELOG.COMQ1, RTSparq499CmtyHARDWARELOG.LINEQ1, " _
           &"RTSparq499CmtyHARDWARELOG.PRTNO, RTSparq499CmtyHARDWARELOG.ENTRYNO, RTSparq499CmtyHARDWARELOG.SEQ, RTSparq499CmtyHARDWARELOG.CHGDAT, " _
           &"RTCode.CODENC, RTObj.CUSNC, RTProdH.PRODNC+'--'+ RTProdD1.SPEC, RTSparq499CmtyHARDWARELOG.QTY, RTSparq499CmtyHARDWARELOG.DROPDAT, " _
           &"HBwarehouse.WARENAME, RTSparq499CmtyHARDWARELOG.ASSETNO FROM RTProdD1 RIGHT OUTER JOIN RTSparq499CmtyHARDWARELOG LEFT OUTER JOIN " _
           &"HBwarehouse ON RTSparq499CmtyHARDWARELOG.WAREHOUSE = HBwarehouse.WAREHOUSE ON  RTProdD1.PRODNO = RTSparq499CmtyHARDWARELOG.PRODNO AND " _
           &"RTProdD1.ITEMNO = RTSparq499CmtyHARDWARELOG.ITEMNO LEFT OUTER JOIN RTProdH ON RTSparq499CmtyHARDWARELOG.PRODNO = RTProdH.PRODNO " _
           &"LEFT OUTER JOIN RTCode ON RTSparq499CmtyHARDWARELOG.CHGCODE = RTCode.CODE AND RTCode.KIND = 'G3' LEFT OUTER JOIN RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTSparq499CmtyHARDWARELOG.CHGUR = RTEmployee.EMPLY  " _
           &"where " & searchqry
    Else
         sqlList="SELECT       RTSparq499CmtyHARDWARELOG.COMQ1, RTSparq499CmtyHARDWARELOG.LINEQ1, " _
           &"RTSparq499CmtyHARDWARELOG.PRTNO, RTSparq499CmtyHARDWARELOG.ENTRYNO, RTSparq499CmtyHARDWARELOG.SEQ, RTSparq499CmtyHARDWARELOG.CHGDAT, " _
           &"RTCode.CODENC, RTObj.CUSNC, RTProdH.PRODNC+'--'+ RTProdD1.SPEC, RTSparq499CmtyHARDWARELOG.QTY, RTSparq499CmtyHARDWARELOG.DROPDAT, " _
           &"HBwarehouse.WARENAME, RTSparq499CmtyHARDWARELOG.ASSETNO FROM RTProdD1 RIGHT OUTER JOIN RTSparq499CmtyHARDWARELOG LEFT OUTER JOIN " _
           &"HBwarehouse ON RTSparq499CmtyHARDWARELOG.WAREHOUSE = HBwarehouse.WAREHOUSE ON  RTProdD1.PRODNO = RTSparq499CmtyHARDWARELOG.PRODNO AND " _
           &"RTProdD1.ITEMNO = RTSparq499CmtyHARDWARELOG.ITEMNO LEFT OUTER JOIN RTProdH ON RTSparq499CmtyHARDWARELOG.PRODNO = RTProdH.PRODNO " _
           &"LEFT OUTER JOIN RTCode ON RTSparq499CmtyHARDWARELOG.CHGCODE = RTCode.CODE AND RTCode.KIND = 'G3' LEFT OUTER JOIN RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTSparq499CmtyHARDWARELOG.CHGUR = RTEmployee.EMPLY  " _
           &"where " & searchqry
    End If  
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>