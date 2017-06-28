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
    //Serial Connection
    public string PortName = "";        // Port Name
    //private SerialPort _serialPort;
    public int Baud = 115200;           // Baud Rate
    public int RebootDelay = 10;        // Amount of time to wait after opening connection for arduino to reboot before sending firmata commands

    public bool AutoConnect = true;     // Connect automatically
    public bool Connected { get; private set; }// true when the device is connected

    //UH instance
    //private static UH instance = null;
    //public static UH global { get { return instance; } }

    public enum READ_TYPE { HIGH_SPEED, LOW_SPEED };

    //Objects: Accel and Gyro
    public float[] UHGyro = new float[3];
    public float[] UHAccel = new float[3];
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

    /*private int count=0;
    // Update is called once per frame
    void Update()
    {
        if (++count > 180)
        {
            if (isOpen) WriteLine("4");
            count = 0;
        }
    }*/

    //////////////////////////////////////////////////////////
    /// READ Hand Movements(Photo-reflector sensors values) 
    ///                               via Serial Connection
    //////////////////////////////////////////////////////////

    public int[] readPhotoSensors()
    {
        return UHPR;
    }

    public void updatePhotoSensors()
    {
        try
        {
            WriteLine("c");
            string data = ReadLine();
            //Debug.Log(data);
            if (data != null && data.Contains("_"))
            {
                if (data.Split(new string[] { "_" }, System.StringSplitOptions.None).Length != 8)
                {
                    return;
                }
                for (int i = 0; i < 8; i++)
                {
                    int prVal = int.Parse(data.Split(new string[] { "_" }, System.StringSplitOptions.None)[i]);
                    if (0 <= prVal && prVal < 1024) UHPR[i] = prVal;
                }
            }
        }
        catch (TimeoutException)
        {
            //UHPR[0] = 0;
            //UHPR[1] = 0;
            //UHPR[2] = 0;
            //UHPR[3] = 0;
            //UHPR[4] = 0;
            //UHPR[5] = 0;
            //UHPR[6] = 0;
            //UHPR[7] = 0;
        }
    }

    //////////////////////////////////////////////////////////
    /// READ Forearm Angles and Tempature via Serial Connection
    //////////////////////////////////////////////////////////
    public void updateAngleTempature()
    {
        try
        {
            WriteLine("A");
            //Debug.Log ("Send a message : A");
            string data2 = ReadLine();
            //Debug.Log(data2);
            if (data2 != null && data2.Contains("+"))
            {
                if (data2.Split(new string[] { "+" }, System.StringSplitOptions.None).Length != 7)
                {
                    return;
                }
                for (int i = 0; i < 3; i++)
                {
                    int ang = int.Parse(data2.Split(new string[] { "+" }, System.StringSplitOptions.None)[i]);
                    if (-180 <= ang && ang <= 180) UHAngle[i] = ang;
                }
                temp = float.Parse(data2.Split(new string[] { "+" }, System.StringSplitOptions.None)[3]);
            }
        }
        catch (Exception)
        {
            //UHAngle[0] = 0;
            //UHAngle[1] = 0;
            //UHAngle[2] = 0;
        }
    }

    //////////////////////////////////////////////////////////
    /// START  Angle and Tempature Process via Serial Connection
    //////////////////////////////////////////////////////////
    public void startAngleTemp()
    {
        updateAngleTempFlg = true;
    }

    //////////////////////////////////////////////////////////
    /// STOP  Angle and Tempature Process via Serial Connection
    //////////////////////////////////////////////////////////
    public void stopAngleTemp()
    {
        updateAngleTempFlg = false;
    }



    //////////////////////////////////////////////////////////
    /// READ Forearm Angles and also Photo-Reflector's Values via Serial Connection
    //////////////////////////////////////////////////////////
    public void updateAnglePR()
    {
        try
        {
            WriteLine("C");
            string data3 = ReadLine();
            if (data3 != null && data3.Contains("+"))
            {
                if (data3.Split(new string[] { "+" }, System.StringSplitOptions.None).Length != 11)
                {
                    return;
                }
                for (int i = 0; i < 3; i++)
                {
                    int ang = int.Parse(data3.Split(new string[] { "+" }, System.StringSplitOptions.None)[i]);
                    UHAngle[i] = ang;
                }
                for (int i = 3; i < 11; i++)
                {
                    UHPR[i - 3] = int.Parse(data3.Split(new string[] { "+" }, System.StringSplitOptions.None)[i]);

                }
            }
        }
        catch (Exception)
        {
            //UHAngle[0] = 0;
            //UHAngle[1] = 0;
            //UHAngle[2] = 0;
        }
    }

    //////////////////////////////////////////////////////////
    /// START  Angle  Process via Serial Connection
    //////////////////////////////////////////////////////////
    public void startAnglePR()
    {
        updateAnglePRFlg = true;
    }

    //////////////////////////////////////////////////////////
    /// STOP  Angle  Process via Serial Connection
    //////////////////////////////////////////////////////////
    public void stopAnglePR()
    {
        updateAnglePRFlg = false;
    }

    //////////////////////////////////////////////////////////
    /// READ Gyro Accel data  via Serial Connection
    //////////////////////////////////////////////////////////
    public float[] readAccel()
    {
        UHAccel[0] = UHGyroAccelData[0];
        UHAccel[1] = UHGyroAccelData[1];
        UHAccel[2] = UHGyroAccelData[2];
        return UHAccel;
    }

    public float[] readGyro()
    {
        UHGyro[0] = UHGyroAccelData[4];
        UHGyro[1] = UHGyroAccelData[5];
        UHGyro[2] = UHGyroAccelData[6];
        return UHGyro;
    }

    public void updateUH3DGyroAccel()
    {
        try
        {
            WriteLine("a");
            //Debug.Log ("Send a message a . ");
            string data = ReadLine();
            //Debug.Log (data );
            if (data != null && data.Contains("+"))
            {
                if (data.Split(new string[] { "+" }, System.StringSplitOptions.None).Length != 7)
                {
                    return;
                }
                UHGyroAccelData[0] = float.Parse(data.Split(new string[] { "+" }, System.StringSplitOptions.None)[0]);
                UHGyroAccelData[1] = float.Parse(data.Split(new string[] { "+" }, System.StringSplitOptions.None)[1]);
                UHGyroAccelData[2] = float.Parse(data.Split(new string[] { "+" }, System.StringSplitOptions.None)[2]);

                UHGyroAccelData[3] = float.Parse(data.Split(new string[] { "+" }, System.StringSplitOptions.None)[3]);

                UHGyroAccelData[4] = float.Parse(data.Split(new string[] { "+" }, System.StringSplitOptions.None)[4]);
                UHGyroAccelData[5] = float.Parse(data.Split(new string[] { "+" }, System.StringSplitOptions.None)[5]);
                UHGyroAccelData[6] = float.Parse(data.Split(new string[] { "+" }, System.StringSplitOptions.None)[6]);
            }
        }
        catch (TimeoutException)
        {
            //UHGyroAccelData[0] = 0.0f;
            //UHGyroAccelData[1] = 0.0f;
            //UHGyroAccelData[2] = 0.0f;
            //UHGyroAccelData[3] = 0.0f;
            //UHGyroAccelData[4] = 0.0f;
            //UHGyroAccelData[5] = 0.0f;
            //UHGyroAccelData[6] = 0.0f;
        }
    }
    //////////////////////////////////////////////////////////
    /// UPDATE Quaternion  via Serial Connection
    //////////////////////////////////////////////////////////
    public void updateQuaternion()
    {
        try
        {
            WriteLine("q");
            //Debug.Log("in Func readQuaternion:");
            string QuaData = ReadLine();

            //Debug.Log(QuaData);
            if (QuaData != null && QuaData.Contains("+"))
            {
                if (QuaData.Split(new string[] { "+" }, System.StringSplitOptions.None).Length != 4)
                {
                    return;
                }
                UHQuaternion[0] = float.Parse(QuaData.Split(new string[] { "+" }, System.StringSplitOptions.None)[0]);
                UHQuaternion[1] = float.Parse(QuaData.Split(new string[] { "+" }, System.StringSplitOptions.None)[1]);
                UHQuaternion[2] = float.Parse(QuaData.Split(new string[] { "+" }, System.StringSplitOptions.None)[2]);
                UHQuaternion[3] = float.Parse(QuaData.Split(new string[] { "+" }, System.StringSplitOptions.None)[3]);
            }
        }
        catch (Exception)
        {

        }

    }
    //////////////////////////////////////////////////////////
    /// READ  Quaternion  via Serial Connection
    //////////////////////////////////////////////////////////
    public void readQuaternion()
    {
        try
        {
            //Debug.Log("in Func readQuaternion:");
            string QuaData = ReadLine();

            //Debug.Log(QuaData);
            if (QuaData != null && QuaData.Contains("+"))
            {
                if (QuaData.Split(new string[] { "+" }, System.StringSplitOptions.None).Length != 4)
                {
                    return;
                }
                UHQuaternion[0] = float.Parse(QuaData.Split(new string[] { "+" }, System.StringSplitOptions.None)[1]);
                UHQuaternion[1] = float.Parse(QuaData.Split(new string[] { "+" }, System.StringSplitOptions.None)[0]);
                UHQuaternion[2] = float.Parse(QuaData.Split(new string[] { "+" }, System.StringSplitOptions.None)[2]);
                UHQuaternion[3] = float.Parse(QuaData.Split(new string[] { "+" }, System.StringSplitOptions.None)[3]);
            }
        }
        catch (Exception)
        {

        }
    }

    //////////////////////////////////////////////////////////
    /// RESET  Quaternion  via Serial Connection
    //////////////////////////////////////////////////////////
    public void resetQuaternion()
    {
        try
        {
            WriteLine("r");
        }
        catch (Exception)
        {

        }
    }

    //////////////////////////////////////////////////////////
    /// START  Quaternion  Process via Serial Connection
    //////////////////////////////////////////////////////////
    public void startQuaternion()
    {
        updateQuaternionFlg = true;
    }

    //////////////////////////////////////////////////////////
    /// STOP  Quaternion  Process via Serial Connection
    //////////////////////////////////////////////////////////
    public void stopQuaternion()
    {
        updateQuaternionFlg = false;
    }


    /////////////////////////////////////////////////////
    /// EMS(Electri Muscle Stimulation) functions to move the user's hand
    /////////////////////////////////////////////////////
    void stimulate(int padNum)
    {
        StartCoroutine(_stimulate(padNum));
    }

    public IEnumerator _stimulate(int padNum)
    {
        bool pastUpdateAnglePRFlg = updateAnglePRFlg;
        bool pastUpdateAngleTempFlg = updateAngleTempFlg;
        bool pastUpdatePhotoSensorsFlg = updatePhotoSensorsFlg;
        bool pastUpdateQuaternionFlg = updateQuaternionFlg;
        updateAnglePRFlg = false;
        updatePhotoSensorsFlg = false;
        updateQuaternionFlg = false;

        WriteLine(padNum.ToString());
        //Thread.Sleep(1000);
        yield return new WaitForSeconds(1f);

        updateAnglePRFlg = pastUpdateAnglePRFlg;
        updateAngleTempFlg = pastUpdateAngleTempFlg;
        updatePhotoSensorsFlg = pastUpdatePhotoSensorsFlg;
        updateQuaternionFlg = pastUpdateQuaternionFlg;
    }



    /////////////////////////////////////////////////////
    /// EMS(Electri Muscle Stimulation) functions to move the user's hand
    /////////////////////////////////////////////////////
    void stimulateSetSleepTime(int padNum, int sleepTimeMs)
    {
        StartCoroutine(_stimulateSetSleepTime(padNum, sleepTimeMs));
    }

    public IEnumerator _stimulateSetSleepTime(int padNum, int sleepTimeMs)
    {
        bool pastUpdateAnglePRFlg = updateAnglePRFlg;
        bool pastUpdateAngleTempFlg = updateAngleTempFlg;
        bool pastUpdatePhotoSensorsFlg = updatePhotoSensorsFlg;
        bool pastUpdateQuaternionFlg = updateQuaternionFlg;
        updateAnglePRFlg = false;
        updatePhotoSensorsFlg = false;
        updateQuaternionFlg = false;

        WriteLine(padNum.ToString());
        //Thread.Sleep(sleepTimeMs);
        yield return new WaitForSeconds (((float)sleepTimeMs/1000f));

        updateAnglePRFlg = pastUpdateAnglePRFlg;
        updateAngleTempFlg = pastUpdateAngleTempFlg;
        updatePhotoSensorsFlg = pastUpdatePhotoSensorsFlg;
        updateQuaternionFlg = pastUpdateQuaternionFlg;
    }

    //
    //オリジナル～
    //

    void stimulateMax(int padNum, int sleepTimeMs, int volt, int sharp)
    {
        StartCoroutine(_stimulateMax(padNum, sleepTimeMs, volt, sharp));
    }

    public IEnumerator _stimulateMax(int padNum, int sleepTimeMs, int volt, int sharp)
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

    /////////////////////////////////////////////////////
    /// temp
    /////////////////////////////////////////////////////
    public float readUHtemp()
    {
        float cels = 0.0f;
        string data = ReadLine();
        cels = float.Parse(data.Split(new string[] { " " }, System.StringSplitOptions.None)[3]);
        return cels;
    }
}