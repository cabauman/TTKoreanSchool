using System.Collections.Generic;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.Services
{
    public abstract class BaseAnalyticsService : IAnalyticsService
    {
        public abstract void SetUserId(string id);

        public abstract void SetUserProperty(string name, string value);

        public void TrackSignInStarted(string method)
        {
            var parameters = new Dictionary<string, object>()
            {
                { ParameterNames.SignUpMethod, method }
            };

            LogEvent(EventNames.SignUp, parameters);
        }

        protected abstract void LogEvent(string name, IDictionary<string, object> parameters);

        private class EventNames
        {
            public const string AddPaymentInfo = "add_payment_info";
            public const string ViewSearchResults = "view_search_results";
            public const string ViewItemList = "view_item_list";
            public const string ViewItem = "view_item";
            public const string UnlockAchievement = "unlock_achievement";
            public const string TutorialComplete = "tutorial_complete";
            public const string TutorialBegin = "tutorial_begin";
            public const string SpendVirtualCurrency = "spend_virtual_currency";
            public const string SignUp = "sign_up";
            public const string Share = "share";
            public const string SetCheckoutOption = "set_checkout_option";
            public const string SelectContent = "select_content";
            public const string Search = "search";
            public const string PurchaseRefund = "purchase_refund";
            public const string RemoveFromCart = "remove_from_cart";
            public const string PostScore = "post_score";
            public const string AddToCart = "add_to_cart";
            public const string AddToWishlist = "add_to_wishlist";
            public const string AppOpen = "app_open";
            public const string BeginCheckout = "begin_checkout";
            public const string CampaignDetails = "campaign_details";
            public const string PresentOffer = "present_offer";
            public const string CheckoutProgress = "checkout_progress";
            public const string EcommercePurchase = "ecommerce_purchase";
            public const string GenerateLead = "generate_lead";
            public const string JoinGroup = "join_group";
            public const string LevelUp = "level_up";
            public const string Login = "login";
            public const string EarnVirtualCurrency = "earn_virtual_currency";
        }

        private class ParameterNames
        {
            public const string AchievementId = "achievement_id";
            public const string Location = "location";
            public const string Medium = "medium";
            public const string NumberOfNights = "number_of_nights";
            public const string NumberOfPassengers = "number_of_passengers";
            public const string NumberOfRooms = "number_of_rooms";
            public const string Origin = "origin";
            public const string Price = "price";
            public const string Quantity = "quantity";
            public const string Score = "score";
            public const string Level = "level";
            public const string SearchTerm = "search_term";
            public const string SignUpMethod = "sign_up_method";
            public const string Source = "source";
            public const string StartDate = "start_date";
            public const string Tax = "tax";
            public const string Term = "term";
            public const string TransactionId = "transaction_id";
            public const string TravelClass = "travel_class";
            public const string Value = "value";
            public const string VirtualCurrencyName = "virtual_currency_name";
            public const string Shipping = "shipping";
            public const string ItemName = "item_name";
            public const string ItemVariant = "item_variant";
            public const string ItemList = "item_list";
            public const string Aclid = "aclid";
            public const string Affiliation = "affiliation";
            public const string Campaign = "campaign";
            public const string Character = "character";
            public const string CheckoutOption = "checkout_option";
            public const string CheckoutStep = "checkout_step";
            public const string Content = "content";
            public const string ContentType = "content_type";
            public const string Coupon = "coupon";
            public const string ItemLocationId = "item_location_id";
            public const string CreativeName = "creative_name";
            public const string Cp1 = "cp1";
            public const string Currency = "currency";
            public const string Destination = "destination";
            public const string EndDate = "end_date";
            public const string FlightNumber = "flight_number";
            public const string GroupId = "group_id";
            public const string Index = "index";
            public const string ItemBrand = "item_brand";
            public const string ItemCategory = "item_category";
            public const string ItemId = "item_id";
            public const string CreativeSlot = "creative_slot";
        }
    }
}