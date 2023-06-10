using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LinQTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var students = new List<Student>()
            {
                new Student() { StudentId = 11, StudentName = "st1", ClassId = 1},
                new Student() { StudentId = 12, StudentName = "st2", ClassId = 1},
                new Student() { StudentId = 13, StudentName = "st3", ClassId = 2},
            };

            var classes = new List<Class>()
            {
                new Class() { ClassId = 1, ClassName = "c1"},
                new Class() { ClassId = 2, ClassName = "c2"},
                new Class() { ClassId = 3, ClassName = "c3"}
            };

            // from-from-select, join query, join method
            #region 1. studentid, classid

            Console.WriteLine("1. studentid, classid");
            var result = from c in classes
                         from s in students
                         select new { s.StudentId, c.ClassId };
            foreach (var x in result)
            {
                Console.WriteLine(x.StudentId + ", " + x.ClassId);
            }






            Console.WriteLine("_______________________________");

            var result11 = from c in classes
                           join s in students on 1 equals 1
                           select new { s.StudentId, c.ClassId };
            foreach (var x in result11)
            {
                Console.WriteLine(x.StudentId + ", " + x.ClassId);
            }






            Console.WriteLine("_______________________________");

            var result12 = classes.Join(students,
                                    s => true,
                                    c => true,
                                    (c, s) => new { s.StudentId, c.ClassId });
            foreach (var x in result12)
            {
                Console.WriteLine(x.StudentId + ", " + x.ClassId);
            }
            #endregion

            Console.WriteLine("______________________________________________________________________");

            //groupjoin, join into, from-select-count
            #region 2. Class name, count student
            Console.WriteLine("2. Class name, count student");
            var result2 = classes.GroupJoin(students,
                                            c => c.ClassId,
                                            s => s.ClassId,
                                            (c, s) => new
                                            {
                                                ClassName = c.ClassName,
                                                StudentCount = s.Count()
                                            });
            foreach (var x in result2)
            {
                Console.WriteLine(x.ClassName + ", " + x.StudentCount);
            }






            Console.WriteLine("_______________________________");

            var result21 = from c in classes
                           select new
                           {
                               ClassName = c.ClassName,
                               StudentCount = students.Count(s => s.ClassId == c.ClassId)
                           };
            foreach (var x in result21)
            {
                Console.WriteLine(x.ClassName + ", " + x.StudentCount);
            }





            Console.WriteLine("_______________________________");

            var result22 = from c in classes
                           join s in students on c.ClassId equals s.ClassId into T
                           select new
                           {
                               ClassName = c.ClassName,
                               StudentCount = T.Count()
                           };

            foreach (var x in result22)
            {
                Console.WriteLine(x.ClassName + ", " + x.StudentCount);
            }
            #endregion

            Console.WriteLine("______________________________________________________________________");

            //join-group, join into-where, groupjoin-where, join query - groupby
            #region 3. ClassId, StudentCount
            Console.WriteLine("3. ClassId, StudentCount");

            var result3 = from c in classes
                          join s in students on c.ClassId equals s.ClassId
                          group new { c, s } by c.ClassId into T
                          select new
                          {
                              ClassId = T.Key,
                              StudentCount = T.Count()
                          };
            foreach (var x in result3)
            {
                Console.WriteLine(x.ClassId + ", " + x.StudentCount);
            }






            Console.WriteLine("_______________________________");

            var result31 = from c in classes
                           join s in students on c.ClassId equals s.ClassId into T
                           where T.Count() > 0
                           select new
                           {
                               ClassId = c.ClassId,
                               StudentCount = T.Count()
                           };

            foreach (var x in result31)
            {
                Console.WriteLine(x.ClassId + ", " + x.StudentCount);
            }






            Console.WriteLine("_______________________________");

            var result32 = classes.GroupJoin(students,
                                    c => c.ClassId,
                                    s => s.ClassId,
                                    (c, s) => new { ClassId = c.ClassId, StudentCount = s.Count() })
                                    .Where(t => t.StudentCount > 0).ToList();
                                    /*.Select(g => new { g.ClassId, g.StudentCount });*/

            foreach (var x in result32)
            {
                Console.WriteLine(x.ClassId + ", " + x.StudentCount);
            }






            Console.WriteLine("_______________________________");

            var result33 = classes.Join(students, c => c.ClassId, s => s.ClassId, (c, s) => new { ClassId = c.ClassId, StudentName = s.StudentName })
                                    .GroupBy(t => t.ClassId)
                                    .Select(g => new { ClassId = g.Key, StudentCount = g.Count() });
            foreach (var x in result33)
            {
                Console.WriteLine(x.ClassId + ", " + x.StudentCount);
            }






            Console.WriteLine("_______________________________");
            var result34 = from c in classes
                           where students.Count(x => x.ClassId == c.ClassId) > 0
                           select new
                           {
                               ClassId = c.ClassId,
                               StudentCount = students.Count(x => x.ClassId == c.ClassId)
                           };
            foreach (var x in result34)
            {
                Console.WriteLine(x.ClassId + ", " + x.StudentCount);
            }

            #endregion

            Console.WriteLine("______________________________________________________________________");

            //join into-where, join-group, from-where-select-count
            #region 4. ClassName, Student Count
            Console.WriteLine("4. ClassName, Student Count");
            var result4 = from c in classes
                          join s in students on c.ClassId equals s.ClassId into T
                          where T.Count() > 0
                          select new { ClassName = c.ClassName, StudentCount = T.Count() };
            foreach (var x in result4)
            {
                Console.WriteLine(x.ClassName + ", " + x.StudentCount);
            }






            Console.WriteLine("_______________________________");

            var result41 = from c in classes
                           join s in students on c.ClassId equals s.ClassId
                           group new { c, s } by c.ClassName into T
                           select new { ClassName = T.Key, StudentCount = T.Count() };
            foreach (var x in result41)
            {
                Console.WriteLine(x.ClassName + ", " + x.StudentCount);
            }






            Console.WriteLine("_______________________________");

            var result42 = classes.GroupJoin(students, c => c.ClassId, s => s.ClassId,
                                                (c, s) => new { ClassName = c.ClassName, StudentCount = s.Count() })
                                    .Where(t => t.StudentCount > 0).ToList();
                                    /*.Select(g => new { g.ClassName, g.StudentCount });*/
            foreach (var x in result42)
            {
                Console.WriteLine(x.ClassName + ", " + x.StudentCount);
            }






            Console.WriteLine("_______________________________");

            var result43 = from c in classes
                           where students.Count(s => s.ClassId == c.ClassId) > 0
                           select new
                           {
                               ClassName = c.ClassName,
                               StudentCount = students.Count(s => s.ClassId == c.ClassId)

                           };
            foreach (var x in result43)
            {
                Console.WriteLine(x.ClassName + ", " + x.StudentCount);
            }

            #endregion

            Console.WriteLine("______________________________________________________________________");

            //join into, from-select-select, groupjoin
            #region 5. ClassName, Name of first student
            Console.WriteLine("5. ClassName, Name of first student");

            var result5 = from c in classes
                          join s in students on c.ClassId equals s.ClassId into T
                          select new { ClassName = c.ClassName, FirstStudent = T.Select(s => s.StudentName).FirstOrDefault() };
            foreach (var x in result5)
            {
                if (x.FirstStudent == null)
                    Console.WriteLine(x.ClassName + ", null");
                else
                    Console.WriteLine(x.ClassName + ",  " + x.FirstStudent);
            }






            Console.WriteLine("_______________________________");

            var result51 = from c in classes
                           select new
                           {
                               ClassName = c.ClassName,
                               FirstStudent = students.Where(s => s.ClassId == c.ClassId)
                                                      .Select(s => s.StudentName).FirstOrDefault()
                           };
            foreach (var x in result51)
            {
                if (x.FirstStudent == null)
                    Console.WriteLine(x.ClassName + ", null");
                else
                    Console.WriteLine(x.ClassName + ",  " + x.FirstStudent);
            }






            Console.WriteLine("_______________________________");

            var result52 = classes.GroupJoin(students, c => c.ClassId, s => s.ClassId,
                                             (c, s) => new { ClassName = c.ClassName, FirstStudent = s.Select(x => x.StudentName).FirstOrDefault() });
            foreach (var x in result52)
            {
                if (x.FirstStudent == null)
                    Console.WriteLine(x.ClassName + ", null");
                else
                    Console.WriteLine(x.ClassName + ",  " + x.FirstStudent);
            }
            #endregion

            Console.WriteLine("______________________________________________________________________");

            #region BAI 2
            Console.WriteLine("BAI 2:");
            var list1 = new List<Class>()
            {
                new Class() {ClassId = 1, ClassName = "a"},
                new Class() {ClassId = 2, ClassName = "b"},
                new Class() {ClassId = 3, ClassName = "c"}
            };
            var list2 = new List<Class>()
            {
                new Class() {ClassId = 2, ClassName = "b"},
                new Class() {ClassId = 4, ClassName = "d"},
                new Class() {ClassId = 5, ClassName = "e"}
            };

            var list3 = from x in list1
                        join y in list2 on x.ClassId equals y.ClassId
                        select new {x.ClassId, x.ClassName };

            var list4 = list1.Where(x => !list2.Any(y => y.ClassId == x.ClassId)).ToList();
            foreach (var x in list4)
            {
                Console.WriteLine(x.ClassId + ", " + x.ClassName);
            }
            #endregion

            Console.ReadKey();
        }
    }
}
