using System.Reflection.Metadata;
using System.Security.Cryptography;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iDotMatrixNET.Classes;

namespace iDotMatrixNET
{
    public class iDotMatrixNet
    {
        BluetoothLENet.BLE bluetoothLENet;

        public iDotMatrixNet(string deviceName)
        {
            bluetoothLENet = new BluetoothLENet.BLE();
            DeviceName = deviceName;
        }

        public string DeviceName { get; private set; }

        public async Task<BluetoothLENet.ConnectDeviceResult> Connect()
        {          
            bluetoothLENet.StartScanning();
            var rst = await bluetoothLENet.ConnectDevice(DeviceName);
            
            if (rst != BluetoothLENet.ConnectDeviceResult.Ok) 
                throw new Exception();

            return rst;
        }

        //public void Disconnect()
        //{
        //    bluetoothLENet.DisconnectDevice();
        //}

        public async Task TurnOn()
        {
            string payload = "05 00 07 01 01";
            await Write(payload);
        }

        public async Task TurnOff()
        {
            string payload = "05 00 07 01 00";
            await Write(payload);
        }

        public async Task SetPixel(byte brightness, Color color)
        {
            string pixel_x = "00";
            string pixel_y = "00";
            string R = $"{color.Red:X}";
            string G = $"{color.Green:X}";
            string B = $"{color.Blue:X}";

            string payload = $"0a 00 05 01 00 {R} {G} {B} {pixel_x} {pixel_y}";

            await Write(payload);
        }


        public async Task SetPng(string pngPath)
        {
            var image = new Image();
            var bytes = image.LoadPng(pngPath);

            var payloads = image.CreatePayloads(bytes);
            await Write("05 00 04 01 01");

            foreach (var payload in payloads)
            {
                await Write(payload);
            }
        }

        public async Task SetGif(string gifPath)
        {
            var gif = new Gif();
            var bytes = gif.LoadGif(gifPath);

            var payloads = gif.CreatePayloads(bytes);
            //await Write("05 00 04 01 01");

            foreach (var payload in payloads)
            {
                await Write(payload);
            }
        }

        public string BytesToString(byte[] bytes)
        {
            return string.Join(' ', bytes.Select(b => $"{b:X}"));
        }

        public async Task Write(byte[] payload)
        {
            var  payload_string = BytesToString(payload);           
            await Write(payload_string);
        }

        public async Task Write(string payload)
        {
            var service = "250";
            var caracteristic = "64002";

            var rst = await bluetoothLENet.WriteCharacteristic(service, caracteristic, payload);
            if (rst != BluetoothLENet.BLE.WriteCharacteristicResult.Write_Success) throw new Exception();
        }
    }
}