<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Sparq VoIP管理系統"
  title="Sparq VoIP用戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  'ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  ButtonEnable="Y;N;Y;Y;Y;Y"
  functionOptName=" 異 動 "
  functionOptProgram="RTSparqVoIPCustChgK.asp"
  functionOptPrompt="N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;營運點;NCIC<br>用戶號碼;用戶名稱;VoIP<br>電話號碼;裝機縣市;業務員;經銷商;經銷業務;申請日;派工日;完工日;報竣日;退租日;作廢日"
  sqlDelete="SELECT	a.CUSID, a.NCICCUSNO, a.CUSNC, a.VOIPTEL, isnull(b.CUTNC , '')+a.TOWNSHIP2, d.CUSNC, isnull(e.shortnc, ''), "_
		   &"		a.CONSIGNEESALE, a.APPLYDAT, a.WRKRCVDAT, a.FINISHDAT, a.DOCKETDAT, a.DROPDAT, a.CANCELDAT "_
		   &"FROM	RTSparqVoIPCust a "_
		   &"		left outer join RTCounty b on a.CUTID2 = b.CUTID "_
		   &"		left outer join RTEmployee c inner join RTObj d on c.CUSID = d.CUSID on c.EMPLY = a.SALESID "_
		   &"		left outer join RTObj e on e.cusid = a.consignee "_
		   &"WHERE	CUSID ='*' "_
		   &"order by a.NCICCUSNO "

  dataTable="RTSparqVoIPCust"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="RTSparqVoIPCustD.asp"
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
  searchProg="RTSparqVoIPCustS.asp"
  '----
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" a.CUSID<>'' "
     searchShow="全部用戶(不含作廢)"
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
  'if Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 or userlevel =1 or userlevel =5 or userlevel =9 then DAreaID="<>'*'"
  
    If searchShow="全部" Then
	sqlList="SELECT	a.CUSID, " _
	     &"CASE WHEN a.CONSIGNEE<>'' THEN e.SHORTNC ELSE  " _
	     &"case when RTCTYTOWN.operationname=''  OR RTCTYTOWN.operationname IS NULL then " _
       &"CASE WHEN a.cutid2 IN ('08','09','10','11','12','13') THEN '第十二營運點' " _
       &"WHEN   a.cutid2 IN ('14','15','16','17','18','19','20','21','23') THEN '第十三營運點' " _
       &"ELSE '無法歸屬' END eLSE RTCTYTOWN.operationname END  END," _
	     &"a.NCICCUSNO, substring(a.CUSNC,1,6)+'....', a.VOIPTEL, isnull(b.CUTNC , '')+a.TOWNSHIP2, d.CUSNC, isnull(e.shortnc, ''), "_
		   &"		a.CONSIGNEESALE, a.APPLYDAT, a.WRKRCVDAT, a.FINISHDAT, a.DOCKETDAT, a.DROPDAT, a.CANCELDAT "_
		   &"FROM	RTSparqVoIPCust a "_
		   &"		left outer join RTCounty b on a.CUTID2 = b.CUTID "_
		   &"		left outer join RTEmployee c inner join RTObj d on c.CUSID = d.CUSID on c.EMPLY = a.SALESID "_
		   &"		left outer join RTObj e on e.cusid = a.consignee "_
		   &" left outer join rtctytown on a.cutid2=rtctytown.cutid and a.township2=rtctytown.township " _
		   &"where " & searchqry _
		   &" order by a.NCICCUSNO "		   
    Else 
	sqlList="SELECT	a.CUSID, " _
	     &"CASE WHEN a.CONSIGNEE<>'' THEN e.SHORTNC ELSE  " _
	     &"case when RTCTYTOWN.operationname=''  OR RTCTYTOWN.operationname IS NULL then " _
       &"CASE WHEN a.cutid2 IN ('08','09','10','11','12','13') THEN '第十二營運點' " _
       &"WHEN   a.cutid2 IN ('14','15','16','17','18','19','20','21','23') THEN '第十三營運點' " _
       &"ELSE '無法歸屬' END eLSE RTCTYTOWN.operationname END  END," _
	     &"a.NCICCUSNO, substring(a.CUSNC,1,6)+'....', a.VOIPTEL, isnull(b.CUTNC , '')+a.TOWNSHIP2, d.CUSNC, isnull(e.shortnc, ''), "_
		   &"		CONSIGNEESALE, a.APPLYDAT, a.WRKRCVDAT, a.FINISHDAT, a.DOCKETDAT, a.DROPDAT, a.CANCELDAT "_
		   &"FROM	RTSparqVoIPCust a "_
		   &"		left outer join RTCounty b on a.CUTID2 = b.CUTID "_
		   &"		left outer join RTEmployee c inner join RTObj d on c.CUSID = d.CUSID on c.EMPLY = a.SALESID "_
		   &"		left outer join RTObj e on e.cusid = a.consignee "_
		   &" left outer join rtctytown on a.cutid2=rtctytown.cutid and a.township2=rtctytown.township " _
		   &"where " & searchqry _
		   &" order by a.NCICCUSNO "		   
          
    End If  
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>