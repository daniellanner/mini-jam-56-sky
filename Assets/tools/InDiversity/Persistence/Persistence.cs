using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace io.daniellanner.indiversity
{
	public class Persistence
	{
		#region Singleton
		private static Persistence Instance => NestedInstance._instance;

		private class NestedInstance
		{
			static NestedInstance() { }
			internal static readonly Persistence _instance = new Persistence();
		}

		private Persistence()
		{
			Init();
		}
		#endregion

		private const string FILE_EXTENSION = ".iddat";
		private string _fileName;
		private string _filePath;
		private SerializationTranslator _translationUnit;
		
		private string PathConcat => _filePath + Path.DirectorySeparatorChar + _fileName + FILE_EXTENSION;

		#region Helper
		private void Init()
		{
			_fileName = "game";
			_filePath = Application.persistentDataPath;
			_translationUnit = new SerializationTranslator();
		}

		private bool WriteFile()
		{
			try
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Create(PathConcat);

				bf.Serialize(file, _translationUnit.GetSessionData());
				file.Close();

				return true;
			}
			catch(System.Exception e)
			{
				Debug.LogError("Failed to write File.\n" + e.StackTrace);
				return false;
			}
		}

		private bool ReadFile(bool p_hardReload = false)
		{
			bool exists = File.Exists(PathConcat);

			if (exists)
			{
				try
				{
					BinaryFormatter bf = new BinaryFormatter();
					FileStream file = File.Open(PathConcat, FileMode.Open);

					var container = (SerializedContainer)bf.Deserialize(file);
					_translationUnit.SetDataFromDisk(container, p_hardReload);

					file.Close();
					return true;
				}
				catch(System.Exception e)
				{
					Debug.LogError("Failed to read File.\n" + e.StackTrace);
					return false;
				}
			}

			return false;
		}
		#endregion

		#region Exposed Methods
		public static void SaveFileAs(string fileName)
		{
			//TODO: input sanitation

			Instance._fileName = fileName;
		}

		public static void SaveFileAtLocation(string filePath)
		{
			Instance._filePath = filePath;
		}

		public static void Save()
		{
			Instance.WriteFile();
		}

		public static bool Load()
		{
			return Instance.ReadFile();
		}

		public static bool IsLoaded => Instance._translationUnit.Initialized;
		#endregion

		#region Getter Setter
		public static Result<int> GetInt(string p_key) => Instance._translationUnit.GetInt(p_key);
		public static bool SetInt(string p_key, int p_value) => Instance._translationUnit.SetInt(p_key, p_value);

		public static Result<float> GetFloat(string p_key) => Instance._translationUnit.GetFloat(p_key);
		public static bool SetFloat(string p_key, float p_value) => Instance._translationUnit.SetFloat(p_key, p_value);

		public static Result<bool> GetBool(string p_key) => Instance._translationUnit.GetBool(p_key);
		public static bool SetBoolInt(string p_key, bool p_value) => Instance._translationUnit.SetBool(p_key, p_value);

		public static Result<string> GetString(string p_key) => Instance._translationUnit.GetString(p_key);
		public static bool SetString(string p_key, string p_value) => Instance._translationUnit.SetString(p_key, p_value);
		#endregion
	}
}