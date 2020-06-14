using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System;

namespace io.daniellanner.indiversity
{
	public class PersistanceEditorWindow : EditorWindow
	{
		private class ContainerState
		{
			internal bool _show = true;
			internal string _name = "New Entry";
			internal bool _addElement = false;
		}

		private static string _saveFileLocation;

		private ContainerState IntegerState = new ContainerState();
		private ContainerState FloatState = new ContainerState();
		private ContainerState BoolState = new ContainerState();
		private ContainerState StringState = new ContainerState();

		// Add menu named "My Window" to the Window menu
		[MenuItem("Window/Persistence Inspector")]
		static void Init()
		{
			// Get existing open window or if none, make a new one:
			PersistanceEditorWindow window = (PersistanceEditorWindow)EditorWindow.GetWindow(typeof(PersistanceEditorWindow));
			window.Show();
		}

		private static SerializedContainer _data = new SerializedContainer();

		void OnGUI()
		{
			IntegerState._show = EditorGUILayout.Foldout(IntegerState._show, "Stored Integer");
			if (IntegerState._show)
			{
				DrawElements(_data.DictInteger, (string s, int i) => { return EditorGUILayout.IntField(s, i); });
				DrawTmpAdd(_data.DictInteger, ref IntegerState._name, ref IntegerState._addElement);
			}

			EditorGUILayout.Space();
			FloatState._show = EditorGUILayout.Foldout(FloatState._show, "Stored Float");
			if (FloatState._show)
			{
				DrawElements(_data.DictFloat, (string s, float i) => { return EditorGUILayout.FloatField(s, i); });
				DrawTmpAdd(_data.DictFloat, ref FloatState._name, ref FloatState._addElement);
			}

			EditorGUILayout.Space();
			BoolState._show = EditorGUILayout.Foldout(BoolState._show, "Stored Bool");
			if (BoolState._show)
			{
				DrawElements(_data.DictBool, (string s, bool i) => { return EditorGUILayout.Toggle(s, i); });
				DrawTmpAdd(_data.DictBool, ref BoolState._name, ref BoolState._addElement);
			}

			EditorGUILayout.Space();
			StringState._show = EditorGUILayout.Foldout(StringState._show, "Stored String");
			if (StringState._show)
			{
				DrawElements(_data.DictString, (string s, string i) => { return EditorGUILayout.TextField(s, i); });
				DrawTmpAdd(_data.DictString, ref StringState._name, ref StringState._addElement);
			}

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Load"))
			{
				string filepath = EditorUtility.OpenFilePanel("Load Save File", Application.persistentDataPath, "iddat");
				ReadFile(filepath);
			}

			if (GUILayout.Button("Save"))
			{
				WriteFile();
			}

			// display the GenericMenu when pressing a button
			if (GUILayout.Button("Add Entry"))
			{
				// create the menu and add items to it
				GenericMenu menu = new GenericMenu();

				menu.AddItem(new GUIContent("Integer"), false, AddInteger);
				// an empty string will create a separator at the top level
				menu.AddSeparator("");

				menu.AddSeparator("");

				menu.ShowAsContext();
			}

			EditorGUILayout.EndHorizontal();
		}

		private void DrawTmpAdd<T>(Dictionary<string,T> p_dict, ref string p_entryName, ref bool p_addElement)
		{
			if (p_addElement)
			{
				GUI.SetNextControlName("new_entry");
				p_entryName = EditorGUILayout.TextField(p_entryName);
				
				EditorGUI.FocusTextInControl("new_entry");

				if(Event.current.keyCode == KeyCode.Return)
				{
					p_dict.Add(p_entryName, default);
					p_addElement = false;
					p_entryName = "New Entry";
				}
			}
			else
			{
				if(GUILayout.Button("Add"))
				{
					p_addElement = true;
				}
			}
		}

		private void DrawElements<T>(Dictionary<string, T> p_dict, Func<string, T, T> p_editorfield)
		{
			Dictionary<string, T> workingCopy = new Dictionary<string, T>();

			foreach(var it in p_dict)
			{
				workingCopy[it.Key] = p_editorfield(it.Key,it.Value);
			}

			foreach (var it in workingCopy)
			{
				p_dict[it.Key] = workingCopy[it.Key];
			}
		}

		private void AddInteger()
		{
			//ShowPopupExample window = (ShowPopupExample)EditorWindow.GetWindow(typeof(ShowPopupExample));
			//var windowposition = position;

			//windowposition.width = 256;
			//windowposition.height = 128;

			//ShowPopupExample._callback = (string key) => _data.DictInteger.Add(key, 0);

			//window.position = windowposition;
			//window.Show();
		}

		//private void DrawField(FieldInfo p_field, ref SerializedContainer p_object)
		//{
		//	System.Type type = p_field.FieldType;

		//	if (type == typeof(float))
		//	{
		//		float tmp = (float)p_field.GetValue(p_object);
		//		p_field.SetValue(p_object, EditorGUILayout.FloatField(p_field.Name, tmp));
		//	}

		//	else if (type == typeof(int))
		//	{
		//		int tmp = (int)p_field.GetValue(p_object);
		//		p_field.SetValue(p_object, EditorGUILayout.IntField(p_field.Name, tmp));
		//	}

		//	else if (type == typeof(string))
		//	{
		//		string tmp = (string)p_field.GetValue(p_object);
		//		p_field.SetValue(p_object, EditorGUILayout.TextField(p_field.Name, tmp));
		//	}

		//	else if (type == typeof(bool))
		//	{
		//		bool tmp = (bool)p_field.GetValue(p_object);
		//		p_field.SetValue(p_object, EditorGUILayout.Toggle(p_field.Name, tmp));
		//	}
		//}

		private void ReadFile(string path)
		{
			bool exists = File.Exists(path);

			if (exists)
			{
				try
				{
					BinaryFormatter bf = new BinaryFormatter();
					FileStream file = File.Open(path, FileMode.Open);

					_data = (SerializedContainer)bf.Deserialize(file);
					
					file.Close();
				}
				catch (System.Exception e)
				{
					Debug.LogError("Failed to read File.\n" + e.StackTrace);
				}

				_saveFileLocation = path;
			}
		}

		private void WriteFile()
		{
			try
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Create(_saveFileLocation);

				bf.Serialize(file, _data);
				file.Close();
			}
			catch (System.Exception e)
			{
				Debug.LogError("Failed to write File.\n" + e.StackTrace);
			}
		}
	}
}