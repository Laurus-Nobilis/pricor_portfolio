using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

interface IView 
{
    public bool IsMove { get; }
    public void FadeIn(Action endCallback);
    public void FadeOut(Action endCallback);
}
