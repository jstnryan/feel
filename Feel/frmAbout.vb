Public Class frmAbout

    Private Sub frmAbout_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'txtAcknowledgements.SelectionLength = 0
        With System.Reflection.Assembly.GetExecutingAssembly.GetName.Version
            lblVersion.Text = "Version: " & .Major & "." & .Minor & "." & .Build & "." & .Revision
        End With
    End Sub
End Class