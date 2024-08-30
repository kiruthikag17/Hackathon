using System.Net.Sockets;
using PacketDotNet;
using SharpPcap;
using ThreatMonitor.Core.Model;

namespace ThreatMonitor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Train the model (only needs to be done once)
            NetworkTrafficModel.TrainModel();

            // List available devices
            var devices = CaptureDeviceList.Instance;
            if (devices.Count < 1)
            {
                Console.WriteLine("No devices were found on this machine");
                return;
            }

            // Select the first device
            var device = devices[0];
            Console.WriteLine($"Listening on {device.Description}");

            // Open the device for capturing
            device.OnPacketArrival += new  PacketArrivalEventHandler(OnPacketArrival);  
            //device.Open(DeviceMode.Promiscuous, 1000);

            // Start capturing packets
            device.StartCapture();
            Console.WriteLine("Press Enter to stop...");
            Console.ReadLine();

            // Stop capturing packets
            device.StopCapture();
            device.Close();
        }

        private static void OnPacketArrival(object sender, PacketCapture e)
        {
            var packet = e.GetPacket();
            //var ipPacket = packet.Extract<IPPacket>();
        }

        //private static void OnPacketArrival(object sender, CaptureEventArgs e)
        //{
        //    var packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
        //    var ipPacket = packet.Extract<IPPacket>();

        //    if (ipPacket != null)
        //    {
        //        var srcIp = ipPacket.SourceAddress.ToString();
        //        var dstIp = ipPacket.DestinationAddress.ToString();
        //        var packetLength = ipPacket.TotalLength;

        //        // Convert IP addresses to numerical values for simplicity
        //        var srcIpNumeric = BitConverter.ToUInt32(ipPacket.SourceAddress.GetAddressBytes(), 0);
        //        var dstIpNumeric = BitConverter.ToUInt32(ipPacket.DestinationAddress.GetAddressBytes(), 0);

        //        var data = new NetworkTrafficData
        //        {
        //            SourceIp = srcIpNumeric,
        //            DestinationIp = dstIpNumeric,
        //            PacketLength = packetLength
        //        };

        //        var analyzer = new NetworkTrafficAnalyzer();
        //        var isMalicious = analyzer.Predict(data);

        //        if (isMalicious)
        //        {
        //            Console.WriteLine($"Malicious traffic detected from IP: {srcIp} to {dstIp}");
        //        }
        //    }
        //}
    }
}
