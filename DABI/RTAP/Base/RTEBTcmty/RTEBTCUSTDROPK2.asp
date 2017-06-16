<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="AVS用戶服務終止(退租)作業維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="拆機工單; 作 廢 ;作廢返轉;異動查詢"
  functionOptProgram="rtebtcustdropsndworkk2.asp;rtebtCUSTdropDROP.asp;rtebtCUSTdropDROPC.asp;rtebtCUSTdropLOGK.asp"
  functionOptPrompt="N;Y;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;none;主線;社區名稱;用戶名稱;退租申請日;預定退租日;作廢日;none;退租申請確認日;退租申請轉檔日;none;拆機完成日;拆機回報確認日;拆機回報轉檔日;拆機單號;派工日;預定拆機員"
  sqlDelete="SELECT  RTEBTCUSTDROP.COMQ1, RTEBTCUSTDROP.LINEQ1, RTEBTCUSTDROP.CUSID, RTEBTCUSTDROP.ENTRYNO,rtrim(convert(char(6),RTEBTCUSTDROP.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTCUSTDROP.lineQ1)) , RTEBTCMTYH.COMN,RTEBTCUST.CUSNC," _
                &"RTEBTCUSTDROP.APPLYDAT, RTEBTCUSTDROP.EXPECTDAT, RTEBTCUSTDROP.DROPDAT, RTObj_3.CUSNC, " _
                &"RTEBTCUSTDROP.TRANSCHKDAT, RTEBTCUSTDROP.TRANSDAT, RTEBTCUSTDROP.TRANSNO, RTEBTCUSTDROP.FINISHDAT, " _
                &"RTEBTCUSTDROP.FINISHCHKDAT, RTEBTCUSTDROP.FINISHTNSDAT, RTEBTCUSTDROPSNDWORK.PRTNO, " _
                &"RTEBTCUSTDROPSNDWORK.SENDWORKDAT, CASE WHEN RTObj_1.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_1.CUSNC END " _
                &"FROM  RTEBTCMTYH RIGHT OUTER JOIN RTEBTCUSTDROP ON RTEBTCMTYH.COMQ1 = RTEBTCUSTDROP.COMQ1 LEFT OUTER JOIN " _
                &"RTEBTCUST ON RTEBTCUSTDROP.COMQ1 = RTEBTCUST.COMQ1 AND RTEBTCUSTDROP.LINEQ1 = RTEBTCUST.LINEQ1 AND " _
                &"RTEBTCUSTDROP.CUSID = RTEBTCUST.CUSID LEFT OUTER JOIN RTObj RTObj_2 RIGHT OUTER JOIN RTEBTCUSTDROPSNDWORK ON " _
                &"RTObj_2.CUSID = RTEBTCUSTDROPSNDWORK.ASSIGNCONSIGNEE LEFT OUTER JOIN RTObj RTObj_1 INNER JOIN " _
                &"RTEmployee RTEmployee_1 ON RTObj_1.CUSID = RTEmployee_1.CUSID ON " _
                &"RTEBTCUSTDROPSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY ON RTEBTCUSTDROP.COMQ1 = RTEBTCUSTDROPSNDWORK.COMQ1 AND " _
                &"RTEBTCUSTDROP.LINEQ1 = RTEBTCUSTDROPSNDWORK.LINEQ1 AND RTEBTCUSTDROP.CUSID = RTEBTCUSTDROPSNDWORK.CUSID AND " _
                &"RTEBTCUSTDROP.ENTRYNO = RTEBTCUSTDROPSNDWORK.ENTRYNO AND RTEBTCUSTDROPSNDWORK.DROPDAT IS NULL LEFT OUTER JOIN " _
                &"RTObj RTObj_3 INNER JOIN RTEmployee RTEmployee_2 ON RTObj_3.CUSID = RTEmployee_2.CUSID ON " _
                &"RTEBTCUSTDROP.DROPUSR = RTEmployee_2.EMPLY WHERE RTEBTCUSTDROP.COMQ1=0" 
  dataTable="RTEBTCUSTDROP"
  userDefineDelete="Yes"
  numberOfKey=4
  dataProg="RTebtCUSTDROPD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=TRUE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTebtCUSTDROPs.asp"
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
     searchQry=" RTEBTCUSTDROP.ComQ1<>0 and RTEBTCUSTDROP.FINISHCHKDAT is null "
     searchShow="全部(尚未拆機回報)"
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
  
    If searchShow="全部" Then
         sqlList="SELECT  RTEBTCUSTDROP.COMQ1, RTEBTCUSTDROP.LINEQ1, RTEBTCUSTDROP.CUSID, RTEBTCUSTDROP.ENTRYNO,rtrim(convert(char(6),RTEBTCUSTDROP.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTCUSTDROP.lineQ1)) , RTEBTCMTYH.COMN,RTEBTCUST.CUSNC," _
                &"RTEBTCUSTDROP.APPLYDAT, RTEBTCUSTDROP.EXPECTDAT, RTEBTCUSTDROP.DROPDAT, RTObj_3.CUSNC, " _
                &"RTEBTCUSTDROP.TRANSCHKDAT, RTEBTCUSTDROP.TRANSDAT, RTEBTCUSTDROP.TRANSNO, RTEBTCUSTDROP.FINISHDAT, " _
                &"RTEBTCUSTDROP.FINISHCHKDAT, RTEBTCUSTDROP.FINISHTNSDAT, RTEBTCUSTDROPSNDWORK.PRTNO, " _
                &"RTEBTCUSTDROPSNDWORK.SENDWORKDAT, CASE WHEN RTObj_1.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_1.CUSNC END " _
                &"FROM  RTEBTCMTYH RIGHT OUTER JOIN RTEBTCUSTDROP ON RTEBTCMTYH.COMQ1 = RTEBTCUSTDROP.COMQ1 LEFT OUTER JOIN " _
                &"RTEBTCUST ON RTEBTCUSTDROP.COMQ1 = RTEBTCUST.COMQ1 AND RTEBTCUSTDROP.LINEQ1 = RTEBTCUST.LINEQ1 AND " _
                &"RTEBTCUSTDROP.CUSID = RTEBTCUST.CUSID LEFT OUTER JOIN RTObj RTObj_2 RIGHT OUTER JOIN RTEBTCUSTDROPSNDWORK ON " _
                &"RTObj_2.CUSID = RTEBTCUSTDROPSNDWORK.ASSIGNCONSIGNEE LEFT OUTER JOIN RTObj RTObj_1 INNER JOIN " _
                &"RTEmployee RTEmployee_1 ON RTObj_1.CUSID = RTEmployee_1.CUSID ON " _
                &"RTEBTCUSTDROPSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY ON RTEBTCUSTDROP.COMQ1 = RTEBTCUSTDROPSNDWORK.COMQ1 AND " _
                &"RTEBTCUSTDROP.LINEQ1 = RTEBTCUSTDROPSNDWORK.LINEQ1 AND RTEBTCUSTDROP.CUSID = RTEBTCUSTDROPSNDWORK.CUSID AND " _
                &"RTEBTCUSTDROP.ENTRYNO = RTEBTCUSTDROPSNDWORK.ENTRYNO AND RTEBTCUSTDROPSNDWORK.DROPDAT IS NULL LEFT OUTER JOIN " _
                &"RTObj RTObj_3 INNER JOIN RTEmployee RTEmployee_2 ON RTObj_3.CUSID = RTEmployee_2.CUSID ON " _
                &"RTEBTCUSTDROP.DROPUSR = RTEmployee_2.EMPLY " _
                &"where " & searchqry
    Else
         sqlList="SELECT  RTEBTCUSTDROP.COMQ1, RTEBTCUSTDROP.LINEQ1, RTEBTCUSTDROP.CUSID, RTEBTCUSTDROP.ENTRYNO,rtrim(convert(char(6),RTEBTCUSTDROP.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTCUSTDROP.lineQ1)) , RTEBTCMTYH.COMN,RTEBTCUST.CUSNC, " _
                &"RTEBTCUSTDROP.APPLYDAT, RTEBTCUSTDROP.EXPECTDAT, RTEBTCUSTDROP.DROPDAT, RTObj_3.CUSNC, " _
                &"RTEBTCUSTDROP.TRANSCHKDAT, RTEBTCUSTDROP.TRANSDAT, RTEBTCUSTDROP.TRANSNO, RTEBTCUSTDROP.FINISHDAT, " _
                &"RTEBTCUSTDROP.FINISHCHKDAT, RTEBTCUSTDROP.FINISHTNSDAT, RTEBTCUSTDROPSNDWORK.PRTNO, " _
                &"RTEBTCUSTDROPSNDWORK.SENDWORKDAT, CASE WHEN RTObj_1.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_1.CUSNC END " _
                &"FROM  RTEBTCMTYH RIGHT OUTER JOIN RTEBTCUSTDROP ON RTEBTCMTYH.COMQ1 = RTEBTCUSTDROP.COMQ1 LEFT OUTER JOIN " _
                &"RTEBTCUST ON RTEBTCUSTDROP.COMQ1 = RTEBTCUST.COMQ1 AND RTEBTCUSTDROP.LINEQ1 = RTEBTCUST.LINEQ1 AND " _
                &"RTEBTCUSTDROP.CUSID = RTEBTCUST.CUSID LEFT OUTER JOIN RTObj RTObj_2 RIGHT OUTER JOIN RTEBTCUSTDROPSNDWORK ON " _
                &"RTObj_2.CUSID = RTEBTCUSTDROPSNDWORK.ASSIGNCONSIGNEE LEFT OUTER JOIN RTObj RTObj_1 INNER JOIN " _
                &"RTEmployee RTEmployee_1 ON RTObj_1.CUSID = RTEmployee_1.CUSID ON " _
                &"RTEBTCUSTDROPSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY ON RTEBTCUSTDROP.COMQ1 = RTEBTCUSTDROPSNDWORK.COMQ1 AND " _
                &"RTEBTCUSTDROP.LINEQ1 = RTEBTCUSTDROPSNDWORK.LINEQ1 AND RTEBTCUSTDROP.CUSID = RTEBTCUSTDROPSNDWORK.CUSID AND " _
                &"RTEBTCUSTDROP.ENTRYNO = RTEBTCUSTDROPSNDWORK.ENTRYNO AND RTEBTCUSTDROPSNDWORK.DROPDAT IS NULL LEFT OUTER JOIN " _
                &"RTObj RTObj_3 INNER JOIN RTEmployee RTEmployee_2 ON RTObj_3.CUSID = RTEmployee_2.CUSID ON " _
                &"RTEBTCUSTDROP.DROPUSR = RTEmployee_2.EMPLY " _
                &"where " & searchqry
    End If  
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>