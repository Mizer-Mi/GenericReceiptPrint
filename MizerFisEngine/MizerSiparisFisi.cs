using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MizerFisEngine
{
    public class MizerSiparisFisi
    {
        public string MizerFirmaAdi { get; set; }
        public string MizerFirmaTelefon { get; set; }
        public string MizerSiparisNo { get; set; }
        public string MizerMusteriAdi { get; set; }
        public string MizerMusteriTelefon { get; set; }
        public string MizerMusteriPlakasi { get; set; }
        public List<MizerKalem> MizerKalemler { get; set; }
        public string TOPLAM { get; set; }
        public string Iskonto { get; set; }
        public string GENELTOPLAM { get; set; }
        public string FisTarihi { get; set; }
    }
    public class MizerKalem
    {
        public string KalemAdi { get; set; }
        public string KalemAdeti { get; set; }
        public string KalemBirimFiyati { get; set; }
        public string KalemToplami { get; set; }
    }
}
