using UnityEngine;
using Zenject;
using UnityEngine.EventSystems;

public static class DiContainerExtentions
{
    
    public static void BindViewPresenter<TView, TPresenter>(this DiContainer container,
        TView view,
        bool copyToAllSubcontainers = false) where TView : UIBehaviour where TPresenter : UiPresenter
    {
        container.BindView<TView>(view);
        container.BindPresenter<TPresenter>(copyToAllSubcontainers);
    }

    public static void BindView<TView>(this DiContainer container, TView view)
        where TView : UIBehaviour
    {
        container.Bind<TView>().FromInstance(view).AsSingle();
    }

    private static void BindPresenter<TPresenter>(this DiContainer container, bool copyToAllSubcontainers = false) where TPresenter : UiPresenter
    {
        if (copyToAllSubcontainers)
            container.BindInterfacesAndSelfTo<TPresenter>().AsSingle().CopyIntoAllSubContainers().NonLazy();
        else
            container.BindInterfacesAndSelfTo<TPresenter>().AsSingle().NonLazy();
    }
}
