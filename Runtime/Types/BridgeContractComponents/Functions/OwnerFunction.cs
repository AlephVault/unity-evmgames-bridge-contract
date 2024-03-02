using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace AlephVault.Unity.EVMGames.Contracts.Bridge
{
    namespace Types
    {
        namespace BridgeContractComponents
        {
            namespace Functions
            {
                [Function("owner", "address")]
                public class OwnerFunction : FunctionMessage
                {

                }
            }
        }
    }
}
