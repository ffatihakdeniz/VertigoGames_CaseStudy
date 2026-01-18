using System.Collections.Generic;
using UnityEngine;
using VertigoCase.Runtime;
using VertigoCase.Systems.CardSystem;
using VertigoCase.Systems.WheelSystem;
using Zenject;

public class AutoBindInstaller : MonoInstaller
{
	[SerializeField] private List<GameObject> targets;

	public override void InstallBindings()
	{
		Container.Bind<RewardParticleController>().FromComponentInHierarchy().AsSingle();
		Container.Bind<WheelController>().FromComponentInHierarchy().AsSingle();
	}
}
