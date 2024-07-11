using System.Collections.Generic;
using Umbraco.Cms.Core.Manifest;

namespace Limbo.Umbraco.Tables.Manifests;

/// <inheritdoc />
public class TablesManifestFilter : IManifestFilter {

    /// <inheritdoc />
    public void Filter(List<PackageManifest> manifests) {

        // Initialize a new manifest filter for this package
        PackageManifest manifest = new() {
            AllowPackageTelemetry = true,
            PackageId = TablesPackage.Alias,
            PackageName = TablesPackage.Name,
            Version = TablesPackage.InformationalVersion,
            BundleOptions = BundleOptions.Independent,
            Scripts = new[] {
                $"/App_Plugins/{TablesPackage.Alias}/Scripts/Controllers/CacheLevel.js",
                $"/App_Plugins/{TablesPackage.Alias}/Scripts/Controllers/TableDataEditor.js",
                $"/App_Plugins/{TablesPackage.Alias}/Scripts/Controllers/TableDataOverlay.js"
            },
            Stylesheets = new[] { $"/App_Plugins/{TablesPackage.Alias}/Styles/Styles.css" }
        };

        // Append the manifest
        manifests.Add(manifest);

    }

}