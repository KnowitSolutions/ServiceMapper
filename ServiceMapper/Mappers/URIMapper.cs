namespace ServiceMapper.Mappers
{
	public class UriMapper : IMapper
	{
		private readonly INameMapper _nameMapper;

		public UriMapper(INameMapper nameMapper)
		{
			_nameMapper = nameMapper;
		}
		public object Map(Map type)
		{
			return _nameMapper.Map(type);
		}
	}
}
