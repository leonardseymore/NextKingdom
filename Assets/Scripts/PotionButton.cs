using BitAura;
using UnityEngine;

public class PotionButton : MonoBehaviour {

    _2dxFX_GrayScale grayScaleFx;

    bool _active;
    public bool Active
    {
        get
        {
            return _active;
        }
        set
        {
            grayScaleFx.enabled = value;
            _active = value;
        }
    }

    private void Awake()
    {
        grayScaleFx = GetComponent<_2dxFX_GrayScale>();
    }
}
