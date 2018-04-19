namespace Merchello.Web.Models.ContentEditing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The product display.
    /// </summary>
    public class ProductCollectionWithSortOrder
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public Guid CollectionKey { get; set; }        
        
        /// <summary>
        /// Gets or sets the products
        /// </summary>
        public IEnumerable<ProductDisplay> Products{ get; set; }
   }
}
