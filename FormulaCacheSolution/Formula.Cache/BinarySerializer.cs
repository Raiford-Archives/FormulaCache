using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Formula.Cache
{
	public class BinarySerializer
	{
		public static byte[] ToByteArray(object obj)
		{
			if (obj == null)
				return null;

			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();
			formatter.Serialize(stream, obj);
			return stream.ToArray();
		}

		public static object ToObject(byte[] bytes)
		{
			MemoryStream stream = new MemoryStream();
			BinaryFormatter formatter = new BinaryFormatter();
			stream.Write(bytes, 0, bytes.Length);
			stream.Seek(0, SeekOrigin.Begin);
			object obj = (object)formatter.Deserialize(stream);
			return obj;
		}
	}
}
