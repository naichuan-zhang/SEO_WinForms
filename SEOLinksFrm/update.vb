Imports System.IO
Imports System.Text

Public Class update
    Public Shared fileList As New List(Of String)
    Public itemReplaceStr As String = String.Empty
    Public ReplaceStr As String = String.Empty
    Public encoding As Encoding = Encoding.UTF8
    Private reg As New RegularExpressions.Regex("<!--begain seo for link-->(.|\n)*?<!--end seo for link-->")
    Private threadLock As Object = New Object()
    Public txt_replaceText As String = String.Empty
    Dim th As Threading.Thread



    Private Sub SelectItemReplace(ByVal folderPath As String)

        getFiles(folderPath, 1)
        'itemReplaceStr = String.Empty
        Dim html As String = String.Empty
        If fileList.Count > 0 Then
            For Each str As String In fileList
                html = File.ReadAllText(str, GetEncoding(str))
                If reg.IsMatch(html) Then
                    ' txt_replace.Text = reg.Match(html).ToString
                    'itemReplaceStr = reg.Match(html).ToString
                    '  btn_select.Enabled = True
                    '  th.Abort()
                    Return
                End If
            Next
            If String.IsNullOrEmpty(itemReplaceStr) Then
                '    txt_replace.Text = "html files no this tag<!--begain seo for link--><!--end seo for link-->"

            End If

        Else
            '  txt_replace.Text = "No html files"
        End If
        '   btn_select.Enabled = True
        ' th.Abort()
    End Sub

    Private Sub getFiles(ByVal path As String, ByVal flag As Integer)
        If String.IsNullOrEmpty(path) Then
            Return
        End If

        If Not flag = 3 Then 'SubLevel.Value
            flag = flag + 1
        Else
            Return
        End If

        Dim nextFloderPath As New List(Of String)
        Dim itemFileList As New List(Of String)

        nextFloderPath.AddRange(Directory.GetDirectories(path))

        If rdo_index.Checked Then
            'fileList.AddRange(Directory.GetFiles(path, "index.html"))
            'fileList.AddRange(Directory.GetFiles(path, "main.html"))
            itemFileList.AddRange(Directory.GetFiles(path, "index.html"))
            itemFileList.AddRange(Directory.GetFiles(path, "main.html"))
            itemFileList.AddRange(Directory.GetFiles(path, "home.html"))
            fileList.AddRange(itemFileList)
            If itemFileList.Count = 0 Then
                For Each nextPath In nextFloderPath
                    getFiles(nextPath, flag)
                Next
            End If
            Return
        End If

        If rdo_allIndex.Checked Then
            'fileList.AddRange(Directory.GetFiles(path, "index.html"))
            'fileList.AddRange(Directory.GetFiles(path, "main.html"))
            itemFileList.AddRange(Directory.GetFiles(path, "index.html"))
            itemFileList.AddRange(Directory.GetFiles(path, "main.html"))
            itemFileList.AddRange(Directory.GetFiles(path, "home.html"))
            fileList.AddRange(itemFileList)

            For Each nextPath In nextFloderPath
                getFiles(nextPath, flag)
            Next

            Return
        End If

        fileList.AddRange(Directory.GetFiles(path, "*.html"))
        If rdo_current.Checked Then
            Return
        Else
            For Each nextPath In nextFloderPath
                getFiles(nextPath, flag)
            Next
        End If
    End Sub



    Public Overloads Function GetEncoding(ByVal fileName As String) As Encoding
        Dim fs As FileStream = New FileStream(fileName, FileMode.Open, FileAccess.Read)
        encoding = GetEncoding(fs)
        fs.Close()
        Return encoding
    End Function
End Class
