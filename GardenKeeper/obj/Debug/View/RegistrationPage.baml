   M S B A M L   `   `   `  ����G  CGardenKeeper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:GardenKeeper.ViewGardenKeeper.View  5      (    "GardenKeeper.View.RegistrationPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:GardenKeeper.View     5        N� Title$  RegistrationPage��5	       � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $$ /Assets/Colors/MainColors.xaml?�6%   %    5       N� 
Background 5      �� 5   
    �� ImageSource$9 3pack://application:,,,/Assets/Images/background.png��6   .��5      � .+�5   
   �� $��Center=�6   $��Center=�60   .+�5      �� $��Garden Store��6   $��26(�6+   $
��Bold"�69   $��Center=�6K   $��Arial)�6h   $��0 0 0 30q�6{   �� 5      $��Логин:��6   $	��200��6%   $��30��61   �� 5      -   5         ��Name$ TextBoxLogin��5      $	��200��6,   $��30��68   �� 5      $��Пароль:��6   $	��200��6&   $��30��62   G� 5      -   5         G�Name$ PasswordBoxPassword��5      $	��200��67   $��30��6C   �� 5       -   5         ��Name$ ButtonSubmit��5       $��
Войти��6+   $	��200��6;   $��30��6G   $��20q�6S   5!      �� 5"      -   5        	 ��Name$	 ForgetPassword��5"      $ ��Забыли пароль?��6,   $��Center=�6E   
����  �
 � Cursor$

 Hand��5#      �� 5$      -   5        $	 RegistrationButton��5$      $*��$Зарегестрироваться��60   $��Center=�6M   
����  �$

 Hand��5%      �� 5&      -   5        $!	 ContinueWithoutRegistration��5&      $,��&Продолжить без входа��69   $��Center=�6X   
����  �$

 Hand��5'      5(      5)      5*      