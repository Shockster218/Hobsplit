﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HobbitAutosplitter {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int cropLeft {
            get {
                return ((int)(this["cropLeft"]));
            }
            set {
                this["cropLeft"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int cropTop {
            get {
                return ((int)(this["cropTop"]));
            }
            set {
                this["cropTop"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int cropRight {
            get {
                return ((int)(this["cropRight"]));
            }
            set {
                this["cropRight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int cropBottom {
            get {
                return ((int)(this["cropBottom"]));
            }
            set {
                this["cropBottom"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("NONAME")]
        public global::WindowsInput.Native.VirtualKeyCode split {
            get {
                return ((global::WindowsInput.Native.VirtualKeyCode)(this["split"]));
            }
            set {
                this["split"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("NONAME")]
        public global::WindowsInput.Native.VirtualKeyCode unsplit {
            get {
                return ((global::WindowsInput.Native.VirtualKeyCode)(this["unsplit"]));
            }
            set {
                this["unsplit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("NONAME")]
        public global::WindowsInput.Native.VirtualKeyCode reset {
            get {
                return ((global::WindowsInput.Native.VirtualKeyCode)(this["reset"]));
            }
            set {
                this["reset"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("NONAME")]
        public global::WindowsInput.Native.VirtualKeyCode pause {
            get {
                return ((global::WindowsInput.Native.VirtualKeyCode)(this["pause"]));
            }
            set {
                this["pause"] = value;
            }
        }
    }
}
