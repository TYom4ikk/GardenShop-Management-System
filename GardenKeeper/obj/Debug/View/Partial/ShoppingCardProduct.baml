   M S B A M L   `   `   `  ����G  CGardenKeeper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:GardenKeeper.View.PartialGardenKeeper.View.Partial  5      3    -GardenKeeper.View.Partial.ShoppingCardProduct   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:GardenKeeper.View.Partial     5        � 	Resources  5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $+ %/Assets/Styles/ShoppingCardStyle.xaml?�6%   %    5      .��5      � -   5         �Name$ ShoppingCardGrid��5         ShoppingCardGrid#��[  6%    � ColumnDefinitions 5   
   �� 5      $
��0.5* �6   �� 5      $��* �6   �� 5      $
��0.4* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $
��0.5* �6   .+�5   
   �� -   5         ��Name$ ProductImage��5      $��Center=�6C   ��6&   �� *	MainImage+6C   �� 5   
   -   5         ��Name$ NameTextBlock��5   $   $��1��6   $��Center=�6Q   ��6;   �� *Name+6Q   �� 5   
   -   5        $ PriceTextBlock��5   $   $��2��6   $��Center=�6<   �� 5   
   -   5        $ QuantityTextBlock��5   $   $��3��6   $��Количество:��6?   $��Center=�6R   �� 5!   
   $��4��6    �� Orientation$ 
Horizontal=�6%   $��Center=�6>   $��Center=�6[   .+�5"      �� -   5         ��Name$ DecreaseButton��5"      $��-��6-   $��20��69   $��20��6D   6P   �� 5#      -   5        	 ��Name$	 QuantityTextBox��5#      $��1��6/   $��30��68   
 �� TextAlignment$
 Center=�6C   $	��5,0q�6Z   �� 5$      -   5        $ IncreaseButton��5$      $��+��6-   $��20��69   $��20��6D   6P   5%      �� 5'   
   -	   5        $ DeleteButton��5'   -   $��5��6   $��X��6!     DeleteButton#��[ 6C   $��Center=�6i   5(      5)      5*      