﻿// <auto-generated/> - StyleCop hack to not enforce commentation errors in this file.
namespace Merchello.Core
{
    using System;
    using System.Linq;

    using Merchello.Core.Configuration;
    using Merchello.Core.Models;
    using Merchello.Core.Services;

    /// <summary>
    /// Extension methods for IAddress
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Gets the <see cref="ICountry"/> for the <see cref="IAddress"/>
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <returns>
        /// The <see cref="ICountry"/>.
        /// </returns>
        public static ICountry Country(this IAddress address)
        {
            if (string.IsNullOrEmpty(address.CountryCode)) return null;

            return MerchelloConfiguration.Current.MerchelloCountries().Countries.FirstOrDefault(x => x.CountryCode.Equals(address.CountryCode, StringComparison.InvariantCultureIgnoreCase));
        }


        /// <summary>
        /// Attempts to split the first name out of the <see cref="IAddress"/> name field
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string TrySplitFirstName(this IAddress address)
        {
            if (string.IsNullOrEmpty(address.Name)) return string.Empty;

            var names = address.Name.Split(' ');

            return names.Any() ? names.First().Trim() : string.Empty;
        }

        /// <summary>
        /// Attempts to split the last name (surname) out of the <see cref="IAddress"/> name field.
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string TrySplitLastName(this IAddress address)
        {
            if (string.IsNullOrEmpty(address.Name)) return string.Empty;

            var names = address.Name.Split(' ');

            return names.Length > 1 ? string.Join(" ", names.Skip(1)).Trim() : string.Empty;
        }

        /// <summary>
        /// Maps a <see cref="IAddress"/> to a <see cref="ICustomerAddress"/>
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="customer">
        /// The customer.
        /// </param>
        /// <param name="label">
        /// The address label
        /// </param>
        /// <param name="addressType">
        /// The type of address to be saved
        /// </param>
        /// <returns>
        /// The <see cref="ICustomerAddress"/>.
        /// </returns>
        internal static ICustomerAddress ToCustomerAddress(this IAddress address, ICustomer customer, string label, AddressType addressType)
        {
            Ensure.ParameterNotNullOrEmpty(label, "Label cannot be empty");

            return new CustomerAddress(customer.Key)
            {
                Label = label,
                FullName = address.Name,
                FirstName = address.FirstName,
                LastName = address.LastName,
                Address1 = address.Address1,
                Address2 = address.Address2,
                Locality = address.Locality,
                Region = address.Region,
                PostalCode = address.PostalCode,
                CountryCode = address.CountryCode,
                Phone = address.Phone,
                Company = address.Organization,
                AddressType = addressType
            };
        }
    }
}