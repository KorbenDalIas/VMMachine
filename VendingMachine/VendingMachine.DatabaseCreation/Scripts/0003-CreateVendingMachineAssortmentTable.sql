CREATE TABLE [dbo].[VendingMachineAssortment] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [ProductName]   NVARCHAR (50) NOT NULL,
    [ProductCost]   INT           NOT NULL,
    [ProductAmount] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);