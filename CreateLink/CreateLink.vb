Public Class CreateLink
    Private Sub CreateBtn_Click(sender As Object, e As EventArgs) Handles CreateBtn.Click
        If Not oldText.Text.Trim.Equals("") Then
            Dim lines = oldText.Lines
            Dim list As New List(Of String)
            For Each line In lines
                If Not line.Contains("http") And Not line.Trim.Equals("") Then
                    list.Add(line)
                Else
                    If Not list.Count = 0 Then
                        For Each title In list
                            newText.Text = newText.Text + "<a title='" & title & "' href='" & line & "'>" & title & "</a>|" & vbCrLf
                        Next
                        list.Clear()
                    End If
                End If


            Next


        End If
    End Sub
End Class
