﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TheHorses.Scraper {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class ScraperSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static ScraperSettings defaultInstance = ((ScraperSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new ScraperSettings())));
        
        public static ScraperSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".\\DatabaseCredentials.xml")]
        public string dbCredFile {
            get {
                return ((string)(this["dbCredFile"]));
            }
        }
    }
}
