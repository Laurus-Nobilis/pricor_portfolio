using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UniRx;

public class NewEditModeTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void NewEditModeTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewEditModeTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.

        var o = Observable.EveryUpdate().Delay(new TimeSpan(5));
        var d = o.Subscribe(_ => Debug.Log("test"));

        const int COUNT_LIMIT = 300;
        int count = 0;
        while (true)
        {
            if (++count > COUNT_LIMIT)
            {
                break;
            }
            Debug.Log("onece");

            yield return null;
        }

        d.Dispose();

        Assert.IsTrue(false, "end");
    }
}
