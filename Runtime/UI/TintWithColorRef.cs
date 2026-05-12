using UnityEngine;
using UnityEngine.UI;

public class TintWithColorRef : MonoBehaviour
{
    Image image;
    IVariableAccess<Color> colorRef;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void SetColorRef(IVariableAccess<Color> newColorRef)
    {
        if (this.isActiveAndEnabled) Unsub();
        colorRef = newColorRef;
        if (this.isActiveAndEnabled) Sub();
    }
    private void OnEnable()
    {
        Sub();
    }
    private void OnDisable()
    {
        Unsub();
    }
    void Sub()
    {
        colorRef?.Subscribe(OnColorChange);
    }
    void Unsub()
    {
        colorRef?.Unsubscribe(OnColorChange);
    }
    void OnColorChange(Color color)
    {
        if (image) image.color = color;
    }
}
