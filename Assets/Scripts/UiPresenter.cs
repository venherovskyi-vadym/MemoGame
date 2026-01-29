using System;
using Zenject;

public abstract class UiPresenter : IInitializable, IDisposable, IUIPresenter, ITickable
{
    public bool IsShown { get; private set; }

	public virtual void InitialEnable()
	{
		IsShown = true;
	}

	public virtual void InitialDisable()
	{
		IsShown = false;
	}

	public virtual void Show()
	{
		IsShown = true;
	}

	public virtual void Hide()
	{
		IsShown = false;
	}

	/// <summary>
	/// Call this AFTER ancestor's logic
	/// </summary>
	public virtual void Dispose()
	{
	}

	public virtual void Initialize()
	{
	}

    public virtual void Tick()
    {        
    }
}