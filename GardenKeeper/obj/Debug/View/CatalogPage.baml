   M S B A M L   `   `   `  ����G  CGardenKeeper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:GardenKeeper.View.UsersViewGardenKeeper.View.UsersView  5      -    'GardenKeeper.View.UsersView.CatalogPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:GardenKeeper.View.UsersView     5        N� Title$  Catalog��5	       � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $( "../Assets/Styles/CatalogStyle.xaml?�6%   %    5       N� 
Background 5      �� 5   
    �� ImageSource$9 3pack://application:,,,/Assets/Images/background.png��6   .��5      � -   5         �Name$ CatalogGrid��5       � RowDefinitions 5   
   �� 5      $	��100 �6   �� 5      $��* �6   .+�5   
   � -   5        $ 
HeaderGrid��5         
HeaderGrid#��[  6#    � ColumnDefinitions 5      �� 5       $��3* �6#   �� 5!      $��* �6#   �� 5"      $��* �6#   .+�5%      �� -   5        	 ��Name$	 	LogoImage��5%      $��0��6'   $)��#/Assets/Images/GardenKeeperLogo.png��67     	LogoImage#��[ 6d   5&      �� 5'      -   5        
 ��Name$
 LoginButton��5'      $��1��6*     LoginButton#��[ 6:   5(      �� 5)      -   5        $
 ShoppingCardButton��5)      $��4��61   $��Корзина��6A   $��40��5*      $	��100��6!     ShoppingCardButton#��[ 5+      5,      � 5.   
   -   5        $ CatalogContent��5.      $��1��6    50      �� 51      $��5* �6#   �� 52      $
��Auto �6#   �� 53      $��* �6#   $	��300��6-   $	��150��6<   .+�56      � -   5        $ ProductsContentContainer��56   #   $��0��6    58      �� 59      $��50 �6$   �� 5:      .+�5=      �� -   5         ��Name$ FiltersStackPanel��5=      $��0��68    �� Orientation$ 
Horizontal=�6E   .+�5?      �� -	   5         ��Name$ CategoriesFilterComboBox��5?        CategoriesFilterComboBox#��[ 6A   $��Center=�5A      $��Center=�5B       �� 
IsEditable . 5C      �� 5F      -
   5        $ PriceFilterComboBox��5F        PriceFilterComboBox#��[ 6<   $��Center=�5G      $��Center=�5H   !   5I      5J      �� 5L      $
��Auto=�6   $��1��6B   .��5M      A� -   5         A�Name$ CatalogUniformGrid��5M   .    A� Columns$ 3��6"   5N      5O      �� 5Q      -   5        $
 AddProductButton��5Q      $��+��5R      $��40��5S      $��40��5T      $��24(�5U      ���	#FF4CAF50
�������$��0q�5X      $��1��5Z      $��Right=�5[      $��Bottom=�5\      $��	0,0,10,10q�5]       K� ZIndex$ 1��5^      5_      5`      �� 5b      -   5         ��Name$ SpecialPanelSplitter��5c   F   $��1��5b       �� ShowsPreview .  $��3��6@   $��Center=�5c      $��Stretch=�6*   6F   � 5e      -   5        $ SpecialPanel��5f      $��2��5e        
AdminPanel#��[ 6#    5g      �� 5h      �� 5i      �� 5j      .+�5m      �� -   5        $"
 GenerateAuditLogReportButton��5m   &   $��0��6   $��40��5n      $	��120��6%     AdminPanelButton#��[ 61     AdminPanelButtonText#��[ 6[   $!��Журнал событий��5o      �� 5p      -   5        $
 AddNewManagerButton��5p   &   $��1��6   $��40��6C   $	��126��6O   $)��#Добавить Менеджера��5q      #��[ 6X   #��[ 5r      �� 5t      -   5        $
 GenerateSalesReportButton��5t   &   $��2��6   $��40��6I   $	��126��6U   $'��!Статистика продаж��5u      #��[ 6]   #��[ 5v      5x      5z      5|      5}      