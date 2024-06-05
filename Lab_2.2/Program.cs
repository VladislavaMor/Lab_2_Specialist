using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lab_2._2
{
    public class Program
    {
        public static IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var courses = db.Courses.ToList();
                db.Teachers.Find(1).Courses.AddRange(courses);//поиск по первичному ключу
                db.Teachers.Find(2).Courses.AddRange(courses.GetRange(0, 2));

                for (int i = 1; i <= 4; i++)
                    db.Students.Find(i).Courses.AddRange(courses.GetRange(0, i));

                db.SaveChanges();
            }
            using (ApplicationContext db = new ApplicationContext())
            {
                var r = db.Teachers.OrderBy(p => p.Name).Include(p => p.Courses);

                foreach (var t in r.ToList())
                {
                    Console.WriteLine(t.Name);
                    foreach (var c in t.Courses)
                    {
                        Console.WriteLine($"\t{c.Title}");
                        var r2 = db.Students.OrderBy(s => s.Name).Where(t => t.Courses.Contains(c));                   
                        foreach (var s in r2)
                            Console.WriteLine($"\t\t{s.Name}");
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
