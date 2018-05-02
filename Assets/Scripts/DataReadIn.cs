using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using UnityEngine;

public class DataReadIn
{
    private static SerialPort _mySerialPort;
    private static string[] _portNames;
    private static bool _readingData = false;

    public static event Action<IReadOnlyList<float>> DataReceived;

    public static void StartReadIn()
    {
        _portNames = SerialPort.GetPortNames();

        foreach (var port in _portNames)
        {
            Debug.Log("Port Name: " + port);
        }

        var newThread = new Thread(ReadData);
        newThread.Start();
    }

    public static void StopReadIn()
    {
        _readingData = false;
    }

    private static void ReadData()
    {
        _readingData = true;

        using (_mySerialPort = new SerialPort())
        {
            _mySerialPort.PortName = _portNames[0];
            _mySerialPort.BaudRate = 9600;
            _mySerialPort.Parity = Parity.None;
            _mySerialPort.StopBits = StopBits.One;
            _mySerialPort.DataBits = 8;
            _mySerialPort.Handshake = Handshake.None;
            _mySerialPort.RtsEnable = true;


            _mySerialPort.Open();

            //To avoid incomplete first read in
            _mySerialPort.ReadLine();

            while (_readingData)
            {
                var indata = _mySerialPort.ReadLine();
                indata = indata.Trim('?');

                var dataSplited = indata.Split(',');
                var numberData = Array.ConvertAll(dataSplited, float.Parse);

                DataReceived?.Invoke(numberData);
            }
        }
    }

}