using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Newtonsoft.Json;
using Pluggy.SDK;
using Pluggy.Tests.Transactions;

namespace Pluggy.Tests.Items
{
    [TestFixture]
    public class ItemScrTest
    {
        private static readonly string ITEM_ID = "d0e8448e-0156-4b4a-ae6c-3e2a6d9bff5c";
        private static readonly string BASE_URL = "https://api.pluggy.ai/";

        private static readonly string AUTH_RESPONSE = JsonConvert.SerializeObject(new { apiKey = "test-api-key" });

        private static readonly string SCR_RESPONSE = @"{
            ""dtbConsult"": ""202604 a 202607"",
            ""cdCli"": ""12345678901"",
            ""tpCli"": ""1"",
            ""lsDtb"": [
                {
                    ""dtb"": 202607,
                    ""docProc"": ""99.5"",
                    ""volProc"": ""98.7"",
                    ""qtdIfs"": 2,
                    ""qtdCongFinc"": 2,
                    ""dtbIniRel"": ""20150301"",
                    ""coobAss"": 0,
                    ""coobRec"": 1500.25,
                    ""lsOp"": [
                        {
                            ""mod"": ""0203"",
                            ""oriRec"": ""0100"",
                            ""indx"": ""11"",
                            ""varCamb"": ""790"",
                            ""subJDisc"": ""D"",
                            ""resVenc"": { ""v20"": 1200.5, ""v110"": 300, ""v320"": 10.75 },
                            ""lsGar"": [ { ""tp"": ""0901"", ""qtd"": 1 } ],
                            ""lsInfAd"": [ { ""tp"": ""01"", ""cd"": ""A"", ""qtd"": 3 } ]
                        }
                    ]
                },
                { ""dtb"": 202606, ""msg"": ""Data-base nao disponivel para consulta"" }
            ],
            ""listaDeMensagensDeValidacao"": [ { ""codigo"": ""W01"", ""mensagem"": ""Aviso"" } ]
        }";

        private static PluggyAPI CreateClient(MockHttpMessageHandler handler)
        {
            return new PluggyAPI("client-id", "client-secret", new HttpClient(handler), BASE_URL);
        }

        [Test]
        public async Task FetchItemScr_WithRange_SendsFromAndTo()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, SCR_RESPONSE);

            await CreateClient(handler).FetchItemScr(Guid.Parse(ITEM_ID), "202604", "202607");

            var request = handler.RequestUris[1];
            Assert.AreEqual(HttpMethod.Get, handler.RequestMethods[1]);
            Assert.AreEqual($"/items/{ITEM_ID}/scr", request.AbsolutePath);
            StringAssert.Contains("from=202604", request.Query);
            StringAssert.Contains("to=202607", request.Query);
        }

        [Test]
        public async Task FetchItemScr_WithoutRange_SendsNoQueryString()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, SCR_RESPONSE);

            await CreateClient(handler).FetchItemScr(Guid.Parse(ITEM_ID));

            var request = handler.RequestUris[1];
            Assert.AreEqual($"/items/{ITEM_ID}/scr", request.AbsolutePath);
            Assert.AreEqual(string.Empty, request.Query);
        }

        [Test]
        public async Task FetchItemScr_DeserializesBacenPayload()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, SCR_RESPONSE);

            var scr = await CreateClient(handler).FetchItemScr(Guid.Parse(ITEM_ID));

            Assert.AreEqual("202604 a 202607", scr.DtbConsult);
            Assert.AreEqual("12345678901", scr.CdCli);
            Assert.AreEqual("1", scr.TpCli);
            Assert.AreEqual(2, scr.LsDtb.Count);

            var baseDate = scr.LsDtb[0];
            Assert.AreEqual(202607, baseDate.Dtb);
            Assert.AreEqual("99.5", baseDate.DocProc);
            Assert.AreEqual("98.7", baseDate.VolProc);
            Assert.AreEqual(2, baseDate.QtdIfs);
            Assert.AreEqual(2, baseDate.QtdCongFinc);
            Assert.AreEqual("20150301", baseDate.DtbIniRel);
            Assert.AreEqual(0, baseDate.CoobAss);
            Assert.AreEqual(1500.25, baseDate.CoobRec);

            var op = baseDate.LsOp[0];
            Assert.AreEqual("0203", op.Mod);
            Assert.AreEqual("0100", op.OriRec);
            Assert.AreEqual("11", op.Indx);
            Assert.AreEqual("790", op.VarCamb);
            Assert.AreEqual("D", op.SubJDisc);
            Assert.AreEqual(1200.5, op.ResVenc.V20);
            Assert.AreEqual(300, op.ResVenc.V110);
            Assert.AreEqual(10.75, op.ResVenc.V320);
            Assert.IsNull(op.ResVenc.V40);
            Assert.AreEqual("0901", op.LsGar[0].Tp);
            Assert.AreEqual(1, op.LsGar[0].Qtd);
            Assert.AreEqual("01", op.LsInfAd[0].Tp);
            Assert.AreEqual("A", op.LsInfAd[0].Cd);
            Assert.AreEqual(3, op.LsInfAd[0].Qtd);

            var emptyBaseDate = scr.LsDtb[1];
            Assert.AreEqual(202606, emptyBaseDate.Dtb);
            Assert.AreEqual("Data-base nao disponivel para consulta", emptyBaseDate.Msg);
            Assert.IsNull(emptyBaseDate.LsOp);

            Assert.AreEqual("W01", scr.ListaDeMensagensDeValidacao[0].Codigo);
            Assert.AreEqual("Aviso", scr.ListaDeMensagensDeValidacao[0].Mensagem);
        }
    }
}
