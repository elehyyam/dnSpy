/*
    Copyright (C) 2014-2019 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.IO;
using dnSpy.Contracts.Utilities;

namespace dnSpy.Search {
	static class FrameworkFileUtils {
		static readonly string[] frameworkAssemblyNamePrefixes = new string[] {
			"Unity.",
			"UnityEngine.",
			"UnityEditor.",
		};

		static readonly HashSet<string> frameworkAssemblyNames = new HashSet<string>(StringComparer.Ordinal) {
			// .NET Core
			"Accessibility",
			"DirectWriteForwarder",
			"Microsoft.AI.DependencyCollector",
			"Microsoft.ApplicationInsights",
			"Microsoft.ApplicationInsights.AspNetCore",
			"Microsoft.AspNetCore",
			"Microsoft.AspNetCore.Antiforgery",
			"Microsoft.AspNetCore.ApplicationInsights.HostingStartup",
			"Microsoft.AspNetCore.Authentication",
			"Microsoft.AspNetCore.Authentication.Abstractions",
			"Microsoft.AspNetCore.Authentication.Cookies",
			"Microsoft.AspNetCore.Authentication.Core",
			"Microsoft.AspNetCore.Authentication.Facebook",
			"Microsoft.AspNetCore.Authentication.Google",
			"Microsoft.AspNetCore.Authentication.JwtBearer",
			"Microsoft.AspNetCore.Authentication.MicrosoftAccount",
			"Microsoft.AspNetCore.Authentication.OAuth",
			"Microsoft.AspNetCore.Authentication.OpenIdConnect",
			"Microsoft.AspNetCore.Authentication.Twitter",
			"Microsoft.AspNetCore.Authentication.WsFederation",
			"Microsoft.AspNetCore.Authorization",
			"Microsoft.AspNetCore.Authorization.Policy",
			"Microsoft.AspNetCore.AzureAppServices.HostingStartup",
			"Microsoft.AspNetCore.AzureAppServicesIntegration",
			"Microsoft.AspNetCore.Components",
			"Microsoft.AspNetCore.Components.Browser",
			"Microsoft.AspNetCore.Components.Server",
			"Microsoft.AspNetCore.Connections.Abstractions",
			"Microsoft.AspNetCore.CookiePolicy",
			"Microsoft.AspNetCore.Cors",
			"Microsoft.AspNetCore.Cryptography.Internal",
			"Microsoft.AspNetCore.Cryptography.KeyDerivation",
			"Microsoft.AspNetCore.DataProtection",
			"Microsoft.AspNetCore.DataProtection.Abstractions",
			"Microsoft.AspNetCore.DataProtection.AzureKeyVault",
			"Microsoft.AspNetCore.DataProtection.AzureStorage",
			"Microsoft.AspNetCore.DataProtection.Extensions",
			"Microsoft.AspNetCore.Diagnostics",
			"Microsoft.AspNetCore.Diagnostics.Abstractions",
			"Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore",
			"Microsoft.AspNetCore.Diagnostics.HealthChecks",
			"Microsoft.AspNetCore.HostFiltering",
			"Microsoft.AspNetCore.Hosting",
			"Microsoft.AspNetCore.Hosting.Abstractions",
			"Microsoft.AspNetCore.Hosting.Server.Abstractions",
			"Microsoft.AspNetCore.Html.Abstractions",
			"Microsoft.AspNetCore.Http",
			"Microsoft.AspNetCore.Http.Abstractions",
			"Microsoft.AspNetCore.Http.Connections",
			"Microsoft.AspNetCore.Http.Connections.Common",
			"Microsoft.AspNetCore.Http.Extensions",
			"Microsoft.AspNetCore.Http.Features",
			"Microsoft.AspNetCore.HttpOverrides",
			"Microsoft.AspNetCore.HttpsPolicy",
			"Microsoft.AspNetCore.Identity",
			"Microsoft.AspNetCore.Identity.EntityFrameworkCore",
			"Microsoft.AspNetCore.Identity.UI",
			"Microsoft.AspNetCore.Identity.UI.Views",
			"Microsoft.AspNetCore.Identity.UI.Views.V3",
			"Microsoft.AspNetCore.Identity.UI.Views.V4",
			"Microsoft.AspNetCore.JsonPatch",
			"Microsoft.AspNetCore.Localization",
			"Microsoft.AspNetCore.Localization.Routing",
			"Microsoft.AspNetCore.Metadata",
			"Microsoft.AspNetCore.MiddlewareAnalysis",
			"Microsoft.AspNetCore.Mvc",
			"Microsoft.AspNetCore.Mvc.Abstractions",
			"Microsoft.AspNetCore.Mvc.ApiExplorer",
			"Microsoft.AspNetCore.Mvc.Components.Prerendering",
			"Microsoft.AspNetCore.Mvc.Core",
			"Microsoft.AspNetCore.Mvc.Cors",
			"Microsoft.AspNetCore.Mvc.DataAnnotations",
			"Microsoft.AspNetCore.Mvc.Formatters.Json",
			"Microsoft.AspNetCore.Mvc.Formatters.Xml",
			"Microsoft.AspNetCore.Mvc.Localization",
			"Microsoft.AspNetCore.Mvc.Razor",
			"Microsoft.AspNetCore.Mvc.Razor.Extensions",
			"Microsoft.AspNetCore.Mvc.RazorPages",
			"Microsoft.AspNetCore.Mvc.TagHelpers",
			"Microsoft.AspNetCore.Mvc.ViewFeatures",
			"Microsoft.AspNetCore.NodeServices",
			"Microsoft.AspNetCore.Owin",
			"Microsoft.AspNetCore.Razor",
			"Microsoft.AspNetCore.Razor.Language",
			"Microsoft.AspNetCore.Razor.Runtime",
			"Microsoft.AspNetCore.ResponseCaching",
			"Microsoft.AspNetCore.ResponseCaching.Abstractions",
			"Microsoft.AspNetCore.ResponseCompression",
			"Microsoft.AspNetCore.Rewrite",
			"Microsoft.AspNetCore.Routing",
			"Microsoft.AspNetCore.Routing.Abstractions",
			"Microsoft.AspNetCore.Server.HttpSys",
			"Microsoft.AspNetCore.Server.IIS",
			"Microsoft.AspNetCore.Server.IISIntegration",
			"Microsoft.AspNetCore.Server.Kestrel",
			"Microsoft.AspNetCore.Server.Kestrel.Core",
			"Microsoft.AspNetCore.Server.Kestrel.Https",
			"Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions",
			"Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv",
			"Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets",
			"Microsoft.AspNetCore.Session",
			"Microsoft.AspNetCore.SignalR",
			"Microsoft.AspNetCore.SignalR.Common",
			"Microsoft.AspNetCore.SignalR.Core",
			"Microsoft.AspNetCore.SignalR.Protocols.Json",
			"Microsoft.AspNetCore.SignalR.Redis",
			"Microsoft.AspNetCore.SpaServices",
			"Microsoft.AspNetCore.SpaServices.Extensions",
			"Microsoft.AspNetCore.StaticFiles",
			"Microsoft.AspNetCore.WebSockets",
			"Microsoft.AspNetCore.WebUtilities",
			"Microsoft.Azure.KeyVault",
			"Microsoft.Azure.KeyVault.WebKey",
			"Microsoft.Azure.Services.AppAuthentication",
			"Microsoft.CodeAnalysis",
			"Microsoft.CodeAnalysis.CSharp",
			"Microsoft.CodeAnalysis.Razor",
			"Microsoft.CodeAnalysis.VisualBasic",
			"Microsoft.CSharp",
			"Microsoft.Data.Edm",
			"Microsoft.Data.OData",
			"Microsoft.Data.Sqlite",
			"Microsoft.DotNet.PlatformAbstractions",
			"Microsoft.EntityFrameworkCore",
			"Microsoft.EntityFrameworkCore.Abstractions",
			"Microsoft.EntityFrameworkCore.Design",
			"Microsoft.EntityFrameworkCore.InMemory",
			"Microsoft.EntityFrameworkCore.Relational",
			"Microsoft.EntityFrameworkCore.Sqlite",
			"Microsoft.EntityFrameworkCore.SqlServer",
			"Microsoft.Extensions.Caching.Abstractions",
			"Microsoft.Extensions.Caching.Memory",
			"Microsoft.Extensions.Caching.Redis",
			"Microsoft.Extensions.Caching.SqlServer",
			"Microsoft.Extensions.Configuration",
			"Microsoft.Extensions.Configuration.Abstractions",
			"Microsoft.Extensions.Configuration.AzureKeyVault",
			"Microsoft.Extensions.Configuration.Binder",
			"Microsoft.Extensions.Configuration.CommandLine",
			"Microsoft.Extensions.Configuration.EnvironmentVariables",
			"Microsoft.Extensions.Configuration.FileExtensions",
			"Microsoft.Extensions.Configuration.Ini",
			"Microsoft.Extensions.Configuration.Json",
			"Microsoft.Extensions.Configuration.KeyPerFile",
			"Microsoft.Extensions.Configuration.UserSecrets",
			"Microsoft.Extensions.Configuration.Xml",
			"Microsoft.Extensions.DependencyInjection",
			"Microsoft.Extensions.DependencyInjection.Abstractions",
			"Microsoft.Extensions.DependencyModel",
			"Microsoft.Extensions.DiagnosticAdapter",
			"Microsoft.Extensions.Diagnostics.HealthChecks",
			"Microsoft.Extensions.Diagnostics.HealthChecks.Abstractions",
			"Microsoft.Extensions.FileProviders.Abstractions",
			"Microsoft.Extensions.FileProviders.Composite",
			"Microsoft.Extensions.FileProviders.Embedded",
			"Microsoft.Extensions.FileProviders.Physical",
			"Microsoft.Extensions.FileSystemGlobbing",
			"Microsoft.Extensions.Hosting",
			"Microsoft.Extensions.Hosting.Abstractions",
			"Microsoft.Extensions.Http",
			"Microsoft.Extensions.Identity.Core",
			"Microsoft.Extensions.Identity.Stores",
			"Microsoft.Extensions.Localization",
			"Microsoft.Extensions.Localization.Abstractions",
			"Microsoft.Extensions.Logging",
			"Microsoft.Extensions.Logging.Abstractions",
			"Microsoft.Extensions.Logging.AzureAppServices",
			"Microsoft.Extensions.Logging.Configuration",
			"Microsoft.Extensions.Logging.Console",
			"Microsoft.Extensions.Logging.Debug",
			"Microsoft.Extensions.Logging.EventLog",
			"Microsoft.Extensions.Logging.EventSource",
			"Microsoft.Extensions.Logging.TraceSource",
			"Microsoft.Extensions.ObjectPool",
			"Microsoft.Extensions.Options",
			"Microsoft.Extensions.Options.ConfigurationExtensions",
			"Microsoft.Extensions.Options.DataAnnotations",
			"Microsoft.Extensions.PlatformAbstractions",
			"Microsoft.Extensions.Primitives",
			"Microsoft.Extensions.WebEncoders",
			"Microsoft.IdentityModel.Clients.ActiveDirectory",
			"Microsoft.IdentityModel.Clients.ActiveDirectory.Platform",
			"Microsoft.IdentityModel.JsonWebTokens",
			"Microsoft.IdentityModel.Logging",
			"Microsoft.IdentityModel.Protocols",
			"Microsoft.IdentityModel.Protocols.OpenIdConnect",
			"Microsoft.IdentityModel.Protocols.WsFederation",
			"Microsoft.IdentityModel.Tokens",
			"Microsoft.IdentityModel.Tokens.Saml",
			"Microsoft.IdentityModel.Xml",
			"Microsoft.JSInterop",
			"Microsoft.Net.Http.Headers",
			"Microsoft.Rest.ClientRuntime",
			"Microsoft.Rest.ClientRuntime.Azure",
			"Microsoft.VisualBasic",
			"Microsoft.VisualBasic.Core",
			"Microsoft.VisualStudio.Web.BrowserLink",
			"Microsoft.Win32.Primitives",
			"Microsoft.Win32.Registry",
			"Microsoft.Win32.SystemEvents",
			"Microsoft.WindowsAzure.Storage",
			"mscorlib",
			"netstandard",
			"PresentationCore",
			"PresentationCore-CommonResources",
			"PresentationFramework",
			"PresentationFramework.Aero",
			"PresentationFramework.Aero2",
			"PresentationFramework.AeroLite",
			"PresentationFramework.Classic",
			"PresentationFramework.Luna",
			"PresentationFramework.Royale",
			"PresentationFramework-SystemCore",
			"PresentationFramework-SystemData",
			"PresentationFramework-SystemDrawing",
			"PresentationFramework-SystemXml",
			"PresentationFramework-SystemXmlLinq",
			"PresentationUI",
			"ReachFramework",
			"SOS.NETCore",
			"System",
			"System.AppContext",
			"System.Buffers",
			"System.CodeDom",
			"System.Collections",
			"System.Collections.Concurrent",
			"System.Collections.Immutable",
			"System.Collections.NonGeneric",
			"System.Collections.Specialized",
			"System.ComponentModel",
			"System.ComponentModel.Annotations",
			"System.ComponentModel.Composition",
			"System.ComponentModel.DataAnnotations",
			"System.ComponentModel.EventBasedAsync",
			"System.ComponentModel.Primitives",
			"System.ComponentModel.TypeConverter",
			"System.Configuration",
			"System.Configuration.ConfigurationManager",
			"System.Console",
			"System.Core",
			"System.Data",
			"System.Data.Common",
			"System.Data.DataSetExtensions",
			"System.Data.SqlClient",
			"System.Design",
			"System.Diagnostics.Contracts",
			"System.Diagnostics.Debug",
			"System.Diagnostics.DiagnosticSource",
			"System.Diagnostics.EventLog",
			"System.Diagnostics.FileVersionInfo",
			"System.Diagnostics.Process",
			"System.Diagnostics.StackTrace",
			"System.Diagnostics.TextWriterTraceListener",
			"System.Diagnostics.Tools",
			"System.Diagnostics.TraceSource",
			"System.Diagnostics.Tracing",
			"System.DirectoryServices",
			"System.Drawing",
			"System.Drawing.Common",
			"System.Drawing.Design",
			"System.Drawing.Primitives",
			"System.Dynamic.Runtime",
			"System.Globalization",
			"System.Globalization.Calendars",
			"System.Globalization.Extensions",
			"System.IdentityModel.Tokens.Jwt",
			"System.Interactive.Async",
			"System.IO",
			"System.IO.Compression",
			"System.IO.Compression.Brotli",
			"System.IO.Compression.FileSystem",
			"System.IO.Compression.ZipFile",
			"System.IO.FileSystem",
			"System.IO.FileSystem.AccessControl",
			"System.IO.FileSystem.DriveInfo",
			"System.IO.FileSystem.Primitives",
			"System.IO.FileSystem.Watcher",
			"System.IO.IsolatedStorage",
			"System.IO.MemoryMappedFiles",
			"System.IO.Packaging",
			"System.IO.Pipelines",
			"System.IO.Pipes",
			"System.IO.Pipes.AccessControl",
			"System.IO.UnmanagedMemoryStream",
			"System.Linq",
			"System.Linq.Expressions",
			"System.Linq.Parallel",
			"System.Linq.Queryable",
			"System.Memory",
			"System.Net",
			"System.Net.Http",
			"System.Net.Http.Formatting",
			"System.Net.HttpListener",
			"System.Net.Mail",
			"System.Net.NameResolution",
			"System.Net.NetworkInformation",
			"System.Net.Ping",
			"System.Net.Primitives",
			"System.Net.Requests",
			"System.Net.Security",
			"System.Net.ServicePoint",
			"System.Net.Sockets",
			"System.Net.WebClient",
			"System.Net.WebHeaderCollection",
			"System.Net.WebProxy",
			"System.Net.WebSockets",
			"System.Net.WebSockets.Client",
			"System.Net.WebSockets.WebSocketProtocol",
			"System.Numerics",
			"System.Numerics.Vectors",
			"System.ObjectModel",
			"System.Printing",
			"System.Private.CoreLib",
			"System.Private.DataContractSerialization",
			"System.Private.Uri",
			"System.Private.Xml",
			"System.Private.Xml.Linq",
			"System.Reflection",
			"System.Reflection.DispatchProxy",
			"System.Reflection.Emit",
			"System.Reflection.Emit.ILGeneration",
			"System.Reflection.Emit.Lightweight",
			"System.Reflection.Extensions",
			"System.Reflection.Metadata",
			"System.Reflection.Primitives",
			"System.Reflection.TypeExtensions",
			"System.Resources.Extensions",
			"System.Resources.Reader",
			"System.Resources.ResourceManager",
			"System.Resources.Writer",
			"System.Runtime",
			"System.Runtime.CompilerServices.Unsafe",
			"System.Runtime.CompilerServices.VisualC",
			"System.Runtime.Extensions",
			"System.Runtime.Handles",
			"System.Runtime.InteropServices",
			"System.Runtime.InteropServices.RuntimeInformation",
			"System.Runtime.InteropServices.WindowsRuntime",
			"System.Runtime.Intrinsics",
			"System.Runtime.Loader",
			"System.Runtime.Numerics",
			"System.Runtime.Serialization",
			"System.Runtime.Serialization.Formatters",
			"System.Runtime.Serialization.Json",
			"System.Runtime.Serialization.Primitives",
			"System.Runtime.Serialization.Xml",
			"System.Runtime.WindowsRuntime",
			"System.Runtime.WindowsRuntime.UI.Xaml",
			"System.Security",
			"System.Security.AccessControl",
			"System.Security.Claims",
			"System.Security.Cryptography.Algorithms",
			"System.Security.Cryptography.Cng",
			"System.Security.Cryptography.Csp",
			"System.Security.Cryptography.Encoding",
			"System.Security.Cryptography.OpenSsl",
			"System.Security.Cryptography.Pkcs",
			"System.Security.Cryptography.Primitives",
			"System.Security.Cryptography.ProtectedData",
			"System.Security.Cryptography.X509Certificates",
			"System.Security.Cryptography.Xml",
			"System.Security.Permissions",
			"System.Security.Principal",
			"System.Security.Principal.Windows",
			"System.Security.SecureString",
			"System.ServiceModel.Web",
			"System.ServiceProcess",
			"System.Spatial",
			"System.Text.Encoding",
			"System.Text.Encoding.CodePages",
			"System.Text.Encoding.Extensions",
			"System.Text.Encodings.Web",
			"System.Text.Json",
			"System.Text.RegularExpressions",
			"System.Threading",
			"System.Threading.AccessControl",
			"System.Threading.Channels",
			"System.Threading.Overlapped",
			"System.Threading.Tasks",
			"System.Threading.Tasks.Dataflow",
			"System.Threading.Tasks.Extensions",
			"System.Threading.Tasks.Parallel",
			"System.Threading.Thread",
			"System.Threading.ThreadPool",
			"System.Threading.Timer",
			"System.Transactions",
			"System.Transactions.Local",
			"System.ValueTuple",
			"System.Web",
			"System.Web.HttpUtility",
			"System.Windows",
			"System.Windows.Controls.Ribbon",
			"System.Windows.Extensions",
			"System.Windows.Forms",
			"System.Windows.Forms.Design",
			"System.Windows.Forms.Design.Editors",
			"System.Windows.Input.Manipulations",
			"System.Windows.Presentation",
			"System.Xaml",
			"System.Xml",
			"System.Xml.Linq",
			"System.Xml.ReaderWriter",
			"System.Xml.Serialization",
			"System.Xml.XDocument",
			"System.Xml.XmlDocument",
			"System.Xml.XmlSerializer",
			"System.Xml.XPath",
			"System.Xml.XPath.XDocument",
			"UIAutomationClient",
			"UIAutomationClientSideProviders",
			"UIAutomationProvider",
			"UIAutomationTypes",
			"WindowsBase",
			"WindowsFormsIntegration",

			// Unity
			"Unity",
			"UnityEngine",
			"UnityEditor",
			"Mono.Security",
			"System.EnterpriseServices",
		};

		public static bool IsFrameworkAssembly(string filename, string? assemblySimpleName) {
			// Check if it's in one of the .NET Core runtime dirs
			if (Directory.Exists(Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(filename))), "Microsoft.NETCore.App")))
				return true;

			if (!(assemblySimpleName is null)) {
				if (frameworkAssemblyNames.Contains(assemblySimpleName))
					return true;
				foreach (var prefix in frameworkAssemblyNamePrefixes) {
					if (assemblySimpleName.StartsWith(prefix, StringComparison.Ordinal))
						return true;
				}
			}

			// .NET Framework
			if (GacInfo.IsGacPath(filename))
				return true;

			return false;
		}
	}
}
