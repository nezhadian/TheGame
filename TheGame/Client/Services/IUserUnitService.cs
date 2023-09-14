using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGame.Shared;

namespace TheGame.Client.Services
{
    public interface IUserUnitService
    {
        public IList<Unit> Units { get; set; }
        public IList<UserUnit> UserUnits { get; set; }

        Task GetUnitsAsync();
        Task GetUserUnitsAsync();
        Task ReviveAllUnitsAsync();

        IList<UserUnit> GetCurrentUnits(int unitId);
    }
}
