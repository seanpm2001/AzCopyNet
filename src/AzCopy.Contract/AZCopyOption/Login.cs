﻿
// <auto-generated />
namespace AzCopy.Contract
{
    public class LoginOption : CommandArgsBase
    {
        /// <summary>
		/// The Azure Active Directory endpoint to use. The default (https://login.microsoftonline.com) is correct for the public Azure cloud. Set this parameter when authenticating in a national cloud. Not needed for Managed Service Identity
        /// </summary>
		[CLIArgumentName("aad-endpoint", true)]
		public string AadEndpoint { get; set; }

        /// <summary>
		/// Application ID of user-assigned identity. Required for service principal auth.
        /// </summary>
		[CLIArgumentName("application-id", true)]
		public string ApplicationId { get; set; }

        /// <summary>
		/// Path to certificate for SPN authentication. Required for certificate-based service principal auth.
        /// </summary>
		[CLIArgumentName("certificate-path", true)]
		public string CertificatePath { get; set; }

        /// <summary>
		/// Log in using virtual machine's identity, also known as managed service identity (MSI).
        /// </summary>
		[CLIArgumentName("identity")]
		public bool? Identity { get; set; }

        /// <summary>
		/// Client ID of user-assigned identity.
        /// </summary>
		[CLIArgumentName("identity-client-id", true)]
		public string IdentityClientId { get; set; }

        /// <summary>
		/// Object ID of user-assigned identity.
        /// </summary>
		[CLIArgumentName("identity-object-id", true)]
		public string IdentityObjectId { get; set; }

        /// <summary>
		/// Resource ID of user-assigned identity.
        /// </summary>
		[CLIArgumentName("identity-resource-id", true)]
		public string IdentityResourceId { get; set; }

        /// <summary>
		/// Log in via Service Principal Name (SPN) by using a certificate or a secret. The client secret or certificate password must be placed in the appropriate environment variable. Type AzCopy env to see names and descriptions of environment variables.
        /// </summary>
		[CLIArgumentName("service-principal")]
		public bool? ServicePrincipal { get; set; }

        /// <summary>
		/// The Azure Active Directory tenant ID to use for OAuth device interactive login.
        /// </summary>
		[CLIArgumentName("tenant-id", true)]
		public string TenantId { get; set; }

        /// <summary>
		/// Caps the transfer rate, in megabits per second. Moment-by-moment throughput might vary slightly from the cap. If this option is set to zero, or it is omitted, the throughput isn't capped.
        /// </summary>
		[CLIArgumentName("cap-mbps")]
		public float? CapMbps { get; set; }

        /// <summary>
		/// Define the log verbosity for the log file, available levels: INFO(all requests/responses), WARNING(slow responses), ERROR(only failed requests), and NONE(no output logs). (default 'INFO'). (default "INFO")
        /// </summary>
		[CLIArgumentName("log-level", true)]
		public string LogLevel { get; set; }

        /// <summary>
		/// Define the output verbosity. Available levels: essential, quiet. (default "default")
        /// </summary>
		[CLIArgumentName("output-level", true)]
		public string OutputLevel { get; set; }

        /// <summary>
		/// Format of the command's output. The choices include: text, json. The default value is 'text'. (default "text")
        /// </summary>
		[CLIArgumentName("output-type", true)]
		public string OutputType { get; set; }

        /// <summary>
		/// Specifies additional domain suffixes where Azure Active Directory login tokens may be sent.  The default is '*.core.windows.net;*.core.chinacloudapi.cn;*.core.cloudapi.de;*.core.usgovcloudapi.net;*.storage.azure.net'. Any listed here are added to the default. For security, you should only put Microsoft Azure domains here. Separate multiple entries with semi-colons.
        /// </summary>
		[CLIArgumentName("trusted-microsoft-suffixes", true)]
		public string TrustedMicrosoftSuffixes { get; set; }

	}
}