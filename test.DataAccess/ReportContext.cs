using Microsoft.EntityFrameworkCore;
using test.Model;
using WalkingTec.Mvvm.Core;

namespace test.DataAccess
{
    public class ReportContext : EmptyContext
    {
        public ReportContext(CS cs) : base(cs)
        {
        }

        public DbSet<Csse> Csses { get; set; }

        public ReportContext(string cs, DBTypeEnum dbtype) : base(cs, dbtype)
        {
        }
    }
}