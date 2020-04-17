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

        public void Delete(Discount discount)
        {
            _context.Discounts.Remove(discount);
            _context.SaveChanges();
        }
    }
}