using System.Collections.Generic;
using UnityEngine;
using VertigoCase.Runtime;
using VertigoCase.Systems.CardSystem;
using VertigoCase.Systems.InventorySystem;
using VertigoCase.Systems.WheelSystem;
using Zenject;

public class AutoBindInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		Container.Bind<RewardParticleController>().FromComponentInHierarchy().AsSingle();
		Container.Bind<WheelController>().FromComponentInHierarchy().AsSingle();
		Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
		Container.Bind<InventoryController>().FromComponentInHierarchy().AsSingle();

	}
}
