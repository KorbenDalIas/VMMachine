CREATE TABLE [dbo].[VendingMachineChangePurse] (
    [Id]            INT IDENTITY (1, 1) NOT NULL,
    [NominalValue]  INT NOT NULL,
    [NumberOfBills] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

