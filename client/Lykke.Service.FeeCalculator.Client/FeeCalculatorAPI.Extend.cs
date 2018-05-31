﻿using System;
using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace Lykke.Service.FeeCalculator.AutorestClient
{
    public partial class FeeCalculatorAPI
    {
        /// <inheritdoc />
        /// <summary>
        /// Should be used to prevent memory leak in RetryPolicy
        /// </summary>
        public FeeCalculatorAPI(Uri baseUri, HttpClient client) : base(client)
        {
            Initialize();

            BaseUri = baseUri ?? throw new ArgumentNullException("baseUri");
        }
    }
}
