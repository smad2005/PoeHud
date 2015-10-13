using System;
using System.Diagnostics.Contracts;
using SharpDX;

namespace PoeHUD.Hud.Interfaces.Contracts
{
    [ContractClassFor(typeof(IPanelChild))]
    public abstract class Contract4IPanelChild : IPanelChild
    {
        public Size2F Size { get; }
       

        public Func<Vector2> StartDrawPointFunc
        {
            get
            {
                Contract.Ensures(Contract.Result<System.Func<SharpDX.Vector2>>() != null);
                return null;
            }
            set
            {
                
            }

        }
        public Vector2 Margin { get; }
    }
}