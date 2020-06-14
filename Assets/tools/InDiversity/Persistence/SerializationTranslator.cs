using System.Collections.Generic;

namespace io.daniellanner.indiversity
{
	internal class SerializationTranslator
	{
		private SerializedContainer _frontContainer = new SerializedContainer();
		private SerializedContainer _backContainer = new SerializedContainer();

		private bool _initialized = false;
		internal bool Initialized => _initialized;

		#region Helpers
		private Result<T> Get<T>(string key, Dictionary<string, T> dict)
		{
			if (dict.TryGetValue(key, out T oint))
			{
				return new Result<T>(oint).AsSafe();
			}

			return Result<T>.FALSE;
		}

		private Result<T> Set<T>(string key, T value, Dictionary<string, T> dict)
		{
			if (dict.ContainsKey(key))
			{
				dict[key] = value;
			}
			else
			{
				dict.Add(key, value);
			}

			return Result<T>.FALSE;
		}
		#endregion

		#region Getter Setter
		internal Result<int> GetInt(string key) => Get(key, _frontContainer.DictInteger);
		internal bool SetInt(string key, int value) => Set(key, value, _frontContainer.DictInteger);

		internal Result<float> GetFloat(string key) => Get(key, _frontContainer.DictFloat);
		internal bool SetFloat(string key, float value) => Set(key, value, _frontContainer.DictFloat);

		internal Result<bool> GetBool(string key) => Get(key, _frontContainer.DictBool);
		internal bool SetBool(string key, bool value) => Set(key, value, _frontContainer.DictBool);

		internal Result<string> GetString(string key) => Get(key, _frontContainer.DictString);
		internal bool SetString(string key, string value) => Set(key, value, _frontContainer.DictString);
		#endregion

		#region Methods
		internal void SetDataFromDisk(SerializedContainer p_data, bool p_hardReload = false)
		{
			_backContainer = p_data;

			if (p_hardReload || !_initialized)
			{
				// TODO: (Daniel) check if the containers are ident before overwriting the session data
				_frontContainer = p_data;
				_initialized = true;
			}
		}

		internal SerializedContainer GetSessionData()
		{
			return _frontContainer;
		}
		#endregion
	}
}
