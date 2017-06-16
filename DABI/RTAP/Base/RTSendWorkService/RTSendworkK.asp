<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="客戶RT發包作業(客服部)"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;" & v(3)
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="發　包;發包撤銷;完　工;完工撤銷;發包記錄"
  functionOptProgram="RTSndWorkInfo.asp;RTDropWorkInfo.asp;RTSndWorkok.asp;RTDropWorkok.asp;RTSendWorkHisK.ASP"
  functionOptPrompt="Y;Y;Y;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=FALSE
  formatName="none;客戶代碼;單次;客戶;電話;地址;應收金額;通知發包日;發包日期;派工單號;完工日期;備　註"
  sqlDelete="SELECT a.comq1,a.CUSID,a.entryno, b.CUSNC, a.HOME, Rtrim(IsNull(a.cutid1,'')) + rtrim(IsNull(a.TOWNSHIP1,''))+rtrim(IsNull(a.RADDR1,'')) as addr1, a.AR,a.sndinfodat, a.REQDAT, " _
           &"a.insprtno,a.finishdat,a.UNFINISHDESC " _
           &"FROM rtcust a, rtobj b, rtobj c " _
           &"WHERE a.cusid *= b.cusid AND a.profac *= c.cusid and a.settype not in ('2','3') " _
           &"and a.cusid='*' " 
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
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="self" 
  searchFirst=false
  DSQL=Session("DSQL")
  If searchQry="" Then
     searchQry=" and a.comq1=" & aryparmkey(0)& "  "
     searchShow=FrGetCmtyDesc(aryParmKey(0))
     searchshow=searchshow & ""
  End If
  sqlList="SELECT a.comq1,a.CUSID,a.entryno, b.CUSNC, a.HOME, Rtrim(IsNull(a.cutid1,'')) + rtrim(IsNull(a.TOWNSHIP1,''))+rtrim(IsNull(a.RADDR1,'')) as addr1, a.AR,a.sndinfodat, a.REQDAT, " _
           &"a.insprtno,a.finishdat,a.UNFINISHDESC " _
           &"FROM rtcust a, rtobj b, rtobj c,rtarea d,rtcmty e,rtareacty f " _
           &"WHERE " & DSQL & " and a.cusid *= b.cusid and a.comq1=e.comq1 and e.cutid=f.cutid and f.areaid=d.areaid and d.areatype='1' AND a.profac *= c.cusid and a.settype not in ('2','3')  "  & searchqry &" " _
           &"ORDER BY c.cusnc,b.Cusnc "
        '   Response.Write "SQL=" & SQllist
End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->