﻿#pragma checksum "..\..\DialogForm.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "97F6D3B60A4246A9EB852A138D8F93A5"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18449
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace iphone6 {
    
    
    /// <summary>
    /// DialogForm
    /// </summary>
    public partial class DialogForm : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\DialogForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox Messagebox;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\DialogForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label SomeoneNum;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\DialogForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ReplyBox;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\DialogForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Send_message_Click;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\DialogForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Back;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\DialogForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label StatusBar;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\DialogForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox name;
        
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
            System.Uri resourceLocater = new System.Uri("/iphone6;component/dialogform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\DialogForm.xaml"
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
            this.Messagebox = ((System.Windows.Controls.ListBox)(target));
            
            #line 20 "..\..\DialogForm.xaml"
            this.Messagebox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Messagebox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SomeoneNum = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.ReplyBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.Send_message_Click = ((System.Windows.Controls.Image)(target));
            
            #line 32 "..\..\DialogForm.xaml"
            this.Send_message_Click.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.send_message_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Back = ((System.Windows.Controls.Image)(target));
            
            #line 33 "..\..\DialogForm.xaml"
            this.Back.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.back_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.StatusBar = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            
            #line 35 "..\..\DialogForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 36 "..\..\DialogForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 9:
            this.name = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

