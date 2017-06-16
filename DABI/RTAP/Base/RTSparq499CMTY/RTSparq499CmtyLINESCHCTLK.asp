<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="速博499管理系統"
  title="速博499主線派工作業"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="設備查詢;用戶查詢;建立派工單"
  functionOptProgram="RTSparq499CmtyHARDWAREK2.asp;RTSparq499CustK.asp;RTSparq499CmtylineSNDWORKk.asp"
  functionOptPrompt="N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;社區名稱;主線;線路IP;附掛電話;聯單號碼;none;可建置;申請日;派工單號;測通日;none;none;none"
  sqlDelete="SELECT RTSparq499CmtyLINE.COMQ1, RTSparq499CmtyLINE.LINEQ1,RTSparq499CmtyH.COMN, rtrim(convert(char(6),RTSparq499Cmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTSparq499Cmtyline.lineQ1))  as comqline, " _
           &"RTSparq499cmtyLINE.LINEIPSTR1+'.'+RTSparq499cmtyLINE.LINEIPSTR2+'.'+RTSparq499cmtyLINE.LINEIPSTR3+'.'+RTSparq499cmtyLINE.LINEIPSTR4+'-'+RTSparq499cmtyLINE.LINEIPEND,RTSparq499CmtyLINE.LINETEL,RTSparq499CmtyLINE.CHTWORKINGNO, " _
           &"RTSparq499CmtyLINE.RCVDAT," _
           &"RTSparq499CmtyLINE.AGREE, " _
           &"RTSparq499CmtyLINE.ADSLAPPLYDAT,RTSparq499CmtyLINESNDWORK.prtno, " _
           &"RTSparq499CmtyLINE.ADSLopenDAT, " _
           &"SUM(CASE WHEN RTSparq499Cust.cusid IS NOT NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
           &"SUM(CASE WHEN RTSparq499Cust.transdat IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
           &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN  " _
           &"FROM RTSalesGroup RIGHT OUTER JOIN " _
           &"RTSparq499CmtyLINE ON RTSalesGroup.AREAID = RTSparq499CmtyLINE.AREAID AND " _
           &"RTSalesGroup.GROUPID = RTSparq499CmtyLINE.GROUPID AND " _
           &"RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN " _
           &"RTCounty ON RTSparq499CmtyLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTObj ON RTSparq499CmtyLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
           &"RTSparq499Cust ON RTSparq499CmtyLINE.COMQ1 = RTSparq499Cust.COMQ1 AND " _
           &"RTSparq499CmtyLINE.LINEQ1 = RTSparq499CmtyLINESNDWORK.LINEQ1 AND " _           
           &"RTSparq499CmtyLINE.LINEQ1 = RTSparq499Cust.LINEQ1  inner join RTSparq499Cmtyh on RTSparq499Cmtyline.comq1=RTSparq499Cmtyh.comq1 " _
           &"WHERE RTSparq499CmtyLINE.COMQ1= 0 " _                
           &"GROUP BY RTSparq499CmtyLINE.COMQ1, RTSparq499CmtyLINE.LINEQ1,RTSparq499CmtyH.COMN, rtrim(convert(varchar(10),RTSparq499Cmtyline.COMQ1)) +'-'+ rtrim(convert(varchar(10),RTSparq499Cmtyline.lineQ1)), " _
           &"RTSalesGroup.GROUPNC, RTSparq499cmtyLINE.LINEIPSTR1+'.'+RTSparq499cmtyLINE.LINEIPSTR2+'.'+RTSparq499cmtyLINE.LINEIPSTR3+'.'+RTSparq499cmtyLINE.LINEIPSTR4+'-'+RTSparq499cmtyLINE.LINEIPEND,RTSparq499CmtyLINE.LINETEL,RTSparq499CmtyLINE.CHTWORKINGNO, " _
           &"RTSparq499CmtyLINE.RCVDAT," _
           &"RTSparq499CmtyLINE.AGREE, " _
           &"RTSparq499CmtyLINE.ADSLAPPLYDAT,RTSparq499CmtyLINESNDWORK.prtno,  " _
           &"RTSparq499CmtyLINE.ADSLopenDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END " 

  dataTable="RTSparq499Cmtyline"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTSparq499CmtylineD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=true
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTSparq499CmtyLINESCHCTLS.ASP"
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
     searchQry=" RTSparq499Cmtyline.LINEIPSTR1 <> '' and RTSparq499Cmtyline.adslopendat is null  "
     searchShow="已提出線路申請(有IP)，且線路尚未開通之主線清單 "
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
         case "C"
            DAreaID="='A2'"         
         case "P"
            DAreaID="='A1'"                        
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
    If searchShow="全部" Then
         sqlList="SELECT RTSparq499CmtyLINE.COMQ1, RTSparq499CmtyLINE.LINEQ1,RTSparq499CmtyH.COMN, rtrim(convert(varchar(10),RTSparq499Cmtyline.COMQ1)) +'-'+ rtrim(convert(varchar(10),RTSparq499Cmtyline.lineQ1))  as comqline, " _
           &"RTSparq499cmtyLINE.LINEIPSTR1+'.'+RTSparq499cmtyLINE.LINEIPSTR2+'.'+RTSparq499cmtyLINE.LINEIPSTR3+'.'+RTSparq499cmtyLINE.LINEIPSTR4+'-'+RTSparq499cmtyLINE.LINEIPEND,RTSparq499CmtyLINE.LINETEL,RTSparq499CmtyLINE.CHTWORKINGNO, " _
           &"RTSparq499CmtyLINE.RCVDAT," _
           &"RTSparq499CmtyLINE.AGREE, " _
           &"RTSparq499CmtyLINE.ADSLAPPLYDAT,RTSparq499CmtyLINESNDWORK.prtno, " _
           &"RTSparq499CmtyLINE.ADSLopenDAT, " _
           &"SUM(CASE WHEN RTSparq499Cust.cusid IS NOT NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
           &"SUM(CASE WHEN RTSparq499Cust.transdat IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
           &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN " _
           &"FROM RTSalesGroup RIGHT OUTER JOIN " _
           &"RTSparq499CmtyLINE ON RTSalesGroup.AREAID = RTSparq499CmtyLINE.AREAID AND " _
           &"RTSalesGroup.GROUPID = RTSparq499CmtyLINE.GROUPID AND " _
           &"RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN " _
           &"RTCounty ON RTSparq499CmtyLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTObj ON RTSparq499CmtyLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
           &"RTSparq499Cust ON RTSparq499CmtyLINE.COMQ1 = RTSparq499Cust.COMQ1 AND " _
           &"RTSparq499CmtyLINE.LINEQ1 = RTSparq499Cust.LINEQ1  inner join RTSparq499Cmtyh on RTSparq499Cmtyline.comq1=RTSparq499Cmtyh.comq1 " _
           &"LEFT OUTER JOIN RTSparq499CmtyLINESNDWORK ON RTSparq499CmtyLINE.COMQ1 = RTSparq499CmtyLINESNDWORK.COMQ1 AND " _
           &"RTSparq499CmtyLINE.LINEQ1 = RTSparq499CmtyLINESNDWORK.LINEQ1 AND " _           
           &"RTSparq499CmtyLINESNDWORK.DROPDAT IS NULL  AND RTSparq499CmtyLINESNDWORK.UNCLOSEDAT IS NULL " _
           &"WHERE RTSparq499CmtyLINE.COMQ1<> 0 AND " & SEARCHQRY & " " _
           &"GROUP BY RTSparq499CmtyLINE.COMQ1, RTSparq499CmtyLINE.LINEQ1,RTSparq499CmtyH.COMN, rtrim(convert(varchar(10),RTSparq499Cmtyline.COMQ1)) +'-'+ rtrim(convert(varchar(10),RTSparq499Cmtyline.lineQ1)), " _
           &"RTSalesGroup.GROUPNC, RTSparq499cmtyLINE.LINEIPSTR1+'.'+RTSparq499cmtyLINE.LINEIPSTR2+'.'+RTSparq499cmtyLINE.LINEIPSTR3+'.'+RTSparq499cmtyLINE.LINEIPSTR4+'-'+RTSparq499cmtyLINE.LINEIPEND,RTSparq499CmtyLINE.LINETEL,RTSparq499CmtyLINE.CHTWORKINGNO, " _
           &"RTSparq499CmtyLINE.RCVDAT," _
           &"RTSparq499CmtyLINE.AGREE, " _
           &"RTSparq499CmtyLINE.ADSLAPPLYDAT,RTSparq499CmtyLINESNDWORK.prtno, " _
           &"RTSparq499CmtyLINE.ADSLopenDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END "
                       
    Else
         sqlList="SELECT RTSparq499CmtyLINE.COMQ1, RTSparq499CmtyLINE.LINEQ1,RTSparq499CmtyH.COMN, rtrim(ltrim(convert(varchar(10),RTSparq499Cmtyline.COMQ1))) +'-'+ rtrim(ltrim(convert(varchar(10),RTSparq499Cmtyline.lineQ1)))  as comqline, " _
           &"RTSparq499cmtyLINE.LINEIPSTR1+'.'+RTSparq499cmtyLINE.LINEIPSTR2+'.'+RTSparq499cmtyLINE.LINEIPSTR3+'.'+RTSparq499cmtyLINE.LINEIPSTR4+'-'+RTSparq499cmtyLINE.LINEIPEND,RTSparq499CmtyLINE.LINETEL,RTSparq499CmtyLINE.CHTWORKINGNO, " _
           &"RTSparq499CmtyLINE.RCVDAT," _
           &"RTSparq499CmtyLINE.AGREE, " _
           &"RTSparq499CmtyLINE.ADSLAPPLYDAT,RTSparq499CmtyLINESNDWORK.prtno, " _
           &"RTSparq499CmtyLINE.ADSLopenDAT, " _
           &"SUM(CASE WHEN RTSparq499Cust.cusid IS NOT NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
           &"SUM(CASE WHEN RTSparq499Cust.transdat IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
           &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN " _
           &"FROM RTSalesGroup RIGHT OUTER JOIN " _
           &"RTSparq499CmtyLINE ON RTSalesGroup.AREAID = RTSparq499CmtyLINE.AREAID AND " _
           &"RTSalesGroup.GROUPID = RTSparq499CmtyLINE.GROUPID AND " _
           &"RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN " _
           &"RTCounty ON RTSparq499CmtyLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTObj ON RTSparq499CmtyLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
           &"RTSparq499Cust ON RTSparq499CmtyLINE.COMQ1 = RTSparq499Cust.COMQ1 AND " _
           &"RTSparq499CmtyLINE.LINEQ1 = RTSparq499Cust.LINEQ1  inner join RTSparq499Cmtyh on RTSparq499Cmtyline.comq1=RTSparq499Cmtyh.comq1 " _
           &"LEFT OUTER JOIN RTSparq499CmtyLINESNDWORK ON RTSparq499CmtyLINE.COMQ1 = RTSparq499CmtyLINESNDWORK.COMQ1 AND " _
           &"RTSparq499CmtyLINE.LINEQ1 = RTSparq499CmtyLINESNDWORK.LINEQ1 AND " _
           &"RTSparq499CmtyLINESNDWORK.DROPDAT IS NULL AND RTSparq499CmtyLINESNDWORK.UNCLOSEDAT IS NULL " _
           &"WHERE RTSparq499CmtyLINE.COMQ1<> 0 AND " & SEARCHQRY & " " _
           &"GROUP BY RTSparq499CmtyLINE.COMQ1, RTSparq499CmtyLINE.LINEQ1,RTSparq499CmtyH.COMN, rtrim(ltrim(convert(varchar(10),RTSparq499Cmtyline.COMQ1))) +'-'+ rtrim(ltrim(convert(varchar(10),RTSparq499Cmtyline.lineQ1))), " _
           &"RTSalesGroup.GROUPNC, RTSparq499cmtyLINE.LINEIPSTR1+'.'+RTSparq499cmtyLINE.LINEIPSTR2+'.'+RTSparq499cmtyLINE.LINEIPSTR3+'.'+RTSparq499cmtyLINE.LINEIPSTR4+'-'+RTSparq499cmtyLINE.LINEIPEND,RTSparq499CmtyLINE.LINETEL,RTSparq499CmtyLINE.CHTWORKINGNO, " _
           &"RTSparq499CmtyLINE.RCVDAT," _
           &"RTSparq499CmtyLINE.AGREE, " _
           &"RTSparq499CmtyLINE.ADSLAPPLYDAT,RTSparq499CmtyLINESNDWORK.prtno, " _
           &"RTSparq499CmtyLINE.ADSLopenDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END " 
                      
    End If  
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>