using Common.Entity;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mapper
{
    public class DriveMapper
    {
        public DriveMapper() { }

        public static Drive MapDriveEntity(DriveEntity drive)
        {
            return new Drive(drive.IdRider, drive.IdDriver, drive.IdDrive, drive.FromLocation, drive.ToLocation, drive.Price, drive.Accepted, drive.TimeToDriverArrivalSeconds, drive.TimeToEndTripInSeconds, drive.IsFinished, drive.IsRated);
        }
    }
}
