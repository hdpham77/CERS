using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace UPF
{
	/// <summary>
	/// Helper class to assist with common XML serialization scenarios.
	/// </summary>
	public static class XmlSerialization
	{
		/// <summary>
		/// Serialize's an object to a base 64 string.
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> of the object to serialize</typeparam>
		/// <param name="input">The instance of the <typeparamref name="T"/> to serialize.</param>
		/// <returns>A base 64 encoded string of the serialized object.</returns>
		public static string SerializeToBase64String<T>(T input)
		{
			string result = string.Empty;
			string rawXml = SerializeToXmlString<T>(input);

			if (rawXml != null && rawXml.Length > 0)
			{
				byte[] xmlData = Encoding.UTF8.GetBytes(rawXml);
				result = Convert.ToBase64String(xmlData);
			}

			return result;
		}

		/// <summary>
		/// Serialize's an object to an xml string.
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> of the object to serialize</typeparam>
		/// <param name="input">The instance of the <typeparamref name="T"/> to serialize.</param>
		/// <returns>A <see cref="String"/> containing the raw xml of the serialized object.</returns>
		public static string SerializeToXmlString<T>(T input)
		{
			string result = string.Empty;
			MemoryStream targetStream = new MemoryStream();
			SerializeToStream<T>(targetStream, input);
			targetStream.Position = 0;
			using (StreamReader reader = new StreamReader(targetStream))
			{
				result = reader.ReadToEnd();
			}
			return result;
		}

		/// <summary>
		/// Serializes an object to a stream.
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> of the object to serialize.</typeparam>
		/// <param name="stream">The <see cref="Stream"/> to serialize the instance to.</param>
		/// <param name="theObject">The instance of the <typeparamref name="T"/> to serialize.</param>
		public static void SerializeToStream<T>(Stream stream, T theObject)
		{
			stream.Position = 0;
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			serializer.Serialize(stream, theObject);
			stream.Position = 0;
		}

		/// <summary>
		/// Deserializes an a base64 string to a <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> of the object to deserialize to.</typeparam>
		/// <param name="base64">The <see cref="String"/> containing the base64 encoded string to deserialize back to an object.</param>
		/// <returns>An instance of the <typeparamref name="T"/> containing the data from the serialized input.</returns>
		public static T DeserializeFromBase64String<T>(string base64)
		{
			byte[] input = Convert.FromBase64String(base64);
			MemoryStream targetStream = new MemoryStream(input);
			return DeserializeFromStream<T>(targetStream);
		}

		/// <summary>
		/// Deserializes a raw xml string to a <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> of the object to deserialize to.</typeparam>
		/// <param name="xmlString">The <see cref="String"/> containing the raw xml string to deserialize back to an object.</param>
		/// <returns>An instance of the <typeparamref name="T"/> containing the data from the serialized input.</returns>
		public static T DeserializeFromXmlString<T>(string xmlString)
		{
			byte[] input = Encoding.UTF8.GetBytes(xmlString);
			MemoryStream targetStream = new MemoryStream(input);
			return DeserializeFromStream<T>(targetStream);
		}

		/// <summary>
		/// Deserializes an a <see cref="Stream"/> to a <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> of the object to deserialize to.</typeparam>
		/// <param name="stream">An existing <see cref="Stream"/> containing the serialized data to deserialize to.</param>
		/// <returns>An instance of the <typeparamref name="T"/> containing the data from the serialized input.</returns>
		public static T DeserializeFromStream<T>(Stream stream)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			return (T)serializer.Deserialize(stream);
		}

		public static void OutputStreamContent(Stream stream)
		{
			StreamReader reader = new StreamReader(stream);
			string xml = reader.ReadToEnd();
			stream.Position = 0;
		}

	}
}
