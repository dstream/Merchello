namespace Merchello.FastTrack.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Merchello.Web.Store.Localization;
    using Merchello.Web.Store.Models;
    using Umbraco.Core;

    /// <summary>
    /// A base class for FastTrack <see cref="StoreAddressModel"/>.
    /// </summary>
    public class FastTrackCheckoutAddressModel : StoreAddressModel, IFastTrackCheckoutAddressModel
    {
        /// <summary>
        /// The split names.
        /// </summary>
        private string[] _names;        

        public override string Name
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }

            set
            {
                // stored but never used
                if (_names == null && !value.IsNullOrWhiteSpace())
                {
                    _names = value.Split(' ');
                    if (_names.Length == 2)
                    {
                        FirstName = _names[0];
                        LastName = _names[1];
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of countries for the view drop down list.
        /// </summary>
        public IEnumerable<SelectListItem> Countries { get; set; }

        /// <summary>
        /// Gets or sets the success URL to redirect to the shipping entry stage.
        /// </summary>
        public string SuccessRedirectUrl { get; set; }
    }
}