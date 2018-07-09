using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace ServiceMapper.Bindings
{
	public class DefaultBindings
	{
		private static readonly TimeSpan TenMinutes = new TimeSpan(0, 10, 0);
		private static readonly int _size = 41943040;
		private static readonly int _largeSize = int.MaxValue;
		public static Binding GetBasicHttpBinding(bool https)
		{
			var binding = new BasicHttpBinding
			{
				Name = "DefaultHttpBinding",
				OpenTimeout = TenMinutes,
				CloseTimeout = TenMinutes,
				ReceiveTimeout = TenMinutes,
				SendTimeout = TenMinutes,
				MaxReceivedMessageSize = _size,
				ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
				{
					MaxStringContentLength = _size,
					MaxArrayLength = _size,
					MaxDepth = 32,
					MaxBytesPerRead = 4096,
					MaxNameTableCharCount = _size
				},

				TextEncoding = Encoding.UTF8,
				MaxBufferSize = _size,
				MaxBufferPoolSize = 524288,
				MessageEncoding = WSMessageEncoding.Text,
			};
			if (https)
				binding.Security = new BasicHttpSecurity
				{
					Mode = BasicHttpSecurityMode.Transport
				};
			return binding;
		}

		public static Binding GetBinaryBinding(bool https)
		{
			var binding = new CustomBinding
			{
				Name = "BinaryHttpBinding",
				CloseTimeout = TenMinutes,
				ReceiveTimeout = TenMinutes,
				OpenTimeout = TenMinutes,
				SendTimeout = TenMinutes,
			};
			binding.Elements.Add(new BinaryMessageEncodingBindingElement
			{
				ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
				{
					MaxArrayLength = _largeSize,
					MaxBytesPerRead = _largeSize,
					MaxDepth = _largeSize,
					MaxNameTableCharCount = _largeSize,
					MaxStringContentLength = _largeSize
				}
			});
			if (https)
				binding.Elements.Add(new HttpsTransportBindingElement
				{
					MaxReceivedMessageSize = _largeSize,
					MaxBufferSize = _largeSize
				});
			else
				binding.Elements.Add(new HttpTransportBindingElement
				{
					MaxReceivedMessageSize = _largeSize,
					MaxBufferSize = _largeSize,
					MaxBufferPoolSize = _largeSize
				});
			return binding;
		}
	}
}
