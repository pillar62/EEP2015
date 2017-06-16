<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City用戶資料維護"
  buttonName=" 新  增 ; 刪  除 ; 結  束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  functionOptName="維修收款;裝機派工;續約作業;復機作業;退租作業;應收應付;客服案件;設備保管收據列印;用戶移動;調整到期;設備查詢;作　　廢;作廢返轉;歷史異動"
  functionOptProgram="RTLessorAVSCustRepairK.asp;RTLessorAVSCustsndworkk.asp;RTLessorAVSCustContK.asp;RTLessorAVSCustReturnK.asp;RTLessorAVSCustDropK.asp;RTLessorAVSCustARK.asp;RTLessorAVSCustfaqK.asp;/RTAP/REPORT/Common/RTStorageReceiptAVS.asp;RTLessorAVSCustmove.asp;RTLessorAVSCustadjdayK.asp;RTLessorAVSCusttothardwareK.asp;RTLessorAVSCustCANCEL.asp;RTLessorAVSCustCANCELRTN.asp;RTLessorAVSCustLOGK.asp"
  functionOptPrompt="N;N;N;N;N;N;N;N;N;N;N;Y;Y;N"
  functionoptopen=  "1;1;1;1;1;1;1;1;1;1;1;1;1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;業務轄區;主線;社區;用戶;方案;週期;繳款;none;none;申請日;完工日;報竣日;開始計費;最近<br>續約日;到期日;公關;退租日;作廢日;none;none;可用<BR>日數;最近<br>收款額"
  sqlDelete="SELECT RTLessorAVSCust.COMQ1, RTLessorAVSCust.LINEQ1, RTLessorAVSCust.CUSID, RTLessorAVSCmtyLine.salesid," _
                &"RTRIM(LTRIM(CONVERT(char(6), RTLessorAVSCust.COMQ1))) " _
                &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorAVSCust.LINEQ1))) AS comqline, " _
                &"RTLessorAVSCmtyH.COMN,RTLessorAVSCust.CUSNC,c.codenc,ltrim(rtrim(RTLessorAVSCust.IP11))+'.'+ltrim(rtrim(RTLessorAVSCust.IP12))+'.'+ltrim(rtrim(RTLessorAVSCust.IP13))+'.'+ltrim(rtrim(RTLessorAVSCust.IP14)), RTLessorAVSCust.TOWNSHIP1 + " _
                &"RTLessorAVSCust.RADDR1 AS ADDR, RTLessorAVSCUST.APPLYDAT, RTLessorAVSCUST.FINISHDAT, RTLessorAVSCUST.DOCKETDAT, " _
                &"RTLessorAVSCUST.STRBILLINGDAT,RTLessorAVSCUST.DUEDAT,RTLessorAVSCUST.DROPDAT,RTLessorAVSCUST.CANCELDAT,ltrim(rtrim(RTLessorAVSCust.IP11))+'.'+ltrim(rtrim(RTLessorAVSCust.IP12))+'.'+ltrim(rtrim(RTLessorAVSCust.IP13))+'.'+ltrim(rtrim(RTLessorAVSCust.IP14)), RTLessorAVSCUST.MAC," _
                &"FROM RTLessorAVSCust LEFT OUTER JOIN RTCounty ON RTLessorAVSCust.CUTID1 = RTCounty.CUTID LEFT OUTER JOIN " _
                &"RTLessorAVSCmtyH ON RTLessorAVSCust.COMQ1 = RTLessorAVSCmtyH.COMQ1 left outer join RTLessorAVScmtyline on " _
                &"RTLessorAVScust.comq1=RTLessorAVScmtyline.comq1 and  RTLessorAVScust.lineq1=RTLessorAVScmtyline.lineq1 " _
                &"left outer join rtcode rtcode_1 on RTLessorAVScust.paycycle=rtcode_1.code and rtcode_1.kind='M8' " _
                &"left outer join rtcode rtcode_2 on RTLessorAVScust.payTYPE=rtcode_2.code and rtcode_2.kind='M9' " _
                &"left outer join RTCode c on c.code = rtlessoravscust.casekind and c.kind ='O9' " _
                &"where RTLessorAVSCust.COMQ1=0 "
  dataTable="RTLessorAVSCust"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="RTLessorAVSCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=500,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="400"
  diaHeight="250"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTLessorAVSCusts2.asp"
  searchFirst=TRUE
  If searchQry="" Then
     searchQry=" RTLessorAVSCust.COMQ1=0 "
     SEATCHQRY2=""
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  'userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=RTLIB"
  sqlxx="select * from RTAreaSales where cusid='" & Emply & "' and areaid ='D0' "
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     limitemply	=" and RTLessorAVScmtyline.salesid ='" & Emply & "' "
  else
     limitemply =" " 
  end if
  rsxx.Close

  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  '-------------------------------------------------------------------------------------------
  '業務工程師只能讀取該工程師的社區
         sqlList="SELECT RTLessorAVSCust.COMQ1, RTLessorAVSCust.LINEQ1, RTLessorAVSCust.CUSID, isnull(RTObj_a.shortnc, RTObj_1.cusnc), " _
                &"RTRIM(LTRIM(CONVERT(char(6), RTLessorAVSCust.COMQ1))) " _
                &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorAVSCust.LINEQ1))) AS comqline, " _
                &"RTLessorAVSCmtyH.COMN,case when len(RTLessorAVSCust.CUSNC) > 4 then substring(RTLessorAVSCust.CUSNC,1,4)+'...' else RTLessorAVSCust.CUSNC end,c.codenc,RTCODE_1.CODENC,RTCODE_2.CODENC,ltrim(rtrim(RTLessorAVSCust.IP11))+'.'+ltrim(rtrim(RTLessorAVSCust.IP12))+'.'+ltrim(rtrim(RTLessorAVSCust.IP13))+'.'+ltrim(rtrim(RTLessorAVSCust.IP14)), RTLessorAVSCust.TOWNSHIP1 + " _
                &"RTLessorAVSCust.RADDR1 AS ADDR, RTLessorAVSCUST.APPLYDAT, RTLessorAVSCUST.FINISHDAT, RTLessorAVSCUST.DOCKETDAT, " _
                &"RTLessorAVSCUST.STRBILLINGDAT,RTLessorAVSCUST.newBILLINGDAT,RTLessorAVSCUST.DUEDAT,case when RTLESSORAVSCUST.freecode='Y' THEN RTLESSORAVSCUST.freecode ELSE '' END,RTLessorAVSCUST.DROPDAT,RTLessorAVSCUST.CANCELDAT,ltrim(rtrim(RTLessorAVSCust.IP11))+'.'+ltrim(rtrim(RTLessorAVSCust.IP12))+'.'+ltrim(rtrim(RTLessorAVSCust.IP13))+'.'+ltrim(rtrim(RTLessorAVSCust.IP14)), RTLessorAVSCUST.MAC," _
                &"case when RTLessorAVSCUST.DUEDAT is null then 0 when RTLessorAVSCUST.canceldat is not null or RTLessorAVSCUST.dropdat is not null then 0 else DATEDiFF(d,getdate(),RTLessorAVSCUST.DUEDAT) end as validdat,RTLessorAVScust.rcvmoney  " _
                &"FROM RTLessorAVSCust LEFT OUTER JOIN RTCounty ON RTLessorAVSCust.CUTID1 = RTCounty.CUTID LEFT OUTER JOIN " _
                &"RTLessorAVSCmtyH ON RTLessorAVSCust.COMQ1 = RTLessorAVSCmtyH.COMQ1 inner join RTLessorAVScmtyline on " _
                &"RTLessorAVScust.comq1=RTLessorAVScmtyline.comq1 and  RTLessorAVScust.lineq1=RTLessorAVScmtyline.lineq1 " _
                &"left outer join rtcode rtcode_1 on RTLessorAVScust.paycycle=rtcode_1.code and rtcode_1.kind='M8' " _
                &"left outer join rtcode rtcode_2 on RTLessorAVScust.payTYPE=rtcode_2.code and rtcode_2.kind='M9' " _
                &"LEFT OUTER JOIN RTCtyTown ON RTLessorAVSCust.CUTID2 = RTCtyTown.CUTID AND " _
                &"RTLessorAVSCust.TOWNSHIP2 = RTCtyTown.TOWNSHIP " _
                &"left outer join rtobj rtobj_a on RTLessorAVSCmtyLine.consignee=rtobj_a.cusid LEFT OUTER JOIN " _
                &"RTEmployee INNER JOIN RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID ON " _
                &"RTLessorAVSCmtyLine.SALESID = RTEmployee.EMPLY " _
                &"left outer join RTCode c on c.code = rtlessoravscust.casekind and c.kind ='O9' " _
                &"where " & searchqry & " " & limitemply & " " & searchqry2 
 'response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>