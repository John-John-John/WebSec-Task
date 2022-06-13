using Microsoft.EntityFrameworkCore;
using WebSec_Task.Models;

namespace WebSec_Task
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }

        protected SqlContext()
        {
        }

        public virtual DbSet<ChatMessageEntity> Messages { get; set; }
    }
}
