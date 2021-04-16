Imports System.Threading

Public Class Form1
    Private ThreadMaxCount As Integer = 5
    Dim waitList As New List(Of String)
    Dim webSiteList As New List(Of String)
    Dim webSiteDoingList As New List(Of String)
    Dim ThreadingList As List(Of System.Threading.Thread) = New List(Of System.Threading.Thread)
    Dim adSmartBtn As Boolean = False
    Dim anyWordBtn As Boolean = False
    Dim BrokenLinkBtn As Boolean = False
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False

    End Sub

    Private Sub excute(task As Task)
        Try
            task.ExcuteTask()
        Catch ex As Exception

            txt_Detail.AppendText("Error:" & ex.Message + vbCrLf)

        End Try

    End Sub

    Private Sub excuteByAnyWord(taskByWord As AnyWordAppendBody)
        Try
            taskByWord.ExcuteBodyTask()
        Catch ex As Exception

            txt_Detail.AppendText("Error:" & ex.Message + vbCrLf)

        End Try

    End Sub

    Private Sub excuteByUpdateBrokenLink(updateBrokenLink As UpdateBrokenLink)
        Try
            updateBrokenLink.ExcuteBodyTask()
        Catch ex As Exception

            txt_Detail.AppendText("Error:" & ex.Message + vbCrLf)

        End Try

    End Sub

    'adSmart 的按钮
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles add.Click
        For Each strWait As String In TextBox1.Text.Trim.Replace(vbCrLf, " ").Split(" ")
            If strWait.StartsWith("C:\WebSite") Then
                waitList.Add(strWait.Trim)
            End If
        Next

        StatisticsWebSite(waitList)
        adSmartBtn = True
        startWork()
    End Sub



    Private Sub StatisticsWebSite(ByVal waitList As List(Of String))
        For Each mainFolderName In waitList

            If RadioButton2.Checked Then
                webSiteList.Add(mainFolderName)
            Else
                If System.IO.Directory.Exists(mainFolderName) Then
                    Dim nextFolders() As String = IO.Directory.GetDirectories(mainFolderName)
                    For Each nextFolder In nextFolders
                        webSiteList.Add(nextFolder)
                        txt_Detail.AppendText("Add webSite to List:" & nextFolder & vbCrLf)
                    Next
                End If
            End If

        Next
    End Sub

    Private Sub startWork()
        While ThreadingList.Count < ThreadMaxCount AndAlso webSiteList.Count > 0
            Dim ThreadingTask As New System.Threading.Thread(AddressOf excute)
            ThreadingList.Add(ThreadingTask)
            ThreadingTask.Start(New Task(webSiteList(0)))
            webSiteList.RemoveAt(0)

        End While
    End Sub

    Private Sub startWorkByAnyWord()
        While ThreadingList.Count < ThreadMaxCount AndAlso webSiteList.Count > 0
            Dim ThreadingTask As New System.Threading.Thread(AddressOf excuteByAnyWord)
            ThreadingList.Add(ThreadingTask)
            ThreadingTask.Start(New AnyWordAppendBody(webSiteList(0), TextBox3.Text))
            webSiteList.RemoveAt(0)

        End While
    End Sub


    Private Sub starUpdateBrokenLink()
        While ThreadingList.Count < ThreadMaxCount AndAlso webSiteList.Count > 0
            Dim ThreadingTask As New System.Threading.Thread(AddressOf excuteByUpdateBrokenLink)
            ThreadingTask.Name = webSiteList(0)
            ThreadingList.Add(ThreadingTask)
            ThreadingTask.Start(New UpdateBrokenLink(webSiteList(0), TextBox3.Text))
            webSiteList.RemoveAt(0)

        End While
    End Sub

    Private Sub CheckThreadStatus()
        If ThreadingList.Count = 0 Then
            Return
        End If

        '返回True表示存在已完成的线程

        Threading.Thread.Sleep(100)

        Dim maxCount = ThreadingList.Count - 1
        For i As Integer = 0 To ThreadingList.Count - 1
            If i <= maxCount Then
                If ThreadingList(i).IsAlive = False Then
                    ThreadingList(i).Abort()
                    ThreadingList.Remove(ThreadingList(i))
                    maxCount = maxCount - 1
                End If
            End If
        Next

        'For Each ThreadingTaskItem In ThreadingList
        '    'ThreadingTaskItem.IsAlive
        '    If ThreadingTaskItem.IsAlive = False Then
        '        ThreadingTaskItem.Abort()
        '        ThreadingList.Remove(ThreadingTaskItem)
        '    End If
        'Next


        'If ThreadingList.Count <= 2 Then
        '    For Each ThreadingTaskItem In ThreadingList
        '        If ThreadingTaskItem.IsAlive = False Then
        '            Return True
        '        End If
        '    Next
        'End If


    End Sub


    Public Sub writeLog(ByVal text As String)
        txt_Detail.AppendText(text)
    End Sub

    'Public Sub threadDone()

    '    If CheckThreadStatus() Then
    '        startWork()
    '    End If

    'End Sub
    Public Sub writeToThreadLog(ByVal text As String)
        TextBox2.AppendText(text)
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If adSmartBtn = True Then
            CheckThreadStatus()
            startWork()
        End If

        If anyWordBtn = True Then
            CheckThreadStatus()
            startWorkByAnyWord()
        End If

        If BrokenLinkBtn = True Then
            CheckThreadStatus()
            For Each th In ThreadingList
                txt_Detail.AppendText("Thread name:" & th.Name & vbCrLf)
            Next

            starUpdateBrokenLink()
        End If
    End Sub


    '添加任意字符的的按钮
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        For Each strWait As String In TextBox1.Text.Trim.Replace(vbCrLf, " ").Split(" ")
            If strWait.StartsWith("C:\WebSite") Then
                waitList.Add(strWait.Trim)
            End If
        Next

        StatisticsWebSite(waitList)
        anyWordBtn = True
        startWorkByAnyWord()
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For Each strWait As String In TextBox1.Text.Trim.Replace(vbCrLf, " ").Split(" ")
            If strWait.StartsWith("C:\WebSite") Then
                waitList.Add(strWait.Trim)
            End If
        Next

        StatisticsWebSite(waitList)
        BrokenLinkBtn = True
        starUpdateBrokenLink()
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged

    End Sub
End Class
