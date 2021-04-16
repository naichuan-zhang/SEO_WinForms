Imports System.IO
Imports System.Runtime.Remoting.Messaging
Imports System.Text
Imports System.Text.RegularExpressions

Public Class Form1
    Public encoding As Encoding = Encoding.UTF8
    Public linkList As New ArrayList
    Private reg As New RegularExpressions.Regex("<!--begain seo for link-->(.|\n)*?<!--end seo for link-->")
    Private Sub updateBtn_Click(sender As Object, e As EventArgs) Handles updateBtn.Click
        Try
            linkList = InsertMapByText()

            Dim fso As Object
            Dim folder As Object
            If Not TextPath.Text.Trim.Equals("") Then
                fso = CreateObject("scripting.filesystemobject") '创建来FSO对象
                folder = fso.getfolder(TextPath.Text)
                Dim html As String = String.Empty
                For Each f In folder.Files
                    Dim filePath = TextPath.Text + "\" + f.name.ToString
                    html = File.ReadAllText(filePath, GetEncoding(filePath))
                    For Each linkInfo In linkList
                        Dim title = GetTitleByLink(linkInfo)
                        Dim linkreg = "<a title='" & title & "' href='(.*)'>" & title & "</a>"
                        reg = New RegularExpressions.Regex(linkreg)
                        html = reg.Replace(html, linkInfo)

                    Next
                    File.WriteAllText(filePath, html, System.Text.Encoding.UTF8)
                    ' html = reg.Replace(html, ReplaceStr)
                Next
            End If

            oldText.Text = "complete!"
        Catch ex As Exception
            oldText.Text = "error"
        End Try

    End Sub

    Private Function GetTitleByLink(ByVal link As String) As String
        '获取a链接中的title
        Dim r As Regex
        Dim m As Match
        r = New Regex("^<a title='(.*)' href='(.*)'>(.*)</a>")
        m = r.Match(link)
        While m.Success
            Dim name = m.Groups.Item(1).ToString
            Return name
        End While

        Return ""
    End Function
    Private Function InsertMapByText() As ArrayList
        Try
            If Not oldText.Text.Trim.Equals("") Then
                Dim lines = oldText.Lines
                Dim list As New List(Of String)
                For Each line In lines
                    If Not line.Contains("http") And Not line.Trim.Equals("") Then
                        list.Add(line)
                    Else
                        If Not list.Count = 0 Then
                            For Each title In list
                                linkList.Add("<a title='" & title & "' href='" & line & "'>" & title & "</a>")
                            Next
                            list.Clear()
                        End If
                    End If
                Next
                Return linkList
            End If
            Return Nothing
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
            Return Nothing
        End Try
    End Function

    Public Overloads Function GetEncoding(ByVal fileName As String) As Encoding
        Dim fs As FileStream = New FileStream(fileName, FileMode.Open, FileAccess.Read)
        encoding = GetEncoding(fs)
        fs.Close()
        Return encoding
    End Function

    '  
    ' 判断文件流编码类型  
    '  
    '   
    Private Overloads Function GetEncoding(ByVal fs As FileStream) As Encoding
        Dim r As BinaryReader = New BinaryReader(fs, System.Text.Encoding.Default)
        Dim ss() As Byte = r.ReadBytes(3)
        r.Close()

        If (ss(0) >= 239) Then
            Return Encoding.UTF8
            If ((ss(0) = 254) _
                        AndAlso (ss(1) = 255)) Then
                Return Encoding.BigEndianUnicode
            ElseIf ((ss(0) = 255) _
                        AndAlso (ss(1) = 254)) Then
                Return Encoding.Unicode
            Else
                Return Encoding.Default
            End If
        Else
            Return Encoding.Default
        End If
    End Function
End Class
