<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="速博Sparq*管理系統"
  title="[有話好說]輕鬆省專案客戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="0809電話維護; 作  廢 ;作廢返轉; 異 動 "
  functionOptProgram="RTSparq0809CustTELK.asp;RTSparq0809custcancel.asp;RTSparq0809custcancelC.asp;RTSparq0809CustChgK.asp"
  functionOptPrompt="N;Y;Y;N"
  EMAILFIELDNO=6
  EMAILFIELDFLAG="Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  'formatName="用戶ID;用戶名稱;身份證號;出生日;行動電話;傳真;EMAIL;帳單地址;申請日;作廢日;退租日"
  formatName="none;營運點;用戶ID;用戶名稱;none;出生日;行動電話;傳真;EMAIL;帳單地址;申請日;作廢日;退租日"
  sqlDelete="SELECT RTSparq0809Cust.CUSID, RTSparq0809Cust.CUSNC, RTSparq0809Cust.SOCIALID, RTSparq0809Cust.BIRTHDAY, " _
           &"RTSparq0809Cust.MOBILE, RTSparq0809Cust.FAX1+'-'+RTSparq0809Cust.FAX12, RTSparq0809Cust.EMAIL, " _
           &"RTCounty.CUTNC + RTSparq0809Cust.TOWNSHIP2 + RTSparq0809Cust.RADDR2, RTSparq0809Cust.APPLYDAT, RTSparq0809Cust.CANCELDAT, RTSparq0809Cust.DROPDAT " _
           &"FROM RTSparq0809Cust LEFT OUTER JOIN RTCounty ON RTSparq0809Cust.CUTID2 = RTCounty.CUTID "            
  dataTable="RTSparq0809Cust"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="RTSparq0809CustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=315,scrollbars=yes"
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
  searchProg="RTSparq0809CustS.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTSparq0809Cust.CUSID<>'' "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
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
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  Domain=Mid(Emply,1,1)
  select case Domain
         case "T"
            DAreaID="='A1'"
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
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076" or Ucase(emply)="T91129" or _
     Ucase(emply)="T92134" or Ucase(emply)="T89031" or Ucase(emply)="P95032" then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  'if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
 if userlevel=31 then DAreaID="<>'*'"  
         sqlList="SELECT RTSparq0809Cust.CUSID," _
           &"CASE WHEN RTSparq0809Cust.CONSIGNEE<>'' tHEN RTOBJ.SHORTNC ELSE  " _
           &"case when RTCTYTOWNx.operationname=''  OR RTCTYTOWNx.operationname IS NULL then " _
           &"CASE WHEN RTSparq0809Cust.cutid1 IN ('08','09','10','11','12','13') THEN '第十二營運點' " _
           &"WHEN   RTSparq0809Cust.cutid1 IN ('14','15','16','17','18','19','20','21','23') THEN '第十三營運點' " _
           &"ELSE '無法歸屬' END ELSE RTCTYTOWNx.operationname END  END," _
           &"case when len(RTSparq0809Cust.CUSNC) > 5 then substring(RTSparq0809Cust.CUSNC,1,5)+ '...' else RTSparq0809Cust.CUSNC end , RTSparq0809Cust.SOCIALID, RTSparq0809Cust.BIRTHDAY, " _
           &"RTSparq0809Cust.MOBILE, RTSparq0809Cust.FAX1+'-'+RTSparq0809Cust.FAX12, RTSparq0809Cust.EMAIL, " _
           &"RTCounty.CUTNC + RTSparq0809Cust.TOWNSHIP2 + RTSparq0809Cust.RADDR2, RTSparq0809Cust.APPLYDAT, RTSparq0809Cust.CANCELDAT, RTSparq0809Cust.DROPDAT " _
           &"FROM RTSparq0809Cust LEFT OUTER JOIN RTCounty ON RTSparq0809Cust.CUTID2 = RTCounty.CUTID " _
           &"left outer join rtobj on RTSparq0809Cust.consignee=rtobj.cusid " _
           &"left outer join rtctytown rtctytownx on RTSparq0809Cust.cutid1=rtctytownx.cutid and RTSparq0809Cust.township1=rtctytownx.township " _
           &"WHERE " & SEARCHQRY        
  '  End If  
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()
End Sub
%>