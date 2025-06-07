using Limbo.Umbraco.Tables.StaticAssets.Manifests;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Infrastructure.Manifest;

namespace Limbo.Umbraco.Tables.StaticAssets.Composers;

/// <inheritdoc />
public class TablesComposer : IComposer {

    /// <inheritdoc />
    public void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IPackageManifestReader, TablesManifestFilter>();
    }

}