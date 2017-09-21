using UnityEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;

#if NETFX_CORE
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
#endif

public class SensorBehaviour : BluetoothDevice {
    public Vector3 Gyro;
    public Vector3 Accel;

    public override void rcvCallback(string value)
    {
        if (value[0] != '!')
        {
            for (int i = 0; i < 3; i++)
            {
                Gyro[i] = (float)(Convert.ToInt32(value.Substring(i * 9, 8), 16)) / (1 << 16);
                Accel[i] = (float)(Convert.ToInt32(value.Substring((i + 3) * 9, 8), 16)) / (1 << 16);
            }
        }
        //‚±‚ê‚ª‚ß‚¿‚á‚­‚¿‚ád‚¢
        //Debug.Log("[Srcv]G:" + Gyro.ToString() +",A:"+Accel.ToString()+",Time(ms):"+DateTime.Now.Hour * 60 * 60 * 1000 + DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond);
    }

    // Use this for initialization
    void Start()
    {
        dataSize = 27;
        Open("BTHello");
        StartCoroutine(StartWaiter());
    }

    // Update is called once per frame
    void Update () {
		
	}
}
