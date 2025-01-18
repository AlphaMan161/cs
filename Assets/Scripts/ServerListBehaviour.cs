// dnSpy decompiler from Assembly-CSharp.dll class: ServerListBehaviour
using System;
using UnityEngine;

public class ServerListBehaviour : MonoBehaviour
{
	public static ServerListBehaviour Instance
	{
		get
		{
			if (ServerListBehaviour.mInstance == null)
			{
				ServerListBehaviour.mInstance = (new GameObject("ServerListBehaviour").AddComponent(typeof(ServerListBehaviour)) as ServerListBehaviour);
			}
			return ServerListBehaviour.mInstance;
		}
	}

	private static ServerListBehaviour mInstance;
}
