using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceApp.Contracts.Invoices;
using InvoiceApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _service;

        public InvoiceController(IInvoiceService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<InvoiceDto>> Get()
        {
            var invoices = _service.GetAllInvoices();
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public ActionResult<InvoiceDto> Get(Guid id)
        {
            var invoice = _service.GetById(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }

        [HttpPost]
        public ActionResult Post([FromBody] NewInvoiceDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var invoice = _service.Add(value);
            return CreatedAtAction("Get", new { id = invoice.Id }, invoice);
        }

        [HttpDelete("{id}")]
        public ActionResult Remove(Guid id)
        {
            var invoice = _service.GetById(id);

            if (invoice == null)
            {
                return NotFound();
            }

            _service.Remove(id);
            return Ok();
        }

    }
}