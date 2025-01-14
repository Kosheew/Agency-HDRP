using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class Base64DataAttribute : Attribute
{
    public string Base64Data { get; }
    public FilterMode FilterMode { get; }

    public Base64DataAttribute(string base64Data)
    {
        Base64Data = base64Data;
        FilterMode = FilterMode.Point;
    }

    public Base64DataAttribute(FilterMode filterMode, string base64Data)
    {
        Base64Data = base64Data;
        FilterMode = filterMode;
    }
}