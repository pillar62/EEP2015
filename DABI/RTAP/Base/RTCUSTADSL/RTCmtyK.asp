<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  '---清除session變數
  session("CMTYNC")=""
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="ADSL(券商專案)資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="客戶明細"
  functionOptProgram="RTCUSTKX.ASP"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;縣市;鄉鎮;社區名稱;戶數"
   sqlDelete="SELECT DISTINCT " _
            &"rtcustadsl.cutid2, RTCustADSL.TOWNSHIP2, RTCustADSL.HOUSENAME,RTCounty.CUTNC, RTCustADSL.TOWNSHIP2, RTCustADSL.HOUSENAME,count(*) " _
            &"FROM RTCustADSL INNER JOIN " _
            &"RTCounty ON RTCustADSL.CUTID2 = RTCounty.CUTID " _
            &"GROUP BY rtcustadsl.cutid2, RTCustADSL.TOWNSHIP2, RTCustADSL.HOUSENAME,RTCounty.CUTNC, RTCustADSL.TOWNSHIP2, RTCustADSL.HOUSENAME " 
  dataTable="RTCUSTADSL"
  userDefineDelete=""
  extTable=""
  numberOfKey=3
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=2
  keyListPageSize=40
  searchProg="RTCustSX.asp"
  searchFirst=false
  If searchQry="" Then
     searchShow="全部"
     searchQry="RTCUSTADSL.cutid2<>'*' "
     searchQry2="HAVING COUNT(*) >=3 "
  ELSE
     SEARCHFIRST=FALSE
  End If  
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  Domain=Mid(Emply,1,1)
  select case Domain
         case "T"
            DAreaID="='A1'"
         case "P"
            DAreaID="='A1'"
         case "C"
            DAreaID="='A2'"         
         case "K"
            DAreaID="='A3'"         
         case else
            DareaID="=''"
  end select
  '高階主管可讀取全部資料
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T89018" or Ucase(emply)="T90076" or _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or  Ucase(emply)="T89020" then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  if userlevel=31 then DAreaID="<>'*'"
  '---做為RTCUSTK.ASP的視別使用,當為"Y"時表示由RTCMTYK呼叫;當為空白時表示直接呼叫!
  SESSION("FALG")="Y"
  sqllist="SELECT DISTINCT " _
         &"RTCustADSL.CUTID2, RTCustADSL.TOWNSHIP2, RTCustADSL.HOUSENAME, " _
         &"RTCounty.CUTNC, RTCustADSL.TOWNSHIP2 , " _
         &"RTCustADSL.HOUSENAME , COUNT(*) " _
         &"FROM RTCustADSL INNER JOIN " _
         &"RTCounty ON RTCustADSL.CUTID2 = RTCounty.CUTID INNER JOIN " _
         &"RTAreaCty ON RTCounty.CUTID = RTAreaCty.CUTID INNER JOIN " _
         &"RTArea ON RTAreaCty.AREAID = RTArea.AREAID AND AREATYPE = '1' " _
         &"WHERE " & searchqry & " AND RTAREACTY.AREAID " & DAreaid  & " and rtcustadsl.housename <> '' " _
         &"GROUP BY RTCustADSL.CUTID2, RTCustADSL.TOWNSHIP2, RTCustADSL.HOUSENAME, " _
         &"RTCounty.CUTNC, RTCustADSL.TOWNSHIP2, RTCustADSL.HOUSENAME, " _
         &"RTAreaCty.AREAID, RTArea.AREATYPE " _
         & searchqry2 
'  "SELECT DISTINCT " _
'            &"rtcustadsl.cutid2, RTCustADSL.TOWNSHIP2, RTCustADSL.HOUSENAME,RTCounty.CUTNC, RTCustADSL.TOWNSHIP2, RTCustADSL.HOUSENAME,count(*) " _
'            &"FROM RTCustADSL INNER JOIN " _
'            &"RTCounty ON RTCustADSL.CUTID2 = RTCounty.CUTID " _
'            &"where " & searchqry & " " _
'            &"GROUP BY rtcustadsl.cutid2, RTCustADSL.TOWNSHIP2, RTCustADSL.HOUSENAME,RTCounty.CUTNC, RTCustADSL.TOWNSHIP2, RTCustADSL.HOUSENAME " _
'            & searchqry2 
  'Response.Write "sql=" & SQLLIST
End Sub

Function SrGetCtyRef(CUTID)
    Dim conn,rs,sql
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    sql="SELECT cutnc FROM RTCounty where cutid='" & cutid & "'" 
    rs.Open sql,conn
    If not rs.Eof Then
       SrGetctyref=rs("cutnc")
    end if
End function
%>
