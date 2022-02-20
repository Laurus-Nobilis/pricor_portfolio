using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class PlayerCameraBase : MonoBehaviourPunCallbacks
{
    //このカメラの現在の水平方向 1Frameでの回転量
    public virtual Quaternion HorizontalRot { get 
        {
            return Quaternion.identity;
        }
    }
}
