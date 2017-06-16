<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="ET-City管理系統"
  title="ET-City用戶主線派工單安裝設備異動資料查詢"
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
  formatName="none;none;none;none;項次;異動日;異動類別;異動人員;設備名稱/規格;數量;作廢日;出貨倉庫;資產編號;帳款編號;轉應收帳款日"
  sqlDelete="SELECT   RTLessorCmtyLineHARDWARELOG.comq1,RTLessorCmtyLineHARDWARELOG.lineq1, " _
           &"RTLessorCmtyLineHARDWARELOG.prtno,RTLessorCmtyLineHARDWARELOG.seq, RTLessorCmtyLineHARDWARELOG.seq2, RTLessorCmtyLineHARDWARELOG.CHGDAT, " _
           &"RTCode.CODENC, RTObj.CUSNC, RTProdH.PRODNC+'--'+ RTProdD1.SPEC, RTLessorCmtyLineHARDWARELOG.QTY, RTLessorCmtyLineHARDWARELOG.DROPDAT, " _
           &"HBwarehouse.WARENAME, RTLessorCmtyLineHARDWARELOG.ASSETNO,RTLessorCmtyLineHARDWARELOG.BATCHNO,RTLessorCmtyLineHARDWARELOG.TARDAT FROM RTProdD1 RIGHT OUTER JOIN RTLessorCmtyLineHARDWARELOG LEFT OUTER JOIN " _
           &"HBwarehouse ON RTLessorCmtyLineHARDWARELOG.WAREHOUSE = HBwarehouse.WAREHOUSE ON  RTProdD1.PRODNO = RTLessorCmtyLineHARDWARELOG.PRODNO AND " _
           &"RTProdD1.ITEMNO = RTLessorCmtyLineHARDWARELOG.ITEMNO LEFT OUTER JOIN RTProdH ON RTLessorCmtyLineHARDWARELOG.PRODNO = RTProdH.PRODNO " _
           &"LEFT OUTER JOIN RTCode ON RTLessorCmtyLineHARDWARELOG.CHGCODE = RTCode.CODE AND RTCode.KIND = 'G2' LEFT OUTER JOIN RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTLessorCmtyLineHARDWARELOG.CHGUsR = RTEmployee.EMPLY WHERE RTLessorCmtyLineHARDWARELOG.cusid = '' "
  dataTable="RTLessorCmtyLineHARDWARELOG"
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
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorCmtyH ON " _
       &"RTCounty.CUTID = RTLessorCmtyH.CUTID " _
       &"where RTLessorCmtyH.comq1=" & ARYPARMKEY(0) 
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorCmtyLine ON  " _
       &"RTCounty.CUTID = RTLessorCmtyLine.CUTID " _
       &"where RTLessorCmtyLine.comq1=" & ARYPARMKEY(0) & " and RTLessorCmtyLine.lineq1=" & ARYPARMKEY(1)
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     comaddr=""
     COMaddr=rsYY("cutnc") & rsyy("township")
     if rsyy("village") <> "" then
         COMaddr= COMaddr & rsyy("village") & rsyy("cod1")
     end if
     if rsyy("NEIGHBOR") <> "" then
         COMaddr= COMaddr & rsyy("NEIGHBOR") & rsyy("cod2")
     end if
     if rsyy("STREET") <> "" then
         COMaddr= COMaddr & rsyy("STREET") & rsyy("cod3")
     end if
     if rsyy("SEC") <> "" then
         COMaddr= COMaddr & rsyy("SEC") & rsyy("cod4")
     end if
     if rsyy("LANE") <> "" then
         COMaddr= COMaddr & rsyy("LANE") & rsyy("cod5")
     end if
     if rsyy("ALLEYWAY") <> "" then
         COMaddr= COMaddr & rsyy("ALLEYWAY") & rsyy("cod7")
     end if
     if rsyy("NUM") <> "" then
         COMaddr= COMaddr & rsyy("NUM") & rsyy("cod8")
     end if
     if rsyy("FLOOR") <> "" then
         COMaddr= COMaddr & rsyy("FLOOR") & rsyy("cod9")
     end if
     if rsyy("ROOM") <> "" then
         COMaddr= COMaddr & rsyy("ROOM") & rsyy("cod10")
     end if
  else
     COMaddr=""
  end if
  RSYY.Close

  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorCmtyLineHARDWARELOG.comq1=" & aryparmkey(0) & " and lineq1=" & aryparmkey(1) & " and RTLessorCmtyLineHARDWARELOG.prtno='" & aryparmkey(2) & "' AND RTLessorCmtyLineHARDWARELOG.seq=" & ARYPARMKEY(3)
     searchShow="主線︰"& aryparmkey(0) & "-" & aryparmkey(1) & ",社區名稱︰" & COMN & ",位址︰" & COMADDR & ",派工單號︰" & aryparmkey(2) & ",項次︰" & ARYPARMKEY(3)
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
  
         sqlList="SELECT     RTLessorCmtyLineHARDWARELOG.comq1, " _
           &"RTLessorCmtyLineHARDWARELOG.lineq1,RTLessorCmtyLineHARDWARELOG.PRTNO, RTLessorCmtyLineHARDWARELOG.seq, RTLessorCmtyLineHARDWARELOG.SEQ2, RTLessorCmtyLineHARDWARELOG.CHGDAT, " _
           &"RTCode.CODENC, RTObj.CUSNC, RTProdH.PRODNC+'--'+ RTProdD1.SPEC, RTLessorCmtyLineHARDWARELOG.QTY, RTLessorCmtyLineHARDWARELOG.DROPDAT, " _
           &"HBwarehouse.WARENAME, RTLessorCmtyLineHARDWARELOG.ASSETNO,RTLessorCmtyLineHARDWARELOG.BATCHNO,RTLessorCmtyLineHARDWARELOG.TARDAT FROM RTProdD1 RIGHT OUTER JOIN RTLessorCmtyLineHARDWARELOG LEFT OUTER JOIN " _
           &"HBwarehouse ON RTLessorCmtyLineHARDWARELOG.WAREHOUSE = HBwarehouse.WAREHOUSE ON  RTProdD1.PRODNO = RTLessorCmtyLineHARDWARELOG.PRODNO AND " _
           &"RTProdD1.ITEMNO = RTLessorCmtyLineHARDWARELOG.ITEMNO LEFT OUTER JOIN RTProdH ON RTLessorCmtyLineHARDWARELOG.PRODNO = RTProdH.PRODNO " _
           &"LEFT OUTER JOIN RTCode ON RTLessorCmtyLineHARDWARELOG.CHGCODE = RTCode.CODE AND RTCode.KIND = 'G2' LEFT OUTER JOIN RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTLessorCmtyLineHARDWARELOG.CHGUsR = RTEmployee.EMPLY  " _
           &"where " & searchqry
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>