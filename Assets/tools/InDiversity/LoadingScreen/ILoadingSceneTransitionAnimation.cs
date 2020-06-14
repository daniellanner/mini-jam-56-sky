
namespace io.daniellanner.indiversity
{
	public interface ILoadingSceneTransitionAnimation
	{
		void Open(System.Action p_callback);
		void Close(System.Action p_callback);
	}
}