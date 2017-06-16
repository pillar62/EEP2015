<%
  Dim search1,parm,vk,debug36,search2
  parm=request("Key")
  vk=split(parm,";")
  if ubound(vk) > 0 then  searchX=vK(0)
%>
<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Sub SrEnvironment()
  company=application("company")
  system="HI-Building管理系統"
  title="RT收款審核確認"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="收款審核"
  functionOptPrompt="Y"
  functionOptProgram="verify.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  
  DSN="DSN=RTLIb"
  formatName="none;none;none;社區名稱;用戶名;電話;地址;收款金額;收款日;工程師;承包商;列印批號;財務確認;none;確認員"
  sqlDelete="SELECT b.comq1,a.cusid,a.entryno,b.COMN, c.CUSNC, a.HOME, Rtrim(IsNull(a.cutid1,'')) + rtrim(IsNull(a.TOWNSHIP1,''))+rtrim(IsNull(a.RADDR1,'')) as addr1, a.ACTRCVAMT, a.SCHDAT, e.CUSNC, f.CUSNC, a.RCVDTLNO, a.FINRDFMDAT, a.FINCFMUSR, h.CUSNC " _
            & "FROM RTObj h INNER JOIN RTEmployee g ON h.CUSID = g.CUSID RIGHT OUTER JOIN RTCust a INNER JOIN " _
            & "RTCmty b ON a.COMQ1 = b.COMQ1 INNER JOIN " _
            & "RTObj c ON a.CUSID = c.CUSID INNER JOIN " _
            & "RTCmtySale d ON a.COMQ1 = d.COMQ1 INNER JOIN "_
            & "RTObj e ON d.CUSID = e.CUSID ON " _
            & "g.EMPLY = a.FINCFMUSR LEFT OUTER JOIN " _
            & "RTObj f ON a.PROFAC = f.CUSID " _
            & "WHERE (GETDATE() BETWEEN d.TDAT AND ISNULL(d.EXDAT, '9999/12/31')) " _
            & "and a.cusid<>'*' "  
  'response.write "sql=" &sqldelete
  dataTable="b"
  numberOfKey=3
  dataProg="/webap/rtap/base/rtcmty/rtcustd.asp"
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
     searchQry=" and a.rcvdtlno='" & aryparmkey(0) & "'"
     searchShow="未審核"
  End If   
  sqlList="SELECT b.comq1,a.cusid,a.entryno,b.COMN, c.CUSNC, a.HOME,  Rtrim(IsNull(a.cutid1,'')) + rtrim(IsNull(a.TOWNSHIP1,''))+rtrim(IsNull(a.RADDR1,'')) as addr1, a.ACTRCVAMT, a.SCHDAT, e.CUSNC, f.CUSNC, a.RCVDTLNO, a.FINRDFMDAT, a.FINCFMUSR, h.CUSNC " _
            & "FROM RTObj h INNER JOIN RTEmployee g ON h.CUSID = g.CUSID RIGHT OUTER JOIN RTCust a INNER JOIN " _
            & "RTCmty b ON a.COMQ1 = b.COMQ1 INNER JOIN " _
            & "RTObj c ON a.CUSID = c.CUSID INNER JOIN " _
            & "RTCmtySale d ON a.COMQ1 = d.COMQ1 INNER JOIN "_
            & "RTObj e ON d.CUSID = e.CUSID ON " _
            & "g.EMPLY = a.FINCFMUSR LEFT OUTER JOIN " _
            & "RTObj f ON a.PROFAC = f.CUSID " _
            & "WHERE (GETDATE() BETWEEN d.TDAT AND ISNULL(d.EXDAT, '9999/12/31')) " _
            & "and a.cusid<>'*' " &searchqry & " order by b.comn,c.cusnc "
 session("revcfmprtno")=aryparmkey(0)
 'Response.Write "SQL=" & SQllist
End Sub
%>