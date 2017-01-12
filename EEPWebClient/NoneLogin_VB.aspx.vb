Partial Class NoneLogin_VB
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Dim langs As String() = Request.UserLanguages
            Try
                If String.Compare(langs(0), "zh-cn", True) = 0 Then 'IgnoreCase
                    Srvtools.CliUtils.fClientLang = SYS_LANGUAGE.SIM
                ElseIf String.Compare(langs(0), "zh-tw", True) = 0 Then 'IgnoreCase
                    Srvtools.CliUtils.fClientLang = SYS_LANGUAGE.TRA
                Else
                    Srvtools.CliUtils.fClientLang = SYS_LANGUAGE.ENG
                End If
            Catch
                Srvtools.CliUtils.fClientLang = SYS_LANGUAGE.ENG
            End Try
        End If

        Dim strPath As String = Request.Path
        strPath = Request.MapPath(strPath)
        strPath = strPath.Substring(0, (strPath.LastIndexOf(Microsoft.VisualBasic.ChrW(92)) + 1))
        Srvtools.CliUtils.LoadLoginServiceConfig(strPath + "EEPWebClient.exe.config")

        If Register() = False Then
            Return
        End If

        Srvtools.CliUtils.fClientSystem = "Web"
        Srvtools.CliUtils.fLoginUser = "001"
        Srvtools.CliUtils.fLoginPassword = ""
        Srvtools.CliUtils.fLoginDB = "ERPS"
        Srvtools.CliUtils.fCurrentProject = "Solution1"

        Dim sParam As String = (Srvtools.CliUtils.fLoginUser + (Microsoft.VisualBasic.ChrW(58) _
            + (Srvtools.CliUtils.fLoginPassword + (Microsoft.VisualBasic.ChrW(58) _
            + (Srvtools.CliUtils.fLoginDB + (Microsoft.VisualBasic.ChrW(58) + "0"))))))
        Dim myRet() As Object = Srvtools.CliUtils.CallMethod("GLModule", "CheckUser", New Object() {CType(sParam, Object)})
        Dim result As Srvtools.LoginResult = CType(myRet(1), Srvtools.LoginResult)

        If (result = Srvtools.LoginResult.UserNotFound) Then
            Me.FailureText.Text = "User or Password is error."
        ElseIf (result = Srvtools.LoginResult.UserNotFound) Then
            Me.FailureText.Text = "User Not Found."
        ElseIf (result = Srvtools.LoginResult.Disabled) Then
            Me.FailureText.Text = "User has been disabled"
        Else
            Srvtools.CliUtils.fUserName = myRet(2).ToString
            Srvtools.CliUtils.GetPasswordPolicy()
            myRet = Srvtools.CliUtils.CallMethod("GLModule", "GetUserGroup", New Object() {Srvtools.CliUtils.fLoginUser})
            If ((Not (myRet) Is Nothing) _
                        AndAlso (CType(myRet(0), Integer) = 0)) Then
                Srvtools.CliUtils.fGroupID = myRet(1).ToString
            End If
            Response.Redirect("webClientMain.aspx", True)
        End If

    End Sub
    Private language As SYS_LANGUAGE
    Private Function Register() As Boolean
        Dim message As String = ""
        Dim rtn As Boolean = Srvtools.CliUtils.Register(message)
        If rtn Then
            Dim path As String = String.Format("{0}\\{1}", EEPRegistry.WebClient, "sysmsg.xml")
            Srvtools.CliUtils.GetSysXml(path)

        Else
            Me.FailureText.Text = message
        End If
        Return rtn
    End Function
End Class
