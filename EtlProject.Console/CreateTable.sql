CREATE TABLE Trips (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    PickupDateTimeUtc DATETIME2 NOT NULL,
    DropoffDateTimeUtc DATETIME2 NOT NULL,
    PassengerCount INT NOT NULL,
    TripDistance DECIMAL(8,2) NOT NULL,
    StoreAndFwdFlag NVARCHAR(3) NOT NULL CHECK (StoreAndFwdFlag IN ('Yes', 'No')),
    PULocationID INT NOT NULL,
    DOLocationID INT NOT NULL,
    FareAmount DECIMAL(8,2) NOT NULL,
    TipAmount DECIMAL(8,2) NOT NULL,
    TravelTime AS DATEDIFF(SECOND, PickupDateTimeUtc, DropoffDateTimeUtc),
    
    INDEX IX_PULocationID_TipAmount (PULocationID, TipAmount),
    INDEX IX_TripDistance (TripDistance DESC),
    INDEX IX_TravelTime (TravelTime DESC),
    INDEX IX_PickupDateTimeUtc (PickupDateTimeUtc)
);