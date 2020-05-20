using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class IAPManager : MonoBehaviour, IStoreListener
{
	public static IAPManager Instance{ set; get; }

	private static IStoreController m_StoreController;          // The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

	public static string PRODUCT_100_GEMS =    "buy100gems";
	public static string PRODUCT_200_GEMS =    "buy200gems";
	public static string PRODUCT_400_GEMS =    "buy400gems";
	public static string PRODUCT_1000_GEMS =    "buy1000gems";
	public static string PRODUCT_2000_GEMS =    "buy2000gems";


	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		// If we haven't set up the Unity Purchasing reference
		if (m_StoreController == null)
		{
			// Begin to configure our connection to Purchasing
			InitializePurchasing();
		}
	}

	public void InitializePurchasing() 
	{
		// If we have already connected to Purchasing ...
		if (IsInitialized())
		{
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		builder.AddProduct(PRODUCT_100_GEMS, ProductType.Consumable);
		builder.AddProduct(PRODUCT_200_GEMS, ProductType.Consumable);
		builder.AddProduct(PRODUCT_400_GEMS, ProductType.Consumable);
		builder.AddProduct(PRODUCT_1000_GEMS, ProductType.Consumable);
		builder.AddProduct(PRODUCT_2000_GEMS, ProductType.Consumable);


		// Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
		// and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
		UnityPurchasing.Initialize(this, builder);
	}


	private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}


	public void Buy100Gems()
	{
		BuyProductID(PRODUCT_100_GEMS);
	}
	public void Buy200Gems()
	{
		BuyProductID(PRODUCT_200_GEMS);
	}
	public void Buy400Gems()
	{
		BuyProductID(PRODUCT_400_GEMS);
	}
	public void Buy1000Gems()
	{
		BuyProductID(PRODUCT_1000_GEMS);
	}
	public void Buy2000Gems()
	{
		BuyProductID(PRODUCT_2000_GEMS);
	}


	void BuyProductID(string productId)
	{
		// If Purchasing has been initialized ...
		if (IsInitialized())
		{
			// ... look up the Product reference with the general product identifier and the Purchasing 
			// system's products collection.
			Product product = m_StoreController.products.WithID(productId);

			// If the look up found a product for this device's store and that product is ready to be sold ... 
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
				// asynchronously.
				m_StoreController.InitiatePurchase(product);
			}
			// Otherwise ...
			else
			{
				// ... report the product look-up failure situation  
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		// Otherwise ...
		else
		{
			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
			// retrying initiailization.
			Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}


	public void OnInitializeFailed(InitializationFailureReason error)
	{
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}


	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
	{
		if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_100_GEMS, StringComparison.Ordinal))
		{
			GemGoldScript.gemGoldScript.PlusGem (100);
			ShopManager.shopManager.SetText ();
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_200_GEMS, StringComparison.Ordinal))
		{
			GemGoldScript.gemGoldScript.PlusGem (200);
			ShopManager.shopManager.SetText ();
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_400_GEMS, StringComparison.Ordinal))
		{
			GemGoldScript.gemGoldScript.PlusGem (400);
			ShopManager.shopManager.SetText ();
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_1000_GEMS, StringComparison.Ordinal))
		{
			GemGoldScript.gemGoldScript.PlusGem (1000);
			ShopManager.shopManager.SetText ();
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_2000_GEMS, StringComparison.Ordinal))
		{
			GemGoldScript.gemGoldScript.PlusGem (2000);
			ShopManager.shopManager.SetText ();
		}
		else 
		{
			Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
		}

		// Return a flag indicating whether this product has completely been received, or if the application needs 
		// to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
		// saving purchased products to the cloud, and when that save is delayed. 
		return PurchaseProcessingResult.Complete;
	}


	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
		// this reason with the user to guide their troubleshooting actions.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	}
}