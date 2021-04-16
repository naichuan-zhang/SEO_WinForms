<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.updateBtn = New System.Windows.Forms.Button()
        Me.TextPath = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'oldText
        '
        Me.oldText.Location = New System.Drawing.Point(22, 97)
        Me.oldText.Multiline = True
        Me.oldText.Name = "oldText"
        Me.oldText.Size = New System.Drawing.Size(754, 331)
        Me.oldText.TabIndex = 0
        '
        'updateBtn
        '
        Me.updateBtn.Location = New System.Drawing.Point(598, 27)
        Me.updateBtn.Name = "updateBtn"
        Me.updateBtn.Size = New System.Drawing.Size(120, 50)
        Me.updateBtn.TabIndex = 2
        Me.updateBtn.Text = "Update Link"
        Me.updateBtn.UseVisualStyleBackColor = True
        '
        'TextPath
        '
        Me.TextPath.Location = New System.Drawing.Point(55, 49)
        Me.TextPath.Name = "TextPath"
        Me.TextPath.Size = New System.Drawing.Size(289, 21)
        Me.TextPath.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 52)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 12)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Path"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextPath)
        Me.Controls.Add(Me.updateBtn)
        Me.Controls.Add(Me.oldText)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents oldText As TextBox
    Friend WithEvents updateBtn As Button
    Friend WithEvents TextPath As TextBox
    Friend WithEvents Label1 As Label
End Class
