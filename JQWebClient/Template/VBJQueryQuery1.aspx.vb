
Partial Class Template_VBJQueryQuery1
    Inherits System.Web.UI.Page

    Public Overrides Sub ProcessRequest(ByVal context As HttpContext)
        If Not JqHttpHandler.ProcessRequest(context) Then
            MyBase.ProcessRequest(context)
        End If
    End Sub

End Class
