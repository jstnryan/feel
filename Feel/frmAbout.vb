Public Class frmAbout

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        'txtAcknowledgements.SelectionLength = 0
        With System.Reflection.Assembly.GetExecutingAssembly.GetName.Version
            'TODO: replace the compile date
            lblVersion.Text = "Version: " & .Major & "." & .Minor & "." & .Build & "." & .Revision & " (" & Date.Today.Month & " " & Date.Today.Year & ")"
        End With

        MyBase.OnLoad(e)
    End Sub
End Class