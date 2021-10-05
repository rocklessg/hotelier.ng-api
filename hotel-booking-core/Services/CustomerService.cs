using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hotel_booking_data.UnitOfWork.Implementation;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.Repositories.Implementations;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_models;

namespace hotel_booking_core.Services
{
    public class CustomerService
    {
        //CustomerRepository _customerRepository;
        IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           // _customerRepository = _unitOfWork.Customers;
        }


        #region 
        /// <summary>
        /// A method to update a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="updateCustomer"></param>
        /// <returns>Task<bool></returns>
        public async Task<bool> UpdateCustomer(string customerId, UpdateCustomerRequest updateCustomer)
        {
            Customer customer = _unitOfWork.Customers.FindAsync(customerId);
            if (customer != null)
            {
                customer.AppUser.FirstName = string.IsNullOrWhiteSpace(updateCustomer.FirstName) ? customer.AppUser.FirstName : updateCustomer.FirstName;
                customer.AppUser.LastName = string.IsNullOrWhiteSpace(updateCustomer.LastName) ? customer.AppUser.LastName : updateCustomer.LastName;
                customer.AppUser.Gender = string.IsNullOrWhiteSpace(updateCustomer.Gender) ? customer.AppUser.Gender : updateCustomer.Gender;
                customer.AppUser.Age = updateCustomer.Age < 1 ? customer.AppUser.Age : updateCustomer.Age;
                customer.AppUser.Avatar = string.IsNullOrWhiteSpace(updateCustomer.Avatar) ? customer.AppUser.Avatar : updateCustomer.Avatar;
                


               

            }
            #endregion

        }
    }
