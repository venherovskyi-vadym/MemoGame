using UnityEngine;
using UnityEngine.EventSystems;


public abstract class UiPresenterBase<TView> : UiPresenter where TView : UIBehaviour
{
    public TView View { get; }

    protected UiPresenterBase(TView view)
    {
        View = view;
    }

    public override void Show()
    {
        View.gameObject.SetActive(true);
    }

    public override void Hide()
    {
        View.gameObject.SetActive(false);
    }

    public override void InitialEnable()
    {
        base.InitialEnable();
        Show();
    }

    public override void InitialDisable()
    {
        Hide();
        base.InitialDisable();
    }
}