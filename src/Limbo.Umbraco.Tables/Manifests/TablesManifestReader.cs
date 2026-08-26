using System.Collections.Generic;
using System.Threading.Tasks;
using Skybrud.Essentials.Security.Extensions;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Limbo.Umbraco.Tables.Manifests;

/// <inheritdoc />
public class TablesManifestReader : IPackageManifestReader {

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
                        alias = $"{TablesPackage.Alias}.EntryPoint",
                        name = $"{TablesPackage.Name}: Entry Point",
                        type = "backofficeEntryPoint",
                        js = $"/App_Plugins/{TablesPackage.Alias}/limbo-umbraco-tables.js?v={cacheBuster}",
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