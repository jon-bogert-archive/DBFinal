using UnityEngine;
using TMPro;
using UnityEngine.UI;

public delegate void GunButtonEvent(bool isPurchased, Gun gun);

public class GunButton : MonoBehaviour
{
    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _damageText;
    [SerializeField] TMP_Text _priceText;
    [SerializeField] Image _greyOutImage;
    [SerializeField] Color _activeColor = Color.green;

    bool _isPurchased = false;
    Gun _gun;
    Color _inactiveColor = Color.black;
    Image _backgroundImage;

    public GunButtonEvent onPress;
    
    public bool isPurchased
    {
        get { return _isPurchased; }
        set
        {
            if (value)
            {
                _greyOutImage.gameObject.SetActive(false);
            }
            else
            {
                _greyOutImage.gameObject.SetActive(true);
            }
            _isPurchased = value;
        }
    }

    public Gun gun { get { return _gun; }
        set
        {
            _gun = value;
            SetText(value.name, value.damage, value.fireLimit, value.price);
        }
    }

    private void Awake()
    {
        isPurchased = false;
        _backgroundImage = GetComponent<Image>();
    }

    private void Start()
    {
        _inactiveColor = _backgroundImage.color;
        UpdateBackground();
    }

    void SetText(string name, float damage, float fireLimit, float price)
    {
        _nameText.text = name;
        _damageText.text = "Dmg: " + damage.ToString() + "\nRoF: " + (1f / fireLimit).ToString();
        _priceText.text = "Price: " + price.ToString();
    }

    public void UpdateBackground()
    {
        if (AppData.activeGunName == _gun.name)
        {
            _backgroundImage.color = _activeColor;
            return;
        }
        _backgroundImage.color = _inactiveColor;
    }

    public void InvokeOnPress()
    {
        onPress?.Invoke(isPurchased, _gun);
    }
}
