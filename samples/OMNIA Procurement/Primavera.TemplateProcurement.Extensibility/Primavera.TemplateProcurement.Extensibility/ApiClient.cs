using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ErpBS100;
using IdentityModel.Client;
using Newtonsoft.Json;
using Primavera.TemplateProcurement.Extensibility.Entities;
using Primavera.TemplateProcurement.Extensibility.Entities.Requests;
using Primavera.TemplateProcurement.Extensibility.Entities.Responses;
using StdPlatBS100;

namespace Primavera.TemplateProcurement.Extensibility
{
    public static class ApiClient
    {
        //OMNIA WebAPI
        private static Uri _apiEndpoint;
        private static string _tenant;
        private static string _clientId;
        private static string _clientSecret;
        private static Uri _identityEndpoint;
        private static string _omniaBD;
        private static MyToken AccessToken;

        //Control Variables
        private static bool _initialized = false;

        //ERP
        private static ErpBS erp;
        private static StdBSInterfPub plataforma;


        #region Public Methods

        public static bool Intialized()
        {
            return _initialized;
        }

        public static void InitializeApiClient(ref ErpBS100.ErpBS BSO, ref StdBSInterfPub PSO)
        {
            erp = BSO;
            plataforma = PSO;

            string sql = "select CDU_EndpointOmnia, CDU_TenantOmnia,CDU_ClientIdOmnia,CDU_ClientSecretOmnia,CDU_OmniaDB,CDU_EndpointIdentity,CDU_EndpointApi from TDU_ParametrosOMNIA";
            DataTable result = erp.ConsultaDataTable(sql);
            if (result?.Rows?.Count > 0)
            {
                string endpointOmnia = result.Rows[0]["CDU_EndpointOmnia"].ToString();
                string tenantOmnia = result.Rows[0]["CDU_TenantOmnia"].ToString();
                string clientIdOmnia = result.Rows[0]["CDU_ClientIdOmnia"].ToString();
                string clientSecretOmnia = result.Rows[0]["CDU_ClientSecretOmnia"].ToString();
                string omniaDB = result.Rows[0]["CDU_OmniaDB"].ToString();
                string endpointIdentity = result.Rows[0]["CDU_EndpointIdentity"].ToString();
                string endpointApi = result.Rows[0]["CDU_EndpointApi"].ToString();

                Uri apiEndpoint;
                Uri _identityEndpoint;

                apiEndpoint = new Uri(new Uri(endpointOmnia), endpointApi);
                _identityEndpoint = new Uri(new Uri(endpointOmnia), endpointIdentity);

                ApiClient.Initialize(apiEndpoint, tenantOmnia, _identityEndpoint, clientIdOmnia, clientSecretOmnia, omniaDB);
            }

        }

        public static async Task<Dictionary<string, object>> ValidateRequisition(string omniaCode, int operation)
        {

            var accessToken = GetJwtTokenAsync().GetAwaiter().GetResult();

            var authValue = new AuthenticationHeaderValue("Bearer", accessToken);
            var client = new HttpClient { DefaultRequestHeaders = { Authorization = authValue } };

            ValidateRequisitionBody validateRequisitionBody = new ValidateRequisitionBody()
            {
                omniaCode = omniaCode,
                operation = operation
            };

            string Body = JsonConvert.SerializeObject(validateRequisitionBody);

            HttpContent _Body = new StringContent(Body);
            _Body.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var apiEndpoint = $"{_apiEndpoint}{_tenant}/PRD/application/behaviours/Default/ValidateRequisition";

            var requestResult = client.PostAsync(apiEndpoint, _Body).GetAwaiter().GetResult();

            if (!requestResult.IsSuccessStatusCode)
            {

                if (requestResult.StatusCode == System.Net.HttpStatusCode.Unauthorized || requestResult.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return new Dictionary<string, object>()
                    {
                        {"errorMessage", responseBody }
                    };
                }
                else
                {
                    var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return new Dictionary<string, object>()
                    {
                        {"errorMessage", responseBody }
                    };
                }
            }
            else
            {
                var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
            }
        }

        public static async Task<Dictionary<string, object>> GetRequisitionPayNextStage(string omniaCode, string amount, string families)
        {
            var accessToken = await GetJwtTokenAsync();

            var authValue = new AuthenticationHeaderValue("Bearer", accessToken);
            var client = new HttpClient { DefaultRequestHeaders = { Authorization = authValue } };

            var dictionary = new Dictionary<string, object>();

            GetRequisitionNextStageBody body = new GetRequisitionNextStageBody()
            {
                Requisition = omniaCode,
                Order = -1,
                Amount = amount,
                Families = families
            };

            string Body = JsonConvert.SerializeObject(body);

            HttpContent _Body = new StringContent(Body);
            _Body.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var apiEndpoint = $"{_apiEndpoint}{_tenant}/PRD/application/behaviours/Default/GetRequisitionPayNextStage";

            var requestResult = client.PostAsync(apiEndpoint, _Body).GetAwaiter().GetResult();

            if (!requestResult.IsSuccessStatusCode)
            {
                if (requestResult.StatusCode == System.Net.HttpStatusCode.Unauthorized || requestResult.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    dictionary.Add("errorMessage", responseBody);
                    return dictionary;
                }
                else
                {
                    var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    dictionary.Add("errorMessage", responseBody);
                    return dictionary;
                }
            }
            else
            {
                var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();


                GetRequisitionPayNextStageBody result = JsonConvert.DeserializeObject<GetRequisitionPayNextStageBody>(responseBody);
                dictionary.Add("result", result);
                return dictionary;
            }
        }

        public static async Task<Dictionary<string, object>> GetCompanyConfig(string company)
        {
            var accessToken = await GetJwtTokenAsync();

            var authValue = new AuthenticationHeaderValue("Bearer", accessToken);
            var client = new HttpClient { DefaultRequestHeaders = { Authorization = authValue } };

            var dictionary = new Dictionary<string, object>();

            var apiEndpoint = $"{_apiEndpoint}{_tenant}/PRD/application/Company/default/{company}";

            var requestResult = client.GetAsync(apiEndpoint).GetAwaiter().GetResult();

            if (!requestResult.IsSuccessStatusCode)
            {
                if (requestResult.StatusCode == System.Net.HttpStatusCode.Unauthorized || requestResult.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    dictionary.Add("errorMessage", responseBody);
                    return dictionary;
                }
                else
                {
                    var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    dictionary.Add("errorMessage", responseBody);
                    return dictionary;
                }
            }
            else
            {
                var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
                return result;
            }
        }

        public static async Task<Dictionary<string, object>> NewFinancialDocumentNotification(string tipoDoc, string serieDoc, int numDoc, string entidade, string organizationalUnit, string data, string author, string approver)
        {
            var accessToken = await GetJwtTokenAsync();

            var authValue = new AuthenticationHeaderValue("Bearer", accessToken);
            var client = new HttpClient { DefaultRequestHeaders = { Authorization = authValue } };

            NewFinancialDocumentNotificationBody body = new NewFinancialDocumentNotificationBody()
            {
                code = String.Concat(tipoDoc, ".", serieDoc, ".", numDoc.ToString()),
                entidade = entidade,
                organizationalUnit = organizationalUnit,
                data = data,
                author = author,
                approver = approver
            };

            string Body = JsonConvert.SerializeObject(body);

            HttpContent _Body = new StringContent(Body);
            _Body.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var apiEndpoint = $"{_apiEndpoint}{_tenant}/PRD/application/behaviours/{erp.Contexto.CodEmp}/NewFinancialDocumentNotification";

            var requestResult = client.PostAsync(apiEndpoint, _Body).GetAwaiter().GetResult();

            if (!requestResult.IsSuccessStatusCode)
            {
                if (requestResult.StatusCode == System.Net.HttpStatusCode.Unauthorized || requestResult.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return new Dictionary<string, object>()
                    {
                        {"errorMessage", responseBody }
                    };
                }
                else
                {
                    var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return new Dictionary<string, object>()
                    {
                        {"errorMessage", responseBody }
                    };
                }
            }
            else
            {
                var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
            }

        }

        public static async Task<Dictionary<string, object>> NewQuoteRequestNotification(string tipoDoc, string serieDoc, int numDoc, string entidade)
        {
            var accessToken = await GetJwtTokenAsync();

            var authValue = new AuthenticationHeaderValue("Bearer", accessToken);
            var client = new HttpClient { DefaultRequestHeaders = { Authorization = authValue } };

            NewQuoteRequestNotificationBody body = new NewQuoteRequestNotificationBody()
            {
                quoterequests = String.Concat(tipoDoc, ".", serieDoc, ".", numDoc.ToString()),
                supplier = entidade
            };

            string Body = JsonConvert.SerializeObject(body);

            HttpContent _Body = new StringContent(Body);
            _Body.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var apiEndpoint = $"{_apiEndpoint}{_tenant}/PRD/application/behaviours/{erp.Contexto.CodEmp}/NewQuoteRequestNotification";

            var requestResult = client.PostAsync(apiEndpoint, _Body).GetAwaiter().GetResult();

            if (!requestResult.IsSuccessStatusCode)
            {
                if (requestResult.StatusCode == System.Net.HttpStatusCode.Unauthorized || requestResult.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return new Dictionary<string, object>()
                    {
                        {"errorMessage", responseBody }
                    };
                }
                else
                {
                    var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return new Dictionary<string, object>()
                    {
                        {"errorMessage", responseBody }
                    };
                }
            }
            else
            {
                var responseBody = requestResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
            }
        }

        public static string GetOmniaDB()
        {
            return _omniaBD;
        }

        #endregion

        #region Private Methods

        private static void Initialize(Uri apiEndpoint, string tenant, Uri identityEndpoint, string clientId, string clientSecret, string omniaBD)
        {
            try
            {
                _apiEndpoint = apiEndpoint;
                _tenant = tenant;
                _identityEndpoint = identityEndpoint;
                _clientId = clientId;
                _clientSecret = clientSecret;
                _omniaBD = omniaBD;

                _initialized = true;
            }
            catch
            {
                _initialized = false;
            }
        }

        private static async Task<string> GetJwtTokenAsync(bool ignoringCache = false)
        {
            if (!ignoringCache && AccessToken != null && DateTime.Now < AccessToken.expires_in.AddSeconds(-60))
            {
                return AccessToken.access_token;
            }

            var _client = new HttpClient();

            var response = _client.RequestTokenAsync(new TokenRequest
            {
                Address = _identityEndpoint.ToString(),
                GrantType = "client_credentials",

                ClientId = _clientId,
                ClientSecret = _clientSecret,

                Parameters =
                {
                    { "scope", "api" }
                }
            }).GetAwaiter().GetResult();

            if (response.IsError)
            {
                _client = null;
                throw new Exception("Client ID inválido");
            }

            var token = response.AccessToken;
            AccessToken = new MyToken()
            {
                access_token = response.AccessToken,
                expires_in = DateTime.Now.AddSeconds(response.ExpiresIn),
                token_type = response.TokenType
            };

            return AccessToken.access_token;

        }

        #endregion

    }
}
