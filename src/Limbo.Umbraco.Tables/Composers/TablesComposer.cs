using Limbo.Umbraco.Tables.Manifests;
using Limbo.Umbraco.Tables.Parsers;
using Limbo.Umbraco.Tables.PropertyEditors;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Infrastructure.Manifest;

namespace Limbo.Umbraco.Tables.Composers;

/// <inheritdoc />
public class TablesComposer : IComposer {

    /// <inheritdoc />
    public void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<TablesHtmlParser>();
        builder.Services.AddSingleton<TablesJsonParser>();
        builder.Services.AddSingleton<TablePropertyIndexValueFactory>();
        builder.PropertyValueConverters().Append<TableValueConverter>();
        builder.Services.AddSingleton<IPackageManifestReader, TablesManifestReader>();
    }

}