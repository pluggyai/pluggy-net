using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Pluggy.SDK.Errors;
using Pluggy.SDK.Helpers;
using Pluggy.SDK.HTTP;
using Pluggy.SDK.Model;

namespace Pluggy.SDK
{
    public class PluggyAPI
    {
        protected readonly APIService httpService;

        protected static readonly string URL_CONNECTORS = "/connectors";
        protected static readonly string URL_ITEMS = "/items";
        protected static readonly string URL_ACCOUNTS = "/accounts";
        protected static readonly string URL_TRANSACTIONS = "/transactions";
        protected static readonly string URL_TRANSACTIONS_V2 = "/v2/transactions";
        protected static readonly string URL_INVESTMENTS = "/investments";
        protected static readonly string URL_CATEGORIES = "/categories";
        protected static readonly string URL_WEBHOOKS = "/webhooks";
        protected static readonly string URL_IDENTITY = "/identity";
        protected static readonly string URL_ITEMS_MFA = "/items/{id}/mfa";
        protected static readonly string URL_CONNECT_TOKEN = "/connect_token";
        protected static readonly string URL_INVESTMENT_TRANSACTIONS = "/investments/{id}/transactions";
        protected static readonly string URL_CONSENTS = "/consents";
        protected static readonly string URL_LOANS = "/loans";
        protected static readonly string URL_PAYMENT_RECIPIENTS = "/payments/recipients";
        protected static readonly string URL_PAYMENT_RECIPIENT_INSTITUTIONS = "/payments/recipients/institutions";
        protected static readonly string URL_PAYMENT_REQUESTS = "/payments/requests";
        protected static readonly string URL_PAYMENT_INTENTS = "/payments/intents";
        protected static readonly string URL_PAYMENT_CUSTOMERS = "/payments/customers";
        protected static readonly string URL_CATEGORY_RULES = "/categories/rules";
        protected static readonly string URL_BILLS = "/bills";
        protected static readonly string URL_MERCHANTS = "/merchants";
        protected static readonly string URL_SMART_TRANSFER_PAYMENTS = "/smart-transfers/payments";
        protected static readonly string URL_SMART_TRANSFER_PREAUTHORIZATIONS = "/smart-transfers/preauthorizations";
        protected static readonly string URL_BOLETOS = "/boletos";
        protected static readonly string URL_BOLETO_CONNECTIONS = "/boleto-connections";

        public static readonly int STATUS_POLL_INTERVAL = 3000;


        public PluggyAPI(string _clientId, string _clientSecret, HttpClient _httpClient = null, string _baseUrl = "https://api.pluggy.ai/")
        {
            httpService = new APIService(_clientId, _clientSecret, _baseUrl, _httpClient);
        }

        public PluggyAPI(string _clientId, string _clientSecret, string _baseUrl)
            : this(_clientId, _clientSecret, null, _baseUrl)
        {
        }

        /// <summary>
        /// Fetch all available connectors
        /// </summary>
        /// <returns>An array of connectors</returns>
        public async Task<PageResults<Connector>> FetchConnectors(ConnectorParameters requestParams = null)
        {
            return await httpService.GetAsync<PageResults<Connector>>(URL_CONNECTORS, null, requestParams?.ToQueryStrings());
        }

        /// <summary>
        /// Fetch a single connector
        /// </summary>
        /// <param name="id">The connector ID</param>
        /// <returns>A connector object</returns>
        public async Task<Connector> FetchConnector(long id)
        {
            return await httpService.GetAsync<Connector>(URL_CONNECTORS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Check that connector parameters are valid
        /// </summary>
        /// <param name="id">The connector ID</param>
        /// <param name="request">The connector parameters</param>
        /// <returns>an object with the info of which parameters are wrong</returns>
        public async Task<ValidationResult> ValidateCredentials(long id, List<ItemParameter> credentials)
        {
            return await httpService.PostAsync<ValidationResult>(
                URL_CONNECTORS + "/{id}/validate",
                credentials?.ToDictionary(x => x.Name, x => x.Value),
                null,
                HTTP.Utils.GetSegment(id.ToString())
            );
        }

        /// <summary>
        /// Creates a new item for an specific connector
        /// </summary>
        /// <param name="request">The item parameters</param>
        /// <returns>an object with the info to retrieve the data when the execution is ready</returns>
        public async Task<Item> CreateItem(ItemParameters request)
        {
            try
            {
                return await httpService.PostAsync<Item>(URL_ITEMS, request.ToBody());
            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Details != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }

        /// <summary>
        /// Fetch a single item
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns></returns>
        public async Task<Item> FetchItem(Guid id)
        {
            return await httpService.GetAsync<Item>(URL_ITEMS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Update an item connection with/without credentials forcing a sync
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>An item object with the status of the connection</returns>
        public async Task<Item> UpdateItem(Guid id, ItemParameters request)
        {
            try
            {
                return await httpService.PatchAsync<Item>(URL_ITEMS + "/{id}", request.ToBody(), null, HTTP.Utils.GetSegment(id.ToString()));
            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Details != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }



        /// <summary>
        /// Sends multi-factor authentication parameter to item that is requesting it. 
        /// </summary>
        /// <param name="id">The item ID</param>
        /// <param name="parameters">Key-value pairs of requested parameters</param>
        /// <returns>An item object with the status of the connection</returns>
        public async Task<Item> UpdateItemMFA(Guid id, List<ItemParameter> parameters)
        {
            try
            {
                return await httpService.PostAsync<Item>(URL_ITEMS_MFA, parameters.ToDictionary(x => x.Name, x => x.Value), null, HTTP.Utils.GetSegment(id.ToString()));
            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Details != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }


        /// <summary>
        /// Deletes an Item by it's primary identifier
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns></returns>
        public async Task DeleteItem(Guid id)
        {
            await httpService.DeleteAsync<dynamic>(URL_ITEMS + "/{id}", HTTP.Utils.GetSegment(id.ToString()), null);
        }

        /// <summary>
        /// Fetch the list of accounts
        /// </summary>
        /// <param name="id">Item Id</param>
        /// <returns>Account results list</returns>
        public async Task<PageResults<Account>> FetchAccounts(Guid id, AccountType? type = null)
        {
            var queryStrings = new Dictionary<string, string>
            {
                { "itemId", id.ToString() },
                { "type", type.ToString() }
            };
            return await httpService.GetAsync<PageResults<Account>>(URL_ACCOUNTS, null, queryStrings);
        }

        /// <summary>
        /// Fetch the account details
        /// </summary>
        /// <param name="id">Account Id</param>
        /// <returns>Account details</returns>
        public async Task<Account> FetchAccount(Guid id)
        {
            return await httpService.GetAsync<Account>(URL_ACCOUNTS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Fetch the point-in-time balance of an account
        /// </summary>
        /// <param name="id">Account Id</param>
        /// <returns>Account balance</returns>
        public async Task<AccountBalance> FetchAccountBalance(Guid id)
        {
            return await httpService.GetAsync<AccountBalance>(URL_ACCOUNTS + "/{id}/balance", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Fetch the list of transactions using page-based pagination.
        /// </summary>
        /// <param name="accountId">Account Id</param>
        /// <param name="pageParams">Optional page-based filter parameters</param>
        /// <returns>Transaction results list with paging data</returns>
        [Obsolete("Use FetchTransactionsCursor (single page) or FetchAllTransactions (full sweep) instead. Both use the GET /v2/transactions cursor-based endpoint, which is more stable and supports the full filter set. This page-based method is kept for backward compatibility and will be removed in a future major release.")]
        public async Task<PageResults<Transaction>> FetchTransactions(Guid accountId, TransactionParameters pageParams = null)
        {
            var queryStrings = pageParams != null ? pageParams.ToQueryStrings() : new Dictionary<string, string>();
            queryStrings.Add("accountId", accountId.ToString());
            return await httpService.GetAsync<PageResults<Transaction>>(URL_TRANSACTIONS, null, queryStrings);
        }

        /// <summary>
        /// Fetch a single page of transactions using cursor-based pagination.
        /// </summary>
        /// <param name="accountId">Account Id</param>
        /// <param name="cursorParams">Optional cursor filter parameters (DateFrom, DateTo, CreatedAtFrom, After, Ids)</param>
        /// <returns>CursorPageResults with the transactions list and the next cursor link</returns>
        public async Task<CursorPageResults<Transaction>> FetchTransactionsCursor(Guid accountId, TransactionCursorParameters cursorParams = null)
        {
            var queryStrings = cursorParams != null ? cursorParams.ToQueryStrings() : new Dictionary<string, string>();
            queryStrings.Add("accountId", accountId.ToString());
            return await httpService.GetAsync<CursorPageResults<Transaction>>(URL_TRANSACTIONS_V2, null, queryStrings);
        }

        /// <summary>
        /// Fetch all transactions from an account by sweeping all cursor pages.
        /// </summary>
        /// <param name="accountId">Account Id</param>
        /// <param name="cursorParams">Optional filters (DateFrom, DateTo, CreatedAtFrom, Ids). The After cursor is managed internally.</param>
        /// <returns>Complete list of all transactions</returns>
        public async Task<IList<Transaction>> FetchAllTransactions(Guid accountId, TransactionCursorParameters cursorParams = null)
        {
            var firstPage = await FetchTransactionsCursor(accountId, cursorParams);
            var transactions = new List<Transaction>(firstPage.Results);

            var next = firstPage.Next;

            while (next != null)
            {
                var afterParam = ParseAfterFromNext(next);
                if (afterParam == null) break;

                var pageParams = cursorParams != null
                    ? new TransactionCursorParameters
                    {
                        DateFrom = cursorParams.DateFrom,
                        DateTo = cursorParams.DateTo,
                        CreatedAtFrom = cursorParams.CreatedAtFrom,
                        Ids = cursorParams.Ids,
                        After = afterParam
                    }
                    : new TransactionCursorParameters { After = afterParam };

                var page = await FetchTransactionsCursor(accountId, pageParams);
                transactions.AddRange(page.Results);
                next = page.Next;
            }

            return transactions;
        }

        private static string ParseAfterFromNext(string next)
        {
            if (string.IsNullOrEmpty(next)) return null;

            var query = next.TrimStart('?');
            foreach (var pair in query.Split('&'))
            {
                var kv = pair.Split(new[] { '=' }, 2);
                if (kv.Length == 2 && Uri.UnescapeDataString(kv[0]) == "after")
                    return Uri.UnescapeDataString(kv[1]);
            }
            return null;
        }

        /// <summary>
        /// Fetch a single transaction by its ID.
        /// </summary>
        /// <param name="id">Transaction Id</param>
        /// <returns>Transaction details</returns>
        public async Task<Transaction> FetchTransaction(Guid id)
        {
            return await httpService.GetAsync<Transaction>(URL_TRANSACTIONS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Fetch the list of investments
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Invesments results list</returns>
        public async Task<PageResults<Investment>> FetchInvestments(Guid id, InvestmentType? type = null)
        {
            var queryStrings = new Dictionary<string, string>
            {
                { "itemId", id.ToString() },
                { "type", type.ToString() }
            };
            return await httpService.GetAsync<PageResults<Investment>>(URL_INVESTMENTS, null, queryStrings);
        }

        /// <summary>
        /// Fetch the Investment
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Investment details</returns>
        public async Task<Investment> FetchInvestment(Guid id)
        {
            return await httpService.GetAsync<Investment>(URL_INVESTMENTS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }


        /// <summary>
        /// Fetch the Identity
        /// </summary>
        /// <param name="id">Identity resource id</param>
        /// <returns>Identity details</returns>
        public async Task<Identity> FetchIdentity(Guid id)
        {
            return await httpService.GetAsync<Identity>(URL_IDENTITY + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Fetch the Identity by the Item's ID
        /// </summary>
        /// <param name="id">Item resource id</param>
        /// <returns>Identity details</returns>
        public async Task<Identity> FetchIdentityByItemId(Guid id)
        {
            var queryStrings = new Dictionary<string, string>
            {
                { "itemId", id.ToString() }
            };
            return await httpService.GetAsync<Identity>(URL_IDENTITY, null, queryStrings);
        }

        /// <summary>
        /// Fetch the list of categories
        /// </summary
        /// <returns>Categories results list</returns>
        public async Task<PageResults<Category>> FetchCategories(string parentId = "")
        {
            var queryStrings = new Dictionary<string, string>() { { "parentId", parentId } };
            return await httpService.GetAsync<PageResults<Category>>(URL_CATEGORIES, null, queryStrings);
        }

        /// <summary>
        /// Fetch the category
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Investment details</returns>
        public async Task<Category> FetchCategory(Guid id)
        {
            return await httpService.GetAsync<Category>(URL_CATEGORIES + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Fetch the list of webhooks
        /// </summary
        /// <returns>Webhooks results list</returns>
        public async Task<PageResults<Webhook>> FetchWebhooks()
        {
            return await httpService.GetAsync<PageResults<Webhook>>(URL_WEBHOOKS);
        }

        /// <summary>
        /// Fetch the webhook
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Webhook details</returns>
        public async Task<Webhook> FetchWebhook(Guid id)
        {
            return await httpService.GetAsync<Webhook>(URL_WEBHOOKS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Create a webhook
        /// </summary>
        /// <param name="url">Webhook url</param>
        /// <param name="event">Webhook event</param>
        /// <param name="headers">Optional headers for webhook notifications</param>
        /// <returns></returns>
        public async Task<Webhook> CreateWebhook(string url, WebhookEvent _event, Dictionary<string, string> headers = null)
        {
            var body = new Dictionary<string, object>
            {
                { "url", url },
                { "event", _event.Value },
                { "headers", headers }
            }.RemoveNulls();
            return await httpService.PostAsync<Webhook>(URL_WEBHOOKS, body);
        }

        /// <summary>
        /// Update a single webhook
        /// </summary>
        /// <param name="id">Webhook id</param>
        /// <param name="url">Webhook url</param>
        /// <param name="event">Webhook event</param>
        /// <returns></returns>
        public async Task<Webhook> UpdateWebhook(Guid id, string url, WebhookEvent _event)
        {
            var body = new Dictionary<string, string>
            {
                { "url", url },
                { "event", _event.Value }
            };
            return await httpService.PatchAsync<Webhook>(URL_WEBHOOKS + "/{id}", body, null, HTTP.Utils.GetSegment(id.ToString()));

        }

        /// <summary>
        /// Deletes a Webhook by it's primary identifier
        /// </summary>
        /// <param name="id">Webhook id</param>
        /// <returns></returns>
        public async Task DeleteWebhook(Guid id)
        {
            await httpService.DeleteAsync<dynamic>(URL_WEBHOOKS + "/{id}", HTTP.Utils.GetSegment(id.ToString()), null);
        }

        /// <summary>
        /// Fetch the list of investment transactions
        /// </summary>
        /// <param name="id">Investment Id</param>
        /// <returns>InvestmentTrasnsactions results list</returns>
        public async Task<PageResults<InvestmentTransaction>> FetchInvestmentTransactions(Guid id, TransactionParameters pageParams = null)
        {
            var queryStrings = pageParams != null ? pageParams.ToQueryStrings() : new Dictionary<string, string>();
            return await httpService.GetAsync<PageResults<InvestmentTransaction>>(URL_INVESTMENT_TRANSACTIONS, HTTP.Utils.GetSegment(id.ToString()), queryStrings);
        }

        /// <summary>
        /// Fetch the list of consents for an item
        /// </summary>
        /// <param name="itemId">Item Id</param>
        /// <returns>Consent results list</returns>
        public async Task<PageResults<Consent>> FetchConsents(Guid itemId)
        {
            var queryStrings = new Dictionary<string, string>
            {
                { "itemId", itemId.ToString() }
            };
            return await httpService.GetAsync<PageResults<Consent>>(URL_CONSENTS, null, queryStrings);
        }

        /// <summary>
        /// Fetch a consent by its ID
        /// </summary>
        /// <param name="id">Consent Id</param>
        /// <returns>Consent details</returns>
        public async Task<Consent> FetchConsent(Guid id)
        {
            return await httpService.GetAsync<Consent>(URL_CONSENTS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Fetch the list of loans for an item
        /// </summary>
        /// <param name="itemId">Item Id</param>
        /// <returns>Loan results list</returns>
        public async Task<PageResults<Loan>> FetchLoans(Guid itemId)
        {
            var queryStrings = new Dictionary<string, string>
            {
                { "itemId", itemId.ToString() }
            };
            return await httpService.GetAsync<PageResults<Loan>>(URL_LOANS, null, queryStrings);
        }

        /// <summary>
        /// Fetch a loan by its ID
        /// </summary>
        /// <param name="id">Loan Id</param>
        /// <returns>Loan details</returns>
        public async Task<Loan> FetchLoan(Guid id)
        {
            return await httpService.GetAsync<Loan>(URL_LOANS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Update a transaction's category
        /// </summary>
        /// <param name="id">Transaction Id</param>
        /// <param name="categoryId">Category Id to assign</param>
        /// <returns>Updated transaction</returns>
        public async Task<Transaction> UpdateTransaction(Guid id, string categoryId)
        {
            var body = new Dictionary<string, string>
            {
                { "categoryId", categoryId }
            };
            return await httpService.PatchAsync<Transaction>(URL_TRANSACTIONS + "/{id}", body, null, HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Creates a "ConnectToken" that provides an "AccessToken" for client-side communication.
        /// </summary>
        /// <returns>An object containing an accessToken</returns>
        public async Task<ConnectTokenResponse> CreateConnectToken(Guid? itemId = null, ItemOptions options = null)
        {
            var body = new Dictionary<string, object>
            {
                { "itemId", itemId?.ToString() },
                { "options", options }
            }.RemoveNulls();
            return await httpService.PostAsync<ConnectTokenResponse>(URL_CONNECT_TOKEN, body);
        }

        #region Payment Recipients

        /// <summary>
        /// Create a payment recipient
        /// </summary>
        /// <param name="request">Payment recipient creation request</param>
        /// <returns>Created payment recipient</returns>
        public async Task<PaymentRecipient> CreatePaymentRecipient(CreatePaymentRecipientRequest request)
        {
            return await httpService.PostAsync<PaymentRecipient>(URL_PAYMENT_RECIPIENTS, request.ToBody());
        }

        /// <summary>
        /// Fetch all payment recipients
        /// </summary>
        /// <returns>Payment recipients list</returns>
        public async Task<PageResults<PaymentRecipient>> FetchPaymentRecipients()
        {
            return await httpService.GetAsync<PageResults<PaymentRecipient>>(URL_PAYMENT_RECIPIENTS);
        }

        /// <summary>
        /// Fetch a payment recipient by ID
        /// </summary>
        /// <param name="id">Payment recipient ID</param>
        /// <returns>Payment recipient details</returns>
        public async Task<PaymentRecipient> FetchPaymentRecipient(Guid id)
        {
            return await httpService.GetAsync<PaymentRecipient>(URL_PAYMENT_RECIPIENTS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Update a payment recipient
        /// </summary>
        /// <param name="id">Payment recipient ID</param>
        /// <param name="request">Payment recipient update request</param>
        /// <returns>Updated payment recipient</returns>
        public async Task<PaymentRecipient> UpdatePaymentRecipient(Guid id, CreatePaymentRecipientRequest request)
        {
            return await httpService.PatchAsync<PaymentRecipient>(URL_PAYMENT_RECIPIENTS + "/{id}", request.ToBody(), null, HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Delete a payment recipient
        /// </summary>
        /// <param name="id">Payment recipient ID</param>
        public async Task DeletePaymentRecipient(Guid id)
        {
            await httpService.DeleteAsync<dynamic>(URL_PAYMENT_RECIPIENTS + "/{id}", HTTP.Utils.GetSegment(id.ToString()), null);
        }

        #endregion

        #region Payment Requests

        /// <summary>
        /// Create a payment request
        /// </summary>
        /// <param name="request">Payment request creation request</param>
        /// <returns>Created payment request with paymentUrl</returns>
        public async Task<PaymentRequest> CreatePaymentRequest(CreatePaymentRequestRequest request)
        {
            return await httpService.PostAsync<PaymentRequest>(URL_PAYMENT_REQUESTS, request.ToBody());
        }

        /// <summary>
        /// Fetch all payment requests
        /// </summary>
        /// <returns>Payment requests list</returns>
        public async Task<PageResults<PaymentRequest>> FetchPaymentRequests()
        {
            return await httpService.GetAsync<PageResults<PaymentRequest>>(URL_PAYMENT_REQUESTS);
        }

        /// <summary>
        /// Fetch a payment request by ID
        /// </summary>
        /// <param name="id">Payment request ID</param>
        /// <returns>Payment request details</returns>
        public async Task<PaymentRequest> FetchPaymentRequest(Guid id)
        {
            return await httpService.GetAsync<PaymentRequest>(URL_PAYMENT_REQUESTS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Delete a payment request
        /// </summary>
        /// <param name="id">Payment request ID</param>
        public async Task DeletePaymentRequest(Guid id)
        {
            await httpService.DeleteAsync<dynamic>(URL_PAYMENT_REQUESTS + "/{id}", HTTP.Utils.GetSegment(id.ToString()), null);
        }

        /// <summary>
        /// Update a payment request
        /// </summary>
        /// <param name="id">Payment request ID</param>
        /// <param name="request">Fields to update</param>
        /// <returns>Updated payment request</returns>
        public async Task<PaymentRequest> UpdatePaymentRequest(Guid id, UpdatePaymentRequestRequest request)
        {
            return await httpService.PatchAsync<PaymentRequest>(URL_PAYMENT_REQUESTS + "/{id}", request.ToBody(), null, HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Create a payment request from a Pix QR code
        /// </summary>
        /// <param name="request">Pix QR payment request</param>
        /// <returns>Created payment request</returns>
        public async Task<PaymentRequest> CreatePixQrPaymentRequest(CreatePixQrPaymentRequest request)
        {
            return await httpService.PostAsync<PaymentRequest>(URL_PAYMENT_REQUESTS + "/pix-qr", request.ToBody());
        }

        /// <summary>
        /// Fetch the scheduled payments of a payment request
        /// </summary>
        /// <param name="id">Payment request ID</param>
        /// <returns>Scheduled payments list</returns>
        public async Task<PageResults<SchedulePayment>> FetchPaymentRequestSchedules(Guid id)
        {
            return await httpService.GetAsync<PageResults<SchedulePayment>>(URL_PAYMENT_REQUESTS + "/{id}/schedules", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Cancel all scheduled payments of a payment request
        /// </summary>
        /// <param name="id">Payment request ID</param>
        public async Task CancelPaymentRequestSchedules(Guid id)
        {
            await httpService.PostAsync<dynamic>(URL_PAYMENT_REQUESTS + "/{id}/schedules/cancel", null, null, HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Cancel a specific scheduled payment of a payment request
        /// </summary>
        /// <param name="id">Payment request ID</param>
        /// <param name="scheduleId">Scheduled payment ID</param>
        public async Task CancelPaymentRequestSchedule(Guid id, Guid scheduleId)
        {
            await httpService.PostAsync<dynamic>(URL_PAYMENT_REQUESTS + "/{id}/schedules/{scheduleId}/cancel", null, null,
                new Dictionary<string, string> { { "id", id.ToString() }, { "scheduleId", scheduleId.ToString() } });
        }

        #endregion

        #region Automatic Pix

        /// <summary>
        /// Create an Automatic Pix payment request (recurring Pix consent)
        /// </summary>
        /// <param name="request">Automatic Pix creation request</param>
        /// <returns>Created payment request with paymentUrl</returns>
        public async Task<PaymentRequest> CreateAutomaticPixPaymentRequest(CreateAutomaticPixPaymentRequest request)
        {
            return await httpService.PostAsync<PaymentRequest>(URL_PAYMENT_REQUESTS + "/automatic-pix", request.ToBody());
        }

        /// <summary>
        /// Cancel an Automatic Pix consent
        /// </summary>
        /// <param name="id">Payment request ID</param>
        public async Task CancelAutomaticPixConsent(Guid id)
        {
            await httpService.PostAsync<dynamic>(URL_PAYMENT_REQUESTS + "/{id}/automatic-pix/cancel", null, null, HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Schedule an Automatic Pix payment under an existing consent
        /// </summary>
        /// <param name="id">Payment request ID</param>
        /// <param name="request">Schedule request</param>
        /// <returns>Scheduled Automatic Pix payment</returns>
        public async Task<AutomaticPixPayment> ScheduleAutomaticPixPayment(Guid id, ScheduleAutomaticPixPaymentRequest request)
        {
            return await httpService.PostAsync<AutomaticPixPayment>(URL_PAYMENT_REQUESTS + "/{id}/automatic-pix/schedule", request.ToBody(), null, HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Fetch the Automatic Pix scheduled payments of a payment request
        /// </summary>
        /// <param name="id">Payment request ID</param>
        /// <returns>Automatic Pix payments list</returns>
        public async Task<PageResults<AutomaticPixPayment>> FetchAutomaticPixSchedules(Guid id)
        {
            return await httpService.GetAsync<PageResults<AutomaticPixPayment>>(URL_PAYMENT_REQUESTS + "/{id}/automatic-pix/schedules", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Fetch a single Automatic Pix scheduled payment
        /// </summary>
        /// <param name="requestId">Payment request ID</param>
        /// <param name="paymentId">Automatic Pix payment ID</param>
        /// <returns>Automatic Pix payment detail</returns>
        public async Task<AutomaticPixPayment> FetchAutomaticPixSchedule(Guid requestId, string paymentId)
        {
            return await httpService.GetAsync<AutomaticPixPayment>(URL_PAYMENT_REQUESTS + "/{requestId}/automatic-pix/schedules/{paymentId}",
                new Dictionary<string, string> { { "requestId", requestId.ToString() }, { "paymentId", paymentId } });
        }

        /// <summary>
        /// Cancel an Automatic Pix scheduled payment
        /// </summary>
        /// <param name="id">Payment request ID</param>
        /// <param name="scheduleId">Automatic Pix payment ID</param>
        public async Task CancelAutomaticPixSchedule(Guid id, string scheduleId)
        {
            await httpService.PostAsync<dynamic>(URL_PAYMENT_REQUESTS + "/{id}/automatic-pix/schedules/{scheduleId}/cancel", null, null,
                new Dictionary<string, string> { { "id", id.ToString() }, { "scheduleId", scheduleId } });
        }

        /// <summary>
        /// Retry an Automatic Pix scheduled payment
        /// </summary>
        /// <param name="id">Payment request ID</param>
        /// <param name="scheduleId">Automatic Pix payment ID</param>
        /// <param name="request">Retry request (target date)</param>
        public async Task RetryAutomaticPixSchedule(Guid id, string scheduleId, RetryAutomaticPixPaymentRequest request)
        {
            await httpService.PostAsync<dynamic>(URL_PAYMENT_REQUESTS + "/{id}/automatic-pix/schedules/{scheduleId}/retry", request.ToBody(), null,
                new Dictionary<string, string> { { "id", id.ToString() }, { "scheduleId", scheduleId } });
        }

        #endregion

        #region Payment Intents

        /// <summary>
        /// Create a payment intent to initiate a payment
        /// </summary>
        /// <param name="request">Payment intent creation request</param>
        /// <returns>Created payment intent with consentUrl</returns>
        public async Task<PaymentIntent> CreatePaymentIntent(CreatePaymentIntentRequest request)
        {
            return await httpService.PostAsync<PaymentIntent>(URL_PAYMENT_INTENTS, request.ToBody());
        }

        /// <summary>
        /// Fetch all payment intents
        /// </summary>
        /// <returns>Payment intents list</returns>
        public async Task<PageResults<PaymentIntent>> FetchPaymentIntents()
        {
            return await httpService.GetAsync<PageResults<PaymentIntent>>(URL_PAYMENT_INTENTS);
        }

        /// <summary>
        /// Fetch a payment intent by ID
        /// </summary>
        /// <param name="id">Payment intent ID</param>
        /// <returns>Payment intent details</returns>
        public async Task<PaymentIntent> FetchPaymentIntent(Guid id)
        {
            return await httpService.GetAsync<PaymentIntent>(URL_PAYMENT_INTENTS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        #endregion

        #region Payment Customers

        /// <summary>
        /// Create a payment customer
        /// </summary>
        /// <param name="request">Payment customer creation request</param>
        /// <returns>Created payment customer</returns>
        public async Task<PaymentCustomer> CreatePaymentCustomer(CreatePaymentCustomerRequest request)
        {
            return await httpService.PostAsync<PaymentCustomer>(URL_PAYMENT_CUSTOMERS, request.ToBody());
        }

        /// <summary>
        /// Fetch all payment customers
        /// </summary>
        /// <returns>Payment customers list</returns>
        public async Task<PageResults<PaymentCustomer>> FetchPaymentCustomers()
        {
            return await httpService.GetAsync<PageResults<PaymentCustomer>>(URL_PAYMENT_CUSTOMERS);
        }

        /// <summary>
        /// Fetch a payment customer by ID
        /// </summary>
        /// <param name="id">Payment customer ID</param>
        /// <returns>Payment customer details</returns>
        public async Task<PaymentCustomer> FetchPaymentCustomer(Guid id)
        {
            return await httpService.GetAsync<PaymentCustomer>(URL_PAYMENT_CUSTOMERS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Update a payment customer
        /// </summary>
        /// <param name="id">Payment customer ID</param>
        /// <param name="request">Payment customer update request</param>
        /// <returns>Updated payment customer</returns>
        public async Task<PaymentCustomer> UpdatePaymentCustomer(Guid id, CreatePaymentCustomerRequest request)
        {
            return await httpService.PatchAsync<PaymentCustomer>(URL_PAYMENT_CUSTOMERS + "/{id}", request.ToBody(), null, HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Delete a payment customer
        /// </summary>
        /// <param name="id">Payment customer ID</param>
        public async Task DeletePaymentCustomer(Guid id)
        {
            await httpService.DeleteAsync<dynamic>(URL_PAYMENT_CUSTOMERS + "/{id}", HTTP.Utils.GetSegment(id.ToString()), null);
        }

        #endregion

        #region Payment Recipient Institutions

        /// <summary>
        /// Fetch the list of payment recipient institutions
        /// </summary>
        /// <returns>Payment institutions list</returns>
        public async Task<PageResults<PaymentInstitution>> FetchPaymentRecipientInstitutions()
        {
            return await httpService.GetAsync<PageResults<PaymentInstitution>>(URL_PAYMENT_RECIPIENT_INSTITUTIONS);
        }

        /// <summary>
        /// Fetch a payment recipient institution by ID
        /// </summary>
        /// <param name="id">Institution ID</param>
        /// <returns>Payment institution details</returns>
        public async Task<PaymentInstitution> FetchPaymentRecipientInstitution(Guid id)
        {
            return await httpService.GetAsync<PaymentInstitution>(URL_PAYMENT_RECIPIENT_INSTITUTIONS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        #endregion

        #region Smart Transfers

        /// <summary>
        /// Create a Smart Transfer payment under an existing preauthorization
        /// </summary>
        /// <param name="request">Smart Transfer payment request</param>
        /// <returns>Created Smart Transfer payment</returns>
        public async Task<SmartTransferPayment> CreateSmartTransferPayment(CreateSmartTransferPayment request)
        {
            return await httpService.PostAsync<SmartTransferPayment>(URL_SMART_TRANSFER_PAYMENTS, request.ToBody());
        }

        /// <summary>
        /// Fetch a Smart Transfer payment by ID
        /// </summary>
        /// <param name="id">Smart Transfer payment ID</param>
        /// <returns>Smart Transfer payment details</returns>
        public async Task<SmartTransferPayment> FetchSmartTransferPayment(string id)
        {
            return await httpService.GetAsync<SmartTransferPayment>(URL_SMART_TRANSFER_PAYMENTS + "/{id}", HTTP.Utils.GetSegment(id));
        }

        /// <summary>
        /// Create a Smart Transfer preauthorization (recurring transfer consent)
        /// </summary>
        /// <param name="request">Preauthorization request</param>
        /// <returns>Created Smart Transfer preauthorization with consentUrl</returns>
        public async Task<SmartTransferPreauthorization> CreateSmartTransferPreauthorization(CreateSmartTransferPreauthorization request)
        {
            return await httpService.PostAsync<SmartTransferPreauthorization>(URL_SMART_TRANSFER_PREAUTHORIZATIONS, request.ToBody());
        }

        /// <summary>
        /// Fetch all Smart Transfer preauthorizations
        /// </summary>
        /// <returns>Preauthorizations list</returns>
        public async Task<PageResults<SmartTransferPreauthorization>> FetchSmartTransferPreauthorizations()
        {
            return await httpService.GetAsync<PageResults<SmartTransferPreauthorization>>(URL_SMART_TRANSFER_PREAUTHORIZATIONS);
        }

        /// <summary>
        /// Fetch a Smart Transfer preauthorization by ID
        /// </summary>
        /// <param name="id">Preauthorization ID</param>
        /// <returns>Preauthorization details</returns>
        public async Task<SmartTransferPreauthorization> FetchSmartTransferPreauthorization(string id)
        {
            return await httpService.GetAsync<SmartTransferPreauthorization>(URL_SMART_TRANSFER_PREAUTHORIZATIONS + "/{id}", HTTP.Utils.GetSegment(id));
        }

        /// <summary>
        /// Fetch the payments of a Smart Transfer preauthorization
        /// </summary>
        /// <param name="id">Preauthorization ID</param>
        /// <returns>Smart Transfer payments list</returns>
        public async Task<PageResults<SmartTransferPayment>> FetchSmartTransferPreauthorizationPayments(string id)
        {
            return await httpService.GetAsync<PageResults<SmartTransferPayment>>(URL_SMART_TRANSFER_PREAUTHORIZATIONS + "/{id}/payments", HTTP.Utils.GetSegment(id));
        }

        #endregion

        #region Category Rules

        /// <summary>
        /// Fetch the list of client category rules
        /// </summary>
        /// <returns>Category rules paged results list</returns>
        public async Task<PageResults<ClientCategoryRule>> FetchCategoryRules()
        {
            return await httpService.GetAsync<PageResults<ClientCategoryRule>>(URL_CATEGORY_RULES);
        }

        /// <summary>
        /// Create a client category rule
        /// </summary>
        /// <param name="request">Category rule creation request</param>
        /// <returns>Created category rule</returns>
        public async Task<ClientCategoryRule> CreateCategoryRule(CreateClientCategoryRule request)
        {
            return await httpService.PostAsync<ClientCategoryRule>(URL_CATEGORY_RULES, request.ToBody());
        }

        #endregion

        #region Bills

        /// <summary>
        /// Fetch the list of credit card bills for an account
        /// </summary>
        /// <param name="accountId">Account ID</param>
        /// <returns>Bills paged results list</returns>
        public async Task<PageResults<Bill>> FetchBills(Guid accountId)
        {
            var queryStrings = new Dictionary<string, string> { { "accountId", accountId.ToString() } };
            return await httpService.GetAsync<PageResults<Bill>>(URL_BILLS, null, queryStrings);
        }

        /// <summary>
        /// Fetch a bill by ID
        /// </summary>
        /// <param name="id">Bill ID</param>
        /// <returns>Bill details</returns>
        public async Task<Bill> FetchBill(string id)
        {
            return await httpService.GetAsync<Bill>(URL_BILLS + "/{id}", HTTP.Utils.GetSegment(id));
        }

        #endregion

        #region Merchants

        /// <summary>
        /// Look up merchants by CNPJ list
        /// </summary>
        /// <param name="cnpjs">List of CNPJs to look up</param>
        /// <returns>Found merchants, plus valid-but-not-found and invalid CNPJs</returns>
        public async Task<GetMerchantsResponse> FetchMerchants(IEnumerable<string> cnpjs)
        {
            var queryStrings = new Dictionary<string, string> { { "cnpjs", string.Join(",", cnpjs) } };
            return await httpService.GetAsync<GetMerchantsResponse>(URL_MERCHANTS, null, queryStrings);
        }

        #endregion

        #region Boleto Management (Beta)

        /// <summary>
        /// BETA. Create a boleto connection from credentials.
        /// </summary>
        /// <param name="request">Boleto connection request</param>
        /// <returns>Created boleto connection</returns>
        public async Task<BoletoConnection> CreateBoletoConnection(CreateBoletoConnection request)
        {
            return await httpService.PostAsync<BoletoConnection>(URL_BOLETO_CONNECTIONS, request.ToBody());
        }

        /// <summary>
        /// BETA. Create a boleto connection from an existing item.
        /// </summary>
        /// <param name="request">Boleto connection from item request</param>
        /// <returns>Created boleto connection</returns>
        public async Task<BoletoConnection> CreateBoletoConnectionFromItem(CreateBoletoConnectionFromItem request)
        {
            return await httpService.PostAsync<BoletoConnection>(URL_BOLETO_CONNECTIONS + "/from-item", request.ToBody());
        }

        /// <summary>
        /// BETA. Issue a boleto.
        /// </summary>
        /// <param name="request">Boleto creation request</param>
        /// <returns>Issued boleto</returns>
        public async Task<IssuedBoleto> CreateBoleto(CreateBoleto request)
        {
            return await httpService.PostAsync<IssuedBoleto>(URL_BOLETOS, request.ToBody());
        }

        /// <summary>
        /// BETA. Fetch a boleto by ID.
        /// </summary>
        /// <param name="id">Boleto ID</param>
        /// <returns>Issued boleto</returns>
        public async Task<IssuedBoleto> FetchBoleto(string id)
        {
            return await httpService.GetAsync<IssuedBoleto>(URL_BOLETOS + "/{id}", HTTP.Utils.GetSegment(id));
        }

        /// <summary>
        /// BETA. Cancel a boleto.
        /// </summary>
        /// <param name="id">Boleto ID</param>
        /// <returns>Issued boleto with updated status</returns>
        public async Task<IssuedBoleto> CancelBoleto(string id)
        {
            return await httpService.PostAsync<IssuedBoleto>(URL_BOLETOS + "/{id}/cancel", null, null, HTTP.Utils.GetSegment(id));
        }

        #endregion


        /*
         * Execution Helpers
         */

        /// <summary>
        /// Creates a new item for a connector and starts reviewing the status until completition
        /// </summary>
        /// <param name="request">The item parameters</param>
        /// <returns>an object with the info to retrieve the data when the execution is ready</returns>
        public async Task<Item> ExecuteAndWait(ItemParameters request)
        {
            try
            {
                Item item = await CreateItem(request);

                do
                {
                    await Task.Delay(STATUS_POLL_INTERVAL);
                    item = await FetchItem(item.Id);
                }
                while (!item.HasFinished());

                return item;
            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Details != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }
    }
}
