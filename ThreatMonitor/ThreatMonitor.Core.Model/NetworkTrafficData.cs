using Microsoft.ML.Data;

namespace ThreatMonitor.Core.Model
{
    public class NetworkTrafficData
    {
        [LoadColumn(0)]
        public float SourceIp { get; set; }

        [LoadColumn(1)]
        public float DestinationIp { get; set; }

        [LoadColumn(2)]
        public float SourcePort { get; set; }

        [LoadColumn(3)]
        public float DestinationPort { get; set; }

        [LoadColumn(4)]
        public float PacketLength { get; set; }

        [LoadColumn(5)]
        public float ProtocolType { get; set; }

        [LoadColumn(6)]
        public float TimeToLive { get; set; }

        [LoadColumn(7)]
        public int IsMalicious { get; set; }
    }

    public class NetworkTrafficPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsMalicious { get; set; }
    }
}
