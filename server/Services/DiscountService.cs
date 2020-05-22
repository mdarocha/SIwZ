using System;
using System.Collections.Generic;
using System.Linq;
using Server.Models;
using Server.Database;

namespace Server.Services
{
    public class DiscountService
    {
        private readonly TrainSystemContext _context;

        public DiscountService(TrainSystemContext context)
        {
            _context = context;
        }

        public List<Discount> GetAll()
        {
            return _context.Discounts.Where(d => true).ToList();
        }

        public Discount GetById(int id)
        {
            return _context.Discounts.Find(id);
        }

        public List<Discount> GetByType(string type)
        {
            return _context.Discounts.Where(d => d.Type.Equals(type)).ToList();
        }

        public Discount Create(Discount discount)
        {
            _context.Discounts.Add(discount);
            _context.SaveChanges();
            return discount;
        }

        public void Edit(Discount discount)
        {
            var dc = _context.Discounts.Find(discount.Id);

            if (dc != null)
            {
                dc.Type = discount.Type;
                dc.Value = discount.Value;
                dc.ValueType = discount.ValueType;

                _context.Discounts.Update(dc);
                _context.SaveChanges();
            }
        }

        public void Delete(Discount discount)
        {
            _context.Discounts.Remove(discount);
            _context.SaveChanges();
        }

        public double ApplyDiscount(double price, int discountId)
        {
            var dsc = _context.Discounts.Find(discountId);
            
            if (dsc != null)
            {
                switch (dsc.ValueType)
                {
                    case Discount.DiscountValueTypes.Flat:
                        return Math.Max(price - dsc.Value, 0);
                    case Discount.DiscountValueTypes.Percentage:
                        return price * (100 - dsc.Value);
                }
            }

            return price;
        }
    }
}