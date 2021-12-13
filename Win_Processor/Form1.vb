Imports RequestConsumer

Public Class Form1
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1_Processor.Tick
        Try
            Dim worker As New Worker

            worker.DeQueueRequest()

        Catch ex As Exception

        End Try
    End Sub
End Class
