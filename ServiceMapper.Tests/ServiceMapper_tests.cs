using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ServiceMapper.Tests
{
	[TestFixture]
	public class ServiceMapper_tests
	{
		private Mock<IMapper> _mapper;

		[SetUp]
		public void Setup()
		{
			_mapper = new Mock<IMapper>();
		}

		[Test]
		public void DoesNotMapIgnored()
		{
			IList<Map> maps = new List<Map>();
			_mapper.Setup(x => x.Map(It.IsAny<Map>())).Callback((Map m) => maps.Add(m)).Returns(null);
			var map = ServiceMapper.Create(x =>
			{
				return GetType().Assembly == x.Assembly;
			}, _mapper.Object)
			.Override<ITest>(x =>
			{
				x.Location("/Dummy/Data.svc");
				x.Binding("SomeBinding");
			})
			.Override<ITest2>(x => x.Ignore())
			.Map();

			_mapper.Verify(x => x.Map(It.IsAny<Map>()), Times.AtLeastOnce);
			Assert.That(maps.Any(x => x.Type == typeof(ITest)), Is.True);
			Assert.That(maps.Any(x => x.Type == typeof(ITest2)), Is.False);
		}
	}
	interface ITest{ };
	interface ITest2{ };
}
