
Imports System.IO



Public Class WebRegister
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Sub MotrarCarpetaArchivo_Click()

        Dim rutaCarpeta_ As String = tbCarpetaArchivo.Text

        ' Verifica si la carpeta existe
        If Directory.Exists(rutaCarpeta_) Then
            ' Obtiene todos los archivos en la carpeta
            Dim archivos_() As String = Directory.GetFiles(rutaCarpeta_)

            ' Limpia el ListBox antes de agregar nuevos elementos (si lo usas para mostrar)
            tbArchivosContenido.Text = ""

            ' Muestra los nombres de los archivos
            For Each archivo As String In archivos_
                ' Puedes mostrar el nombre completo del archivo (con ruta)
                ' Console.WriteLine(archivo) ' Para mostrar en la ventana de Salida de Visual Studio
                ' ListBox1.Items.Add(archivo) ' Para agregar al ListBox

                ' O solo el nombre del archivo sin la ruta
                Dim nombreArchivo As String = Path.GetFileName(archivo)
                tbArchivosContenido.Text &= nombreArchivo & "," ' Para agregar al ListBox
            Next

            Dim directorios_() As String = Directory.GetDirectories(rutaCarpeta_)

            ' Limpia el ListBox antes de agregar nuevos elementos
            tbCarpetasDentro.Text = "" ' Asegúrate de tener un ListBox llamado ListBox1 en tu formulario

            ' Muestra los nombres de las subcarpetas
            For Each directorio_ As String In directorios_
                ' Puedes mostrar la ruta completa de la subcarpeta
                ' Console.WriteLine(directorio) ' Para mostrar en la ventana de Salida de Visual Studio
                ' ListBox1.Items.Add(directorio) ' Para agregar al ListBox

                ' O solo el nombre de la subcarpeta sin la ruta
                Dim nombreSubcarpeta_ As String = Path.GetFileName(directorio_)

                Console.WriteLine(nombreSubcarpeta_) ' Para mostrar en la ventana de Salida de Visual Studio

                tbCarpetasDentro.Text &= nombreSubcarpeta_ & "," ' Para agregar al ListBox
            Next
        Else
            tbCarpetasDentro.Text = "No existe esa carpeta"
        End If

    End Sub

    Sub MostrarContenidoArchivo_Click()

        If Not File.Exists(tbCarpetaArchivo.Text) Then

        End If

        Try
            ' Usa StreamReader para leer el archivo.
            Using sr As New StreamReader(tbCarpetaArchivo.Text, System.Text.Encoding.UTF8)
                tbArchivosContenido.Text = sr.ReadToEnd() ' Lee todo el contenido del archivo de una vez
            End Using
        Catch ex As Exception

        End Try

    End Sub



    Sub AsignarPrivilegios_Click()

        Try
            ' Usa StreamWriter para escribir en el archivo.
            ' El segundo parámetro 'False' indica que se sobrescribirá el archivo si ya existe.
            ' Si quieres añadir al final sin borrar lo anterior, usa 'True'.
            Using sw As New StreamWriter("C:\SAX\ZERG.txt", False, System.Text.Encoding.UTF8)

                sw.Write("ENTRO AL CONSTRUCTOR Y CARGO EL STATEMENT")

            End Using


        Catch ex As Exception

        End Try

    End Sub


End Class