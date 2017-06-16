<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="So-net管理系統"
  title="合約資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  if ARYPARMKEY(0) ="" then
    ButtonEnable="N;N;Y;Y;Y;Y"
  else
    ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  end if
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName="作　廢"
  functionOptProgram="RTSonetContractCancel.asp"
  functionOptPrompt="Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  dataTable="RTContract"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTSonetContractD.asp"
  datawindowFeature=""
  searchWindowFeature="width=450,height=350,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=30
  searchProg=""

  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" a.CTNO>=0 "
     searchShow="全部"
  End If

    if ARYPARMKEY(0) <>"" then 
		searchQry = searchQry & " and a.ComQ1=" & aryparmkey(0)
	end if

  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  'Response.Write "GP=" & usergroup
  	formatName="社區<br>序號;合約<br>序號;社區名稱;合約<br>起始日;合約<br>到期日;電費結算<br>週期(月);計算方式;合約作廢日"
	'-------------------------------------------------------------------------------------------  
  	sqlDelete="select i_smallint, i_int, c_varchar, d_datetime, d_datetime, i_tinyint, c_varchar, d_datetime " &_
			"from RTTemplate "
    sqlList="select a.COMQ1, a.CTNO, b.comn, a.STRDAT, a.ENDDAT, a.PERIOD, case a.counttype " &_
			"when '01' then convert(varchar(5),a.meterrate)+' 元/每度電' " &_
			"when '02' then convert(varchar(5),a.mpay)+' 元/每月' " &_
			"when '03' then convert(varchar(5),a.mpay)+' 元/每月；'+convert(varchar(5),a.custnumup)+'戶以上：'+ convert(varchar(5),a.mpay2)+' 元/每月' " &_
			"when '04' then convert(varchar(5),a.mpay)+' 元/每月；'+convert(varchar(5),a.custnumup)+'戶以上,每增加'+convert(varchar(5),a.custnuminc)+'戶：加'+ convert(varchar(5),a.mpay2)+' 元/每月' " &_
			"else 'N/A' end, canceldat " &_
			"from RTContract a " &_
			"inner join RTSonetCmtyH b on a.comq1 = b.comq1 " &_
			"where " & searchqry &_
			" ORDER BY a.CTNO "
 'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>