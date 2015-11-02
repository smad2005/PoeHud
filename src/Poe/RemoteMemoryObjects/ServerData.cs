namespace PoeHUD.Poe.RemoteMemoryObjects
{
    public class ServerData : RemoteMemoryObject
    {
        public bool IsInGame
        {
            get
            {
                return M.ReadInt(Address + Offsets.InGameOffset) == 3; 
            }
        }

        public InventoryList PlayerInventories
        {
            get { return base.GetObject<InventoryList>(Address + 10496); }
        }
    }
}