using System.Diagnostics.CodeAnalysis;
using Limbo.Umbraco.Tables.PropertyEditors;
using Umbraco.Cms.Core.Templates;

namespace Limbo.Umbraco.Tables.Parsers;

/// <summary>
/// HTML parsed used by the <see cref="TableValueConverter"/> class.
/// </summary>
public class TablesHtmlParser {

    private readonly HtmlLocalLinkParser _linkParser;
    private readonly HtmlUrlParser _urlParser;
    private readonly HtmlImageSourceParser _imageSourceParser;

    /// <summary>
    /// Initializes a new instance based on the specified dependencies.
    /// </summary>
    /// <param name="linkParser"></param>
    /// <param name="urlParser"></param>
    /// <param name="imageSourceParser"></param>
    public TablesHtmlParser(HtmlLocalLinkParser linkParser, HtmlUrlParser urlParser, HtmlImageSourceParser imageSourceParser) {
        _linkParser = linkParser;
        _urlParser = urlParser;
        _imageSourceParser = imageSourceParser;
    }

    /// <summary>
    /// Parses the specified <paramref name="sourceString"/>.
    /// </summary>
    /// <param name="sourceString">The HTML string to be parsed.</param>
    /// <param name="preview">Whether the HTML string should be parsed in preview mode.</param>
    /// <returns>The parsed HTML string.</returns>
    [return: NotNullIfNotNull("sourceString")]
    public string? Parse(string? sourceString, bool preview) {

        if (sourceString == null) return null;

        // ensures string is parsed for {localLink} and URLs and media are resolved correctly
        sourceString = _linkParser.EnsureInternalLinks(sourceString, preview);
        sourceString = _urlParser.EnsureUrls(sourceString);
        sourceString = _imageSourceParser.EnsureImageSources(sourceString);

        return sourceString;

    }

}