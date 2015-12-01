using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroboSensor
{
    using DevelopingTrends.DroboInterface;

    using SensorHost.Shared;
    public class DroboSensor : SensorHost.Shared.SensorBase
    {
        public bool _connected = false;
        private Dictionary<string,IDrobo>  _drobos  = new Dictionary<string, IDrobo>();
        private readonly DroboConnection _connection;
        public DroboSensor()
        {
            _connection = new DroboConnection(30000);
            _connected =_connection.Connect();
            _connection.ConnectionLost += ConnectionOnConnectionLost;
            _connection.DataRecieved += Connection_DataRecieved;
            Console.WriteLine($"Drobo is connected: {_connected}");
        }

        private void ConnectionOnConnectionLost()
        {
            _connected = false;
            _drobos.Clear();
            Console.WriteLine("Connection Lost");
        }

        private void Connection_DataRecieved(IDrobo drobo)
        {
            Console.WriteLine("Drobo data received");
            if (drobo is DroboError)
            {
                Console.WriteLine("Drobo Error");
            }
            else
            {

                if (_drobos.ContainsKey(drobo.NameOfDrobo))
                {
                    _drobos[drobo.NameOfDrobo] = drobo;
                }
                else
                {
                    _drobos.Add(drobo.NameOfDrobo, drobo);
                }
            }
        }

        public override IEnumerable<Result> Results()
        {
            if (!_connected)
            {
                _connected = _connection.Connect();
            }

            if (!_drobos.Any())
            {
                return Enumerable.Empty<Result>();
            }
            //only going to return the first drobo found for this version.
            return ConvertToResult(_drobos.First().Value);
        }

        public override string Description
        {
            get
            {
                if (_drobos.Any())
                {
                    IDrobo firstDrobo = _drobos.First().Value;
                    switch (firstDrobo.Status)
                    {
                        case ConnectionStatus.TimeOut:
                            return "The drobo service could not locate a drobo";
                            break;
                        case ConnectionStatus.Error:
                            return "An unhandled error has occured";

                            break;
                        case ConnectionStatus.DroboServiceNotListening:
                            return "The drobo dashboard service is not running - please check it is installed";

                            break;
                        case ConnectionStatus.InvalidResponse:
                            return "The XML returned is not understood";
                            break;                        
                    }

                    return firstDrobo.NameOfDrobo;
                }
                return string.Empty;
            }
        }

        private IEnumerable<Result> ConvertToResult(IDrobo drobo)
        {
            List<Result> results = new List<Result>();      

            foreach (var drive in drobo.Drives)
            {
                results.Add(ResultForDrive(drive));
            }


            results.Add(new Result("Free capacity protected", drobo.FreeCapacityProtected)
                            {
                                Unit = UnitTypes.BytesDisk,
                                Mode = SensorModes.Absolute,
                VolumeSize = SensorSpeedSize.GigaByte,
                Data= DataType.Percent,
                Maximum = drobo.TotalCapacityProtected
            });
            results.Add(new Result("Total Capacity", drobo.UsedCapacityProtected)
                            {
                                Unit = UnitTypes.BytesDisk,
                                VolumeSize = SensorSpeedSize.GigaByte,
                                
                                
                            });            
            return results;
        }

        private static Result ResultForDrive(DroboDrive drive)
        {
            Result result = new Result($"Drive {drive.SlotNumber} ({drive.PhysicalCapcity})", drive.Status)
                                {   
                                       ValueLookup        = "developingtrends.drobosensor.drivestatus"
            };

            return result;
        }
    }
}
