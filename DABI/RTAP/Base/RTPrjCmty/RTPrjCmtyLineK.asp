<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="專案社區管理系統"
  title="專案社區主線資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  if len(ARYPARMKEY(0))=0  Then
	buttonEnable="N;N;Y;Y;Y;Y"  
  else
	ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  end if	
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'functionOptName="主線派工;設備查詢;用戶維護;客服案件;到期續約;撤線作業;作　　廢;作廢返轉;歷史異動"
  'functionOptProgram="RTLessorCmtyLineSNDWORKK.asp;RTLessorCmtylineHardwareK2.asp;RTLessorCustK.asp;RTLessorCmtyLineFAQK.asp;RTLessorCmtyLineContK.asp;RTLessorCmtyLineDROPK.asp;RTLessorCmtyLineCANCEL.asp;RTLessorCmtyLineCANCELRTN.asp;RTLessorCmtyLineLOGK.asp"
  functionOptName="用戶維護;作　　廢;作廢返轉"
  functionOptProgram="RTPrjCustK.asp;RTPrjCmtyLineCancel.asp;RTPrjCmtyLineCancelRTN.asp"
  functionOptPrompt="N;Y;Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;社區名稱;主線;主線IP;主線附掛;主線速率;申請日;到位日;撤線日;作廢日;報竣戶;計費戶;退租戶"
  sqlDelete="select	a.comq1, a.lineq1, b.comn, convert(varchar(5), a.comq1)+'-'+convert(varchar(5), a.lineq1), " &_
			"		a.lineip, a.linetel, c.codenc, a.applydat, a.arrivedat, a.dropdat, a.canceldat, " &_
			"		isnull(d.validnum,0), isnull(f.billnum,0), isnull(e.dropnum,0) " &_
			"from	RTPrjCmtyLine a " &_
			"inner join RTPrjCmtyH b on a.comq1 = b.comq1 " &_
			"left outer join RTCode c on c.code = a.linerate and c.kind ='D3' " &_
			"left outer join (select comq1, lineq1, count(*) as validnum from RTPrjCust where freecode <>'Y' " &_
			"			and docketdat is not null and dropdat is null and canceldat is null " &_
			"			group by comq1, lineq1) d on d.comq1 = a.comq1 and d.lineq1 = a.lineq1 " &_
			"left outer join (select comq1, lineq1, count(*) as dropnum from RTPrjCust where freecode <>'Y' " &_
			"			and docketdat is not null and dropdat is not null and canceldat is null " &_
			"			group by comq1, lineq1) e on e.comq1 = a.comq1 and e.lineq1 = a.lineq1 " &_
			"left outer join (select comq1, lineq1, count(*) as billnum from RTPrjCust where freecode <>'Y' " &_
			"			and strbillingdat is not null and dropdat is null and canceldat is null " &_
			"			group by comq1, lineq1) f on d.comq1 = a.comq1 and f.lineq1 = a.lineq1 " &_
			"where a.comq1 =0 "

  dataTable="RTPrjCmtyLine"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTPrjCmtyLineD.asp"
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
  searchFirst=FALSE
  If searchQry="" and len(aryparmkey(0))>0 Then
     searchQry=" a.ComQ1=" & aryparmkey(0)
     searchShow="社區序號︰"& aryparmkey(0) & ",社區名稱︰" & COMN & ",社區地址︰" & COMADDR
  elseIf searchQry="" and len(aryparmkey(0))=0 Then
     searchQry=" a.comq1 >=0 "
  ELSE
     SEARCHFIRST=FALSE
  End If


  if len(aryparmkey(0))>0 Then
		set connYY=server.CreateObject("ADODB.connection")
		set rsYY=server.CreateObject("ADODB.recordset")
		dsnYY="DSN=RTLIB"
		sqlYY="select * from RTPrjCmtyH LEFT OUTER JOIN RTCOUNTY ON RTPrjCmtyH.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0)
		connYY.Open dsnYY
		rsYY.Open sqlYY,connYY
		if not rsYY.EOF then
			COMN=rsYY("COMN")
			COMADDR=RSYY("CUTNC") & RSYY("TOWNSHIP") & RSYY("RADDR")
		else
			COMN=""
			COMADDR=""
		end if
		rsYY.Close
		connYY.Close
		set rsYY=nothing
		set connYY=nothing
   end if

  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  'Response.Write "GP=" & usergroup
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=RTLIB"
  sqlxx="select * from RTAreaSales where cusid='" & Emply & "' and areaid ='D0' "
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     limitemply	=" and b.salesid ='" & Emply & "' "
  else
     limitemply =" " 
  end if
  rsxx.Close
  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  
  '-------------------------------------------------------------------------------------------
  sqlList=	"select	a.comq1, a.lineq1, b.comn, convert(varchar(5), a.comq1)+'-'+convert(varchar(5), a.lineq1), " &_
			"		a.lineip, a.linetel, c.codenc, a.applydat, a.arrivedat, a.dropdat, a.canceldat, " &_
			"		isnull(d.validnum,0), isnull(f.billnum,0), isnull(e.dropnum,0) " &_
			"from	RTPrjCmtyLine a " &_
			"inner join RTPrjCmtyH b on a.comq1 = b.comq1 " &_
			"left outer join RTCode c on c.code = a.linerate and c.kind ='D3' " &_
			"left outer join (select comq1, lineq1, count(*) as validnum from RTPrjCust where freecode <>'Y' " &_
			"			and docketdat is not null and dropdat is null and canceldat is null " &_
			"			group by comq1, lineq1) d on d.comq1 = a.comq1 and d.lineq1 = a.lineq1 " &_
			"left outer join (select comq1, lineq1, count(*) as dropnum from RTPrjCust where freecode <>'Y' " &_
			"			and docketdat is not null and dropdat is not null and canceldat is null " &_
			"			group by comq1, lineq1) e on e.comq1 = a.comq1 and e.lineq1 = a.lineq1 " &_
			"left outer join (select comq1, lineq1, count(*) as billnum from RTPrjCust where freecode <>'Y' " &_
			"			and strbillingdat is not null and dropdat is null and canceldat is null " &_
			"			group by comq1, lineq1) f on f.comq1 = a.comq1 and f.lineq1 = a.lineq1 " &_
            "where " & SEARCHQRY & limitemply & " ORDER BY a.comq1, a.lineq1 "
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
