using TeaFramework.API.Features.ID;

namespace TeaFramework.Features.ID
{
    // TODO: Figure out if (and how) we should specify "Count", since these are negatives.
    public class BankID : IIdBase<BankID, int>
    {
        public const int None = -1;
        public const int PiggyBank = -2;
        public const int Safe = -3;
        public const int DefendersForge = -4;
        public const int VoidVault = -5;
    }
}