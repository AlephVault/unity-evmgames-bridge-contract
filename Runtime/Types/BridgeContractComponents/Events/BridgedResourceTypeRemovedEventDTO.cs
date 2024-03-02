using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace AlephVault.Unity.EVMGames.Contracts.Bridge
{
    namespace Types
    {
        namespace BridgeContractComponents
        {
            namespace Events
            {
                [Event("BridgedResourceTypeRemoved")]
                public class BridgedResourceTypeRemovedEventDTO : IEventDTO
                {
                    [Parameter("uint256", "id", 1, true)]
                    public BigInteger Id { get; set; }
                }
            }
        }
    }
}
