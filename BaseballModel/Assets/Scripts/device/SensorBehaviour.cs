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
        message = value;
        Debug.Log("received value = " + value);
        if (value[0] != '!')
        {
            for (int i = 0; i < 3; i++)
            {
                Gyro[i] = (float)(Convert.ToInt32(value.Substring(i * 9, 8), 16)) / (1 << 16);
                Accel[i] = (float)(Convert.ToInt32(value.Substring((i + 3) * 9, 8), 16)) / (1 << 16);
            }
        }
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
