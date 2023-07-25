using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using WebHookSample.Helpers;

namespace WebhookSample.Controllers
{
    [RoutePrefix("api/Webhook")]
    public class WebhookController : ApiController
    {
        private string logFilePath = $@"C:\temp\WebHookSampleLog.txt";

        /// <summary>The secret. Must be a 24-character unsigned alphanumeric string.</summary>
        private static string decodeSecret = "@24-S!z3dEnc0d!ngS3cr3t@";

        #region Base

        /// <summary>The route must be constructed using the following configuration: "ACCOUNT/SUBSCRIPTIONALIAS/INSTANCE/COMPANY/MODULE/SENDER/SOURCE/EVENT".</summary>
        [Route("ACCOUNT/SUBSCRIPTIONALIAS/INSTANCE/COMPANY/Base/Service/BasBSClientes/DepoisDeGravar/")]
        [HttpPost]
        public HttpResponseMessage BasBSClientesDepoisDeGravarWebhookHandler([FromBody] Dictionary<string, object> parameters)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                Guid requestId = new Guid(parameters["requestId"].ToString());
                DateTime timestamp = (DateTime)parameters["timestamp"];
                string payload = parameters["payload"].ToString();

                if (string.IsNullOrEmpty(payload))
                {
                    sb.AppendLine("Sem conteúdo para processar!\r\n");
                    return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Sem conteúdo para processar!" };
                }

                sb.AppendLine($"Request Id: {requestId} | Timestamp: {timestamp}\r\n");

                string decryptedMessage = CryptoHelper.Decrypt(requestId, timestamp, decodeSecret, payload);
                Dictionary<string, object> message = JsonConvert.DeserializeObject<Dictionary<string, object>>(decryptedMessage);

                sb.AppendLine("Mensagem desencriptada:");
                sb.AppendLine(decryptedMessage + "\r\n");

                var context = JsonConvert.DeserializeObject<Dictionary<string, object>>(message["Context"].ToString());
                var content = JsonConvert.DeserializeObject<object[]>(message["Content"].ToString());
                BasBE100.BasBECliente cliente = JsonConvert.DeserializeObject<BasBE100.BasBECliente>(content[0].ToString());
                
                sb.AppendLine($"Dados desserializados => Cliente {cliente.Cliente}.");
                sb.AppendLine("\r\n");

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Erro ao processar resposta! {ex.Message}");
                sb.AppendLine("\r\n");

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Erro ao processar resposta!", Content = new StringContent(ex.Message) });
            }
            finally
            {
                this.Log(sb.ToString());
            }
        }

        /// <summary>The route must be constructed using the following configuration: "ACCOUNT/SUBSCRIPTIONALIAS/INSTANCE/COMPANY/MODULE/SENDER/SOURCE/EVENT".</summary>
        [Route("ACCOUNT/SUBSCRIPTIONALIAS/INSTANCE/COMPANY/Base/Editor/FichaClientes/TeclaPressionada/")]
        [HttpPost]
        public HttpResponseMessage BasFichaClientesTeclaPressionadaWebhookHandler([FromBody] Dictionary<string, object> parameters)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                Guid requestId = new Guid(parameters["requestId"].ToString());
                DateTime timestamp = (DateTime)parameters["timestamp"];
                string payload = parameters["payload"].ToString();

                if (string.IsNullOrEmpty(payload))
                {
                    sb.AppendLine("Sem conteúdo para processar!\r\n");
                    return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Sem conteúdo para processar!" };
                }

                sb.AppendLine($"Request Id: {requestId} | Timestamp: {timestamp}\r\n");

                string decryptedMessage = CryptoHelper.Decrypt(requestId, timestamp, decodeSecret, payload);
                Dictionary<string, object> message = JsonConvert.DeserializeObject<Dictionary<string, object>>(decryptedMessage);

                sb.AppendLine("Mensagem desencriptada:");
                sb.AppendLine(decryptedMessage + "\r\n");

                var context = JsonConvert.DeserializeObject<Dictionary<string, object>>(message["Context"].ToString());
                var content = JsonConvert.DeserializeObject<object[]>(message["Content"].ToString());

                sb.AppendLine($"Dados desserializados => Key {content[0]}; Shift: {content[1]}.");
                sb.AppendLine("\r\n");

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Erro ao processar resposta! {ex.Message}");
                sb.AppendLine("\r\n");

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Erro ao processar resposta!", Content = new StringContent(ex.Message) });
            }
            finally
            {
                this.Log(sb.ToString());
            }
        }

        #endregion

        #region Internal

        /// <summary>The route must be constructed using the following configuration: "ACCOUNT/SUBSCRIPTIONALIAS/INSTANCE/COMPANY/MODULE/SENDER/SOURCE/EVENT".</summary>
        [Route("ACCOUNT/SUBSCRIPTIONALIAS/INSTANCE/COMPANY/Internal/Service/IntBSInternos/DepoisDeEditar/")]
        [HttpPost]
        public HttpResponseMessage IntBSInternosDepoisDeEditarWebhookHandler([FromBody] Dictionary<string, object> parameters)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                Guid requestId = new Guid(parameters["requestId"].ToString());
                DateTime timestamp = (DateTime)parameters["timestamp"];
                string payload = parameters["payload"].ToString();

                if (string.IsNullOrEmpty(payload))
                {
                    sb.AppendLine("Sem conteúdo para processar!\r\n");
                    return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Sem conteúdo para processar!" };
                }

                sb.AppendLine($"Request Id: {requestId} | Timestamp: {timestamp}\r\n");

                string decryptedMessage = CryptoHelper.Decrypt(requestId, timestamp, decodeSecret, payload);
                Dictionary<string, object> message = JsonConvert.DeserializeObject<Dictionary<string, object>>(decryptedMessage);

                sb.AppendLine("Mensagem desencriptada:");
                sb.AppendLine(decryptedMessage + "\r\n");

                var context = JsonConvert.DeserializeObject<Dictionary<string, object>>(message["Context"].ToString());
                var content = JsonConvert.DeserializeObject<object[]>(message["Content"].ToString());

                sb.AppendLine($"Dados desserializados => Tipo Documento: {content[0]}; Número documento: {content[1]}; Série: {content[2]}; Filial: {content[3]}.");
                sb.AppendLine("\r\n");

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Erro ao processar resposta! {ex.Message}");
                sb.AppendLine("\r\n");

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Erro ao processar resposta!", Content = new StringContent(ex.Message) });
            }
            finally
            {
                this.Log(sb.ToString());
            }
        }

        /// <summary>The route must be constructed using the following configuration: "ACCOUNT/SUBSCRIPTIONALIAS/INSTANCE/COMPANY/MODULE/SENDER/SOURCE/EVENT".</summary>
        [Route("ACCOUNT/SUBSCRIPTIONALIAS/INSTANCE/COMPANY/Internal/Editor/EditorInternos/DepoisDeImprimir/")]
        [HttpPost]
        public HttpResponseMessage InEditorInternosDepoisDeImprimirWebhookHandler([FromBody] Dictionary<string, object> parameters)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                Guid requestId = new Guid(parameters["requestId"].ToString());
                DateTime timestamp = (DateTime)parameters["timestamp"];
                string payload = parameters["payload"].ToString();

                if (string.IsNullOrEmpty(payload))
                {
                    sb.AppendLine("Sem conteúdo para processar!\r\n");
                    return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Sem conteúdo para processar!" };
                }

                sb.AppendLine($"Request Id: {requestId} | Timestamp: {timestamp}\r\n");

                string decryptedMessage = CryptoHelper.Decrypt(requestId, timestamp, decodeSecret, payload);
                Dictionary<string, object> message = JsonConvert.DeserializeObject<Dictionary<string, object>>(decryptedMessage);

                sb.AppendLine("Mensagem desencriptada:");
                sb.AppendLine(decryptedMessage + "\r\n");

                var context = JsonConvert.DeserializeObject<Dictionary<string, object>>(message["Context"].ToString());
                var content = JsonConvert.DeserializeObject<object[]>(message["Content"].ToString());

                sb.AppendLine($"Dados desserializados => Filial: {content[0]}; Tipo Documento: {content[1]}; Série: {content[2]}; Número documento: {content[3]}.");
                sb.AppendLine("\r\n");

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Erro ao processar resposta! {ex.Message}");
                sb.AppendLine("\r\n");

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Erro ao processar resposta!", Content = new StringContent(ex.Message) });
            }
            finally
            {
                this.Log(sb.ToString());
            }
        }

        #endregion

        #region Production

        /// <summary>The route must be constructed using the following configuration: "ACCOUNT/SUBSCRIPTIONALIAS/INSTANCE/COMPANY/MODULE/SENDER/SOURCE/EVENT".</summary>
        [Route("ACCOUNT/SUBSCRIPTIONALIAS/INSTANCE/COMPANY/Production/Service/GprBSOrdensFabrico/DepoisDeAnularID/")]
        [HttpPost]
        public HttpResponseMessage GprBSOrdensFabricoDepoisDeAnularIDWebhookHandler([FromBody] Dictionary<string, object> parameters)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                Guid requestId = new Guid(parameters["requestId"].ToString());
                DateTime timestamp = (DateTime)parameters["timestamp"];
                string payload = parameters["payload"].ToString();

                if (string.IsNullOrEmpty(payload))
                {
                    sb.AppendLine("Sem conteúdo para processar!\r\n");
                    return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Sem conteúdo para processar!" };
                }

                sb.AppendLine($"Request Id: {requestId} | Timestamp: {timestamp}\r\n");

                string decryptedMessage = CryptoHelper.Decrypt(requestId, timestamp, decodeSecret, payload);
                Dictionary<string, object> message = JsonConvert.DeserializeObject<Dictionary<string, object>>(decryptedMessage);

                sb.AppendLine("Mensagem desencriptada:");
                sb.AppendLine(decryptedMessage + "\r\n");

                var context = JsonConvert.DeserializeObject<Dictionary<string, object>>(message["Context"].ToString());
                var content = JsonConvert.DeserializeObject<object[]>(message["Content"].ToString());

                sb.AppendLine($"Dados desserializados => Identificador Ordem de Fabrico: {content[0]}.");
                sb.AppendLine("\r\n");

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Erro ao processar resposta! {ex.Message}");
                sb.AppendLine("\r\n");

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Erro ao processar resposta!", Content = new StringContent(ex.Message) });
            }
            finally
            {
                this.Log(sb.ToString());
            }
        }


        /// <summary>The route must be constructed using the following configuration: "ACCOUNT/SUBSCRIPTIONALIAS/INSTANCE/COMPANY/MODULE/SENDER/SOURCE/EVENT".</summary>
        [Route("ACCOUNT/SUBSCRIPTIONALIAS/INSTANCE/COMPANY/Production/Editor/EditorOrdensFabrico/ErroAoGravar/")]
        [HttpPost]
        public HttpResponseMessage GprEditorOrdensFabricoErroAoGravarWebhookHandler([FromBody] Dictionary<string, object> parameters)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                Guid requestId = new Guid(parameters["requestId"].ToString());
                DateTime timestamp = (DateTime)parameters["timestamp"];
                string payload = parameters["payload"].ToString();

                if (string.IsNullOrEmpty(payload))
                {
                    sb.AppendLine("Sem conteúdo para processar!\r\n");
                    return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Sem conteúdo para processar!" };
                }

                sb.AppendLine($"Request Id: {requestId} | Timestamp: {timestamp}\r\n");

                string decryptedMessage = CryptoHelper.Decrypt(requestId, timestamp, decodeSecret, payload);
                Dictionary<string, object> message = JsonConvert.DeserializeObject<Dictionary<string, object>>(decryptedMessage);

                sb.AppendLine("Mensagem desencriptada:");
                sb.AppendLine(decryptedMessage + "\r\n");

                var context = JsonConvert.DeserializeObject<Dictionary<string, object>>(message["Context"].ToString());
                var content = JsonConvert.DeserializeObject<object[]>(message["Content"].ToString());

                sb.AppendLine($"Dados desserializados => Ordem de Fabrico: {content[0]}; Error Number: {content[1]}; Mensagem: {content[2]}.");
                sb.AppendLine("\r\n");

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Erro ao processar resposta! {ex.Message}");
                sb.AppendLine("\r\n");

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Erro ao processar resposta!", Content = new StringContent(ex.Message) });
            }
            finally
            {
                this.Log(sb.ToString());
            }
        }

        #endregion

        private void Log(string message)
        {
            try
            {
                File.AppendAllText(logFilePath, message, new UTF8Encoding(false, true));
            }
            catch (Exception)
            {
            }
        }
    }
}