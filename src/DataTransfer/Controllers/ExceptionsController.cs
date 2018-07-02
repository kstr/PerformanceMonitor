﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PerfMonitor.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExceptionController : ControllerBase
    {
        public MetricContext _MetricContext;

        public ExceptionController(MetricContext context)
        {
            _MetricContext = context ?? throw new ArgumentNullException(nameof(context));
        }
        [HttpGet]
        [Route("Daterange")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> getExceptionDataByTimerange(DateTime start, DateTime end)
        {
            List<Exceptions> data = await _MetricContext.Exception_Data.Where(d => (d.timestamp > start && d.timestamp < end)).ToListAsync();
            string jsonOfData = JsonConvert.SerializeObject(data);
            return Ok(jsonOfData);
        }

        [HttpGet]
        [Route("Exceptions")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExceptionDataByTime(DateTime d)
        {
            var point = await _MetricContext.Exception_Data.SingleOrDefaultAsync(cpu => cpu.timestamp == d);
            return Ok(point);
        }

        [HttpGet]
        [Route("EXCEPTIONSBYUSAGE")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExceptionDataByType(string kind)
        {
            var point = await _MetricContext.Exception_Data.SingleOrDefaultAsync(exception => exception.type == kind);
            return Ok(point);
        }
        [HttpPost]
        [Route("EXCEPTIONJSON")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateExceptionDatapointFromJSON([FromBody]string j)
        {
            Metric_List met = new Metric_List();
            met = JsonConvert.DeserializeObject<Metric_List>(j);
            foreach (Exceptions point in met.exceptions)
            {
                _MetricContext.Exception_Data.Add(point);
            }
            await _MetricContext.SaveChangesAsync();
            return CreatedAtAction("Exception Data Created", new { obj = j }, null);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateExceptionDatapoint([FromBody]Exceptions c)
        {
            Exceptions point = new Exceptions
            {
                type = c.type,
                timestamp = c.timestamp
            };
            _MetricContext.Exception_Data.Add(point);
            await _MetricContext.SaveChangesAsync();
            return CreatedAtAction("Exceptions Data Created", new { date = point.timestamp }, null);
        }

    }
}