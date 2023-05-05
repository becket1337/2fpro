using _2fpro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.Service.Interface
{
    public interface IYaMarketRepo
    {
        Task YamExport();
        Task UpdateProfile(YaMakret yam);
        Task CreateProfile(YaMakret yam);
        Task<List<YaMakret>> Get();
        Task<YaMakret> GetMarketProfile();
    }
}
