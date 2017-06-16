<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->

<%
Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    conn.Execute("usp_ATableDesc '" & aryparmkey(0) & "'")
    conn.Close
Set conn=Nothing

Sub SrEnvironment()
  company=application("company")
  system="HI-Building管理系統"
  title="Table欄位資料維護檔"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  'ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y"
  buttonEnable="Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLIb"
  formatName="none;序號;欄位名稱(英);欄位名稱(中);資料類別;主鍵;Null;預設值;Identity;備註"
  sqlDelete="SELECT tbName, colOrdinal, colName, colNameC, colDataType, " &_
			"       colIsKey, colIsNull, colDefault, colIsIdentity, colDesc " &_
            "FROM   AColumnList WHERE tbName ='*' " 
  dataTable=" AColumnList"
  numberOfKey=2
  dataProg="RTColumnD.asp"  
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
  keyListPageSize=80
  'searchProg="rtctysearch.asp"
  'If searchQry="" Then
     searchQry=" tbName='" & aryparmkey(0) & "'"
  '   searchShow=FrGetctyDesc(aryParmKey(0))       
  'End If
  sqlList="SELECT tbName, colOrdinal, colName, colNameC, colDataType, colIsKey, " &_
          "       colIsNull, colDefault, colIsIdentity, colDesc " &_
          "  FROM AColumnList WHERE " & searchQry & " " &_
          " ORDER BY colOrdinal "
'Response.Write sqlList
End Sub
%>
