using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UniRx;
using System;

public class NewTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void NewTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
        var o = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Q));

        var d = o.Subscribe(l => Debug.Log("!"));

        //d.Dispose();
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.

        var o = Observable.EveryUpdate().Where(_=>Input.GetKeyDown(KeyCode.A));
        var d = o.Subscribe(_ => Debug.Log("test"));

        var t = Time.time;
        while (true)
        {
            if (Time.time - t > 10)
                break;

            yield return null;
        }

        d.Dispose();

        Assert.IsTrue(true);
    }
}
