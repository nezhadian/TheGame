﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Client.Services
{
    public interface IBattleService
    {
        public int CurrentBattleId { get; set; }
        public bool IsUserInBattle { get; set; }

        public event Action OnChanged;

        Task GetCurrentBattleId();
        Task StartBattleAsync(int opponentId);

    }
}

