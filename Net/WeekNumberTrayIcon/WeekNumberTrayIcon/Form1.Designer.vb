<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.notifyIconWeekNumber = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.SuspendLayout()
        '
        'notifyIconWeekNumber
        '
        Me.notifyIconWeekNumber.Text = "NotifyIcon1"
        Me.notifyIconWeekNumber.Visible = True
        '
        'Form1
        '
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Name = "Form1"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents notifyIconWeekNumber As NotifyIcon
End Class
