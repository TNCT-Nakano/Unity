#define STUB //スタブとして動作させ無い場合はコメントアウトすること
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 作成　川上　輝
 * 目的　BlueNinjaとの接続
 * 補足　現段階において動作を確認するためのスタブである。
 */

public class BlueComm /*: MonoBehaviour*/ {
    //フィールド宣言
    private Vector3 acc, gyro, geo;//加速度[m/s2]、角速度[rad/s2]、地磁気
    private float batt;//バッテリー電圧[0~1]
    private float period;//周期[s]


    //メソッド宣言
    public BlueComm(float _period = 1.0f)
    {
        acc = new Vector3();
        gyro = new Vector3();
        geo = new Vector3();
        batt = 1.0f;
        period = _period;
    }

    public void Update()
    {
#if (STUB)
        acc.Set(1, 2, 3);
        gyro.Set(4, 5, 6);
        geo.Set(7, 8, 9);
        batt = 0.9f;
#endif
    }


    //プロパティ宣言
    public Vector3 Acceleration { get { return acc; } }
    public Vector3 Gyro { get { return gyro; } }
    public Vector3 Geomagnetism{ get { return geo; } }
    public float Battery { get { return batt; } }
    public float Period { get { return period; } }
   

}
