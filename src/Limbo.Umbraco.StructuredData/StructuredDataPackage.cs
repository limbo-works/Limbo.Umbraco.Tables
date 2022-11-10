using System;
using Skybrud.Essentials.Reflection;
using Umbraco.Cms.Core.Semver;

namespace Limbo.Umbraco.StructuredData {

    /// <summary>
    /// Static class with various information and constants about the package.
    /// </summary>
    public class StructuredDataPackage {

        /// <summary>
        /// Gets the alias of the package.
        /// </summary>
        public const string Alias = "Limbo.Umbraco.StructuredData";

        /// <summary>
        /// Gets the friendly name of the package.
        /// </summary>
        public const string Name = "Limbo Structured Data";

        /// <summary>
        /// Gets the version of the package.
        /// </summary>
        public static readonly Version Version = typeof(StructuredDataPackage).Assembly.GetName().Version!;

        /// <summary>
        /// Gets the semantic version of the package.
        /// </summary>
        public static readonly SemVersion SemVersion = SemVersion.Parse(ReflectionUtils.GetInformationalVersion<StructuredDataPackage>());

        /// <summary>
        /// Gets the URL of the GitHub repository for this package.
        /// </summary>
        public const string GitHubUrl = "https://github.com/limbo-works/Limbo.Umbraco.StructuredData";

        /// <summary>
        /// Gets the URL of the issue tracker for this package.
        /// </summary>
        public const string IssuesUrl = "https://github.com/limbo-works/Limbo.Umbraco.StructuredData/issues";

        /// <summary>
        /// Gets the URL of the documentation for this package.
        /// </summary>
        public const string DocumentationUrl = "https://github.com/limbo-works/Limbo.Umbraco.StructuredData#documentation";


    }

}