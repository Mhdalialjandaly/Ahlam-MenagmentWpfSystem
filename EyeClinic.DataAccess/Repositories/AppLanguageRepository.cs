using System;
using System.Linq;
using System.Threading.Tasks;
using EyeClinic.Core;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess.Repositories
{
    public class AppLanguageRepository : IAppLanguageRepository
    {
        private readonly EyeClinicContext _context;

        public AppLanguageRepository(EyeClinicContext context) {
            _context = context;
        }

        public async Task<string> GetCurrentCulture(string deviceName) {
            var culture = await _context.AppLanguages.FirstOrDefaultAsync(
                e => e.DeviceName == deviceName);
            if (culture != null)
                return culture.LanguageCode;

            culture = new AppLanguage { LanguageCode = "en", DeviceName = deviceName };
            await _context.AppLanguages.AddAsync(culture);
            await _context.SaveChangesAsync();

            return culture.LanguageCode;
        }

        public async Task SaveCulture(string languageCode, string deviceName) {
            var culture = await _context.AppLanguages.FirstOrDefaultAsync(
                e => e.DeviceName == deviceName);

            culture.LanguageCode = languageCode;
            await _context.SaveChangesAsync();
        }

        public async Task SetDevicePrinter(string printerName, string deviceName) {
            var item = await _context.AppLanguages.FirstOrDefaultAsync(
                e => e.DeviceName == deviceName);

            item.PrinterName = printerName;
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetDevicePrinter(string deviceName) {
            var item = await _context.AppLanguages.FirstOrDefaultAsync(
                e => e.DeviceName == deviceName);

            return item.PrinterName;
        }

        public async Task RequestNextPatient(string deviceName) {
            var item = await _context.AppLanguages.FirstOrDefaultAsync(
                e => e.DeviceName == deviceName);

            item.WaitingNext = true;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsWaitingForNextPatient() {
            return await _context.AppLanguages.AnyAsync(e => e.WaitingNext);
        }

        public async Task AcceptNext() {
            var items = await _context.AppLanguages.Where(
                    e => e.WaitingNext)
                .ToListAsync();

            foreach (var appLanguage in items) {
                appLanguage.WaitingNext = false;
            }

            await _context.SaveChangesAsync();
        }
    }
}
