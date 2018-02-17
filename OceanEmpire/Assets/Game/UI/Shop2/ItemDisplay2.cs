﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemDisplay2 : MonoBehaviour {


    private IShopDisplayable displayableCategory;

    public Text titre;
    public Image icon;
    public Text description;

    public ShopButton2 coinCostButton;
    public ShopButton2 ticketCostButton;

    public void SetValues(IShopDisplayable category)
    {
        displayableCategory = category;
        UpdateView();
    }

    public void UpdateView()
    {
        titre.text = displayableCategory.GetTitle();
        if (description)
            description.text = displayableCategory.GetDescription();
        icon.sprite = displayableCategory.GetShopIcon();

        coinCostButton.SetButton(Buy, displayableCategory.GetPrice(CurrencyType.Coin), CurrencyType.Coin);
        ticketCostButton.SetButton(Buy, displayableCategory.GetPrice(CurrencyType.Ticket), CurrencyType.Ticket);
    }

    public void Buy(CurrencyType type)
    {
        if (PlayerCurrency.GetCurrency(type) > displayableCategory.GetPrice(type))
        {
            ConfirmBuy.OpenWindowAndConfirm(null, type, (hasConfirmed) =>
            {
                if (hasConfirmed)
                {
                    displayableCategory.Buy(type);
                    Scenes.GetActive(ItemDescScene.SCENENAME).FindRootObject<ItemDescScene>().Quit();
                }
            });
        }
    }
}
