<%
Sub SrGetBranchBuss(accessMode,sw,protect,buss,BRANCH,bussman,s,t,U)
    'USERTYPE:���n�J�t�Ϊ��ϥΪ����O=>'1'����~�� '2'���~�ȭ�
    'USERID:���n�J�t�Τ��ϥΪ̨����Ҧr��
    Dim conn,rs,sql,sx,USERTYPE,USERID
    'USERTYPE=SESSION("LOGONUSRTYPE")
    'USERID=SESSION("LOGONUSERID")
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    '���
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then 
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '06') AND RTOBJ.CUSID NOT IN ('70770184', '47224065') "
       s="<option value="""" >(�Ҩ餽�q)</option>"
    Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC  " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '06') AND RTOBJ.CUSID='" & BUSS & "'"
       s=""
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(�Ҩ餽�q)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=BUSS Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    '����
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then   
       SQL="SELECT * FROM RTBRANCH WHERE CUSID='" & BUSS &"' ORDER BY BRANCH "
       rs.Open SQL,conn     
       t=""
       If rs.Eof Then t="<option value="""" >(����)</option>"  
       sx=""    
       Do While Not rs.Eof    
          If rs("BRANCH")=BRANCH Then sx=" selected "    
          t=t &"<option value=""" &rs("BRANCH") &"""" &sx &">" &rs("BRANCH") &"</option>"    
          rs.MoveNext    
          sx=""    
       Loop    
       rs.Close    
    Else    
       t="<option value=""" &BRANCH &""" selected>" &BRANCH &"</option>"    
    End If
    '��~��
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then    
       rs.Open "SELECT RTBUSSMAN.CUSID,RTOBJ.CUSNC FROM RTBussMan INNER JOIN RTObj ON RTBussMan.CUSID = RTObj.CUSID " _
             & "WHERE STOCKID='" & BUSS &"' AND BRANCH='" & BRANCH & "' ORDER BY BRANCH ",conn    
       U=""
       If rs.Eof Then u="<option value="""" >(��~��)</option>"  
       sx=""    
       Do While Not rs.Eof    
          If rs("CUSID")=BUSSMAN Then sx=" selected "    
          U=U &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("CUSNC") &"</option>"    
          rs.MoveNext    
          sx=""    
       Loop    
       rs.Close    
    Else    
       U="<option value=""" &bussman &""" selected>" &bussman &"</option>"    
    End If    
    conn.Close
    Set rs=Nothing
    Set conn=Nothing 
End Sub
%>    