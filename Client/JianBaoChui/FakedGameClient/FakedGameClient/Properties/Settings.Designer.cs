﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18052
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FakedGameClient.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.16.223")]
        public string Ip {
            get {
                return ((string)(this["Ip"]));
            }
            set {
                this["Ip"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8888")]
        public int Port {
            get {
                return ((int)(this["Port"]));
            }
            set {
                this["Port"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}^{1}^{2}^0001^0")]
        public string LoginCmd {
            get {
                return ((string)(this["LoginCmd"]));
            }
            set {
                this["LoginCmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}^{1}^{2}^0003^2")]
        public string BaoCmd {
            get {
                return ((string)(this["BaoCmd"]));
            }
            set {
                this["BaoCmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}^{1}^{2}^0002^00")]
        public string SignCmd {
            get {
                return ((string)(this["SignCmd"]));
            }
            set {
                this["SignCmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}^{1}^{2}^0003^1")]
        public string JianCmd {
            get {
                return ((string)(this["JianCmd"]));
            }
            set {
                this["JianCmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}^{1}^{2}^0003^3")]
        public string ChuiCmd {
            get {
                return ((string)(this["ChuiCmd"]));
            }
            set {
                this["ChuiCmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\r\\n")]
        public string EOF {
            get {
                return ((string)(this["EOF"]));
            }
            set {
                this["EOF"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}^{1}^{2}^0006^0")]
        public string WatchingCmd {
            get {
                return ((string)(this["WatchingCmd"]));
            }
            set {
                this["WatchingCmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}^{1}^{2}^0000^0")]
        public string QuitGameCmd {
            get {
                return ((string)(this["QuitGameCmd"]));
            }
            set {
                this["QuitGameCmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}^{1}^{2}^0004^{3}")]
        public string BetCmd {
            get {
                return ((string)(this["BetCmd"]));
            }
            set {
                this["BetCmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int WaitingTime {
            get {
                return ((int)(this["WaitingTime"]));
            }
            set {
                this["WaitingTime"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("yang")]
        public string AuthCode {
            get {
                return ((string)(this["AuthCode"]));
            }
            set {
                this["AuthCode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0223")]
        public string ID {
            get {
                return ((string)(this["ID"]));
            }
            set {
                this["ID"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("|")]
        public string ResponseSeparator {
            get {
                return ((string)(this["ResponseSeparator"]));
            }
            set {
                this["ResponseSeparator"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("^")]
        public string DataSeparator {
            get {
                return ((string)(this["DataSeparator"]));
            }
            set {
                this["DataSeparator"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("@P@")]
        public string PlayerSeparator {
            get {
                return ((string)(this["PlayerSeparator"]));
            }
            set {
                this["PlayerSeparator"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowSocketLog {
            get {
                return ((bool)(this["ShowSocketLog"]));
            }
            set {
                this["ShowSocketLog"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}^{1}^{2}^0010^0")]
        public string SpecialCmd {
            get {
                return ((string)(this["SpecialCmd"]));
            }
            set {
                this["SpecialCmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8")]
        public int ThreadCount {
            get {
                return ((int)(this["ThreadCount"]));
            }
            set {
                this["ThreadCount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoGame {
            get {
                return ((bool)(this["AutoGame"]));
            }
            set {
                this["AutoGame"] = value;
            }
        }
    }
}
