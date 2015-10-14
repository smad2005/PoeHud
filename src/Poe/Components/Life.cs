using System.Collections.Generic;
using System.Linq;
using PoeHUD.Poe.RemoteMemoryObjects;

namespace PoeHUD.Poe.Components
{
    public class Life : Component
    {
        public int MaxHP
        {
            get
            {
                if (Address != 0)
                {
                    return M.ReadInt(Address + 0x30);
                }
                return 1;
            }
        }

        public int CurHP
        {
            get
            {
                if (Address != 0)
                {
                    return M.ReadInt(Address + 0x34);
                }
                return 1;
            }
        }

        public int ReservedHP
        {
            get
            {
                if (Address != 0)
                {
                    return M.ReadInt(Address + 0x3C);
                }
                return 0;
            }
        }

        public int MaxMana
        {
            get
            {
                if (Address != 0)
                {
                    return M.ReadInt(Address + 0x54);
                }
                return 1;
            }
        }

        public int CurMana
        {
            get
            {
                if (Address != 0)
                {
                    return M.ReadInt(Address + 0x58);
                }
                return 1;
            }
        }

        public int ReservedMana
        {
            get
            {
                if (Address != 0)
                {
                    return M.ReadInt(Address + 0x60);
                }
                return 0;
            }
        }

        public int MaxES
        {
            get
            {
                if (Address != 0)
                {
                    return M.ReadInt(Address + 0x78);
                }
                return 0;
            }
        }

        public int CurES
        {
            get
            {
                if (Address != 0)
                {
                    return M.ReadInt(Address + 0x7c);
                }
                return 0;
            }
        }

        public float HPPercentage
        {
            get { return CurHP/(float) (MaxHP - ReservedHP); }
        }

        public float MPPercentage
        {
            get { return CurMana/(float) (MaxMana - ReservedMana); }
        }

        public float ESPercentage
        {
            get
            {
                if (MaxES != 0)
                {
                    return CurES/(float) MaxES;
                }
                return 0f;
            }
        }

        public bool CorpseUsable
        {
            get { return M.ReadByte(Address + 212) == 1; }
        }

        public List<Buff> Buffs
        {
            get
            {
                var list = new List<Buff>();
                int start = M.ReadInt(Address + 0x98);
                int end = M.ReadInt(Address + 0x9C);
                int count = (end - start)/4;
                if (count <= 0 || count > 32)
                {
                    return list;
                }
                for (int i = 0; i < count; i++)
                {
                    list.Add(base.ReadObject<Buff>(M.ReadInt(start + i*4) + 4));
                }
                return list;
            }
        }

        public bool HasBuff(string buff)
        {
            return Buffs.Exists((Buff x) => x.Name == buff);
        }
    }
}