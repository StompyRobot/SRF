namespace Scripts.Framework.Service
{
	public class SRServiceBase<T> : SRMonoBehaviour where T : class 
	{

		protected virtual void Awake()
		{
			SRServiceManager.RegisterService<T>(this);
		}

		protected virtual void OnDestroy()
		{
			SRServiceManager.UnRegisterService<T>();
		}

	}
}
