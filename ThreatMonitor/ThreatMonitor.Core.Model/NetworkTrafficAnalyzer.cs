using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatMonitor.Core.Model
{
    public class NetworkTrafficAnalyzer
    {
        private readonly ITransformer _model;
        private readonly MLContext _context;

        public NetworkTrafficAnalyzer()
        {
            _context = new MLContext();
            _model = NetworkTrafficModel.LoadModel();
        }

        public bool Predict(NetworkTrafficData data)
        {
            var predictionEngine = _context.Model.CreatePredictionEngine<NetworkTrafficData, NetworkTrafficPrediction>(_model);
            var prediction = predictionEngine.Predict(data);
            return prediction.IsMalicious;
        }
    }
}
