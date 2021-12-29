using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DDD
{
    public class DDDataTEST
    {
        public List<int> _list = new List<int>();
    }


    public class Manager : MonoBehaviour
    {
        EquipmentData _edata;
        CharacterViewTextMaster _master;


        DDDataTEST testdataClass = new DDDataTEST();

    
        [Inject]
        public void Construct(EquipmentData data)
        {
            Debug.Log("Equipment");
            _edata = data;
        }

        [Inject]
        public void Construct(CharacterViewTextMaster master)
        {
            Debug.Log("TextMaster");

            _master = master;
        }

        private void Start()
        {
            for (int i=0; i<1000; i++)
            {
                testdataClass._list.Add(i*i-i);
            }
            var save = new LocalSaveData();

            // SAVE
            var t = save.SaveAsync(testdataClass);
            t.Wait();

            // LOAD
            var tt = save.LoadAsync<DDDataTEST>();
            //tt.Wait();//止まる、デッドロックか、っていう。
            Debug.LogWarning(tt);
            
            Debug.LogWarning(tt.Result._list);//Result もデッドロックかっ、ていう。ではどうやって結果を受け取るのか？

        }
    }
}
