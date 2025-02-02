﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MillionaireMOD.Resources;

internal static class ResourceManager
{
    private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();
    private static readonly Dictionary<string, object> _cache = new();

    public static AssetBundle GetAssetBundle(string name)
    {
        string targetedName = /*"AssetBundles." +*/ name;

        if (TryGetCached(targetedName, out AssetBundle cached)) return cached;

        byte[] buffer = GetEmbeddedBytes(targetedName, true);
        AssetBundle assetBundle = AssetBundle.LoadFromMemory(buffer);
        return Cache(targetedName, assetBundle);
    }

    private static byte[] GetEmbeddedBytes(string name, bool throwIfNotFound)
    {
        string path = _assembly.GetManifestResourceNames().FirstOrDefault(n => n.Contains(name));
        if (path == null) return !throwIfNotFound ? null : throw new FileNotFoundException($"Embedded resource {name} not found");

        Stream manifestResourceStream = _assembly.GetManifestResourceStream(path)!;
        return manifestResourceStream.ReadFully();
    }

    private static bool TryGetCached<T>(string name, out T value) where T : class
    {
        if (!_cache.TryGetValue(name, out object cachedObject))
        {
            value = null;
            return false;
        }

        value = cachedObject as T ?? throw new InvalidCastException($"Cached object is not of type {typeof(T)}");
        return true;
    }

    private static T Cache<T>(string name, T obj)
    {
        _cache.Add(name, obj);

        return obj;
    }

    private static byte[] ReadFully(this Stream stream)
    {
        using MemoryStream ms = new();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
}
