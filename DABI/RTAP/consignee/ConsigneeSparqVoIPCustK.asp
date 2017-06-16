<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<%
if not Session("passed") then
   Response.Redirect "http://www.cbbn.com.tw/Consignee/logon.asp"
end if
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Sparq VoIP管理系統"
  title="Sparq VoIP用戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
 ' V=split(SrAccessPermit,";")
 ' AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'functionOptName="派工單;電話明細; 作 廢 ;作廢返轉;報竣異動;  退  租  "
  'functionOptProgram="KTSCUSTSNDWORKK.asp;KTSCUSTtK.asp;KTSCUSTCANCEL.asp;KTSCUSTCANCELRTN.asp;KTSCUSTCHGK.asp;KTSCUSTDROPK.asp"
  'functionOptPrompt="N;N;Y;Y;Y;N"
  functionOptName=""  
  functionOptProgram=""
  functionOptPrompt=""
 ' If V(1)="Y" then
 '    accessMode="U"
 ' Else
     accessMode="I"
 ' End IF
  DSN="DSN=RTLib"
  formatName="用戶代號;用戶名稱;VoIP電話號碼;裝機縣市;業務員;經銷商;申請日;派工日;完工日;報竣日;退租日;作廢日"
  sqlDelete="SELECT	a.CUSID, a.CUSNC, a.VOIPTEL, isnull(b.CUTNC , '')+a.TOWNSHIP2, d.CUSNC, isnull(e.shortnc, ''), "_
		   &"		a.APPLYDAT, a.WRKRCVDAT, a.FINISHDAT, a.DOCKETDAT, a.DROPDAT, a.CANCELDAT "_
		   &"FROM	RTSparqVoIPCust a "_
		   &"		left outer join RTCounty b on a.CUTID2 = b.CUTID "_
		   &"		left outer join RTEmployee c inner join RTObj d on c.CUSID = d.CUSID on c.EMPLY = a.SALESID "_
		   &"		left outer join RTObj e on e.cusid = a.consignee "_
		   &"WHERE	CUSID ='*' "_
		   &"order by a.CUSID "

  dataTable="RTSparqVoIPCust"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="ConsigneeSparqVoIPCustD.asp"
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
  searchProg="ConsigneeSparqVoIPCustS.asp"
  '----
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" and a.CUSID<>'' AND a.CANCELDAT IS NULL and a.consignee='"  & Session("UserID")& "' "
     searchShow="全部用戶(不含作廢)"
  ELSE
     SEARCHFIRST=FALSE
  End If
  sqlList="SELECT	a.CUSID, a.CUSNC, a.VOIPTEL, isnull(b.CUTNC , '')+a.TOWNSHIP2, d.CUSNC, isnull(e.shortnc, ''), "_
		   &"		a.APPLYDAT, a.WRKRCVDAT, a.FINISHDAT, a.DOCKETDAT, a.DROPDAT, a.CANCELDAT "_
		   &"FROM	RTSparqVoIPCust a "_
		   &"		left outer join RTCounty b on a.CUTID2 = b.CUTID "_
		   &"		left outer join RTEmployee c inner join RTObj d on c.CUSID = d.CUSID on c.EMPLY = a.SALESID "_
		   &"		left outer join RTObj e on e.cusid = a.consignee "_
		   &"where a.consignee='" & Session("UserID")& "' " & searchqry _
		   &" order by a.CUSID "		   
          
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>