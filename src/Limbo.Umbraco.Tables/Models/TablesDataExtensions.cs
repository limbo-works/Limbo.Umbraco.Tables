using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;

namespace Limbo.Umbraco.Tables.Models;

internal static class TablesDataExtensions {

    public static JArray GetArrayOrNew(this JObject obj, string propertyName) {
        return obj.GetArray(propertyName) ?? new JArray();
    }

    public static IEnumerable<TResult> ForEach<TResult>(this JArray array, Func<int, JObject, TResult> callback) {

        List<TResult> temp = new();

        for (int i = 0; i < array.Count; i++) {

            temp.Add(callback(i, array.GetObject(i)!));

        }

        return temp;

    }

    public static IEnumerable<TResult> ForEach<TResult>(this JArray array, Func<int, JArray, TResult> callback) {

        List<TResult> temp = new();

        for (int i = 0; i < array.Count; i++) {

            temp.Add(callback(i, array.GetArray(i)!));

        }

        return temp;

    }

}