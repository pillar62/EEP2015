<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="客戶RT發包異動記錄查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;N" 
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;序號;異動別;異動日期;異動人員;安裝<BR>工程師1;安裝<BR>工程師2;安裝<BR>工程師3;安裝<BR>工程師4;安裝<BR>工程師5;施工廠商"
  sqlDelete="SELECT RTCustSWSch.COMQ1, RTCustSWSch.CUSID, RTCustSWSch.ENTRYNO, " _
           &"RTCustSWSch.TRANSNO, RTCode.CODENC, RTCustSWSch.RUNDAT, RTObj1.CUSNC, " _
           &"RTObj2.CUSNC AS Expr1, RTObj3.CUSNC AS Expr2, RTObj4.CUSNC AS Expr3, " _
           &"RTObj5.CUSNC AS Expr4, RTObj6.CUSNC AS Expr5, RTObj7.SHORTNC " _
           &"FROM RTObj RTObj4 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee2 ON " _
           &"RTObj4.CUSID = RTEmployee2.CUSID RIGHT OUTER JOIN " _
           &"RTObj RTObj6 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee4 ON " _
           &"RTObj6.CUSID = RTEmployee4.CUSID RIGHT OUTER JOIN " _
           &"RTCode RIGHT OUTER JOIN " _
           &"RTObj RTObj2 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee6 ON " _
           &"RTObj2.CUSID = RTEmployee6.CUSID RIGHT OUTER JOIN " _
           &"RTObj RTObj7 RIGHT OUTER JOIN " _
           &"RTCustSWSch ON RTObj7.CUSID = RTCustSWSch.OBJSUPP ON " _
           &"RTEmployee6.EMPLY = RTCustSWSch.OBJEMPLOY1 ON " _
           &"RTCode.CODE = RTCustSWSch.StatusID LEFT OUTER JOIN " _
           &"RTObj RTObj1 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee5 ON RTObj1.CUSID = RTEmployee5.CUSID ON " _
           &"RTCustSWSch.RUNUSR = RTEmployee5.EMPLY ON  " _
           &"RTEmployee4.EMPLY = RTCustSWSch.OBJEMPLOY5 LEFT OUTER JOIN " _
           &"RTObj RTObj5 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee3 ON RTObj5.CUSID = RTEmployee3.CUSID ON  " _
           &"RTCustSWSch.OBJEMPLOY4 = RTEmployee3.EMPLY ON " _
           &"RTEmployee2.EMPLY = RTCustSWSch.OBJEMPLOY3 LEFT OUTER JOIN " _
           &"RTObj RTObj3 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee1 ON RTObj3.CUSID = RTEmployee1.CUSID ON " _
           &"RTCustSWSch.OBJEMPLOY2 = RTEmployee1.EMPLY " _
           &"WHERE (RTCode.KIND = 'A8') and RTCustSWSch.COMQ1 = 0 " _
           &"ORDER BY RTCustSWSch.COMQ1, RTCustSWSch.CUSID, RTCustSWSch.ENTRYNO, " _
           &"RTCustSWSch.TRANSNO "
  dataTable=""
  extTable=""
  numberOfKey=4
  dataProg="None"
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
  keyListPageSize=20
  searchProg="self"
  searchFirst=false
  If searchQry="" Then
     searchQry=" RTCustSWSch.COMQ1=" & aryparmkey(0) & " AND RTCustSWSch.CUSID='" & aryparmkey(1) & "'" _
              &" AND RTCustSWSch.ENTRYNO=" & aryParmKey(2)
     searchShow=FrGetCmtyDesc(aryParmKey(0))
     searchShow=searchShow & "  " & FrGetCustDesc(aryParmKey(1))
  End If
  sqlList  ="SELECT RTCustSWSch.COMQ1, RTCustSWSch.CUSID, RTCustSWSch.ENTRYNO, " _
           &"RTCustSWSch.TRANSNO, RTCode.CODENC, RTCustSWSch.RUNDAT, RTObj1.CUSNC, " _
           &"RTObj2.CUSNC AS Expr1, RTObj3.CUSNC AS Expr2, RTObj4.CUSNC AS Expr3, " _
           &"RTObj5.CUSNC AS Expr4, RTObj6.CUSNC AS Expr5, RTObj7.SHORTNC " _
           &"FROM RTObj RTObj4 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee2 ON " _
           &"RTObj4.CUSID = RTEmployee2.CUSID RIGHT OUTER JOIN " _
           &"RTObj RTObj6 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee4 ON " _
           &"RTObj6.CUSID = RTEmployee4.CUSID RIGHT OUTER JOIN " _
           &"RTCode RIGHT OUTER JOIN " _
           &"RTObj RTObj2 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee6 ON " _
           &"RTObj2.CUSID = RTEmployee6.CUSID RIGHT OUTER JOIN " _
           &"RTObj RTObj7 RIGHT OUTER JOIN " _
           &"RTCustSWSch ON RTObj7.CUSID = RTCustSWSch.OBJSUPP ON " _
           &"RTEmployee6.EMPLY = RTCustSWSch.OBJEMPLOY1 ON " _
           &"RTCode.CODE = RTCustSWSch.StatusID LEFT OUTER JOIN " _
           &"RTObj RTObj1 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee5 ON RTObj1.CUSID = RTEmployee5.CUSID ON " _
           &"RTCustSWSch.RUNUSR = RTEmployee5.EMPLY ON  " _
           &"RTEmployee4.EMPLY = RTCustSWSch.OBJEMPLOY5 LEFT OUTER JOIN " _
           &"RTObj RTObj5 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee3 ON RTObj5.CUSID = RTEmployee3.CUSID ON  " _
           &"RTCustSWSch.OBJEMPLOY4 = RTEmployee3.EMPLY ON " _
           &"RTEmployee2.EMPLY = RTCustSWSch.OBJEMPLOY3 LEFT OUTER JOIN " _
           &"RTObj RTObj3 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee1 ON RTObj3.CUSID = RTEmployee1.CUSID ON " _
           &"RTCustSWSch.OBJEMPLOY2 = RTEmployee1.EMPLY " _
           &"WHERE (RTCode.KIND = 'A8') and " & SearchQry & "  " _
           &"ORDER BY RTCustSWSch.COMQ1, RTCustSWSch.CUSID, RTCustSWSch.ENTRYNO, " _
           &"RTCustSWSch.TRANSNO "
           'Response.Write "sql=" & sqLLIST
End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->
<!-- #include virtual="/webap/include/RTGetCustDesc.inc" -->