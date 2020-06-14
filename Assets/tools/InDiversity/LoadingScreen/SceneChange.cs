using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace io.daniellanner.indiversity
{
	public class SceneChange
	{
		private int _fromSceneIndex;
		public int FomScene => _fromSceneIndex;
		private int _toSceneIndex;
		public int ToScene => _toSceneIndex;

		public SceneChange(int fromSceneIndex, int toSceneIndex)
		{
			_fromSceneIndex = fromSceneIndex;
			_toSceneIndex = toSceneIndex;
		}

		public IEnumerator LoadScene()
		{
			yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
			GameObject.FindObjectOfType<LoadingScreenLevelTransition>().SetSceneChange(this);
		}
	}
}