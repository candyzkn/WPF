   M S B A M L   `   `   `  ����U  QMyWPF.CustomControlLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:MyWPF.CustomControlLibraryMyWPF.CustomControlLibrary  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   TextBoxBorder  �� EndPoint � MappingMode �� 
StartPoint � Color � Offset  TextBox_MouseOver  TextBox_Focused  TextBox_DisabledBorder �� Color  TextBox_DisabledBackground  DefaultWatermarkTemplate  WatermarkTextBox �� 
TargetType1    +MyWPF.CustomControlLibrary.WatermarkTextBox �� Property �� Value	 �� 
TargetType
 ��Name �� CornerRadius J� Opacity ��Name J� SnapsToDevicePixels ��Name    	Watermark    WatermarkTemplate J� IsHitTestVisible �� Triggers ]� 
Conditions �� Property �� Value �� 
TargetName P� Property P� Value �� BasedOn�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      2local(clr-namespace:MyWPF.CustomControlLibrary  5      %�	  &	        &	 �     &	 �    &	 m    &	 �    &	 �    &	 �    7   7  7  7  7  7  '   m	    7  �� 5      $
  0,20&�60   $ Absolute=�6@   $	 0,0&�6W   .��5   
   � $ #ABADB3��6   $
 0.05Q�6'   � 5   
   $ #E2E3EA��6   $
 0.07Q�6'   � 5   
   $ #E3E9EF��6   $ 1Q�6'   5	      �� 5      $	  0,1&�64   $	 0,0&�6C   .��5   
   � $ #5794BF��6   $
 0.05Q�6'   � 5   
   $ #B7D5EA��6   $
 0.07Q�6'   � 5   
   $ #C7E2F1��6   $ 1Q�6'   5      �� 5      $	  0,1&�62   $	 0,0&�6A   .��5   
   � $ #3D7BAD��6   $
 0.05Q�6'   � 5   
   $ #A4C9E3��6   $
 0.07Q�6'   � 5   
   $ #B7D9ED��6   $ 1Q�6'   5      �� 5      $ #ADB2B5��65   �� 5      $ #F4F4F4��69   �� 5      .R�5   
   �� 
�������|�.  ��6   �� 6?   5      �� 5      "   6%   .�5   
   ��  � ��# � ��6'   �� 5    
    � ��# � ��6'   �� 5!   
    � ��8   6(   �� 5"   
    � ��$ 1q�6,   �� 5#   
    � ��$ 3q�6$   �� 5$   
    �@J�	AllowDrop . 6&   �� 5%   
    �@�FocusVisualStyle 6-   Z� �� 5&   
    �@��PanningMode$ VerticalFirst=�65   �� 5'   
    �@��IsFlicksEnabled .  63   �� 5(   
    �@  WatermarkTemplate8  6.   �� 5)   
    � �� 5*      �� 5+      "	   6"   .R�5,      � .+�5.      �� $
 Border��6!   #��z��61   #��z��6e   $ 1��6�   #��z��6�   �� 5/      $
 MouseOverVisual��6!   $ 0Q�6:   #��z��6F   8�� $ 1��6�   �� 50      $
 FocusVisual��6!   $ 0Q�66   #��z��6B   8�� $ 1��6�   �� 51      $ PART_ContentHost��6'   # z 6A   �� 52      $ PART_WatermarkHost��6+   #��z 53   %   #��z 54   %   #��z��55   %   #��z��56   %    .  #��z��58   %   $y�	Collapsed=�59   %   5;       5<      ]� 5=       5>      �� 5?   "    �@J�	IsFocused .  6A   �� 5@   "    � ��$  ��6<   /�5B      �� 5C   "   $ PART_WatermarkHost��6?    � y�$ Visible=�6_   5E      P� 5F       �@J�IsMouseOver . . �5G      �� $ MouseOverVisual��6%    �@J�Opacity$ 1Q�6U   5H      P� 5I       �@J�	IsFocused . . �5J      �� $ FocusVisual��6%    �@J�Opacity$ 1Q�6Q   5K      P� 5L       � {� .  . �5M      �� $ Border��6%   8   � ��6Q   �� 5N      $ Border��6%   8   � ��6P   �� 5O      # � ��6;    � ��5P      5R      5T      5U      �� 5W      "   6   8   69   5Y      