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

        Task Fight();
        Task GetLog(int battleId);

    }
}
