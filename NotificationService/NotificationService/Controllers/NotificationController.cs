using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        [HttpPost]
        [Route("fire-and-forget")]
        public IActionResult FireAndForget(string client)
        {
            string jobId = BackgroundJob.Enqueue(() =>
                Console.WriteLine($"{client}, thank you for contacting us"));

            return Ok($"Job Id: {jobId}");
        }

        [HttpPost]
        [Route("delayed")]
        public IActionResult Delayed(string client)
        {
            string jobId = BackgroundJob.Schedule(() =>
                Console.WriteLine($"Session for the client {client} has been closed"), TimeSpan.FromSeconds(60));

            return Ok($"Job Id: {jobId}");
        }

        [HttpPost]
        [Route("recurring")]
        public IActionResult Reccuring()
        {
            RecurringJob.AddOrUpdate(() =>
                Console.WriteLine($"Happy birthday!"), Cron.Daily);
            return Ok();
        }

        [HttpPost]
        [Route("continuations")]
        public IActionResult Continuations(string client)
        {
            string jobId = BackgroundJob.Enqueue(() =>
                Console.WriteLine($"Check balance logic for the client {client}"));

            BackgroundJob.ContinueJobWith(jobId, () =>
                Console.WriteLine($"{client}, your balance has been changed"));

            return Ok();
        }
    }
}