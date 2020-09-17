namespace Merchello.FastTrack.Models
{
    using System.ComponentModel.DataAnnotations;

    using Merchello.Web.Store.Localization;

    using Umbraco.Core;

    /// <summary>
    /// The checkout billing address model.
    /// </summary>
    public class FastTrackBillingAddressModel : FastTrackCheckoutAddressModel
    {             
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Display(ResourceType = typeof(StoreFormsResource), Name = "LabelEmailAddress")]
        [Required(ErrorMessageResourceType = typeof(StoreFormsResource), ErrorMessageResourceName = "RequiredEmailAddress")]
        [EmailAddress(ErrorMessageResourceType = typeof(StoreFormsResource), ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessage = null)]
        public override string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use billing address for the shipping address.
        /// </summary>
        [Display(ResourceType = typeof(StoreFormsResource), Name = "LabelUseForShipping")]
        public bool UseForShipping { get; set; }        

        /// <summary>
        /// Gets or sets the success URL to redirect to the  ship rate quote stage.
        /// </summary>
        /// <remarks>
        /// Used if customer opts to use the billing address for the shipping address 
        /// </remarks>
        public string SuccessUrlShipRateQuote { get; set; }
    }
}