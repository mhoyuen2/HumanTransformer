   M S B A M L   `   `   `  ����K  GHumanTransformer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:HumanTransformerHumanTransformer  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089d `Microsoft.Kinect.Wpf.Controls, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e354   .System.Windows.Media.Animation.ExponentialEase   VisualStateEasingFunction     Exponent  MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  MorphButton �� 
TargetType �� Orientation ��Name �� TextAlignment	 ��Name
 �� Triggers P� Property P� Value �� 
TargetName  ChangeSceneButton  ResetButton  ARModeButton  CapBgButton0  *Microsoft.Kinect.Wpf.Controls.KinectRegion ��Name4  .Microsoft.Kinect.Wpf.Controls.KinectUserViewer ��Name�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      (localclr-namespace:HumanTransformer  5      0k(http://schemas.microsoft.com/kinect/2014  5      %X  &	        &	      &	 >     &	 a     '�� �     &	 �     7  7  &	     7  7  &	 �    7  7  &	 �
    7  7  &	     7  7  '  B       5      $  7Q�68   �� 5      $ 	#ff6e6e6e��6.   �� 5	      $ 	#ff52318f��60   �� 5
      $ 	#ff00BCF2��6.   �� 5      " ��6   .�5   
   ��  �@J�SnapsToDevicePixels . 60   5      �� 5      " ��6   .�5   
   ��  � �� 5      �� 5      " ��6"   .R�5      � .+�5      �� $ 
Horizontal=�6%   
������ .+�5      �� $ 	MorphText��6(   #��z��6;   $
 Left=�6\   $��Center=�6r   8��  $��15(�6�   � 5      $��9,0,0,0q�6#   .+�5   "   �� $	 MorphNormal��6(   $��Images\MorphNormal.png��6=   $
��None=�6]   $��Center=�6l   �� 5   "   $	 
MorphHover��6(   $��Images\MorphHover.png��6<   $
��None=�6[   $��Center=�6j   $y�	Collapsed=�6�   5      5      5      
 5      P� 5       �@J�IsMouseOver . . �5      �� $ MorphNormal��6M    � y�$ 	Collapsed=�6;   6M   �� 5       $ 
MorphHover��6K    � y�$ Visible=�6;   6K   �� 5!      8  $ 	MorphText��6d    � ��5"      5$      5&      5'      �� 5)      " ��6	   .�5*   
   ��  � �� 5+      �� 5,      " ��6"   .R�5-      � .+�5.      �� $ 
Horizontal=�6%   
������ .+�5/      �� $ ChangeSceneText��6(   #��z��6A   $
 Left=�6b   $��Center=�6x   8��  $��15(�6�   � 50      $��9,0,0,0q�6#   .+�51   "   �� $	 ChangeSceneNormal��6(   $"��Images\ChangeSceneNormal.png��6C   $
��None=�6i   $��Center=�6x   �� 52   "   $	 ChangeSceneHover��6(   $!��Images\ChangeSceneHover.png��6B   $
��None=�6g   $��Center=�6v   $y�	Collapsed=�6�   53      54      55      
 56      P� 57       �@J�IsMouseOver . . �58      �� $ ChangeSceneNormal��6M    � y�$ 	Collapsed=�6;   6M   �� 59      $ ChangeSceneHover��6K    � y�$ Visible=�6;   6K   �� 5:      8  $ ChangeSceneText��6d    � ��5;      5>      5@      5A      �� 5C      " ��6	   .�5D   
   ��  � �� 5E      �� 5F      " ��6"   .R�5G      � .+�5H      �� $ 
Horizontal=�6%   
������ .+�5I      �� $ 	ResetText��6(   #��z��6;   $
 Left=�6\   $��Center=�6r   8��  $��15(�6�   � 5J      $��9,0,0,0q�6#   .+�5K   "   �� $	 ResetNormal��6(   $��Images\ResetNormal.png��6=   $
��None=�6]   $��Center=�6l   �� 5L   "   $	 
ResetHover��6(   $��Images\ResetHover.png��6<   $
��None=�6[   $��Center=�6j   $y�	Collapsed=�6�   5M      5N      5O      
 5P      P� 5Q       �@J�IsMouseOver . . �5R      �� $ ResetNormal��6M    � y�$ 	Collapsed=�6;   6M   �� 5S      $ 
ResetHover��6K    � y�$ Visible=�6;   6K   �� 5T      8  $ 	ResetText��6d    � ��5U      5X      5Z      5[      �� 5]      " ��6   .�5^   
   ��  � �� 5_      �� 5`      " ��6"   .R�5a      � .+�5b      �� $ 
Horizontal=�6%   
������ .+�5c      �� $ 
ARModeText��6(   #��z��6<   $
 Left=�6]   $��Center=�6s   8��  $��15(�6�   � 5d      $��9,0,0,0q�6#   .+�5e   "   �� $	 ARModeNormal��6(   $��Images\ARModeNormal.png��6>   $
��None=�6_   $��Center=�6n   �� 5f   "   $	 ARModeHover��6(   $��Images\ARModeHover.png��6=   $
��None=�6]   $��Center=�6l   $y�	Collapsed=�6�   5g      5h      5i      
 5j      P� 5k       �@J�IsMouseOver . . �5l      �� $ ARModeNormal��6M    � y�$ 	Collapsed=�6;   6M   �� 5m      $ ARModeHover��6K    � y�$ Visible=�6;   6K   �� 5n      8  $ 
ARModeText��6d    � ��5o      5q      5s      5t      �� 5v      " ��6   .�5w   
   ��  � �� 5x      �� 5y      " ��6"   .R�5z      � .+�5{      �� $ 
Horizontal=�6%   
������ .+�5|      �� $ 	CapBgText��6(   #��z��6;   $
 Left=�6\   $��Center=�6r   8��  $��15(�6�   � 5}      $��9,0,0,0q�6#   .+�5~   "   �� $	 CapBgNormal��6(   $��Images\CapBgNormal.png��6=   $
��None=�6]   $��Center=�6l   �� 5   "   $	 
CapBgHover��6(   $��Images\CapBgHover.png��6<   $
��None=�6[   $��Center=�6j   $y�	Collapsed=�6�   5�      5�      5�      
 5�      P� 5�       �@J�IsMouseOver . . �5�      �� $ CapBgNormal��6M    � y�$ 	Collapsed=�6;   6M   �� 5�      $ 
CapBgHover��6K    � y�$ Visible=�6;   6K   �� 5�      8  $ 	CapBgText��6d    � ��5�      5�      5�      5�      �� 5�      "  6   .�5�   
   ��  � �� 5�      �� 5�      "  6"   .R�5�      �� #��z��6   #��z��5�      #��z��5�      ._�5�      � .+�5�      �� $ contentHolder��6/     5�      $��73��61   $��Center=�5�   !   $	��Top=�5�   !   �� 5�      $ cursorRenderingLayer��6%   ��5�   !   Z� 5�      5�      5�      5�      5�      5�      