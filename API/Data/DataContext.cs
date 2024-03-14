using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Enitites;
using Microsoft.EntityFrameworkCore;


// this class bridge between our code and db
namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {   }

        public DbSet<AppUsers> Users {get;set;} // Users is table name we create in DB
        
    }

   
}