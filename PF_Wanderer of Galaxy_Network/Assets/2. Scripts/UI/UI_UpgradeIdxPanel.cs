using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_UpgradeIdxPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Text _upgradeTitle;
    UpgradeUIManager _upgradeUIManager;
    ScrollRect _scRect;
    int _upgradeIdx;

    private void Awake()
    {
        _scRect = transform.parent.parent.parent.GetComponent<ScrollRect>();
    }

    public void Init(int idx)
    {
        _upgradeIdx = idx;
        _upgradeTitle.text = TableDataManager._Instance.Get(LowDataType.UpgradeList).ToS(idx, "UpgradeItem");
    }

    public void PanelClick()
    {
        UpgradeUIManager._Instance.SelectUpgradeItem(_upgradeIdx);
    }

    public void OnBeginDrag(PointerEventData e)
    {
        _scRect.OnBeginDrag(e);
    }

    public void OnDrag(PointerEventData e)
    {
        _scRect.OnDrag(e);
    }

    public void OnEndDrag(PointerEventData e)
    {
        _scRect.OnEndDrag(e);
    }
}
