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
                [Function("parcels", typeof(ParcelsOutput))]
                public class ParcelsFunction : FunctionMessage
                {
                    [Parameter("bytes32", "arg0", 1)]
                    public byte[] Id { get; set; }
                }
            }
        }
    }
}
