<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="RT派工單列印作業(技術部)"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;" & v(3)
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="列印確認"
  functionOptProgram="verify.ASP"
  functionOptPrompt="Y;Y;Y;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=FALSE
  formatName="none;none;none;社區;客戶;單次;地址;應收金額;發包日;派工單號;施工廠商;完工日"
  sqlDelete="SELECT a.comq1,a.cusid,a.entryno,b.COMN, c.CUSNC,a.entryno,Rtrim(IsNull(a.cutid1,'')) + rtrim(IsNull(a.TOWNSHIP1,''))+rtrim(IsNull(a.RADDR1,'')) as addr1, a.AR, a.reqdat,a.insprtno , f.CUSNC, a.finishdat " _
           & "FROM   RTCust a, RTCmty b, RTObj c, RTCmtySale d, RTObj e, RTObj f " _
           & "WHERE  a.COMQ1 = b.COMQ1 " _
           & "AND a.CUSID = c.CUSID " _
           & "AND a.COMQ1 =d.COMQ1  AND GetDate() Between d.TDAT AND IsNull(d.EXDAT, '9999/12/31') " _
           & "AND d.CUSID = e.CUSID " _
           & "AND a.PROFAC *= f.CUSID and a.settype in ('2','3') " _
           & "and a.cusid='*' "
  dataTable="RTCust"
  extTable=""
  numberOfKey=3
  dataProg="/webap/rtap/base/rtcmty/RTCustD.asp"
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
  searchProg="RTSendWorkPrtS.asp" 
  searchFirst=false
  If searchQry="" Then
     SD=cstr(datevalue(now())-1)
     ED=cstr(datevalue(now()))
     searchQry=" and a.reqdat is not null and a.insprtdat is null and a.finishdat is null  and a.reqdat between '" & sd + " 00:00:00.000 " & "' and '" & ED + " 23:59:59.997' "  & ";;0;<>'*'"
    ' searchShow=FrGetCmtyDesc(aryParmKey(0))
     searchshow="施工廠商：全部　派工進度狀況：已發包,未列印,未完工  發包日期：自(" & SD & ")至(" & ED & ")"
  End If
  X=split(searchqry,";")
  sqlList="SELECT a.comq1,a.cusid,a.entryno,b.COMN, c.CUSNC,a.entryno," _
           & "Rtrim(IsNull(a.cutid1,'')) + rtrim(IsNull(a.TOWNSHIP1,''))+rtrim(IsNull(a.RADDR1,'')) as addr1," _
           & "a.AR, a.reqdat,a.insprtno , case when a.settype='3' then f.CUSNC when a.settype='2' then '技術部安裝' end as Xcusnc, a.finishdat " _
           & "FROM   RTCust a, RTCmty b, RTObj c, RTCmtySale d, RTObj e, RTObj f " _
           & "WHERE  a.COMQ1 = b.COMQ1 " _
           & "AND a.CUSID = c.CUSID " _
           & "AND a.COMQ1 =d.COMQ1  AND GetDate() Between d.TDAT AND IsNull(d.EXDAT, '9999/12/31') " _
           & "AND d.CUSID = e.CUSID " _
           & "AND a.PROFAC *= f.CUSID and a.settype in ('2','3') " & X(0) &" " _
           &"ORDER BY b.comn,c.cusnc "
  sqlstr= "FROM   RTCust a, RTCmty b, RTObj c, RTCmtySale d, RTObj e, RTObj f " _
           & "WHERE  a.COMQ1 = b.COMQ1 " _
           & "AND a.CUSID = c.CUSID " _
           & "AND a.COMQ1 =d.COMQ1  AND GetDate() Between d.TDAT AND IsNull(d.EXDAT, '9999/12/31') " _
           & "AND d.CUSID = e.CUSID " _
           & "AND a.PROFAC *= f.CUSID and a.settype in ('2','3') " & X(0) &" " 
        
         ' Response.Write "SQL=" & SQllist
 session("SQLSTRWorkPRT")=replace(SQLstr,"'","""")
 session("ExistPrtNo")= X(1)    
 if V(3)="Y" and x(3)<>"<>'*'" and (len(trim(x(1))) > 0 or  X(2)="0") then
    ButtonEnable="N;N;Y;Y;Y;Y"
 else
    ButtonEnable="N;N;Y;Y;Y;N"
 end if  
End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->