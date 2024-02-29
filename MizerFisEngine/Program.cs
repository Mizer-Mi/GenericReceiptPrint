using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.VisualBasic;

namespace MizerFisEngine
{
    internal class Program
    {
        public const string eClear = "\u001b@";
        public const string eCentre = "\u001ba1";
        public const string eLeft = "\u001ba0";
        public const string eRight = "\u001ba2";
        public const string eDrawer = "\u001b@\u001bp\0.}";
        public const string eCut = "\u001bi\r\n";
        public const string eSmlText = "\u001b!\u0001";
        public const string eNmlText = "\u001b!\0";
        public const string eInit = "\u001b!\0\r\u001bc6\u0001\u001bR3\r\n";
        public const string eBigCharOn = "\u001b!8";
        public const string eBigCharOff = "\u001b!\0";

        public static RawPrinterHelper prn = new RawPrinterHelper();

        public static string PrinterName = "Generic / Text Only";

        public static void StartPrint()
        {
            prn.OpenPrint(PrinterName);
        }



        public static void PrintHeader(MizerSiparisFisi fisi)
        {
            Print(eInit + eCentre + fisi.MizerFirmaAdi);
            Print("TELEFON: " + fisi.MizerFirmaTelefon);
            Print("SIPARIS NO: " + fisi.MizerSiparisNo + eLeft);
            PrintDashes();
            Print(eCentre + "MUSTERI BILGILERI" + eLeft);
            Print(eSmlText + "MUSTERI ADI:" + fisi.MizerMusteriAdi);
            Print(eSmlText + "MUSTERI TELEFON:" + fisi.MizerMusteriTelefon);
            Print(eSmlText + "ARAC PLAKASI:" + fisi.MizerMusteriPlakasi);
            PrintDashes();
        }

        public static void PrintBody(MizerSiparisFisi fisi)
        {
            Print(eSmlText + "URUN/SERVIS ADI                   ADET     TOPLAM");
            foreach (MizerKalem kalemler in fisi.MizerKalemler)
            {
                string adiBoslugu = "                                ";
                kalemler.KalemAdi = kalemler.KalemAdi + adiBoslugu;
                kalemler.KalemAdi = kalemler.KalemAdi.Substring(0, adiBoslugu.Length);
                string adetBoslugu = "       ";
                kalemler.KalemAdeti = kalemler.KalemAdeti + adetBoslugu;
                kalemler.KalemAdeti = kalemler.KalemAdeti.Substring(0, adetBoslugu.Length);
                string fiyatBoslugu = "            ";
                kalemler.KalemToplami = kalemler.KalemToplami + fiyatBoslugu;
                kalemler.KalemToplami = kalemler.KalemToplami.Substring(0, fiyatBoslugu.Length);

                Print(eSmlText + kalemler.KalemAdi+"| "+kalemler.KalemAdeti+"| "+kalemler.KalemToplami +" TL");

            }
            PrintDashes();
            string fiyatBoslugu2 = "            ";
            fisi.TOPLAM = fisi.TOPLAM + fiyatBoslugu2;
            fisi.TOPLAM = fisi.TOPLAM.Substring(0, fiyatBoslugu2.Length);
            fisi.GENELTOPLAM = fisi.GENELTOPLAM + fiyatBoslugu2;
            fisi.GENELTOPLAM = fisi.GENELTOPLAM.Substring(0, fiyatBoslugu2.Length);
            Print(eSmlText + "                                   TOPLAM: "+fisi.TOPLAM +" TL");
            if (fisi.Iskonto!="0,00")
            {
                fisi.Iskonto = fisi.Iskonto + fiyatBoslugu2;
                fisi.Iskonto = fisi.Iskonto.Substring(0, fiyatBoslugu2.Length);
                Print(eSmlText + "                                  ISKONTO: " + fisi.Iskonto + " TL");
            }
            Print(eSmlText + "                             GENEL TOPLAM: " + fisi.GENELTOPLAM + " TL");
        }

        public static void PrintFooter(MizerSiparisFisi fisi)
        {
            Print(eSmlText + "              ");
            PrintDashes();
            Print(eCentre + fisi.FisTarihi  + eLeft);
            Print(eCentre + "Bizi Tercih ettiginiz icin TESEKKURLER!" + eLeft);
            Print(Constants.vbLf + Constants.vbLf + Constants.vbLf + Constants.vbLf + Constants.vbLf + eCut + eDrawer);
        }

        public static void Print(string Line)
        {
            prn.SendStringToPrinter(PrinterName, Line + Constants.vbLf);
        }

        public static void PrintDashes()
        {
            Print(eLeft + eNmlText + "-".PadRight(42, '-'));
        }

        public static void EndPrint()
        {
            prn.ClosePrint();
        }



        public static void Main(string[] args)
        {
           
            /* fis.MizerKalemler.Add(kalem);
            StartPrint();

            if (prn.PrinterIsOpen == true)
            {
                PrintHeader();

                PrintBody();

                PrintFooter();

                EndPrint();
            }
            */


            XmlSerializer serializer2 = new XmlSerializer(typeof(MizerSiparisFisi));

            FileStream fs = new FileStream(args[0], FileMode.Open);
            PrinterName = args[1];
             MizerSiparisFisi fis2;

            fis2 = (MizerSiparisFisi)serializer2.Deserialize(fs);



            fis2.MizerFirmaAdi = Regex.Replace(TurkishChrToEnglishChr(fis2.MizerFirmaAdi), @"[^a-zA-Z0-9\s]+", String.Empty).ToUpper();
            fis2.MizerFirmaTelefon = Regex.Replace(TurkishChrToEnglishChr(fis2.MizerFirmaTelefon), @"[^a-zA-Z0-9\s]+", String.Empty).ToUpper();
            fis2.MizerMusteriAdi = Regex.Replace(TurkishChrToEnglishChr(fis2.MizerMusteriAdi), @"[^a-zA-Z0-9\s]+", String.Empty).ToUpper();
            fis2.MizerMusteriTelefon = Regex.Replace(TurkishChrToEnglishChr(fis2.MizerMusteriTelefon), @"[^a-zA-Z0-9\s]+", String.Empty).ToUpper();
            fis2.MizerMusteriPlakasi = Regex.Replace(TurkishChrToEnglishChr(fis2.MizerMusteriPlakasi), @"[^a-zA-Z0-9\s]+", String.Empty).ToUpper();

            foreach (MizerKalem mizerKalem in fis2.MizerKalemler)
            {
                mizerKalem.KalemAdi = Regex.Replace(TurkishChrToEnglishChr(mizerKalem.KalemAdi), @"[^a-zA-Z0-9\s]+", String.Empty).ToUpper();
            }



            StartPrint();

            if (prn.PrinterIsOpen == true)
            {
                PrintHeader(fis2);

                PrintBody(fis2);

                PrintFooter(fis2);

                EndPrint();
            }

  


        }

        public static string TurkishChrToEnglishChr(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            Dictionary<char, char> TurkishChToEnglishChDic = new Dictionary<char, char>()
        {
            {'ç','c'},
            {'Ç','C'},
            {'ğ','g'},
            {'Ğ','G'},
            {'ı','i'},
            {'İ','I'},
            {'ş','s'},
            {'Ş','S'},
            {'ö','o'},
            {'Ö','O'},
            {'ü','u'},
            {'Ü','U'}
        };

            return text.Aggregate(new StringBuilder(), (sb, chr) =>
            {
                if (TurkishChToEnglishChDic.ContainsKey(chr))
                    sb.Append(TurkishChToEnglishChDic[chr]);
                else
                    sb.Append(chr);

                return sb;
            }).ToString();
        }


    }
}
