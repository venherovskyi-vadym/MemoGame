using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ScriptableInstaller", menuName = "Installers/ScriptableInstaller")]
public class ScriptableInstaller : ScriptableObjectInstaller
{
    [SerializeField] private CardsStorage CardsStorage;

    public override void InstallBindings()
    {
        Container.Bind<CardsStorage>().FromScriptableObject(CardsStorage).AsSingle();
    }
}
