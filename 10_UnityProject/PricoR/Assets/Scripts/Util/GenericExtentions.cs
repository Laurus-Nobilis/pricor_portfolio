using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic�̊g�����\�b�h�W
/// </summary>
public static class GenericExtensions
{
    public static void Swap<T>(ref this T a, ref T b) where T : struct
    {
        var tmp = a;
        a = b;
        b = tmp;
    }
}

