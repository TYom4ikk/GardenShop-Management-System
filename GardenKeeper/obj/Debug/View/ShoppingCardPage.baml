   M S B A M L   `   `   `  ����G  CGardenKeeper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:GardenKeeper.ViewGardenKeeper.View  5      (    "GardenKeeper.View.ShoppingCardPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:GardenKeeper.View     5        N� Title$  ShoppingCardPage��5	       � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $+ %/Assets/Styles/ShoppingCardStyle.xaml?�6%   �� 5      $&  /Assets/Styles/CatalogStyle.xaml?�6%   %    5       N� 
Background 5      �� 5   
    �� ImageSource$9 3pack://application:,,,/Assets/Images/background.png��6   .��5      �  � RowDefinitions 5   
   �� 5      $	��100 �6   �� 5      .+�5   
   � -   5         �Name$ 
HeaderGrid��5         
HeaderGrid#��[  6#    � ColumnDefinitions 5       �� 5!      $��3* �6#   �� 5"      $��* �6#   �� 5#      $��* �6#   .+�5&      �� -   5        	 ��Name$	 	LogoImage��5&      $��0��6'   $)��#/Assets/Images/GardenKeeperLogo.png��67     	LogoImage#��[ 6d   6�   �� 5'      -   5        
 ��Name$
 LoginButton��5'      $��1��6*     LoginButton#��[ 6:   5(      �� 5)      -   5        $
 ShoppingCardButton��5)      $��3��61   $��Корзина��6A   $��40��5*      $	��100��6     ShoppingCardButton#��[ 5+      5,      � 5.   
   $��1��6    50      �� 51      $��3* �6#   �� 52      .+�54      �� -   5         ��Name$ ProductListStackPanel��54      56      �� 57      $��1��6   .+�58      �� -   5        $
 	BuyButton��58      $��50��6,   $��Купить��68   
���2�2�6a   59      5:      5;      5<      