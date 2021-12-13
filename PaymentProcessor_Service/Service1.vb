Imports Dapper
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ServiceProcess
Imports System.Threading

Namespace PaymentService
    Partial Public Class Service1
        Inherits ServiceBase

        Private timer As Timer
        Private INTERVAL As Integer = 10


        Public Sub New(ByVal args As String())

        End Sub

        Protected Overrides Sub OnStart(ByVal args As String())
            StartTheTimer()
        End Sub

        Public Sub OnDebug()
            OnStart(Nothing)
        End Sub

        <Obsolete>
        Private Sub StartTheTimer()
            Try
                timer = New Timer(New TimerCallback(AddressOf TimerCallback))
                Dim runTime As DateTime = DateTime.Now.AddSeconds(INTERVAL)

                If DateTime.Now > runTime Then
                    runTime = runTime.AddSeconds(INTERVAL)
                End If

                Dim timeSpan As TimeSpan = runTime.Subtract(DateTime.Now)
                Dim dueTime As Integer = Convert.ToInt32(timeSpan.TotalMilliseconds)
                timer.Change(dueTime, Timeout.Infinite)
            Catch ex As Exception
            End Try
        End Sub

        <Obsolete>
        Private Sub TimerCallback(ByVal e As Object)
            Try



            Catch ex As Exception

            End Try

            Me.StartTheTimer()
        End Sub

        Protected Overrides Sub OnStop()
        End Sub

        Friend WithEvents Timer1 As Windows.Forms.Timer
        Private components As ComponentModel.IContainer

        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
            '
            'Timer1
            '
            '
            'Service1
            '
            Me.ServiceName = "Service1"

        End Sub

        Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
            Dim MyLog As New EventLog() ' create a new event log
            ' Check if the the Event Log Exists
            If Not MyLog.SourceExists("MyService") Then
                MyLog.CreateEventSource("MyService", "Myservice Log")
                ' Create Log
            End If
            MyLog.Source = "MyService"
            ' Write to the Log
            MyLog.WriteEntry("MyService Log", "This is log on " &
                              CStr(TimeOfDay),
                              EventLogEntryType.Information)
        End Sub
    End Class
End Namespace