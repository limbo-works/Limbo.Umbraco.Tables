using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;
using Skybrud.Essentials.Security.Extensions;
namespace Limbo.Umbraco.Tables.StaticAssets.Manifests;

/// <inheritdoc />
public class TablesManifestFilter : IPackageManifestReader {
    public Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync() {
        string cacheBuster = TablesPackage.InformationalVersion.ToMd5Hash();
        List<PackageManifest> list = [
            new() {
                AllowTelemetry = true,
                Id = TablesPackage.Alias,
                Name = TablesPackage.Name,
                Version = TablesPackage.InformationalVersion,
                AllowPublicAccess = false,
                Extensions = [
                    new {
                        name = "limbo.tables.entryPoint",
                        alias = "Limbo.Tables.entryPoint",
                        type = "backofficeEntryPoint",
                        js = $"/App_Plugins/{TablesPackage.AppPluginsName}/limbo-umbraco-tables.js?v={cacheBuster}",
                    }
                ],
                Importmap = null,
            }

        ];
        return Task.FromResult<IEnumerable<PackageManifest>>(
            list
        );
    }

}