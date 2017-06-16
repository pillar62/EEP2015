<%
  Dim LXDa1Cnt,LXDa2Cnt,LXDb1Cnt,LXDb2Cnt
  Dim LRDa1Cnt,LRDa2Cnt,LRDb1Cnt,LRDb2Cnt
  Dim LRComCnt
%>
<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyListB.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="RT安裝發包進度查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  buttonEnable="N;N;Y;Y;N;N"
  accessMode="I"
  DSN="DSN=RTLib"
  formatName="建檔<br>流水號;社區<br>序號;社區名稱;單<br>次;社區<br>戶數;開發<br>戶數;開發<br>比率%;申裝<br>戶數;深耕<br>戶數;未安<br>裝數;T1開通日"
  DCTypes="003;003;200;003;003;003;003;200;200;003;135"
  sqlDelete="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCust.ENTRYNO, RTCmty.COMCNT, " _
         &"RTCmty.APPLYCNT, RTCust.CUSTYPE, RTCust.FINISHDAT, RTCmty.T1APPLY " _
         &"FROM ((((RTCmty INNER JOIN RTAreaCty ON RTCmty.CUTID = RTAreaCty.CUTID) " _
         &"INNER JOIN RTAreaCty AS RTAreaCty_1 ON RTCmty.CUTID = RTAreaCty_1.CUTID) " _
         &"INNER JOIN RTArea AS RTArea_1 ON RTAreaCty_1.AREAID = RTArea_1.AREAID) " _
         &"INNER JOIN RTArea ON RTAreaCty.AREAID = RTArea.AREAID) " _
         &"INNER JOIN RTCust ON RTCmty.COMQ1 = RTCust.COMQ1 " _
         &"WHERE (((RTArea.AREATYPE)='1') AND ((RTArea_1.AREATYPE)='2') AND RTCmty=0 " 
  dataTable="RTCmty"
  userDefineList="Yes"
  numberOfKey=1
  dataProg="RTCmtyKB2.asp"
  datawindowFeature=""
  searchWindowFeature=""
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  goodMorning=true
  goodMorningImage="CBBN.JPG"
  searchProg="RTCmtyS.asp"
  searchFirst=False
  If searchQry="" Then
     searchQry=" RTCmty.ComQ1<>0 "
     searchShow="全部"
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  if userlevel=2 then    
    If searchShow="全部" Then
    sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCust.ENTRYNO, RTCmty.COMCNT, " _
         &"RTCmty.APPLYCNT, RTCust.CUSTYPE, RTCust.FINISHDAT, RTCmty.T1APPLY " _
         &"FROM RTCmty INNER JOIN " _
         &"RTAreaCty ON RTCmty.CUTID = RTAreaCty.CUTID INNER JOIN " _
         &"RTAreaCty RTAreaCty_1 ON RTCmty.CUTID = RTAreaCty_1.CUTID INNER JOIN " _
         &"RTArea RTArea_1 ON RTAreaCty_1.AREAID = RTArea_1.AREAID INNER JOIN " _
         &"RTArea ON RTAreaCty.AREAID = RTArea_1.AREAID INNER JOIN " _
         &"RTCust ON RTCmty.COMQ1 = RTCust.COMQ1 INNER JOIN " _
         &"RTCmtySale ON RTCmty.COMQ1 = RTCmtySale.COMQ1 INNER JOIN " _
         &"RTEmployee ON RTCmtySale.CUSID = RTEmployee.CUSID " _
         &"WHERE (((RTArea.AREATYPE)='1') AND ((RTArea_1.AREATYPE)='2') " _
         &"AND ((RTCmty.DROPDESC)='')) and rtemployee.netid='" & Request.ServerVariables("LOGON_USER") & "' " _
         &"ORDER BY RTCmty.COMQ1, RTCmty.COMQ2, RTCust.ENTRYNO "
    Else
    sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCust.ENTRYNO, RTCmty.COMCNT, " _
         &"RTCmty.APPLYCNT, RTCust.CUSTYPE, RTCust.FINISHDAT, RTCmty.T1APPLY " _
         &"FROM RTCmty INNER JOIN " _
         &"RTAreaCty ON RTCmty.CUTID = RTAreaCty.CUTID INNER JOIN " _
         &"RTAreaCty RTAreaCty_1 ON RTCmty.CUTID = RTAreaCty_1.CUTID INNER JOIN " _
         &"RTArea RTArea_1 ON RTAreaCty_1.AREAID = RTArea_1.AREAID INNER JOIN " _
         &"RTArea ON RTAreaCty.AREAID = RTArea_1.AREAID INNER JOIN " _
         &"RTCust ON RTCmty.COMQ1 = RTCust.COMQ1 INNER JOIN " _
         &"RTCmtySale ON RTCmty.COMQ1 = RTCmtySale.COMQ1 INNER JOIN " _
         &"RTEmployee ON RTCmtySale.CUSID = RTEmployee.CUSID " _
         &"WHERE (((RTArea.AREATYPE)='1') AND ((RTArea_1.AREATYPE)='2') " _
         &"AND ((RTCmty.DROPDESC)=' ')) AND " &searchQry & " and rtemployee.netid='" & Request.ServerVariables("LOGON_USER") & "' "  _
         &"ORDER BY RTCmty.COMQ1, RTCmty.COMQ2, RTCust.ENTRYNO "
    End If
  else
    If searchShow="全部" Then
    sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCust.ENTRYNO, RTCmty.COMCNT, " _
         &"RTCmty.APPLYCNT, RTCust.CUSTYPE, RTCust.FINISHDAT, RTCmty.T1APPLY " _
         &"FROM ((((RTCmty INNER JOIN RTAreaCty ON RTCmty.CUTID = RTAreaCty.CUTID) " _
         &"INNER JOIN RTAreaCty AS RTAreaCty_1 ON RTCmty.CUTID = RTAreaCty_1.CUTID) " _
         &"INNER JOIN RTArea AS RTArea_1 ON RTAreaCty_1.AREAID = RTArea_1.AREAID) " _
         &"INNER JOIN RTArea ON RTAreaCty.AREAID = RTArea.AREAID) " _
         &"INNER JOIN RTCust ON RTCmty.COMQ1 = RTCust.COMQ1 " _
         &"WHERE (((RTArea.AREATYPE)='1') AND ((RTArea_1.AREATYPE)='2') " _
         &"AND ((RTCmty.DROPDESC)='')) " _
         &"ORDER BY RTCmty.COMQ1, RTCmty.COMQ2, RTCust.ENTRYNO "
    Else
    sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCust.ENTRYNO, RTCmty.COMCNT, " _
         &"RTCmty.APPLYCNT, RTCust.CUSTYPE, RTCust.FINISHDAT, RTCmty.T1APPLY " _
         &"FROM (((((RTCmty INNER JOIN RTAreaCty ON RTCmty.CUTID = RTAreaCty.CUTID) " _
         &"INNER JOIN RTAreaCty AS RTAreaCty_1 ON RTCmty.CUTID = RTAreaCty_1.CUTID) " _
         &"INNER JOIN RTArea AS RTArea_1 ON RTAreaCty_1.AREAID = RTArea_1.AREAID) " _
         &"INNER JOIN RTArea ON RTAreaCty.AREAID = RTArea.AREAID) " _
         &"INNER JOIN RTCust ON RTCmty.COMQ1 = RTCust.COMQ1) " _
         &"INNER JOIN RTCmtySale ON RTCmty.COMQ1 = RTCmtySale.COMQ1 " _
         &"WHERE (((RTArea.AREATYPE)='1') AND ((RTArea_1.AREATYPE)='2') " _
         &"AND ((RTCmty.DROPDESC)=' ')) AND " &searchQry &" " _
         &"ORDER BY RTCmty.COMQ1, RTCmty.COMQ2, RTCust.ENTRYNO "
    End If  
  end if
  'Response.Write "sql=" & SQLLIST
End Sub
Sub SrUserDefineProcess()
  on error resume next
  Dim DZ
  If DLX Then LXDa1Cnt=0:LXDa2Cnt=0:LXDb1Cnt=0:LXDb2Cnt=0
  If  Trim(DB(6))="申裝戶" Then
      LXDa1Cnt=LXDa1Cnt + 1
      If IsDate(DB(7)) Then LXDa2Cnt=LXDa2Cnt + 1
  Else
      LXDb1Cnt=LXDb1Cnt + 1
      If IsDate(DB(7)) Then LXDb2Cnt=LXDb2Cnt + 1
  End If
  If TLX Then 
     LRDa1Cnt=LRDa1Cnt + LXDa1Cnt
     LRDa2Cnt=LRDa2Cnt + LXDa2Cnt
     LRDb1Cnt=LRDb1Cnt + LXDb1Cnt
     LRDb2Cnt=LRDb2Cnt + LXDb2Cnt
     LRComCnt=LRComCnt + DB(4)
     DC(0)=DB(0)
     DC(1)=DB(1)
     DC(2)=DB(2)
     DC(3)=DB(3)
     DC(4)=DB(4)
     DC(5)=LXDa1Cnt + LXDb1Cnt
     DC(6)=Int(DC(5)/DC(4) * 10000 +.5)/100
     DC(7)=Cstr(LXDa1Cnt) &"(" &Cstr(LXDa2Cnt) &")"
     DC(8)=Cstr(LXDb1Cnt) &"(" &Cstr(LXDb2Cnt) &")"
     DC(9)=(LXDa1Cnt - LXDa2Cnt) + (LXDb1Cnt -  LXDb2Cnt) 
     DC(10)=DB(8)
     DZ=Split("003;003;200;003;003;003;006;200;200;003;135",";")     
     Call SrAddListEntry(DC,"D",DZ)
  End If
  If TLR Then
     DC(0)="":DC(1)="":DC(2)="合計":DC(3)=""
     DC(4)=LRComCnt
     DC(5)=LRDa1Cnt + LRDb1Cnt
     DC(6)=Int(DC(5)/DC(4) * 10000 +.5)/100
     DC(7)=Cstr(LRDa1Cnt) &"/" &Cstr(LRDa2Cnt)
     DC(8)=Cstr(LRDb1Cnt) &"/" &Cstr(LRDb2Cnt) 
     DC(9)=LRDa1Cnt - LRDa2Cnt + LRDb1Cnt - LRDb2Cnt
     DC(10)=""
     DZ=Split("200;200;200;200;003;003;006;200;200;003;200",";")     
     Call SrAddListEntry(DC,"T",DZ)
  End If
End Sub
%>