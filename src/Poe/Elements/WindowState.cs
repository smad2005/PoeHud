﻿namespace PoeHUD.Poe.UI.Elements

{
    public class WindowState : Element
    {
        public new bool IsVisibleLocal
        {
            get
            {
                return M.ReadInt(this.Address + 0x860) == 1;
            }
        }
    }
}
