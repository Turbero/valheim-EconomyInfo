using EconomyInfo.tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EconomyInfo.money_vendor
{
    public class VendorPanelValuable
    {
        private readonly GameObject vendorPanelValuableGO;
        private readonly UITooltip sellButtonTooltip;
        private readonly string spriteName;
        
        public VendorPanelValuable(Transform storeTransform, string valuableName, string spriteName,
            Vector2 anchoredPosition, Vector2? anchoredPositionIcon = null, Vector2? sizeDeltaIcon = null)
        {
            this.spriteName = spriteName;
            
            GameObject coins = storeTransform.Find("coins").gameObject;
            
            vendorPanelValuableGO = GameObject.Instantiate(coins, storeTransform);
            vendorPanelValuableGO.name = valuableName;
            vendorPanelValuableGO.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
            
            //Change icon
            Transform child = vendorPanelValuableGO.transform.GetChild(0);
            child.name = "icon";
            child.GetComponent<Image>().sprite = ModUtils.getSprite(spriteName);
            if (anchoredPositionIcon != null)
                child.GetComponent<RectTransform>().anchoredPosition = anchoredPositionIcon.Value;
            if (sizeDeltaIcon != null)
                child.GetComponent<RectTransform>().sizeDelta = sizeDeltaIcon.Value;

            //Value to 0 to start
            Transform amount = vendorPanelValuableGO.transform.GetChild(1);
            amount.name = "amount";
            amount.GetComponent<TextMeshProUGUI>().text = "0 (0)";

            //SellPanel with sell button
            var sellPanel = GameObject.Instantiate(GameObject.Find("Store/SellPanel"), vendorPanelValuableGO.transform);
            sellPanel.name = "SellPanel";
            sellPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(245, 8);
            sellPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32); //smaller button panel
            Object.Destroy(sellPanel.GetComponent<Image>());
            
            //SellButton
            var sellButton = sellPanel.transform.Find("SellButton");
            sellButton.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32); //smaller button
            sellButton.transform.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32); //smaller coin icon
            sellButton.transform.Find("Image").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -3.5f); //centered smaller coin icon
            sellButtonTooltip = sellButton.GetComponent<UITooltip>(); 
            updateTooltip();
            Button buttonSellButton = sellPanel.GetComponentInChildren<Button>();
            buttonSellButton.onClick = new Button.ButtonClickedEvent();
            buttonSellButton.onClick.AddListener(() =>
            {
                MoneyStoreGuiSelectedItemPatch.lastValuableToSell = "$item_"+spriteName.ToLowerInvariant();
                ModUtils.RunPrivateMethod(StoreGui.instance, "SellItem"); 
                MoneyStoreGuiSelectedItemPatch.lastValuableToSell = null;
            });
        }

        public void updateValue(int amount, int  value)
        {
            Transform childAmount = vendorPanelValuableGO.transform.GetChild(1);
            childAmount.GetComponent<TextMeshProUGUI>().text = amount + " (" + value + ")";
            childAmount.GetComponent<TextMeshProUGUI>().faceColor = amount == 0
                ? new Color(255, 0, 0, 255)
                : new Color(255, 255, 255, 255);
            updateTooltip();
        }
        
        private void updateTooltip() 
        {
            var itemName = "$item_" + spriteName.ToLowerInvariant();
            sellButtonTooltip.m_text = Player.m_localPlayer.IsKnownMaterial(itemName)
                ? "$store_sell " + itemName
                : "???";
        }

        public GameObject getMainPanel()
        {
            return vendorPanelValuableGO; 
        }
    }
}
