using Moq;
using NUnit.Framework;
using ServiceMapper.Bindings;
using ServiceMapper.Mappers;

namespace ServiceMapper.Tests
{
	[TestFixture]
	class ProxyMapper_tests
	{
		IMapper _subject;
		Mock<INameMapper> _nameMapper;
		[SetUp]
		public void Setup()
		{
			_nameMapper = new Mock<INameMapper>();
			_nameMapper.Setup(x => x.Map(It.IsAny<Map>())).Returns("http://example.com/Services/service.svc");
			_subject = new ProxyMapper(DefaultBindings.GetBasicHttpBinding(false), null, _nameMapper.Object);
		}
		[Test]
		public void DoesNotCrash()
		{
			ServiceMap.Create(x =>
			{
				return GetType().Assembly == x.Assembly;
			}, _subject).Map();
		}
	}
}
