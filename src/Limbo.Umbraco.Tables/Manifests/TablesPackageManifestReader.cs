using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Limbo.Umbraco.Tables.Manifests;

/// <inheritdoc />
public class TablesPackageManifestReader : IPackageManifestReader {

    public Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync() {

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
                        js = $"/App_Plugins/{TablesPackage.Alias}/limbo-umbraco-tables.js",
                    }
                ]
            }

        ];

        return Task.FromResult<IEnumerable<PackageManifest>>(
            list
        );

    }

}