using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectIconProvider<TEnum> where TEnum: Enum
{
    private Dictionary<TEnum, Texture2D> _textures = new Dictionary<TEnum, Texture2D>();

    public Texture2D Get(TEnum id)
    {
        if (!_textures.TryGetValue(id, out var texture))
            throw new ArgumentException($"Passed invalid id to game object icon provider, passed: {id}. Did you forget to fill provider first?");

        return texture;
    }

    public void FillWith(TEnum id, Texture2D texture) => _textures.Add(id, texture);
}