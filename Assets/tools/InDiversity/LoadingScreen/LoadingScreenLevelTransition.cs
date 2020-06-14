using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace io.daniellanner.indiversity
{
	public class LoadingScreenLevelTransition : MonoBehaviour
	{

		private int _fromScene = 1000;
		private int _toScene = 1000;
		private LoadingScreenAnimationDeployer _animation;

		public LoadingScreenLevelTransition SetSceneChange(SceneChange p_sceneInfo)
		{
			_fromScene = p_sceneInfo.FomScene;
			_toScene = p_sceneInfo.ToScene;
			return this;
		}

		private void Awake()
		{
			_animation = FindObjectOfType<LoadingScreenAnimationDeployer>();
		}

		private void Start()
		{
			if (_animation != null)
			{
				StartCoroutine(Transition());
			}
		}

		IEnumerator Transition()
		{
			if (_fromScene == 1000 && _toScene == 1000) //Opened Loading Scene in Editor for LookDev
			{
				yield return _animation.OverlayScenes();
				yield return new WaitForSeconds(1f);
				yield return _animation.ClearScenes();
			}
			else
			{
				yield return new WaitForSeconds(0.1f);

				yield return _animation.OverlayScenes();
				yield return SceneManager.UnloadSceneAsync(_fromScene);
				yield return SceneManager.LoadSceneAsync(_toScene, LoadSceneMode.Additive);
				//yield return new WaitForSeconds(1f);
				yield return _animation.ClearScenes();
				yield return SceneManager.UnloadSceneAsync(1);
			}
		}
	}
}