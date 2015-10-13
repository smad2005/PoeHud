using System;
using System.Diagnostics.Contracts;
using PoeHUD.Hud.Interfaces.Contracts;
using SharpDX;

namespace PoeHUD.Hud.Interfaces
{
    [ContractClass(typeof(Contract4IPanelChild))]
    public interface IPanelChild
    {
        Size2F Size { get; }

        Func<Vector2> StartDrawPointFunc { get; set; }

        Vector2 Margin { get; }
    }
}