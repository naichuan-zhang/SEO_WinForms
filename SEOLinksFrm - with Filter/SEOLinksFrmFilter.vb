Imports System.IO
Imports System.Text
Public Class SEOLinksFrmFilter

    Private Shared ignoredFilesStr As String = "africaafrica.org
,adriennepauly.com
,all-thats-left.net
,amballbleiben.org
,americanindie.net
,amiens-catholiques-sdf.com
,Angie2006.com
,AtheistBillboards.com
,attentionmarketing.net
,audioannals.com
,bebezaharabenet.com
,bollyhollyhd.com
,bonzaschooldays.com
,burystedmundsdrivingschool.net
,c12superactive.com
,cadaad.org
,ccittv.org
,chernobyltwentyfive.org
,chevroletwtcc.com
,chocoboheaven.com
,choplair.org
,cityhoodforeastla.org
,cityofbaldwyn.com
,croatica.net
,custerchannelwing.com
,der-hoerwurm.de
,dgt-si.org
,DimOkraTiki.org
,downsizeca.org
,DragonNaturallySpeakingSupport.com
,eastonmass.net
,eilisnidhuibhne.net
,electricarcradio.com
,esdocs.org
,experimusic.com
,fenechka.net
,ghalibacademy.org
,gibralfaro.net
,godbrand.net
,greenbriervalley.org
,harmanli-bg.com
,herpscope.com
,hes-inc.org
,hokagesroom.com
,holidays2menorca.com
,howardtone.com
,incitoprima.com
,indl.fr
,InterPolit.net
,iranairjp.com
,john-corabi.net
,KeystoneCurriculum.org
,latrobeuniversitybulletin.com
,lielaiskristaps.org
,live-film.net
,londonlibraries.org
,maemo-apps.org
,maxcaratulas.net
,mazariegos.org
,melodies-modernes.com
,minneapolisksorg.org
,mountbattenofburma.com
,mounthopemonitor.org
,mutiloa.net
,NarragansettBeer.net
,neelscompany.com
,nikeairjordan.cc
,noigenoani.net
,nyrond.org
,offkeymp3.com
,omarbongo.org
,owoot.org
,patrialibre.org
,pixiepressworldwide.com
,preemnik2008.net
,queimada-agency.com
,radiofreemaine.com
,radio-glas.org
,rafvalley.org
,rationalveracity.com
,sceneadvisor.com
,senzafili.org
,settoplinux.org
,skyharbormusic.com
,song-teksten.com
,Sos-ChildrensVillagesKenya.org
,stjsaverne.com
,stmaryspoint.org
,swingwiki.org
,tallshipschool.org
,The-Index.org
,theoccupiedamendment.org
,thomasmermillod.com
,top10rate.com
,truegritcasting.com
,turismoactual.net
,valehospice.org
,victoriajacksonshow.com
,vietnam-dioxine.org
,warlordsofafghanistan.com
,wcrr2011.org
,writersblockmedia.net
,zamislisrbiju.org
,zebandhaniya.com
,zooey-deschanel.net
,stjornlagathing.is"

    Private ignoredFiles As New List(Of String)

    Public Shared fileList As New List(Of String)
    Public itemReplaceStr As String = String.Empty
    Public ReplaceStr As String = String.Empty
    Public encoding As Encoding = Encoding.UTF8
    Private reg As New RegularExpressions.Regex("<!--begain seo for link-->(.|\n)*?<!--end seo for link-->")
    Private threadLock As Object = New Object()
    Public txt_replaceText As String = String.Empty
    Dim th As Threading.Thread
    Private Sub btn_choose_Click(sender As Object, e As EventArgs) Handles btn_choose.Click
        FolderBrowserDialog1.ShowDialog()
        txt_folderPath.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub SEOLinksFrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False


        ' Initial an ignoring files list. The files inside will not be processed.
        Dim list As String() = ignoredFilesStr.Replace(vbCrLf, "").Replace(vbLf, "").Replace(vbCr, "").Split(",")
        For i = 0 To list.Length - 1
            ignoredFiles.Add(list(i))
        Next

    End Sub

    Private Sub btn_select_Click(sender As Object, e As EventArgs)
        txt_replace.Text = "Pls wait..."
        '  btn_select.Enabled = False
        fileList.Clear()
        '   th = New Threading.Thread(New Threading.ThreadStart(AddressOf SelectItemReplace))
        th.Start()

    End Sub

    Private Sub SelectItemReplace(ByVal folderPath As String)

        getFiles(folderPath, 1)
        'itemReplaceStr = String.Empty
        Dim html As String = String.Empty
        If fileList.Count > 0 Then
            For Each str As String In fileList
                html = File.ReadAllText(str, GetEncoding(str))
                If reg.IsMatch(html) Then
                    txt_replace.Text = reg.Match(html).ToString
                    'itemReplaceStr = reg.Match(html).ToString
                    '  btn_select.Enabled = True
                    '  th.Abort()
                    Return
                End If
            Next
            If String.IsNullOrEmpty(itemReplaceStr) Then
                txt_replace.Text = "html files no this tag<!--begain seo for link--><!--end seo for link-->"
            End If

        Else
            txt_replace.Text = "No html files"
        End If
        '   btn_select.Enabled = True
        ' th.Abort()
    End Sub

    '  
    ' 判断文件编码类型  
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
        ElseIf (ss(0) = 60) Then
            Return Encoding.GetEncoding("gb2312")
        Else
            Return Encoding.Default
        End If

    End Function
    '  
    '   
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



    Private Sub getFiles(ByVal path As String, ByVal flag As Integer)
        If String.IsNullOrEmpty(path) Then
            Return
        End If

        If Not flag = SubLevel.Value Then
            flag = flag + 1
        Else
            Return
        End If

        Dim nextFloderPath As New List(Of String)
        Dim itemFileList As New List(Of String)

        ' Add folder filter
        If flag = 2 Then
            Dim dirs As String() = Directory.GetDirectories(path)
            For i = 0 To dirs.Length - 1
                dirs(i) = dirs(i).ToLower()
            Next
            Dim ignoredPaths As New List(Of String)
            For i = 0 To ignoredFiles.Count - 1
                ignoredPaths.Add((path & "\" & ignoredFiles(i)).ToString().ToLower())
            Next
            Dim result = dirs.Except(ignoredPaths).ToArray()
            nextFloderPath.AddRange(result)
        Else
            Dim result = Directory.GetDirectories(path)
            nextFloderPath.AddRange(result)
        End If


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

    Private Sub btn_update_Click(sender As Object, e As EventArgs)
        If Not txt_replace.Text.Trim.Contains("<!--begain seo for link-->") OrElse Not txt_replace.Text.Contains("<!--end seo for link-->") Then
            txt_replace.Text = "Pls add content in this tag<!--begain seo for link--><!--end seo for link-->"
            '   btn_update.Enabled = True
            Return
        End If
        ReplaceStr = txt_replace.Text.Trim
        txt_replace.Text = "Pls wait..."
        '   btn_update.Enabled = False
        th = New Threading.Thread(New Threading.ThreadStart(AddressOf UpdateHtml))
        th.Start()

    End Sub

    Private Sub UpdateHtml()
        SyncLock threadLock
            loging.Text = loging.Text & vbCrLf & "线程Update" + "运行" & vbCrLf
            Dim html As String = String.Empty
            If fileList.Count = 0 Then
                txt_replace.Text = "pls select first"
                '   btn_update.Enabled = True
                '   th.Abort()
                Return
            End If

            Try
                For Each filepath In fileList
                    loging.Text = loging.Text & vbCrLf & "线程Update-》for" + filepath + "运行" & vbCrLf
                    html = File.ReadAllText(filepath, GetEncoding(filepath))
                    html = reg.Replace(html, ReplaceStr)
                    'html = html.Replace(itemReplaceStr, ReplaceStr)
                    File.WriteAllText(filepath, html, GetEncoding(filepath))
                    '  File.WriteAllText(filepath, html, System.Text.Encoding.UTF8)
                Next
                txt_replace.Text = "Complete!"
            Catch ex As Exception
                txt_replace.Text = ex.Message

            End Try
            '  btn_update.Enabled = True
            'th.Abort()
        End SyncLock
    End Sub

    Private Sub btn_addTag_Click(sender As Object, e As EventArgs) Handles btn_addTag.Click
        If fileList.Count < 1 Then
            txt_replace.Text = "No html files..."
            Return
        End If
        txt_replace.Text = "Pls wait..."
        btn_addTag.Enabled = False
        th = New Threading.Thread(New Threading.ThreadStart(AddressOf addTag))
        th.Start()
    End Sub

    Private Sub addTag()
        Dim str As String = String.Empty
        For Each item As String In fileList
            str = File.ReadAllText(item, encoding)
            If reg.IsMatch(str) Then
                Continue For
            End If
            str = str.Replace("</body>", "<!--begain seo for link--><div></div><!--end seo for link--></body>")
            File.WriteAllText(item, str, encoding)
        Next
        txt_replace.Text = "Complete!"
        btn_addTag.Enabled = True
    End Sub

    Private Sub btn_CreateFolder_Click(sender As Object, e As EventArgs) Handles btn_CreateFolder.Click
        Dim filePath As String = txt_folderPath.Text.Trim()
        For Each folderName As String In txt_replace.Text.Trim.Replace(vbCrLf, " ").Split(" ")
            If Not Directory.Exists(Path.Combine(filePath, folderName)) Then
                Directory.CreateDirectory(Path.Combine(filePath, folderName))
            End If
        Next
        txt_replace.Text = "Complete!"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles UpdateByDirc.Click

        Dim fso As Object
        Dim folder As Object
        Dim subfolder As Object
        ' Dim file As Object
        fso = CreateObject("scripting.filesystemobject") '创建来FSO对象
        folder = fso.getfolder("C:\WebSite_txt")
        For Each f In folder.Files '遍历根文件夹源下的文件
            SyncLock Me


                fileList.Clear()
                Dim Index = f.name.ToString.IndexOf(".")
                Dim fn = f.name.ToString.Substring(0, Index)
                Dim html As String = String.Empty
                Dim Str = "C:\WebSite_txt\" + f.name
                '      MsgBox(fn)
                html = File.ReadAllText(Str, System.Text.Encoding.UTF8)
                If reg.IsMatch(html) Then
                    txt_replaceText = reg.Match(html).ToString
                End If
                ' MsgBox(fileName) '输出文2113件名

                'btn_select.Enabled = False

                If System.IO.Directory.Exists("C:\" + fn) = True Then
                    SelectItemReplace("C:\" + fn)

                    If Not reg.IsMatch(html) Then
                        txt_replace.Text = "Pls add content in this tag<!--begain seo for link--><!--end seo for link-->"
                        '   btn_update.Enabled = True
                        Return
                    End If
                    ReplaceStr = txt_replaceText.Trim
                    txt_replace.Text = "Pls wait..."
                    '   btn_update.Enabled = False

                    ' th = New Threading.Thread(New Threading.ThreadStart(AddressOf UpdateHtml))
                    loging.Text = loging.Text & vbCrLf & "线程" + fn + "启动" & vbCrLf
                    UpdateHtml()

                    '  th.Start()



                End If
            End SyncLock

        Next
        fso = Nothing
        folder = Nothing

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles loging.TextChanged

    End Sub
End Class