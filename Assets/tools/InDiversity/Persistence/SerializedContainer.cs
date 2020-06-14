using System.Collections.Generic;

namespace io.daniellanner.indiversity
{
	[System.Serializable]
	public class SerializedContainer
	{
		private Dictionary<string, int> _integer = new Dictionary<string, int>();
		public Dictionary<string, int> DictInteger => _integer;

		private Dictionary<string, float> _float = new Dictionary<string, float>();
		public Dictionary<string, float> DictFloat => _float;

		private Dictionary<string, bool> _bool = new Dictionary<string, bool>();
		public Dictionary<string, bool> DictBool => _bool;

		private Dictionary<string, string> _string = new Dictionary<string, string>();
		public Dictionary<string, string> DictString => _string;
	}
}