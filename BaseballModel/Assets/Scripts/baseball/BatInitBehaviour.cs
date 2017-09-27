using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatInitBehaviour : MonoBehaviour {
    private GameObject initBat = null;
    public GameObject batPrefab;
    public void CreateInitializer()
    {
        if (!initBat) //初期化中じゃなければ初期化用ホログラムが存在しない
        {
            //既存のバットを消去
            var oldBat = GameObject.FindWithTag("Bat");
            if (oldBat)
                Destroy(oldBat);

            //初期化用ホログラム生成
            //TODO プリミティブなオブジェクトではなく、Prefabによる初期化用のバットにするべき
            initBat = Instantiate(batPrefab);
            initBat.GetComponentInChildren<BatBehaviourScript>().forInit = true;
        }
    }

    public void FinishInitializing()
    {
        if (initBat)    //初期化中なら初期化用ホログラムが存在する
        {
            var newBat = Instantiate(batPrefab);
            newBat.transform.position = initBat.transform.position;

            Debug.Log(newBat.transform.position +"::"+ initBat.transform.position);

            Destroy(initBat);
            initBat = null;
        }
    }
}
