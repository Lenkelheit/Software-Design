using System;

using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories;

using Microsoft.EntityFrameworkCore;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=Apartment_io_DataBase;Trusted_Connection=True;";

            DbContextOptionsBuilder<DataBaseContext> optionsBuilder = new DbContextOptionsBuilder<DataBaseContext>();
            optionsBuilder.UseSqlServer(connectionstring);

            DataBaseContext dataBaseContext = new DataBaseContext(optionsBuilder.Options);
            UnitOfWork unitOfWork = new UnitOfWork(dataBaseContext);
            UserRepository userRepository = unitOfWork.GetRepository<User, UserRepository>();

            User u1 = userRepository.Get(9038);
            User u2 = userRepository.Get(9039);

            u1.Manager = u2;

            unitOfWork.Update(u1);
            unitOfWork.Save();

            dataBaseContext.Dispose();

            Console.WriteLine("done");
            Console.Read();
        }
    }
}
