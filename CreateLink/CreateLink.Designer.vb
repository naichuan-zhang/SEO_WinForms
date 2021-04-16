<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CreateLink
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.oldText = New System.Windows.Forms.TextBox()
        Me.newText = New System.Windows.Forms.TextBox()
        Me.CreateBtn = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'oldText
        '
        Me.oldText.Location = New System.Drawing.Point(12, 104)
        Me.oldText.Multiline = True
        Me.oldText.Name = "oldText"
        Me.oldText.Size = New System.Drawing.Size(366, 334)
        Me.oldText.TabIndex = 0
        '
        'newText
        '
        Me.newText.Location = New System.Drawing.Point(388, 104)
        Me.newText.Multiline = True
        Me.newText.Name = "newText"
        Me.newText.Size = New System.Drawing.Size(400, 334)
        Me.newText.TabIndex = 1
        '
        'CreateBtn
        '
        Me.CreateBtn.Location = New System.Drawing.Point(315, 12)
        Me.CreateBtn.Name = "CreateBtn"
        Me.CreateBtn.Size = New System.Drawing.Size(147, 40)
        Me.CreateBtn.TabIndex = 2
        Me.CreateBtn.Text = "Create Link"
        Me.CreateBtn.UseVisualStyleBackColor = True
        '
        'CreateLink
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.CreateBtn)
        Me.Controls.Add(Me.newText)
        Me.Controls.Add(Me.oldText)
        Me.Name = "CreateLink"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents oldText As TextBox
    Friend WithEvents newText As TextBox
    Friend WithEvents CreateBtn As Button
End Class
