using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
namespace ServiceMapper.Mappers
{
	public class ProxyMapper : IMapper
	{
		private ProxyGenerator _generator;
		private readonly INameMapper _nameMapper;
		private readonly Binding _binding;
		private readonly IEndpointBehavior _endpointBehavior;

		public ProxyMapper(Binding binding, IEndpointBehavior endpointBehavior, INameMapper nameMapper)
		{
			_generator = new ProxyGenerator();
			_nameMapper = nameMapper;
			_binding = binding;
			_endpointBehavior = endpointBehavior;
		}
		public object Map(Map type)
		{
			var URI = _nameMapper.Map(type);
			var channel = _generator.GetType().GetMethod("GenerateProxy").MakeGenericMethod(type.Type).Invoke(_generator, new object[] { URI, type.Binding ?? _binding, _endpointBehavior});
			return Convert.ChangeType(channel, type.Type);
		}
	}

	public class ProxyGenerator
	{
		private static readonly ConcurrentDictionary<string, ChannelFactory> _channels = new ConcurrentDictionary<string, ChannelFactory>();
		public T GenerateProxy<T>(string URI, Binding binding, IEndpointBehavior endpointBehavior)
		{
			var channel = GetChannelFactory<T>(URI, binding, endpointBehavior);
			return ((ChannelFactory<T>)channel).CreateChannel();
		}
		private ChannelFactory GetChannelFactory<T>(string URI, Binding binding, IEndpointBehavior endpointBehavior)
		{
			string key = typeof(T).FullName + URI;
			return _channels.GetOrAdd(key, k =>
			{
				var channel = new ChannelFactory<T>(binding);
				if (endpointBehavior != null)
					channel.Endpoint.Behaviors.Add(endpointBehavior);
				channel.Endpoint.Address = new EndpointAddress(URI);
				return channel;
			});
		}
	}
}
