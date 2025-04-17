using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemIcon : MonoBehaviour
{
    Image _itemIcon;
    [SerializeField] Image _itemTitleBG;
    [SerializeField] Text _itemTitle;

    int _itemIdx;

    private void Awake()
    {
        _itemIcon = gameObject.GetComponent<Image>();
    }

    public void SetItemIdx(int idx)
    {
        _itemIdx = idx;
        _itemIcon.sprite = Resources.Load("ItemIconImg/" + idx, typeof(Sprite)) as Sprite;
    }
}
