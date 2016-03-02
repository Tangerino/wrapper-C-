using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WimdioApiProxy.v2.Rest;
using WimdioApiProxy.v2.DataTransferObjects;
using WimdioApiProxy.v2.DataTransferObjects.Accounts;
using WimdioApiProxy.v2.DataTransferObjects.Etls;
using WimdioApiProxy.v2.DataTransferObjects.Places;
using WimdioApiProxy.v2.DataTransferObjects.Things;
using WimdioApiProxy.v2.DataTransferObjects.Users;
using WimdioApiProxy.v2.DataTransferObjects.NormalizationFactors;
using WimdioApiProxy.v2.DataTransferObjects.Formulas;

namespace WimdioApiProxy.v2
{
    public class WimdioApiClient : IWimdioApiClient
    {
        private const string _baseUrl = "https://wimd.io/v2";
        private string _apiKey;

        private readonly ILog _log = LogManager.GetLogger(typeof(WimdioApiClient));

        public async Task Login(Credentials credentials)
        {
            try
            {
                var client = new AuthenticationClient(_baseUrl);

                _apiKey = (await client.Post<LoginResponse>("account/login", credentials))?.ApiKey;
            }
            catch (Exception ex)
            {
                _log.Error($"Login(credentials.Email={credentials.Email}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task Logout()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Post<BasicResponse>("account/logout", new EmptyObject());
            }
            catch (Exception ex)
            {
                _log.Error($"Logout() failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<string> ChangePassword(Credentials credentials)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Post<BasicResponse>("account/password", credentials);

                return response.Status;
            }
            catch (Exception ex)
            {
                _log.Error($"ChangePassword(credentials.Email={credentials.Email}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<string> ResetAccount(Account account)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Post<BasicResponse>("account/reset", account);

                return response.Status;
            }
            catch (Exception ex)
            {
                _log.Error($"ResetAccount(account.Email={account.Email}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task CreatePocket(string pocketName, object pocket)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Post<BasicResponse>($"account/pocket/{pocketName}", pocket);
            }
            catch (Exception ex)
            {
                _log.Error($"CreatePocket(pocketName={pocketName}, pocket='{JsonConvert.SerializeObject(pocket)}') failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeletePocket(string pocketName)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Delete<BasicResponse>($"account/pocket/{pocketName}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeletePocket(pocketName={pocketName}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<dynamic> ReadPocket(string pocketName)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<object>($"account/pocket/{pocketName}");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadPocket(pocketName={pocketName}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<User>> ReadUsers()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<User[]>("users");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadUsers() failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<User> CreateUser(NewUser user)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<User[]>("user", user))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateUser(user={JsonConvert.SerializeObject(user)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<User> ReadUser(Guid userId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<User[]>($"user/{userId}"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadUser(userId={userId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<User> UpdateUser(Guid userId, UpdateUser user)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<User[]>($"user/{userId}", user))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateUser(userId={userId}, user={JsonConvert.SerializeObject(user)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteUser(Guid userId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"user/{userId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteUser(userId={userId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<User> ChangePermissions(Guid userId, Permission permissions)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<User[]>($"user/permissions/{userId}", new { permissions }))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ChangePermissions(userId={userId}, permissions) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Place>> ReadPlaces()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<Place[]>("places");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadPlaces() failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Place> CreatePlace(NewPlace place)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<Place[]>("place", place))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreatePlace(place={JsonConvert.SerializeObject(place)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Place> ReadPlace(Guid placeId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<Place[]>($"place/{placeId}"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadPlace(placeId={placeId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Place> UpdatePlace(Guid placeId, NewPlace place)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<Place[]>($"place/{placeId}", place))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdatePlace(placeId={placeId}, user={JsonConvert.SerializeObject(place)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeletePlace(Guid placeId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"place/{placeId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeletePlace(placeId={placeId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task LinkPlace(Guid placeId, Guid userId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Post<BasicResponse>($"place/{placeId}/link/{userId}", new EmptyObject());
            }
            catch (Exception ex)
            {
                _log.Error($"LinkPlace(placeId={placeId}, userId={userId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task UnlinkPlace(Guid placeId, Guid userId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"place/{placeId}/link/{userId}");
            }
            catch (Exception ex)
            {
                _log.Error($"UnlinkPlace(placeId={placeId}, userId={userId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<NormalizationFactor>> ReadNormalizationFactors(Guid placeId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<NormalizationFactor[]>($"place/{placeId}/nfs");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadNormalizationFactors(placeId={placeId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<NormalizationFactor> CreateNormalizationFactor(Guid placeId, NewNormalizationFactor normalizationFactor)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<NormalizationFactor[]>($"place/{placeId}/nf", normalizationFactor))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateNormalizationFactor(placeId={placeId}, normalizationFactor={JsonConvert.SerializeObject(normalizationFactor)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<NormalizationFactor> ReadNormalizationFactor(Guid normalizationFactorId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<NormalizationFactor[]>($"nf/{normalizationFactorId}"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadNormalizationFactor(normalizationFactorId={normalizationFactorId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<NormalizationFactor> UpdateNormalizationFactor(Guid normalizationFactorId, NewNormalizationFactor normalizationFactor)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<NormalizationFactor[]>($"nf/{normalizationFactorId}", normalizationFactor))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateNormalizationFactor(normalizationFactorId={normalizationFactorId}, normalizationFactor={JsonConvert.SerializeObject(normalizationFactor)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteNormalizationFactor(Guid normalizationFactorId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"nf/{normalizationFactorId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteNormalizationFactor(normalizationFactorId={normalizationFactorId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<NormalizationFactorValue>> ReadNormalizationFactorValues(Guid normalizationFactorId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<NormalizationFactorValue[]>($"nf/{normalizationFactorId}/values");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadNormalizationFactorValues(normalizationFactorId={normalizationFactorId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task CreateNormalizationFactorValue(Guid normalizationFactorId, NormalizationFactorValue normalizationFactorValue)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Post<NormalizationFactorValue[]>($"nf/{normalizationFactorId}/value", normalizationFactorValue);
            }
            catch (Exception ex)
            {
                _log.Error($"CreateNormalizationFactorValue(normalizationFactorId={normalizationFactorId}, normalizationFactorValue={JsonConvert.SerializeObject(normalizationFactorValue)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task UpdateNormalizationFactorValue(Guid normalizationFactorId, NormalizationFactorValue normalizationFactorValue)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Put<BasicResponse>($"nf/{normalizationFactorId}/value", normalizationFactorValue);
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateNormalizationFactorValue(normalizationFactorId={normalizationFactorId}, normalizationFactorValue={JsonConvert.SerializeObject(normalizationFactorValue)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteNormalizationFactorValue(Guid normalizationFactorId, DateTime date)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"nf/{normalizationFactorId}/value/{date.ToString("o")}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteNormalizationFactorValue(normalizationFactorId={normalizationFactorId}, date={date.ToString("o")}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Thing>> ReadThings(Guid placeId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return await client.Get<Thing[]>($"place/{placeId}");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadThings(placeId={placeId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Thing> CreateThing(Guid placeId, NewThing thing)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<Thing[]>($"place/{placeId}/thing", thing))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateThing(thing={JsonConvert.SerializeObject(thing)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Thing> ReadThing(Guid thingId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<Thing[]>($"thing/{thingId}"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadThing(thingId={thingId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Thing> UpdateThing(Guid thingId, NewThing thing)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<Thing[]>($"thing/{thingId}", thing))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateThing(thingId={thingId}, thing={JsonConvert.SerializeObject(thing)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteThing(Guid thingId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"thing/{thingId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteThing(thingId={thingId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Formula>> ReadFormulas()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<Formula[]>("formulas");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadFormulas() failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Formula> CreateFormula(NewFormula formula)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<Formula[]>("formula", formula))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateFormula(formula={JsonConvert.SerializeObject(formula)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Formula> ReadFormula(Guid formulaId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<Formula[]>($"formula/{formulaId}"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadFormula(formulaId={formulaId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<string> ReadFormulaCode(Guid formulaId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<FormulaCode[]>($"formula/{formulaId}/code"))?.FirstOrDefault()?.Code;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadFormulaCode(formulaId={formulaId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Formula> UpdateFormula(Guid formulaId, NewFormula formula)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<Formula[]>($"formula/{formulaId}", formula))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateFormula(formulaId={formulaId}, user={JsonConvert.SerializeObject(formula)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteFormula(Guid formulaId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"formula/{formulaId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteFormula(formulaId={formulaId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Etl>> ReadEtls()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<Etl[]>("etls");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadEtls() failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Etl> CreateEtl(NewEtl etl)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<Etl[]>("etl", etl))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateEtl(etl={JsonConvert.SerializeObject(etl)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Etl> ReadEtl(Guid etlId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<Etl[]>($"etl/{etlId}"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadEtl(etlId={etlId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Etl> UpdateEtl(Guid etlId, NewEtl etl)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<Etl[]>($"etl/{etlId}", etl))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateEtl(etlId={etlId}, user={JsonConvert.SerializeObject(etl)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteEtl(Guid etlId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"etl/{etlId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteEtl(etlId={etlId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
    }
}
