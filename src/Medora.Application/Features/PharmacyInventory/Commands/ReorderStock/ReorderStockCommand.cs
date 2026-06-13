using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.PharmacyInventory.Commands.ReorderStock;

public sealed record ReorderStockCommand(
    string PurchaseOrderNumber,
    string ItemName,
    string VendorName,
    int Quantity,
    decimal TotalAmount,
    DateOnly? ExpectedDate) : ICommand<CommandResult>;

public interface IReorderStockCommandHandler : ICommandHandler<ReorderStockCommand, CommandResult>;
