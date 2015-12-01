namespace DevelopingTrends
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DroboInterface;

    using SensorHost.Shared;

    public class DroboSensor : SensorHost.Shared.SensorBase
    {
        public bool _connected = false;
        private Dictionary<string,IDrobo>  _drobos  = new Dictionary<string, IDrobo>();
        private readonly DroboConnection _connection;
        public DroboSensor()
        {
            this._connection = new DroboConnection(30000);
            this._connected =this._connection.Connect();
            this._connection.ConnectionLost += this.ConnectionOnConnectionLost;
            this._connection.DataRecieved += this.Connection_DataRecieved;
            Console.WriteLine($"Drobo is connected: {this._connected}");
        }

        private void ConnectionOnConnectionLost()
        {
            this._connected = false;
            this._drobos.Clear();
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

                if (this._drobos.ContainsKey(drobo.SerialNumber))
                {
                    this._drobos[drobo.SerialNumber] = drobo;
                }
                else
                {
                    this._drobos.Add(drobo.SerialNumber, drobo);
                }
            }
        }

        public override IEnumerable<Result> Results()
        {
            if (!this._connected)
            {
                this._connected = this._connection.Connect();
            }

            if (!this._drobos.Any())
            {
                return Enumerable.Empty<Result>();
            }
            //only going to return the first drobo found for this version.
            return this.ConvertToResult(this._drobos.First().Value);
        }

        public override string Description
        {
            get
            {
                if (this._drobos.Any())
                {
                    IDrobo firstDrobo = this._drobos.First().Value;
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


            double usedPercent =100- Math.Round((drobo.FreeCapacityProtected / drobo.TotalCapacityProtected) * 100,2);

            results.Add(new Result("Used protected space", usedPercent)
            {
                Unit = UnitTypes.Percent,
                Mode = SensorModes.Absolute,
                LimitMaxWarning = 75,
                LimitWarningMsg = "You have less than 25% protected space available",
                LimitMaxError = 90,
                LimitErrorMsg = "You are nearly out of protected space",
                LimitMode = true,
                Float = true
            });



            foreach (var drive in drobo.Drives)
            {
                results.AddRange(ResultForDrive(drive));
            }

            Result freeProtectedSpace = new Result("Free protected space", drobo.FreeCapacityProtected)
            {
                Unit =
                                               UnitTypes.BytesDisk,
                VolumeSize =
                                               SensorSpeedSize
                                               .MegaByte,
            };
         
            results.Add(freeProtectedSpace);

            Result totalCapacity = new Result("Total Capacity", drobo.TotalCapacityProtected)
                                       {
                                           Unit =
                                               UnitTypes.BytesDisk,
                                           VolumeSize =
                                               SensorSpeedSize
                                               .MegaByte,                                        
                                       };
        
            results.Add(totalCapacity);            
            return results;
        }

        private static Result[] ResultForDrive(DroboDrive drive)
        {
            Result result1 = new Result($"Drive {drive.SlotNumber + 1} Status", drive.Status)
                                 {
                                     ValueLookup =
                                         "developingtrends.drobosensor.drivestatus"
                                 };


            Result result2 = new Result($"Drive {drive.SlotNumber + 1} Capacity", drive.PhysicalCapcity)
                                 {
                                  
                                     VolumeSize =
                                         SensorSpeedSize
                                         .GigaByte,
                                     Unit =
                                         UnitTypes.BytesDisk                                         ,
                                     ShowTable =
                                         false
                                 };
            // BytesToResult(result2);

            return new[] { result1, result2 };
        }


        public static void BytesToResult( Result result)
        {
            double val = result.Value;
            string[] array = new string[]
            {
        "B",
        "KB",
        "MB",
        "GB"
            };
            int num = 0;
            while (val >= 1000.0 && num + 1 < array.Length)
            {
                num++;
                val /= 1000.0;
            }
            result.Value = Math.Round(val,2);
            switch (num)
            {
                case 0:
                    result.VolumeSize = SensorSpeedSize.Byte;
                    break;
                case 1:
                    result.VolumeSize = SensorSpeedSize.KiloByte;
                    break;
                case 2:
                    result.VolumeSize = SensorSpeedSize.MegaByte;
                    break;
                case 3:
                    result.VolumeSize = SensorSpeedSize.GigaByte;
                    break;
            }
        }
        public static string BytesToHDSizeString(double val)
        {
            string[] array = new string[]
            {
        "B",
        "KB",
        "MB",
        "GB",
        "TB",
        "PB"
            };
            int num = 0;
            while (val >= 1000.0 && num + 1 < array.Length)
            {
                num++;
                val /= 1000.0;
            }
            return string.Format("{0:0} {1}", val, array[num]);
        }
    }
}
