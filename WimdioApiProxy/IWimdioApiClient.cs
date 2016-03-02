using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WimdioApiProxy.v2.DataTransferObjects.Accounts;
using WimdioApiProxy.v2.DataTransferObjects.Etls;
using WimdioApiProxy.v2.DataTransferObjects.Formulas;
using WimdioApiProxy.v2.DataTransferObjects.NormalizationFactors;
using WimdioApiProxy.v2.DataTransferObjects.Places;
using WimdioApiProxy.v2.DataTransferObjects.Things;
using WimdioApiProxy.v2.DataTransferObjects.Users;

namespace WimdioApiProxy.v2
{
    public partial interface IWimdioApiClient
    {
        Task Login(Credentials credentials);
        Task Logout();
        Task<string> ChangePassword(Credentials credentials);
        Task<string> ResetAccount(Account account);
        Task CreatePocket(string pocketName, object pocket);
        Task DeletePocket(string pocketName);
        Task<dynamic> ReadPocket(string pocketName);

        Task<IEnumerable<User>> ReadUsers();
        Task<User> CreateUser(NewUser user);
        Task<User> ReadUser(Guid userId);
        Task<User> UpdateUser(Guid userId, UpdateUser user);
        Task DeleteUser(Guid userId);
        Task<User> ChangePermissions(Guid userId, Permission permissions);

        Task<IEnumerable<Place>> ReadPlaces();
        Task<Place> CreatePlace(NewPlace place);
        Task<Place> ReadPlace(Guid placeId);
        Task<Place> UpdatePlace(Guid placeId, NewPlace place);
        Task DeletePlace(Guid placeId);
        Task LinkPlace(Guid placeId, Guid userId);
        Task UnlinkPlace(Guid placeId, Guid userId);

        Task<IEnumerable<NormalizationFactor>> ReadNormalizationFactors(Guid placeId);
        Task<NormalizationFactor> CreateNormalizationFactor(Guid placeId, NewNormalizationFactor normalizationFactor);
        Task<NormalizationFactor> ReadNormalizationFactor(Guid normalizationFactorId);
        Task<NormalizationFactor> UpdateNormalizationFactor(Guid normalizationFactorId, NewNormalizationFactor normalizationFactor);
        Task DeleteNormalizationFactor(Guid normalizationFactorId);

        Task<IEnumerable<NormalizationFactorValue>> ReadNormalizationFactorValues(Guid normalizationFactorId);
        Task CreateNormalizationFactorValue(Guid normalizationFactorId, NormalizationFactorValue normalizationFactorValue);
        Task UpdateNormalizationFactorValue(Guid normalizationFactorId, NormalizationFactorValue normalizationFactorValue);
        Task DeleteNormalizationFactorValue(Guid normalizationFactorId, DateTime date);

        Task<IEnumerable<Thing>> ReadThings(Guid placeId);
        Task<Thing> CreateThing(Guid placeId, NewThing thing);
        Task<Thing> ReadThing(Guid thingId);
        Task<Thing> UpdateThing(Guid thingId, NewThing thing);
        Task DeleteThing(Guid thingId);

        Task<IEnumerable<Formula>> ReadFormulas();
        Task<Formula> CreateFormula(NewFormula formula);
        Task<Formula> ReadFormula(Guid formulaId);
        Task<string> ReadFormulaCode(Guid formulaId);
        Task<Formula> UpdateFormula(Guid formulaId, NewFormula formula);
        Task DeleteFormula(Guid formulaId);

        Task<IEnumerable<Etl>> ReadEtls();
        Task<Etl> CreateEtl(NewEtl etl);
        Task<Etl> ReadEtl(Guid etlId);
        Task<Etl> UpdateEtl(Guid etlId, NewEtl etl);
        Task DeleteEtl(Guid etlId);
    }
}
