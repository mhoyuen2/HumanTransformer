//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace HumanTransformer
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using MathWorks.MATLAB.NET.Arrays;
    using IMFDalgorithm;
    using WpfAnimatedGif;
    using System.Windows.Threading;
    using System.Threading;
    using Microsoft.Kinect.Toolkit.Input;
    using Microsoft.Kinect.Wpf.Controls;
    using KinectBackgroundRemoval;
    using System.Collections.Generic;
    using LightBuzz.Vitruvius;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Size of the RGB pixel in the bitmap
        /// </summary>
        private readonly int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Reader for depth/color/body index frames
        /// </summary>
        private MultiSourceFrameReader multiFrameSourceReader = null;

        /// <summary>
        /// Bitmap to display
        /// </summary>
        private WriteableBitmap bitmap = null;

        /// <summary>
        /// Bitmap to capture the background image
        /// </summary>
        private WriteableBitmap capturedBackground = null;

        /// <summary>
        /// The size in bytes of the bitmap back buffer
        /// </summary>
        private uint bitmapBackBufferSize = 0;

        /// <summary>
        /// Intermediate storage for the color to depth mapping
        /// </summary>
        private DepthSpacePoint[] colorMappedToDepthPoints = null;

        /// <summary>
        /// Current status text to display
        /// </summary>
        private string statusText = null;

        /// <summary>
        /// Compiled library from Matlab for the morphing algorithm
        /// </summary>
        private ClassIMFD morphingEngine = null;

        /// <summary>
        /// DispatcherTimer for five seconds delay
        /// </summary>
        private DispatcherTimer dispatcherTimer;

        /// <summary>
        /// Variables to control the UI
        /// </summary>
        private int scene = 0;
        private Boolean morphBtnClicked = false;
        private Boolean CapBgBtnClicked = false;
        private Boolean ARButtonClicked = false;

        /// <summary>
        /// Variables for store the path of images
        /// </summary>
        private string[,] sceneObject = null;
        private string[] stickerObject = null;

        /// <summary>
        /// Module for image segmentation 
        /// </summary>
        BackgroundRemovalTool _backgroundRemovalTool;

        /// <summary>
        /// Module for gesture detection
        /// </summary>
        GestureController _gestureController;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            this.kinectSensor = KinectSensor.GetDefault();

            this.multiFrameSourceReader = this.kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Depth | FrameSourceTypes.Color | FrameSourceTypes.BodyIndex | FrameSourceTypes.Body);

            this.multiFrameSourceReader.MultiSourceFrameArrived += this.Reader_MultiSourceFrameArrived;

            this.coordinateMapper = this.kinectSensor.CoordinateMapper;

            FrameDescription depthFrameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;

            int depthWidth = depthFrameDescription.Width;
            int depthHeight = depthFrameDescription.Height;

            FrameDescription colorFrameDescription = this.kinectSensor.ColorFrameSource.FrameDescription;

            int colorWidth = colorFrameDescription.Width;
            int colorHeight = colorFrameDescription.Height;

            this.colorMappedToDepthPoints = new DepthSpacePoint[colorWidth * colorHeight];

            this.bitmap = new WriteableBitmap(colorWidth, colorHeight, 96.0, 96.0, PixelFormats.Bgra32, null);
            this.capturedBackground = new WriteableBitmap(colorWidth, colorHeight, 96.0, 96.0, PixelFormats.Bgra32, null);


            // Calculate the WriteableBitmap back buffer size
            this.bitmapBackBufferSize = (uint)((this.bitmap.BackBufferStride * (this.bitmap.PixelHeight - 1)) + (this.bitmap.PixelWidth * this.bytesPerPixel));

            this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            this.kinectSensor.Open();

            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;

            this.DataContext = this;

            this.InitializeComponent();

            this.morphingEngine = new ClassIMFD();

            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, 5);

            KinectRegion.SetKinectRegion(this, kinectRegion);

            App app = ((App)Application.Current);
            app.KinectRegion = kinectRegion;

            // Use the default sensor
            this.kinectRegion.KinectSensor = KinectSensor.GetDefault();

            this.sceneObject = new string[2, 4] { { "\\Images\\BackgroundLion.png", "\\Images\\GestureLion.png", "Images\\Lion.png", "\\Images\\Lion2.png" },
                         { "\\Images\\BackgroundAngryBird.png", "\\Images\\GestureAngryBird.png", "Images\\AngryBird.png", "\\Images\\AngryBird.gif" } };
            this.stickerObject = new string[4] { "\\Sticker1.gif", "\\Images\\Sticker2.gif", "\\Images\\Sticker2.gif", "\\Images\\Sticker3.gif" };

            this._backgroundRemovalTool = new BackgroundRemovalTool(this.kinectSensor.CoordinateMapper);

            this._gestureController = new GestureController();
            this._gestureController.GestureRecognized += GestureController_GestureRecognized;

            UpdateBackground();
        }

        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the bitmap to display
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return this.bitmap;
            }
        }

        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        public string StatusText
        {
            get
            {
                return this.statusText;
            }

            set
            {
                if (this.statusText != value)
                {
                    this.statusText = value;

                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.multiFrameSourceReader != null)
            {
                // MultiSourceFrameReder is IDisposable
                this.multiFrameSourceReader.Dispose();
                this.multiFrameSourceReader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        /// <summary>
        /// Handles the depth/color/body index frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            using (var colorFrame = reference.ColorFrameReference.AcquireFrame())
            using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
            using (var bodyIndexFrame = reference.BodyIndexFrameReference.AcquireFrame())
            {
                if (colorFrame != null && depthFrame != null && bodyIndexFrame != null)
                {
                    // 3) Update the image source.
                    Camera.Source = _backgroundRemovalTool.GreenScreen(colorFrame, depthFrame, bodyIndexFrame);
                    CaptureOriginalColorFrame(colorFrame);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary> 
        private void GestureController_GestureRecognized(object sender, GestureEventArgs e)
        {
            if (ARButtonClicked && scene == 1)
            {
                var gesture = e.GestureType;
                String path = null;
                switch (gesture)
                {
                    case (GestureType.SwipeLeft):
                        path = Directory.GetCurrentDirectory() + stickerObject[0];
                        break;
                    case (GestureType.SwipeRight):
                        path = Directory.GetCurrentDirectory() + stickerObject[1];
                        break;
                    case (GestureType.WaveLeft):
                        path = Directory.GetCurrentDirectory() + stickerObject[2];
                        break;
                    case (GestureType.WaveRight):
                        path = Directory.GetCurrentDirectory() + stickerObject[3];
                        break;
                }

                if (path != null)
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(path, UriKind.Absolute); ;
                    image.EndInit();
                    ImageBehavior.SetAnimatedSource(Animation, image);

                    Animation.Visibility = Visibility.Visible;
                    dispatcherTimer.Start();
                }
            }
        }

        /// <summary>
        /// Tick function for dispatcher timer
        /// </summary>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (morphBtnClicked)
            {
                GestureTutorial.Visibility = Visibility.Collapsed;
                dispatcherTimer.IsEnabled = false;
                morphBtnClicked = false;

                GenerateMorphingSequence();

            }
            else if (CapBgBtnClicked)
            {
                GestureTutorial.Visibility = Visibility.Collapsed;
                dispatcherTimer.IsEnabled = false;
                CapBgBtnClicked = false;

                CaptureBackground();
            }
            else if (Animation.Visibility == Visibility.Visible)
            {
                Animation.Visibility = Visibility.Collapsed;
                dispatcherTimer.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles to click ARmodebtuoon
        /// </summary>
        private void ARModeButton_Click(object sender, RoutedEventArgs e)
        {
            ARButtonClicked = true;
            Camera.Visibility = Visibility.Collapsed;
            CapturedBackground.Visibility = Visibility.Visible;
            TransformObject.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles to click capbgbutton
        /// </summary>
        private void CapBgButton_Click(object sender, RoutedEventArgs e)
        {
            CapBgBtnClicked = true;
            String gifPath = Directory.GetCurrentDirectory() + "\\Images\\InformCapturedBackground.png";
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(gifPath, UriKind.Absolute); ;
            image.EndInit();
            ImageBehavior.SetAnimatedSource(GestureTutorial, image);

            GestureTutorial.Visibility = Visibility.Visible;
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Save background as png image
        /// </summary>
        private void CaptureBackground()
        {
            if (this.capturedBackground != null)
            {
                // create a png bitmap encoder which knows how to save a .png file
                BitmapEncoder encoder = new PngBitmapEncoder();

                // create frame from the writable bitmap and add to encoder
                encoder.Frames.Add(BitmapFrame.Create(this.capturedBackground));

                string path = "Images\\CapturedBackground.png";

                // write the new file to disk
                try
                {
                    // FileStream is IDisposable
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }

                    this.StatusText = string.Format(Properties.Resources.SavedScreenshotStatusTextFormat, path);

                    String path1 = Directory.GetCurrentDirectory() + "\\Images\\CapturedBackground.png";

                    CapturedBackground.Source = new BitmapImage(new Uri(path1, UriKind.Absolute));
                }
                catch (IOException)
                {
                    this.StatusText = string.Format(Properties.Resources.FailedScreenshotStatusTextFormat, path);
                }
            }
        }

        /// <summary>
        /// Use color frame and store to a WritableBitmap
        /// </summary>
        private void CaptureOriginalColorFrame(ColorFrame e)
        {

            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e)
            {
                if (colorFrame != null)
                {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        this.capturedBackground.Lock();

                        // verify data and write the new color frame data to the display bitmap
                        if ((colorFrameDescription.Width == this.capturedBackground.PixelWidth) && (colorFrameDescription.Height == this.capturedBackground.PixelHeight))
                        {
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                this.capturedBackground.BackBuffer,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);

                            this.capturedBackground.AddDirtyRect(new Int32Rect(0, 0, this.capturedBackground.PixelWidth, this.capturedBackground.PixelHeight));
                        }

                        this.capturedBackground.Unlock();
                    }
                }
            }

        }

        /// <summary>
        /// Handles the user clicking on the screenshot button
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MorphButton_Click(object sender, RoutedEventArgs e)
        {
            morphBtnClicked = true;

            // Show Gesture Tutorial Image
            String gifPath = Directory.GetCurrentDirectory() + sceneObject[scene, 1];
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(gifPath, UriKind.Absolute); ;
            image.EndInit();
            ImageBehavior.SetAnimatedSource(GestureTutorial, image);

            // Set Visible
            GestureTutorial.Visibility = Visibility.Visible;

            // Start five seconds delay
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Handles the user clicking on the screenshot button
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void GenerateMorphingSequence()
        {
            // Create a render target to which we'll render our composite image
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)CompositeImage.ActualWidth, (int)CompositeImage.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(CompositeImage);
                dc.DrawRectangle(brush, null, new Rect(new Point(), new Size(CompositeImage.ActualWidth, CompositeImage.ActualHeight)));
            }

            renderBitmap.Render(dv);

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            string path = "Images\\Human.png";

            // Write the new file to disk
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    encoder.Save(fs);
                }

                this.StatusText = string.Format(Properties.Resources.SavedScreenshotStatusTextFormat, path);

                morphingEngine.IMFDalgorithm(sceneObject[scene, 2], "Images\\Human.png", (MWArray)8, (MWArray)0.1);

                String gifPath = Directory.GetCurrentDirectory() + "\\Images\\MorphResult_" + sceneObject[scene, 2].Substring(7, 2) + ".gif";

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(gifPath, UriKind.Absolute); ;
                image.EndInit();
                ImageBehavior.SetAnimatedSource(MorphGif, image);

            }
            catch (IOException)
            {
                this.StatusText = string.Format(Properties.Resources.FailedScreenshotStatusTextFormat, path);
            }
        }

        /// <summary>
        /// Handles the change scene button
        /// </summary>
        private void ChangeSceneButton_Click(object sender, RoutedEventArgs e)
        {
            if (scene == 0)
                scene = 1;
            else if (scene == 1)
                scene = 0;

            UpdateBackground();
            ImageBehavior.SetAnimatedSource(MorphGif, null);
        }

        /// <summary>
        /// Updates Background
        /// </summary>
        private void UpdateBackground()
        {
            String path = Directory.GetCurrentDirectory() + sceneObject[scene, 0];
            BackgroundImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));

            path = Directory.GetCurrentDirectory() + sceneObject[scene, 3];
            if (scene == 0)
            {
                ImageBehavior.SetAnimatedSource(TransformObject, null);
                TransformObject.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            }
            else if (scene == 1)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(path, UriKind.Absolute); ;
                image.EndInit();
                ImageBehavior.SetAnimatedSource(TransformObject, image);
            }
        }

        /// <summary>
        /// Handles the reset scene button
        /// </summary>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ImageBehavior.SetAnimatedSource(MorphGif, null);
            CapturedBackground.Source =  null;
            TransformObject.Visibility = Visibility.Collapsed;
            ImageBehavior.SetAnimatedSource(Animation, null);
            Camera.Visibility = Visibility.Visible;

            scene = 0;
            UpdateBackground();
        }

        /// <summary>
        /// Handles the event which the sensor becomes unavailable (E.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.SensorNotAvailableStatusText;
        }



    }
}
