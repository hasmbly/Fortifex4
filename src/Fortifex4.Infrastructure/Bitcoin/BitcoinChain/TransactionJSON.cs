using System.Collections.Generic;

namespace Fortifex4.Infrastructure.Bitcoin.BitcoinChain
{
    public class TransactionContainerJSON
    {
        public float post_balance { get; set; }
        public float post_total_rec { get; set; }
        public TransactionJSON tx { get; set; }
    }

    public class TransactionJSON
    {
        public int block_time { get; set; }
        public IList<string> blocks { get; set; }
        public decimal fee { get; set; }
        public IList<InputJSON> inputs { get; set; }
        public string ip { get; set; }
        public int lock_time { get; set; }
        public IList<OutputJSON> outputs { get; set; }
        public int rec_time { get; set; }
        public string self_hash { get; set; }
        public decimal total_input { get; set; }
        public decimal total_output { get; set; }
        public decimal total_spend_output { get; set; }
        public int version { get; set; }
    }

    public class InputJSON
    {
        public In_ScriptJSON in_script { get; set; }
        public Output_RefJSON output_ref { get; set; }
        public string sender { get; set; }
        public decimal value { get; set; }
    }

    public class In_ScriptJSON
    {
        public string asm { get; set; }
        public string hex { get; set; }
    }

    public class Output_RefJSON
    {
        public int number { get; set; }
        public string tx { get; set; }
    }

    public class OutputJSON
    {
        public Out_ScriptJSON out_script { get; set; }
        public string receiver { get; set; }
        public bool spent { get; set; }
        public decimal value { get; set; }
        public Spending_InputJSON spending_input { get; set; }
    }

    public class Out_ScriptJSON
    {
        public string asm { get; set; }
        public string hex { get; set; }
        public int reqSigs { get; set; }
        public string type { get; set; }
    }

    public class Spending_InputJSON
    {
        public int number { get; set; }
        public string tx { get; set; }
    }
}