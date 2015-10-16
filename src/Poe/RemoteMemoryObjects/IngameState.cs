using System;
using System.Linq;
using PoeHUD.Models.Enums;
using PoeHUD.Poe.UI;

namespace PoeHUD.Poe.RemoteMemoryObjects
{
    public class IngameState : RemoteMemoryObject
    {
        public Camera Camera
        {
            get { return base.GetObject<Camera>(Address + 0x15F0 + Offsets.IgsOffsetDelta); }
        }
        public IngameData Data
        {
            get { return base.ReadObject<IngameData>(Address + 0x13C + Offsets.IgsOffset); }
        }

        public bool InGame
        {
            get
            {
                return M.ReadInt(Address + 0x13C + Offsets.IgsOffset) != 0 && ServerData.IsInGame;
            }
        }

        public ServerData ServerData
        {
            get { return base.ReadObjectAt<ServerData>(0x140 + Offsets.IgsOffset); }
        }

        public IngameUIElements IngameUi
        {
            get { return base.ReadObjectAt<IngameUIElements>(0x5EC + Offsets.IgsOffset); }
        }

        public Element UIRoot
        {
            get { return base.ReadObjectAt<Element>(0xC10 + Offsets.IgsOffset); }
        }

        public Element UIHover
        {
            get { return base.ReadObjectAt<Element>(0xC24 + Offsets.IgsOffset); }
        }

        public int EntityLabelMap
        {
            get { return M.ReadInt(Address + 68, 2528); } //todo deprecated maybe need to remove
        }


        public DiagnosticInfoType DiagnosticInfoType
        {
            get { return (DiagnosticInfoType)M.ReadInt(Address + 0xC90); }
        }

        public DiagnosticElement LatencyRectangle
        {
            get { return GetObjectAt<DiagnosticElement>(0xEB0); }
        }

        public DiagnosticElement FrameTimeRectangle
        {
            get { return GetObjectAt<DiagnosticElement>(0x1310); }
        }

        public DiagnosticElement FPSRectangle
        {
            get { return GetObjectAt<DiagnosticElement>(0x1540); }
        }

        /// <summary>
        /// Latency in ms
        /// </summary>
        public float CurLatency
        {
            get { return M.ReadFloat(Address + 0xCA0); }
        }

        /// <summary>
        /// Frame time in ms
        /// </summary>
        public float CurFrameTime
        {
            get { return M.ReadFloat(Address + 0x1100); }
        }


        public float CurFps
        {
            get { return M.ReadFloat(Address + 0x1370); }
        }

        /// <summary>
        /// How much time client is running
        /// </summary>
        public TimeSpan TimeInGame
        {
            get { return TimeSpan.FromMilliseconds(M.ReadInt(Address + 0xc80)); }
        }

    }
}