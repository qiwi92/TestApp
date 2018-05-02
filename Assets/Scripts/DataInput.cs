using System;
using UnityEngine;
using System.IO.Ports;

public class DataInput : MonoBehaviour
{
    [SerializeField] private HistogramView _histogramView;

    private SerialPort _mySerialPort;
    private string[] _portNames;
    private float _timer = 0f;

    private float[,] _arrayData;

    private bool _test = false;

    void Start ()
	{
        _arrayData = new float[SensorData.Width, SensorData.Height];

	    _portNames = SerialPort.GetPortNames();

	    foreach (var port in _portNames)
	    {
	        Debug.Log("Port Name: " +port);
	    }

	    _mySerialPort = new SerialPort()
	    {
	        BaudRate = 9600,
	        Parity = Parity.None,
            StopBits = StopBits.One,
            DataBits = 8,
            Handshake = Handshake.None,
            RtsEnable = true
        };




        _mySerialPort.Open();

        _mySerialPort.DataReceived += DataReceivedHandler;

        

	    _histogramView.Setup(64,23,34);


	}


    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > 0.1f)
        {

            var indata = _mySerialPort.ReadLine();
            indata = indata.Trim('?');

            var data = indata.Split(',');
            var trimmedData = SubArray(data, 1, 64);

            var numberData = Array.ConvertAll(trimmedData, float.Parse);

            _histogramView.SetValues(numberData);

            


            for (var y = 0; y < SensorData.Height; y++)
            {
                for (var x = 0; x < SensorData.Width; x++)
                {
                    _arrayData[x,y] = numberData[y + SensorData.Height*x];
                }
            }

            Hello();

            _timer = 0;
        }
    }


    private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
    {
        string indata = _mySerialPort.ReadLine();
        _test = true;
    }

    public void Hello()
    {
        Debug.Log("Data Received:" + _test );
    }


    private T[] SubArray<T>(T[] data, int index, int length)
    {
        T[] result = new T[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }



}
