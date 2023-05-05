using _2fpro.Data;
using _2fpro.Models;
using _2fpro.ViewModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.Service.Repository
{
    public class DiagnosticService
    : IDiagnosticService
    {
        private ApplicationDbContext _db;

        public DiagnosticService(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<Diagnostic> Diagnostics { get { return _db.Diagnostics; } }

        public async Task<Diagnostic> GetInfo()
        {
            return await _db.Diagnostics.FindAsync(1);
        }
        public async Task<List<Diagnostic>> GetAllAsync()
        {
            return await _db.Diagnostics.ToListAsync();
        }
        public async Task<List<Diagnostic>> GetAllOnlineAsync()
        {
            //foreach(var item in await _db.Diagnostics.ToListAsync())
            //{
            //    var lastTimeVisit = DateTime.Now - item.CurrDateTime;
            //    if (lastTimeVisit.TotalSeconds > 60)
            //    {
            //        item.IsActivate = false;
            //        item.CustomIntField = 0;
            //    }

            //}
            //await _db.SaveChangesAsync();

            return await _db.Diagnostics.Where(x => getLastTimeVisit(x).TotalSeconds < 60 && !x.CustomBoolField && !string.IsNullOrEmpty(x.ConnectionID)).ToListAsync();
        }
        public TimeSpan getLastTimeVisit(Diagnostic diag)
        {
            TimeSpan lastTimeVisit = DateTime.Now - diag.CurrDateTime;
            return lastTimeVisit;
        }
        public async Task<Diagnostic> GetAsync(string username)
        {
            var item = await _db.Diagnostics.FirstOrDefaultAsync(x => x.CustomStringField == username);
            return item;
        }
        public async Task<Diagnostic> GetAsyncById(int id)
        {
            var item = await _db.Diagnostics.FirstOrDefaultAsync(x => x.ID == id);
            return item;
        }
        public async Task DiagUpdate(Diagnostic diag)
        {
            if (diag != null)
            {
                await _db.Diagnostics.AddAsync(diag);
                await _db.SaveChangesAsync();
            }

        }
        public async Task<string> EditConnId(Diagnostic diag, string signalrUserId = null, bool isActive = true, bool ismobile = false)
        {
            string connSiguserid = null;
            var item = await _db.Diagnostics.FirstOrDefaultAsync(x => x.CustomStringField == diag.CustomStringField);
            if (item != null)
            {
                connSiguserid = signalrUserId == null ? item.ConnectionID : signalrUserId;
                item.ConnectionID = connSiguserid;
                item.CurrDateTime = DateTime.Now;
                item.IsActivate = isActive;
                if (isActive)
                {//только зашел на сайт - Isactive true, узнаем мобильный браузер и ставим индикатор "первого захода" на 1
                    item.IsMobileConn = ismobile;

                }
                else
                {
                    item.CustomIntField = 0;
                    item.IsActivate = false;
                }// IsACtive false - пользователь не отзывается, сбрасываем индикатор "первого захода" на 0
                await _db.SaveChangesAsync();
            }
            return connSiguserid;
        }
        public async Task FirstTimeVisiter(int id)
        {
            if (id != 0)
            {
                var diag = await _db.Diagnostics.FindAsync(id);

                diag.CustomIntField = 1;
                await _db.SaveChangesAsync();

            }
        }
        public async Task UpdateCurrDateTime(int id)
        {
            if (id != 0)
            {
                var diag = await _db.Diagnostics.FindAsync(id);

                diag.CurrDateTime = DateTime.Now;
                await _db.SaveChangesAsync();

            }
        }
        public async Task EditAdminOnlineStatus(Diagnostic diag)
        {
            var item = await _db.Diagnostics.FirstOrDefaultAsync(x => x.CustomBoolField);
            if (item != null)
            {
                item.CustomIntField = 1;
                await _db.SaveChangesAsync();
            }
        }
        public async Task DelFromMigrate()
        {
            var item = await _db.Diagnostics.FirstOrDefaultAsync(x => x.ID == 1);
            if (item != null)
            {
                _db.Diagnostics.Remove(item);
                await _db.SaveChangesAsync();
            }
        }

        public async Task DiagRemove(Diagnostic diag)
        {
            _db.Diagnostics.Remove(diag);
            await _db.SaveChangesAsync();
        }
        public async Task DiagRangeRemove(IEnumerable<Diagnostic> diag)
        {
            _db.Diagnostics.RemoveRange(diag);
            await _db.SaveChangesAsync();
        }
        public async Task DelHistory()
        {
            if (await _db.Diagnostics.AnyAsync())
                await _db.Database.ExecuteSqlCommandAsync("truncate table [Diagnostics]");

        }
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
        public IQueryable<Diagnostic> UsersPerDay(int lastDayVisit = 0)
        {

            var list = _db.Diagnostics.Where(x => string.IsNullOrEmpty(x.ConnectionID) || string.IsNullOrEmpty(x.ConnectionID)).Where(x => (DateTime.Now - x.CurrDateTime).TotalMinutes > 2 && (DateTime.Now.Date - x.CurrDateTime.Date).Days == lastDayVisit);
            var DupesList = list.GroupBy(x => x.IpAddress).Select(g => new { Key = g.Key, Items = g.OrderBy(x => x.CurrDateTime) }).Select(x => x.Items.FirstOrDefault());
            return DupesList;
        }
        public async Task<List<Diagnostic>> UsersPerDaySQL(int lastDayVisit = 0)
        {

            var userList = await _db.Diagnostics.FromSql($"GetUsersOnline {lastDayVisit}").ToListAsync();

            //    +",facebookBot,amazonBot).ToListAsync();
            return userList;

        }
    }

    public interface IDiagnosticService
    {
        IQueryable<Diagnostic> Diagnostics { get; }
        Task DelFromMigrate();
        Task<Diagnostic> GetAsync(string username);
        Task<Diagnostic> GetInfo();
        Task DiagUpdate(Diagnostic diag);
        Task DiagRemove(Diagnostic diag);
        Task<List<Diagnostic>> GetAllAsync();
        Task DelHistory();
        Task<string> EditConnId(Diagnostic diag, string signalrUserId = null, bool isActive = true, bool ismobile = false);
        Task EditAdminOnlineStatus(Diagnostic diag);
        Task<List<Diagnostic>> GetAllOnlineAsync();
        Task SaveChangesAsync();
        Task FirstTimeVisiter(int id);
        TimeSpan getLastTimeVisit(Diagnostic diag);
        Task UpdateCurrDateTime(int id);
        Task<Diagnostic> GetAsyncById(int id);
        Task DiagRangeRemove(IEnumerable<Diagnostic> diag);
        IQueryable<Diagnostic> UsersPerDay(int lastDayVisit = 0);
        Task<List<Diagnostic>> UsersPerDaySQL(int lastDayVisit = 0);
    }
}