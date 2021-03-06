Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading

Public Class AnyWordAppendBody
    Public Delegate Sub WriteToText(ByVal text As String) '写全部log到窗体的委托
    Private writeToForm As WriteToText
    Public Delegate Sub WriteThreadToText(ByVal text As String) '告诉线程结束给窗体的委托
    Private writeThread As WriteThreadToText
    ' Private askMainThreadDone As threadDone
    Public Property taskPath As String
    Public Property encoding As Encoding = Encoding.UTF8
    Private Property adSmart As String
    Private fileList As New List(Of String)
    Private reg As New RegularExpressions.Regex("")
    Private bodyReg As New RegularExpressions.Regex("<body(([A-Za-z0-9\s\-]{1,}=""[A-Za-z0-9\-\s\#\(\)\;\:\,\.\/\?\=_\']{0,}""){1,}|(\s){0,})>")
    Private anyword As String



    Public Sub New(ByVal path As String, ByVal word As String)
        writeToForm = AddressOf Form1.writeLog '绑定委托方法

        writeThread = AddressOf Form1.writeToThreadLog '绑定委托方法
        taskPath = path
        Dim regexWord = ReplaceWordByRegex(word)
        reg = New Regex(regexWord)
        anyword = word
    End Sub

    Private Function ReplaceWordByRegex(word As String) As Object
        word = word.Replace("?", "\?")
        word = word.Replace("&", "\&")
        word = word.Replace("(", "\(")
        word = word.Replace(")", "\)")

        Return word
    End Function

    Public Sub ExcuteBodyTask()



        'Dim fso As Object = CreateObject("scripting.filesystemobject") '创建来FSO对象
        'Dim files As Object = fso.getfolder(taskPath)
        Dim d As New System.IO.DirectoryInfo(taskPath)
        Dim f As System.IO.FileInfo
        '  writeToForm("run in task:" & vbCrLf)

        For Each f In d.GetFiles
            If f.Name.ToString.Equals("index.html") OrElse f.Name.ToString.Equals("index.htm") Then
                Dim html As String = String.Empty
                Dim str = taskPath + "\" + f.Name.ToString
                writeToForm("find website index:" + str & vbCrLf)
                html = File.ReadAllText(str, GetEncoding(str))
                '  adSmart = anyword

                adSmart = reg.Match(html).ToString
                If adSmart = anyword Then
                    writeToForm("find  text:" + adSmart & vbCrLf)
                End If
            End If
        Next

        getFiles(taskPath, 1)
        writeToForm("this directory " + taskPath + ",have :" + fileList.Count.ToString + "" & vbCrLf)
        If Not anyword = "" Then
            UpdateHtml()
        End If

        'For Each file In fileList
        '    writeToForm("in this directory :" + taskPath + ",get file name :" + file & vbCrLf)
        'Next
        fileList.Clear()

        writeThread("thread:" + taskPath + "  ;done!" & vbCrLf)

        ' askMainThreadDone()

    End Sub




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



    Private Sub getFiles(ByVal path As String, ByVal flag As Integer)
        If String.IsNullOrEmpty(path) Then
            Return
        End If

        If Not flag = 10 Then
            flag = flag + 1
        Else
            Return
        End If

        Dim nextFloderPath As New List(Of String)
        Dim itemFileList As New List(Of String)

        nextFloderPath.AddRange(Directory.GetDirectories(path))


        fileList.AddRange(Directory.GetFiles(path, "*.html"))



        For Each nextPath In nextFloderPath
            getFiles(nextPath, flag)
        Next

    End Sub



    Private Sub UpdateHtml()

        ' Dim adSmartWithBody = "<body>" + adSmart
        Dim html As String = String.Empty
        If fileList.Count = 0 Then
        Else
            Try
                For Each filepath In fileList
                    html = File.ReadAllText(filepath, GetEncoding(filepath))
                    If Not reg.IsMatch(html) Then '如果没有先前添加匹配

                        Dim bodyContent = bodyReg.Match(html).ToString
                        html = bodyReg.Replace(html, bodyContent + anyword)
                        File.WriteAllText(filepath, html, System.Text.Encoding.UTF8)
                    End If
                Next
                writeToForm("this directory is: " + taskPath + ",already complete!" + vbCrLf)
            Catch ex As Exception
                '  txt_replace.Text = ex.Message
                writeToForm("UpdateHtml Error:" + ex.Message + vbCrLf)
            End Try
        End If




    End Sub
End Class
