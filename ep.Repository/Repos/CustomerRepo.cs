using ep.Domain.Models;
using ep.Repository.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ep.Repository.Repos
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly SQLiteAsyncConnection _connection;

        public CustomerRepo()
        {
            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        public async Task DeleteAsync(Customer customer)
        {
            await _connection.DeleteAsync(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            await _connection.CreateTableAsync<Customer>();
            return await _connection.Table<Customer>().ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _connection.Table<Customer>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetByKeywordAsync(string keyword)
        {
            return await _connection.Table<Customer>().Where(m => m.OrderNo.Contains(keyword)).ToListAsync();
        }

        public IEnumerable<Customer> GetTodaysCustomers()
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(Customer customer)
        {
            return await _connection.InsertAsync(customer);
        }

        public async Task UpdateAsync(Customer customer)
        {
            await _connection.UpdateAsync(customer);
        }

        public IEnumerable<Customer> GetTodaysMessages()
        {
            var customers = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Name = "Jimmy K",
                    Mobile = "0422230861",
                    OrderNo = "01-01022022",
                    ShopId = 2,
                    Messages = new List<Message>
                    {
                        new Message
                        {
                            Id =1,
                            CreatedOn= DateTimeOffset.UtcNow,
                            Icon = "prep",
                            ShopId = 2,
                            Status = MessageStatus.Prep,
                            Text = "Order has been received"
                        }
                    }
                },
                new Customer
                {
                    Id = 2,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Name = "Joseph J",
                    Mobile = "0422230861",
                    OrderNo = "02-01022022",
                    ShopId = 2,
                    Messages = new List<Message>
                    {
                        new Message
                        {
                            Id = 2,
                            CreatedOn= DateTimeOffset.UtcNow,
                            Icon = "prep",
                            ShopId = 2,
                            Status = MessageStatus.Prep,
                            Text = "Order has been received"
                        },
                        new Message
                        {
                            Id = 3,
                            CreatedOn= DateTimeOffset.UtcNow,
                            Icon = "sent",
                            ShopId = 2,
                            Status = MessageStatus.Sent,
                            Text = "Your order: 02-01022022 is ready to pick up"
                        }
                    }
                },

                new Customer
                {
                    Id = 3,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Name = "Matt H",
                    Mobile = "0422230861",
                    OrderNo = "03-01022022",
                    ShopId = 2,
                    Messages = new List<Message>
                    {
                        new Message
                        {
                            Id = 4,
                            CreatedOn= DateTimeOffset.UtcNow,
                            Icon = "prep",
                            ShopId = 2,
                            Status = MessageStatus.Prep,
                            Text = "Order has been received"
                        },
                        new Message
                        {
                            Id = 5,
                            CreatedOn= DateTimeOffset.UtcNow,
                            Icon = "sent",
                            ShopId = 2,
                            Status = MessageStatus.Sent,
                            Text = "Your order: 03-01022022 is ready to pick up"
                        },
                        new Message
                        {
                            Id = 6,
                            CreatedOn= DateTimeOffset.UtcNow,
                            Icon = "resent",
                            ShopId = 2,
                            Status = MessageStatus.Resent,
                            Text = "Your order: 03-01022022 is ready to pick up"
                        }
                    }
                }
            };

            return customers;
        }
    }
}
