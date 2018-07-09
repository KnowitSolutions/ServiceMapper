using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace ServiceMapper.Tests
{
	[TestFixture]
	public class ServiceMapper_tests
	{
		private Mock<IMapper> _mapper;
		IList<Map> maps;

		[SetUp]
		public void Setup()
		{
			_mapper = new Mock<IMapper>();
			maps = new List<Map>();
			_mapper.Setup(x => x.Map(It.IsAny<Map>())).Callback((Map m) => maps.Add(m)).Returns(null);
		}

		[Test]
		public void Maps_all_not_ignored()
		{
			IDictionary<Type, object> map = DefaultMap().Map();
			_mapper.Verify(x => x.Map(It.IsAny<Map>()), Times.AtLeast(2));
			Assert.That(maps.Any(x => x.Type == typeof(ITest)), Is.True);
			Assert.That(maps.Any(x => x.Type == typeof(ITest2)), Is.True);
		}

		[Test]
		public void DoesNotMapIgnored()
		{
			IDictionary<Type, object> map = DefaultMap()
				.Override<ITest2>(x => x.Ignore())
				.Map();
			_mapper.Verify(x => x.Map(It.IsAny<Map>()), Times.AtLeastOnce);
			Assert.That(maps.Any(x => x.Type == typeof(ITest)), Is.True);
			Assert.That(maps.Any(x => x.Type == typeof(ITest2)), Is.False);
		}

		private IServiceMap DefaultMap()
		{
			return ServiceMap.Create(x =>
			{
				return GetType().Assembly == x.Assembly;
			}, _mapper.Object);
		}
	}
	[ServiceContract]
	interface ITest {
		[OperationContract]
		void test1();
	};

	[ServiceContract]
	interface ITest2 {
		[OperationContract]
		void test2();
	};
}
