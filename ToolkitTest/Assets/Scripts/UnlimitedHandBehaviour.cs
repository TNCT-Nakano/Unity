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

public class UnlimitedHandBehaviour : MonoBehaviour
{

    private string _message = null;
    public string message
    {
        get
        {
            return _message;
        }
        set
        {
            if (_message != value)
            {

                _message = value;
                Debug.Log("message changed:" + value);
            }
        }
    }
    private bool _isOpen = false;
    public bool isOpen
    {
        get
        {
            return _isOpen;
        }
        set
        {
            if (_isOpen != value)
            {
                _isOpen = value;
                Debug.Log("isOpen changed:" + value);
            }
        }
    }

#if NETFX_CORE
    private RfcommDeviceService DeviceService;
    private StreamSocket BtSocket;
    private DataWriter Writer;
    private DataReader Reader;

    private async Task _Open(string deviceName)
    {
        if (isOpen)
        {
            message = "Already port opened.";
            return;
        }
        if (deviceName != null)
        {
            //Bluetoothデバイス名と一致するデバイス情報を取得し接続する 
            var forwards = await DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort));
            foreach (var forward in forwards)
            {
                if (forward.Name == deviceName)
                {
                    await _Open(forward);
                    break;
                }
            }
        }
    }
    private async Task _Open(DeviceInformation serviceInfo)
    {
        try
        {
            //指定されたデバイス情報で接続を行う 
            if (DeviceService == null)
            {
                DeviceService = await RfcommDeviceService.FromIdAsync(serviceInfo.Id);
                BtSocket = new StreamSocket();
                await BtSocket.ConnectAsync(
                    this.DeviceService.ConnectionHostName,
                    this.DeviceService.ConnectionServiceName,
                    SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);
                Writer = new DataWriter(BtSocket.OutputStream);
                Reader = new DataReader(BtSocket.InputStream);
                this.message = "Connected " + DeviceService.ConnectionHostName.DisplayName;
                isOpen = true;
            }
        }
        catch (Exception ex)
        {
            this.message = ex.Message;
            DeviceService = null;
            isOpen = false;
        }
    }


    private async Task _Close()
    {
        try
        {
            DeviceService = null;
            await BtSocket.CancelIOAsync();
            Writer = null;
            Reader = null;
            message = "Closed";
            isOpen = false;                
        }
        catch(Exception ex)
        {
            this.message = ex.Message;
            DeviceService = null;
            isOpen = false;
        }
    }

    private async Task _WriteLine(string data)
    {
        try
        {
            if (DeviceService != null)
            {
                Writer.WriteString(data);
                var sendResult = await Writer.StoreAsync();
            }
        }
        catch (Exception ex)
        {
            this.message = ex.Message;
            DeviceService = null;
            isOpen = false;
        }
    }

    private async Task<string> _ReadLine()
    {
        try
        {
            if (DeviceService != null)
            {
                await Reader.LoadAsync(512);
                return Reader.ReadString(32);
            }
            return "";
        }
        catch(Exception ex)
        {
            this.message = ex.Message;
            DeviceService = null;
            isOpen = false;
            return "";
        }
    }

    private async Task<List<string>> _GuessDeviceNames()
    {
        List<string> deviceNames = new List<string>();
        var serviceInfos = await DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort));
        foreach (var serviceInfo in serviceInfos)
        {
            deviceNames.Add(serviceInfo.Name);
        }
        return deviceNames;
    }
#endif

    private void Open(string deviceName)
    {
#if NETFX_CORE
        Task.Run(async () =>
            {
                await _Open(deviceName);
            });
#else
        Debug.Log("This function is only supported on HoloLens.");
#endif
    }

    private void Close()
    {
#if NETFX_CORE
        if(!isOpen)return;
        Task.Run(async () =>
            {
                await _Close();
            });
#else
        Debug.Log("This function is only supported on HoloLens.");
#endif
    }

    private void WriteLine(string data)
    {
#if NETFX_CORE
        if(!isOpen)return;
        Task.Run(async () =>
            {
                await _WriteLine(data);
            });
#else
        Debug.Log("This function is only supported on HoloLens.");
#endif
    }

    private string ReadLine()
    {
#if NETFX_CORE
        if(!isOpen)return;
        return Task.Run<string>(async () => {return await _ReadLine();}).Result;
#else
        Debug.Log("This function is only supported on HoloLens.");
        return "";
#endif
    }

    private List<string> GuessDeviceNames()
    {
#if NETFX_CORE
        return Task<List<string>>.Run(async () => {return await _GuessDeviceNames();}).Result;
#else
        Debug.Log("This function is only supported on HoloLens.");
        return null;
#endif
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("Devices:"+string.Join(",",GuessDeviceNames().ToArray()));
        Open("RNI-SPP");
    }

    private int count=0;
    // Update is called once per frame
    void Update()
    {
        if (++count>180)
        {
            if(isOpen) WriteLine("4");
            count = 0;
        }
    }
}
