using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoeHUD.Poe;

namespace PoeHUD.Models.Interfaces.Contracts
{
    [ContractClassFor(typeof(IEntity))]
    public abstract class Contract4IEntity:IEntity
    {
        public string Path { get; }
        public int Id { get; }
        public long LongId { get; }
        public bool IsValid { get; }
        public bool IsHostile { get; }
        public int Address { get; }
        public bool HasComponent<T>() where T : Component, new()
        {
            return true;
        }

        public T GetComponent<T>() where T : Component, new()
        {
            Contract.Ensures(Contract.Result<T>()!=null);
            return null;
        }
    }
}
