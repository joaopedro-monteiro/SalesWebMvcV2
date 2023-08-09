using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;
        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<SalesRecord> FindByDate(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if(minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }

            if(maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToList();
        }
        public List<IGrouping<Department, SalesRecord>> FindByDateGrouping(DateTime? minDate, DateTime? maxDate)
        {
            var query = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                query = query.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                query = query.Where(x => x.Date <= maxDate.Value);
            }

            var intermediateResult = query
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToList();

            var finalResult = intermediateResult
                .GroupBy(x => x.Seller.Department)
                .ToList();

            return finalResult;
        }
    }
}
