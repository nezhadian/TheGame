using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheGame.Client.Services
{
    public interface IShopService
    {
        public int Costs { get; set; }
        public event Action OnChanged;
        Task GetCostsAsync();
    }
}
