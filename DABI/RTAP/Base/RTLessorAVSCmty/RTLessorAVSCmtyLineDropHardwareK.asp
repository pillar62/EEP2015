<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City主線拆機派工設備資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="轉移轉單;返轉移轉單; 作　廢 ; 作廢返轉;異動查詢"
  functionOptProgram="RTLessorAVSCmtyLineDROPHARDWARETRNrtn.ASP;RTLessorAVSCmtyLineDROPHARDWARETRNrtnRTN.ASP;RTLessorAVSCmtyLineDROPHARDWAREDROP.ASP;RTLessorAVSCmtyLineDROPHARDWAREDROPc.ASP;RTLessorAVSCmtyLineDROPHARDWARELOGK.ASP"
  functionOptPrompt="Y;Y;Y;Y;N"  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;<center>派工單號</center>;<center>項次</center>;<center>設備名稱/規格</center>;<center>數量</center>;<center>金額</center>;<center>出庫別</center>;<center>作廢日期</center>;<center>作廢原因</center>;<center>作廢人員</center>;帳款編號;轉應收帳款日;移轉單號;移轉結案日"
  sqlDelete="SELECT RTLessorAVSCmtyLineDropHARDWARE.COMQ1,RTLessorAVSCmtyLineDropHARDWARE.LINEQ1, " _
         &"RTLessorAVSCmtyLineDropHARDWARE.ENTRYNO,RTLessorAVSCmtyLineDropHARDWARE.PRTNO, RTLessorAVSCmtyLineDropHARDWARE.seq, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.SPEC, RTLessorAVSCmtyLineDropHARDWARE.QTY, RTLessorAVSCmtyLineDropHARDWARE.amt, " _
         &"HBwarehouse.WARENAME, RTLessorAVSCmtyLineDropHARDWARE.DROPDAT, RTLessorAVSCmtyLineDropHARDWARE.DROPREASON, RTObj.CUSNC,RTLessorAVSCmtyLineDropHARDWARE.BATCHNO,RTLessorAVSCmtyLineDropHARDWARE.TARDAT,RTLessorAVSCmtyLineDropHARDWARE.rcvprtno,RTLessorAVSCmtyLineDropHARDWARE.rcvfinishdat " _
         &"FROM RTProdH RIGHT OUTER JOIN RTLessorAVSCmtyLineDropHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTLessorAVSCmtyLineDropHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTLessorAVSCmtyLineDropHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTLessorAVSCmtyLineDropHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTLessorAVSCmtyLineDropHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTLessorAVSCmtyLineDropHARDWARE.PRODNO " _
         &"left outer join RTLessorAVSCmtyLine on RTLessorAVSCmtyLineDropHARDWARE.comq1=RTLessorAVSCmtyLine.comq1 " _
         &"WHERE RTLessorAVSCmtyLineDropHARDWARE.comq1=0 "
  dataTable="RTLessorAVSCmtyLineDropHARDWARE"
  extTable=""
  numberOfKey=5
  dataProg="RTLessorAVSCmtyLineDropHARDWARED.asp"
  datawindowFeature=""
  searchWindowFeature=""
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
  '----
    set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorAVSCmtyH ON " _
       &"RTCounty.CUTID = RTLessorAVSCmtyH.CUTID RIGHT OUTER JOIN RTLessorAVSCmtyLine ON RTLessorAVSCmtyH.COMQ1 = RTLessorAVSCmtyLine.COMQ1 " _
       &"where RTLessorAVSCmtyLine.comq1=" & ARYPARMKEY(0) 
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
       &"where RTLessorAVSCmtyLine.comq1=" & ARYPARMKEY(0) & " and RTLessorAVSCmtyLine.lineq1=" & ARYPARMKEY(1)
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
     searchQry=" RTLessorAVSCmtyLineDropHARDWARE.comq1=" & aryparmkey(0) & " and RTLessorAVSCmtyLineDropHARDWARE.lineq1=" & aryparmkey(1) & " and RTLessorAVSCmtyLineDropHARDWARE.ENTRYNO=" & aryparmkey(2) & " and RTLessorAVSCmtyLineDropHARDWARE.prtno='" & aryparmkey(3) & "' "
     searchShow="主線︰"& aryparmkey(0) & "-" & aryparmkey(1) & ",社區名稱︰" & COMN & ",主線位址︰" & COMADDR  & ",拆機派工單號︰" & aryparmkey(3)
  ELSE
     SEARCHFIRST=FALSE
  End If  
  searchProg="self"
  sqlList="SELECT RTLessorAVSCmtyLineDropHARDWARE.comq1,RTLessorAVSCmtyLineDropHARDWARE.lineq1, " _
         &"RTLessorAVSCmtyLineDropHARDWARE.ENTRYNO,RTLessorAVSCmtyLineDropHARDWARE.PRTNO, RTLessorAVSCmtyLineDropHARDWARE.seq, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.itemnc + '('+ RTProdD1.SPEC+')', RTLessorAVSCmtyLineDropHARDWARE.QTY, RTLessorAVSCmtyLineDropHARDWARE.amt, " _
         &"HBwarehouse.WARENAME, RTLessorAVSCmtyLineDropHARDWARE.DROPDAT, RTLessorAVSCmtyLineDropHARDWARE.DROPREASON, RTObj.CUSNC,RTLessorAVSCmtyLineDropHARDWARE.BATCHNO,RTLessorAVSCmtyLineDropHARDWARE.TARDAT,RTLessorAVSCmtyLineDropHARDWARE.rcvprtno,RTLessorAVSCmtyLineDropHARDWARE.rcvfinishdat  " _
         &"FROM RTProdH RIGHT OUTER JOIN RTLessorAVSCmtyLineDropHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTLessorAVSCmtyLineDropHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTLessorAVSCmtyLineDropHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTLessorAVSCmtyLineDropHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTLessorAVSCmtyLineDropHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTLessorAVSCmtyLineDropHARDWARE.PRODNO " _
         &"left outer join RTLessorAVSCmtyLine on  RTLessorAVSCmtyLineDropHARDWARE.comq1=RTLessorAVSCmtyLine.comq1 and RTLessorAVSCmtyLineDropHARDWARE.lineq1=RTLessorAVSCmtyLine.lineq1 " _
         &"WHERE " &searchQry & ""
'Response.Write "sql=" & SQLLIST         
End Sub
%>
