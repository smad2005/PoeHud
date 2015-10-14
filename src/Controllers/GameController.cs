using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using PoeHUD.Framework;
using PoeHUD.Models;
using PoeHUD.Poe;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.RemoteMemoryObjects;
using PoeHUD.Poe.UI;

namespace PoeHUD.Controllers
{
    public class GameController
    {
        public GameController(Memory memory)
        {
            Contract.Requires(memory != null);
            Memory = memory;
            Area = new AreaController(this);
            EntityListWrapper = new EntityListWrapper(this);
            Window = new GameWindow(memory.Process);
            Game = new TheGame(memory);
            Files = new FsController(memory);
        }
        public EntityListWrapper EntityListWrapper { get; private set; }
        public GameWindow Window { get; private set; }
        public TheGame Game { get; private set; }
        public AreaController Area { get; private set; }

        public Memory Memory { get; private set; }

        public IEnumerable<EntityWrapper> Entities
        {
            get
            {
                return EntityListWrapper.Entities;
            }
        }

        public EntityWrapper Player
        {
            get
            {
                Contract.Requires(EntityListWrapper != null);
                return EntityListWrapper.Player;
            }
        }

        public bool InGame
        {
            get
            {
                Contract.Requires(Game != null);
                return Game.IngameState.InGame;
            }
        }

        public FsController Files { get; private set; }

        public void RefreshState()
        {
            if (InGame)
            {
                EntityListWrapper.RefreshState();
                Area.RefreshState();
            }
        }

        public List<EntityWrapper> GetAllPlayerMinions()
        {
            Contract.Requires(Entities != null);
            return Entities.Where(x => x.HasComponent<Player>()).SelectMany(c => c.Minions).ToList();
        }

    }
}