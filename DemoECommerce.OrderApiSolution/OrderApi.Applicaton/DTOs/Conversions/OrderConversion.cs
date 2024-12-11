﻿using OrderApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Applicaton.DTOs.Conversions
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDTO order) => new Order()
        {
            Id = order.Id,
            ClientId = order.ClientId,
            OrderedDate = order.OrderedDate,
            ProductId = order.ProductId,
            PurchaseQuantity = order.PurchaseQuantity,
        };

        public static (OrderDTO?, IEnumerable<OrderDTO>?) FromEntity(Order? order, IEnumerable<Order>? orders)
        {
            //return single
            if(order is not null || orders is null)
            {
                var singleOrder = new OrderDTO(
                    order!.Id,
                    order.ProductId,
                    order.ClientId,
                    order.PurchaseQuantity,
                    order.OrderedDate

                    );

                return (singleOrder, null);
            }

            if(orders is not null || order is null)
            {
                var ordersList = orders!.Select(o =>
                new OrderDTO(
                        o.Id,
                        o.ProductId,
                        o.ClientId,
                        o.PurchaseQuantity,
                        o.OrderedDate
                        )
                );

                return (null, ordersList);
            }

            return (null, null);
        }
    }
}
