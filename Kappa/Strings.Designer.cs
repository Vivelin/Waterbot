﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kappa {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Kappa.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A chat message should always contain at least two parameters..
        /// </summary>
        internal static string Arg_ChatMessageParamCount {
            get {
                return ResourceManager.GetString("Arg_ChatMessageParamCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connected to {0}.
        /// </summary>
        internal static string Connected {
            get {
                return ResourceManager.GetString("Connected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unhandled error occurred in IrcDotNet.
        /// </summary>
        internal static string IrcDotNetError {
            get {
                return ResourceManager.GetString("IrcDotNetError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}.
        /// </summary>
        internal static string NoticeReceived {
            get {
                return ResourceManager.GetString("NoticeReceived", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cave Johnson, we&apos;re done here..
        /// </summary>
        internal static string QuitMessage {
            get {
                return ResourceManager.GetString("QuitMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}.
        /// </summary>
        internal static string RawMessageReceived {
            get {
                return ResourceManager.GetString("RawMessageReceived", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} playing {1}.
        /// </summary>
        internal static string Stream_String {
            get {
                return ResourceManager.GetString("Stream_String", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}.
        /// </summary>
        internal static string UnhandledMessageReceived {
            get {
                return ResourceManager.GetString("UnhandledMessageReceived", resourceCulture);
            }
        }
    }
}
