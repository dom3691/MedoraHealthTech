using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;

namespace Medora.Application.Features.PharmacyInventory.Commands.AddInventoryItem;

public sealed record AddInventoryItemCommand(
    string Sku,
    string Name,
    string Category,
    int QuantityOnHand,
    int ReorderLevel,
    string Unit,
    DateOnly? ExpiryDate) : ICommand<CommandResult>;

public interface IAddInventoryItemCommandHandler : ICommandHandler<AddInventoryItemCommand, CommandResult>;
