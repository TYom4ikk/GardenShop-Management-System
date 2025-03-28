﻿#pragma checksum "..\..\..\..\View\UsersView\CatalogPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5F572AE10D009BF57187A5724C2DE77A89A1852C2DD591F3193FC7663CFBE3DF"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using GardenKeeper.View.UsersView;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace GardenKeeper.View.UsersView {
    
    
    /// <summary>
    /// CatalogPage
    /// </summary>
    public partial class CatalogPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\..\View\UsersView\CatalogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid CatalogGrid;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\View\UsersView\CatalogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel HeaderStackPanel;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\View\UsersView\CatalogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel FiltersStackPanel;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\View\UsersView\CatalogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CategoriesFilterComboBox;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\View\UsersView\CatalogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox PriceFilterComboBox;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\View\UsersView\CatalogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.UniformGrid CatalogUniformGrid;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GardenKeeper;component/view/usersview/catalogpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\UsersView\CatalogPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.CatalogGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.HeaderStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.FiltersStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.CategoriesFilterComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 39 "..\..\..\..\View\UsersView\CatalogPage.xaml"
            this.CategoriesFilterComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CategoriesFilterComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.PriceFilterComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 46 "..\..\..\..\View\UsersView\CatalogPage.xaml"
            this.PriceFilterComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.PriceFilterComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.CatalogUniformGrid = ((System.Windows.Controls.Primitives.UniformGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

