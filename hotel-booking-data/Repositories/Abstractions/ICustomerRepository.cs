﻿using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Customer GetCustomer(string id);
        IQueryable<Customer> GetAllUsers();
        Task<IQueryable<Customer>> GetCustomerHotelsAsync(string customerId);
    }
}