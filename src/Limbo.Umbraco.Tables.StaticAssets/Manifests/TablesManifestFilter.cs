using Umbraco.Cms.Core.Manifest;

namespace Limbo.Umbraco.Tables.StaticAssets.Manifests;

/// <inheritdoc />
public class TablesManifestFilter : IManifestReader {
    public Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync() {
        return Task.FromResult<IEnumerable<PackageManifest>>(Task.FromResult(
            new List<PackageManifest>() {
                new() {
                    AllowPackageTelemetry = true,
                    Id = TablesPackage.Alias,
                    Name = TablesPackage.Name,
                    Version = TablesPackage.InformationalVersion,
                    Extensions = new object[] {
                        Scripts = new[] {
                            $"/App_Plugins/{TablesPackage.Alias}/Scripts/Controllers/CacheLevel.js",
                            $"/App_Plugins/{TablesPackage.Alias}/Scripts/Controllers/TableDataEditor.js",
                            $"/App_Plugins/{TablesPackage.Alias}/Scripts/Controllers/TableDataOverlay.js"
                        },
                        Stylesheets = new[] { $"/App_Plugins/{TablesPackage.Alias}/Styles/Styles.css" },
                    }
                }
            }
        ));
    }

}