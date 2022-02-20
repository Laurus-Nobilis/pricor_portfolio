//------------------------------------------
//Miscは悪しき慣習だが個人用の物はこちらへ。
//------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class Vector
    {
        static bool IsHitDistance(Vector3 target, Vector3 self, float hitDistance)
        {
            var offset = target - self;
            if(offset.sqrMagnitude > hitDistance * hitDistance)
            {
                return true;
            }
            return false;
        }
    }
}
