using System.Diagnostics.Contracts;
using PoeHUD.Models.Interfaces.Contracts;
using PoeHUD.Poe;

namespace PoeHUD.Models.Interfaces
{
    [ContractClass(typeof(Contract4IEntity))]
    public interface IEntity
    {
        string Path { get; }
        int Id { get; }
        long LongId { get; }

        bool IsValid { get; }

        bool IsHostile { get; }
        int Address { get; }
        bool HasComponent<T>() where T : Component, new();
        T GetComponent<T>() where T : Component, new();
    }
}