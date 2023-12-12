using Newtonsoft.Json.Linq;

namespace Limbo.Umbraco.Tables.Models;

/// <summary>
/// Class representing an object that was parsed from an instance of <see cref="Newtonsoft.Json.Linq.JObject"/>.
/// </summary>
public class TableObject {

    #region Properties

    /// <summary>
    /// Gets the internal <see cref="Newtonsoft.Json.Linq.JObject"/> the object was created from.
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public JObject JObject { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance from the specified <paramref name="json"/> object.
    /// </summary>
    /// <param name="json">The instance of <see cref="Newtonsoft.Json.Linq.JObject"/> representing the object.</param>

    protected TableObject(JObject json) {
        JObject = json;
    }

    #endregion

}