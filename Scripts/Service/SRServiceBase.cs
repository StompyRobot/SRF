namespace Scripts.Framework.Service
{
	public class SRServiceBase<T> : SRMonoBehaviourEx where T : class 
	{

		protected override void Awake()
		{
			base.Awake();
			SRServiceManager.RegisterService<T>(this);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			SRServiceManager.UnRegisterService<T>();
		}

	}
}
