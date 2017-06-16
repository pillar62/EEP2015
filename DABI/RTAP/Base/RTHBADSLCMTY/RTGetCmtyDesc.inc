<%
  Function FrGetCmtyDesc(Comq1,LINEQ1,KIND)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
    SELECT CASE KIND
    '元訊599
       CASE "1"
            SQLxx="SELECT * FROM RTCMTY left outer join rtcmtysp on rtcmty.comq1=rtcmtysp.comq1 left outer join rtcounty on rtcmty.cutid=rtcounty.cutid WHERE rtcmty.COMQ1=" &  COMQ1 
             rs.open sqlxx,conn
             if rs.eof then
                COMN=""
             else
                comn=rs("comn")
             end if
             rs.close
    '中華399   
       CASE "2"
            SQLxx="SELECT * FROM RTCUSTADSLCMTY WHERE CUTYID=" & COMQ1
            rs.open sqlxx,conn
            if rs.eof then
               comn=""
            else
               comn=rs("comn")
            end if
            rs.close
    '速博399   
       CASE "3"
            SQLxx="SELECT * FROM RTSPARQADSLCMTY WHERE CUTYID=" & COMQ1
            rs.open sqlxx,conn
            if rs.eof then
               comn=""
            else
              comn=rs("comn")
            end if
            rs.close
    '東訊599   
       CASE "4"
            SQLxx="SELECT * FROM RTCMTY left outer join rtcmtysp on rtcmty.comq1=rtcmtysp.comq1 left outer join rtcounty on rtcmty.cutid=rtcounty.cutid WHERE rtcmty.COMQ1=" &  COMQ1 
             rs.open sqlxx,conn
             if rs.eof then
                comn=""
             else
                comn=rs("comn")
             end if
             rs.close
     '東森499   
       CASE "5"
            SQLxx="SELECT *, RTEBTCMTYLINE.TOWNSHIP AS XTOWNSHIP, RTEBTCMTYLINE.VILLAGE AS XVILLAGE, RTEBTCMTYLINE.COD1 AS XCOD1, " _
                 &"RTEBTCMTYLINE.NEIGHBOR AS XNEIGHBOR, RTEBTCMTYLINE.COD2 AS XCOD2, RTEBTCMTYLINE.STREET AS XSTREET, RTEBTCMTYLINE.COD3 AS XCOD3, " _
                 &"RTEBTCMTYLINE.SEC AS XSEC,  RTEBTCMTYLINE.COD4 AS XCOD4, RTEBTCMTYLINE.LANE AS XLANE, RTEBTCMTYLINE.COD5 AS XCOD5, RTEBTCMTYLINE.TOWN AS XTOWN, " _
                 &"RTEBTCMTYLINE.COD6 AS XCOD6, RTEBTCMTYLINE.ALLEYWAY AS XALLEYWAY, RTEBTCMTYLINE.COD7 AS XCOD7, RTEBTCMTYLINE.NUM AS XNUM, RTEBTCMTYLINE.COD8 AS XCOD8, " _
                 &"RTEBTCMTYLINE.FLOOR AS XFLOOR, RTEBTCMTYLINE.COD9 AS XCOD9, RTEBTCMTYLINE.ROOM AS XROOM, RTEBTCMTYLINE.COD10 AS XCOD10, RTEBTCMTYLINE.ADDROTHER AS XADDROTHER " _
                 &"FROM RTEBTCMTYLINE INNER JOIN RTEBTCMTYH ON RTEBTCMTYLINE.COMQ1=RTEBTCMTYH.COMQ1 LEFT OUTER JOIN RTCOUNTY ON RTEBTCMTYLINE.CUTID=RTCOUNTY.CUTID WHERE RTEBTCMTYLINE.COMQ1=" &  COMQ1 & " AND RTEBTCMTYLINE.LINEQ1=" &  LINEQ1
            rs.open sqlxx,conn
            if rs.eof then
               comn=""
            else
               comn=rs("comn")
            end if
            rs.close
       CASE ELSE
            COMN=""
    END SELECT
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetCmtyDesc=COMN
End Function
%>