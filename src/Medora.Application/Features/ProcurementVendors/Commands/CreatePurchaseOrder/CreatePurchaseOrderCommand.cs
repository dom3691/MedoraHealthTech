using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.ProcurementVendors.Commands.CreatePurchaseOrder;

public sealed record CreatePurchaseOrderCommand(
    string PurchaseOrderNumber,
    string ItemName,
    string VendorName,
    int Quantity,
    decimal TotalAmount,
    DateOnly? ExpectedDate) : ICommand<CommandResult>;

public interface ICreatePurchaseOrderCommandHandler : ICommandHandler<CreatePurchaseOrderCommand, CommandResult>;
