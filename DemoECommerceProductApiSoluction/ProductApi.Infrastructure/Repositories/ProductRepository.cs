using ECommerce.SharedLibrary.Logs;
using ECommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.Repositories
{
    internal class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                // check if product alredy exist
                var getProduct = await GetByAsync(_ => _.Name!.Equals(entity.Name));
                if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name)) 
                {
                    return new Response(false, $"{entity.Name} alredy added");
                }

                var currentEntity = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (currentEntity is not null && currentEntity.Id > 0)
                {
                    return new Response(true, $"{entity.Name} added to database successfuly");
                }
                else
                {
                    return new Response(false, $"Error trying to add {entity.Name}");
                }
            }
            catch (Exception ex) 
            {
                //Log the original exception
                LogException.LogExceptions(ex);

                //Display scary-free
                return new Response(false, "Error occurred adding a new product");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if(product is null)
                {
                    return new Response(false, $"Error trying to delete {entity.Name} not found");
                }

                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} deleted from database successfuly");
            }
            catch (Exception ex)
            {
                //Log the original exception
                LogException.LogExceptions(ex);

                //Display scary-free
                return new Response(false, "Error occurred deleting product");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                return product is not null ? product : null!;

            }
            catch (Exception ex)
            {
                //Log the original exception
                LogException.LogExceptions(ex);

                //Display scary-free
                throw new Exception("Error occurred retrieving a product");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await context.Products.ToListAsync();
                return products is not null ? products : null!;

            }
            catch (Exception ex)
            {
                //Log the original exception
                LogException.LogExceptions(ex);

                //Display scary-free
                throw new Exception("Error occurred retrieving products");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await context.Products.Where(predicate).FirstOrDefaultAsync()!;
                return product is not null ? product : null!;

            }
            catch (Exception ex)
            {
                //Log the original exception
                LogException.LogExceptions(ex);

                //Display scary-free
                throw new Exception("Error occurred retrieving a product");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var currentProduct = await FindByIdAsync(entity.Id);
                if (currentProduct is null)
                {
                    return new Response(false, $"{entity.Name} entity not found");
                }

                context.Entry(currentProduct).State = EntityState.Detached;
                context.Products.Update(entity);
                await context.SaveChangesAsync();

                return new Response(true, $"{entity.Name} updated successfuly");

            }
            catch (Exception ex)
            {
                //Log the original exception
                LogException.LogExceptions(ex);

                //Display scary-free
                return new Response(false, "Error occurred updating a product");
            }
        }
    }
}
