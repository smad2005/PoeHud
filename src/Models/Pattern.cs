using System.Diagnostics.Contracts;

namespace PoeHUD.Models
{
    public struct Pattern
    {
        public byte[] Bytes;
        public string Mask;

        public Pattern(byte[] pattern, string mask)
        {
            Contract.Requires(pattern != null);
            Contract.Requires(pattern.Length != 0);
            Bytes = pattern;
            Mask = mask;
        }


        [ContractInvariantMethod]
        void InvariantTest()
        {
            Contract.Invariant(Bytes != null);
            Contract.Invariant(Bytes.Length > 0);
        }


    }
}