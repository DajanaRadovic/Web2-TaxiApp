﻿using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entity
{
    public class DriveEntity : TableEntity
    {
        public string FromLocation { get; set; }

        public string ToLocation { get; set; }

        public Guid IdRider { get; set; }

        public Guid IdDriver { get; set; }

        public double Price { get; set; }

        public bool Accepted { get; set; }

        public Guid IdDrive { get; set; }

        public int TimeToDriverArrivalSeconds { get; set; }

        public int TimeToEndTripInSeconds { get; set; }

        public bool IsFinished { get; set; }

        public bool IsRated { get; set; }

        public DriveEntity() { }

        public DriveEntity(Guid userId, Guid idDriver, Guid idDrive, string fromLocation, string toLocation, double price, bool accepted, bool isFinished, bool isRated, int minutes, Guid tId)
        {
            IdRider = userId;
            IdDriver = idDriver;
            IdDrive = idDrive;
            FromLocation = fromLocation;
            ToLocation = toLocation;
            Price = price;
            Accepted = accepted;
            TimeToDriverArrivalSeconds = minutes;
            TimeToEndTripInSeconds = 0;
            IsFinished = isFinished;
            IsRated = isRated;
            RowKey = tId.ToString();
            PartitionKey = tId.ToString();
        }
    }
}
