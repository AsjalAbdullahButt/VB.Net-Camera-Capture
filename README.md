# CameraCaptureApp

A simple Windows Forms application built with VB.NET to capture images from a webcam (including Canon EOS Webcam Utility), preview them live, and save them as JPEG files. It also provides a list of previously saved images and a preview feature.

## 📸 Features

- Live camera preview using AForge.NET
- Support for multiple video input devices
- Canon EOS Webcam Utility support (auto-detected if available)
- Capture and save images in JPEG format
- Keyboard shortcuts:
  - `Spacebar` to capture an image
  - `Esc` to close the application
- Preview previously saved images in the interface

## 🔧 Technologies Used

- **VB.NET (Visual Basic .NET)**
- **Windows Forms**
- **AForge.NET Framework** for video capture and frame handling
- **.NET Framework**

## 📁 Project Structure

- `Form1.vb`: Main form with camera integration logic
- `captured_images/`: Folder where images are saved
- Uses `AForge.Video` and `AForge.Video.DirectShow` for video input

## 🚀 Getting Started

1. Clone this repository:
    ```bash
    git clone https://github.com/your-username/CameraCaptureApp.git
    ```

2. Open the project in **Visual Studio**.

3. Restore or add the required AForge.NET libraries (you can use NuGet or manually add references).

4. Build and run the project.

5. Select a camera from the dropdown and start capturing!

## ✅ Requirements

- Windows OS
- Visual Studio (preferably 2019 or later)
- .NET Framework installed
- A connected webcam (external or built-in)

## 📦 Dependencies

- [AForge.NET](http://www.aforgenet.com/framework/) (Video and DirectShow libraries)

## 🔗 Canon EOS Webcam Utility

To use your **Canon DSLR/Mirrorless camera** as a webcam, download and install Canon EOS Webcam Utility:

- **Free Version (Single Camera Support)**  
  👉 [Download from Canon Official Site](https://en.canon-cna.com/cameras/eos-webcam-utility/)

- **Pro Version (Multi-Camera, Overlays, Wireless, 60fps)**  
  👉 [EOS Webcam Utility Pro (Subscription)](https://www.usa.canon.com/digital-cameras/eos-webcam-utility)

This application automatically detects and supports EOS Webcam Utility if installed.

Feel free to contribute, suggest improvements, or fork the project!
