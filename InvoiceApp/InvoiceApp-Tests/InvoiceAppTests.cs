using InvoiceApp.Constants;
using InvoiceApp.Contracts.Invoices;
using InvoiceApp.Controllers;
using InvoiceApp.Services;
using InvoiceApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    /// <summary>
    /// Controller testing to determine if it works
    /// </summary>
    [TestFixture]
    public class ControllerTests
    {
        private IInvoiceService _service;
        private InvoiceController _controller;

        [SetUp]
        public void Setup()
        {
            _service = Substitute.For<IInvoiceService>(); //Mocked service
            _controller = new InvoiceController(_service); //Controller
        }

        [Test]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            IList<InvoiceDto> listOfInvoices = new List<InvoiceDto>();
            listOfInvoices.Add(GenerateDummyInvoiceDto());
            listOfInvoices.Add(GenerateDummyInvoiceDto());
            _service.GetAllInvoices().Returns(listOfInvoices);

            // Act
            var okResult = _controller.Get();

            // Assert
            var okObjectResult = okResult.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
        }

        [Test]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Arrange
            IList<InvoiceDto> listOfInvoices = new List<InvoiceDto>();
            listOfInvoices.Add(GenerateDummyInvoiceDto());
            listOfInvoices.Add(GenerateDummyInvoiceDto());
            _service.GetAllInvoices().Returns(listOfInvoices);

            // Act
            var okResult = _controller.Get();

            // Assert
            var okObjectResult = okResult.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var invoices = okObjectResult.Value as List<InvoiceDto>;
            Assert.IsNotNull(invoices);
            Assert.AreEqual(2, invoices.Count);
        }

        [Test]
        public void GetById_ExistingGuidPassed_ReturnsOkResult()
        {
            // Arrange
            var invoice = GenerateDummyInvoiceDto();
            _service.GetById(Arg.Any<Guid>()).Returns(invoice);

            // Act
            var okResult = _controller.Get(Guid.NewGuid());

            // Assert
            var okObjectResult = okResult.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var invoiceResult = okObjectResult.Value as InvoiceDto;
            Assert.IsNotNull(invoiceResult);
            Assert.AreEqual(invoice.Id, invoiceResult.Id);
        }

        [Test]
        public void GetById_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            // Arrange
            InvoiceDto invoice = null;
            _service.GetById(Arg.Any<Guid>()).Returns(invoice);

            // Act
            var okResult = _controller.Get(Guid.NewGuid());

            // Assert
            var okObjectResult = okResult.Result as NotFoundResult;
            Assert.IsNotNull(okObjectResult);
        }

        [Test]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedInvoice()
        {
            // Arrange
            var passInvoice = GenerateDummyNewInvoiceDto();
            var resultInvoice = GenerateDummyInvoiceDto();
            _service.Add(passInvoice).Returns(resultInvoice);

            // Act
            var createdResponse = _controller.Post(passInvoice) as CreatedAtActionResult;
            var addedInvoice = createdResponse.Value as InvoiceDto;

            // Assert
            Assert.IsNotNull(createdResponse);
            Assert.IsNotNull(addedInvoice);
            Assert.AreEqual(resultInvoice.Name, addedInvoice.Name);
        }

        [Test]
        public void Remove_ValidObjectPassed_ReturnedResponseOkResult()
        {
            // Arrange
            var resultInvoice = GenerateDummyInvoiceDto();
            _service.GetById(Arg.Any<Guid>()).Returns(resultInvoice);

            // Act
            var okResult = _controller.Remove(Guid.NewGuid()) as OkResult;

            // Assert
            Assert.IsNotNull(okResult);
        }

        [Test]
        public void Remove_UnknownObjectPassed_ReturnedResponseNotFound()
        {
            // Arrange
            InvoiceDto resultInvoice = null;
            _service.GetById(Arg.Any<Guid>()).Returns(resultInvoice);

            // Act
            var notFoundResult = _controller.Remove(Guid.NewGuid()) as NotFoundResult;

            // Assert
            Assert.IsNotNull(notFoundResult);
        }

        /// <summary>
        /// Function to generate a fake Invoice object for testing purposes
        /// </summary>
        /// <returns>Invoice object</returns>
        private InvoiceDto GenerateDummyInvoiceDto()
        {
            var invoice = new InvoiceDto
            {
                Id = Guid.NewGuid(),
                Client = ClientType.FizinisAsmuo,
                DateCreated = new DateTime(2018, 7, 13, 15, 2, 3),
                ClientCountry = "LTU",
                ClientPayer = ClientPayerType.NotPayer,
                Description = "Pirmoji saskaita",
                Name = "DomantasTest",
                Price = 510,
                Provider = ProviderType.JuridinisAsmuo,
                ProviderPayer = ProviderPayerType.VATPayer,
                ProviderCountry = "LTU",
                VATAmount = 21
            };
            return invoice;
        }

        /// <summary>
        /// Function to generate a fake NewInvoice object for testing purposes
        /// </summary>
        /// <returns>NewInvoice object</returns>
        private NewInvoiceDto GenerateDummyNewInvoiceDto()
        {
            var invoice = new NewInvoiceDto
            {
                Client = ClientType.FizinisAsmuo,
                DateCreated = new DateTime(2018, 7, 13, 15, 2, 3),
                ClientCountry = "LTU",
                ClientPayer = ClientPayerType.NotPayer,
                Description = "Pirmoji saskaita",
                Name = "DomantasTest",
                Price = 510,
                Provider = ProviderType.JuridinisAsmuo,
                ProviderPayer = ProviderPayerType.VATPayer,
                ProviderCountry = "LTU"
            };
            return invoice;
        }

    }
    
    /// <summary>
    /// VAT Service testing to determine if it give correct values
    /// </summary>
    [TestFixture]
    public class VATServiceTests
    {
        [TestCase("LTU", ClientPayerType.NotPayer, "LTU", ProviderPayerType.VATPayer, 21)]
        [TestCase("LAT", ClientPayerType.VATPayer, "LAT", ProviderPayerType.VATPayer, 22)]
        [TestCase("LTU", ClientPayerType.NotPayer, "LTU", ProviderPayerType.NotPayer, 0)]
        [TestCase("LTU", ClientPayerType.VATPayer, "LTU", ProviderPayerType.NotPayer, 0)]
        [TestCase("RUS", ClientPayerType.NotPayer, "LTU", ProviderPayerType.VATPayer, 0)]
        [TestCase("EST", ClientPayerType.NotPayer, "LTU", ProviderPayerType.VATPayer, 23)]
        [TestCase("EST", ClientPayerType.VATPayer, "LTU", ProviderPayerType.VATPayer, 0)]
        public void FindVAT_DataPassed_CalculatedCorrectVAT(
            string clientCountry, ClientPayerType clientPayer,
            string providerCountry, ProviderPayerType providerPayer,
            decimal correctVAT)
        {
            // Arrange
            IVATService _service = new VATService(clientCountry, clientPayer, providerCountry, providerPayer);

            // Act
            var vatCalculated = _service.FindVAT();

            // Assert
            Assert.IsNotNull(vatCalculated);
            Assert.AreEqual(correctVAT, vatCalculated);
        }
    }
}