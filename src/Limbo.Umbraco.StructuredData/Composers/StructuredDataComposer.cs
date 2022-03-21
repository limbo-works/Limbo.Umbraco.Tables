using Limbo.Umbraco.StructuredData.Manifests;
using Limbo.Umbraco.StructuredData.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Limbo.Umbraco.StructuredData.Composers {

    /// <inheritdoc />
    public class StructuredDataComposer : IComposer {

        /// <inheritdoc />
        public void Compose(IUmbracoBuilder builder) {
            builder.Services.AddSingleton<StructuredDataHtmlParser>();
            builder.ManifestFilters().Append<StructuredDataManifestFilter>();
        }

    }

}