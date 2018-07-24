using System.ServiceModel.Activation;

namespace ServiceMapper.Mappers
{
	public class ServiceRouteMapper : IMapper
	{
		private readonly INameMapper _nameMapper;
		private readonly ServiceHostFactory _serviceHostFactory;

		public ServiceRouteMapper(INameMapper nameMapper, ServiceHostFactory serviceHostFactory)
		{
			_nameMapper = nameMapper;
			_serviceHostFactory = serviceHostFactory;
		}
		public object Map(Map type)
		{
			return new ServiceRoute(_nameMapper.Map(type), _serviceHostFactory, type.Type);
		}
	}
}
