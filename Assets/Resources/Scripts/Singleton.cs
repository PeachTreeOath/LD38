using UnityEngine;
using System.Collections;

/// <summary>
/// Unity singleton implementation. Call SetDontDestroy() to persist objects between scenes. 
/// </summary>
/// <typeparam name="T">Class to singleton</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

	static T instance;

	public static T Instance
	{
		get
		{
			if(instance == null)
			{
				GameObject foo = new GameObject();
				foo.name = typeof(T).ToString();
				instance = foo.AddComponent<T>();
				instance.SendMessage("Start", SendMessageOptions.DontRequireReceiver);
				DontDestroyOnLoad(foo);
			}

			return instance;
		}
	}
}
