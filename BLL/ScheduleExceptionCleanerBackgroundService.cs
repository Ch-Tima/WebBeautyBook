using DAL.Context;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BLL
{
    public class ScheduleExceptionCleanerBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly IEmailSender _emailService;

        public ScheduleExceptionCleanerBackgroundService(IServiceProvider services, IEmailSender emailService)
        {
            _services = services;
            _emailService = emailService;
        }

        /// <summary>
        /// Executes the background service to clean up schedule exceptions.
        /// Cleaning occurs every Sunday at 11:55:00 PM
        /// </summary>
        /// <param name="stoppingToken">The cancellation token to stop the service.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 55, 0);
                var cleaningDate = now.AddDays(-(int)now.DayOfWeek).AddDays(7);
                string msg = "<html><body><h1>Schedule Exception Cleaning Report</h1><p>";
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<BeautyBookDbContext>();

                        var list = await dbContext.CompanyScheduleExceptions.Where(x => x.IsOnce && x.ExceptionDate < cleaningDate).ToListAsync();

                        foreach (var item in list)
                        {
                            msg += $"\nId: {item.Id}, ExceptionDate: {item.ExceptionDate}, OpenFrom: {item.OpenFrom}, OpenUntil: {item.OpenUntil}, IsClosed: {item.IsClosed}, IsOnce: {item.IsOnce}, Reason: {item.Reason}<br/>";
                            dbContext.CompanyScheduleExceptions.Remove(item);
                        }

                        await dbContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)//Later I will connect to a proper logger.
                {
                    msg += $"\nException:<br/><br/>Date:{DateTime.Now}<br/>Message: <br/>{ex.Message}<br/><br/><br/>Source: <br/>{ex.Source}";
                }

                msg += "</p></body></html>";

                await _emailService.SendEmailAsync("help.developer.forge@gmail.com", "BackgroundService", msg);
                await Task.Delay((int)(cleaningDate - now).TotalMilliseconds, stoppingToken);
            }
        }
    }
}
