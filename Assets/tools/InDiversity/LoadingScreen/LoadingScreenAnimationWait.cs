using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	public class LoadinScreenAnimationWait : CustomYieldInstruction
	{
		private bool _done = false;

		public override bool keepWaiting
		{
			get
			{
				return !_done;
			}
		}

		public LoadinScreenAnimationWait()
		{
			LoadingScreenAnimationDeployer.Done += Callback;
		}

		private void Callback()
		{
			_done = true;
			LoadingScreenAnimationDeployer.Done -= Callback;
		}
	}
}