using SqlSugar;
using DbType = SqlSugar.DbType;

namespace  Common.DBHelper
{
    public class SugarDbHelper
    {

       static     NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();   
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
        public static List<U> GetListBySql<U>(string sql, string orderby = "") where U : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            List<U>? result = null;
            using (var db = sugar.Db)
            {
                if (string.IsNullOrEmpty(orderby))
                {
                    result = db.SqlQueryable<U>(sql).ToList();
                }
                else
                {
                    result = db.SqlQueryable<U>(sql).OrderBy(orderby).ToList();
                }
            }
            return result;
        }
        public static List<U> GetListBySql<U>(string sql, string where, object parameters) where U : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            List<U>? result = null;
            using (var db = sugar.Db)
            {
                result = db.SqlQueryable<U>(sql).Where(where, parameters).ToList();
            }
            return result;
        }


        public static U GetOneBySql<U>(string sql)
            where U : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            U? result = null;
            using (var db = sugar.Db)
            {
                result = db.SqlQueryable<U>(sql).First();
            }
            return result;
        }

        public static int GetInt(string sql)
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                return db.Ado.GetInt(sql);
            }
        }

        public static double GetDouble(string sql)
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                return db.Ado.GetDouble(sql);
            }
        }

        public static E? PageOne<E>(string sql)
            where E : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            var db = sugar.Db;
            var one = db.SqlQueryable<E>(sql).ToList().FirstOrDefault();
            return one;
        }
        public static E? PageOne<E>(string sql, string where, object parameters)
           where E : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            if (parameters == null)
            {
                return PageOne<E>(sql);
            }

            var db = sugar.Db;
            var one = db.SqlQueryable<E>(sql).Where(where, parameters).ToList().FirstOrDefault();
            return one;
        }
        public static object ExecuteScalar(string sql, object? parameters = null)
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                return db.Ado.GetScalar(sql, parameters);
            }
        }
        public static int ExecuteCommand(string sql, object? parameters = null)
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                return db.Ado.ExecuteCommand(sql, parameters);
            }
        }

        public static object ExecuteScalar(string sql)
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                return db.Ado.GetScalar(sql);
            }
        }
        public static async Task<object> ExecuteScalarAsync(string sql, object? parameters = null)
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                return await db.Ado.GetScalarAsync(sql, parameters);
            }
        }
        public static async Task<object> ExecuteScalarAsync(string sql)
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                return await db.Ado.GetScalarAsync(sql);
            }
        }
        public static E GetOneBySql<E>(string sql, object? parameters = null)
            where E : class
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                return db.Ado.SqlQuerySingle<E>(sql, parameters);
            }

        }
        public static async Task<E> GetOneBySqlAsync<E>(string sql, object? parameters = null)
          where E : class
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                return await db.Ado.SqlQuerySingleAsync<E>(sql, parameters);
            }
        }

        public static List<E> GetBySql<E>(string sql, object? parameters = null)
            where E : class
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                return db.Ado.SqlQuery<E>(sql, parameters);
            }

        }

        public static async Task<List<E>> GetBySqlAsync<E>(string sql, object? parameters = null)
            where E : class
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                return await db.Ado.SqlQueryAsync<E>(sql, parameters);
            }
        }

        public static void ExecTransaction(List<string> sqls)
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                try
                {
                    db.Ado.BeginTran();
                    foreach (var item in sqls)
                    {
                        db.Ado.ExecuteCommand(item);
                    }
                    db.Ado.CommitTran();

                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    logger.Error    (ex, "ExecTransaction执行事务失败");

                }
            }
        }

    }

    public class SugarDBaseHelper
    {

        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();    
        SugarDbContext SDC = new SugarDbContext();
        public SugarDBaseHelper()
        {
            SDC.CreateTable();
        }
        public List<T> GetQueryable<T>() where T : class, new()
        {
            SugarDbContext sugar = new();
            using var db = sugar.Db;
            {
                return db.Queryable<T>().ToList();
                ////取前5条
                //var top5 = db.Queryable<T>().Take(5).ToList();
                ////无锁查询
                //var getAllNoLock = db.Queryable<T>().With(SqlWith.NoLock).ToList();
                ////根据主键查询
                //var getByPrimaryKey = db.Queryable<T>().InSingle("0000d82f-b1f2-4c7d-a3b9-6c70f9678282");
                ////查询单条没有数据返回NULL, Single超过1条会报错，First不会
                ////var getSingleOrDefault = db.Queryable<Student>().Single(); //会报错，数据量大于一条
                //var getSingleOrDefault = db.Queryable<T>().Where(A => A.StudentID == "0000d82f-b1f2-4c7d-a3b9-6c70f9678282").Single();
                //var getFirstOrDefault = db.Queryable<T>().First();
                ////UNION ALL Count = 2420838  240多万条数据
                //var UNIONLst = db.UnionAll<T>(db.Queryable<T>(), db.Queryable<T>()).ToList();
                ////in 查询
                //var in1 = db.Queryable<T>().In(A => A.StudentID, new string[] { "000136bf-f968-4a59-9091-bae8ebca42fb", "00020ba7-44e6-494c-8fcb-c1be288a39b3" }).ToList();
                ////主键 In (1,2,3)  不指定列, 默认根据主键In
                //var in2 = db.Queryable<T>().In(new string[] { "000136bf-f968-4a59-9091-bae8ebca42fb", "00020ba7-44e6-494c-8fcb-c1be288a39b3" }).ToList();
                ////in 查询
                //List<string> array = new List<string> { "000136bf-f968-4a59-9091-bae8ebca42fb", "00020ba7-44e6-494c-8fcb-c1be288a39b3" };
                //var in3 = db.Queryable<T>().Where(it => array.Contains(it.StudentID)).ToList();
                ////not in
                //var in4 = db.Queryable<T>().Where(it => !array.Contains(it.StudentID)).ToList();
                ////where
                //var getByWhere = db.Queryable<T>().Where(it => it.StudentID == "000136bf-f968-4a59-9091-bae8ebca42fb" || it.StudentName == "陈丽").ToList();
                ////SqlFunc
                //var getByFuns = db.Queryable<T>().Where(it => SqlFunc.IsNullOrEmpty(it.StudentName)).ToList();
                ////between and 
                //var between = db.Queryable<T>().Where(it => SqlFunc.Between(it.CreateTime, DateTime.Now.AddDays(-10), DateTime.Now)).ToList();
                ////排序
                //var getAllOrder = db.Queryable<T>().Take(100).OrderBy(it => it.CreateTime).ToList(); //默认为ASC排序
                //                                                                                           //组合排序
                //var data = db.Queryable<T>()
                //    .OrderBy(it => it.StudentName, OrderByType.Asc)
                //    .OrderBy(it => it.CreateTime, OrderByType.Desc)
                //    .ToList();

                ////是否存在 any
                //var isAny = db.Queryable<T>().Where(it => it.StudentName == "张龙").Any();
                //var isAny2 = db.Queryable<T>().Any(it => it.StudentSex == "女");
                ////获取同一天的记录
                //var getTodayList = db.Queryable<T>().Where(it => SqlFunc.DateIsSame(it.CreateTime, DateTime.Now)).ToList();

                //Console.ReadLine();
            }
        }

        public void Z<T>() where T : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                //    var list = db.Queryable<Student, StudentGrad>((st, sc) => new object[] {
                //            JoinType.Inner,st.GradID==sc.GradID})
                //           .Select((st, sc) => new { StudentName = st.StudentName, GradName = sc.GradName }).ToList();
                //    var list2 = db.Queryable<Student, StudentGrad>((st, sc) => new object[] {
                //            JoinType.Inner,st.GradID==sc.GradID})
                //              .Select((st, sc) => new ViewModel { StudentName = st.StudentName, GradName = sc.GradName }).ToList();

                //    ///3张表关联查询
                //    //var list3 = db.Queryable<Student, School, Student>((st, sc, st2) => new object[] {
                //    //  JoinType.Left,st.SchoolId==sc.Id,
                //    //  JoinType.Left,st.SchoolId==st2.Id
                //    //})
                //    //         .Where((st, sc, st2) => st2.Id == 1 || sc.Id == 1 || st.Id == 1)
                //    //         .OrderBy((sc) => sc.Id)
                //    //         .OrderBy((st, sc) => st.Name, OrderByType.Desc)
                //    //         .Select((st, sc, st2) => new { st = st, sc = sc }).ToList();

                //    ///分页查询
                //    var pageIndex = 1;
                //    var pageSize = 10;
                //    var list4 = db.Queryable<Student, StudentGrad>((st, sc) => new object[] {
                //  JoinType.Left,st.GradID==sc.GradID
                //}).Select((st, sc) => new ViewModel { StudentName = st.StudentName, GradName = sc.GradName })
                //   .ToPageList(pageIndex, pageSize);

                //    foreach (var item in list4)
                //    {
                //        Console.WriteLine(item.GradName + item.StudentName);
                //    }

                //    ///五张表关联查询
                //    //var list2 = db.Queryable<Student, School, Student, Student, Student>((st, sc, st2, st3, st4) => new object[] {
                //    //  JoinType.Left,st.SchoolId==sc.Id,
                //    //  JoinType.Left,st.Id==st2.Id,
                //    //  JoinType.Left,st.Id==st3.Id,
                //    //  JoinType.Left,st.Id==st4.Id
                //    //}).Where((st, sc) => sc.Id == 1)
                //    // .Select((st, sc, st2, st3, st4) => new { id = st.Id, name = st.Name, st4 = st4 }).ToList();

                //    ///二个Queryable的Join(4.6.0.9)
                //    var q1 = db.Queryable<Student>();
                //    var q2 = db.Queryable<StudentGrad>();
                //    var innerJoinList = db.Queryable(q1, q2, (j1, j2) => j1.GradID == j2.GradID).Select((j1, j2) => j1).ToList();//inner join

                //    ///多表查询的简化 默认为inner join 
                //    var list5 = db.Queryable<Student, StudentGrad>((st, sc) => st.GradID == sc.GradID).Select((st, sc) => new ViewModel { StudentName = st.StudentName, GradName = sc.GradName }).ToList();

                //    ///3表查询
                //    //var list6 = db.Queryable<Student, School, School>((st, sc, sc2) => st.SchoolId == sc.Id && sc.Id == sc2.Id)
                //    //    .Select((st, sc, sc2) => new { st.Name, st.Id, schoolName = sc.Name, schoolName2 = sc2.Name }).ToList();

                //    ///3表查询分页
                //    //var list7 = db.Queryable<Student, School, School>((st, sc, sc2) => st.SchoolId == sc.Id && sc.Id == sc2.Id)
                //    //.Select((st, sc, sc2) => new { st.Name, st.Id, schoolName = sc.Name, schoolName2 = sc2.Name }).ToPageList(1, 2);


                //    ///qlFunc.Subqueryable 子查询
                //    //            var getAll = db.Queryable<Student, School>((st, sc) => new object[] {
                //    //JoinType.Left,st.Id==sc.Id})
                //    //.Where(st => st.Id == SqlFunc.Subqueryable<School>().Where(s => s.Id == st.Id).Select(s => s.Id))
                //    //.ToList();

                //    //            //生成的MYSQL语句，如果是SqlServer就是TOP 1
                //    //            SELECT `st`.`ID`,`st`.`SchoolId`,`st`.`Name`,`st`.`CreateTime` 
                //    //     FROM `STudent` st Left JOIN `School` sc ON( `st`.`ID` = `sc`.`Id` )  
                //    //      WHERE( `st`.`ID` = (SELECT `Id` FROM `School` WHERE( `Id` = `st`.`ID` ) limit 0, 1))
                //    Console.ReadLine();

            }

        }

        public void SqlFunc<T>() where T : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                /////SqlFunc.ToLower 小写
                //var slst = db.Queryable<Student>().Where(it => SqlFunc.ToLower(it.StudentName) == SqlFunc.ToLower("JACK")).ToList();

                /////qlFunc.ToUpper 大写
                //var slst_2 = db.Queryable<Student>().Where(it => SqlFunc.ToUpper(it.StudentName) == "JACK").ToList();
                //var slst_21 = db.Queryable<Student>().Where(it => it.StudentName == "JACK").ToList();

                /////三元判段 ，相当于 it.id==1?1:2
                //var slst_3 = db.Queryable<Student>().Where(it => SqlFunc.ToLower(it.StudentName) == SqlFunc.ToLower("JACK")).Select(A => new StuModel { StudentName = SqlFunc.IIF(A.StudentName == "JACK", "杰克", "其他"), StuSex = A.StudentSex }).ToList();

                /////if else end 4.6.0.1
                //var slst_4 = db.Queryable<Student>().Where(it => SqlFunc.ToLower(it.StudentName).Contains("tom")).Select(A => new StuModel
                //{
                //    StudentName = SqlFunc.IF(A.StudentName == "tom1")
                // .Return("大汤姆")
                // .ElseIF(A.StudentName == "tom2")
                // .Return("中汤姆").End("小汤姆"),
                //    StuSex = A.StudentSex
                //}).ToList();
                //foreach (var item in slst_4)
                //{
                //    Console.WriteLine(item.StudentName);
                //}
                /////ISNULL 查询
                /////IsNullOrEmpty 判段是NULL或者空
                /////SqlFunc.HasValue 判段不是NULL并且不是空
                /////SqlFunc.HasNumber 判段大于0并且不等于NULL
                /////SqlFunc.Trim 去空格
                //var slst_5 = db.Queryable<Student>().Where(A => A.StudentID == "00013a00-9067-40c1-be2a-a06fea47c632").Select(A => new StuModel { StudentName = SqlFunc.IsNull(A.StudentName, "暂无姓名"), StuSex = A.StudentSex }).ToList();
                //foreach (var item in slst_5)
                //{
                //    Console.WriteLine(item.StudentName);
                //}
                /////获取数据库时间 SqlFunc.GetDate()
                ////var tim = SqlFunc.GetDate();//会报错
                //var date = DateTime.Now.AddDays(-10);
                //var slst_6 = db.Queryable<Student>().Where(A => SqlFunc.Between(A.CreateTime, date, SqlFunc.GetDate())).ToList();
                //Console.WriteLine(slst_6.Count);
                /////Contains 模糊查询 like %@p%
                /////StartsWith 模糊查询 like %@p%
                /////EndsWith 模糊查询 like %@p%
                /////
                //var slst_7 = db.Queryable<Student>().Where(A => A.StudentName.EndsWith("婷")).ToList();
                /////等于 SqlFunc.Equals(object thisValue, object parameterValue)
                /////是否是同一天 SqlFunc.DateIsSame(DateTime date1, DateTime date2)
                /////是否是同一时间 （dataType 可以是年、月、天、小时、分钟、秒和毫秒） SqlFunc.DateIsSame(DateTime date1, DateTime date2, DateType dataType)
                /////在当前时间加一定时间（dataType 可以是年、月、天、小时、分钟、秒和毫秒） SqlFunc.DateAdd(DateTime date, int addValue, DateType dataType)
                /////在当前时间加N天 SqlFunc.DateAdd(DateTime date, int addValue)
                /////获取当前时间的年、月、天、小时、分钟、秒或者毫秒 SqlFunc.DateValue(DateTime date, DateType dataType)
                /////范围判段 SqlFunc.Between(object value, object start, object end)
                /////
                //var slst_8 = db.Queryable<Student>().Where(A => SqlFunc.DateAdd(Convert.ToDateTime(A.CreateTime), 1, DateType.Day) > Convert.ToDateTime("2020-10-23")).ToList();
                /////类型转换
                /////
                ///*
                //SqlFunc.ToInt32(object value) 
                //SqlFunc.ToInt64(object value)
                //SqlFunc.ToDate(object value) 
                //SqlFunc.ToString(object value) 
                //SqlFunc.ToDecimal(object value) 
                //SqlFunc.ToGuid(object value) 
                //SqlFunc.ToDouble(object value) 
                //SqlFunc.ToBool(object value)
                //*/
                /////
                /////截取字符串 SqlFunc.Substring(object value, int index, int length)
                /////替换字符串 SqlFunc.Replace(object value, string oldChar, string newChar)
                /////获取字符串长度 SqlFunc.Length(object value)
                /////

                /////聚合函数
                ///*
                // SqlFunc.AggregateSum<TResult>(TResult thisValue) 
                // SqlFunc.AggregateAvg<TResult>(TResult thisValue)
                // SqlFunc.AggregateMin(TResult thisValue) 
                // SqlFunc.AggregateMax<TResult>(TResult thisValue) 
                // SqlFunc.AggregateCount<TResult>(TResult thisValue)
                // */
                /////
                //var slst_9 = db.Queryable<Student>().Select(A => SqlFunc.AggregateMax<DateTime?>(A.CreateTime)).ToList();
                //var slst_91 = db.Queryable<Student>().Select(A => SqlFunc.AggregateMax(A.CreateTime)).ToList();
                //var slst_10 = db.Queryable<Student>().Select(A => SqlFunc.AggregateMin(A.CreateTime)).ToList();

                //Console.ReadLine();
            }
        }

        public void SqlWhereFunc<T>() where T : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                var queryable = db.Queryable<T>();
                ////拼接会比EF方便些，不像EF需要queryable+=
                //queryable.Where(it => it.StudentName.Contains("陈"));
                //queryable.Where(it => it.StudentSex == "女");
                ////防止queryable相互影响我们用clone解决
                //var StudentName = queryable.Clone().Select(it => it.StudentName).First();
                ///////正确答案是两条数据 如果去掉Clone，受上面一条影响，只会有一条数据
                //var list = queryable.Clone().ToList();//正确答案是两条数据 如果去掉Clone，受上面一条影响，只会有一条数据
                /////案例1： WhereIF函数
                /////根据条件判段是否执行过滤，我们可以用WhereIf来实现，true执行过滤，false则不执行
                /////
                //var a = "陈";
                //var b = "飞";
                //var c = "女";
                /////陈性女同学一个
                //var list2 = db.Queryable<T>()
                //.WhereIF(!string.IsNullOrEmpty(a), it => it.StudentName.StartsWith(a))
                //.WhereIF(!string.IsNullOrEmpty(b), it => it.StudentName.EndsWith(b))
                //.WhereIF(!string.IsNullOrEmpty(c), it => it.StudentSex == c).ToList();
                ////

                /////所有叫陈飞的童鞋9人   string.IsNullOrEmpty(c) 这个语句不会执行
                //var list3 = db.Queryable<T>()
                //.WhereIF(!string.IsNullOrEmpty(a), it => it.StudentName.StartsWith(a))
                //.WhereIF(!string.IsNullOrEmpty(b), it => it.StudentName.EndsWith(b))
                //.WhereIF(string.IsNullOrEmpty(c), it => it.StudentName == c).ToList();


                /////
                ///*
                //案例2.：MergeTable 函数 4.4
                //是将多表查询的结果Select里的内容变成一张表， 如果是多表查询的时候，我们无论是使用 where 还是 orderBy 都需要加别名，这样我们就不能实现动态排序，因为我不知道别名叫什么, 可以用MergeTable解决这个问题
                //*/
                /////多表查询方式
                //var pageJoin = db.Queryable<T, StudentGrad>((st, sc) => new object[]
                //{
                //JoinType.Inner, st.GradID == sc.GradID
                //})
                //.Where(st => st.StudentName.EndsWith("芬"))//别名是st
                //.OrderBy("st.StudentName asc")//别名是sc
                //.Select((st, sc) => new { StudentName = st.StudentName, gradeName = sc.GradName })
                //.ToList();

                /////等同于MergeTable 方式
                /////
                //var pageJoin_2 = db.Queryable<T, StudentGrad>((st, sc) => new object[]
                //        {
                //        JoinType.Inner,st.GradID==sc.GradID
                //        })
                //        .Select((st, sc) => new
                //        {
                //            StudentName = st.StudentName,
                //            gradeName = sc.GradName
                //        })
                //        .MergeTable()
                //        .Where(A => A.StudentName.EndsWith("芬")).OrderBy("StudentName asc").ToList();//别名不限


                /////案例3： SqlQueryable 4.5.2.5 , 可以方便的把SQL变成表来操作 直接执行SQL语句
                /////
                //var t12 = db.SqlQueryable<T>("select * from student").ToPageList(1, 2);

                /////案例4： 将表单组装成 List<ConditionalModel>实现查询 4.5.9
                /////查询女生中 带有 飞 子的同学
                //List<IConditionalModel> conModels = new List<IConditionalModel>();
                //conModels.Add(new ConditionalModel() { FieldName = "StudentSex", ConditionalType = ConditionalType.Equal, FieldValue = "女" });
                //conModels.Add(new ConditionalModel() { FieldName = "StudentName", ConditionalType = ConditionalType.Like, FieldValue = "飞" });
                //var student = db.Queryable<T>().Where(conModels).ToList();

                /////
                ////4.6.4.4 版本支持了 复杂的OR 
                //// and StudentSex='女' And (StudentName='陈芬' or StudentName='王芬' Or StudentName='李芬') 
                //List<IConditionalModel> conModels__22 = new List<IConditionalModel>();
                //conModels__22.Add(new ConditionalModel() { FieldName = "StudentSex", ConditionalType = ConditionalType.Equal, FieldValue = "女" });
                //conModels__22.Add(new ConditionalCollections()
                //{
                //ConditionalList =
                //new List<KeyValuePair<WhereType, SqlSugar.ConditionalModel>>()
                //{
                //new  KeyValuePair<WhereType, ConditionalModel>
                //( WhereType.And , //And
                //new ConditionalModel() { FieldName = "StudentName", ConditionalType = ConditionalType.Equal, FieldValue = "陈芬" }),
                //new  KeyValuePair<WhereType, ConditionalModel>
                //(WhereType.Or,
                //new ConditionalModel() { FieldName = "StudentName", ConditionalType = ConditionalType.Equal, FieldValue = "王芬" }),
                //new  KeyValuePair<WhereType, ConditionalModel>
                //( WhereType.Or,
                //new ConditionalModel() { FieldName = "StudentName", ConditionalType = ConditionalType.Equal, FieldValue = "李芬" })
                //}
                //});
                //var studentResult = db.Queryable<T>().Where(conModels__22).ToList();


                /////案例5: 拼接拉姆达 4.5.9.8
                /////
                //var exp = Expressionable.Create<T>()
                //.OrIF(1 == 1, it => it.StudentSex == "女")
                //.And(it => it.StudentName.Contains("陈"))
                //.AndIF(2 == 3, it => SqlFunc.IsNullOrEmpty(it.StudentName)) //此where 不执行
                //.Or(it => it.StudentName.Contains("飞")).ToExpression();//拼接表达式

                //var list55 = db.Queryable<T>().Where(exp).ToList();


                ///Queryable是支持字符串与拉姆达混用或者纯字符串拼接模式，可以满足复杂的一些需求
                ///复杂动态 表达式和SQL子查询混合模式
                ///
                ////例子1:

                ////    var queryable = db.Queryable<Student>("t");

                ////    queryable.Where("t.id in (select id from xxx)");

                ////    queryable.Where(it => it.Id == 1);

                ////    //更多操作拼接qureyable  

                ////    var result = queryable.Select(@"
                ////          id,
                ////          name,
                ////          (select name form school where shoolid=t.id) as schoolName
                ////           ").ToList();


                ////例子2:

                ////    dynamic join3 = db.Queryable("Student", "st")
                ////            .AddJoinInfo("School", "sh", "sh.id=st.schoolid")
                ////            .Where("st.id>@id")
                ////            .AddParameters(new { id = 1 })
                ////            .Select("st.*").ToList(); //也可以Select<T>(“*”).ToList()返回实体集合


                ////例子3:

                ////    var list = db.Queryable<Student>().
                ////                    Select(it => new Student()
                ////                    {
                ////                        Name = it.Name,
                ////                        Id = SqlFunc.MappingColumn(it.Id, "(select top 1 id from school)") // 动态子查询
                ////        }).ToList();



                ///安全拼SQL
                ///安全拼SQL 安全拼SQL 安全拼SQL 安全拼SQL 安全拼SQL 安全拼SQL 安全拼SQL 安全拼SQL 安全拼SQL
                ////            安全拼SQL
                ////使用参数化过滤

                ////private static void Where()
                ////            {
                ////                var db = GetInstance();
                ////                string value = "'jack';drop table Student";
                ////                var list = db.Queryable<Student>().Where("name=@name", new { name = value }).ToList();
                ////                //没有发生任何事情
                ////            }


                ////            字段是无法用参数化实现的，我们就可以采用这种方式过滤

                ////private static void OrderBy()
                ////            {
                ////                var db = GetInstance();
                ////                try
                ////                {
                ////                    var propertyName = "Id'"; //类中的属性的名称
                ////                    var dbColumnName = db.EntityProvider.GetDbColumnName<Student>(propertyName);
                ////                    var list2 = db.Queryable<Student>().OrderBy(dbColumnName).ToList();
                ////                }
                ////                catch (Exception ex)
                ////                {
                ////                    Console.WriteLine(ex.Message);
                ////                }
                ////            }
            }
        }

        public void SqlGroupByFunc<T>() where T : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                //var group = db.Queryable<T>().GroupBy(it => it.Id)
                //.Having(it => SqlFunc.AggregateCount(it.Id) > 10)
                //.Select(it => new { id = SqlFunc.AggregateCount(it.Id) }).ToList();

                //var group = db.Queryable<T>().GroupBy(it => it.Id).GroupBy(it => it.Type)
                //.Having(it => SqlFunc.AggregateCount(it.Id) > 10)
                //.Select(it => new { id = SqlFunc.AggregateCount(it.Id) }).ToList();
                //.GroupBy(it => new { it.Id, it.Type })

                //var list3 = db.Queryable<T>()
                //.GroupBy(it => new { it.Id, it.Name }).Select(it => new { it.id, it.Name }).ToList();
                //// 性能优于 select distinct id,name from student所以我暂不支持distinct去重，结果是一样的
                //var list3 = db.Queryable<T>()
                //.PartitionBy(it => new { it.Id, it.Name }).Take(1).ToList();

            }
        }


        public void SqlToPageListFunc<T>() where T : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                //            var pageIndex = 1;
                //            var pageSize = 2;
                //            var totalCount = 0;
                //            var page = db.Queryable<T>().OrderBy(it => it.Id).ToPageList(pageIndex, pageSize, ref totalCount);
                //            var pageJoin = db.Queryable<T, School>((st, sc) => new object[] {
                //JoinType.Left,st.SchoolId==sc.Id
                //            }).ToPageList(pageIndex, pageSize, ref totalCount);
                //            var top5 = db.Queryable<T>().Take(5).ToList();
                //            var skip5 = db.Queryable<T>().Skip(5).ToList();
                //            db.Queryable<T>().OrderBy(it => it.Id).ToPageList(pageIndex, pageSize);
                //            //针对rowNumber分页的优化写法，该写法可以达到分页最高性能，非对性能要求过高的可以不用这么写
                //            var Tempdb = db.Queryable<T>();
                //            int count = Tempdb.Count();
                //            var Skip = (R.Page - 1) * R.PageCount;
                //            var Take = R.PageCount;
                //            if (R.Page * R.PageCount > P.Count / 2)//页码大于一半用倒序
                //            {
                //                Tempdb.OrderBy(x => x.ID, OrderByType.Desc);
                //                var Mod = P.Count % R.PageCount;
                //                var Page = (int)Math.Ceiling((Decimal)P.Count / R.PageCount);
                //                if (R.Page * R.PageCount >= P.Count)
                //                {
                //                    Skip = 0; Take = Mod == 0 ? R.PageCount : Mod;
                //                }
                //                else
                //                {
                //                    Skip = (Page - R.Page - 1) * R.PageCount + Mod;
                //                }
                //            }
                //            else
                //            {
                //                Tempdb.OrderBy(x => x.ID);//升序
                //            }
                //            Tempdb.Skip(Skip);
                //            Tempdb.Take(Take);
                //            var list = Tempdb.ToList();

            }
        }

        public void SqlCommitTranFunc<T>() where T : class, new()
        {


            SugarDbContext sugar = new SugarDbContext();
             using (var db = sugar.Db)
            {

                var result = sugar.Db.Ado.UseTran(() =>
                {
                    var beginCount = sugar.Db.Queryable<T>().Count();
                    sugar.Db.Ado.ExecuteCommand("delete student");
                    //throw new Exception("error haha"); 测试代码
                });

                // result.ErrorException
                // result.IsSuccess


                var result2 = sugar.Db.Ado.UseTran<List<T>>(() =>
                {
                    return sugar.Db.Queryable<T>().ToList();
                });
                // result.ErrorException
                // result.IsSuccess
                // result.Data

                try
                {
                    sugar.Db.Ado.BeginTran();
                    sugar.Db.Ado.CommitTran();
                }
                catch (Exception ex)
                {
                    sugar.Db.Ado.RollbackTran();
                    logger.Error("事务提交失败，已回滚" + ex.Message);
                }
                var asyncResult = db.Ado.UseTranAsync(() =>
                {
                    var beginCount = db.Queryable<T>().ToList();
                    db.Ado.ExecuteCommand("delete student");
                    var endCount = db.Queryable<T>().Count();
                    throw new Exception("error haha");
                });
                asyncResult.Wait();
                var asyncCount = db.Queryable<T>().Count();

                //有返回值和状态
                var asyncResult2 = db.Ado.UseTran<List<T>>(() =>
                {
                    return db.Queryable<T>().ToList();
                });
                // asyncResult2.Wait();

            }
        }

        public void SqlSqlqueryableFunc<T>() where T : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                //var t12 = db.SqlQueryable<Student>("select * from student").Where(it => it.id > 0).ToPageList(1, 2);
                //var t12 = db.SqlQueryable<dynamic>("select * from student").ToPageList(1, 2);//返回动态类型
                //var dt = db.Ado.SqlQuery<Student>("select * from table where id=@id and name=@name", new { id = 1, name = "a" });
                //var dt = db.Ado.SqlQuery<Student>("select * from table where id=@id and name=@name", 字典);
                //var dt = db.Ado.GetDataTable("select * from table where id=@id and name=@name", new List<SugarParameter>(){
                //new SugarParameter("@id",1),
                //new SugarParameter("@name",2)
                //});
                //var dt = db.Ado.GetDataTable("select * from table");
                //var dt = db.Ado.GetDataTable("select * from table where id=@id", new SugarParameter("@id", 1));
                //var dt = db.Ado.GetDataTable("select * from table where id=@id and name=@name", new SugarParameter[]{
                //new SugarParameter("@id",1),
                //new SugarParameter("@name",2)
                //});


                //1.获取DataTable

                //  db.Ado.GetDataTable(string sql, object parameters);
                //  db.Ado.GetDataTable(string sql, params SugarParameter[] parameters);
                //  db.Ado.GetDataTable(string sql, List < SugarParameter > parameters);


                //  2.获取DataSet

                //  db.Ado.GetDataSetAll(string sql, object parameters);
                //  db.Ado.GetDataSetAll(string sql, params SugarParameter[] parameters);
                //  db.Ado.GetDataSetAll(string sql, List < SugarParameter > parameters);


                //  3.获取DataReader

                //  db.Ado.GetDataReader(string sql, object parameters);
                //  db.Ado.GetDataReader(string sql, params SugarParameter[] parameters);
                //  db.Ado.GetDataReader(string sql, List < SugarParameter > parameters);


                //  4.获取首行首列返回object类型

                //  db.Ado.GetScalar(string sql, object parameters);
                //  db.Ado.GetScalar(string sql, params SugarParameter[] parameters);
                //  db.Ado.GetScalar(string sql, List < SugarParameter > parameters);


                //  5.执行数据库返回受影响行数

                //  int ExecuteCommand(string sql, object parameters);
                //  int ExecuteCommand(string sql, params SugarParameter[] parameters);
                //  int ExecuteCommand(string sql, List<SugarParameter> parameters);


                //  6.获取首行首列更多重载

                //  //以下为返回string
                //  string GetString(string sql, object parameters);
                //  string GetString(string sql, params SugarParameter[] parameters);
                //  string GetString(string sql, List<SugarParameter> parameters);

                //  //返回int
                //  int GetInt(string sql, object pars);
                //  int GetInt(string sql, params SugarParameter[] parameters);
                //  int GetInt(string sql, List<SugarParameter> parameters);

                //  //返回double
                //  db.Ado.GetDouble(string sql, object parameters);
                //  db.Ado.GetDouble(string sql, params SugarParameter[] parameters);
                //  db.Ado.GetDouble(string sql, List < SugarParameter > parameters);

                //  //返回decimal
                //  db.Ado.GetDecimal(string sql, object parameters);
                //  db.Ado.GetDecimal(string sql, params SugarParameter[] parameters);
                //  db.Ado.GetDecimal(string sql, List < SugarParameter > parameters);

                //  //返回DateTime
                //  db.Ado.GetDateTime(string sql, object parameters);
                //  db.Ado.GetDateTime(string sql, params SugarParameter[] parameters);
                //  db.Ado.GetDateTime(string sql, List < SugarParameter > parameters);


                //  7.查询并返回List < T >

                //  db.Ado.SqlQuery<T>(string sql, object whereObj = null);
                //  db.Ado.SqlQuery<T>(string sql, params SugarParameter[] parameters);
                //  db.Ado.SqlQuery<T>(string sql, List < SugarParameter > parameters);


                //  8.查询返回单条记录

                //  db.Ado.SqlQuerySingle<T>(string sql, object whereObj = null);
                //  db.Ado.SqlQuerySingle<T>(string sql, params SugarParameter[] parameters);
                //  db.Ado.SqlQuerySingle<T>(string sql, List < SugarParameter > parameters);


                //  9.查询返回动态类型(该类型为Newtonsoft.Json里面的JObject类型, 使用方法自行百度)

                //  db.Ado.SqlQueryDynamic(string sql, object whereObj = null);
                //  db.Ado.SqlQueryDynamic(string sql, params SugarParameter[] parameters);
                //  db.Ado.SqlQueryDynamic(string sql, List < SugarParameter > parameters);

            }
        }


        public void SqlStoredProcedureFunc<T>() where T : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {

                //db.Ado.GetInt("exec spName @p1", new { p = 1 })
                //var dt2 = db.Ado.UseStoredProcedure().GetDataTable("sp_school", new { name = "张三", age = 0 });//  GetInt SqlQuery<T>  等等都可以用

                //var nameP = new SugarParameter("@name", "张三");
                //var ageP = new SugarParameter("@age", null, true);//isOutput=true
                //var dt2 = db.Ado.UseStoredProcedure().GetDataTable("sp_school", nameP, ageP);
                //parameter.IsRefCursor = true;

                //string p = null;
                //SugarParameter[] pars = db.Ado.GetParameters(new { p = 1, p2 = p });
                //var p2 = pars[1].Direction = ParameterDirection.Output;

            }
        }
        public void SqlExecuteCommandFunc<T>(List<T>? insertObj) where T : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {

                var t61 = db.Insertable(insertObj).IgnoreColumns().ExecuteCommand();
                //var t2 = db.Insertable(insertObj).ExecuteCommand();
                //int t30 = db.Insertable(insertObj).ExecuteReturnIdentity();
                //                long t31 = db.Insertable(insertObj).ExecuteReturnBigIdentity(); 
                //                var t3 = db.Insertable(insertObj).ExecuteReturnEntity();
                //                var t3 = db.Insertable(insertObj).ExecuteCommandIdentityIntoEntity();
                //var t4 = db.Insertable(insertObj).InsertColumns(it => new { it.Name, it.SchoolId }).ExecuteReturnIdentity();
                //var t5 = db.Insertable(insertObj).IgnoreColumns(it => new { it.Name, it.TestId }).ExecuteReturnIdentity();
                //                var t6 = db.Insertable(insertObj).IgnoreColumns(it => it == "Name" || it == "TestId").ExecuteReturnIdentity();
                //                var t61 = db.Insertable(updateObj).IgnoreColumns(it => list.Contains(it)).ExecuteCommand();
                //                var t8 = db.Insertable(insertObj).With(SqlWith.UpdLock).ExecuteCommand();
                //                var t9 = db.Insertable(insertObj2)
                //                .Where(true/* Is no insert null */, true/*off identity*/)
                //                .ExecuteCommand();
                //var insertObjs = new List<Student>();
                //                var s9 = db.Insertable(insertObjs.ToArray()).ExecuteCommand();

                //                var t12 = db.Insertable<Student>(new { Name = "a" }).ExecuteCommand();
                //                //INSERT INTO [STudent]  ([Name]) VALUES ('a') ;SELECT SCOPE_IDENTITY();
                //                var t13 = db.Insertable<Student>(new Dictionary<string, object>() { { "name", "a" } }).ExecuteCommand();
                //                //INSERT INTO [STudent]  ([Name]) VALUES ('a') ;SELECT SCOPE_IDENTITY();

                //                var dt = new Dictionary<string, object>();
                //                dt.Add("name", "1");
                //                var t66 = db.Insertable(dt).AS("student").ExecuteReturnIdentity();

                //                [SugarColumn(IsOnlyIgnoreInsert = true)]
                // public DateTime CreateTime { get; set; }


                //        db.Insertable(db.Queryable<A>().Select<B>().ToList()).ExecuteCommand();
            }
        }
        public void SqlDeleteableFunc<T>() where T : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {

                //var t0 = db.Deleteable<Student>().Where(new Student() { Id = 1 }).ExecuteCommand();

                //                var  t1 = db.Deleteable<Student>().Where(new List<Student>() { new Student() { Id = 1 } }).ExecuteCommand();

                //                var t2 = db.Deleteable<Student>().With(SqlWith.RowLock).ExecuteCommand();


                //                var t3 = db.Deleteable<Student>().In(1).ExecuteCommand();


                //                var t4 = db.Deleteable<Student>().In(new int[] { 1, 2 }).ExecuteCommand();


                //var t4 = db.Deleteable<Student>().In(it => it.SchoolId, new int[] { 1, 2 }).ExecuteCommand();


                //                var t5 = db.Deleteable<Student>().Where(it => it.Id == 1).ExecuteCommand();//删除等于1的

                //                //批量删除非主键
                //                list<int> list = new list<int>() { 1, 3 };
                //                var t5 = db.Deleteable<Student>().Where(it => !list.Contains(it.Id)).ExecuteCommand();


                //db.Deleteable<Student>(1).ExecuteCommand();
                //                db.Deleteable<Student>(it => it.id == 1).ExecuteCommand();
                //                db.Deleteable<Student>(new int[] { 1, 2, 3 }).ExecuteCommand();
                //                db.Deleteable<Student>(实体).ExecuteCommand();

            }
        }
        public void SqlUpdateableFunc<T>() where T : class, new()
        {
            SugarDbContext sugar = new SugarDbContext();
            using (var db = sugar.Db)
            {
                //                db.Updateable(updateObj);
                //                db.Updateable<T>();
                //var t1 = db.Updateable(updateObj).ExecuteCommand(); //这种方式会以主键为条件       
                //                var t1 = db.Updateable(updateObj).WhereColumns(it => new { it.XId }).ExecuteCommand();//单列可以用 it=>it.XId
                //                var update = db.Updateable(updateObj).UpdateColumns(s => new { s.RowStatus, s.Id }).WhereColumns(it => new { it.Id });
                //var t3 = db.Updateable(updateObj).UpdateColumns(it => new { it.Name }).ExecuteCommand();

                //var t4 = db.Updateable(updateObj).IgnoreColumns(it => new { it.Name, it.TestId }).ExecuteCommand();
                //var t5 = db.Updateable(updateObj)
                //.IgnoreColumns(it => it == "name").ExecuteCommand(); 

                //                var t5 = db.Updateable(updateObj)
                //                .IgnoreColumns(it => list.Contains(it)).ExecuteCommand(); 
                //                var t6 = db.Updateable(updateObj).With(SqlWith.UpdLock).ExecuteCommand();
                //            List<Students> list = GetList();
                //                var t7 = db.Updateable(list).ExecuteCommand();

                //                var t8 = db.Updateable(updateObj)
                //                .ReSetValue(it => it.Name == (it.Name + 1)).ExecuteCommand();

                //                var t8 = db.Updateable(updateObj)
                //                .UpdateColumns(it => new { it.Name }).ReSetValue(it => it.Name == (it.Name + 1)).ExecuteCommand();

                //                var t9 = db.Updateable(updateObj).Where(it => it.Id == 1).ExecuteCommand();
                //                db.Updateable(updateObj).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommand();

                //                var dt = new Dictionary<string, object>();
                //                dt.Add("id", 1);
                //                dt.Add("name", "1");
                //                var t66 = db.Updateable(dt).AS("student").ExecuteCommand();
                //                //UPDATE STUDENT  SET  NAME=@name  WHERE ID=@ID

                //                //如果initkey.Attribute方式是拿不出主键信息的需要写成这样
                //                var dt = new Dictionary<string, object>();
                //                dt.Add("id", 1);
                //                dt.Add("name", "1");
                //                var t66 = db.Updateable(dt).AS("student").WhereColumns("id").With(SqlWith.UpdLock).ExecuteCommand()


                // var t13 = db.Updateable<Student>(new { Name = "a", id = 1 }).ExecuteCommand();
                //                //UPDATE [STudent]  SET [Name]='a' WHERE [Id]=1
                //                var t14 = db.Updateable<Student>(new Dictionary<string, object>() { { "id", 1 }, { "name", "a" } }).ExecuteCommand();

                //                 var t10 = db.Updateable<Student>()
                //                .UpdateColumns(it => new Student() { Name = "a", CreateTime = DateTime.Now })
                //                .Where(it => it.Id == 11).ExecuteCommand();

                //                var st = new Student() { Name = "a", CreateTime = DateTime.Now };
                //                var t10 = db.Updateable<Student>()
                //                .UpdateColumns(it => st)
                //                .Where(it => it.Id == 11).ExecuteCommand();

                //db.Updateable<School>().AS("Student")
                //.UpdateColumns(it => new School() { Name = "jack" })
                //.Where(it => it.Id == 1).ExecuteCommand();
                //                //Update Student set Name='jack' Where Id=1

                //                var t17 = db.Updateable<Student>().UpdateColumns(it =>
                //                       new Student()
                //                       {
                //                           SchoolId = SqlFunc.Subqueryable<School>().Where(s => s.Id == it.SchoolId).Select(s => s.Id),
                //                           Name = "newname"
                //                       }).Where(it => it.Id == 1).ExecuteCommand();

                //var t8 = db.Updateable<Student>().UpdateColumns(it => new Student() { Name = it.Name + 1 }).Where(it => it.Id == 11).ExecuteCommand();

                //                //写法2 (注意：4.5.9.8以上版本支持) 如果只有一列可以简化成这种写法 
                //                var t8 = db.Updateable<Student>().UpdateColumns(it => it.Name == it.Name + 1).Where(it => it.Id == 11).ExecuteCommand();


                //                //根据不同条件执行更新不同的列
                //                var t3 = db.Updateable(updateObj)
                //                                .UpdateColumnsIF(caseValue == "1", it => new { it.Name })
                //                                .UpdateColumnsIF(caseValue == "2", it => new { it.Name, it.CreateTime })
                //                                .ExecuteCommand();
            }
        }
    }

    public class SugarDbSqlHelper
    {
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
        public static List<U> GetListBySql<U>(string sql, string orderby = "") where U : class, new()
        {
            SugarDbContext sugar = new();
            List<U> result;
            using (var db = sugar.Db)
            {
                if (string.IsNullOrEmpty(orderby))
                {
                    result = db.SqlQueryable<U>(sql).ToList();
                }
                else
                {
                    result = db.SqlQueryable<U>(sql).OrderBy(orderby).ToList();
                }
            }
            return result;
        }
        public static List<U> GetListBySql<U>(string sql, string where, object parameters) where U : class, new()
        {
            SugarDbContext sugar = new();
            List<U> result;
            using (var db = sugar.Db)
            {
                result = db.SqlQueryable<U>(sql).Where(where, parameters).ToList();
            }
            return result;
        }


        public static U GetOneBySql<U>(string sql)
            where U : class, new()
        {
            SugarDbContext sugar = new();
            U result;
            using (var db = sugar.Db)
            {
                result = db.SqlQueryable<U>(sql).First();
            }
            return result;
        }

        public static int GetInt(string sql)
        {
            SugarDbContext sugar = new();
            using var db = sugar.Db;
            {
                return db.Ado.GetInt(sql);
            }
        }

        public static double GetDouble(string sql)
        {
            SugarDbContext sugar = new();
            using var db = sugar.Db;
            {
                return db.Ado.GetDouble(sql);
            }
        }

        public static E? PageOne<E>(string sql)
            where E : class, new()
        {
            SugarDbContext sugar = new();
            var db = sugar.Db;
            var one = db.SqlQueryable<E>(sql).ToList().FirstOrDefault();
            return one;
        }
        public static E? PageOne<E>(string sql, string where, object parameters)
           where E : class, new()
        {
            SugarDbContext sugar = new();
            if (parameters == null)
            {
                return PageOne<E>(sql);
            }

            var db = sugar.Db;
            var one = db.SqlQueryable<E>(sql).Where(where, parameters).ToList().FirstOrDefault();
            return one;
        }
        public static object ExecuteScalar(string sql, object? parameters = null)
        {
            SugarDbContext sugar = new();
            using var db = sugar.Db;
            {
                return db.Ado.GetScalar(sql, parameters);
            }
        }
        public static int ExecuteCommand(string sql, object? parameters = null)
        {
            SugarDbContext sugar = new();
            using var db = sugar.Db;
            {
                return db.Ado.ExecuteCommand(sql, parameters);
            }
        }

        public static object ExecuteScalar(string sql)
        {
            SugarDbContext sugar = new();
            using var db = sugar.Db;
            {
                return db.Ado.GetScalar(sql);
            }
        }
        public static async Task<object> ExecuteScalarAsync(string sql, object? parameters = null)
        {
            SugarDbContext sugar = new();
            using var db = sugar.Db;
            {
                return await db.Ado.GetScalarAsync(sql, parameters);
            }
        }
        public static async Task<object> ExecuteScalarAsync(string sql)
        {
            SugarDbContext sugar = new();
            using var db = sugar.Db;
            {
                return await db.Ado.GetScalarAsync(sql);
            }
        }
        public static E GetOneBySql<E>(string sql, object? parameters = null)
            where E : class
        {
            SugarDbContext sugar = new();
            using var db = sugar.Db;
            {
                return db.Ado.SqlQuerySingle<E>(sql, parameters);
            }

        }
        public static async Task<E> GetOneBySqlAsync<E>(string sql, object? parameters = null)
          where E : class
        {
            SugarDbContext sugar = new();
            using var db = sugar.Db;
            {
                return await db.Ado.SqlQuerySingleAsync<E>(sql, parameters);
            }
        }

        public static List<E> GetBySql<E>(string sql, object? parameters = null)
            where E : class
        {
            SugarDbContext sugar = new();
            using var db = sugar.Db;
            {
                return db.Ado.SqlQuery<E>(sql, parameters);
            }

        }

        public static async Task<List<E>> GetBySqlAsync<E>(string sql, object? parameters = null)
            where E : class
        {
            SugarDbContext sugar = new();
            using var db = sugar.Db;
            {
                return await db.Ado.SqlQueryAsync<E>(sql, parameters);
            }
        }

        public static void ExecTransaction(List<string> sqls)
        {
            SugarDbContext sugar = new();
            using var db = sugar.Db;
            {
                try
                {
                    db.Ado.BeginTran();
                    foreach (var item in sqls)
                    {
                        db.Ado.ExecuteCommand(item);
                    }
                    db.Ado.CommitTran();

                }
                catch (Exception)
                {
                    db.Ado.RollbackTran();

                }
            }
        }

    }

    public class SugarPGDBHelper
    {
        readonly SugarDbContext SDC;
        public SugarPGDBHelper()
        {
            SDC = new SugarDbContext();
            SDC.CreatePGTable();
        }

        public List<T> QueryableDistinct<T>() where T : class, new()
        {
            using var db = SDC.PGdb;
            return db.Queryable<T>().ToList();
            ////取前5条
            //var top5 = db.Queryable<T>().Take(5).ToList();
            ////无锁查询
            //var getAllNoLock = db.Queryable<T>().With(SqlWith.NoLock).ToList();
            ////根据主键查询
            //var getByPrimaryKey = db.Queryable<T>().InSingle("0000d82f-b1f2-4c7d-a3b9-6c70f9678282");
            ////查询单条没有数据返回NULL, Single超过1条会报错，First不会
            ////var getSingleOrDefault = db.Queryable<Student>().Single(); //会报错，数据量大于一条
            //var getSingleOrDefault = db.Queryable<T>().Where(A => A.StudentID == "0000d82f-b1f2-4c7d-a3b9-6c70f9678282").Single();
            //var getFirstOrDefault = db.Queryable<T>().First();
            ////UNION ALL Count = 2420838  240多万条数据
            //var UNIONLst = db.UnionAll<T>(db.Queryable<T>(), db.Queryable<T>()).ToList();
            ////in 查询
            //var in1 = db.Queryable<T>().In(A => A.StudentID, new string[] { "000136bf-f968-4a59-9091-bae8ebca42fb", "00020ba7-44e6-494c-8fcb-c1be288a39b3" }).ToList();
            ////主键 In (1,2,3)  不指定列, 默认根据主键In
            //var in2 = db.Queryable<T>().In(new string[] { "000136bf-f968-4a59-9091-bae8ebca42fb", "00020ba7-44e6-494c-8fcb-c1be288a39b3" }).ToList();
            ////in 查询
            //List<string> array = new List<string> { "000136bf-f968-4a59-9091-bae8ebca42fb", "00020ba7-44e6-494c-8fcb-c1be288a39b3" };
            //var in3 = db.Queryable<T>().Where(it => array.Contains(it.StudentID)).ToList();
            ////not in
            //var in4 = db.Queryable<T>().Where(it => !array.Contains(it.StudentID)).ToList();
            ////where
            //var getByWhere = db.Queryable<T>().Where(it => it.StudentID == "000136bf-f968-4a59-9091-bae8ebca42fb" || it.StudentName == "陈丽").ToList();
            ////SqlFunc
            //var getByFuns = db.Queryable<T>().Where(it => SqlFunc.IsNullOrEmpty(it.StudentName)).ToList();
            ////between and 
            //var between = db.Queryable<T>().Where(it => SqlFunc.Between(it.CreateTime, DateTime.Now.AddDays(-10), DateTime.Now)).ToList();
            ////排序
            //var getAllOrder = db.Queryable<T>().Take(100).OrderBy(it => it.CreateTime).ToList(); //默认为ASC排序
            //                                                                                           //组合排序
            //var data = db.Queryable<T>()
            //    .OrderBy(it => it.StudentName, OrderByType.Asc)
            //    .OrderBy(it => it.CreateTime, OrderByType.Desc)
            //    .ToList();

            ////是否存在 any
            //var isAny = db.Queryable<T>().Where(it => it.StudentName == "张龙").Any();
            //var isAny2 = db.Queryable<T>().Any(it => it.StudentSex == "女");
            ////获取同一天的记录
            //var getTodayList = db.Queryable<T>().Where(it => SqlFunc.DateIsSame(it.CreateTime, DateTime.Now)).ToList();

            //Console.ReadLine();

        }

        public void SqlExecuteCommandFunc<T>(List<T> insertObj) where T : class, new()
        {
            using var db = SDC.PGdb;
            var t61 = db.Insertable(insertObj).IgnoreColumns().ExecuteCommand();
            //var t2 = db.Insertable(insertObj).ExecuteCommand();
            //int t30 = db.Insertable(insertObj).ExecuteReturnIdentity();
            //                long t31 = db.Insertable(insertObj).ExecuteReturnBigIdentity(); 
            //                var t3 = db.Insertable(insertObj).ExecuteReturnEntity();
            //                var t3 = db.Insertable(insertObj).ExecuteCommandIdentityIntoEntity();
            //var t4 = db.Insertable(insertObj).InsertColumns(it => new { it.Name, it.SchoolId }).ExecuteReturnIdentity();
            //var t5 = db.Insertable(insertObj).IgnoreColumns(it => new { it.Name, it.TestId }).ExecuteReturnIdentity();
            //                var t6 = db.Insertable(insertObj).IgnoreColumns(it => it == "Name" || it == "TestId").ExecuteReturnIdentity();
            //                var t61 = db.Insertable(updateObj).IgnoreColumns(it => list.Contains(it)).ExecuteCommand();
            //                var t8 = db.Insertable(insertObj).With(SqlWith.UpdLock).ExecuteCommand();
            //                var t9 = db.Insertable(insertObj2)
            //                .Where(true/* Is no insert null */, true/*off identity*/)
            //                .ExecuteCommand();
            //var insertObjs = new List<Student>();
            //                var s9 = db.Insertable(insertObjs.ToArray()).ExecuteCommand();

            //                var t12 = db.Insertable<Student>(new { Name = "a" }).ExecuteCommand();
            //                //INSERT INTO [STudent]  ([Name]) VALUES ('a') ;SELECT SCOPE_IDENTITY();
            //                var t13 = db.Insertable<Student>(new Dictionary<string, object>() { { "name", "a" } }).ExecuteCommand();
            //                //INSERT INTO [STudent]  ([Name]) VALUES ('a') ;SELECT SCOPE_IDENTITY();

            //                var dt = new Dictionary<string, object>();
            //                dt.Add("name", "1");
            //                var t66 = db.Insertable(dt).AS("student").ExecuteReturnIdentity();

            //                [SugarColumn(IsOnlyIgnoreInsert = true)]
            // public DateTime CreateTime { get; set; }
            //        db.Insertable(db.Queryable<A>().Select<B>().ToList()).ExecuteCommand();
        }
    }




    public class SugarDbContext
    {
        /// 获取连接字符串        
        // private static string Connection ="Data Source=.;User Id=sa;Password=201015;Initial Catalog=Stock;TrustServerCertificate=true;Pooling=true;Min Pool Size=1";
        private readonly string PostgreSQLConnection = "PORT=5432;DATABASE=stock;HOST=192.168.1.70;PASSWORD=201015;USER ID=postgres";

        private static string Connection = "Data Source = localhost; Port=3306;User ID = root; Password=123456;Initial Catalog =stock; Charset=utf8;SslMode=none;Max pool size=10";


        //  private static string Connection = "Data Source = 192.168.1.70; Port=3306;User ID = root; Password=123456;Initial Catalog =stock; Charset=utf8;SslMode=none;Max pool size=10";
        public SugarDbContext()
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Connection,
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了
            });
            //调式代码 用来打印SQL 
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                //Console.WriteLine(sql + "\r\n" +
                //    Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                //Console.WriteLine();
            };


            PGdb = new SqlSugarClient(new ConnectionConfig()
            {
                ConfigId = 1,
                DbType = DbType.PostgreSQL,
                ConnectionString = PostgreSQLConnection,
                IsAutoCloseConnection = true,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    EntityService = (x, p) => //处理列名
                    {
                        //最好排除DTO类
                        p.DbColumnName = UtilMethods.ToUnderLine(p.DbColumnName);//ToUnderLine驼峰转下划线方法
                    },
                    EntityNameService = (x, p) => //处理表名
                    {
                        //最好排除DTO类
                        p.DbTableName = UtilMethods.ToUnderLine(p.DbTableName);//ToUnderLine驼峰转下划线方法
                    }
                }
            });

        }
        //注意：不能写成静态的

        public SqlSugarClient PGdb;

        public SqlSugarClient Db;

        public void CreatePGTable()
        {
            PGdb.DbMaintenance.CreateDatabase();//没有数据库则新建
            PGdb.CodeFirst.Context.CodeFirst.InitTables(
          //  typeof(ShareholdersOverseas)
            );
        }

        public void CreateTable()
        {
            Db.DbMaintenance.CreateDatabase();//没有数据库则新建

            Db.CodeFirst.Context.CodeFirst.InitTables(
            //typeof(stockinfos),
            //typeof(Users),
            //typeof(StockInfo),
            //typeof(AllDataRateInfo),
            //typeof(TenDayInfo),
            //typeof(ff)
            );

            //Db.CodeFirst.SetStringDefaultLength(50).BackupTable().InitTables(new Type[]
            //{
            //   typeof(CodeFirstTable1),   
            //});
        }

        public bool IsExist<T>() where T : class, new()
        {
            Type type = typeof(T);
            var attr = type.GetCustomAttributes(typeof(SugarTable), true).FirstOrDefault() as SugarTable;
            string tableName = attr?.TableName ?? type.Name;
            return Db.DbMaintenance.IsAnyTable(tableName);
        }



        /// <summary>
        /// //参数1：路径  参数2：命名空间
        //IsCreateAttribute 代表生成SqlSugar特性
        /// </summary>
        public void CreateClassFile()
        {
            Db.DbFirst.IsCreateAttribute().CreateClassFile(@"E:\\GitHub\\StockInfo\\StockCommonLib", "Models");
        }
    }


    public class CodeFirstTable1
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [SugarColumn(ColumnDataType = "Nvarchar(255)")]
        public string Text { get; set; } = string.Empty;
        [SugarColumn(IsNullable = true)]
        public DateTime CreateTime { get; set; }
    }

}

