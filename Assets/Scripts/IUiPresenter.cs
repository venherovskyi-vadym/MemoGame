using UnityEngine;

public interface IUIPresenter
{
	void Show();
	void Hide();
	bool IsShown { get; }
}