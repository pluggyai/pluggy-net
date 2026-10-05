using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// SCR (Bacen's Sistema de Informacoes de Credito) for the document behind an item.
    /// This is Bacen's own payload, forwarded unchanged: field names and codes are the ones
    /// the Banco Central returns, so each property maps 1:1 to Bacen's documentation.
    /// </summary>
    public class Scr
    {
        /// <summary>The base dates consulted, for example "202604 a 202607".</summary>
        [JsonProperty("dtbConsult")]
        public string DtbConsult { get; set; }

        /// <summary>The consulted document: the CPF for an individual, or the 8-digit CNPJ root for a company.</summary>
        [JsonProperty("cdCli")]
        public string CdCli { get; set; }

        /// <summary>Type of client: "1" for an individual, "2" for a legal entity.</summary>
        [JsonProperty("tpCli")]
        public string TpCli { get; set; }

        /// <summary>
        /// One entry per consulted base date. A base date with no data for the client is still
        /// listed, with no operations.
        /// </summary>
        [JsonProperty("lsDtb")]
        public IList<ScrDatabase> LsDtb { get; set; }

        /// <summary>Validation messages raised by Bacen for this request.</summary>
        [JsonProperty("listaDeMensagensDeValidacao")]
        public IList<ScrValidationMessage> ListaDeMensagensDeValidacao { get; set; }
    }

    /// <summary>
    /// One consulted base date, and what the SCR holds for the client in it.
    /// </summary>
    public class ScrDatabase
    {
        /// <summary>The base date, as a YYYYMM number (for example 202607).</summary>
        [JsonProperty("dtb")]
        public long? Dtb { get; set; }

        /// <summary>
        /// Bacen's message for this base date, when it has one (for example, that the base date
        /// is not available for consultation).
        /// </summary>
        [JsonProperty("msg")]
        public string Msg { get; set; }

        /// <summary>
        /// Percentage of the expected 3040 documents already incorporated by Bacen for this base
        /// date. A low value means the picture is still partial.
        /// </summary>
        [JsonProperty("docProc")]
        public string DocProc { get; set; }

        /// <summary>Percentage of the expected operation volume already accepted for this base date.</summary>
        [JsonProperty("volProc")]
        public string VolProc { get; set; }

        /// <summary>Number of financial institutions where the client has operations.</summary>
        [JsonProperty("qtdIfs")]
        public double? QtdIfs { get; set; }

        /// <summary>Number of financial conglomerates where the client has operations.</summary>
        [JsonProperty("qtdCongFinc")]
        public double? QtdCongFinc { get; set; }

        /// <summary>Start of the client's relationship with the national financial system.</summary>
        [JsonProperty("dtbIniRel")]
        public string DtbIniRel { get; set; }

        /// <summary>Co-obligation assumed by the client in credit assignments, in BRL.</summary>
        [JsonProperty("coobAss")]
        public double? CoobAss { get; set; }

        /// <summary>Co-obligation received in credit assignments, in BRL.</summary>
        [JsonProperty("coobRec")]
        public double? CoobRec { get; set; }

        /// <summary>Operation groups reported for this base date.</summary>
        [JsonProperty("lsOp")]
        public IList<ScrOperation> LsOp { get; set; }
    }

    /// <summary>
    /// A group of credit operations. The SCR does not return contracts one by one: it aggregates
    /// them by the combination of modality, source of funds, index and exchange variation.
    /// </summary>
    public class ScrOperation
    {
        /// <summary>Bacen's code for the operation modality.</summary>
        [JsonProperty("mod")]
        public string Mod { get; set; }

        /// <summary>Bacen's code for the source of funds.</summary>
        [JsonProperty("oriRec")]
        public string OriRec { get; set; }

        /// <summary>Bacen's code for the reference rate or index.</summary>
        [JsonProperty("indx")]
        public string Indx { get; set; }

        /// <summary>Bacen's code for the exchange rate variation.</summary>
        [JsonProperty("varCamb")]
        public string VarCamb { get; set; }

        /// <summary>
        /// Present when the operation is under dispute: "D" for disagreement, "J" for sub judice,
        /// "JD" for both.
        /// </summary>
        [JsonProperty("subJDisc")]
        public string SubJDisc { get; set; }

        /// <summary>Balance of the operation group split across Bacen's maturity vertices, in BRL.</summary>
        [JsonProperty("resVenc")]
        public ScrMaturityBalances ResVenc { get; set; }

        /// <summary>Guarantees backing the operations in this group.</summary>
        [JsonProperty("lsGar")]
        public IList<ScrGuarantee> LsGar { get; set; }

        /// <summary>Complementary information reported for this group.</summary>
        [JsonProperty("lsInfAd")]
        public IList<ScrAdditionalInfo> LsInfAd { get; set; }
    }

    /// <summary>
    /// Balance of an operation group split across Bacen's maturity vertices, in BRL. Each property
    /// is a vertex code: amounts not yet due, amounts overdue graded by how old the delay is, and a
    /// few categories that are not time windows. The window behind each code is defined by Bacen's
    /// DOC3040 reference. Only the vertices that carry a value are present; the rest are null.
    /// </summary>
    public class ScrMaturityBalances
    {
        [JsonProperty("v20")]
        public double? V20 { get; set; }

        [JsonProperty("v40")]
        public double? V40 { get; set; }

        [JsonProperty("v60")]
        public double? V60 { get; set; }

        [JsonProperty("v80")]
        public double? V80 { get; set; }

        [JsonProperty("v110")]
        public double? V110 { get; set; }

        [JsonProperty("v120")]
        public double? V120 { get; set; }

        [JsonProperty("v130")]
        public double? V130 { get; set; }

        [JsonProperty("v140")]
        public double? V140 { get; set; }

        [JsonProperty("v150")]
        public double? V150 { get; set; }

        [JsonProperty("v160")]
        public double? V160 { get; set; }

        [JsonProperty("v165")]
        public double? V165 { get; set; }

        [JsonProperty("v170")]
        public double? V170 { get; set; }

        [JsonProperty("v175")]
        public double? V175 { get; set; }

        [JsonProperty("v180")]
        public double? V180 { get; set; }

        [JsonProperty("v190")]
        public double? V190 { get; set; }

        [JsonProperty("v199")]
        public double? V199 { get; set; }

        [JsonProperty("v205")]
        public double? V205 { get; set; }

        [JsonProperty("v210")]
        public double? V210 { get; set; }

        [JsonProperty("v220")]
        public double? V220 { get; set; }

        [JsonProperty("v230")]
        public double? V230 { get; set; }

        [JsonProperty("v240")]
        public double? V240 { get; set; }

        [JsonProperty("v245")]
        public double? V245 { get; set; }

        [JsonProperty("v250")]
        public double? V250 { get; set; }

        [JsonProperty("v255")]
        public double? V255 { get; set; }

        [JsonProperty("v260")]
        public double? V260 { get; set; }

        [JsonProperty("v270")]
        public double? V270 { get; set; }

        [JsonProperty("v280")]
        public double? V280 { get; set; }

        [JsonProperty("v290")]
        public double? V290 { get; set; }

        [JsonProperty("v310")]
        public double? V310 { get; set; }

        [JsonProperty("v320")]
        public double? V320 { get; set; }
    }

    public class ScrGuarantee
    {
        /// <summary>Bacen's code for the guarantee type.</summary>
        [JsonProperty("tp")]
        public string Tp { get; set; }

        /// <summary>Number of operations grouped under this type.</summary>
        [JsonProperty("qtd")]
        public double? Qtd { get; set; }
    }

    public class ScrAdditionalInfo
    {
        /// <summary>Type of the complementary information.</summary>
        [JsonProperty("tp")]
        public string Tp { get; set; }

        /// <summary>Code of the complementary information.</summary>
        [JsonProperty("cd")]
        public string Cd { get; set; }

        /// <summary>Number of operations grouped under this entry.</summary>
        [JsonProperty("qtd")]
        public double? Qtd { get; set; }
    }

    public class ScrValidationMessage
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("mensagem")]
        public string Mensagem { get; set; }
    }
}
