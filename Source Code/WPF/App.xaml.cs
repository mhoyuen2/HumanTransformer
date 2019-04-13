//------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace HumanTransformer
{
    using System;
    using System.Windows;
    using Microsoft.Kinect.Wpf.Controls;

    /// <summary>
    /// Interaction logic for App
    /// </summary>
    public partial class App : Application
    {
        ///<summary>
        /// Gets the app level KinectRegion element, 
        ///which is created in App.xaml.cs
        ///</summary>
        internal KinectRegion KinectRegion { get; set; }
    }
}
