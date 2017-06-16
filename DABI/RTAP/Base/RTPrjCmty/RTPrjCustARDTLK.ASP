<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="專案用戶管理系統"
  title="專案用戶應收應付帳款明細查詢"
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
  sqlDelete="SELECT     RTPrjCustARDTL.CUSID, RTPrjCustARDTL.BATCHNO, " _
                        &"  RTPrjCustARDTL.SEQ, " _
                        &"  RTPrjCustARDTL.L14 + '-' + RTPrjCustARDTL.L23 AS Expr2, " _
                        &"  RTAccountNo.ACNAMEC, RTPrjCustARDTL.ITEMNC, " _
                        &"  RTPrjCustARDTL.PORM, RTPrjCustARDTL.AMT, " _
                        &"  RTPrjCustARDTL.REALAMT, " _
                        &"  RTPrjCustARDTL.AMT - RTPrjCustARDTL.REALAMT AS Expr1, " _
                        &"  RTPrjCustARDTL.CDAT, RTPrjCustARDTL.MDAT, " _
                        &"  RTPrjCustARDTL.CANCELDAT, RTPrjCustARDTL.CANCELMEMO " _
           &"FROM           RTPrjCustARDTL LEFT OUTER JOIN " _
                        &"  RTAccountNo ON RTPrjCustARDTL.L14 = RTAccountNo.L14 AND " _
                        &"  RTPrjCustARDTL.L23 = RTAccountNo.L23 " _
           &"where RTPrjCustARDTL.cusid='' "
  dataTable="RTPrjCustARDTL"
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
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTPrjCmtyH ON " _
       &"RTCounty.CUTID = RTPrjCmtyH.CUTID RIGHT OUTER JOIN RTPrjCust ON RTPrjCmtyH.COMQ1 = RTPrjCust.COMQ1 " _
       &"where RTPrjCust.cusid='" & ARYPARMKEY(0) & "'"
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select RTCounty.CUTNC+RTPrjCmtyLine.township+RTPrjCmtyLine.raddr as comaddr from RTCounty RIGHT OUTER JOIN RTPrjCmtyLine ON  " _
       &"RTCounty.CUTID = RTPrjCmtyLine.CUTID RIGHT OUTER JOIN " _
       &"RTPrjCust ON RTPrjCmtyLine.COMQ1 = RTPrjCust.COMQ1 AND " _
       &"RTPrjCmtyLine.LINEQ1 = RTPrjCust.LINEQ1 " _
       &"where RTPrjCust.cusid='" & ARYPARMKEY(0) & "'"
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMaddr=rsyy("comaddr")
  else
     COMaddr=""
  end if
  RSYY.Close
  sqlYY="select * from RTPrjCUST  where CUSID='" & ARYPARMKEY(0) & "' "
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
     searchQry=" RTPrjCustARDTL.CUSID='" & ARYPARMKEY(0) & "' AND RTPrjCustARDTL.BATCHNO='" & ARYPARMKEY(1) & "' "
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
  
    sqlList="SELECT          RTPrjCustARDTL.CUSID, RTPrjCustARDTL.BATCHNO, " _
                        &"  RTPrjCustARDTL.SEQ, " _
                        &"  RTPrjCustARDTL.L14 + '-' + RTPrjCustARDTL.L23 AS Expr2, " _
                        &"  RTAccountNo.ACNAMEC,convert(varchar(4),RTPrjCustARDTL.syy)+'/'+convert(varchar(2),RTPrjCustARDTL.smm),convert(varchar(4),RTPrjCustARDTL.tyy)+'/'+convert(varchar(2),RTPrjCustARDTL.tmm), RTPrjCustARDTL.ITEMNC, " _
                        &"  RTPrjCustARDTL.PORM, RTPrjCustARDTL.AMT, " _
                        &"  RTPrjCustARDTL.REALAMT, " _
                        &"  RTPrjCustARDTL.AMT - RTPrjCustARDTL.REALAMT AS Expr1, " _
                        &"  RTPrjCustARDTL.CDAT, RTPrjCustARDTL.MDAT, " _
                        &"  RTPrjCustARDTL.CANCELDAT, RTPrjCustARDTL.CANCELMEMO " _
           &"FROM           RTPrjCustARDTL LEFT OUTER JOIN " _
                        &"  RTAccountNo ON RTPrjCustARDTL.L14 = RTAccountNo.L14 AND " _
                        &"  RTPrjCustARDTL.L23 = RTAccountNo.L23 " _
           &"where " & searchqry & " ORDER BY RTPrjCustARDTL.SEQ "


  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>