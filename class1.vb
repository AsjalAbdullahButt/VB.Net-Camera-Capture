Imports AForge.Video
Imports AForge.Video.DirectShow
Imports System.IO
Imports System.Drawing

Public Class Form1

    Private WithEvents PictureBox1 As New PictureBox()
    Private WithEvents PictureBoxPreview As New PictureBox()
    Private WithEvents ListBoxImages As New ListBox()
    Private WithEvents ComboBoxCameras As New ComboBox()
    Private WithEvents CaptureButton As New Button()

    Private videoSource As VideoCaptureDevice
    Private videoDevices As FilterInfoCollection
    Private currentFrame As Bitmap
    Private imageCount As Integer = 0
    Private saveFolder As String = "captured_images"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Setup Form
        Me.Text = "Camera Capture App"
        Me.Width = 1000
        Me.Height = 600
        Me.KeyPreview = True

        ' Setup PictureBox for live camera
        PictureBox1.Width = 640
        PictureBox1.Height = 480
        PictureBox1.Top = 10
        PictureBox1.Left = 10
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        Me.Controls.Add(PictureBox1)

        ' Setup ListBox for saved image list
        ListBoxImages.Top = 10
        ListBoxImages.Left = 660
        ListBoxImages.Width = 300
        ListBoxImages.Height = 200
        Me.Controls.Add(ListBoxImages)

        ' Setup PictureBox for preview
        PictureBoxPreview.Width = 300
        PictureBoxPreview.Height = 270
        PictureBoxPreview.Top = 220
        PictureBoxPreview.Left = 660
        PictureBoxPreview.SizeMode = PictureBoxSizeMode.StretchImage
        Me.Controls.Add(PictureBoxPreview)

        ' Setup ComboBox for camera selection
        ComboBoxCameras.Top = 500
        ComboBoxCameras.Left = 10
        ComboBoxCameras.Width = 300
        AddHandler ComboBoxCameras.SelectedIndexChanged, AddressOf ComboBoxCameras_SelectedIndexChanged
        Me.Controls.Add(ComboBoxCameras)

        ' Setup Capture Button
        CaptureButton.Text = "Capture Image"
        CaptureButton.Top = 500
        CaptureButton.Left = 350
        CaptureButton.Width = 150
        AddHandler CaptureButton.Click, AddressOf CaptureButton_Click
        Me.Controls.Add(CaptureButton)

        ' Create save folder
        If Not Directory.Exists(saveFolder) Then
            Directory.CreateDirectory(saveFolder)
        End If

        ' Load existing images
        LoadSavedImages()

        ' Get available video devices
        videoDevices = New FilterInfoCollection(FilterCategory.VideoInputDevice)
        If videoDevices.Count = 0 Then
            MessageBox.Show("No camera found.")
            Me.Close()
            Return
        End If

        ' Populate camera list and look for EOS Webcam Utility
        Dim selectedIndex As Integer = 0
        Dim eosFound As Boolean = False
        Dim allCams As New List(Of String)

        For i As Integer = 0 To videoDevices.Count - 1
            Dim camName = videoDevices(i).Name
            ComboBoxCameras.Items.Add(camName)
            allCams.Add(camName)

            If camName.ToLower().Contains("eos webcam utility") OrElse camName.ToLower().Contains("webcam utility 3") Then
                selectedIndex = i
                eosFound = True
            End If
        Next

        ' Show available cameras for debug
        MessageBox.Show("Available Cameras:" & vbCrLf & String.Join(vbCrLf, allCams), "Camera Devices")

        ' Notify user if EOS was found
        If eosFound Then
            MessageBox.Show("Canon EOS Webcam Utility detected and selected.")
        End If

        ' Select the detected camera
        ComboBoxCameras.SelectedIndex = selectedIndex
        StartCamera(selectedIndex)

        ' Add image selection handler
        AddHandler ListBoxImages.SelectedIndexChanged, AddressOf ListBoxImages_SelectedIndexChanged
    End Sub

    Private Sub StartCamera(index As Integer)
        If videoSource IsNot Nothing AndAlso videoSource.IsRunning Then
            videoSource.SignalToStop()
            videoSource.WaitForStop()
        End If

        videoSource = New VideoCaptureDevice(videoDevices(index).MonikerString)
        AddHandler videoSource.NewFrame, AddressOf videoSource_NewFrame
        videoSource.Start()
    End Sub

    Private Sub ComboBoxCameras_SelectedIndexChanged(sender As Object, e As EventArgs)
        StartCamera(ComboBoxCameras.SelectedIndex)
    End Sub

    Private Sub LoadSavedImages()
        ListBoxImages.Items.Clear()
        For Each file In Directory.GetFiles(saveFolder, "*.jpg")
            ListBoxImages.Items.Add(Path.GetFileName(file))
        Next
    End Sub

    Private Sub videoSource_NewFrame(sender As Object, eventArgs As NewFrameEventArgs)
        If currentFrame IsNot Nothing Then
            currentFrame.Dispose()
        End If
        currentFrame = CType(eventArgs.Frame.Clone(), Bitmap)
        PictureBox1.Image = CType(currentFrame.Clone(), Bitmap)
    End Sub

    Private Sub CaptureButton_Click(sender As Object, e As EventArgs)
        CaptureAndSaveImage()
    End Sub

    Private Sub CaptureAndSaveImage()
        If currentFrame IsNot Nothing Then
            imageCount += 1
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "JPEG Image|*.jpg"
            saveFileDialog.Title = "Save Image"
            saveFileDialog.FileName = $"image_{imageCount:000}.jpg"

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                currentFrame.Save(saveFileDialog.FileName, Imaging.ImageFormat.Jpeg)
                MessageBox.Show($"Image saved as {saveFileDialog.FileName}")
                LoadSavedImages()
            End If
        End If
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Space Then
            CaptureAndSaveImage()
        ElseIf e.KeyCode = Keys.Escape Then
            If videoSource IsNot Nothing AndAlso videoSource.IsRunning Then
                videoSource.SignalToStop()
                videoSource.WaitForStop()
            End If
            Me.Close()
        End If
    End Sub

    Private Sub ListBoxImages_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ListBoxImages.SelectedItem IsNot Nothing Then
            Dim selectedFile As String = ListBoxImages.SelectedItem.ToString()
            Dim fullPath As String = Path.Combine(saveFolder, selectedFile)

            If File.Exists(fullPath) Then
                PictureBoxPreview.Image = Image.FromFile(fullPath)
            End If
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If videoSource IsNot Nothing AndAlso videoSource.IsRunning Then
            videoSource.SignalToStop()
            videoSource.WaitForStop()
        End If
    End Sub

End Class
