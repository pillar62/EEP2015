<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City主線派工設備資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="轉領用單;領用單返轉;設備作廢;作廢返轉;異動查詢"
  functionOptProgram="RTLessorAVSCmtyLineHARDWARETRNRCV.ASP;RTLessorAVSCmtyLineHARDWARETRNRCVRTN.ASP;RTLessorAVSCmtyLineHARDWAREDROP.ASP;RTLessorAVSCmtyLineHARDWAREDROPc.ASP;RTLessorAVSCmtyLineHARDWARELOGK.ASP"
  functionOptPrompt="Y;Y;Y;Y;N"  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;<center>派工單號</center>;<center>項次</center>;<center>設備名稱/規格</center>;<center>數量</center>;<center>金額</center>;<center>出庫別</center>;<center>作廢日期</center>;<center>作廢原因</center>;<center>作廢人員</center>;帳款編號;轉應收帳款日;領用單號;領用結案日"
  sqlDelete="SELECT RTLessorAVSCmtyLineHARDWARE.comq1,RTLessorAVSCmtyLineHARDWARE.lineq1, " _
         &"RTLessorAVSCmtyLineHARDWARE.PRTNO, RTLessorAVSCmtyLineHARDWARE.seq, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.SPEC, RTLessorAVSCmtyLineHARDWARE.QTY, RTLessorAVSCmtyLineHARDWARE.amt, " _
         &"HBwarehouse.WARENAME, RTLessorAVSCmtyLineHARDWARE.DROPDAT, RTLessorAVSCmtyLineHARDWARE.DROPREASON, RTObj.CUSNC,RTLessorAVSCmtyLineHARDWARE.BATCHNO,RTLessorAVSCmtyLineHARDWARE.TARDAT,RTLessorAVSCmtyLineHARDWARE.rcvprtno,RTLessorAVSCmtyLineHARDWARE.rcvfinishdat " _
         &"FROM RTProdH RIGHT OUTER JOIN RTLessorAVSCmtyLineHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTLessorAVSCmtyLineHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTLessorAVSCmtyLineHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTLessorAVSCmtyLineHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTLessorAVSCmtyLineHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTLessorAVSCmtyLineHARDWARE.PRODNO " _
         &"left outer join RTLessorAVSCmtyLine on  RTLessorAVSCmtyLinehardware.cusid=RTLessorAVSCmtyLine.cusid " _
         &"WHERE RTLessorAVSCmtyLineHARDWARE.cusid='' "
  dataTable="RTLessorAVSCmtyLineHARDWARE"
  extTable=""
  numberOfKey=4
  dataProg="RTLessorAVSCmtyLineHARDWARED.asp"
  datawindowFeature=""
  searchWindowFeature="width=350,height=160,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage=""
  colSplit=1
  keyListPageSize=25
  searchProg="RTLessorAVSCmtyLineHardwareS.ASP"
  '----
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorAVSCmtyH ON " _
       &"RTCounty.CUTID = RTLessorAVSCmtyH.CUTID RIGHT OUTER JOIN RTLessorAVSCmtyLine ON RTLessorAVSCmtyH.COMQ1 = RTLessorAVSCmtyLine.COMQ1 " _
       &"where RTLessorAVSCmtyLine.comq1=" & ARYPARMKEY(0) & " and RTLessorAVSCmtyLine.lineq1="  & ARYPARMKEY(1)
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorAVSCmtyLine ON  " _
       &"RTCounty.CUTID = RTLessorAVSCmtyLine.CUTID " _
       &"where RTLessorAVSCmtyLine.comq1=" & ARYPARMKEY(0) & " and RTLessorAVSCmtyLine.lineq1="  & ARYPARMKEY(1)
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     comaddr=""
     COMaddr=rsYY("cutnc") & rsyy("township")
     if rsyy("village") <> "" then
         COMaddr= COMaddr & rsyy("village") & rsyy("cod1")
     end if
     if rsyy("NEIGHBOR") <> "" then
         COMaddr= COMaddr & rsyy("NEIGHBOR") & rsyy("cod2")
     end if
     if rsyy("STREET") <> "" then
         COMaddr= COMaddr & rsyy("STREET") & rsyy("cod3")
     end if
     if rsyy("SEC") <> "" then
         COMaddr= COMaddr & rsyy("SEC") & rsyy("cod4")
     end if
     if rsyy("LANE") <> "" then
         COMaddr= COMaddr & rsyy("LANE") & rsyy("cod5")
     end if
     if rsyy("ALLEYWAY") <> "" then
         COMaddr= COMaddr & rsyy("ALLEYWAY") & rsyy("cod7")
     end if
     if rsyy("NUM") <> "" then
         COMaddr= COMaddr & rsyy("NUM") & rsyy("cod8")
     end if
     if rsyy("FLOOR") <> "" then
         COMaddr= COMaddr & rsyy("FLOOR") & rsyy("cod9")
     end if
     if rsyy("ROOM") <> "" then
         COMaddr= COMaddr & rsyy("ROOM") & rsyy("cod10")
     end if
  else
     COMaddr=""
  end if
  RSYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorAVSCmtyLinehardware.comq1=" & aryparmkey(0) & " and RTLessorAVSCmtyLineHARDWARE.lineq1=" & aryparmkey(1) & " and RTLessorAVSCmtyLineHARDWARE.prtno='" & aryparmkey(2) & "' and RTLessorAVSCmtyLineHARDWARE.dropdat is null"
     searchShow="主線︰"& aryparmkey(0) & "-" & aryparmkey(1) & ",社區名稱︰" & COMN & ",主線位址︰" & COMADDR & ",派工單號︰" & aryparmkey(2)
  ELSE
     SEARCHFIRST=FALSE
  End If  

  sqlList="SELECT RTLessorAVSCmtyLineHARDWARE.comq1,RTLessorAVSCmtyLineHARDWARE.lineq1, " _
         &"RTLessorAVSCmtyLineHARDWARE.PRTNO, RTLessorAVSCmtyLineHARDWARE.seq, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.itemnc + '('+ RTProdD1.SPEC+')', RTLessorAVSCmtyLineHARDWARE.QTY, RTLessorAVSCmtyLineHARDWARE.amt, " _
         &"HBwarehouse.WARENAME, RTLessorAVSCmtyLineHARDWARE.DROPDAT, RTLessorAVSCmtyLineHARDWARE.DROPREASON, RTObj.CUSNC,RTLessorAVSCmtyLineHARDWARE.BATCHNO,RTLessorAVSCmtyLineHARDWARE.TARDAT,RTLessorAVSCmtyLineHARDWARE.rcvprtno,RTLessorAVSCmtyLineHARDWARE.rcvfinishdat  " _
         &"FROM RTProdH RIGHT OUTER JOIN RTLessorAVSCmtyLineHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTLessorAVSCmtyLineHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTLessorAVSCmtyLineHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTLessorAVSCmtyLineHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTLessorAVSCmtyLineHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTLessorAVSCmtyLineHARDWARE.PRODNO " _
         &"WHERE RTLessorAVSCmtyLinehardware.comq1=" & aryparmkey(0) & " and RTLessorAVSCmtyLineHARDWARE.lineq1=" & aryparmkey(1) & " and RTLessorAVSCmtyLineHARDWARE.prtno='" & aryparmkey(2) & "' and " &searchQry & ""
'Response.Write "sql=" & SQLLIST         
End Sub
%>
