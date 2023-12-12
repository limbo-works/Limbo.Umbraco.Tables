using Limbo.Umbraco.Tables.Manifests;
using Limbo.Umbraco.Tables.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Limbo.Umbraco.Tables.Composers;

/// <inheritdoc />
public class TablesComposer : IComposer {

    /// <inheritdoc />
    public void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<TablesHtmlParser>();
        builder.ManifestFilters().Append<TablesManifestFilter>();
    }

}