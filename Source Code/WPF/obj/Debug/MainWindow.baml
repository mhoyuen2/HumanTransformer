   M S B A M L   `   `   `  ����N  JLightBuzz.Vitruvius, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nullhGclr-namespace:LightBuzz.Vitruvius.Controls;assembly=LightBuzz.VitruviusLightBuzz.Vitruvius.Controls  5   	    HumanTransformer!   HumanTransformer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   d `Microsoft.Kinect.Wpf.Controls, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e350k(http://schemas.microsoft.com/kinect/2014  5   	   J FWpfAnimatedGif, Version=1.4.18.0, Culture=neutral, PublicKeyToken=null,gif"http://wpfanimatedgif.codeplex.com  5   	   VcontrolsGclr-namespace:LightBuzz.Vitruvius.Controls;assembly=LightBuzz.Vitruvius  5   	   -   5          � Title$  Human Transformer��5   	   $	��600��5   	   $	��700��6   .��5      0  *Microsoft.Kinect.Wpf.Controls.KinectRegion  -   5          Name$ kinectRegion��5      .��5   
   � $��	10 0 10 0q�6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5      �� $��0��6   $��Images\Logo.png��6!   $
��Left=�6:   $
��Fill=�6U   $��24��6d   $��81��6p   $��0 10 0 5q�6{   �� 5      -   5        $��0��5         ARModeButton#��[  6"   $
��Auto��6H   $��Right=�6V   $��Center=�6r   $��10 10 60 10q�6�   6�   �� 5      -   5        $��0��5        CapBgButton#��[ 6"   $
��Auto��6G   $��Right=�6U   $��Center=�6q   $��
10 10 0 10q�6�   6�   (� 5      $��1��6   $��Center=�6#   .��5      �� $
��1920��6   $
��1080��6&   .+�5      � -   5        $��CompositeImage��5      $
��1920��6-   $
��1080��6:   .+�5      �� -   5        $��BackgroundImage��5      $��Images\BackgroundLion.png��64   $��UniformToFill=�6W   �� 5      -   5        $��Camera��5      �� 5      -   5        $��GestureTutorial��5      $y�	Collapsed=�63   $��UniformToFill=�6K   �� 5      -	   5        $��MorphGif��5      $��UniformToFill=�6,   �� 5      -
   5        $��CapturedBackground��5      $��UniformToFill=�66   $y�	Collapsed=�6N   �� 5       -   5        $��TransformObject��5       $��UniformToFill=�63   $y�	Collapsed=�6K   �� 5!      -   5        $��	Animation��5!      $
��None=�6-   $��900,231,0,-231q�6<   $y�	Collapsed=�6T   5"      5#      5$      �� 5&      -   5        $��2��5&        MorphButton#��[ 6"   $��Morph��6G   $
��Auto��6W   $
��Left=�6e   $��Center=�6�   $��
10 10 0 10q�6�   6�   �� 5'      -   5        $��2��5'        ChangeSceneButton#��[ 6"   $��Scene��6M   $
��Auto��6]   $��Center=�6k   $��Center=�6�   $��
10 10 0 10q�6�   6�   �� 5(      -   5        $��2��5(        ResetButton#��[ 6"   $��Reset��6G   $
��Auto��6W   $��Right=�6e   $��Center=�6�   $��
10 10 0 10q�6�   6�   �� 5*      -   5        $��	statusBar��5*   C   $��3��6   $��Stretch=�6%   $��Bottom=�6T   
�������  MediumGreyBrush#��[ 6�   .@�5+      �� ��6    �� *
StatusText+5,      5-      5.      5/      