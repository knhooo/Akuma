using UnityEngine;
using UnityEngine.UI;

public class AugmentList : MonoBehaviour
{
    [SerializeField] GameObject augIcon;
    [SerializeField] Sprite[] sprites;
    int code;

    public void AddAugmentIcon()
    {
        GameObject icon = Instantiate(augIcon);
        icon.transform.SetParent(transform, false);
        icon.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sprites[code];
    }

    public void SetCode(int num) 
    {
        code = num;
    }
}
