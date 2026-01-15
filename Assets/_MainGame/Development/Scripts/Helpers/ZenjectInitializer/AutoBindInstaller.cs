using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AutoBindInstaller : MonoInstaller
{
	[SerializeField] private List<GameObject> targets;

	public override void InstallBindings()
	{
		foreach (var go in targets)
		{
			if (go == null)
				continue;

			foreach (var component in go.GetComponents<MonoBehaviour>())
			{
				if (component is not IAutoBindable)
					continue;

				foreach (var iface in component.GetType().GetInterfaces())
				{
					if (iface == typeof(IAutoBindable))
						continue;
                    if (Container.HasBinding(iface))//TODO
	                    continue;

					Container.Bind(iface)
						.FromInstance(component)
						.AsSingle();
				}
			}
		}
	}
}
