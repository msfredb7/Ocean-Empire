﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemDisplay : MonoBehaviour {

    public UnityAction OnEquip;
    public UnityAction OnBuyWithMoney;
    public UnityAction OnBuyWithTicket;

    public ShopButton moneyCostButton;
    public ShopButton fullButton;
    public ShopButton ticketCostButton;


    public Item item;

    public Text itemName;
    public Text itemDescription;

    public Sprite ticketImage;
    public Sprite moneyImage;

    public Image itemImage;

    // Use this for initialization
    void Start()
    {

        itemName.text = item.description.GetName();
        itemDescription.text = item.description.GetDescription();

        itemImage.sprite = item.description.GetImage();

        OnEquip += Equip;
        OnBuyWithMoney += BuyWithMoney;
        OnBuyWithTicket += BuyWithTicket;

        UpdateButton();
    }

    public void UpdateButton()
    {
        string itemID = item.GetItemID();

        if (ItemsList.ItemOwned(itemID))
        {
            if (ItemsList.instance.IsEquiped(itemID))
                ButtonEquiped();
            else
                ButtonOwned();
        }
        else {
            int moneyCost = item.description.GetMoneyCost();
            int ticketCost = item.description.GetTicketCost();

            if (moneyCost >= 0 && ticketCost >= 0)
                ButtonTicketAndMoneyCost();

            else
            {
                if (moneyCost <= 0 && ticketCost < 0)
                    ButtonMoneyCostOnly();
                else if (moneyCost > 0 && ticketCost <= 0)
                    ButtonTickeyCostOnly();
            }
        }

    }

    public void ButtonEquiped()
    {
        moneyCostButton.DisableButton();
        ticketCostButton.DisableButton();
        fullButton.EquipedButton();
    }

    public void ButtonOwned()
    {
        moneyCostButton.DisableButton();
        ticketCostButton.DisableButton();
        fullButton.OwnedButton(OnEquip);
    }

    public void ButtonTicketAndMoneyCost()
    {
        moneyCostButton.MoneyButton(OnBuyWithMoney, item.description.GetMoneyCost());
        ticketCostButton.TicketButton(OnBuyWithTicket, item.description.GetTicketCost());
        fullButton.DisableButton();
    }

    public void ButtonMoneyCostOnly()
    {
        moneyCostButton.DisableButton();
        ticketCostButton.DisableButton();
        fullButton.MoneyButton(OnBuyWithMoney, item.description.GetMoneyCost());

    }

    public void ButtonTickeyCostOnly()
    {
        moneyCostButton.DisableButton();
        ticketCostButton.DisableButton();
        fullButton.TicketButton(OnBuyWithTicket, item.description.GetTicketCost());
    }

    public void Equip()
    {
        ItemsList.EquipUpgrade(item.GetItemID());

        GetComponentInParent<ShopUI>().UpdateDisplay();
    }

    public void BuyWithMoney()
    {
        ItemsList.BuyUpgrade(item.GetItemID());

        Equip();
    }
    public void BuyWithTicket()
    {
        ItemsList.BuyUpgrade(item.GetItemID());

        Equip();
    }
}