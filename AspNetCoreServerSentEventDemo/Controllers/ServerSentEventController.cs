using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Net.Http.Headers;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ServerSentEventSample
{
    [Route("/api/sse")]
    public class ServerSentEventController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private List<PayrollResult> _payresult;
        public ServerSentEventController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }

        private void ComputePayroll()
        {
            _payresult = new List<PayrollResult>()
            {
                new PayrollResult() { EmployeeId = 1, EmployeeName = "Eva Green", Pay = 3824 },
                new PayrollResult() { EmployeeId = 1, EmployeeName = "Mia Wasikowska", Pay = 3721 },
            };
        }

        [HttpGet("payroll1")]
        public async Task GetCustomer1()
        {
            var response = _httpContextAccessor.HttpContext.Response;
            response.Headers.Add("Content-Type", "text/event-stream");
            response.StatusCode = 200;

            await response.WriteAsync("data:Computing Payroll ....\r\r");
            ComputePayroll();

            StringBuilder sb = new StringBuilder();
            string jsonCustomer = JsonConvert.SerializeObject(_payresult);
            sb.Append(jsonCustomer);

            Thread.Sleep(3000);
            await response.WriteAsync($"data:{sb.ToString()}\r\r");
            await response.WriteAsync($"data: END-OF-STREAM\n\n");
            response.Body.Flush();
            response.Body.Close();
        }

        [HttpGet("payroll2")]
        public async Task GetCustomer2()
        {
            var response = _httpContextAccessor.HttpContext.Response;
            response.Headers.Add("Content-Type", "text/event-stream");
            response.StatusCode = 200;

            await response.WriteAsync("data:Computing Payroll ....\r\r");
            ComputePayroll();

            foreach (var pay in _payresult)
            {
                string jsonCustomer = JsonConvert.SerializeObject(pay);
                string data = $"data: {jsonCustomer}\n\n";
                Thread.Sleep(1000);
                await response.WriteAsync(data);
                response.Body.Flush();
            }

            await response.WriteAsync($"data: END-OF-STREAM\n\n");
            response.Body.Close();
        }

        public class PayrollResult
        {
            public int EmployeeId { get; set; }
            public string EmployeeName { get; set; }
            public decimal Pay { get; set; }
        }
    }
}