<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="速博399管理系統"
  title="速博399用戶應收應付帳款查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=" 沖  帳 ;沖帳明細;帳款明細"
  functionOptProgram="RTSparqAdslCustARClear.asp;RTSparqAdslCustARClearK.asp;RTSparqAdslCustARDTLK.ASP"
  functionOptPrompt="N;N;N"
  functionoptopen="2;1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;應收帳款編號;AR/AP;明細項期數;應沖金額;已沖金額;未沖金額;沖立要項一;沖立要項二;沖立要項三;none;none;產生日;沖帳日;作廢日"
  sqlDelete="SELECT  RTSparqAdslCustAR.CUSID, " _
                &"RTSparqAdslCustAR.BATCHNO, " _
                &"RTCode.CODENC, RTSparqAdslCustAR.PERIOD, RTSparqAdslCustAR.AMT, RTSparqAdslCustAR.REALAMT, RTSparqAdslCustAR.AMT - RTSparqAdslCustAR.REALAMT, " _
                &"RTSparqAdslCustAR.COD1, RTSparqAdslCustAR.COD2, RTSparqAdslCustAR.COD3, " _
                &"RTSparqAdslCustAR.COD4, RTSparqAdslCustAR.COD5, RTSparqAdslCustAR.CDAT, " _
                &"RTSparqAdslCustAR.MDAT, RTSparqAdslCustAR.CANCELDAT " _
                &"FROM    RTSparqAdslCustAR LEFT OUTER JOIN " _
                &"RTCode ON RTSparqAdslCustAR.ARTYPE = RTCode.CODE AND " _
                &"RTCode.KIND = 'N2' " _
                &"where RTSparqAdslCUSTLOG.COMQ1=0 "
  dataTable="RTSparqAdslCUSTLOG"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg=""
  datawindowFeature=""
  searchWindowFeature="width=350,height=160,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="500"
  diaHeight="500"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTSparqAdslcustARS2.ASP"
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
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTSparqAdslCmty ON " _
       &"RTCounty.CUTID = RTSparqAdslCmty.CUTID RIGHT OUTER JOIN RTSparqAdslCust ON RTSparqAdslCmty.cutyid = RTSparqAdslCust.COMQ1 " _
       &"where RTSparqAdslCust.cusid='" & ARYPARMKEY(0) & "'"
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTSparqAdslCmty ON  " _
       &"RTCounty.CUTID = RTSparqAdslCmty.CUTID RIGHT OUTER JOIN " _
       &"RTSparqAdslCust ON RTSparqAdslCmty.cutyid = RTSparqAdslCust.COMQ1 " _
       &"where RTSparqAdslCust.cusid='" & ARYPARMKEY(0) & "'"
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     comaddr=""
     COMaddr=rsYY("cutnc") & rsyy("township") & rsyy("ADDR")
  else
     COMaddr=""
  end if
  RSYY.Close
  sqlYY="select b.cusnc, a.comq1 from RTSparqAdslCUST a " &_
		"inner join RTObj b on a.cusid = b.cusid Where a.CUSID='" & ARYPARMKEY(0) & "' "
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     CUSNC=rsYY("CUSNC")
     comq1xx=rsyy("comq1")
  else
     CUSNC=""
     comq1xx=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry="  RTSparqAdslCustAR.CUSID='" & ARYPARMKEY(0) & "' AND RTSparqAdslCustAR.CANCELDAT IS NULL "
     searchShow="主線︰"& comq1xx & ",社區︰" & COMN & ",主線位址︰" & COMADDR & ",用戶序號︰" & aryparmkey(0) & ",用戶名稱︰" & CUSNC
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
  
         sqlList="SELECT  RTSparqAdslCustAR.CUSID, " _
                &"RTSparqAdslCustAR.BATCHNO, " _
                &"RTCode.CODENC, RTSparqAdslCustAR.PERIOD, RTSparqAdslCustAR.AMT, RTSparqAdslCustAR.REALAMT,case when RTSparqAdslCustAR.CANCELDAT is not null then 0 else RTSparqAdslCustAR.AMT - RTSparqAdslCustAR.REALAMT end, " _
                &"RTSparqAdslCustAR.COD1, RTSparqAdslCustAR.COD2, RTSparqAdslCustAR.COD3, " _
                &"RTSparqAdslCustAR.COD4, RTSparqAdslCustAR.COD5, RTSparqAdslCustAR.CDAT, " _
                &"RTSparqAdslCustAR.MDAT, RTSparqAdslCustAR.CANCELDAT " _
                &"FROM    RTSparqAdslCustAR LEFT OUTER JOIN " _
                &"RTCode ON RTSparqAdslCustAR.ARTYPE = RTCode.CODE AND " _
                &"RTCode.KIND = 'N2' " _
                &"where  RTSparqAdslCustAR.CUSID='" & ARYPARMKEY(0) & "' AND " & searchqry & " ORDER BY RTSparqAdslCustAR.cdat "

  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>