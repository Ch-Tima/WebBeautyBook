// See https://aka.ms/new-console-template for more information
using Domain.Models;

Console.WriteLine("Hello, World!");

List<CompanyScheduleException> companyScheduleExceptionList = new List<CompanyScheduleException>
        {
            new CompanyScheduleException {
                ExceptionDate = new DateTime(2024, 3, 8),
                OpenFrom = TimeSpan.FromHours(10),
                OpenUntil = TimeSpan.FromHours(15),
                IsClosed = false,
                IsOnce = false,
                Reason = "b"
            },
            new CompanyScheduleException {
                ExceptionDate = new DateTime(2027, 3, 3),
                OpenFrom = TimeSpan.FromHours(10),
                OpenUntil = TimeSpan.FromHours(15),
                IsClosed = false,
                IsOnce = true,
                Reason = "c"
            },
            new CompanyScheduleException {
                ExceptionDate = new DateTime(2022, 3, 4),
                OpenFrom = TimeSpan.FromHours(10),
                OpenUntil = TimeSpan.FromHours(15),
                IsClosed = false,
                IsOnce = true,
                Reason = "d"
            },
            new CompanyScheduleException {
                ExceptionDate = new DateTime(2019, 3, 8),
                OpenFrom = TimeSpan.FromHours(10),
                OpenUntil = TimeSpan.FromHours(15),
                IsClosed = false,
                IsOnce = true,
                Reason = "e"
            },
            new CompanyScheduleException {
                ExceptionDate = new DateTime(2023, 3, 9),
                OpenFrom = TimeSpan.FromHours(10),
                OpenUntil = TimeSpan.FromHours(15),
                IsClosed = false,
                IsOnce = true,
                Reason = "f"
            },
            new CompanyScheduleException {
                ExceptionDate = new DateTime(2024, 3, 24),
                OpenFrom = TimeSpan.FromHours(10),
                OpenUntil = TimeSpan.FromHours(15),
                IsClosed = false,
                IsOnce = true,
                Reason = "g"
            },
        };


DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 55, 0);

Console.WriteLine($"now: {now}");
//Console.WriteLine($"su: {now.AddDays(-(int)now.DayOfWeek)}");
//Console.WriteLine($"sa: {(now.AddDays(-(int)now.DayOfWeek)).AddDays(6)}");

var dateDeleting = now.AddDays(-(int)now.DayOfWeek).AddDays(7);//.AddMinutes(59);
Console.WriteLine($"day delete: {dateDeleting} msd: {(dateDeleting - now).TotalMilliseconds}");

Console.WriteLine("\nCompanyScheduleException:");
foreach (var item in companyScheduleExceptionList)
{
    Console.WriteLine($"Id: {item.Id}, ExceptionDate: {item.ExceptionDate}, OpenFrom: {item.OpenFrom}, OpenUntil: {item.OpenUntil}, IsClosed: {item.IsClosed}, IsOnce: {item.IsOnce}, Reason: {item.Reason}");
}

Console.WriteLine("\nCompanyScheduleException dateDeleting:");
foreach (var item in companyScheduleExceptionList)
{
    if(item.IsOnce && item.ExceptionDate < dateDeleting)
    {
        //companyScheduleExceptionList.Remove(item);
    }
    else
    {
        Console.WriteLine($"Id: {item.Id}, ExceptionDate: {item.ExceptionDate}, OpenFrom: {item.OpenFrom}, OpenUntil: {item.OpenUntil}, IsClosed: {item.IsClosed}, IsOnce: {item.IsOnce}, Reason: {item.Reason}");
    }
}

Console.WriteLine("s");

try
{
    Console.WriteLine("try");
    //throw new Exception();
    Console.WriteLine("ex");
}
catch (Exception ex)
{
    Console.WriteLine("catch");
}
finally
{
    Console.WriteLine("finally");
}

Console.WriteLine("e");