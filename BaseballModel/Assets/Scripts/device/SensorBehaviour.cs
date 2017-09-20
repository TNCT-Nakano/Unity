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
    public override void rcvCallback(string value)
    {
        message = value;
    }

    // Use this for initialization
    void Start()
    {
        dataSize = 54;
        Open("BTHello");
        StartCoroutine(StartWaiter());
    }

    // Update is called once per frame
    void Update () {
		
	}
}
