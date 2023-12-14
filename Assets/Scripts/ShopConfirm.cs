using TMPro;
using UnityEngine;

public delegate void ShopConfirmEvent(Gun gun);

public class ShopConfirm : MonoBehaviour
{
    public ShopConfirmEvent onConfirm;
    [SerializeField] TMP_Text _infoText;
    Gun _activeGun = null;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Begin(Gun gun)
    {
        _activeGun = gun;
        _infoText.text = "Gun: " + gun.name + "\nPrice: " + gun.price;
        gameObject.SetActive(true);
    }

    public void Yes()
    {
        onConfirm?.Invoke(_activeGun);
        ClearEvents();
        gameObject.SetActive(false);
    }

    public void No()
    {
        ClearEvents();
        gameObject.SetActive(false);
    }

    void ClearEvents()
    {
        foreach (ShopConfirmEvent sce in onConfirm.GetInvocationList())
        {
            onConfirm -= sce;
        }
    }
}
