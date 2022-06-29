using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ep.Tests.Service
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerService> _customerService;
        private readonly CustomerService _customer;
        public CustomerServiceTests()
        {
            _customer = new CustomerService();
            _customerService = new Mock<ICustomerService>();
        }

        [Fact]
        public async Task GetCustomer_WhenCalled_ShouldReturnCustomer()
        {
            // Arrange
            //_service.Setup(x => x.GetCustomer(It.IsAny<string>(), It.IsAny<string>()));

            // Acat
           
        }
    }
}
