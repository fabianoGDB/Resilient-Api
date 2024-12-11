﻿using ECommerce.SharedLibrary.Logs;
using ECommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using OrderApi.Applicaton.Interfaces;
using OrderApi.Domain.Entities;
using OrderApi.Infrasructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Infrasructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                var order = context.Add(entity).Entity;
                await context.SaveChangesAsync();
                return order.Id > 0 ? new Response(true, "Order placed successfully") :
                    new Response(false, "An Error occured wjile placing your order");

            }
            catch (Exception ex) 
            {
                //Log Original Exception
                LogException.LogExceptions(ex);

                //display scary-free message to client
                return new Response(false, "An error occured while placing your order");
            }
        }

        public async Task<Response> DeleteAsync(Order entity)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id);
                if (order is null)
                {
                    return new Response(false, "Order not found");
                }

                context.Orders.Remove(order);
                await context.SaveChangesAsync();
                return new Response(true, "Order successfully deleted");
            }
            catch (Exception ex)
            {
                //Log Original Exception
                LogException.LogExceptions(ex);

                //display scary-free message to client
                return new Response(false, "An error occured while deleting your order");
            }
        }

        public async Task<Order> FindByIdAsync(int id)
        {
            try
            {
                var order = await context.Orders!.FindAsync(id);
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {
                //Log Original Exception
                LogException.LogExceptions(ex);

                //display scary-free message to client
                throw new Exception("An error occured while retriveing your order");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await context.Orders.AsNoTracking().ToListAsync();
                return orders is not null? orders : null!;
            }
            catch (Exception ex)
            {
                //Log Original Exception
                LogException.LogExceptions(ex);

                //display scary-free message to client
                throw new Exception("An error occured while retriveing your orders");
            };
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.Where(predicate).FirstOrDefaultAsync()!;
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {
                //Log Original Exception
                LogException.LogExceptions(ex);

                //display scary-free message to client
                throw new Exception("An error occured while retriveing your order");
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var orders = await context.Orders.Where(predicate).ToListAsync()!;
                return orders is not null? orders : null!;
            }
            catch (Exception ex)
            {
                //Log Original Exception
                LogException.LogExceptions(ex);

                //display scary-free message to client
                throw new Exception("An error occured while retriveing your orders");
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            try
            {
                var getOrder = await FindByIdAsync(entity.Id);
                if (getOrder is null) 
                {
                    return new Response(false, $"Order not found");
                }

                context.Entry(getOrder).State = EntityState.Detached;
                context.Orders.Update(entity);
                await context.SaveChangesAsync();

                return new Response(true, "Order updated successfully");

                //4:07:00

            }
            catch (Exception ex)
            {
                //Log Original Exception
                LogException.LogExceptions(ex);

                //display scary-free message to client
                return new Response(false, "An error occured while updating your order");
            }
        }
    }
}
