<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City用戶應收應付帳款明細查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  functionoptopen=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;項次;會計科目;科目名稱;立帳年月;認列年月;項目名稱;正/負;應收(付)金額;已沖銷金額;未沖帳金額;產生日;沖帳日;作廢日;作廢原因"
  sqlDelete="SELECT     RTLessorAVSCustARDTL.CUSID, RTLessorAVSCustARDTL.BATCHNO, " _
                        &"  RTLessorAVSCustARDTL.SEQ, " _
                        &"  RTLessorAVSCustARDTL.L14 + '-' + RTLessorAVSCustARDTL.L23 AS Expr2, " _
                        &"  RTAccountNo.ACNAMEC, RTLessorAVSCustARDTL.ITEMNC, " _
                        &"  RTLessorAVSCustARDTL.PORM, RTLessorAVSCustARDTL.AMT, " _
                        &"  RTLessorAVSCustARDTL.REALAMT, " _
                        &"  RTLessorAVSCustARDTL.AMT - RTLessorAVSCustARDTL.REALAMT AS Expr1, " _
                        &"  RTLessorAVSCustARDTL.CDAT, RTLessorAVSCustARDTL.MDAT, " _
                        &"  RTLessorAVSCustARDTL.CANCELDAT, RTLessorAVSCustARDTL.CANCELMEMO " _
           &"FROM           RTLessorAVSCustARDTL LEFT OUTER JOIN " _
                        &"  RTAccountNo ON RTLessorAVSCustARDTL.L14 = RTAccountNo.L14 AND " _
                        &"  RTLessorAVSCustARDTL.L23 = RTAccountNo.L23 " _
           &"where RTLessorAVSCustARDTL.cusid='' "
  dataTable="RTLessorAVSCustARDTL"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg=""
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
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorAVSCmtyH ON " _
       &"RTCounty.CUTID = RTLessorAVSCmtyH.CUTID RIGHT OUTER JOIN RTLessorAVSCust ON RTLessorAVSCmtyH.COMQ1 = RTLessorAVSCust.COMQ1 " _
       &"where RTLessorAVSCust.cusid='" & ARYPARMKEY(0) & "'"
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorAVSCmtyLine ON  " _
       &"RTCounty.CUTID = RTLessorAVSCmtyLine.CUTID RIGHT OUTER JOIN " _
       &"RTLessorAVSCust ON RTLessorAVSCmtyLine.COMQ1 = RTLessorAVSCust.COMQ1 AND " _
       &"RTLessorAVSCmtyLine.LINEQ1 = RTLessorAVSCust.LINEQ1 " _
       &"where RTLessorAVSCust.cusid='" & ARYPARMKEY(0) & "'"
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
  sqlYY="select * from RTLessorAVSCUST  where CUSID='" & ARYPARMKEY(0) & "' "
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     CUSNC=rsYY("CUSNC")
     comq1xx=rsyy("comq1")
     lineq1xx=rsyy("lineq1")
  else
     CUSNC=""
     comq1xx=""
     lineq1xx=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorAVSCustARDTL.CUSID='" & ARYPARMKEY(0) & "' AND RTLessorAVSCustARDTL.BATCHNO='" & ARYPARMKEY(1) & "' "
     searchShow="主線︰"& comq1xx & "-" & lineq1xx & ",社區︰" & COMN & ",主線位址︰" & COMADDR & ",用戶序號︰" & aryparmkey(0) & ",用戶名稱︰" & CUSNC & ",應收帳款編號︰" & ARYPARMKEY(1)
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
  
    sqlList="SELECT          RTLessorAVSCustARDTL.CUSID, RTLessorAVSCustARDTL.BATCHNO, " _
                        &"  RTLessorAVSCustARDTL.SEQ, " _
                        &"  RTLessorAVSCustARDTL.L14 + '-' + RTLessorAVSCustARDTL.L23 AS Expr2, " _
                        &"  RTAccountNo.ACNAMEC,convert(varchar(4),RTLessorAVSCustARDTL.syy)+'/'+convert(varchar(2),RTLessorAVSCustARDTL.smm),convert(varchar(4),RTLessorAVSCustARDTL.tyy)+'/'+convert(varchar(2),RTLessorAVSCustARDTL.tmm), RTLessorAVSCustARDTL.ITEMNC, " _
                        &"  RTLessorAVSCustARDTL.PORM, RTLessorAVSCustARDTL.AMT, " _
                        &"  RTLessorAVSCustARDTL.REALAMT, " _
                        &"  RTLessorAVSCustARDTL.AMT - RTLessorAVSCustARDTL.REALAMT AS Expr1, " _
                        &"  RTLessorAVSCustARDTL.CDAT, RTLessorAVSCustARDTL.MDAT, " _
                        &"  RTLessorAVSCustARDTL.CANCELDAT, RTLessorAVSCustARDTL.CANCELMEMO " _
           &"FROM           RTLessorAVSCustARDTL LEFT OUTER JOIN " _
                        &"  RTAccountNo ON RTLessorAVSCustARDTL.L14 = RTAccountNo.L14 AND " _
                        &"  RTLessorAVSCustARDTL.L23 = RTAccountNo.L23 " _
           &"where " & searchqry & " ORDER BY RTLessorAVSCustARDTL.SEQ "


  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>