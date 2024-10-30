Imports System.Drawing
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.Win32
Imports Microsoft.Win32.SafeHandles
Imports System.IO

Public Class Form1

    Private LastUpdate As DateTime = DateTime.MinValue
    Private timerHandle As IntPtr
    Private autoResetEvent As AutoResetEvent
    Private selectedFontSize As Integer = 46
    Private selectedTextColor As Color = Color.White
    Private useSystemThemeColor As Boolean = False
    Private settingsFilePath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WeekNumberSettings.txt")

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load settings from file
        LoadSettings()

        ' Set NotifyIcon properties
        notifyIconWeekNumber.Icon = SystemIcons.Application ' Temporary icon until we generate the week number icon
        notifyIconWeekNumber.Visible = True
        notifyIconWeekNumber.Text = "Week Number"
        notifyIconWeekNumber.ContextMenuStrip = CreateContextMenu() ' Add context menu to allow exit

        ' Update the tray icon initially
        UpdateWeekNumberIcon()

        AddHandler SystemEvents.TimeChanged, AddressOf OnTimeChanged
        AddHandler SystemEvents.UserPreferenceChanged, AddressOf SystemEvents_UserPreferenceChanged

        ' Create a waitable timer
        timerHandle = CreateWaitableTimer(IntPtr.Zero, False, Nothing)
        If timerHandle = IntPtr.Zero Then Return

        ' Set the timer for midnight
        SetMidnightTimer()
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ' Hide the form completely once it has been shown initially
        Me.Hide()
        Me.ShowInTaskbar = False ' Ensure the form doesn't show in the taskbar
    End Sub

    Private Function GetSystemThemeColor() As Color
        ' Determine if system is using light or dark mode
        Try
            Dim registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Themes\Personalize")
            Dim value As Object = registryKey.GetValue("SystemUsesLightTheme")
            Dim isLightTheme As Boolean = (value IsNot Nothing AndAlso value = 1)

            ' Return the appropriate color
            If isLightTheme Then
                Return Color.Black ' Use black text for light theme
            Else
                Return Color.White ' Use white text for dark theme
            End If
        Catch ex As Exception
            ' Default to white color if any error occurs
            Return Color.White
        End Try
    End Function

    Private Sub UpdateWeekNumberIcon()
        ' Get current week number
        Dim currentWeekNumber As Integer = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)

        ' Create bitmap and graphics to draw text
        Using bmp As New Bitmap(64, 64)
            Using g As Graphics = Graphics.FromImage(bmp)
                ' Set background color
                g.Clear(Color.Transparent)

                ' Determine text color based on system theme or user selection
                Dim textColor As Color = If(useSystemThemeColor, GetSystemThemeColor(), selectedTextColor)

                ' Draw the week number
                Using font As New Font("Aptos", selectedFontSize, FontStyle.Regular, GraphicsUnit.Pixel)
                    Using brush As New SolidBrush(textColor)
                        ' Calculate text size to align it with the system tray
                        Dim text As String = currentWeekNumber.ToString()
                        Dim textSize As SizeF = g.MeasureString(text, font)
                        Dim x As Single = (bmp.Width - textSize.Width) / 2
                        Dim y As Single = (bmp.Height - textSize.Height) / 2 + 4 ' Adjust y-position to align with other tray icons

                        g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                        g.DrawString(text, font, brush, x, y)
                    End Using
                End Using
            End Using

            ' Convert the Bitmap to an Icon
            Dim iconHandle As IntPtr = bmp.GetHicon()
            Dim weekIcon As Icon = Icon.FromHandle(iconHandle)

            ' Update NotifyIcon with the new icon
            notifyIconWeekNumber.Icon = weekIcon
        End Using
    End Sub

    Private Function CreateContextMenu() As ContextMenuStrip
        ' Create a context menu with options for font size, text color, and exit
        Dim contextMenu As New ContextMenuStrip()

        ' Font size menu
        Dim fontSizeMenuItem As New ToolStripMenuItem("Font Size")
        fontSizeMenuItem.DropDownItems.Add("Small", Nothing, Sub() SetFontSize(30))
        fontSizeMenuItem.DropDownItems.Add("Medium", Nothing, Sub() SetFontSize(46))
        fontSizeMenuItem.DropDownItems.Add("Large", Nothing, Sub() SetFontSize(60))
        contextMenu.Items.Add(fontSizeMenuItem)

        ' Text color menu
        Dim textColorMenuItem As New ToolStripMenuItem("Text Color")
        textColorMenuItem.DropDownItems.Add("System Theme", Nothing, Sub() SetTextColor(Color.Empty, True))
        textColorMenuItem.DropDownItems.Add("Black", Nothing, Sub() SetTextColor(Color.Black, False))
        textColorMenuItem.DropDownItems.Add("White", Nothing, Sub() SetTextColor(Color.White, False))
        textColorMenuItem.DropDownItems.Add("Red", Nothing, Sub() SetTextColor(Color.Red, False))
        textColorMenuItem.DropDownItems.Add("Green", Nothing, Sub() SetTextColor(Color.Green, False))
        textColorMenuItem.DropDownItems.Add("Blue", Nothing, Sub() SetTextColor(Color.Blue, False))
        contextMenu.Items.Add(textColorMenuItem)

        ' Exit menu
        Dim exitMenuItem As New ToolStripMenuItem("Exit", Nothing, AddressOf ExitMenuItem_Click)
        contextMenu.Items.Add(exitMenuItem)

        Return contextMenu
    End Function

    Private Sub SetFontSize(size As Integer)
        selectedFontSize = size
        SaveSettings()
        UpdateWeekNumberIcon()
    End Sub

    Private Sub SetTextColor(color As Color, useSystemTheme As Boolean)
        If useSystemTheme Then
            useSystemThemeColor = True
        Else
            useSystemThemeColor = False
            selectedTextColor = color
        End If
        SaveSettings()
        UpdateWeekNumberIcon()
    End Sub

    Private Sub ExitMenuItem_Click(sender As Object, e As EventArgs)
        ' Clean up and exit the application
        notifyIconWeekNumber.Visible = False
        Application.Exit()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseHandle(timerHandle)
        RemoveHandler SystemEvents.TimeChanged, AddressOf OnTimeChanged
    End Sub

    Private Sub OnTimeChanged()
        ' Update the week number icon whenever the system time changes
        UpdateWeekNumberIcon()
        ' Update the midnight timer as the system time has changed
        SetMidnightTimer()
    End Sub



    Private Sub SystemEvents_UserPreferenceChanged(sender As Object, e As UserPreferenceChangedEventArgs)
        If e.Category = UserPreferenceCategory.General Then
            If useSystemThemeColor Then
                selectedTextColor = GetSystemThemeColor()
            End If
            UpdateWeekNumberIcon()
        End If
    End Sub


    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function CreateWaitableTimer(lpTimerAttributes As IntPtr, bManualReset As Boolean, lpTimerName As String) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function SetWaitableTimer(hTimer As IntPtr, pDueTime As Long(), lPeriod As Integer, pfnCompletionRoutine As IntPtr, lpArgToCompletionRoutine As IntPtr, fResume As Boolean) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function CloseHandle(hObject As IntPtr) As Boolean
    End Function

    Private Sub SetMidnightTimer()
        ' Calculate the time span until midnight
        Dim now As DateTime = DateTime.Now
        Dim midnight As DateTime = now.Date.AddDays(1) ' Next midnight
        Dim timeUntilMidnight As TimeSpan = midnight - now

        ' Convert to 100-nanosecond intervals (negative value for relative time)
        Dim dueTime As Long = -timeUntilMidnight.Ticks

        ' Set the waitable timer
        Dim success As Boolean = SetWaitableTimer(timerHandle, New Long() {dueTime}, 0, IntPtr.Zero, IntPtr.Zero, False)

        If success Then
            ' Create a new AutoResetEvent to wait for the timer signal
            autoResetEvent = New AutoResetEvent(False)
            Dim safeWaitHandle As New SafeWaitHandle(timerHandle, False)
            autoResetEvent.SafeWaitHandle = safeWaitHandle

            ' Create a new thread to wait for the timer
            Dim timerThread As New Thread(AddressOf WaitForTimer)
            timerThread.IsBackground = True
            timerThread.Start()
        Else
            ' Log an error if the timer could not be set
            Console.WriteLine("Failed to set waitable timer.")
        End If
    End Sub

    Private Sub WaitForTimer()
        autoResetEvent.WaitOne()
        Me.Invoke(Sub()
                      UpdateWeekNumberIcon()
                      SetMidnightTimer()
                  End Sub)
    End Sub

    Private Sub SaveSettings()
        Try
            Using writer As New StreamWriter(settingsFilePath, False)
                writer.WriteLine(selectedFontSize.ToString())
                writer.WriteLine(If(useSystemThemeColor, "SystemTheme", selectedTextColor.ToArgb().ToString()))
            End Using
        Catch ex As Exception
            Console.WriteLine("Failed to save settings: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadSettings()
        Try
            If File.Exists(settingsFilePath) Then
                Using reader As New StreamReader(settingsFilePath)
                    selectedFontSize = Integer.Parse(reader.ReadLine())
                    Dim colorValue = reader.ReadLine()
                    If colorValue = "SystemTheme" Then
                        useSystemThemeColor = True
                    Else
                        useSystemThemeColor = False
                        selectedTextColor = Color.FromArgb(Integer.Parse(colorValue))
                    End If
                End Using
            End If
        Catch ex As Exception
            selectedFontSize = 46
            selectedTextColor = GetSystemThemeColor()
            SaveSettings()
            Console.WriteLine("Failed to load settings: " & ex.Message)
        End Try
    End Sub

End Class
