Imports Console = System.Console

Public Class Colorful
    Public Shared Sub Print(ByVal text As String, ByVal color As ConsoleColor)
        Console.ForegroundColor = color
        Console.WriteLine(text)
        Console.ResetColor()
    End Sub
End Class

