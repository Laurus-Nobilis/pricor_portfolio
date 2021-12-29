using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *  何をやりたいか？
 *  
 *  シリアライズ・オブジェクトを、カスタムエディターから変更する。
 *  
 *  Editorが変更を受け取り、
 *  
 *  Undoできるようにする！！
 *  
 */
public class UndoTest : MonoBehaviour
{
    //曰く
    //>>
    //　カスタムエディターを使用してコンポーネントかアセットのシリアライズしたプロパティーを変更する場合は、
    //　SerializedObject.FindProperty、SerializedObject.Update、EditorGUILayout.PropertyField、SerializedObject.ApplyModifiedProperties を使用します。
    //　これらによって、変更されたオブジェクトに「ダーティ」と印をつけて、「元に戻す」(Undo) のステートにします。



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
