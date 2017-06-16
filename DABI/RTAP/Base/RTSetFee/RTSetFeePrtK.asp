<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="RT施工費支付明細表列印(技術部)"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;" & v(3)
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="列印確認"
  functionOptProgram="Verify.asp"
  functionOptPrompt="Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=FALSE
  formatName="none;none;none;社區名稱;客戶;完工日;入帳日;峻工單收件日;標準施工費;施工補助費;收款入帳日;會計確認日;列印批號"
  sqlDelete="SELECT a.comq1,a.cusid,a.entryno,b.COMN, c.CUSNC, a.finishdat, a.incomedat,a.docketdat,a.setfee, a.setfeediff,a.incomedat, a.acccfmdat,a.paydtlprtNO " _
           & "FROM   RTCust a, RTCmty b, RTObj c, RTCmtySale d, RTObj e, RTObj f " _
           & "WHERE  a.COMQ1 = b.COMQ1 " _
           & "AND a.CUSID = c.CUSID " _
           & "AND a.COMQ1 =d.COMQ1  AND GetDate() Between d.TDAT AND IsNull(d.EXDAT, '9999/12/31') " _
           & "AND d.CUSID = e.CUSID " _
           & "AND a.PROFAC *= f.CUSID and a.settype='3' " _
           & "and a.cusid='*' " 
  dataTable=""
  extTable=""
  numberOfKey=3
  dataProg="/webap/rtap/base/rtcmty/RTcustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=true
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="RTSetFeePrtS.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=false
  If searchQry="" Then
     searchQry=" (a.PROFAC <> '*') and (a.paydtldat is null) AND (a.paydtlprtno <> '*')  and (a.docketdat is not null) and (a.incomedat is not null)" & ";;;<>'*';：已入帳"
     searchShow="施工廠商：全部　會計審核：全部　列印狀況：未列印  峻工單：已收件  收款入帳：已入帳"
  End If
  X=split(searchqry,";")
  '---列印批號空白"
  sqlList="SELECT a.comq1,a.cusid,a.entryno,b.COMN, c.CUSNC, a.finishdat, a.incomedat,a.docketdat,a.setfee, a.setfeediff, a.incomedat,a.acccfmdat,a.paydtlprtNO " _
           & "FROM   RTCust a, RTCmty b, RTObj c, RTCmtySale d, RTObj e, RTObj f " _
           & "WHERE  a.COMQ1 = b.COMQ1 " _
           & "AND a.CUSID = c.CUSID " _
           & "AND a.COMQ1 =d.COMQ1  AND GetDate() Between d.TDAT AND IsNull(d.EXDAT, '9999/12/31') " _
           & "AND d.CUSID = e.CUSID " _
           & "AND a.PROFAC *= f.CUSID  and a.settype='3'  AND " &X(0) &" " _
           &"ORDER BY B.COMN "
  sqlstr=  "FROM   RTCust a, RTCmty b, RTObj c, RTCmtySale d, RTObj e, RTObj f " _
           & "WHERE  a.COMQ1 = b.COMQ1 " _
           & "AND a.CUSID = c.CUSID " _
           & "AND a.COMQ1 =d.COMQ1  AND GetDate() Between d.TDAT AND IsNull(d.EXDAT, '9999/12/31') " _
           & "AND d.CUSID = e.CUSID " _
           & "AND a.PROFAC *= f.CUSID  and a.settype='3'  AND " &X(0)
 'Response.Write "SQL=" & SQLlist
 session("SQLSTRREVPRT")=replace(SQLstr,"'","""")
 session("ExistPrtNo")= X(2) 
' Response.Write "SQL=" & session("sqlstrrevprt") & "<BR>"
 '---X(1):施工費付款表列印paydtlprtno(='':未列印;<>'':已列印;<>'*':全部)
 '---X(2):收款表批號paydtlprtno(<>'*':無批號)
 '---X(3):廠商profac(<>'*':全部)
 '---當搜尋條件:(1)列印狀態為"已列印且列印批號為空白"時不可列印(2)廠商為全部且列印批號為空白時,不可列印
 '---當搜尋條件:(3)廠商為全部,不可列印
  if (X(1)="：全部" and X(3)="<>'*'") or (X(3)="<>'*'") or (X(4)<>"：已入帳") then
     ButtonEnable="N;N;Y;Y;Y;N"
  end if
End Sub
%>
