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
  formatName="none;none;社區名稱;用戶;none;帳款編號;none;會計科目;none;立帳<br>年月;項目名稱;正/負;應收(付)<br>金額;已沖銷<br>金額;未沖帳<br>金額;產生日;沖帳日;作廢日;作廢原因"
  sqlDelete="SELECT     RTLessorAVSCustARDTL.TYY,RTLessorAVSCustARDTL.TMM,RTLessorAVScmtyh.comn,RTLessorAVScust.cusnc,RTLessorAVSCustARDTL.CUSID, RTLessorAVSCustARDTL.BATCHNO, " _
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
 
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorAVSCustARDTL.TYY=" & ARYPARMKEY(0) & " AND RTLessorAVSCustARDTL.TMM=" & ARYPARMKEY(1) & " "
     searchShow="帳款認列年月︰" & ARYPARMKEY(0) & "/" & right("00" + ARYPARMKEY(1),2)
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
  
    sqlList="SELECT         RTLessorAVSCustARDTL.TYY,RTLessorAVSCustARDTL.TMM,RTLessorAVScmtyh.comn,RTLessorAVScust.cusnc, RTLessorAVSCustARDTL.CUSID, RTLessorAVSCustARDTL.BATCHNO, " _
                        &"  RTLessorAVSCustARDTL.SEQ, " _
                        &"  RTLessorAVSCustARDTL.L14 + '-' + RTLessorAVSCustARDTL.L23 AS Expr2, " _
                        &"  RTAccountNo.ACNAMEC,convert(varchar(4),RTLessorAVSCustARDTL.syy)+'/'+convert(varchar(2),RTLessorAVSCustARDTL.smm), RTLessorAVSCustARDTL.ITEMNC, " _
                        &"  RTLessorAVSCustARDTL.PORM, RTLessorAVSCustARDTL.AMT, " _
                        &"  RTLessorAVSCustARDTL.REALAMT, " _
                        &"  RTLessorAVSCustARDTL.AMT - RTLessorAVSCustARDTL.REALAMT AS Expr1, " _
                        &"  RTLessorAVSCustARDTL.CDAT, RTLessorAVSCustARDTL.MDAT, " _
                        &"  RTLessorAVSCustARDTL.CANCELDAT, RTLessorAVSCustARDTL.CANCELMEMO " _
           &"FROM           RTLessorAVSCustARDTL LEFT OUTER JOIN " _
                        &"  RTAccountNo ON RTLessorAVSCustARDTL.L14 = RTAccountNo.L14 AND " _
                        &"  RTLessorAVSCustARDTL.L23 = RTAccountNo.L23 LEFT OUTER JOIN RTLessorAVSCUST ON RTLessorAVSCustARDTL.CUSID=RTLessorAVSCUST.CUSID LEFT OUTER JOIN " _
                        &"  RTLessorAVSCMTYH ON RTLessorAVSCUST.COMQ1=RTLessorAVSCMTYH.COMQ1 " _
           &"where RTLessorAVSCustARDTL.MDAT IS NOT NULL and RTLessorAVSCustARDTL.canceldat is null AND " & searchqry & " ORDER BY RTLessorAVSCustARDTL.CUSID,RTLessorAVSCustARDTL.SEQ "


  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>