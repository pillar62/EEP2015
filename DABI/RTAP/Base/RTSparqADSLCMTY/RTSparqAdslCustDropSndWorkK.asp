<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Sparq* 管理系統"
  title="速博ADSL退租戶拆機派工作業"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=" 列 印 ;完工結案;未完工結案;結案返轉; 作 廢 ;作廢返轉;歷史異動"
  functionOptProgram="RTSparqAdslCustDropSndPV.asp;RTSparqadslcustdropsndworkF.asp;RTSparqadslcustdropsndworkUF.asp;RTSparqadslcustdropsndworkFR.asp;RTSparqadslcustdropsndworkdrop.asp;RTSparqadslcustdropsndworkdropc.asp;RTSparqadslcustdropsndworkLOGK.asp"
  functionOptPrompt="N;Y;Y;Y;Y;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;社區名稱;派工單號;派工日期;預定施工員;實際施工員;拆機完成日;完工結案日;未完工結案日;作廢日;獎金結算月;獎金審核日;庫存結算月;庫存審核日"
  sqlDelete="SELECT RTSparqadslcustdropsndwork.CUSID,RTSparqadslcustdropsndwork.entryno, RTSparqadslcustdropsndwork.PRTNO, RTSPARQADSLCMTY.COMN, RTSparqadslcustdropsndwork.PRTNO, " _
           &"RTSparqadslcustdropsndwork.SENDWORKDAT,  CASE WHEN rtobj.cusnc <> '' THEN rtobj.cusnc ELSE rtobj_1.shortnc END, " _
           &"CASE WHEN rtobj_2.cusnc <> '' THEN rtobj_2.cusnc ELSE rtobj_3.shortnc END, " _
           &"RTSparqadslcustdropsndwork.CLOSEDAT,RTSparqadslcustdropsndwork.finishDAT,RTSparqadslcustdropsndwork.UNCLOSEDAT,RTSparqadslcustdropsndwork.DROPDAT,RTSparqadslcustdropsndwork.BONUSCLOSEYM, RTSparqadslcustdropsndwork.BONUSFINCHK, RTSparqadslcustdropsndwork.STOCKCLOSEYM, " _
           &"RTSparqadslcustdropsndwork.STOCKFINCHK " _
           &"FROM  RTObj RTObj_3 RIGHT OUTER JOIN RTSparqadslcustdropsndwork ON " _
           &"RTObj_3.CUSID = RTSparqadslcustdropsndwork.REALCONSIGNEE LEFT OUTER JOIN RTEmployee RTEmployee_1 LEFT OUTER JOIN " _
           &"RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON RTSparqadslcustdropsndwork.REALENGINEER = RTEmployee_1.EMPLY " _
           &"LEFT OUTER JOIN RTObj RTObj_1 ON RTSparqadslcustdropsndwork.ASSIGNCONSIGNEE = RTObj_1.CUSID LEFT OUTER JOIN " _
           &"RTObj RIGHT OUTER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
           &"RTSparqadslcustdropsndwork.ASSIGNENGINEER = RTEmployee.EMPLY " _
           &"LEFT OUTER JOIN RTSPARQADSLCMTY ON RTSparqadslcustdropsndwork.comq1 = RTSPARQADSLCMTY.CUTYID " 
  dataTable="RTSparqadslcustdropsndwork"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="RTSparqadslcustdropsndworkD.asp"
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
'     searchQry=" RTCmty.CUTYID=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.CUTYID <> 0
  '----
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select * from rtobj where cusid='" & ARYPARMKEY(0) & "' "
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     cusnc=rsYY("cusnc")
  else
     cusnc=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTSparqadslcustdropsndwork.CUSID='" & ARYPARMKEY(0) & "' AND RTSparqadslcustdropsndwork.ENTRYNO=" & ARYPARMKEY(1)
     searchShow="用戶名稱︰" & cusnc
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
  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
  
         sqlList="SELECT RTSparqadslcustdropsndwork.CUSID,RTSparqadslcustdropsndwork.entryno, RTSparqadslcustdropsndwork.PRTNO, RTSPARQADSLCMTY.COMN, RTSparqadslcustdropsndwork.PRTNO,  " _
           &"RTSparqadslcustdropsndwork.SENDWORKDAT,  CASE WHEN rtobj.cusnc <> '' THEN rtobj.cusnc ELSE rtobj_1.shortnc END, " _
           &"CASE WHEN rtobj_2.cusnc <> '' THEN rtobj_2.cusnc ELSE rtobj_3.shortnc END, " _
           &"RTSparqadslcustdropsndwork.CLOSEDAT,RTSparqadslcustdropsndwork.finishDAT,RTSparqadslcustdropsndwork.UNCLOSEDAT,RTSparqadslcustdropsndwork.DROPDAT,RTSparqadslcustdropsndwork.BONUSCLOSEYM, RTSparqadslcustdropsndwork.BONUSFINCHK, RTSparqadslcustdropsndwork.STOCKCLOSEYM, " _
           &"RTSparqadslcustdropsndwork.STOCKFINCHK " _
           &"FROM  RTObj RTObj_3 RIGHT OUTER JOIN RTSparqadslcustdropsndwork ON " _
           &"RTObj_3.CUSID = RTSparqadslcustdropsndwork.REALCONSIGNEE LEFT OUTER JOIN RTEmployee RTEmployee_1 LEFT OUTER JOIN " _
           &"RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON RTSparqadslcustdropsndwork.REALENGINEER = RTEmployee_1.EMPLY " _
           &"LEFT OUTER JOIN RTObj RTObj_1 ON RTSparqadslcustdropsndwork.ASSIGNCONSIGNEE = RTObj_1.CUSID LEFT OUTER JOIN " _
           &"RTObj RIGHT OUTER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
           &"RTSparqadslcustdropsndwork.ASSIGNENGINEER = RTEmployee.EMPLY " _
           &"LEFT OUTER JOIN RTSPARQADSLCMTY ON RTSparqadslcustdropsndwork.comq1 = RTSPARQADSLCMTY.CUTYID " _
           &"where " & searchqry
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>