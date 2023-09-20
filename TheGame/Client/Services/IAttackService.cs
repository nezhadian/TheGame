using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Client.Services
{
    public interface IAttackService
    {
        public IList<AttackResault> Attacks { get; set; }
        public event Action OnChanged;

        Task<bool> Attack();
        Task GetLog(int battleId);
        Task GetMoreLog(int battleId);

    }
}
