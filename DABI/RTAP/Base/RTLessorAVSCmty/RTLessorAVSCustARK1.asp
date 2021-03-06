<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City用戶應收應付帳款查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=" 沖  帳 ;沖帳明細;帳款明細;超商未沖(Excel)"
  functionOptProgram="RTLessorAVSCustARClear.asp;RTLessorAVSCustARClearK.asp;RTLessorAVSCustARDTLK.ASP;RTLessorAVSCustArCSXls.asp"
  functionOptPrompt="Y;N;N;N"
  functionoptopen="2;1;1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;帳款編號;社區;客戶;AR/AP;期數;應沖<br>金額;已沖<br>金額;未沖<br>金額;沖帳日;沖帳員;沖立項一;沖立項二;none;產生日;作廢日;作廢員;作廢原因;退租日"
  sqlDelete="SELECT  RTLessorAVSCustAR.CUSID, RTLessorAVSCustAR.BATCHNO, RTLessorAVSCmtyH.COMN,RTLessorAVSCust.CUSNC," _
                &" RTCode.CODENC, RTLessorAVSCustAR.PERIOD,RTLessorAVSCustAR.AMT,RTLessorAVSCustAR.REALAMT," _
                &"RTLessorAVSCustAR.AMT - RTLessorAVSCustAR.REALAMT AS DIFFAMT, RTLessorAVSCustAR.MDAT, RTObj_1.CUSNC AS MUSR, " _
                &"RTLessorAVSCustAR.COD1, RTLessorAVSCustAR.COD2,RTLessorAVSCustAR.COD3, RTLessorAVSCustAR.CDAT, " _
                &"RTLessorAVSCustAR.CANCELDAT, RTObj_2.CUSNC AS CANCELUSR, " _
                &"RTLessorAVSCustAR.CANCELMEMO, RTLessorAVSCust.Dropdat " _
                &"FROM    RTLessorAVSCmtyH RIGHT OUTER JOIN RTLessorAVSCust ON RTLessorAVSCmtyH.COMQ1 = RTLessorAVSCust.COMQ1 " _
                &"RIGHT OUTER JOIN RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_2 ON RTEmployee_2.CUSID = " _
                &"RTObj_2.CUSID RIGHT OUTER JOIN RTLessorAVSCustAR ON RTEmployee_2.EMPLY = RTLessorAVSCustAR.CANCELUSR " _
                &"LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = " _
                &"RTObj_1.CUSID ON RTLessorAVSCustAR.MUSR = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
                &"RTCode ON RTLessorAVSCustAR.ARTYPE = RTCode.CODE AND RTCode.KIND = 'N2' ON RTLessorAVSCust.CUSID = " _
                &"RTLessorAVSCustAR.CUSID " _
                &"WHERE RTLessorAVSCustAR.cusid='' "
  dataTable="RTLessorAVSCUSTLOG"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg=""
  datawindowFeature=""
  searchWindowFeature="width=350,height=250,scrollbars=yes"
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
  searchProg="RTLessorAVScustARS1.ASP"
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
     searchQry="  (RTLessorAVSCustAR.AMT <> RTLessorAVSCustAR.REALAMT) and RTLessorAVSCustAR.canceldat is null "
     searchShow="全部未沖銷帳款"
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
  
         sqlList="SELECT  RTLessorAVSCustAR.CUSID, RTLessorAVSCustAR.BATCHNO, RTLessorAVSCmtyH.COMN,RTLessorAVSCust.CUSNC," _
                &"RTCode.CODENC, RTLessorAVSCustAR.PERIOD,RTLessorAVSCustAR.AMT,RTLessorAVSCustAR.REALAMT, " _
                &"RTLessorAVSCustAR.AMT - RTLessorAVSCustAR.REALAMT AS DIFFAMT, RTLessorAVSCustAR.MDAT, RTObj_1.CUSNC AS MUSR," _
                &"RTLessorAVSCustAR.COD1, " _
                &"case when RTLessorAVSCustAR.COD2 like '超商%' then '<font color =blue>'+RTLessorAVSCustAR.COD2+'</font>' " _
				&"		when RTLessorAVSCustAR.COD2 like '退租%' then '<font color =red>'+RTLessorAVSCustAR.COD2+'</font>' " _
				&"		when RTLessorAVSCustAR.COD2 like '信用卡%' then '<font color =green>'+RTLessorAVSCustAR.COD2+'</font>' " _
				&"else RTLessorAVSCustAR.COD2 end, " _
				&"RTLessorAVSCustAR.COD3, RTLessorAVSCustAR.CDAT, " _
                &"RTLessorAVSCustAR.CANCELDAT, RTObj_2.CUSNC AS CANCELUSR, RTLessorAVSCustAR.CANCELMEMO, RTLessorAVSCust.Dropdat " _
                &"FROM    RTLessorAVSCmtyH RIGHT OUTER JOIN RTLessorAVSCust ON RTLessorAVSCmtyH.COMQ1 = RTLessorAVSCust.COMQ1 " _
                &"RIGHT OUTER JOIN RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_2 ON RTEmployee_2.CUSID = " _
                &"RTObj_2.CUSID RIGHT OUTER JOIN RTLessorAVSCustAR ON RTEmployee_2.EMPLY = RTLessorAVSCustAR.CANCELUSR " _
                &"LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = " _
                &"RTObj_1.CUSID ON RTLessorAVSCustAR.MUSR = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
                &"RTCode ON RTLessorAVSCustAR.ARTYPE = RTCode.CODE AND RTCode.KIND = 'N2' ON RTLessorAVSCust.CUSID = " _
                &"RTLessorAVSCustAR.CUSID " _
                &"WHERE " & searchqry & " " _ 
                &"ORDER BY  RTLessorAVSCustAR.CDAT desc" 

  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>