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
    //UH instance
    //private static UH instance = null;
    //public static UH global { get { return instance; } }

    public enum READ_TYPE { HIGH_SPEED, LOW_SPEED };

    //Objects: Accel and Gyro
    public Vector3 Gyro;
    public Vector3 Accel;
    public float[] UHGyroAccelData = new float[7];

    //Objects: Quaternion
    public float[] UHQuaternion = new float[4];
    public bool updateQuaternionFlg;

    //Object: Foream angle
    public int[] UHAngle = new int[3];
    public bool updateAnglePRFlg;
    public bool updateAngleTempFlg;

    //Object: Photo-reflectors' value to detect hand movements
    public int[] UHPR = new int[8];
    public bool updatePhotoSensorsFlg;

    //Object: Tempature 
    public float temp;

    //Object: EMS
    public int EMS_voltageLevel;    //the voltage level(Stimulation Level) of EMS
    public int EMS_sharpnessLevel;  // the sharpness level of EMS

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
    private string _rcv = "";
    public string rcv
    {
        get
        {
            return _rcv;
        }
        set
        {
            if (_rcv != value)
            {
                _rcv = value;
                //ここでイベント起こすこともできるよ
                Debug.Log("received value = "+value);
                if (value[0] != '!')
                {
                    Debug.Log("not debug value");
                    for (int i = 0; i < 3; i++)
                    {
                        Gyro[i] = (Convert.ToInt32(value.Substring(i * 5, 4), 16));
                        Accel[i] = (Convert.ToInt32(value.Substring((i + 3) * 5, 4), 16));
                    }
                }
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
                uint count = await Reader.LoadAsync(sizeof(char)*30);
                return Reader.ReadString(count);
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

    public void Open(string deviceName)
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

    public void Close()
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

    public void WriteLine(string data)
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

    public string ReadLine()
    {
#if NETFX_CORE
        if(!isOpen)return "";
        return Task.Run<string>(async () => {return await _ReadLine();}).Result;
#else
        Debug.Log("This function is only supported on HoloLens.");
        return "";
#endif
    }

    public void _StartWaiter()
    {
#if NETFX_CORE
        if(!isOpen){
            message = "port closed. starting waiter is cancelled";
        }
        Task.Run(async () =>{
            message = "waiter is started.";
            while(isOpen)
                rcv=await _ReadLine();
            message = "waiter is stopped.";
        });
#endif
    }

    public List<string> GuessDeviceNames()
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
        Debug.Log("Devices:" + string.Join(",", GuessDeviceNames().ToArray()));
        Open("RNI-SPP");
        StartCoroutine(StartWaiter());
    }

    IEnumerator StartWaiter()
    {
        yield return new WaitUntil(()=> { return isOpen; });
        _StartWaiter();
    }

    public void stimulate(int channel, int time, int volt, int sharpness)
    {

        bool pastUpdateAnglePRFlg = updateAnglePRFlg;
        bool pastUpdateAngleTempFlg = updateAngleTempFlg;
        bool pastUpdatePhotoSensorsFlg = updatePhotoSensorsFlg;
        bool pastUpdateQuaternionFlg = updateQuaternionFlg;
        updateAnglePRFlg = false;
        updatePhotoSensorsFlg = false;
        updateQuaternionFlg = false;

        WriteLine("ems " + channel.ToString() + " " + time.ToString() + " " + volt.ToString() + " " + sharpness.ToString() + "\n");

        updateAnglePRFlg = pastUpdateAnglePRFlg;
        updateAngleTempFlg = pastUpdateAngleTempFlg;
        updatePhotoSensorsFlg = pastUpdatePhotoSensorsFlg;
        updateQuaternionFlg = pastUpdateQuaternionFlg;
    }

    public void vibrationOn()
    {
        WriteLine("vbr_on\n");
    }

    public void vibrationOff()
    {
        WriteLine("vbr_off\n");
    }

    public void vibrationTime(int time)
    {
        WriteLine("vbr " + time.ToString() + "\n");
    }

    /*

    public void readGyro()
    {
        try
        {
            if (isOpen)
            {
                string[] Gyro = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    Gyro[i] = rcv.Substring(i * 5, 4);//文字指定0~34~
                    UHGyro[i] = float.Parse(Gyro[i]);
                }
            }
        }
        catch (Exception e)
        {
        }
    }

    public void readAccel()
    {
        try
        {
            if (isOpen)
            {
                string[] Accel = new string[3];
                for (int i = 3; i < 6; i++)
                {
                    Accel[i - 3] = rcv.Substring(i * 5,4);//文字指定0~34~
                    UHAccel[i - 3] = float.Parse(Accel[i - 3]);
                }
            }
        }
        catch (TimeoutException)
        {
        }
    }
    
    public void stimulate(int padNum)
    {
        StartCoroutine(_stimulate(padNum));
    }

    IEnumerator _stimulate(int padNum)
    {
        bool pastUpdateAnglePRFlg = updateAnglePRFlg;
        bool pastUpdateAngleTempFlg = updateAngleTempFlg;
        bool pastUpdatePhotoSensorsFlg = updatePhotoSensorsFlg;
        bool pastUpdateQuaternionFlg = updateQuaternionFlg;
        updateAnglePRFlg = false;
        updatePhotoSensorsFlg = false;
        updateQuaternionFlg = false;

        WriteLine(padNum.ToString());
        yield return new WaitForSeconds(1f);

        updateAnglePRFlg = pastUpdateAnglePRFlg;
        updateAngleTempFlg = pastUpdateAngleTempFlg;
        updatePhotoSensorsFlg = pastUpdatePhotoSensorsFlg;
        updateQuaternionFlg = pastUpdateQuaternionFlg;
    }



    /////////////////////////////////////////////////////
    /// EMS(Electri Muscle Stimulation) functions to move the user's hand
    /////////////////////////////////////////////////////
    public void stimulateSetSleepTime(int padNum, int sleepTimeMs)
    {
        StartCoroutine(_stimulateSetSleepTime(padNum, sleepTimeMs));
    }

    IEnumerator _stimulateSetSleepTime(int padNum, int sleepTimeMs)
    {
        bool pastUpdateAnglePRFlg = updateAnglePRFlg;
        bool pastUpdateAngleTempFlg = updateAngleTempFlg;
        bool pastUpdatePhotoSensorsFlg = updatePhotoSensorsFlg;
        bool pastUpdateQuaternionFlg = updateQuaternionFlg;
        updateAnglePRFlg = false;
        updatePhotoSensorsFlg = false;
        updateQuaternionFlg = false;

        WriteLine(padNum.ToString());
        yield return new WaitForSeconds(((float)sleepTimeMs / 1000f));

        updateAnglePRFlg = pastUpdateAnglePRFlg;
        updateAngleTempFlg = pastUpdateAngleTempFlg;
        updatePhotoSensorsFlg = pastUpdatePhotoSensorsFlg;
        updateQuaternionFlg = pastUpdateQuaternionFlg;
    }

    /////////////////////////////////////////////////////
    /// Level up the voltage(Stimulation Level) of EMS
    /////////////////////////////////////////////////////
    public void setVoltageLevelUp()
    {
        try
        {
            if (EMS_voltageLevel <= 12)
            {
                WriteLine("h");
                EMS_voltageLevel++;
            }
        }
        catch (TimeoutException)
        {

        }
    }

    /////////////////////////////////////////////////////
    ///  Level down the voltage(Stimulation Level) of EMS
    /////////////////////////////////////////////////////
    public void setVoltageLevelDown()
    {
        try
        {
            if (0 < EMS_voltageLevel)
            {
                WriteLine("l");
                EMS_voltageLevel--;
            }
        }
        catch (TimeoutException)
        {

        }
    }

    /////////////////////////////////////////////////////
    /// Level up the sharpness level of EMS
    /////////////////////////////////////////////////////
    public void setSharpnessLevelUp()
    {
        try
        {
            if (EMS_sharpnessLevel <= 20)
            {
                WriteLine("t");
                EMS_sharpnessLevel += 5;
            }
        }
        catch (TimeoutException)
        {

        }
    }

    /////////////////////////////////////////////////////
    ///  Level down the sharpness level of EMS
    /////////////////////////////////////////////////////
    public void setShapnessLevelDown()
    {
        try
        {
            if (5 <= EMS_sharpnessLevel)
            {
                WriteLine("u");
                EMS_sharpnessLevel -= 5;
            }
        }
        catch (TimeoutException)
        {

        }
    }

    /////////////////////////////////////////////////////
    /// Vibration function
    /////////////////////////////////////////////////////
    public void vibrate()
    {
        try
        {
            WriteLine("b");
        }
        catch (TimeoutException)
        {

        }
    }


    //
    //オリジナル～
    //

    public void stimulateMax(int padNum, int sleepTimeMs, int volt, int sharp)
    {
        StartCoroutine(_stimulateMax(padNum, sleepTimeMs, volt, sharp));
    }

    IEnumerator _stimulateMax(int padNum, int sleepTimeMs, int volt, int sharp)
    {
        bool pastUpdateAnglePRFlg = updateAnglePRFlg;
        bool pastUpdateAngleTempFlg = updateAngleTempFlg;
        bool pastUpdatePhotoSensorsFlg = updatePhotoSensorsFlg;
        bool pastUpdateQuaternionFlg = updateQuaternionFlg;
        updateAnglePRFlg = false;
        updatePhotoSensorsFlg = false;
        updateQuaternionFlg = false;
        if (EMS_voltageLevel < volt)
        {
            while (EMS_voltageLevel < volt)
            {
                setVoltageLevelUp();
            }
        }
        else
        {
            while (EMS_voltageLevel > volt)
            {
                setVoltageLevelDown();
            }
        }

        if (EMS_sharpnessLevel < sharp)
        {
            while (EMS_sharpnessLevel < sharp)
            {
                setSharpnessLevelUp();
            }
        }
        else
        {
            while (EMS_sharpnessLevel > sharp)
            {
                setShapnessLevelDown();
            }
        }

        WriteLine(padNum.ToString());
        //Thread.Sleep(sleepTimeMs);
        yield return new WaitForSeconds(((float)sleepTimeMs / 1000f));

        updateAnglePRFlg = pastUpdateAnglePRFlg;
        updateAngleTempFlg = pastUpdateAngleTempFlg;
        updatePhotoSensorsFlg = pastUpdatePhotoSensorsFlg;
        updateQuaternionFlg = pastUpdateQuaternionFlg;
    }


    public void readGyro()
    {
        try
        {
            if (isOpen)
            {
                string readData = ReadLine();
                string[] Gyro = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    Gyro[i] = readData.Substring(i * 4, i * 4 + 3);//文字指定0~34~
                    UHGyro[i] = float.Parse(Gyro[i]);
                }
            }
        }
        catch (TimeoutException)
        {
        }
    }*/
}