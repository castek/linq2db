﻿using System;
using System.Linq;

using LinqToDB;

using NUnit.Framework;

namespace Tests.Linq
{
	[TestFixture]
	public class ArrayTableTests : TestBase
	{
		[Test, IncludeDataContextSource(ProviderName.SqlServer2008, ProviderName.SqlServer2012, ProviderName.SqlServer2014, ProviderName.PostgreSQL)]
		public void ApplyJoinArray(string context)
		{
			var doe = "Doe";
			using (var db = GetDataContext(context))
			{
				var q =
					from p in db.Person
					from n in new[] {p.FirstName, p.LastName, "John", doe}
					select n;

				var result = q.ToList();

				var expected =
					from p in Person
					from n in new[] {p.FirstName, p.LastName, "John", doe}
					select n;

				AreEqual(expected, result);
			}
		}

		[Test, DataContextSource(ProviderName.Access, ProviderName.DB2, ProviderName.Informix)]
		public void InnerJoinArray(string context)
		{
			var doe = "Doe";
			using (var db = GetDataContext(context))
			{
				var q =
					from p in db.Person
					join n in new[] {"Janet", "Doe", "John", doe}.AsQueryable() on p.LastName equals n
					select p;

				var result = q.ToList();

				var expected =
					from p in Person
					join n in new[] {"Janet", "Doe", "John", doe}.AsQueryable() on p.LastName equals n
					select p;

				AreEqual(expected, result);
			}
		}

		[Test, DataContextSource(ProviderName.Access, ProviderName.DB2, ProviderName.Informix)]
		public void InnerJoinArray2(string context)
		{
			var doe = "Doe";
			using (var db = GetDataContext(context))
			{
				var q =
					from p in db.Person
					join n in new[] {"Janet", "Doe", "John", doe} on p.LastName equals n
					select p;

				var result = q.ToList();

				var expected =
					from p in Person
					join n in new[] {"Janet", "Doe", "John", doe} on p.LastName equals n
					select p;

				AreEqual(expected, result);
			}
		}

		[Test, DataContextSource(ProviderName.Access, ProviderName.DB2, ProviderName.Informix)]
		public void InnerJoinArray3(string context)
		{
			var doe = "Doe";

			using (var db = GetDataContext(context))
			{
				for (var i = 0; i < 2; i++)
				{
					if (i > 0)
						doe += i;

					var q =
						from p in db.Person
						join n in new[] {"Janet", "Doe", "John", doe} on p.LastName equals n
						select p;

					var result = q.ToList();

					var expected =
						from p in Person
						join n in new[] {"Janet", "Doe", "John", doe} on p.LastName equals n
						select p;

					AreEqual(expected, result);
				}
			}
		}

		[Test, DataContextSource(ProviderName.Access, ProviderName.DB2, ProviderName.Informix)]
		public void InnerJoinArray4(string context)
		{
			var doe = "Doe";
			var arr = new[] {"Janet", "Doe", "John", doe};

			using (var db = GetDataContext(context))
			{
				for (var i = 0; i < 2; i++)
				{
					if (i > 0)
						arr[1] += i;

					var q =
						from p in db.Person
						join n in arr on p.LastName equals n
						select p;

					var result = q.ToList();

					var expected =
						from p in Person
						join n in arr on p.LastName equals n
						select p;

					AreEqual(expected, result);
				}
			}
		}

		[Test, DataContextSource(ProviderName.Access, ProviderName.DB2, ProviderName.Informix)]
		public void InnerJoinArray5(string context)
		{
			var doe = "Doe";

			using (var db = GetDataContext(context))
			{
				var q =
					from n in new[] {"Janet", "Doe", "John", doe}.AsQueryable()
					join p in db.Person on n equals p.LastName
					select p;

				var result = q.ToList();
				var sql    = q.ToString();

				Assert.That(sql, Contains.Substring("JOIN"));

				var expected =
					from n in new[] {"Janet", "Doe", "John", doe}.AsQueryable()
					join p in Person on n equals p.LastName
					select p;

				AreEqual(expected, result);
			}
		}

		[Test, DataContextSource(ProviderName.Access)]
		public void InnerJoinArray6(string context)
		{
			using (var db = GetDataContext(context))
			{
				var q =
					from p in db.Person
					join n in new[] { "Doe" } on p.LastName equals n
					select p;

				var result = q.ToList();

				var expected =
					from p in Person
					join n in new[] { "Doe" } on p.LastName equals n
					select p;

				AreEqual(expected, result);
			}
		}

		[Test, DataContextSource(ProviderName.Access, ProviderName.DB2, ProviderName.Informix)]
		public void InnerJoinClassArray(string context)
		{
			using (var db = GetDataContext(context))
			{
				var q =
					from p in db.Person
					join n in new[]
					{
						new { ID = 1, Name = "Janet" },
						new { ID = 1, Name = "Doe" },
					} on p.LastName equals n.Name
					select p;

				var result = q.ToList();

				var expected =
					from p in Person
					join n in new[]
					{
						new { ID = 1, Name = "Janet" },
						new { ID = 1, Name = "Doe" },
					} on p.LastName equals n.Name
					select p;

				AreEqual(expected, result);
			}
		}

	}
}