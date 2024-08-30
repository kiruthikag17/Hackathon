using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace ThreatMonitor.Core.Model
{
    public class NetworkTrafficModel
    {
        private static readonly string DataPath = @"C:\Users\sesa527952\Desktop\Hackathon\Hackathon\threat-intell\src\services\ThreatMonitor\network_traffic_data.csv";
        private static readonly string ModelPath = "network_traffic_model.zip";

        public static void TrainModel()
        {
            var context = new MLContext();

            // Load data
            var data = context.Data.LoadFromTextFile<NetworkTrafficData>(DataPath, hasHeader: true, separatorChar: ',');

            // Split data into training and test sets
            var splitData = context.Data.TrainTestSplit(data, testFraction: 0.2);

            // Define data preparation and training pipeline
            var pipeline = context.Transforms.Conversion.MapValueToKey("Label", nameof(NetworkTrafficData.IsMalicious))
                .Append(context.Transforms.Concatenate("Features", nameof(NetworkTrafficData.SourceIp), nameof(NetworkTrafficData.DestinationIp), nameof(NetworkTrafficData.SourcePort), nameof(NetworkTrafficData.DestinationPort), nameof(NetworkTrafficData.PacketLength), nameof(NetworkTrafficData.ProtocolType), nameof(NetworkTrafficData.TimeToLive)))
                .Append(context.Transforms.NormalizeMinMax("Features"))
                .Append(context.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(context.Transforms.Conversion.MapKeyToValue("PredictedLabel", "Label"));

            // Train the model
            var model = pipeline.Fit(splitData.TrainSet);

            // Evaluate the model
            var predictions = model.Transform(splitData.TestSet);
            var metrics = context.MulticlassClassification.Evaluate(predictions);

            Console.WriteLine($"Log-loss: {metrics.LogLoss}");

            // Save the model
            context.Model.Save(model, data.Schema, ModelPath);
        }

        public static ITransformer LoadModel()
        {
            var context = new MLContext();
            return context.Model.Load(ModelPath, out _);
        }
    }
}